using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000326 RID: 806
	public class NativePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x0600152F RID: 5423 RVA: 0x00042082 File Offset: 0x00040282
		internal NativePdbWriter(ModuleDefinition module, SymWriter writer)
		{
			this.module = module;
			this.metadata = module.metadata_builder;
			this.writer = writer;
			this.documents = new Dictionary<string, SymDocumentWriter>();
			this.import_info_to_parent = new Dictionary<ImportDebugInformation, MetadataToken>();
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x000420BA File Offset: 0x000402BA
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new NativePdbReaderProvider();
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x000420C4 File Offset: 0x000402C4
		public ImageDebugHeader GetDebugHeader()
		{
			ImageDebugDirectory imageDebugDirectory;
			byte[] debugInfo = this.writer.GetDebugInfo(out imageDebugDirectory);
			imageDebugDirectory.TimeDateStamp = (int)this.module.timestamp;
			return new ImageDebugHeader(new ImageDebugHeaderEntry(imageDebugDirectory, debugInfo));
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00042100 File Offset: 0x00040300
		public void Write(MethodDebugInformation info)
		{
			int num = info.method.MetadataToken.ToInt32();
			if (!info.HasSequencePoints && info.scope == null && !info.HasCustomDebugInformations && info.StateMachineKickOffMethod == null)
			{
				return;
			}
			this.writer.OpenMethod(num);
			if (!info.sequence_points.IsNullOrEmpty<SequencePoint>())
			{
				this.DefineSequencePoints(info.sequence_points);
			}
			MetadataToken metadataToken = default(MetadataToken);
			if (info.scope != null)
			{
				this.DefineScope(info.scope, info, out metadataToken);
			}
			this.DefineCustomMetadata(info, metadataToken);
			this.writer.CloseMethod();
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0004219C File Offset: 0x0004039C
		private void DefineCustomMetadata(MethodDebugInformation info, MetadataToken import_parent)
		{
			CustomMetadataWriter customMetadataWriter = new CustomMetadataWriter(this.writer);
			if (import_parent.RID != 0U)
			{
				customMetadataWriter.WriteForwardInfo(import_parent);
			}
			else if (info.scope != null && info.scope.Import != null && info.scope.Import.HasTargets)
			{
				customMetadataWriter.WriteUsingInfo(info.scope.Import);
			}
			if (info.Method.HasCustomAttributes)
			{
				foreach (CustomAttribute customAttribute in info.Method.CustomAttributes)
				{
					TypeReference attributeType = customAttribute.AttributeType;
					if (attributeType.IsTypeOf("System.Runtime.CompilerServices", "IteratorStateMachineAttribute") || attributeType.IsTypeOf("System.Runtime.CompilerServices", "AsyncStateMachineAttribute"))
					{
						TypeReference typeReference = customAttribute.ConstructorArguments[0].Value as TypeReference;
						if (typeReference != null)
						{
							customMetadataWriter.WriteForwardIterator(typeReference);
						}
					}
				}
			}
			if (info.HasCustomDebugInformations)
			{
				StateMachineScopeDebugInformation stateMachineScopeDebugInformation = info.CustomDebugInformations.FirstOrDefault((CustomDebugInformation cdi) => cdi.Kind == CustomDebugInformationKind.StateMachineScope) as StateMachineScopeDebugInformation;
				if (stateMachineScopeDebugInformation != null)
				{
					customMetadataWriter.WriteIteratorScopes(stateMachineScopeDebugInformation, info);
				}
			}
			customMetadataWriter.WriteCustomMetadata();
			this.DefineAsyncCustomMetadata(info);
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x000422FC File Offset: 0x000404FC
		private void DefineAsyncCustomMetadata(MethodDebugInformation info)
		{
			if (!info.HasCustomDebugInformations)
			{
				return;
			}
			foreach (CustomDebugInformation customDebugInformation in info.CustomDebugInformations)
			{
				AsyncMethodBodyDebugInformation asyncMethodBodyDebugInformation = customDebugInformation as AsyncMethodBodyDebugInformation;
				if (asyncMethodBodyDebugInformation != null)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						BinaryStreamWriter binaryStreamWriter = new BinaryStreamWriter(memoryStream);
						binaryStreamWriter.WriteUInt32((info.StateMachineKickOffMethod != null) ? info.StateMachineKickOffMethod.MetadataToken.ToUInt32() : 0U);
						binaryStreamWriter.WriteUInt32((uint)asyncMethodBodyDebugInformation.CatchHandler.Offset);
						binaryStreamWriter.WriteUInt32((uint)asyncMethodBodyDebugInformation.Resumes.Count);
						for (int i = 0; i < asyncMethodBodyDebugInformation.Resumes.Count; i++)
						{
							binaryStreamWriter.WriteUInt32((uint)asyncMethodBodyDebugInformation.Yields[i].Offset);
							binaryStreamWriter.WriteUInt32(asyncMethodBodyDebugInformation.resume_methods[i].MetadataToken.ToUInt32());
							binaryStreamWriter.WriteUInt32((uint)asyncMethodBodyDebugInformation.Resumes[i].Offset);
						}
						this.writer.DefineCustomMetadata("asyncMethodInfo", memoryStream.ToArray());
					}
				}
			}
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x00042474 File Offset: 0x00040674
		private void DefineScope(ScopeDebugInformation scope, MethodDebugInformation info, out MetadataToken import_parent)
		{
			int offset = scope.Start.Offset;
			int num = (scope.End.IsEndOfMethod ? info.code_size : scope.End.Offset);
			import_parent = new MetadataToken(0U);
			this.writer.OpenScope(offset);
			if (scope.Import != null && scope.Import.HasTargets && !this.import_info_to_parent.TryGetValue(info.scope.Import, out import_parent))
			{
				foreach (ImportTarget importTarget in scope.Import.Targets)
				{
					ImportTargetKind kind = importTarget.Kind;
					if (kind <= ImportTargetKind.ImportType)
					{
						if (kind != ImportTargetKind.ImportNamespace)
						{
							if (kind == ImportTargetKind.ImportType)
							{
								this.writer.UsingNamespace("T" + TypeParser.ToParseable(importTarget.type, true));
							}
						}
						else
						{
							this.writer.UsingNamespace("U" + importTarget.@namespace);
						}
					}
					else if (kind != ImportTargetKind.DefineNamespaceAlias)
					{
						if (kind == ImportTargetKind.DefineTypeAlias)
						{
							this.writer.UsingNamespace("A" + importTarget.Alias + " T" + TypeParser.ToParseable(importTarget.type, true));
						}
					}
					else
					{
						this.writer.UsingNamespace("A" + importTarget.Alias + " U" + importTarget.@namespace);
					}
				}
				this.import_info_to_parent.Add(info.scope.Import, info.method.MetadataToken);
			}
			int num2 = info.local_var_token.ToInt32();
			if (!scope.variables.IsNullOrEmpty<VariableDebugInformation>())
			{
				for (int i = 0; i < scope.variables.Count; i++)
				{
					VariableDebugInformation variableDebugInformation = scope.variables[i];
					this.DefineLocalVariable(variableDebugInformation, num2, offset, num);
				}
			}
			if (!scope.constants.IsNullOrEmpty<ConstantDebugInformation>())
			{
				for (int j = 0; j < scope.constants.Count; j++)
				{
					ConstantDebugInformation constantDebugInformation = scope.constants[j];
					this.DefineConstant(constantDebugInformation);
				}
			}
			if (!scope.scopes.IsNullOrEmpty<ScopeDebugInformation>())
			{
				for (int k = 0; k < scope.scopes.Count; k++)
				{
					MetadataToken metadataToken;
					this.DefineScope(scope.scopes[k], info, out metadataToken);
				}
			}
			this.writer.CloseScope(num);
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00042710 File Offset: 0x00040910
		private void DefineSequencePoints(Collection<SequencePoint> sequence_points)
		{
			for (int i = 0; i < sequence_points.Count; i++)
			{
				SequencePoint sequencePoint = sequence_points[i];
				this.writer.DefineSequencePoints(this.GetDocument(sequencePoint.Document), new int[] { sequencePoint.Offset }, new int[] { sequencePoint.StartLine }, new int[] { sequencePoint.StartColumn }, new int[] { sequencePoint.EndLine }, new int[] { sequencePoint.EndColumn });
			}
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00042798 File Offset: 0x00040998
		private void DefineLocalVariable(VariableDebugInformation variable, int local_var_token, int start_offset, int end_offset)
		{
			this.writer.DefineLocalVariable2(variable.Name, variable.Attributes, local_var_token, variable.Index, 0, 0, start_offset, end_offset);
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x000427C8 File Offset: 0x000409C8
		private void DefineConstant(ConstantDebugInformation constant)
		{
			uint num = this.metadata.AddStandAloneSignature(this.metadata.GetConstantTypeBlobIndex(constant.ConstantType));
			MetadataToken metadataToken = new MetadataToken(TokenType.Signature, num);
			this.writer.DefineConstant2(constant.Name, constant.Value, metadataToken.ToInt32());
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x00042820 File Offset: 0x00040A20
		private SymDocumentWriter GetDocument(Document document)
		{
			if (document == null)
			{
				return null;
			}
			SymDocumentWriter symDocumentWriter;
			if (this.documents.TryGetValue(document.Url, out symDocumentWriter))
			{
				return symDocumentWriter;
			}
			symDocumentWriter = this.writer.DefineDocument(document.Url, document.LanguageGuid, document.LanguageVendorGuid, document.TypeGuid);
			if (!document.Hash.IsNullOrEmpty<byte>())
			{
				symDocumentWriter.SetCheckSum(document.HashAlgorithmGuid, document.Hash);
			}
			this.documents[document.Url] = symDocumentWriter;
			return symDocumentWriter;
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x000428A0 File Offset: 0x00040AA0
		public void Dispose()
		{
			MethodDefinition entryPoint = this.module.EntryPoint;
			if (entryPoint != null)
			{
				this.writer.SetUserEntryPoint(entryPoint.MetadataToken.ToInt32());
			}
			this.writer.Close();
		}

		// Token: 0x04000A71 RID: 2673
		private readonly ModuleDefinition module;

		// Token: 0x04000A72 RID: 2674
		private readonly MetadataBuilder metadata;

		// Token: 0x04000A73 RID: 2675
		private readonly SymWriter writer;

		// Token: 0x04000A74 RID: 2676
		private readonly Dictionary<string, SymDocumentWriter> documents;

		// Token: 0x04000A75 RID: 2677
		private readonly Dictionary<ImportDebugInformation, MetadataToken> import_info_to_parent;
	}
}

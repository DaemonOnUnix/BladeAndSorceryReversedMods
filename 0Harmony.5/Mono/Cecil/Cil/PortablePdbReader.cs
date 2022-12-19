using System;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002C6 RID: 710
	internal sealed class PortablePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x0003B538 File Offset: 0x00039738
		private bool IsEmbedded
		{
			get
			{
				return this.reader.image == this.debug_reader.image;
			}
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0003B552 File Offset: 0x00039752
		internal PortablePdbReader(Image image, ModuleDefinition module)
		{
			this.image = image;
			this.module = module;
			this.reader = module.reader;
			this.debug_reader = new MetadataReader(image, module, this.reader);
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0003B587 File Offset: 0x00039787
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new PortablePdbWriterProvider();
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0003B590 File Offset: 0x00039790
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			if (this.image == this.module.Image)
			{
				return true;
			}
			foreach (ImageDebugHeaderEntry imageDebugHeaderEntry in header.Entries)
			{
				if (PortablePdbReader.IsMatchingEntry(this.image.PdbHeap, imageDebugHeaderEntry))
				{
					this.ReadModule();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0003B5E8 File Offset: 0x000397E8
		private static bool IsMatchingEntry(PdbHeap heap, ImageDebugHeaderEntry entry)
		{
			if (entry.Directory.Type != ImageDebugType.CodeView)
			{
				return false;
			}
			byte[] data = entry.Data;
			if (data.Length < 24)
			{
				return false;
			}
			if (PortablePdbReader.ReadInt32(data, 0) != 1396986706)
			{
				return false;
			}
			byte[] array = new byte[16];
			Buffer.BlockCopy(data, 4, array, 0, 16);
			Guid guid = new Guid(array);
			Buffer.BlockCopy(heap.Id, 0, array, 0, 16);
			Guid guid2 = new Guid(array);
			return guid == guid2;
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0003B65C File Offset: 0x0003985C
		private static int ReadInt32(byte[] bytes, int start)
		{
			return (int)bytes[start] | ((int)bytes[start + 1] << 8) | ((int)bytes[start + 2] << 16) | ((int)bytes[start + 3] << 24);
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0003B67B File Offset: 0x0003987B
		private void ReadModule()
		{
			this.module.custom_infos = this.debug_reader.GetCustomDebugInformation(this.module);
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0003B69C File Offset: 0x0003989C
		public MethodDebugInformation Read(MethodDefinition method)
		{
			MethodDebugInformation methodDebugInformation = new MethodDebugInformation(method);
			this.ReadSequencePoints(methodDebugInformation);
			this.ReadScope(methodDebugInformation);
			this.ReadStateMachineKickOffMethod(methodDebugInformation);
			this.ReadCustomDebugInformations(methodDebugInformation);
			return methodDebugInformation;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0003B6CD File Offset: 0x000398CD
		private void ReadSequencePoints(MethodDebugInformation method_info)
		{
			method_info.sequence_points = this.debug_reader.ReadSequencePoints(method_info.method);
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0003B6E6 File Offset: 0x000398E6
		private void ReadScope(MethodDebugInformation method_info)
		{
			method_info.scope = this.debug_reader.ReadScope(method_info.method);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0003B6FF File Offset: 0x000398FF
		private void ReadStateMachineKickOffMethod(MethodDebugInformation method_info)
		{
			method_info.kickoff_method = this.debug_reader.ReadStateMachineKickoffMethod(method_info.method);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0003B718 File Offset: 0x00039918
		private void ReadCustomDebugInformations(MethodDebugInformation info)
		{
			info.method.custom_infos = this.debug_reader.GetCustomDebugInformation(info.method);
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0003B736 File Offset: 0x00039936
		public void Dispose()
		{
			if (this.IsEmbedded)
			{
				return;
			}
			this.image.Dispose();
		}

		// Token: 0x04000928 RID: 2344
		private readonly Image image;

		// Token: 0x04000929 RID: 2345
		private readonly ModuleDefinition module;

		// Token: 0x0400092A RID: 2346
		private readonly MetadataReader reader;

		// Token: 0x0400092B RID: 2347
		private readonly MetadataReader debug_reader;
	}
}

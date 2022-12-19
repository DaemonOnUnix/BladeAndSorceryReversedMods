using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000C7 RID: 199
	internal sealed class MetadataReader : ByteBuffer
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x00015870 File Offset: 0x00013A70
		public MetadataReader(ModuleDefinition module)
			: base(module.Image.TableHeap.data)
		{
			this.image = module.Image;
			this.module = module;
			this.metadata = module.MetadataSystem;
			this.code = new CodeReader(this);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000158BE File Offset: 0x00013ABE
		public MetadataReader(Image image, ModuleDefinition module, MetadataReader metadata_reader)
			: base(image.TableHeap.data)
		{
			this.image = image;
			this.module = module;
			this.metadata = module.MetadataSystem;
			this.metadata_reader = metadata_reader;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000158F2 File Offset: 0x00013AF2
		private int GetCodedIndexSize(CodedIndex index)
		{
			return this.image.GetCodedIndexSize(index);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00015900 File Offset: 0x00013B00
		private uint ReadByIndexSize(int size)
		{
			if (size == 4)
			{
				return base.ReadUInt32();
			}
			return (uint)base.ReadUInt16();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00015914 File Offset: 0x00013B14
		private byte[] ReadBlob()
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			if (blobHeap == null)
			{
				this.position += 2;
				return Empty<byte>.Array;
			}
			return blobHeap.Read(this.ReadBlobIndex());
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00015950 File Offset: 0x00013B50
		private byte[] ReadBlob(uint signature)
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			if (blobHeap == null)
			{
				return Empty<byte>.Array;
			}
			return blobHeap.Read(signature);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001597C File Offset: 0x00013B7C
		private uint ReadBlobIndex()
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			return this.ReadByIndexSize((blobHeap != null) ? blobHeap.IndexSize : 2);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000159A8 File Offset: 0x00013BA8
		private void GetBlobView(uint signature, out byte[] blob, out int index, out int count)
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			if (blobHeap == null)
			{
				blob = null;
				index = (count = 0);
				return;
			}
			blobHeap.GetView(signature, out blob, out index, out count);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000159DC File Offset: 0x00013BDC
		private string ReadString()
		{
			return this.image.StringHeap.Read(this.ReadByIndexSize(this.image.StringHeap.IndexSize));
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00015A04 File Offset: 0x00013C04
		private uint ReadStringIndex()
		{
			return this.ReadByIndexSize(this.image.StringHeap.IndexSize);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00015A1C File Offset: 0x00013C1C
		private Guid ReadGuid()
		{
			return this.image.GuidHeap.Read(this.ReadByIndexSize(this.image.GuidHeap.IndexSize));
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00015A44 File Offset: 0x00013C44
		private uint ReadTableIndex(Table table)
		{
			return this.ReadByIndexSize(this.image.GetTableIndexSize(table));
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00015A58 File Offset: 0x00013C58
		private MetadataToken ReadMetadataToken(CodedIndex index)
		{
			return index.GetMetadataToken(this.ReadByIndexSize(this.GetCodedIndexSize(index)));
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00015A70 File Offset: 0x00013C70
		private int MoveTo(Table table)
		{
			TableInformation tableInformation = this.image.TableHeap[table];
			if (tableInformation.Length != 0U)
			{
				this.position = (int)tableInformation.Offset;
			}
			return (int)tableInformation.Length;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00015AAC File Offset: 0x00013CAC
		private bool MoveTo(Table table, uint row)
		{
			TableInformation tableInformation = this.image.TableHeap[table];
			uint length = tableInformation.Length;
			if (length == 0U || row > length)
			{
				return false;
			}
			this.position = (int)(tableInformation.Offset + tableInformation.RowSize * (row - 1U));
			return true;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00015AF4 File Offset: 0x00013CF4
		public AssemblyNameDefinition ReadAssemblyNameDefinition()
		{
			if (this.MoveTo(Table.Assembly) == 0)
			{
				return null;
			}
			AssemblyNameDefinition assemblyNameDefinition = new AssemblyNameDefinition();
			assemblyNameDefinition.HashAlgorithm = (AssemblyHashAlgorithm)base.ReadUInt32();
			this.PopulateVersionAndFlags(assemblyNameDefinition);
			assemblyNameDefinition.PublicKey = this.ReadBlob();
			this.PopulateNameAndCulture(assemblyNameDefinition);
			return assemblyNameDefinition;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00015B3A File Offset: 0x00013D3A
		public ModuleDefinition Populate(ModuleDefinition module)
		{
			if (this.MoveTo(Table.Module) == 0)
			{
				return module;
			}
			base.Advance(2);
			module.Name = this.ReadString();
			module.Mvid = this.ReadGuid();
			return module;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00015B68 File Offset: 0x00013D68
		private void InitializeAssemblyReferences()
		{
			if (this.metadata.AssemblyReferences != null)
			{
				return;
			}
			int num = this.MoveTo(Table.AssemblyRef);
			AssemblyNameReference[] array = (this.metadata.AssemblyReferences = new AssemblyNameReference[num]);
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				AssemblyNameReference assemblyNameReference = new AssemblyNameReference();
				assemblyNameReference.token = new MetadataToken(TokenType.AssemblyRef, num2 + 1U);
				this.PopulateVersionAndFlags(assemblyNameReference);
				byte[] array2 = this.ReadBlob();
				if (assemblyNameReference.HasPublicKey)
				{
					assemblyNameReference.PublicKey = array2;
				}
				else
				{
					assemblyNameReference.PublicKeyToken = array2;
				}
				this.PopulateNameAndCulture(assemblyNameReference);
				assemblyNameReference.Hash = this.ReadBlob();
				array[(int)num2] = assemblyNameReference;
				num2 += 1U;
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00015C14 File Offset: 0x00013E14
		public Collection<AssemblyNameReference> ReadAssemblyReferences()
		{
			this.InitializeAssemblyReferences();
			Collection<AssemblyNameReference> collection = new Collection<AssemblyNameReference>(this.metadata.AssemblyReferences);
			if (this.module.IsWindowsMetadata())
			{
				this.module.Projections.AddVirtualReferences(collection);
			}
			return collection;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00015C58 File Offset: 0x00013E58
		public MethodDefinition ReadEntryPoint()
		{
			if (this.module.Image.EntryPointToken == 0U)
			{
				return null;
			}
			MetadataToken metadataToken = new MetadataToken(this.module.Image.EntryPointToken);
			return this.GetMethodDefinition(metadataToken.RID);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00015CA0 File Offset: 0x00013EA0
		public Collection<ModuleDefinition> ReadModules()
		{
			Collection<ModuleDefinition> collection = new Collection<ModuleDefinition>(1);
			collection.Add(this.module);
			int num = this.MoveTo(Table.File);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				bool flag = base.ReadUInt32() != 0U;
				string text = this.ReadString();
				this.ReadBlobIndex();
				if (!flag)
				{
					ReaderParameters readerParameters = new ReaderParameters
					{
						ReadingMode = this.module.ReadingMode,
						SymbolReaderProvider = this.module.SymbolReaderProvider,
						AssemblyResolver = this.module.AssemblyResolver
					};
					collection.Add(ModuleDefinition.ReadModule(this.GetModuleFileName(text), readerParameters));
				}
				num2 += 1U;
			}
			return collection;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00015D3C File Offset: 0x00013F3C
		private string GetModuleFileName(string name)
		{
			if (this.module.FileName == null)
			{
				throw new NotSupportedException();
			}
			return Path.Combine(Path.GetDirectoryName(this.module.FileName), name);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00015D68 File Offset: 0x00013F68
		private void InitializeModuleReferences()
		{
			if (this.metadata.ModuleReferences != null)
			{
				return;
			}
			int num = this.MoveTo(Table.ModuleRef);
			ModuleReference[] array = (this.metadata.ModuleReferences = new ModuleReference[num]);
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				array[(int)num2] = new ModuleReference(this.ReadString())
				{
					token = new MetadataToken(TokenType.ModuleRef, num2 + 1U)
				};
				num2 += 1U;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00015DD5 File Offset: 0x00013FD5
		public Collection<ModuleReference> ReadModuleReferences()
		{
			this.InitializeModuleReferences();
			return new Collection<ModuleReference>(this.metadata.ModuleReferences);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00015DF0 File Offset: 0x00013FF0
		public bool HasFileResource()
		{
			int num = this.MoveTo(Table.File);
			if (num == 0)
			{
				return false;
			}
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				if (this.ReadFileRecord(num2).Col1 == FileAttributes.ContainsNoMetaData)
				{
					return true;
				}
				num2 += 1U;
			}
			return false;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00015E2C File Offset: 0x0001402C
		public Collection<Resource> ReadResources()
		{
			int num = this.MoveTo(Table.ManifestResource);
			Collection<Resource> collection = new Collection<Resource>(num);
			int i = 1;
			while (i <= num)
			{
				uint num2 = base.ReadUInt32();
				ManifestResourceAttributes manifestResourceAttributes = (ManifestResourceAttributes)base.ReadUInt32();
				string text = this.ReadString();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.Implementation);
				Resource resource;
				if (metadataToken.RID == 0U)
				{
					resource = new EmbeddedResource(text, manifestResourceAttributes, num2, this);
					goto IL_C6;
				}
				if (metadataToken.TokenType == TokenType.AssemblyRef)
				{
					resource = new AssemblyLinkedResource(text, manifestResourceAttributes)
					{
						Assembly = (AssemblyNameReference)this.GetTypeReferenceScope(metadataToken)
					};
					goto IL_C6;
				}
				if (metadataToken.TokenType == TokenType.File)
				{
					Row<FileAttributes, string, uint> row = this.ReadFileRecord(metadataToken.RID);
					resource = new LinkedResource(text, manifestResourceAttributes)
					{
						File = row.Col2,
						hash = this.ReadBlob(row.Col3)
					};
					goto IL_C6;
				}
				IL_CE:
				i++;
				continue;
				IL_C6:
				collection.Add(resource);
				goto IL_CE;
			}
			return collection;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00015F14 File Offset: 0x00014114
		private Row<FileAttributes, string, uint> ReadFileRecord(uint rid)
		{
			int position = this.position;
			if (!this.MoveTo(Table.File, rid))
			{
				throw new ArgumentException();
			}
			Row<FileAttributes, string, uint> row = new Row<FileAttributes, string, uint>((FileAttributes)base.ReadUInt32(), this.ReadString(), this.ReadBlobIndex());
			this.position = position;
			return row;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00015F58 File Offset: 0x00014158
		public byte[] GetManagedResource(uint offset)
		{
			return this.image.GetReaderAt<uint, byte[]>(this.image.Resources.VirtualAddress, offset, delegate(uint o, BinaryStreamReader reader)
			{
				reader.Advance((int)o);
				return reader.ReadBytes(reader.ReadInt32());
			}) ?? Empty<byte>.Array;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00015FA9 File Offset: 0x000141A9
		private void PopulateVersionAndFlags(AssemblyNameReference name)
		{
			name.Version = new Version((int)base.ReadUInt16(), (int)base.ReadUInt16(), (int)base.ReadUInt16(), (int)base.ReadUInt16());
			name.Attributes = (AssemblyAttributes)base.ReadUInt32();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00015FDA File Offset: 0x000141DA
		private void PopulateNameAndCulture(AssemblyNameReference name)
		{
			name.Name = this.ReadString();
			name.Culture = this.ReadString();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00015FF4 File Offset: 0x000141F4
		public TypeDefinitionCollection ReadTypes()
		{
			this.InitializeTypeDefinitions();
			TypeDefinition[] types = this.metadata.Types;
			int num = types.Length - this.metadata.NestedTypes.Count;
			TypeDefinitionCollection typeDefinitionCollection = new TypeDefinitionCollection(this.module, num);
			foreach (TypeDefinition typeDefinition in types)
			{
				if (!MetadataReader.IsNested(typeDefinition.Attributes))
				{
					typeDefinitionCollection.Add(typeDefinition);
				}
			}
			if (this.image.HasTable(Table.MethodPtr) || this.image.HasTable(Table.FieldPtr))
			{
				this.CompleteTypes();
			}
			return typeDefinitionCollection;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00016084 File Offset: 0x00014284
		private void CompleteTypes()
		{
			foreach (TypeDefinition typeDefinition in this.metadata.Types)
			{
				Mixin.Read(typeDefinition.Fields);
				Mixin.Read(typeDefinition.Methods);
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000160C4 File Offset: 0x000142C4
		private void InitializeTypeDefinitions()
		{
			if (this.metadata.Types != null)
			{
				return;
			}
			this.InitializeNestedTypes();
			this.InitializeFields();
			this.InitializeMethods();
			int num = this.MoveTo(Table.TypeDef);
			TypeDefinition[] array = (this.metadata.Types = new TypeDefinition[num]);
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				if (array[(int)num2] == null)
				{
					array[(int)num2] = this.ReadType(num2 + 1U);
				}
				num2 += 1U;
			}
			if (this.module.IsWindowsMetadata())
			{
				uint num3 = 0U;
				while ((ulong)num3 < (ulong)((long)num))
				{
					WindowsRuntimeProjections.Project(array[(int)num3]);
					num3 += 1U;
				}
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00016158 File Offset: 0x00014358
		private static bool IsNested(TypeAttributes attributes)
		{
			TypeAttributes typeAttributes = attributes & TypeAttributes.VisibilityMask;
			return typeAttributes - TypeAttributes.NestedPublic <= 5U;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00016174 File Offset: 0x00014374
		public bool HasNestedTypes(TypeDefinition type)
		{
			this.InitializeNestedTypes();
			Collection<uint> collection;
			return this.metadata.TryGetNestedTypeMapping(type, out collection) && collection.Count > 0;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000161A4 File Offset: 0x000143A4
		public Collection<TypeDefinition> ReadNestedTypes(TypeDefinition type)
		{
			this.InitializeNestedTypes();
			Collection<uint> collection;
			if (!this.metadata.TryGetNestedTypeMapping(type, out collection))
			{
				return new MemberDefinitionCollection<TypeDefinition>(type);
			}
			MemberDefinitionCollection<TypeDefinition> memberDefinitionCollection = new MemberDefinitionCollection<TypeDefinition>(type, collection.Count);
			for (int i = 0; i < collection.Count; i++)
			{
				TypeDefinition typeDefinition = this.GetTypeDefinition(collection[i]);
				if (typeDefinition != null)
				{
					memberDefinitionCollection.Add(typeDefinition);
				}
			}
			this.metadata.RemoveNestedTypeMapping(type);
			return memberDefinitionCollection;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00016214 File Offset: 0x00014414
		private void InitializeNestedTypes()
		{
			if (this.metadata.NestedTypes != null)
			{
				return;
			}
			int num = this.MoveTo(Table.NestedClass);
			this.metadata.NestedTypes = new Dictionary<uint, Collection<uint>>(num);
			this.metadata.ReverseNestedTypes = new Dictionary<uint, uint>(num);
			if (num == 0)
			{
				return;
			}
			for (int i = 1; i <= num; i++)
			{
				uint num2 = this.ReadTableIndex(Table.TypeDef);
				uint num3 = this.ReadTableIndex(Table.TypeDef);
				this.AddNestedMapping(num3, num2);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00016282 File Offset: 0x00014482
		private void AddNestedMapping(uint declaring, uint nested)
		{
			this.metadata.SetNestedTypeMapping(declaring, MetadataReader.AddMapping<uint, uint>(this.metadata.NestedTypes, declaring, nested));
			this.metadata.SetReverseNestedTypeMapping(nested, declaring);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000162B0 File Offset: 0x000144B0
		private static Collection<TValue> AddMapping<TKey, TValue>(Dictionary<TKey, Collection<TValue>> cache, TKey key, TValue value)
		{
			Collection<TValue> collection;
			if (!cache.TryGetValue(key, out collection))
			{
				collection = new Collection<TValue>();
			}
			collection.Add(value);
			return collection;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000162D8 File Offset: 0x000144D8
		private TypeDefinition ReadType(uint rid)
		{
			if (!this.MoveTo(Table.TypeDef, rid))
			{
				return null;
			}
			TypeAttributes typeAttributes = (TypeAttributes)base.ReadUInt32();
			string text = this.ReadString();
			TypeDefinition typeDefinition = new TypeDefinition(this.ReadString(), text, typeAttributes);
			typeDefinition.token = new MetadataToken(TokenType.TypeDef, rid);
			typeDefinition.scope = this.module;
			typeDefinition.module = this.module;
			this.metadata.AddTypeDefinition(typeDefinition);
			this.context = typeDefinition;
			typeDefinition.BaseType = this.GetTypeDefOrRef(this.ReadMetadataToken(CodedIndex.TypeDefOrRef));
			typeDefinition.fields_range = this.ReadListRange(rid, Table.TypeDef, Table.Field);
			typeDefinition.methods_range = this.ReadListRange(rid, Table.TypeDef, Table.Method);
			if (MetadataReader.IsNested(typeAttributes))
			{
				typeDefinition.DeclaringType = this.GetNestedTypeDeclaringType(typeDefinition);
			}
			return typeDefinition;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00016390 File Offset: 0x00014590
		private TypeDefinition GetNestedTypeDeclaringType(TypeDefinition type)
		{
			uint num;
			if (!this.metadata.TryGetReverseNestedTypeMapping(type, out num))
			{
				return null;
			}
			this.metadata.RemoveReverseNestedTypeMapping(type);
			return this.GetTypeDefinition(num);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000163C4 File Offset: 0x000145C4
		private Range ReadListRange(uint current_index, Table current, Table target)
		{
			Range range = default(Range);
			uint num = this.ReadTableIndex(target);
			if (num == 0U)
			{
				return range;
			}
			TableInformation tableInformation = this.image.TableHeap[current];
			uint num2;
			if (current_index == tableInformation.Length)
			{
				num2 = this.image.TableHeap[target].Length + 1U;
			}
			else
			{
				int position = this.position;
				this.position += (int)((ulong)tableInformation.RowSize - (ulong)((long)this.image.GetTableIndexSize(target)));
				num2 = this.ReadTableIndex(target);
				this.position = position;
			}
			range.Start = num;
			range.Length = num2 - num;
			return range;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001646C File Offset: 0x0001466C
		public Row<short, int> ReadTypeLayout(TypeDefinition type)
		{
			this.InitializeTypeLayouts();
			uint rid = type.token.RID;
			Row<ushort, uint> row;
			if (!this.metadata.ClassLayouts.TryGetValue(rid, out row))
			{
				return new Row<short, int>(-1, -1);
			}
			type.PackingSize = (short)row.Col1;
			type.ClassSize = (int)row.Col2;
			this.metadata.ClassLayouts.Remove(rid);
			return new Row<short, int>((short)row.Col1, (int)row.Col2);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000164E8 File Offset: 0x000146E8
		private void InitializeTypeLayouts()
		{
			if (this.metadata.ClassLayouts != null)
			{
				return;
			}
			int num = this.MoveTo(Table.ClassLayout);
			Dictionary<uint, Row<ushort, uint>> dictionary = (this.metadata.ClassLayouts = new Dictionary<uint, Row<ushort, uint>>(num));
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				ushort num3 = base.ReadUInt16();
				uint num4 = base.ReadUInt32();
				uint num5 = this.ReadTableIndex(Table.TypeDef);
				dictionary.Add(num5, new Row<ushort, uint>(num3, num4));
				num2 += 1U;
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00016559 File Offset: 0x00014759
		public TypeReference GetTypeDefOrRef(MetadataToken token)
		{
			return (TypeReference)this.LookupToken(token);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00016568 File Offset: 0x00014768
		public TypeDefinition GetTypeDefinition(uint rid)
		{
			this.InitializeTypeDefinitions();
			TypeDefinition typeDefinition = this.metadata.GetTypeDefinition(rid);
			if (typeDefinition != null)
			{
				return typeDefinition;
			}
			typeDefinition = this.ReadTypeDefinition(rid);
			if (this.module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(typeDefinition);
			}
			return typeDefinition;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000165A9 File Offset: 0x000147A9
		private TypeDefinition ReadTypeDefinition(uint rid)
		{
			if (!this.MoveTo(Table.TypeDef, rid))
			{
				return null;
			}
			return this.ReadType(rid);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000165BE File Offset: 0x000147BE
		private void InitializeTypeReferences()
		{
			if (this.metadata.TypeReferences != null)
			{
				return;
			}
			this.metadata.TypeReferences = new TypeReference[this.image.GetTableLength(Table.TypeRef)];
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000165EC File Offset: 0x000147EC
		public TypeReference GetTypeReference(string scope, string full_name)
		{
			this.InitializeTypeReferences();
			int num = this.metadata.TypeReferences.Length;
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				TypeReference typeReference = this.GetTypeReference(num2);
				if (!(typeReference.FullName != full_name))
				{
					if (string.IsNullOrEmpty(scope))
					{
						return typeReference;
					}
					if (typeReference.Scope.Name == scope)
					{
						return typeReference;
					}
				}
				num2 += 1U;
			}
			return null;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00016654 File Offset: 0x00014854
		private TypeReference GetTypeReference(uint rid)
		{
			this.InitializeTypeReferences();
			TypeReference typeReference = this.metadata.GetTypeReference(rid);
			if (typeReference != null)
			{
				return typeReference;
			}
			return this.ReadTypeReference(rid);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00016680 File Offset: 0x00014880
		private TypeReference ReadTypeReference(uint rid)
		{
			if (!this.MoveTo(Table.TypeRef, rid))
			{
				return null;
			}
			TypeReference typeReference = null;
			MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.ResolutionScope);
			string text = this.ReadString();
			TypeReference typeReference2 = new TypeReference(this.ReadString(), text, this.module, null);
			typeReference2.token = new MetadataToken(TokenType.TypeRef, rid);
			this.metadata.AddTypeReference(typeReference2);
			IMetadataScope metadataScope3;
			if (metadataToken.TokenType == TokenType.TypeRef)
			{
				if (metadataToken.RID != rid)
				{
					typeReference = this.GetTypeDefOrRef(metadataToken);
					IMetadataScope metadataScope2;
					if (typeReference == null)
					{
						IMetadataScope metadataScope = this.module;
						metadataScope2 = metadataScope;
					}
					else
					{
						metadataScope2 = typeReference.Scope;
					}
					metadataScope3 = metadataScope2;
				}
				else
				{
					metadataScope3 = this.module;
				}
			}
			else
			{
				metadataScope3 = this.GetTypeReferenceScope(metadataToken);
			}
			typeReference2.scope = metadataScope3;
			typeReference2.DeclaringType = typeReference;
			MetadataSystem.TryProcessPrimitiveTypeReference(typeReference2);
			if (typeReference2.Module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(typeReference2);
			}
			return typeReference2;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00016758 File Offset: 0x00014958
		private IMetadataScope GetTypeReferenceScope(MetadataToken scope)
		{
			if (scope.TokenType == TokenType.Module)
			{
				return this.module;
			}
			TokenType tokenType = scope.TokenType;
			IMetadataScope[] array2;
			if (tokenType != TokenType.ModuleRef)
			{
				if (tokenType != TokenType.AssemblyRef)
				{
					throw new NotSupportedException();
				}
				this.InitializeAssemblyReferences();
				IMetadataScope[] array = this.metadata.AssemblyReferences;
				array2 = array;
			}
			else
			{
				this.InitializeModuleReferences();
				IMetadataScope[] array = this.metadata.ModuleReferences;
				array2 = array;
			}
			uint num = scope.RID - 1U;
			if (num < 0U || (ulong)num >= (ulong)((long)array2.Length))
			{
				return null;
			}
			return array2[(int)num];
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000167DC File Offset: 0x000149DC
		public IEnumerable<TypeReference> GetTypeReferences()
		{
			this.InitializeTypeReferences();
			int tableLength = this.image.GetTableLength(Table.TypeRef);
			TypeReference[] array = new TypeReference[tableLength];
			uint num = 1U;
			while ((ulong)num <= (ulong)((long)tableLength))
			{
				array[(int)(num - 1U)] = this.GetTypeReference(num);
				num += 1U;
			}
			return array;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00016820 File Offset: 0x00014A20
		private TypeReference GetTypeSpecification(uint rid)
		{
			if (!this.MoveTo(Table.TypeSpec, rid))
			{
				return null;
			}
			TypeReference typeReference = this.ReadSignature(this.ReadBlobIndex()).ReadTypeSignature();
			if (typeReference.token.RID == 0U)
			{
				typeReference.token = new MetadataToken(TokenType.TypeSpec, rid);
			}
			return typeReference;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001686B File Offset: 0x00014A6B
		private SignatureReader ReadSignature(uint signature)
		{
			return new SignatureReader(signature, this);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00016874 File Offset: 0x00014A74
		public bool HasInterfaces(TypeDefinition type)
		{
			this.InitializeInterfaces();
			Collection<Row<uint, MetadataToken>> collection;
			return this.metadata.TryGetInterfaceMapping(type, out collection);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00016898 File Offset: 0x00014A98
		public InterfaceImplementationCollection ReadInterfaces(TypeDefinition type)
		{
			this.InitializeInterfaces();
			Collection<Row<uint, MetadataToken>> collection;
			if (!this.metadata.TryGetInterfaceMapping(type, out collection))
			{
				return new InterfaceImplementationCollection(type);
			}
			InterfaceImplementationCollection interfaceImplementationCollection = new InterfaceImplementationCollection(type, collection.Count);
			this.context = type;
			for (int i = 0; i < collection.Count; i++)
			{
				interfaceImplementationCollection.Add(new InterfaceImplementation(this.GetTypeDefOrRef(collection[i].Col2), new MetadataToken(TokenType.InterfaceImpl, collection[i].Col1)));
			}
			this.metadata.RemoveInterfaceMapping(type);
			return interfaceImplementationCollection;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00016928 File Offset: 0x00014B28
		private void InitializeInterfaces()
		{
			if (this.metadata.Interfaces != null)
			{
				return;
			}
			int num = this.MoveTo(Table.InterfaceImpl);
			this.metadata.Interfaces = new Dictionary<uint, Collection<Row<uint, MetadataToken>>>(num);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadTableIndex(Table.TypeDef);
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.TypeDefOrRef);
				this.AddInterfaceMapping(num3, new Row<uint, MetadataToken>(num2, metadataToken));
				num2 += 1U;
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00016989 File Offset: 0x00014B89
		private void AddInterfaceMapping(uint type, Row<uint, MetadataToken> @interface)
		{
			this.metadata.SetInterfaceMapping(type, MetadataReader.AddMapping<uint, Row<uint, MetadataToken>>(this.metadata.Interfaces, type, @interface));
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000169AC File Offset: 0x00014BAC
		public Collection<FieldDefinition> ReadFields(TypeDefinition type)
		{
			Range fields_range = type.fields_range;
			if (fields_range.Length == 0U)
			{
				return new MemberDefinitionCollection<FieldDefinition>(type);
			}
			MemberDefinitionCollection<FieldDefinition> memberDefinitionCollection = new MemberDefinitionCollection<FieldDefinition>(type, (int)fields_range.Length);
			this.context = type;
			if (!this.MoveTo(Table.FieldPtr, fields_range.Start))
			{
				if (!this.MoveTo(Table.Field, fields_range.Start))
				{
					return memberDefinitionCollection;
				}
				for (uint num = 0U; num < fields_range.Length; num += 1U)
				{
					this.ReadField(fields_range.Start + num, memberDefinitionCollection);
				}
			}
			else
			{
				this.ReadPointers<FieldDefinition>(Table.FieldPtr, Table.Field, fields_range, memberDefinitionCollection, new Action<uint, Collection<FieldDefinition>>(this.ReadField));
			}
			return memberDefinitionCollection;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00016A3C File Offset: 0x00014C3C
		private void ReadField(uint field_rid, Collection<FieldDefinition> fields)
		{
			FieldAttributes fieldAttributes = (FieldAttributes)base.ReadUInt16();
			string text = this.ReadString();
			uint num = this.ReadBlobIndex();
			FieldDefinition fieldDefinition = new FieldDefinition(text, fieldAttributes, this.ReadFieldType(num));
			fieldDefinition.token = new MetadataToken(TokenType.Field, field_rid);
			this.metadata.AddFieldDefinition(fieldDefinition);
			if (MetadataReader.IsDeleted(fieldDefinition))
			{
				return;
			}
			fields.Add(fieldDefinition);
			if (this.module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(fieldDefinition);
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00016AAB File Offset: 0x00014CAB
		private void InitializeFields()
		{
			if (this.metadata.Fields != null)
			{
				return;
			}
			this.metadata.Fields = new FieldDefinition[this.image.GetTableLength(Table.Field)];
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00016AD7 File Offset: 0x00014CD7
		private TypeReference ReadFieldType(uint signature)
		{
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.ReadByte() != 6)
			{
				throw new NotSupportedException();
			}
			return signatureReader.ReadTypeSignature();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00016AF4 File Offset: 0x00014CF4
		public int ReadFieldRVA(FieldDefinition field)
		{
			this.InitializeFieldRVAs();
			uint rid = field.token.RID;
			uint num;
			if (!this.metadata.FieldRVAs.TryGetValue(rid, out num))
			{
				return 0;
			}
			int fieldTypeSize = MetadataReader.GetFieldTypeSize(field.FieldType);
			if (fieldTypeSize == 0 || num == 0U)
			{
				return 0;
			}
			this.metadata.FieldRVAs.Remove(rid);
			field.InitialValue = this.GetFieldInitializeValue(fieldTypeSize, num);
			return (int)num;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00016B5F File Offset: 0x00014D5F
		private byte[] GetFieldInitializeValue(int size, uint rva)
		{
			return this.image.GetReaderAt<int, byte[]>(rva, size, (int s, BinaryStreamReader reader) => reader.ReadBytes(s)) ?? Empty<byte>.Array;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00016B98 File Offset: 0x00014D98
		private static int GetFieldTypeSize(TypeReference type)
		{
			int num = 0;
			switch (type.etype)
			{
			case ElementType.Boolean:
			case ElementType.I1:
			case ElementType.U1:
				return 1;
			case ElementType.Char:
			case ElementType.I2:
			case ElementType.U2:
				return 2;
			case ElementType.I4:
			case ElementType.U4:
			case ElementType.R4:
				return 4;
			case ElementType.I8:
			case ElementType.U8:
			case ElementType.R8:
				return 8;
			case ElementType.Ptr:
			case ElementType.FnPtr:
				return IntPtr.Size;
			case ElementType.CModReqD:
			case ElementType.CModOpt:
				return MetadataReader.GetFieldTypeSize(((IModifierType)type).ElementType);
			}
			TypeDefinition typeDefinition = type.Resolve();
			if (typeDefinition != null && typeDefinition.HasLayoutInfo)
			{
				num = typeDefinition.ClassSize;
			}
			return num;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00016C78 File Offset: 0x00014E78
		private void InitializeFieldRVAs()
		{
			if (this.metadata.FieldRVAs != null)
			{
				return;
			}
			int num = this.MoveTo(Table.FieldRVA);
			Dictionary<uint, uint> dictionary = (this.metadata.FieldRVAs = new Dictionary<uint, uint>(num));
			for (int i = 0; i < num; i++)
			{
				uint num2 = base.ReadUInt32();
				uint num3 = this.ReadTableIndex(Table.Field);
				dictionary.Add(num3, num2);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00016CD8 File Offset: 0x00014ED8
		public int ReadFieldLayout(FieldDefinition field)
		{
			this.InitializeFieldLayouts();
			uint rid = field.token.RID;
			uint num;
			if (!this.metadata.FieldLayouts.TryGetValue(rid, out num))
			{
				return -1;
			}
			this.metadata.FieldLayouts.Remove(rid);
			return (int)num;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00016D24 File Offset: 0x00014F24
		private void InitializeFieldLayouts()
		{
			if (this.metadata.FieldLayouts != null)
			{
				return;
			}
			int num = this.MoveTo(Table.FieldLayout);
			Dictionary<uint, uint> dictionary = (this.metadata.FieldLayouts = new Dictionary<uint, uint>(num));
			for (int i = 0; i < num; i++)
			{
				uint num2 = base.ReadUInt32();
				uint num3 = this.ReadTableIndex(Table.Field);
				dictionary.Add(num3, num2);
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00016D84 File Offset: 0x00014F84
		public bool HasEvents(TypeDefinition type)
		{
			this.InitializeEvents();
			Range range;
			return this.metadata.TryGetEventsRange(type, out range) && range.Length > 0U;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00016DB4 File Offset: 0x00014FB4
		public Collection<EventDefinition> ReadEvents(TypeDefinition type)
		{
			this.InitializeEvents();
			Range range;
			if (!this.metadata.TryGetEventsRange(type, out range))
			{
				return new MemberDefinitionCollection<EventDefinition>(type);
			}
			MemberDefinitionCollection<EventDefinition> memberDefinitionCollection = new MemberDefinitionCollection<EventDefinition>(type, (int)range.Length);
			this.metadata.RemoveEventsRange(type);
			if (range.Length == 0U)
			{
				return memberDefinitionCollection;
			}
			this.context = type;
			if (!this.MoveTo(Table.EventPtr, range.Start))
			{
				if (!this.MoveTo(Table.Event, range.Start))
				{
					return memberDefinitionCollection;
				}
				for (uint num = 0U; num < range.Length; num += 1U)
				{
					this.ReadEvent(range.Start + num, memberDefinitionCollection);
				}
			}
			else
			{
				this.ReadPointers<EventDefinition>(Table.EventPtr, Table.Event, range, memberDefinitionCollection, new Action<uint, Collection<EventDefinition>>(this.ReadEvent));
			}
			return memberDefinitionCollection;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00016E68 File Offset: 0x00015068
		private void ReadEvent(uint event_rid, Collection<EventDefinition> events)
		{
			EventAttributes eventAttributes = (EventAttributes)base.ReadUInt16();
			string text = this.ReadString();
			TypeReference typeDefOrRef = this.GetTypeDefOrRef(this.ReadMetadataToken(CodedIndex.TypeDefOrRef));
			EventDefinition eventDefinition = new EventDefinition(text, eventAttributes, typeDefOrRef);
			eventDefinition.token = new MetadataToken(TokenType.Event, event_rid);
			if (MetadataReader.IsDeleted(eventDefinition))
			{
				return;
			}
			events.Add(eventDefinition);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00016EBC File Offset: 0x000150BC
		private void InitializeEvents()
		{
			if (this.metadata.Events != null)
			{
				return;
			}
			int num = this.MoveTo(Table.EventMap);
			this.metadata.Events = new Dictionary<uint, Range>(num);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadTableIndex(Table.TypeDef);
				Range range = this.ReadListRange(num2, Table.EventMap, Table.Event);
				this.metadata.AddEventsRange(num3, range);
				num2 += 1U;
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00016F20 File Offset: 0x00015120
		public bool HasProperties(TypeDefinition type)
		{
			this.InitializeProperties();
			Range range;
			return this.metadata.TryGetPropertiesRange(type, out range) && range.Length > 0U;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00016F50 File Offset: 0x00015150
		public Collection<PropertyDefinition> ReadProperties(TypeDefinition type)
		{
			this.InitializeProperties();
			Range range;
			if (!this.metadata.TryGetPropertiesRange(type, out range))
			{
				return new MemberDefinitionCollection<PropertyDefinition>(type);
			}
			this.metadata.RemovePropertiesRange(type);
			MemberDefinitionCollection<PropertyDefinition> memberDefinitionCollection = new MemberDefinitionCollection<PropertyDefinition>(type, (int)range.Length);
			if (range.Length == 0U)
			{
				return memberDefinitionCollection;
			}
			this.context = type;
			if (!this.MoveTo(Table.PropertyPtr, range.Start))
			{
				if (!this.MoveTo(Table.Property, range.Start))
				{
					return memberDefinitionCollection;
				}
				for (uint num = 0U; num < range.Length; num += 1U)
				{
					this.ReadProperty(range.Start + num, memberDefinitionCollection);
				}
			}
			else
			{
				this.ReadPointers<PropertyDefinition>(Table.PropertyPtr, Table.Property, range, memberDefinitionCollection, new Action<uint, Collection<PropertyDefinition>>(this.ReadProperty));
			}
			return memberDefinitionCollection;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00017004 File Offset: 0x00015204
		private void ReadProperty(uint property_rid, Collection<PropertyDefinition> properties)
		{
			PropertyAttributes propertyAttributes = (PropertyAttributes)base.ReadUInt16();
			string text = this.ReadString();
			uint num = this.ReadBlobIndex();
			SignatureReader signatureReader = this.ReadSignature(num);
			byte b = signatureReader.ReadByte();
			if ((b & 8) == 0)
			{
				throw new NotSupportedException();
			}
			bool flag = (b & 32) > 0;
			signatureReader.ReadCompressedUInt32();
			PropertyDefinition propertyDefinition = new PropertyDefinition(text, propertyAttributes, signatureReader.ReadTypeSignature());
			propertyDefinition.HasThis = flag;
			propertyDefinition.token = new MetadataToken(TokenType.Property, property_rid);
			if (MetadataReader.IsDeleted(propertyDefinition))
			{
				return;
			}
			properties.Add(propertyDefinition);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00017088 File Offset: 0x00015288
		private void InitializeProperties()
		{
			if (this.metadata.Properties != null)
			{
				return;
			}
			int num = this.MoveTo(Table.PropertyMap);
			this.metadata.Properties = new Dictionary<uint, Range>(num);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadTableIndex(Table.TypeDef);
				Range range = this.ReadListRange(num2, Table.PropertyMap, Table.Property);
				this.metadata.AddPropertiesRange(num3, range);
				num2 += 1U;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000170EC File Offset: 0x000152EC
		private MethodSemanticsAttributes ReadMethodSemantics(MethodDefinition method)
		{
			this.InitializeMethodSemantics();
			Row<MethodSemanticsAttributes, MetadataToken> row;
			if (!this.metadata.Semantics.TryGetValue(method.token.RID, out row))
			{
				return MethodSemanticsAttributes.None;
			}
			TypeDefinition declaringType = method.DeclaringType;
			MethodSemanticsAttributes col = row.Col1;
			if (col <= MethodSemanticsAttributes.AddOn)
			{
				switch (col)
				{
				case MethodSemanticsAttributes.Setter:
					MetadataReader.GetProperty(declaringType, row.Col2).set_method = method;
					goto IL_16B;
				case MethodSemanticsAttributes.Getter:
					MetadataReader.GetProperty(declaringType, row.Col2).get_method = method;
					goto IL_16B;
				case MethodSemanticsAttributes.Setter | MethodSemanticsAttributes.Getter:
					break;
				case MethodSemanticsAttributes.Other:
				{
					TokenType tokenType = row.Col2.TokenType;
					if (tokenType == TokenType.Event)
					{
						EventDefinition @event = MetadataReader.GetEvent(declaringType, row.Col2);
						if (@event.other_methods == null)
						{
							@event.other_methods = new Collection<MethodDefinition>();
						}
						@event.other_methods.Add(method);
						goto IL_16B;
					}
					if (tokenType != TokenType.Property)
					{
						throw new NotSupportedException();
					}
					PropertyDefinition property = MetadataReader.GetProperty(declaringType, row.Col2);
					if (property.other_methods == null)
					{
						property.other_methods = new Collection<MethodDefinition>();
					}
					property.other_methods.Add(method);
					goto IL_16B;
				}
				default:
					if (col == MethodSemanticsAttributes.AddOn)
					{
						MetadataReader.GetEvent(declaringType, row.Col2).add_method = method;
						goto IL_16B;
					}
					break;
				}
			}
			else
			{
				if (col == MethodSemanticsAttributes.RemoveOn)
				{
					MetadataReader.GetEvent(declaringType, row.Col2).remove_method = method;
					goto IL_16B;
				}
				if (col == MethodSemanticsAttributes.Fire)
				{
					MetadataReader.GetEvent(declaringType, row.Col2).invoke_method = method;
					goto IL_16B;
				}
			}
			throw new NotSupportedException();
			IL_16B:
			this.metadata.Semantics.Remove(method.token.RID);
			return row.Col1;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00017286 File Offset: 0x00015486
		private static EventDefinition GetEvent(TypeDefinition type, MetadataToken token)
		{
			if (token.TokenType != TokenType.Event)
			{
				throw new ArgumentException();
			}
			return MetadataReader.GetMember<EventDefinition>(type.Events, token);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000172A8 File Offset: 0x000154A8
		private static PropertyDefinition GetProperty(TypeDefinition type, MetadataToken token)
		{
			if (token.TokenType != TokenType.Property)
			{
				throw new ArgumentException();
			}
			return MetadataReader.GetMember<PropertyDefinition>(type.Properties, token);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000172CC File Offset: 0x000154CC
		private static TMember GetMember<TMember>(Collection<TMember> members, MetadataToken token) where TMember : IMemberDefinition
		{
			for (int i = 0; i < members.Count; i++)
			{
				TMember tmember = members[i];
				if (tmember.MetadataToken == token)
				{
					return tmember;
				}
			}
			throw new ArgumentException();
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00017310 File Offset: 0x00015510
		private void InitializeMethodSemantics()
		{
			if (this.metadata.Semantics != null)
			{
				return;
			}
			int num = this.MoveTo(Table.MethodSemantics);
			Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>> dictionary = (this.metadata.Semantics = new Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>>(0));
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				MethodSemanticsAttributes methodSemanticsAttributes = (MethodSemanticsAttributes)base.ReadUInt16();
				uint num3 = this.ReadTableIndex(Table.Method);
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasSemantics);
				dictionary[num3] = new Row<MethodSemanticsAttributes, MetadataToken>(methodSemanticsAttributes, metadataToken);
				num2 += 1U;
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00017382 File Offset: 0x00015582
		public void ReadMethods(PropertyDefinition property)
		{
			this.ReadAllSemantics(property.DeclaringType);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00017390 File Offset: 0x00015590
		public void ReadMethods(EventDefinition @event)
		{
			this.ReadAllSemantics(@event.DeclaringType);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001739E File Offset: 0x0001559E
		public void ReadAllSemantics(MethodDefinition method)
		{
			this.ReadAllSemantics(method.DeclaringType);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000173AC File Offset: 0x000155AC
		private void ReadAllSemantics(TypeDefinition type)
		{
			Collection<MethodDefinition> methods = type.Methods;
			for (int i = 0; i < methods.Count; i++)
			{
				MethodDefinition methodDefinition = methods[i];
				if (!methodDefinition.sem_attrs_ready)
				{
					methodDefinition.sem_attrs = this.ReadMethodSemantics(methodDefinition);
					methodDefinition.sem_attrs_ready = true;
				}
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000173FC File Offset: 0x000155FC
		public Collection<MethodDefinition> ReadMethods(TypeDefinition type)
		{
			Range methods_range = type.methods_range;
			if (methods_range.Length == 0U)
			{
				return new MemberDefinitionCollection<MethodDefinition>(type);
			}
			MemberDefinitionCollection<MethodDefinition> memberDefinitionCollection = new MemberDefinitionCollection<MethodDefinition>(type, (int)methods_range.Length);
			if (!this.MoveTo(Table.MethodPtr, methods_range.Start))
			{
				if (!this.MoveTo(Table.Method, methods_range.Start))
				{
					return memberDefinitionCollection;
				}
				for (uint num = 0U; num < methods_range.Length; num += 1U)
				{
					this.ReadMethod(methods_range.Start + num, memberDefinitionCollection);
				}
			}
			else
			{
				this.ReadPointers<MethodDefinition>(Table.MethodPtr, Table.Method, methods_range, memberDefinitionCollection, new Action<uint, Collection<MethodDefinition>>(this.ReadMethod));
			}
			return memberDefinitionCollection;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00017488 File Offset: 0x00015688
		private void ReadPointers<TMember>(Table ptr, Table table, Range range, Collection<TMember> members, Action<uint, Collection<TMember>> reader) where TMember : IMemberDefinition
		{
			for (uint num = 0U; num < range.Length; num += 1U)
			{
				this.MoveTo(ptr, range.Start + num);
				uint num2 = this.ReadTableIndex(table);
				this.MoveTo(table, num2);
				reader(num2, members);
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000174D1 File Offset: 0x000156D1
		private static bool IsDeleted(IMemberDefinition member)
		{
			return member.IsSpecialName && member.Name == "_Deleted";
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000174ED File Offset: 0x000156ED
		private void InitializeMethods()
		{
			if (this.metadata.Methods != null)
			{
				return;
			}
			this.metadata.Methods = new MethodDefinition[this.image.GetTableLength(Table.Method)];
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001751C File Offset: 0x0001571C
		private void ReadMethod(uint method_rid, Collection<MethodDefinition> methods)
		{
			MethodDefinition methodDefinition = new MethodDefinition();
			methodDefinition.rva = base.ReadUInt32();
			methodDefinition.ImplAttributes = (MethodImplAttributes)base.ReadUInt16();
			methodDefinition.Attributes = (MethodAttributes)base.ReadUInt16();
			methodDefinition.Name = this.ReadString();
			methodDefinition.token = new MetadataToken(TokenType.Method, method_rid);
			if (MetadataReader.IsDeleted(methodDefinition))
			{
				return;
			}
			methods.Add(methodDefinition);
			uint num = this.ReadBlobIndex();
			Range range = this.ReadListRange(method_rid, Table.Method, Table.Param);
			this.context = methodDefinition;
			this.ReadMethodSignature(num, methodDefinition);
			this.metadata.AddMethodDefinition(methodDefinition);
			if (range.Length != 0U)
			{
				int position = this.position;
				this.ReadParameters(methodDefinition, range);
				this.position = position;
			}
			if (this.module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(methodDefinition);
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000175E0 File Offset: 0x000157E0
		private void ReadParameters(MethodDefinition method, Range param_range)
		{
			if (this.MoveTo(Table.ParamPtr, param_range.Start))
			{
				this.ReadParameterPointers(method, param_range);
				return;
			}
			if (!this.MoveTo(Table.Param, param_range.Start))
			{
				return;
			}
			for (uint num = 0U; num < param_range.Length; num += 1U)
			{
				this.ReadParameter(param_range.Start + num, method);
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00017638 File Offset: 0x00015838
		private void ReadParameterPointers(MethodDefinition method, Range range)
		{
			for (uint num = 0U; num < range.Length; num += 1U)
			{
				this.MoveTo(Table.ParamPtr, range.Start + num);
				uint num2 = this.ReadTableIndex(Table.Param);
				this.MoveTo(Table.Param, num2);
				this.ReadParameter(num2, method);
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00017680 File Offset: 0x00015880
		private void ReadParameter(uint param_rid, MethodDefinition method)
		{
			ParameterAttributes parameterAttributes = (ParameterAttributes)base.ReadUInt16();
			ushort num = base.ReadUInt16();
			string text = this.ReadString();
			ParameterDefinition parameterDefinition = ((num == 0) ? method.MethodReturnType.Parameter : method.Parameters[(int)(num - 1)]);
			parameterDefinition.token = new MetadataToken(TokenType.Param, param_rid);
			parameterDefinition.Name = text;
			parameterDefinition.Attributes = parameterAttributes;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000176DE File Offset: 0x000158DE
		private void ReadMethodSignature(uint signature, IMethodSignature method)
		{
			this.ReadSignature(signature).ReadMethodSignature(method);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x000176F0 File Offset: 0x000158F0
		public PInvokeInfo ReadPInvokeInfo(MethodDefinition method)
		{
			this.InitializePInvokes();
			uint rid = method.token.RID;
			Row<PInvokeAttributes, uint, uint> row;
			if (!this.metadata.PInvokes.TryGetValue(rid, out row))
			{
				return null;
			}
			this.metadata.PInvokes.Remove(rid);
			return new PInvokeInfo(row.Col1, this.image.StringHeap.Read(row.Col2), this.module.ModuleReferences[(int)(row.Col3 - 1U)]);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00017774 File Offset: 0x00015974
		private void InitializePInvokes()
		{
			if (this.metadata.PInvokes != null)
			{
				return;
			}
			int num = this.MoveTo(Table.ImplMap);
			Dictionary<uint, Row<PInvokeAttributes, uint, uint>> dictionary = (this.metadata.PInvokes = new Dictionary<uint, Row<PInvokeAttributes, uint, uint>>(num));
			for (int i = 1; i <= num; i++)
			{
				PInvokeAttributes pinvokeAttributes = (PInvokeAttributes)base.ReadUInt16();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.MemberForwarded);
				uint num2 = this.ReadStringIndex();
				uint num3 = this.ReadTableIndex(Table.File);
				if (metadataToken.TokenType == TokenType.Method)
				{
					dictionary.Add(metadataToken.RID, new Row<PInvokeAttributes, uint, uint>(pinvokeAttributes, num2, num3));
				}
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00017804 File Offset: 0x00015A04
		public bool HasGenericParameters(IGenericParameterProvider provider)
		{
			this.InitializeGenericParameters();
			Range[] array;
			return this.metadata.TryGetGenericParameterRanges(provider, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00017834 File Offset: 0x00015A34
		public Collection<GenericParameter> ReadGenericParameters(IGenericParameterProvider provider)
		{
			this.InitializeGenericParameters();
			Range[] array;
			if (!this.metadata.TryGetGenericParameterRanges(provider, out array))
			{
				return new GenericParameterCollection(provider);
			}
			this.metadata.RemoveGenericParameterRange(provider);
			GenericParameterCollection genericParameterCollection = new GenericParameterCollection(provider, MetadataReader.RangesSize(array));
			for (int i = 0; i < array.Length; i++)
			{
				this.ReadGenericParametersRange(array[i], provider, genericParameterCollection);
			}
			return genericParameterCollection;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00017898 File Offset: 0x00015A98
		private void ReadGenericParametersRange(Range range, IGenericParameterProvider provider, GenericParameterCollection generic_parameters)
		{
			if (!this.MoveTo(Table.GenericParam, range.Start))
			{
				return;
			}
			for (uint num = 0U; num < range.Length; num += 1U)
			{
				base.ReadUInt16();
				GenericParameterAttributes genericParameterAttributes = (GenericParameterAttributes)base.ReadUInt16();
				this.ReadMetadataToken(CodedIndex.TypeOrMethodDef);
				generic_parameters.Add(new GenericParameter(this.ReadString(), provider)
				{
					token = new MetadataToken(TokenType.GenericParam, range.Start + num),
					Attributes = genericParameterAttributes
				});
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00017911 File Offset: 0x00015B11
		private void InitializeGenericParameters()
		{
			if (this.metadata.GenericParameters != null)
			{
				return;
			}
			this.metadata.GenericParameters = this.InitializeRanges(Table.GenericParam, delegate
			{
				base.Advance(4);
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.TypeOrMethodDef);
				this.ReadStringIndex();
				return metadataToken;
			});
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00017940 File Offset: 0x00015B40
		private Dictionary<MetadataToken, Range[]> InitializeRanges(Table table, Func<MetadataToken> get_next)
		{
			int num = this.MoveTo(table);
			Dictionary<MetadataToken, Range[]> dictionary = new Dictionary<MetadataToken, Range[]>(num);
			if (num == 0)
			{
				return dictionary;
			}
			MetadataToken metadataToken = MetadataToken.Zero;
			Range range = new Range(1U, 0U);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				MetadataToken metadataToken2 = get_next();
				if (num2 == 1U)
				{
					metadataToken = metadataToken2;
					range.Length += 1U;
				}
				else if (metadataToken2 != metadataToken)
				{
					MetadataReader.AddRange(dictionary, metadataToken, range);
					range = new Range(num2, 1U);
					metadataToken = metadataToken2;
				}
				else
				{
					range.Length += 1U;
				}
				num2 += 1U;
			}
			MetadataReader.AddRange(dictionary, metadataToken, range);
			return dictionary;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000179D8 File Offset: 0x00015BD8
		private static void AddRange(Dictionary<MetadataToken, Range[]> ranges, MetadataToken owner, Range range)
		{
			if (owner.RID == 0U)
			{
				return;
			}
			Range[] array;
			if (!ranges.TryGetValue(owner, out array))
			{
				ranges.Add(owner, new Range[] { range });
				return;
			}
			ranges[owner] = array.Add(range);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00017A20 File Offset: 0x00015C20
		public bool HasGenericConstraints(GenericParameter generic_parameter)
		{
			this.InitializeGenericConstraints();
			Collection<Row<uint, MetadataToken>> collection;
			return this.metadata.TryGetGenericConstraintMapping(generic_parameter, out collection) && collection.Count > 0;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00017A50 File Offset: 0x00015C50
		public GenericParameterConstraintCollection ReadGenericConstraints(GenericParameter generic_parameter)
		{
			this.InitializeGenericConstraints();
			Collection<Row<uint, MetadataToken>> collection;
			if (!this.metadata.TryGetGenericConstraintMapping(generic_parameter, out collection))
			{
				return new GenericParameterConstraintCollection(generic_parameter);
			}
			GenericParameterConstraintCollection genericParameterConstraintCollection = new GenericParameterConstraintCollection(generic_parameter, collection.Count);
			this.context = (IGenericContext)generic_parameter.Owner;
			for (int i = 0; i < collection.Count; i++)
			{
				genericParameterConstraintCollection.Add(new GenericParameterConstraint(this.GetTypeDefOrRef(collection[i].Col2), new MetadataToken(TokenType.GenericParamConstraint, collection[i].Col1)));
			}
			this.metadata.RemoveGenericConstraintMapping(generic_parameter);
			return genericParameterConstraintCollection;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00017AEC File Offset: 0x00015CEC
		private void InitializeGenericConstraints()
		{
			if (this.metadata.GenericConstraints != null)
			{
				return;
			}
			int num = this.MoveTo(Table.GenericParamConstraint);
			this.metadata.GenericConstraints = new Dictionary<uint, Collection<Row<uint, MetadataToken>>>(num);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				this.AddGenericConstraintMapping(this.ReadTableIndex(Table.GenericParam), new Row<uint, MetadataToken>(num2, this.ReadMetadataToken(CodedIndex.TypeDefOrRef)));
				num2 += 1U;
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00017B4A File Offset: 0x00015D4A
		private void AddGenericConstraintMapping(uint generic_parameter, Row<uint, MetadataToken> constraint)
		{
			this.metadata.SetGenericConstraintMapping(generic_parameter, MetadataReader.AddMapping<uint, Row<uint, MetadataToken>>(this.metadata.GenericConstraints, generic_parameter, constraint));
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00017B6C File Offset: 0x00015D6C
		public bool HasOverrides(MethodDefinition method)
		{
			this.InitializeOverrides();
			Collection<MetadataToken> collection;
			return this.metadata.TryGetOverrideMapping(method, out collection) && collection.Count > 0;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00017B9C File Offset: 0x00015D9C
		public Collection<MethodReference> ReadOverrides(MethodDefinition method)
		{
			this.InitializeOverrides();
			Collection<MetadataToken> collection;
			if (!this.metadata.TryGetOverrideMapping(method, out collection))
			{
				return new Collection<MethodReference>();
			}
			Collection<MethodReference> collection2 = new Collection<MethodReference>(collection.Count);
			this.context = method;
			for (int i = 0; i < collection.Count; i++)
			{
				collection2.Add((MethodReference)this.LookupToken(collection[i]));
			}
			this.metadata.RemoveOverrideMapping(method);
			return collection2;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00017C10 File Offset: 0x00015E10
		private void InitializeOverrides()
		{
			if (this.metadata.Overrides != null)
			{
				return;
			}
			int num = this.MoveTo(Table.MethodImpl);
			this.metadata.Overrides = new Dictionary<uint, Collection<MetadataToken>>(num);
			for (int i = 1; i <= num; i++)
			{
				this.ReadTableIndex(Table.TypeDef);
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.MethodDefOrRef);
				if (metadataToken.TokenType != TokenType.Method)
				{
					throw new NotSupportedException();
				}
				MetadataToken metadataToken2 = this.ReadMetadataToken(CodedIndex.MethodDefOrRef);
				this.AddOverrideMapping(metadataToken.RID, metadataToken2);
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00017C8B File Offset: 0x00015E8B
		private void AddOverrideMapping(uint method_rid, MetadataToken @override)
		{
			this.metadata.SetOverrideMapping(method_rid, MetadataReader.AddMapping<uint, MetadataToken>(this.metadata.Overrides, method_rid, @override));
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00017CAB File Offset: 0x00015EAB
		public MethodBody ReadMethodBody(MethodDefinition method)
		{
			return this.code.ReadMethodBody(method);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00017CB9 File Offset: 0x00015EB9
		public int ReadCodeSize(MethodDefinition method)
		{
			return this.code.ReadCodeSize(method);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00017CC8 File Offset: 0x00015EC8
		public CallSite ReadCallSite(MetadataToken token)
		{
			if (!this.MoveTo(Table.StandAloneSig, token.RID))
			{
				return null;
			}
			uint num = this.ReadBlobIndex();
			CallSite callSite = new CallSite();
			this.ReadMethodSignature(num, callSite);
			callSite.MetadataToken = token;
			return callSite;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00017D08 File Offset: 0x00015F08
		public VariableDefinitionCollection ReadVariables(MetadataToken local_var_token)
		{
			if (!this.MoveTo(Table.StandAloneSig, local_var_token.RID))
			{
				return null;
			}
			SignatureReader signatureReader = this.ReadSignature(this.ReadBlobIndex());
			if (signatureReader.ReadByte() != 7)
			{
				throw new NotSupportedException();
			}
			uint num = signatureReader.ReadCompressedUInt32();
			if (num == 0U)
			{
				return null;
			}
			VariableDefinitionCollection variableDefinitionCollection = new VariableDefinitionCollection((int)num);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				variableDefinitionCollection.Add(new VariableDefinition(signatureReader.ReadTypeSignature()));
				num2++;
			}
			return variableDefinitionCollection;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00017D78 File Offset: 0x00015F78
		public IMetadataTokenProvider LookupToken(MetadataToken token)
		{
			uint rid = token.RID;
			if (rid == 0U)
			{
				return null;
			}
			if (this.metadata_reader != null)
			{
				return this.metadata_reader.LookupToken(token);
			}
			int position = this.position;
			IGenericContext genericContext = this.context;
			TokenType tokenType = token.TokenType;
			IMetadataTokenProvider metadataTokenProvider;
			if (tokenType <= TokenType.Field)
			{
				if (tokenType == TokenType.TypeRef)
				{
					metadataTokenProvider = this.GetTypeReference(rid);
					goto IL_D8;
				}
				if (tokenType == TokenType.TypeDef)
				{
					metadataTokenProvider = this.GetTypeDefinition(rid);
					goto IL_D8;
				}
				if (tokenType == TokenType.Field)
				{
					metadataTokenProvider = this.GetFieldDefinition(rid);
					goto IL_D8;
				}
			}
			else if (tokenType <= TokenType.MemberRef)
			{
				if (tokenType == TokenType.Method)
				{
					metadataTokenProvider = this.GetMethodDefinition(rid);
					goto IL_D8;
				}
				if (tokenType == TokenType.MemberRef)
				{
					metadataTokenProvider = this.GetMemberReference(rid);
					goto IL_D8;
				}
			}
			else
			{
				if (tokenType == TokenType.TypeSpec)
				{
					metadataTokenProvider = this.GetTypeSpecification(rid);
					goto IL_D8;
				}
				if (tokenType == TokenType.MethodSpec)
				{
					metadataTokenProvider = this.GetMethodSpecification(rid);
					goto IL_D8;
				}
			}
			return null;
			IL_D8:
			this.position = position;
			this.context = genericContext;
			return metadataTokenProvider;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00017E6C File Offset: 0x0001606C
		public FieldDefinition GetFieldDefinition(uint rid)
		{
			this.InitializeTypeDefinitions();
			FieldDefinition fieldDefinition = this.metadata.GetFieldDefinition(rid);
			if (fieldDefinition != null)
			{
				return fieldDefinition;
			}
			return this.LookupField(rid);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00017E98 File Offset: 0x00016098
		private FieldDefinition LookupField(uint rid)
		{
			TypeDefinition fieldDeclaringType = this.metadata.GetFieldDeclaringType(rid);
			if (fieldDeclaringType == null)
			{
				return null;
			}
			Mixin.Read(fieldDeclaringType.Fields);
			return this.metadata.GetFieldDefinition(rid);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00017ED0 File Offset: 0x000160D0
		public MethodDefinition GetMethodDefinition(uint rid)
		{
			this.InitializeTypeDefinitions();
			MethodDefinition methodDefinition = this.metadata.GetMethodDefinition(rid);
			if (methodDefinition != null)
			{
				return methodDefinition;
			}
			return this.LookupMethod(rid);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00017EFC File Offset: 0x000160FC
		private MethodDefinition LookupMethod(uint rid)
		{
			TypeDefinition methodDeclaringType = this.metadata.GetMethodDeclaringType(rid);
			if (methodDeclaringType == null)
			{
				return null;
			}
			Mixin.Read(methodDeclaringType.Methods);
			return this.metadata.GetMethodDefinition(rid);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00017F34 File Offset: 0x00016134
		private MethodSpecification GetMethodSpecification(uint rid)
		{
			if (!this.MoveTo(Table.MethodSpec, rid))
			{
				return null;
			}
			MethodReference methodReference = (MethodReference)this.LookupToken(this.ReadMetadataToken(CodedIndex.MethodDefOrRef));
			uint num = this.ReadBlobIndex();
			MethodSpecification methodSpecification = this.ReadMethodSpecSignature(num, methodReference);
			methodSpecification.token = new MetadataToken(TokenType.MethodSpec, rid);
			return methodSpecification;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00017F84 File Offset: 0x00016184
		private MethodSpecification ReadMethodSpecSignature(uint signature, MethodReference method)
		{
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.ReadByte() != 10)
			{
				throw new NotSupportedException();
			}
			uint num = signatureReader.ReadCompressedUInt32();
			GenericInstanceMethod genericInstanceMethod = new GenericInstanceMethod(method, (int)num);
			signatureReader.ReadGenericInstanceSignature(method, genericInstanceMethod, num);
			return genericInstanceMethod;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00017FC0 File Offset: 0x000161C0
		private MemberReference GetMemberReference(uint rid)
		{
			this.InitializeMemberReferences();
			MemberReference memberReference = this.metadata.GetMemberReference(rid);
			if (memberReference != null)
			{
				return memberReference;
			}
			memberReference = this.ReadMemberReference(rid);
			if (memberReference != null && !memberReference.ContainsGenericParameter)
			{
				this.metadata.AddMemberReference(memberReference);
			}
			return memberReference;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00018008 File Offset: 0x00016208
		private MemberReference ReadMemberReference(uint rid)
		{
			if (!this.MoveTo(Table.MemberRef, rid))
			{
				return null;
			}
			MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.MemberRefParent);
			string text = this.ReadString();
			uint num = this.ReadBlobIndex();
			TokenType tokenType = metadataToken.TokenType;
			MemberReference memberReference;
			if (tokenType <= TokenType.TypeDef)
			{
				if (tokenType != TokenType.TypeRef && tokenType != TokenType.TypeDef)
				{
					goto IL_73;
				}
			}
			else
			{
				if (tokenType == TokenType.Method)
				{
					memberReference = this.ReadMethodMemberReference(metadataToken, text, num);
					goto IL_79;
				}
				if (tokenType != TokenType.TypeSpec)
				{
					goto IL_73;
				}
			}
			memberReference = this.ReadTypeMemberReference(metadataToken, text, num);
			goto IL_79;
			IL_73:
			throw new NotSupportedException();
			IL_79:
			memberReference.token = new MetadataToken(TokenType.MemberRef, rid);
			if (this.module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(memberReference);
			}
			return memberReference;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000180B4 File Offset: 0x000162B4
		private MemberReference ReadTypeMemberReference(MetadataToken type, string name, uint signature)
		{
			TypeReference typeDefOrRef = this.GetTypeDefOrRef(type);
			if (!typeDefOrRef.IsArray)
			{
				this.context = typeDefOrRef;
			}
			MemberReference memberReference = this.ReadMemberReferenceSignature(signature, typeDefOrRef);
			memberReference.Name = name;
			return memberReference;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000180E8 File Offset: 0x000162E8
		private MemberReference ReadMemberReferenceSignature(uint signature, TypeReference declaring_type)
		{
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.buffer[signatureReader.position] == 6)
			{
				signatureReader.position++;
				return new FieldReference
				{
					DeclaringType = declaring_type,
					FieldType = signatureReader.ReadTypeSignature()
				};
			}
			MethodReference methodReference = new MethodReference();
			methodReference.DeclaringType = declaring_type;
			signatureReader.ReadMethodSignature(methodReference);
			return methodReference;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001814C File Offset: 0x0001634C
		private MemberReference ReadMethodMemberReference(MetadataToken token, string name, uint signature)
		{
			MethodDefinition methodDefinition = this.GetMethodDefinition(token.RID);
			this.context = methodDefinition;
			MemberReference memberReference = this.ReadMemberReferenceSignature(signature, methodDefinition.DeclaringType);
			memberReference.Name = name;
			return memberReference;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00018182 File Offset: 0x00016382
		private void InitializeMemberReferences()
		{
			if (this.metadata.MemberReferences != null)
			{
				return;
			}
			this.metadata.MemberReferences = new MemberReference[this.image.GetTableLength(Table.MemberRef)];
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000181B0 File Offset: 0x000163B0
		public IEnumerable<MemberReference> GetMemberReferences()
		{
			this.InitializeMemberReferences();
			int tableLength = this.image.GetTableLength(Table.MemberRef);
			TypeSystem typeSystem = this.module.TypeSystem;
			MethodDefinition methodDefinition = new MethodDefinition(string.Empty, MethodAttributes.Static, typeSystem.Void);
			methodDefinition.DeclaringType = new TypeDefinition(string.Empty, string.Empty, TypeAttributes.Public);
			MemberReference[] array = new MemberReference[tableLength];
			uint num = 1U;
			while ((ulong)num <= (ulong)((long)tableLength))
			{
				this.context = methodDefinition;
				array[(int)(num - 1U)] = this.GetMemberReference(num);
				num += 1U;
			}
			return array;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00018238 File Offset: 0x00016438
		private void InitializeConstants()
		{
			if (this.metadata.Constants != null)
			{
				return;
			}
			int num = this.MoveTo(Table.Constant);
			Dictionary<MetadataToken, Row<ElementType, uint>> dictionary = (this.metadata.Constants = new Dictionary<MetadataToken, Row<ElementType, uint>>(num));
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				ElementType elementType = (ElementType)base.ReadUInt16();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasConstant);
				uint num3 = this.ReadBlobIndex();
				dictionary.Add(metadataToken, new Row<ElementType, uint>(elementType, num3));
				num2 += 1U;
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000182AA File Offset: 0x000164AA
		public TypeReference ReadConstantSignature(MetadataToken token)
		{
			if (token.TokenType != TokenType.Signature)
			{
				throw new NotSupportedException();
			}
			if (token.RID == 0U)
			{
				return null;
			}
			if (!this.MoveTo(Table.StandAloneSig, token.RID))
			{
				return null;
			}
			return this.ReadFieldType(this.ReadBlobIndex());
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000182EC File Offset: 0x000164EC
		public object ReadConstant(IConstantProvider owner)
		{
			this.InitializeConstants();
			Row<ElementType, uint> row;
			if (!this.metadata.Constants.TryGetValue(owner.MetadataToken, out row))
			{
				return Mixin.NoValue;
			}
			this.metadata.Constants.Remove(owner.MetadataToken);
			return this.ReadConstantValue(row.Col1, row.Col2);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00018348 File Offset: 0x00016548
		private object ReadConstantValue(ElementType etype, uint signature)
		{
			if (etype == ElementType.String)
			{
				return this.ReadConstantString(signature);
			}
			if (etype == ElementType.Class || etype == ElementType.Object)
			{
				return null;
			}
			return this.ReadConstantPrimitive(etype, signature);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001836C File Offset: 0x0001656C
		private string ReadConstantString(uint signature)
		{
			byte[] array;
			int num;
			int num2;
			this.GetBlobView(signature, out array, out num, out num2);
			if (num2 == 0)
			{
				return string.Empty;
			}
			if ((num2 & 1) == 1)
			{
				num2--;
			}
			return Encoding.Unicode.GetString(array, num, num2);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000183A6 File Offset: 0x000165A6
		private object ReadConstantPrimitive(ElementType type, uint signature)
		{
			return this.ReadSignature(signature).ReadConstantSignature(type);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000183B5 File Offset: 0x000165B5
		internal void InitializeCustomAttributes()
		{
			if (this.metadata.CustomAttributes != null)
			{
				return;
			}
			this.metadata.CustomAttributes = this.InitializeRanges(Table.CustomAttribute, delegate
			{
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasCustomAttribute);
				this.ReadMetadataToken(CodedIndex.CustomAttributeType);
				this.ReadBlobIndex();
				return metadataToken;
			});
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x000183E4 File Offset: 0x000165E4
		public bool HasCustomAttributes(ICustomAttributeProvider owner)
		{
			this.InitializeCustomAttributes();
			Range[] array;
			return this.metadata.TryGetCustomAttributeRanges(owner, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00018414 File Offset: 0x00016614
		public Collection<CustomAttribute> ReadCustomAttributes(ICustomAttributeProvider owner)
		{
			this.InitializeCustomAttributes();
			Range[] array;
			if (!this.metadata.TryGetCustomAttributeRanges(owner, out array))
			{
				return new Collection<CustomAttribute>();
			}
			Collection<CustomAttribute> collection = new Collection<CustomAttribute>(MetadataReader.RangesSize(array));
			for (int i = 0; i < array.Length; i++)
			{
				this.ReadCustomAttributeRange(array[i], collection);
			}
			this.metadata.RemoveCustomAttributeRange(owner);
			if (this.module.IsWindowsMetadata())
			{
				foreach (CustomAttribute customAttribute in collection)
				{
					WindowsRuntimeProjections.Project(owner, customAttribute);
				}
			}
			return collection;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000184C4 File Offset: 0x000166C4
		private void ReadCustomAttributeRange(Range range, Collection<CustomAttribute> custom_attributes)
		{
			if (!this.MoveTo(Table.CustomAttribute, range.Start))
			{
				return;
			}
			int num = 0;
			while ((long)num < (long)((ulong)range.Length))
			{
				this.ReadMetadataToken(CodedIndex.HasCustomAttribute);
				MethodReference methodReference = (MethodReference)this.LookupToken(this.ReadMetadataToken(CodedIndex.CustomAttributeType));
				uint num2 = this.ReadBlobIndex();
				custom_attributes.Add(new CustomAttribute(num2, methodReference));
				num++;
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00018528 File Offset: 0x00016728
		private static int RangesSize(Range[] ranges)
		{
			uint num = 0U;
			for (int i = 0; i < ranges.Length; i++)
			{
				num += ranges[i].Length;
			}
			return (int)num;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00018558 File Offset: 0x00016758
		public IEnumerable<CustomAttribute> GetCustomAttributes()
		{
			this.InitializeTypeDefinitions();
			uint length = this.image.TableHeap[Table.CustomAttribute].Length;
			Collection<CustomAttribute> collection = new Collection<CustomAttribute>((int)length);
			this.ReadCustomAttributeRange(new Range(1U, length), collection);
			return collection;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00018599 File Offset: 0x00016799
		public byte[] ReadCustomAttributeBlob(uint signature)
		{
			return this.ReadBlob(signature);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000185A4 File Offset: 0x000167A4
		public void ReadCustomAttributeSignature(CustomAttribute attribute)
		{
			SignatureReader signatureReader = this.ReadSignature(attribute.signature);
			if (!signatureReader.CanReadMore())
			{
				return;
			}
			if (signatureReader.ReadUInt16() != 1)
			{
				throw new InvalidOperationException();
			}
			MethodReference constructor = attribute.Constructor;
			if (constructor.HasParameters)
			{
				signatureReader.ReadCustomAttributeConstructorArguments(attribute, constructor.Parameters);
			}
			if (!signatureReader.CanReadMore())
			{
				return;
			}
			ushort num = signatureReader.ReadUInt16();
			if (num == 0)
			{
				return;
			}
			signatureReader.ReadCustomAttributeNamedArguments(num, ref attribute.fields, ref attribute.properties);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001861C File Offset: 0x0001681C
		private void InitializeMarshalInfos()
		{
			if (this.metadata.FieldMarshals != null)
			{
				return;
			}
			int num = this.MoveTo(Table.FieldMarshal);
			Dictionary<MetadataToken, uint> dictionary = (this.metadata.FieldMarshals = new Dictionary<MetadataToken, uint>(num));
			for (int i = 0; i < num; i++)
			{
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasFieldMarshal);
				uint num2 = this.ReadBlobIndex();
				if (metadataToken.RID != 0U)
				{
					dictionary.Add(metadataToken, num2);
				}
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00018685 File Offset: 0x00016885
		public bool HasMarshalInfo(IMarshalInfoProvider owner)
		{
			this.InitializeMarshalInfos();
			return this.metadata.FieldMarshals.ContainsKey(owner.MetadataToken);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000186A4 File Offset: 0x000168A4
		public MarshalInfo ReadMarshalInfo(IMarshalInfoProvider owner)
		{
			this.InitializeMarshalInfos();
			uint num;
			if (!this.metadata.FieldMarshals.TryGetValue(owner.MetadataToken, out num))
			{
				return null;
			}
			SignatureReader signatureReader = this.ReadSignature(num);
			this.metadata.FieldMarshals.Remove(owner.MetadataToken);
			return signatureReader.ReadMarshalInfo();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000186F6 File Offset: 0x000168F6
		private void InitializeSecurityDeclarations()
		{
			if (this.metadata.SecurityDeclarations != null)
			{
				return;
			}
			this.metadata.SecurityDeclarations = this.InitializeRanges(Table.DeclSecurity, delegate
			{
				base.ReadUInt16();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasDeclSecurity);
				this.ReadBlobIndex();
				return metadataToken;
			});
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00018728 File Offset: 0x00016928
		public bool HasSecurityDeclarations(ISecurityDeclarationProvider owner)
		{
			this.InitializeSecurityDeclarations();
			Range[] array;
			return this.metadata.TryGetSecurityDeclarationRanges(owner, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00018758 File Offset: 0x00016958
		public Collection<SecurityDeclaration> ReadSecurityDeclarations(ISecurityDeclarationProvider owner)
		{
			this.InitializeSecurityDeclarations();
			Range[] array;
			if (!this.metadata.TryGetSecurityDeclarationRanges(owner, out array))
			{
				return new Collection<SecurityDeclaration>();
			}
			Collection<SecurityDeclaration> collection = new Collection<SecurityDeclaration>(MetadataReader.RangesSize(array));
			for (int i = 0; i < array.Length; i++)
			{
				this.ReadSecurityDeclarationRange(array[i], collection);
			}
			this.metadata.RemoveSecurityDeclarationRange(owner);
			return collection;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000187B8 File Offset: 0x000169B8
		private void ReadSecurityDeclarationRange(Range range, Collection<SecurityDeclaration> security_declarations)
		{
			if (!this.MoveTo(Table.DeclSecurity, range.Start))
			{
				return;
			}
			int num = 0;
			while ((long)num < (long)((ulong)range.Length))
			{
				SecurityAction securityAction = (SecurityAction)base.ReadUInt16();
				this.ReadMetadataToken(CodedIndex.HasDeclSecurity);
				uint num2 = this.ReadBlobIndex();
				security_declarations.Add(new SecurityDeclaration(securityAction, num2, this.module));
				num++;
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00018599 File Offset: 0x00016799
		public byte[] ReadSecurityDeclarationBlob(uint signature)
		{
			return this.ReadBlob(signature);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00018814 File Offset: 0x00016A14
		public void ReadSecurityDeclarationSignature(SecurityDeclaration declaration)
		{
			uint signature = declaration.signature;
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.buffer[signatureReader.position] != 46)
			{
				this.ReadXmlSecurityDeclaration(signature, declaration);
				return;
			}
			signatureReader.position++;
			uint num = signatureReader.ReadCompressedUInt32();
			Collection<SecurityAttribute> collection = new Collection<SecurityAttribute>((int)num);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				collection.Add(signatureReader.ReadSecurityAttribute());
				num2++;
			}
			declaration.security_attributes = collection;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001888C File Offset: 0x00016A8C
		private void ReadXmlSecurityDeclaration(uint signature, SecurityDeclaration declaration)
		{
			declaration.security_attributes = new Collection<SecurityAttribute>(1)
			{
				new SecurityAttribute(this.module.TypeSystem.LookupType("System.Security.Permissions", "PermissionSetAttribute"))
				{
					properties = new Collection<CustomAttributeNamedArgument>(1),
					properties = 
					{
						new CustomAttributeNamedArgument("XML", new CustomAttributeArgument(this.module.TypeSystem.String, this.ReadUnicodeStringBlob(signature)))
					}
				}
			};
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001890C File Offset: 0x00016B0C
		public Collection<ExportedType> ReadExportedTypes()
		{
			int num = this.MoveTo(Table.ExportedType);
			if (num == 0)
			{
				return new Collection<ExportedType>();
			}
			Collection<ExportedType> collection = new Collection<ExportedType>(num);
			for (int i = 1; i <= num; i++)
			{
				TypeAttributes typeAttributes = (TypeAttributes)base.ReadUInt32();
				uint num2 = base.ReadUInt32();
				string text = this.ReadString();
				string text2 = this.ReadString();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.Implementation);
				ExportedType exportedType = null;
				IMetadataScope metadataScope = null;
				TokenType tokenType = metadataToken.TokenType;
				if (tokenType != TokenType.AssemblyRef && tokenType != TokenType.File)
				{
					if (tokenType == TokenType.ExportedType)
					{
						exportedType = collection[(int)(metadataToken.RID - 1U)];
					}
				}
				else
				{
					metadataScope = this.GetExportedTypeScope(metadataToken);
				}
				ExportedType exportedType2 = new ExportedType(text2, text, this.module, metadataScope)
				{
					Attributes = typeAttributes,
					Identifier = (int)num2,
					DeclaringType = exportedType
				};
				exportedType2.token = new MetadataToken(TokenType.ExportedType, i);
				collection.Add(exportedType2);
			}
			return collection;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x000189FC File Offset: 0x00016BFC
		private IMetadataScope GetExportedTypeScope(MetadataToken token)
		{
			int position = this.position;
			TokenType tokenType = token.TokenType;
			IMetadataScope metadataScope;
			if (tokenType != TokenType.AssemblyRef)
			{
				if (tokenType != TokenType.File)
				{
					throw new NotSupportedException();
				}
				this.InitializeModuleReferences();
				metadataScope = this.GetModuleReferenceFromFile(token);
			}
			else
			{
				this.InitializeAssemblyReferences();
				metadataScope = this.metadata.GetAssemblyNameReference(token.RID);
			}
			this.position = position;
			return metadataScope;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00018A64 File Offset: 0x00016C64
		private ModuleReference GetModuleReferenceFromFile(MetadataToken token)
		{
			if (!this.MoveTo(Table.File, token.RID))
			{
				return null;
			}
			base.ReadUInt32();
			string text = this.ReadString();
			Collection<ModuleReference> moduleReferences = this.module.ModuleReferences;
			ModuleReference moduleReference;
			for (int i = 0; i < moduleReferences.Count; i++)
			{
				moduleReference = moduleReferences[i];
				if (moduleReference.Name == text)
				{
					return moduleReference;
				}
			}
			moduleReference = new ModuleReference(text);
			moduleReferences.Add(moduleReference);
			return moduleReference;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00018AD8 File Offset: 0x00016CD8
		private void InitializeDocuments()
		{
			if (this.metadata.Documents != null)
			{
				return;
			}
			int num = this.MoveTo(Table.Document);
			Document[] array = (this.metadata.Documents = new Document[num]);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadBlobIndex();
				Guid guid = this.ReadGuid();
				byte[] array2 = this.ReadBlob();
				Guid guid2 = this.ReadGuid();
				string text = this.ReadSignature(num3).ReadDocumentName();
				array[(int)(num2 - 1U)] = new Document(text)
				{
					HashAlgorithmGuid = guid,
					Hash = array2,
					LanguageGuid = guid2,
					token = new MetadataToken(TokenType.Document, num2)
				};
				num2 += 1U;
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00018B84 File Offset: 0x00016D84
		public Collection<SequencePoint> ReadSequencePoints(MethodDefinition method)
		{
			this.InitializeDocuments();
			if (!this.MoveTo(Table.MethodDebugInformation, method.MetadataToken.RID))
			{
				return new Collection<SequencePoint>(0);
			}
			uint num = this.ReadTableIndex(Table.Document);
			uint num2 = this.ReadBlobIndex();
			if (num2 == 0U)
			{
				return new Collection<SequencePoint>(0);
			}
			Document document = this.GetDocument(num);
			return this.ReadSignature(num2).ReadSequencePoints(document);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00018BE8 File Offset: 0x00016DE8
		public Document GetDocument(uint rid)
		{
			Document document = this.metadata.GetDocument(rid);
			if (document == null)
			{
				return null;
			}
			document.custom_infos = this.GetCustomDebugInformation(document);
			return document;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00018C18 File Offset: 0x00016E18
		private void InitializeLocalScopes()
		{
			if (this.metadata.LocalScopes != null)
			{
				return;
			}
			this.InitializeMethods();
			int num = this.MoveTo(Table.LocalScope);
			this.metadata.LocalScopes = new Dictionary<uint, Collection<Row<uint, Range, Range, uint, uint, uint>>>();
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadTableIndex(Table.Method);
				uint num4 = this.ReadTableIndex(Table.ImportScope);
				Range range = this.ReadListRange(num2, Table.LocalScope, Table.LocalVariable);
				Range range2 = this.ReadListRange(num2, Table.LocalScope, Table.LocalConstant);
				uint num5 = base.ReadUInt32();
				uint num6 = base.ReadUInt32();
				this.metadata.SetLocalScopes(num3, MetadataReader.AddMapping<uint, Row<uint, Range, Range, uint, uint, uint>>(this.metadata.LocalScopes, num3, new Row<uint, Range, Range, uint, uint, uint>(num4, range, range2, num5, num6, num2)));
				num2 += 1U;
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00018CC8 File Offset: 0x00016EC8
		public ScopeDebugInformation ReadScope(MethodDefinition method)
		{
			this.InitializeLocalScopes();
			this.InitializeImportScopes();
			Collection<Row<uint, Range, Range, uint, uint, uint>> collection;
			if (!this.metadata.TryGetLocalScopes(method, out collection))
			{
				return null;
			}
			ScopeDebugInformation scopeDebugInformation = null;
			for (int i = 0; i < collection.Count; i++)
			{
				ScopeDebugInformation scopeDebugInformation2 = this.ReadLocalScope(collection[i]);
				if (i == 0)
				{
					scopeDebugInformation = scopeDebugInformation2;
				}
				else if (!MetadataReader.AddScope(scopeDebugInformation.scopes, scopeDebugInformation2))
				{
					scopeDebugInformation.Scopes.Add(scopeDebugInformation2);
				}
			}
			return scopeDebugInformation;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00018D38 File Offset: 0x00016F38
		private static bool AddScope(Collection<ScopeDebugInformation> scopes, ScopeDebugInformation scope)
		{
			if (scopes.IsNullOrEmpty<ScopeDebugInformation>())
			{
				return false;
			}
			foreach (ScopeDebugInformation scopeDebugInformation in scopes)
			{
				if (scopeDebugInformation.HasScopes && MetadataReader.AddScope(scopeDebugInformation.Scopes, scope))
				{
					return true;
				}
				if (scope.Start.Offset >= scopeDebugInformation.Start.Offset && scope.End.Offset <= scopeDebugInformation.End.Offset)
				{
					scopeDebugInformation.Scopes.Add(scope);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00018DF4 File Offset: 0x00016FF4
		private ScopeDebugInformation ReadLocalScope(Row<uint, Range, Range, uint, uint, uint> record)
		{
			ScopeDebugInformation scopeDebugInformation = new ScopeDebugInformation
			{
				start = new InstructionOffset((int)record.Col4),
				end = new InstructionOffset((int)(record.Col4 + record.Col5)),
				token = new MetadataToken(TokenType.LocalScope, record.Col6)
			};
			if (record.Col1 > 0U)
			{
				scopeDebugInformation.import = this.metadata.GetImportScope(record.Col1);
			}
			if (record.Col2.Length > 0U)
			{
				scopeDebugInformation.variables = new Collection<VariableDebugInformation>((int)record.Col2.Length);
				for (uint num = 0U; num < record.Col2.Length; num += 1U)
				{
					VariableDebugInformation variableDebugInformation = this.ReadLocalVariable(record.Col2.Start + num);
					if (variableDebugInformation != null)
					{
						scopeDebugInformation.variables.Add(variableDebugInformation);
					}
				}
			}
			if (record.Col3.Length > 0U)
			{
				scopeDebugInformation.constants = new Collection<ConstantDebugInformation>((int)record.Col3.Length);
				for (uint num2 = 0U; num2 < record.Col3.Length; num2 += 1U)
				{
					ConstantDebugInformation constantDebugInformation = this.ReadLocalConstant(record.Col3.Start + num2);
					if (constantDebugInformation != null)
					{
						scopeDebugInformation.constants.Add(constantDebugInformation);
					}
				}
			}
			return scopeDebugInformation;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00018F24 File Offset: 0x00017124
		private VariableDebugInformation ReadLocalVariable(uint rid)
		{
			if (!this.MoveTo(Table.LocalVariable, rid))
			{
				return null;
			}
			VariableAttributes variableAttributes = (VariableAttributes)base.ReadUInt16();
			int num = (int)base.ReadUInt16();
			string text = this.ReadString();
			VariableDebugInformation variableDebugInformation = new VariableDebugInformation(num, text)
			{
				Attributes = variableAttributes,
				token = new MetadataToken(TokenType.LocalVariable, rid)
			};
			variableDebugInformation.custom_infos = this.GetCustomDebugInformation(variableDebugInformation);
			return variableDebugInformation;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00018F80 File Offset: 0x00017180
		private ConstantDebugInformation ReadLocalConstant(uint rid)
		{
			if (!this.MoveTo(Table.LocalConstant, rid))
			{
				return null;
			}
			string text = this.ReadString();
			SignatureReader signatureReader = this.ReadSignature(this.ReadBlobIndex());
			TypeReference typeReference = signatureReader.ReadTypeSignature();
			object obj;
			if (typeReference.etype == ElementType.String)
			{
				if (signatureReader.buffer[signatureReader.position] != 255)
				{
					byte[] array = signatureReader.ReadBytes((int)((ulong)signatureReader.sig_length - (ulong)((long)signatureReader.position - (long)((ulong)signatureReader.start))));
					obj = Encoding.Unicode.GetString(array, 0, array.Length);
				}
				else
				{
					obj = null;
				}
			}
			else if (typeReference.IsTypeOf("System", "Decimal"))
			{
				byte b = signatureReader.ReadByte();
				obj = new decimal(signatureReader.ReadInt32(), signatureReader.ReadInt32(), signatureReader.ReadInt32(), (b & 128) > 0, b & 127);
			}
			else if (typeReference.IsTypeOf("System", "DateTime"))
			{
				obj = new DateTime(signatureReader.ReadInt64());
			}
			else if (typeReference.etype == ElementType.Object || typeReference.etype == ElementType.None || typeReference.etype == ElementType.Class || typeReference.etype == ElementType.Array)
			{
				obj = null;
			}
			else
			{
				obj = signatureReader.ReadConstantSignature(typeReference.etype);
			}
			ConstantDebugInformation constantDebugInformation = new ConstantDebugInformation(text, typeReference, obj)
			{
				token = new MetadataToken(TokenType.LocalConstant, rid)
			};
			constantDebugInformation.custom_infos = this.GetCustomDebugInformation(constantDebugInformation);
			return constantDebugInformation;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x000190E0 File Offset: 0x000172E0
		private void InitializeImportScopes()
		{
			if (this.metadata.ImportScopes != null)
			{
				return;
			}
			int num = this.MoveTo(Table.ImportScope);
			this.metadata.ImportScopes = new ImportDebugInformation[num];
			for (int i = 1; i <= num; i++)
			{
				this.ReadTableIndex(Table.ImportScope);
				ImportDebugInformation importDebugInformation = new ImportDebugInformation();
				importDebugInformation.token = new MetadataToken(TokenType.ImportScope, i);
				SignatureReader signatureReader = this.ReadSignature(this.ReadBlobIndex());
				while (signatureReader.CanReadMore())
				{
					importDebugInformation.Targets.Add(this.ReadImportTarget(signatureReader));
				}
				this.metadata.ImportScopes[i - 1] = importDebugInformation;
			}
			this.MoveTo(Table.ImportScope);
			for (int j = 0; j < num; j++)
			{
				uint num2 = this.ReadTableIndex(Table.ImportScope);
				this.ReadBlobIndex();
				if (num2 != 0U)
				{
					this.metadata.ImportScopes[j].Parent = this.metadata.GetImportScope(num2);
				}
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000191C8 File Offset: 0x000173C8
		public string ReadUTF8StringBlob(uint signature)
		{
			return this.ReadStringBlob(signature, Encoding.UTF8);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x000191D6 File Offset: 0x000173D6
		private string ReadUnicodeStringBlob(uint signature)
		{
			return this.ReadStringBlob(signature, Encoding.Unicode);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x000191E4 File Offset: 0x000173E4
		private string ReadStringBlob(uint signature, Encoding encoding)
		{
			byte[] array;
			int num;
			int num2;
			this.GetBlobView(signature, out array, out num, out num2);
			if (num2 == 0)
			{
				return string.Empty;
			}
			return encoding.GetString(array, num, num2);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00019210 File Offset: 0x00017410
		private ImportTarget ReadImportTarget(SignatureReader signature)
		{
			AssemblyNameReference assemblyNameReference = null;
			string text = null;
			string text2 = null;
			TypeReference typeReference = null;
			ImportTargetKind importTargetKind = (ImportTargetKind)signature.ReadCompressedUInt32();
			switch (importTargetKind)
			{
			case ImportTargetKind.ImportNamespace:
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.ImportNamespaceInAssembly:
				assemblyNameReference = this.metadata.GetAssemblyNameReference(signature.ReadCompressedUInt32());
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.ImportType:
				typeReference = signature.ReadTypeToken();
				break;
			case ImportTargetKind.ImportXmlNamespaceWithAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.ImportAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.DefineAssemblyAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				assemblyNameReference = this.metadata.GetAssemblyNameReference(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.DefineNamespaceAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.DefineNamespaceInAssemblyAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				assemblyNameReference = this.metadata.GetAssemblyNameReference(signature.ReadCompressedUInt32());
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.DefineTypeAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				typeReference = signature.ReadTypeToken();
				break;
			}
			return new ImportTarget(importTargetKind)
			{
				alias = text2,
				type = typeReference,
				@namespace = text,
				reference = assemblyNameReference
			};
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00019374 File Offset: 0x00017574
		private void InitializeStateMachineMethods()
		{
			if (this.metadata.StateMachineMethods != null)
			{
				return;
			}
			int num = this.MoveTo(Table.StateMachineMethod);
			this.metadata.StateMachineMethods = new Dictionary<uint, uint>(num);
			for (int i = 0; i < num; i++)
			{
				this.metadata.StateMachineMethods.Add(this.ReadTableIndex(Table.Method), this.ReadTableIndex(Table.Method));
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000193D4 File Offset: 0x000175D4
		public MethodDefinition ReadStateMachineKickoffMethod(MethodDefinition method)
		{
			this.InitializeStateMachineMethods();
			uint num;
			if (!this.metadata.TryGetStateMachineKickOffMethod(method, out num))
			{
				return null;
			}
			return this.GetMethodDefinition(num);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00019400 File Offset: 0x00017600
		private void InitializeCustomDebugInformations()
		{
			if (this.metadata.CustomDebugInformations != null)
			{
				return;
			}
			int num = this.MoveTo(Table.CustomDebugInformation);
			this.metadata.CustomDebugInformations = new Dictionary<MetadataToken, Row<Guid, uint, uint>[]>();
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasCustomDebugInformation);
				Row<Guid, uint, uint> row = new Row<Guid, uint, uint>(this.ReadGuid(), this.ReadBlobIndex(), num2);
				Row<Guid, uint, uint>[] array;
				this.metadata.CustomDebugInformations.TryGetValue(metadataToken, out array);
				this.metadata.CustomDebugInformations[metadataToken] = array.Add(row);
				num2 += 1U;
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001948C File Offset: 0x0001768C
		public Collection<CustomDebugInformation> GetCustomDebugInformation(ICustomDebugInformationProvider provider)
		{
			this.InitializeCustomDebugInformations();
			Row<Guid, uint, uint>[] array;
			if (!this.metadata.CustomDebugInformations.TryGetValue(provider.MetadataToken, out array))
			{
				return null;
			}
			Collection<CustomDebugInformation> collection = new Collection<CustomDebugInformation>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Col1 == StateMachineScopeDebugInformation.KindIdentifier)
				{
					SignatureReader signatureReader = this.ReadSignature(array[i].Col2);
					Collection<StateMachineScope> collection2 = new Collection<StateMachineScope>();
					while (signatureReader.CanReadMore())
					{
						int num = signatureReader.ReadInt32();
						int num2 = num + signatureReader.ReadInt32();
						collection2.Add(new StateMachineScope(num, num2));
					}
					collection.Add(new StateMachineScopeDebugInformation
					{
						scopes = collection2
					});
				}
				else if (array[i].Col1 == AsyncMethodBodyDebugInformation.KindIdentifier)
				{
					SignatureReader signatureReader2 = this.ReadSignature(array[i].Col2);
					int num3 = signatureReader2.ReadInt32() - 1;
					Collection<InstructionOffset> collection3 = new Collection<InstructionOffset>();
					Collection<InstructionOffset> collection4 = new Collection<InstructionOffset>();
					Collection<MethodDefinition> collection5 = new Collection<MethodDefinition>();
					while (signatureReader2.CanReadMore())
					{
						collection3.Add(new InstructionOffset(signatureReader2.ReadInt32()));
						collection4.Add(new InstructionOffset(signatureReader2.ReadInt32()));
						collection5.Add(this.GetMethodDefinition(signatureReader2.ReadCompressedUInt32()));
					}
					collection.Add(new AsyncMethodBodyDebugInformation(num3)
					{
						yields = collection3,
						resumes = collection4,
						resume_methods = collection5
					});
				}
				else if (array[i].Col1 == EmbeddedSourceDebugInformation.KindIdentifier)
				{
					SignatureReader signatureReader3 = this.ReadSignature(array[i].Col2);
					int num4 = signatureReader3.ReadInt32();
					uint num5 = signatureReader3.sig_length - 4U;
					CustomDebugInformation customDebugInformation = null;
					if (num4 == 0)
					{
						customDebugInformation = new EmbeddedSourceDebugInformation(signatureReader3.ReadBytes((int)num5), false);
					}
					else if (num4 > 0)
					{
						Stream stream = new MemoryStream(signatureReader3.ReadBytes((int)num5));
						byte[] array2 = new byte[num4];
						MemoryStream memoryStream = new MemoryStream(array2);
						using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress, true))
						{
							deflateStream.CopyTo(memoryStream);
						}
						customDebugInformation = new EmbeddedSourceDebugInformation(array2, true);
					}
					else if (num4 < 0)
					{
						customDebugInformation = new BinaryCustomDebugInformation(array[i].Col1, this.ReadBlob(array[i].Col2));
					}
					collection.Add(customDebugInformation);
				}
				else if (array[i].Col1 == SourceLinkDebugInformation.KindIdentifier)
				{
					collection.Add(new SourceLinkDebugInformation(Encoding.UTF8.GetString(this.ReadBlob(array[i].Col2))));
				}
				else
				{
					collection.Add(new BinaryCustomDebugInformation(array[i].Col1, this.ReadBlob(array[i].Col2)));
				}
				collection[i].token = new MetadataToken(TokenType.CustomDebugInformation, array[i].Col3);
			}
			return collection;
		}

		// Token: 0x04000252 RID: 594
		internal readonly Image image;

		// Token: 0x04000253 RID: 595
		internal readonly ModuleDefinition module;

		// Token: 0x04000254 RID: 596
		internal readonly MetadataSystem metadata;

		// Token: 0x04000255 RID: 597
		internal CodeReader code;

		// Token: 0x04000256 RID: 598
		internal IGenericContext context;

		// Token: 0x04000257 RID: 599
		private readonly MetadataReader metadata_reader;
	}
}

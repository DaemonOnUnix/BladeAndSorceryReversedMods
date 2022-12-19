using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil.PE
{
	// Token: 0x02000197 RID: 407
	internal sealed class ImageReader : BinaryStreamReader
	{
		// Token: 0x06000D19 RID: 3353 RVA: 0x0002C3B0 File Offset: 0x0002A5B0
		public ImageReader(Disposable<Stream> stream, string file_name)
			: base(stream.value)
		{
			this.image = new Image();
			this.image.Stream = stream;
			this.image.FileName = file_name;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0002C3E1 File Offset: 0x0002A5E1
		private void MoveTo(DataDirectory directory)
		{
			this.BaseStream.Position = (long)((ulong)this.image.ResolveVirtualAddress(directory.VirtualAddress));
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0002C400 File Offset: 0x0002A600
		private void ReadImage()
		{
			if (this.BaseStream.Length < 128L)
			{
				throw new BadImageFormatException();
			}
			if (this.ReadUInt16() != 23117)
			{
				throw new BadImageFormatException();
			}
			base.Advance(58);
			base.MoveTo(this.ReadUInt32());
			if (this.ReadUInt32() != 17744U)
			{
				throw new BadImageFormatException();
			}
			this.image.Architecture = this.ReadArchitecture();
			ushort num = this.ReadUInt16();
			this.image.Timestamp = this.ReadUInt32();
			base.Advance(10);
			ushort num2 = this.ReadUInt16();
			ushort num3;
			ushort num4;
			this.ReadOptionalHeaders(out num3, out num4);
			this.ReadSections(num);
			this.ReadCLIHeader();
			this.ReadMetadata();
			this.ReadDebugHeader();
			this.image.Kind = ImageReader.GetModuleKind(num2, num3);
			this.image.Characteristics = (ModuleCharacteristics)num4;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0002C4D9 File Offset: 0x0002A6D9
		private TargetArchitecture ReadArchitecture()
		{
			return (TargetArchitecture)this.ReadUInt16();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0002C4E1 File Offset: 0x0002A6E1
		private static ModuleKind GetModuleKind(ushort characteristics, ushort subsystem)
		{
			if ((characteristics & 8192) != 0)
			{
				return ModuleKind.Dll;
			}
			if (subsystem == 2 || subsystem == 9)
			{
				return ModuleKind.Windows;
			}
			return ModuleKind.Console;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0002C4FC File Offset: 0x0002A6FC
		private void ReadOptionalHeaders(out ushort subsystem, out ushort dll_characteristics)
		{
			bool flag = this.ReadUInt16() == 523;
			this.image.LinkerVersion = this.ReadUInt16();
			base.Advance(44);
			this.image.SubSystemMajor = this.ReadUInt16();
			this.image.SubSystemMinor = this.ReadUInt16();
			base.Advance(16);
			subsystem = this.ReadUInt16();
			dll_characteristics = this.ReadUInt16();
			base.Advance(flag ? 56 : 40);
			this.image.Win32Resources = base.ReadDataDirectory();
			base.Advance(24);
			this.image.Debug = base.ReadDataDirectory();
			base.Advance(56);
			this.cli = base.ReadDataDirectory();
			if (this.cli.IsZero)
			{
				throw new BadImageFormatException();
			}
			base.Advance(8);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0002C5D4 File Offset: 0x0002A7D4
		private string ReadAlignedString(int length)
		{
			int i = 0;
			char[] array = new char[length];
			while (i < length)
			{
				byte b = this.ReadByte();
				if (b == 0)
				{
					break;
				}
				array[i++] = (char)b;
			}
			base.Advance(-1 + ((i + 4) & -4) - i);
			return new string(array, 0, i);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0002C61C File Offset: 0x0002A81C
		private string ReadZeroTerminatedString(int length)
		{
			int i = 0;
			char[] array = new char[length];
			byte[] array2 = this.ReadBytes(length);
			while (i < length)
			{
				byte b = array2[i];
				if (b == 0)
				{
					break;
				}
				array[i++] = (char)b;
			}
			return new string(array, 0, i);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0002C658 File Offset: 0x0002A858
		private void ReadSections(ushort count)
		{
			Section[] array = new Section[(int)count];
			for (int i = 0; i < (int)count; i++)
			{
				Section section = new Section();
				section.Name = this.ReadZeroTerminatedString(8);
				base.Advance(4);
				section.VirtualAddress = this.ReadUInt32();
				section.SizeOfRawData = this.ReadUInt32();
				section.PointerToRawData = this.ReadUInt32();
				base.Advance(16);
				array[i] = section;
			}
			this.image.Sections = array;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0002C6D0 File Offset: 0x0002A8D0
		private void ReadCLIHeader()
		{
			this.MoveTo(this.cli);
			base.Advance(8);
			this.metadata = base.ReadDataDirectory();
			this.image.Attributes = (ModuleAttributes)this.ReadUInt32();
			this.image.EntryPointToken = this.ReadUInt32();
			this.image.Resources = base.ReadDataDirectory();
			this.image.StrongName = base.ReadDataDirectory();
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0002C740 File Offset: 0x0002A940
		private void ReadMetadata()
		{
			this.MoveTo(this.metadata);
			if (this.ReadUInt32() != 1112167234U)
			{
				throw new BadImageFormatException();
			}
			base.Advance(8);
			this.image.RuntimeVersion = this.ReadZeroTerminatedString(this.ReadInt32());
			base.Advance(2);
			ushort num = this.ReadUInt16();
			Section sectionAtVirtualAddress = this.image.GetSectionAtVirtualAddress(this.metadata.VirtualAddress);
			if (sectionAtVirtualAddress == null)
			{
				throw new BadImageFormatException();
			}
			this.image.MetadataSection = sectionAtVirtualAddress;
			for (int i = 0; i < (int)num; i++)
			{
				this.ReadMetadataStream(sectionAtVirtualAddress);
			}
			if (this.image.PdbHeap != null)
			{
				this.ReadPdbHeap();
			}
			if (this.image.TableHeap != null)
			{
				this.ReadTableHeap();
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0002C800 File Offset: 0x0002AA00
		private void ReadDebugHeader()
		{
			if (this.image.Debug.IsZero)
			{
				this.image.DebugHeader = new ImageDebugHeader(Empty<ImageDebugHeaderEntry>.Array);
				return;
			}
			this.MoveTo(this.image.Debug);
			ImageDebugHeaderEntry[] array = new ImageDebugHeaderEntry[this.image.Debug.Size / 28U];
			for (int i = 0; i < array.Length; i++)
			{
				ImageDebugDirectory imageDebugDirectory = new ImageDebugDirectory
				{
					Characteristics = this.ReadInt32(),
					TimeDateStamp = this.ReadInt32(),
					MajorVersion = this.ReadInt16(),
					MinorVersion = this.ReadInt16(),
					Type = (ImageDebugType)this.ReadInt32(),
					SizeOfData = this.ReadInt32(),
					AddressOfRawData = this.ReadInt32(),
					PointerToRawData = this.ReadInt32()
				};
				if (imageDebugDirectory.PointerToRawData == 0 || imageDebugDirectory.SizeOfData < 0)
				{
					array[i] = new ImageDebugHeaderEntry(imageDebugDirectory, Empty<byte>.Array);
				}
				else
				{
					int position = base.Position;
					try
					{
						base.MoveTo((uint)imageDebugDirectory.PointerToRawData);
						byte[] array2 = this.ReadBytes(imageDebugDirectory.SizeOfData);
						array[i] = new ImageDebugHeaderEntry(imageDebugDirectory, array2);
					}
					finally
					{
						base.Position = position;
					}
				}
			}
			this.image.DebugHeader = new ImageDebugHeader(array);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0002C960 File Offset: 0x0002AB60
		private void ReadMetadataStream(Section section)
		{
			uint num = this.metadata.VirtualAddress - section.VirtualAddress + this.ReadUInt32();
			uint num2 = this.ReadUInt32();
			byte[] array = this.ReadHeapData(num, num2);
			string text = this.ReadAlignedString(16);
			if (text != null)
			{
				uint num3 = <5c122140-1889-445a-a95b-56bc787f60ce><PrivateImplementationDetails>.ComputeStringHash(text);
				if (num3 <= 617129517U)
				{
					if (num3 != 368124450U)
					{
						if (num3 != 491825896U)
						{
							if (num3 != 617129517U)
							{
								return;
							}
							if (!(text == "#-"))
							{
								return;
							}
						}
						else
						{
							if (!(text == "#Strings"))
							{
								return;
							}
							this.image.StringHeap = new StringHeap(array);
							return;
						}
					}
					else
					{
						if (!(text == "#US"))
						{
							return;
						}
						this.image.UserStringHeap = new UserStringHeap(array);
						return;
					}
				}
				else if (num3 <= 1422005491U)
				{
					if (num3 != 1372122372U)
					{
						if (num3 != 1422005491U)
						{
							return;
						}
						if (!(text == "#GUID"))
						{
							return;
						}
						this.image.GuidHeap = new GuidHeap(array);
						return;
					}
					else if (!(text == "#~"))
					{
						return;
					}
				}
				else if (num3 != 1638201209U)
				{
					if (num3 != 2979271308U)
					{
						return;
					}
					if (!(text == "#Pdb"))
					{
						return;
					}
					this.image.PdbHeap = new PdbHeap(array);
					return;
				}
				else
				{
					if (!(text == "#Blob"))
					{
						return;
					}
					this.image.BlobHeap = new BlobHeap(array);
					return;
				}
				this.image.TableHeap = new TableHeap(array);
				this.table_heap_offset = num;
				return;
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0002CADC File Offset: 0x0002ACDC
		private byte[] ReadHeapData(uint offset, uint size)
		{
			long position = this.BaseStream.Position;
			base.MoveTo(offset + this.image.MetadataSection.PointerToRawData);
			byte[] array = this.ReadBytes((int)size);
			this.BaseStream.Position = position;
			return array;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0002CB20 File Offset: 0x0002AD20
		private void ReadTableHeap()
		{
			TableHeap tableHeap = this.image.TableHeap;
			base.MoveTo(this.table_heap_offset + this.image.MetadataSection.PointerToRawData);
			base.Advance(6);
			byte b = this.ReadByte();
			base.Advance(1);
			tableHeap.Valid = this.ReadInt64();
			tableHeap.Sorted = this.ReadInt64();
			if (this.image.PdbHeap != null)
			{
				for (int i = 0; i < 58; i++)
				{
					if (this.image.PdbHeap.HasTable((Table)i))
					{
						tableHeap.Tables[i].Length = this.image.PdbHeap.TypeSystemTableRows[i];
					}
				}
			}
			for (int j = 0; j < 58; j++)
			{
				if (tableHeap.HasTable((Table)j))
				{
					tableHeap.Tables[j].Length = this.ReadUInt32();
				}
			}
			ImageReader.SetIndexSize(this.image.StringHeap, (uint)b, 1);
			ImageReader.SetIndexSize(this.image.GuidHeap, (uint)b, 2);
			ImageReader.SetIndexSize(this.image.BlobHeap, (uint)b, 4);
			this.ComputeTableInformations();
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0002CC3E File Offset: 0x0002AE3E
		private static void SetIndexSize(Heap heap, uint sizes, byte flag)
		{
			if (heap == null)
			{
				return;
			}
			heap.IndexSize = (((sizes & (uint)flag) > 0U) ? 4 : 2);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0002CC54 File Offset: 0x0002AE54
		private int GetTableIndexSize(Table table)
		{
			return this.image.GetTableIndexSize(table);
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0002CC62 File Offset: 0x0002AE62
		private int GetCodedIndexSize(CodedIndex index)
		{
			return this.image.GetCodedIndexSize(index);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0002CC70 File Offset: 0x0002AE70
		private void ComputeTableInformations()
		{
			uint num = (uint)this.BaseStream.Position - this.table_heap_offset - this.image.MetadataSection.PointerToRawData;
			int indexSize = this.image.StringHeap.IndexSize;
			int num2 = ((this.image.GuidHeap != null) ? this.image.GuidHeap.IndexSize : 2);
			int num3 = ((this.image.BlobHeap != null) ? this.image.BlobHeap.IndexSize : 2);
			TableHeap tableHeap = this.image.TableHeap;
			TableInformation[] tables = tableHeap.Tables;
			for (int i = 0; i < 58; i++)
			{
				Table table = (Table)i;
				if (tableHeap.HasTable(table))
				{
					int num4;
					switch (table)
					{
					case Table.Module:
						num4 = 2 + indexSize + num2 * 3;
						break;
					case Table.TypeRef:
						num4 = this.GetCodedIndexSize(CodedIndex.ResolutionScope) + indexSize * 2;
						break;
					case Table.TypeDef:
						num4 = 4 + indexSize * 2 + this.GetCodedIndexSize(CodedIndex.TypeDefOrRef) + this.GetTableIndexSize(Table.Field) + this.GetTableIndexSize(Table.Method);
						break;
					case Table.FieldPtr:
						num4 = this.GetTableIndexSize(Table.Field);
						break;
					case Table.Field:
						num4 = 2 + indexSize + num3;
						break;
					case Table.MethodPtr:
						num4 = this.GetTableIndexSize(Table.Method);
						break;
					case Table.Method:
						num4 = 8 + indexSize + num3 + this.GetTableIndexSize(Table.Param);
						break;
					case Table.ParamPtr:
						num4 = this.GetTableIndexSize(Table.Param);
						break;
					case Table.Param:
						num4 = 4 + indexSize;
						break;
					case Table.InterfaceImpl:
						num4 = this.GetTableIndexSize(Table.TypeDef) + this.GetCodedIndexSize(CodedIndex.TypeDefOrRef);
						break;
					case Table.MemberRef:
						num4 = this.GetCodedIndexSize(CodedIndex.MemberRefParent) + indexSize + num3;
						break;
					case Table.Constant:
						num4 = 2 + this.GetCodedIndexSize(CodedIndex.HasConstant) + num3;
						break;
					case Table.CustomAttribute:
						num4 = this.GetCodedIndexSize(CodedIndex.HasCustomAttribute) + this.GetCodedIndexSize(CodedIndex.CustomAttributeType) + num3;
						break;
					case Table.FieldMarshal:
						num4 = this.GetCodedIndexSize(CodedIndex.HasFieldMarshal) + num3;
						break;
					case Table.DeclSecurity:
						num4 = 2 + this.GetCodedIndexSize(CodedIndex.HasDeclSecurity) + num3;
						break;
					case Table.ClassLayout:
						num4 = 6 + this.GetTableIndexSize(Table.TypeDef);
						break;
					case Table.FieldLayout:
						num4 = 4 + this.GetTableIndexSize(Table.Field);
						break;
					case Table.StandAloneSig:
						num4 = num3;
						break;
					case Table.EventMap:
						num4 = this.GetTableIndexSize(Table.TypeDef) + this.GetTableIndexSize(Table.Event);
						break;
					case Table.EventPtr:
						num4 = this.GetTableIndexSize(Table.Event);
						break;
					case Table.Event:
						num4 = 2 + indexSize + this.GetCodedIndexSize(CodedIndex.TypeDefOrRef);
						break;
					case Table.PropertyMap:
						num4 = this.GetTableIndexSize(Table.TypeDef) + this.GetTableIndexSize(Table.Property);
						break;
					case Table.PropertyPtr:
						num4 = this.GetTableIndexSize(Table.Property);
						break;
					case Table.Property:
						num4 = 2 + indexSize + num3;
						break;
					case Table.MethodSemantics:
						num4 = 2 + this.GetTableIndexSize(Table.Method) + this.GetCodedIndexSize(CodedIndex.HasSemantics);
						break;
					case Table.MethodImpl:
						num4 = this.GetTableIndexSize(Table.TypeDef) + this.GetCodedIndexSize(CodedIndex.MethodDefOrRef) + this.GetCodedIndexSize(CodedIndex.MethodDefOrRef);
						break;
					case Table.ModuleRef:
						num4 = indexSize;
						break;
					case Table.TypeSpec:
						num4 = num3;
						break;
					case Table.ImplMap:
						num4 = 2 + this.GetCodedIndexSize(CodedIndex.MemberForwarded) + indexSize + this.GetTableIndexSize(Table.ModuleRef);
						break;
					case Table.FieldRVA:
						num4 = 4 + this.GetTableIndexSize(Table.Field);
						break;
					case Table.EncLog:
						num4 = 8;
						break;
					case Table.EncMap:
						num4 = 4;
						break;
					case Table.Assembly:
						num4 = 16 + num3 + indexSize * 2;
						break;
					case Table.AssemblyProcessor:
						num4 = 4;
						break;
					case Table.AssemblyOS:
						num4 = 12;
						break;
					case Table.AssemblyRef:
						num4 = 12 + num3 * 2 + indexSize * 2;
						break;
					case Table.AssemblyRefProcessor:
						num4 = 4 + this.GetTableIndexSize(Table.AssemblyRef);
						break;
					case Table.AssemblyRefOS:
						num4 = 12 + this.GetTableIndexSize(Table.AssemblyRef);
						break;
					case Table.File:
						num4 = 4 + indexSize + num3;
						break;
					case Table.ExportedType:
						num4 = 8 + indexSize * 2 + this.GetCodedIndexSize(CodedIndex.Implementation);
						break;
					case Table.ManifestResource:
						num4 = 8 + indexSize + this.GetCodedIndexSize(CodedIndex.Implementation);
						break;
					case Table.NestedClass:
						num4 = this.GetTableIndexSize(Table.TypeDef) + this.GetTableIndexSize(Table.TypeDef);
						break;
					case Table.GenericParam:
						num4 = 4 + this.GetCodedIndexSize(CodedIndex.TypeOrMethodDef) + indexSize;
						break;
					case Table.MethodSpec:
						num4 = this.GetCodedIndexSize(CodedIndex.MethodDefOrRef) + num3;
						break;
					case Table.GenericParamConstraint:
						num4 = this.GetTableIndexSize(Table.GenericParam) + this.GetCodedIndexSize(CodedIndex.TypeDefOrRef);
						break;
					case (Table)45:
					case (Table)46:
					case (Table)47:
						goto IL_50E;
					case Table.Document:
						num4 = num3 + num2 + num3 + num2;
						break;
					case Table.MethodDebugInformation:
						num4 = this.GetTableIndexSize(Table.Document) + num3;
						break;
					case Table.LocalScope:
						num4 = this.GetTableIndexSize(Table.Method) + this.GetTableIndexSize(Table.ImportScope) + this.GetTableIndexSize(Table.LocalVariable) + this.GetTableIndexSize(Table.LocalConstant) + 8;
						break;
					case Table.LocalVariable:
						num4 = 4 + indexSize;
						break;
					case Table.LocalConstant:
						num4 = indexSize + num3;
						break;
					case Table.ImportScope:
						num4 = this.GetTableIndexSize(Table.ImportScope) + num3;
						break;
					case Table.StateMachineMethod:
						num4 = this.GetTableIndexSize(Table.Method) + this.GetTableIndexSize(Table.Method);
						break;
					case Table.CustomDebugInformation:
						num4 = this.GetCodedIndexSize(CodedIndex.HasCustomDebugInformation) + num2 + num3;
						break;
					default:
						goto IL_50E;
					}
					tables[i].RowSize = (uint)num4;
					tables[i].Offset = num;
					num += (uint)(num4 * (int)tables[i].Length);
					goto IL_547;
					IL_50E:
					throw new NotSupportedException();
				}
				IL_547:;
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0002D1D4 File Offset: 0x0002B3D4
		private void ReadPdbHeap()
		{
			PdbHeap pdbHeap = this.image.PdbHeap;
			ByteBuffer byteBuffer = new ByteBuffer(pdbHeap.data);
			pdbHeap.Id = byteBuffer.ReadBytes(20);
			pdbHeap.EntryPoint = byteBuffer.ReadUInt32();
			pdbHeap.TypeSystemTables = byteBuffer.ReadInt64();
			pdbHeap.TypeSystemTableRows = new uint[58];
			for (int i = 0; i < 58; i++)
			{
				Table table = (Table)i;
				if (pdbHeap.HasTable(table))
				{
					pdbHeap.TypeSystemTableRows[i] = byteBuffer.ReadUInt32();
				}
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0002D254 File Offset: 0x0002B454
		public static Image ReadImage(Disposable<Stream> stream, string file_name)
		{
			Image image;
			try
			{
				ImageReader imageReader = new ImageReader(stream, file_name);
				imageReader.ReadImage();
				image = imageReader.image;
			}
			catch (EndOfStreamException ex)
			{
				throw new BadImageFormatException(stream.value.GetFileName(), ex);
			}
			return image;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0002D29C File Offset: 0x0002B49C
		public static Image ReadPortablePdb(Disposable<Stream> stream, string file_name)
		{
			Image image;
			try
			{
				ImageReader imageReader = new ImageReader(stream, file_name);
				uint num = (uint)stream.value.Length;
				imageReader.image.Sections = new Section[]
				{
					new Section
					{
						PointerToRawData = 0U,
						SizeOfRawData = num,
						VirtualAddress = 0U,
						VirtualSize = num
					}
				};
				imageReader.metadata = new DataDirectory(0U, num);
				imageReader.ReadMetadata();
				image = imageReader.image;
			}
			catch (EndOfStreamException ex)
			{
				throw new BadImageFormatException(stream.value.GetFileName(), ex);
			}
			return image;
		}

		// Token: 0x040005CC RID: 1484
		private readonly Image image;

		// Token: 0x040005CD RID: 1485
		private DataDirectory cli;

		// Token: 0x040005CE RID: 1486
		private DataDirectory metadata;

		// Token: 0x040005CF RID: 1487
		private uint table_heap_offset;
	}
}

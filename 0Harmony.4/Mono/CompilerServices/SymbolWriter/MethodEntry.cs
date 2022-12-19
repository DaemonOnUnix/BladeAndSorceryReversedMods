using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200030E RID: 782
	public class MethodEntry : IComparable
	{
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x0003F6A8 File Offset: 0x0003D8A8
		public MethodEntry.Flags MethodFlags
		{
			get
			{
				return this.flags;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x0003F6B0 File Offset: 0x0003D8B0
		// (set) Token: 0x06001397 RID: 5015 RVA: 0x0003F6B8 File Offset: 0x0003D8B8
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0003F6C4 File Offset: 0x0003D8C4
		internal MethodEntry(MonoSymbolFile file, MyBinaryReader reader, int index)
		{
			this.SymbolFile = file;
			this.index = index;
			this.Token = reader.ReadInt32();
			this.DataOffset = reader.ReadInt32();
			this.LineNumberTableOffset = reader.ReadInt32();
			long position = reader.BaseStream.Position;
			reader.BaseStream.Position = (long)this.DataOffset;
			this.CompileUnitIndex = reader.ReadLeb128();
			this.LocalVariableTableOffset = reader.ReadLeb128();
			this.NamespaceID = reader.ReadLeb128();
			this.CodeBlockTableOffset = reader.ReadLeb128();
			this.ScopeVariableTableOffset = reader.ReadLeb128();
			this.RealNameOffset = reader.ReadLeb128();
			this.flags = (MethodEntry.Flags)reader.ReadLeb128();
			reader.BaseStream.Position = position;
			this.CompileUnit = file.GetCompileUnit(this.CompileUnitIndex);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0003F79C File Offset: 0x0003D99C
		internal MethodEntry(MonoSymbolFile file, CompileUnitEntry comp_unit, int token, ScopeVariable[] scope_vars, LocalVariableEntry[] locals, LineNumberEntry[] lines, CodeBlockEntry[] code_blocks, string real_name, MethodEntry.Flags flags, int namespace_id)
		{
			this.SymbolFile = file;
			this.real_name = real_name;
			this.locals = locals;
			this.code_blocks = code_blocks;
			this.scope_vars = scope_vars;
			this.flags = flags;
			this.index = -1;
			this.Token = token;
			this.CompileUnitIndex = comp_unit.Index;
			this.CompileUnit = comp_unit;
			this.NamespaceID = namespace_id;
			MethodEntry.CheckLineNumberTable(lines);
			this.lnt = new LineNumberTable(file, lines);
			file.NumLineNumbers += lines.Length;
			int num = ((locals != null) ? locals.Length : 0);
			if (num <= 32)
			{
				for (int i = 0; i < num; i++)
				{
					string name = locals[i].Name;
					for (int j = i + 1; j < num; j++)
					{
						if (locals[j].Name == name)
						{
							flags |= MethodEntry.Flags.LocalNamesAmbiguous;
							return;
						}
					}
				}
				return;
			}
			Dictionary<string, LocalVariableEntry> dictionary = new Dictionary<string, LocalVariableEntry>();
			foreach (LocalVariableEntry localVariableEntry in locals)
			{
				if (dictionary.ContainsKey(localVariableEntry.Name))
				{
					flags |= MethodEntry.Flags.LocalNamesAmbiguous;
					return;
				}
				dictionary.Add(localVariableEntry.Name, localVariableEntry);
			}
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0003F8D4 File Offset: 0x0003DAD4
		private static void CheckLineNumberTable(LineNumberEntry[] line_numbers)
		{
			int num = -1;
			int num2 = -1;
			if (line_numbers == null)
			{
				return;
			}
			foreach (LineNumberEntry lineNumberEntry in line_numbers)
			{
				if (lineNumberEntry.Equals(LineNumberEntry.Null))
				{
					throw new MonoSymbolFileException();
				}
				if (lineNumberEntry.Offset < num)
				{
					throw new MonoSymbolFileException();
				}
				if (lineNumberEntry.Offset > num)
				{
					num2 = lineNumberEntry.Row;
					num = lineNumberEntry.Offset;
				}
				else if (lineNumberEntry.Row > num2)
				{
					num2 = lineNumberEntry.Row;
				}
			}
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0003F946 File Offset: 0x0003DB46
		internal void Write(MyBinaryWriter bw)
		{
			if (this.index <= 0 || this.DataOffset == 0)
			{
				throw new InvalidOperationException();
			}
			bw.Write(this.Token);
			bw.Write(this.DataOffset);
			bw.Write(this.LineNumberTableOffset);
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0003F984 File Offset: 0x0003DB84
		internal void WriteData(MonoSymbolFile file, MyBinaryWriter bw)
		{
			if (this.index <= 0)
			{
				throw new InvalidOperationException();
			}
			this.LocalVariableTableOffset = (int)bw.BaseStream.Position;
			int num = ((this.locals != null) ? this.locals.Length : 0);
			bw.WriteLeb128(num);
			for (int i = 0; i < num; i++)
			{
				this.locals[i].Write(file, bw);
			}
			file.LocalCount += num;
			this.CodeBlockTableOffset = (int)bw.BaseStream.Position;
			int num2 = ((this.code_blocks != null) ? this.code_blocks.Length : 0);
			bw.WriteLeb128(num2);
			for (int j = 0; j < num2; j++)
			{
				this.code_blocks[j].Write(bw);
			}
			this.ScopeVariableTableOffset = (int)bw.BaseStream.Position;
			int num3 = ((this.scope_vars != null) ? this.scope_vars.Length : 0);
			bw.WriteLeb128(num3);
			for (int k = 0; k < num3; k++)
			{
				this.scope_vars[k].Write(bw);
			}
			if (this.real_name != null)
			{
				this.RealNameOffset = (int)bw.BaseStream.Position;
				bw.Write(this.real_name);
			}
			foreach (LineNumberEntry lineNumberEntry in this.lnt.LineNumbers)
			{
				if (lineNumberEntry.EndRow != -1 || lineNumberEntry.EndColumn != -1)
				{
					this.flags |= MethodEntry.Flags.EndInfoIncluded;
				}
			}
			this.LineNumberTableOffset = (int)bw.BaseStream.Position;
			this.lnt.Write(file, bw, (this.flags & MethodEntry.Flags.ColumnsInfoIncluded) > (MethodEntry.Flags)0, (this.flags & MethodEntry.Flags.EndInfoIncluded) > (MethodEntry.Flags)0);
			this.DataOffset = (int)bw.BaseStream.Position;
			bw.WriteLeb128(this.CompileUnitIndex);
			bw.WriteLeb128(this.LocalVariableTableOffset);
			bw.WriteLeb128(this.NamespaceID);
			bw.WriteLeb128(this.CodeBlockTableOffset);
			bw.WriteLeb128(this.ScopeVariableTableOffset);
			bw.WriteLeb128(this.RealNameOffset);
			bw.WriteLeb128((int)this.flags);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0003FBA0 File Offset: 0x0003DDA0
		public void ReadAll()
		{
			this.GetLineNumberTable();
			this.GetLocals();
			this.GetCodeBlocks();
			this.GetScopeVariables();
			this.GetRealName();
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0003FBC8 File Offset: 0x0003DDC8
		public LineNumberTable GetLineNumberTable()
		{
			MonoSymbolFile symbolFile = this.SymbolFile;
			LineNumberTable lineNumberTable;
			lock (symbolFile)
			{
				if (this.lnt != null)
				{
					lineNumberTable = this.lnt;
				}
				else if (this.LineNumberTableOffset == 0)
				{
					lineNumberTable = null;
				}
				else
				{
					MyBinaryReader binaryReader = this.SymbolFile.BinaryReader;
					long position = binaryReader.BaseStream.Position;
					binaryReader.BaseStream.Position = (long)this.LineNumberTableOffset;
					this.lnt = LineNumberTable.Read(this.SymbolFile, binaryReader, (this.flags & MethodEntry.Flags.ColumnsInfoIncluded) > (MethodEntry.Flags)0, (this.flags & MethodEntry.Flags.EndInfoIncluded) > (MethodEntry.Flags)0);
					binaryReader.BaseStream.Position = position;
					lineNumberTable = this.lnt;
				}
			}
			return lineNumberTable;
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0003FC8C File Offset: 0x0003DE8C
		public LocalVariableEntry[] GetLocals()
		{
			MonoSymbolFile symbolFile = this.SymbolFile;
			LocalVariableEntry[] array;
			lock (symbolFile)
			{
				if (this.locals != null)
				{
					array = this.locals;
				}
				else if (this.LocalVariableTableOffset == 0)
				{
					array = null;
				}
				else
				{
					MyBinaryReader binaryReader = this.SymbolFile.BinaryReader;
					long position = binaryReader.BaseStream.Position;
					binaryReader.BaseStream.Position = (long)this.LocalVariableTableOffset;
					int num = binaryReader.ReadLeb128();
					this.locals = new LocalVariableEntry[num];
					for (int i = 0; i < num; i++)
					{
						this.locals[i] = new LocalVariableEntry(this.SymbolFile, binaryReader);
					}
					binaryReader.BaseStream.Position = position;
					array = this.locals;
				}
			}
			return array;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0003FD6C File Offset: 0x0003DF6C
		public CodeBlockEntry[] GetCodeBlocks()
		{
			MonoSymbolFile symbolFile = this.SymbolFile;
			CodeBlockEntry[] array;
			lock (symbolFile)
			{
				if (this.code_blocks != null)
				{
					array = this.code_blocks;
				}
				else if (this.CodeBlockTableOffset == 0)
				{
					array = null;
				}
				else
				{
					MyBinaryReader binaryReader = this.SymbolFile.BinaryReader;
					long position = binaryReader.BaseStream.Position;
					binaryReader.BaseStream.Position = (long)this.CodeBlockTableOffset;
					int num = binaryReader.ReadLeb128();
					this.code_blocks = new CodeBlockEntry[num];
					for (int i = 0; i < num; i++)
					{
						this.code_blocks[i] = new CodeBlockEntry(i, binaryReader);
					}
					binaryReader.BaseStream.Position = position;
					array = this.code_blocks;
				}
			}
			return array;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0003FE44 File Offset: 0x0003E044
		public ScopeVariable[] GetScopeVariables()
		{
			MonoSymbolFile symbolFile = this.SymbolFile;
			ScopeVariable[] array;
			lock (symbolFile)
			{
				if (this.scope_vars != null)
				{
					array = this.scope_vars;
				}
				else if (this.ScopeVariableTableOffset == 0)
				{
					array = null;
				}
				else
				{
					MyBinaryReader binaryReader = this.SymbolFile.BinaryReader;
					long position = binaryReader.BaseStream.Position;
					binaryReader.BaseStream.Position = (long)this.ScopeVariableTableOffset;
					int num = binaryReader.ReadLeb128();
					this.scope_vars = new ScopeVariable[num];
					for (int i = 0; i < num; i++)
					{
						this.scope_vars[i] = new ScopeVariable(binaryReader);
					}
					binaryReader.BaseStream.Position = position;
					array = this.scope_vars;
				}
			}
			return array;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0003FF1C File Offset: 0x0003E11C
		public string GetRealName()
		{
			MonoSymbolFile symbolFile = this.SymbolFile;
			string text;
			lock (symbolFile)
			{
				if (this.real_name != null)
				{
					text = this.real_name;
				}
				else if (this.RealNameOffset == 0)
				{
					text = null;
				}
				else
				{
					this.real_name = this.SymbolFile.BinaryReader.ReadString(this.RealNameOffset);
					text = this.real_name;
				}
			}
			return text;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0003FF98 File Offset: 0x0003E198
		public int CompareTo(object obj)
		{
			MethodEntry methodEntry = (MethodEntry)obj;
			if (methodEntry.Token < this.Token)
			{
				return 1;
			}
			if (methodEntry.Token > this.Token)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0003FFD0 File Offset: 0x0003E1D0
		public override string ToString()
		{
			return string.Format("[Method {0}:{1:x}:{2}:{3}]", new object[] { this.index, this.Token, this.CompileUnitIndex, this.CompileUnit });
		}

		// Token: 0x04000A22 RID: 2594
		public readonly int CompileUnitIndex;

		// Token: 0x04000A23 RID: 2595
		public readonly int Token;

		// Token: 0x04000A24 RID: 2596
		public readonly int NamespaceID;

		// Token: 0x04000A25 RID: 2597
		private int DataOffset;

		// Token: 0x04000A26 RID: 2598
		private int LocalVariableTableOffset;

		// Token: 0x04000A27 RID: 2599
		private int LineNumberTableOffset;

		// Token: 0x04000A28 RID: 2600
		private int CodeBlockTableOffset;

		// Token: 0x04000A29 RID: 2601
		private int ScopeVariableTableOffset;

		// Token: 0x04000A2A RID: 2602
		private int RealNameOffset;

		// Token: 0x04000A2B RID: 2603
		private MethodEntry.Flags flags;

		// Token: 0x04000A2C RID: 2604
		private int index;

		// Token: 0x04000A2D RID: 2605
		public readonly CompileUnitEntry CompileUnit;

		// Token: 0x04000A2E RID: 2606
		private LocalVariableEntry[] locals;

		// Token: 0x04000A2F RID: 2607
		private CodeBlockEntry[] code_blocks;

		// Token: 0x04000A30 RID: 2608
		private ScopeVariable[] scope_vars;

		// Token: 0x04000A31 RID: 2609
		private LineNumberTable lnt;

		// Token: 0x04000A32 RID: 2610
		private string real_name;

		// Token: 0x04000A33 RID: 2611
		public readonly MonoSymbolFile SymbolFile;

		// Token: 0x04000A34 RID: 2612
		public const int Size = 12;

		// Token: 0x0200030F RID: 783
		[Flags]
		public enum Flags
		{
			// Token: 0x04000A36 RID: 2614
			LocalNamesAmbiguous = 1,
			// Token: 0x04000A37 RID: 2615
			ColumnsInfoIncluded = 2,
			// Token: 0x04000A38 RID: 2616
			EndInfoIncluded = 4
		}
	}
}

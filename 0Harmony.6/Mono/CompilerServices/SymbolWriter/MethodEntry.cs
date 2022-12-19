using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000218 RID: 536
	public class MethodEntry : IComparable
	{
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0003775C File Offset: 0x0003595C
		public MethodEntry.Flags MethodFlags
		{
			get
			{
				return this.flags;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x00037764 File Offset: 0x00035964
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x0003776C File Offset: 0x0003596C
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

		// Token: 0x06001028 RID: 4136 RVA: 0x00037778 File Offset: 0x00035978
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

		// Token: 0x06001029 RID: 4137 RVA: 0x00037850 File Offset: 0x00035A50
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

		// Token: 0x0600102A RID: 4138 RVA: 0x00037988 File Offset: 0x00035B88
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

		// Token: 0x0600102B RID: 4139 RVA: 0x000379FA File Offset: 0x00035BFA
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

		// Token: 0x0600102C RID: 4140 RVA: 0x00037A38 File Offset: 0x00035C38
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

		// Token: 0x0600102D RID: 4141 RVA: 0x00037C54 File Offset: 0x00035E54
		public void ReadAll()
		{
			this.GetLineNumberTable();
			this.GetLocals();
			this.GetCodeBlocks();
			this.GetScopeVariables();
			this.GetRealName();
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00037C7C File Offset: 0x00035E7C
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

		// Token: 0x0600102F RID: 4143 RVA: 0x00037D40 File Offset: 0x00035F40
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

		// Token: 0x06001030 RID: 4144 RVA: 0x00037E20 File Offset: 0x00036020
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

		// Token: 0x06001031 RID: 4145 RVA: 0x00037EF8 File Offset: 0x000360F8
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

		// Token: 0x06001032 RID: 4146 RVA: 0x00037FD0 File Offset: 0x000361D0
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

		// Token: 0x06001033 RID: 4147 RVA: 0x0003804C File Offset: 0x0003624C
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

		// Token: 0x06001034 RID: 4148 RVA: 0x00038084 File Offset: 0x00036284
		public override string ToString()
		{
			return string.Format("[Method {0}:{1:x}:{2}:{3}]", new object[] { this.index, this.Token, this.CompileUnitIndex, this.CompileUnit });
		}

		// Token: 0x040009E3 RID: 2531
		public readonly int CompileUnitIndex;

		// Token: 0x040009E4 RID: 2532
		public readonly int Token;

		// Token: 0x040009E5 RID: 2533
		public readonly int NamespaceID;

		// Token: 0x040009E6 RID: 2534
		private int DataOffset;

		// Token: 0x040009E7 RID: 2535
		private int LocalVariableTableOffset;

		// Token: 0x040009E8 RID: 2536
		private int LineNumberTableOffset;

		// Token: 0x040009E9 RID: 2537
		private int CodeBlockTableOffset;

		// Token: 0x040009EA RID: 2538
		private int ScopeVariableTableOffset;

		// Token: 0x040009EB RID: 2539
		private int RealNameOffset;

		// Token: 0x040009EC RID: 2540
		private MethodEntry.Flags flags;

		// Token: 0x040009ED RID: 2541
		private int index;

		// Token: 0x040009EE RID: 2542
		public readonly CompileUnitEntry CompileUnit;

		// Token: 0x040009EF RID: 2543
		private LocalVariableEntry[] locals;

		// Token: 0x040009F0 RID: 2544
		private CodeBlockEntry[] code_blocks;

		// Token: 0x040009F1 RID: 2545
		private ScopeVariable[] scope_vars;

		// Token: 0x040009F2 RID: 2546
		private LineNumberTable lnt;

		// Token: 0x040009F3 RID: 2547
		private string real_name;

		// Token: 0x040009F4 RID: 2548
		public readonly MonoSymbolFile SymbolFile;

		// Token: 0x040009F5 RID: 2549
		public const int Size = 12;

		// Token: 0x02000219 RID: 537
		[Flags]
		public enum Flags
		{
			// Token: 0x040009F7 RID: 2551
			LocalNamesAmbiguous = 1,
			// Token: 0x040009F8 RID: 2552
			ColumnsInfoIncluded = 2,
			// Token: 0x040009F9 RID: 2553
			EndInfoIncluded = 4
		}
	}
}

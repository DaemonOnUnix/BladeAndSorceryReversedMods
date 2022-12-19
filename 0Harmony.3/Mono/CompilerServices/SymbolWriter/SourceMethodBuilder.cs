using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021C RID: 540
	internal class SourceMethodBuilder
	{
		// Token: 0x06001053 RID: 4179 RVA: 0x000384F4 File Offset: 0x000366F4
		public SourceMethodBuilder(ICompileUnit comp_unit)
		{
			this._comp_unit = comp_unit;
			this.method_lines = new List<LineNumberEntry>();
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0003850E File Offset: 0x0003670E
		public SourceMethodBuilder(ICompileUnit comp_unit, int ns_id, IMethodDef method)
			: this(comp_unit)
		{
			this.ns_id = ns_id;
			this.method = method;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00038525 File Offset: 0x00036725
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, bool is_hidden)
		{
			this.MarkSequencePoint(offset, file, line, column, -1, -1, is_hidden);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00038538 File Offset: 0x00036738
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, int end_line, int end_column, bool is_hidden)
		{
			LineNumberEntry lineNumberEntry = new LineNumberEntry((file != null) ? file.Index : 0, line, column, end_line, end_column, offset, is_hidden);
			if (this.method_lines.Count > 0)
			{
				LineNumberEntry lineNumberEntry2 = this.method_lines[this.method_lines.Count - 1];
				if (lineNumberEntry2.Offset == offset)
				{
					if (LineNumberEntry.LocationComparer.Default.Compare(lineNumberEntry, lineNumberEntry2) > 0)
					{
						this.method_lines[this.method_lines.Count - 1] = lineNumberEntry;
					}
					return;
				}
			}
			this.method_lines.Add(lineNumberEntry);
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x000385C6 File Offset: 0x000367C6
		public void StartBlock(CodeBlockEntry.Type type, int start_offset)
		{
			this.StartBlock(type, start_offset, (this._blocks == null) ? 1 : (this._blocks.Count + 1));
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x000385E8 File Offset: 0x000367E8
		public void StartBlock(CodeBlockEntry.Type type, int start_offset, int scopeIndex)
		{
			if (this._block_stack == null)
			{
				this._block_stack = new Stack<CodeBlockEntry>();
			}
			if (this._blocks == null)
			{
				this._blocks = new List<CodeBlockEntry>();
			}
			int num = ((this.CurrentBlock != null) ? this.CurrentBlock.Index : (-1));
			CodeBlockEntry codeBlockEntry = new CodeBlockEntry(scopeIndex, num, type, start_offset);
			this._block_stack.Push(codeBlockEntry);
			this._blocks.Add(codeBlockEntry);
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00038654 File Offset: 0x00036854
		public void EndBlock(int end_offset)
		{
			this._block_stack.Pop().Close(end_offset);
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x00038668 File Offset: 0x00036868
		public CodeBlockEntry[] Blocks
		{
			get
			{
				if (this._blocks == null)
				{
					return new CodeBlockEntry[0];
				}
				CodeBlockEntry[] array = new CodeBlockEntry[this._blocks.Count];
				this._blocks.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x000386A3 File Offset: 0x000368A3
		public CodeBlockEntry CurrentBlock
		{
			get
			{
				if (this._block_stack != null && this._block_stack.Count > 0)
				{
					return this._block_stack.Peek();
				}
				return null;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x000386C8 File Offset: 0x000368C8
		public LocalVariableEntry[] Locals
		{
			get
			{
				if (this._locals == null)
				{
					return new LocalVariableEntry[0];
				}
				return this._locals.ToArray();
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x000386E4 File Offset: 0x000368E4
		public ICompileUnit SourceFile
		{
			get
			{
				return this._comp_unit;
			}
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000386EC File Offset: 0x000368EC
		public void AddLocal(int index, string name)
		{
			if (this._locals == null)
			{
				this._locals = new List<LocalVariableEntry>();
			}
			int num = ((this.CurrentBlock != null) ? this.CurrentBlock.Index : 0);
			this._locals.Add(new LocalVariableEntry(index, name, num));
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x00038736 File Offset: 0x00036936
		public ScopeVariable[] ScopeVariables
		{
			get
			{
				if (this._scope_vars == null)
				{
					return new ScopeVariable[0];
				}
				return this._scope_vars.ToArray();
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x00038752 File Offset: 0x00036952
		public void AddScopeVariable(int scope, int index)
		{
			if (this._scope_vars == null)
			{
				this._scope_vars = new List<ScopeVariable>();
			}
			this._scope_vars.Add(new ScopeVariable(scope, index));
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00038779 File Offset: 0x00036979
		public void DefineMethod(MonoSymbolFile file)
		{
			this.DefineMethod(file, this.method.Token);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00038790 File Offset: 0x00036990
		public void DefineMethod(MonoSymbolFile file, int token)
		{
			CodeBlockEntry[] array = this.Blocks;
			if (array.Length != 0)
			{
				List<CodeBlockEntry> list = new List<CodeBlockEntry>(array.Length);
				int num = 0;
				for (int i = 0; i < array.Length; i++)
				{
					num = Math.Max(num, array[i].Index);
				}
				for (int j = 0; j < num; j++)
				{
					int num2 = j + 1;
					if (j < array.Length && array[j].Index == num2)
					{
						list.Add(array[j]);
					}
					else
					{
						bool flag = false;
						for (int k = 0; k < array.Length; k++)
						{
							if (array[k].Index == num2)
							{
								list.Add(array[k]);
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							list.Add(new CodeBlockEntry(num2, -1, CodeBlockEntry.Type.CompilerGenerated, 0));
						}
					}
				}
				array = list.ToArray();
			}
			MethodEntry methodEntry = new MethodEntry(file, this._comp_unit.Entry, token, this.ScopeVariables, this.Locals, this.method_lines.ToArray(), array, null, MethodEntry.Flags.ColumnsInfoIncluded, this.ns_id);
			file.AddMethod(methodEntry);
		}

		// Token: 0x04000A05 RID: 2565
		private List<LocalVariableEntry> _locals;

		// Token: 0x04000A06 RID: 2566
		private List<CodeBlockEntry> _blocks;

		// Token: 0x04000A07 RID: 2567
		private List<ScopeVariable> _scope_vars;

		// Token: 0x04000A08 RID: 2568
		private Stack<CodeBlockEntry> _block_stack;

		// Token: 0x04000A09 RID: 2569
		private readonly List<LineNumberEntry> method_lines;

		// Token: 0x04000A0A RID: 2570
		private readonly ICompileUnit _comp_unit;

		// Token: 0x04000A0B RID: 2571
		private readonly int ns_id;

		// Token: 0x04000A0C RID: 2572
		private readonly IMethodDef method;
	}
}

using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000312 RID: 786
	internal class SourceMethodBuilder
	{
		// Token: 0x060013C3 RID: 5059 RVA: 0x00040440 File Offset: 0x0003E640
		public SourceMethodBuilder(ICompileUnit comp_unit)
		{
			this._comp_unit = comp_unit;
			this.method_lines = new List<LineNumberEntry>();
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0004045A File Offset: 0x0003E65A
		public SourceMethodBuilder(ICompileUnit comp_unit, int ns_id, IMethodDef method)
			: this(comp_unit)
		{
			this.ns_id = ns_id;
			this.method = method;
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00040471 File Offset: 0x0003E671
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, bool is_hidden)
		{
			this.MarkSequencePoint(offset, file, line, column, -1, -1, is_hidden);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00040484 File Offset: 0x0003E684
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

		// Token: 0x060013C7 RID: 5063 RVA: 0x00040512 File Offset: 0x0003E712
		public void StartBlock(CodeBlockEntry.Type type, int start_offset)
		{
			this.StartBlock(type, start_offset, (this._blocks == null) ? 1 : (this._blocks.Count + 1));
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00040534 File Offset: 0x0003E734
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

		// Token: 0x060013C9 RID: 5065 RVA: 0x000405A0 File Offset: 0x0003E7A0
		public void EndBlock(int end_offset)
		{
			this._block_stack.Pop().Close(end_offset);
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x000405B4 File Offset: 0x0003E7B4
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

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x000405EF File Offset: 0x0003E7EF
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

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x00040614 File Offset: 0x0003E814
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

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x00040630 File Offset: 0x0003E830
		public ICompileUnit SourceFile
		{
			get
			{
				return this._comp_unit;
			}
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00040638 File Offset: 0x0003E838
		public void AddLocal(int index, string name)
		{
			if (this._locals == null)
			{
				this._locals = new List<LocalVariableEntry>();
			}
			int num = ((this.CurrentBlock != null) ? this.CurrentBlock.Index : 0);
			this._locals.Add(new LocalVariableEntry(index, name, num));
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x00040682 File Offset: 0x0003E882
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

		// Token: 0x060013D0 RID: 5072 RVA: 0x0004069E File Offset: 0x0003E89E
		public void AddScopeVariable(int scope, int index)
		{
			if (this._scope_vars == null)
			{
				this._scope_vars = new List<ScopeVariable>();
			}
			this._scope_vars.Add(new ScopeVariable(scope, index));
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x000406C5 File Offset: 0x0003E8C5
		public void DefineMethod(MonoSymbolFile file)
		{
			this.DefineMethod(file, this.method.Token);
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x000406DC File Offset: 0x0003E8DC
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

		// Token: 0x04000A44 RID: 2628
		private List<LocalVariableEntry> _locals;

		// Token: 0x04000A45 RID: 2629
		private List<CodeBlockEntry> _blocks;

		// Token: 0x04000A46 RID: 2630
		private List<ScopeVariable> _scope_vars;

		// Token: 0x04000A47 RID: 2631
		private Stack<CodeBlockEntry> _block_stack;

		// Token: 0x04000A48 RID: 2632
		private readonly List<LineNumberEntry> method_lines;

		// Token: 0x04000A49 RID: 2633
		private readonly ICompileUnit _comp_unit;

		// Token: 0x04000A4A RID: 2634
		private readonly int ns_id;

		// Token: 0x04000A4B RID: 2635
		private readonly IMethodDef method;
	}
}

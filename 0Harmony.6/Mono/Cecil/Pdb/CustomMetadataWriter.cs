using System;
using System.IO;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000233 RID: 563
	internal class CustomMetadataWriter : IDisposable
	{
		// Token: 0x060011CF RID: 4559 RVA: 0x0003A9B0 File Offset: 0x00038BB0
		public CustomMetadataWriter(SymWriter sym_writer)
		{
			this.sym_writer = sym_writer;
			this.stream = new MemoryStream();
			this.writer = new BinaryStreamWriter(this.stream);
			this.writer.WriteByte(4);
			this.writer.WriteByte(0);
			this.writer.Align(4);
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0003AA0C File Offset: 0x00038C0C
		public void WriteUsingInfo(ImportDebugInformation import_info)
		{
			this.Write(CustomMetadataType.UsingInfo, delegate
			{
				this.writer.WriteUInt16(1);
				this.writer.WriteUInt16((ushort)import_info.Targets.Count);
			});
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0003AA40 File Offset: 0x00038C40
		public void WriteForwardInfo(MetadataToken import_parent)
		{
			this.Write(CustomMetadataType.ForwardInfo, delegate
			{
				this.writer.WriteUInt32(import_parent.ToUInt32());
			});
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0003AA74 File Offset: 0x00038C74
		public void WriteIteratorScopes(StateMachineScopeDebugInformation state_machine, MethodDebugInformation debug_info)
		{
			this.Write(CustomMetadataType.IteratorScopes, delegate
			{
				Collection<StateMachineScope> scopes = state_machine.Scopes;
				this.writer.WriteInt32(scopes.Count);
				foreach (StateMachineScope stateMachineScope in scopes)
				{
					int offset = stateMachineScope.Start.Offset;
					int num = (stateMachineScope.End.IsEndOfMethod ? debug_info.code_size : stateMachineScope.End.Offset);
					this.writer.WriteInt32(offset);
					this.writer.WriteInt32(num - 1);
				}
			});
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0003AAB0 File Offset: 0x00038CB0
		public void WriteForwardIterator(TypeReference type)
		{
			this.Write(CustomMetadataType.ForwardIterator, delegate
			{
				this.writer.WriteBytes(Encoding.Unicode.GetBytes(type.Name));
			});
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0003AAE4 File Offset: 0x00038CE4
		private void Write(CustomMetadataType type, Action write)
		{
			this.count++;
			this.writer.WriteByte(4);
			this.writer.WriteByte((byte)type);
			this.writer.Align(4);
			int position = this.writer.Position;
			this.writer.WriteUInt32(0U);
			write();
			this.writer.Align(4);
			int position2 = this.writer.Position;
			int num = position2 - position + 4;
			this.writer.Position = position;
			this.writer.WriteInt32(num);
			this.writer.Position = position2;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0003AB84 File Offset: 0x00038D84
		public void WriteCustomMetadata()
		{
			if (this.count == 0)
			{
				return;
			}
			this.writer.BaseStream.Position = 1L;
			this.writer.WriteByte((byte)this.count);
			this.writer.Flush();
			this.sym_writer.DefineCustomMetadata("MD2", this.stream.ToArray());
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0003ABE4 File Offset: 0x00038DE4
		public void Dispose()
		{
			this.stream.Dispose();
		}

		// Token: 0x04000A3E RID: 2622
		private readonly SymWriter sym_writer;

		// Token: 0x04000A3F RID: 2623
		private readonly MemoryStream stream;

		// Token: 0x04000A40 RID: 2624
		private readonly BinaryStreamWriter writer;

		// Token: 0x04000A41 RID: 2625
		private int count;

		// Token: 0x04000A42 RID: 2626
		private const byte version = 4;
	}
}

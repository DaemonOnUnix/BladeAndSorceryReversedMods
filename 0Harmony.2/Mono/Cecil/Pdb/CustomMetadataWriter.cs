using System;
using System.IO;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000329 RID: 809
	internal class CustomMetadataWriter : IDisposable
	{
		// Token: 0x0600153E RID: 5438 RVA: 0x000428F8 File Offset: 0x00040AF8
		public CustomMetadataWriter(SymWriter sym_writer)
		{
			this.sym_writer = sym_writer;
			this.stream = new MemoryStream();
			this.writer = new BinaryStreamWriter(this.stream);
			this.writer.WriteByte(4);
			this.writer.WriteByte(0);
			this.writer.Align(4);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00042954 File Offset: 0x00040B54
		public void WriteUsingInfo(ImportDebugInformation import_info)
		{
			this.Write(CustomMetadataType.UsingInfo, delegate
			{
				this.writer.WriteUInt16(1);
				this.writer.WriteUInt16((ushort)import_info.Targets.Count);
			});
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x00042988 File Offset: 0x00040B88
		public void WriteForwardInfo(MetadataToken import_parent)
		{
			this.Write(CustomMetadataType.ForwardInfo, delegate
			{
				this.writer.WriteUInt32(import_parent.ToUInt32());
			});
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x000429BC File Offset: 0x00040BBC
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

		// Token: 0x06001542 RID: 5442 RVA: 0x000429F8 File Offset: 0x00040BF8
		public void WriteForwardIterator(TypeReference type)
		{
			this.Write(CustomMetadataType.ForwardIterator, delegate
			{
				this.writer.WriteBytes(Encoding.Unicode.GetBytes(type.Name));
			});
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00042A2C File Offset: 0x00040C2C
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

		// Token: 0x06001544 RID: 5444 RVA: 0x00042ACC File Offset: 0x00040CCC
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

		// Token: 0x06001545 RID: 5445 RVA: 0x00042B2C File Offset: 0x00040D2C
		public void Dispose()
		{
			this.stream.Dispose();
		}

		// Token: 0x04000A7D RID: 2685
		private readonly SymWriter sym_writer;

		// Token: 0x04000A7E RID: 2686
		private readonly MemoryStream stream;

		// Token: 0x04000A7F RID: 2687
		private readonly BinaryStreamWriter writer;

		// Token: 0x04000A80 RID: 2688
		private int count;

		// Token: 0x04000A81 RID: 2689
		private const byte version = 4;
	}
}

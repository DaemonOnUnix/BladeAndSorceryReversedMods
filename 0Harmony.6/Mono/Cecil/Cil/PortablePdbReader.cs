using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D0 RID: 464
	internal sealed class PortablePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x00033744 File Offset: 0x00031944
		private bool IsEmbedded
		{
			get
			{
				return this.reader.image == this.debug_reader.image;
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0003375E File Offset: 0x0003195E
		internal PortablePdbReader(Image image, ModuleDefinition module)
		{
			this.image = image;
			this.module = module;
			this.reader = module.reader;
			this.debug_reader = new MetadataReader(image, module, this.reader);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00033793 File Offset: 0x00031993
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new PortablePdbWriterProvider();
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0003379C File Offset: 0x0003199C
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			if (this.image == this.module.Image)
			{
				return true;
			}
			ImageDebugHeaderEntry codeViewEntry = header.GetCodeViewEntry();
			if (codeViewEntry == null)
			{
				return false;
			}
			byte[] data = codeViewEntry.Data;
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
			Buffer.BlockCopy(this.image.PdbHeap.Id, 0, array, 0, 16);
			Guid guid2 = new Guid(array);
			if (guid != guid2)
			{
				return false;
			}
			this.ReadModule();
			return true;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00033836 File Offset: 0x00031A36
		private static int ReadInt32(byte[] bytes, int start)
		{
			return (int)bytes[start] | ((int)bytes[start + 1] << 8) | ((int)bytes[start + 2] << 16) | ((int)bytes[start + 3] << 24);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00033855 File Offset: 0x00031A55
		private void ReadModule()
		{
			this.module.custom_infos = this.debug_reader.GetCustomDebugInformation(this.module);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00033874 File Offset: 0x00031A74
		public MethodDebugInformation Read(MethodDefinition method)
		{
			MethodDebugInformation methodDebugInformation = new MethodDebugInformation(method);
			this.ReadSequencePoints(methodDebugInformation);
			this.ReadScope(methodDebugInformation);
			this.ReadStateMachineKickOffMethod(methodDebugInformation);
			this.ReadCustomDebugInformations(methodDebugInformation);
			return methodDebugInformation;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x000338A5 File Offset: 0x00031AA5
		private void ReadSequencePoints(MethodDebugInformation method_info)
		{
			method_info.sequence_points = this.debug_reader.ReadSequencePoints(method_info.method);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x000338BE File Offset: 0x00031ABE
		private void ReadScope(MethodDebugInformation method_info)
		{
			method_info.scope = this.debug_reader.ReadScope(method_info.method);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x000338D7 File Offset: 0x00031AD7
		private void ReadStateMachineKickOffMethod(MethodDebugInformation method_info)
		{
			method_info.kickoff_method = this.debug_reader.ReadStateMachineKickoffMethod(method_info.method);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x000338F0 File Offset: 0x00031AF0
		private void ReadCustomDebugInformations(MethodDebugInformation info)
		{
			info.method.custom_infos = this.debug_reader.GetCustomDebugInformation(info.method);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0003390E File Offset: 0x00031B0E
		public void Dispose()
		{
			if (this.IsEmbedded)
			{
				return;
			}
			this.image.Dispose();
		}

		// Token: 0x040008EC RID: 2284
		private readonly Image image;

		// Token: 0x040008ED RID: 2285
		private readonly ModuleDefinition module;

		// Token: 0x040008EE RID: 2286
		private readonly MetadataReader reader;

		// Token: 0x040008EF RID: 2287
		private readonly MetadataReader debug_reader;
	}
}

using System;
using System.IO;

namespace Mono.Cecil
{
	// Token: 0x020001FA RID: 506
	internal sealed class EmbeddedResource : Resource
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x000183ED File Offset: 0x000165ED
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Embedded;
			}
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00026388 File Offset: 0x00024588
		public EmbeddedResource(string name, ManifestResourceAttributes attributes, byte[] data)
			: base(name, attributes)
		{
			this.data = data;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00026399 File Offset: 0x00024599
		public EmbeddedResource(string name, ManifestResourceAttributes attributes, Stream stream)
			: base(name, attributes)
		{
			this.stream = stream;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000263AA File Offset: 0x000245AA
		internal EmbeddedResource(string name, ManifestResourceAttributes attributes, uint offset, MetadataReader reader)
			: base(name, attributes)
		{
			this.offset = new uint?(offset);
			this.reader = reader;
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x000263C8 File Offset: 0x000245C8
		public Stream GetResourceStream()
		{
			if (this.stream != null)
			{
				return this.stream;
			}
			if (this.data != null)
			{
				return new MemoryStream(this.data);
			}
			if (this.offset != null)
			{
				return new MemoryStream(this.reader.GetManagedResource(this.offset.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00026428 File Offset: 0x00024628
		public byte[] GetResourceData()
		{
			if (this.stream != null)
			{
				return EmbeddedResource.ReadStream(this.stream);
			}
			if (this.data != null)
			{
				return this.data;
			}
			if (this.offset != null)
			{
				return this.reader.GetManagedResource(this.offset.Value);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00026484 File Offset: 0x00024684
		private static byte[] ReadStream(Stream stream)
		{
			int num3;
			if (stream.CanSeek)
			{
				int num = (int)stream.Length;
				byte[] array = new byte[num];
				int num2 = 0;
				while ((num3 = stream.Read(array, num2, num - num2)) > 0)
				{
					num2 += num3;
				}
				return array;
			}
			byte[] array2 = new byte[8192];
			MemoryStream memoryStream = new MemoryStream();
			while ((num3 = stream.Read(array2, 0, array2.Length)) > 0)
			{
				memoryStream.Write(array2, 0, num3);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x040002E6 RID: 742
		private readonly MetadataReader reader;

		// Token: 0x040002E7 RID: 743
		private uint? offset;

		// Token: 0x040002E8 RID: 744
		private byte[] data;

		// Token: 0x040002E9 RID: 745
		private Stream stream;
	}
}

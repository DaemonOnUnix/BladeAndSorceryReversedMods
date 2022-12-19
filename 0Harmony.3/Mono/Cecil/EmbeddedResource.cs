using System;
using System.IO;

namespace Mono.Cecil
{
	// Token: 0x02000108 RID: 264
	internal sealed class EmbeddedResource : Resource
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x00012561 File Offset: 0x00010761
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Embedded;
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000204E0 File Offset: 0x0001E6E0
		public EmbeddedResource(string name, ManifestResourceAttributes attributes, byte[] data)
			: base(name, attributes)
		{
			this.data = data;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x000204F1 File Offset: 0x0001E6F1
		public EmbeddedResource(string name, ManifestResourceAttributes attributes, Stream stream)
			: base(name, attributes)
		{
			this.stream = stream;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00020502 File Offset: 0x0001E702
		internal EmbeddedResource(string name, ManifestResourceAttributes attributes, uint offset, MetadataReader reader)
			: base(name, attributes)
		{
			this.offset = new uint?(offset);
			this.reader = reader;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00020520 File Offset: 0x0001E720
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

		// Token: 0x0600071F RID: 1823 RVA: 0x00020580 File Offset: 0x0001E780
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

		// Token: 0x06000720 RID: 1824 RVA: 0x000205DC File Offset: 0x0001E7DC
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

		// Token: 0x040002B4 RID: 692
		private readonly MetadataReader reader;

		// Token: 0x040002B5 RID: 693
		private uint? offset;

		// Token: 0x040002B6 RID: 694
		private byte[] data;

		// Token: 0x040002B7 RID: 695
		private Stream stream;
	}
}

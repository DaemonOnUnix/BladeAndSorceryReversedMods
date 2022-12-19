using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HarmonyLib
{
	// Token: 0x02000073 RID: 115
	internal static class PatchInfoSerialization
	{
		// Token: 0x06000220 RID: 544 RVA: 0x0000C5A8 File Offset: 0x0000A7A8
		internal static byte[] Serialize(this PatchInfo patchInfo)
		{
			byte[] buffer;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PatchInfoSerialization.binaryFormatter.Serialize(memoryStream, patchInfo);
				buffer = memoryStream.GetBuffer();
			}
			return buffer;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000C5EC File Offset: 0x0000A7EC
		internal static PatchInfo Deserialize(byte[] bytes)
		{
			PatchInfo patchInfo;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				patchInfo = (PatchInfo)PatchInfoSerialization.binaryFormatter.Deserialize(memoryStream);
			}
			return patchInfo;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000C630 File Offset: 0x0000A830
		internal static int PriorityComparer(object obj, int index, int priority)
		{
			Traverse traverse = Traverse.Create(obj);
			int value = traverse.Field("priority").GetValue<int>();
			int value2 = traverse.Field("index").GetValue<int>();
			if (priority != value)
			{
				return -priority.CompareTo(value);
			}
			return index.CompareTo(value2);
		}

		// Token: 0x04000152 RID: 338
		internal static readonly BinaryFormatter binaryFormatter = new BinaryFormatter
		{
			Binder = new PatchInfoSerialization.Binder()
		};

		// Token: 0x02000074 RID: 116
		private class Binder : SerializationBinder
		{
			// Token: 0x06000224 RID: 548 RVA: 0x0000C694 File Offset: 0x0000A894
			public override Type BindToType(string assemblyName, string typeName)
			{
				foreach (Type type in new Type[]
				{
					typeof(PatchInfo),
					typeof(Patch[]),
					typeof(Patch)
				})
				{
					if (typeName == type.FullName)
					{
						return type;
					}
				}
				return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
			}
		}
	}
}

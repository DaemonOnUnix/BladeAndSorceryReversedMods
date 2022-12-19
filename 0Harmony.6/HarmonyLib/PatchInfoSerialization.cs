using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HarmonyLib
{
	// Token: 0x02000070 RID: 112
	internal static class PatchInfoSerialization
	{
		// Token: 0x06000205 RID: 517 RVA: 0x0000B3C4 File Offset: 0x000095C4
		internal static byte[] Serialize(this PatchInfo patchInfo)
		{
			byte[] buffer;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, patchInfo);
				buffer = memoryStream.GetBuffer();
			}
			return buffer;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000B408 File Offset: 0x00009608
		internal static PatchInfo Deserialize(byte[] bytes)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Binder = new PatchInfoSerialization.Binder();
			MemoryStream memoryStream = new MemoryStream(bytes);
			return (PatchInfo)binaryFormatter.Deserialize(memoryStream);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000B438 File Offset: 0x00009638
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

		// Token: 0x02000071 RID: 113
		private class Binder : SerializationBinder
		{
			// Token: 0x06000208 RID: 520 RVA: 0x0000B484 File Offset: 0x00009684
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

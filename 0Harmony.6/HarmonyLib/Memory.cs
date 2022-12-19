using System;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200001E RID: 30
	public static class Memory
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x000050B0 File Offset: 0x000032B0
		public unsafe static void MarkForNoInlining(MethodBase method)
		{
			if (AccessTools.IsMonoRuntime)
			{
				byte* ptr = (byte*)(void*)method.MethodHandle.Value + 2;
				*(short*)ptr = (short)(*(ushort*)ptr | 8);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000050E0 File Offset: 0x000032E0
		public static string DetourMethod(MethodBase original, MethodBase replacement)
		{
			Exception ex;
			long methodStart = Memory.GetMethodStart(original, out ex);
			if (methodStart == 0L)
			{
				return ex.Message;
			}
			Memory.PadShortMethods(original);
			long methodStart2 = Memory.GetMethodStart(replacement, out ex);
			if (methodStart2 == 0L)
			{
				return ex.Message;
			}
			return Memory.WriteJump(methodStart, methodStart2);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005120 File Offset: 0x00003320
		internal static void DetourMethodAndPersist(MethodBase original, MethodBase replacement)
		{
			string text = Memory.DetourMethod(original, replacement);
			if (text != null)
			{
				throw new FormatException("Method " + original.FullDescription() + " cannot be patched. Reason: " + text);
			}
			PatchTools.RememberObject(original, replacement);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000515C File Offset: 0x0000335C
		internal static void PadShortMethods(MethodBase method)
		{
			if (Memory.isWindows)
			{
				return;
			}
			MethodBody methodBody = method.GetMethodBody();
			int? num;
			if (methodBody == null)
			{
				num = null;
			}
			else
			{
				byte[] ilasByteArray = methodBody.GetILAsByteArray();
				num = ((ilasByteArray != null) ? new int?(ilasByteArray.Length) : null);
			}
			int? num2 = num;
			int valueOrDefault = num2.GetValueOrDefault();
			if (valueOrDefault == 0)
			{
				return;
			}
			if (valueOrDefault >= 16)
			{
				return;
			}
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(string.Format("PadMethod-{0}", Guid.NewGuid()), typeof(void), new Type[0]);
			dynamicMethodDefinition.GetILGenerator().Emit(OpCodes.Ret);
			Exception ex;
			Memory.GetMethodStart(dynamicMethodDefinition.Generate(), out ex);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000051FC File Offset: 0x000033FC
		public static string WriteJump(long memory, long destination)
		{
			NativeDetourData nativeDetourData = DetourHelper.Native.Create((IntPtr)memory, (IntPtr)destination, null);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			DetourHelper.Native.Free(nativeDetourData);
			return null;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005264 File Offset: 0x00003464
		public static long GetMethodStart(MethodBase method, out Exception exception)
		{
			long num;
			try
			{
				exception = null;
				num = method.Pin<MethodBase>().GetNativeStart().ToInt64();
			}
			catch (Exception ex)
			{
				exception = ex;
				num = 0L;
			}
			return num;
		}

		// Token: 0x04000058 RID: 88
		private static readonly bool isWindows = Environment.OSVersion.Platform.Equals(PlatformID.Win32NT);
	}
}

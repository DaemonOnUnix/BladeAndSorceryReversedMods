using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000077 RID: 119
	[Serializable]
	public class Patch : IComparable
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000B904 File Offset: 0x00009B04
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000B99C File Offset: 0x00009B9C
		public MethodInfo PatchMethod
		{
			get
			{
				if (this.patchMethod == null)
				{
					Module module = (from a in AppDomain.CurrentDomain.GetAssemblies()
						where !a.FullName.StartsWith("Microsoft.VisualStudio")
						select a).SelectMany((Assembly a) => a.GetLoadedModules()).First((Module m) => m.ModuleVersionId.ToString() == this.moduleGUID);
					this.patchMethod = (MethodInfo)module.ResolveMethod(this.methodToken);
				}
				return this.patchMethod;
			}
			set
			{
				this.patchMethod = value;
				this.methodToken = this.patchMethod.MetadataToken;
				this.moduleGUID = this.patchMethod.Module.ModuleVersionId.ToString();
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000B9E8 File Offset: 0x00009BE8
		public Patch(MethodInfo patch, int index, string owner, int priority, string[] before, string[] after, bool debug)
		{
			if (patch is DynamicMethod)
			{
				throw new Exception("Cannot directly reference dynamic method \"" + patch.FullDescription() + "\" in Harmony. Use a factory method instead that will return the dynamic method.");
			}
			this.index = index;
			this.owner = owner;
			this.priority = ((priority == -1) ? 400 : priority);
			this.before = before ?? new string[0];
			this.after = after ?? new string[0];
			this.debug = debug;
			this.PatchMethod = patch;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000BA73 File Offset: 0x00009C73
		public Patch(HarmonyMethod method, int index, string owner)
			: this(method.method, index, owner, method.priority, method.before, method.after, method.debug.GetValueOrDefault())
		{
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000BAA0 File Offset: 0x00009CA0
		public MethodInfo GetMethod(MethodBase original)
		{
			MethodInfo methodInfo = this.PatchMethod;
			if (methodInfo.ReturnType != typeof(DynamicMethod) && methodInfo.ReturnType != typeof(MethodInfo))
			{
				return methodInfo;
			}
			if (!methodInfo.IsStatic)
			{
				return methodInfo;
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Count<ParameterInfo>() != 1)
			{
				return methodInfo;
			}
			if (parameters[0].ParameterType != typeof(MethodBase))
			{
				return methodInfo;
			}
			return methodInfo.Invoke(null, new object[] { original }) as MethodInfo;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000BB31 File Offset: 0x00009D31
		public override bool Equals(object obj)
		{
			return obj != null && obj is Patch && this.PatchMethod == ((Patch)obj).PatchMethod;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000BB56 File Offset: 0x00009D56
		public int CompareTo(object obj)
		{
			return PatchInfoSerialization.PriorityComparer(obj, this.index, this.priority);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000BB6A File Offset: 0x00009D6A
		public override int GetHashCode()
		{
			return this.PatchMethod.GetHashCode();
		}

		// Token: 0x0400014F RID: 335
		public readonly int index;

		// Token: 0x04000150 RID: 336
		public readonly string owner;

		// Token: 0x04000151 RID: 337
		public readonly int priority;

		// Token: 0x04000152 RID: 338
		public readonly string[] before;

		// Token: 0x04000153 RID: 339
		public readonly string[] after;

		// Token: 0x04000154 RID: 340
		public readonly bool debug;

		// Token: 0x04000155 RID: 341
		[NonSerialized]
		private MethodInfo patchMethod;

		// Token: 0x04000156 RID: 342
		private int methodToken;

		// Token: 0x04000157 RID: 343
		private string moduleGUID;
	}
}

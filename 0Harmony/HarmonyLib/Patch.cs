using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200007A RID: 122
	[Serializable]
	public class Patch : IComparable
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000CB14 File Offset: 0x0000AD14
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000CBAC File Offset: 0x0000ADAC
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

		// Token: 0x06000249 RID: 585 RVA: 0x0000CBF8 File Offset: 0x0000ADF8
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

		// Token: 0x0600024A RID: 586 RVA: 0x0000CC83 File Offset: 0x0000AE83
		public Patch(HarmonyMethod method, int index, string owner)
			: this(method.method, index, owner, method.priority, method.before, method.after, method.debug.GetValueOrDefault())
		{
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
		internal Patch(int index, string owner, int priority, string[] before, string[] after, bool debug, int methodToken, string moduleGUID)
		{
			this.index = index;
			this.owner = owner;
			this.priority = ((priority == -1) ? 400 : priority);
			this.before = before ?? new string[0];
			this.after = after ?? new string[0];
			this.debug = debug;
			this.methodToken = methodToken;
			this.moduleGUID = moduleGUID;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000CD20 File Offset: 0x0000AF20
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
			if (parameters.Length != 1)
			{
				return methodInfo;
			}
			if (parameters[0].ParameterType != typeof(MethodBase))
			{
				return methodInfo;
			}
			return methodInfo.Invoke(null, new object[] { original }) as MethodInfo;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000CDAE File Offset: 0x0000AFAE
		public override bool Equals(object obj)
		{
			return obj != null && obj is Patch && this.PatchMethod == ((Patch)obj).PatchMethod;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000CDD3 File Offset: 0x0000AFD3
		public int CompareTo(object obj)
		{
			return PatchInfoSerialization.PriorityComparer(obj, this.index, this.priority);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000CDE7 File Offset: 0x0000AFE7
		public override int GetHashCode()
		{
			return this.PatchMethod.GetHashCode();
		}

		// Token: 0x04000161 RID: 353
		public readonly int index;

		// Token: 0x04000162 RID: 354
		public readonly string owner;

		// Token: 0x04000163 RID: 355
		public readonly int priority;

		// Token: 0x04000164 RID: 356
		public readonly string[] before;

		// Token: 0x04000165 RID: 357
		public readonly string[] after;

		// Token: 0x04000166 RID: 358
		public readonly bool debug;

		// Token: 0x04000167 RID: 359
		[NonSerialized]
		private MethodInfo patchMethod;

		// Token: 0x04000168 RID: 360
		private int methodToken;

		// Token: 0x04000169 RID: 361
		private string moduleGUID;
	}
}

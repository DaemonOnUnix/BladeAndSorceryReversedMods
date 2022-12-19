using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoMod.Utils
{
	// Token: 0x02000333 RID: 819
	public abstract class DMDGenerator<TSelf> : _IDMDGenerator where TSelf : DMDGenerator<TSelf>, new()
	{
		// Token: 0x060012FF RID: 4863
		protected abstract MethodInfo _Generate(DynamicMethodDefinition dmd, object context);

		// Token: 0x06001300 RID: 4864 RVA: 0x00040661 File Offset: 0x0003E861
		MethodInfo _IDMDGenerator.Generate(DynamicMethodDefinition dmd, object context)
		{
			return DMDGenerator<TSelf>._Postbuild(this._Generate(dmd, context));
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00040670 File Offset: 0x0003E870
		public static MethodInfo Generate(DynamicMethodDefinition dmd, object context = null)
		{
			TSelf tself;
			if ((tself = DMDGenerator<TSelf>._Instance) == null)
			{
				tself = (DMDGenerator<TSelf>._Instance = new TSelf());
			}
			return DMDGenerator<TSelf>._Postbuild(tself._Generate(dmd, context));
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0004069C File Offset: 0x0003E89C
		internal static MethodInfo _Postbuild(MethodInfo mi)
		{
			if (mi == null)
			{
				return null;
			}
			if (DynamicMethodDefinition._IsMono && !(mi is DynamicMethod) && mi.DeclaringType != null)
			{
				Module module = ((mi != null) ? mi.Module : null);
				if (module == null)
				{
					return mi;
				}
				Assembly assembly = module.Assembly;
				if (((assembly != null) ? assembly.GetType() : null) == null)
				{
					return mi;
				}
				assembly.SetMonoCorlibInternal(true);
			}
			return mi;
		}

		// Token: 0x04000F67 RID: 3943
		private static TSelf _Instance;
	}
}

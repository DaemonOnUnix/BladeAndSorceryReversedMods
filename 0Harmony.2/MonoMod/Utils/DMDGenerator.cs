using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoMod.Utils
{
	// Token: 0x0200042B RID: 1067
	public abstract class DMDGenerator<TSelf> : _IDMDGenerator where TSelf : DMDGenerator<TSelf>, new()
	{
		// Token: 0x06001675 RID: 5749
		protected abstract MethodInfo _Generate(DynamicMethodDefinition dmd, object context);

		// Token: 0x06001676 RID: 5750 RVA: 0x000485EF File Offset: 0x000467EF
		MethodInfo _IDMDGenerator.Generate(DynamicMethodDefinition dmd, object context)
		{
			return DMDGenerator<TSelf>._Postbuild(this._Generate(dmd, context));
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000485FE File Offset: 0x000467FE
		public static MethodInfo Generate(DynamicMethodDefinition dmd, object context = null)
		{
			TSelf tself;
			if ((tself = DMDGenerator<TSelf>._Instance) == null)
			{
				tself = (DMDGenerator<TSelf>._Instance = new TSelf());
			}
			return DMDGenerator<TSelf>._Postbuild(tself._Generate(dmd, context));
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x0004862C File Offset: 0x0004682C
		internal static MethodInfo _Postbuild(MethodInfo mi)
		{
			if (mi == null)
			{
				return null;
			}
			if (ReflectionHelper.IsMono && !(mi is DynamicMethod) && mi.DeclaringType != null)
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

		// Token: 0x04000FA5 RID: 4005
		private static TSelf _Instance;
	}
}

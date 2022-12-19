using System;

namespace MonoMod.Utils
{
	// Token: 0x0200043D RID: 1085
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	internal class DynDllImportAttribute : Attribute
	{
		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x0004C780 File Offset: 0x0004A980
		// (set) Token: 0x060016E7 RID: 5863 RVA: 0x0004C788 File Offset: 0x0004A988
		public string LibraryName { get; set; }

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x0004C791 File Offset: 0x0004A991
		// (set) Token: 0x060016E9 RID: 5865 RVA: 0x0004C799 File Offset: 0x0004A999
		public string[] EntryPoints { get; set; }

		// Token: 0x060016EA RID: 5866 RVA: 0x0004C7A2 File Offset: 0x0004A9A2
		public DynDllImportAttribute(string libraryName, params string[] entryPoints)
		{
			this.LibraryName = libraryName;
			this.EntryPoints = entryPoints;
		}
	}
}

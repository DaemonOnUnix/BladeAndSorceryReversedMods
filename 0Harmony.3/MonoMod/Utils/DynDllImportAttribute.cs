using System;

namespace MonoMod.Utils
{
	// Token: 0x02000345 RID: 837
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	internal class DynDllImportAttribute : Attribute
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x00044530 File Offset: 0x00042730
		// (set) Token: 0x06001368 RID: 4968 RVA: 0x00044538 File Offset: 0x00042738
		public string LibraryName { get; set; }

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x00044541 File Offset: 0x00042741
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x00044549 File Offset: 0x00042749
		public string[] EntryPoints { get; set; }

		// Token: 0x0600136B RID: 4971 RVA: 0x00044552 File Offset: 0x00042752
		public DynDllImportAttribute(string libraryName, params string[] entryPoints)
		{
			this.LibraryName = libraryName;
			this.EntryPoints = entryPoints;
		}
	}
}

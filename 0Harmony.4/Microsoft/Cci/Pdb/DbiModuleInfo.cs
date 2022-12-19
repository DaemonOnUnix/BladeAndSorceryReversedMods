using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020003F3 RID: 1011
	internal class DbiModuleInfo
	{
		// Token: 0x0600159B RID: 5531 RVA: 0x00043AF0 File Offset: 0x00041CF0
		internal DbiModuleInfo(BitAccess bits, bool readStrings)
		{
			bits.ReadInt32(out this.opened);
			new DbiSecCon(bits);
			bits.ReadUInt16(out this.flags);
			bits.ReadInt16(out this.stream);
			bits.ReadInt32(out this.cbSyms);
			bits.ReadInt32(out this.cbOldLines);
			bits.ReadInt32(out this.cbLines);
			bits.ReadInt16(out this.files);
			bits.ReadInt16(out this.pad1);
			bits.ReadUInt32(out this.offsets);
			bits.ReadInt32(out this.niSource);
			bits.ReadInt32(out this.niCompiler);
			if (readStrings)
			{
				bits.ReadCString(out this.moduleName);
				bits.ReadCString(out this.objectName);
			}
			else
			{
				bits.SkipCString(out this.moduleName);
				bits.SkipCString(out this.objectName);
			}
			bits.Align(4);
		}

		// Token: 0x04000F13 RID: 3859
		internal int opened;

		// Token: 0x04000F14 RID: 3860
		internal ushort flags;

		// Token: 0x04000F15 RID: 3861
		internal short stream;

		// Token: 0x04000F16 RID: 3862
		internal int cbSyms;

		// Token: 0x04000F17 RID: 3863
		internal int cbOldLines;

		// Token: 0x04000F18 RID: 3864
		internal int cbLines;

		// Token: 0x04000F19 RID: 3865
		internal short files;

		// Token: 0x04000F1A RID: 3866
		internal short pad1;

		// Token: 0x04000F1B RID: 3867
		internal uint offsets;

		// Token: 0x04000F1C RID: 3868
		internal int niSource;

		// Token: 0x04000F1D RID: 3869
		internal int niCompiler;

		// Token: 0x04000F1E RID: 3870
		internal string moduleName;

		// Token: 0x04000F1F RID: 3871
		internal string objectName;
	}
}

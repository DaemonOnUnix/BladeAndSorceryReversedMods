using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020002FD RID: 765
	internal class DbiModuleInfo
	{
		// Token: 0x0600122C RID: 4652 RVA: 0x0003BBA8 File Offset: 0x00039DA8
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

		// Token: 0x04000ED4 RID: 3796
		internal int opened;

		// Token: 0x04000ED5 RID: 3797
		internal ushort flags;

		// Token: 0x04000ED6 RID: 3798
		internal short stream;

		// Token: 0x04000ED7 RID: 3799
		internal int cbSyms;

		// Token: 0x04000ED8 RID: 3800
		internal int cbOldLines;

		// Token: 0x04000ED9 RID: 3801
		internal int cbLines;

		// Token: 0x04000EDA RID: 3802
		internal short files;

		// Token: 0x04000EDB RID: 3803
		internal short pad1;

		// Token: 0x04000EDC RID: 3804
		internal uint offsets;

		// Token: 0x04000EDD RID: 3805
		internal int niSource;

		// Token: 0x04000EDE RID: 3806
		internal int niCompiler;

		// Token: 0x04000EDF RID: 3807
		internal string moduleName;

		// Token: 0x04000EE0 RID: 3808
		internal string objectName;
	}
}

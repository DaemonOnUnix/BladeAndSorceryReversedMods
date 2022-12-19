using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
[Serializable]
public class PID
{
	// Token: 0x06000004 RID: 4 RVA: 0x00002152 File Offset: 0x00000352
	public PID(float pFactor, float iFactor, float dFactor)
	{
		this.pFactor = pFactor;
		this.iFactor = iFactor;
		this.dFactor = dFactor;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002174 File Offset: 0x00000374
	public Vector3 Update(Vector3 present, float timeFrame)
	{
		this.integral += present * timeFrame;
		Vector3 deriv = (present - this.lastError) / timeFrame;
		this.lastError = present;
		return present * this.pFactor + this.integral * this.iFactor + deriv * this.dFactor;
	}

	// Token: 0x04000001 RID: 1
	public float pFactor;

	// Token: 0x04000002 RID: 2
	public float iFactor;

	// Token: 0x04000003 RID: 3
	public float dFactor;

	// Token: 0x04000004 RID: 4
	private Vector3 integral;

	// Token: 0x04000005 RID: 5
	private Vector3 lastError;
}

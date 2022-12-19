using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class DynamicBoneColliderBase : MonoBehaviour
{
	// Token: 0x0600008B RID: 139 RVA: 0x0000635D File Offset: 0x0000455D
	public virtual bool Collide(ref Vector3 particlePosition, float particleRadius)
	{
		return false;
	}

	// Token: 0x04000063 RID: 99
	public DynamicBoneColliderBase.Direction m_Direction = DynamicBoneColliderBase.Direction.Y;

	// Token: 0x04000064 RID: 100
	public Vector3 m_Center = Vector3.zero;

	// Token: 0x04000065 RID: 101
	public DynamicBoneColliderBase.Bound m_Bound;

	// Token: 0x02000018 RID: 24
	public enum Direction
	{
		// Token: 0x04000067 RID: 103
		X,
		// Token: 0x04000068 RID: 104
		Y,
		// Token: 0x04000069 RID: 105
		Z
	}

	// Token: 0x02000019 RID: 25
	public enum Bound
	{
		// Token: 0x0400006B RID: 107
		Outside,
		// Token: 0x0400006C RID: 108
		Inside
	}
}

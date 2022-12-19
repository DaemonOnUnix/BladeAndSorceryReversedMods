using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class Laser : MonoBehaviour
{
	// Token: 0x04000001 RID: 1
	public Transform postProcessingVolumeTransform;

	// Token: 0x04000002 RID: 2
	[Space]
	public Transform LeftLaserRoot;

	// Token: 0x04000003 RID: 3
	public MeshRenderer LeftLaserMesh;

	// Token: 0x04000004 RID: 4
	public Transform RightLaserRoot;

	// Token: 0x04000005 RID: 5
	public MeshRenderer RightLaserMesh;

	// Token: 0x04000006 RID: 6
	[Space]
	public AudioSource v1Loop;

	// Token: 0x04000007 RID: 7
	public AudioSource v2Loop;

	// Token: 0x04000008 RID: 8
	public AudioSource v3Loop;

	// Token: 0x04000009 RID: 9
	[Space]
	public AudioSource LeftImpactAudio;

	// Token: 0x0400000A RID: 10
	public ParticleSystem LeftImpact;

	// Token: 0x0400000B RID: 11
	public Transform LeftImpactPoint;

	// Token: 0x0400000C RID: 12
	[Space]
	public AudioSource RightImpactAudio;

	// Token: 0x0400000D RID: 13
	public ParticleSystem RightImpact;

	// Token: 0x0400000E RID: 14
	public Transform RightImpactPoint;
}

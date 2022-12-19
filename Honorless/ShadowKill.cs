using System;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x02000003 RID: 3
public class ShadowKill : MonoBehaviour
{
	// Token: 0x06000007 RID: 7 RVA: 0x0000233E File Offset: 0x0000053E
	private void OnValidate()
	{
		this.vfx = base.GetComponent<VisualEffect>();
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000234C File Offset: 0x0000054C
	private void Start()
	{
		this.SetRenderer(this.body);
		this.vfx.Play();
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002368 File Offset: 0x00000568
	public void SetRenderer(Renderer renderer)
	{
		Mesh mesh = ((renderer is SkinnedMeshRenderer) ? (renderer as SkinnedMeshRenderer).sharedMesh : renderer.GetComponent<MeshFilter>().sharedMesh);
		if (!mesh.isReadable)
		{
			Debug.LogError("Cannot access vertices on mesh " + mesh.name + " for generating point cache (isReadable is false; Read/Write must be enabled in import settings)");
			return;
		}
		if (renderer is SkinnedMeshRenderer)
		{
			mesh = new Mesh();
			(renderer as SkinnedMeshRenderer).BakeMesh(mesh);
			this.pointCacheSkinnedMeshRenderer = renderer as SkinnedMeshRenderer;
		}
		this.pCache = PointCacheGenerator.ComputePCacheFromMesh(mesh, this.pointCacheMapSize, this.pointCachePointCount, this.pointCacheSeed, this.pointCacheDistribution, this.pointCacheBakeMode);
		this.vfx.SetTexture("PositionMap", this.pCache.positionMap);
		if (this.vfx.HasTexture("NormalMap"))
		{
			this.vfx.SetTexture("NormalMap", this.pCache.normalMap);
		}
		if (this.pointCacheSkinnedMeshUpdate)
		{
			return;
		}
		this.pCache.Dispose();
		this.pCache = null;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x0000246F File Offset: 0x0000066F
	public void LateUpdate()
	{
		if (!this.pointCacheSkinnedMeshUpdate)
		{
			return;
		}
		this.pCache.Update(this.pointCacheSkinnedMeshRenderer);
	}

	// Token: 0x04000005 RID: 5
	public float lifeTime = 5f;

	// Token: 0x04000006 RID: 6
	public bool lookAtTarget;

	// Token: 0x04000007 RID: 7
	protected PointCacheGenerator.PCache pCache;

	// Token: 0x04000008 RID: 8
	[NonSerialized]
	public float playTime;

	// Token: 0x04000009 RID: 9
	public PointCacheGenerator.MeshBakeMode pointCacheBakeMode = 1;

	// Token: 0x0400000A RID: 10
	public PointCacheGenerator.Distribution pointCacheDistribution = 2;

	// Token: 0x0400000B RID: 11
	public int pointCacheMapSize = 512;

	// Token: 0x0400000C RID: 12
	public int pointCachePointCount = 4096;

	// Token: 0x0400000D RID: 13
	public int pointCacheSeed;

	// Token: 0x0400000E RID: 14
	protected SkinnedMeshRenderer pointCacheSkinnedMeshRenderer;

	// Token: 0x0400000F RID: 15
	public bool pointCacheSkinnedMeshUpdate;

	// Token: 0x04000010 RID: 16
	public VisualEffect vfx;

	// Token: 0x04000011 RID: 17
	public Renderer body;
}

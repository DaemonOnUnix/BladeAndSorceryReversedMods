using System;
using ThunderRoad;
using UnityEngine;

namespace Wully.MoreSlots
{
	// Token: 0x02000004 RID: 4
	public class MoreSlotsHolder : Holder
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000024D5 File Offset: 0x000006D5
		protected override void Awake()
		{
			if (Loader.local.debug)
			{
				this.SetupDebug();
			}
			this.linkedContainer = Player.local.creature.container;
			base.OnLinkedContainerContentLoad();
			base.Awake();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000250A File Offset: 0x0000070A
		protected override ManagedLoops ManagedLoops
		{
			get
			{
				return this.DefineManagedLoops();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002512 File Offset: 0x00000712
		private ManagedLoops DefineManagedLoops()
		{
			if (Loader.local.debug)
			{
				return 2;
			}
			return 0;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002524 File Offset: 0x00000724
		protected override void ManagedUpdate()
		{
			if (!this.moreSlotsData.enabled)
			{
				return;
			}
			Vector3 pos = base.gameObject.transform.position;
			this.lineRendererX.SetPosition(0, pos);
			this.lineRendererX.SetPosition(1, pos + base.gameObject.transform.right * 0.2f);
			this.lineRendererY.SetPosition(0, pos);
			this.lineRendererY.SetPosition(1, pos + base.gameObject.transform.up * 0.2f);
			this.lineRendererZ.SetPosition(0, pos);
			this.lineRendererZ.SetPosition(1, pos + base.gameObject.transform.forward * 0.2f);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000025FC File Offset: 0x000007FC
		private void SetupDebug()
		{
			this.lineRendererX = this.CreateLineRenderer();
			this.lineRendererX.startColor = Color.red;
			this.lineRendererX.endColor = Color.red;
			this.lineRendererY = this.CreateLineRenderer();
			this.lineRendererY.startColor = Color.green;
			this.lineRendererY.endColor = Color.green;
			this.lineRendererZ = this.CreateLineRenderer();
			this.lineRendererZ.startColor = Color.blue;
			this.lineRendererZ.endColor = Color.blue;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002690 File Offset: 0x00000890
		private LineRenderer CreateLineRenderer()
		{
			LineRenderer lineRenderer = new GameObject("line")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<LineRenderer>();
			lineRenderer.startWidth = 0.01f;
			lineRenderer.endWidth = 0.01f;
			lineRenderer.textureMode = 1;
			lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			return lineRenderer;
		}

		// Token: 0x0400000D RID: 13
		public MoreSlotsData moreSlotsData;

		// Token: 0x0400000E RID: 14
		public RagdollPart part;

		// Token: 0x0400000F RID: 15
		private LineRenderer lineRendererX;

		// Token: 0x04000010 RID: 16
		private LineRenderer lineRendererY;

		// Token: 0x04000011 RID: 17
		private LineRenderer lineRendererZ;
	}
}

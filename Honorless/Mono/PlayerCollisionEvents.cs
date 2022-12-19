using System;
using ThunderRoad;
using UnityEngine;

namespace Wully.Mono
{
	// Token: 0x0200000C RID: 12
	public class PlayerCollisionEvents : MonoBehaviour
	{
		// Token: 0x06000050 RID: 80 RVA: 0x000047CB File Offset: 0x000029CB
		private void Awake()
		{
			this.player = Player.local;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000047D8 File Offset: 0x000029D8
		private void OnCollisionEnter(Collision collision)
		{
			if (((1 << collision.collider.gameObject.layer) & this.layerMask) != 0)
			{
				PlayerCollisionEvents.CollideEvent onCollideWithWorldEvent = PlayerCollisionEvents.OnCollideWithWorldEvent;
				if (onCollideWithWorldEvent == null)
				{
					return;
				}
				onCollideWithWorldEvent();
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000052 RID: 82 RVA: 0x00004808 File Offset: 0x00002A08
		// (remove) Token: 0x06000053 RID: 83 RVA: 0x0000483C File Offset: 0x00002A3C
		public static event PlayerCollisionEvents.CollideEvent OnCollideWithWorldEvent;

		// Token: 0x0400006F RID: 111
		private Player player;

		// Token: 0x04000070 RID: 112
		private int layerMask;

		// Token: 0x02000015 RID: 21
		// (Invoke) Token: 0x0600007C RID: 124
		public delegate void CollideEvent();
	}
}

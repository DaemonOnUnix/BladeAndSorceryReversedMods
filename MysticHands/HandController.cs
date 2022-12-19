using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace MysticHands
{
	// Token: 0x0200000A RID: 10
	public class HandController : MonoBehaviour
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00007914 File Offset: 0x00005B14
		public void Init()
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007917 File Offset: 0x00005B17
		public void SwitchMode(Side side, bool claw)
		{
			this.hands[side].Toggle(false);
			this.hands[side].Toggle(claw);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007940 File Offset: 0x00005B40
		public void Toggle(Side side, bool claw)
		{
			this.hands[side].Toggle(claw);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007958 File Offset: 0x00005B58
		public void Start()
		{
			this.hands[1] = new Hand(1, true);
			this.hands[0] = new Hand(0, true);
			this.hands[1].otherHand = this.hands[0];
			this.hands[0].otherHand = this.hands[1];
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000079C8 File Offset: 0x00005BC8
		public void Update()
		{
			this.hands[1].Update();
			this.hands[0].Update();
		}

		// Token: 0x04000025 RID: 37
		public Dictionary<Side, Hand> hands = new Dictionary<Side, Hand>();
	}
}

using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000021 RID: 33
	internal class WaddiwassiPerItem : MonoBehaviour
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0000758C File Offset: 0x0000578C
		private void Start()
		{
			this.item = base.GetComponent<Item>();
			this.cantWaddiwassi = false;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000075A4 File Offset: 0x000057A4
		private void Update()
		{
			bool flag = !this.cantWaddiwassi;
			if (flag)
			{
				Vector3 targetPos;
				targetPos..ctor(this.target.transform.position.x, this.target.transform.position.y + 0.3f, this.target.transform.position.z);
				Vector3 direction = targetPos - this.item.transform.position;
				this.item.rb.AddForce(direction * (10f * this.item.rb.mass), 1);
				this.cantWaddiwassi = true;
				Object.Destroy(this);
			}
		}

		// Token: 0x040000F4 RID: 244
		private bool cantWaddiwassi;

		// Token: 0x040000F5 RID: 245
		private Item item;

		// Token: 0x040000F6 RID: 246
		internal Creature target;

		// Token: 0x040000F7 RID: 247
		internal Item shootItem;
	}
}

using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000018 RID: 24
	public class Levicorpus : MonoBehaviour
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00003FE3 File Offset: 0x000021E3
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003FF4 File Offset: 0x000021F4
		public void OnCollisionEnter(Collision c)
		{
			bool flag = c.gameObject.GetComponentInParent<Creature>() != null;
			if (flag)
			{
				bool flag2 = this.spawnerWeapon.GetComponent<VoiceWeaponComponent>() != null;
				if (flag2)
				{
					this.spawnerWeapon.GetComponent<VoiceWeaponComponent>().hitByLevicorpus.Add(c.gameObject.GetComponentInParent<Creature>());
				}
				this.floater1 = new GameObject();
				this.floater1.AddComponent<Rigidbody>();
				this.floater1.GetComponent<Rigidbody>().useGravity = false;
				this.floater2 = new GameObject();
				this.floater2.AddComponent<Rigidbody>();
				this.floater2.GetComponent<Rigidbody>().useGravity = false;
				this.creature = c.gameObject.GetComponentInParent<Creature>();
				this.creature.ragdoll.SetState(1);
				this.creature.footLeft.gameObject.AddComponent<SpringJoint>();
				this.creature.footRight.gameObject.AddComponent<SpringJoint>();
				Debug.Log("Floater 1 pre: " + this.floater1.transform.position.ToString());
				Debug.Log("Floater 2 pre: " + this.floater2.transform.position.ToString());
				this.floater1.transform.position = new Vector3(this.creature.ragdoll.headPart.transform.position.x, this.creature.ragdoll.headPart.transform.position.y + 2f, this.creature.ragdoll.headPart.transform.position.z);
				this.floater2.transform.position = new Vector3(this.creature.ragdoll.headPart.transform.position.x, this.creature.ragdoll.headPart.transform.position.y + 2f, this.creature.ragdoll.headPart.transform.position.z);
				Debug.Log("Floater 1 post: " + this.floater1.transform.position.ToString());
				Debug.Log("Floater 2 post: " + this.floater2.transform.position.ToString());
				Debug.Log("footLeft transform: " + this.creature.footLeft.transform.position.ToString());
				Debug.Log("footLeft transform joint: " + this.creature.footLeft.gameObject.GetComponent<SpringJoint>().transform.position.ToString());
				this.creature.footLeft.gameObject.GetComponent<SpringJoint>().connectedBody = this.floater1.GetComponent<Rigidbody>();
				this.creature.footLeft.gameObject.GetComponent<SpringJoint>().autoConfigureConnectedAnchor = false;
				this.creature.footLeft.gameObject.GetComponent<SpringJoint>().connectedAnchor = new Vector3(0f, 0f, 0f);
				Debug.Log("Creature connected Anchor: " + this.creature.footLeft.gameObject.GetComponent<SpringJoint>().connectedAnchor.ToString());
				this.creature.footLeft.gameObject.GetComponent<SpringJoint>().spring = 3000f;
				this.creature.footLeft.gameObject.GetComponent<SpringJoint>().damper = 100f;
				this.creature.footRight.gameObject.GetComponent<SpringJoint>().connectedBody = this.floater2.GetComponent<Rigidbody>();
				this.creature.footRight.gameObject.GetComponent<SpringJoint>().autoConfigureConnectedAnchor = false;
				this.creature.footRight.gameObject.GetComponent<SpringJoint>().connectedAnchor = new Vector3(0f, 0f, 0f);
				Debug.Log("Creature connected Anchor: " + this.creature.footRight.gameObject.GetComponent<SpringJoint>().connectedAnchor.ToString());
				this.creature.footRight.gameObject.GetComponent<SpringJoint>().spring = 3000f;
				this.creature.footRight.gameObject.GetComponent<SpringJoint>().damper = 100f;
				this.floater1.AddComponent<FixedJoint>();
				this.floater2.AddComponent<FixedJoint>();
			}
		}

		// Token: 0x04000064 RID: 100
		private Item item;

		// Token: 0x04000065 RID: 101
		private Item npcItem;

		// Token: 0x04000066 RID: 102
		private GameObject floater1;

		// Token: 0x04000067 RID: 103
		private GameObject floater2;

		// Token: 0x04000068 RID: 104
		private SpringJoint joint;

		// Token: 0x04000069 RID: 105
		internal Creature creature;

		// Token: 0x0400006A RID: 106
		internal Item spawnerWeapon;
	}
}

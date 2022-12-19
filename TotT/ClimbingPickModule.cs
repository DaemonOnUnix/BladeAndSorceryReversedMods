using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200001E RID: 30
	public class ClimbingPickModule : MonoBehaviour
	{
		// Token: 0x060000DC RID: 220 RVA: 0x0000728C File Offset: 0x0000548C
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.anim = base.GetComponent<Animator>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldAction);
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.OnCollisionStart);
			this.item.OnSnapEvent += new Item.HolderDelegate(this.Item_OnSnap);
			this.BaseCollider = this.item.GetCustomReference("BaseColliders", true).gameObject;
			this.Mode1Collider1 = this.item.GetCustomReference("Mode1Collider1", true).gameObject;
			this.Mode1Collider2 = this.item.GetCustomReference("Mode1Collider2", true).gameObject;
			this.Mode2Collider1 = this.item.GetCustomReference("Mode2Collider1", true).gameObject;
			this.Mode2Collider2 = this.item.GetCustomReference("Mode2Collider2", true).gameObject;
			this.ExamplePierceDamager = this.item.GetCustomReference("ExamplePierceDamager", true).gameObject.GetComponent<Damager>();
			this.baseDamager = this.ExamplePierceDamager.data;
			this.stuckDamager = this.baseDamager;
			this.stuckDamager.penetrationDamper = 1000000f;
			this.stuckDamager.penetrationHeldDamperIn = 1000000f;
			this.stuckDamager.penetrationHeldDamperOut = 1000000f;
			this.stuckDamager.penetrationSlideDamper = 1000000f;
			this.og_DamperOut = this.baseDamager.penetrationTempModifierDamperOut;
			this.Mode1Collider1.SetActive(false);
			this.Mode1Collider2.SetActive(false);
			this.Mode2Collider1.SetActive(false);
			this.Mode2Collider2.SetActive(false);
			this.open = false;
			this.Mode1 = true;
			this.canInteract = true;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00007464 File Offset: 0x00005664
		private void Item_OnSnap(Holder holder)
		{
			bool flag = this.open;
			if (flag)
			{
				this.UnHook();
				base.StartCoroutine(this.CloseUp());
				this.item.rb.isKinematic = false;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000074A4 File Offset: 0x000056A4
		private void OnCollisionStart(CollisionInstance collisionInstance)
		{
			this.lastHitCollider = collisionInstance.targetCollider;
			RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
			Creature creature;
			if (hitRagdollPart == null)
			{
				creature = null;
			}
			else
			{
				Ragdoll ragdoll = hitRagdollPart.ragdoll;
				creature = ((ragdoll != null) ? ragdoll.creature : null);
			}
			Creature target = creature;
			bool flag = target != null && target.brain.state != 5 && !this.open;
			if (flag)
			{
				bool flag2 = collisionInstance.damageStruct.hitRagdollPart == target.ragdoll.headPart && collisionInstance.damageStruct.damageType == 3;
				if (flag2)
				{
					KnockOutBehaviour temp = target.gameObject.AddComponent<KnockOutBehaviour>();
					temp.Setup(this.KnockOutMinutes);
				}
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00007558 File Offset: 0x00005758
		private void Item_OnHeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = this.canInteract;
			if (flag)
			{
				bool flag2 = action == 2 && !this.open;
				if (flag2)
				{
					this.VibrateHand();
					base.StartCoroutine(this.OpenUp());
				}
				else
				{
					bool flag3 = this.open;
					if (flag3)
					{
						bool flag4 = action == 2 && !this.firstTap;
						if (flag4)
						{
							this.firstTap = true;
							this.Timer = true;
						}
						else
						{
							bool flag5 = action == 3 && this.firstTap;
							if (flag5)
							{
								this.Tapped = true;
								this.tapTimer = 0f;
							}
						}
					}
				}
			}
			bool flag6 = action == 0;
			if (flag6)
			{
				bool flag7 = this.item.isPenetrating && this.item.GetComponentInParent<Creature>() == null;
				if (flag7)
				{
					this.item.rb.isKinematic = true;
				}
			}
			else
			{
				bool flag8 = action == 1;
				if (flag8)
				{
					this.item.rb.isKinematic = false;
				}
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007664 File Offset: 0x00005864
		public void UnHook()
		{
			this.item.rb.isKinematic = false;
			foreach (Damager d in this.item.GetComponentsInChildren<Damager>())
			{
				d.UnPenetrateAll();
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000076AC File Offset: 0x000058AC
		public void UpdateDamagers(DamagerData data)
		{
			foreach (Damager d in this.item.gameObject.GetComponentsInChildren<Damager>())
			{
				bool flag = d.direction == 1;
				if (flag)
				{
					d.Load(data, d.collisionHandler);
				}
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007700 File Offset: 0x00005900
		public void UpdateDampers(float DamperOut)
		{
			foreach (Damager d in this.item.gameObject.GetComponentsInChildren<Damager>())
			{
				bool flag = d.direction == 1;
				if (flag)
				{
					d.data.penetrationTempModifierDamperOut = DamperOut;
				}
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000774F File Offset: 0x0000594F
		public IEnumerator OpenUp()
		{
			this.canInteract = false;
			bool mode = this.Mode1;
			if (mode)
			{
				this.anim.SetTrigger("Open1");
			}
			else
			{
				this.anim.SetTrigger("Open2");
			}
			this.UnHook();
			this.BaseCollider.SetActive(false);
			yield return new WaitForSeconds(0.25f);
			bool mode2 = this.Mode1;
			if (mode2)
			{
				this.Mode1Collider1.SetActive(true);
				this.Mode1Collider2.SetActive(true);
			}
			else
			{
				this.Mode2Collider1.SetActive(true);
				this.Mode2Collider2.SetActive(true);
			}
			this.open = true;
			this.canInteract = true;
			yield break;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000775E File Offset: 0x0000595E
		public IEnumerator CloseUp()
		{
			this.canInteract = false;
			this.anim.SetTrigger("Close");
			this.item.rb.isKinematic = false;
			bool mode = this.Mode1;
			if (mode)
			{
				this.Mode1Collider1.SetActive(false);
				this.Mode1Collider2.SetActive(false);
			}
			else
			{
				this.Mode2Collider1.SetActive(false);
				this.Mode2Collider2.SetActive(false);
			}
			this.UnHook();
			yield return new WaitForSeconds(0.25f);
			this.BaseCollider.SetActive(true);
			this.open = false;
			this.canInteract = true;
			yield break;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000776D File Offset: 0x0000596D
		public IEnumerator SwitchMode()
		{
			this.canInteract = false;
			this.anim.SetTrigger("Switch");
			this.UnHook();
			bool mode = this.Mode1;
			if (mode)
			{
				this.Mode1Collider1.SetActive(false);
				this.Mode1Collider2.SetActive(false);
			}
			else
			{
				this.Mode2Collider1.SetActive(false);
				this.Mode2Collider2.SetActive(false);
			}
			yield return new WaitForSeconds(0.16666667f);
			this.Mode1 = !this.Mode1;
			bool mode2 = this.Mode1;
			if (mode2)
			{
				this.Mode1Collider1.SetActive(true);
				this.Mode1Collider2.SetActive(true);
			}
			else
			{
				this.Mode2Collider1.SetActive(true);
				this.Mode2Collider2.SetActive(true);
			}
			this.canInteract = true;
			yield break;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000777C File Offset: 0x0000597C
		public void Update()
		{
			this.TimerFunction();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007788 File Offset: 0x00005988
		public void VibrateHand()
		{
			bool flag = this.item.mainHandler != null;
			if (flag)
			{
				this.item.mainHandler.playerHand.controlHand.HapticShort(5f);
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000077D0 File Offset: 0x000059D0
		public void TimerFunction()
		{
			bool timer = this.Timer;
			if (timer)
			{
				bool flag = this.tapTimer >= 0f;
				if (flag)
				{
					this.tapTimer -= Time.deltaTime;
				}
				else
				{
					this.Timer = false;
					this.tapTimer = ClimbingPickModule.tapTimerMax;
					bool tapped = this.Tapped;
					if (tapped)
					{
						this.VibrateHand();
						base.StartCoroutine(this.CloseUp());
					}
					else
					{
						this.VibrateHand();
						base.StartCoroutine(this.SwitchMode());
					}
					this.firstTap = false;
					this.Tapped = false;
				}
			}
		}

		// Token: 0x0400009C RID: 156
		private Item item;

		// Token: 0x0400009D RID: 157
		private Animator anim;

		// Token: 0x0400009E RID: 158
		private GameObject BaseCollider;

		// Token: 0x0400009F RID: 159
		private GameObject Mode1Collider1;

		// Token: 0x040000A0 RID: 160
		private GameObject Mode1Collider2;

		// Token: 0x040000A1 RID: 161
		private GameObject Mode2Collider1;

		// Token: 0x040000A2 RID: 162
		private GameObject Mode2Collider2;

		// Token: 0x040000A3 RID: 163
		private Damager ExamplePierceDamager;

		// Token: 0x040000A4 RID: 164
		private Collider lastHitCollider;

		// Token: 0x040000A5 RID: 165
		private DamagerData baseDamager;

		// Token: 0x040000A6 RID: 166
		private DamagerData stuckDamager;

		// Token: 0x040000A7 RID: 167
		private float og_DamperOut;

		// Token: 0x040000A8 RID: 168
		private bool open;

		// Token: 0x040000A9 RID: 169
		private bool Mode1;

		// Token: 0x040000AA RID: 170
		private bool canInteract;

		// Token: 0x040000AB RID: 171
		private bool firstTap;

		// Token: 0x040000AC RID: 172
		private bool Tapped;

		// Token: 0x040000AD RID: 173
		private bool Timer;

		// Token: 0x040000AE RID: 174
		private static float tapTimerMax = ClimbingPickParser.TapDelay;

		// Token: 0x040000AF RID: 175
		private float tapTimer = ClimbingPickModule.tapTimerMax;

		// Token: 0x040000B0 RID: 176
		private float KnockOutMinutes = ClimbingPickParser.KnockOutMinutes;
	}
}

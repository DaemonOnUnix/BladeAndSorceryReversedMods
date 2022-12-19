using System;
using System.Collections;
using ThunderRoad;
using TotT;
using UnityEngine;

namespace WristFlamethrower
{
	// Token: 0x02000003 RID: 3
	public class FlameMono : ArmModule
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		public override void OnStart()
		{
			this.spawnPos = this.item.GetCustomReference("SpawnPos", true).gameObject;
			this.fireEffect = Catalog.GetData<EffectData>("ImbueFireRagdoll", true);
			this.useDeactivate = true;
			this.HasAltMode = false;
			this.OnOff = true;
			this.item.mainCollisionHandler.OnTriggerEnterEvent += new CollisionHandler.TriggerEvent(this.MainCollisionHandler_OnTriggerEnterEvent);
			this.item.OnUnSnapEvent += new Item.HolderDelegate(this.Item_OnUnSnapEvent);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F8 File Offset: 0x000002F8
		private void Item_OnUnSnapEvent(Holder holder)
		{
			Object.Destroy(this.item.gameObject.GetComponent<ModuleHandWatcher>());
			bool flag = this.handWatcher != null;
			if (flag)
			{
				Debug.Log("Hand watcher deleted");
				this.handWatcher.Delete();
				this.handWatcher = null;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000214C File Offset: 0x0000034C
		private void MainCollisionHandler_OnTriggerEnterEvent(Collider other)
		{
			Rigidbody attachedRigidbody = other.attachedRigidbody;
			bool flag = other.attachedRigidbody;
			if (flag)
			{
				Debug.Log("trigger rb");
				Creature creature = other.GetComponentInParent<Creature>();
				bool flag2 = creature != null;
				if (flag2)
				{
					Debug.Log("trigger creature");
					bool flag3 = !creature.isPlayer;
					if (flag3)
					{
						Creature componentInParent = other.GetComponentInParent<Creature>();
						componentInParent.TryElectrocute(20f, 5f, true, true, this.fireEffect);
						componentInParent.ragdoll.SetState(1);
						bool flag4 = componentInParent != componentInParent.isPlayer && attachedRigidbody != null;
						if (flag4)
						{
							attachedRigidbody.AddForce(-componentInParent.transform.forward * 7f * other.attachedRigidbody.mass, 1);
							bool flag5 = !componentInParent.isKilled;
							if (flag5)
							{
								componentInParent.currentHealth -= 0.015f;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000225D File Offset: 0x0000045D
		public override void On()
		{
			this.OnOff = true;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002267 File Offset: 0x00000467
		public override void Off()
		{
			this.OnOff = false;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002271 File Offset: 0x00000471
		public override void Activate()
		{
			Catalog.LoadAssetAsync<GameObject>("FlameWrist", delegate(GameObject flameObject)
			{
				this.obj = Object.Instantiate<GameObject>(flameObject, null);
				this.obj.transform.position = this.spawnPos.transform.position;
				this.obj.transform.rotation = this.spawnPos.transform.rotation;
				this.obj.AddComponent<FixedJoint>().connectedBody = this.item.rb;
				this.obj.AddComponent<FireCollision>();
				foreach (ParticleSystem system in this.obj.GetComponentsInChildren<ParticleSystem>())
				{
					system.gameObject.AddComponent<FireCollision>();
				}
			}, "WristFlamethrower");
			this.Activated = true;
			base.Activate();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000229E File Offset: 0x0000049E
		public override void Deactivate()
		{
			Debug.Log("deactivate working");
			base.StartCoroutine(this.Stop());
			this.Activated = false;
			base.Deactivate();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022C7 File Offset: 0x000004C7
		public IEnumerator Stop()
		{
			this.obj.GetComponent<ParticleSystem>().Stop();
			this.obj.GetComponent<AudioSource>().Stop();
			this.obj.GetComponentInChildren<Light>().enabled = false;
			foreach (ParticleSystem partSys in this.obj.GetComponentsInChildren<ParticleSystem>())
			{
				partSys.Stop();
				partSys = null;
			}
			ParticleSystem[] array = null;
			yield return new WaitForSeconds(0.5f);
			Object.Destroy(this.obj);
			yield break;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022D8 File Offset: 0x000004D8
		public override void OnSnapEvent(Holder holder)
		{
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID";
			if (flag)
			{
				this.Hand = Player.currentCreature.handRight;
				this.handWatcher = this.item.gameObject.AddComponent<ModuleHandWatcher>();
				this.handWatcher.Setup(this.Hand, this);
			}
			else
			{
				bool flag2 = holder.data.id == "GrooveSlinger.LeftWristHolderID";
				if (flag2)
				{
					this.Hand = Player.currentCreature.handLeft;
					this.handWatcher = this.item.gameObject.AddComponent<ModuleHandWatcher>();
					this.handWatcher.Setup(this.Hand, this);
				}
				else
				{
					this.Mount = holder.parentItem.GetComponent<RailMountsMono>();
					this.Hand = this.Mount.GetHand();
				}
			}
			base.OnSnapEvent(holder);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023C0 File Offset: 0x000005C0
		public override void OnUnSnapEvent(Holder holder)
		{
			base.OnUnSnapEvent(holder);
			Object.Destroy(this.item.gameObject.GetComponent<ModuleHandWatcher>());
			bool flag = !(this.handWatcher != null);
			if (!flag)
			{
				this.handWatcher.Delete();
				this.handWatcher = null;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002414 File Offset: 0x00000614
		public override void giveHand(RagdollHand hand)
		{
			this.Hand = hand;
			base.giveHand(hand);
		}

		// Token: 0x04000001 RID: 1
		public GameObject spawnPos;

		// Token: 0x04000002 RID: 2
		public static GameObject triggerCol;

		// Token: 0x04000003 RID: 3
		public GameObject obj;

		// Token: 0x04000004 RID: 4
		private EffectData fireEffect;
	}
}

using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x0200000E RID: 14
	public class SheathComponent : MonoBehaviour
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00003E08 File Offset: 0x00002008
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.handleColliders = this.item.GetCustomReference("HandleColliders", true).gameObject;
			this.item.GetComponentInChildren<Holder>().Snapped += new Holder.HolderDelegate(this.SheathComponent_Snapped);
			this.item.GetComponentInChildren<Holder>().UnSnapped += new Holder.HolderDelegate(this.SheathComponent_UnSnapped);
			this.item.data.category = "Utilities";
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003EA5 File Offset: 0x000020A5
		private void SheathComponent_UnSnapped(Item item)
		{
			this.handleColliders.SetActive(false);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003EB5 File Offset: 0x000020B5
		private void SheathComponent_Snapped(Item item)
		{
			this.handleColliders.SetActive(true);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003EC8 File Offset: 0x000020C8
		public void FixedUpdate()
		{
			bool flag = this.holding && !this.firing;
			if (flag)
			{
				bool flag2 = Time.time - this.cdH >= 0.75f;
				if (flag2)
				{
					base.StartCoroutine(this.ShootMultiDaggers());
					this.firing = true;
				}
			}
			else
			{
				this.cdH = Time.time;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003F30 File Offset: 0x00002130
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = (!this.SwapButtons && action == 2) || (this.SwapButtons && action == 0);
			if (flag)
			{
				base.StopCoroutine(this.Dash());
				base.StartCoroutine(this.Dash());
			}
			bool flag2 = (!this.SwapButtons && action == null) || (this.SwapButtons && action == 2);
			if (flag2)
			{
				Vector3 vector;
				vector.x = Random.Range(-0.15f, 0.15f);
				vector.y = Random.Range(-0.15f, 0.15f);
				vector.z = Random.Range(-0.15f, 0.15f);
				Catalog.GetData<ItemData>("MirageBlade", true).SpawnAsync(new Action<Item>(this.ShootDagger), new Vector3?(new Vector3(Player.local.head.cam.transform.position.x + vector.x, Player.local.head.cam.transform.position.y + vector.y, Player.local.head.cam.transform.position.z + vector.z)), new Quaternion?(Player.local.head.cam.transform.rotation), null, true, null);
				GameObject gameObject = new GameObject();
				gameObject.transform.position = new Vector3(Player.local.head.cam.transform.position.x + vector.x, Player.local.head.cam.transform.position.y + vector.y, Player.local.head.cam.transform.position.z + vector.z);
				gameObject.transform.rotation = Quaternion.identity;
				EffectInstance effectInstance = Catalog.GetData<EffectData>("MirageBladeSpawn", true).Spawn(gameObject.transform, false, null, false, Array.Empty<Type>());
				effectInstance.SetIntensity(1f);
				effectInstance.Play(0, false);
				Object.Destroy(gameObject, 2f);
				this.holding = true;
			}
			bool flag3 = (!this.SwapButtons && action == 1) || (this.SwapButtons && action == 3);
			if (flag3)
			{
				this.holding = false;
				this.firing = false;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000041B0 File Offset: 0x000023B0
		public IEnumerator Dash()
		{
			bool stopOnStart = this.StopOnStart;
			if (stopOnStart)
			{
				Player.local.locomotion.rb.velocity = Vector3.zero;
			}
			bool flag = Player.local.locomotion.moveDirection.magnitude <= 0f || !this.ThumbstickDash;
			if (flag)
			{
				bool flag2 = this.DashDirection == "Item";
				if (flag2)
				{
					Player.local.locomotion.rb.AddForce(this.item.mainHandler.grip.up * this.DashSpeed, 1);
				}
				else
				{
					Player.local.locomotion.rb.AddForce(Player.local.head.transform.forward * this.DashSpeed, 1);
				}
			}
			else
			{
				Player.local.locomotion.rb.AddForce(Player.local.locomotion.moveDirection.normalized * this.DashSpeed, 1);
			}
			bool disableGravity = this.DisableGravity;
			if (disableGravity)
			{
				Player.local.locomotion.rb.useGravity = false;
			}
			bool disableCollision = this.DisableCollision;
			if (disableCollision)
			{
				Player.local.locomotion.rb.detectCollisions = false;
				this.item.rb.detectCollisions = false;
				this.item.mainHandler.rb.detectCollisions = false;
				this.item.mainHandler.otherHand.rb.detectCollisions = false;
			}
			yield return new WaitForSeconds(this.DashTime);
			bool disableGravity2 = this.DisableGravity;
			if (disableGravity2)
			{
				Player.local.locomotion.rb.useGravity = true;
			}
			bool disableCollision2 = this.DisableCollision;
			if (disableCollision2)
			{
				Player.local.locomotion.rb.detectCollisions = true;
				this.item.rb.detectCollisions = true;
				this.item.mainHandler.rb.detectCollisions = true;
				this.item.mainHandler.otherHand.rb.detectCollisions = true;
			}
			bool stopOnEnd = this.StopOnEnd;
			if (stopOnEnd)
			{
				Player.local.locomotion.rb.velocity = Vector3.zero;
			}
			yield break;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000041BF File Offset: 0x000023BF
		public IEnumerator ShootMultiDaggers()
		{
			GameObject effect = new GameObject();
			effect.transform.position = Player.local.head.cam.transform.position;
			effect.transform.rotation = Quaternion.identity;
			EffectInstance instance = Catalog.GetData<EffectData>("MirageBladeSpawn", true).Spawn(effect.transform, false, null, false, Array.Empty<Type>());
			instance.SetIntensity(1f);
			instance.Play(0, false);
			Object.Destroy(effect, 2f);
			int num;
			for (int i = 0; i < 8; i = num + 1)
			{
				Vector3 v;
				v.x = Random.Range(-0.15f, 0.15f);
				v.y = Random.Range(-0.15f, 0.15f);
				v.z = Random.Range(-0.15f, 0.15f);
				Catalog.GetData<ItemData>("MirageBlade", true).SpawnAsync(new Action<Item>(this.ShootDagger), new Vector3?(new Vector3(Player.local.head.cam.transform.position.x + v.x, Player.local.head.cam.transform.position.y + v.y, Player.local.head.cam.transform.position.z + v.z)), new Quaternion?(Player.local.head.cam.transform.rotation), null, true, null);
				yield return new WaitForSeconds(0.07f);
				v = default(Vector3);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000041D0 File Offset: 0x000023D0
		public void ShootDagger(Item spawnedItem)
		{
			Creature enemy = this.GetEnemy();
			Transform transform;
			if (enemy == null)
			{
				transform = null;
			}
			else
			{
				Ragdoll ragdoll = enemy.ragdoll;
				if (ragdoll == null)
				{
					transform = null;
				}
				else
				{
					RagdollPart targetPart = ragdoll.targetPart;
					transform = ((targetPart != null) ? targetPart.transform : null);
				}
			}
			Transform transform2 = transform;
			spawnedItem.rb.useGravity = false;
			spawnedItem.rb.drag = 0f;
			bool flag = transform2 != null;
			if (flag)
			{
				spawnedItem.rb.AddForce((transform2.position - spawnedItem.transform.position).normalized * 45f, 1);
			}
			else
			{
				spawnedItem.rb.AddForce(Player.local.head.transform.forward * 45f, 1);
			}
			spawnedItem.RefreshCollision(true);
			spawnedItem.IgnoreRagdollCollision(Player.currentCreature.ragdoll);
			spawnedItem.IgnoreObjectCollision(this.item);
			spawnedItem.gameObject.AddComponent<DaggerDespawn>();
			spawnedItem.Throw(1f, 1);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000042D4 File Offset: 0x000024D4
		public Creature GetEnemy()
		{
			Creature creature = null;
			bool flag = Creature.allActive.Count <= 0;
			Creature creature2;
			if (flag)
			{
				creature2 = null;
			}
			else
			{
				foreach (Creature creature3 in Creature.allActive)
				{
					bool flag2 = creature3 != null && !creature3.isPlayer && creature3.ragdoll.isActiveAndEnabled && !creature3.isKilled && Vector3.Dot(Player.local.head.cam.transform.forward.normalized, (creature3.transform.position - Player.local.transform.position).normalized) >= 0.9f && creature == null && Vector3.Distance(Player.local.transform.position, creature3.transform.position) <= 25f;
					if (flag2)
					{
						creature = creature3;
					}
					else
					{
						bool flag3 = creature3 != null && !creature3.isPlayer && creature3.ragdoll.isActiveAndEnabled && !creature3.isKilled && Vector3.Dot(Player.local.head.cam.transform.forward.normalized, (creature3.transform.position - Player.local.transform.position).normalized) >= 0.9f && creature != null && Vector3.Distance(Player.local.transform.position, creature3.transform.position) <= 25f;
						if (flag3)
						{
							bool flag4 = Vector3.Dot(Player.local.head.cam.transform.forward, creature3.transform.position - Player.local.transform.position) > Vector3.Dot(Player.local.head.cam.transform.forward, creature.transform.position - Player.local.transform.position);
							if (flag4)
							{
								creature = creature3;
							}
						}
					}
				}
				creature2 = creature;
			}
			return creature2;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004580 File Offset: 0x00002780
		public void Setup(float speed, string direction, bool gravity, bool collision, float time, bool stop, bool start, bool thumbstick, bool swap)
		{
			this.DashSpeed = speed;
			this.DashDirection = direction;
			this.DisableGravity = gravity;
			this.DisableCollision = collision;
			this.DashTime = time;
			bool flag = direction.ToLower().Contains("player") || direction.ToLower().Contains("head") || direction.ToLower().Contains("sight");
			if (flag)
			{
				this.DashDirection = "Player";
			}
			else
			{
				bool flag2 = direction.ToLower().Contains("item") || direction.ToLower().Contains("sheath") || direction.ToLower().Contains("flyref") || direction.ToLower().Contains("weapon");
				if (flag2)
				{
					this.DashDirection = "Item";
				}
			}
			this.StopOnEnd = stop;
			this.StopOnStart = start;
			this.ThumbstickDash = thumbstick;
			this.SwapButtons = swap;
		}

		// Token: 0x04000062 RID: 98
		private Item item;

		// Token: 0x04000063 RID: 99
		public float DashSpeed;

		// Token: 0x04000064 RID: 100
		public string DashDirection;

		// Token: 0x04000065 RID: 101
		public bool DisableGravity;

		// Token: 0x04000066 RID: 102
		public bool DisableCollision;

		// Token: 0x04000067 RID: 103
		public float DashTime;

		// Token: 0x04000068 RID: 104
		public bool StopOnEnd;

		// Token: 0x04000069 RID: 105
		public bool StopOnStart;

		// Token: 0x0400006A RID: 106
		private bool ThumbstickDash;

		// Token: 0x0400006B RID: 107
		private bool SwapButtons;

		// Token: 0x0400006C RID: 108
		private bool holding = false;

		// Token: 0x0400006D RID: 109
		private bool firing = false;

		// Token: 0x0400006E RID: 110
		private float cdH;

		// Token: 0x0400006F RID: 111
		private GameObject handleColliders;
	}
}

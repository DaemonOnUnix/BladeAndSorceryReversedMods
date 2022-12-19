using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x02000008 RID: 8
	public class YamatoComponent : MonoBehaviour
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000023B8 File Offset: 0x000005B8
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnSnapEvent += new Item.HolderDelegate(this.Item_OnSnapEvent);
			this.item.OnUnSnapEvent += new Item.HolderDelegate(this.Item_OnUnSnapEvent);
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.Item_OnUngrabEvent);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGrabEvent);
			this.item.OnTelekinesisReleaseEvent += new Item.TelekinesisDelegate(this.Item_OnTelekinesisReleaseEvent);
			this.item.OnTelekinesisGrabEvent += new Item.TelekinesisDelegate(this.Item_OnTelekinesisGrabEvent);
			Transform customReference = this.item.GetCustomReference("Pierce", true);
			Damager damager;
			if (customReference == null)
			{
				damager = null;
			}
			else
			{
				GameObject gameObject = customReference.gameObject;
				damager = ((gameObject != null) ? gameObject.GetComponent<Damager>() : null);
			}
			this.pierce = damager;
			Transform customReference2 = this.item.GetCustomReference("Slash", true);
			Damager damager2;
			if (customReference2 == null)
			{
				damager2 = null;
			}
			else
			{
				GameObject gameObject2 = customReference2.gameObject;
				damager2 = ((gameObject2 != null) ? gameObject2.GetComponent<Damager>() : null);
			}
			this.slash = damager2;
			Transform customReference3 = this.item.GetCustomReference("Blades", true);
			this.blades = ((customReference3 != null) ? customReference3.gameObject : null);
			Transform customReference4 = this.item.GetCustomReference("Triggers", true);
			this.triggers = ((customReference4 != null) ? customReference4.gameObject : null);
			GameObject gameObject3 = this.triggers;
			if (gameObject3 != null)
			{
				gameObject3.SetActive(false);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002531 File Offset: 0x00000731
		public void Setup(float speed, float cd, bool swap, bool stop, bool toggle, bool toggleBeam, bool swapJC, bool noJudgementCut)
		{
			this.swordSpeed = speed;
			this.cooldown = cd;
			this.swapButtons = swap;
			this.stopOnJC = stop;
			this.toggleAnimeSlice = toggle;
			this.toggleSwordBeams = toggleBeam;
			this.swapJCActivation = swapJC;
			this.noJC = noJudgementCut;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002574 File Offset: 0x00000774
		private void Item_OnUnSnapEvent(Holder holder)
		{
			this.sheathed = false;
			bool flag = (PlayerControl.GetHand(PlayerControl.handLeft.side).castPressed && PlayerControl.GetHand(PlayerControl.handLeft.side).alternateUsePressed) || (PlayerControl.GetHand(PlayerControl.handRight.side).castPressed && PlayerControl.GetHand(PlayerControl.handRight.side).alternateUsePressed);
			if (flag)
			{
				base.StartCoroutine(this.JCE());
			}
			bool flag2 = this.swapJCActivation && !this.noJC;
			if (flag2)
			{
				base.StartCoroutine(this.JudgementCut(holder.GetComponentInParent<Item>()));
			}
			this.beam = false;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000262A File Offset: 0x0000082A
		public IEnumerator JCE()
		{
			EffectInstance instance = Catalog.GetData<EffectData>("JudgementCutEnd", true).Spawn(Player.local.transform, false, null, false, Array.Empty<Type>());
			instance.SetIntensity(1f);
			instance.Play(0, false);
			Catalog.GetData<ItemData>("JCESlashes", true).SpawnAsync(delegate(Item spawnedItem)
			{
				spawnedItem.rb.isKinematic = true;
				spawnedItem.Despawn(3f);
			}, new Vector3?(this.item.transform.position), new Quaternion?(Quaternion.identity), null, true, null);
			this.judgementCutEnd = true;
			foreach (Creature creature in Creature.allActive)
			{
				bool flag = !creature.isKilled && creature != Player.local.creature && creature.loaded;
				if (flag)
				{
					bool flag2 = Level.current.dungeon == null || (Level.current.dungeon != null && creature.currentRoom == Player.local.creature.currentRoom);
					if (flag2)
					{
						this.creatures.Add(creature);
						creature.animator.speed = 0f;
						creature.locomotion.allowMove = false;
						creature.locomotion.allowTurn = false;
						creature.locomotion.allowJump = false;
					}
				}
				creature = null;
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
			yield break;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000263C File Offset: 0x0000083C
		private void Item_OnSnapEvent(Holder holder)
		{
			bool flag = this.judgementCutEnd;
			if (flag)
			{
				base.StartCoroutine(this.AnimeSlice());
				this.judgementCutEnd = false;
			}
			this.sheathed = true;
			bool flag2 = !this.swapJCActivation && !this.noJC;
			if (flag2)
			{
				base.StartCoroutine(this.JudgementCut(holder.GetComponentInParent<Item>()));
			}
			bool flag3 = this.parts != null;
			if (flag3)
			{
				base.StartCoroutine(this.AnimeSlice());
			}
			this.Deactivate();
			this.beam = false;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026C6 File Offset: 0x000008C6
		private void Item_OnTelekinesisReleaseEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			this.telekinesis = null;
			this.Deactivate();
			this.beam = false;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026DE File Offset: 0x000008DE
		private void Item_OnTelekinesisGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			this.telekinesis = teleGrabber;
			this.Deactivate();
			this.beam = false;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026F6 File Offset: 0x000008F6
		private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			this.Deactivate();
			this.beam = false;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002707 File Offset: 0x00000907
		private void Item_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			this.Deactivate();
			this.beam = false;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002718 File Offset: 0x00000918
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = !this.toggleAnimeSlice;
			if (flag)
			{
				bool flag2 = (!this.swapButtons && action == 2) || (this.swapButtons && action == 0);
				if (flag2)
				{
					this.Activate();
				}
				else
				{
					bool flag3 = (!this.swapButtons && action == 3) || (this.swapButtons && action == 1);
					if (flag3)
					{
						this.Deactivate();
					}
				}
			}
			else
			{
				bool flag4 = (!this.swapButtons && action == 2) || (this.swapButtons && action == 0);
				if (flag4)
				{
					bool flag5 = !this.active;
					if (flag5)
					{
						this.Activate();
					}
					else
					{
						this.Deactivate();
					}
				}
			}
			bool flag6 = !this.toggleSwordBeams;
			if (flag6)
			{
				bool flag7 = (!this.swapButtons && action == null) || (this.swapButtons && action == 2);
				if (flag7)
				{
					this.beam = true;
				}
				else
				{
					bool flag8 = (!this.swapButtons && action == 1) || (this.swapButtons && action == 3);
					if (flag8)
					{
						this.beam = false;
					}
				}
			}
			else
			{
				bool flag9 = (!this.swapButtons && action == null) || (this.swapButtons && action == 2);
				if (flag9)
				{
					this.beam = !this.beam;
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002878 File Offset: 0x00000A78
		public void FixedUpdate()
		{
			bool flag = this.telekinesis != null && this.telekinesis.spinMode && !this.active;
			if (flag)
			{
				this.Activate();
			}
			else
			{
				bool flag2 = this.telekinesis != null && !this.telekinesis.spinMode && this.active;
				if (flag2)
				{
					this.Deactivate();
				}
			}
			bool flag3 = Time.time - this.cdH <= this.cooldown || !this.beam || this.item.rb.velocity.magnitude - Player.local.locomotion.rb.velocity.magnitude < this.swordSpeed;
			if (!flag3)
			{
				this.cdH = Time.time;
				Catalog.GetData<ItemData>("YamatoBeam", true).SpawnAsync(null, new Vector3?(this.item.flyDirRef.position), new Quaternion?(Quaternion.LookRotation(this.item.flyDirRef.forward, this.item.rb.velocity)), null, true, null);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000029A5 File Offset: 0x00000BA5
		public void Deactivate()
		{
			this.blades.SetActive(true);
			this.triggers.SetActive(false);
			this.active = false;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000029C9 File Offset: 0x00000BC9
		public void Activate()
		{
			this.blades.SetActive(false);
			this.triggers.SetActive(true);
			this.pierce.UnPenetrateAll();
			this.slash.UnPenetrateAll();
			this.active = true;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002A05 File Offset: 0x00000C05
		public IEnumerator JCEAnimeSlice(Creature creature)
		{
			foreach (RagdollPart part in creature.ragdoll.parts)
			{
				part.gameObject.SetActive(true);
				bool sliceAllowed = part.sliceAllowed;
				if (sliceAllowed)
				{
					CollisionInstance instance = new CollisionInstance(new DamageStruct(2, 20f), null, null);
					instance.damageStruct.hitRagdollPart = part;
					part.ragdoll.creature.Damage(instance);
					part.ragdoll.TrySlice(part);
					bool sliceForceKill = part.data.sliceForceKill;
					if (sliceForceKill)
					{
						part.ragdoll.creature.Kill();
					}
					yield return null;
					instance = null;
				}
				else
				{
					bool flag = !part.ragdoll.creature.isKilled;
					if (flag)
					{
						CollisionInstance instance2 = new CollisionInstance(new DamageStruct(2, 20f), null, null);
						instance2.damageStruct.hitRagdollPart = part;
						part.ragdoll.creature.Damage(instance2);
						instance2 = null;
					}
				}
				part = null;
			}
			List<RagdollPart>.Enumerator enumerator = default(List<RagdollPart>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002A1B File Offset: 0x00000C1B
		public IEnumerator AnimeSlice()
		{
			bool flag = this.judgementCutEnd;
			if (flag)
			{
				foreach (Creature creature in this.creatures)
				{
					bool flag2 = creature != Player.local.creature;
					if (flag2)
					{
						creature.animator.speed = 1f;
						creature.locomotion.allowMove = true;
						creature.locomotion.allowTurn = true;
						creature.locomotion.allowJump = true;
						bool loaded = creature.loaded;
						if (loaded)
						{
							base.StartCoroutine(this.JCEAnimeSlice(creature));
						}
					}
					creature = null;
				}
				List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
				this.creatures.Clear();
			}
			foreach (RagdollPart part in this.parts)
			{
				RagdollPart ragdollPart = part;
				bool flag3;
				if (ragdollPart == null)
				{
					flag3 = false;
				}
				else
				{
					Ragdoll ragdoll = ragdollPart.ragdoll;
					bool? flag4;
					if (ragdoll == null)
					{
						flag4 = null;
					}
					else
					{
						Creature creature2 = ragdoll.creature;
						if (creature2 == null)
						{
							flag4 = null;
						}
						else
						{
							GameObject gameObject = creature2.gameObject;
							flag4 = ((gameObject != null) ? new bool?(gameObject.activeSelf) : null);
						}
					}
					bool? flag5 = flag4;
					bool flag6 = true;
					flag3 = (flag5.GetValueOrDefault() == flag6) & (flag5 != null);
				}
				bool flag7;
				if (flag3 && part != null && !part.isSliced)
				{
					RagdollPart ragdollPart2 = part;
					Object @object;
					if (ragdollPart2 == null)
					{
						@object = null;
					}
					else
					{
						Ragdoll ragdoll2 = ragdollPart2.ragdoll;
						@object = ((ragdoll2 != null) ? ragdoll2.creature : null);
					}
					flag7 = @object != Player.currentCreature;
				}
				else
				{
					flag7 = false;
				}
				bool flag8 = flag7;
				if (flag8)
				{
					part.gameObject.SetActive(true);
					bool sliceAllowed = part.sliceAllowed;
					if (sliceAllowed)
					{
						CollisionInstance instance = new CollisionInstance(new DamageStruct(2, 20f), null, null);
						instance.damageStruct.hitRagdollPart = part;
						part.ragdoll.creature.Damage(instance);
						part.ragdoll.TrySlice(part);
						bool sliceForceKill = part.data.sliceForceKill;
						if (sliceForceKill)
						{
							part.ragdoll.creature.Kill();
						}
						yield return null;
						instance = null;
					}
					else
					{
						bool flag9 = !part.sliceAllowed && !part.ragdoll.creature.isKilled;
						if (flag9)
						{
							CollisionInstance instance2 = new CollisionInstance(new DamageStruct(2, 20f), null, null);
							instance2.damageStruct.hitRagdollPart = part;
							part.ragdoll.creature.Damage(instance2);
							instance2 = null;
						}
					}
				}
				part = null;
			}
			List<RagdollPart>.Enumerator enumerator2 = default(List<RagdollPart>.Enumerator);
			this.parts.Clear();
			yield break;
			yield break;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A2A File Offset: 0x00000C2A
		public IEnumerator JudgementCut(Item otherItem)
		{
			yield return new WaitForSecondsRealtime(0.1f);
			bool flag = (this.sheathed && !this.swapJCActivation) || (!this.sheathed && this.swapJCActivation) || (otherItem.GetComponent<YamatoSheathFrameworkComponent>() != null && this.item.mainHandler == null) || (otherItem.GetComponent<YamatoSheathFrameworkComponent>() != null && this.item.mainHandler != null && !this.item.mainHandler.playerHand.controlHand.usePressed);
			if (flag)
			{
				yield break;
			}
			GameObject creature = new GameObject();
			Creature enemy = this.GetEnemy();
			Object @object;
			if (enemy == null)
			{
				@object = null;
			}
			else
			{
				Ragdoll ragdoll = enemy.ragdoll;
				@object = ((ragdoll != null) ? ragdoll.targetPart : null);
			}
			bool flag2 = @object != null;
			if (flag2)
			{
				creature.AddComponent<JudgementCutPosition>().position = this.GetEnemy().ragdoll.targetPart.transform.position;
			}
			else
			{
				RaycastHit hit;
				bool flag3 = Physics.Raycast(Player.local.head.transform.position, Player.local.head.cam.transform.forward, ref hit, float.PositiveInfinity, -1, 1);
				if (flag3)
				{
					creature.AddComponent<JudgementCutPosition>().position = hit.point;
				}
				hit = default(RaycastHit);
			}
			bool flag4 = this.stopOnJC && !Player.local.locomotion.isGrounded;
			if (flag4)
			{
				Player.local.locomotion.rb.velocity = Vector3.zero;
				Player.local.locomotion.rb.AddForce(Vector3.up * 200f, 1);
			}
			EffectInstance instance = Catalog.GetData<EffectData>("JudgementCutStart", true).Spawn(this.item.transform, false, null, false, Array.Empty<Type>());
			instance.SetIntensity(1f);
			instance.Play(0, false);
			yield break;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002A40 File Offset: 0x00000C40
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
					bool flag2 = creature3 != null && !creature3.isPlayer && creature3.ragdoll.isActiveAndEnabled && !creature3.isKilled && Vector3.Dot(Player.local.head.cam.transform.forward.normalized, (creature3.transform.position - Player.local.transform.position).normalized) >= 0.75f && creature == null && Vector3.Distance(Player.local.transform.position, creature3.transform.position) <= 25f && ((Level.current.dungeon != null && creature3.currentRoom == Player.local.creature.currentRoom) || Level.current.dungeon == null);
					if (flag2)
					{
						creature = creature3;
					}
					else
					{
						bool flag3 = creature3 != null && !creature3.isPlayer && creature3.ragdoll.isActiveAndEnabled && !creature3.isKilled && Vector3.Dot(Player.local.head.cam.transform.forward.normalized, (creature3.transform.position - Player.local.transform.position).normalized) >= 0.75f && creature != null && Vector3.Distance(Player.local.transform.position, creature3.transform.position) <= 25f && ((Level.current.dungeon != null && creature3.currentRoom == Player.local.creature.currentRoom) || Level.current.dungeon == null);
						if (flag3)
						{
							bool flag4 = Vector3.Dot(Player.local.head.cam.transform.forward.normalized, (creature3.transform.position - Player.local.transform.position).normalized) > Vector3.Dot(Player.local.head.cam.transform.forward.normalized, (creature.transform.position - Player.local.transform.position).normalized);
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

		// Token: 0x06000022 RID: 34 RVA: 0x00002D90 File Offset: 0x00000F90
		public void OnTriggerEnter(Collider c)
		{
			bool flag = this.item.holder == null && c.GetComponentInParent<ColliderGroup>() != null;
			if (flag)
			{
				ColliderGroup componentInParent = c.GetComponentInParent<ColliderGroup>();
				Object @object;
				if (componentInParent == null)
				{
					@object = null;
				}
				else
				{
					CollisionHandler collisionHandler = componentInParent.collisionHandler;
					@object = ((collisionHandler != null) ? collisionHandler.ragdollPart : null);
				}
				bool flag2;
				if (@object != null)
				{
					Object object2;
					if (componentInParent == null)
					{
						object2 = null;
					}
					else
					{
						CollisionHandler collisionHandler2 = componentInParent.collisionHandler;
						if (collisionHandler2 == null)
						{
							object2 = null;
						}
						else
						{
							RagdollPart ragdollPart = collisionHandler2.ragdollPart;
							if (ragdollPart == null)
							{
								object2 = null;
							}
							else
							{
								Ragdoll ragdoll = ragdollPart.ragdoll;
								object2 = ((ragdoll != null) ? ragdoll.creature : null);
							}
						}
					}
					flag2 = object2 != Player.currentCreature;
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					RagdollPart ragdollPart2 = componentInParent.collisionHandler.ragdollPart;
					ragdollPart2.gameObject.SetActive(true);
					Creature creature = ragdollPart2.ragdoll.creature;
					bool flag4 = creature != Player.currentCreature && !this.parts.Contains(ragdollPart2);
					if (flag4)
					{
						this.parts.Add(ragdollPart2);
					}
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002E90 File Offset: 0x00001090
		public void OnCollisionEnter(Collision c)
		{
			bool flag = c.collider.gameObject.GetComponentInParent<SheathComponent>() != null;
			if (flag)
			{
				this.item.IgnoreObjectCollision(c.collider.gameObject.GetComponentInParent<Item>());
			}
		}

		// Token: 0x0400000F RID: 15
		private Item item;

		// Token: 0x04000010 RID: 16
		public List<RagdollPart> parts = new List<RagdollPart>();

		// Token: 0x04000011 RID: 17
		private List<Creature> creatures = new List<Creature>();

		// Token: 0x04000012 RID: 18
		private bool active = false;

		// Token: 0x04000013 RID: 19
		private bool sheathed = true;

		// Token: 0x04000014 RID: 20
		private bool beam = false;

		// Token: 0x04000015 RID: 21
		private bool judgementCutEnd = false;

		// Token: 0x04000016 RID: 22
		private float cdH;

		// Token: 0x04000017 RID: 23
		private Damager pierce;

		// Token: 0x04000018 RID: 24
		private Damager slash;

		// Token: 0x04000019 RID: 25
		private GameObject blades;

		// Token: 0x0400001A RID: 26
		private GameObject triggers;

		// Token: 0x0400001B RID: 27
		private SpellTelekinesis telekinesis;

		// Token: 0x0400001C RID: 28
		public float swordSpeed;

		// Token: 0x0400001D RID: 29
		public float cooldown;

		// Token: 0x0400001E RID: 30
		public bool swapButtons;

		// Token: 0x0400001F RID: 31
		private bool stopOnJC;

		// Token: 0x04000020 RID: 32
		private bool toggleAnimeSlice;

		// Token: 0x04000021 RID: 33
		private bool toggleSwordBeams;

		// Token: 0x04000022 RID: 34
		private bool swapJCActivation;

		// Token: 0x04000023 RID: 35
		private bool noJC;
	}
}

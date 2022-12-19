using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Speech.Recognition;
using ThunderRoad;
using UnityEngine;

namespace Threat
{
	// Token: 0x02000002 RID: 2
	public class ThreatBehaviour : ThunderBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected override ManagedLoops ManagedLoops
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002054 File Offset: 0x00000254
		public void Awake()
		{
			base.gameObject.layer = GameManager.GetLayer(18);
			this.neckPart = this.creature.ragdoll.GetPart(2);
			this.moduleMove = this.creature.brain.instance.GetModule<BrainModuleMove>(false);
			this.moduleMelee = this.creature.brain.instance.GetModule<BrainModuleMelee>(false);
			this.moduleCast = this.creature.brain.instance.GetModule<BrainModuleCast>(false);
			this.moduleFollow = this.creature.brain.instance.GetModule<BrainModuleFollow>(false);
			this.moduleBow = this.creature.brain.instance.GetModule<BrainModuleBow>(false);
			this.creature.ragdoll.SetState(3);
			base.transform.SetParent(this.neckPart.bone.animation);
			base.transform.localPosition = new Vector3(-0.04f, 0f, (this.creature.data.gender == 1) ? 0.03f : 0.02f);
			base.transform.localRotation = Quaternion.identity;
			base.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
			this.CreateCollider();
			bool flag = this.creature.ragdoll.state != 5 && this.creature.ragdoll.state != 6 && this.creature.ragdoll.state != 4;
			if (flag)
			{
				base.transform.SetParent(this.neckPart.transform);
				base.transform.localPosition = new Vector3(-0.04f, 0f, (this.creature.data.gender == 1) ? 0.03f : 0.02f);
				base.transform.localRotation = Quaternion.identity;
				base.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
			}
			this.creature.OnDespawnEvent += new Creature.DespawnEvent(this.Creature_OnDespawnEvent);
			this.creature.ragdoll.OnStateChange += new Ragdoll.StateChange(this.Ragdoll_OnStateChange);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000022B8 File Offset: 0x000004B8
		private void SpeechRecognition_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
		{
			Debug.Log("[Threat] Recognised: " + e.Result.Text + " | Confidence: " + e.Result.Confidence.ToString());
			bool flag = e.Result.Confidence < ThreatLevelModule.local.minConfidence;
			if (!flag)
			{
				bool flag2 = e.Result.Grammar == ThreatLevelModule.dropGrammar;
				if (flag2)
				{
					this.DropWeapons();
				}
				else
				{
					bool flag3 = e.Result.Grammar == ThreatLevelModule.joinGrammar;
					if (flag3)
					{
						this.TryMakeAlly();
					}
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002352 File Offset: 0x00000552
		private void CreateCollider()
		{
			this.collider = base.gameObject.AddComponent<SphereCollider>();
			this.collider.isTrigger = true;
			this.creature.ragdoll.IgnoreCollision(this.collider, true, 0);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000238C File Offset: 0x0000058C
		protected override void ManagedUpdate()
		{
			bool flag = this.isFollowing;
			if (flag)
			{
				this.creature.brain.instance.tree.blackboard.UpdateVariable<Creature>("FollowTarget", Player.local.creature);
			}
			bool flag2 = !ThreatLevelModule.local.threatAffectsAllies || this.creature.brain.state != 5;
			if (!flag2)
			{
				bool flag3 = this.creature.brain.currentTarget && this.creature.brain.currentTarget.isPlayer && ThreatBehaviour.threatenedCreatures.Any((Creature creature) => creature != this.creature && creature.factionId == this.creature.factionId);
				if (flag3)
				{
					this.moduleMelee.meleeEnabled = false;
					bool isAttacking = this.creature.brain.isAttacking;
					if (isAttacking)
					{
						this.moduleMelee.StopAttack(this.moduleMelee, this.moduleMelee.animationDataClip, this.moduleMelee.attackCount);
					}
					bool isCasting = this.creature.brain.isCasting;
					if (isCasting)
					{
						this.moduleCast.StopCast(this.creature.mana.casterLeft.spellInstance != null, this.creature.mana.casterRight.spellInstance != null);
					}
					bool isShooting = this.creature.brain.isShooting;
					if (isShooting)
					{
						ThreatBehaviour.aimTime.SetValue(this.moduleBow, (float)ThreatBehaviour.aimTime.GetValue(this.moduleBow) + Time.deltaTime);
					}
				}
				else
				{
					this.moduleMelee.meleeEnabled = true;
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002544 File Offset: 0x00000744
		private void Creature_OnDespawnEvent(EventTime eventTime)
		{
			bool flag = eventTime == 0;
			if (!flag)
			{
				this.creature.OnDespawnEvent -= new Creature.DespawnEvent(this.Creature_OnDespawnEvent);
				this.creature.ragdoll.OnStateChange -= new Ragdoll.StateChange(this.Ragdoll_OnStateChange);
				bool flag2 = ThreatLevelModule.speechRecognition != null;
				if (flag2)
				{
					ThreatLevelModule.speechRecognition.SpeechRecognized -= this.SpeechRecognition_SpeechRecognized;
				}
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000025C0 File Offset: 0x000007C0
		private void Ragdoll_OnStateChange(Ragdoll.State previousState, Ragdoll.State newState, Ragdoll.PhysicStateChange physicStateChange, EventTime eventTime)
		{
			bool flag = eventTime == 0;
			if (flag)
			{
				if (physicStateChange != 1)
				{
					if (physicStateChange == 2)
					{
						base.transform.SetParent(null);
					}
				}
			}
			else if (physicStateChange != 1)
			{
				if (physicStateChange == 2)
				{
					base.transform.SetParent(this.neckPart.bone.animation);
					base.transform.localPosition = new Vector3(-0.04f, 0f, (this.creature.data.gender == 1) ? 0.03f : 0.02f);
					base.transform.localRotation = Quaternion.identity;
					base.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
				}
			}
			else
			{
				base.transform.SetParent(this.neckPart.colliderGroup.transform);
				base.transform.localPosition = new Vector3(-0.04f, 0f, (this.creature.data.gender == 1) ? 0.03f : 0.02f);
				base.transform.localRotation = Quaternion.identity;
				base.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002730 File Offset: 0x00000930
		private void OnTriggerEnter(Collider other)
		{
			bool flag = this.creature.state == null || !this.IsColliderThreat(other);
			if (!flag)
			{
				this.threateningColliders.Add(other);
				bool flag2 = this.isThreatened;
				if (!flag2)
				{
					this.isThreatened = true;
					ThreatBehaviour.threatenedCreatures.Add(this.creature);
					this.Freeze();
					bool useSpeech = ThreatLevelModule.local.useSpeech;
					if (useSpeech)
					{
						ThreatLevelModule.speechRecognition.SpeechRecognized += this.SpeechRecognition_SpeechRecognized;
					}
					else
					{
						this.DropWeapons();
						this.TryMakeAlly();
					}
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000027D0 File Offset: 0x000009D0
		private void OnTriggerExit(Collider other)
		{
			bool flag = this.threateningColliders.Remove(other) && this.threateningColliders.Count == 0;
			if (flag)
			{
				this.ResetThreat();
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002808 File Offset: 0x00000A08
		private bool IsColliderThreat(Collider collider)
		{
			Item item = collider.GetComponentInParent<Item>();
			bool flag = !item || !item.IsHanded(null);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				ColliderGroup colliderGroup = collider.GetComponentInParent<ColliderGroup>();
				foreach (Damager damager in item.mainCollisionHandler.damagers)
				{
					bool flag3 = damager.colliderOnly == collider || (colliderGroup && damager.colliderGroup == colliderGroup);
					if (flag3)
					{
						bool flag4 = damager.penetrationDepth != 0f || damager.penetrationLength != 0f;
						if (flag4)
						{
							return true;
						}
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000028F4 File Offset: 0x00000AF4
		private void ResetThreat()
		{
			this.isThreatened = false;
			ThreatBehaviour.threatenedCreatures.Remove(this.creature);
			bool flag = this.freezeCoroutine != null;
			if (flag)
			{
				base.StopCoroutine(this.freezeCoroutine);
			}
			this.freezeCoroutine = null;
			bool flag2 = this.dropWeaponsCoroutine != null;
			if (flag2)
			{
				base.StopCoroutine(this.dropWeaponsCoroutine);
			}
			this.dropWeaponsCoroutine = null;
			bool flag3 = this.tryMakeAllyCoroutine != null;
			if (flag3)
			{
				base.StopCoroutine(this.tryMakeAllyCoroutine);
			}
			this.tryMakeAllyCoroutine = null;
			bool flag4 = ThreatLevelModule.speechRecognition != null;
			if (flag4)
			{
				ThreatLevelModule.speechRecognition.SpeechRecognized -= this.SpeechRecognition_SpeechRecognized;
			}
			this.creature.animator.speed = 1f;
			this.moduleMove.allowMove = true;
			this.creature.brain.RemoveNoStandUpModifier(this);
			this.moduleMelee.meleeEnabled = true;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000029E0 File Offset: 0x00000BE0
		public void Freeze()
		{
			bool flag = this.freezeCoroutine != null;
			if (flag)
			{
				base.StopCoroutine(this.freezeCoroutine);
			}
			this.freezeCoroutine = base.StartCoroutine(this.FreezeCoroutine());
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002A1A File Offset: 0x00000C1A
		private IEnumerator FreezeCoroutine()
		{
			yield return Yielders.ForSeconds(ThreatLevelModule.local.minTimeBeforeThreatInSeconds);
			while (this.isThreatened)
			{
				this.creature.ragdoll.CancelGetUp(true);
				this.creature.animator.speed = 0.5f;
				this.moduleMove.allowMove = false;
				this.creature.brain.AddNoStandUpModifier(this);
				this.moduleMelee.meleeEnabled = false;
				bool isAttacking = this.creature.brain.isAttacking;
				if (isAttacking)
				{
					this.moduleMelee.StopAttack(this.moduleMelee, this.moduleMelee.animationDataClip, this.moduleMelee.attackCount);
				}
				bool isCasting = this.creature.brain.isCasting;
				if (isCasting)
				{
					this.moduleCast.StopCast(this.creature.mana.casterLeft.spellInstance != null, this.creature.mana.casterRight.spellInstance != null);
				}
				yield return Yielders.EndOfFrame;
			}
			yield break;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002A2C File Offset: 0x00000C2C
		public void DropWeapons()
		{
			bool flag = this.dropWeaponsCoroutine != null;
			if (flag)
			{
				base.StopCoroutine(this.dropWeaponsCoroutine);
			}
			this.dropWeaponsCoroutine = base.StartCoroutine(this.DropWeaponsCoroutine());
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002A66 File Offset: 0x00000C66
		private IEnumerator DropWeaponsCoroutine()
		{
			yield return Yielders.ForSeconds(ThreatLevelModule.local.minTimeBeforeDisarmInSeconds);
			this.creature.GetHand(0).TryRelease();
			this.creature.GetHand(1).TryRelease();
			this.hasDroppedWeapons = true;
			yield break;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002A78 File Offset: 0x00000C78
		public void TryMakeAlly()
		{
			bool flag = this.tryMakeAllyCoroutine != null;
			if (flag)
			{
				base.StopCoroutine(this.tryMakeAllyCoroutine);
			}
			this.tryMakeAllyCoroutine = base.StartCoroutine(this.TryMakeAllyCoroutine());
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002AB2 File Offset: 0x00000CB2
		private IEnumerator TryMakeAllyCoroutine()
		{
			yield return Yielders.ForSeconds(ThreatLevelModule.local.minTimeBeforeAllyInSeconds);
			float multiplier = 1f;
			bool useHonour = ThreatLevelModule.local.useHonour;
			if (useHonour)
			{
				int currentHonour = Level.current.mode.GetModule<LevelModuleXP>().currentHonor;
				bool flag = currentHonour < ThreatLevelModule.local.minHonourAmount;
				if (flag)
				{
					yield break;
				}
				bool flag2 = ThreatLevelModule.local.targetHonourAmount != 0;
				if (flag2)
				{
					multiplier = Utils.CalculateRatio((float)currentHonour, (float)ThreatLevelModule.local.minHonourAmount, (float)ThreatLevelModule.local.targetHonourAmount, 0f, 1f);
				}
			}
			bool flag3 = Random.value <= ThreatLevelModule.local.chanceOfAlly * multiplier;
			if (flag3)
			{
				this.creature.SetFaction(2);
				this.creature.brain.Load(this.creature.brain.instance.id);
				bool flag4 = this.creature.IsFromWave();
				if (flag4)
				{
					foreach (WaveSpawner waveSpawner in WaveSpawner.instances)
					{
						waveSpawner.RemoveFromWave(this.creature);
						waveSpawner = null;
					}
					List<WaveSpawner>.Enumerator enumerator = default(List<WaveSpawner>.Enumerator);
				}
				this.isFollowing = true;
			}
			yield break;
		}

		// Token: 0x04000001 RID: 1
		public static HashSet<Creature> threatenedCreatures = new HashSet<Creature>();

		// Token: 0x04000002 RID: 2
		[NonSerialized]
		public Creature creature;

		// Token: 0x04000003 RID: 3
		protected RagdollPart neckPart;

		// Token: 0x04000004 RID: 4
		protected Collider collider;

		// Token: 0x04000005 RID: 5
		protected HashSet<Collider> threateningColliders = new HashSet<Collider>();

		// Token: 0x04000006 RID: 6
		protected BrainModuleMove moduleMove;

		// Token: 0x04000007 RID: 7
		protected BrainModuleMelee moduleMelee;

		// Token: 0x04000008 RID: 8
		protected BrainModuleCast moduleCast;

		// Token: 0x04000009 RID: 9
		protected BrainModuleFollow moduleFollow;

		// Token: 0x0400000A RID: 10
		protected BrainModuleBow moduleBow;

		// Token: 0x0400000B RID: 11
		protected bool isThreatened;

		// Token: 0x0400000C RID: 12
		protected bool isFollowing;

		// Token: 0x0400000D RID: 13
		protected bool hasDroppedWeapons;

		// Token: 0x0400000E RID: 14
		private Coroutine freezeCoroutine;

		// Token: 0x0400000F RID: 15
		private Coroutine dropWeaponsCoroutine;

		// Token: 0x04000010 RID: 16
		private Coroutine tryMakeAllyCoroutine;

		// Token: 0x04000011 RID: 17
		private static readonly PropertyInfo aimTime = typeof(BrainModuleRanged).GetProperty("aimTime", BindingFlags.Instance | BindingFlags.Public);
	}
}

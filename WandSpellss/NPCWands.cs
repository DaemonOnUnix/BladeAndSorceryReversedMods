using System;
using System.Timers;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000012 RID: 18
	internal class NPCWands : BrainData.Module
	{
		// Token: 0x06000034 RID: 52 RVA: 0x000038D4 File Offset: 0x00001AD4
		public override void Load(Creature creature)
		{
			base.Load(creature);
			this.npc = creature;
			this.npc.brain.OnAttackEvent += new Brain.AttackEvent(this.Brain_OnAttackEvent);
			this.playSound = false;
			this.animation = Catalog.GetData<AnimationData>("HumanWandCast", true);
			this.isAttacking = false;
			string text = "This is the animation: ";
			AnimationClip animationClip = this.animation.animationClips[0].animationClip;
			Debug.Log(text + ((animationClip != null) ? animationClip.ToString() : null));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003960 File Offset: 0x00001B60
		public override void Update()
		{
			base.Update();
			bool flag = !this.npc.isPlayingDynamicAnimation;
			if (flag)
			{
				this.isAttacking = false;
			}
			bool flag2 = this.npc.handRight.grabbedHandle != null;
			if (flag2)
			{
				this.weapon = this.npc.handRight.grabbedHandle.item;
			}
			this.distanceSqr = (this.npc.transform.position - Player.local.transform.position).sqrMagnitude;
			bool flag3 = this.distanceSqr <= this.attackRange && !this.isAttacking;
			if (flag3)
			{
				this.npc.brain.InvokeAttackEvent(2, false, Player.local.creature);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003A38 File Offset: 0x00001C38
		private void Brain_OnAttackEvent(Brain.AttackType attackType, bool strong, Creature target)
		{
			this.isAttacking = true;
			this.npc.PlayAnimation(this.animation.animationClips[0].animationClip, false, 1f, null, false, true);
			bool flag = this.distanceSqr <= this.attackRange;
			if (flag)
			{
				bool flag2 = this.weapon.GetComponent<MyWeaponComponent>() != null;
				if (flag2)
				{
					this.weapon.GetComponent<MyWeaponComponent>().spellsList[0].SpawnAsync(delegate(Item projectile)
					{
						projectile.transform.position = this.weapon.flyDirRef.position;
						projectile.transform.rotation = this.weapon.flyDirRef.rotation;
						projectile.IgnoreRagdollCollision(this.npc.ragdoll);
						projectile.IgnoreObjectCollision(this.weapon);
						projectile.Throw(1f, 1);
						projectile.gameObject.AddComponent<Stupefy>();
						projectile.rb.useGravity = false;
						projectile.rb.drag = 0f;
						projectile.rb.AddForce(this.weapon.flyDirRef.transform.forward * 5f, 1);
						projectile.gameObject.AddComponent<SpellDespawn>();
						foreach (AudioSource c in this.weapon.GetComponentsInChildren<AudioSource>())
						{
							string name = c.name;
							string text = name;
							if (text == "StupefySound")
							{
								this.sourceCurrent = c;
							}
						}
						this.sourceCurrent.Play();
						this.playSound = true;
					}, null, null, null, true, null);
				}
				this.SetTimer();
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003AEC File Offset: 0x00001CEC
		private void SetTimer()
		{
			this.aTimer = new Timer(5000.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003B40 File Offset: 0x00001D40
		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			this.isAttacking = false;
		}

		// Token: 0x0400004A RID: 74
		private Creature npc;

		// Token: 0x0400004B RID: 75
		private Player player;

		// Token: 0x0400004C RID: 76
		private Creature target;

		// Token: 0x0400004D RID: 77
		private float distanceSqr;

		// Token: 0x0400004E RID: 78
		private float attackRange = 15f;

		// Token: 0x0400004F RID: 79
		private Item weapon;

		// Token: 0x04000050 RID: 80
		private AudioSource sourceCurrent;

		// Token: 0x04000051 RID: 81
		private bool playSound;

		// Token: 0x04000052 RID: 82
		private AnimationData animation;

		// Token: 0x04000053 RID: 83
		private bool isAttacking;

		// Token: 0x04000054 RID: 84
		private Timer aTimer;
	}
}

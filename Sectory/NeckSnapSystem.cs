using System;
using ThunderRoad;
using UnityEngine;

namespace Sectory
{
	// Token: 0x0200001A RID: 26
	public class NeckSnapSystem : MonoBehaviour
	{
		// Token: 0x0600003B RID: 59 RVA: 0x0000348C File Offset: 0x0000168C
		public void Setup(float neckStrength)
		{
			this.creature = base.GetComponentInParent<Creature>();
			this.creature.ragdoll.OnSliceEvent += new Ragdoll.SliceEvent(this.Slice);
			this.creature.OnKillEvent += new Creature.KillEvent(this.Creature_OnKillEvent);
			this.neckStrength = neckStrength;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000034E2 File Offset: 0x000016E2
		private void Creature_OnKillEvent(CollisionInstance collisionInstance, EventTime eventTime)
		{
			this.wasKilled = true;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000034EC File Offset: 0x000016EC
		private void Slice(RagdollPart ragdollPart, EventTime eventTime)
		{
			bool flag = ragdollPart.type == 2 || ragdollPart.type == 1;
			if (flag)
			{
				this.isSnapped = true;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000351C File Offset: 0x0000171C
		private void Update()
		{
			bool flag = (Time.time - this.timer > 3f && this.count) || this.creature == null;
			if (flag)
			{
				this.snapSound.Despawn();
			}
			bool flag2 = !this.creature.isKilled && this.wasKilled;
			if (flag2)
			{
				this.Reset();
			}
			bool flag3 = Quaternion.Dot(this.creature.ragdoll.GetPart(1).transform.rotation, this.creature.ragdoll.GetPart(4).transform.rotation) < this.neckStrength && !this.isSnapped;
			if (flag3)
			{
				this.Snap();
			}
			bool flag4 = this.isSnapped;
			if (flag4)
			{
				this.creature.ragdoll.GetPart(2).DisableCharJointLimit();
				this.creature.ragdoll.GetPart(2).rb.useGravity = true;
				this.creature.ragdoll.GetPart(1).DisableCharJointLimit();
				this.creature.ragdoll.GetPart(1).rb.useGravity = true;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003650 File Offset: 0x00001850
		private void Reset()
		{
			this.isSnapped = false;
			this.wasKilled = false;
			bool flag = this.snapSound != null;
			if (flag)
			{
				this.snapSound.Despawn();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003688 File Offset: 0x00001888
		private void Snap()
		{
			bool flag = !this.isSnapped;
			if (flag)
			{
				this.creature.Damage(new CollisionInstance(new DamageStruct(0, 200f), null, null));
				this.snapSound = Catalog.GetData<EffectData>("NeckBreak", true).Spawn(this.creature.ragdoll.headPart.transform.position, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
				this.snapSound.Play(0, false);
				this.timer = Time.time;
				this.count = true;
				this.isSnapped = true;
			}
		}

		// Token: 0x0400008F RID: 143
		private Creature creature;

		// Token: 0x04000090 RID: 144
		private float timer;

		// Token: 0x04000091 RID: 145
		public bool count;

		// Token: 0x04000092 RID: 146
		public bool isSnapped;

		// Token: 0x04000093 RID: 147
		private EffectInstance snapSound;

		// Token: 0x04000094 RID: 148
		private float neckStrength;

		// Token: 0x04000095 RID: 149
		private bool wasKilled;
	}
}

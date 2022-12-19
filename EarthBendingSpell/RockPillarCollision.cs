using System;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x02000005 RID: 5
	public class RockPillarCollision : MonoBehaviour
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00003C18 File Offset: 0x00001E18
		private void OnCollisionEnter(Collision col)
		{
			EffectInstance effectInstance = this.effectData.Spawn(col.contacts[0].point, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
			effectInstance.Play(0, false);
			bool flag = col.collider.GetComponentInParent<Creature>();
			if (flag)
			{
				Creature componentInParent = col.collider.GetComponentInParent<Creature>();
				bool flag2 = componentInParent != Player.currentCreature;
				if (flag2)
				{
					bool flag3 = !componentInParent.isKilled;
					if (flag3)
					{
						CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(4, 20f), null, null);
						componentInParent.Damage(collisionInstance);
					}
				}
			}
			Object.Destroy(this);
		}

		// Token: 0x04000073 RID: 115
		public EffectData effectData;
	}
}

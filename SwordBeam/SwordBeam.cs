using System;
using ThunderRoad;
using UnityEngine;

namespace SwordBeam
{
	// Token: 0x02000004 RID: 4
	public class SwordBeam : MonoBehaviour
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000242C File Offset: 0x0000062C
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.renderers[0].material.SetColor("_BaseColor", this.BeamColor);
			this.item.renderers[0].material.SetColor("_EmissionColor", this.BeamEmission * 2f);
			this.item.renderers[0].gameObject.transform.localScale = this.BeamSize;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000024C5 File Offset: 0x000006C5
		public void Setup(float Damage, bool Dismember, Color color, Color emission, Vector3 size, Vector3 scale)
		{
			this.damage = Damage;
			this.dismember = Dismember;
			this.BeamColor = color;
			this.BeamEmission = emission;
			this.BeamSize = size;
			this.BeamScaleIncrease = scale;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024F5 File Offset: 0x000006F5
		public void FixedUpdate()
		{
			this.item.gameObject.transform.localScale += this.BeamScaleIncrease;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002520 File Offset: 0x00000720
		public void OnTriggerEnter(Collider c)
		{
			bool flag = c.GetComponentInParent<ColliderGroup>() != null;
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
					bool flag7;
					if (ragdollPart2.ragdoll.creature != Player.currentCreature && ragdollPart2 != null)
					{
						Ragdoll ragdoll2 = ragdollPart2.ragdoll;
						bool? flag4;
						if (ragdoll2 == null)
						{
							flag4 = null;
						}
						else
						{
							Creature creature = ragdoll2.creature;
							if (creature == null)
							{
								flag4 = null;
							}
							else
							{
								GameObject gameObject = creature.gameObject;
								flag4 = ((gameObject != null) ? new bool?(gameObject.activeSelf) : null);
							}
						}
						bool? flag5 = flag4;
						bool flag6 = true;
						if (((flag5.GetValueOrDefault() == flag6) & (flag5 != null)) && ragdollPart2 != null)
						{
							flag7 = !ragdollPart2.isSliced;
							goto IL_120;
						}
					}
					flag7 = false;
					IL_120:
					bool flag8 = flag7;
					if (flag8)
					{
						bool flag9 = ragdollPart2.sliceAllowed && this.dismember;
						if (flag9)
						{
							ragdollPart2.gameObject.SetActive(true);
							ragdollPart2.bone.animationJoint.gameObject.SetActive(true);
							ragdollPart2.ragdoll.TrySlice(ragdollPart2);
							bool sliceForceKill = ragdollPart2.data.sliceForceKill;
							if (sliceForceKill)
							{
								ragdollPart2.ragdoll.creature.Kill();
							}
						}
						else
						{
							bool flag10 = !ragdollPart2.ragdoll.creature.isKilled;
							if (flag10)
							{
								CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(2, this.damage), null, null);
								collisionInstance.damageStruct.hitRagdollPart = ragdollPart2;
								ragdollPart2.ragdoll.creature.Damage(collisionInstance);
								ragdollPart2.ragdoll.creature.TryPush(2, (ragdollPart2.transform.position - this.initialPosition).normalized, 1, 0);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400001C RID: 28
		private Item item;

		// Token: 0x0400001D RID: 29
		private float damage;

		// Token: 0x0400001E RID: 30
		private bool dismember;

		// Token: 0x0400001F RID: 31
		public Color BeamColor;

		// Token: 0x04000020 RID: 32
		public Color BeamEmission;

		// Token: 0x04000021 RID: 33
		public Vector3 BeamSize;

		// Token: 0x04000022 RID: 34
		public Vector3 BeamScaleIncrease;

		// Token: 0x04000023 RID: 35
		private Vector3 initialPosition = default(Vector3);
	}
}

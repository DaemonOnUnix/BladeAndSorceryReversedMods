using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x0200000C RID: 12
	public class BeamCustomization : MonoBehaviour
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00003904 File Offset: 0x00001B04
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.renderers[0].material.SetColor("_BaseColor", this.beamColor);
			this.item.renderers[0].material.SetColor("_EmissionColor", this.beamEmission * 2f);
			this.item.renderers[0].gameObject.transform.localScale = this.beamSize;
			this.item.rb.useGravity = false;
			this.item.rb.drag = 0f;
			this.item.rb.AddForce(Player.local.head.transform.forward * this.beamSpeed, 1);
			this.item.IgnoreRagdollCollision(Player.currentCreature.ragdoll);
			this.item.RefreshCollision(true);
			this.item.Throw(1f, 1);
			this.item.Despawn(this.despawnTime);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003A3D File Offset: 0x00001C3D
		public void Setup(bool beamDismember, float BeamSpeed, float BeamDespawn, float BeamDamage, Color color, Color emission, Vector3 size, Vector3 scaleUpdate)
		{
			this.dismember = beamDismember;
			this.beamSpeed = BeamSpeed;
			this.despawnTime = BeamDespawn;
			this.beamDamage = BeamDamage;
			this.beamColor = color;
			this.beamEmission = emission;
			this.beamSize = size;
			this.beamScaleUpdate = scaleUpdate;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003A80 File Offset: 0x00001C80
		public void FixedUpdate()
		{
			this.item.gameObject.transform.localScale += this.beamScaleUpdate;
			bool flag = this.parts.Count > 0;
			if (flag)
			{
				this.parts[0].gameObject.SetActive(true);
				this.parts[0].bone.animationJoint.gameObject.SetActive(true);
				this.parts[0].ragdoll.TrySlice(this.parts[0]);
				bool sliceForceKill = this.parts[0].data.sliceForceKill;
				if (sliceForceKill)
				{
					this.parts[0].ragdoll.creature.Kill();
				}
				this.parts.RemoveAt(0);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003B6C File Offset: 0x00001D6C
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
							bool flag10 = !this.parts.Contains(ragdollPart2);
							if (flag10)
							{
								this.parts.Add(ragdollPart2);
							}
							CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(2, 20f), null, null);
							collisionInstance.damageStruct.hitRagdollPart = ragdollPart2;
							ragdollPart2.ragdoll.creature.Damage(collisionInstance);
						}
						else
						{
							bool flag11 = !ragdollPart2.ragdoll.creature.isKilled;
							if (flag11)
							{
								CollisionInstance collisionInstance2 = new CollisionInstance(new DamageStruct(2, this.beamDamage), null, null);
								collisionInstance2.damageStruct.hitRagdollPart = ragdollPart2;
								ragdollPart2.ragdoll.creature.Damage(collisionInstance2);
								ragdollPart2.ragdoll.creature.TryPush(2, this.item.rb.velocity, 1, 0);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400004F RID: 79
		private Item item;

		// Token: 0x04000050 RID: 80
		public Color beamColor;

		// Token: 0x04000051 RID: 81
		public Color beamEmission;

		// Token: 0x04000052 RID: 82
		public Vector3 beamSize;

		// Token: 0x04000053 RID: 83
		private float despawnTime;

		// Token: 0x04000054 RID: 84
		private float beamSpeed;

		// Token: 0x04000055 RID: 85
		private float beamDamage;

		// Token: 0x04000056 RID: 86
		private bool dismember;

		// Token: 0x04000057 RID: 87
		private Vector3 beamScaleUpdate;

		// Token: 0x04000058 RID: 88
		private List<RagdollPart> parts = new List<RagdollPart>();
	}
}

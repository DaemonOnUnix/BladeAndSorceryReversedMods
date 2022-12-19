using System;
using System.Collections.Generic;
using System.Linq;
using RainyReignGames.RevealMask;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GhettosLaserEyes
{
	// Token: 0x02000003 RID: 3
	public class PlayerModule : MonoBehaviour
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
		private static float Hitscan(Transform muzzle, float damage, bool drawDecal, bool slice)
		{
			foreach (RaycastHit hit in Physics.RaycastAll(muzzle.position, muzzle.forward, float.PositiveInfinity, LayerMask.GetMask(new string[] { "BodyLocomotion" })))
			{
				bool flag = hit.collider.gameObject.GetComponentInParent<Creature>() != null;
				if (flag)
				{
					Creature cr = hit.collider.gameObject.GetComponentInParent<Creature>();
					bool flag2 = cr;
					if (flag2)
					{
						foreach (RagdollPart part in cr.ragdoll.parts)
						{
							part.gameObject.SetActive(true);
						}
					}
				}
			}
			RaycastHit hit2;
			bool flag3 = Physics.Raycast(muzzle.position, muzzle.forward, ref hit2, float.PositiveInfinity, LayerMask.GetMask(new string[] { "NPC", "Ragdoll", "Default", "DroppedItem", "MovingItem", "PlayerLocomotionObject" }));
			if (flag3)
			{
				bool flag4 = hit2.rigidbody != null;
				if (flag4)
				{
					bool flag5 = hit2.collider.gameObject.GetComponentInParent<Ragdoll>() != null;
					if (flag5)
					{
						Ragdoll rag = hit2.collider.gameObject.GetComponentInParent<Ragdoll>();
						Creature cr2 = hit2.collider.gameObject.GetComponentInParent<Ragdoll>().creature;
						RagdollPart ragdollPart = hit2.collider.gameObject.GetComponentInParent<RagdollPart>();
						if (drawDecal)
						{
							PlayerModule.DrawDecal(ragdollPart, hit2);
						}
						cr2.TryPush(2, muzzle.forward, 1, 0);
						float DamageToBeDealt = 0f;
						RagdollPart.Type type = ragdollPart.type;
						RagdollPart.Type type2 = type;
						if (type2 <= 32)
						{
							if (type2 <= 8)
							{
								switch (type2)
								{
								case 1:
								{
									bool flag6 = damage > 0f;
									if (flag6)
									{
										DamageToBeDealt = float.PositiveInfinity;
									}
									else
									{
										DamageToBeDealt = 0f;
									}
									cr2.TryPush(2, muzzle.forward, 3, 0);
									break;
								}
								case 2:
								{
									bool flag7 = damage > 0f;
									if (flag7)
									{
										DamageToBeDealt = float.PositiveInfinity;
									}
									else
									{
										DamageToBeDealt = 0f;
									}
									cr2.TryPush(2, muzzle.forward, 1, 0);
									break;
								}
								case 3:
									break;
								case 4:
									DamageToBeDealt = damage;
									cr2.TryPush(2, muzzle.forward, 2, 0);
									break;
								default:
									if (type2 == 8)
									{
										DamageToBeDealt = damage / 3f;
										cr2.TryPush(2, muzzle.forward, 1, 0);
										bool flag8 = !cr2.isKilled;
										if (flag8)
										{
											cr2.handRight.TryRelease();
										}
									}
									break;
								}
							}
							else if (type2 != 16)
							{
								if (type2 == 32)
								{
									DamageToBeDealt = damage / 4f;
									bool flag9 = !cr2.isKilled;
									if (flag9)
									{
										cr2.handLeft.TryRelease();
									}
								}
							}
							else
							{
								DamageToBeDealt = damage / 3f;
								cr2.TryPush(2, muzzle.forward, 1, 0);
								bool flag10 = !cr2.isKilled;
								if (flag10)
								{
									cr2.handRight.TryRelease();
								}
							}
						}
						else if (type2 <= 128)
						{
							if (type2 != 64)
							{
								if (type2 == 128)
								{
									DamageToBeDealt = damage / 3f;
									cr2.TryPush(2, muzzle.forward, 3, 0);
									bool flag11 = !cr2.isKilled;
									if (flag11)
									{
										cr2.ragdoll.SetState(1);
									}
								}
							}
							else
							{
								DamageToBeDealt = damage / 4f;
								bool flag12 = !cr2.isKilled;
								if (flag12)
								{
									cr2.handRight.TryRelease();
								}
							}
						}
						else if (type2 != 256)
						{
							if (type2 != 512)
							{
								if (type2 == 1024)
								{
									DamageToBeDealt = damage / 4f;
									cr2.TryPush(2, muzzle.forward, 3, 0);
									bool flag13 = !cr2.isKilled;
									if (flag13)
									{
										cr2.ragdoll.SetState(1);
									}
								}
							}
							else
							{
								DamageToBeDealt = damage / 4f;
								cr2.TryPush(2, muzzle.forward, 3, 0);
								bool flag14 = !cr2.isKilled;
								if (flag14)
								{
									cr2.ragdoll.SetState(1);
								}
							}
						}
						else
						{
							DamageToBeDealt = damage / 3f;
							cr2.TryPush(2, muzzle.forward, 3, 0);
							bool flag15 = !cr2.isKilled;
							if (flag15)
							{
								cr2.ragdoll.SetState(1);
							}
						}
						CollisionInstance coll = new CollisionInstance(new DamageStruct(1, damage), null, null);
						coll.damageStruct.damage = DamageToBeDealt;
						coll.damageStruct.damageType = 1;
						coll.sourceMaterial = Catalog.GetData<MaterialData>("Blade", true);
						coll.targetMaterial = Catalog.GetData<MaterialData>("Flesh", true);
						coll.targetColliderGroup = ragdollPart.colliderGroup;
						coll.contactPoint = hit2.point;
						coll.contactNormal = hit2.normal;
						Transform penPoint = new GameObject().transform;
						penPoint.position = hit2.point;
						penPoint.rotation = Quaternion.LookRotation(hit2.normal);
						penPoint.parent = hit2.transform;
						coll.damageStruct.penetration = 2;
						coll.damageStruct.penetrationPoint = penPoint;
						coll.damageStruct.penetrationDepth = 10f;
						coll.damageStruct.hitRagdollPart = ragdollPart;
						coll.intensity = DamageToBeDealt;
						coll.pressureRelativeVelocity = Vector3.one;
						cr2.Damage(coll);
						bool flag16 = slice && ragdollPart.sliceAllowed;
						if (flag16)
						{
							ragdollPart.TrySlice();
						}
					}
				}
			}
			Vector3 point = hit2.point;
			bool flag17 = true;
			float num;
			if (flag17)
			{
				num = hit2.distance;
			}
			else
			{
				num = 5000f;
			}
			return num;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000269C File Offset: 0x0000089C
		private Vector3 v(float z)
		{
			return new Vector3(1f, 1f, z);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000026C0 File Offset: 0x000008C0
		private static void DrawDecal(RagdollPart rp, RaycastHit hit)
		{
			EffectModuleReveal rem = (EffectModuleReveal)Catalog.GetData<EffectData>("HitBladeDecalFlesh", true).modules[4];
			List<RevealMaterialController> rmcs = new List<RevealMaterialController>();
			IEnumerable<Creature.RendererData> renderers = rp.renderers;
			Func<Creature.RendererData, bool> <>9__0;
			Func<Creature.RendererData, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (Creature.RendererData renderer) => rem != null && renderer.revealDecal && ((renderer.revealDecal.type == null && rem.typeFilter.HasFlag(1)) || (renderer.revealDecal.type == 1 && rem.typeFilter.HasFlag(2)) || (renderer.revealDecal.type == 2 && rem.typeFilter.HasFlag(4))));
			}
			foreach (Creature.RendererData r in renderers.Where(func))
			{
				rmcs.Add(r.revealDecal.revealMaterialController);
			}
			Transform rev = new GameObject().transform;
			rev.position = hit.point;
			rev.rotation = Quaternion.LookRotation(hit.normal);
			GameManager.local.StartCoroutine(RevealMaskProjection.ProjectAsync(rev.position + rev.forward * rem.offsetDistance, -rev.forward, rev.up, rem.depth, rem.maxSize, rem.maskTexture, rem.maxChannelMultiplier, rmcs, rem.revealData, null));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000281C File Offset: 0x00000A1C
		private void Update()
		{
			bool active = this.getActive();
			if (active)
			{
				Debug.Log("Setting laser");
				this.laserItem.transform.parent = Player.local.head.cam.transform;
				this.laserItem.transform.localPosition = Vector3.zero;
				this.laserItem.transform.localEulerAngles = Vector3.zero;
				bool draw = false;
				bool flag = this.framesSinceLastDecal == this.framesBetweenDecals;
				if (flag)
				{
					draw = true;
					this.framesSinceLastDecal = 0;
				}
				else
				{
					this.framesSinceLastDecal++;
				}
				float distanceL = PlayerModule.Hitscan(this.data.LeftLaserRoot, this.damage, draw, this.dismember);
				float distanceR = PlayerModule.Hitscan(this.data.RightLaserRoot, this.damage, draw, this.dismember);
				bool flag2 = this.data.LeftLaserRoot != null;
				if (flag2)
				{
					this.data.LeftLaserRoot.localScale = this.v(distanceL);
				}
				bool flag3 = this.data.RightLaserRoot != null;
				if (flag3)
				{
					this.data.RightLaserRoot.localScale = this.v(distanceR);
				}
				bool flag4 = this.data.LeftImpact != null;
				if (flag4)
				{
					this.data.LeftImpact.transform.position = this.data.LeftImpactPoint.position;
				}
				bool flag5 = this.data.LeftImpactAudio != null;
				if (flag5)
				{
					this.data.LeftImpactAudio.transform.position = this.data.LeftImpactPoint.position;
				}
				bool flag6 = this.data.RightImpact != null;
				if (flag6)
				{
					this.data.RightImpact.transform.position = this.data.RightImpactPoint.position;
				}
				bool flag7 = this.data.RightImpactAudio != null;
				if (flag7)
				{
					this.data.RightImpactAudio.transform.position = this.data.RightImpactPoint.position;
				}
			}
			else
			{
				this.Toggle(false);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002A60 File Offset: 0x00000C60
		public void Toggle(bool active)
		{
			bool flag = active && this.laserItem == null;
			if (flag)
			{
				Catalog.GetData<ItemData>("LaserEyes_Ghetto05_LaserEyes", true).SpawnAsync(delegate(Item item)
				{
					this.laserItem = item;
					this.data = item.GetComponent<Laser>();
					Volume vol = this.data.postProcessingVolumeTransform.GetComponent<Volume>();
					bool flag10 = this.overrideColor;
					if (flag10)
					{
						Bloom b;
						vol.profile.TryGet<Bloom>(ref b);
						Vignette v;
						vol.profile.TryGet<Vignette>(ref v);
						b.tint.Override(this.color);
						v.color.Override(this.color);
					}
					bool flag11 = !this.showVignette;
					if (flag11)
					{
						Vignette vignette;
						vol.profile.TryGet<Vignette>(ref vignette);
						vignette.active = false;
					}
					this.data.LeftLaserRoot.localScale = this.v(0f);
					this.data.RightLaserRoot.localScale = this.v(0f);
					bool flag12 = this.data.LeftImpact != null;
					if (flag12)
					{
						this.data.LeftImpact.Play();
					}
					bool flag13 = this.data.RightImpact != null;
					if (flag13)
					{
						this.data.RightImpact.Play();
					}
					bool flag14 = this.data.LeftImpactAudio != null && this.data.LeftImpactAudio.clip != null;
					if (flag14)
					{
						this.data.LeftImpactAudio.Play();
					}
					bool flag15 = this.data.RightImpactAudio != null && this.data.RightImpactAudio.clip != null;
					if (flag15)
					{
						this.data.RightImpactAudio.Play();
					}
					bool flag16 = this.playLaserSound && this.useV1Sound && this.data.v1Loop != null && this.data.v1Loop.clip != null;
					if (flag16)
					{
						this.data.v1Loop.Play();
					}
					bool flag17 = this.playLaserSound && this.useV2Sound && this.data.v2Loop != null && this.data.v2Loop.clip != null;
					if (flag17)
					{
						this.data.v2Loop.Play();
					}
					bool flag18 = this.playLaserSound && this.useV3Sound && this.data.v3Loop != null && this.data.v3Loop.clip != null;
					if (flag18)
					{
						this.data.v3Loop.Play();
					}
					item.transform.parent = Player.local.head.cam.transform;
				}, new Vector3?(Player.local.head.cam.transform.position), new Quaternion?(Player.local.head.cam.transform.rotation), Player.local.head.cam.transform, true, null);
			}
			else
			{
				bool flag2 = this.laserItem != null;
				if (flag2)
				{
					bool flag3 = this.data.LeftImpact != null;
					if (flag3)
					{
						this.data.LeftImpact.Stop();
					}
					bool flag4 = this.data.RightImpact != null;
					if (flag4)
					{
						this.data.RightImpact.Stop();
					}
					bool flag5 = this.data.LeftImpactAudio != null;
					if (flag5)
					{
						this.data.LeftImpactAudio.Stop();
					}
					bool flag6 = this.data.RightImpactAudio != null;
					if (flag6)
					{
						this.data.RightImpactAudio.Stop();
					}
					bool flag7 = this.data.v1Loop != null;
					if (flag7)
					{
						this.data.v1Loop.Stop();
					}
					bool flag8 = this.data.v2Loop != null;
					if (flag8)
					{
						this.data.v2Loop.Stop();
					}
					bool flag9 = this.data.v3Loop != null;
					if (flag9)
					{
						this.data.v2Loop.Stop();
					}
					this.laserItem.Despawn();
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002C30 File Offset: 0x00000E30
		private bool getActive()
		{
			bool left = Player.local.creature.mana.casterLeft.spellInstance != null && Player.local.creature.mana.casterLeft.spellInstance.id.Equals("LaserEyesSpell") && Player.local.creature.mana.casterLeft.isFiring;
			bool right = Player.local.creature.mana.casterRight.spellInstance != null && Player.local.creature.mana.casterRight.spellInstance.id.Equals("LaserEyesSpell") && Player.local.creature.mana.casterRight.isFiring;
			return left || right;
		}

		// Token: 0x0400000F RID: 15
		private Laser data;

		// Token: 0x04000010 RID: 16
		private Item laserItem;

		// Token: 0x04000011 RID: 17
		public float damage;

		// Token: 0x04000012 RID: 18
		public bool dismember;

		// Token: 0x04000013 RID: 19
		public int framesBetweenDecals;

		// Token: 0x04000014 RID: 20
		public bool showVignette;

		// Token: 0x04000015 RID: 21
		private int framesSinceLastDecal = 0;

		// Token: 0x04000016 RID: 22
		public bool overrideColor;

		// Token: 0x04000017 RID: 23
		public Color color;

		// Token: 0x04000018 RID: 24
		public bool playLaserSound;

		// Token: 0x04000019 RID: 25
		public bool useV1Sound;

		// Token: 0x0400001A RID: 26
		public bool useV2Sound;

		// Token: 0x0400001B RID: 27
		public bool useV3Sound;
	}
}

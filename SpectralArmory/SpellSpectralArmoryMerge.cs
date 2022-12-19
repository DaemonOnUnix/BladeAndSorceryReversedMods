using System;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SpectralArmory
{
	// Token: 0x02000006 RID: 6
	public class SpellSpectralArmoryMerge : SpellMergeData
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000033CC File Offset: 0x000015CC
		public override void Load(Mana mana)
		{
			bool flag = this.useSpectralArmoryShader;
			if (flag)
			{
				Addressables.LoadAssetAsync<Shader>(this.SpectralArmoryShader).Completed += delegate(AsyncOperationHandle<Shader> handle)
				{
					bool flag4 = handle.Result == null;
					if (flag4)
					{
						Debug.Log("Spectral Armory Error: Couldn't Find Spectral Armory Shader");
						this.ShaderToUse = null;
					}
					else
					{
						this.ShaderToUse = handle.Result;
						this.isSAShader = true;
					}
				};
			}
			bool flag2 = this.useOldSpectralArmoryShader;
			if (flag2)
			{
				Addressables.LoadAssetAsync<Shader>(this.oldShader).Completed += delegate(AsyncOperationHandle<Shader> handle)
				{
					bool flag4 = handle.Result == null;
					if (flag4)
					{
						Debug.Log("Spectral Armory Error: Couldn't Find Spectral Armory Shader");
						this.ShaderToUse = null;
					}
					else
					{
						this.ShaderToUse = handle.Result;
						this.isSAShader = true;
					}
				};
			}
			else
			{
				bool flag3 = this.useCustomShader;
				if (flag3)
				{
					Addressables.LoadAssetAsync<Shader>(this.CustomShaderAddress).Completed += delegate(AsyncOperationHandle<Shader> handle)
					{
						bool flag4 = handle.Result == null;
						if (flag4)
						{
							Debug.Log("Spectral Armory Error: Couldn't Find Custom Shader");
							this.ShaderToUse = null;
						}
						else
						{
							this.ShaderToUse = handle.Result;
							this.isSAShader = false;
						}
					};
				}
				else
				{
					this.ShaderToUse = null;
					this.isSAShader = false;
				}
			}
			base.Load(mana);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000347C File Offset: 0x0000167C
		public override void Merge(bool active)
		{
			base.Merge(active);
			this.isCasting = active;
			if (active)
			{
				this.Left = Player.currentCreature.handLeft;
				this.Right = Player.currentCreature.handRight;
				this.Explode = false;
				foreach (Effect vfx in this.chargeEffect.effects)
				{
					bool flag = vfx is EffectVfx;
					if (flag)
					{
						EffectVfx v = (EffectVfx)vfx;
						v.SetTarget(Player.local.head.cam.transform);
						v.vfx.SetBool("isEnding", false);
					}
				}
			}
			else
			{
				foreach (Effect fx in this.chargeEffect.effects)
				{
					bool flag2 = fx is EffectVfx;
					if (flag2)
					{
						EffectVfx v2 = (EffectVfx)fx;
						v2.SetTarget(Player.local.head.cam.transform);
						bool explode = this.Explode;
						if (explode)
						{
							v2.vfx.SetBool("isEnding", true);
						}
					}
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003600 File Offset: 0x00001800
		public override void Update()
		{
			base.Update();
			bool flag = this.isCasting;
			if (flag)
			{
				Vector3 velocityL = this.Left.transform.InverseTransformVector(this.Left.Velocity() - Player.currentCreature.ragdoll.headPart.rb.velocity);
				Vector3 velocityR = this.Right.transform.InverseTransformVector(this.Right.Velocity() - Player.currentCreature.ragdoll.headPart.rb.velocity);
				bool flag2 = Vector3.Distance(this.test, velocityL) > Vector3.Distance(this.test, velocityR);
				if (flag2)
				{
					switch (velocityL.GetDirection())
					{
					case Direction.X:
						this.CastX(velocityL, this.Left);
						break;
					case Direction.Y:
						this.CastY(velocityL, this.Left);
						break;
					case Direction.Z:
						this.CastZ(velocityL, this.Left);
						break;
					}
				}
				else
				{
					switch (velocityR.GetDirection())
					{
					case Direction.X:
						this.CastX(velocityR, this.Right);
						break;
					case Direction.Y:
						this.CastY(velocityR, this.Right);
						break;
					case Direction.Z:
						this.CastZ(velocityR, this.Right);
						break;
					}
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003764 File Offset: 0x00001964
		public void CastX(Vector3 velocity, RagdollHand hand)
		{
			bool flag = this.isCasting && ((hand.side == null && velocity.x < -2f) || (hand.side == 1 && velocity.x < -2f)) && this.mana.CanConsumeMana(this.Summon_Mana_Cost);
			if (flag)
			{
				this.mana.ConsumeMana(this.Summon_Mana_Cost);
				Catalog.GetData<ItemData>(this.MergeForwardItemID, true).SpawnAsync(delegate(Item item)
				{
					SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
					module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
					hand.Grab(item.GetMainHandle(hand.side));
				}, new Vector3?(hand.grip.position), new Quaternion?(hand.grip.rotation), null, true, null);
				this.isCasting = false;
				this.mana.casterLeft.intensity = 0f;
				this.mana.casterRight.intensity = 0f;
				this.mana.casterLeft.Fire(false);
				this.mana.casterRight.Fire(false);
				this.Explode = true;
				this.Merge(false);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000038A4 File Offset: 0x00001AA4
		public void CastY(Vector3 velocity, RagdollHand hand)
		{
			bool flag = this.isCasting && ((hand.side == null && velocity.y > 2f) || (hand.side == 1 && velocity.y < -2f)) && this.mana.CanConsumeMana(this.Summon_Mana_Cost);
			if (flag)
			{
				this.mana.ConsumeMana(this.Summon_Mana_Cost);
				Catalog.GetData<ItemData>(this.MergeUpItemID, true).SpawnAsync(delegate(Item item)
				{
					SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
					module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
					hand.Grab(item.GetMainHandle(hand.side));
				}, new Vector3?(hand.grip.position), new Quaternion?(hand.grip.rotation), null, true, null);
				this.isCasting = false;
				this.mana.casterLeft.intensity = 0f;
				this.mana.casterRight.intensity = 0f;
				this.mana.casterLeft.Fire(false);
				this.mana.casterRight.Fire(false);
				this.Explode = true;
				this.Merge(false);
			}
			else
			{
				bool flag2 = this.isCasting && ((hand.side == null && velocity.y < -2f) || (hand.side == 1 && velocity.y > 2f)) && this.mana.CanConsumeMana(this.Summon_Mana_Cost);
				if (flag2)
				{
					this.mana.ConsumeMana(this.Summon_Mana_Cost);
					Catalog.GetData<ItemData>(this.MergeDownItemID, true).SpawnAsync(delegate(Item item)
					{
						SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
						module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
						hand.Grab(item.GetMainHandle(hand.side));
					}, new Vector3?(hand.grip.position), new Quaternion?(hand.grip.rotation), null, true, null);
					this.isCasting = false;
					this.mana.casterLeft.intensity = 0f;
					this.mana.casterRight.intensity = 0f;
					this.mana.casterLeft.Fire(false);
					this.mana.casterRight.Fire(false);
					this.Explode = true;
					this.Merge(false);
				}
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003B04 File Offset: 0x00001D04
		public void CastZ(Vector3 velocity, RagdollHand hand)
		{
			bool flag = this.isCasting && ((hand.side == null && velocity.z > 2f) || (hand.side == 1 && velocity.z > 2f)) && this.mana.CanConsumeMana(this.Summon_Mana_Cost);
			if (flag)
			{
				this.mana.ConsumeMana(this.Summon_Mana_Cost);
				Catalog.GetData<ItemData>(this.MergeOutItemID, true).SpawnAsync(delegate(Item item)
				{
					SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
					module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
					hand.Grab(item.GetMainHandle(hand.side));
				}, new Vector3?(hand.grip.position), new Quaternion?(hand.grip.rotation), null, true, null);
				this.isCasting = false;
				this.mana.casterLeft.intensity = 0f;
				this.mana.casterRight.intensity = 0f;
				this.mana.casterLeft.Fire(false);
				this.mana.casterRight.Fire(false);
				this.Explode = true;
				this.Merge(false);
			}
			bool flag2 = this.isCasting && ((hand.side == null && velocity.z < -2f) || (hand.side == 1 && velocity.z < -2f)) && this.mana.CanConsumeMana(this.Summon_Mana_Cost);
			if (flag2)
			{
				this.mana.ConsumeMana(this.Summon_Mana_Cost);
				Catalog.GetData<ItemData>(this.MergeInItemID, true).SpawnAsync(delegate(Item item)
				{
					SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
					module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
					hand.Grab(item.GetMainHandle(hand.side));
				}, new Vector3?(hand.grip.position), new Quaternion?(hand.grip.rotation), null, true, null);
				this.isCasting = false;
				this.mana.casterLeft.intensity = 0f;
				this.mana.casterRight.intensity = 0f;
				this.mana.casterLeft.Fire(false);
				this.mana.casterRight.Fire(false);
				this.Explode = true;
				this.Merge(false);
			}
		}

		// Token: 0x04000024 RID: 36
		private bool isCasting;

		// Token: 0x04000025 RID: 37
		public float Summon_Mana_Cost;

		// Token: 0x04000026 RID: 38
		public string MergeUpItemID;

		// Token: 0x04000027 RID: 39
		public string MergeDownItemID;

		// Token: 0x04000028 RID: 40
		public string MergeForwardItemID;

		// Token: 0x04000029 RID: 41
		public string MergeOutItemID;

		// Token: 0x0400002A RID: 42
		public string MergeInItemID;

		// Token: 0x0400002B RID: 43
		public bool useSpectralArmoryShader;

		// Token: 0x0400002C RID: 44
		public bool useOldSpectralArmoryShader;

		// Token: 0x0400002D RID: 45
		public Color SpectralColor;

		// Token: 0x0400002E RID: 46
		public Color DissolveColor;

		// Token: 0x0400002F RID: 47
		public bool useCustomShader;

		// Token: 0x04000030 RID: 48
		public string CustomShaderAddress;

		// Token: 0x04000031 RID: 49
		public float DisappearInSeconds;

		// Token: 0x04000032 RID: 50
		private Shader ShaderToUse;

		// Token: 0x04000033 RID: 51
		private bool isSAShader;

		// Token: 0x04000034 RID: 52
		private string SpectralArmoryShader = "GrooveSlinger.SpectralArmory.Shader";

		// Token: 0x04000035 RID: 53
		private string oldShader = "GrooveSlinger.SpectralArmory.OldShader";

		// Token: 0x04000036 RID: 54
		private RagdollHand Left;

		// Token: 0x04000037 RID: 55
		private RagdollHand Right;

		// Token: 0x04000038 RID: 56
		private Vector3 test = new Vector3(0f, 0f, 0f);

		// Token: 0x04000039 RID: 57
		private bool Explode;
	}
}

using System;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SpectralArmory
{
	// Token: 0x02000004 RID: 4
	public class SpellSpectralArmory : SpellCastCharge
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002308 File Offset: 0x00000508
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
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
			base.Load(spellCaster, level);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023B7 File Offset: 0x000005B7
		public override void Unload()
		{
			base.Unload();
			this.isCasting = false;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000023C8 File Offset: 0x000005C8
		public override void Fire(bool active)
		{
			base.Fire(active);
			this.isCasting = active;
			if (active)
			{
				this.Explode = false;
				foreach (Effect vfx in this.chargeEffectInstance.effects)
				{
					bool flag = vfx is EffectVfx;
					if (flag)
					{
						EffectVfx v = (EffectVfx)vfx;
						v.vfx.SetBool("isEnding", false);
					}
				}
			}
			else
			{
				this.spellCaster.StopFingersEffect();
				foreach (Effect vfx2 in this.chargeEffectInstance.effects)
				{
					bool flag2 = vfx2 is EffectVfx;
					if (flag2)
					{
						EffectVfx v2 = (EffectVfx)vfx2;
						bool explode = this.Explode;
						if (explode)
						{
							v2.vfx.SetBool("isEnding", true);
						}
						v2.gameObject.transform.parent = null;
					}
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002510 File Offset: 0x00000710
		public override void UpdateCaster()
		{
			base.UpdateCaster();
			bool flag = this.isCasting && this.spellCaster.ragdollHand.IsEmpty();
			if (flag)
			{
				Vector3 velocity = this.spellCaster.ragdollHand.transform.InverseTransformVector(this.spellCaster.ragdollHand.Velocity() - Player.currentCreature.ragdoll.headPart.rb.velocity);
				switch (velocity.GetDirection())
				{
				case Direction.X:
					this.CastX(velocity);
					break;
				case Direction.Y:
					this.CastY(velocity);
					break;
				case Direction.Z:
					this.CastZ(velocity);
					break;
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025CC File Offset: 0x000007CC
		public void CastX(Vector3 velocity)
		{
			bool flag = this.isCasting && ((this.spellCaster.ragdollHand.side == null && velocity.x < -2f) || (this.spellCaster.ragdollHand.side == 1 && velocity.x < -2f)) && this.spellCaster.mana.CanConsumeMana(this.Summon_Mana_Cost);
			if (flag)
			{
				this.spellCaster.mana.ConsumeMana(this.Summon_Mana_Cost);
				Catalog.GetData<ItemData>(this.ForwardItemID, true).SpawnAsync(delegate(Item item)
				{
					SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
					module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
					this.spellCaster.ragdollHand.Grab(item.GetMainHandle(this.spellCaster.ragdollHand.side));
				}, new Vector3?(this.spellCaster.ragdollHand.grip.position), new Quaternion?(this.spellCaster.ragdollHand.grip.rotation), null, true, null);
				this.isCasting = false;
				this.spellCaster.intensity = 0f;
				this.Explode = true;
				this.spellCaster.Fire(false);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000026DC File Offset: 0x000008DC
		public void CastY(Vector3 velocity)
		{
			bool flag = this.isCasting && ((this.spellCaster.ragdollHand.side == null && velocity.y > 2f) || (this.spellCaster.ragdollHand.side == 1 && velocity.y < -2f)) && this.spellCaster.mana.CanConsumeMana(this.Summon_Mana_Cost);
			if (flag)
			{
				this.spellCaster.mana.ConsumeMana(this.Summon_Mana_Cost);
				Catalog.GetData<ItemData>(this.UpItemID, true).SpawnAsync(delegate(Item item)
				{
					SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
					module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
					this.spellCaster.ragdollHand.Grab(item.GetMainHandle(this.spellCaster.ragdollHand.side));
				}, new Vector3?(this.spellCaster.ragdollHand.grip.position), new Quaternion?(this.spellCaster.ragdollHand.grip.rotation), null, true, null);
				this.isCasting = false;
				this.spellCaster.intensity = 0f;
				this.Explode = true;
				this.spellCaster.Fire(false);
			}
			else
			{
				bool flag2 = this.isCasting && ((this.spellCaster.ragdollHand.side == null && velocity.y < -2f) || (this.spellCaster.ragdollHand.side == 1 && velocity.y > 2f)) && this.spellCaster.mana.CanConsumeMana(this.Summon_Mana_Cost);
				if (flag2)
				{
					this.spellCaster.mana.ConsumeMana(this.Summon_Mana_Cost);
					Catalog.GetData<ItemData>(this.DownItemID, true).SpawnAsync(delegate(Item item)
					{
						SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
						module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
						this.spellCaster.ragdollHand.Grab(item.GetMainHandle(this.spellCaster.ragdollHand.side));
					}, new Vector3?(this.spellCaster.ragdollHand.grip.position), new Quaternion?(this.spellCaster.ragdollHand.grip.rotation), null, true, null);
					this.isCasting = false;
					this.spellCaster.intensity = 0f;
					this.Explode = true;
					this.spellCaster.Fire(false);
				}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000028F0 File Offset: 0x00000AF0
		public void CastZ(Vector3 velocity)
		{
			bool flag = this.isCasting && ((this.spellCaster.ragdollHand.side == null && velocity.z > 2f) || (this.spellCaster.ragdollHand.side == 1 && velocity.z > 2f)) && this.spellCaster.mana.CanConsumeMana(this.Summon_Mana_Cost);
			if (flag)
			{
				this.spellCaster.mana.ConsumeMana(this.Summon_Mana_Cost);
				Catalog.GetData<ItemData>(this.OutItemID, true).SpawnAsync(delegate(Item item)
				{
					SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
					module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
					this.spellCaster.ragdollHand.Grab(item.GetMainHandle(this.spellCaster.ragdollHand.side));
				}, new Vector3?(this.spellCaster.ragdollHand.grip.position), new Quaternion?(this.spellCaster.ragdollHand.grip.rotation), null, true, null);
				this.isCasting = false;
				this.spellCaster.intensity = 0f;
				this.Explode = true;
				this.spellCaster.Fire(false);
			}
			bool flag2 = this.isCasting && ((this.spellCaster.ragdollHand.side == null && velocity.z < -2f) || (this.spellCaster.ragdollHand.side == 1 && velocity.z < -2f)) && this.spellCaster.mana.CanConsumeMana(this.Summon_Mana_Cost);
			if (flag2)
			{
				this.spellCaster.mana.ConsumeMana(this.Summon_Mana_Cost);
				Catalog.GetData<ItemData>(this.InItemID, true).SpawnAsync(delegate(Item item)
				{
					SpectralArmorySummon module = item.gameObject.AddComponent<SpectralArmorySummon>();
					module.SetUp(this.isSAShader, this.ShaderToUse, this.DisappearInSeconds, this.SpectralColor, this.DissolveColor);
					this.spellCaster.ragdollHand.Grab(item.GetMainHandle(this.spellCaster.ragdollHand.side));
				}, new Vector3?(this.spellCaster.ragdollHand.grip.position), new Quaternion?(this.spellCaster.ragdollHand.grip.rotation), null, true, null);
				this.isCasting = false;
				this.spellCaster.intensity = 0f;
				this.Explode = true;
				this.spellCaster.Fire(false);
			}
		}

		// Token: 0x04000005 RID: 5
		private bool isCasting;

		// Token: 0x04000006 RID: 6
		public float Summon_Mana_Cost;

		// Token: 0x04000007 RID: 7
		public string UpItemID;

		// Token: 0x04000008 RID: 8
		public string DownItemID;

		// Token: 0x04000009 RID: 9
		public string ForwardItemID;

		// Token: 0x0400000A RID: 10
		public string OutItemID;

		// Token: 0x0400000B RID: 11
		public string InItemID;

		// Token: 0x0400000C RID: 12
		public bool useSpectralArmoryShader;

		// Token: 0x0400000D RID: 13
		public bool useOldSpectralArmoryShader;

		// Token: 0x0400000E RID: 14
		public Color SpectralColor;

		// Token: 0x0400000F RID: 15
		public Color DissolveColor;

		// Token: 0x04000010 RID: 16
		public bool useCustomShader;

		// Token: 0x04000011 RID: 17
		public string CustomShaderAddress;

		// Token: 0x04000012 RID: 18
		public float DisappearInSeconds;

		// Token: 0x04000013 RID: 19
		public Shader ShaderToUse;

		// Token: 0x04000014 RID: 20
		private bool isSAShader;

		// Token: 0x04000015 RID: 21
		private bool Explode;

		// Token: 0x04000016 RID: 22
		private string SpectralArmoryShader = "GrooveSlinger.SpectralArmory.Shader";

		// Token: 0x04000017 RID: 23
		private string oldShader = "GrooveSlinger.SpectralArmory.OldShader";
	}
}

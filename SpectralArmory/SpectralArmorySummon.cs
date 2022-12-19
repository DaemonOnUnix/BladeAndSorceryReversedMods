using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace SpectralArmory
{
	// Token: 0x02000005 RID: 5
	public class SpectralArmorySummon : MonoBehaviour
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002E0A File Offset: 0x0000100A
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002E1C File Offset: 0x0000101C
		public void SetUp(bool isSAShader, Shader shader, float DisTime, Color SpectralColor, Color DissolveColor)
		{
			this.shaderReceived = shader;
			this.SpecColor = SpectralColor;
			this.DisColor = DissolveColor;
			this.isBow = false;
			if (isSAShader)
			{
				this.SpecAShader = true;
				foreach (Renderer r in this.item.renderers)
				{
					foreach (Material i in r.materials)
					{
						i.shader = shader;
						i.SetColor("SpectralColor", SpectralColor);
						i.SetColor("DissolveColor", DissolveColor);
					}
				}
			}
			else
			{
				bool flag = shader != null;
				if (flag)
				{
					foreach (Renderer r2 in this.item.renderers)
					{
						foreach (Material j in r2.materials)
						{
							j.shader = shader;
						}
					}
				}
			}
			this.AdditionalSettings(isSAShader, shader, DisTime, SpectralColor, DissolveColor);
			this.DisappearTimeMax = DisTime;
			this.DespawnTimer = this.DisappearTimeMax;
			this.isReady = true;
			this.OnStart();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002FA0 File Offset: 0x000011A0
		public void OnStart()
		{
			base.StartCoroutine(this.holderCheck());
			bool flag = this.isReady && this.SpecAShader;
			if (flag)
			{
				foreach (Renderer r in this.item.renderers)
				{
					base.StartCoroutine(this.SpawnIn(r));
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003028 File Offset: 0x00001228
		public void AdditionalSettings(bool useShader, Shader shader, float DisTime, Color SpecColor, Color DissolveColor)
		{
			this.bowString = this.item.GetComponentInChildren<BowString>();
			bool flag = this.bowString != null;
			if (flag)
			{
				this.isBow = true;
				this.isLoaded = false;
				this.bowString.module.autoSpawnArrow = true;
				this.bowString.module.arrowProjectileID = "GrooveSlinger.SpectralArmory.SummonedArrow";
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003090 File Offset: 0x00001290
		public void Update()
		{
			bool flag = this.isReady;
			if (flag)
			{
				bool flag2 = this.isBow;
				if (flag2)
				{
					bool flag3 = this.bowString.loadedArrow != null && !this.isLoaded;
					if (flag3)
					{
						this.isLoaded = true;
						bool flag4 = this.bowString.loadedArrow.data.id == "GrooveSlinger.SpectralArmory.SummonedArrow";
						if (flag4)
						{
							SpectralArrow sArrow = this.bowString.loadedArrow.gameObject.AddComponent<SpectralArrow>();
							sArrow.SetUp(this.SpecAShader, this.shaderReceived, this.DisappearTimeMax, this.SpecColor, this.DisColor);
						}
					}
					else
					{
						bool flag5 = this.isLoaded && this.bowString.loadedArrow == null;
						if (flag5)
						{
							this.isLoaded = false;
						}
					}
				}
				bool flag6 = this.item.holder != null || this.item.mainHandler != null || this.item.isTelekinesisGrabbed || this.item.isGripped;
				if (flag6)
				{
					this.DespawnTimer = this.DisappearTimeMax;
				}
				else
				{
					bool flag7 = this.DespawnTimer > 0f;
					if (flag7)
					{
						this.DespawnTimer -= Time.deltaTime;
					}
					else
					{
						bool despawnNow = this.DespawnNow;
						if (despawnNow)
						{
							foreach (CollisionHandler handler in this.item.collisionHandlers)
							{
								foreach (Damager damager in handler.damagers)
								{
									damager.UnPenetrateAll();
								}
							}
							this.item.Despawn(2f);
						}
						else
						{
							this.item.mainHandleLeft.data.disableTouch = true;
							this.item.mainHandleLeft.data.allowTelekinesis = false;
							bool specAShader = this.SpecAShader;
							if (specAShader)
							{
								foreach (Renderer r in this.item.renderers)
								{
									bool flag8 = r.material.HasProperty("DissolveFade");
									if (flag8)
									{
										base.StartCoroutine(this.SpawnOut(r));
									}
								}
							}
							else
							{
								this.DespawnNow = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003370 File Offset: 0x00001570
		public IEnumerator SpawnIn(Renderer r)
		{
			float tts = 0.75f;
			float timeElapsed = 0f;
			float toStart = 1f;
			float current = 1f;
			float toHit = 0f;
			foreach (Material i in r.materials)
			{
				i.SetFloat("DissolveFade", current);
				i = null;
			}
			Material[] array = null;
			while (timeElapsed <= tts)
			{
				current = Mathf.Lerp(toStart, toHit, timeElapsed / tts);
				foreach (Material j in r.materials)
				{
					j.SetFloat("DissolveFade", current);
					j = null;
				}
				Material[] array2 = null;
				timeElapsed += Time.deltaTime;
				yield return null;
			}
			r.material.SetFloat("DissolveFade", toHit);
			yield break;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003386 File Offset: 0x00001586
		public IEnumerator SpawnOut(Renderer r)
		{
			float tts = 0.75f;
			float timeElapsed = 0f;
			float toStart = 0f;
			float current = 0f;
			float toHit = 1f;
			foreach (Holder h in this.item.childHolders)
			{
				foreach (Item i in h.items)
				{
					SpectralArmorySummon b = i.GetComponent<SpectralArmorySummon>();
					bool flag = b != null;
					if (flag)
					{
						foreach (Renderer br in b.item.renderers)
						{
							b.StartCoroutine(this.SpawnOut(br));
							br = null;
						}
						List<Renderer>.Enumerator enumerator3 = default(List<Renderer>.Enumerator);
					}
					b = null;
					i = null;
				}
				List<Item>.Enumerator enumerator2 = default(List<Item>.Enumerator);
				h = null;
			}
			List<Holder>.Enumerator enumerator = default(List<Holder>.Enumerator);
			foreach (Material j in r.materials)
			{
				j.SetFloat("DissolveFade", current);
				j = null;
			}
			Material[] array = null;
			while (timeElapsed <= tts)
			{
				timeElapsed += Time.deltaTime;
				current = Mathf.Lerp(toStart, toHit, timeElapsed / tts);
				foreach (Material k in r.materials)
				{
					k.SetFloat("DissolveFade", current);
					k = null;
				}
				Material[] array2 = null;
				yield return null;
			}
			r.material.SetFloat("DissolveFade", toHit);
			this.DespawnNow = true;
			yield break;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000339C File Offset: 0x0000159C
		public IEnumerator holderCheck()
		{
			yield return new WaitForSeconds(0.05f);
			foreach (Holder h in this.item.childHolders)
			{
				foreach (Item i in h.items)
				{
					SpectralArmorySummon e = i.gameObject.AddComponent<SpectralArmorySummon>();
					e.SetUp(this.SpecAShader, this.shaderReceived, this.DisappearTimeMax, this.SpecColor, this.DisColor);
					e = null;
					i = null;
				}
				List<Item>.Enumerator enumerator2 = default(List<Item>.Enumerator);
				h = null;
			}
			List<Holder>.Enumerator enumerator = default(List<Holder>.Enumerator);
			yield break;
		}

		// Token: 0x04000018 RID: 24
		private Item item;

		// Token: 0x04000019 RID: 25
		private bool isReady = false;

		// Token: 0x0400001A RID: 26
		private float DisappearTimeMax;

		// Token: 0x0400001B RID: 27
		private float DespawnTimer;

		// Token: 0x0400001C RID: 28
		private bool SpecAShader = false;

		// Token: 0x0400001D RID: 29
		private bool DespawnNow = false;

		// Token: 0x0400001E RID: 30
		private Shader shaderReceived;

		// Token: 0x0400001F RID: 31
		private Color SpecColor;

		// Token: 0x04000020 RID: 32
		private Color DisColor;

		// Token: 0x04000021 RID: 33
		private bool isBow;

		// Token: 0x04000022 RID: 34
		private bool isLoaded;

		// Token: 0x04000023 RID: 35
		private BowString bowString;
	}
}

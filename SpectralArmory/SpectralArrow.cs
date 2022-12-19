using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace SpectralArmory
{
	// Token: 0x02000007 RID: 7
	public class SpectralArrow : MonoBehaviour
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00003E7B File Offset: 0x0000207B
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003E8C File Offset: 0x0000208C
		public void SetUp(bool isSAShader, Shader shader, float DisTime, Color SpectralColor, Color DissolveColor)
		{
			this.shaderReceived = shader;
			this.SpecColor = SpectralColor;
			this.DisColor = DissolveColor;
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
			this.DisappearTimeMax = DisTime;
			this.DespawnTimer = this.DisappearTimeMax;
			this.isReady = true;
			this.OnStart();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003FFC File Offset: 0x000021FC
		public void OnStart()
		{
			bool flag = this.isReady && this.SpecAShader;
			if (flag)
			{
				foreach (Renderer r in this.item.renderers)
				{
					base.StartCoroutine(this.SpawnIn(r));
				}
			}
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004094 File Offset: 0x00002294
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			this.item.mainHandleLeft.data.disableTouch = true;
			this.item.mainHandleLeft.data.allowTelekinesis = false;
			foreach (Renderer r in this.item.renderers)
			{
				bool specAShader = this.SpecAShader;
				if (specAShader)
				{
					base.StartCoroutine(this.SpawnOut(r));
				}
				else
				{
					this.DespawnNow = true;
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000413C File Offset: 0x0000233C
		public void Update()
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
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000041FC File Offset: 0x000023FC
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

		// Token: 0x06000036 RID: 54 RVA: 0x00004212 File Offset: 0x00002412
		public IEnumerator SpawnOut(Renderer r)
		{
			float tts = 0.75f;
			float timeElapsed = 0f;
			float toStart = 0f;
			float current = 0f;
			float toHit = 1f;
			foreach (Material i in r.materials)
			{
				i.SetFloat("DissolveFade", current);
				i = null;
			}
			Material[] array = null;
			while (timeElapsed <= tts)
			{
				timeElapsed += Time.deltaTime;
				current = Mathf.Lerp(toStart, toHit, timeElapsed / tts);
				foreach (Material j in r.materials)
				{
					j.SetFloat("DissolveFade", current);
					j = null;
				}
				Material[] array2 = null;
				yield return null;
			}
			r.material.SetFloat("DissolveFade", toHit);
			this.DespawnNow = true;
			yield break;
		}

		// Token: 0x0400003A RID: 58
		private Item item;

		// Token: 0x0400003B RID: 59
		private bool isReady = false;

		// Token: 0x0400003C RID: 60
		private float DisappearTimeMax;

		// Token: 0x0400003D RID: 61
		private float DespawnTimer;

		// Token: 0x0400003E RID: 62
		private bool SpecAShader = false;

		// Token: 0x0400003F RID: 63
		private bool DespawnNow = false;

		// Token: 0x04000040 RID: 64
		private Shader shaderReceived;

		// Token: 0x04000041 RID: 65
		private Color SpecColor;

		// Token: 0x04000042 RID: 66
		private Color DisColor;
	}
}

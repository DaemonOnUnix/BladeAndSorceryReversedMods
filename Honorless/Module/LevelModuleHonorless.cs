using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Wully.Mono;
using Wully.RenderPass;
using Wully.Utils;

namespace Wully.Module
{
	// Token: 0x0200000F RID: 15
	public class LevelModuleHonorless : LevelModule
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00004C0C File Offset: 0x00002E0C
		private void Setup()
		{
			try
			{
				if (LevelModuleHonorless.local == null)
				{
					LevelModuleHonorless.local = this;
					Debug.Log("Manifest: " + Extensions.GetManifest(typeof(LevelModuleHonorless)));
				}
			}
			catch (Exception exception)
			{
				Debug.Log(string.Format("Error patching Wully.Honorless: {0}", exception));
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004C6C File Offset: 0x00002E6C
		public override IEnumerator OnLoadCoroutine()
		{
			this.Setup();
			this.movement = this.level.gameObject.AddComponent<Movement>();
			EventManager.onPossess += new EventManager.PossessEvent(this.EventManager_onPossess);
			EventManager.onUnpossess += new EventManager.PossessEvent(this.EventManager_onUnpossess);
			this.AddTips();
			bool flag = this.enableDishonoredShader;
			return base.OnLoadCoroutine();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004CCC File Offset: 0x00002ECC
		private void AddRendererFeatures()
		{
			this.material = Addressables.LoadAssetAsync<Material>(this.shaderMaterialAddress).WaitForCompletion();
			if (!this.material)
			{
				Debug.LogError("Couldn't load material from " + this.shaderMaterialAddress);
				return;
			}
			this.customPassBlit = new GameObject("DishonoredShader")
			{
				transform = 
				{
					parent = this.level.transform
				}
			}.AddComponent<CustomPassBlit>();
			this.customPassBlit.settings.blitMaterial = this.material;
			this.customPassBlit.CreateFeature();
			this.customPassBlit.EnableFeature();
			Debug.Log("Added dishonored shader");
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004D78 File Offset: 0x00002F78
		private void AddBrains()
		{
			BrainModuleAlert alert = new BrainModuleAlert();
			foreach (CatalogData catalogData in Catalog.GetDataList(7))
			{
				BrainData brainData = catalogData as BrainData;
				if (brainData != null && brainData.id != "Player")
				{
					Debug.Log("Added alert module to " + brainData.id);
					brainData.modules.Add(alert);
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004E08 File Offset: 0x00003008
		private void AddTips()
		{
			TextData textData = Catalog.GetTextData();
			for (int i = 0; i < textData.textGroups.Count; i++)
			{
				TextData.TextGroup group = textData.textGroups[i];
				if (group.id.Equals("Tips"))
				{
					int id = group.texts.Count;
					foreach (string t in this.tips)
					{
						id++;
						TextData.TextID tip = new TextData.TextID
						{
							id = id.ToString(),
							text = t
						};
						group.texts.Add(tip);
					}
				}
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004ED0 File Offset: 0x000030D0
		private void EventManager_onPossess(Creature creature, EventTime eventTime)
		{
			if (eventTime == null)
			{
				return;
			}
			this.playerCollisionEvents = Player.local.locomotion.gameObject.AddComponent<PlayerCollisionEvents>();
			if (this.enableMark && !string.IsNullOrEmpty(this.handDecalId))
			{
				Catalog.InstantiateAsync(this.handDecalId, Vector3.zero, Quaternion.identity, null, delegate(GameObject value)
				{
					if (!value)
					{
						Debug.Log("Unabel to spawn hand decal");
						return;
					}
					this.handDecal = value;
					this.handDecal.SetActive(true);
					RagdollPart part = Extensions.GetRagdollPartByName("LeftHand");
					if (part != null)
					{
						this.handDecal.transform.parent = part.transform;
						this.handDecal.transform.localPosition = new Vector3(-0.07f, 0.0056f, 0.0126f);
						this.handDecal.transform.localEulerAngles = new Vector3(180f, 90f, 90f);
						this.handDecal.transform.localScale = new Vector3(0.07f, 0.03f, 0.07f);
						this.handDecalRenderer = this.handDecal.GetComponent<Renderer>();
						if (this.handDecalRenderer != null)
						{
							this.handDecalMpb = new MaterialPropertyBlock();
							this.handDecalRenderer.GetPropertyBlock(this.handDecalMpb);
							this.markColor * 0f;
							this.handDecalMpb.SetColor("_EmissionColor", Color.black);
							this.handDecalRenderer.SetPropertyBlock(this.handDecalMpb);
							this.colourChangeRunning = true;
						}
						else
						{
							Debug.Log("Renderer for hand decal null");
						}
						Debug.Log("HandDecal Spawned");
						return;
					}
					Debug.Log("HandDecal could not spawn, part was null");
				}, "decalLoad");
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004F37 File Offset: 0x00003137
		private void EventManager_onUnpossess(Creature creature, EventTime eventTime)
		{
			if (this.handDecal)
			{
				Object.Destroy(this.handDecal);
			}
			if (this.playerCollisionEvents)
			{
				Object.Destroy(this.playerCollisionEvents);
				this.playerCollisionEvents = null;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004F70 File Offset: 0x00003170
		public override void Update()
		{
			base.Update();
			if (Player.local && Player.local.creature && PlayerControl.local && this.handDecal && this.enableMark)
			{
				Color currentColor = this.handDecalMpb.GetColor("_EmissionColor");
				bool flag = this.markGlowing;
				if (Player.local.creature.mana.casterLeft.isFiring)
				{
					this.startValue = this.endValue;
					this.endValue = this.markColor;
					this.markGlowing = true;
				}
				else
				{
					this.startValue = this.endValue;
					this.endValue = Color.black;
					this.markGlowing = false;
				}
				if (flag != this.markGlowing && this.colourChange != null)
				{
					this.level.StopCoroutine(this.colourChange);
				}
				if (flag != this.markGlowing && currentColor != this.endValue)
				{
					this.colourChange = this.level.StartCoroutine(this.MarkColourChange());
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005090 File Offset: 0x00003290
		private IEnumerator MarkColourChange()
		{
			float timeElapsed = 0f;
			float duration = this.lerpDuration;
			while (timeElapsed < duration)
			{
				this.valueToLerp = Color.Lerp(this.startValue, this.endValue, timeElapsed / duration);
				timeElapsed += Time.deltaTime;
				this.handDecalMpb.SetColor("_EmissionColor", this.valueToLerp);
				this.handDecalRenderer.SetPropertyBlock(this.handDecalMpb);
				yield return null;
			}
			this.valueToLerp = this.endValue;
			yield break;
		}

		// Token: 0x0400007D RID: 125
		public string handDecalId = "Wully.Decal.OutsiderMark";

		// Token: 0x0400007E RID: 126
		public bool enableDoubleJump = true;

		// Token: 0x0400007F RID: 127
		public bool enableMark = true;

		// Token: 0x04000080 RID: 128
		public bool enableDishonoredShader;

		// Token: 0x04000081 RID: 129
		public string shaderMaterialAddress = "Wully.Material.Dishonored";

		// Token: 0x04000082 RID: 130
		public List<string> tips;

		// Token: 0x04000083 RID: 131
		private GameObject handDecal;

		// Token: 0x04000084 RID: 132
		private Renderer handDecalRenderer;

		// Token: 0x04000085 RID: 133
		private MaterialPropertyBlock handDecalMpb;

		// Token: 0x04000086 RID: 134
		public Color markColor = new Color(4.636402f, 2.9282541f, 0.48804238f);

		// Token: 0x04000087 RID: 135
		public static LevelModuleHonorless local;

		// Token: 0x04000088 RID: 136
		private PlayerCollisionEvents playerCollisionEvents;

		// Token: 0x04000089 RID: 137
		public CustomPassBlit customPassBlit;

		// Token: 0x0400008A RID: 138
		public Material material;

		// Token: 0x0400008B RID: 139
		public Movement movement;

		// Token: 0x0400008C RID: 140
		private bool colourChangeRunning = true;

		// Token: 0x0400008D RID: 141
		private Coroutine colourChange;

		// Token: 0x0400008E RID: 142
		private bool markGlowing;

		// Token: 0x0400008F RID: 143
		private float lerpDuration = 3f;

		// Token: 0x04000090 RID: 144
		private Color startValue = Color.black;

		// Token: 0x04000091 RID: 145
		private Color endValue = Color.black;

		// Token: 0x04000092 RID: 146
		private Color valueToLerp;
	}
}

using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TimeStopSpell
{
	// Token: 0x02000008 RID: 8
	public class TimeStopData : MonoBehaviour
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002C40 File Offset: 0x00000E40
		private void Start()
		{
			this.stopTimeAudioSource = base.gameObject.AddComponent<AudioSource>();
			this.stopTimeAudioSource.outputAudioMixerGroup = GameManager.GetAudioMixerGroup(1);
			this.resumeTimeAudioSource = base.gameObject.AddComponent<AudioSource>();
			this.resumeTimeAudioSource.outputAudioMixerGroup = GameManager.GetAudioMixerGroup(1);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C91 File Offset: 0x00000E91
		public void OnDataChanged()
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002E3C File Offset: 0x0000103C
		private IEnumerator TimeStopEffectCoroutine()
		{
			Transform playerLocalTransform = Player.local.transform;
			EffectInstance zaWarudoEffect = Catalog.GetData<EffectData>("SpellGravityBubble", true).Spawn(playerLocalTransform.position, playerLocalTransform.rotation, null, null, true, null, false, new Type[0]);
			if (!(this.stopTimeAudioSource == null))
			{
				float duration = this.stopTimeAudioSource.clip.length;
				this.stopTimeAudioSource.Play();
				zaWarudoEffect.SetIntensity(0f);
				zaWarudoEffect.Play(0, false);
				float startTime = Time.time;
				float elapsedTime = Time.time - startTime;
				while (elapsedTime <= duration)
				{
					if (elapsedTime <= duration / 2f)
					{
						zaWarudoEffect.SetIntensity(elapsedTime);
					}
					else
					{
						zaWarudoEffect.SetIntensity(duration - elapsedTime);
					}
					elapsedTime = Time.time - startTime;
					yield return new WaitForFixedUpdate();
				}
				zaWarudoEffect.End(false, -1f);
			}
			yield break;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002F0C File Offset: 0x0000110C
		private IEnumerator ZaWarudoResumeCoroutine()
		{
			if (!(this.resumeTimeAudioSource == null))
			{
				float duration = this.resumeTimeAudioSource.clip.length;
				this.resumeTimeAudioSource.Play();
				yield return new WaitForSeconds(duration);
			}
			yield break;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003030 File Offset: 0x00001230
		private IEnumerator StopTime()
		{
			this._timeIsStopping = true;
			this.isTimeStopped = true;
			if (this.data.hasEffect)
			{
				CameraEffects.SetSepia(this.data.saturationMultiplier);
				yield return this.TimeStopEffectCoroutine();
			}
			if (this.data.hasTimer)
			{
				base.StartCoroutine(this.ResumeTimeCountdownCoroutine());
			}
			this._timeIsStopping = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003100 File Offset: 0x00001300
		private IEnumerator ResumeTimeCountdownCoroutine()
		{
			yield return new WaitForSeconds(this.data.timerInSeconds);
			if (this.isTimeStopped)
			{
				yield return this.ResumeTime();
			}
			yield break;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003304 File Offset: 0x00001504
		private IEnumerator ResumeTime()
		{
			this._timeIsResuming = true;
			foreach (Creature creature in Creature.allActive)
			{
				if (!creature.isPlayer)
				{
					try
					{
						Object.Destroy(creature.gameObject.GetComponent<FrozenRagdollCreature>());
					}
					catch (Exception ex)
					{
						Debug.Log(ex.Message);
					}
					try
					{
						Object.Destroy(creature.gameObject.GetComponent<FrozenAnimationCreature>());
					}
					catch (Exception ex2)
					{
						Debug.Log(ex2.Message);
					}
				}
			}
			foreach (Item item in Item.allActive)
			{
				try
				{
					Object.Destroy(item.gameObject.GetComponent<FrozenItem>());
				}
				catch (Exception ex3)
				{
					Debug.Log(ex3.Message);
				}
			}
			this.isTimeStopped = false;
			if (this.data.hasEffect)
			{
				yield return this.ZaWarudoResumeCoroutine();
			}
			CameraEffects.RefreshPostProcess();
			this._timeIsResuming = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003320 File Offset: 0x00001520
		public void Toggle()
		{
			if (!this._timeIsResuming && !this._timeIsStopping)
			{
				if (this.isTimeStopped)
				{
					base.StartCoroutine(this.ResumeTime());
					return;
				}
				base.StartCoroutine(this.StopTime());
			}
		}

		// Token: 0x04000017 RID: 23
		public bool isTimeStopped;

		// Token: 0x04000018 RID: 24
		public TimeStopJSONData data = new TimeStopJSONData();

		// Token: 0x04000019 RID: 25
		private bool _timeIsStopping;

		// Token: 0x0400001A RID: 26
		private bool _timeIsResuming;

		// Token: 0x0400001B RID: 27
		public AudioSource stopTimeAudioSource;

		// Token: 0x0400001C RID: 28
		public AudioSource resumeTimeAudioSource;
	}
}

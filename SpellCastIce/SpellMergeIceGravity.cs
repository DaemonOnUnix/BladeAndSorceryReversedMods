using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace SpellCastIce
{
	// Token: 0x0200000B RID: 11
	internal class SpellMergeIceGravity : SpellMergeData
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00003678 File Offset: 0x00001878
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			bool flag = this.bubbleEffectId != null && this.bubbleEffectId != "";
			if (flag)
			{
				this.bubbleEffectData = Catalog.GetData<EffectData>(this.bubbleEffectId, true);
				Debug.Log("Got bubble effect DATA");
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000036CC File Offset: 0x000018CC
		public void StartCapture(float radius)
		{
			this.captureTrigger = new GameObject("IceTrigger").AddComponent<Trigger>();
			this.captureTrigger.transform.SetParent(this.mana.mergePoint);
			this.captureTrigger.transform.localPosition = Vector3.zero;
			this.captureTrigger.transform.localRotation = Quaternion.identity;
			this.captureTrigger.SetCallback(new Trigger.CallBack(this.OnTrigger));
			this.captureTrigger.SetLayer(GameManager.GetLayer(5));
			this.captureTrigger.SetRadius(radius);
			this.captureTrigger.SetActive(true);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000377C File Offset: 0x0000197C
		protected void OnTrigger(Collider other, bool enter)
		{
			bool flag = other.attachedRigidbody && !other.attachedRigidbody.isKinematic;
			if (flag)
			{
				CollisionHandler component = other.attachedRigidbody.GetComponent<CollisionHandler>();
				bool flag2 = component && component.ragdollPart && component.ragdollPart.ragdoll != this.mana.creature.ragdoll && this.bubbleActive;
				if (flag2)
				{
					if (enter)
					{
						Creature creature = component.ragdollPart.ragdoll.creature;
						bool flag3 = creature != Player.currentCreature && !creature.isKilled;
						if (flag3)
						{
							bool flag4 = creature.ragdoll.state != 2;
							if (flag4)
							{
								bool flag5 = !creature.GetComponent<IceSpellMWE>();
								if (flag5)
								{
									creature.gameObject.AddComponent<IceSpellMWE>();
								}
								IceSpellMWE component2 = creature.GetComponent<IceSpellMWE>();
								component2.SlowStartCoroutine(creature, 100f, 0f, 0f, 50f);
							}
						}
						this.capturedObjects.Add(component);
					}
					else
					{
						Creature creature2 = component.ragdollPart.ragdoll.creature;
						bool flag6 = creature2.GetComponent<IceSpellMWE>();
						if (flag6)
						{
							IceSpellMWE component3 = creature2.GetComponent<IceSpellMWE>();
							component3.UnFreezeCreature(creature2);
						}
						this.capturedObjects.Remove(component);
					}
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003900 File Offset: 0x00001B00
		public override void Merge(bool active)
		{
			base.Merge(active);
			if (active)
			{
				this.StartCapture(0f);
			}
			else
			{
				Vector3 vector = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
				Vector3 vector2 = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
				bool flag = vector.magnitude > SpellCaster.throwMinHandVelocity && vector2.magnitude > SpellCaster.throwMinHandVelocity;
				if (flag)
				{
					bool flag2 = Vector3.Angle(vector, this.mana.casterLeft.magicSource.position - this.mana.mergePoint.position) < 45f || Vector3.Angle(vector2, this.mana.casterRight.magicSource.position - this.mana.mergePoint.position) < 45f;
					if (flag2)
					{
						bool flag3 = this.currentCharge > this.bubbleMinCharge;
						if (flag3)
						{
							this.mana.StopCoroutine("BubbleCoroutine");
							this.mana.StartCoroutine(this.BubbleCoroutine(this.bubbleDuration));
						}
					}
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003A4D File Offset: 0x00001C4D
		protected IEnumerator BubbleCoroutine(float duration)
		{
			this.bubbleActive = true;
			this.StopCapture();
			EffectInstance bubbleEffect = null;
			bool flag = this.bubbleEffectData != null;
			if (flag)
			{
				bubbleEffect = this.bubbleEffectData.Spawn(this.captureTrigger.transform, true, null, true, Array.Empty<Type>());
				bubbleEffect.SetIntensity(0f);
				bubbleEffect.Play(0, false);
			}
			yield return new WaitForFixedUpdate();
			this.StartCapture(0f);
			this.captureTrigger.transform.SetParent(null);
			float startTime = Time.time;
			while (Time.time - startTime < duration)
			{
				bool flag2 = !this.captureTrigger;
				if (flag2)
				{
					yield break;
				}
				float num = this.bubbleScaleCurveOverTime.Evaluate((Time.time - startTime) / duration);
				this.captureTrigger.SetRadius(num * this.bubbleEffectMaxScale * 0.5f);
				bool flag3 = bubbleEffect != null;
				if (flag3)
				{
					bubbleEffect.SetIntensity(num);
				}
				yield return null;
			}
			bool flag4 = bubbleEffect != null;
			if (flag4)
			{
				bubbleEffect.End(false, -1f);
			}
			this.StopCapture();
			this.bubbleActive = false;
			yield break;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003A64 File Offset: 0x00001C64
		public void StopCapture()
		{
			this.captureTrigger.SetActive(false);
			for (int i = this.capturedObjects.Count - 1; i >= 0; i--)
			{
				Creature creature = this.capturedObjects[i].ragdollPart.ragdoll.creature;
				bool flag = creature.GetComponent<IceSpellMWE>();
				if (flag)
				{
					IceSpellMWE component = creature.GetComponent<IceSpellMWE>();
					component.UnFreezeCreature(creature);
				}
				this.capturedObjects.RemoveAt(i);
			}
			Object.Destroy(this.captureTrigger.gameObject);
		}

		// Token: 0x04000029 RID: 41
		public Trigger captureTrigger;

		// Token: 0x0400002A RID: 42
		public float bubbleMinCharge;

		// Token: 0x0400002B RID: 43
		public float bubbleDuration;

		// Token: 0x0400002C RID: 44
		public string bubbleEffectId;

		// Token: 0x0400002D RID: 45
		public AnimationCurve bubbleScaleCurveOverTime;

		// Token: 0x0400002E RID: 46
		public float bubbleEffectMaxScale;

		// Token: 0x0400002F RID: 47
		protected List<CollisionHandler> capturedObjects = new List<CollisionHandler>();

		// Token: 0x04000030 RID: 48
		protected bool bubbleActive;

		// Token: 0x04000031 RID: 49
		protected EffectData bubbleEffectData;
	}
}

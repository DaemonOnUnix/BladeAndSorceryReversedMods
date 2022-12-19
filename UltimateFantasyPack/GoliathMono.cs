using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x0200000F RID: 15
	public class GoliathMono : MonoBehaviour
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002B34 File Offset: 0x00000D34
		public bool IsHead(RagdollPart part)
		{
			return part.type == 1;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B5C File Offset: 0x00000D5C
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			bool flag = this.item.mainHandler != null;
			if (flag)
			{
				this.isHeld = true;
			}
			else
			{
				this.isHeld = false;
			}
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGrabEvent);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.Item_OnUngrabEvent);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002BCF File Offset: 0x00000DCF
		private void Item_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			this.isHeld = false;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002BD9 File Offset: 0x00000DD9
		private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			this.isHeld = true;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public void Update()
		{
			bool flag;
			if (this.isHeld)
			{
				RagdollHand mainHandler = this.item.mainHandler;
				Object @object;
				if (mainHandler == null)
				{
					@object = null;
				}
				else
				{
					Handle grabbedHandle = mainHandler.otherHand.grabbedHandle;
					@object = ((grabbedHandle != null) ? grabbedHandle.GetComponentInParent<Creature>() : null);
				}
				if (@object != null)
				{
					flag = this.item.mainHandler.playerHand.ragdollHand.otherHand.playerHand.controlHand.usePressed;
					goto IL_65;
				}
			}
			flag = false;
			IL_65:
			bool flag2 = flag;
			if (flag2)
			{
				Creature creature = this.item.mainHandler.otherHand.grabbedHandle.GetComponentInParent<Creature>();
				RagdollHand mainHandler2 = this.item.mainHandler;
				bool flag3 = mainHandler2 != null && mainHandler2.playerHand.ragdollHand.otherHand.grabbedHandle.GetComponentInParent<RagdollPart>().type == 1 && this.item.mainHandler.otherHand.playerHand.controlHand.usePressed;
				if (flag3)
				{
					GameManager.local.StartCoroutine(this.Delay(creature, 3f));
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002CEE File Offset: 0x00000EEE
		public IEnumerator Delay(Creature creature, float delay)
		{
			yield return new WaitForSeconds(delay);
			RagdollPart ragdollPart = this.item.mainHandler.otherHand.grabbedHandle.GetComponentInParent<RagdollPart>();
			bool flag = ragdollPart != null;
			if (flag)
			{
				bool flag2 = this.IsHead(ragdollPart) && ragdollPart.ragdoll.creature == creature;
				if (flag2)
				{
					this.Decap(creature);
				}
			}
			yield break;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002D0C File Offset: 0x00000F0C
		public void Decap(Creature creature)
		{
			Catalog.LoadAssetAsync<AudioClip>("UFP.HeadSplat", delegate(AudioClip audioClip)
			{
				GameObject obj = new GameObject();
				AudioSource source = obj.AddComponent<AudioSource>();
				source.clip = audioClip;
				source.transform.position = creature.ragdoll.headPart.transform.position;
				source.Play();
				GameManager.local.StartCoroutine(this.DespawnSound(obj, audioClip.length));
			}, "UFP");
			creature.ragdoll.headPart.sliceAllowed = true;
			creature.ragdoll.headPart.TrySlice();
			this.item.mainHandler.playerHand.ragdollHand.otherHand.playerHand.ragdollHand.UnGrab(false);
			creature.ragdoll.headPart.gameObject.SetActive(false);
			creature.Kill();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002DC9 File Offset: 0x00000FC9
		public IEnumerator DespawnSound(GameObject obj, float delay)
		{
			yield return new WaitForSeconds(delay);
			Object.Destroy(obj, delay);
			yield break;
		}

		// Token: 0x0400001D RID: 29
		public Item item;

		// Token: 0x0400001E RID: 30
		public bool isHeld;

		// Token: 0x0400001F RID: 31
		public Goliath settings;

		// Token: 0x04000020 RID: 32
		public AudioSource crushSource;
	}
}

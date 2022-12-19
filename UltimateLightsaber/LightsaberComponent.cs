using System;
using ThunderRoad;
using UnityEngine;

namespace UltimateLightsaber
{
	// Token: 0x02000003 RID: 3
	public class LightsaberComponent : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.Holder = this.item.GetCustomReference("Holder", true).GetComponent<Holder>();
			this.Holder.Snapped += new Holder.HolderDelegate(this.Holder_Snapped);
			this.anim = base.gameObject.GetComponent<Animator>();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E8 File Offset: 0x000002E8
		private void Holder_Snapped(Item i)
		{
			this.item.GetCustomReference("LightsaberBlade", true).GetComponent<MeshRenderer>().material.SetColor("EColor", i.renderers[0].material.GetColor("_EmissionColor") * 10f);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002144 File Offset: 0x00000344
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = this.isPlaying("SaberOn") || this.isPlaying("SaberOff") || this.isPlaying("ModuleOn") || this.isPlaying("ModuleOff");
			if (!flag)
			{
				bool flag2 = action == null && this.canOn;
				if (flag2)
				{
					bool flag3 = !this.On;
					if (flag3)
					{
						this.item.GetCustomReference("Idle", true).GetComponent<AudioSource>().Play();
						this.item.GetCustomReference("On", true).GetComponent<AudioSource>().Play();
						this.anim.Play("SaberOn");
					}
					else
					{
						this.item.GetCustomReference("Idle", true).GetComponent<AudioSource>().Stop();
						this.item.GetCustomReference("Off", true).GetComponent<AudioSource>().Play();
						this.anim.Play("SaberOff");
					}
					this.On = !this.On;
				}
				bool flag4 = action == 2 && !this.On;
				if (flag4)
				{
					this.Out = !this.Out;
					bool @out = this.Out;
					if (@out)
					{
						this.anim.Play("ModuleOn");
						this.Holder.data.disableTouch = false;
						this.Holder.data.locked = false;
						this.Holder.data.disableObjectProximityHighlighter = false;
					}
					else
					{
						this.anim.Play("ModuleOff");
						this.Holder.data.disableTouch = true;
						this.Holder.data.locked = true;
						this.Holder.data.disableObjectProximityHighlighter = true;
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000231C File Offset: 0x0000051C
		private void Update()
		{
			bool flag = this.Out || this.Holder.currentQuantity <= 0;
			if (flag)
			{
				this.canOn = false;
			}
			else
			{
				this.canOn = true;
			}
			bool on = this.On;
			if (on)
			{
				this.item.imbues.ForEach(delegate(Imbue i)
				{
					i.Transfer(Catalog.GetData<SpellCastCharge>("Fire", true), 100f);
				});
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002394 File Offset: 0x00000594
		private bool isPlaying(string stateName)
		{
			return this.anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) && this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
		}

		// Token: 0x04000001 RID: 1
		private Item item;

		// Token: 0x04000002 RID: 2
		private Animator anim;

		// Token: 0x04000003 RID: 3
		private Holder Holder;

		// Token: 0x04000004 RID: 4
		private bool On = false;

		// Token: 0x04000005 RID: 5
		private bool Out = false;

		// Token: 0x04000006 RID: 6
		private bool canOn = false;
	}
}

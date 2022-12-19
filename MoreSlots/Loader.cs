using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace Wully.MoreSlots
{
	// Token: 0x02000002 RID: 2
	public class Loader : CustomData
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnCatalogRefresh()
		{
			if (Loader.local != null)
			{
				return;
			}
			Loader.local = this;
			Debug.Log("MoreSlots Loader!");
			this.moreSlotsDatas = Catalog.GetDataList<MoreSlotsData>();
			this.dataLookup = new Dictionary<string, MoreSlotsData>();
			foreach (MoreSlotsData moreSlotsData in this.moreSlotsDatas)
			{
				this.dataLookup[moreSlotsData.id] = moreSlotsData;
			}
			this.holders = new List<Holder>(this.moreSlotsDatas.Count);
			EventManager.onPossess += new EventManager.PossessEvent(this.EventManagerOnPossess);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002104 File Offset: 0x00000304
		private void EventManagerOnPossess(Creature creature, EventTime eventTime)
		{
			if (eventTime == null)
			{
				return;
			}
			if (!Player.currentCreature)
			{
				return;
			}
			this.DestroySlots();
			this.OverrideExistingSlots();
			this.AddSlots();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000212C File Offset: 0x0000032C
		protected void DestroySlots()
		{
			int count = this.holders.Count;
			for (int i = 0; i < count; i++)
			{
				Holder customHolder = this.holders[i];
				if (customHolder && customHolder is MoreSlotsHolder)
				{
					Debug.Log("Destroying old slot: " + customHolder.name);
					Object.Destroy(customHolder);
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000218C File Offset: 0x0000038C
		private void OverrideExistingSlots()
		{
			foreach (Holder currentHolder in Player.currentCreature.gameObject.GetComponentsInChildren<Holder>())
			{
				if (!currentHolder.parentItem)
				{
					MoreSlotsData moreSlotsData;
					if (this.dataLookup.TryGetValue(currentHolder.name, out moreSlotsData))
					{
						Debug.Log("Overriding existing holder " + currentHolder.name + " with MoreSlots configuration " + moreSlotsData.ToString());
						currentHolder.gameObject.SetActive(moreSlotsData.enabled);
						if (moreSlotsData.localPosition != Vector3.zero)
						{
							currentHolder.transform.localPosition = moreSlotsData.localPosition;
						}
						if (moreSlotsData.localRotation != Vector3.zero)
						{
							currentHolder.transform.localPosition = (currentHolder.transform.localEulerAngles = moreSlotsData.localRotation);
						}
						this.dataLookup.Remove(currentHolder.name);
						this.moreSlotsDatas.Remove(moreSlotsData);
					}
					this.holders.Add(currentHolder);
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000229C File Offset: 0x0000049C
		protected void AddSlots()
		{
			int count = this.moreSlotsDatas.Count;
			for (int i = 0; i < count; i++)
			{
				MoreSlotsData customHolder = this.moreSlotsDatas[i];
				if (customHolder.enabled)
				{
					Debug.Log(string.Format("Attempting to add customHolderData:{0}", customHolder));
					this.CreateHolder(customHolder);
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022F0 File Offset: 0x000004F0
		private void CreateHolder(MoreSlotsData moreSlotsData)
		{
			RagdollPart part = Player.currentCreature.ragdoll.GetPartByName(moreSlotsData.ragdollPartName);
			HolderData data = Catalog.GetData<HolderData>(moreSlotsData.holderDataId, true);
			if (part == null || data == null)
			{
				Debug.LogWarning("Could not create holder for " + moreSlotsData.id);
				return;
			}
			GameObject gameObject = new GameObject(moreSlotsData.id ?? "");
			Transform transform = gameObject.transform;
			transform.parent = part.transform;
			transform.localPosition = moreSlotsData.localPosition;
			transform.localEulerAngles = moreSlotsData.localRotation;
			MoreSlotsHolder holder = gameObject.AddComponent<MoreSlotsHolder>();
			this.holders.Add(holder);
			holder.moreSlotsData = moreSlotsData;
			holder.part = part;
			(holder.touchCollider as SphereCollider).radius = moreSlotsData.triggerColliderRadius;
			holder.ignoredColliders = new List<Collider>();
			holder.Load(data);
			holder.data.forceAllowTouchOnPlayer = false;
			holder.data.disableTouch = false;
			holder.allowedHandSide = moreSlotsData.handSide;
			holder.RefreshChildAndParentHolder();
			Debug.Log("Added customHolderData:" + moreSlotsData.id);
		}

		// Token: 0x04000001 RID: 1
		public bool debug;

		// Token: 0x04000002 RID: 2
		private List<Holder> holders;

		// Token: 0x04000003 RID: 3
		private List<MoreSlotsData> moreSlotsDatas;

		// Token: 0x04000004 RID: 4
		private Dictionary<string, MoreSlotsData> dataLookup;

		// Token: 0x04000005 RID: 5
		public static Loader local;
	}
}

using System;
using System.Collections.Generic;
using System.Timers;
using ThunderRoad;
using UnityEngine;

namespace Kamui
{
	// Token: 0x02000004 RID: 4
	internal class Kamui : MonoBehaviour
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000237C File Offset: 0x0000057C
		private void Start()
		{
			this.distortionAmount = 0f;
			this.stopChecking = false;
			this.item = base.GetComponent<Item>();
			this.thisAttractor = this.item.gameObject.GetComponent<Attractor>();
			this.attractorOn = false;
			this.startDestroy = false;
			this.SetTimer();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000023D4 File Offset: 0x000005D4
		private void Update()
		{
			bool flag = this.distortionAmount < 1f && !this.startDestroy;
			if (flag)
			{
				this.distortionAmount += 0.01f;
				foreach (Material mat in this.item.gameObject.GetComponentInChildren<Renderer>().materials)
				{
					mat.SetFloat("Vector1_07761f96fcf147a7b17d362b38af7e11", this.distortionAmount);
				}
			}
			float distance = Vector3.Distance(Player.local.creature.transform.position, this.item.transform.position);
			bool flag2 = distance > 7f && !this.stopChecking;
			if (flag2)
			{
				this.item.rb.isKinematic = true;
				this.attractorOn = true;
				this.thisAttractor.attractorOn = this.attractorOn;
				this.stopChecking = true;
				this.colliderObjects = Physics.OverlapSphere(this.item.transform.position, 4f);
				this.FindAttractors();
				this.CreateAttractors();
				this.thisAttractor.SetFoundAttractor(this.attractors);
			}
			bool flag3 = this.startDestroy;
			if (flag3)
			{
				bool flag4 = this.distortionAmount > 0.01f;
				if (flag4)
				{
					this.distortionAmount -= 0.01f;
					foreach (Material mat2 in this.item.gameObject.GetComponentInChildren<Renderer>().materials)
					{
						mat2.SetFloat("Vector1_07761f96fcf147a7b17d362b38af7e11", this.distortionAmount);
					}
				}
				else
				{
					this.item.Despawn();
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002598 File Offset: 0x00000798
		private void FindAttractors()
		{
			bool flag = this.colliderObjects != null;
			if (flag)
			{
				foreach (Collider collider in this.colliderObjects)
				{
					bool flag2 = collider.gameObject.GetComponentInParent<Item>() != null;
					if (flag2)
					{
						bool flag3 = !this.colliderItems.Contains(collider.gameObject.GetComponentInParent<Item>());
						if (flag3)
						{
							this.colliderItems.Add(collider.gameObject.GetComponentInParent<Item>());
						}
					}
					else
					{
						bool flag4 = collider.gameObject.GetComponentInParent<Creature>() != null;
						if (flag4)
						{
							bool flag5 = !this.colliderCreature.Contains(collider.gameObject.GetComponentInParent<Creature>());
							if (flag5)
							{
								this.colliderCreature.Add(collider.gameObject.GetComponentInParent<Creature>());
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002680 File Offset: 0x00000880
		private void CreateAttractors()
		{
			bool flag = this.colliderItems != null;
			if (flag)
			{
				Debug.Log("Got past colliderItems null check");
				foreach (Item collideItem in this.colliderItems)
				{
					bool flag2 = collideItem != null;
					if (flag2)
					{
						Debug.Log(collideItem);
						collideItem.gameObject.AddComponent<Attractor>();
						Attractor added = collideItem.gameObject.GetComponent<Attractor>();
						added.rb = collideItem.gameObject.GetComponent<Rigidbody>();
						Debug.Log(added);
						bool flag3 = !this.attractors.Contains(added);
						if (flag3)
						{
							Debug.Log("Add to list");
							this.attractors.Add(added);
						}
					}
				}
				foreach (Creature collideCreatures in this.colliderCreature)
				{
					bool flag4 = this.colliderCreature != null;
					if (flag4)
					{
						collideCreatures.gameObject.AddComponent<Attractor>();
						Attractor added2 = collideCreatures.gameObject.GetComponent<Attractor>();
						added2.rb = collideCreatures.gameObject.GetComponent<Rigidbody>();
						bool flag5 = !this.attractors.Contains(added2);
						if (flag5)
						{
							Debug.Log("Add to list");
							this.attractors.Add(added2);
						}
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002820 File Offset: 0x00000A20
		private void SetTimer()
		{
			this.aTimer = new Timer(10000.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002874 File Offset: 0x00000A74
		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			this.startDestroy = true;
		}

		// Token: 0x0400000B RID: 11
		private Item item;

		// Token: 0x0400000C RID: 12
		private Collider[] colliderObjects;

		// Token: 0x0400000D RID: 13
		private List<Creature> colliderCreature = new List<Creature>();

		// Token: 0x0400000E RID: 14
		private List<Item> colliderItems = new List<Item>();

		// Token: 0x0400000F RID: 15
		private List<Attractor> attractors = new List<Attractor>();

		// Token: 0x04000010 RID: 16
		private Attractor thisAttractor;

		// Token: 0x04000011 RID: 17
		private bool attractorOn;

		// Token: 0x04000012 RID: 18
		private float attractorToItemDistance;

		// Token: 0x04000013 RID: 19
		private float distortionAmount;

		// Token: 0x04000014 RID: 20
		private bool stopChecking;

		// Token: 0x04000015 RID: 21
		private float elapsedTime;

		// Token: 0x04000016 RID: 22
		private bool isSucked;

		// Token: 0x04000017 RID: 23
		private Timer aTimer;

		// Token: 0x04000018 RID: 24
		private bool startDestroy;
	}
}

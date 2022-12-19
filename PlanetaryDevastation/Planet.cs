using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ThunderRoad;
using UnityEngine;

namespace PlanetaryDevastation
{
	// Token: 0x02000004 RID: 4
	public class Planet : MonoBehaviour
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000027F0 File Offset: 0x000009F0
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.meshTransform = this.item.GetCustomReference("PlanetaryDevastationReference", true);
			this.mesh = this.meshTransform.gameObject;
			foreach (object obj in this.mesh.transform)
			{
				Transform chunk = (Transform)obj;
				chunk.gameObject.AddComponent<Chunk>();
				chunk.gameObject.AddComponent<Rigidbody>();
				chunk.gameObject.GetComponent<Chunk>().clip = this.item.GetComponentsInChildren<AudioSource>()[Random.Range(0, this.item.GetComponentsInChildren<AudioSource>().Length - 2)].clip;
			}
			this.item.GetComponentsInChildren<AudioSource>()[2].playOnAwake = false;
			this.canPut = true;
			this.started = false;
			this.destroyed = false;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000028F8 File Offset: 0x00000AF8
		public void OnCollisionEnter(Collision c)
		{
			bool flag = !this.destroyed;
			if (flag)
			{
				Debug.Log("CollisionOccured");
				List<Rigidbody> rigids = new List<Rigidbody>();
				Collider[] colliders = Physics.OverlapSphere(this.item.transform.position, 50f);
				foreach (Collider collider in colliders)
				{
					Rigidbody body = collider.GetComponentInParent<Rigidbody>();
					bool flag2 = body != null;
					if (flag2)
					{
						bool flag3 = !Player.local.GetComponentsInChildren<Rigidbody>().Contains(body);
						if (flag3)
						{
							bool flag4 = !rigids.Contains(body);
							if (flag4)
							{
								rigids.Add(body);
							}
						}
					}
				}
				foreach (object obj in this.mesh.transform)
				{
					Transform chunk = (Transform)obj;
					rigids.Add(chunk.gameObject.GetComponent<Rigidbody>());
				}
				foreach (Rigidbody rigid in rigids)
				{
					Vector3 forceDirection;
					forceDirection..ctor(Random.Range(-50f, 50f), Random.Range(0f, 50f), Random.Range(-50f, 50f));
					rigid.isKinematic = false;
					bool flag5 = rigid.gameObject.GetComponent<Chunk>() != null;
					if (flag5)
					{
						rigid.AddForce(forceDirection * (0.75f * rigid.mass), 1);
					}
					else
					{
						rigid.AddForce(forceDirection * (4f * rigid.mass), 1);
					}
				}
				this.destroyed = true;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002AF8 File Offset: 0x00000CF8
		private void Update()
		{
			bool flag = this.numberFinished >= this.mesh.transform.childCount && this.canPut;
			if (flag)
			{
				this.canPut = false;
				this.item.rb.useGravity = true;
			}
			bool flag2 = this.canPut;
			if (flag2)
			{
				this.PutTogether();
				this.started = true;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002B60 File Offset: 0x00000D60
		private void PutTogether()
		{
			foreach (object obj in this.mesh.transform)
			{
				Transform chunk = (Transform)obj;
				bool flag = chunk.gameObject.GetComponent<Chunk>() != null;
				if (flag)
				{
					bool flag2 = !this.started;
					if (flag2)
					{
						chunk.gameObject.GetComponent<Chunk>().itemIn = this.item;
						chunk.gameObject.GetComponent<Chunk>().canStart = true;
					}
					else
					{
						bool flag3 = chunk.gameObject.GetComponent<Chunk>().finished && !chunk.gameObject.GetComponent<Chunk>().hasBeenAdded;
						if (flag3)
						{
							chunk.gameObject.GetComponent<Chunk>().hasBeenAdded = true;
							this.numberFinished++;
						}
					}
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002C68 File Offset: 0x00000E68
		private void SetTimer()
		{
			this.aTimer = new Timer(35000.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002CBC File Offset: 0x00000EBC
		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			this.item.GetComponentsInChildren<AudioSource>()[2].Stop();
		}

		// Token: 0x0400001A RID: 26
		private Transform meshTransform;

		// Token: 0x0400001B RID: 27
		private GameObject mesh;

		// Token: 0x0400001C RID: 28
		private Item item;

		// Token: 0x0400001D RID: 29
		private bool canPut;

		// Token: 0x0400001E RID: 30
		internal int numberFinished = 0;

		// Token: 0x0400001F RID: 31
		private bool started;

		// Token: 0x04000020 RID: 32
		private bool destroyed;

		// Token: 0x04000021 RID: 33
		private List<AudioSource> audios;

		// Token: 0x04000022 RID: 34
		private Timer aTimer;
	}
}

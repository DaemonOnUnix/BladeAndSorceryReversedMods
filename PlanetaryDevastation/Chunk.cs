using System;
using System.Collections.Generic;
using System.Timers;
using ThunderRoad;
using UnityEngine;

namespace PlanetaryDevastation
{
	// Token: 0x02000003 RID: 3
	internal class Chunk : MonoBehaviour
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002194 File Offset: 0x00000394
		private void Start()
		{
			this.firstSound = false;
			this.secondSound = false;
			base.gameObject.AddComponent<AudioSource>();
			base.gameObject.GetComponent<AudioSource>().spatialBlend = 1f;
			base.gameObject.GetComponent<AudioSource>().volume = 1f;
			base.gameObject.GetComponent<AudioSource>().priority = 80;
			base.gameObject.GetComponent<AudioSource>().spread = 200f;
			base.gameObject.GetComponent<AudioSource>().minDistance = 100f;
			base.gameObject.GetComponent<AudioSource>().maxDistance = 200f;
			this.originalRotation = base.gameObject.transform.rotation;
			this.hasBeenAdded = false;
			this.phaseNumber = new string[] { "1", "2", "3", "4" };
			this.phase = this.phaseNumber[Random.Range(0, 3)];
			this.originalItemPos = base.gameObject.transform.position;
			Rigidbody rigid = base.gameObject.GetComponent<Rigidbody>();
			bool flag = rigid != null;
			if (flag)
			{
				rigid.mass = 1000f / (251f / base.gameObject.GetComponent<Renderer>().bounds.size.magnitude);
				rigid.isKinematic = true;
			}
			bool flag2 = this.phase == "1";
			if (flag2)
			{
				this.randomWait = Random.Range(0f, 12.5f);
			}
			else
			{
				bool flag3 = this.phase == "2";
				if (flag3)
				{
					this.randomWait = Random.Range(12.5f, 25f);
				}
				else
				{
					bool flag4 = this.phase == "3";
					if (flag4)
					{
						this.randomWait = Random.Range(25f, 37.5f);
					}
					else
					{
						bool flag5 = this.phase == "4";
						if (flag5)
						{
							this.randomWait = Random.Range(37.5f, 50f);
						}
					}
				}
			}
			this.stopCheckFor = false;
			this.randomSpawnPoint();
			this.canStart = false;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023D4 File Offset: 0x000005D4
		private void randomSpawnPoint()
		{
			this.CastRay();
			float num = this.yPos;
			bool flag = true;
			if (flag)
			{
				this.randomSpawn = new Vector3(Player.local.creature.transform.position.x + Random.Range(-50f, 50f), this.yPos, Player.local.creature.transform.position.z + Random.Range(-50f, 50f));
			}
			base.gameObject.transform.position = this.randomSpawn;
			base.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000249C File Offset: 0x0000069C
		internal void Update()
		{
			bool flag = this.clip != null && !this.stopCheckFor;
			if (flag)
			{
				base.gameObject.GetComponent<AudioSource>().clip = this.clip;
				this.stopCheckFor = true;
			}
			bool flag2 = this.canStart;
			if (flag2)
			{
				this.distance = Vector3.Distance(this.itemIn.transform.position, base.gameObject.transform.position);
				this.totalElapsedTime += Time.deltaTime;
				bool flag3 = this.totalElapsedTime / this.randomWait >= 1f;
				if (flag3)
				{
					this.elapsedTime += Time.deltaTime;
					this.percentageComplete = this.elapsedTime / (5f * this.distance);
					base.gameObject.transform.position = Vector3.Lerp(base.gameObject.transform.position, this.originalItemPos, Mathf.SmoothStep(0f, 1f, this.percentageComplete));
					base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, this.originalRotation, Mathf.SmoothStep(0f, 1f, this.percentageComplete));
				}
				bool flag4 = base.gameObject.transform.position == this.originalItemPos;
				if (flag4)
				{
					this.finished = true;
					this.canStart = false;
					this.elapsedTime = 0f;
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000263C File Offset: 0x0000083C
		internal void CreateRigidbodies()
		{
			bool flag = base.gameObject.GetComponent<Rigidbody>() != null;
			if (flag)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002670 File Offset: 0x00000870
		internal void CastRay()
		{
			RaycastHit hit;
			bool flag = Physics.Raycast(base.transform.position, Vector3.down, ref hit);
			if (flag)
			{
				Debug.Log("Did hit.");
				Debug.Log(hit.collider.gameObject.transform.parent.name);
				this.yPos = hit.collider.gameObject.transform.parent.position.y - 10f;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000026F4 File Offset: 0x000008F4
		public void IgnoreChunks(Transform chunks)
		{
			foreach (object obj in chunks)
			{
				Transform ignore = (Transform)obj;
				bool flag = ignore != this;
				if (flag)
				{
					Physics.IgnoreCollision(base.gameObject.GetComponent<MeshCollider>(), ignore.gameObject.GetComponent<MeshCollider>(), true);
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002770 File Offset: 0x00000970
		public void StartDespawnTimer()
		{
			this.SetTimer();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000277C File Offset: 0x0000097C
		private void SetTimer()
		{
			this.aTimer = new Timer(8000.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000027D0 File Offset: 0x000009D0
		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			Object.Destroy(this);
		}

		// Token: 0x04000002 RID: 2
		private Vector3 randomSpawn;

		// Token: 0x04000003 RID: 3
		private Quaternion originalRotation;

		// Token: 0x04000004 RID: 4
		internal Vector3 originalItemPos;

		// Token: 0x04000005 RID: 5
		private float distance;

		// Token: 0x04000006 RID: 6
		private float lerpTime;

		// Token: 0x04000007 RID: 7
		private float elapsedTime;

		// Token: 0x04000008 RID: 8
		internal Item itemIn;

		// Token: 0x04000009 RID: 9
		private string phase;

		// Token: 0x0400000A RID: 10
		private Dictionary<string, float> phases = new Dictionary<string, float>();

		// Token: 0x0400000B RID: 11
		internal bool canStart;

		// Token: 0x0400000C RID: 12
		private bool addOrSubtract;

		// Token: 0x0400000D RID: 13
		internal float randomWait;

		// Token: 0x0400000E RID: 14
		internal bool finished;

		// Token: 0x0400000F RID: 15
		private string[] phaseNumber;

		// Token: 0x04000010 RID: 16
		private float percentageComplete;

		// Token: 0x04000011 RID: 17
		private float totalElapsedTime;

		// Token: 0x04000012 RID: 18
		internal bool hasBeenAdded;

		// Token: 0x04000013 RID: 19
		private float yPos;

		// Token: 0x04000014 RID: 20
		private Timer aTimer;

		// Token: 0x04000015 RID: 21
		internal AudioSource[] audios;

		// Token: 0x04000016 RID: 22
		internal AudioClip clip;

		// Token: 0x04000017 RID: 23
		private bool firstSound;

		// Token: 0x04000018 RID: 24
		private bool secondSound;

		// Token: 0x04000019 RID: 25
		private bool stopCheckFor;
	}
}

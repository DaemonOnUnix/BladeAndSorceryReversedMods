using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kamui
{
	// Token: 0x02000002 RID: 2
	internal class Attractor : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void Update()
		{
			bool flag = this.mainAttractor;
			bool flag2 = true;
			if (flag2)
			{
				Debug.Log("Main attractor is not null");
				bool flag3 = this.mainAttractor && this.attractorOn;
				if (flag3)
				{
					Debug.Log("Main attractor is true");
					bool flag4 = this.foundAttractors != null;
					if (flag4)
					{
						Debug.Log("foundAttractors is not null");
						foreach (Attractor attractor in this.foundAttractors)
						{
							Debug.Log("Attracting");
							this.Attract(attractor);
							bool flag5 = attractor.gameObject.GetComponent<ReduceSizeOverTime>() == null && attractor != this;
							if (flag5)
							{
								attractor.gameObject.AddComponent<ReduceSizeOverTime>();
								attractor.gameObject.GetComponent<ReduceSizeOverTime>().isSucked = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000215C File Offset: 0x0000035C
		private void Attract(Attractor objToAttract)
		{
			Rigidbody rbToAttract = objToAttract.rb;
			Vector3 direction = this.rb.position - rbToAttract.position;
			float distance = direction.magnitude;
			float forceMagnitude = 6.84f * (this.rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2f);
			Vector3 force = direction.normalized * forceMagnitude;
			rbToAttract.AddForce(force);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021D0 File Offset: 0x000003D0
		public void SetFoundAttractor(List<Attractor> attractorsList)
		{
			foreach (Attractor attractor in attractorsList)
			{
				this.foundAttractors.Add(attractor);
			}
		}

		// Token: 0x04000001 RID: 1
		private const float gravConstant = 6.84f;

		// Token: 0x04000002 RID: 2
		public Rigidbody rb;

		// Token: 0x04000003 RID: 3
		internal List<Attractor> foundAttractors = new List<Attractor>();

		// Token: 0x04000004 RID: 4
		internal bool mainAttractor;

		// Token: 0x04000005 RID: 5
		internal bool attractorOn;

		// Token: 0x04000006 RID: 6
		private bool isSucked = true;
	}
}

using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace Alcamoth
{
	// Token: 0x02000002 RID: 2
	public class SetTexture : StateMachineBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Start()
		{
			Level current = Level.current;
			Object @object;
			if (current == null)
			{
				@object = null;
			}
			else
			{
				Level.CustomReference customReference = current.customReferences[0];
				@object = ((customReference != null) ? customReference.transforms[0] : null);
			}
			bool flag = @object != null;
			if (flag)
			{
				foreach (Transform transform in Level.current.customReferences[0].transforms)
				{
					bool flag2 = !this.renderers.Contains(transform.GetComponent<Renderer>());
					if (flag2)
					{
						this.renderers.Add(transform.GetComponent<Renderer>());
					}
				}
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002110 File Offset: 0x00000310
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			bool flag = Utils.IsNullOrEmpty(this.renderers);
			if (flag)
			{
				foreach (Transform transform in Level.current.customReferences[0].transforms)
				{
					bool flag2 = !this.renderers.Contains(transform.GetComponent<Renderer>());
					if (flag2)
					{
						this.renderers.Add(transform.GetComponent<Renderer>());
					}
				}
			}
			bool flag3 = !Utils.IsNullOrEmpty(this.renderers);
			if (flag3)
			{
				foreach (Renderer renderer in this.renderers)
				{
					bool flag4 = renderer != null;
					if (flag4)
					{
						renderer.material.SetTexture("_BaseMap", this.texture);
					}
				}
			}
		}

		// Token: 0x04000001 RID: 1
		public List<Renderer> renderers;

		// Token: 0x04000002 RID: 2
		public Texture2D texture;
	}
}

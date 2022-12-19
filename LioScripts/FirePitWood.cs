using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Events;

namespace LioScripts
{
	// Token: 0x0200000E RID: 14
	internal class FirePitWood : MonoBehaviour
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002C1C File Offset: 0x00000E1C
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.isStarted = false;
			this.fireStart = new UnityEvent();
			this.fireStart.AddListener(new UnityAction(this.OnFireStart));
			base.transform.root.gameObject.AddComponent<SpellTouchEventLinker>().spellEvents = new List<SpellTouchEventLinker.SpellUnityEvent>
			{
				new SpellTouchEventLinker.SpellUnityEvent
				{
					step = 0,
					onActivate = this.fireStart,
					spellId = "Fire"
				}
			};
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002CAC File Offset: 0x00000EAC
		public void OnFireStart()
		{
			bool flag = this.isStarted;
			if (!flag)
			{
				this.isStarted = true;
				base.StartCoroutine(this.SpawnEffect());
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002CDA File Offset: 0x00000EDA
		private IEnumerator SpawnEffect()
		{
			GameObject p = new GameObject(base.gameObject.name + "f");
			p.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z);
			EffectInstance e = Catalog.GetData<EffectData>("Torch", true).Spawn(p.transform, true, null, false, Array.Empty<Type>());
			EffectInstance e2 = Catalog.GetData<EffectData>("Torch", true).Spawn(p.transform, true, null, false, Array.Empty<Type>());
			e.sourceTransform.localScale *= 2f;
			yield return new WaitForSeconds(1f);
			yield break;
		}

		// Token: 0x04000022 RID: 34
		private Item item;

		// Token: 0x04000023 RID: 35
		private UnityEvent fireStart;

		// Token: 0x04000024 RID: 36
		private bool isStarted;
	}
}

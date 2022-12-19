using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Events;

namespace LioScripts
{
	// Token: 0x0200000B RID: 11
	internal class FirePitWood : MonoBehaviour
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002884 File Offset: 0x00000A84
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

		// Token: 0x0600001C RID: 28 RVA: 0x00002914 File Offset: 0x00000B14
		public void OnFireStart()
		{
			bool flag = this.isStarted;
			if (!flag)
			{
				this.isStarted = true;
				base.StartCoroutine(this.SpawnEffect());
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002942 File Offset: 0x00000B42
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

		// Token: 0x04000012 RID: 18
		private Item item;

		// Token: 0x04000013 RID: 19
		private UnityEvent fireStart;

		// Token: 0x04000014 RID: 20
		private bool isStarted;
	}
}

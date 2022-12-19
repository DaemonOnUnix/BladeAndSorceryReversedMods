using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;

namespace Sectory
{
	// Token: 0x02000002 RID: 2
	public class InternalHelper
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void WriteHitInfoToJson()
		{
			string text = "{0} {1}";
			object obj = InternalHelper.latestInstance != null;
			CollisionInstance collisionInstance = InternalHelper.latestInstance;
			Debug.Log(string.Format(text, obj, ((collisionInstance != null) ? collisionInstance.damageStruct.hitRagdollPart : null) == null));
			bool flag = InternalHelper.latestInstance == null;
			if (!flag)
			{
				InternalsInfo internalsInfo = JsonConvert.DeserializeObject<InternalsInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "InternalsSettings.json")));
				List<InternalsInfo.Internal> list = internalsInfo.internals.ToList<InternalsInfo.Internal>();
				RagdollPart hitRagdollPart = InternalHelper.latestInstance.damageStruct.hitRagdollPart;
				bool flag2 = hitRagdollPart == null;
				if (!flag2)
				{
					list.Add(new InternalsInfo.Internal("Placeholder for - " + hitRagdollPart.name, hitRagdollPart.name, hitRagdollPart.transform.InverseTransformPoint(InternalHelper.latestInstance.contactPoint), 0.1f, InternalHelper.latestInstance.damageStruct.penetrationDepth, 1f, true, true, InternalInjuryAction.None, 50f));
					File.WriteAllText(Path.Combine(Entry.GetSettingsPath, "InternalsSettings.json"), JsonConvert.SerializeObject(new InternalsInfo(list.ToArray()), 1));
				}
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002170 File Offset: 0x00000370
		private static void BindNearest()
		{
			bool flag = InternalHelper.currentTarget;
			if (flag)
			{
				InternalHelper.currentTarget.OnDamageEvent -= new Creature.DamageEvent(InternalHelper.Damage);
			}
			InternalHelper.currentTarget = (from creature in Creature.allActive
				where !creature.isPlayer
				orderby (creature.transform.position - Player.local.transform.position).sqrMagnitude
				select creature).FirstOrDefault<Creature>();
			bool flag2 = InternalHelper.currentTarget == null;
			if (flag2)
			{
				Player.local.transform.position += new Vector3(0f, 0.25f, 0f);
			}
			else
			{
				InternalHelper.currentTarget.OnDamageEvent += new Creature.DamageEvent(InternalHelper.Damage);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002254 File Offset: 0x00000454
		private static void EnableIndicators()
		{
			bool flag = InternalHelper.currentTarget;
			if (flag)
			{
				InternalsInfo internalsInfo = JsonConvert.DeserializeObject<InternalsInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "InternalsSettings.json")));
				InternalsInfo.Internal[] internals = internalsInfo.internals;
				for (int j = 0; j < internals.Length; j++)
				{
					InternalsInfo.Internal vessel = internals[j];
					GameObject gameObject = GameObject.CreatePrimitive(0);
					Object.Destroy(gameObject.GetComponent<SphereCollider>());
					gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
					gameObject.transform.localScale = Vector3.one * vessel.size;
					gameObject.name = vessel.name + " Indicator";
					GameManager.local.StartCoroutine(InternalHelper.UpdateIndicator(gameObject, InternalHelper.currentTarget.ragdoll.parts.Find((RagdollPart i) => i.name == vessel.host).transform, vessel.offset));
					InternalHelper.indicators.Add(gameObject);
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002381 File Offset: 0x00000581
		private static IEnumerator UpdateIndicator(GameObject indicator, Transform part, Vector3 offset)
		{
			while (indicator != null && part != null)
			{
				indicator.transform.position = part.TransformPoint(offset);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000239E File Offset: 0x0000059E
		public static IEnumerator FlashRed(GameObject indicator)
		{
			indicator.GetComponent<MeshRenderer>().material.color = Color.red;
			yield return new WaitForSeconds(1f);
			indicator.GetComponent<MeshRenderer>().material.color = Color.white;
			yield break;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023B0 File Offset: 0x000005B0
		private static void DisableIndicators()
		{
			foreach (GameObject gameObject in InternalHelper.indicators)
			{
				Object.Destroy(gameObject);
			}
			InternalHelper.indicators.Clear();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002410 File Offset: 0x00000610
		private static void Damage(CollisionInstance collisionInstance)
		{
			InternalHelper.latestInstance = collisionInstance;
		}

		// Token: 0x04000001 RID: 1
		public Action BindNearestCreature = new Action(InternalHelper.BindNearest);

		// Token: 0x04000002 RID: 2
		public Action SerializeToJson = new Action(InternalHelper.WriteHitInfoToJson);

		// Token: 0x04000003 RID: 3
		public Action EnableIndicator = new Action(InternalHelper.EnableIndicators);

		// Token: 0x04000004 RID: 4
		public Action DisableIndicator = new Action(InternalHelper.DisableIndicators);

		// Token: 0x04000005 RID: 5
		[MenuIgnore]
		private static Creature currentTarget;

		// Token: 0x04000006 RID: 6
		[MenuIgnore]
		private static CollisionInstance latestInstance;

		// Token: 0x04000007 RID: 7
		[MenuIgnore]
		private static HashSet<GameObject> indicators = new HashSet<GameObject>();
	}
}

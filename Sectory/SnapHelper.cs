using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;

namespace Sectory
{
	// Token: 0x02000003 RID: 3
	public class SnapHelper
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002480 File Offset: 0x00000680
		private static void BindNearest()
		{
			SnapHelper.currentTarget = (from creature in Creature.allActive
				where !creature.isPlayer
				orderby (creature.transform.position - Player.local.transform.position).sqrMagnitude
				select creature).FirstOrDefault<Creature>();
			bool flag = SnapHelper.currentTarget == null;
			if (flag)
			{
				Player.local.transform.position += new Vector3(0f, 0.25f, 0f);
			}
			else
			{
				TwistInjuriesInfo twistInjuriesInfo = new TwistInjuriesInfo();
				twistInjuriesInfo.partToTwists = new TwistInjuriesInfo.PartToTwist[SnapHelper.currentTarget.ragdoll.parts.Count];
				for (int i = 0; i < SnapHelper.currentTarget.ragdoll.parts.Count; i++)
				{
					RagdollPart ragdollPart = SnapHelper.currentTarget.ragdoll.parts[i];
					twistInjuriesInfo.partToTwists[i] = new TwistInjuriesInfo.PartToTwist(ragdollPart.name, true, 500f);
				}
				File.WriteAllText(Path.Combine(Entry.GetSettingsPath, "TwistInjurySettings.json"), JsonConvert.SerializeObject(twistInjuriesInfo, 1));
			}
		}

		// Token: 0x04000008 RID: 8
		public Action BindNearestCreature = new Action(SnapHelper.BindNearest);

		// Token: 0x04000009 RID: 9
		[MenuIgnore]
		private static Creature currentTarget;
	}
}

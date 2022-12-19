using System;
using ThunderRoad;
using UnityEngine;

namespace Amenotejikara
{
	// Token: 0x02000002 RID: 2
	public class Sasuke : SpellCastCharge
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
		{
			base.Load(spellCaster, level);
			Sasuke.spellActive = true;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		public override void Unload()
		{
			Sasuke.spellActive = false;
			base.Unload();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002088 File Offset: 0x00000288
		public override void Fire(bool active)
		{
			base.Fire(active);
			Player local = Player.local;
			if (this.spellCaster.ragdollHand.playerHand.controlHand.castPressed && Sasuke.spellActive && this.creatureSwapActive)
			{
				Vector3 forward = Player.local.head.cam.transform.forward;
				RaycastHit raycastHit;
				if (Physics.SphereCast(Player.currentCreature.ragdoll.headPart.transform.position + Player.currentCreature.ragdoll.headPart.transform.forward * 1.5f, 0.3f, forward, ref raycastHit, this.amenotejikaraDistance))
				{
					Creature componentInParent = raycastHit.collider.GetComponentInParent<Creature>();
					Item componentInParent2 = raycastHit.collider.gameObject.GetComponentInParent<Item>();
					if (componentInParent != null && componentInParent != Player.currentCreature)
					{
						Sasuke.SwapNPCCoroutine(local, componentInParent);
						Debug.Log("You have now swapped places with an enemy Amenotejikara patched.");
						return;
					}
					if (componentInParent2 != null)
					{
						Sasuke.SwapItemCoroutine(local, componentInParent2);
						Debug.Log("You have now swapped places with an item Amenotejikara patched.");
						return;
					}
					Debug.Log("Hit " + raycastHit.collider.gameObject.name);
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021D0 File Offset: 0x000003D0
		public static void SwapNPCCoroutine(Player player, Creature creature)
		{
			Debug.Log(string.Format("First param {0}, Second {1}", player, creature));
			Vector3 position = player.transform.position;
			player.transform.position = creature.transform.position;
			creature.transform.position = position;
			player.transform.rotation *= Quaternion.FromToRotation(Vector3.ProjectOnPlane(player.creature.transform.forward, Vector3.up), Vector3.ProjectOnPlane(creature.transform.position - player.transform.position, Vector3.up));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002278 File Offset: 0x00000478
		public static void SwapItemCoroutine(Player player, Item item)
		{
			Debug.Log(string.Format("First param {0}, Second {1}", player, item));
			Vector3 position = player.transform.position;
			player.transform.position = item.transform.position;
			item.transform.position = position;
		}

		// Token: 0x04000001 RID: 1
		public static bool spellActive;

		// Token: 0x04000002 RID: 2
		public float amenotejikaraDistance = 500000f;

		// Token: 0x04000003 RID: 3
		public bool creatureSwapActive = true;

		// Token: 0x04000004 RID: 4
		public bool itemSwapActive;
	}
}

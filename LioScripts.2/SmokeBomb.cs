﻿using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace LioScripts
{
	// Token: 0x0200000E RID: 14
	internal class SmokeBomb : MonoBehaviour
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002AB0 File Offset: 0x00000CB0
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.wasUsed = (SmokeBomb.gotBaseValues = false);
			this.EnemiesHit = new List<Creature>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
			this.item.OnDespawnEvent += new Item.SpawnEvent(this.Item_OnDespawnEvent);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002B18 File Offset: 0x00000D18
		private void Item_OnDespawnEvent(EventTime eventTime)
		{
			this.EnemiesHit = new List<Creature>();
			this.wasUsed = false;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002B30 File Offset: 0x00000D30
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool flag = collisionInstance.impactVelocity.magnitude >= 7f && !this.wasUsed;
			if (flag)
			{
				base.StartCoroutine(this.SmokeExplode());
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B6D File Offset: 0x00000D6D
		private IEnumerator SmokeExplode()
		{
			this.wasUsed = true;
			EffectInstance i = Catalog.GetData<EffectData>("SmokeBurst", true).Spawn(base.gameObject.transform, true, null, false, Array.Empty<Type>());
			i.SetParent(null);
			Collider[] hitColliders = Physics.OverlapSphere(base.gameObject.transform.position, 3.5f);
			foreach (Collider hitCollider in hitColliders)
			{
				bool flag = hitCollider.transform.root.GetComponent<Creature>();
				if (flag)
				{
					bool flag2 = !hitCollider.transform.root.GetComponent<Creature>().isPlayer && !this.EnemiesHit.Contains(hitCollider.transform.root.GetComponent<Creature>()) && hitCollider.transform.root.GetComponent<Creature>().state > 0;
					if (flag2)
					{
						Creature enemy = hitCollider.transform.root.GetComponent<Creature>();
						this.EnemiesHit.Add(enemy);
						bool flag3 = !SmokeBomb.gotBaseValues;
						if (flag3)
						{
							SmokeBomb.gotBaseValues = true;
							SmokeBomb.baseSightMaxDistance = Catalog.GetData<BrainData>(enemy.data.brainId, true).GetModule<BrainModuleSightable>(true).sightMaxDistance;
							SmokeBomb.baseSightDetectionMaxDistance = Catalog.GetData<BrainData>(enemy.data.brainId, true).GetModule<BrainModuleSightable>(true).sightDetectionMaxDistance;
							SmokeBomb.baseHearMaxDistance = Catalog.GetData<BrainData>(enemy.data.brainId, true).GetModule<BrainModuleDetection>(true).hearMaxDistance;
							SmokeBomb.baseHearRememberDuration = Catalog.GetData<BrainData>(enemy.data.brainId, true).GetModule<BrainModuleDetection>(true).hearRememberDuration;
						}
						Catalog.GetData<BrainData>(enemy.data.brainId, true).GetModule<BrainModuleSightable>(true).sightDetectionMaxDistance = 1f;
						Catalog.GetData<BrainData>(enemy.data.brainId, true).GetModule<BrainModuleSightable>(true).sightMaxDistance = 1f;
						Catalog.GetData<BrainData>(enemy.data.brainId, true).GetModule<BrainModuleDetection>(true).hearMaxDistance = 1f;
						Catalog.GetData<BrainData>(enemy.data.brainId, true).GetModule<BrainModuleDetection>(true).hearRememberDuration = 1f;
						enemy.brain.SetState(3);
						enemy.brain.currentTarget = null;
						enemy = null;
					}
				}
				hitCollider = null;
			}
			Collider[] array = null;
			yield return new WaitForSeconds(5.5f);
			foreach (Creature creature in this.EnemiesHit)
			{
				bool flag4 = creature.state > 0;
				if (flag4)
				{
					Catalog.GetData<BrainData>(creature.data.brainId, true).GetModule<BrainModuleSightable>(true).sightMaxDistance = SmokeBomb.baseSightMaxDistance;
					Catalog.GetData<BrainData>(creature.data.brainId, true).GetModule<BrainModuleSightable>(true).sightDetectionMaxDistance = SmokeBomb.baseSightDetectionMaxDistance;
					Catalog.GetData<BrainData>(creature.data.brainId, true).GetModule<BrainModuleDetection>(true).hearMaxDistance = SmokeBomb.baseHearMaxDistance;
					Catalog.GetData<BrainData>(creature.data.brainId, true).GetModule<BrainModuleDetection>(true).hearRememberDuration = SmokeBomb.baseHearRememberDuration;
				}
				creature = null;
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
			this.item.Despawn();
			yield break;
		}

		// Token: 0x04000019 RID: 25
		private Item item;

		// Token: 0x0400001A RID: 26
		private bool wasUsed;

		// Token: 0x0400001B RID: 27
		private List<Creature> EnemiesHit;

		// Token: 0x0400001C RID: 28
		private static bool gotBaseValues;

		// Token: 0x0400001D RID: 29
		public static float baseSightMaxDistance;

		// Token: 0x0400001E RID: 30
		public static float baseSightDetectionMaxDistance;

		// Token: 0x0400001F RID: 31
		public static float baseHearMaxDistance;

		// Token: 0x04000020 RID: 32
		public static float baseHearRememberDuration;
	}
}

using System;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x02000006 RID: 6
	internal class EarthFireMerge : SpellMergeData
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00003CCC File Offset: 0x00001ECC
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			this.bulletEffectData = Catalog.GetData<EffectData>(this.bulletEffectId, true);
			this.bulletCollisionEffectData = Catalog.GetData<EffectData>(this.bulletCollisionEffectId, true);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003CFC File Offset: 0x00001EFC
		public override void Merge(bool active)
		{
			base.Merge(active);
			bool flag = !active;
			if (flag)
			{
				this.currentCharge = 0f;
				bool flag2 = this.bulletInstance != null;
				if (flag2)
				{
					this.bulletInstance.Despawn();
					this.bulletInstance = null;
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003D4C File Offset: 0x00001F4C
		public override void Update()
		{
			base.Update();
			bool flag = this.currentCharge > this.bulletMinCharge;
			if (flag)
			{
				bool flag2 = this.bulletInstance == null;
				if (flag2)
				{
					this.SpawnBulletInstance();
				}
			}
			else
			{
				bool flag3 = this.bulletInstance != null;
				if (flag3)
				{
					this.bulletInstance.Despawn();
					this.bulletInstance = null;
				}
				bool flag4 = this.rotatingMergePoint != null;
				if (flag4)
				{
					Object.Destroy(this.rotatingMergePoint);
				}
			}
			bool flag5 = this.rotatingMergePoint != null;
			if (flag5)
			{
				this.rotatingMergePoint.transform.rotation = Quaternion.LookRotation(this.mana.casterLeft.magic.transform.up + this.mana.casterRight.magic.transform.up);
				this.rotatingMergePoint.transform.position = Vector3.Lerp(this.mana.casterLeft.magic.transform.position, this.mana.casterRight.magic.transform.position, 0.5f);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003E84 File Offset: 0x00002084
		private void SpawnBulletInstance()
		{
			this.rotatingMergePoint = new GameObject("rotmpoint");
			this.bulletInstance = this.bulletEffectData.Spawn(this.rotatingMergePoint.transform, true, null, false, Array.Empty<Type>());
			this.bulletInstance.Play(0, false);
			this.bulletInstance.SetIntensity(1f);
			foreach (ParticleSystem particleSystem in this.bulletInstance.effects[0].GetComponentsInChildren<ParticleSystem>())
			{
				bool flag = particleSystem.gameObject.name == "Bullets";
				if (flag)
				{
					BulletCollisionClass bulletCollisionClass = particleSystem.gameObject.AddComponent<BulletCollisionClass>();
					bulletCollisionClass.part = particleSystem;
					bulletCollisionClass.bulletColData = this.bulletCollisionEffectData;
				}
			}
		}

		// Token: 0x04000074 RID: 116
		public string bulletEffectId;

		// Token: 0x04000075 RID: 117
		public float bulletMinCharge;

		// Token: 0x04000076 RID: 118
		public string bulletCollisionEffectId;

		// Token: 0x04000077 RID: 119
		private EffectData bulletEffectData;

		// Token: 0x04000078 RID: 120
		private EffectData bulletCollisionEffectData;

		// Token: 0x04000079 RID: 121
		private EffectInstance bulletInstance;

		// Token: 0x0400007A RID: 122
		private GameObject rotatingMergePoint;
	}
}

using System;
using ThunderRoad;
using UnityEngine;

namespace MergesSpellsUp
{
	// Token: 0x02000008 RID: 8
	public class MergeLightningMergeUp : SpellMergeData
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00008131 File Offset: 0x00006331
		public override void Load(Mana mana)
		{
			base.Load(mana);
			this.startTime = Time.time;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00008147 File Offset: 0x00006347
		public override void Merge(bool active)
		{
			base.Merge(active);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00008154 File Offset: 0x00006354
		public void SpawnBeam()
		{
			bool flag = Random.Range(0, 100) >= this.chancesOfRandomPosition;
			Vector3 origin;
			if (flag)
			{
				bool flag2 = Player.local.creature.transform.position.RandomCreatureInRadius(this.rangeOfDetection, true, false, false, null, false) != null;
				if (flag2)
				{
					origin = Player.local.creature.transform.position.RandomCreatureInRadius(this.rangeOfDetection, true, false, false, null, false).transform.position;
				}
				else
				{
					origin = Player.local.creature.RandomPositionAroundCreatureInRadius(Vector3.up * 0.5f, this.rangeOfDetection);
				}
			}
			else
			{
				origin = Player.local.creature.RandomPositionAroundCreatureInRadius(Vector3.up * 0.5f, this.rangeOfDetection);
			}
			Ray ray;
			ray..ctor(origin, Vector3.down);
			RaycastHit raycastHit;
			bool flag3 = Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity, 1, 1);
			if (flag3)
			{
				bool flag4 = raycastHit.collider;
				if (flag4)
				{
					origin = raycastHit.point + Vector3.up * 0.3f;
				}
			}
			Player.local.creature.handRight.caster.gameObject.AddComponent<LightningBeam>().Init(origin, Vector3.up, this.rangeOfDetection);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000082B8 File Offset: 0x000064B8
		public override void Update()
		{
			base.Update();
			bool flag = this.startTime < Time.time - this.pulse;
			if (flag)
			{
				for (int i = 0; i < this.nbBeamPerPulse; i++)
				{
					this.SpawnBeam();
				}
				this.startTime = Time.time;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00008310 File Offset: 0x00006510
		public override void Unload()
		{
			base.Unload();
		}

		// Token: 0x04000065 RID: 101
		private float startTime;

		// Token: 0x04000066 RID: 102
		[Range(1f, 10f)]
		public float pulse = 1f;

		// Token: 0x04000067 RID: 103
		[Range(1f, 20f)]
		public int nbBeamPerPulse = 2;

		// Token: 0x04000068 RID: 104
		[Range(0f, 100f)]
		public int chancesOfRandomPosition = 25;

		// Token: 0x04000069 RID: 105
		[Range(1f, 50f)]
		public float rangeOfDetection = 3f;
	}
}

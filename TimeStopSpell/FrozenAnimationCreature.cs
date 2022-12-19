using System;
using ThunderRoad;
using UnityEngine;

namespace TimeStopSpell
{
	// Token: 0x02000004 RID: 4
	public class FrozenAnimationCreature : MonoBehaviour
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000026F0 File Offset: 0x000008F0
		private void Start()
		{
			this._creature = base.gameObject.GetComponentInParent<Creature>();
			this._defaultCreatureBrainId = this._creature.brain.instance.id;
			this._defaultAnimatorSpeed = this._creature.animator.speed;
			this._creature.brain.Stop();
			if (this._creature.animator.isHuman)
			{
				this._creature.brain.Load("FrozenCreature");
			}
			this._creature.StopAnimation(false);
			this._creature.brain.StopAllCoroutines();
			this._creature.locomotion.MoveStop();
			this._creature.locomotion.SetSpeedModifier(this, 0f, 0f, 0f, 0f, 0f);
			this._creature.animator.speed = 0f;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000027E1 File Offset: 0x000009E1
		private void OnDestroy()
		{
			this._creature.brain.Load(this._defaultCreatureBrainId);
			this._creature.locomotion.ClearSpeedModifiers();
			this._creature.animator.speed = this._defaultAnimatorSpeed;
		}

		// Token: 0x04000005 RID: 5
		private Creature _creature;

		// Token: 0x04000006 RID: 6
		private float _defaultAnimatorSpeed;

		// Token: 0x04000007 RID: 7
		private string _defaultCreatureBrainId;
	}
}

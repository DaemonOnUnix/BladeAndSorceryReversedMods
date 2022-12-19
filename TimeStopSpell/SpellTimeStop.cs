using System;
using ThunderRoad;

namespace TimeStopSpell
{
	// Token: 0x0200000A RID: 10
	public class SpellTimeStop : SpellCastCharge
	{
		// Token: 0x0600002E RID: 46 RVA: 0x000034AC File Offset: 0x000016AC
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
		{
			base.Load(spellCaster, level);
			this._timeStopData = GameManager.local.gameObject.GetComponent<TimeStopData>();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000034CB File Offset: 0x000016CB
		public override void UpdateCaster()
		{
			base.UpdateCaster();
			if (this.spellCaster.isFiring && !this._lastIsFiring)
			{
				this._timeStopData.Toggle();
			}
			this._lastIsFiring = this.spellCaster.isFiring;
		}

		// Token: 0x0400001E RID: 30
		private bool _lastIsFiring;

		// Token: 0x0400001F RID: 31
		private TimeStopData _timeStopData;
	}
}

using System;
using ThunderRoad;
using UnityEngine;

namespace TimeStopSpell
{
	// Token: 0x02000006 RID: 6
	public class FrozenRagdollPart : MonoBehaviour
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002A74 File Offset: 0x00000C74
		private void Start()
		{
			this._ragdollPart = base.GetComponent<RagdollPart>();
			this.CacheVelocity();
			this._ragdollPart.rb.constraints = 126;
			this._multiplier = 1f;
			this._ragdollPart.collisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.CollisionHandlerOnOnCollisionStartEvent);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002ACC File Offset: 0x00000CCC
		private void CacheVelocity()
		{
			this._savedVelocity = this._ragdollPart.rb.velocity;
			this._savedAngularVelocity = this._ragdollPart.rb.angularVelocity;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002AFA File Offset: 0x00000CFA
		private void ReleaseVelocity()
		{
			this._ragdollPart.rb.velocity = this._savedVelocity;
			this._ragdollPart.rb.angularVelocity = this._savedAngularVelocity;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B28 File Offset: 0x00000D28
		private void CollisionHandlerOnOnCollisionStartEvent(CollisionInstance collisioninstance)
		{
			Vector3 vector = this._multiplier * collisioninstance.sourceCollider.attachedRigidbody.mass * collisioninstance.impactVelocity;
			this._impact += vector;
			if (collisioninstance.damageStruct.damageType == 3)
			{
				this._bluntImpact += vector;
			}
			this._multiplier += 0.5f;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B9C File Offset: 0x00000D9C
		private void OnDestroy()
		{
			this._ragdollPart.rb.constraints = 0;
			this.ReleaseVelocity();
			this._ragdollPart.rb.AddForce(this._impact, 1);
			this._ragdollPart.ragdoll.creature.locomotion.rb.AddForce(this._bluntImpact, 2);
			this._ragdollPart.collisionHandler.OnCollisionStartEvent -= new CollisionHandler.CollisionEvent(this.CollisionHandlerOnOnCollisionStartEvent);
		}

		// Token: 0x0400000B RID: 11
		private RagdollPart _ragdollPart;

		// Token: 0x0400000C RID: 12
		private Vector3 _impact;

		// Token: 0x0400000D RID: 13
		private Vector3 _bluntImpact;

		// Token: 0x0400000E RID: 14
		private float _multiplier;

		// Token: 0x0400000F RID: 15
		private Vector3 _savedVelocity = Vector3.zero;

		// Token: 0x04000010 RID: 16
		private Vector3 _savedAngularVelocity = Vector3.zero;
	}
}

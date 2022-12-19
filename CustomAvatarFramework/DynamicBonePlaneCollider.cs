using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[AddComponentMenu("Dynamic Bone/Dynamic Bone Plane Collider")]
public class DynamicBonePlaneCollider : DynamicBoneColliderBase
{
	// Token: 0x06000095 RID: 149 RVA: 0x00006935 File Offset: 0x00004B35
	private void OnValidate()
	{
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00006938 File Offset: 0x00004B38
	public override bool Collide(ref Vector3 particlePosition, float particleRadius)
	{
		Vector3 vector = Vector3.up;
		switch (this.m_Direction)
		{
		case DynamicBoneColliderBase.Direction.X:
			vector = base.transform.right;
			break;
		case DynamicBoneColliderBase.Direction.Y:
			vector = base.transform.up;
			break;
		case DynamicBoneColliderBase.Direction.Z:
			vector = base.transform.forward;
			break;
		}
		Vector3 vector2 = base.transform.TransformPoint(this.m_Center);
		Plane plane;
		plane..ctor(vector, vector2);
		float distanceToPoint = plane.GetDistanceToPoint(particlePosition);
		if (this.m_Bound == DynamicBoneColliderBase.Bound.Outside)
		{
			if (distanceToPoint < 0f)
			{
				particlePosition -= vector * distanceToPoint;
				return true;
			}
		}
		else if (distanceToPoint > 0f)
		{
			particlePosition -= vector * distanceToPoint;
			return true;
		}
		return false;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00006A08 File Offset: 0x00004C08
	private void OnDrawGizmosSelected()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.m_Bound == DynamicBoneColliderBase.Bound.Outside)
		{
			Gizmos.color = Color.yellow;
		}
		else
		{
			Gizmos.color = Color.magenta;
		}
		Vector3 vector = Vector3.up;
		switch (this.m_Direction)
		{
		case DynamicBoneColliderBase.Direction.X:
			vector = base.transform.right;
			break;
		case DynamicBoneColliderBase.Direction.Y:
			vector = base.transform.up;
			break;
		case DynamicBoneColliderBase.Direction.Z:
			vector = base.transform.forward;
			break;
		}
		Vector3 vector2 = base.transform.TransformPoint(this.m_Center);
		Gizmos.DrawLine(vector2, vector2 + vector);
	}
}

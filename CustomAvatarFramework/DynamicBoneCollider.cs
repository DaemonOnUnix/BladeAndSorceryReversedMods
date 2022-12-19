using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
[AddComponentMenu("Dynamic Bone/Dynamic Bone Collider")]
public class DynamicBoneCollider : DynamicBoneColliderBase
{
	// Token: 0x0600008D RID: 141 RVA: 0x0000637A File Offset: 0x0000457A
	private void OnValidate()
	{
		this.m_Radius = Mathf.Max(this.m_Radius, 0f);
		this.m_Height = Mathf.Max(this.m_Height, 0f);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000063A8 File Offset: 0x000045A8
	public override bool Collide(ref Vector3 particlePosition, float particleRadius)
	{
		float num = this.m_Radius * Mathf.Abs(base.transform.lossyScale.x);
		float num2 = this.m_Height * 0.5f - this.m_Radius;
		if (num2 <= 0f)
		{
			if (this.m_Bound == DynamicBoneColliderBase.Bound.Outside)
			{
				return DynamicBoneCollider.OutsideSphere(ref particlePosition, particleRadius, base.transform.TransformPoint(this.m_Center), num);
			}
			return DynamicBoneCollider.InsideSphere(ref particlePosition, particleRadius, base.transform.TransformPoint(this.m_Center), num);
		}
		else
		{
			Vector3 center = this.m_Center;
			Vector3 center2 = this.m_Center;
			switch (this.m_Direction)
			{
			case DynamicBoneColliderBase.Direction.X:
				center.x -= num2;
				center2.x += num2;
				break;
			case DynamicBoneColliderBase.Direction.Y:
				center.y -= num2;
				center2.y += num2;
				break;
			case DynamicBoneColliderBase.Direction.Z:
				center.z -= num2;
				center2.z += num2;
				break;
			}
			if (this.m_Bound == DynamicBoneColliderBase.Bound.Outside)
			{
				return DynamicBoneCollider.OutsideCapsule(ref particlePosition, particleRadius, base.transform.TransformPoint(center), base.transform.TransformPoint(center2), num);
			}
			return DynamicBoneCollider.InsideCapsule(ref particlePosition, particleRadius, base.transform.TransformPoint(center), base.transform.TransformPoint(center2), num);
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x000064FC File Offset: 0x000046FC
	private static bool OutsideSphere(ref Vector3 particlePosition, float particleRadius, Vector3 sphereCenter, float sphereRadius)
	{
		float num = sphereRadius + particleRadius;
		float num2 = num * num;
		Vector3 vector = particlePosition - sphereCenter;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude > 0f && sqrMagnitude < num2)
		{
			float num3 = Mathf.Sqrt(sqrMagnitude);
			particlePosition = sphereCenter + vector * (num / num3);
			return true;
		}
		return false;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00006554 File Offset: 0x00004754
	private static bool InsideSphere(ref Vector3 particlePosition, float particleRadius, Vector3 sphereCenter, float sphereRadius)
	{
		float num = sphereRadius - particleRadius;
		float num2 = num * num;
		Vector3 vector = particlePosition - sphereCenter;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude > num2)
		{
			float num3 = Mathf.Sqrt(sqrMagnitude);
			particlePosition = sphereCenter + vector * (num / num3);
			return true;
		}
		return false;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x000065A4 File Offset: 0x000047A4
	private static bool OutsideCapsule(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius)
	{
		float num = capsuleRadius + particleRadius;
		float num2 = num * num;
		Vector3 vector = capsuleP1 - capsuleP0;
		Vector3 vector2 = particlePosition - capsuleP0;
		float num3 = Vector3.Dot(vector2, vector);
		if (num3 <= 0f)
		{
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude > 0f && sqrMagnitude < num2)
			{
				float num4 = Mathf.Sqrt(sqrMagnitude);
				particlePosition = capsuleP0 + vector2 * (num / num4);
				return true;
			}
		}
		else
		{
			float sqrMagnitude2 = vector.sqrMagnitude;
			if (num3 >= sqrMagnitude2)
			{
				vector2 = particlePosition - capsuleP1;
				float sqrMagnitude3 = vector2.sqrMagnitude;
				if (sqrMagnitude3 > 0f && sqrMagnitude3 < num2)
				{
					float num5 = Mathf.Sqrt(sqrMagnitude3);
					particlePosition = capsuleP1 + vector2 * (num / num5);
					return true;
				}
			}
			else if (sqrMagnitude2 > 0f)
			{
				num3 /= sqrMagnitude2;
				vector2 -= vector * num3;
				float sqrMagnitude4 = vector2.sqrMagnitude;
				if (sqrMagnitude4 > 0f && sqrMagnitude4 < num2)
				{
					float num6 = Mathf.Sqrt(sqrMagnitude4);
					particlePosition += vector2 * ((num - num6) / num6);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000066DC File Offset: 0x000048DC
	private static bool InsideCapsule(ref Vector3 particlePosition, float particleRadius, Vector3 capsuleP0, Vector3 capsuleP1, float capsuleRadius)
	{
		float num = capsuleRadius - particleRadius;
		float num2 = num * num;
		Vector3 vector = capsuleP1 - capsuleP0;
		Vector3 vector2 = particlePosition - capsuleP0;
		float num3 = Vector3.Dot(vector2, vector);
		if (num3 <= 0f)
		{
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude > num2)
			{
				float num4 = Mathf.Sqrt(sqrMagnitude);
				particlePosition = capsuleP0 + vector2 * (num / num4);
				return true;
			}
		}
		else
		{
			float sqrMagnitude2 = vector.sqrMagnitude;
			if (num3 >= sqrMagnitude2)
			{
				vector2 = particlePosition - capsuleP1;
				float sqrMagnitude3 = vector2.sqrMagnitude;
				if (sqrMagnitude3 > num2)
				{
					float num5 = Mathf.Sqrt(sqrMagnitude3);
					particlePosition = capsuleP1 + vector2 * (num / num5);
					return true;
				}
			}
			else if (sqrMagnitude2 > 0f)
			{
				num3 /= sqrMagnitude2;
				vector2 -= vector * num3;
				float sqrMagnitude4 = vector2.sqrMagnitude;
				if (sqrMagnitude4 > num2)
				{
					float num6 = Mathf.Sqrt(sqrMagnitude4);
					particlePosition += vector2 * ((num - num6) / num6);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x000067F0 File Offset: 0x000049F0
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
		float num = this.m_Radius * Mathf.Abs(base.transform.lossyScale.x);
		float num2 = this.m_Height * 0.5f - this.m_Radius;
		if (num2 <= 0f)
		{
			Gizmos.DrawWireSphere(base.transform.TransformPoint(this.m_Center), num);
			return;
		}
		Vector3 center = this.m_Center;
		Vector3 center2 = this.m_Center;
		switch (this.m_Direction)
		{
		case DynamicBoneColliderBase.Direction.X:
			center.x -= num2;
			center2.x += num2;
			break;
		case DynamicBoneColliderBase.Direction.Y:
			center.y -= num2;
			center2.y += num2;
			break;
		case DynamicBoneColliderBase.Direction.Z:
			center.z -= num2;
			center2.z += num2;
			break;
		}
		Gizmos.DrawWireSphere(base.transform.TransformPoint(center), num);
		Gizmos.DrawWireSphere(base.transform.TransformPoint(center2), num);
	}

	// Token: 0x0400006D RID: 109
	public float m_Radius = 0.1f;

	// Token: 0x0400006E RID: 110
	public float m_Height;
}

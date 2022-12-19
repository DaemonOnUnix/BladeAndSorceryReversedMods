using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000013 RID: 19
[AddComponentMenu("Dynamic Bone/Dynamic Bone")]
public class DynamicBone : MonoBehaviour
{
	// Token: 0x06000072 RID: 114 RVA: 0x00004F90 File Offset: 0x00003190
	private void Start()
	{
		this.SetupParticles();
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00004F98 File Offset: 0x00003198
	private void FixedUpdate()
	{
		if (this.m_UpdateMode == DynamicBone.UpdateMode.AnimatePhysics)
		{
			this.PreUpdate();
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004FA9 File Offset: 0x000031A9
	private void Update()
	{
		if (this.m_UpdateMode != DynamicBone.UpdateMode.AnimatePhysics)
		{
			this.PreUpdate();
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00004FBC File Offset: 0x000031BC
	private void LateUpdate()
	{
		if (this.m_DistantDisable)
		{
			this.CheckDistance();
		}
		if (this.m_Weight > 0f && (!this.m_DistantDisable || !this.m_DistantDisabled))
		{
			float deltaTime = Time.deltaTime;
			this.UpdateDynamicBones(deltaTime);
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00005001 File Offset: 0x00003201
	private void PreUpdate()
	{
		if (this.m_Weight > 0f && (!this.m_DistantDisable || !this.m_DistantDisabled))
		{
			this.InitTransforms();
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00005028 File Offset: 0x00003228
	private void CheckDistance()
	{
		Transform transform = this.m_ReferenceObject;
		if (transform == null && Camera.main != null)
		{
			transform = Camera.main.transform;
		}
		if (transform != null)
		{
			float sqrMagnitude = (transform.position - base.transform.position).sqrMagnitude;
			bool flag = sqrMagnitude > this.m_DistanceToObject * this.m_DistanceToObject;
			if (flag != this.m_DistantDisabled)
			{
				if (!flag)
				{
					this.ResetParticlesPosition();
				}
				this.m_DistantDisabled = flag;
			}
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000050AF File Offset: 0x000032AF
	private void OnEnable()
	{
		this.ResetParticlesPosition();
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000050B7 File Offset: 0x000032B7
	private void OnDisable()
	{
		this.InitTransforms();
	}

	// Token: 0x0600007A RID: 122 RVA: 0x000050C0 File Offset: 0x000032C0
	private void OnValidate()
	{
		this.m_UpdateRate = Mathf.Max(this.m_UpdateRate, 0f);
		this.m_Damping = Mathf.Clamp01(this.m_Damping);
		this.m_Elasticity = Mathf.Clamp01(this.m_Elasticity);
		this.m_Stiffness = Mathf.Clamp01(this.m_Stiffness);
		this.m_Inert = Mathf.Clamp01(this.m_Inert);
		this.m_Friction = Mathf.Clamp01(this.m_Friction);
		this.m_Radius = Mathf.Max(this.m_Radius, 0f);
		if (Application.isEditor && Application.isPlaying)
		{
			this.InitTransforms();
			this.SetupParticles();
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00005168 File Offset: 0x00003368
	private void OnDrawGizmosSelected()
	{
		if (!base.enabled || this.m_Root == null)
		{
			return;
		}
		if (Application.isEditor && !Application.isPlaying && base.transform.hasChanged)
		{
			this.InitTransforms();
			this.SetupParticles();
		}
		Gizmos.color = Color.white;
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
				Gizmos.DrawLine(particle.m_Position, particle2.m_Position);
			}
			if (particle.m_Radius > 0f)
			{
				Gizmos.DrawWireSphere(particle.m_Position, particle.m_Radius * this.m_ObjectScale);
			}
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00005231 File Offset: 0x00003431
	public void SetWeight(float w)
	{
		if (this.m_Weight != w)
		{
			if (w == 0f)
			{
				this.InitTransforms();
			}
			else if (this.m_Weight == 0f)
			{
				this.ResetParticlesPosition();
			}
			this.m_Weight = w;
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00005266 File Offset: 0x00003466
	public float GetWeight()
	{
		return this.m_Weight;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00005270 File Offset: 0x00003470
	private void UpdateDynamicBones(float t)
	{
		if (this.m_Root == null)
		{
			return;
		}
		this.m_ObjectScale = Mathf.Abs(base.transform.lossyScale.x);
		this.m_ObjectMove = base.transform.position - this.m_ObjectPrevPosition;
		this.m_ObjectPrevPosition = base.transform.position;
		int num = 1;
		float num2 = 1f;
		if (this.m_UpdateMode == DynamicBone.UpdateMode.Default)
		{
			if (this.m_UpdateRate > 0f)
			{
				num2 = Time.deltaTime * this.m_UpdateRate;
			}
			else
			{
				num2 = Time.deltaTime;
			}
		}
		else if (this.m_UpdateRate > 0f)
		{
			float num3 = 1f / this.m_UpdateRate;
			this.m_Time += t;
			num = 0;
			while (this.m_Time >= num3)
			{
				this.m_Time -= num3;
				if (++num >= 3)
				{
					this.m_Time = 0f;
					break;
				}
			}
		}
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				this.UpdateParticles1(num2);
				this.UpdateParticles2(num2);
				this.m_ObjectMove = Vector3.zero;
			}
		}
		else
		{
			this.SkipUpdateParticles();
		}
		this.ApplyParticlesToTransforms();
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00005398 File Offset: 0x00003598
	public void SetupParticles()
	{
		this.m_Particles.Clear();
		if (this.m_Root == null)
		{
			return;
		}
		this.m_LocalGravity = this.m_Root.InverseTransformDirection(this.m_Gravity);
		this.m_ObjectScale = Mathf.Abs(base.transform.lossyScale.x);
		this.m_ObjectPrevPosition = base.transform.position;
		this.m_ObjectMove = Vector3.zero;
		this.m_BoneTotalLength = 0f;
		this.AppendParticles(this.m_Root, -1, 0f);
		this.UpdateParameters();
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00005430 File Offset: 0x00003630
	private void AppendParticles(Transform b, int parentIndex, float boneLength)
	{
		DynamicBone.Particle particle = new DynamicBone.Particle();
		particle.m_Transform = b;
		particle.m_ParentIndex = parentIndex;
		if (b != null)
		{
			particle.m_Position = (particle.m_PrevPosition = b.position);
			particle.m_InitLocalPosition = b.localPosition;
			particle.m_InitLocalRotation = b.localRotation;
		}
		else
		{
			Transform transform = this.m_Particles[parentIndex].m_Transform;
			if (this.m_EndLength > 0f)
			{
				Transform parent = transform.parent;
				if (parent != null)
				{
					particle.m_EndOffset = transform.InverseTransformPoint(transform.position * 2f - parent.position) * this.m_EndLength;
				}
				else
				{
					particle.m_EndOffset = new Vector3(this.m_EndLength, 0f, 0f);
				}
			}
			else
			{
				particle.m_EndOffset = transform.InverseTransformPoint(base.transform.TransformDirection(this.m_EndOffset) + transform.position);
			}
			particle.m_Position = (particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset));
		}
		if (parentIndex >= 0)
		{
			boneLength += (this.m_Particles[parentIndex].m_Transform.position - particle.m_Position).magnitude;
			particle.m_BoneLength = boneLength;
			this.m_BoneTotalLength = Mathf.Max(this.m_BoneTotalLength, boneLength);
		}
		int count = this.m_Particles.Count;
		this.m_Particles.Add(particle);
		if (b != null)
		{
			for (int i = 0; i < b.childCount; i++)
			{
				Transform child = b.GetChild(i);
				bool flag = false;
				if (this.m_Exclusions != null)
				{
					flag = this.m_Exclusions.Contains(child);
				}
				if (!flag)
				{
					this.AppendParticles(child, count, boneLength);
				}
				else if (this.m_EndLength > 0f || this.m_EndOffset != Vector3.zero)
				{
					this.AppendParticles(null, count, boneLength);
				}
			}
			if (b.childCount == 0 && (this.m_EndLength > 0f || this.m_EndOffset != Vector3.zero))
			{
				this.AppendParticles(null, count, boneLength);
			}
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00005664 File Offset: 0x00003864
	public void UpdateParameters()
	{
		if (this.m_Root == null)
		{
			return;
		}
		this.m_LocalGravity = this.m_Root.InverseTransformDirection(this.m_Gravity);
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			particle.m_Damping = this.m_Damping;
			particle.m_Elasticity = this.m_Elasticity;
			particle.m_Stiffness = this.m_Stiffness;
			particle.m_Inert = this.m_Inert;
			particle.m_Friction = this.m_Friction;
			particle.m_Radius = this.m_Radius;
			if (this.m_BoneTotalLength > 0f)
			{
				float num = particle.m_BoneLength / this.m_BoneTotalLength;
				if (this.m_DampingDistrib != null && this.m_DampingDistrib.keys.Length > 0)
				{
					particle.m_Damping *= this.m_DampingDistrib.Evaluate(num);
				}
				if (this.m_ElasticityDistrib != null && this.m_ElasticityDistrib.keys.Length > 0)
				{
					particle.m_Elasticity *= this.m_ElasticityDistrib.Evaluate(num);
				}
				if (this.m_StiffnessDistrib != null && this.m_StiffnessDistrib.keys.Length > 0)
				{
					particle.m_Stiffness *= this.m_StiffnessDistrib.Evaluate(num);
				}
				if (this.m_InertDistrib != null && this.m_InertDistrib.keys.Length > 0)
				{
					particle.m_Inert *= this.m_InertDistrib.Evaluate(num);
				}
				if (this.m_FrictionDistrib != null && this.m_FrictionDistrib.keys.Length > 0)
				{
					particle.m_Friction *= this.m_FrictionDistrib.Evaluate(num);
				}
				if (this.m_RadiusDistrib != null && this.m_RadiusDistrib.keys.Length > 0)
				{
					particle.m_Radius *= this.m_RadiusDistrib.Evaluate(num);
				}
			}
			particle.m_Damping = Mathf.Clamp01(particle.m_Damping);
			particle.m_Elasticity = Mathf.Clamp01(particle.m_Elasticity);
			particle.m_Stiffness = Mathf.Clamp01(particle.m_Stiffness);
			particle.m_Inert = Mathf.Clamp01(particle.m_Inert);
			particle.m_Friction = Mathf.Clamp01(particle.m_Friction);
			particle.m_Radius = Mathf.Max(particle.m_Radius, 0f);
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000058B8 File Offset: 0x00003AB8
	private void InitTransforms()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_Transform != null)
			{
				particle.m_Transform.localPosition = particle.m_InitLocalPosition;
				particle.m_Transform.localRotation = particle.m_InitLocalRotation;
			}
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00005918 File Offset: 0x00003B18
	private void ResetParticlesPosition()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_Transform != null)
			{
				particle.m_Position = (particle.m_PrevPosition = particle.m_Transform.position);
			}
			else
			{
				Transform transform = this.m_Particles[particle.m_ParentIndex].m_Transform;
				particle.m_Position = (particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset));
			}
			particle.m_isCollide = false;
		}
		this.m_ObjectPrevPosition = base.transform.position;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x000059C4 File Offset: 0x00003BC4
	private void UpdateParticles1(float timeVar)
	{
		Vector3 vector = this.m_Gravity;
		Vector3 normalized = this.m_Gravity.normalized;
		Vector3 vector2 = this.m_Root.TransformDirection(this.m_LocalGravity);
		Vector3 vector3 = normalized * Mathf.Max(Vector3.Dot(vector2, normalized), 0f);
		vector -= vector3;
		vector = (vector + this.m_Force) * (this.m_ObjectScale * timeVar);
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				Vector3 vector4 = particle.m_Position - particle.m_PrevPosition;
				Vector3 vector5 = this.m_ObjectMove * particle.m_Inert;
				particle.m_PrevPosition = particle.m_Position + vector5;
				float num = particle.m_Damping;
				if (particle.m_isCollide)
				{
					num += particle.m_Friction;
					if (num > 1f)
					{
						num = 1f;
					}
					particle.m_isCollide = false;
				}
				particle.m_Position += vector4 * (1f - num) + vector + vector5;
			}
			else
			{
				particle.m_PrevPosition = particle.m_Position;
				particle.m_Position = particle.m_Transform.position;
			}
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00005B34 File Offset: 0x00003D34
	private void UpdateParticles2(float timeVar)
	{
		Plane plane = default(Plane);
		for (int i = 1; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
			float num;
			if (particle.m_Transform != null)
			{
				num = (particle2.m_Transform.position - particle.m_Transform.position).magnitude;
			}
			else
			{
				num = particle2.m_Transform.localToWorldMatrix.MultiplyVector(particle.m_EndOffset).magnitude;
			}
			float num2 = Mathf.Lerp(1f, particle.m_Stiffness, this.m_Weight);
			if (num2 > 0f || particle.m_Elasticity > 0f)
			{
				Matrix4x4 localToWorldMatrix = particle2.m_Transform.localToWorldMatrix;
				localToWorldMatrix.SetColumn(3, particle2.m_Position);
				Vector3 vector;
				if (particle.m_Transform != null)
				{
					vector = localToWorldMatrix.MultiplyPoint3x4(particle.m_Transform.localPosition);
				}
				else
				{
					vector = localToWorldMatrix.MultiplyPoint3x4(particle.m_EndOffset);
				}
				Vector3 vector2 = vector - particle.m_Position;
				particle.m_Position += vector2 * (particle.m_Elasticity * timeVar);
				if (num2 > 0f)
				{
					vector2 = vector - particle.m_Position;
					float magnitude = vector2.magnitude;
					float num3 = num * (1f - num2) * 2f;
					if (magnitude > num3)
					{
						particle.m_Position += vector2 * ((magnitude - num3) / magnitude);
					}
				}
			}
			if (this.m_Colliders != null)
			{
				float num4 = particle.m_Radius * this.m_ObjectScale;
				for (int j = 0; j < this.m_Colliders.Count; j++)
				{
					DynamicBoneColliderBase dynamicBoneColliderBase = this.m_Colliders[j];
					if (dynamicBoneColliderBase != null && dynamicBoneColliderBase.enabled)
					{
						particle.m_isCollide |= dynamicBoneColliderBase.Collide(ref particle.m_Position, num4);
					}
				}
			}
			if (this.m_FreezeAxis != DynamicBone.FreezeAxis.None)
			{
				switch (this.m_FreezeAxis)
				{
				case DynamicBone.FreezeAxis.X:
					plane.SetNormalAndPosition(particle2.m_Transform.right, particle2.m_Position);
					break;
				case DynamicBone.FreezeAxis.Y:
					plane.SetNormalAndPosition(particle2.m_Transform.up, particle2.m_Position);
					break;
				case DynamicBone.FreezeAxis.Z:
					plane.SetNormalAndPosition(particle2.m_Transform.forward, particle2.m_Position);
					break;
				}
				particle.m_Position -= plane.normal * plane.GetDistanceToPoint(particle.m_Position);
			}
			Vector3 vector3 = particle2.m_Position - particle.m_Position;
			float magnitude2 = vector3.magnitude;
			if (magnitude2 > 0f)
			{
				particle.m_Position += vector3 * ((magnitude2 - num) / magnitude2);
			}
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00005E40 File Offset: 0x00004040
	private void SkipUpdateParticles()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				particle.m_PrevPosition += this.m_ObjectMove;
				particle.m_Position += this.m_ObjectMove;
				DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
				float num;
				if (particle.m_Transform != null)
				{
					num = (particle2.m_Transform.position - particle.m_Transform.position).magnitude;
				}
				else
				{
					num = particle2.m_Transform.localToWorldMatrix.MultiplyVector(particle.m_EndOffset).magnitude;
				}
				float num2 = Mathf.Lerp(1f, particle.m_Stiffness, this.m_Weight);
				if (num2 > 0f)
				{
					Matrix4x4 localToWorldMatrix = particle2.m_Transform.localToWorldMatrix;
					localToWorldMatrix.SetColumn(3, particle2.m_Position);
					Vector3 vector;
					if (particle.m_Transform != null)
					{
						vector = localToWorldMatrix.MultiplyPoint3x4(particle.m_Transform.localPosition);
					}
					else
					{
						vector = localToWorldMatrix.MultiplyPoint3x4(particle.m_EndOffset);
					}
					Vector3 vector2 = vector - particle.m_Position;
					float magnitude = vector2.magnitude;
					float num3 = num * (1f - num2) * 2f;
					if (magnitude > num3)
					{
						particle.m_Position += vector2 * ((magnitude - num3) / magnitude);
					}
				}
				Vector3 vector3 = particle2.m_Position - particle.m_Position;
				float magnitude2 = vector3.magnitude;
				if (magnitude2 > 0f)
				{
					particle.m_Position += vector3 * ((magnitude2 - num) / magnitude2);
				}
			}
			else
			{
				particle.m_PrevPosition = particle.m_Position;
				particle.m_Position = particle.m_Transform.position;
			}
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00006045 File Offset: 0x00004245
	private static Vector3 MirrorVector(Vector3 v, Vector3 axis)
	{
		return v - axis * (Vector3.Dot(v, axis) * 2f);
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00006060 File Offset: 0x00004260
	private void ApplyParticlesToTransforms()
	{
		Vector3 vector = Vector3.right;
		Vector3 vector2 = Vector3.up;
		Vector3 vector3 = Vector3.forward;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		Vector3 lossyScale = base.transform.lossyScale;
		if (lossyScale.x < 0f || lossyScale.y < 0f || lossyScale.z < 0f)
		{
			Transform transform = base.transform;
			do
			{
				Vector3 localScale = transform.localScale;
				flag = localScale.x < 0f;
				if (flag)
				{
					vector = transform.right;
				}
				flag2 = localScale.y < 0f;
				if (flag2)
				{
					vector2 = transform.up;
				}
				flag3 = localScale.z < 0f;
				if (flag3)
				{
					vector3 = transform.forward;
				}
				if (flag || flag2 || flag3)
				{
					break;
				}
				transform = transform.parent;
			}
			while (transform != null);
		}
		for (int i = 1; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
			if (particle2.m_Transform.childCount <= 1)
			{
				Vector3 vector4;
				if (particle.m_Transform != null)
				{
					vector4 = particle.m_Transform.localPosition;
				}
				else
				{
					vector4 = particle.m_EndOffset;
				}
				Vector3 vector5 = particle.m_Position - particle2.m_Position;
				if (flag)
				{
					vector5 = DynamicBone.MirrorVector(vector5, vector);
				}
				if (flag2)
				{
					vector5 = DynamicBone.MirrorVector(vector5, vector2);
				}
				if (flag3)
				{
					vector5 = DynamicBone.MirrorVector(vector5, vector3);
				}
				Quaternion quaternion = Quaternion.FromToRotation(particle2.m_Transform.TransformDirection(vector4), vector5);
				particle2.m_Transform.rotation = quaternion * particle2.m_Transform.rotation;
			}
			if (particle.m_Transform != null)
			{
				particle.m_Transform.position = particle.m_Position;
			}
		}
	}

	// Token: 0x04000028 RID: 40
	public Transform m_Root;

	// Token: 0x04000029 RID: 41
	public float m_UpdateRate = 60f;

	// Token: 0x0400002A RID: 42
	public DynamicBone.UpdateMode m_UpdateMode = DynamicBone.UpdateMode.Default;

	// Token: 0x0400002B RID: 43
	[Range(0f, 1f)]
	public float m_Damping = 0.1f;

	// Token: 0x0400002C RID: 44
	public AnimationCurve m_DampingDistrib;

	// Token: 0x0400002D RID: 45
	[Range(0f, 1f)]
	public float m_Elasticity = 0.1f;

	// Token: 0x0400002E RID: 46
	public AnimationCurve m_ElasticityDistrib;

	// Token: 0x0400002F RID: 47
	[Range(0f, 1f)]
	public float m_Stiffness = 0.1f;

	// Token: 0x04000030 RID: 48
	public AnimationCurve m_StiffnessDistrib;

	// Token: 0x04000031 RID: 49
	[Range(0f, 1f)]
	public float m_Inert;

	// Token: 0x04000032 RID: 50
	public AnimationCurve m_InertDistrib;

	// Token: 0x04000033 RID: 51
	public float m_Friction;

	// Token: 0x04000034 RID: 52
	public AnimationCurve m_FrictionDistrib;

	// Token: 0x04000035 RID: 53
	public float m_Radius;

	// Token: 0x04000036 RID: 54
	public AnimationCurve m_RadiusDistrib;

	// Token: 0x04000037 RID: 55
	public float m_EndLength;

	// Token: 0x04000038 RID: 56
	public Vector3 m_EndOffset = Vector3.zero;

	// Token: 0x04000039 RID: 57
	public Vector3 m_Gravity = Vector3.zero;

	// Token: 0x0400003A RID: 58
	public Vector3 m_Force = Vector3.zero;

	// Token: 0x0400003B RID: 59
	public List<DynamicBoneColliderBase> m_Colliders;

	// Token: 0x0400003C RID: 60
	public List<Transform> m_Exclusions;

	// Token: 0x0400003D RID: 61
	public DynamicBone.FreezeAxis m_FreezeAxis;

	// Token: 0x0400003E RID: 62
	public bool m_DistantDisable;

	// Token: 0x0400003F RID: 63
	public Transform m_ReferenceObject;

	// Token: 0x04000040 RID: 64
	public float m_DistanceToObject = 20f;

	// Token: 0x04000041 RID: 65
	private Vector3 m_LocalGravity = Vector3.zero;

	// Token: 0x04000042 RID: 66
	private Vector3 m_ObjectMove = Vector3.zero;

	// Token: 0x04000043 RID: 67
	private Vector3 m_ObjectPrevPosition = Vector3.zero;

	// Token: 0x04000044 RID: 68
	private float m_BoneTotalLength;

	// Token: 0x04000045 RID: 69
	private float m_ObjectScale = 1f;

	// Token: 0x04000046 RID: 70
	private float m_Time;

	// Token: 0x04000047 RID: 71
	private float m_Weight = 1f;

	// Token: 0x04000048 RID: 72
	private bool m_DistantDisabled;

	// Token: 0x04000049 RID: 73
	private List<DynamicBone.Particle> m_Particles = new List<DynamicBone.Particle>();

	// Token: 0x02000014 RID: 20
	public enum UpdateMode
	{
		// Token: 0x0400004B RID: 75
		Normal,
		// Token: 0x0400004C RID: 76
		AnimatePhysics,
		// Token: 0x0400004D RID: 77
		UnscaledTime,
		// Token: 0x0400004E RID: 78
		Default
	}

	// Token: 0x02000015 RID: 21
	public enum FreezeAxis
	{
		// Token: 0x04000050 RID: 80
		None,
		// Token: 0x04000051 RID: 81
		X,
		// Token: 0x04000052 RID: 82
		Y,
		// Token: 0x04000053 RID: 83
		Z
	}

	// Token: 0x02000016 RID: 22
	private class Particle
	{
		// Token: 0x04000054 RID: 84
		public Transform m_Transform;

		// Token: 0x04000055 RID: 85
		public int m_ParentIndex = -1;

		// Token: 0x04000056 RID: 86
		public float m_Damping;

		// Token: 0x04000057 RID: 87
		public float m_Elasticity;

		// Token: 0x04000058 RID: 88
		public float m_Stiffness;

		// Token: 0x04000059 RID: 89
		public float m_Inert;

		// Token: 0x0400005A RID: 90
		public float m_Friction;

		// Token: 0x0400005B RID: 91
		public float m_Radius;

		// Token: 0x0400005C RID: 92
		public float m_BoneLength;

		// Token: 0x0400005D RID: 93
		public bool m_isCollide;

		// Token: 0x0400005E RID: 94
		public Vector3 m_Position = Vector3.zero;

		// Token: 0x0400005F RID: 95
		public Vector3 m_PrevPosition = Vector3.zero;

		// Token: 0x04000060 RID: 96
		public Vector3 m_EndOffset = Vector3.zero;

		// Token: 0x04000061 RID: 97
		public Vector3 m_InitLocalPosition = Vector3.zero;

		// Token: 0x04000062 RID: 98
		public Quaternion m_InitLocalRotation = Quaternion.identity;
	}
}

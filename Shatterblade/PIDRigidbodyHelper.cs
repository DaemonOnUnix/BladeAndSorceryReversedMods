using System;
using ExtensionMethods;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class PIDRigidbodyHelper
{
	// Token: 0x06000006 RID: 6 RVA: 0x000021EC File Offset: 0x000003EC
	public PIDRigidbodyHelper(Rigidbody rigidbody, float acceleration, float dampening)
	{
		this.rb = rigidbody;
		this.isActive = true;
		this.velocityPID = new PID(acceleration, 0f, 0.3f);
		this.slowingPID = new PID(dampening, 0f, 0.3f);
		this.headingPID = new PID(acceleration, 0f, 0.3f);
		this.dampeningPID = new PID(dampening * 0.2f, 0.2f, 0.1f);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002270 File Offset: 0x00000470
	public void Update(Vector3 targetPos, Quaternion targetRot)
	{
		bool flag = !this.isActive;
		if (!flag)
		{
			this.UpdateVelocity(targetPos, 1f, 1f);
			this.UpdateTorque(targetRot, 1f, 1f);
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000022B4 File Offset: 0x000004B4
	public void UpdateVelocity(Vector3 targetPos, float forceMult = 1f, float slowMult = 1f)
	{
		bool flag = !this.isActive;
		if (!flag)
		{
			bool flag2 = Time.deltaTime == 0f || Time.deltaTime == float.NaN;
			if (!flag2)
			{
				Vector3 force = this.velocityPID.Update(targetPos - this.rb.transform.position, Time.deltaTime).SafetyClamp() * forceMult + this.slowingPID.Update(-this.rb.velocity * 0.1f, Time.deltaTime).SafetyClamp() * slowMult;
				this.rb.AddForce(force);
			}
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002370 File Offset: 0x00000570
	public void UpdateTorque(Quaternion targetRot, float forceMult = 1f, float slowMult = 1f)
	{
		bool flag = !this.isActive;
		if (!flag)
		{
			bool flag2 = Time.deltaTime == 0f || Time.deltaTime == float.NaN;
			if (!flag2)
			{
				Vector3 rotation = (Vector3.Cross(this.rb.transform.rotation * Vector3.forward, targetRot * Vector3.forward) + Vector3.Cross(this.rb.transform.rotation * Vector3.up, targetRot * Vector3.up)).normalized * Quaternion.Angle(this.rb.transform.rotation, targetRot) / 360f;
				Vector3 torque = this.headingPID.Update(rotation, Time.deltaTime).SafetyClamp() * forceMult + this.dampeningPID.Update(-this.rb.angularVelocity, Time.deltaTime).SafetyClamp() * slowMult * 0.1f;
				this.rb.AddTorque(torque);
			}
		}
	}

	// Token: 0x04000006 RID: 6
	public PID velocityPID;

	// Token: 0x04000007 RID: 7
	public PID slowingPID;

	// Token: 0x04000008 RID: 8
	public PID headingPID;

	// Token: 0x04000009 RID: 9
	public PID dampeningPID;

	// Token: 0x0400000A RID: 10
	public Rigidbody rb;

	// Token: 0x0400000B RID: 11
	public bool isActive;
}

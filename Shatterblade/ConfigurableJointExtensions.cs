using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class ConfigurableJointExtensions
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void SetTargetRotationLocal(this ConfigurableJoint joint, Quaternion targetLocalRotation, Quaternion startLocalRotation)
	{
		bool configuredInWorldSpace = joint.configuredInWorldSpace;
		if (configuredInWorldSpace)
		{
			Debug.LogError("SetTargetRotationLocal should not be used with joints that are configured in world space. For world space joints, use SetTargetRotation.", joint);
		}
		ConfigurableJointExtensions.SetTargetRotationInternal(joint, targetLocalRotation, startLocalRotation, 1);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002080 File Offset: 0x00000280
	public static void SetTargetRotation(this ConfigurableJoint joint, Quaternion targetWorldRotation, Quaternion startWorldRotation)
	{
		bool flag = !joint.configuredInWorldSpace;
		if (flag)
		{
			Debug.LogError("SetTargetRotation must be used with joints that are configured in world space. For local space joints, use SetTargetRotationLocal.", joint);
		}
		ConfigurableJointExtensions.SetTargetRotationInternal(joint, targetWorldRotation, startWorldRotation, 0);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000020B4 File Offset: 0x000002B4
	private static void SetTargetRotationInternal(ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation, Space space)
	{
		Vector3 right = joint.axis;
		Vector3 forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
		Vector3 up = Vector3.Cross(forward, right).normalized;
		Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);
		Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);
		bool flag = space == 0;
		if (flag)
		{
			resultRotation *= startRotation * Quaternion.Inverse(targetRotation);
		}
		else
		{
			resultRotation *= Quaternion.Inverse(targetRotation) * startRotation;
		}
		resultRotation *= worldToJointSpace;
		joint.targetRotation = resultRotation;
	}
}

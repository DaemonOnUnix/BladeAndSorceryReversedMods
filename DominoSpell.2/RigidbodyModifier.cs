using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

// Token: 0x02000003 RID: 3
internal class RigidbodyModifier : MonoBehaviour
{
	// Token: 0x0600001D RID: 29 RVA: 0x000033D0 File Offset: 0x000015D0
	public void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		Locomotion loco = this.rb.GetComponent<Locomotion>();
		bool flag = loco != null;
		if (flag)
		{
			this.orgColliderHeight = loco.capsuleCollider.height;
		}
		CollisionHandler component = this.rb.GetComponent<CollisionHandler>();
		ItemData itemData;
		if (component == null)
		{
			itemData = null;
		}
		else
		{
			Item item = component.item;
			itemData = ((item != null) ? item.data : null);
		}
		ItemData data = itemData;
		bool flag2 = data != null;
		if (flag2)
		{
			this.orgDrag = data.drag;
			this.orgAngularDrag = data.angularDrag;
			this.orgMass = data.mass;
		}
		else
		{
			this.orgDrag = this.rb.drag;
			this.orgAngularDrag = this.rb.angularDrag;
			this.orgMass = this.rb.mass;
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00003498 File Offset: 0x00001698
	public void AddModifier(object handler, int priority, float? gravity = null, float? drag = null, float? mass = null)
	{
		this.modifiers[handler] = new RigidbodyModifier.Modifier(priority, gravity, drag, mass);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000034B4 File Offset: 0x000016B4
	public void RemoveModifier(object handler)
	{
		this.modifiers.Remove(handler);
		bool flag = this.rb == null;
		if (!flag)
		{
			bool flag2 = !this.modifiers.Where((KeyValuePair<object, RigidbodyModifier.Modifier> mod) => mod.Value.gravity != null).Any<KeyValuePair<object, RigidbodyModifier.Modifier>>();
			if (flag2)
			{
				this.rb.useGravity = true;
				CollisionHandler component = this.rb.GetComponent<CollisionHandler>();
				if (component != null)
				{
					component.RefreshPhysicModifiers();
				}
				Locomotion loco = this.rb.GetComponent<Locomotion>();
				bool flag3 = loco != null;
				if (flag3)
				{
					loco.capsuleCollider.height = this.orgColliderHeight;
				}
			}
			bool flag4 = !this.modifiers.Where((KeyValuePair<object, RigidbodyModifier.Modifier> mod) => mod.Value.drag != null).Any<KeyValuePair<object, RigidbodyModifier.Modifier>>();
			if (flag4)
			{
				this.rb.drag = this.orgDrag;
				this.rb.angularDrag = this.orgAngularDrag;
			}
			bool flag5 = !this.modifiers.Where((KeyValuePair<object, RigidbodyModifier.Modifier> mod) => mod.Value.mass != null).Any<KeyValuePair<object, RigidbodyModifier.Modifier>>();
			if (flag5)
			{
				this.rb.mass = this.orgMass;
			}
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00003610 File Offset: 0x00001810
	public void Update()
	{
		bool flag = !this.rb;
		if (!flag)
		{
			int lastGravPriority = int.MinValue;
			int lastDragPriority = int.MinValue;
			int lastMassPriority = int.MinValue;
			foreach (RigidbodyModifier.Modifier modifier in this.modifiers.Values)
			{
				float? num = modifier.gravity;
				float gravity;
				bool flag2;
				if (num != null)
				{
					gravity = num.GetValueOrDefault();
					flag2 = modifier.priority > lastGravPriority;
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					lastGravPriority = modifier.priority;
					this.rb.useGravity = false;
					Locomotion loco = this.rb.GetComponent<Locomotion>();
					bool flag4 = loco != null;
					if (flag4)
					{
						loco.capsuleCollider.height = 0.9f;
					}
					this.rb.AddForce(Physics.gravity * gravity);
				}
				num = modifier.drag;
				float drag;
				bool flag5;
				if (num != null)
				{
					drag = num.GetValueOrDefault();
					flag5 = modifier.priority > lastDragPriority;
				}
				else
				{
					flag5 = false;
				}
				bool flag6 = flag5;
				if (flag6)
				{
					lastDragPriority = modifier.priority;
					this.rb.drag = ((this.orgDrag == 0f) ? 1f : this.orgDrag) * drag;
					this.rb.angularDrag = ((this.orgAngularDrag == 0f) ? 1f : this.orgAngularDrag) * drag;
				}
				num = modifier.mass;
				float mass;
				bool flag7;
				if (num != null)
				{
					mass = num.GetValueOrDefault();
					flag7 = modifier.priority > lastMassPriority;
				}
				else
				{
					flag7 = false;
				}
				bool flag8 = flag7;
				if (flag8)
				{
					lastMassPriority = modifier.priority;
					this.rb.mass = ((this.orgMass == 0f) ? 1f : this.orgMass) * mass;
				}
			}
		}
	}

	// Token: 0x04000001 RID: 1
	private Dictionary<object, RigidbodyModifier.Modifier> modifiers = new Dictionary<object, RigidbodyModifier.Modifier>();

	// Token: 0x04000002 RID: 2
	private Rigidbody rb;

	// Token: 0x04000003 RID: 3
	private float orgDrag;

	// Token: 0x04000004 RID: 4
	private float orgAngularDrag;

	// Token: 0x04000005 RID: 5
	private float orgMass;

	// Token: 0x04000006 RID: 6
	private float orgColliderHeight;

	// Token: 0x02000014 RID: 20
	private struct Modifier
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00006BBF File Offset: 0x00004DBF
		public Modifier(int priority, float? gravity = null, float? drag = null, float? mass = null)
		{
			this.priority = priority;
			this.gravity = gravity;
			this.drag = drag;
			this.mass = mass;
		}

		// Token: 0x04000050 RID: 80
		public int priority;

		// Token: 0x04000051 RID: 81
		public float? gravity;

		// Token: 0x04000052 RID: 82
		public float? drag;

		// Token: 0x04000053 RID: 83
		public float? mass;
	}
}

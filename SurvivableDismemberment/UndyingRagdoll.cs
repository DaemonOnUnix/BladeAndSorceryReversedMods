using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace SurvivableDismemberment
{
	// Token: 0x02000003 RID: 3
	public class UndyingRagdoll : MonoBehaviour
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020D8 File Offset: 0x000002D8
		private void Awake()
		{
			Ragdoll componentInChildren = base.gameObject.GetComponentInChildren<Ragdoll>();
			this.part_tree = new UndyingRagdoll.PartTree(componentInChildren.rootPart);
			this.original_connected_bodies = new Dictionary<RagdollPart, Rigidbody>();
			this.original_animation_parent = new Dictionary<RagdollPart, Transform>();
			foreach (RagdollPart ragdollPart in componentInChildren.parts)
			{
				this.part_tree.register(ragdollPart);
				ragdollPart.data.sliceForceKill = false;
				this.original_connected_bodies[ragdollPart] = ragdollPart.bone.animationJoint.connectedBody;
				this.original_animation_parent[ragdollPart] = ragdollPart.bone.animation.parent;
			}
			this.original_max_stabilization_velocity = componentInChildren.creature.groundStabilizationMaxVelocity;
			this.part_tree.arrange_tree();
			componentInChildren.OnSliceEvent += new Ragdoll.SliceEvent(this.Ragdoll_OnSliceEvent);
			componentInChildren.OnStateChange += new Ragdoll.StateChange(this.Ragdoll_OnStateChange);
			componentInChildren.creature.OnDespawnEvent += new Creature.DespawnEvent(this.Creature_OnDespawnEvent);
			componentInChildren.creature.OnKillEvent += new Creature.KillEvent(this.Creature_OnKillEvent);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002224 File Offset: 0x00000424
		private void revert_changes()
		{
			Ragdoll componentInChildren = base.gameObject.GetComponentInChildren<Ragdoll>();
			componentInChildren.creature.groundStabilizationMaxVelocity = this.original_max_stabilization_velocity;
			componentInChildren.creature.stepEnabled = true;
			componentInChildren.RemovePhysicToggleModifier(this);
			foreach (RagdollPart ragdollPart in componentInChildren.parts)
			{
				UndyingRagdoll.PartNode node = this.part_tree.getNode(ragdollPart);
				bool flag = node == null;
				if (!flag)
				{
					bool sliced_off = node.sliced_off;
					if (sliced_off)
					{
						ragdollPart.bone.animationJoint.connectedBody = this.original_connected_bodies[ragdollPart];
						ragdollPart.bone.animation.SetParent(this.original_animation_parent[ragdollPart]);
						ragdollPart.bone.animationJoint.gameObject.SetActive(true);
						ragdollPart.characterJointLocked = false;
					}
				}
			}
			this.part_tree.reset_slice_status();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002338 File Offset: 0x00000538
		private void Creature_OnDespawnEvent(EventTime eventTime)
		{
			bool flag = eventTime == 0;
			if (flag)
			{
				Creature componentInChildren = base.gameObject.GetComponentInChildren<Creature>();
				this.revert_changes();
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002364 File Offset: 0x00000564
		private void onDestroy()
		{
			Creature componentInChildren = base.gameObject.GetComponentInChildren<Creature>();
			this.revert_changes();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002388 File Offset: 0x00000588
		private void Creature_OnKillEvent(CollisionInstance collisionInstance, EventTime eventTime)
		{
			Creature componentInChildren = base.gameObject.GetComponentInChildren<Creature>();
			this.revert_changes();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023AC File Offset: 0x000005AC
		private void Ragdoll_OnStateChange(Ragdoll.State previousState, Ragdoll.State newState, Ragdoll.PhysicStateChange physicStateChange, EventTime eventTime)
		{
			Ragdoll componentInChildren = base.gameObject.GetComponentInChildren<Ragdoll>();
			foreach (RagdollPart ragdollPart in componentInChildren.parts)
			{
				UndyingRagdoll.PartNode node = this.part_tree.getNode(ragdollPart);
				bool flag = node == null;
				if (!flag)
				{
					bool sliced_off = node.sliced_off;
					if (sliced_off)
					{
						ragdollPart.bone.animation.SetParent(ragdollPart.transform);
						ragdollPart.bone.animationJoint.gameObject.SetActive(false);
						ragdollPart.bone.animation.localPosition = Vector3.zero;
						ragdollPart.bone.animation.localRotation = Quaternion.identity;
						bool flag2 = !node.slice_root;
						if (flag2)
						{
							ragdollPart.bone.mesh.SetParent(ragdollPart.transform);
							ragdollPart.bone.mesh.localPosition = Vector3.zero;
							ragdollPart.bone.mesh.localRotation = Quaternion.identity;
						}
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024EC File Offset: 0x000006EC
		private void Ragdoll_OnSliceEvent(RagdollPart ragdollPart, EventTime eventTime)
		{
			bool flag = eventTime == 1;
			if (flag)
			{
				ragdollPart.ragdoll.AddPhysicToggleModifier(this);
				UndyingRagdoll.PartNode node = this.part_tree.getNode(ragdollPart);
				bool flag2 = node != null;
				if (flag2)
				{
					node.sliced_off = true;
					node.slice_root = true;
				}
				foreach (UndyingRagdoll.PartNode partNode in this.part_tree.getSubNodes(ragdollPart))
				{
					partNode.sliced_off = true;
				}
				foreach (HandleRagdoll handleRagdoll in ragdollPart.handles)
				{
					handleRagdoll.handleRagdollData.liftBehaviour = 0;
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000025DC File Offset: 0x000007DC
		private void check_destabilize(Ragdoll ragdoll)
		{
			bool flag = ragdoll.state == 1;
			if (!flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				foreach (RagdollPart ragdollPart in ragdoll.parts)
				{
					bool flag5 = ragdollPart.type == 512 && this.part_tree.getNode(ragdollPart).sliced_off;
					if (flag5)
					{
						flag2 = true;
					}
					bool flag6 = ragdollPart.type == 1024 && this.part_tree.getNode(ragdollPart).sliced_off;
					if (flag6)
					{
						flag3 = true;
					}
					bool flag7 = ragdollPart.type == 1 && this.part_tree.getNode(ragdollPart).sliced_off;
					if (flag7)
					{
						flag4 = true;
					}
				}
				bool flag8 = (flag2 && flag3) || flag4 || (flag2 && LoadModule.destabilizeOneLeg) || (flag3 && LoadModule.destabilizeOneLeg);
				if (flag8)
				{
					ragdoll.SetState(1);
					ragdoll.creature.groundStabilizationMaxVelocity = 0f;
				}
				bool flag9 = flag4;
				if (flag9)
				{
					ragdoll.creature.stepEnabled = false;
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000271C File Offset: 0x0000091C
		private void check_handle_release()
		{
			Ragdoll componentInChildren = base.gameObject.GetComponentInChildren<Ragdoll>();
			RagdollPart part = componentInChildren.GetPart(32);
			RagdollPart part2 = componentInChildren.GetPart(64);
			bool flag = part && this.part_tree.getNode(part).sliced_off;
			if (flag)
			{
				componentInChildren.creature.handLeft.TryRelease();
			}
			bool flag2 = part2 && this.part_tree.getNode(part2).sliced_off;
			if (flag2)
			{
				componentInChildren.creature.handRight.TryRelease();
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000027AC File Offset: 0x000009AC
		private void check_head_kill()
		{
			bool dieOnHeadChop = LoadModule.dieOnHeadChop;
			if (dieOnHeadChop)
			{
				Ragdoll componentInChildren = base.gameObject.GetComponentInChildren<Ragdoll>();
				bool flag = componentInChildren.headPart && this.part_tree.getNode(componentInChildren.headPart).sliced_off;
				if (flag)
				{
					bool flag2 = componentInChildren.creature.maxHealth != float.MaxValue;
					if (flag2)
					{
						CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(4, float.MaxValue), null, null);
						componentInChildren.creature.Damage(collisionInstance);
					}
				}
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000283C File Offset: 0x00000A3C
		private void Update()
		{
			Ragdoll componentInChildren = base.gameObject.GetComponentInChildren<Ragdoll>();
			bool isKilled = componentInChildren.creature.isKilled;
			if (!isKilled)
			{
				this.check_destabilize(componentInChildren);
				this.check_handle_release();
				this.check_head_kill();
				foreach (RagdollPart ragdollPart in componentInChildren.parts)
				{
					UndyingRagdoll.PartNode node = this.part_tree.getNode(ragdollPart);
					bool flag = node != null;
					if (flag)
					{
						bool sliced_off = node.sliced_off;
						if (sliced_off)
						{
							ragdollPart.rb.isKinematic = false;
							bool slice_root = node.slice_root;
							if (slice_root)
							{
								ragdollPart.DestroyCharJoint();
								ragdollPart.characterJointLocked = true;
							}
							ragdollPart.transform.SetParent(null);
							ragdollPart.bone.animationJoint.connectedBody = null;
							bool flag2 = ragdollPart.bone.fixedJoint;
							if (flag2)
							{
								Object.Destroy(ragdollPart.bone.fixedJoint);
							}
							ragdollPart.collisionHandler.RemovePhysicModifier(componentInChildren);
						}
					}
				}
			}
		}

		// Token: 0x04000003 RID: 3
		private UndyingRagdoll.PartTree part_tree;

		// Token: 0x04000004 RID: 4
		private Dictionary<RagdollPart, Rigidbody> original_connected_bodies = new Dictionary<RagdollPart, Rigidbody>();

		// Token: 0x04000005 RID: 5
		private Dictionary<RagdollPart, Transform> original_animation_parent = new Dictionary<RagdollPart, Transform>();

		// Token: 0x04000006 RID: 6
		private float original_max_stabilization_velocity;

		// Token: 0x02000004 RID: 4
		public class PartNode
		{
			// Token: 0x04000007 RID: 7
			public RagdollPart part;

			// Token: 0x04000008 RID: 8
			public UndyingRagdoll.PartNode parent;

			// Token: 0x04000009 RID: 9
			public List<UndyingRagdoll.PartNode> children;

			// Token: 0x0400000A RID: 10
			public bool sliced_off = false;

			// Token: 0x0400000B RID: 11
			public bool slice_root = false;
		}

		// Token: 0x02000005 RID: 5
		public class PartTree
		{
			// Token: 0x06000012 RID: 18 RVA: 0x000029AA File Offset: 0x00000BAA
			public PartTree(RagdollPart _root)
			{
				this.register(_root);
				this.root = this.part_map[_root];
			}

			// Token: 0x06000013 RID: 19 RVA: 0x000029DC File Offset: 0x00000BDC
			public void arrange_tree()
			{
				foreach (KeyValuePair<RagdollPart, UndyingRagdoll.PartNode> keyValuePair in this.part_map)
				{
					UndyingRagdoll.PartNode value = keyValuePair.Value;
					RagdollPart part = value.part;
					value.parent = null;
					bool flag = part.parentPart && this.part_map.ContainsKey(part.parentPart);
					if (flag)
					{
						value.parent = this.part_map[part.parentPart];
						bool flag2 = !value.parent.children.Contains(value);
						if (flag2)
						{
							value.parent.children.Add(value);
						}
					}
				}
			}

			// Token: 0x06000014 RID: 20 RVA: 0x00002AB8 File Offset: 0x00000CB8
			public void register(RagdollPart rp)
			{
				UndyingRagdoll.PartNode partNode = new UndyingRagdoll.PartNode();
				partNode.part = rp;
				partNode.children = new List<UndyingRagdoll.PartNode>();
				bool flag = !this.part_map.ContainsKey(rp);
				if (flag)
				{
					this.part_map[rp] = partNode;
				}
			}

			// Token: 0x06000015 RID: 21 RVA: 0x00002B00 File Offset: 0x00000D00
			public void reset_slice_status()
			{
				this.root.sliced_off = false;
				this.root.slice_root = false;
				foreach (UndyingRagdoll.PartNode partNode in this.getSubNodes(this.root.part))
				{
					partNode.sliced_off = false;
					partNode.slice_root = false;
				}
			}

			// Token: 0x06000016 RID: 22 RVA: 0x00002B84 File Offset: 0x00000D84
			public UndyingRagdoll.PartNode getNode(RagdollPart p)
			{
				bool flag = p && this.part_map.ContainsKey(p);
				UndyingRagdoll.PartNode partNode;
				if (flag)
				{
					partNode = this.part_map[p];
				}
				else
				{
					partNode = null;
				}
				return partNode;
			}

			// Token: 0x06000017 RID: 23 RVA: 0x00002BC4 File Offset: 0x00000DC4
			public List<UndyingRagdoll.PartNode> getSubNodes(RagdollPart p)
			{
				bool flag = !p;
				List<UndyingRagdoll.PartNode> list;
				if (flag)
				{
					list = null;
				}
				else
				{
					bool flag2 = !this.part_map.ContainsKey(p);
					if (flag2)
					{
						list = null;
					}
					else
					{
						List<UndyingRagdoll.PartNode> list2 = new List<UndyingRagdoll.PartNode>();
						UndyingRagdoll.PartNode partNode = this.part_map[p];
						this.getSubNodes_impl(partNode, list2);
						list = list2;
					}
				}
				return list;
			}

			// Token: 0x06000018 RID: 24 RVA: 0x00002C20 File Offset: 0x00000E20
			private void getSubNodes_impl(UndyingRagdoll.PartNode n, List<UndyingRagdoll.PartNode> parts)
			{
				foreach (UndyingRagdoll.PartNode partNode in n.children)
				{
					parts.Add(partNode);
					this.getSubNodes_impl(partNode, parts);
				}
			}

			// Token: 0x0400000C RID: 12
			private Dictionary<RagdollPart, UndyingRagdoll.PartNode> part_map = new Dictionary<RagdollPart, UndyingRagdoll.PartNode>();

			// Token: 0x0400000D RID: 13
			public UndyingRagdoll.PartNode root;
		}
	}
}

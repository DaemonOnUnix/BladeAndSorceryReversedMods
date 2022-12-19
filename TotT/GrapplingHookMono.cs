using System;
using System.Collections;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TotT
{
	// Token: 0x02000005 RID: 5
	public class GrapplingHookMono : ArmModule
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002398 File Offset: 0x00000598
		public override void OnStart()
		{
			base.OnStart();
			this.RopeTarget = new GameObject().transform;
			this.HasAltMode = false;
			this.Whirring = false;
			this.Indicator = this.item.GetCustomReference("Indicator", true).GetComponent<Renderer>();
			this.Hook = this.item.GetCustomReference("Hook", true).GetComponent<Renderer>();
			this.LoadRopeMaterial();
			this.CurrentColor = this.OnColor;
			this.Positions = new Vector3[2];
			ModuleHandWatcher[] checker = this.item.GetComponents<ModuleHandWatcher>();
			foreach (ModuleHandWatcher watcher in checker)
			{
				Object.Destroy(watcher);
			}
			this.lineRenderer = this.item.flyDirRef.gameObject.GetComponent<LineRenderer>();
			this.SetUpLineRenderer();
			this.customData = new ArmModuleSave();
			this.item.TryGetCustomData<ArmModuleSave>(ref this.customData);
			bool flag = this.customData != null;
			if (flag)
			{
				bool onOff = this.customData.OnOff;
				if (onOff)
				{
					this.OnOff = true;
				}
				else
				{
					this.OnOff = false;
				}
			}
			else
			{
				this.OnOff = true;
				this.customData = new ArmModuleSave();
			}
			bool flag2 = this.item.holder != null;
			if (flag2)
			{
				this.OnSnapEvent(this.item.holder);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002504 File Offset: 0x00000704
		public void SaveData()
		{
			bool flag = this.customData != null;
			if (flag)
			{
				this.customData.OnOff = this.OnOff;
				this.item.RemoveCustomData<ArmModuleSave>();
				this.item.AddCustomData<ArmModuleSave>(this.customData);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002554 File Offset: 0x00000754
		public override void Activate()
		{
			RaycastHit hitInfo;
			bool flag = Physics.Raycast(this.item.flyDirRef.transform.position, -this.Hand.transform.right, ref hitInfo, this.maxDistance, LayerMask.GetMask(new string[] { "Default", "MovingItem", "DroppedItem", "ItemAndRagdollOnly", "NPC", "Ragdoll", "Item", "PlayerLocomotionObject" }), 1);
			if (flag)
			{
				this.Activated = true;
				bool flag2 = hitInfo.rigidbody == null;
				if (flag2)
				{
					this.FireSFX.Spawn(this.Hand.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
					this.Joint = Player.local.locomotion.gameObject.AddComponent<SpringJoint>();
					this.SetupJoint(this.Joint, hitInfo);
					this.ControllerGrab();
					bool flag3 = this.lineRendererReady;
					if (flag3)
					{
						this.RopeTarget.position = hitInfo.point;
						this.lineRendererOn = true;
					}
					this.Hook.enabled = false;
				}
				else
				{
					this.ItemGrappled = hitInfo.rigidbody.GetComponentInParent<Item>();
					this.CreatureGrappled = hitInfo.rigidbody.GetComponentInParent<Creature>();
					bool flag4 = !this.ItemGrappled && !this.CreatureGrappled;
					if (flag4)
					{
						this.FireSFX.Spawn(this.Hand.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
						this.Joint = Player.local.locomotion.gameObject.AddComponent<SpringJoint>();
						this.SetupJoint(this.Joint, hitInfo);
						this.RopeTarget.position = hitInfo.point;
						this.ControllerGrab();
						this.lineRendererOn = true;
						this.Hook.enabled = false;
					}
					else
					{
						this.Activated = false;
					}
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000276E File Offset: 0x0000096E
		public override void On()
		{
			this.OnOff = true;
			base.StartCoroutine(this.ChangeColor(this.OnColor));
			this.SaveData();
			base.On();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002799 File Offset: 0x00000999
		public override void Off()
		{
			this.OnOff = false;
			base.StartCoroutine(this.ChangeColor(this.OffColor));
			this.SaveData();
			base.Off();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000027C4 File Offset: 0x000009C4
		public override void giveHand(RagdollHand hand)
		{
			this.Hand = hand;
			bool flag = this.controlItem == null;
			if (flag)
			{
				this.Controller.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Controller_OnUngrab);
					sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Controller_OnHeldAction);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.controlItem = sitem;
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
			}
			base.giveHand(hand);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002838 File Offset: 0x00000A38
		public void ControllerGrab()
		{
			bool flag = this.Hand.side == 1;
			Vector3 offset;
			if (flag)
			{
				offset..ctor(0f, -95f, -75f);
			}
			else
			{
				offset..ctor(0f, -95f, -75f);
			}
			bool flag2 = this.controlItem != null;
			if (flag2)
			{
				this.controlItem.gameObject.SetActive(true);
				this.controlItem.gameObject.transform.position = this.Hand.transform.position;
				this.controlItem.gameObject.transform.rotation = this.Hand.transform.rotation * Quaternion.Euler(offset.x, offset.y, offset.z);
				foreach (Renderer r in this.controlItem.renderers)
				{
					r.enabled = true;
				}
				this.Hand.Grab(this.controlItem.GetMainHandle(this.Hand.side));
				base.WaterFix(this.controlItem);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000299C File Offset: 0x00000B9C
		public void SetupJoint(SpringJoint joint, RaycastHit hitInfo)
		{
			joint.autoConfigureConnectedAnchor = false;
			joint.spring = 6000f;
			joint.damper = 800f;
			joint.breakForce = float.PositiveInfinity;
			joint.breakTorque = float.PositiveInfinity;
			joint.anchor = Vector3.zero;
			joint.connectedAnchor = hitInfo.point;
			joint.minDistance = 0.01f;
			joint.maxDistance = hitInfo.distance;
			joint.enableCollision = true;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A20 File Offset: 0x00000C20
		public void SetupItemJoint(SpringJoint joint, RaycastHit hitInfo, Item grappledItem)
		{
			joint.autoConfigureConnectedAnchor = false;
			joint.spring = 6000f;
			joint.damper = 800f;
			joint.breakForce = float.PositiveInfinity;
			joint.breakTorque = float.PositiveInfinity;
			joint.connectedBody = grappledItem.rb;
			joint.connectedMassScale = 20f;
			joint.anchor = Vector3.zero;
			joint.connectedAnchor = this.item.rb.gameObject.transform.InverseTransformPoint(hitInfo.point);
			joint.minDistance = 0.01f;
			joint.maxDistance = hitInfo.distance;
			joint.enableCollision = true;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public override void OnUpdate()
		{
			base.OnUpdate();
			bool flag = this.Joint;
			if (flag)
			{
				this.Joint.anchor = Player.local.locomotion.transform.InverseTransformPoint(this.item.flyDirRef.position);
				bool flag2 = this.Retract && this.Extend;
				if (flag2)
				{
					bool whirring = this.Whirring;
					if (whirring)
					{
						this.WhirEffect.End(false, -1f);
						this.Whirring = false;
					}
				}
				else
				{
					bool retract = this.Retract;
					if (retract)
					{
						bool flag3 = !this.Whirring;
						if (flag3)
						{
							this.WhirEffect = this.WhirSFX.Spawn(this.Hand.transform, true, null, false, Array.Empty<Type>());
							this.WhirEffect.Play(0, false);
							this.Whirring = true;
						}
						bool flag4 = this.Joint.maxDistance - Time.deltaTime * 3f > this.Joint.minDistance;
						if (flag4)
						{
							this.Joint.maxDistance -= Time.deltaTime * 3f;
						}
					}
					else
					{
						bool extend = this.Extend;
						if (extend)
						{
							bool flag5 = !this.Whirring;
							if (flag5)
							{
								this.WhirEffect = this.WhirSFX.Spawn(this.Hand.transform, true, null, false, Array.Empty<Type>());
								this.WhirEffect.Play(0, false);
								this.Whirring = true;
							}
							bool flag6 = this.Joint.maxDistance + Time.deltaTime * 3f < this.maxDistance;
							if (flag6)
							{
								this.Joint.maxDistance += Time.deltaTime * 3f;
							}
						}
						else
						{
							bool whirring2 = this.Whirring;
							if (whirring2)
							{
								this.WhirEffect.End(false, -1f);
								this.Whirring = false;
							}
						}
					}
				}
			}
			bool flag7 = this.lineRendererOn;
			if (flag7)
			{
				this.Positions[0] = this.item.flyDirRef.position;
				this.Positions[1] = this.RopeTarget.position;
				this.lineRenderer.SetPositions(this.Positions);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002D38 File Offset: 0x00000F38
		private void Controller_OnHeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 0;
			if (flag)
			{
				this.Retract = true;
			}
			else
			{
				bool flag2 = action == 1;
				if (flag2)
				{
					this.Retract = false;
				}
				else
				{
					bool flag3 = action == 2;
					if (flag3)
					{
						this.Extend = true;
					}
					else
					{
						bool flag4 = action == 3;
						if (flag4)
						{
							this.Extend = false;
						}
					}
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002D90 File Offset: 0x00000F90
		private void Controller_OnUngrab(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			this.Whirring = false;
			bool flag = this.WhirEffect != null;
			if (flag)
			{
				this.WhirEffect.End(false, -1f);
			}
			this.controlItem.gameObject.SetActive(false);
			bool flag2 = this.Joint;
			if (flag2)
			{
				Object.Destroy(this.Joint);
			}
			this.Extend = false;
			this.Retract = false;
			this.Positions[0] = this.ZeroPoint;
			this.Positions[1] = this.ZeroPoint;
			this.lineRenderer.SetPositions(this.Positions);
			this.lineRendererOn = false;
			this.ItemGrappled = null;
			this.isItemGrappled = false;
			this.RopeTarget.parent = null;
			ragdollHand.ClearTouch();
			this.Activated = false;
			this.Hook.enabled = true;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002E78 File Offset: 0x00001078
		public override void OnUnSnapEvent(Holder holder)
		{
			base.OnUnSnapEvent(holder);
			RagdollHand hand = this.Hand;
			string text;
			if (hand == null)
			{
				text = null;
			}
			else
			{
				Handle grabbedHandle = hand.grabbedHandle;
				if (grabbedHandle == null)
				{
					text = null;
				}
				else
				{
					Item item = grabbedHandle.item;
					text = ((item != null) ? item.data.id : null);
				}
			}
			bool flag = text == "GrooveSlinger.TotT.GrappleController";
			if (flag)
			{
				this.Hand.UnGrab(false);
			}
			bool flag2 = this.controlItem != null;
			if (flag2)
			{
				this.controlItem.Despawn();
				this.controlItem = null;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002F00 File Offset: 0x00001100
		public override void OnSnapEvent(Holder holder)
		{
			base.OnSnapEvent(holder);
			bool flag = holder.data.id == "GrooveSlinger.RightWristHolderID";
			if (flag)
			{
				this.Controller.SpawnAsync(delegate(Item sitem)
				{
					sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Controller_OnUngrab);
					sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Controller_OnHeldAction);
					sitem.disallowDespawn = true;
					sitem.disallowRoomDespawn = true;
					sitem.Set("cullingDetectionEnabled", false);
					sitem.gameObject.SetActive(false);
					this.controlItem = sitem;
				}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
			}
			else
			{
				bool flag2 = holder.data.id == "GrooveSlinger.LeftWristHolderID";
				if (flag2)
				{
					this.Controller.SpawnAsync(delegate(Item sitem)
					{
						sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Controller_OnUngrab);
						sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Controller_OnHeldAction);
						sitem.disallowDespawn = true;
						sitem.disallowRoomDespawn = true;
						sitem.Set("cullingDetectionEnabled", false);
						sitem.gameObject.SetActive(false);
						this.controlItem = sitem;
					}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
				}
				else
				{
					this.Mount = holder.parentItem.GetComponent<RailMountsMono>();
					this.Hand = this.Mount.GetHand();
					bool flag3 = this.Hand != null;
					if (flag3)
					{
						this.Controller.SpawnAsync(delegate(Item sitem)
						{
							sitem.OnUngrabEvent += new Item.ReleaseDelegate(this.Controller_OnUngrab);
							sitem.OnHeldActionEvent += new Item.HeldActionDelegate(this.Controller_OnHeldAction);
							sitem.disallowDespawn = true;
							sitem.disallowRoomDespawn = true;
							sitem.Set("cullingDetectionEnabled", false);
							sitem.gameObject.SetActive(false);
							this.controlItem = sitem;
						}, new Vector3?(this.Hand.transform.position), new Quaternion?(this.Hand.transform.rotation), null, true, null);
					}
				}
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003058 File Offset: 0x00001258
		public void TriggerAction()
		{
			bool flag = this.toggleMethod == toggleMethod.Trigger;
			if (flag)
			{
				this.Hand.playerHand.controlHand.HapticShort(2f);
				bool onOff = this.OnOff;
				if (onOff)
				{
					this.OnOff = false;
					base.StartCoroutine(this.ChangeColor(this.OffColor));
				}
				else
				{
					this.OnOff = true;
					base.StartCoroutine(this.ChangeColor(this.OnColor));
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000030D4 File Offset: 0x000012D4
		public void AltUseAction()
		{
			bool flag = this.toggleMethod == toggleMethod.AltUse;
			if (flag)
			{
				this.Hand.playerHand.controlHand.HapticShort(2f);
				bool onOff = this.OnOff;
				if (onOff)
				{
					this.OnOff = false;
					base.StartCoroutine(this.ChangeColor(this.OffColor));
				}
				else
				{
					this.OnOff = true;
					base.StartCoroutine(this.ChangeColor(this.OnColor));
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003150 File Offset: 0x00001350
		public float getJointDistance()
		{
			bool flag = this.Joint;
			float num;
			if (flag)
			{
				num = Vector3.Distance(this.item.flyDirRef.position, this.RopeTarget.position);
			}
			else
			{
				num = 1000f;
			}
			return num;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000319C File Offset: 0x0000139C
		public void SetUpLineRenderer()
		{
			this.lineRenderer.positionCount = 2;
			this.Positions[0] = this.ZeroPoint;
			this.Positions[1] = this.ZeroPoint;
			this.lineRenderer.SetPositions(this.Positions);
			this.lineRendererReady = true;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000031F4 File Offset: 0x000013F4
		public void LoadRopeMaterial()
		{
			Addressables.LoadAssetAsync<Material>("GrooveSlinger.TotT.RopeMat").Completed += delegate(AsyncOperationHandle<Material> handle)
			{
				bool flag = handle.Result == null;
				if (flag)
				{
					Debug.Log("TotT: Couldn't Find Rope Material!");
				}
				else
				{
					this.RopeMaterial = handle.Result;
				}
			};
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003221 File Offset: 0x00001421
		public IEnumerator ChangeColor(Color newColor)
		{
			yield return new WaitForSeconds(0.1f);
			float tts = 0.2f;
			float timeElapsed = 0f;
			Color toHitC = newColor;
			Color CurrentC = this.CurrentColor;
			Material i = this.Indicator.material;
			while (timeElapsed <= tts)
			{
				float tempR = Mathf.Lerp(CurrentC.r, toHitC.r, timeElapsed / tts);
				float tempG = Mathf.Lerp(CurrentC.g, toHitC.g, timeElapsed / tts);
				float tempB = Mathf.Lerp(CurrentC.b, toHitC.b, timeElapsed / tts);
				float tempA = Mathf.Lerp(CurrentC.a, toHitC.a, timeElapsed / tts);
				Color tempColor = new Color(tempR, tempG, tempB, tempA);
				i.SetColor("_EmissionColor", tempColor);
				timeElapsed += Time.deltaTime;
				yield return null;
			}
			i.SetColor("_EmissionColor", toHitC);
			this.CurrentColor = newColor;
			yield break;
		}

		// Token: 0x04000003 RID: 3
		private Renderer Indicator;

		// Token: 0x04000004 RID: 4
		private Renderer Hook;

		// Token: 0x04000005 RID: 5
		private Item controlItem;

		// Token: 0x04000006 RID: 6
		private toggleMethod toggleMethod = GrapplingHookParser.toggleMethod;

		// Token: 0x04000007 RID: 7
		private float maxDistance = 30f;

		// Token: 0x04000008 RID: 8
		private Transform RopeTarget;

		// Token: 0x04000009 RID: 9
		private Material RopeMaterial;

		// Token: 0x0400000A RID: 10
		private SpringJoint Joint;

		// Token: 0x0400000B RID: 11
		private Vector3[] Positions;

		// Token: 0x0400000C RID: 12
		private LineRenderer lineRenderer;

		// Token: 0x0400000D RID: 13
		private Vector3 ZeroPoint = Vector3.zero;

		// Token: 0x0400000E RID: 14
		private bool lineRendererReady = false;

		// Token: 0x0400000F RID: 15
		private bool lineRendererOn = false;

		// Token: 0x04000010 RID: 16
		private bool Extend = false;

		// Token: 0x04000011 RID: 17
		private bool Retract = false;

		// Token: 0x04000012 RID: 18
		private bool isItemGrappled = false;

		// Token: 0x04000013 RID: 19
		private Item ItemGrappled;

		// Token: 0x04000014 RID: 20
		private Creature CreatureGrappled;

		// Token: 0x04000015 RID: 21
		private ItemData Controller = Catalog.GetData<ItemData>("GrooveSlinger.TotT.GrappleController", true);

		// Token: 0x04000016 RID: 22
		private EffectData FireSFX = Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredFire", true);

		// Token: 0x04000017 RID: 23
		private EffectData WhirSFX = Catalog.GetData<EffectData>("GrooveSlinger.Effect.GrapplingHook.Whir", true);

		// Token: 0x04000018 RID: 24
		private EffectInstance WhirEffect;

		// Token: 0x04000019 RID: 25
		private bool Whirring;

		// Token: 0x0400001A RID: 26
		private Color OnColor = new Color(0f, 3.564869f, 0.4960452f, 1f);

		// Token: 0x0400001B RID: 27
		private Color CurrentColor;

		// Token: 0x0400001C RID: 28
		private Color OffColor = new Color(3.564869f, 0f, 0.03601667f, 1f);
	}
}

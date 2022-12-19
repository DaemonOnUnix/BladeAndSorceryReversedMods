using System;
using ThunderRoad;
using UnityEngine;

namespace SwordBeam
{
	// Token: 0x02000003 RID: 3
	public class SwordBeamComponent : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C8 File Offset: 0x000002C8
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F0 File Offset: 0x000002F0
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			string text = this.activation;
			string text2 = text;
			if (!(text2 == "Trigger"))
			{
				if (!(text2 == "Alt Use"))
				{
					bool flag = action == 0;
					if (flag)
					{
						this.beam = true;
					}
					else
					{
						bool flag2 = action == 1;
						if (flag2)
						{
							this.beam = false;
						}
					}
				}
				else
				{
					bool flag3 = action == 2;
					if (flag3)
					{
						this.beam = true;
					}
					else
					{
						bool flag4 = action == 3;
						if (flag4)
						{
							this.beam = false;
						}
					}
				}
			}
			else
			{
				bool flag5 = action == 0;
				if (flag5)
				{
					this.beam = true;
				}
				else
				{
					bool flag6 = action == 1;
					if (flag6)
					{
						this.beam = false;
					}
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002194 File Offset: 0x00000394
		public void Setup(string projId, string activateMethod, bool beamDismember, float BeamSpeed, float SwordSpeed, float BeamDespawn, float BeamDamage, float BeamCooldown, Color color, Color emission, Vector3 size, Vector3 scale)
		{
			this.beamID = projId;
			this.dismember = beamDismember;
			this.beamSpeed = BeamSpeed;
			this.swordSpeed = SwordSpeed;
			this.despawnTime = BeamDespawn;
			this.beamDamage = BeamDamage;
			this.cooldown = BeamCooldown;
			bool flag = activateMethod.ToLower().Contains("trigger") || activateMethod.ToLower() == "use";
			if (flag)
			{
				this.activation = "Trigger";
			}
			else
			{
				bool flag2 = activateMethod.ToLower().Contains("alt") || activateMethod.ToLower().Contains("spell");
				if (flag2)
				{
					this.activation = "Alt Use";
				}
			}
			this.beamColor = color;
			this.beamEmission = emission;
			this.beamSize = size;
			this.beamScaleIncrease = scale;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002268 File Offset: 0x00000468
		public void FixedUpdate()
		{
			bool flag = Time.time - this.cdH <= this.cooldown || !this.beam || this.item.rb.velocity.magnitude - Player.local.locomotion.rb.velocity.magnitude < this.swordSpeed;
			if (!flag)
			{
				this.cdH = Time.time;
				Catalog.GetData<ItemData>(this.beamID, true).SpawnAsync(new Action<Item>(this.ShootBeam), new Vector3?(this.item.transform.position), new Quaternion?(Quaternion.LookRotation(Player.local.head.cam.transform.forward, this.item.rb.velocity)), null, true, null);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002350 File Offset: 0x00000550
		public void ShootBeam(Item spawnedItem)
		{
			spawnedItem.rb.useGravity = false;
			spawnedItem.rb.drag = 0f;
			spawnedItem.rb.AddForce(Player.local.head.transform.forward * this.beamSpeed, 1);
			spawnedItem.IgnoreRagdollCollision(Player.local.creature.ragdoll);
			spawnedItem.IgnoreObjectCollision(this.item);
			spawnedItem.RefreshCollision(true);
			spawnedItem.gameObject.AddComponent<SwordBeam>().Setup(this.beamDamage, this.dismember, this.beamColor, this.beamEmission, this.beamSize, this.beamScaleIncrease);
			spawnedItem.Throw(1f, 1);
			spawnedItem.Despawn(this.despawnTime);
		}

		// Token: 0x0400000D RID: 13
		private Item item;

		// Token: 0x0400000E RID: 14
		private float cdH;

		// Token: 0x0400000F RID: 15
		private float cooldown;

		// Token: 0x04000010 RID: 16
		private string activation;

		// Token: 0x04000011 RID: 17
		private float despawnTime;

		// Token: 0x04000012 RID: 18
		private float swordSpeed;

		// Token: 0x04000013 RID: 19
		private float beamSpeed;

		// Token: 0x04000014 RID: 20
		private float beamDamage;

		// Token: 0x04000015 RID: 21
		private bool dismember;

		// Token: 0x04000016 RID: 22
		private string beamID;

		// Token: 0x04000017 RID: 23
		private bool beam;

		// Token: 0x04000018 RID: 24
		public Color beamColor;

		// Token: 0x04000019 RID: 25
		public Color beamEmission;

		// Token: 0x0400001A RID: 26
		public Vector3 beamSize;

		// Token: 0x0400001B RID: 27
		public Vector3 beamScaleIncrease;
	}
}

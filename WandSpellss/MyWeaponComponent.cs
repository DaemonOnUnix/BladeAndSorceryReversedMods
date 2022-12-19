using System;
using System.Collections.Generic;
using System.Timers;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace WandSpellss
{
	// Token: 0x0200001E RID: 30
	public class MyWeaponComponent : MonoBehaviour
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00004848 File Offset: 0x00002A48
		public void Start()
		{
			this.canAccio = false;
			this.playSound = false;
			this.ignoreTouch = false;
			this.canFire = false;
			this.firstTouch = 0;
			this.touch = base.GetComponent<AudioSource>();
			this.pressedIndex = 0;
			this.waitFlag = false;
			this.avadaLightning = Catalog.GetData<ItemData>("AvadaKedavraLightning", true);
			this.distortionEffect = Catalog.GetData<ItemData>("Distortion", true);
			this.item = base.GetComponent<Item>();
			this.spellsListWithText.Add("Stupefy");
			this.spellsListWithText.Add("Expelliarmus");
			this.spellsListWithText.Add("AvadaKedavra");
			this.spellsListWithText.Add("PetrifucTotallus");
			this.spellsListWithText.Add("Protego");
			this.spellsListWithText.Add("Lumos");
			this.spellsListWithText.Add("Accio");
			this.spellsListWithText.Add("Engorgio");
			this.spellsListWithText.Add("Evanesco");
			this.spellsListWithText.Add("Geminio");
			this.spellsListWithText.Add("SectumSempra");
			this.spellsListWithText.Add("Levicorpus");
			this.spellsListWithText.Add("WindgardiumLeviosa");
			this.spellsList.Add(Catalog.GetData<ItemData>("StupefyObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("ExpelliarmusObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("AvadaKedavraObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("PetrificusTotalusObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("LumosObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("LumosObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("SectumsempraObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("LevicorpusObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("LevicorpusObject", true));
			this.textObject = Catalog.GetData<ItemData>("TextForSpells", true);
			this.engorgioMaxSize = new Vector3(2f, 2f, 2f);
			Addressables.LoadAssetAsync<Material>("apoz123Wand.SpellEffect.Evanesco.Mat").Completed += this.Op_Completed1;
			this.dissolveVal = 0f;
			this.currIndex = 0;
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGrabEvent);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004B58 File Offset: 0x00002D58
		private void Op_Completed1(AsyncOperationHandle<Material> obj)
		{
			bool flag = obj.Status == 1;
			if (flag)
			{
				this.evanescoDissolve = obj.Result;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004B84 File Offset: 0x00002D84
		private void Item_OnGrabEvent(Handle arg1, RagdollHand arg2)
		{
			bool flag = this.firstTouch == 0 && this.magicEffect;
			if (flag)
			{
				this.canFire = false;
				Vector3 transformLocal;
				transformLocal..ctor(Player.local.head.transform.position.x, Player.local.head.transform.position.y, Player.local.head.transform.position.z);
				Quaternion tranformRotLocal;
				tranformRotLocal..ctor(Player.local.head.transform.rotation.x, Player.local.head.transform.rotation.y, Player.local.head.transform.rotation.z, Player.local.head.transform.rotation.w);
				Catalog.GetData<ItemData>("GoldenLight", true).SpawnAsync(delegate(Item spawned)
				{
					Debug.Log(spawned);
					spawned.transform.position = new Vector3(Player.local.head.transform.position.x, Player.local.head.transform.position.y + 2.5f, Player.local.head.transform.position.z);
					spawned.transform.rotation = Player.local.head.transform.rotation;
					spawned.rb.useGravity = false;
					spawned.rb.drag = 0f;
					this.currentGoldenLight = spawned;
				}, null, null, null, true, null);
				this.touch.Play();
				this.firstTouch++;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004CC0 File Offset: 0x00002EC0
		public void Update()
		{
			bool flag = this.currentText != null;
			if (flag)
			{
				this.currentText.transform.position = this.item.flyDirRef.position;
				this.currentText.transform.rotation = Player.local.head.transform.rotation;
			}
			bool flag2 = this.item.mainHandler != null;
			if (flag2)
			{
				this.oppositeHand = this.item.mainHandler.otherHand;
				this.endPoint = this.oppositeHand.transform.position;
			}
			bool flag3 = this.currentDistortion != null;
			if (flag3)
			{
				this.currentDistortion.transform.position = this.item.flyDirRef.position;
				this.currentDistortion.transform.rotation = this.item.flyDirRef.rotation;
			}
			bool flag4 = this.currentGoldenLight != null;
			if (flag4)
			{
				this.currentGoldenLight.transform.position = new Vector3(Player.local.head.transform.position.x, Player.local.head.transform.position.y + 2.5f, Player.local.head.transform.position.z);
				this.currentGoldenLight.transform.rotation = Player.local.head.transform.rotation;
			}
			bool flag5 = this.currIndex == 4;
			if (flag5)
			{
				this.current.transform.position = this.item.flyDirRef.transform.position;
				this.current.transform.rotation = this.item.flyDirRef.transform.rotation;
			}
			else
			{
				bool flag6 = this.currIndex == 5;
				if (flag6)
				{
					this.current.transform.position = this.item.flyDirRef.position;
					this.current.transform.rotation = this.item.flyDirRef.transform.rotation;
				}
			}
			bool flag7 = this.canAccio;
			if (flag7)
			{
				this.elapsedTime += Time.deltaTime;
				float percentageComplete = this.elapsedTime / this.duration;
				float distanceSqr = (this.endPoint - this.parentLocal.transform.position).sqrMagnitude;
				this.parentLocal.transform.position = Vector3.Lerp(this.startPoint, this.endPoint, Mathf.SmoothStep(0f, 1f, percentageComplete));
				bool flag8 = distanceSqr <= 0.03f;
				if (flag8)
				{
					this.canAccio = false;
					this.elapsedTime = 0f;
					bool flag9 = this.oppositeHand == Player.local.handLeft.ragdollHand;
					if (flag9)
					{
						Player.currentCreature.GetHand(1).Grab(this.parentLocal.GetComponent<Item>().mainHandleRight);
					}
					else
					{
						Player.currentCreature.GetHand(0).Grab(this.parentLocal.GetComponent<Item>().mainHandleRight);
					}
				}
			}
			bool flag10 = this.objectIsHovering;
			if (flag10)
			{
				Debug.Log("Got here: past objectIsHoveringCheck");
				float targetPos = Vector3.Distance(this.item.gameObject.GetComponent<WingardiumLeviosaJoint>().hitObjectItem.transform.position, this.item.flyDirRef.transform.position);
				Vector3 newestPos = this.item.flyDirRef.transform.forward * targetPos;
				this.item.gameObject.GetComponent<WingardiumLeviosaJoint>().hitObjectItem.gameObject.GetComponent<ConfigurableJoint>().targetPosition = newestPos;
			}
			bool flag11 = !this.touch.isPlaying;
			if (flag11)
			{
				this.canFire = true;
				this.ignoreTouch = true;
				bool flag12 = this.currentGoldenLight != null;
				if (flag12)
				{
					this.currentGoldenLight.Despawn();
				}
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005110 File Offset: 0x00003310
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 0;
			if (flag)
			{
				this.parentLocal = null;
				bool flag2 = this.item.gameObject.GetComponent<WingardiumLeviosa>() != null;
				if (flag2)
				{
					this.item.gameObject.GetComponent<WingardiumLeviosa>().canLift = false;
				}
				bool flag3 = this.currentText != null;
				if (flag3)
				{
					Object.Destroy(this.currentText);
				}
				bool flag4 = this.currIndex == this.spellsList.Count - 1;
				if (flag4)
				{
					this.currIndex = 0;
					this.pressedIndex = 0;
					this.textObject.SpawnAsync(delegate(Item text)
					{
						text.gameObject.GetComponent<TextMesh>().text = this.spellsListWithText[this.currIndex];
						this.currentText = text.gameObject;
						this.SetTextTimer();
					}, null, null, null, true, null);
					bool flag5 = this.previous != null;
					if (flag5)
					{
						this.previous.Despawn();
					}
				}
				else
				{
					this.currIndex++;
					this.pressedIndex = 0;
					this.textObject.SpawnAsync(delegate(Item text)
					{
						text.gameObject.GetComponent<TextMesh>().text = this.spellsListWithText[this.currIndex];
						this.currentText = text.gameObject;
						this.bTimerTime = 1500f;
						this.SetTextTimer();
					}, null, null, null, true, null);
					bool flag6 = this.previous != null;
					if (flag6)
					{
						this.previous.Despawn();
					}
				}
			}
			bool flag7 = action == 2 && this.canFire;
			if (flag7)
			{
				bool flag8 = this.currIndex != 4 && this.currIndex != 5 && this.currIndex != 6 && this.currIndex != 7 && this.currIndex != 8 && this.currIndex != 9 && this.currIndex != 12 && this.currIndex != 13;
				if (flag8)
				{
					this.SetTimer();
					this.distortionEffect.SpawnAsync(delegate(Item distortion)
					{
						this.currentDistortion = distortion;
					}, null, null, null, true, null);
				}
				bool flag9 = this.currIndex != 6 && this.currIndex != 7 && this.currIndex != 8 && this.currIndex != 9 && this.currIndex != 12 && this.currIndex != 13;
				if (flag9)
				{
					this.spellsList[this.currIndex].SpawnAsync(delegate(Item projectile)
					{
						projectile.transform.position = this.item.flyDirRef.position;
						projectile.transform.rotation = this.item.flyDirRef.rotation;
						projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
						projectile.IgnoreObjectCollision(this.item);
						projectile.Throw(1f, 1);
						bool flag14 = this.currIndex == 0;
						if (flag14)
						{
							projectile.gameObject.AddComponent<Stupefy>();
							projectile.rb.useGravity = false;
							projectile.rb.drag = 0f;
							this.currentShooters = projectile;
							projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
							projectile.gameObject.AddComponent<SpellDespawn>();
							foreach (AudioSource c in this.item.GetComponentsInChildren<AudioSource>())
							{
								string name = c.name;
								string text = name;
								if (text == "StupefySound")
								{
									this.sourceCurrent = c;
								}
							}
							this.sourceCurrent.Play();
							this.playSound = true;
						}
						else
						{
							bool flag15 = this.currIndex == 1;
							if (flag15)
							{
								projectile.gameObject.AddComponent<Expelliarmus>();
								projectile.gameObject.GetComponent<Expelliarmus>().power = this.expelliarmusPower;
								projectile.rb.useGravity = false;
								projectile.rb.drag = 0f;
								this.currentShooters = projectile;
								projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
								projectile.gameObject.AddComponent<SpellDespawn>();
								foreach (AudioSource c2 in this.item.GetComponentsInChildren<AudioSource>())
								{
									string name2 = c2.name;
									string text2 = name2;
									if (text2 == "ExpelliarmusSound")
									{
										this.sourceCurrent = c2;
									}
								}
								this.sourceCurrent.Play();
							}
							else
							{
								bool flag16 = this.currIndex == 2;
								if (flag16)
								{
									projectile.gameObject.AddComponent<AvadaKedavra>();
									projectile.rb.useGravity = false;
									projectile.rb.drag = 0f;
									this.currentShooters = projectile;
									projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
									projectile.gameObject.AddComponent<SpellDespawn>();
									foreach (AudioSource c3 in this.item.GetComponentsInChildren<AudioSource>())
									{
										string name3 = c3.name;
										string text3 = name3;
										if (text3 == "AvadaSound")
										{
											this.sourceCurrent = c3;
										}
									}
									this.sourceCurrent.Play();
								}
								else
								{
									bool flag17 = this.currIndex == 3;
									if (flag17)
									{
										projectile.gameObject.AddComponent<PetrificusTotalus>();
										projectile.rb.useGravity = false;
										projectile.rb.drag = 0f;
										this.currentShooters = projectile;
										projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
										projectile.gameObject.AddComponent<SpellDespawn>();
										foreach (AudioSource c4 in this.item.GetComponentsInChildren<AudioSource>())
										{
											string name4 = c4.name;
											string text4 = name4;
											if (text4 == "PetrificusTotallusSound")
											{
												this.sourceCurrent = c4;
											}
										}
										this.sourceCurrent.Play();
									}
									else
									{
										bool flag18 = this.currIndex == 4;
										if (flag18)
										{
											this.pressedIndex++;
											projectile.gameObject.AddComponent<Protego>();
											projectile.rb.useGravity = false;
											projectile.rb.drag = 0f;
											this.current = projectile;
											bool flag19 = this.pressedIndex == 2;
											if (flag19)
											{
												this.previous.Despawn();
												this.previous = null;
												this.pressedIndex = 0;
												this.previous = this.current;
											}
											else
											{
												bool flag20 = this.previous != null;
												if (flag20)
												{
													this.previous.Despawn();
													this.previous = null;
												}
												this.previous = this.current;
											}
											this.sourceCurrent = projectile.gameObject.GetComponent<Protego>().source;
											this.sourceCurrent.Play();
										}
										else
										{
											bool flag21 = this.currIndex == 5;
											if (flag21)
											{
												this.pressedIndex++;
												projectile.rb.useGravity = false;
												projectile.rb.drag = 0f;
												this.current = projectile;
												bool flag22 = this.pressedIndex == 2;
												if (flag22)
												{
													this.previous.Despawn();
													this.previous = null;
													this.pressedIndex = 0;
													this.previous = this.current;
												}
												else
												{
													bool flag23 = this.previous != null;
													if (flag23)
													{
														this.previous.Despawn();
														this.previous = null;
													}
													this.previous = this.current;
												}
												this.sourceCurrent = projectile.gameObject.GetComponent<AudioSource>();
												this.sourceCurrent.Play();
											}
											else
											{
												bool flag24 = this.currIndex == 10;
												if (flag24)
												{
													projectile.rb.useGravity = false;
													projectile.rb.drag = 0f;
													this.currentShooters = projectile;
													projectile.rb.AddForce(this.item.flyDirRef.transform.forward * 100f, 1);
													projectile.gameObject.AddComponent<sempraDespawn>();
												}
												else
												{
													bool flag25 = this.currIndex == 11;
													if (flag25)
													{
														projectile.gameObject.AddComponent<Levicorpus>();
														projectile.rb.useGravity = false;
														projectile.rb.drag = 0f;
														this.currentShooters = projectile;
														projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
														projectile.gameObject.AddComponent<SpellDespawn>();
														foreach (AudioSource c5 in this.item.GetComponentsInChildren<AudioSource>())
														{
															string name5 = c5.name;
															string text5 = name5;
															if (text5 == "LevicorpusSound")
															{
																this.sourceCurrent = c5;
															}
														}
														this.sourceCurrent.Play();
													}
												}
											}
										}
									}
								}
							}
						}
					}, null, null, null, true, null);
				}
				bool flag10 = this.currIndex == 6;
				if (flag10)
				{
					this.item.gameObject.AddComponent<Accio>();
					this.item.gameObject.GetComponent<Accio>().CastRay();
					this.parentLocal = this.item.gameObject.GetComponent<Accio>().parentLocal;
					this.startPoint = this.item.gameObject.GetComponent<Accio>().startPoint;
					this.endPoint = this.item.gameObject.GetComponent<Accio>().endPoint;
					this.duration = 0.6f;
					bool cantAccio = this.item.gameObject.GetComponent<Accio>().cantAccio;
					if (cantAccio)
					{
						this.canAccio = false;
					}
					else
					{
						bool isGrabbed = this.oppositeHand.playerHand.ragdollHand.isGrabbed;
						if (isGrabbed)
						{
							this.canAccio = false;
						}
						else
						{
							this.canAccio = true;
						}
					}
				}
				bool flag11 = this.currIndex == 7;
				if (flag11)
				{
					this.item.gameObject.AddComponent<Engorgio>();
					this.item.gameObject.GetComponent<Engorgio>().CastRay();
				}
				bool flag12 = this.currIndex == 8;
				if (flag12)
				{
					this.item.gameObject.AddComponent<Evanesco>();
					this.item.gameObject.GetComponent<Evanesco>().CastRay();
					this.parentLocal = this.item.gameObject.GetComponent<Evanesco>().parentLocal;
					this.item.gameObject.GetComponent<Evanesco>().evanescoDissolve = this.evanescoDissolve;
				}
				bool flag13 = this.currIndex == 9;
				if (flag13)
				{
					this.item.gameObject.AddComponent<Geminio>();
					this.item.gameObject.GetComponent<Geminio>().CastRay();
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005558 File Offset: 0x00003758
		private void SetTimer()
		{
			this.aTimer = new Timer(200.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000055AC File Offset: 0x000037AC
		private void SetTextTimer()
		{
			this.bTimer = new Timer((double)this.bTimerTime);
			this.bTimer.Elapsed += this.OnTextTimedEvent;
			this.bTimer.AutoReset = false;
			this.bTimer.Enabled = true;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000055FE File Offset: 0x000037FE
		private void OnTextTimedEvent(object sender, ElapsedEventArgs e)
		{
			Object.Destroy(this.currentText);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005610 File Offset: 0x00003810
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			bool flag = this.currentDistortion != null;
			if (flag)
			{
				this.currentDistortion.Despawn();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000563C File Offset: 0x0000383C
		public void Setup(float importSpeed, bool importMagicEffect, float importExpelliarmusPower)
		{
			this.spellSpeed = importSpeed;
			this.magicEffect = importMagicEffect;
			this.expelliarmusPower = importExpelliarmusPower;
		}

		// Token: 0x04000076 RID: 118
		private float bTimerTime = 1500f;

		// Token: 0x04000077 RID: 119
		private List<Material> myMaterials;

		// Token: 0x04000078 RID: 120
		private Stack<GameObject> stack = new Stack<GameObject>();

		// Token: 0x04000079 RID: 121
		private Item item;

		// Token: 0x0400007A RID: 122
		private Item currentDistortion;

		// Token: 0x0400007B RID: 123
		private Item current;

		// Token: 0x0400007C RID: 124
		private Item previous;

		// Token: 0x0400007D RID: 125
		private Item currentShooters;

		// Token: 0x0400007E RID: 126
		private Item currentGoldenLight;

		// Token: 0x0400007F RID: 127
		private Item currentAvadaLightning;

		// Token: 0x04000080 RID: 128
		private Material evanescoDissolve;

		// Token: 0x04000081 RID: 129
		private GameObject keyWordRecog;

		// Token: 0x04000082 RID: 130
		private ItemData avadaLightning;

		// Token: 0x04000083 RID: 131
		public float spellSpeed;

		// Token: 0x04000084 RID: 132
		public bool magicEffect;

		// Token: 0x04000085 RID: 133
		private float explliarmusPower;

		// Token: 0x04000086 RID: 134
		private int currIndex;

		// Token: 0x04000087 RID: 135
		private Dictionary<string, ItemData> spells = new Dictionary<string, ItemData>();

		// Token: 0x04000088 RID: 136
		private KeyWordRecogWand speech;

		// Token: 0x04000089 RID: 137
		private bool waitFlag;

		// Token: 0x0400008A RID: 138
		private bool determiner;

		// Token: 0x0400008B RID: 139
		internal List<ItemData> spellsList = new List<ItemData>();

		// Token: 0x0400008C RID: 140
		private List<string> spellsListWithText = new List<string>();

		// Token: 0x0400008D RID: 141
		private int pressedIndex;

		// Token: 0x0400008E RID: 142
		private AudioSource sourceCurrent;

		// Token: 0x0400008F RID: 143
		private AudioSource touch;

		// Token: 0x04000090 RID: 144
		private GameObject sound;

		// Token: 0x04000091 RID: 145
		private int firstTouch;

		// Token: 0x04000092 RID: 146
		internal bool canSpeak;

		// Token: 0x04000093 RID: 147
		private bool canFire;

		// Token: 0x04000094 RID: 148
		private bool ignoreTouch;

		// Token: 0x04000095 RID: 149
		private bool playSound;

		// Token: 0x04000096 RID: 150
		private GameObject sourceObj;

		// Token: 0x04000097 RID: 151
		private bool canRayCast;

		// Token: 0x04000098 RID: 152
		private bool canAccio;

		// Token: 0x04000099 RID: 153
		private bool canEngorgio;

		// Token: 0x0400009A RID: 154
		private bool canEvanesco;

		// Token: 0x0400009B RID: 155
		private float elapsedTime;

		// Token: 0x0400009C RID: 156
		private float duration;

		// Token: 0x0400009D RID: 157
		private Vector3 startPoint;

		// Token: 0x0400009E RID: 158
		private Vector3 endPoint;

		// Token: 0x0400009F RID: 159
		private GameObject parentLocal;

		// Token: 0x040000A0 RID: 160
		private RagdollHand oppositeHand;

		// Token: 0x040000A1 RID: 161
		private Timer aTimer;

		// Token: 0x040000A2 RID: 162
		private ItemData distortionEffect;

		// Token: 0x040000A3 RID: 163
		private Creature currentHit;

		// Token: 0x040000A4 RID: 164
		private Vector3 engorgioMaxSize;

		// Token: 0x040000A5 RID: 165
		private Item dissovledItem;

		// Token: 0x040000A6 RID: 166
		private List<GameObject> evanescoStore = new List<GameObject>();

		// Token: 0x040000A7 RID: 167
		private float dissolveVal;

		// Token: 0x040000A8 RID: 168
		private ItemData textObject;

		// Token: 0x040000A9 RID: 169
		private Timer bTimer;

		// Token: 0x040000AA RID: 170
		private GameObject currentText;

		// Token: 0x040000AB RID: 171
		private bool objectIsHovering;

		// Token: 0x040000AC RID: 172
		private float expelliarmusPower;
	}
}

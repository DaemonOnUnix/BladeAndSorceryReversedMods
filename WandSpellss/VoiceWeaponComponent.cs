using System;
using System.Collections.Generic;
using System.Timers;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace WandSpellss
{
	// Token: 0x0200001F RID: 31
	internal class VoiceWeaponComponent : MonoBehaviour
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00005F10 File Offset: 0x00004110
		private void Start()
		{
			this.playerSoul = Player.local.creature.gameObject.GetComponent<Soul>();
			this.canMakeHorcrux = false;
			this.item = base.GetComponent<Item>();
			this.item.gameObject.AddComponent<KeyWordRecogWand>();
			this.recognizer = this.item.gameObject;
			this.recogWand = this.recognizer.GetComponent<KeyWordRecogWand>();
			this.spellsList.Add(Catalog.GetData<ItemData>("StupefyObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("ExpelliarmusObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("AvadaKedavraObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("PetrificusTotalusObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("LevicorpusObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("LumosObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("SectumsempraObject", true));
			this.spellsList.Add(Catalog.GetData<ItemData>("MorsmordreObject", true));
			this.DarkMark = Catalog.GetData<ItemData>("TheDarkMark", true);
			Addressables.LoadAssetAsync<Material>("apoz123Wand.SpellEffect.Evanesco.Mat").Completed += this.Op_Completed1;
			Player.local.locomotion.OnGroundEvent += new Locomotion.GroundEvent(this.Locomotion_OnGroundEvent);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000609C File Offset: 0x0000429C
		private void Op_Completed1(AsyncOperationHandle<Material> obj)
		{
			bool flag = obj.Status == 1;
			if (flag)
			{
				this.evanescoDissolve = obj.Result;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000060C8 File Offset: 0x000042C8
		private void Update()
		{
			bool flag = this.recogWand != null;
			if (flag)
			{
				bool hasRecognizedWord = this.recogWand.hasRecognizedWord;
				if (hasRecognizedWord)
				{
					this.recogWand.hasRecognizedWord = false;
					bool flag2 = this.recogWand.knownCurrent != null;
					if (flag2)
					{
						Debug.Log(this.recogWand.knownCurrent);
						bool flag3 = this.recogWand.knownCurrent.Contains("Stewpify");
						if (flag3)
						{
							Debug.Log("Got past comparing words");
							this.spellsList[0].SpawnAsync(delegate(Item projectile)
							{
								projectile.transform.position = this.item.flyDirRef.position;
								projectile.transform.rotation = this.item.flyDirRef.rotation;
								projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
								projectile.IgnoreObjectCollision(this.item);
								projectile.Throw(1f, 1);
								projectile.gameObject.AddComponent<Stupefy>();
								projectile.rb.useGravity = false;
								projectile.rb.drag = 0f;
								this.currentShooters = projectile;
								projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
								projectile.gameObject.AddComponent<SpellDespawn>();
								foreach (AudioSource c2 in this.item.GetComponentsInChildren<AudioSource>())
								{
									string name2 = c2.name;
									string text3 = name2;
									if (text3 == "StupefySound")
									{
										this.sourceCurrent = c2;
									}
								}
								this.sourceCurrent.Play();
								this.playSound = true;
							}, null, null, null, true, null);
						}
						else
						{
							bool flag4 = this.recogWand.knownCurrent.Contains("Expelliarmus");
							if (flag4)
							{
								Debug.Log("Got past comparing words");
								this.spellsList[1].SpawnAsync(delegate(Item projectile)
								{
									projectile.transform.position = this.item.flyDirRef.position;
									projectile.transform.rotation = this.item.flyDirRef.rotation;
									projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
									projectile.IgnoreObjectCollision(this.item);
									projectile.Throw(1f, 1);
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
										string text3 = name2;
										if (text3 == "ExpelliarmusSound")
										{
											this.sourceCurrent = c2;
										}
									}
									this.sourceCurrent.Play();
								}, null, null, null, true, null);
							}
							else
							{
								bool flag5 = this.recogWand.knownCurrent.Contains("Ahvahduhkuhdahvra");
								if (flag5)
								{
									Debug.Log("Got past comparing words");
									this.spellsList[2].SpawnAsync(delegate(Item projectile)
									{
										projectile.transform.position = this.item.flyDirRef.position;
										projectile.transform.rotation = this.item.flyDirRef.rotation;
										projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
										projectile.IgnoreObjectCollision(this.item);
										projectile.Throw(1f, 1);
										projectile.gameObject.AddComponent<AvadaKedavra>();
										projectile.rb.useGravity = false;
										projectile.rb.drag = 0f;
										this.currentShooters = projectile;
										projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
										projectile.gameObject.AddComponent<SpellDespawn>();
										foreach (AudioSource c2 in this.item.GetComponentsInChildren<AudioSource>())
										{
											string name2 = c2.name;
											string text3 = name2;
											if (text3 == "AvadaSound")
											{
												this.sourceCurrent = c2;
											}
										}
										this.sourceCurrent.Play();
									}, null, null, null, true, null);
									this.canMakeHorcrux = true;
									this.SetTimer();
								}
								else
								{
									bool flag6 = this.recogWand.knownCurrent.Contains("PetrificusTotalus");
									if (flag6)
									{
										Debug.Log("Got past comparing words");
										this.spellsList[3].SpawnAsync(delegate(Item projectile)
										{
											projectile.transform.position = this.item.flyDirRef.position;
											projectile.transform.rotation = this.item.flyDirRef.rotation;
											projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
											projectile.IgnoreObjectCollision(this.item);
											projectile.Throw(1f, 1);
											projectile.gameObject.AddComponent<PetrificusTotalus>();
											projectile.rb.useGravity = false;
											projectile.rb.drag = 0f;
											this.currentShooters = projectile;
											projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
											projectile.gameObject.AddComponent<SpellDespawn>();
											foreach (AudioSource c2 in this.item.GetComponentsInChildren<AudioSource>())
											{
												string name2 = c2.name;
												string text3 = name2;
												if (text3 == "PetrificusTotallusSound")
												{
													this.sourceCurrent = c2;
												}
											}
											this.sourceCurrent.Play();
										}, null, null, null, true, null);
									}
									else
									{
										bool flag7 = this.recogWand.knownCurrent.Contains("Levicorpus");
										if (flag7)
										{
											Debug.Log("Got past comparing words");
											this.spellsList[4].SpawnAsync(delegate(Item projectile)
											{
												projectile.transform.position = this.item.flyDirRef.position;
												projectile.transform.rotation = this.item.flyDirRef.rotation;
												projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
												projectile.IgnoreObjectCollision(this.item);
												projectile.Throw(1f, 1);
												projectile.gameObject.AddComponent<Levicorpus>();
												projectile.gameObject.GetComponent<Levicorpus>().spawnerWeapon = this.item;
												projectile.rb.useGravity = false;
												projectile.rb.drag = 0f;
												this.currentShooters = projectile;
												projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
												projectile.gameObject.AddComponent<SpellDespawn>();
												foreach (AudioSource c2 in this.item.GetComponentsInChildren<AudioSource>())
												{
													string name2 = c2.name;
													string text3 = name2;
													if (text3 == "LevicorpusSound")
													{
														this.sourceCurrent = c2;
													}
												}
												this.sourceCurrent.Play();
											}, null, null, null, true, null);
										}
										else
										{
											bool flag8 = this.recogWand.knownCurrent.Contains("Liberacorpus");
											if (flag8)
											{
												Debug.Log("Got past LiberaCorpus check");
												bool flag9 = this.hitByLevicorpus.Count > 0;
												if (flag9)
												{
													foreach (Creature creature in this.hitByLevicorpus)
													{
														Debug.Log(creature);
														bool flag10 = creature.footLeft.gameObject.GetComponent<SpringJoint>() != null && creature.footRight.gameObject.GetComponent<SpringJoint>() != null;
														if (flag10)
														{
															Debug.Log("Got past double foot check.");
															Object.Destroy(creature.footLeft.gameObject.GetComponent<SpringJoint>());
															Object.Destroy(creature.footRight.gameObject.GetComponent<SpringJoint>());
														}
													}
													this.hitByLevicorpus.Clear();
													this.hitByLevicorpus.TrimExcess();
												}
											}
											else
											{
												bool flag11 = this.recogWand.knownCurrent.Contains("Lumos");
												if (flag11)
												{
													bool flag12 = this.current != null;
													if (flag12)
													{
														bool flag13 = !this.current.name.Contains("Lumos");
														if (flag13)
														{
															this.spellsList[5].SpawnAsync(delegate(Item projectile)
															{
																this.current = projectile;
																this.sourceCurrent = projectile.gameObject.GetComponent<AudioSource>();
																this.sourceCurrent.Play();
															}, null, null, null, true, null);
														}
													}
													else
													{
														this.spellsList[5].SpawnAsync(delegate(Item projectile)
														{
															this.current = projectile;
															this.sourceCurrent = projectile.gameObject.GetComponent<AudioSource>();
															this.sourceCurrent.Play();
														}, null, null, null, true, null);
													}
												}
												else
												{
													bool flag14 = this.recogWand.knownCurrent.Contains("Nox");
													if (flag14)
													{
														bool flag15 = this.current != null;
														if (flag15)
														{
															bool flag16 = this.current.name.Contains("Lumos");
															if (flag16)
															{
																foreach (AudioSource c in this.item.GetComponentsInChildren<AudioSource>())
																{
																	string name = c.name;
																	string text = name;
																	if (text == "NoxSound")
																	{
																		this.sourceCurrent = c;
																	}
																}
																this.sourceCurrent.Play();
																this.current.Despawn();
																this.current = null;
															}
														}
													}
													else
													{
														bool flag17 = this.recogWand.knownCurrent.Contains("Protego");
														if (flag17)
														{
															this.spellsList[6].SpawnAsync(delegate(Item projectile)
															{
																projectile.rb.useGravity = false;
																projectile.rb.drag = 0f;
																this.current = projectile;
																foreach (AudioSource c2 in this.item.GetComponentsInChildren<AudioSource>())
																{
																	string name2 = c2.name;
																	string text3 = name2;
																	if (text3 == "ProtegoSound")
																	{
																		this.sourceCurrent = c2;
																	}
																}
																this.sourceCurrent.Play();
															}, null, null, null, true, null);
														}
														else
														{
															bool flag18 = this.recogWand.knownCurrent.Contains("Evanesco");
															if (flag18)
															{
																this.item.gameObject.AddComponent<Evanesco>();
																this.item.gameObject.GetComponent<Evanesco>().evanescoDissolve = this.evanescoDissolve;
																this.item.gameObject.GetComponent<Evanesco>().CastRay();
															}
															else
															{
																bool flag19 = this.recogWand.knownCurrent.Contains("Ascendio");
																if (flag19)
																{
																	this.item.gameObject.AddComponent<Ascendio>();
																	this.item.gameObject.GetComponent<Ascendio>().Ascend();
																	this.usedAscendio = true;
																}
																else
																{
																	bool flag20 = this.recogWand.knownCurrent.Contains("Sectumsempra");
																	if (flag20)
																	{
																		this.spellsList[7].SpawnAsync(delegate(Item projectile)
																		{
																			projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
																			projectile.IgnoreObjectCollision(this.item);
																			projectile.rb.useGravity = false;
																			Debug.Log(projectile);
																			projectile.rb.drag = 0f;
																			this.currentShooters = projectile;
																			projectile.rb.AddForce(this.item.flyDirRef.transform.forward * 100f, 1);
																			projectile.gameObject.AddComponent<sempraDespawn>();
																			foreach (AudioSource c2 in this.item.GetComponentsInChildren<AudioSource>())
																			{
																				string name2 = c2.name;
																				string text3 = name2;
																				if (text3 == "SectumsempraSound")
																				{
																					this.sourceCurrent = c2;
																				}
																			}
																			this.sourceCurrent.Play();
																		}, null, null, null, true, null);
																	}
																	else
																	{
																		bool flag21 = this.recogWand.knownCurrent.Contains("Vincere mortem");
																		if (flag21)
																		{
																			bool flag22 = this.item.mainHandler.otherHand.grabbedHandle.item.gameObject.GetComponent<Horcrux>() == null && this.canMakeHorcrux;
																			if (flag22)
																			{
																				bool flag23 = this.item.mainHandler.otherHand != null;
																				if (flag23)
																				{
																					this.item.mainHandler.otherHand.grabbedHandle.item.gameObject.AddComponent<Horcrux>();
																					this.horcruxes.Add(this.item.mainHandler.otherHand.grabbedHandle.item.gameObject.GetComponent<Horcrux>());
																					this.canMakeHorcrux = false;
																				}
																			}
																		}
																		else
																		{
																			bool flag24 = this.recogWand.knownCurrent.Contains("Engorgio");
																			if (flag24)
																			{
																				this.item.gameObject.AddComponent<Engorgio>();
																				this.item.gameObject.GetComponent<Engorgio>().CastRay();
																			}
																			else
																			{
																				bool flag25 = this.recogWand.knownCurrent.Contains("Morsmordre");
																				if (flag25)
																				{
																					this.spellsList[8].SpawnAsync(delegate(Item projectile)
																					{
																						projectile.transform.position = this.item.flyDirRef.position;
																						projectile.transform.rotation = this.item.flyDirRef.rotation;
																						projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
																						projectile.IgnoreObjectCollision(this.item);
																						projectile.Throw(1f, 1);
																						projectile.gameObject.AddComponent<Morsmordre>();
																						projectile.gameObject.GetComponent<Morsmordre>().darkMark = this.DarkMark;
																						projectile.rb.useGravity = false;
																						projectile.rb.drag = 0f;
																						this.currentShooters = projectile;
																						projectile.rb.AddForce(this.item.flyDirRef.transform.forward * this.spellSpeed, 1);
																						projectile.gameObject.AddComponent<SpellDespawn>();
																					}, null, null, null, true, null);
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				foreach (Horcrux horcrux in this.horcruxes)
				{
					string text2 = "Horcrux: ";
					Horcrux horcrux2 = horcrux;
					Debug.Log(text2 + ((horcrux2 != null) ? horcrux2.ToString() : null));
				}
				bool flag26 = this.current != null;
				if (flag26)
				{
					bool flag27 = this.current.name.Contains("Lumos");
					if (flag27)
					{
						this.current.transform.position = this.item.flyDirRef.transform.position;
						this.current.transform.rotation = this.item.flyDirRef.transform.rotation;
					}
					else
					{
						bool flag28 = this.current.name.Contains("Quad");
						if (flag28)
						{
							this.current.transform.position = this.item.flyDirRef.transform.position;
							this.current.transform.rotation = this.item.flyDirRef.transform.rotation;
							bool flag29 = this.sourceCurrent != null;
							if (flag29)
							{
								bool flag30 = !this.sourceCurrent.isPlaying;
								if (flag30)
								{
									this.current.Despawn();
									this.current = null;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00006A44 File Offset: 0x00004C44
		private void Locomotion_OnGroundEvent(Vector3 groundPoint, Vector3 velocity, Collider groundCollider)
		{
			bool flag = this.usedAscendio;
			if (flag)
			{
				this.usedAscendio = false;
				Player.fallDamage = true;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00006A6B File Offset: 0x00004C6B
		public void Setup(float importSpeed, bool importMagicEffect, float importExpelliarmusPower)
		{
			this.spellSpeed = importSpeed;
			this.magicEffect = importMagicEffect;
			this.expelliarmusPower = importExpelliarmusPower;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00006A84 File Offset: 0x00004C84
		private void SetTimer()
		{
			this.aTimer = new Timer(30000.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00006AD8 File Offset: 0x00004CD8
		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			this.canMakeHorcrux = false;
		}

		// Token: 0x040000AD RID: 173
		private float bTimerTime = 1500f;

		// Token: 0x040000AE RID: 174
		private List<Material> myMaterials;

		// Token: 0x040000AF RID: 175
		private Stack<GameObject> stack = new Stack<GameObject>();

		// Token: 0x040000B0 RID: 176
		private Item item;

		// Token: 0x040000B1 RID: 177
		private Item currentDistortion;

		// Token: 0x040000B2 RID: 178
		private Item current;

		// Token: 0x040000B3 RID: 179
		private Item previous;

		// Token: 0x040000B4 RID: 180
		private Item currentShooters;

		// Token: 0x040000B5 RID: 181
		private Item currentGoldenLight;

		// Token: 0x040000B6 RID: 182
		private Item currentAvadaLightning;

		// Token: 0x040000B7 RID: 183
		private Material evanescoDissolve;

		// Token: 0x040000B8 RID: 184
		private GameObject keyWordRecog;

		// Token: 0x040000B9 RID: 185
		private ItemData avadaLightning;

		// Token: 0x040000BA RID: 186
		public float spellSpeed;

		// Token: 0x040000BB RID: 187
		public bool magicEffect;

		// Token: 0x040000BC RID: 188
		private float expelliarmusPower;

		// Token: 0x040000BD RID: 189
		private int currIndex;

		// Token: 0x040000BE RID: 190
		private Dictionary<string, ItemData> spells = new Dictionary<string, ItemData>();

		// Token: 0x040000BF RID: 191
		private KeyWordRecogWand speech;

		// Token: 0x040000C0 RID: 192
		private bool waitFlag;

		// Token: 0x040000C1 RID: 193
		private bool determiner;

		// Token: 0x040000C2 RID: 194
		internal List<ItemData> spellsList = new List<ItemData>();

		// Token: 0x040000C3 RID: 195
		private List<string> spellsListWithText = new List<string>();

		// Token: 0x040000C4 RID: 196
		private int pressedIndex;

		// Token: 0x040000C5 RID: 197
		private AudioSource sourceCurrent;

		// Token: 0x040000C6 RID: 198
		private AudioSource touch;

		// Token: 0x040000C7 RID: 199
		private GameObject sound;

		// Token: 0x040000C8 RID: 200
		private int firstTouch;

		// Token: 0x040000C9 RID: 201
		internal bool canSpeak;

		// Token: 0x040000CA RID: 202
		private bool canFire;

		// Token: 0x040000CB RID: 203
		private bool ignoreTouch;

		// Token: 0x040000CC RID: 204
		private bool playSound;

		// Token: 0x040000CD RID: 205
		private GameObject sourceObj;

		// Token: 0x040000CE RID: 206
		private bool canRayCast;

		// Token: 0x040000CF RID: 207
		private bool canAccio;

		// Token: 0x040000D0 RID: 208
		private bool canEngorgio;

		// Token: 0x040000D1 RID: 209
		private bool canEvanesco;

		// Token: 0x040000D2 RID: 210
		private float elapsedTime;

		// Token: 0x040000D3 RID: 211
		private float duration;

		// Token: 0x040000D4 RID: 212
		private Vector3 startPoint;

		// Token: 0x040000D5 RID: 213
		private Vector3 endPoint;

		// Token: 0x040000D6 RID: 214
		private GameObject parentLocal;

		// Token: 0x040000D7 RID: 215
		private RagdollHand oppositeHand;

		// Token: 0x040000D8 RID: 216
		private Timer aTimer;

		// Token: 0x040000D9 RID: 217
		private ItemData distortionEffect;

		// Token: 0x040000DA RID: 218
		private Creature currentHit;

		// Token: 0x040000DB RID: 219
		private Vector3 engorgioMaxSize;

		// Token: 0x040000DC RID: 220
		private Item dissovledItem;

		// Token: 0x040000DD RID: 221
		private List<GameObject> evanescoStore = new List<GameObject>();

		// Token: 0x040000DE RID: 222
		private float dissolveVal;

		// Token: 0x040000DF RID: 223
		private ItemData textObject;

		// Token: 0x040000E0 RID: 224
		private Timer bTimer;

		// Token: 0x040000E1 RID: 225
		private GameObject currentText;

		// Token: 0x040000E2 RID: 226
		private bool objectIsHovering;

		// Token: 0x040000E3 RID: 227
		private ItemData DarkMark;

		// Token: 0x040000E4 RID: 228
		private GameObject keyWords;

		// Token: 0x040000E5 RID: 229
		internal List<Creature> hitByLevicorpus = new List<Creature>();

		// Token: 0x040000E6 RID: 230
		private GameObject recognizer;

		// Token: 0x040000E7 RID: 231
		private string recognizedWord;

		// Token: 0x040000E8 RID: 232
		private KeyWordRecogWand recogWand;

		// Token: 0x040000E9 RID: 233
		private bool usedAscendio;

		// Token: 0x040000EA RID: 234
		private List<Horcrux> horcruxes = new List<Horcrux>();

		// Token: 0x040000EB RID: 235
		private Soul playerSoul;

		// Token: 0x040000EC RID: 236
		private bool canMakeHorcrux;
	}
}

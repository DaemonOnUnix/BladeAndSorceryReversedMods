using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CustomAvatarFramework.Editor.Items;
using CustomAvatarFramework.Extensions;
using IngameDebugConsole;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomAvatarFramework
{
	// Token: 0x02000007 RID: 7
	public class CustomAvatarInGameEditorLevelModule : LevelModule
	{
		// Token: 0x0600001A RID: 26 RVA: 0x0000275C File Offset: 0x0000095C
		public override IEnumerator OnLoadCoroutine()
		{
			DebugLogConsole.AddCommandInstance("custom_avatar_test_dismemberment", "Custom Avatar Test Dismemberment", "TestDismemberment", this, new string[0]);
			DebugLogConsole.AddCommandInstance("custom_avatar_editor_on", "Custom Avatar Editor On", "CustomAvatarEditorOn", this, new string[0]);
			DebugLogConsole.AddCommandInstance("custom_avatar_editor_off", "Custom Avatar Editor Off", "CustomAvatarEditorOff", this, new string[0]);
			Catalog.LoadAssetAsync<GameObject>("CustomAvatarInGameEditor", delegate(GameObject spawnedCanvas)
			{
				Debug.Log("canvas spawned");
				this._customAvatarInGameEditorGameObject = Object.Instantiate<GameObject>(spawnedCanvas);
				this.ProcessCustomAvatarInGameEditor(this._customAvatarInGameEditorGameObject);
			}, "CustomAvatarInGameEditor");
			return base.OnLoadCoroutine();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027DB File Offset: 0x000009DB
		public void CustomAvatarEditorOn()
		{
			this._customAvatarInGameEditorGameObject.SetActive(true);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027E9 File Offset: 0x000009E9
		public void CustomAvatarEditorOff()
		{
			this._customAvatarInGameEditorGameObject.SetActive(false);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000027F7 File Offset: 0x000009F7
		public void DisplayMessage(string text, float duration = 1f)
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000028FC File Offset: 0x00000AFC
		private void ProcessCustomAvatarInGameEditor(GameObject go)
		{
			this._customAvatarInGameEditor = go.GetComponent<CustomAvatarInGameEditor>();
			this._customAvatarInGameEditor.spawnCreatureButton.onClick.AddListener(new UnityAction(this.HandleSpawnCreatureButtonClick));
			this._customAvatarInGameEditor.spawnMannequinButton.onClick.AddListener(new UnityAction(this.HandleSpawnMannequinButtonClick));
			this._customAvatarInGameEditor.extraRotationXInputField.onValueChanged.AddListener(delegate(string arg0)
			{
				this.HandleExtraRotationInputChange(arg0, "x");
			});
			this._customAvatarInGameEditor.extraRotationYInputField.onValueChanged.AddListener(delegate(string arg0)
			{
				this.HandleExtraRotationInputChange(arg0, "y");
			});
			this._customAvatarInGameEditor.extraRotationZInputField.onValueChanged.AddListener(delegate(string arg0)
			{
				this.HandleExtraRotationInputChange(arg0, "z");
			});
			this._customAvatarInGameEditor.extraRotationXSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleExtraRotationSliderChange(arg0, "x");
			});
			this._customAvatarInGameEditor.extraRotationYSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleExtraRotationSliderChange(arg0, "y");
			});
			this._customAvatarInGameEditor.extraRotationZSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleExtraRotationSliderChange(arg0, "z");
			});
			this._customAvatarInGameEditor.bonesSelector.options.Clear();
			foreach (KeyValuePair<string, HumanBodyBones?> keyValuePair in CustomAvatar.boneMappers)
			{
				this._customAvatarInGameEditor.bonesSelector.options.Add(new Dropdown.OptionData
				{
					image = null,
					text = keyValuePair.Key
				});
			}
			this._customAvatarInGameEditor.bonesSelector.onValueChanged.AddListener(delegate(int arg0)
			{
				this._currentMannequinBoneName = this._customAvatarInGameEditor.bonesSelector.options[arg0].text;
				this.RefreshInputs();
			});
			this._customAvatarInGameEditor.copyBonesButton.onClick.AddListener(new UnityAction(this.HandleCopyBonesButtonClick));
			this._customAvatarInGameEditor.gripsSelector.options.Clear();
			foreach (KeyValuePair<string, HumanBodyBones?> keyValuePair2 in CustomAvatar.boneMappers)
			{
				if (keyValuePair2.Key.Contains("Hand"))
				{
					this._customAvatarInGameEditor.gripsSelector.options.Add(new Dropdown.OptionData
					{
						image = null,
						text = keyValuePair2.Key
					});
				}
			}
			this._customAvatarInGameEditor.gripsSelector.onValueChanged.AddListener(delegate(int arg0)
			{
				if (!this._mannequinCreature)
				{
					return;
				}
				this._currentMannequinGripBoneName = this._customAvatarInGameEditor.gripsSelector.options[arg0].text;
				this.RefreshExtraHipPositionInputs();
			});
			this._customAvatarInGameEditor.extraHipPositionXSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleHipCalibrationSliderChange(arg0, "x");
			});
			this._customAvatarInGameEditor.extraHipPositionYSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleHipCalibrationSliderChange(arg0, "y");
			});
			this._customAvatarInGameEditor.testGripButton.onClick.AddListener(new UnityAction(this.HandleTestGripButtonClick));
			this._customAvatarInGameEditor.copyExtraHipPositionButton.onClick.AddListener(new UnityAction(this.HandleCopyExtraHipPositionButtonClick));
			this._customAvatarInGameEditor.showHideCreatureButton.onClick.AddListener(new UnityAction(this.HandleShowHideCreatureButtonClick));
			this._customAvatarInGameEditor.avatarHeightSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleAvatarHeightSliderChange(arg0);
			});
			this._customAvatarInGameEditor.copyAvatarHeightButton.onClick.AddListener(new UnityAction(this.HandleCopyAvatarHeightButtonClick));
			this._customAvatarInGameEditorGameObject.SetActive(false);
			this._customAvatarInGameEditor.extraDimensionXSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleExtraDimensionSliderChange(arg0, "x");
			});
			this._customAvatarInGameEditor.extraDimensionYSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleExtraDimensionSliderChange(arg0, "y");
			});
			this._customAvatarInGameEditor.extraDimensionZSlider.onValueChanged.AddListener(delegate(float arg0)
			{
				this.HandleExtraDimensionSliderChange(arg0, "z");
			});
			this._customAvatarInGameEditor.copyExtraDimensionButton.onClick.AddListener(new UnityAction(this.HandleCopyExtraDimensionButtonClick));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002D10 File Offset: 0x00000F10
		private void HandleCopyAvatarHeightButtonClick()
		{
			DebugLogEntry debugLogEntry = new DebugLogEntry
			{
				logString = this._mannequinCreature.CustomAvatarCreatureV2().avatarHeight.ToString(CultureInfo.InvariantCulture)
			};
			DebugLogItem debugLogItem = new DebugLogItem();
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			debugLogItem.GetType().GetField("logEntry", bindingFlags).SetValue(debugLogItem, debugLogEntry);
			debugLogItem.CopyLog();
			this.DisplayMessage("avatar height copied to clipboard", 1f);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002D7C File Offset: 0x00000F7C
		private void HandleAvatarHeightSliderChange(float height)
		{
			if (!this._mannequinCreature)
			{
				return;
			}
			this._mannequinCreature.CustomAvatarCreatureV2().avatarHeight = height;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002D9D File Offset: 0x00000F9D
		private void HandleShowHideCreatureButtonClick()
		{
			if (this._mannequinCreature.IsTrulyVisible())
			{
				this._mannequinCreature.ChangeVisibility(false);
				return;
			}
			this._mannequinCreature.ChangeVisibility(true);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002DC8 File Offset: 0x00000FC8
		private void HandleCopyExtraHipPositionButtonClick()
		{
			string text = JsonConvert.SerializeObject(this._mannequinCreature.GetExtraHipPosition(), 1);
			DebugLogEntry debugLogEntry = new DebugLogEntry
			{
				logString = text
			};
			DebugLogItem debugLogItem = new DebugLogItem();
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			debugLogItem.GetType().GetField("logEntry", bindingFlags).SetValue(debugLogItem, debugLogEntry);
			debugLogItem.CopyLog();
			this.DisplayMessage("grips copied to clipboard", 1f);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002E38 File Offset: 0x00001038
		private void HandleCopyExtraDimensionButtonClick()
		{
			string text = JsonConvert.SerializeObject(this._mannequinCreature.GetExtraDimension(), 1);
			DebugLogEntry debugLogEntry = new DebugLogEntry
			{
				logString = text
			};
			DebugLogItem debugLogItem = new DebugLogItem();
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			debugLogItem.GetType().GetField("logEntry", bindingFlags).SetValue(debugLogItem, debugLogEntry);
			debugLogItem.CopyLog();
			this.DisplayMessage("extra dimension copied to clipboard", 1f);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002EA5 File Offset: 0x000010A5
		private void HandleTestGripButtonClick()
		{
			this._mannequinCreature.ResetGrip(this._currentMannequinGripBoneName, true);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002EBC File Offset: 0x000010BC
		private void HandleCopyBonesButtonClick()
		{
			string text = JsonConvert.SerializeObject(this._mannequinCreature.CustomAvatarCreatureV2().extraRotationBonesForEditor, 1);
			DebugLogEntry debugLogEntry = new DebugLogEntry
			{
				logString = text
			};
			DebugLogItem debugLogItem = new DebugLogItem();
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			debugLogItem.GetType().GetField("logEntry", bindingFlags).SetValue(debugLogItem, debugLogEntry);
			debugLogItem.CopyLog();
			this.DisplayMessage("bones copied to clipboard", 1f);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002F2C File Offset: 0x0000112C
		private void HandleExtraRotationInputChange(string input, string axis)
		{
			try
			{
				if (this._mannequinCreature)
				{
					if (!(input == "-"))
					{
						float num = float.Parse(input);
						if (axis != null)
						{
							if (!(axis == "x"))
							{
								if (!(axis == "y"))
								{
									if (axis == "z")
									{
										this._customAvatarInGameEditor.extraRotationZSlider.value = num;
										this._mannequinCreature.SetExtraRotationBone(this._currentMannequinBoneName, this._customAvatarInGameEditor.extraRotationXSlider.value, this._customAvatarInGameEditor.extraRotationYSlider.value, num);
										this._mannequinCreature.SetExtraRotationBoneForEditor(this._currentMannequinBoneName, this._customAvatarInGameEditor.extraRotationXSlider.value, this._customAvatarInGameEditor.extraRotationYSlider.value, num);
									}
								}
								else
								{
									this._customAvatarInGameEditor.extraRotationYSlider.value = num;
									this._mannequinCreature.SetExtraRotationBone(this._currentMannequinBoneName, this._customAvatarInGameEditor.extraRotationXSlider.value, num, this._customAvatarInGameEditor.extraRotationZSlider.value);
									this._mannequinCreature.SetExtraRotationBoneForEditor(this._currentMannequinBoneName, this._customAvatarInGameEditor.extraRotationXSlider.value, num, this._customAvatarInGameEditor.extraRotationZSlider.value);
								}
							}
							else
							{
								this._customAvatarInGameEditor.extraRotationXSlider.value = num;
								this._mannequinCreature.SetExtraRotationBone(this._currentMannequinBoneName, num, this._customAvatarInGameEditor.extraRotationYSlider.value, this._customAvatarInGameEditor.extraRotationZSlider.value);
								this._mannequinCreature.SetExtraRotationBoneForEditor(this._currentMannequinBoneName, num, this._customAvatarInGameEditor.extraRotationYSlider.value, this._customAvatarInGameEditor.extraRotationZSlider.value);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003128 File Offset: 0x00001328
		private void HandleHipCalibrationSliderChange(float value, string axis)
		{
			if (!this._mannequinCreature)
			{
				return;
			}
			this._mannequinCreature.SetExtraHipPosition(value, axis);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003145 File Offset: 0x00001345
		private void HandleExtraDimensionSliderChange(float value, string axis)
		{
			if (!this._mannequinCreature)
			{
				return;
			}
			this._mannequinCreature.SetExtraDimension(value, axis);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003164 File Offset: 0x00001364
		private void HandleExtraRotationSliderChange(float extraRotation, string axis)
		{
			if (!this._mannequinCreature)
			{
				return;
			}
			if (axis != null)
			{
				if (axis == "x")
				{
					this._customAvatarInGameEditor.extraRotationXInputField.text = extraRotation.ToString(CultureInfo.InvariantCulture);
					this._mannequinCreature.SetExtraRotationBone(this._currentMannequinBoneName, extraRotation, this._customAvatarInGameEditor.extraRotationYSlider.value, this._customAvatarInGameEditor.extraRotationZSlider.value);
					this._mannequinCreature.SetExtraRotationBoneForEditor(this._currentMannequinBoneName, extraRotation, this._customAvatarInGameEditor.extraRotationYSlider.value, this._customAvatarInGameEditor.extraRotationZSlider.value);
					return;
				}
				if (axis == "y")
				{
					this._customAvatarInGameEditor.extraRotationYInputField.text = extraRotation.ToString(CultureInfo.InvariantCulture);
					this._mannequinCreature.SetExtraRotationBone(this._currentMannequinBoneName, this._customAvatarInGameEditor.extraRotationXSlider.value, extraRotation, this._customAvatarInGameEditor.extraRotationZSlider.value);
					this._mannequinCreature.SetExtraRotationBoneForEditor(this._currentMannequinBoneName, this._customAvatarInGameEditor.extraRotationXSlider.value, extraRotation, this._customAvatarInGameEditor.extraRotationZSlider.value);
					return;
				}
				if (!(axis == "z"))
				{
					return;
				}
				this._customAvatarInGameEditor.extraRotationZInputField.text = extraRotation.ToString(CultureInfo.InvariantCulture);
				this._mannequinCreature.SetExtraRotationBone(this._currentMannequinBoneName, this._customAvatarInGameEditor.extraRotationXSlider.value, this._customAvatarInGameEditor.extraRotationYSlider.value, extraRotation);
				this._mannequinCreature.SetExtraRotationBoneForEditor(this._currentMannequinBoneName, this._customAvatarInGameEditor.extraRotationXSlider.value, this._customAvatarInGameEditor.extraRotationYSlider.value, extraRotation);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003338 File Offset: 0x00001538
		private void HandleSpawnCreatureButtonClick()
		{
			string text = this._customAvatarInGameEditor.creatureIdText.text;
			this.SpawnCreature(text);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003360 File Offset: 0x00001560
		private void HandleSpawnMannequinButtonClick()
		{
			string text = this._customAvatarInGameEditor.creatureIdText.text;
			this.SpawnMannequin(text);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003390 File Offset: 0x00001590
		private void SpawnCreature(string creatureId)
		{
			GameManager.local.StartCoroutine(Catalog.GetData<CreatureData>(creatureId, true).SpawnCoroutine(Player.local.transform.position + Player.local.transform.forward, 0f, null, delegate(Creature rsCreature)
			{
				this._mannequinCreature = rsCreature;
			}, true, null));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003418 File Offset: 0x00001618
		private void SpawnMannequin(string creatureId)
		{
			if (this._mannequinCreature != null)
			{
				this._mannequinCreature.Despawn();
			}
			GameManager.local.StartCoroutine(Catalog.GetData<CreatureData>(creatureId, true).SpawnCoroutine(Player.local.transform.position + Player.local.transform.forward, 0f, null, delegate(Creature rsCreature)
			{
				rsCreature.ToogleTPose();
				rsCreature.brain.Stop();
				this._mannequinCreature = rsCreature;
				GameManager.local.StartCoroutine(this.RefreshInputsCoroutine());
			}, true, null));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000348C File Offset: 0x0000168C
		private void RefreshInputs()
		{
			Vector3 extraRotationBoneForEditor = this._mannequinCreature.GetExtraRotationBoneForEditor(this._currentMannequinBoneName);
			this._customAvatarInGameEditor.extraRotationXSlider.value = extraRotationBoneForEditor.x;
			this._customAvatarInGameEditor.extraRotationYSlider.value = extraRotationBoneForEditor.y;
			this._customAvatarInGameEditor.extraRotationZSlider.value = extraRotationBoneForEditor.z;
			this._customAvatarInGameEditor.extraRotationXInputField.text = extraRotationBoneForEditor.x.ToString(CultureInfo.InvariantCulture);
			this._customAvatarInGameEditor.extraRotationYInputField.text = extraRotationBoneForEditor.y.ToString(CultureInfo.InvariantCulture);
			this._customAvatarInGameEditor.extraRotationZInputField.text = extraRotationBoneForEditor.z.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003554 File Offset: 0x00001754
		private void RefreshExtraHipPositionInputs()
		{
			Vector3 extraHipPosition = this._mannequinCreature.GetExtraHipPosition();
			this._customAvatarInGameEditor.extraHipPositionXSlider.value = extraHipPosition.x;
			this._customAvatarInGameEditor.extraHipPositionYSlider.value = extraHipPosition.y;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000359C File Offset: 0x0000179C
		private void RefreshExtraDimensionInputs()
		{
			Vector3 extraDimension = this._mannequinCreature.GetExtraDimension();
			this._customAvatarInGameEditor.extraDimensionXSlider.value = extraDimension.x;
			this._customAvatarInGameEditor.extraDimensionYSlider.value = extraDimension.y;
			this._customAvatarInGameEditor.extraDimensionZSlider.value = extraDimension.z;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000036BC File Offset: 0x000018BC
		private IEnumerator RefreshInputsCoroutine()
		{
			yield return new WaitForSeconds(4f);
			this.RefreshInputs();
			this.RefreshExtraHipPositionInputs();
			this.RefreshExtraDimensionInputs();
			this._customAvatarInGameEditor.avatarHeightSlider.value = this._mannequinCreature.CustomAvatarCreatureV2().avatarHeight;
			yield break;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000036F0 File Offset: 0x000018F0
		private void TestDismemberment()
		{
			List<RagdollPart> list = this._mannequinCreature.ragdoll.parts.Where((RagdollPart part) => part.sliceAllowed && !part.isSliced).ToList<RagdollPart>();
			RagdollPart ragdollPart = list[Random.Range(0, list.Count - 1)];
			ragdollPart.TrySlice();
			this._mannequinCreature.Kill();
		}

		// Token: 0x0400000B RID: 11
		private Creature _mannequinCreature;

		// Token: 0x0400000C RID: 12
		private CustomAvatarInGameEditor _customAvatarInGameEditor;

		// Token: 0x0400000D RID: 13
		private string _currentMannequinBoneName = "Rig_Mesh";

		// Token: 0x0400000E RID: 14
		private string _currentMannequinGripBoneName = "LeftHand_Mesh";

		// Token: 0x0400000F RID: 15
		private GameObject _customAvatarInGameEditorGameObject;
	}
}

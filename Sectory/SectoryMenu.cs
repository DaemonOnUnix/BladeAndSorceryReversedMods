using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sectory
{
	// Token: 0x02000014 RID: 20
	public class SectoryMenu : MenuModule
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000032C4 File Offset: 0x000014C4
		public override void Init(MenuData menuData, Menu menu)
		{
			base.Init(menuData, menu);
			GameManager.local.StartCoroutine(this.NewMenu(menu));
			bool flag = SectoryMenu.injectedClass == null;
			if (flag)
			{
				Addressables.LoadAssetAsync<GameObject>("Sectory.InjectedClass").Task.ContinueWith<GameObject>((Task<GameObject> go) => SectoryMenu.injectedClass = go.Result);
			}
			bool flag2 = SectoryMenu.numberVar == null;
			if (flag2)
			{
				Addressables.LoadAssetAsync<GameObject>("Sectory.NumberVar").Task.ContinueWith<GameObject>((Task<GameObject> go) => SectoryMenu.numberVar = go.Result);
			}
			bool flag3 = SectoryMenu.boolVar == null;
			if (flag3)
			{
				Addressables.LoadAssetAsync<GameObject>("Sectory.BoolVar").Task.ContinueWith<GameObject>((Task<GameObject> go) => SectoryMenu.boolVar = go.Result);
			}
			bool flag4 = SectoryMenu.action == null;
			if (flag4)
			{
				Addressables.LoadAssetAsync<GameObject>("Sectory.Action").Task.ContinueWith<GameObject>((Task<GameObject> go) => SectoryMenu.action = go.Result);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003407 File Offset: 0x00001607
		private IEnumerator NewMenu(Menu menu)
		{
			yield return new WaitForSeconds(3f);
			SectoryMenu.mostRecentMenu = menu;
			SectoryMenu.content1 = menu.GetCustomReference("Page1Content", true);
			SectoryMenu.content2 = menu.GetCustomReference("Page2Content", true);
			SectoryMenu.title = menu.GetCustomReference("Title", true);
			using (List<SectoryMenu.InjectEditClass>.Enumerator enumerator = SectoryMenu.injectedClasses.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SectoryMenu.<>c__DisplayClass10_0 CS$<>8__locals1 = new SectoryMenu.<>c__DisplayClass10_0();
					CS$<>8__locals1.edit = enumerator.Current;
					GameObject go = Object.Instantiate<GameObject>(SectoryMenu.injectedClass, SectoryMenu.content1);
					go.GetComponentInChildren<Text>().text = CS$<>8__locals1.edit.modName;
					go.GetComponent<Button>().onClick.AddListener(delegate()
					{
						CS$<>8__locals1.edit.InjectVariables();
						SectoryMenu.title.GetComponent<Text>().text = CS$<>8__locals1.edit.modName + "\nVariables";
					});
					go = null;
					CS$<>8__locals1 = null;
				}
			}
			List<SectoryMenu.InjectEditClass>.Enumerator enumerator = default(List<SectoryMenu.InjectEditClass>.Enumerator);
			yield break;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000341D File Offset: 0x0000161D
		public static void InjectClassIntoMenu(SectoryMenu.InjectEditClass editClass)
		{
			SectoryMenu.injectedClasses.Add(editClass);
		}

		// Token: 0x04000082 RID: 130
		internal static Menu mostRecentMenu;

		// Token: 0x04000083 RID: 131
		internal static Transform content1;

		// Token: 0x04000084 RID: 132
		internal static Transform content2;

		// Token: 0x04000085 RID: 133
		internal static Transform title;

		// Token: 0x04000086 RID: 134
		internal static GameObject injectedClass;

		// Token: 0x04000087 RID: 135
		internal static GameObject numberVar;

		// Token: 0x04000088 RID: 136
		internal static GameObject boolVar;

		// Token: 0x04000089 RID: 137
		internal static GameObject action;

		// Token: 0x0400008A RID: 138
		private static List<SectoryMenu.InjectEditClass> injectedClasses = new List<SectoryMenu.InjectEditClass>();

		// Token: 0x0200002E RID: 46
		public class InjectEditClass
		{
			// Token: 0x060000A9 RID: 169 RVA: 0x00007BE9 File Offset: 0x00005DE9
			public InjectEditClass(string modName, object classInstance)
			{
				this.modName = modName;
				this.classInstance = classInstance;
				this.type = classInstance.GetType();
			}

			// Token: 0x060000AA RID: 170 RVA: 0x00007C10 File Offset: 0x00005E10
			public void InjectVariables()
			{
				foreach (object obj in SectoryMenu.content2.transform)
				{
					Transform transform = (Transform)obj;
					Object.Destroy(transform.gameObject);
				}
				PropertyInfo[] properties = this.type.GetProperties();
				for (int i = 0; i < properties.Length; i++)
				{
					PropertyInfo info = properties[i];
					bool flag = info.GetCustomAttributes(typeof(MenuIgnore)).Any<Attribute>();
					if (!flag)
					{
						bool flag2 = info.PropertyType == typeof(float) || info.PropertyType == typeof(int);
						if (flag2)
						{
							bool percentage = info.GetCustomAttributes(typeof(Percentage)).Any<Attribute>();
							GameObject go = Object.Instantiate<GameObject>(SectoryMenu.numberVar, SectoryMenu.content2);
							go.transform.Find("Name").GetComponent<Text>().text = info.Name;
							Text component = go.transform.Find("Value").GetComponent<Text>();
							string text;
							if (!percentage)
							{
								object value4 = info.GetValue(this.classInstance);
								text = ((value4 != null) ? value4.ToString() : null);
							}
							else
							{
								text = ((float)info.GetValue(this.classInstance) * 100f).ToString();
							}
							component.text = text;
							Slider slider = go.transform.Find("Slider").GetComponent<Slider>();
							bool flag3 = info.GetCustomAttributes(typeof(Range)).Any<Attribute>();
							if (flag3)
							{
								Range range = (Range)info.GetCustomAttributes(typeof(Range)).Single<Attribute>();
								slider.minValue = (float)range.min;
								slider.maxValue = (float)range.max;
							}
							slider.value = (percentage ? ((float)info.GetValue(this.classInstance) * 100f) : ((float)info.GetValue(this.classInstance)));
							slider.onValueChanged.AddListener(delegate(float value)
							{
								value = (float)Math.Round((double)value, (slider.maxValue > 30f) ? 0 : 1);
								info.SetValue(this.classInstance, value);
								go.transform.Find("Value").GetComponent<Text>().text = value.ToString();
								bool flag11 = this.classInstance.GetType().GetCustomAttributes(typeof(Serialize)).Any<Attribute>();
								if (flag11)
								{
									bool percentage = percentage;
									if (percentage)
									{
										info.SetValue(this.classInstance, value * 100f);
									}
									File.WriteAllText(Path.Combine(Entry.GetSettingsPath, ((Serialize)this.classInstance.GetType().GetCustomAttributes(typeof(Serialize)).Single<Attribute>()).path + ".json"), JsonConvert.SerializeObject(this.classInstance, 1));
									bool percentage2 = percentage;
									if (percentage2)
									{
										info.SetValue(this.classInstance, value);
									}
								}
							});
						}
						else
						{
							bool flag4 = info.PropertyType == typeof(bool);
							if (flag4)
							{
								GameObject go = Object.Instantiate<GameObject>(SectoryMenu.boolVar, SectoryMenu.content2);
								go.transform.Find("Name").GetComponent<Text>().text = info.Name;
								Text component2 = go.transform.Find("Value").GetComponent<Text>();
								object value2 = info.GetValue(this.classInstance);
								component2.text = ((value2 != null) ? value2.ToString() : null);
								go.transform.GetComponent<Button>().onClick.AddListener(delegate()
								{
									info.SetValue(this.classInstance, !(bool)info.GetValue(this.classInstance));
									go.transform.Find("Value").GetComponent<Text>().text = ((bool)info.GetValue(this.classInstance)).ToString();
									bool flag11 = this.classInstance.GetType().GetCustomAttributes(typeof(Serialize)).Any<Attribute>();
									if (flag11)
									{
										File.WriteAllText(Path.Combine(Entry.GetSettingsPath, ((Serialize)this.classInstance.GetType().GetCustomAttributes(typeof(Serialize)).Single<Attribute>()).path + ".json"), JsonConvert.SerializeObject(this.classInstance, 1));
									}
								});
							}
						}
					}
				}
				FieldInfo[] fields = this.type.GetFields();
				for (int j = 0; j < fields.Length; j++)
				{
					FieldInfo info = fields[j];
					bool flag5 = info.GetCustomAttributes(typeof(MenuIgnore)).Any<Attribute>();
					if (!flag5)
					{
						bool flag6 = info.FieldType == typeof(float) || info.FieldType == typeof(int);
						if (flag6)
						{
							bool percentage = info.GetCustomAttributes(typeof(Percentage)).Any<Attribute>();
							GameObject go = Object.Instantiate<GameObject>(SectoryMenu.numberVar, SectoryMenu.content2);
							go.transform.Find("Name").GetComponent<Text>().text = info.Name;
							Text component3 = go.transform.Find("Value").GetComponent<Text>();
							string text2;
							if (!percentage)
							{
								object value3 = info.GetValue(this.classInstance);
								text2 = ((value3 != null) ? value3.ToString() : null);
							}
							else
							{
								text2 = ((float)info.GetValue(this.classInstance) * 100f).ToString();
							}
							component3.text = text2;
							Slider slider = go.transform.Find("Slider").GetComponent<Slider>();
							bool flag7 = info.GetCustomAttributes(typeof(Range)).Any<Attribute>();
							if (flag7)
							{
								Range range2 = (Range)info.GetCustomAttributes(typeof(Range)).Single<Attribute>();
								slider.minValue = (float)range2.min;
								slider.maxValue = (float)range2.max;
							}
							slider.value = (percentage ? ((float)info.GetValue(this.classInstance) * 100f) : ((float)info.GetValue(this.classInstance)));
							slider.onValueChanged.AddListener(delegate(float value)
							{
								value = (float)Math.Round((double)value, (slider.maxValue > 30f) ? 0 : 1);
								info.SetValue(this.classInstance, value);
								go.transform.Find("Value").GetComponent<Text>().text = value.ToString();
								bool flag11 = this.classInstance.GetType().GetCustomAttributes(typeof(Serialize)).Any<Attribute>();
								if (flag11)
								{
									bool percentage = percentage;
									if (percentage)
									{
										info.SetValue(this.classInstance, (float)Math.Round((double)(value * 100f), 1));
									}
									File.WriteAllText(Path.Combine(Entry.GetSettingsPath, ((Serialize)this.classInstance.GetType().GetCustomAttributes(typeof(Serialize)).Single<Attribute>()).path + ".json"), JsonConvert.SerializeObject(this.classInstance, 1));
									bool percentage2 = percentage;
									if (percentage2)
									{
										info.SetValue(this.classInstance, (float)Math.Round((double)value, (slider.maxValue > 30f) ? 0 : 1));
									}
								}
							});
						}
						else
						{
							bool flag8 = info.FieldType == typeof(bool);
							if (flag8)
							{
								GameObject go = Object.Instantiate<GameObject>(SectoryMenu.boolVar, SectoryMenu.content2);
								go.transform.Find("Name").GetComponent<Text>().text = info.Name;
								go.transform.Find("Value").GetComponent<Text>().text = info.GetValue(this.classInstance).ToString();
								go.transform.GetComponent<Button>().onClick.AddListener(delegate()
								{
									info.SetValue(this.classInstance, !(bool)info.GetValue(this.classInstance));
									go.transform.Find("Value").GetComponent<Text>().text = ((bool)info.GetValue(this.classInstance)).ToString();
									bool flag11 = this.classInstance.GetType().GetCustomAttributes(typeof(Serialize)).Any<Attribute>();
									if (flag11)
									{
										File.WriteAllText(Path.Combine(Entry.GetSettingsPath, ((Serialize)this.classInstance.GetType().GetCustomAttributes(typeof(Serialize)).Single<Attribute>()).path + ".json"), JsonConvert.SerializeObject(this.classInstance, 1));
									}
								});
							}
							else
							{
								bool flag9 = info.FieldType == typeof(Action);
								if (flag9)
								{
									GameObject gameObject = Object.Instantiate<GameObject>(SectoryMenu.action, SectoryMenu.content2);
									gameObject.transform.Find("Name").GetComponent<Text>().text = info.Name;
									gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(((Action)info.GetValue(this.classInstance)).Invoke));
									ActionImage actionImage = info.GetCustomAttributes(typeof(ActionImage)).SingleOrDefault<Attribute>() as ActionImage;
									bool flag10 = actionImage != null;
									if (flag10)
									{
										gameObject.transform.GetComponent<Image>().sprite = Addressables.LoadAssetAsync<Sprite>(actionImage.address).WaitForCompletion();
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x040000FE RID: 254
			public string modName;

			// Token: 0x040000FF RID: 255
			private object classInstance;

			// Token: 0x04000100 RID: 256
			public Type type;
		}
	}
}

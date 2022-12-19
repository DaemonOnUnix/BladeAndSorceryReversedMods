using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TimeStopSpell
{
	// Token: 0x02000002 RID: 2
	public class TimeStopMenuModule : MenuModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020AC File Offset: 0x000002AC
		public override void Init(MenuData menuData, Menu menu)
		{
			base.Init(menuData, menu);
			this._hasEffectStatusButton = menu.GetCustomReference("HasEffectStatusButton", true).GetComponent<Button>();
			this.LoadData();
			this._hasEffectStatusButton.GetComponentInChildren<Text>().text = (this.timeStopData.data.hasEffect ? "Enabled" : "Disabled");
			this._hasEffectStatusButton.onClick.AddListener(delegate()
			{
				TimeStopJSONData data = this.timeStopData.data;
				data.hasEffect = !data.hasEffect;
				this._hasEffectStatusButton.GetComponentInChildren<Text>().text = (this.timeStopData.data.hasEffect ? "Enabled" : "Disabled");
				this.SaveData();
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002128 File Offset: 0x00000328
		private string GetDataFilePath()
		{
			return FileManager.GetFullPath(1, 1, this.modFolderName + this.dataFilePath);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002144 File Offset: 0x00000344
		public void LoadData()
		{
			string text = File.ReadAllText(this.GetDataFilePath());
			TimeStopJSONData timeStopJSONData = JsonConvert.DeserializeObject<TimeStopJSONData>(text, Catalog.GetJsonNetSerializerSettings());
			this.timeStopData = GameManager.local.gameObject.AddComponent<TimeStopData>();
			Debug.Log("Time stop data loaded in menubook");
			Debug.Log(this.timeStopData);
			this.timeStopData.data = timeStopJSONData;
			GameManager.local.StartCoroutine(this.LoadStopTimeAudioClip());
			GameManager.local.StartCoroutine(this.LoadResumeTimeAudioClip());
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000023AC File Offset: 0x000005AC
		private IEnumerator LoadStopTimeAudioClip()
		{
			if (this.timeStopData.data.stopTimeAudioClip == null)
			{
				this.timeStopData.stopTimeAudioSource = null;
			}
			else
			{
				string dir = FileManager.GetFullPath(1, 1, "TimeStopSpell/Data/").Replace("\\", "/").Replace(" ", "%20");
				string url = "file:///" + dir + this.timeStopData.data.stopTimeAudioClip;
				using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, 13))
				{
					yield return www.SendWebRequest();
					if (www.error != null)
					{
						Debug.Log(www.error);
					}
					else
					{
						this.timeStopData.stopTimeAudioSource.clip = DownloadHandlerAudioClip.GetContent(www);
					}
				}
			}
			yield break;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000025B0 File Offset: 0x000007B0
		private IEnumerator LoadResumeTimeAudioClip()
		{
			if (this.timeStopData.data.resumeTimeAudioClip == null)
			{
				this.timeStopData.resumeTimeAudioSource = null;
			}
			else
			{
				string dir = FileManager.GetFullPath(1, 1, "TimeStopSpell/Data/").Replace("\\", "/").Replace(" ", "%20");
				string url = "file:///" + dir + this.timeStopData.data.resumeTimeAudioClip;
				using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, 13))
				{
					yield return www.SendWebRequest();
					if (www.error != null)
					{
						Debug.Log(www.error);
					}
					else
					{
						this.timeStopData.resumeTimeAudioSource.clip = DownloadHandlerAudioClip.GetContent(www);
					}
				}
			}
			yield break;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000025CC File Offset: 0x000007CC
		public void SaveData()
		{
			string text = JsonConvert.SerializeObject(this.timeStopData.data, Catalog.GetJsonNetSerializerSettings());
			File.WriteAllText(this.GetDataFilePath(), text);
		}

		// Token: 0x04000001 RID: 1
		private Button _hasEffectStatusButton;

		// Token: 0x04000002 RID: 2
		public string modFolderName = "TimeStopSpell";

		// Token: 0x04000003 RID: 3
		public string dataFilePath = "\\Data\\TimeStopSpellData.json";

		// Token: 0x04000004 RID: 4
		public TimeStopData timeStopData;
	}
}

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Speech.Recognition;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;

namespace Threat
{
	// Token: 0x02000004 RID: 4
	public class ThreatLevelModule : LevelModule
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002B44 File Offset: 0x00000D44
		public override IEnumerator OnLoadCoroutine()
		{
			ThreatLevelModule.local = this;
			string text = "[Threat] Mod v";
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			Debug.Log(text + ((version != null) ? version.ToString() : null) + " loaded.");
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.EventManager_onCreatureSpawn);
			Application.quitting += delegate()
			{
				Process.GetCurrentProcess().Kill();
			};
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002BCC File Offset: 0x00000DCC
		public override void Update()
		{
			bool flag = this.useSpeech && !this.speechInitialised;
			if (flag)
			{
				this.SetupSpeechRecognition();
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002BF9 File Offset: 0x00000DF9
		public override void OnUnload()
		{
			EventManager.onCreatureSpawn -= new EventManager.CreatureSpawnedEvent(this.EventManager_onCreatureSpawn);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002C10 File Offset: 0x00000E10
		private void EventManager_onCreatureSpawn(Creature creature)
		{
			bool flag = !creature.isPlayer;
			if (flag)
			{
				bool flag2 = Array.Exists<string>(ThreatLevelModule.humanCreatureList, (string elem) => elem == creature.creatureId);
				if (flag2)
				{
					GameObject threatDetection = new GameObject("ThreatDetection");
					threatDetection.SetActive(false);
					ThreatBehaviour threatBehaviour = threatDetection.AddComponent<ThreatBehaviour>();
					threatBehaviour.creature = creature;
					threatDetection.SetActive(true);
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002C8C File Offset: 0x00000E8C
		private void SetupSpeechRecognition()
		{
			try
			{
				ThreatLevelModule.speechRecognition = new SpeechRecognitionEngine();
				ThreatLevelModule.speechRecognition.SetInputToDefaultAudioDevice();
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "[HJ_Threat] Cannot initialise SpeechRecognitionEngine : (";
				array[1] = ex.Message;
				array[2] = ") \n (";
				int num = 3;
				Exception innerException = ex.InnerException;
				array[num] = ((innerException != null) ? innerException.Message : null);
				array[4] = ")\n";
				array[5] = ex.StackTrace;
				Debug.LogError(string.Concat(array));
				return;
			}
			string filePath = Path.Combine(Application.streamingAssetsPath, FileManager.modFolderName, "HJ_Threat", "commands.txt");
			try
			{
				string jsonText = File.ReadAllText(filePath);
				ThreatCommands commands = JsonConvert.DeserializeObject<ThreatCommands>(jsonText);
				ThreatLevelModule.dropGrammar = new Grammar(new GrammarBuilder(new Choices(commands.drop))
				{
					Culture = ThreatLevelModule.speechRecognition.RecognizerInfo.Culture
				});
				ThreatLevelModule.joinGrammar = new Grammar(new GrammarBuilder(new Choices(commands.join))
				{
					Culture = ThreatLevelModule.speechRecognition.RecognizerInfo.Culture
				});
				ThreatLevelModule.speechRecognition.LoadGrammar(ThreatLevelModule.dropGrammar);
				ThreatLevelModule.speechRecognition.LoadGrammar(ThreatLevelModule.joinGrammar);
			}
			catch (Exception ex2)
			{
				string[] array2 = new string[8];
				array2[0] = "[HJ_Threat] LoadJson : Cannot read file ";
				array2[1] = filePath;
				array2[2] = " (";
				array2[3] = ex2.Message;
				array2[4] = ") \n (";
				int num2 = 5;
				Exception innerException2 = ex2.InnerException;
				array2[num2] = ((innerException2 != null) ? innerException2.Message : null);
				array2[6] = ")\n";
				array2[7] = ex2.StackTrace;
				Debug.LogError(string.Concat(array2));
				return;
			}
			try
			{
				ThreatLevelModule.speechRecognition.RecognizeAsync(RecognizeMode.Multiple);
			}
			catch (Exception ex3)
			{
				string[] array3 = new string[6];
				array3[0] = "[HJ_Threat] Failed to start speech recognition : (";
				array3[1] = ex3.Message;
				array3[2] = ") \n (";
				int num3 = 3;
				Exception innerException3 = ex3.InnerException;
				array3[num3] = ((innerException3 != null) ? innerException3.Message : null);
				array3[4] = ")\n";
				array3[5] = ex3.StackTrace;
				Debug.LogError(string.Concat(array3));
				return;
			}
			this.speechInitialised = true;
		}

		// Token: 0x04000014 RID: 20
		public static ThreatLevelModule local;

		// Token: 0x04000015 RID: 21
		public static SpeechRecognitionEngine speechRecognition;

		// Token: 0x04000016 RID: 22
		public static Grammar dropGrammar;

		// Token: 0x04000017 RID: 23
		public static Grammar joinGrammar;

		// Token: 0x04000018 RID: 24
		public float minTimeBeforeThreatInSeconds;

		// Token: 0x04000019 RID: 25
		public float minTimeBeforeDisarmInSeconds;

		// Token: 0x0400001A RID: 26
		public float minTimeBeforeAllyInSeconds;

		// Token: 0x0400001B RID: 27
		[Range(0f, 1f)]
		public float chanceOfAlly;

		// Token: 0x0400001C RID: 28
		public bool useHonour;

		// Token: 0x0400001D RID: 29
		public int minHonourAmount;

		// Token: 0x0400001E RID: 30
		public int targetHonourAmount;

		// Token: 0x0400001F RID: 31
		public bool useSpeech;

		// Token: 0x04000020 RID: 32
		[Range(0f, 1f)]
		public float minConfidence;

		// Token: 0x04000021 RID: 33
		public bool threatAffectsAllies;

		// Token: 0x04000022 RID: 34
		private static string[] humanCreatureList = new string[] { "HumanFemale", "HumanMale" };

		// Token: 0x04000023 RID: 35
		private bool speechInitialised;
	}
}

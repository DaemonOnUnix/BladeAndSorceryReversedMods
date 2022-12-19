using System;
using System.IO;
using System.Reflection;
using ThunderRoad;
using UnityEngine;

namespace Wully.Utils
{
	// Token: 0x02000005 RID: 5
	public static class Extensions
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000025F8 File Offset: 0x000007F8
		public static string GetManifest(Type type)
		{
			string manifestName = type.Assembly.GetName().Name + "\\manifest.json";
			return File.ReadAllText(FileManager.GetFullPath(1, 1, manifestName));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000262D File Offset: 0x0000082D
		public static bool IsNull(this Object gameObject)
		{
			return gameObject == null;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002633 File Offset: 0x00000833
		public static bool IsNotNull(this Object gameObject)
		{
			return !gameObject.IsNull();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000263E File Offset: 0x0000083E
		public static bool ObjectEqual(this Object gameObject, Object otherObject)
		{
			return gameObject == otherObject;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002644 File Offset: 0x00000844
		public static T Read<T>(this object source, string fieldName)
		{
			return (T)((object)source.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(source));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000265F File Offset: 0x0000085F
		public static void Set<T>(this object source, string fieldName, T val)
		{
			source.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(source, val);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000267C File Offset: 0x0000087C
		internal static void Raise(this object source, string eventName, object eventArgs)
		{
			MulticastDelegate eventDelegate = (MulticastDelegate)source.GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(source);
			if (eventDelegate != null)
			{
				eventDelegate.DynamicInvoke(new object[] { eventArgs });
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026B8 File Offset: 0x000008B8
		public static void Enable(this EffectInstance instance)
		{
			for (int i = instance.effects.Count - 1; i >= 0; i--)
			{
				instance.effects[i].gameObject.SetActive(true);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026F4 File Offset: 0x000008F4
		public static void Disable(this EffectInstance instance)
		{
			for (int i = instance.effects.Count - 1; i >= 0; i--)
			{
				instance.effects[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002730 File Offset: 0x00000930
		public static void SetRotation(this EffectInstance instance, Quaternion rotation)
		{
			for (int i = instance.effects.Count - 1; i >= 0; i--)
			{
				instance.effects[i].transform.rotation = rotation;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000276C File Offset: 0x0000096C
		public static void SetPosition(this EffectInstance instance, Vector3 position)
		{
			for (int i = instance.effects.Count - 1; i >= 0; i--)
			{
				instance.effects[i].transform.position = position;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027A8 File Offset: 0x000009A8
		public static RagdollPart GetRagdollPartByName(string name)
		{
			if (Player.local && Player.local.creature)
			{
				foreach (RagdollPart ragdollPart in Player.local.creature.ragdoll.parts)
				{
					if (ragdollPart.name.Equals(name))
					{
						return ragdollPart;
					}
				}
			}
			Debug.Log("Couldn't find ragdoll part: " + name);
			return null;
		}
	}
}

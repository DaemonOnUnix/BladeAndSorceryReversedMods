﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x0200007C RID: 124
	public class PatchClassProcessor
	{
		// Token: 0x06000255 RID: 597 RVA: 0x0000CE4C File Offset: 0x0000B04C
		public PatchClassProcessor(Harmony instance, Type type)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.instance = instance;
			this.containerType = type;
			List<HarmonyMethod> fromType = HarmonyMethodExtensions.GetFromType(type);
			if (fromType == null || fromType.Count == 0)
			{
				return;
			}
			this.containerAttributes = HarmonyMethod.Merge(fromType);
			MethodType? methodType = this.containerAttributes.methodType;
			if (methodType == null)
			{
				this.containerAttributes.methodType = new MethodType?(MethodType.Normal);
			}
			this.auxilaryMethods = new Dictionary<Type, MethodInfo>();
			foreach (Type type2 in PatchClassProcessor.auxilaryTypes)
			{
				MethodInfo patchMethod = PatchTools.GetPatchMethod(this.containerType, type2.FullName);
				if (patchMethod != null)
				{
					this.auxilaryMethods[type2] = patchMethod;
				}
			}
			this.patchMethods = PatchTools.GetPatchMethods(this.containerType);
			foreach (AttributePatch attributePatch in this.patchMethods)
			{
				MethodInfo method = attributePatch.info.method;
				attributePatch.info = this.containerAttributes.Merge(attributePatch.info);
				attributePatch.info.method = method;
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		public List<MethodInfo> Patch()
		{
			if (this.containerAttributes == null)
			{
				return null;
			}
			Exception ex = null;
			if (!this.RunMethod<HarmonyPrepare, bool>(true, false, null, Array.Empty<object>()))
			{
				this.RunMethod<HarmonyCleanup>(ref ex, Array.Empty<object>());
				this.ReportException(ex, null);
				return new List<MethodInfo>();
			}
			List<MethodInfo> list = new List<MethodInfo>();
			MethodBase methodBase = null;
			try
			{
				List<MethodBase> bulkMethods = this.GetBulkMethods();
				if (bulkMethods.Count == 1)
				{
					methodBase = bulkMethods[0];
				}
				this.ReversePatch(ref methodBase);
				list = ((bulkMethods.Count > 0) ? this.BulkPatch(bulkMethods, ref methodBase) : this.PatchWithAttributes(ref methodBase));
			}
			catch (Exception ex)
			{
			}
			this.RunMethod<HarmonyCleanup>(ref ex, new object[] { ex });
			this.ReportException(ex, methodBase);
			return list;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000D07C File Offset: 0x0000B27C
		private void ReversePatch(ref MethodBase lastOriginal)
		{
			for (int i = 0; i < this.patchMethods.Count; i++)
			{
				AttributePatch attributePatch = this.patchMethods[i];
				HarmonyPatchType? type = attributePatch.type;
				HarmonyPatchType harmonyPatchType = HarmonyPatchType.ReversePatch;
				if ((type.GetValueOrDefault() == harmonyPatchType) & (type != null))
				{
					MethodBase originalMethod = attributePatch.info.GetOriginalMethod();
					if (originalMethod != null)
					{
						lastOriginal = originalMethod;
					}
					ReversePatcher reversePatcher = this.instance.CreateReversePatcher(lastOriginal, attributePatch.info);
					object locker = PatchProcessor.locker;
					lock (locker)
					{
						reversePatcher.Patch(HarmonyReversePatchType.Original);
					}
				}
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000D134 File Offset: 0x0000B334
		private List<MethodInfo> BulkPatch(List<MethodBase> originals, ref MethodBase lastOriginal)
		{
			PatchJobs<MethodInfo> patchJobs = new PatchJobs<MethodInfo>();
			for (int i = 0; i < originals.Count; i++)
			{
				lastOriginal = originals[i];
				PatchJobs<MethodInfo>.Job job = patchJobs.GetJob(lastOriginal);
				foreach (AttributePatch attributePatch in this.patchMethods)
				{
					string text = "You cannot combine TargetMethod, TargetMethods or [HarmonyPatchAll] with individual annotations";
					HarmonyMethod info = attributePatch.info;
					if (info.methodName != null)
					{
						throw new ArgumentException(text + " [" + info.methodName + "]");
					}
					if (info.methodType != null && info.methodType.Value != MethodType.Normal)
					{
						throw new ArgumentException(string.Format("{0} [{1}]", text, info.methodType));
					}
					if (info.argumentTypes != null)
					{
						throw new ArgumentException(text + " [" + info.argumentTypes.Description() + "]");
					}
					job.AddPatch(attributePatch);
				}
			}
			foreach (PatchJobs<MethodInfo>.Job job2 in patchJobs.GetJobs())
			{
				lastOriginal = job2.original;
				this.ProcessPatchJob(job2);
			}
			return patchJobs.GetReplacements();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		private List<MethodInfo> PatchWithAttributes(ref MethodBase lastOriginal)
		{
			PatchJobs<MethodInfo> patchJobs = new PatchJobs<MethodInfo>();
			foreach (AttributePatch attributePatch in this.patchMethods)
			{
				lastOriginal = attributePatch.info.GetOriginalMethod();
				if (lastOriginal == null)
				{
					throw new ArgumentException("Undefined target method for patch method " + attributePatch.info.method.FullDescription());
				}
				patchJobs.GetJob(lastOriginal).AddPatch(attributePatch);
			}
			foreach (PatchJobs<MethodInfo>.Job job in patchJobs.GetJobs())
			{
				lastOriginal = job.original;
				this.ProcessPatchJob(job);
			}
			return patchJobs.GetReplacements();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000D394 File Offset: 0x0000B594
		private void ProcessPatchJob(PatchJobs<MethodInfo>.Job job)
		{
			MethodInfo methodInfo = null;
			bool flag = this.RunMethod<HarmonyPrepare, bool>(true, false, null, new object[] { job.original });
			Exception ex = null;
			if (flag)
			{
				object locker = PatchProcessor.locker;
				lock (locker)
				{
					try
					{
						PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(job.original) ?? new PatchInfo();
						patchInfo.AddPrefixes(this.instance.Id, job.prefixes.ToArray());
						patchInfo.AddPostfixes(this.instance.Id, job.postfixes.ToArray());
						patchInfo.AddTranspilers(this.instance.Id, job.transpilers.ToArray());
						patchInfo.AddFinalizers(this.instance.Id, job.finalizers.ToArray());
						methodInfo = PatchFunctions.UpdateWrapper(job.original, patchInfo);
						HarmonySharedState.UpdatePatchInfo(job.original, methodInfo, patchInfo);
					}
					catch (Exception ex)
					{
					}
				}
			}
			this.RunMethod<HarmonyCleanup>(ref ex, new object[] { job.original, ex });
			this.ReportException(ex, job.original);
			job.replacement = methodInfo;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
		private List<MethodBase> GetBulkMethods()
		{
			if (this.containerType.GetCustomAttributes(true).Any((object a) => a.GetType().FullName == typeof(HarmonyPatchAll).FullName))
			{
				Type declaringType = this.containerAttributes.declaringType;
				if (declaringType == null)
				{
					throw new ArgumentException("Using " + typeof(HarmonyPatchAll).FullName + " requires an additional attribute for specifying the Class/Type");
				}
				List<MethodBase> list = new List<MethodBase>();
				list.AddRange(AccessTools.GetDeclaredConstructors(declaringType, null).Cast<MethodBase>());
				list.AddRange(AccessTools.GetDeclaredMethods(declaringType).Cast<MethodBase>());
				List<PropertyInfo> declaredProperties = AccessTools.GetDeclaredProperties(declaringType);
				list.AddRange((from prop in declaredProperties
					select prop.GetGetMethod(true) into method
					where method != null
					select method).Cast<MethodBase>());
				list.AddRange((from prop in declaredProperties
					select prop.GetSetMethod(true) into method
					where method != null
					select method).Cast<MethodBase>());
				return list;
			}
			else
			{
				IEnumerable<MethodBase> enumerable = this.RunMethod<HarmonyTargetMethods, IEnumerable<MethodBase>>(null, null, new Func<IEnumerable<MethodBase>, string>(PatchClassProcessor.<GetBulkMethods>g__FailOnResult|12_1), Array.Empty<object>());
				if (enumerable != null)
				{
					return enumerable.ToList<MethodBase>();
				}
				List<MethodBase> list2 = new List<MethodBase>();
				MethodBase methodBase = this.RunMethod<HarmonyTargetMethod, MethodBase>(null, null, delegate(MethodBase method)
				{
					if (method != null)
					{
						return null;
					}
					return "null";
				}, Array.Empty<object>());
				if (methodBase != null)
				{
					list2.Add(methodBase);
				}
				return list2;
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000D690 File Offset: 0x0000B890
		private void ReportException(Exception exception, MethodBase original)
		{
			if (exception == null)
			{
				return;
			}
			if (this.containerAttributes.debug.GetValueOrDefault() || Harmony.DEBUG)
			{
				Version version;
				Harmony.VersionInfo(out version);
				FileLog.indentLevel = 0;
				FileLog.Log(string.Format("### Exception from user \"{0}\", Harmony v{1}", this.instance.Id, version));
				FileLog.Log("### Original: " + (((original != null) ? original.FullDescription() : null) ?? "NULL"));
				FileLog.Log("### Patch class: " + this.containerType.FullDescription());
				Exception ex = exception;
				HarmonyException ex2 = ex as HarmonyException;
				if (ex2 != null)
				{
					ex = ex2.InnerException;
				}
				string text = ex.ToString();
				while (text.Contains("\n\n"))
				{
					text = text.Replace("\n\n", "\n");
				}
				text = text.Split(new char[] { '\n' }).Join((string line) => "### " + line, "\n");
				FileLog.Log(text.Trim());
			}
			if (exception is HarmonyException)
			{
				throw exception;
			}
			throw new HarmonyException("Patching exception in method " + original.FullDescription(), exception);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000D7C4 File Offset: 0x0000B9C4
		private T RunMethod<S, T>(T defaultIfNotExisting, T defaultIfFailing, Func<T, string> failOnResult = null, params object[] parameters)
		{
			MethodInfo methodInfo;
			if (!this.auxilaryMethods.TryGetValue(typeof(S), out methodInfo))
			{
				return defaultIfNotExisting;
			}
			object[] array = (parameters ?? new object[0]).Union(new object[] { this.instance }).ToArray<object>();
			object[] array2 = AccessTools.ActualParameters(methodInfo, array);
			if (methodInfo.ReturnType != typeof(void) && !typeof(T).IsAssignableFrom(methodInfo.ReturnType))
			{
				throw new Exception(string.Concat(new string[]
				{
					"Method ",
					methodInfo.FullDescription(),
					" has wrong return type (should be assignable to ",
					typeof(T).FullName,
					")"
				}));
			}
			T t = defaultIfFailing;
			try
			{
				if (methodInfo.ReturnType == typeof(void))
				{
					methodInfo.Invoke(null, array2);
					t = defaultIfNotExisting;
				}
				else
				{
					t = (T)((object)methodInfo.Invoke(null, array2));
				}
				if (failOnResult != null)
				{
					string text = failOnResult(t);
					if (text != null)
					{
						throw new Exception("Method " + methodInfo.FullDescription() + " returned an unexpected result: " + text);
					}
				}
			}
			catch (Exception ex)
			{
				this.ReportException(ex, methodInfo);
			}
			return t;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000D910 File Offset: 0x0000BB10
		private void RunMethod<S>(ref Exception exception, params object[] parameters)
		{
			MethodInfo methodInfo;
			if (this.auxilaryMethods.TryGetValue(typeof(S), out methodInfo))
			{
				object[] array = (parameters ?? new object[0]).Union(new object[] { this.instance }).ToArray<object>();
				object[] array2 = AccessTools.ActualParameters(methodInfo, array);
				try
				{
					object obj = methodInfo.Invoke(null, array2);
					if (methodInfo.ReturnType == typeof(Exception))
					{
						exception = obj as Exception;
					}
				}
				catch (Exception ex)
				{
					this.ReportException(ex, methodInfo);
				}
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000DA03 File Offset: 0x0000BC03
		[CompilerGenerated]
		internal static string <GetBulkMethods>g__FailOnResult|12_1(IEnumerable<MethodBase> res)
		{
			if (res == null)
			{
				return "null";
			}
			if (res.Any((MethodBase m) => m == null))
			{
				return "some element was null";
			}
			return null;
		}

		// Token: 0x0400016D RID: 365
		private readonly Harmony instance;

		// Token: 0x0400016E RID: 366
		private readonly Type containerType;

		// Token: 0x0400016F RID: 367
		private readonly HarmonyMethod containerAttributes;

		// Token: 0x04000170 RID: 368
		private readonly Dictionary<Type, MethodInfo> auxilaryMethods;

		// Token: 0x04000171 RID: 369
		private readonly List<AttributePatch> patchMethods;

		// Token: 0x04000172 RID: 370
		private static readonly List<Type> auxilaryTypes = new List<Type>
		{
			typeof(HarmonyPrepare),
			typeof(HarmonyCleanup),
			typeof(HarmonyTargetMethod),
			typeof(HarmonyTargetMethods)
		};
	}
}

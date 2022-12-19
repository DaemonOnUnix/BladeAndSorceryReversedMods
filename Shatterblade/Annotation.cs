using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Shatterblade
{
	// Token: 0x0200000A RID: 10
	public class Annotation : MonoBehaviour
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x0000564C File Offset: 0x0000384C
		public static string GetButtonString()
		{
			return "A/X/Touchpad";
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005653 File Offset: 0x00003853
		public static string GetButtonStringHand(Side side)
		{
			return ((side == 1) ? "X" : "A") + "Touchpad";
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005670 File Offset: 0x00003870
		public static Annotation CreateAnnotation(Shatterblade sword, Transform target, Transform rotationTarget, Vector3 offset)
		{
			Annotation annotation = new GameObject().AddComponent<Annotation>();
			annotation.Init(sword, target, rotationTarget, offset);
			return annotation;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005699 File Offset: 0x00003899
		public IEnumerator LoadData()
		{
			AsyncOperationHandle<Material> textHandle = Addressables.LoadAssetAsync<Material>("Lyneca.Shatterblade.TextMat");
			yield return textHandle;
			this.text.GetComponent<MeshRenderer>().material = textHandle.Result;
			AsyncOperationHandle<Font> fontHandle = Addressables.LoadAssetAsync<Font>("Lyneca.Shatterblade.Font");
			yield return fontHandle;
			this.text.font = fontHandle.Result;
			AsyncOperationHandle<Material> lineHandle = Addressables.LoadAssetAsync<Material>("Lyneca.Shatterblade.LineMat");
			yield return lineHandle;
			this.lines.material = lineHandle.Result;
			yield break;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000056A8 File Offset: 0x000038A8
		public void Init(Shatterblade sword, Transform target, Transform rotationTarget, Vector3 offset)
		{
			this.shouldShow = sword.isTutorialBlade;
			this.sword = sword;
			this.target = target;
			this.rotationTarget = target;
			this.offset = offset;
			this.text = base.gameObject.AddComponent<TextMesh>();
			this.text.alignment = 1;
			this.text.text = "";
			base.transform.localScale = Vector3.one * 0.003f;
			this.text.anchor = 4;
			base.transform.position = this.TargetPosition();
			this.lines = base.gameObject.AddComponent<LineRenderer>();
			this.lines.startWidth = 0.001f;
			this.lines.endWidth = 0.001f;
			this.lines.startColor = Color.black;
			this.lines.endColor = Color.black;
			this.lines.colorGradient = new Gradient();
			this.lines.colorGradient.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(Color.grey, 0f),
				new GradientColorKey(Color.black, 1f)
			}, new GradientAlphaKey[]
			{
				new GradientAlphaKey(0f, 0f),
				new GradientAlphaKey(1f, 1f)
			});
			base.StartCoroutine(this.LoadData());
			bool flag = !this.shouldShow;
			if (flag)
			{
				this.Hide();
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005844 File Offset: 0x00003A44
		public void SetTarget(Transform target)
		{
			this.target = target;
			this.rotationTarget = target;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005858 File Offset: 0x00003A58
		public void SetText(string newText, RagdollHand hand = null)
		{
			bool flag = !this.shouldShow;
			if (!flag)
			{
				this.Show();
				this.text.text = newText.Replace("[[BUTTON]]", (hand == null) ? Annotation.GetButtonString() : Annotation.GetButtonStringHand(hand.side));
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000058AE File Offset: 0x00003AAE
		private Camera PlayerCamera()
		{
			return Player.local.head.cam;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000058C0 File Offset: 0x00003AC0
		private Vector3 TargetPosition()
		{
			return this.target.position + this.rotationTarget.rotation * this.offset * 0.1f + (this.PlayerCamera().transform.position - this.target.position).normalized * 0.1f;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005934 File Offset: 0x00003B34
		public void Show()
		{
			bool flag = !this.shouldShow;
			if (!flag)
			{
				this.text.GetComponent<MeshRenderer>().enabled = true;
				this.lines.enabled = true;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005970 File Offset: 0x00003B70
		public void Hide()
		{
			this.text.GetComponent<MeshRenderer>().enabled = false;
			this.lines.enabled = false;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005992 File Offset: 0x00003B92
		public void Destroy()
		{
			Object.Destroy(this.lines);
			this.text.text = "";
			Object.Destroy(this.text);
			Object.Destroy(this);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000059C8 File Offset: 0x00003BC8
		public void Update()
		{
			bool flag = !this.shouldShow;
			if (!flag)
			{
				bool isDespawned = this.sword.isDespawned;
				if (isDespawned)
				{
					Object.Destroy(this);
				}
				base.transform.position = Vector3.Lerp(base.transform.position, this.TargetPosition(), Time.deltaTime * 10f);
				base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.PlayerCamera().transform.position);
				this.lines.SetPositions(new Vector3[]
				{
					this.target.position,
					base.transform.position + base.transform.forward * 0.01f - base.transform.up * 0.01f
				});
			}
		}

		// Token: 0x04000016 RID: 22
		private TextMesh text;

		// Token: 0x04000017 RID: 23
		private Shatterblade sword;

		// Token: 0x04000018 RID: 24
		private LineRenderer lines;

		// Token: 0x04000019 RID: 25
		private Transform target;

		// Token: 0x0400001A RID: 26
		public Vector3 offset;

		// Token: 0x0400001B RID: 27
		private Transform rotationTarget;

		// Token: 0x0400001C RID: 28
		public bool shouldShow;
	}
}

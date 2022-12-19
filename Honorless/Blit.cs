using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

// Token: 0x02000002 RID: 2
public class Blit : ScriptableRendererFeature
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public override void Create()
	{
		int passIndex = ((this.settings.blitMaterial != null) ? (this.settings.blitMaterial.passCount - 1) : 1);
		this.settings.blitMaterialPassIndex = Mathf.Clamp(this.settings.blitMaterialPassIndex, -1, passIndex);
		this.blitPass = new Blit.BlitPass(this.settings.Event, this.settings, base.name);
		if (this.settings.Event == 600)
		{
			Debug.LogWarning("Note that the \"After Rendering Post Processing\"'s Color target doesn't seem to work? (or might work, but doesn't contain the post processing) :( -- Use \"After Rendering\" instead!");
		}
		this.UpdateSrcIdentifier();
		this.UpdateDstIdentifier();
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000020ED File Offset: 0x000002ED
	private void UpdateSrcIdentifier()
	{
		this.srcIdentifier = this.UpdateIdentifier(this.settings.srcType, this.settings.srcTextureId, this.settings.srcTextureObject);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000211C File Offset: 0x0000031C
	private void UpdateDstIdentifier()
	{
		this.dstIdentifier = this.UpdateIdentifier(this.settings.dstType, this.settings.dstTextureId, this.settings.dstTextureObject);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000214C File Offset: 0x0000034C
	private RenderTargetIdentifier UpdateIdentifier(Blit.Target type, string s, RenderTexture obj)
	{
		if (type == Blit.Target.RenderTextureObject)
		{
			return obj;
		}
		if (type == Blit.Target.TextureID)
		{
			return s;
		}
		return default(RenderTargetIdentifier);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002178 File Offset: 0x00000378
	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (this.settings.blitMaterial == null)
		{
			Debug.LogWarningFormat("Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", new object[] { base.GetType().Name });
			return;
		}
		if (this.settings.Event != 600)
		{
			if (this.settings.Event == 1000 && renderingData.postProcessingEnabled)
			{
				if (this.settings.srcType == Blit.Target.CameraColor)
				{
					this.settings.srcType = Blit.Target.TextureID;
					this.settings.srcTextureId = "_AfterPostProcessTexture";
					this.UpdateSrcIdentifier();
				}
				if (this.settings.dstType == Blit.Target.CameraColor)
				{
					this.settings.dstType = Blit.Target.TextureID;
					this.settings.dstTextureId = "_AfterPostProcessTexture";
					this.UpdateDstIdentifier();
				}
			}
			else
			{
				if (this.settings.srcType == Blit.Target.TextureID && this.settings.srcTextureId == "_AfterPostProcessTexture")
				{
					this.settings.srcType = Blit.Target.CameraColor;
					this.settings.srcTextureId = "";
					this.UpdateSrcIdentifier();
				}
				if (this.settings.dstType == Blit.Target.TextureID && this.settings.dstTextureId == "_AfterPostProcessTexture")
				{
					this.settings.dstType = Blit.Target.CameraColor;
					this.settings.dstTextureId = "";
					this.UpdateDstIdentifier();
				}
			}
		}
		RenderTargetIdentifier src = ((this.settings.srcType == Blit.Target.CameraColor) ? renderer.cameraColorTarget : this.srcIdentifier);
		RenderTargetIdentifier dest = ((this.settings.dstType == Blit.Target.CameraColor) ? renderer.cameraColorTarget : this.dstIdentifier);
		this.blitPass.Setup(src, dest);
		renderer.EnqueuePass(this.blitPass);
	}

	// Token: 0x04000001 RID: 1
	public Blit.BlitSettings settings = new Blit.BlitSettings();

	// Token: 0x04000002 RID: 2
	private Blit.BlitPass blitPass;

	// Token: 0x04000003 RID: 3
	private RenderTargetIdentifier srcIdentifier;

	// Token: 0x04000004 RID: 4
	private RenderTargetIdentifier dstIdentifier;

	// Token: 0x02000010 RID: 16
	public class BlitPass : ScriptableRenderPass
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00005282 File Offset: 0x00003482
		// (set) Token: 0x06000069 RID: 105 RVA: 0x0000528A File Offset: 0x0000348A
		public FilterMode filterMode { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00005293 File Offset: 0x00003493
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000529B File Offset: 0x0000349B
		private RenderTargetIdentifier source { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000052A4 File Offset: 0x000034A4
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000052AC File Offset: 0x000034AC
		private RenderTargetIdentifier destination { get; set; }

		// Token: 0x0600006E RID: 110 RVA: 0x000052B8 File Offset: 0x000034B8
		public BlitPass(RenderPassEvent renderPassEvent, Blit.BlitSettings settings, string tag)
		{
			base.renderPassEvent = renderPassEvent;
			this.settings = settings;
			this.blitMaterial = settings.blitMaterial;
			this.m_ProfilerTag = tag;
			this.m_TemporaryColorTexture.Init("_TemporaryColorTexture");
			if (settings.dstType == Blit.Target.TextureID)
			{
				this.m_DestinationTexture.Init(settings.dstTextureId);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005316 File Offset: 0x00003516
		public void Setup(RenderTargetIdentifier source, RenderTargetIdentifier destination)
		{
			this.source = source;
			this.destination = destination;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005328 File Offset: 0x00003528
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer cmd = CommandBufferPool.Get(this.m_ProfilerTag);
			RenderTextureDescriptor opaqueDesc = (XRSettings.enabled ? XRSettings.eyeTextureDesc : renderingData.cameraData.cameraTargetDescriptor);
			opaqueDesc.depthBufferBits = 0;
			if (this.settings.setInverseViewMatrix)
			{
				Shader.SetGlobalMatrix("_InverseView", renderingData.cameraData.camera.cameraToWorldMatrix);
			}
			if (this.settings.dstType == Blit.Target.TextureID)
			{
				cmd.GetTemporaryRT(this.m_DestinationTexture.id, opaqueDesc, this.filterMode);
			}
			if (this.source == this.destination || (this.settings.srcType == this.settings.dstType && this.settings.srcType == Blit.Target.CameraColor))
			{
				cmd.GetTemporaryRT(this.m_TemporaryColorTexture.id, opaqueDesc, this.filterMode);
				base.Blit(cmd, this.source, this.m_TemporaryColorTexture.Identifier(), this.blitMaterial, this.settings.blitMaterialPassIndex);
				base.Blit(cmd, this.m_TemporaryColorTexture.Identifier(), this.destination, null, 0);
			}
			else
			{
				base.Blit(cmd, this.source, this.destination, this.blitMaterial, this.settings.blitMaterialPassIndex);
			}
			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005478 File Offset: 0x00003678
		public override void FrameCleanup(CommandBuffer cmd)
		{
			if (this.settings.dstType == Blit.Target.TextureID)
			{
				cmd.ReleaseTemporaryRT(this.m_DestinationTexture.id);
			}
			if (this.source == this.destination || (this.settings.srcType == this.settings.dstType && this.settings.srcType == Blit.Target.CameraColor))
			{
				cmd.ReleaseTemporaryRT(this.m_TemporaryColorTexture.id);
			}
		}

		// Token: 0x04000093 RID: 147
		public Material blitMaterial;

		// Token: 0x04000095 RID: 149
		private Blit.BlitSettings settings;

		// Token: 0x04000098 RID: 152
		private RenderTargetHandle m_TemporaryColorTexture;

		// Token: 0x04000099 RID: 153
		private RenderTargetHandle m_DestinationTexture;

		// Token: 0x0400009A RID: 154
		private string m_ProfilerTag;
	}

	// Token: 0x02000011 RID: 17
	[Serializable]
	public class BlitSettings
	{
		// Token: 0x0400009B RID: 155
		public RenderPassEvent Event = 300;

		// Token: 0x0400009C RID: 156
		public Material blitMaterial;

		// Token: 0x0400009D RID: 157
		public int blitMaterialPassIndex;

		// Token: 0x0400009E RID: 158
		public bool setInverseViewMatrix;

		// Token: 0x0400009F RID: 159
		public Blit.Target srcType;

		// Token: 0x040000A0 RID: 160
		public string srcTextureId = "_CameraColorTexture";

		// Token: 0x040000A1 RID: 161
		public RenderTexture srcTextureObject;

		// Token: 0x040000A2 RID: 162
		public Blit.Target dstType;

		// Token: 0x040000A3 RID: 163
		public string dstTextureId = "_BlitPassTexture";

		// Token: 0x040000A4 RID: 164
		public RenderTexture dstTextureObject;
	}

	// Token: 0x02000012 RID: 18
	public enum Target
	{
		// Token: 0x040000A6 RID: 166
		CameraColor,
		// Token: 0x040000A7 RID: 167
		TextureID,
		// Token: 0x040000A8 RID: 168
		RenderTextureObject
	}
}

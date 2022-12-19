using System;
using Wully.Render;

namespace Wully.RenderPass
{
	// Token: 0x02000007 RID: 7
	public class CustomPassBlit : CustomPass<Blit>
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002BCC File Offset: 0x00000DCC
		public override void UpdateSettings()
		{
			if (this.settings == null || this.feature == null)
			{
				return;
			}
			Blit renderObjectsFeature = this.feature as Blit;
			if (renderObjectsFeature != null)
			{
				bool flag = renderObjectsFeature.settings.blitMaterial != this.settings.blitMaterial;
				renderObjectsFeature.settings = this.settings;
				if (flag && this.feature.isActive)
				{
					this.DisableFeature();
					this.EnableFeature();
				}
			}
			base.UpdateSettings();
		}

		// Token: 0x04000028 RID: 40
		public Blit.BlitSettings settings = new Blit.BlitSettings();
	}
}

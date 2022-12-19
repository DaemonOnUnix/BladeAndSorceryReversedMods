using System;
using UnityEngine;

namespace ModularFirearms
{
	// Token: 0x0200000B RID: 11
	public class TextureProcessor
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000039C8 File Offset: 0x00001BC8
		private Color[,] GetPixelColorArray(int digit, Texture2D baseTexture, int x_size, int y_size, int overflowIndex = 7, int verticalBufferPx = 256)
		{
			digit %= 10;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (digit > overflowIndex)
			{
				num3 = verticalBufferPx;
			}
			Color[,] array = new Color[x_size, y_size];
			for (int i = 0; i < 256; i++)
			{
				for (int j = 128 * (digit % (overflowIndex + 1)); j < 128 + 128 * (digit % (overflowIndex + 1)) - 1; j++)
				{
					array[num, num2] = baseTexture.GetPixel(j, i + num3);
					num++;
				}
				num = 0;
				num2++;
			}
			return array;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003A53 File Offset: 0x00001C53
		public void SetTargetRenderer(Renderer newRenderer)
		{
			this.outputMesh = newRenderer;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003A5C File Offset: 0x00001C5C
		public void SetGridTexture(Texture2D newDigitGrid)
		{
			this.baseTexture = newDigitGrid;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003A65 File Offset: 0x00001C65
		public void DisplayUpdate(int displayValue)
		{
			this.RenderToMesh(this.outputMesh, this.GetNumberTexture(displayValue, 128, 256));
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003A84 File Offset: 0x00001C84
		protected Texture2D GetNumberTexture(int numberValue, int digit_size_x = 128, int digit_size_y = 256)
		{
			int num = numberValue % 10;
			int num2 = numberValue / 10 % 10;
			this.pixelColorsOnes = this.GetPixelColorArray(num, this.baseTexture, digit_size_x, digit_size_y, 7, 256);
			this.pixelColorsTens = this.GetPixelColorArray(num2, this.baseTexture, digit_size_x, digit_size_y, 7, 256);
			this.outputTexture = new Texture2D(digit_size_x * 2, digit_size_y);
			for (int i = 0; i < this.outputTexture.height; i++)
			{
				for (int j = 0; j < digit_size_x; j++)
				{
					if (i <= this.pixelColorsTens.GetUpperBound(1) && j <= this.pixelColorsTens.GetUpperBound(0))
					{
						this.outputTexture.SetPixel(j, i, this.pixelColorsTens[j, i]);
					}
					else
					{
						this.outputTexture.SetPixel(j, i, new Color(0f, 0f, 0f, 0f));
					}
				}
				for (int k = digit_size_x; k < digit_size_x * 2; k++)
				{
					if (i <= this.pixelColorsOnes.GetUpperBound(1) && k - digit_size_x <= this.pixelColorsOnes.GetUpperBound(0))
					{
						this.outputTexture.SetPixel(k, i, this.pixelColorsOnes[k - digit_size_x, i]);
					}
					else
					{
						this.outputTexture.SetPixel(k, i, new Color(0f, 0f, 0f, 0f));
					}
				}
			}
			this.outputTexture.Apply();
			return this.outputTexture;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003BF6 File Offset: 0x00001DF6
		protected void RenderToMesh(Renderer r, Texture2D t)
		{
			r.material.mainTexture = t;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003C04 File Offset: 0x00001E04
		protected Texture2D CurrentDigitTexture()
		{
			return this.outputTexture;
		}

		// Token: 0x0400002B RID: 43
		public Renderer outputMesh;

		// Token: 0x0400002C RID: 44
		public Texture2D baseTexture;

		// Token: 0x0400002D RID: 45
		private Texture2D outputTexture;

		// Token: 0x0400002E RID: 46
		private Color[,] pixelColorsOnes;

		// Token: 0x0400002F RID: 47
		private Color[,] pixelColorsTens;
	}
}

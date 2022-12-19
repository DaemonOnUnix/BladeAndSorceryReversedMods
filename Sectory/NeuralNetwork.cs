using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sectory
{
	// Token: 0x0200001B RID: 27
	public class NeuralNetwork
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00003738 File Offset: 0x00001938
		public NeuralNetwork(int[] layers)
		{
			this.layers = new int[layers.Length];
			for (int i = 0; i < layers.Length; i++)
			{
				this.layers[i] = layers[i];
			}
			this.InitNeurons();
			this.InitBiases();
			this.InitWeights();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003790 File Offset: 0x00001990
		public NeuralNetwork Copy(NeuralNetwork nn)
		{
			for (int i = 0; i < this.biases.Length; i++)
			{
				for (int j = 0; j < this.biases[i].Length; j++)
				{
					nn.biases[i][j] = this.biases[i][j];
				}
			}
			for (int k = 0; k < this.weights.Length; k++)
			{
				for (int l = 0; l < this.weights[k].Length; l++)
				{
					for (int m = 0; m < this.weights[k][l].Length; m++)
					{
						nn.weights[k][l][m] = this.weights[k][l][m];
					}
				}
			}
			return nn;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003874 File Offset: 0x00001A74
		private void InitNeurons()
		{
			List<float[]> list = new List<float[]>();
			for (int i = 0; i < this.layers.Length; i++)
			{
				list.Add(new float[this.layers[i]]);
			}
			this.neurons = list.ToArray();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000038C4 File Offset: 0x00001AC4
		private void InitBiases()
		{
			List<float[]> list = new List<float[]>();
			for (int i = 0; i < this.layers.Length; i++)
			{
				float[] array = new float[this.layers[i]];
				for (int j = 0; j < this.layers[i]; j++)
				{
					array[j] = Random.Range(-0.5f, 0.5f);
				}
				list.Add(array);
			}
			this.biases = list.ToArray();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003944 File Offset: 0x00001B44
		private void InitWeights()
		{
			List<float[][]> list = new List<float[][]>();
			for (int i = 1; i < this.layers.Length; i++)
			{
				List<float[]> list2 = new List<float[]>();
				int num = this.layers[i - 1];
				for (int j = 0; j < this.neurons[i].Length; j++)
				{
					float[] array = new float[num];
					for (int k = 0; k < num; k++)
					{
						array[k] = Random.Range(-0.5f, 0.5f);
					}
					list2.Add(array);
				}
				list.Add(list2.ToArray());
			}
			this.weights = list.ToArray();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003A04 File Offset: 0x00001C04
		public float[] FeedForward(float[] inputs)
		{
			bool flag = this.weights == null;
			if (flag)
			{
				Debug.Log(this.weights);
			}
			bool flag2 = this.biases == null;
			if (flag2)
			{
				Debug.Log(this.biases);
			}
			bool flag3 = this.layers == null;
			if (flag3)
			{
				Debug.Log(this.layers);
			}
			for (int i = 0; i < inputs.Length; i++)
			{
				this.neurons[0][i] = inputs[i];
			}
			for (int j = 1; j < this.layers.Length; j++)
			{
				int num = j - 1;
				for (int k = 0; k < this.neurons[j].Length; k++)
				{
					float num2 = 0f;
					for (int l = 0; l < this.neurons[j - 1].Length; l++)
					{
						num2 += this.weights[j - 1][k][l] * this.neurons[j - 1][l];
					}
					this.neurons[j][k] = this.Activation(num2 + this.biases[j][k]);
				}
			}
			return this.neurons[this.neurons.Length - 1];
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003B58 File Offset: 0x00001D58
		public void Mutate(int chance, float val)
		{
			for (int i = 0; i < this.biases.Length; i++)
			{
				for (int j = 0; j < this.biases[i].Length; j++)
				{
					this.biases[i][j] = ((Random.Range(0f, 100f) <= (float)chance) ? (this.biases[i][j] += Random.Range(-val, val)) : this.biases[i][j]);
				}
			}
			for (int k = 0; k < this.weights.Length; k++)
			{
				for (int l = 0; l < this.weights[k].Length; l++)
				{
					for (int m = 0; m < this.weights[k][l].Length; m++)
					{
						this.weights[k][l][m] = ((Random.Range(0f, 100f) <= (float)chance) ? (this.weights[k][l][m] += Random.Range(-val, val)) : this.weights[k][l][m]);
					}
				}
			}
			this.fitness = 0f;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003CB4 File Offset: 0x00001EB4
		public float Activation(float value)
		{
			return (float)Math.Tanh((double)value);
		}

		// Token: 0x04000096 RID: 150
		public int[] layers;

		// Token: 0x04000097 RID: 151
		public float[][] neurons;

		// Token: 0x04000098 RID: 152
		public float[][] biases;

		// Token: 0x04000099 RID: 153
		public float[][][] weights;

		// Token: 0x0400009A RID: 154
		public float fitness;
	}
}

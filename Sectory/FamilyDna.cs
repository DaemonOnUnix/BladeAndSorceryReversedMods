using System;

namespace Sectory
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class FamilyDna
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002630 File Offset: 0x00000830
		public void Init(string familyName, NeuralNetwork network = null)
		{
			this.familyName = familyName;
			this.fitness = 0f;
			bool flag = network == null;
			if (flag)
			{
				network = new NeuralNetwork(Entry.GetNNInit);
			}
			this.layers = network.layers;
			this.biases = network.biases;
			this.weights = network.weights;
		}

		// Token: 0x0400002C RID: 44
		public string familyName;

		// Token: 0x0400002D RID: 45
		public float fitness;

		// Token: 0x0400002E RID: 46
		public int[] layers;

		// Token: 0x0400002F RID: 47
		public float[][] biases;

		// Token: 0x04000030 RID: 48
		public float[][][] weights;
	}
}

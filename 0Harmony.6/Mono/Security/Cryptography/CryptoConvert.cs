using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	// Token: 0x020000B4 RID: 180
	internal static class CryptoConvert
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x0001195B File Offset: 0x0000FB5B
		private static int ToInt32LE(byte[] bytes, int offset)
		{
			return ((int)bytes[offset + 3] << 24) | ((int)bytes[offset + 2] << 16) | ((int)bytes[offset + 1] << 8) | (int)bytes[offset];
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001195B File Offset: 0x0000FB5B
		private static uint ToUInt32LE(byte[] bytes, int offset)
		{
			return (uint)(((int)bytes[offset + 3] << 24) | ((int)bytes[offset + 2] << 16) | ((int)bytes[offset + 1] << 8) | (int)bytes[offset]);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001197A File Offset: 0x0000FB7A
		private static byte[] GetBytesLE(int val)
		{
			return new byte[]
			{
				(byte)(val & 255),
				(byte)((val >> 8) & 255),
				(byte)((val >> 16) & 255),
				(byte)((val >> 24) & 255)
			};
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x000119B8 File Offset: 0x0000FBB8
		private static byte[] Trim(byte[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 0)
				{
					byte[] array2 = new byte[array.Length - i];
					Buffer.BlockCopy(array, i, array2, 0, array2.Length);
					return array2;
				}
			}
			return null;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x000119F4 File Offset: 0x0000FBF4
		private static RSA FromCapiPrivateKeyBlob(byte[] blob, int offset)
		{
			RSAParameters rsaparameters = default(RSAParameters);
			try
			{
				if (blob[offset] != 7 || blob[offset + 1] != 2 || blob[offset + 2] != 0 || blob[offset + 3] != 0 || CryptoConvert.ToUInt32LE(blob, offset + 8) != 843141970U)
				{
					throw new CryptographicException("Invalid blob header");
				}
				int num = CryptoConvert.ToInt32LE(blob, offset + 12);
				byte[] array = new byte[4];
				Buffer.BlockCopy(blob, offset + 16, array, 0, 4);
				Array.Reverse(array);
				rsaparameters.Exponent = CryptoConvert.Trim(array);
				int num2 = offset + 20;
				int num3 = num >> 3;
				rsaparameters.Modulus = new byte[num3];
				Buffer.BlockCopy(blob, num2, rsaparameters.Modulus, 0, num3);
				Array.Reverse(rsaparameters.Modulus);
				num2 += num3;
				int num4 = num3 >> 1;
				rsaparameters.P = new byte[num4];
				Buffer.BlockCopy(blob, num2, rsaparameters.P, 0, num4);
				Array.Reverse(rsaparameters.P);
				num2 += num4;
				rsaparameters.Q = new byte[num4];
				Buffer.BlockCopy(blob, num2, rsaparameters.Q, 0, num4);
				Array.Reverse(rsaparameters.Q);
				num2 += num4;
				rsaparameters.DP = new byte[num4];
				Buffer.BlockCopy(blob, num2, rsaparameters.DP, 0, num4);
				Array.Reverse(rsaparameters.DP);
				num2 += num4;
				rsaparameters.DQ = new byte[num4];
				Buffer.BlockCopy(blob, num2, rsaparameters.DQ, 0, num4);
				Array.Reverse(rsaparameters.DQ);
				num2 += num4;
				rsaparameters.InverseQ = new byte[num4];
				Buffer.BlockCopy(blob, num2, rsaparameters.InverseQ, 0, num4);
				Array.Reverse(rsaparameters.InverseQ);
				num2 += num4;
				rsaparameters.D = new byte[num3];
				if (num2 + num3 + offset <= blob.Length)
				{
					Buffer.BlockCopy(blob, num2, rsaparameters.D, 0, num3);
					Array.Reverse(rsaparameters.D);
				}
			}
			catch (Exception ex)
			{
				throw new CryptographicException("Invalid blob.", ex);
			}
			RSA rsa = null;
			try
			{
				rsa = RSA.Create();
				rsa.ImportParameters(rsaparameters);
			}
			catch (CryptographicException)
			{
				bool flag = false;
				try
				{
					rsa = new RSACryptoServiceProvider(new CspParameters
					{
						Flags = CspProviderFlags.UseMachineKeyStore
					});
					rsa.ImportParameters(rsaparameters);
				}
				catch
				{
					flag = true;
				}
				if (flag)
				{
					throw;
				}
			}
			return rsa;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00011C64 File Offset: 0x0000FE64
		private static RSA FromCapiPublicKeyBlob(byte[] blob, int offset)
		{
			RSA rsa2;
			try
			{
				if (blob[offset] != 6 || blob[offset + 1] != 2 || blob[offset + 2] != 0 || blob[offset + 3] != 0 || CryptoConvert.ToUInt32LE(blob, offset + 8) != 826364754U)
				{
					throw new CryptographicException("Invalid blob header");
				}
				int num = CryptoConvert.ToInt32LE(blob, offset + 12);
				RSAParameters rsaparameters = new RSAParameters
				{
					Exponent = new byte[3]
				};
				rsaparameters.Exponent[0] = blob[offset + 18];
				rsaparameters.Exponent[1] = blob[offset + 17];
				rsaparameters.Exponent[2] = blob[offset + 16];
				int num2 = offset + 20;
				int num3 = num >> 3;
				rsaparameters.Modulus = new byte[num3];
				Buffer.BlockCopy(blob, num2, rsaparameters.Modulus, 0, num3);
				Array.Reverse(rsaparameters.Modulus);
				RSA rsa = null;
				try
				{
					rsa = RSA.Create();
					rsa.ImportParameters(rsaparameters);
				}
				catch (CryptographicException)
				{
					rsa = new RSACryptoServiceProvider(new CspParameters
					{
						Flags = CspProviderFlags.UseMachineKeyStore
					});
					rsa.ImportParameters(rsaparameters);
				}
				rsa2 = rsa;
			}
			catch (Exception ex)
			{
				throw new CryptographicException("Invalid blob.", ex);
			}
			return rsa2;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00011D80 File Offset: 0x0000FF80
		public static RSA FromCapiKeyBlob(byte[] blob)
		{
			return CryptoConvert.FromCapiKeyBlob(blob, 0);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00011D8C File Offset: 0x0000FF8C
		public static RSA FromCapiKeyBlob(byte[] blob, int offset)
		{
			if (blob == null)
			{
				throw new ArgumentNullException("blob");
			}
			if (offset >= blob.Length)
			{
				throw new ArgumentException("blob is too small.");
			}
			byte b = blob[offset];
			if (b != 0)
			{
				if (b == 6)
				{
					return CryptoConvert.FromCapiPublicKeyBlob(blob, offset);
				}
				if (b == 7)
				{
					return CryptoConvert.FromCapiPrivateKeyBlob(blob, offset);
				}
			}
			else if (blob[offset + 12] == 6)
			{
				return CryptoConvert.FromCapiPublicKeyBlob(blob, offset + 12);
			}
			throw new CryptographicException("Unknown blob format.");
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00011DF8 File Offset: 0x0000FFF8
		public static byte[] ToCapiPublicKeyBlob(RSA rsa)
		{
			RSAParameters rsaparameters = rsa.ExportParameters(false);
			int num = rsaparameters.Modulus.Length;
			byte[] array = new byte[20 + num];
			array[0] = 6;
			array[1] = 2;
			array[5] = 36;
			array[8] = 82;
			array[9] = 83;
			array[10] = 65;
			array[11] = 49;
			byte[] bytesLE = CryptoConvert.GetBytesLE(num << 3);
			array[12] = bytesLE[0];
			array[13] = bytesLE[1];
			array[14] = bytesLE[2];
			array[15] = bytesLE[3];
			int num2 = 16;
			int i = rsaparameters.Exponent.Length;
			while (i > 0)
			{
				array[num2++] = rsaparameters.Exponent[--i];
			}
			num2 = 20;
			byte[] modulus = rsaparameters.Modulus;
			int num3 = modulus.Length;
			Array.Reverse(modulus, 0, num3);
			Buffer.BlockCopy(modulus, 0, array, num2, num3);
			num2 += num3;
			return array;
		}
	}
}

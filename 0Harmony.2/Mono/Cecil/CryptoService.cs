using System;
using System.IO;
using System.Security.Cryptography;
using Mono.Cecil.PE;
using Mono.Security.Cryptography;

namespace Mono.Cecil
{
	// Token: 0x02000285 RID: 645
	internal static class CryptoService
	{
		// Token: 0x06001029 RID: 4137 RVA: 0x00032D90 File Offset: 0x00030F90
		public static byte[] GetPublicKey(WriterParameters parameters)
		{
			byte[] array3;
			using (RSA rsa = parameters.CreateRSA())
			{
				byte[] array = CryptoConvert.ToCapiPublicKeyBlob(rsa);
				byte[] array2 = new byte[12 + array.Length];
				Buffer.BlockCopy(array, 0, array2, 12, array.Length);
				array2[1] = 36;
				array2[4] = 4;
				array2[5] = 128;
				array2[8] = (byte)array.Length;
				array2[9] = (byte)(array.Length >> 8);
				array2[10] = (byte)(array.Length >> 16);
				array2[11] = (byte)(array.Length >> 24);
				array3 = array2;
			}
			return array3;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00032E1C File Offset: 0x0003101C
		public static void StrongName(Stream stream, ImageWriter writer, WriterParameters parameters)
		{
			int num;
			byte[] array = CryptoService.CreateStrongName(parameters, CryptoService.HashStream(stream, writer, out num));
			CryptoService.PatchStrongName(stream, num, array);
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00032E41 File Offset: 0x00031041
		private static void PatchStrongName(Stream stream, int strong_name_pointer, byte[] strong_name)
		{
			stream.Seek((long)strong_name_pointer, SeekOrigin.Begin);
			stream.Write(strong_name, 0, strong_name.Length);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x00032E58 File Offset: 0x00031058
		private static byte[] CreateStrongName(WriterParameters parameters, byte[] hash)
		{
			byte[] array2;
			using (RSA rsa = parameters.CreateRSA())
			{
				RSAPKCS1SignatureFormatter rsapkcs1SignatureFormatter = new RSAPKCS1SignatureFormatter(rsa);
				rsapkcs1SignatureFormatter.SetHashAlgorithm("SHA1");
				byte[] array = rsapkcs1SignatureFormatter.CreateSignature(hash);
				Array.Reverse(array);
				array2 = array;
			}
			return array2;
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00032EA8 File Offset: 0x000310A8
		private static byte[] HashStream(Stream stream, ImageWriter writer, out int strong_name_pointer)
		{
			Section text = writer.text;
			int headerSize = (int)writer.GetHeaderSize();
			int pointerToRawData = (int)text.PointerToRawData;
			DataDirectory strongNameSignatureDirectory = writer.GetStrongNameSignatureDirectory();
			if (strongNameSignatureDirectory.Size == 0U)
			{
				throw new InvalidOperationException();
			}
			strong_name_pointer = (int)((long)pointerToRawData + (long)((ulong)(strongNameSignatureDirectory.VirtualAddress - text.VirtualAddress)));
			int size = (int)strongNameSignatureDirectory.Size;
			SHA1Managed sha1Managed = new SHA1Managed();
			byte[] array = new byte[8192];
			using (CryptoStream cryptoStream = new CryptoStream(Stream.Null, sha1Managed, CryptoStreamMode.Write))
			{
				stream.Seek(0L, SeekOrigin.Begin);
				CryptoService.CopyStreamChunk(stream, cryptoStream, array, headerSize);
				stream.Seek((long)pointerToRawData, SeekOrigin.Begin);
				CryptoService.CopyStreamChunk(stream, cryptoStream, array, strong_name_pointer - pointerToRawData);
				stream.Seek((long)size, SeekOrigin.Current);
				CryptoService.CopyStreamChunk(stream, cryptoStream, array, (int)(stream.Length - (long)(strong_name_pointer + size)));
			}
			return sha1Managed.Hash;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00032F94 File Offset: 0x00031194
		private static void CopyStreamChunk(Stream stream, Stream dest_stream, byte[] buffer, int length)
		{
			while (length > 0)
			{
				int num = stream.Read(buffer, 0, Math.Min(buffer.Length, length));
				dest_stream.Write(buffer, 0, num);
				length -= num;
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00032FC8 File Offset: 0x000311C8
		public static byte[] ComputeHash(string file)
		{
			if (!File.Exists(file))
			{
				return Empty<byte>.Array;
			}
			byte[] array;
			using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = CryptoService.ComputeHash(fileStream);
			}
			return array;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00033014 File Offset: 0x00031214
		public static byte[] ComputeHash(Stream stream)
		{
			SHA1Managed sha1Managed = new SHA1Managed();
			byte[] array = new byte[8192];
			using (CryptoStream cryptoStream = new CryptoStream(Stream.Null, sha1Managed, CryptoStreamMode.Write))
			{
				CryptoService.CopyStreamChunk(stream, cryptoStream, array, (int)stream.Length);
			}
			return sha1Managed.Hash;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00033070 File Offset: 0x00031270
		public static byte[] ComputeHash(params ByteBuffer[] buffers)
		{
			SHA1Managed sha1Managed = new SHA1Managed();
			using (CryptoStream cryptoStream = new CryptoStream(Stream.Null, sha1Managed, CryptoStreamMode.Write))
			{
				for (int i = 0; i < buffers.Length; i++)
				{
					cryptoStream.Write(buffers[i].buffer, 0, buffers[i].length);
				}
			}
			return sha1Managed.Hash;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x000330D8 File Offset: 0x000312D8
		public static Guid ComputeGuid(byte[] hash)
		{
			byte[] array = new byte[16];
			Buffer.BlockCopy(hash, 0, array, 0, 16);
			array[7] = (array[7] & 15) | 64;
			array[8] = (array[8] & 63) | 128;
			return new Guid(array);
		}
	}
}

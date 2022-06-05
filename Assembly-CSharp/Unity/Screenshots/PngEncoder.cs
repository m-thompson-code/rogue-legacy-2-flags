using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace Unity.Screenshots
{
	// Token: 0x02000D0B RID: 3339
	public static class PngEncoder
	{
		// Token: 0x06005F24 RID: 24356 RVA: 0x00164924 File Offset: 0x00162B24
		private static uint Adler32(byte[] bytes)
		{
			uint num = 1U;
			uint num2 = 0U;
			foreach (byte b in bytes)
			{
				num = (num + (uint)b) % 65521U;
				num2 = (num2 + num) % 65521U;
			}
			return num2 << 16 | num;
		}

		// Token: 0x06005F25 RID: 24357 RVA: 0x000346CA File Offset: 0x000328CA
		private static void AppendByte(this byte[] data, ref int position, byte value)
		{
			data[position] = value;
			position++;
		}

		// Token: 0x06005F26 RID: 24358 RVA: 0x00164968 File Offset: 0x00162B68
		private static void AppendBytes(this byte[] data, ref int position, byte[] value)
		{
			foreach (byte value2 in value)
			{
				data.AppendByte(ref position, value2);
			}
		}

		// Token: 0x06005F27 RID: 24359 RVA: 0x00164994 File Offset: 0x00162B94
		private static void AppendChunk(this byte[] data, ref int position, string chunkType, byte[] chunkData)
		{
			byte[] chunkTypeBytes = PngEncoder.GetChunkTypeBytes(chunkType);
			if (chunkTypeBytes != null)
			{
				data.AppendInt(ref position, chunkData.Length);
				data.AppendBytes(ref position, chunkTypeBytes);
				data.AppendBytes(ref position, chunkData);
				data.AppendUInt(ref position, PngEncoder.GetChunkCrc(chunkTypeBytes, chunkData));
			}
		}

		// Token: 0x06005F28 RID: 24360 RVA: 0x001649D4 File Offset: 0x00162BD4
		private static void AppendInt(this byte[] data, ref int position, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			data.AppendBytes(ref position, bytes);
		}

		// Token: 0x06005F29 RID: 24361 RVA: 0x00164A00 File Offset: 0x00162C00
		private static void AppendUInt(this byte[] data, ref int position, uint value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			data.AppendBytes(ref position, bytes);
		}

		// Token: 0x06005F2A RID: 24362 RVA: 0x00164A2C File Offset: 0x00162C2C
		private static byte[] Compress(byte[] bytes)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
				{
					using (MemoryStream memoryStream2 = new MemoryStream(bytes))
					{
						memoryStream2.WriteTo(deflateStream);
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06005F2B RID: 24363 RVA: 0x00164AA8 File Offset: 0x00162CA8
		public static byte[] Encode(byte[] dataRgba, int stride)
		{
			if (dataRgba == null)
			{
				throw new ArgumentNullException("dataRgba");
			}
			if (dataRgba.Length == 0)
			{
				throw new ArgumentException("The data length must be greater than 0.");
			}
			if (stride == 0)
			{
				throw new ArgumentException("The stride must be greater than 0.");
			}
			if (stride % 4 != 0)
			{
				throw new ArgumentException("The stride must be evenly divisible by 4.");
			}
			if (dataRgba.Length % 4 != 0)
			{
				throw new ArgumentException("The data must be evenly divisible by 4.");
			}
			if (dataRgba.Length % stride != 0)
			{
				throw new ArgumentException("The data must be evenly divisible by the stride.");
			}
			int num = dataRgba.Length / 4;
			int num2 = stride / 4;
			int num3 = num / num2;
			byte[] array = new byte[13];
			int num4 = 0;
			array.AppendInt(ref num4, num2);
			array.AppendInt(ref num4, num3);
			array.AppendByte(ref num4, 8);
			array.AppendByte(ref num4, 6);
			array.AppendByte(ref num4, 0);
			array.AppendByte(ref num4, 0);
			array.AppendByte(ref num4, 0);
			byte[] array2 = new byte[dataRgba.Length + num3];
			int num5 = 0;
			int num6 = 0;
			for (int i = 0; i < dataRgba.Length; i++)
			{
				if (num6 >= stride)
				{
					num6 = 0;
				}
				if (num6 == 0)
				{
					array2.AppendByte(ref num5, 0);
				}
				array2.AppendByte(ref num5, dataRgba[i]);
				num6++;
			}
			byte[] array3 = PngEncoder.Compress(array2);
			byte[] array4 = new byte[2 + array3.Length + 4];
			int num7 = 0;
			array4.AppendByte(ref num7, 120);
			array4.AppendByte(ref num7, 156);
			array4.AppendBytes(ref num7, array3);
			array4.AppendUInt(ref num7, PngEncoder.Adler32(array2));
			byte[] array5 = new byte[8 + array.Length + 12 + array4.Length + 12 + 12];
			int num8 = 0;
			array5.AppendByte(ref num8, 137);
			array5.AppendByte(ref num8, 80);
			array5.AppendByte(ref num8, 78);
			array5.AppendByte(ref num8, 71);
			array5.AppendByte(ref num8, 13);
			array5.AppendByte(ref num8, 10);
			array5.AppendByte(ref num8, 26);
			array5.AppendByte(ref num8, 10);
			array5.AppendChunk(ref num8, "IHDR", array);
			array5.AppendChunk(ref num8, "IDAT", array4);
			array5.AppendChunk(ref num8, "IEND", new byte[0]);
			return array5;
		}

		// Token: 0x06005F2C RID: 24364 RVA: 0x000346D7 File Offset: 0x000328D7
		public static void EncodeAsync(byte[] dataRgba, int stride, Action<Exception, byte[]> callback)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				try
				{
					byte[] arg = PngEncoder.Encode(dataRgba, stride);
					callback(null, arg);
				}
				catch (Exception arg2)
				{
					callback(arg2, null);
					throw;
				}
			}, null);
		}

		// Token: 0x06005F2D RID: 24365 RVA: 0x00164CA4 File Offset: 0x00162EA4
		private static uint GetChunkCrc(byte[] chunkTypeBytes, byte[] chunkData)
		{
			byte[] array = new byte[chunkTypeBytes.Length + chunkData.Length];
			Array.Copy(chunkTypeBytes, 0, array, 0, chunkTypeBytes.Length);
			Array.Copy(chunkData, 0, array, 4, chunkData.Length);
			return PngEncoder.crc32.Calculate<byte>(array);
		}

		// Token: 0x06005F2E RID: 24366 RVA: 0x00164CE4 File Offset: 0x00162EE4
		private static byte[] GetChunkTypeBytes(string value)
		{
			char[] array = value.ToCharArray();
			if (array.Length < 4)
			{
				return null;
			}
			byte[] array2 = new byte[4];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (byte)array[i];
			}
			return array2;
		}

		// Token: 0x04004E30 RID: 20016
		private static PngEncoder.Crc32 crc32 = new PngEncoder.Crc32();

		// Token: 0x02000D0C RID: 3340
		public class Crc32
		{
			// Token: 0x06005F2F RID: 24367 RVA: 0x00164D20 File Offset: 0x00162F20
			public Crc32()
			{
				this.checksumTable = Enumerable.Range(0, 256).Select(delegate(int i)
				{
					uint num = (uint)i;
					for (int j = 0; j < 8; j++)
					{
						num = (((num & 1U) != 0U) ? (PngEncoder.Crc32.generator ^ num >> 1) : (num >> 1));
					}
					return num;
				}).ToArray<uint>();
			}

			// Token: 0x06005F30 RID: 24368 RVA: 0x00034705 File Offset: 0x00032905
			public uint Calculate<T>(IEnumerable<T> byteStream)
			{
				return ~byteStream.Aggregate(uint.MaxValue, (uint checksumRegister, T currentByte) => this.checksumTable[(int)((checksumRegister & 255U) ^ (uint)Convert.ToByte(currentByte))] ^ checksumRegister >> 8);
			}

			// Token: 0x04004E31 RID: 20017
			private static uint generator = 3988292384U;

			// Token: 0x04004E32 RID: 20018
			private readonly uint[] checksumTable;
		}
	}
}

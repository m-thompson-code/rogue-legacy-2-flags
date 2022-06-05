using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace Unity.Screenshots
{
	// Token: 0x0200082C RID: 2092
	public static class PngEncoder
	{
		// Token: 0x0600452B RID: 17707 RVA: 0x000F6E0C File Offset: 0x000F500C
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

		// Token: 0x0600452C RID: 17708 RVA: 0x000F6E4D File Offset: 0x000F504D
		private static void AppendByte(this byte[] data, ref int position, byte value)
		{
			data[position] = value;
			position++;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x000F6E5C File Offset: 0x000F505C
		private static void AppendBytes(this byte[] data, ref int position, byte[] value)
		{
			foreach (byte value2 in value)
			{
				data.AppendByte(ref position, value2);
			}
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x000F6E88 File Offset: 0x000F5088
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

		// Token: 0x0600452F RID: 17711 RVA: 0x000F6EC8 File Offset: 0x000F50C8
		private static void AppendInt(this byte[] data, ref int position, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			data.AppendBytes(ref position, bytes);
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x000F6EF4 File Offset: 0x000F50F4
		private static void AppendUInt(this byte[] data, ref int position, uint value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			data.AppendBytes(ref position, bytes);
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x000F6F20 File Offset: 0x000F5120
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

		// Token: 0x06004532 RID: 17714 RVA: 0x000F6F9C File Offset: 0x000F519C
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

		// Token: 0x06004533 RID: 17715 RVA: 0x000F7197 File Offset: 0x000F5397
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

		// Token: 0x06004534 RID: 17716 RVA: 0x000F71C8 File Offset: 0x000F53C8
		private static uint GetChunkCrc(byte[] chunkTypeBytes, byte[] chunkData)
		{
			byte[] array = new byte[chunkTypeBytes.Length + chunkData.Length];
			Array.Copy(chunkTypeBytes, 0, array, 0, chunkTypeBytes.Length);
			Array.Copy(chunkData, 0, array, 4, chunkData.Length);
			return PngEncoder.crc32.Calculate<byte>(array);
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x000F7208 File Offset: 0x000F5408
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

		// Token: 0x04003B0E RID: 15118
		private static PngEncoder.Crc32 crc32 = new PngEncoder.Crc32();

		// Token: 0x02000E51 RID: 3665
		public class Crc32
		{
			// Token: 0x06006C3D RID: 27709 RVA: 0x00193B68 File Offset: 0x00191D68
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

			// Token: 0x06006C3E RID: 27710 RVA: 0x00193BB5 File Offset: 0x00191DB5
			public uint Calculate<T>(IEnumerable<T> byteStream)
			{
				return ~byteStream.Aggregate(uint.MaxValue, (uint checksumRegister, T currentByte) => this.checksumTable[(int)((checksumRegister & 255U) ^ (uint)Convert.ToByte(currentByte))] ^ checksumRegister >> 8);
			}

			// Token: 0x04005798 RID: 22424
			private static uint generator = 3988292384U;

			// Token: 0x04005799 RID: 22425
			private readonly uint[] checksumTable;
		}
	}
}

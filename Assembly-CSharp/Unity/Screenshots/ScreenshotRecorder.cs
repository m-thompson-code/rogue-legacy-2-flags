using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Unity.Screenshots
{
	// Token: 0x02000D11 RID: 3345
	public class ScreenshotRecorder
	{
		// Token: 0x06005F51 RID: 24401 RVA: 0x00034848 File Offset: 0x00032A48
		public ScreenshotRecorder()
		{
			this.operationPool = new List<ScreenshotRecorder.ScreenshotOperation>();
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x00165090 File Offset: 0x00163290
		private ScreenshotRecorder.ScreenshotOperation GetOperation()
		{
			foreach (ScreenshotRecorder.ScreenshotOperation screenshotOperation in this.operationPool)
			{
				if (!screenshotOperation.IsInUse)
				{
					screenshotOperation.IsInUse = true;
					return screenshotOperation;
				}
			}
			ScreenshotRecorder.ScreenshotOperation screenshotOperation2 = new ScreenshotRecorder.ScreenshotOperation();
			screenshotOperation2.IsInUse = true;
			this.operationPool.Add(screenshotOperation2);
			return screenshotOperation2;
		}

		// Token: 0x06005F53 RID: 24403 RVA: 0x0016510C File Offset: 0x0016330C
		public void Screenshot(int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			Texture2D source = ScreenCapture.CaptureScreenshotAsTexture();
			this.Screenshot(source, maximumWidth, maximumHeight, type, callback, state);
		}

		// Token: 0x06005F54 RID: 24404 RVA: 0x00165130 File Offset: 0x00163330
		public void Screenshot(Camera source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			RenderTexture renderTexture = new RenderTexture(maximumWidth, maximumHeight, 24);
			RenderTexture targetTexture = source.targetTexture;
			source.targetTexture = renderTexture;
			source.Render();
			source.targetTexture = targetTexture;
			this.Screenshot(renderTexture, maximumWidth, maximumHeight, type, callback, state);
		}

		// Token: 0x06005F55 RID: 24405 RVA: 0x0003485B File Offset: 0x00032A5B
		public void Screenshot(RenderTexture source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			this.ScreenshotInternal(source, maximumWidth, maximumHeight, type, callback, state);
		}

		// Token: 0x06005F56 RID: 24406 RVA: 0x0003485B File Offset: 0x00032A5B
		public void Screenshot(Texture2D source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			this.ScreenshotInternal(source, maximumWidth, maximumHeight, type, callback, state);
		}

		// Token: 0x06005F57 RID: 24407 RVA: 0x00165174 File Offset: 0x00163374
		private void ScreenshotInternal(Texture source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			ScreenshotRecorder.ScreenshotOperation operation = this.GetOperation();
			operation.Identifier = ScreenshotRecorder.nextIdentifier++;
			operation.Source = source;
			operation.MaximumWidth = maximumWidth;
			operation.MaximumHeight = maximumHeight;
			operation.Type = type;
			operation.Callback = callback;
			operation.State = state;
			AsyncGPUReadback.Request(source, 0, TextureFormat.RGBA32, operation.ScreenshotCallbackDelegate);
		}

		// Token: 0x04004E44 RID: 20036
		private static int nextIdentifier;

		// Token: 0x04004E45 RID: 20037
		private List<ScreenshotRecorder.ScreenshotOperation> operationPool;

		// Token: 0x02000D12 RID: 3346
		private class ScreenshotOperation
		{
			// Token: 0x06005F58 RID: 24408 RVA: 0x0003486C File Offset: 0x00032A6C
			public ScreenshotOperation()
			{
				this.ScreenshotCallbackDelegate = new Action<AsyncGPUReadbackRequest>(this.ScreenshotCallback);
				this.EncodeCallbackDelegate = new WaitCallback(this.EncodeCallback);
			}

			// Token: 0x17001F3A RID: 7994
			// (get) Token: 0x06005F59 RID: 24409 RVA: 0x00034898 File Offset: 0x00032A98
			// (set) Token: 0x06005F5A RID: 24410 RVA: 0x000348A0 File Offset: 0x00032AA0
			public Action<byte[], object> Callback { get; set; }

			// Token: 0x17001F3B RID: 7995
			// (get) Token: 0x06005F5B RID: 24411 RVA: 0x000348A9 File Offset: 0x00032AA9
			// (set) Token: 0x06005F5C RID: 24412 RVA: 0x000348B1 File Offset: 0x00032AB1
			public int Height { get; set; }

			// Token: 0x17001F3C RID: 7996
			// (get) Token: 0x06005F5D RID: 24413 RVA: 0x000348BA File Offset: 0x00032ABA
			// (set) Token: 0x06005F5E RID: 24414 RVA: 0x000348C2 File Offset: 0x00032AC2
			public int Identifier { get; set; }

			// Token: 0x17001F3D RID: 7997
			// (get) Token: 0x06005F5F RID: 24415 RVA: 0x000348CB File Offset: 0x00032ACB
			// (set) Token: 0x06005F60 RID: 24416 RVA: 0x000348D3 File Offset: 0x00032AD3
			public bool IsInUse { get; set; }

			// Token: 0x17001F3E RID: 7998
			// (get) Token: 0x06005F61 RID: 24417 RVA: 0x000348DC File Offset: 0x00032ADC
			// (set) Token: 0x06005F62 RID: 24418 RVA: 0x000348E4 File Offset: 0x00032AE4
			public int MaximumHeight { get; set; }

			// Token: 0x17001F3F RID: 7999
			// (get) Token: 0x06005F63 RID: 24419 RVA: 0x000348ED File Offset: 0x00032AED
			// (set) Token: 0x06005F64 RID: 24420 RVA: 0x000348F5 File Offset: 0x00032AF5
			public int MaximumWidth { get; set; }

			// Token: 0x17001F40 RID: 8000
			// (get) Token: 0x06005F65 RID: 24421 RVA: 0x000348FE File Offset: 0x00032AFE
			// (set) Token: 0x06005F66 RID: 24422 RVA: 0x00034906 File Offset: 0x00032B06
			public NativeArray<byte> NativeData { get; set; }

			// Token: 0x17001F41 RID: 8001
			// (get) Token: 0x06005F67 RID: 24423 RVA: 0x0003490F File Offset: 0x00032B0F
			// (set) Token: 0x06005F68 RID: 24424 RVA: 0x00034917 File Offset: 0x00032B17
			public Texture Source { get; set; }

			// Token: 0x17001F42 RID: 8002
			// (get) Token: 0x06005F69 RID: 24425 RVA: 0x00034920 File Offset: 0x00032B20
			// (set) Token: 0x06005F6A RID: 24426 RVA: 0x00034928 File Offset: 0x00032B28
			public object State { get; set; }

			// Token: 0x17001F43 RID: 8003
			// (get) Token: 0x06005F6B RID: 24427 RVA: 0x00034931 File Offset: 0x00032B31
			// (set) Token: 0x06005F6C RID: 24428 RVA: 0x00034939 File Offset: 0x00032B39
			public ScreenshotType Type { get; set; }

			// Token: 0x17001F44 RID: 8004
			// (get) Token: 0x06005F6D RID: 24429 RVA: 0x00034942 File Offset: 0x00032B42
			// (set) Token: 0x06005F6E RID: 24430 RVA: 0x0003494A File Offset: 0x00032B4A
			public int Width { get; set; }

			// Token: 0x06005F6F RID: 24431 RVA: 0x001651D8 File Offset: 0x001633D8
			private void EncodeCallback(object state)
			{
				byte[] array = this.NativeData.ToArray();
				int stride;
				array = Downsampler.Downsample(array, this.Width * 4, this.MaximumWidth, this.MaximumHeight, out stride);
				if (this.Type == ScreenshotType.Png)
				{
					array = PngEncoder.Encode(array, stride);
				}
				if (this.Callback != null)
				{
					this.Callback(array, this.State);
				}
				this.NativeData.Dispose();
				this.IsInUse = false;
			}

			// Token: 0x06005F70 RID: 24432 RVA: 0x00034953 File Offset: 0x00032B53
			private void SavePngToDisk(byte[] byteData)
			{
				if (!Directory.Exists("Screenshots"))
				{
					Directory.CreateDirectory("Screenshots");
				}
				File.WriteAllBytes(string.Format("Screenshots/{0}.png", this.Identifier % 60), byteData);
			}

			// Token: 0x06005F71 RID: 24433 RVA: 0x00165254 File Offset: 0x00163454
			private void ScreenshotCallback(AsyncGPUReadbackRequest request)
			{
				if (!request.hasError)
				{
					NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
					NativeArray<byte> data = request.GetData<byte>(0);
					NativeArray<byte> nativeData = new NativeArray<byte>(data, Allocator.Persistent);
					this.Width = request.width;
					this.Height = request.height;
					this.NativeData = nativeData;
					ThreadPool.QueueUserWorkItem(this.EncodeCallbackDelegate, null);
				}
				else if (this.Callback != null)
				{
					this.Callback(null, this.State);
				}
				if (this.Source != null)
				{
					UnityEngine.Object.Destroy(this.Source);
				}
			}

			// Token: 0x04004E46 RID: 20038
			public WaitCallback EncodeCallbackDelegate;

			// Token: 0x04004E47 RID: 20039
			public Action<AsyncGPUReadbackRequest> ScreenshotCallbackDelegate;
		}
	}
}

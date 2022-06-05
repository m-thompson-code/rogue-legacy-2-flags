using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Unity.Screenshots
{
	// Token: 0x0200082E RID: 2094
	public class ScreenshotRecorder
	{
		// Token: 0x0600453B RID: 17723 RVA: 0x000F74F1 File Offset: 0x000F56F1
		public ScreenshotRecorder()
		{
			this.operationPool = new List<ScreenshotRecorder.ScreenshotOperation>();
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x000F7504 File Offset: 0x000F5704
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

		// Token: 0x0600453D RID: 17725 RVA: 0x000F7580 File Offset: 0x000F5780
		public void Screenshot(int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			Texture2D source = ScreenCapture.CaptureScreenshotAsTexture();
			this.Screenshot(source, maximumWidth, maximumHeight, type, callback, state);
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x000F75A4 File Offset: 0x000F57A4
		public void Screenshot(Camera source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			RenderTexture renderTexture = new RenderTexture(maximumWidth, maximumHeight, 24);
			RenderTexture targetTexture = source.targetTexture;
			source.targetTexture = renderTexture;
			source.Render();
			source.targetTexture = targetTexture;
			this.Screenshot(renderTexture, maximumWidth, maximumHeight, type, callback, state);
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x000F75E5 File Offset: 0x000F57E5
		public void Screenshot(RenderTexture source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			this.ScreenshotInternal(source, maximumWidth, maximumHeight, type, callback, state);
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x000F75F6 File Offset: 0x000F57F6
		public void Screenshot(Texture2D source, int maximumWidth, int maximumHeight, ScreenshotType type, Action<byte[], object> callback, object state)
		{
			this.ScreenshotInternal(source, maximumWidth, maximumHeight, type, callback, state);
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x000F7608 File Offset: 0x000F5808
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

		// Token: 0x04003B12 RID: 15122
		private static int nextIdentifier;

		// Token: 0x04003B13 RID: 15123
		private List<ScreenshotRecorder.ScreenshotOperation> operationPool;

		// Token: 0x02000E54 RID: 3668
		private class ScreenshotOperation
		{
			// Token: 0x06006C57 RID: 27735 RVA: 0x00193D40 File Offset: 0x00191F40
			public ScreenshotOperation()
			{
				this.ScreenshotCallbackDelegate = new Action<AsyncGPUReadbackRequest>(this.ScreenshotCallback);
				this.EncodeCallbackDelegate = new WaitCallback(this.EncodeCallback);
			}

			// Token: 0x1700234C RID: 9036
			// (get) Token: 0x06006C58 RID: 27736 RVA: 0x00193D6C File Offset: 0x00191F6C
			// (set) Token: 0x06006C59 RID: 27737 RVA: 0x00193D74 File Offset: 0x00191F74
			public Action<byte[], object> Callback { get; set; }

			// Token: 0x1700234D RID: 9037
			// (get) Token: 0x06006C5A RID: 27738 RVA: 0x00193D7D File Offset: 0x00191F7D
			// (set) Token: 0x06006C5B RID: 27739 RVA: 0x00193D85 File Offset: 0x00191F85
			public int Height { get; set; }

			// Token: 0x1700234E RID: 9038
			// (get) Token: 0x06006C5C RID: 27740 RVA: 0x00193D8E File Offset: 0x00191F8E
			// (set) Token: 0x06006C5D RID: 27741 RVA: 0x00193D96 File Offset: 0x00191F96
			public int Identifier { get; set; }

			// Token: 0x1700234F RID: 9039
			// (get) Token: 0x06006C5E RID: 27742 RVA: 0x00193D9F File Offset: 0x00191F9F
			// (set) Token: 0x06006C5F RID: 27743 RVA: 0x00193DA7 File Offset: 0x00191FA7
			public bool IsInUse { get; set; }

			// Token: 0x17002350 RID: 9040
			// (get) Token: 0x06006C60 RID: 27744 RVA: 0x00193DB0 File Offset: 0x00191FB0
			// (set) Token: 0x06006C61 RID: 27745 RVA: 0x00193DB8 File Offset: 0x00191FB8
			public int MaximumHeight { get; set; }

			// Token: 0x17002351 RID: 9041
			// (get) Token: 0x06006C62 RID: 27746 RVA: 0x00193DC1 File Offset: 0x00191FC1
			// (set) Token: 0x06006C63 RID: 27747 RVA: 0x00193DC9 File Offset: 0x00191FC9
			public int MaximumWidth { get; set; }

			// Token: 0x17002352 RID: 9042
			// (get) Token: 0x06006C64 RID: 27748 RVA: 0x00193DD2 File Offset: 0x00191FD2
			// (set) Token: 0x06006C65 RID: 27749 RVA: 0x00193DDA File Offset: 0x00191FDA
			public NativeArray<byte> NativeData { get; set; }

			// Token: 0x17002353 RID: 9043
			// (get) Token: 0x06006C66 RID: 27750 RVA: 0x00193DE3 File Offset: 0x00191FE3
			// (set) Token: 0x06006C67 RID: 27751 RVA: 0x00193DEB File Offset: 0x00191FEB
			public Texture Source { get; set; }

			// Token: 0x17002354 RID: 9044
			// (get) Token: 0x06006C68 RID: 27752 RVA: 0x00193DF4 File Offset: 0x00191FF4
			// (set) Token: 0x06006C69 RID: 27753 RVA: 0x00193DFC File Offset: 0x00191FFC
			public object State { get; set; }

			// Token: 0x17002355 RID: 9045
			// (get) Token: 0x06006C6A RID: 27754 RVA: 0x00193E05 File Offset: 0x00192005
			// (set) Token: 0x06006C6B RID: 27755 RVA: 0x00193E0D File Offset: 0x0019200D
			public ScreenshotType Type { get; set; }

			// Token: 0x17002356 RID: 9046
			// (get) Token: 0x06006C6C RID: 27756 RVA: 0x00193E16 File Offset: 0x00192016
			// (set) Token: 0x06006C6D RID: 27757 RVA: 0x00193E1E File Offset: 0x0019201E
			public int Width { get; set; }

			// Token: 0x06006C6E RID: 27758 RVA: 0x00193E28 File Offset: 0x00192028
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

			// Token: 0x06006C6F RID: 27759 RVA: 0x00193EA1 File Offset: 0x001920A1
			private void SavePngToDisk(byte[] byteData)
			{
				if (!Directory.Exists("Screenshots"))
				{
					Directory.CreateDirectory("Screenshots");
				}
				File.WriteAllBytes(string.Format("Screenshots/{0}.png", this.Identifier % 60), byteData);
			}

			// Token: 0x06006C70 RID: 27760 RVA: 0x00193ED8 File Offset: 0x001920D8
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

			// Token: 0x040057A6 RID: 22438
			public WaitCallback EncodeCallbackDelegate;

			// Token: 0x040057A7 RID: 22439
			public Action<AsyncGPUReadbackRequest> ScreenshotCallbackDelegate;
		}
	}
}

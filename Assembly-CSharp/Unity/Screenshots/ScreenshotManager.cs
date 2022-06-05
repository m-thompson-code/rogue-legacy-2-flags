using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Screenshots
{
	// Token: 0x02000D0F RID: 3343
	public class ScreenshotManager
	{
		// Token: 0x06005F38 RID: 24376 RVA: 0x00034753 File Offset: 0x00032953
		public ScreenshotManager()
		{
			this.screenshotRecorder = new ScreenshotRecorder();
			this.screenshotCallbackDelegate = new Action<byte[], object>(this.ScreenshotCallback);
			this.screenshotOperations = new List<ScreenshotManager.ScreenshotOperation>();
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x00164DF0 File Offset: 0x00162FF0
		private ScreenshotManager.ScreenshotOperation GetScreenshotOperation()
		{
			foreach (ScreenshotManager.ScreenshotOperation screenshotOperation in this.screenshotOperations)
			{
				if (!screenshotOperation.IsInUse)
				{
					screenshotOperation.Use();
					return screenshotOperation;
				}
			}
			ScreenshotManager.ScreenshotOperation screenshotOperation2 = new ScreenshotManager.ScreenshotOperation();
			screenshotOperation2.Use();
			this.screenshotOperations.Add(screenshotOperation2);
			return screenshotOperation2;
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x00164E6C File Offset: 0x0016306C
		public void OnEndOfFrame()
		{
			foreach (ScreenshotManager.ScreenshotOperation screenshotOperation in this.screenshotOperations)
			{
				if (screenshotOperation.IsInUse)
				{
					if (screenshotOperation.IsAwaiting)
					{
						screenshotOperation.IsAwaiting = false;
						if (screenshotOperation.Source == null)
						{
							this.screenshotRecorder.Screenshot(screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, screenshotOperation);
						}
						else if (screenshotOperation.Source is Camera)
						{
							this.screenshotRecorder.Screenshot(screenshotOperation.Source as Camera, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, screenshotOperation);
						}
						else if (screenshotOperation.Source is RenderTexture)
						{
							this.screenshotRecorder.Screenshot(screenshotOperation.Source as RenderTexture, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, screenshotOperation);
						}
						else if (screenshotOperation.Source is Texture2D)
						{
							this.screenshotRecorder.Screenshot(screenshotOperation.Source as Texture2D, screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, ScreenshotType.Png, this.screenshotCallbackDelegate, screenshotOperation);
						}
						else
						{
							this.ScreenshotCallback(null, screenshotOperation);
						}
					}
					else if (screenshotOperation.IsComplete)
					{
						screenshotOperation.IsInUse = false;
						try
						{
							if (screenshotOperation != null && screenshotOperation.Callback != null)
							{
								screenshotOperation.Callback(screenshotOperation.FrameNumber, screenshotOperation.Data);
							}
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x0016501C File Offset: 0x0016321C
		private void ScreenshotCallback(byte[] data, object state)
		{
			ScreenshotManager.ScreenshotOperation screenshotOperation = state as ScreenshotManager.ScreenshotOperation;
			if (screenshotOperation != null)
			{
				screenshotOperation.Data = data;
				screenshotOperation.IsComplete = true;
			}
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x00034783 File Offset: 0x00032983
		public void TakeScreenshot(object source, int frameNumber, int maximumWidth, int maximumHeight, Action<int, byte[]> callback)
		{
			ScreenshotManager.ScreenshotOperation screenshotOperation = this.GetScreenshotOperation();
			screenshotOperation.FrameNumber = frameNumber;
			screenshotOperation.MaximumWidth = maximumWidth;
			screenshotOperation.MaximumHeight = maximumHeight;
			screenshotOperation.Source = source;
			screenshotOperation.Callback = callback;
		}

		// Token: 0x04004E38 RID: 20024
		private Action<byte[], object> screenshotCallbackDelegate;

		// Token: 0x04004E39 RID: 20025
		private List<ScreenshotManager.ScreenshotOperation> screenshotOperations;

		// Token: 0x04004E3A RID: 20026
		private ScreenshotRecorder screenshotRecorder;

		// Token: 0x02000D10 RID: 3344
		private class ScreenshotOperation
		{
			// Token: 0x17001F31 RID: 7985
			// (get) Token: 0x06005F3D RID: 24381 RVA: 0x000347AF File Offset: 0x000329AF
			// (set) Token: 0x06005F3E RID: 24382 RVA: 0x000347B7 File Offset: 0x000329B7
			public Action<int, byte[]> Callback { get; set; }

			// Token: 0x17001F32 RID: 7986
			// (get) Token: 0x06005F3F RID: 24383 RVA: 0x000347C0 File Offset: 0x000329C0
			// (set) Token: 0x06005F40 RID: 24384 RVA: 0x000347C8 File Offset: 0x000329C8
			public byte[] Data { get; set; }

			// Token: 0x17001F33 RID: 7987
			// (get) Token: 0x06005F41 RID: 24385 RVA: 0x000347D1 File Offset: 0x000329D1
			// (set) Token: 0x06005F42 RID: 24386 RVA: 0x000347D9 File Offset: 0x000329D9
			public int FrameNumber { get; set; }

			// Token: 0x17001F34 RID: 7988
			// (get) Token: 0x06005F43 RID: 24387 RVA: 0x000347E2 File Offset: 0x000329E2
			// (set) Token: 0x06005F44 RID: 24388 RVA: 0x000347EA File Offset: 0x000329EA
			public bool IsAwaiting { get; set; }

			// Token: 0x17001F35 RID: 7989
			// (get) Token: 0x06005F45 RID: 24389 RVA: 0x000347F3 File Offset: 0x000329F3
			// (set) Token: 0x06005F46 RID: 24390 RVA: 0x000347FB File Offset: 0x000329FB
			public bool IsComplete { get; set; }

			// Token: 0x17001F36 RID: 7990
			// (get) Token: 0x06005F47 RID: 24391 RVA: 0x00034804 File Offset: 0x00032A04
			// (set) Token: 0x06005F48 RID: 24392 RVA: 0x0003480C File Offset: 0x00032A0C
			public bool IsInUse { get; set; }

			// Token: 0x17001F37 RID: 7991
			// (get) Token: 0x06005F49 RID: 24393 RVA: 0x00034815 File Offset: 0x00032A15
			// (set) Token: 0x06005F4A RID: 24394 RVA: 0x0003481D File Offset: 0x00032A1D
			public int MaximumHeight { get; set; }

			// Token: 0x17001F38 RID: 7992
			// (get) Token: 0x06005F4B RID: 24395 RVA: 0x00034826 File Offset: 0x00032A26
			// (set) Token: 0x06005F4C RID: 24396 RVA: 0x0003482E File Offset: 0x00032A2E
			public int MaximumWidth { get; set; }

			// Token: 0x17001F39 RID: 7993
			// (get) Token: 0x06005F4D RID: 24397 RVA: 0x00034837 File Offset: 0x00032A37
			// (set) Token: 0x06005F4E RID: 24398 RVA: 0x0003483F File Offset: 0x00032A3F
			public object Source { get; set; }

			// Token: 0x06005F4F RID: 24399 RVA: 0x00165044 File Offset: 0x00163244
			public void Use()
			{
				this.Callback = null;
				this.Data = null;
				this.FrameNumber = 0;
				this.IsAwaiting = true;
				this.IsComplete = false;
				this.IsInUse = true;
				this.MaximumHeight = 0;
				this.MaximumWidth = 0;
				this.Source = null;
			}
		}
	}
}

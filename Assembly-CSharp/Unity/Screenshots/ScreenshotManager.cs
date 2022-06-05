using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Screenshots
{
	// Token: 0x0200082D RID: 2093
	public class ScreenshotManager
	{
		// Token: 0x06004536 RID: 17718 RVA: 0x000F7241 File Offset: 0x000F5441
		public ScreenshotManager()
		{
			this.screenshotRecorder = new ScreenshotRecorder();
			this.screenshotCallbackDelegate = new Action<byte[], object>(this.ScreenshotCallback);
			this.screenshotOperations = new List<ScreenshotManager.ScreenshotOperation>();
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x000F7274 File Offset: 0x000F5474
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

		// Token: 0x06004538 RID: 17720 RVA: 0x000F72F0 File Offset: 0x000F54F0
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

		// Token: 0x06004539 RID: 17721 RVA: 0x000F74A0 File Offset: 0x000F56A0
		private void ScreenshotCallback(byte[] data, object state)
		{
			ScreenshotManager.ScreenshotOperation screenshotOperation = state as ScreenshotManager.ScreenshotOperation;
			if (screenshotOperation != null)
			{
				screenshotOperation.Data = data;
				screenshotOperation.IsComplete = true;
			}
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x000F74C5 File Offset: 0x000F56C5
		public void TakeScreenshot(object source, int frameNumber, int maximumWidth, int maximumHeight, Action<int, byte[]> callback)
		{
			ScreenshotManager.ScreenshotOperation screenshotOperation = this.GetScreenshotOperation();
			screenshotOperation.FrameNumber = frameNumber;
			screenshotOperation.MaximumWidth = maximumWidth;
			screenshotOperation.MaximumHeight = maximumHeight;
			screenshotOperation.Source = source;
			screenshotOperation.Callback = callback;
		}

		// Token: 0x04003B0F RID: 15119
		private Action<byte[], object> screenshotCallbackDelegate;

		// Token: 0x04003B10 RID: 15120
		private List<ScreenshotManager.ScreenshotOperation> screenshotOperations;

		// Token: 0x04003B11 RID: 15121
		private ScreenshotRecorder screenshotRecorder;

		// Token: 0x02000E53 RID: 3667
		private class ScreenshotOperation
		{
			// Token: 0x17002343 RID: 9027
			// (get) Token: 0x06006C43 RID: 27715 RVA: 0x00193C50 File Offset: 0x00191E50
			// (set) Token: 0x06006C44 RID: 27716 RVA: 0x00193C58 File Offset: 0x00191E58
			public Action<int, byte[]> Callback { get; set; }

			// Token: 0x17002344 RID: 9028
			// (get) Token: 0x06006C45 RID: 27717 RVA: 0x00193C61 File Offset: 0x00191E61
			// (set) Token: 0x06006C46 RID: 27718 RVA: 0x00193C69 File Offset: 0x00191E69
			public byte[] Data { get; set; }

			// Token: 0x17002345 RID: 9029
			// (get) Token: 0x06006C47 RID: 27719 RVA: 0x00193C72 File Offset: 0x00191E72
			// (set) Token: 0x06006C48 RID: 27720 RVA: 0x00193C7A File Offset: 0x00191E7A
			public int FrameNumber { get; set; }

			// Token: 0x17002346 RID: 9030
			// (get) Token: 0x06006C49 RID: 27721 RVA: 0x00193C83 File Offset: 0x00191E83
			// (set) Token: 0x06006C4A RID: 27722 RVA: 0x00193C8B File Offset: 0x00191E8B
			public bool IsAwaiting { get; set; }

			// Token: 0x17002347 RID: 9031
			// (get) Token: 0x06006C4B RID: 27723 RVA: 0x00193C94 File Offset: 0x00191E94
			// (set) Token: 0x06006C4C RID: 27724 RVA: 0x00193C9C File Offset: 0x00191E9C
			public bool IsComplete { get; set; }

			// Token: 0x17002348 RID: 9032
			// (get) Token: 0x06006C4D RID: 27725 RVA: 0x00193CA5 File Offset: 0x00191EA5
			// (set) Token: 0x06006C4E RID: 27726 RVA: 0x00193CAD File Offset: 0x00191EAD
			public bool IsInUse { get; set; }

			// Token: 0x17002349 RID: 9033
			// (get) Token: 0x06006C4F RID: 27727 RVA: 0x00193CB6 File Offset: 0x00191EB6
			// (set) Token: 0x06006C50 RID: 27728 RVA: 0x00193CBE File Offset: 0x00191EBE
			public int MaximumHeight { get; set; }

			// Token: 0x1700234A RID: 9034
			// (get) Token: 0x06006C51 RID: 27729 RVA: 0x00193CC7 File Offset: 0x00191EC7
			// (set) Token: 0x06006C52 RID: 27730 RVA: 0x00193CCF File Offset: 0x00191ECF
			public int MaximumWidth { get; set; }

			// Token: 0x1700234B RID: 9035
			// (get) Token: 0x06006C53 RID: 27731 RVA: 0x00193CD8 File Offset: 0x00191ED8
			// (set) Token: 0x06006C54 RID: 27732 RVA: 0x00193CE0 File Offset: 0x00191EE0
			public object Source { get; set; }

			// Token: 0x06006C55 RID: 27733 RVA: 0x00193CEC File Offset: 0x00191EEC
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

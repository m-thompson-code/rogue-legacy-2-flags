using System;

// Token: 0x0200068D RID: 1677
public class CameraZoomChangeEventArgs : EventArgs
{
	// Token: 0x06003CC2 RID: 15554 RVA: 0x000D2186 File Offset: 0x000D0386
	public CameraZoomChangeEventArgs(float zoomLevel, float previousZoomLevel)
	{
		this.Initialize(zoomLevel, previousZoomLevel);
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x000D2196 File Offset: 0x000D0396
	public void Initialize(float zoomLevel, float previousZoomLevel)
	{
		this.ZoomLevel = zoomLevel;
		this.PreviousZoomLevel = previousZoomLevel;
	}

	// Token: 0x17001525 RID: 5413
	// (get) Token: 0x06003CC4 RID: 15556 RVA: 0x000D21A6 File Offset: 0x000D03A6
	// (set) Token: 0x06003CC5 RID: 15557 RVA: 0x000D21AE File Offset: 0x000D03AE
	public float ZoomLevel { get; private set; }

	// Token: 0x17001526 RID: 5414
	// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x000D21B7 File Offset: 0x000D03B7
	// (set) Token: 0x06003CC7 RID: 15559 RVA: 0x000D21BF File Offset: 0x000D03BF
	public float PreviousZoomLevel { get; private set; }
}

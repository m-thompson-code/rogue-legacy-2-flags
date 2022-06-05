using System;

// Token: 0x02000B15 RID: 2837
public class CameraZoomChangeEventArgs : EventArgs
{
	// Token: 0x06005554 RID: 21844 RVA: 0x0002E521 File Offset: 0x0002C721
	public CameraZoomChangeEventArgs(float zoomLevel, float previousZoomLevel)
	{
		this.Initialize(zoomLevel, previousZoomLevel);
	}

	// Token: 0x06005555 RID: 21845 RVA: 0x0002E531 File Offset: 0x0002C731
	public void Initialize(float zoomLevel, float previousZoomLevel)
	{
		this.ZoomLevel = zoomLevel;
		this.PreviousZoomLevel = previousZoomLevel;
	}

	// Token: 0x17001CF1 RID: 7409
	// (get) Token: 0x06005556 RID: 21846 RVA: 0x0002E541 File Offset: 0x0002C741
	// (set) Token: 0x06005557 RID: 21847 RVA: 0x0002E549 File Offset: 0x0002C749
	public float ZoomLevel { get; private set; }

	// Token: 0x17001CF2 RID: 7410
	// (get) Token: 0x06005558 RID: 21848 RVA: 0x0002E552 File Offset: 0x0002C752
	// (set) Token: 0x06005559 RID: 21849 RVA: 0x0002E55A File Offset: 0x0002C75A
	public float PreviousZoomLevel { get; private set; }
}

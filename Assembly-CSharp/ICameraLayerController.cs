using System;

// Token: 0x020005CC RID: 1484
public interface ICameraLayerController
{
	// Token: 0x1700134C RID: 4940
	// (get) Token: 0x06003684 RID: 13956
	CameraLayer CameraLayer { get; }

	// Token: 0x1700134D RID: 4941
	// (get) Token: 0x06003685 RID: 13957
	int SubLayer { get; }

	// Token: 0x06003686 RID: 13958
	void SetCameraLayer(CameraLayer value);

	// Token: 0x06003687 RID: 13959
	void SetSubLayer(int value, bool isDeco = false);
}

using System;

// Token: 0x020009D4 RID: 2516
public interface ICameraLayerController
{
	// Token: 0x17001A79 RID: 6777
	// (get) Token: 0x06004C96 RID: 19606
	CameraLayer CameraLayer { get; }

	// Token: 0x17001A7A RID: 6778
	// (get) Token: 0x06004C97 RID: 19607
	int SubLayer { get; }

	// Token: 0x06004C98 RID: 19608
	void SetCameraLayer(CameraLayer value);

	// Token: 0x06004C99 RID: 19609
	void SetSubLayer(int value, bool isDeco = false);
}

using System;

// Token: 0x02000BC1 RID: 3009
public struct CameraLayerAndSubLayer
{
	// Token: 0x06005A25 RID: 23077 RVA: 0x000314AA File Offset: 0x0002F6AA
	public CameraLayerAndSubLayer(CameraLayer cameraLayer, int subLayer)
	{
		this.CameraLayer = cameraLayer;
		this.SubLayer = subLayer;
	}

	// Token: 0x17001E21 RID: 7713
	// (get) Token: 0x06005A26 RID: 23078 RVA: 0x000314BA File Offset: 0x0002F6BA
	public readonly CameraLayer CameraLayer { get; }

	// Token: 0x17001E22 RID: 7714
	// (get) Token: 0x06005A27 RID: 23079 RVA: 0x000314C2 File Offset: 0x0002F6C2
	public readonly int SubLayer { get; }
}

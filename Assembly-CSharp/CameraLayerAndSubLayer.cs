using System;

// Token: 0x0200070E RID: 1806
public struct CameraLayerAndSubLayer
{
	// Token: 0x060040DC RID: 16604 RVA: 0x000E5A19 File Offset: 0x000E3C19
	public CameraLayerAndSubLayer(CameraLayer cameraLayer, int subLayer)
	{
		this.CameraLayer = cameraLayer;
		this.SubLayer = subLayer;
	}

	// Token: 0x17001625 RID: 5669
	// (get) Token: 0x060040DD RID: 16605 RVA: 0x000E5A29 File Offset: 0x000E3C29
	public readonly CameraLayer CameraLayer { get; }

	// Token: 0x17001626 RID: 5670
	// (get) Token: 0x060040DE RID: 16606 RVA: 0x000E5A31 File Offset: 0x000E3C31
	public readonly int SubLayer { get; }
}

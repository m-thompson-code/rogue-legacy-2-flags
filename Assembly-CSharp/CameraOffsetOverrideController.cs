using System;
using Cinemachine;
using UnityEngine;

// Token: 0x020006A0 RID: 1696
public class CameraOffsetOverrideController : MonoBehaviour
{
	// Token: 0x06003426 RID: 13350 RVA: 0x000DC3B0 File Offset: 0x000DA5B0
	private void Initialize(CinemachineVirtualCamera vcam)
	{
		if (!this.m_isInitialized)
		{
			this.m_framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
			if (this.m_framingTransposer)
			{
				this.m_storedOffset = new Vector2(this.m_framingTransposer.m_ScreenX, this.m_framingTransposer.m_ScreenY);
			}
			this.m_isInitialized = true;
		}
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x000DC408 File Offset: 0x000DA608
	public void ApplyOffset(CinemachineVirtualCamera vcam)
	{
		this.Initialize(vcam);
		if (this.m_framingTransposer)
		{
			this.m_framingTransposer.m_ScreenX = this.m_storedOffset.x + this.Offset.x;
			this.m_framingTransposer.m_ScreenY = this.m_storedOffset.y + this.Offset.y;
		}
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x0001C9D9 File Offset: 0x0001ABD9
	public void ResetOffset()
	{
		if (this.m_framingTransposer)
		{
			this.m_framingTransposer.m_ScreenX = this.m_storedOffset.x;
			this.m_framingTransposer.m_ScreenY = this.m_storedOffset.y;
		}
	}

	// Token: 0x04002A3B RID: 10811
	[Tooltip("Ranges from -0.5f to 1.5f (set by Cinemachine)")]
	public Vector2 Offset;

	// Token: 0x04002A3C RID: 10812
	private Vector2 m_storedOffset;

	// Token: 0x04002A3D RID: 10813
	private CinemachineFramingTransposer m_framingTransposer;

	// Token: 0x04002A3E RID: 10814
	private bool m_isInitialized;
}

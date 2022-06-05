using System;
using Cinemachine;
using UnityEngine;

// Token: 0x020003F7 RID: 1015
public class CameraOffsetOverrideController : MonoBehaviour
{
	// Token: 0x060025B8 RID: 9656 RVA: 0x0007C7CC File Offset: 0x0007A9CC
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

	// Token: 0x060025B9 RID: 9657 RVA: 0x0007C824 File Offset: 0x0007AA24
	public void ApplyOffset(CinemachineVirtualCamera vcam)
	{
		this.Initialize(vcam);
		if (this.m_framingTransposer)
		{
			this.m_framingTransposer.m_ScreenX = this.m_storedOffset.x + this.Offset.x;
			this.m_framingTransposer.m_ScreenY = this.m_storedOffset.y + this.Offset.y;
		}
	}

	// Token: 0x060025BA RID: 9658 RVA: 0x0007C889 File Offset: 0x0007AA89
	public void ResetOffset()
	{
		if (this.m_framingTransposer)
		{
			this.m_framingTransposer.m_ScreenX = this.m_storedOffset.x;
			this.m_framingTransposer.m_ScreenY = this.m_storedOffset.y;
		}
	}

	// Token: 0x04001F9A RID: 8090
	[Tooltip("Ranges from -0.5f to 1.5f (set by Cinemachine)")]
	public Vector2 Offset;

	// Token: 0x04001F9B RID: 8091
	private Vector2 m_storedOffset;

	// Token: 0x04001F9C RID: 8092
	private CinemachineFramingTransposer m_framingTransposer;

	// Token: 0x04001F9D RID: 8093
	private bool m_isInitialized;
}

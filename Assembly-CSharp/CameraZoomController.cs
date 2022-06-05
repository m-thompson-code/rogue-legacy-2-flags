using System;
using UnityEngine;

// Token: 0x020003F8 RID: 1016
public class CameraZoomController : MonoBehaviour
{
	// Token: 0x17000F32 RID: 3890
	// (get) Token: 0x060025BC RID: 9660 RVA: 0x0007C8CC File Offset: 0x0007AACC
	// (set) Token: 0x060025BD RID: 9661 RVA: 0x0007C8D4 File Offset: 0x0007AAD4
	public bool OverrideZoomLevel
	{
		get
		{
			return this.m_overrideZoomLevel;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_overrideZoomLevel = value;
			}
		}
	}

	// Token: 0x17000F33 RID: 3891
	// (get) Token: 0x060025BE RID: 9662 RVA: 0x0007C8E4 File Offset: 0x0007AAE4
	// (set) Token: 0x060025BF RID: 9663 RVA: 0x0007C8FA File Offset: 0x0007AAFA
	public float ZoomLevel
	{
		get
		{
			if (this.OverrideZoomLevel)
			{
				return this.m_zoomLevel;
			}
			return 1f;
		}
		set
		{
			if (this.OverrideZoomLevel && !Application.isPlaying)
			{
				this.m_zoomLevel = Mathf.Clamp(value, 1f, 10f);
			}
		}
	}

	// Token: 0x060025C0 RID: 9664 RVA: 0x0007C921 File Offset: 0x0007AB21
	public void SetZoomLevel(float zoomLevel)
	{
		if (!this.OverrideZoomLevel)
		{
			this.m_overrideZoomLevel = true;
			this.m_zoomLevel = zoomLevel;
		}
	}

	// Token: 0x04001F9E RID: 8094
	[SerializeField]
	private float m_zoomLevel = 1f;

	// Token: 0x04001F9F RID: 8095
	[SerializeField]
	private bool m_overrideZoomLevel;
}

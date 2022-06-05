using System;
using UnityEngine;

// Token: 0x020006A1 RID: 1697
public class CameraZoomController : MonoBehaviour
{
	// Token: 0x170013E1 RID: 5089
	// (get) Token: 0x0600342A RID: 13354 RVA: 0x0001CA14 File Offset: 0x0001AC14
	// (set) Token: 0x0600342B RID: 13355 RVA: 0x0001CA1C File Offset: 0x0001AC1C
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

	// Token: 0x170013E2 RID: 5090
	// (get) Token: 0x0600342C RID: 13356 RVA: 0x0001CA2C File Offset: 0x0001AC2C
	// (set) Token: 0x0600342D RID: 13357 RVA: 0x0001CA42 File Offset: 0x0001AC42
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

	// Token: 0x0600342E RID: 13358 RVA: 0x0001CA69 File Offset: 0x0001AC69
	public void SetZoomLevel(float zoomLevel)
	{
		if (!this.OverrideZoomLevel)
		{
			this.m_overrideZoomLevel = true;
			this.m_zoomLevel = zoomLevel;
		}
	}

	// Token: 0x04002A3F RID: 10815
	[SerializeField]
	private float m_zoomLevel = 1f;

	// Token: 0x04002A40 RID: 10816
	[SerializeField]
	private bool m_overrideZoomLevel;
}

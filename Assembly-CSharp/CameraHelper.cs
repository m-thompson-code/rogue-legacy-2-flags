using System;
using UnityEngine;

// Token: 0x020001C9 RID: 457
[RequireComponent(typeof(Camera))]
public class CameraHelper : MonoBehaviour
{
	// Token: 0x06001270 RID: 4720 RVA: 0x00035D1C File Offset: 0x00033F1C
	private void Awake()
	{
		this.m_camera = base.GetComponent<Camera>();
		this.m_camera.eventMask = 0;
	}

	// Token: 0x040012D8 RID: 4824
	private Camera m_camera;
}

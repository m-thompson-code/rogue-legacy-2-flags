using System;
using UnityEngine;

// Token: 0x02000341 RID: 833
[RequireComponent(typeof(Camera))]
public class CameraHelper : MonoBehaviour
{
	// Token: 0x06001AEC RID: 6892 RVA: 0x0000DF5C File Offset: 0x0000C15C
	private void Awake()
	{
		this.m_camera = base.GetComponent<Camera>();
		this.m_camera.eventMask = 0;
	}

	// Token: 0x04001906 RID: 6406
	private Camera m_camera;
}

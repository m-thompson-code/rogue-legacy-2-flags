using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000602 RID: 1538
public class HUDManager : MonoBehaviour
{
	// Token: 0x06002F62 RID: 12130 RVA: 0x00019ED3 File Offset: 0x000180D3
	private IEnumerator Start()
	{
		while (!CameraController.IsInstantiated)
		{
			yield return null;
		}
		this.m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
		this.m_canvas.worldCamera = CameraController.UICamera;
		this.m_canvas.planeDistance = this.m_planeDistance;
		yield break;
	}

	// Token: 0x040026C6 RID: 9926
	[SerializeField]
	private Canvas m_canvas;

	// Token: 0x040026C7 RID: 9927
	[SerializeField]
	private float m_planeDistance = 150f;
}

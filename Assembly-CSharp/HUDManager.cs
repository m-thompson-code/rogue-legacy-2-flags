using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200037C RID: 892
public class HUDManager : MonoBehaviour
{
	// Token: 0x06002175 RID: 8565 RVA: 0x000694DD File Offset: 0x000676DD
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

	// Token: 0x04001CF2 RID: 7410
	[SerializeField]
	private Canvas m_canvas;

	// Token: 0x04001CF3 RID: 7411
	[SerializeField]
	private float m_planeDistance = 150f;
}

using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class ClearTrailRenderer : MonoBehaviour
{
	// Token: 0x060012DC RID: 4828 RVA: 0x00037FAE File Offset: 0x000361AE
	private void Awake()
	{
		this.m_trailRenderer = base.GetComponent<TrailRenderer>();
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x00037FBC File Offset: 0x000361BC
	private void OnDisable()
	{
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
	}

	// Token: 0x0400131D RID: 4893
	private TrailRenderer m_trailRenderer;
}

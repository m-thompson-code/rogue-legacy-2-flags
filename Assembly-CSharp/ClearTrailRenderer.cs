using System;
using UnityEngine;

// Token: 0x02000351 RID: 849
public class ClearTrailRenderer : MonoBehaviour
{
	// Token: 0x06001B69 RID: 7017 RVA: 0x0000E3BB File Offset: 0x0000C5BB
	private void Awake()
	{
		this.m_trailRenderer = base.GetComponent<TrailRenderer>();
	}

	// Token: 0x06001B6A RID: 7018 RVA: 0x0000E3C9 File Offset: 0x0000C5C9
	private void OnDisable()
	{
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.Clear();
		}
	}

	// Token: 0x0400195D RID: 6493
	private TrailRenderer m_trailRenderer;
}

using System;
using UnityEngine;

// Token: 0x020006F6 RID: 1782
public class FadeOutTrailRenderer : MonoBehaviour
{
	// Token: 0x0600366D RID: 13933 RVA: 0x0001DF0F File Offset: 0x0001C10F
	private void Awake()
	{
		this.m_matBlock = new MaterialPropertyBlock();
		this.m_trailRenderer = base.GetComponentInChildren<TrailRenderer>();
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x000E3F44 File Offset: 0x000E2144
	public void OnEnable()
	{
		if (this.m_trailRenderer && this.m_resetAlphaOnEnable)
		{
			this.m_trailRenderer.GetPropertyBlock(this.m_matBlock);
			this.m_matBlock.SetFloat(ShaderID_RL._Opacity, 1f);
			this.m_trailRenderer.SetPropertyBlock(this.m_matBlock);
		}
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x000E3FA0 File Offset: 0x000E21A0
	public void SetOpacity(float opacity)
	{
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.GetPropertyBlock(this.m_matBlock);
			this.m_matBlock.SetFloat(ShaderID_RL._Opacity, opacity);
			this.m_trailRenderer.SetPropertyBlock(this.m_matBlock);
		}
	}

	// Token: 0x04002C2E RID: 11310
	[SerializeField]
	private bool m_resetAlphaOnEnable;

	// Token: 0x04002C2F RID: 11311
	private MaterialPropertyBlock m_matBlock;

	// Token: 0x04002C30 RID: 11312
	private TrailRenderer m_trailRenderer;
}

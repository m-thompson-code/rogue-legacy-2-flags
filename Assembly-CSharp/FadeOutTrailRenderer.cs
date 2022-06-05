using System;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class FadeOutTrailRenderer : MonoBehaviour
{
	// Token: 0x06002764 RID: 10084 RVA: 0x00083128 File Offset: 0x00081328
	private void Awake()
	{
		this.m_matBlock = new MaterialPropertyBlock();
		this.m_trailRenderer = base.GetComponentInChildren<TrailRenderer>();
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x00083144 File Offset: 0x00081344
	public void OnEnable()
	{
		if (this.m_trailRenderer && this.m_resetAlphaOnEnable)
		{
			this.m_trailRenderer.GetPropertyBlock(this.m_matBlock);
			this.m_matBlock.SetFloat(ShaderID_RL._Opacity, 1f);
			this.m_trailRenderer.SetPropertyBlock(this.m_matBlock);
		}
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x000831A0 File Offset: 0x000813A0
	public void SetOpacity(float opacity)
	{
		if (this.m_trailRenderer)
		{
			this.m_trailRenderer.GetPropertyBlock(this.m_matBlock);
			this.m_matBlock.SetFloat(ShaderID_RL._Opacity, opacity);
			this.m_trailRenderer.SetPropertyBlock(this.m_matBlock);
		}
	}

	// Token: 0x04002103 RID: 8451
	[SerializeField]
	private bool m_resetAlphaOnEnable;

	// Token: 0x04002104 RID: 8452
	private MaterialPropertyBlock m_matBlock;

	// Token: 0x04002105 RID: 8453
	private TrailRenderer m_trailRenderer;
}

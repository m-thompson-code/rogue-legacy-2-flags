using System;
using UnityEngine;

// Token: 0x0200059B RID: 1435
[ExecuteInEditMode]
public class HeirloomWarp_Effect : MonoBehaviour
{
	// Token: 0x060035F8 RID: 13816 RVA: 0x000BC11F File Offset: 0x000BA31F
	private void OnEnable()
	{
		if (this.m_material == null)
		{
			this.m_material = new Material(Shader.Find("RL2/PPS/HeirloomWarp"));
			this.m_material.SetTexture("_WarpDistortionMap", this.WarpDistortionMap);
		}
	}

	// Token: 0x060035F9 RID: 13817 RVA: 0x000BC15C File Offset: 0x000BA35C
	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (this.prevWarpCenterX != this.WarpCenterX)
		{
			this.m_material.SetFloat("_WarpCenterX", this.WarpCenterX);
		}
		this.prevWarpCenterX = this.WarpCenterX;
		if (this.prevWarpCenterY != this.WarpCenterY)
		{
			this.m_material.SetFloat("_WarpCenterY", this.WarpCenterY);
		}
		this.prevWarpCenterY = this.WarpCenterY;
		if (this.prevDistortionAmount != this.DistortionAmount)
		{
			this.m_material.SetFloat("_DistortionAmount", this.DistortionAmount);
		}
		this.prevDistortionAmount = this.DistortionAmount;
		Graphics.Blit(src, dst, this.m_material);
	}

	// Token: 0x04002A11 RID: 10769
	public Texture WarpDistortionMap;

	// Token: 0x04002A12 RID: 10770
	public float WarpCenterX;

	// Token: 0x04002A13 RID: 10771
	public float WarpCenterY;

	// Token: 0x04002A14 RID: 10772
	public float DistortionAmount;

	// Token: 0x04002A15 RID: 10773
	private Material m_material;

	// Token: 0x04002A16 RID: 10774
	private float prevWarpCenterX;

	// Token: 0x04002A17 RID: 10775
	private float prevWarpCenterY;

	// Token: 0x04002A18 RID: 10776
	private float prevDistortionAmount;
}

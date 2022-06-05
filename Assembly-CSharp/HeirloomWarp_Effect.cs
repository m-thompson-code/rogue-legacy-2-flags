using System;
using UnityEngine;

// Token: 0x020009A3 RID: 2467
[ExecuteInEditMode]
public class HeirloomWarp_Effect : MonoBehaviour
{
	// Token: 0x06004C0A RID: 19466 RVA: 0x00029AB0 File Offset: 0x00027CB0
	private void OnEnable()
	{
		if (this.m_material == null)
		{
			this.m_material = new Material(Shader.Find("RL2/PPS/HeirloomWarp"));
			this.m_material.SetTexture("_WarpDistortionMap", this.WarpDistortionMap);
		}
	}

	// Token: 0x06004C0B RID: 19467 RVA: 0x0012A194 File Offset: 0x00128394
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

	// Token: 0x04003A0F RID: 14863
	public Texture WarpDistortionMap;

	// Token: 0x04003A10 RID: 14864
	public float WarpCenterX;

	// Token: 0x04003A11 RID: 14865
	public float WarpCenterY;

	// Token: 0x04003A12 RID: 14866
	public float DistortionAmount;

	// Token: 0x04003A13 RID: 14867
	private Material m_material;

	// Token: 0x04003A14 RID: 14868
	private float prevWarpCenterX;

	// Token: 0x04003A15 RID: 14869
	private float prevWarpCenterY;

	// Token: 0x04003A16 RID: 14870
	private float prevDistortionAmount;
}

using System;
using UnityEngine;

// Token: 0x020009A0 RID: 2464
[ExecuteInEditMode]
public class AngryOnHit_Effect : MonoBehaviour
{
	// Token: 0x06004C04 RID: 19460 RVA: 0x00029A4F File Offset: 0x00027C4F
	private void OnEnable()
	{
		if (this.m_material == null)
		{
			this.m_material = new Material(Shader.Find("Ferr/2D Terrain/Unlit/Tinted Textured Vertex Color"));
		}
	}

	// Token: 0x06004C05 RID: 19461 RVA: 0x0012A118 File Offset: 0x00128318
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.Amount == 0f && this.m_previousAmount == 0f)
		{
			base.enabled = false;
		}
		if (this.Amount != this.m_previousAmount)
		{
			this.m_material.SetColor("_Color", Color.Lerp(Color.white, this.m_fullAngryColor, this.Amount));
		}
		this.m_previousAmount = this.Amount;
		Graphics.Blit(source, destination, this.m_material);
	}

	// Token: 0x04003A05 RID: 14853
	[Range(0f, 1f)]
	public float Amount;

	// Token: 0x04003A06 RID: 14854
	[SerializeField]
	private Color m_fullAngryColor;

	// Token: 0x04003A07 RID: 14855
	private Material m_material;

	// Token: 0x04003A08 RID: 14856
	private float m_previousAmount;
}

using System;
using UnityEngine;

// Token: 0x02000599 RID: 1433
[ExecuteInEditMode]
public class AngryOnHit_Effect : MonoBehaviour
{
	// Token: 0x060035F2 RID: 13810 RVA: 0x000BC031 File Offset: 0x000BA231
	private void OnEnable()
	{
		if (this.m_material == null)
		{
			this.m_material = new Material(Shader.Find("Ferr/2D Terrain/Unlit/Tinted Textured Vertex Color"));
		}
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x000BC058 File Offset: 0x000BA258
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

	// Token: 0x04002A0B RID: 10763
	[Range(0f, 1f)]
	public float Amount;

	// Token: 0x04002A0C RID: 10764
	[SerializeField]
	private Color m_fullAngryColor;

	// Token: 0x04002A0D RID: 10765
	private Material m_material;

	// Token: 0x04002A0E RID: 10766
	private float m_previousAmount;
}

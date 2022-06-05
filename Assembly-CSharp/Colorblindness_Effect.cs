using System;
using UnityEngine;

// Token: 0x0200059A RID: 1434
public class Colorblindness_Effect : MonoBehaviour
{
	// Token: 0x060035F5 RID: 13813 RVA: 0x000BC0DB File Offset: 0x000BA2DB
	private void Awake()
	{
		this.m_material = new Material(Shader.Find("RL2/Colorblindness"));
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x000BC0F2 File Offset: 0x000BA2F2
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.m_material.SetInt("_CBType", (int)this.m_colorblindType);
		Graphics.Blit(source, destination, this.m_material);
	}

	// Token: 0x04002A0F RID: 10767
	[SerializeField]
	private Colorblindness_Effect.ColorblindType m_colorblindType;

	// Token: 0x04002A10 RID: 10768
	private Material m_material;

	// Token: 0x02000D7D RID: 3453
	private enum ColorblindType
	{
		// Token: 0x04005483 RID: 21635
		Protanopia,
		// Token: 0x04005484 RID: 21636
		Deuteranopia,
		// Token: 0x04005485 RID: 21637
		Tritanopia
	}
}

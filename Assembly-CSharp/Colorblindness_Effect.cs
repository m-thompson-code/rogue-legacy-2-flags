using System;
using UnityEngine;

// Token: 0x020009A1 RID: 2465
public class Colorblindness_Effect : MonoBehaviour
{
	// Token: 0x06004C07 RID: 19463 RVA: 0x00029A74 File Offset: 0x00027C74
	private void Awake()
	{
		this.m_material = new Material(Shader.Find("RL2/Colorblindness"));
	}

	// Token: 0x06004C08 RID: 19464 RVA: 0x00029A8B File Offset: 0x00027C8B
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.m_material.SetInt("_CBType", (int)this.m_colorblindType);
		Graphics.Blit(source, destination, this.m_material);
	}

	// Token: 0x04003A09 RID: 14857
	[SerializeField]
	private Colorblindness_Effect.ColorblindType m_colorblindType;

	// Token: 0x04003A0A RID: 14858
	private Material m_material;

	// Token: 0x020009A2 RID: 2466
	private enum ColorblindType
	{
		// Token: 0x04003A0C RID: 14860
		Protanopia,
		// Token: 0x04003A0D RID: 14861
		Deuteranopia,
		// Token: 0x04003A0E RID: 14862
		Tritanopia
	}
}

using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FXAA : MonoBehaviour
{
	// Token: 0x0600000E RID: 14 RVA: 0x00002ADE File Offset: 0x00000CDE
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetFloat(FXAA.sharpnessString, this.Sharpness);
		this.material.SetFloat(FXAA.thresholdString, this.Threshold);
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x0400000A RID: 10
	public Material material;

	// Token: 0x0400000B RID: 11
	public float Sharpness = 4f;

	// Token: 0x0400000C RID: 12
	public float Threshold = 0.2f;

	// Token: 0x0400000D RID: 13
	private static readonly int sharpnessString = Shader.PropertyToID("_Sharpness");

	// Token: 0x0400000E RID: 14
	private static readonly int thresholdString = Shader.PropertyToID("_Threshold");
}

using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FXAA : MonoBehaviour
{
	// Token: 0x0600000D RID: 13 RVA: 0x00003A78 File Offset: 0x00001C78
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetFloat(FXAA.sharpnessString, this.Sharpness);
		this.material.SetFloat(FXAA.thresholdString, this.Threshold);
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x04000008 RID: 8
	public Material material;

	// Token: 0x04000009 RID: 9
	public float Sharpness = 4f;

	// Token: 0x0400000A RID: 10
	public float Threshold = 0.2f;

	// Token: 0x0400000B RID: 11
	private static readonly int sharpnessString = Shader.PropertyToID("_Sharpness");

	// Token: 0x0400000C RID: 12
	private static readonly int thresholdString = Shader.PropertyToID("_Threshold");
}

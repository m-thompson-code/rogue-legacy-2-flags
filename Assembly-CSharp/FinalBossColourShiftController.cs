using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class FinalBossColourShiftController : MonoBehaviour
{
	// Token: 0x06000781 RID: 1921 RVA: 0x0005D8CC File Offset: 0x0005BACC
	public void ShiftColour(bool shiftToWhite, float lerpSpeed, bool isAdvanced)
	{
		if (FinalBossColourShiftController.m_colourShiftMatBlock == null)
		{
			FinalBossColourShiftController.m_colourShiftMatBlock = new MaterialPropertyBlock();
		}
		if (this.m_colourShiftLerpCoroutine != null)
		{
			base.StopCoroutine(this.m_colourShiftLerpCoroutine);
		}
		if (!shiftToWhite)
		{
			if (lerpSpeed > 0f)
			{
				this.m_colourShiftLerpCoroutine = base.StartCoroutine(this.ColourShiftLerpCoroutine(false, lerpSpeed, isAdvanced));
				return;
			}
			foreach (Renderer renderer in this.m_renderers)
			{
				renderer.GetPropertyBlock(FinalBossColourShiftController.m_colourShiftMatBlock);
				FinalBossColourShiftController.m_colourShiftMatBlock.SetFloat("_TextureBlend", 1f);
				renderer.SetPropertyBlock(FinalBossColourShiftController.m_colourShiftMatBlock);
			}
			return;
		}
		else
		{
			if (lerpSpeed > 0f)
			{
				this.m_colourShiftLerpCoroutine = base.StartCoroutine(this.ColourShiftLerpCoroutine(true, lerpSpeed, isAdvanced));
				return;
			}
			foreach (Renderer renderer2 in this.m_renderers)
			{
				renderer2.GetPropertyBlock(FinalBossColourShiftController.m_colourShiftMatBlock);
				FinalBossColourShiftController.m_colourShiftMatBlock.SetFloat("_TextureBlend", 0f);
				renderer2.SetPropertyBlock(FinalBossColourShiftController.m_colourShiftMatBlock);
			}
			return;
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x000059C7 File Offset: 0x00003BC7
	private IEnumerator ColourShiftLerpCoroutine(bool shiftToWhite, float lerpSpeed, bool isAdvanced)
	{
		if (!shiftToWhite)
		{
			Color black = Color.black;
		}
		else
		{
			Color white = Color.white;
		}
		base.GetComponent<EnemyController>();
		PlayerManager.GetPlayerController();
		float lerpDuration = lerpSpeed + Time.time;
		while (Time.time < lerpDuration)
		{
			float value;
			if (!shiftToWhite)
			{
				value = 1f - (lerpDuration - Time.time) / lerpSpeed;
			}
			else
			{
				value = (lerpDuration - Time.time) / lerpSpeed;
			}
			foreach (Renderer renderer in this.m_renderers)
			{
				renderer.GetPropertyBlock(FinalBossColourShiftController.m_colourShiftMatBlock);
				FinalBossColourShiftController.m_colourShiftMatBlock.SetFloat("_TextureBlend", value);
				renderer.SetPropertyBlock(FinalBossColourShiftController.m_colourShiftMatBlock);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000AD9 RID: 2777
	private const string TextureBlendShaderID = "_TextureBlend";

	// Token: 0x04000ADA RID: 2778
	[SerializeField]
	private Renderer[] m_renderers;

	// Token: 0x04000ADB RID: 2779
	private static MaterialPropertyBlock m_colourShiftMatBlock;

	// Token: 0x04000ADC RID: 2780
	private Coroutine m_colourShiftLerpCoroutine;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class FinalBossColourShiftController : MonoBehaviour
{
	// Token: 0x06000542 RID: 1346 RVA: 0x00016EA4 File Offset: 0x000150A4
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

	// Token: 0x06000543 RID: 1347 RVA: 0x00016F98 File Offset: 0x00015198
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

	// Token: 0x04000904 RID: 2308
	private const string TextureBlendShaderID = "_TextureBlend";

	// Token: 0x04000905 RID: 2309
	[SerializeField]
	private Renderer[] m_renderers;

	// Token: 0x04000906 RID: 2310
	private static MaterialPropertyBlock m_colourShiftMatBlock;

	// Token: 0x04000907 RID: 2311
	private Coroutine m_colourShiftLerpCoroutine;
}

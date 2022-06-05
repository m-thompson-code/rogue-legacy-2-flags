using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003C3 RID: 963
public class IntroSkyColourChanger : MonoBehaviour
{
	// Token: 0x17000E39 RID: 3641
	// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x00010C47 File Offset: 0x0000EE47
	public PatchType PatchTypeToUse
	{
		get
		{
			if (HolidayLookController.IsHoliday(HolidayType.Halloween))
			{
				return PatchType.HallowsEve;
			}
			if (HolidayLookController.IsHoliday(HolidayType.Christmas))
			{
				return PatchType.WinterWonderland;
			}
			return this.m_patchTypeToUse;
		}
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x00010C63 File Offset: 0x0000EE63
	private void Awake()
	{
		this.m_matBlock = new MaterialPropertyBlock();
		this.SetSkyColor(this.PatchTypeToUse);
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000A398C File Offset: 0x000A1B8C
	public void SetSkyColor(PatchType patchTypeToUse)
	{
		IntroSkyColorEntry introSkyColorEntry;
		if (this.m_skyColorTable.TryGetValue(patchTypeToUse, out introSkyColorEntry))
		{
			this.m_skyRenderer.GetPropertyBlock(this.m_matBlock);
			this.m_matBlock.SetColor("_SkyColor", introSkyColorEntry.SkyColor);
			this.m_matBlock.SetColor("_HorizonColor", introSkyColorEntry.HorizonColor);
			this.m_matBlock.SetColor("_CloudMainColor", introSkyColorEntry.CloudMainColor);
			this.m_matBlock.SetColor("_CloudHighlightColor", introSkyColorEntry.CloudHighlightColor);
			this.m_matBlock.SetColor("_StarColor", introSkyColorEntry.StarColor);
			this.m_matBlock.SetColor("_RightColor", introSkyColorEntry.RightColor);
			this.m_matBlock.SetColor("_BottomColor", introSkyColorEntry.BottomColor);
			this.m_skyRenderer.SetPropertyBlock(this.m_matBlock);
			this.m_mistRenderer.GetPropertyBlock(this.m_matBlock);
			this.m_matBlock.SetColor("_MistColor", introSkyColorEntry.MistColor);
			this.m_mistRenderer.SetPropertyBlock(this.m_matBlock);
			this.m_beam1ParticleSystem.main.startColor = introSkyColorEntry.Beam1Color;
			this.m_beam2ParticleSystem.main.startColor = introSkyColorEntry.Beam2Color;
			ParticleSystem.MainModule main = this.m_rubbleParticleSystem.main;
			ParticleSystem.MinMaxGradient startColor = main.startColor;
			startColor.colorMin = introSkyColorEntry.RubbleColor1;
			startColor.colorMax = introSkyColorEntry.RubbleColor2;
			main.startColor = startColor;
			this.m_glowRenderer.color = introSkyColorEntry.GlowColor;
			this.m_postProcessing.SetBaseProfile(introSkyColorEntry.PostProcessProfile);
			RenderSettings.fogColor = introSkyColorEntry.FogRenderColor;
			this.m_logoBannerRenderer.sprite = introSkyColorEntry.BannerSprite;
			return;
		}
		Debug.Log("<color=yellow>WARNING: Failed to change intro sky colour. Patch Type: " + patchTypeToUse.ToString() + " not defined in sky color table.</color>");
	}

	// Token: 0x04001C51 RID: 7249
	[SerializeField]
	private PatchType m_patchTypeToUse;

	// Token: 0x04001C52 RID: 7250
	[SerializeField]
	private PatchTypeSkyColorEntryDictionary m_skyColorTable;

	// Token: 0x04001C53 RID: 7251
	[Space(10f)]
	[SerializeField]
	private Renderer m_skyRenderer;

	// Token: 0x04001C54 RID: 7252
	[SerializeField]
	[FormerlySerializedAs("m_fogRenderer")]
	private Renderer m_mistRenderer;

	// Token: 0x04001C55 RID: 7253
	[SerializeField]
	private ParticleSystem m_beam1ParticleSystem;

	// Token: 0x04001C56 RID: 7254
	[SerializeField]
	private ParticleSystem m_beam2ParticleSystem;

	// Token: 0x04001C57 RID: 7255
	[SerializeField]
	private ParticleSystem m_rubbleParticleSystem;

	// Token: 0x04001C58 RID: 7256
	[SerializeField]
	private SpriteRenderer m_glowRenderer;

	// Token: 0x04001C59 RID: 7257
	[SerializeField]
	private MobilePostProcessing m_postProcessing;

	// Token: 0x04001C5A RID: 7258
	[SerializeField]
	private SpriteRenderer m_logoBannerRenderer;

	// Token: 0x04001C5B RID: 7259
	private MaterialPropertyBlock m_matBlock;
}

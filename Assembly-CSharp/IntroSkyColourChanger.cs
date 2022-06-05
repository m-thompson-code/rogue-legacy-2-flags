using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000210 RID: 528
public class IntroSkyColourChanger : MonoBehaviour
{
	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x0600162A RID: 5674 RVA: 0x000451D7 File Offset: 0x000433D7
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

	// Token: 0x0600162B RID: 5675 RVA: 0x000451F3 File Offset: 0x000433F3
	private void Awake()
	{
		this.m_matBlock = new MaterialPropertyBlock();
		this.SetSkyColor(this.PatchTypeToUse);
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x0004520C File Offset: 0x0004340C
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

	// Token: 0x0400154E RID: 5454
	[SerializeField]
	private PatchType m_patchTypeToUse;

	// Token: 0x0400154F RID: 5455
	[SerializeField]
	private PatchTypeSkyColorEntryDictionary m_skyColorTable;

	// Token: 0x04001550 RID: 5456
	[Space(10f)]
	[SerializeField]
	private Renderer m_skyRenderer;

	// Token: 0x04001551 RID: 5457
	[SerializeField]
	[FormerlySerializedAs("m_fogRenderer")]
	private Renderer m_mistRenderer;

	// Token: 0x04001552 RID: 5458
	[SerializeField]
	private ParticleSystem m_beam1ParticleSystem;

	// Token: 0x04001553 RID: 5459
	[SerializeField]
	private ParticleSystem m_beam2ParticleSystem;

	// Token: 0x04001554 RID: 5460
	[SerializeField]
	private ParticleSystem m_rubbleParticleSystem;

	// Token: 0x04001555 RID: 5461
	[SerializeField]
	private SpriteRenderer m_glowRenderer;

	// Token: 0x04001556 RID: 5462
	[SerializeField]
	private MobilePostProcessing m_postProcessing;

	// Token: 0x04001557 RID: 5463
	[SerializeField]
	private SpriteRenderer m_logoBannerRenderer;

	// Token: 0x04001558 RID: 5464
	private MaterialPropertyBlock m_matBlock;
}

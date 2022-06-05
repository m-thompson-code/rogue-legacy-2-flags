using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000007 RID: 7
[ExecuteInEditMode]
public class MobilePostProcessing : MonoBehaviour
{
	// Token: 0x06000018 RID: 24 RVA: 0x0003FDB8 File Offset: 0x0003DFB8
	private void ResetBlur()
	{
		this.Blur = false;
		this.BlurOverallAmount = 1f;
		this.BlurBaselineAmount = 0f;
		this.BlurRedChannel = 0f;
		this.BlurGreenChannel = 0f;
		this.BlurBlueChannel = 0f;
		this.BlurAlphaChannel = 0f;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0003FE10 File Offset: 0x0003E010
	private void ApplyProfileBlur(MobilePostProcessingProfile profile)
	{
		if (profile.EnableBlurEffect)
		{
			this.Blur = true;
			if (profile.OverrideBlurOverallAmount)
			{
				this.BlurOverallAmount = profile.BlurOverallAmount;
			}
			if (profile.OverrideBlurBaselineAmount)
			{
				this.BlurBaselineAmount = profile.BlurBaselineAmount;
			}
			if (profile.OverrideBlurRedChannel)
			{
				this.BlurRedChannel = profile.BlurRedChannel;
			}
			if (profile.OverrideBlurGreenChannel)
			{
				this.BlurGreenChannel = profile.BlurGreenChannel;
			}
			if (profile.OverrideBlurBlueChannel)
			{
				this.BlurBlueChannel = profile.BlurBlueChannel;
			}
			if (profile.OverrideBlurAlphaChannel)
			{
				this.BlurAlphaChannel = profile.BlurAlphaChannel;
			}
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0003FEA4 File Offset: 0x0003E0A4
	private void ApplyShaderBlur()
	{
		if (this.Blur && this.BlurOverallAmount > 0f)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.BlurKeyword);
			int num = Mathf.Max(Mathf.CeilToInt(this.BlurOverallAmount * 4f), 1);
			this.m_material.SetFloat(MobilePostProcessing.ID_blurOverallAmount, (num > 1) ? ((this.BlurOverallAmount * 4f - (float)Mathf.FloorToInt(this.BlurOverallAmount * 4f - 0.001f)) * 0.5f + 0.5f) : (this.BlurOverallAmount * 4f));
			this.m_material.SetFloat(MobilePostProcessing.ID_blurBaselineAmount, this.BlurBaselineAmount);
			this.m_material.SetVector(MobilePostProcessing.ID_blurRGBAChannels, new Vector4(this.BlurRedChannel, this.BlurGreenChannel, this.BlurBlueChannel, this.BlurAlphaChannel));
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.BlurKeyword);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002BB1 File Offset: 0x00000DB1
	private void ResetBloom()
	{
		this.Bloom = false;
		this.BloomColor = MobilePostProcessing.BLOOM_COLOR_DEFAULT;
		this.BloomAmount = 1f;
		this.BloomDiffuse = 0.5f;
		this.BloomThreshold = 0.5f;
		this.BloomSoftness = 0.5f;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x0003FF9C File Offset: 0x0003E19C
	private void ApplyProfileBloom(MobilePostProcessingProfile profile)
	{
		if (profile.EnableBloomEffect)
		{
			this.Bloom = true;
			if (profile.OverrideBloomColor)
			{
				this.BloomColor = profile.BloomColor;
			}
			if (profile.OverrideBloomAmount)
			{
				this.BloomAmount = profile.BloomAmount;
			}
			if (profile.OverrideBloomDiffuse)
			{
				this.BloomDiffuse = profile.BloomDiffuse;
			}
			if (profile.OverrideBloomThreshold)
			{
				this.BloomThreshold = profile.BloomThreshold;
			}
			if (profile.OverrideBloomSoftness)
			{
				this.BloomSoftness = profile.BloomSoftness;
			}
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0004001C File Offset: 0x0003E21C
	private void ApplyShaderBloom()
	{
		if (this.Bloom && this.BloomAmount > 0f)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.BloomKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_bloomColor, this.BloomColor * this.BloomAmount);
			this.m_material.SetFloat(MobilePostProcessing.ID_bloomDiffuse, this.BloomDiffuse);
			int num = Mathf.Max(Mathf.CeilToInt(this.BloomDiffuse * 4f), 1);
			this.m_material.SetFloat(MobilePostProcessing.ID_bloomDiffuse, (num > 1) ? ((this.BloomDiffuse * 4f - (float)Mathf.FloorToInt(this.BloomDiffuse * 4f - 0.001f)) * 0.5f + 0.5f) : (this.BloomDiffuse * 4f));
			float num2 = this.BloomThreshold * this.BloomSoftness;
			this.m_material.SetVector(MobilePostProcessing.ID_bloomData, new Vector4(this.BloomThreshold, this.BloomThreshold - num2, 2f * num2, 1f / (4f * num2 + 1E-05f)));
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.BloomKeyword);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002BF1 File Offset: 0x00000DF1
	private void ResetLut()
	{
		this.LUT = false;
		this.LutAmount = 0.5f;
		this.SourceLut = null;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002C0C File Offset: 0x00000E0C
	private void ApplyProfileLut(MobilePostProcessingProfile profile)
	{
		if (profile.EnableLUTEffect)
		{
			this.LUT = true;
			if (profile.OverrideLutAmount)
			{
				this.LutAmount = profile.LutAmount;
			}
			if (profile.OverrideSourceLut)
			{
				this.SourceLut = profile.SourceLut;
			}
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00040154 File Offset: 0x0003E354
	private void ApplyShaderLut()
	{
		if (this.SourceLut != this.m_previousLut)
		{
			this.m_previousLut = this.SourceLut;
			if (this.SourceLut != null)
			{
				this.Convert3D(this.SourceLut);
			}
		}
		if (this.LUT)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.LutKeyword);
			this.m_material.SetFloat(MobilePostProcessing.ID_lutAmount, this.LutAmount);
			this.m_material.SetTexture(MobilePostProcessing.ID_lutTexture, this.m_converted3D);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.LutKeyword);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000401F0 File Offset: 0x0003E3F0
	private void Convert3D(Texture2D temp3DTex)
	{
		Color[] pixels = temp3DTex.GetPixels();
		Color[] array = new Color[pixels.Length];
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					int num = 16 - j - 1;
					array[i + j * 16 + k * 256] = pixels[k * 16 + i + num * 256];
				}
			}
		}
		this.m_converted3D.SetPixels(array);
		this.m_converted3D.Apply();
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00040284 File Offset: 0x0003E484
	private void ResetImageFiltering()
	{
		this.ImageFiltering = false;
		this.Color = MobilePostProcessing.COLOR_DEFAULT;
		this.Contrast = 0f;
		this.Brightness = 0f;
		this.Saturation = 0f;
		this.Exposure = 0f;
		this.Gamma = 0f;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000402DC File Offset: 0x0003E4DC
	private void ApplyProfileImageFiltering(MobilePostProcessingProfile profile)
	{
		if (profile.EnableImageFilteringEffect)
		{
			this.ImageFiltering = true;
			if (profile.OverrideColor)
			{
				this.Color = profile.Color;
			}
			if (profile.OverrideContrast)
			{
				this.Contrast = profile.Contrast;
			}
			if (profile.OverrideBrightness)
			{
				this.Brightness = profile.Brightness;
			}
			if (profile.OverrideSaturation)
			{
				this.Saturation = profile.Saturation;
			}
			if (profile.OverrideExposure)
			{
				this.Exposure = profile.Exposure;
			}
			if (profile.OverrideGamma)
			{
				this.Gamma = profile.Gamma;
			}
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00040370 File Offset: 0x0003E570
	private void ApplyShaderImageFiltering()
	{
		if (this.ImageFiltering)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.FilterKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_filterColor, (Mathf.Pow(2f, this.Exposure) - this.Gamma) * this.Color);
			this.m_material.SetFloat(MobilePostProcessing.ID_filterContrast, this.Contrast + 1f);
			this.m_material.SetFloat(MobilePostProcessing.ID_filterBrightness, this.Brightness * 0.5f - this.Contrast);
			this.m_material.SetFloat(MobilePostProcessing.ID_filterSaturation, this.Saturation + 1f);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.FilterKeyword);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00040438 File Offset: 0x0003E638
	private void ResetGradientMap()
	{
		this.GradientMap = false;
		this.GradientBase = MobilePostProcessing.GRADIENT_BASE_DEFAULT;
		this.GradientDark = MobilePostProcessing.GRADIENT_DARK_DEFAULT;
		this.GradientMid = MobilePostProcessing.GRADIENT_MID_DEFAULT;
		this.GradientLight = MobilePostProcessing.GRADIENT_LIGHT_DEFAULT;
		this.GradientAmount = 0.5f;
		this.DarkCutoff = 0f;
		this.LightCutoff = 1f;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x0004049C File Offset: 0x0003E69C
	private void ApplyProfileGradientMap(MobilePostProcessingProfile profile)
	{
		if (profile.EnableGradientMapEffect)
		{
			this.GradientMap = true;
			if (profile.OverrideGradientBase)
			{
				this.GradientBase = profile.GradientBase;
			}
			if (profile.OverrideGradientDark)
			{
				this.GradientDark = profile.GradientDark;
			}
			if (profile.OverrideGradientMid)
			{
				this.GradientMid = profile.GradientMid;
			}
			if (profile.OverrideGradientLight)
			{
				this.GradientLight = profile.GradientLight;
			}
			if (profile.OverrideGradientAmount)
			{
				this.GradientAmount = profile.GradientAmount;
			}
			if (profile.OverrideDarkCutoff)
			{
				this.DarkCutoff = profile.DarkCutoff;
			}
			if (profile.OverrideLightCutoff)
			{
				this.LightCutoff = profile.LightCutoff;
			}
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00040548 File Offset: 0x0003E748
	private void ApplyShaderGradientMap()
	{
		if (this.GradientMap)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.GradientKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_gradientBase, this.GradientBase);
			this.m_material.SetColor(MobilePostProcessing.ID_gradientDark, this.GradientDark);
			this.m_material.SetColor(MobilePostProcessing.ID_gradientMid, this.GradientMid);
			this.m_material.SetColor(MobilePostProcessing.ID_gradientLight, this.GradientLight);
			this.m_material.SetFloat(MobilePostProcessing.ID_gradientAmount, this.GradientAmount);
			this.m_material.SetFloat(MobilePostProcessing.ID_gradientDarkCutoff, this.DarkCutoff);
			this.m_material.SetFloat(MobilePostProcessing.ID_gradientLightCutoff, this.LightCutoff);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.GradientKeyword);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002C45 File Offset: 0x00000E45
	private void ResetOverlayGradient()
	{
		this.OverlayGradient = false;
		this.OverlayTop = MobilePostProcessing.OVERLAY_TOP_DEFAULT;
		this.OverlayBottom = MobilePostProcessing.OVERLAY_BOTTOM_DEFAULT;
		this.OverlayAmount = 0.5f;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x0004061C File Offset: 0x0003E81C
	private void ApplyProfileOverlayGradient(MobilePostProcessingProfile profile)
	{
		if (profile.EnableOverlayGradientEffect)
		{
			this.OverlayGradient = true;
			if (profile.OverrideOverlayTop)
			{
				this.OverlayTop = profile.OverlayTop;
			}
			if (profile.OverrideOverlayBottom)
			{
				this.OverlayBottom = profile.OverlayBottom;
			}
			if (profile.OverrideOverlayAmount)
			{
				this.OverlayAmount = profile.OverlayAmount;
			}
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00040674 File Offset: 0x0003E874
	private void ApplyShaderOverlayGradient()
	{
		if (this.OverlayGradient)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.OverlayKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_overlayTop, this.OverlayTop);
			this.m_material.SetColor(MobilePostProcessing.ID_overlayBottom, this.OverlayBottom);
			this.m_material.SetFloat(MobilePostProcessing.ID_overlayAmount, this.OverlayAmount);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.OverlayKeyword);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002C6F File Offset: 0x00000E6F
	private void ResetClampBlack()
	{
		this.ClampBlack = false;
		this.ClampColor = MobilePostProcessing.CLAMP_COLOR_DEFAULT;
		this.ClampFillAmount = 0.5f;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002C8E File Offset: 0x00000E8E
	private void ApplyProfileClampBlack(MobilePostProcessingProfile profile)
	{
		if (profile.EnableClampBlackEffect)
		{
			this.ClampBlack = true;
			if (profile.OverrideClampColor)
			{
				this.ClampColor = profile.ClampColor;
			}
			if (profile.OverrideClampFillAmount)
			{
				this.ClampFillAmount = profile.ClampFillAmount;
			}
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000406EC File Offset: 0x0003E8EC
	private void ApplyShaderClampBlack()
	{
		if (this.ClampBlack)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.ClampKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_clampColor, this.ClampColor);
			this.m_material.SetFloat(MobilePostProcessing.ID_clampFillAmount, this.ClampFillAmount);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.ClampKeyword);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00040750 File Offset: 0x0003E950
	private void ResetTint()
	{
		this.Tint = false;
		this.TintColor = MobilePostProcessing.TINT_COLOR_DEFAULT;
		this.TintOverallAmount = 1f;
		this.TintBaselineAmount = 0f;
		this.TintRedChannel = 0f;
		this.TintGreenChannel = 0f;
		this.TintBlueChannel = 0f;
		this.TintAlphaChannel = 0f;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000407B4 File Offset: 0x0003E9B4
	private void ApplyProfileTint(MobilePostProcessingProfile profile)
	{
		if (profile.EnableTintEffect)
		{
			this.Tint = true;
			if (profile.OverrideTintColor)
			{
				this.TintColor = profile.TintColor;
			}
			if (profile.OverrideTintOverallAmount)
			{
				this.TintOverallAmount = profile.TintOverallAmount;
			}
			if (profile.OverrideTintBaselineAmount)
			{
				this.TintBaselineAmount = profile.TintBaselineAmount;
			}
			if (profile.OverrideTintRedChannel)
			{
				this.TintRedChannel = profile.TintRedChannel;
			}
			if (profile.OverrideTintGreenChannel)
			{
				this.TintGreenChannel = profile.TintGreenChannel;
			}
			if (profile.OverrideTintBlueChannel)
			{
				this.TintBlueChannel = profile.TintBlueChannel;
			}
			if (profile.OverrideTintAlphaChannel)
			{
				this.TintAlphaChannel = profile.TintAlphaChannel;
			}
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00040860 File Offset: 0x0003EA60
	private void ApplyShaderTint()
	{
		if (this.Tint)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.TintKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_tintColor, this.TintColor);
			this.m_material.SetFloat(MobilePostProcessing.ID_tintOverallAmount, this.TintOverallAmount);
			this.m_material.SetFloat(MobilePostProcessing.ID_tintBaselineAmount, this.TintBaselineAmount);
			this.m_material.SetVector(MobilePostProcessing.ID_tintRGBAChannels, new Vector4(this.TintRedChannel, this.TintGreenChannel, this.TintBlueChannel, this.TintAlphaChannel));
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.TintKeyword);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002CC7 File Offset: 0x00000EC7
	private void ResetChromaticAbberation()
	{
		this.ChromaticAberration = false;
		this.Offset = 1f;
		this.FishEyeDistortion = 0f;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002CE6 File Offset: 0x00000EE6
	private void ApplyProfileChromaticAbberation(MobilePostProcessingProfile profile)
	{
		if (profile.EnableChromaticAbberationEffect)
		{
			this.ChromaticAberration = true;
			if (profile.OverrideOffset)
			{
				this.Offset = profile.Offset;
			}
			if (profile.OverrideFishEyeDistortion)
			{
				this.FishEyeDistortion = profile.FishEyeDistortion;
			}
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00040908 File Offset: 0x0003EB08
	private void ApplyShaderChromaticAbberation()
	{
		if (this.ChromaticAberration)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.ChromaKeyword);
			this.m_material.SetFloat(MobilePostProcessing.ID_chromaOffset, 10f * this.Offset);
			this.m_material.SetFloat(MobilePostProcessing.ID_chromaFishEye, 0.1f * this.FishEyeDistortion);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.ChromaKeyword);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002D1F File Offset: 0x00000F1F
	private void ResetLensDistortion()
	{
		this.Distortion = false;
		this.LensDistortion = 0.5f;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002D33 File Offset: 0x00000F33
	private void ApplyProfileDistortion(MobilePostProcessingProfile profile)
	{
		if (profile.EnableDistortionEffect)
		{
			this.Distortion = true;
			if (profile.OverrideLensDistortion)
			{
				this.LensDistortion = profile.LensDistortion;
			}
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00040978 File Offset: 0x0003EB78
	private void ApplyShaderDistortion()
	{
		if (this.Distortion)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.DistortionKeyword);
			this.m_material.SetFloat(MobilePostProcessing.ID_lensdistortion, -this.LensDistortion);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.DistortionKeyword);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002D58 File Offset: 0x00000F58
	private void ResetVignette()
	{
		this.Vignette = false;
		this.VignetteCenter = MobilePostProcessing.VIGNETTE_CENTER_DEFAULT;
		this.VignetteColor = MobilePostProcessing.VIGNETTE_COLOR_DEFAULT;
		this.VignetteAmount = 0.5f;
		this.VignetteSoftness = 0.5f;
		this.VignetteRoundness = 0.5f;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000409C8 File Offset: 0x0003EBC8
	private void ApplyProfileVignette(MobilePostProcessingProfile profile)
	{
		if (profile.EnableVignetteEffect)
		{
			this.Vignette = true;
			if (profile.OverrideVignetteCenter)
			{
				this.VignetteCenter = profile.VignetteCenter;
			}
			if (profile.OverrideVignetteColor)
			{
				this.VignetteColor = profile.VignetteColor;
			}
			if (profile.OverrideVignetteAmount)
			{
				this.VignetteAmount = profile.VignetteAmount;
			}
			if (profile.OverrideVignetteSoftness)
			{
				this.VignetteSoftness = profile.VignetteSoftness;
			}
			if (profile.OverrideVignetteRoundness)
			{
				this.VignetteRoundness = profile.VignetteRoundness;
			}
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00040A48 File Offset: 0x0003EC48
	private void ApplyShaderVignette()
	{
		if (this.Vignette)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.VignetteKeyword);
			this.m_material.SetVector(MobilePostProcessing.ID_vignetteCenter, this.VignetteCenter);
			this.m_material.SetColor(MobilePostProcessing.ID_vignetteColor, this.VignetteColor);
			this.m_material.SetFloat(MobilePostProcessing.ID_vignetteAmount, 1f - this.VignetteAmount);
			this.m_material.SetFloat(MobilePostProcessing.ID_vignetteSoftness, 1f - this.VignetteSoftness - this.VignetteAmount);
			float value = 6f - this.VignetteRoundness * 4f;
			this.m_material.SetFloat(MobilePostProcessing.ID_vignetteRoundness, value);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.VignetteKeyword);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002D98 File Offset: 0x00000F98
	private void ResetMist()
	{
		this.Mist = false;
		this.MistColor = MobilePostProcessing.MIST_COLOR_DEFAULT;
		this.MistAmount = 0.1f;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002DB7 File Offset: 0x00000FB7
	private void ApplyProfileMist(MobilePostProcessingProfile profile)
	{
		if (profile.EnableMistEffect)
		{
			this.Mist = true;
			if (profile.OverrideMistColor)
			{
				this.MistColor = profile.MistColor;
			}
			if (profile.OverrideMistAmount)
			{
				this.MistAmount = profile.MistAmount;
			}
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00040B18 File Offset: 0x0003ED18
	private void ApplyShaderMist()
	{
		if (this.Mist)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.MistKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_mistColor, this.MistColor);
			this.m_material.SetFloat(MobilePostProcessing.ID_mistAmount, this.MistAmount * 10f);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.MistKeyword);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00040B80 File Offset: 0x0003ED80
	private void ResetPixelation()
	{
		this.Pixelation = false;
		this.PixelResolution = MobilePostProcessing.PIXEL_RESOLUTION_DEFAULT;
		this.PixelBaselineAmount = 0f;
		this.PixelRedChannel = 0f;
		this.PixelGreenChannel = 0f;
		this.PixelBlueChannel = 0f;
		this.PixelAlphaChannel = 0f;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00040BD8 File Offset: 0x0003EDD8
	private void ApplyProfilePixelation(MobilePostProcessingProfile profile)
	{
		if (profile.EnablePixelationEffect)
		{
			this.Pixelation = true;
			if (profile.OverridePixelResolution)
			{
				this.PixelResolution = profile.PixelResolution;
			}
			if (profile.OverridePixelBaselineAmount)
			{
				this.PixelBaselineAmount = profile.PixelBaselineAmount;
			}
			if (profile.OverridePixelRedChannel)
			{
				this.PixelRedChannel = profile.PixelRedChannel;
			}
			if (profile.OverridePixelGreenChannel)
			{
				this.PixelGreenChannel = profile.PixelGreenChannel;
			}
			if (profile.OverridePixelBlueChannel)
			{
				this.PixelBlueChannel = profile.PixelBlueChannel;
			}
			if (profile.OverridePixelAlphaChannel)
			{
				this.PixelAlphaChannel = profile.PixelAlphaChannel;
			}
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00040C6C File Offset: 0x0003EE6C
	private void ApplyShaderPixelation()
	{
		if (this.Pixelation)
		{
			this.m_material.SetVector(MobilePostProcessing.ID_pixelResolution, this.PixelResolution);
			this.m_material.SetFloat(MobilePostProcessing.ID_pixelBaselineAmount, this.PixelBaselineAmount);
			this.m_material.SetVector(MobilePostProcessing.ID_pixelRGBAChannels, new Vector4(this.PixelRedChannel, this.PixelGreenChannel, this.PixelBlueChannel, this.PixelAlphaChannel));
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002DF0 File Offset: 0x00000FF0
	private void ResetDither()
	{
		this.Dither = false;
		this.DitherDark = MobilePostProcessing.DITHER_DARK_DEFAULT;
		this.DitherLight = MobilePostProcessing.DITHER_LIGHT_DEFAULT;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00002E0F File Offset: 0x0000100F
	private void ApplyProfileDither(MobilePostProcessingProfile profile)
	{
		if (profile.EnableDitherEffect)
		{
			this.Dither = true;
			if (profile.OverrideDitherDark)
			{
				this.DitherDark = profile.DitherDark;
			}
			if (profile.OverrideDitherLight)
			{
				this.DitherLight = profile.DitherLight;
			}
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00040CE0 File Offset: 0x0003EEE0
	private void ApplyShaderDither()
	{
		if (this.Dither)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.DitherKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_ditherDark, this.DitherDark);
			this.m_material.SetColor(MobilePostProcessing.ID_ditherLight, this.DitherLight);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.DitherKeyword);
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00002E48 File Offset: 0x00001048
	private void ResetScreenFlip()
	{
		this.ScreenFlipX = false;
		this.ScreenFlipY = false;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00002E58 File Offset: 0x00001058
	private void ApplyProfileScreenFlip(MobilePostProcessingProfile profile)
	{
		if (profile.OverrideScreenFlipXEffect)
		{
			this.ScreenFlipX = profile.ScreenFlipX;
		}
		if (profile.OverrideScreenFlipYEffect)
		{
			this.ScreenFlipY = profile.ScreenFlipY;
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00040D44 File Offset: 0x0003EF44
	private void ApplyShaderScreenFlip()
	{
		if (this.ScreenFlipX)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.ScreenFlipXKeyword);
		}
		else
		{
			this.m_material.DisableKeyword(MobilePostProcessing.ScreenFlipXKeyword);
		}
		if (this.ScreenFlipY)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.ScreenFlipYKeyword);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.ScreenFlipYKeyword);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002E82 File Offset: 0x00001082
	private void ResetAnimatedOverlay()
	{
		this.AnimatedOverlay = false;
		this.AnimOverlayTexture = MobilePostProcessing.ANIMOVERLAY_TEXTURE_DEFAULT;
		this.AnimOverlaySpritesheetSize = MobilePostProcessing.ANIMOVERLAY_SPRITESHEET_SIZE_DEFAULT;
		this.AnimOverlayFramerate = 30;
		this.AnimOverlayStrength = 0.5f;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00040DA4 File Offset: 0x0003EFA4
	private void ApplyProfileAnimatedOverlay(MobilePostProcessingProfile profile)
	{
		if (profile.EnableAnimatedOverlayEffect)
		{
			this.AnimatedOverlay = true;
			if (profile.OverrideAnimOverlayTexture)
			{
				this.AnimOverlayTexture = profile.AnimOverlayTexture;
			}
			if (profile.OverrideAnimOverlaySpritesheetSize)
			{
				this.AnimOverlaySpritesheetSize = profile.AnimOverlaySpritesheetSize;
			}
			if (profile.OverrideAnimOverlayFramerate)
			{
				this.AnimOverlayFramerate = profile.AnimOverlayFramerate;
			}
			if (profile.OverrideAnimOverlayStrength)
			{
				this.AnimOverlayStrength = profile.AnimOverlayStrength;
			}
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00040E10 File Offset: 0x0003F010
	private void ApplyShaderAnimatedOverlay()
	{
		if (this.AnimatedOverlay)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.AnimOverlayKeyword);
			this.m_material.SetTexture(MobilePostProcessing.ID_animOverlayTexture, this.AnimOverlayTexture);
			this.m_material.SetVector(MobilePostProcessing.ID_animOverlaySpritesheetSize, this.AnimOverlaySpritesheetSize);
			this.m_material.SetFloat(MobilePostProcessing.ID_animOverlayFramerate, (float)this.AnimOverlayFramerate);
			this.m_material.SetFloat(MobilePostProcessing.ID_animOverlayStrength, this.AnimOverlayStrength);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.AnimOverlayKeyword);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00002EB4 File Offset: 0x000010B4
	private void ResetCircularDarkness()
	{
		this.CircularDarkness = false;
		this.CircDarknessColor = MobilePostProcessing.CIRCDARKNESS_COLOR_DEFAULT;
		this.CircDarknessAmount = 0.5f;
		this.CircDarknessSoftness = 0.5f;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00040EA4 File Offset: 0x0003F0A4
	private void ApplyProfileCircularDarkness(MobilePostProcessingProfile profile)
	{
		if (profile.EnableCircularDarknessEffect)
		{
			this.CircularDarkness = true;
			if (profile.OverrideCircDarknessColor)
			{
				this.CircDarknessColor = profile.CircDarknessColor;
			}
			if (profile.OverrideCircDarknessAmount)
			{
				this.CircDarknessAmount = profile.CircDarknessAmount;
			}
			if (profile.OverrideCircDarknessSoftness)
			{
				this.CircDarknessSoftness = profile.CircDarknessSoftness;
			}
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00040EFC File Offset: 0x0003F0FC
	private void ApplyShaderCircularDarkness()
	{
		if (this.CircularDarkness)
		{
			this.m_material.EnableKeyword(MobilePostProcessing.CircDarknessKeyword);
			this.m_material.SetColor(MobilePostProcessing.ID_circDarknessColor, this.CircDarknessColor);
			this.m_material.SetFloat(MobilePostProcessing.ID_circDarknessAmount, 1f - this.CircDarknessAmount);
			this.m_material.SetFloat(MobilePostProcessing.ID_circDarknessSoftness, 1f - this.CircDarknessSoftness - this.CircDarknessAmount);
			return;
		}
		this.m_material.DisableKeyword(MobilePostProcessing.CircDarknessKeyword);
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00040F88 File Offset: 0x0003F188
	public void Start()
	{
		if (this.m_material == null)
		{
			this.m_material = new Material(Shader.Find("SupGames/Mobile/PostProcess"));
		}
		if (this.Mask == null)
		{
			Shader.SetGlobalTexture(MobilePostProcessing.ID_maskTexture, Texture2D.whiteTexture);
		}
		else
		{
			Shader.SetGlobalTexture(MobilePostProcessing.ID_maskTexture, this.Mask);
		}
		this.m_converted3D = new Texture3D(16, 16, 16, TextureFormat.ARGB32, false);
		this.m_converted3D.wrapMode = TextureWrapMode.Clamp;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00041008 File Offset: 0x0003F208
	public void LateUpdate()
	{
		if (this.baseProfile != this.m_previousProfile)
		{
			this.m_isDirty = true;
		}
		if (this.m_isDirty)
		{
			this.ApplyEffects();
			this.m_isDirty = false;
		}
		if (this.CircularDarkness && PlayerManager.IsInstantiated && !PlayerManager.IsDisposed && CameraController.ForegroundPerspCam)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			Vector2 v = CameraController.ForegroundPerspCam.WorldToViewportPoint(playerController.Midpoint);
			this.m_material.SetVector(MobilePostProcessing.ID_circDarknessCenter, v);
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00002EDE File Offset: 0x000010DE
	private void OnDestroy()
	{
		if (this.m_converted3D != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_converted3D);
		}
		this.m_converted3D = null;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00002F00 File Offset: 0x00001100
	public void AddTraitOverride(MobilePostProcessingProfile overrideProfile)
	{
		this.m_traitOverrides.Add(overrideProfile);
		this.m_isDirty = true;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00002F15 File Offset: 0x00001115
	public void RemoveTraitOverride(MobilePostProcessingProfile overrideProfile)
	{
		this.m_traitOverrides.Remove(overrideProfile);
		this.m_isDirty = true;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00002F2B File Offset: 0x0000112B
	public void AddDimensionOverride(MobilePostProcessingProfile dimensionProfile)
	{
		this.m_dimensionOverrides.Add(dimensionProfile);
		this.m_isDirty = true;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00002F40 File Offset: 0x00001140
	public void RemoveDimensionOverride(MobilePostProcessingProfile dimensionProfile)
	{
		this.m_dimensionOverrides.Remove(dimensionProfile);
		this.m_isDirty = true;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00002F56 File Offset: 0x00001156
	public void SetBaseProfile(MobilePostProcessingProfile profile)
	{
		this.StopLerpToProfile();
		this.baseProfile = profile;
		this.m_isDirty = true;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00002F6C File Offset: 0x0000116C
	public void ForceDirty()
	{
		this.m_isDirty = true;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00002F75 File Offset: 0x00001175
	public void LerpToProfile(MobilePostProcessingProfile profile, float lerpSpeed)
	{
		this.StopLerpToProfile();
		if (this.baseProfile)
		{
			this.m_lerpToProfileCoroutine = base.StartCoroutine(this.LerpToProfileCoroutine(profile, lerpSpeed));
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00002F9E File Offset: 0x0000119E
	private IEnumerator LerpToProfileCoroutine(MobilePostProcessingProfile profile, float lerpSpeed)
	{
		if (this.baseProfile.EnableBlurEffect && profile.EnableBlurEffect)
		{
			this.m_tweenArray[0] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"BlurOverallAmount",
				profile.BlurOverallAmount,
				"BlurBaselineAmount",
				profile.BlurBaselineAmount,
				"BlurRedChannel",
				profile.BlurRedChannel,
				"BlurGreenChannel",
				profile.BlurGreenChannel,
				"BlurBlueChannel",
				profile.BlurBlueChannel,
				"BlurAlphaChannel",
				profile.BlurAlphaChannel
			});
		}
		if (this.baseProfile.EnableBloomEffect && profile.EnableBloomEffect)
		{
			this.m_tweenArray[1] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"BloomAmount",
				profile.BloomAmount,
				"BloomDiffuse",
				profile.BloomDiffuse,
				"BloomThreshold",
				profile.BloomThreshold,
				"BloomSoftness",
				profile.BloomSoftness,
				"BloomColor.r",
				profile.BloomColor.r,
				"BloomColor.g",
				profile.BloomColor.g,
				"BloomColor.b",
				profile.BloomColor.b,
				"BloomColor.a",
				profile.BloomColor.a
			});
		}
		if (this.baseProfile.EnableImageFilteringEffect && profile.EnableImageFilteringEffect)
		{
			this.m_tweenArray[2] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"Color.r",
				profile.Color.r,
				"Color.g",
				profile.Color.g,
				"Color.b",
				profile.Color.b,
				"Color.a",
				profile.Color.a,
				"Contrast",
				profile.Contrast,
				"Brightness",
				profile.Brightness,
				"Saturation",
				profile.Saturation,
				"Exposure",
				profile.Exposure,
				"Gamma",
				profile.Gamma
			});
		}
		if (this.baseProfile.EnableGradientMapEffect && profile.EnableGradientMapEffect)
		{
			this.m_tweenArray[3] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"GradientBase.r",
				profile.GradientBase.r,
				"GradientBase.g",
				profile.GradientBase.g,
				"GradientBase.b",
				profile.GradientBase.b,
				"GradientBase.a",
				profile.GradientBase.a,
				"GradientDark.r",
				profile.GradientDark.r,
				"GradientDark.g",
				profile.GradientDark.g,
				"GradientDark.b",
				profile.GradientDark.b,
				"GradientDark.a",
				profile.GradientDark.a,
				"GradientMid.r",
				profile.GradientMid.r,
				"GradientMid.g",
				profile.GradientMid.g
			});
			this.m_tweenArray[4] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"GradientMid.b",
				profile.GradientMid.b,
				"GradientMid.a",
				profile.GradientMid.a,
				"GradientLight.r",
				profile.GradientLight.r,
				"GradientLight.g",
				profile.GradientLight.g,
				"GradientLight.b",
				profile.GradientLight.b,
				"GradientLight.a",
				profile.GradientLight.a,
				"GradientAmount",
				profile.GradientAmount,
				"DarkCutoff",
				profile.DarkCutoff,
				"LightCutoff",
				profile.LightCutoff
			});
		}
		if (this.baseProfile.EnableOverlayGradientEffect && profile.EnableOverlayGradientEffect)
		{
			this.m_tweenArray[5] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"OverlayTop.r",
				profile.OverlayTop.r,
				"OverlayTop.g",
				profile.OverlayTop.g,
				"OverlayTop.b",
				profile.OverlayTop.b,
				"OverlayTop.a",
				profile.OverlayTop.a,
				"OverlayBottom.r",
				profile.OverlayBottom.r,
				"OverlayBottom.g",
				profile.OverlayBottom.g,
				"OverlayBottom.b",
				profile.OverlayBottom.b,
				"OverlayBottom.a",
				profile.OverlayBottom.a,
				"OverlayAmount",
				profile.OverlayAmount
			});
		}
		if (this.baseProfile.EnableClampBlackEffect && profile.EnableClampBlackEffect)
		{
			this.m_tweenArray[6] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"ClampColor.r",
				profile.ClampColor.r,
				"ClampColor.g",
				profile.ClampColor.g,
				"ClampColor.b",
				profile.ClampColor.b,
				"ClampColor.a",
				profile.ClampColor.a,
				"ClampFillAmount",
				profile.ClampFillAmount
			});
		}
		if (this.baseProfile.EnableTintEffect && profile.EnableTintEffect)
		{
			this.m_tweenArray[7] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"TintColor.r",
				profile.TintColor.r,
				"TintColor.g",
				profile.TintColor.g,
				"TintColor.b",
				profile.TintColor.b,
				"TintColor.a",
				profile.TintColor.a,
				"TintOverallAmount",
				profile.TintOverallAmount,
				"TintBaselineAmount",
				profile.TintBaselineAmount,
				"TintRedChannel",
				profile.TintRedChannel,
				"TintGreenChannel",
				profile.TintGreenChannel,
				"TintBlueChannel",
				profile.TintBlueChannel,
				"TintAlphaChannel",
				profile.TintAlphaChannel
			});
		}
		if (this.baseProfile.EnableChromaticAbberationEffect && profile.EnableChromaticAbberationEffect)
		{
			this.m_tweenArray[8] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"Offset",
				profile.Offset,
				"FishEyeDistortion",
				profile.FishEyeDistortion
			});
		}
		if (this.baseProfile.EnableDistortionEffect && profile.EnableDistortionEffect)
		{
			this.m_tweenArray[9] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"LensDistortion",
				profile.LensDistortion
			});
		}
		if (this.baseProfile.EnableVignetteEffect && profile.EnableVignetteEffect)
		{
			this.m_tweenArray[10] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"VignetteCenter.x",
				profile.VignetteCenter.x,
				"VignetteCenter.y",
				profile.VignetteCenter.y,
				"VignetteColor.r",
				profile.VignetteColor.r,
				"VignetteColor.g",
				profile.VignetteColor.g,
				"VignetteColor.b",
				profile.VignetteColor.b,
				"VignetteColor.a",
				profile.VignetteColor.a,
				"VignetteAmount",
				profile.VignetteAmount,
				"VignetteSoftness",
				profile.VignetteSoftness,
				"VignetteRoundness",
				profile.VignetteRoundness
			});
		}
		if (this.baseProfile.EnableMistEffect && profile.EnableMistEffect)
		{
			this.m_tweenArray[11] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"MistColor.r",
				profile.MistColor.r,
				"MistColor.g",
				profile.MistColor.g,
				"MistColor.b",
				profile.MistColor.b,
				"MistColor.a",
				profile.MistColor.a,
				"MistAmount",
				profile.MistAmount
			});
		}
		if (this.baseProfile.EnablePixelationEffect && profile.EnablePixelationEffect)
		{
			this.m_tweenArray[12] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"PixelResolution.x",
				profile.PixelResolution.x,
				"PixelResolution.y",
				profile.PixelResolution.y,
				"PixelBaselineAmount",
				profile.PixelBaselineAmount,
				"PixelRedChannel",
				profile.PixelRedChannel,
				"PixelGreenChannel",
				profile.PixelGreenChannel,
				"PixelBlueChannel",
				profile.PixelBlueChannel,
				"PixelAlphaChannel",
				profile.PixelAlphaChannel
			});
		}
		if (this.baseProfile.EnableDitherEffect && profile.EnableDitherEffect)
		{
			this.m_tweenArray[13] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"DitherDark.r",
				profile.DitherDark.r,
				"DitherDark.g",
				profile.DitherDark.g,
				"DitherDark.b",
				profile.DitherDark.b,
				"DitherDark.a",
				profile.DitherDark.a,
				"DitherLight.r",
				profile.DitherLight.r,
				"DitherLight.g",
				profile.DitherLight.g,
				"DitherLight.b",
				profile.DitherLight.b,
				"DitherLight.a",
				profile.DitherLight.a
			});
		}
		if (this.baseProfile.EnableCircularDarknessEffect && profile.EnableCircularDarknessEffect)
		{
			this.m_tweenArray[14] = TweenManager.TweenTo(this, lerpSpeed, new EaseDelegate(Ease.None), new object[]
			{
				"CircDarknessColor.r",
				profile.CircDarknessColor.r,
				"CircDarknessColor.g",
				profile.CircDarknessColor.g,
				"CircDarknessColor.b",
				profile.CircDarknessColor.b,
				"CircDarknessColor.a",
				profile.CircDarknessColor.a,
				"CircDarknessAmount",
				profile.CircDarknessAmount,
				"CircDarknessSoftness",
				profile.CircDarknessSoftness
			});
		}
		float duration = Time.time + lerpSpeed;
		while (Time.time < duration)
		{
			this.ApplyShader();
			yield return null;
		}
		this.m_lerpToProfileCoroutine = null;
		yield break;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00041098 File Offset: 0x0003F298
	public void StopLerpToProfile()
	{
		if (this.m_lerpToProfileCoroutine != null)
		{
			base.StopCoroutine(this.m_lerpToProfileCoroutine);
			for (int i = 0; i < this.m_tweenArray.Length; i++)
			{
				Tween tween = this.m_tweenArray[i];
				if (tween)
				{
					tween.StopTweenWithConditionChecks(false, this.baseProfile, null);
					this.m_tweenArray[i] = null;
				}
			}
		}
		this.m_lerpToProfileCoroutine = null;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000410FC File Offset: 0x0003F2FC
	private void ApplyEffects()
	{
		if (this.baseProfile == null)
		{
			Debug.Log("<color=red>No base MobilePostProcessingProfile is set!</color>");
			return;
		}
		this.m_previousProfile = this.baseProfile;
		this.ResetToDefaults();
		this.ApplyProfile(this.baseProfile);
		for (int i = 0; i < this.m_dimensionOverrides.Count; i++)
		{
			this.ApplyProfile(this.m_dimensionOverrides[i]);
		}
		for (int j = 0; j < this.m_traitOverrides.Count; j++)
		{
			this.ApplyProfile(this.m_traitOverrides[j]);
		}
		this.ApplyShader();
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00041198 File Offset: 0x0003F398
	private void ResetToDefaults()
	{
		this.ResetBlur();
		this.ResetBloom();
		this.ResetLut();
		this.ResetImageFiltering();
		this.ResetGradientMap();
		this.ResetOverlayGradient();
		this.ResetClampBlack();
		this.ResetTint();
		this.ResetChromaticAbberation();
		this.ResetLensDistortion();
		this.ResetVignette();
		this.ResetMist();
		this.ResetPixelation();
		this.ResetDither();
		this.ResetScreenFlip();
		this.ResetAnimatedOverlay();
		this.ResetCircularDarkness();
	}

	// Token: 0x0600005A RID: 90 RVA: 0x0004120C File Offset: 0x0003F40C
	private void ApplyProfile(MobilePostProcessingProfile profile)
	{
		this.ApplyProfileBlur(profile);
		this.ApplyProfileBloom(profile);
		this.ApplyProfileLut(profile);
		this.ApplyProfileImageFiltering(profile);
		this.ApplyProfileGradientMap(profile);
		this.ApplyProfileOverlayGradient(profile);
		this.ApplyProfileClampBlack(profile);
		this.ApplyProfileTint(profile);
		this.ApplyProfileChromaticAbberation(profile);
		this.ApplyProfileDistortion(profile);
		this.ApplyProfileVignette(profile);
		this.ApplyProfileMist(profile);
		this.ApplyProfilePixelation(profile);
		this.ApplyProfileDither(profile);
		this.ApplyProfileScreenFlip(profile);
		this.ApplyProfileAnimatedOverlay(profile);
		this.ApplyProfileCircularDarkness(profile);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00041290 File Offset: 0x0003F490
	public void ApplyShader()
	{
		this.ApplyShaderBlur();
		this.ApplyShaderBloom();
		this.ApplyShaderLut();
		this.ApplyShaderImageFiltering();
		this.ApplyShaderGradientMap();
		this.ApplyShaderOverlayGradient();
		this.ApplyShaderClampBlack();
		this.ApplyShaderTint();
		this.ApplyShaderChromaticAbberation();
		this.ApplyShaderDistortion();
		this.ApplyShaderVignette();
		this.ApplyShaderMist();
		this.ApplyShaderPixelation();
		this.ApplyShaderDither();
		this.ApplyShaderScreenFlip();
		this.ApplyShaderAnimatedOverlay();
		this.ApplyShaderCircularDarkness();
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00041304 File Offset: 0x0003F504
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture renderTexture = null;
		int num = 0;
		if (this.Bloom && this.BloomAmount > 0f)
		{
			num = Mathf.Max(Mathf.CeilToInt(this.BloomDiffuse * 4f), 1);
		}
		if (this.Blur && this.BlurOverallAmount > 0f)
		{
			num = Mathf.Max(Mathf.CeilToInt(this.BlurOverallAmount * 4f), 1);
		}
		if (num > 0)
		{
			if (num == 1)
			{
				renderTexture = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2, 0, source.format);
				Graphics.Blit(source, renderTexture, this.m_material, 0);
			}
			else if (num == 2)
			{
				renderTexture = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2, 0, source.format);
				RenderTexture temporary = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
				Graphics.Blit(source, temporary);
				Graphics.Blit(temporary, renderTexture, this.m_material, 0);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else if (num == 3)
			{
				renderTexture = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
				RenderTexture temporary2 = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8, 0, source.format);
				Graphics.Blit(source, renderTexture);
				Graphics.Blit(renderTexture, temporary2);
				Graphics.Blit(temporary2, renderTexture, this.m_material, 0);
				RenderTexture.ReleaseTemporary(temporary2);
			}
			else if (num == 4)
			{
				renderTexture = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
				RenderTexture temporary3 = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8, 0, source.format);
				RenderTexture temporary4 = RenderTexture.GetTemporary(Screen.width / 16, Screen.height / 16, 0, source.format);
				Graphics.Blit(source, renderTexture);
				Graphics.Blit(renderTexture, temporary3);
				Graphics.Blit(temporary3, temporary4);
				Graphics.Blit(temporary4, temporary3, this.m_material, 0);
				Graphics.Blit(temporary3, renderTexture, this.m_material, 0);
				RenderTexture.ReleaseTemporary(temporary3);
				RenderTexture.ReleaseTemporary(temporary4);
			}
			this.m_material.SetTexture(MobilePostProcessing.ID_blurTex, renderTexture);
		}
		if (this.Pixelation)
		{
			RenderTexture temporary5 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, source.format);
			Graphics.Blit(source, temporary5, this.m_material, 1);
			Graphics.Blit(temporary5, destination, this.m_material, 2);
			RenderTexture.ReleaseTemporary(temporary5);
		}
		else
		{
			Graphics.Blit(source, destination, this.m_material, 1);
		}
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}

	// Token: 0x04000013 RID: 19
	[Space]
	public RenderTexture Mask;

	// Token: 0x04000014 RID: 20
	[SerializeField]
	private MobilePostProcessingProfile baseProfile;

	// Token: 0x04000015 RID: 21
	private static readonly int ID_maskTexture = Shader.PropertyToID("_MaskTex");

	// Token: 0x04000016 RID: 22
	public static readonly string BlurKeyword = "BLUR";

	// Token: 0x04000017 RID: 23
	public const float BLUR_OVERALL_AMOUNT_DEFAULT = 1f;

	// Token: 0x04000018 RID: 24
	public const float BLUR_BASELINE_AMOUNT_DEFAULT = 0f;

	// Token: 0x04000019 RID: 25
	public const float BLUR_RED_CHANNEL_DEFAULT = 0f;

	// Token: 0x0400001A RID: 26
	public const float BLUR_GREEN_CHANNEL_DEFAULT = 0f;

	// Token: 0x0400001B RID: 27
	public const float BLUR_BLUE_CHANNEL_DEFAULT = 0f;

	// Token: 0x0400001C RID: 28
	public const float BLUR_ALPHA_CHANNEL_DEFAULT = 0f;

	// Token: 0x0400001D RID: 29
	private static readonly int ID_blurTex = Shader.PropertyToID("_BlurTex");

	// Token: 0x0400001E RID: 30
	private static readonly int ID_blurOverallAmount = Shader.PropertyToID("_BlurOverallAmount");

	// Token: 0x0400001F RID: 31
	private static readonly int ID_blurBaselineAmount = Shader.PropertyToID("_BlurBaselineAmount");

	// Token: 0x04000020 RID: 32
	private static readonly int ID_blurRGBAChannels = Shader.PropertyToID("_BlurRGBAChannels");

	// Token: 0x04000021 RID: 33
	[Space]
	public bool Blur;

	// Token: 0x04000022 RID: 34
	[Range(0f, 1f)]
	public float BlurOverallAmount = 1f;

	// Token: 0x04000023 RID: 35
	[Range(0f, 1f)]
	public float BlurBaselineAmount;

	// Token: 0x04000024 RID: 36
	[Range(-1f, 1f)]
	public float BlurRedChannel;

	// Token: 0x04000025 RID: 37
	[Range(-1f, 1f)]
	public float BlurGreenChannel;

	// Token: 0x04000026 RID: 38
	[Range(-1f, 1f)]
	public float BlurBlueChannel;

	// Token: 0x04000027 RID: 39
	[Range(-1f, 1f)]
	public float BlurAlphaChannel;

	// Token: 0x04000028 RID: 40
	public static readonly string BloomKeyword = "BLOOM";

	// Token: 0x04000029 RID: 41
	public static readonly Color BLOOM_COLOR_DEFAULT = Color.white;

	// Token: 0x0400002A RID: 42
	public const float BLOOM_AMOUNT_DEFAULT = 1f;

	// Token: 0x0400002B RID: 43
	public const float BLOOM_DIFFUSE_DEFAULT = 0.5f;

	// Token: 0x0400002C RID: 44
	public const float BLOOM_THRESHOLD_DEFAULT = 0.5f;

	// Token: 0x0400002D RID: 45
	public const float BLOOM_SOFTNESS_DEFAULT = 0.5f;

	// Token: 0x0400002E RID: 46
	private static readonly int ID_bloomColor = Shader.PropertyToID("_BloomColor");

	// Token: 0x0400002F RID: 47
	private static readonly int ID_bloomDiffuse = Shader.PropertyToID("_BloomDiffuse");

	// Token: 0x04000030 RID: 48
	private static readonly int ID_bloomData = Shader.PropertyToID("_BloomData");

	// Token: 0x04000031 RID: 49
	[Space]
	public bool Bloom;

	// Token: 0x04000032 RID: 50
	public Color BloomColor = MobilePostProcessing.BLOOM_COLOR_DEFAULT;

	// Token: 0x04000033 RID: 51
	[Range(0f, 5f)]
	public float BloomAmount = 1f;

	// Token: 0x04000034 RID: 52
	[Range(0f, 1f)]
	public float BloomDiffuse = 0.5f;

	// Token: 0x04000035 RID: 53
	[Range(0f, 1f)]
	public float BloomThreshold = 0.5f;

	// Token: 0x04000036 RID: 54
	[Range(0f, 1f)]
	public float BloomSoftness = 0.5f;

	// Token: 0x04000037 RID: 55
	public static readonly string LutKeyword = "LUT";

	// Token: 0x04000038 RID: 56
	public const Texture2D SOURCE_LUT_DEFAULT = null;

	// Token: 0x04000039 RID: 57
	public const float LUT_AMOUNT_DEFAULT = 0.5f;

	// Token: 0x0400003A RID: 58
	private static readonly int ID_lutTexture = Shader.PropertyToID("_LutTex");

	// Token: 0x0400003B RID: 59
	private static readonly int ID_lutAmount = Shader.PropertyToID("_LutAmount");

	// Token: 0x0400003C RID: 60
	[Space]
	public bool LUT;

	// Token: 0x0400003D RID: 61
	[Range(0f, 1f)]
	public float LutAmount = 0.5f;

	// Token: 0x0400003E RID: 62
	public Texture2D SourceLut;

	// Token: 0x0400003F RID: 63
	public static readonly string FilterKeyword = "FILTER";

	// Token: 0x04000040 RID: 64
	public static readonly Color COLOR_DEFAULT = Color.white;

	// Token: 0x04000041 RID: 65
	public const float CONTRAST_DEFAULT = 0f;

	// Token: 0x04000042 RID: 66
	public const float BRIGHTNESS_DEFAULT = 0f;

	// Token: 0x04000043 RID: 67
	public const float SATURATION_DEFAULT = 0f;

	// Token: 0x04000044 RID: 68
	public const float EXPOSURE_DEFAULT = 0f;

	// Token: 0x04000045 RID: 69
	public const float GAMMA_DEFAULT = 0f;

	// Token: 0x04000046 RID: 70
	private static readonly int ID_filterColor = Shader.PropertyToID("_FilterColor");

	// Token: 0x04000047 RID: 71
	private static readonly int ID_filterContrast = Shader.PropertyToID("_FilterContrast");

	// Token: 0x04000048 RID: 72
	private static readonly int ID_filterBrightness = Shader.PropertyToID("_FilterBrightness");

	// Token: 0x04000049 RID: 73
	private static readonly int ID_filterSaturation = Shader.PropertyToID("_FilterSaturation");

	// Token: 0x0400004A RID: 74
	[Space]
	public bool ImageFiltering;

	// Token: 0x0400004B RID: 75
	public Color Color = Color.white;

	// Token: 0x0400004C RID: 76
	[Range(0f, 1f)]
	public float Contrast;

	// Token: 0x0400004D RID: 77
	[Range(-1f, 1f)]
	public float Brightness;

	// Token: 0x0400004E RID: 78
	[Range(-1f, 1f)]
	public float Saturation;

	// Token: 0x0400004F RID: 79
	[Range(-1f, 1f)]
	public float Exposure;

	// Token: 0x04000050 RID: 80
	[Range(-1f, 1f)]
	public float Gamma;

	// Token: 0x04000051 RID: 81
	public static readonly string GradientKeyword = "GRADIENT";

	// Token: 0x04000052 RID: 82
	public static readonly Color GRADIENT_BASE_DEFAULT = Color.black;

	// Token: 0x04000053 RID: 83
	public static readonly Color GRADIENT_DARK_DEFAULT = Color.white;

	// Token: 0x04000054 RID: 84
	public static readonly Color GRADIENT_MID_DEFAULT = Color.white;

	// Token: 0x04000055 RID: 85
	public static readonly Color GRADIENT_LIGHT_DEFAULT = Color.white;

	// Token: 0x04000056 RID: 86
	public const float GRADIENT_AMOUNT_DEFAULT = 0.5f;

	// Token: 0x04000057 RID: 87
	public const float DARK_CUTOFF_DEFAULT = 0f;

	// Token: 0x04000058 RID: 88
	public const float LIGHT_CUTOFF_DEFAULT = 1f;

	// Token: 0x04000059 RID: 89
	private static readonly int ID_gradientBase = Shader.PropertyToID("_GradientBase");

	// Token: 0x0400005A RID: 90
	private static readonly int ID_gradientDark = Shader.PropertyToID("_GradientDark");

	// Token: 0x0400005B RID: 91
	private static readonly int ID_gradientMid = Shader.PropertyToID("_GradientMid");

	// Token: 0x0400005C RID: 92
	private static readonly int ID_gradientLight = Shader.PropertyToID("_GradientLight");

	// Token: 0x0400005D RID: 93
	private static readonly int ID_gradientAmount = Shader.PropertyToID("_GradientAmount");

	// Token: 0x0400005E RID: 94
	private static readonly int ID_gradientDarkCutoff = Shader.PropertyToID("_GradientDarkCutoff");

	// Token: 0x0400005F RID: 95
	private static readonly int ID_gradientLightCutoff = Shader.PropertyToID("_GradientLightCutoff");

	// Token: 0x04000060 RID: 96
	[Space]
	public bool GradientMap;

	// Token: 0x04000061 RID: 97
	public Color GradientBase = MobilePostProcessing.GRADIENT_BASE_DEFAULT;

	// Token: 0x04000062 RID: 98
	public Color GradientDark = MobilePostProcessing.GRADIENT_DARK_DEFAULT;

	// Token: 0x04000063 RID: 99
	public Color GradientMid = MobilePostProcessing.GRADIENT_MID_DEFAULT;

	// Token: 0x04000064 RID: 100
	public Color GradientLight = MobilePostProcessing.GRADIENT_LIGHT_DEFAULT;

	// Token: 0x04000065 RID: 101
	[Range(0f, 1f)]
	public float GradientAmount = 0.5f;

	// Token: 0x04000066 RID: 102
	[Range(0f, 0.3f)]
	public float DarkCutoff;

	// Token: 0x04000067 RID: 103
	[Range(0.7f, 1f)]
	public float LightCutoff = 1f;

	// Token: 0x04000068 RID: 104
	public static readonly string OverlayKeyword = "OVERLAY";

	// Token: 0x04000069 RID: 105
	public static readonly Color OVERLAY_TOP_DEFAULT = Color.white;

	// Token: 0x0400006A RID: 106
	public static readonly Color OVERLAY_BOTTOM_DEFAULT = Color.black;

	// Token: 0x0400006B RID: 107
	public const float OVERLAY_AMOUNT_DEFAULT = 0.5f;

	// Token: 0x0400006C RID: 108
	private static readonly int ID_overlayTop = Shader.PropertyToID("_OverlayTop");

	// Token: 0x0400006D RID: 109
	private static readonly int ID_overlayBottom = Shader.PropertyToID("_OverlayBottom");

	// Token: 0x0400006E RID: 110
	private static readonly int ID_overlayAmount = Shader.PropertyToID("_OverlayAmount");

	// Token: 0x0400006F RID: 111
	[Space]
	public bool OverlayGradient;

	// Token: 0x04000070 RID: 112
	public Color OverlayTop = MobilePostProcessing.OVERLAY_TOP_DEFAULT;

	// Token: 0x04000071 RID: 113
	public Color OverlayBottom = MobilePostProcessing.OVERLAY_BOTTOM_DEFAULT;

	// Token: 0x04000072 RID: 114
	[Range(0f, 1f)]
	public float OverlayAmount = 0.5f;

	// Token: 0x04000073 RID: 115
	public static readonly string ClampKeyword = "CLAMP";

	// Token: 0x04000074 RID: 116
	public static readonly Color CLAMP_COLOR_DEFAULT = Color.black;

	// Token: 0x04000075 RID: 117
	public const float CLAMP_FILL_AMOUNT_DEFAULT = 0.5f;

	// Token: 0x04000076 RID: 118
	private static readonly int ID_clampColor = Shader.PropertyToID("_ClampColor");

	// Token: 0x04000077 RID: 119
	private static readonly int ID_clampFillAmount = Shader.PropertyToID("_ClampFillAmount");

	// Token: 0x04000078 RID: 120
	[Space]
	public bool ClampBlack;

	// Token: 0x04000079 RID: 121
	public Color ClampColor = MobilePostProcessing.CLAMP_COLOR_DEFAULT;

	// Token: 0x0400007A RID: 122
	[Range(0f, 1f)]
	public float ClampFillAmount = 0.5f;

	// Token: 0x0400007B RID: 123
	public static readonly string TintKeyword = "TINT";

	// Token: 0x0400007C RID: 124
	public static readonly Color TINT_COLOR_DEFAULT = Color.black;

	// Token: 0x0400007D RID: 125
	public const float TINT_OVERALL_AMOUNT_DEFAULT = 1f;

	// Token: 0x0400007E RID: 126
	public const float TINT_BASELINE_AMOUNT_DEFAULT = 0f;

	// Token: 0x0400007F RID: 127
	public const float TINT_RED_CHANNEL_DEFAULT = 0f;

	// Token: 0x04000080 RID: 128
	public const float TINT_GREEN_CHANNEL_DEFAULT = 0f;

	// Token: 0x04000081 RID: 129
	public const float TINT_BLUE_CHANNEL_DEFAULT = 0f;

	// Token: 0x04000082 RID: 130
	public const float TINT_ALPHA_CHANNEL_DEFAULT = 0f;

	// Token: 0x04000083 RID: 131
	private static readonly int ID_tintColor = Shader.PropertyToID("_TintColor");

	// Token: 0x04000084 RID: 132
	private static readonly int ID_tintOverallAmount = Shader.PropertyToID("_TintOverallAmount");

	// Token: 0x04000085 RID: 133
	private static readonly int ID_tintBaselineAmount = Shader.PropertyToID("_TintBaselineAmount");

	// Token: 0x04000086 RID: 134
	private static readonly int ID_tintRGBAChannels = Shader.PropertyToID("_TintRGBAChannels");

	// Token: 0x04000087 RID: 135
	[Space]
	public bool Tint;

	// Token: 0x04000088 RID: 136
	public Color TintColor = MobilePostProcessing.TINT_COLOR_DEFAULT;

	// Token: 0x04000089 RID: 137
	[Range(0f, 1f)]
	public float TintOverallAmount = 1f;

	// Token: 0x0400008A RID: 138
	[Range(0f, 1f)]
	public float TintBaselineAmount;

	// Token: 0x0400008B RID: 139
	[Range(-1f, 1f)]
	public float TintRedChannel;

	// Token: 0x0400008C RID: 140
	[Range(-1f, 1f)]
	public float TintGreenChannel;

	// Token: 0x0400008D RID: 141
	[Range(-1f, 1f)]
	public float TintBlueChannel;

	// Token: 0x0400008E RID: 142
	[Range(-1f, 1f)]
	public float TintAlphaChannel;

	// Token: 0x0400008F RID: 143
	public static readonly string ChromaKeyword = "CHROMA";

	// Token: 0x04000090 RID: 144
	public const float OFFSET_DEFAULT = 1f;

	// Token: 0x04000091 RID: 145
	public const float FISH_EYE_DISTORTION_DEFAULT = 0f;

	// Token: 0x04000092 RID: 146
	private static readonly int ID_chromaOffset = Shader.PropertyToID("_ChromaOffset");

	// Token: 0x04000093 RID: 147
	private static readonly int ID_chromaFishEye = Shader.PropertyToID("_ChromaFishEye");

	// Token: 0x04000094 RID: 148
	[Space]
	public bool ChromaticAberration;

	// Token: 0x04000095 RID: 149
	public float Offset = 1f;

	// Token: 0x04000096 RID: 150
	[Range(-1f, 1f)]
	public float FishEyeDistortion;

	// Token: 0x04000097 RID: 151
	public static readonly string DistortionKeyword = "DISTORTION";

	// Token: 0x04000098 RID: 152
	public const float LENS_DISTORTION_DEFAULT = 0.5f;

	// Token: 0x04000099 RID: 153
	private static readonly int ID_lensdistortion = Shader.PropertyToID("_LensDistortion");

	// Token: 0x0400009A RID: 154
	[Space]
	public bool Distortion;

	// Token: 0x0400009B RID: 155
	[Range(0f, 1f)]
	public float LensDistortion = 0.5f;

	// Token: 0x0400009C RID: 156
	public static readonly string VignetteKeyword = "VIGNETTE";

	// Token: 0x0400009D RID: 157
	public static readonly Vector2 VIGNETTE_CENTER_DEFAULT = new Vector2(0.5f, 0.5f);

	// Token: 0x0400009E RID: 158
	public static readonly Color VIGNETTE_COLOR_DEFAULT = Color.black;

	// Token: 0x0400009F RID: 159
	public const float VIGNETTE_AMOUNT_DEFAULT = 0.5f;

	// Token: 0x040000A0 RID: 160
	public const float VIGNETTE_SOFTNESS_DEFAULT = 0.5f;

	// Token: 0x040000A1 RID: 161
	public const float VIGNETTE_ROUNDNESS_DEFAULT = 0.5f;

	// Token: 0x040000A2 RID: 162
	private static readonly int ID_vignetteColor = Shader.PropertyToID("_VignetteColor");

	// Token: 0x040000A3 RID: 163
	private static readonly int ID_vignetteAmount = Shader.PropertyToID("_VignetteAmount");

	// Token: 0x040000A4 RID: 164
	private static readonly int ID_vignetteSoftness = Shader.PropertyToID("_VignetteSoftness");

	// Token: 0x040000A5 RID: 165
	private static readonly int ID_vignetteCenter = Shader.PropertyToID("_VignetteCenter");

	// Token: 0x040000A6 RID: 166
	private static readonly int ID_vignetteRoundness = Shader.PropertyToID("_VignetteRoundness");

	// Token: 0x040000A7 RID: 167
	[Space]
	public bool Vignette;

	// Token: 0x040000A8 RID: 168
	public Vector2 VignetteCenter = MobilePostProcessing.VIGNETTE_CENTER_DEFAULT;

	// Token: 0x040000A9 RID: 169
	public Color VignetteColor = MobilePostProcessing.VIGNETTE_COLOR_DEFAULT;

	// Token: 0x040000AA RID: 170
	[Range(0f, 1f)]
	public float VignetteAmount = 0.5f;

	// Token: 0x040000AB RID: 171
	[Range(0.001f, 1f)]
	public float VignetteSoftness = 0.5f;

	// Token: 0x040000AC RID: 172
	[Range(0f, 1f)]
	public float VignetteRoundness = 0.5f;

	// Token: 0x040000AD RID: 173
	public static readonly string MistKeyword = "MIST";

	// Token: 0x040000AE RID: 174
	public static readonly Color MIST_COLOR_DEFAULT = Color.white;

	// Token: 0x040000AF RID: 175
	public const float MIST_AMOUNT_DEFAULT = 0.1f;

	// Token: 0x040000B0 RID: 176
	private static readonly int ID_mistColor = Shader.PropertyToID("_MistColor");

	// Token: 0x040000B1 RID: 177
	private static readonly int ID_mistAmount = Shader.PropertyToID("_MistAmount");

	// Token: 0x040000B2 RID: 178
	[Space]
	public bool Mist;

	// Token: 0x040000B3 RID: 179
	public Color MistColor = MobilePostProcessing.MIST_COLOR_DEFAULT;

	// Token: 0x040000B4 RID: 180
	[Range(0f, 1f)]
	public float MistAmount = 0.1f;

	// Token: 0x040000B5 RID: 181
	public static readonly Vector2 PIXEL_RESOLUTION_DEFAULT = new Vector2(100f, 100f);

	// Token: 0x040000B6 RID: 182
	public const float PIXEL_BASELINE_AMOUNT_DEFAULT = 0f;

	// Token: 0x040000B7 RID: 183
	public const float PIXEL_RED_CHANNEL_DEFAULT = 0f;

	// Token: 0x040000B8 RID: 184
	public const float PIXEL_GREEN_CHANNEL_DEFAULT = 0f;

	// Token: 0x040000B9 RID: 185
	public const float PIXEL_BLUE_CHANNEL_DEFAULT = 0f;

	// Token: 0x040000BA RID: 186
	public const float PIXEL_ALPHA_CHANNEL_DEFAULT = 0f;

	// Token: 0x040000BB RID: 187
	private static readonly int ID_pixelResolution = Shader.PropertyToID("_PixelResolution");

	// Token: 0x040000BC RID: 188
	private static readonly int ID_pixelBaselineAmount = Shader.PropertyToID("_PixelBaselineAmount");

	// Token: 0x040000BD RID: 189
	private static readonly int ID_pixelRGBAChannels = Shader.PropertyToID("_PixelRGBAChannels");

	// Token: 0x040000BE RID: 190
	[Space]
	public bool Pixelation;

	// Token: 0x040000BF RID: 191
	public Vector2 PixelResolution = MobilePostProcessing.PIXEL_RESOLUTION_DEFAULT;

	// Token: 0x040000C0 RID: 192
	[Range(0f, 1f)]
	public float PixelBaselineAmount;

	// Token: 0x040000C1 RID: 193
	[Range(-1f, 1f)]
	public float PixelRedChannel;

	// Token: 0x040000C2 RID: 194
	[Range(-1f, 1f)]
	public float PixelGreenChannel;

	// Token: 0x040000C3 RID: 195
	[Range(-1f, 1f)]
	public float PixelBlueChannel;

	// Token: 0x040000C4 RID: 196
	[Range(-1f, 1f)]
	public float PixelAlphaChannel;

	// Token: 0x040000C5 RID: 197
	public static readonly string DitherKeyword = "DITHER";

	// Token: 0x040000C6 RID: 198
	public static readonly Color DITHER_DARK_DEFAULT = Color.black;

	// Token: 0x040000C7 RID: 199
	public static readonly Color DITHER_LIGHT_DEFAULT = Color.white;

	// Token: 0x040000C8 RID: 200
	private static readonly int ID_ditherDark = Shader.PropertyToID("_DitherDark");

	// Token: 0x040000C9 RID: 201
	private static readonly int ID_ditherLight = Shader.PropertyToID("_DitherLight");

	// Token: 0x040000CA RID: 202
	[Space]
	public bool Dither;

	// Token: 0x040000CB RID: 203
	public Color DitherDark = MobilePostProcessing.DITHER_DARK_DEFAULT;

	// Token: 0x040000CC RID: 204
	public Color DitherLight = MobilePostProcessing.DITHER_LIGHT_DEFAULT;

	// Token: 0x040000CD RID: 205
	public static readonly string ScreenFlipXKeyword = "SCREENFLIP_X";

	// Token: 0x040000CE RID: 206
	public static readonly string ScreenFlipYKeyword = "SCREENFLIP_Y";

	// Token: 0x040000CF RID: 207
	[Space]
	public bool ScreenFlipX;

	// Token: 0x040000D0 RID: 208
	public bool ScreenFlipY;

	// Token: 0x040000D1 RID: 209
	public static readonly string AnimOverlayKeyword = "ANIMOVERLAY";

	// Token: 0x040000D2 RID: 210
	public static readonly Texture2D ANIMOVERLAY_TEXTURE_DEFAULT = null;

	// Token: 0x040000D3 RID: 211
	public static readonly Vector2 ANIMOVERLAY_SPRITESHEET_SIZE_DEFAULT = Vector2.one;

	// Token: 0x040000D4 RID: 212
	public const int ANIMOVERLAY_FRAMERATE_DEFAULT = 30;

	// Token: 0x040000D5 RID: 213
	public const float ANIMOVERLAY_STRENGTH_DEFAULT = 0.5f;

	// Token: 0x040000D6 RID: 214
	private static readonly int ID_animOverlayTexture = Shader.PropertyToID("_AnimOverlayTex");

	// Token: 0x040000D7 RID: 215
	private static readonly int ID_animOverlaySpritesheetSize = Shader.PropertyToID("_AnimOverlaySpritesheetSize");

	// Token: 0x040000D8 RID: 216
	private static readonly int ID_animOverlayFramerate = Shader.PropertyToID("_AnimOverlayFramerate");

	// Token: 0x040000D9 RID: 217
	private static readonly int ID_animOverlayStrength = Shader.PropertyToID("_AnimOverlayStrength");

	// Token: 0x040000DA RID: 218
	[Space]
	public bool AnimatedOverlay;

	// Token: 0x040000DB RID: 219
	public Texture2D AnimOverlayTexture = MobilePostProcessing.ANIMOVERLAY_TEXTURE_DEFAULT;

	// Token: 0x040000DC RID: 220
	public Vector2 AnimOverlaySpritesheetSize = MobilePostProcessing.ANIMOVERLAY_SPRITESHEET_SIZE_DEFAULT;

	// Token: 0x040000DD RID: 221
	[Range(0f, 60f)]
	public int AnimOverlayFramerate = 30;

	// Token: 0x040000DE RID: 222
	[Range(0f, 1f)]
	public float AnimOverlayStrength = 0.5f;

	// Token: 0x040000DF RID: 223
	public static readonly string CircDarknessKeyword = "CIRCDARKNESS";

	// Token: 0x040000E0 RID: 224
	public static readonly Color CIRCDARKNESS_COLOR_DEFAULT = Color.black;

	// Token: 0x040000E1 RID: 225
	public const float CIRCDARKNESS_AMOUNT_DEFAULT = 0.5f;

	// Token: 0x040000E2 RID: 226
	public const float CIRCDARKNESS_SOFTNESS_DEFAULT = 0.5f;

	// Token: 0x040000E3 RID: 227
	private static readonly int ID_circDarknessColor = Shader.PropertyToID("_CircDarknessColor");

	// Token: 0x040000E4 RID: 228
	private static readonly int ID_circDarknessAmount = Shader.PropertyToID("_CircDarknessAmount");

	// Token: 0x040000E5 RID: 229
	private static readonly int ID_circDarknessSoftness = Shader.PropertyToID("_CircDarknessSoftness");

	// Token: 0x040000E6 RID: 230
	private static readonly int ID_circDarknessCenter = Shader.PropertyToID("_CircDarknessCenter");

	// Token: 0x040000E7 RID: 231
	[Space]
	public bool CircularDarkness;

	// Token: 0x040000E8 RID: 232
	public Color CircDarknessColor = MobilePostProcessing.CIRCDARKNESS_COLOR_DEFAULT;

	// Token: 0x040000E9 RID: 233
	[Range(0f, 1f)]
	public float CircDarknessAmount = 0.5f;

	// Token: 0x040000EA RID: 234
	[Range(0.001f, 1f)]
	public float CircDarknessSoftness = 0.5f;

	// Token: 0x040000EB RID: 235
	private Material m_material;

	// Token: 0x040000EC RID: 236
	private MobilePostProcessingProfile m_previousProfile;

	// Token: 0x040000ED RID: 237
	private Texture2D m_previousLut;

	// Token: 0x040000EE RID: 238
	private Texture3D m_converted3D;

	// Token: 0x040000EF RID: 239
	private List<MobilePostProcessingProfile> m_traitOverrides = new List<MobilePostProcessingProfile>(2);

	// Token: 0x040000F0 RID: 240
	private List<MobilePostProcessingProfile> m_dimensionOverrides = new List<MobilePostProcessingProfile>(2);

	// Token: 0x040000F1 RID: 241
	private bool m_isDirty;

	// Token: 0x040000F2 RID: 242
	private Coroutine m_lerpToProfileCoroutine;

	// Token: 0x040000F3 RID: 243
	private Tween[] m_tweenArray = new Tween[15];
}

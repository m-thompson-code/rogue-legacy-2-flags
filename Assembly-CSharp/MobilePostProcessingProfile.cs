using System;
using UnityEngine;

// Token: 0x020006E0 RID: 1760
[CreateAssetMenu(fileName = "New Profile", menuName = "Custom/Mobile Post Processing Profile")]
public class MobilePostProcessingProfile : ScriptableObject
{
	// Token: 0x06003FEE RID: 16366 RVA: 0x000E29FA File Offset: 0x000E0BFA
	public MobilePostProcessingProfile Clone()
	{
		return (MobilePostProcessingProfile)base.MemberwiseClone();
	}

	// Token: 0x0400306B RID: 12395
	[Header("Blur")]
	public bool EnableBlurEffect;

	// Token: 0x0400306C RID: 12396
	[Space]
	public bool OverrideBlurOverallAmount;

	// Token: 0x0400306D RID: 12397
	[Range(0f, 1f)]
	public float BlurOverallAmount = 1f;

	// Token: 0x0400306E RID: 12398
	[Space]
	public bool OverrideBlurBaselineAmount;

	// Token: 0x0400306F RID: 12399
	[Range(0f, 1f)]
	public float BlurBaselineAmount;

	// Token: 0x04003070 RID: 12400
	[Space]
	public bool OverrideBlurRedChannel;

	// Token: 0x04003071 RID: 12401
	[Range(-1f, 1f)]
	public float BlurRedChannel;

	// Token: 0x04003072 RID: 12402
	[Space]
	public bool OverrideBlurGreenChannel;

	// Token: 0x04003073 RID: 12403
	[Range(-1f, 1f)]
	public float BlurGreenChannel;

	// Token: 0x04003074 RID: 12404
	[Space]
	public bool OverrideBlurBlueChannel;

	// Token: 0x04003075 RID: 12405
	[Range(-1f, 1f)]
	public float BlurBlueChannel;

	// Token: 0x04003076 RID: 12406
	[Space]
	public bool OverrideBlurAlphaChannel;

	// Token: 0x04003077 RID: 12407
	[Range(-1f, 1f)]
	public float BlurAlphaChannel;

	// Token: 0x04003078 RID: 12408
	[Header("Bloom")]
	public bool EnableBloomEffect;

	// Token: 0x04003079 RID: 12409
	[Space]
	public bool OverrideBloomColor;

	// Token: 0x0400307A RID: 12410
	public Color BloomColor = MobilePostProcessing.BLOOM_COLOR_DEFAULT;

	// Token: 0x0400307B RID: 12411
	[Space]
	public bool OverrideBloomAmount;

	// Token: 0x0400307C RID: 12412
	[Range(0f, 5f)]
	public float BloomAmount = 1f;

	// Token: 0x0400307D RID: 12413
	[Space]
	public bool OverrideBloomDiffuse;

	// Token: 0x0400307E RID: 12414
	[Range(0f, 1f)]
	public float BloomDiffuse = 0.5f;

	// Token: 0x0400307F RID: 12415
	[Space]
	public bool OverrideBloomThreshold;

	// Token: 0x04003080 RID: 12416
	[Range(0f, 1f)]
	public float BloomThreshold = 0.5f;

	// Token: 0x04003081 RID: 12417
	[Space]
	public bool OverrideBloomSoftness;

	// Token: 0x04003082 RID: 12418
	[Range(0f, 1f)]
	public float BloomSoftness = 0.5f;

	// Token: 0x04003083 RID: 12419
	[Space]
	[Header("LUT")]
	public bool EnableLUTEffect;

	// Token: 0x04003084 RID: 12420
	[Space]
	public bool OverrideLutAmount;

	// Token: 0x04003085 RID: 12421
	[Range(0f, 1f)]
	public float LutAmount = 0.5f;

	// Token: 0x04003086 RID: 12422
	[Space]
	public bool OverrideSourceLut;

	// Token: 0x04003087 RID: 12423
	public Texture2D SourceLut;

	// Token: 0x04003088 RID: 12424
	[Space]
	[Header("Image Filtering")]
	public bool EnableImageFilteringEffect;

	// Token: 0x04003089 RID: 12425
	[Space]
	public bool OverrideColor;

	// Token: 0x0400308A RID: 12426
	public Color Color = MobilePostProcessing.COLOR_DEFAULT;

	// Token: 0x0400308B RID: 12427
	[Space]
	public bool OverrideContrast;

	// Token: 0x0400308C RID: 12428
	[Range(0f, 1f)]
	public float Contrast;

	// Token: 0x0400308D RID: 12429
	[Space]
	public bool OverrideBrightness;

	// Token: 0x0400308E RID: 12430
	[Range(-1f, 1f)]
	public float Brightness;

	// Token: 0x0400308F RID: 12431
	[Space]
	public bool OverrideSaturation;

	// Token: 0x04003090 RID: 12432
	[Range(-1f, 1f)]
	public float Saturation;

	// Token: 0x04003091 RID: 12433
	[Space]
	public bool OverrideExposure;

	// Token: 0x04003092 RID: 12434
	[Range(-1f, 1f)]
	public float Exposure;

	// Token: 0x04003093 RID: 12435
	[Space]
	public bool OverrideGamma;

	// Token: 0x04003094 RID: 12436
	[Range(-1f, 1f)]
	public float Gamma;

	// Token: 0x04003095 RID: 12437
	[Space]
	[Header("Gradient Map")]
	public bool EnableGradientMapEffect;

	// Token: 0x04003096 RID: 12438
	[Space]
	public bool OverrideGradientBase;

	// Token: 0x04003097 RID: 12439
	public Color GradientBase = MobilePostProcessing.GRADIENT_BASE_DEFAULT;

	// Token: 0x04003098 RID: 12440
	[Space]
	public bool OverrideGradientDark;

	// Token: 0x04003099 RID: 12441
	public Color GradientDark = MobilePostProcessing.GRADIENT_DARK_DEFAULT;

	// Token: 0x0400309A RID: 12442
	[Space]
	public bool OverrideGradientMid;

	// Token: 0x0400309B RID: 12443
	public Color GradientMid = MobilePostProcessing.GRADIENT_MID_DEFAULT;

	// Token: 0x0400309C RID: 12444
	[Space]
	public bool OverrideGradientLight;

	// Token: 0x0400309D RID: 12445
	public Color GradientLight = MobilePostProcessing.GRADIENT_LIGHT_DEFAULT;

	// Token: 0x0400309E RID: 12446
	[Space]
	public bool OverrideGradientAmount;

	// Token: 0x0400309F RID: 12447
	[Range(0f, 1f)]
	public float GradientAmount = 0.5f;

	// Token: 0x040030A0 RID: 12448
	[Space]
	public bool OverrideDarkCutoff;

	// Token: 0x040030A1 RID: 12449
	[Range(0f, 0.3f)]
	public float DarkCutoff;

	// Token: 0x040030A2 RID: 12450
	[Space]
	public bool OverrideLightCutoff;

	// Token: 0x040030A3 RID: 12451
	[Range(0.7f, 1f)]
	public float LightCutoff = 1f;

	// Token: 0x040030A4 RID: 12452
	[Space]
	[Header("Overlay Gradient")]
	public bool EnableOverlayGradientEffect;

	// Token: 0x040030A5 RID: 12453
	[Space]
	public bool OverrideOverlayTop;

	// Token: 0x040030A6 RID: 12454
	public Color OverlayTop = MobilePostProcessing.OVERLAY_TOP_DEFAULT;

	// Token: 0x040030A7 RID: 12455
	[Space]
	public bool OverrideOverlayBottom;

	// Token: 0x040030A8 RID: 12456
	public Color OverlayBottom = MobilePostProcessing.OVERLAY_BOTTOM_DEFAULT;

	// Token: 0x040030A9 RID: 12457
	[Space]
	public bool OverrideOverlayAmount;

	// Token: 0x040030AA RID: 12458
	[Range(0f, 1f)]
	public float OverlayAmount = 0.5f;

	// Token: 0x040030AB RID: 12459
	[Space]
	[Header("Clamp Black")]
	public bool EnableClampBlackEffect;

	// Token: 0x040030AC RID: 12460
	[Space]
	public bool OverrideClampColor;

	// Token: 0x040030AD RID: 12461
	public Color ClampColor = MobilePostProcessing.CLAMP_COLOR_DEFAULT;

	// Token: 0x040030AE RID: 12462
	[Space]
	public bool OverrideClampFillAmount;

	// Token: 0x040030AF RID: 12463
	[Range(0f, 1f)]
	public float ClampFillAmount = 0.5f;

	// Token: 0x040030B0 RID: 12464
	[Space]
	[Header("Tint")]
	public bool EnableTintEffect;

	// Token: 0x040030B1 RID: 12465
	[Space]
	public bool OverrideTintColor;

	// Token: 0x040030B2 RID: 12466
	public Color TintColor = MobilePostProcessing.TINT_COLOR_DEFAULT;

	// Token: 0x040030B3 RID: 12467
	[Space]
	public bool OverrideTintOverallAmount;

	// Token: 0x040030B4 RID: 12468
	[Range(0f, 1f)]
	public float TintOverallAmount = 1f;

	// Token: 0x040030B5 RID: 12469
	[Space]
	public bool OverrideTintBaselineAmount;

	// Token: 0x040030B6 RID: 12470
	[Range(0f, 1f)]
	public float TintBaselineAmount;

	// Token: 0x040030B7 RID: 12471
	[Space]
	public bool OverrideTintRedChannel;

	// Token: 0x040030B8 RID: 12472
	[Range(-1f, 1f)]
	public float TintRedChannel;

	// Token: 0x040030B9 RID: 12473
	[Space]
	public bool OverrideTintGreenChannel;

	// Token: 0x040030BA RID: 12474
	[Range(-1f, 1f)]
	public float TintGreenChannel;

	// Token: 0x040030BB RID: 12475
	[Space]
	public bool OverrideTintBlueChannel;

	// Token: 0x040030BC RID: 12476
	[Range(-1f, 1f)]
	public float TintBlueChannel;

	// Token: 0x040030BD RID: 12477
	[Space]
	public bool OverrideTintAlphaChannel;

	// Token: 0x040030BE RID: 12478
	[Range(-1f, 1f)]
	public float TintAlphaChannel;

	// Token: 0x040030BF RID: 12479
	[Space]
	[Header("Chromatic Abberation")]
	public bool EnableChromaticAbberationEffect;

	// Token: 0x040030C0 RID: 12480
	[Space]
	public bool OverrideOffset;

	// Token: 0x040030C1 RID: 12481
	public float Offset = 1f;

	// Token: 0x040030C2 RID: 12482
	[Space]
	public bool OverrideFishEyeDistortion;

	// Token: 0x040030C3 RID: 12483
	[Range(-1f, 1f)]
	public float FishEyeDistortion;

	// Token: 0x040030C4 RID: 12484
	[Space]
	[Header("Distortion")]
	public bool EnableDistortionEffect;

	// Token: 0x040030C5 RID: 12485
	[Space]
	public bool OverrideLensDistortion;

	// Token: 0x040030C6 RID: 12486
	[Range(0f, 1f)]
	public float LensDistortion = 0.5f;

	// Token: 0x040030C7 RID: 12487
	[Space]
	[Header("Vignette")]
	public bool EnableVignetteEffect;

	// Token: 0x040030C8 RID: 12488
	[Space]
	public bool OverrideVignetteCenter;

	// Token: 0x040030C9 RID: 12489
	public Vector2 VignetteCenter = MobilePostProcessing.VIGNETTE_CENTER_DEFAULT;

	// Token: 0x040030CA RID: 12490
	[Space]
	public bool OverrideVignetteColor;

	// Token: 0x040030CB RID: 12491
	public Color VignetteColor = MobilePostProcessing.VIGNETTE_COLOR_DEFAULT;

	// Token: 0x040030CC RID: 12492
	[Space]
	public bool OverrideVignetteAmount;

	// Token: 0x040030CD RID: 12493
	[Range(0f, 1f)]
	public float VignetteAmount = 0.5f;

	// Token: 0x040030CE RID: 12494
	[Space]
	public bool OverrideVignetteSoftness;

	// Token: 0x040030CF RID: 12495
	[Range(0.001f, 1f)]
	public float VignetteSoftness = 0.5f;

	// Token: 0x040030D0 RID: 12496
	[Space]
	public bool OverrideVignetteRoundness;

	// Token: 0x040030D1 RID: 12497
	[Range(0f, 1f)]
	public float VignetteRoundness = 0.5f;

	// Token: 0x040030D2 RID: 12498
	[Space]
	[Header("Mist")]
	public bool EnableMistEffect;

	// Token: 0x040030D3 RID: 12499
	[Space]
	public bool OverrideMistColor;

	// Token: 0x040030D4 RID: 12500
	public Color MistColor = MobilePostProcessing.MIST_COLOR_DEFAULT;

	// Token: 0x040030D5 RID: 12501
	[Space]
	public bool OverrideMistAmount;

	// Token: 0x040030D6 RID: 12502
	[Range(0f, 1f)]
	public float MistAmount = 0.1f;

	// Token: 0x040030D7 RID: 12503
	[Space]
	[Header("Pixelation")]
	public bool EnablePixelationEffect;

	// Token: 0x040030D8 RID: 12504
	[Space]
	public bool OverridePixelResolution;

	// Token: 0x040030D9 RID: 12505
	public Vector2 PixelResolution = MobilePostProcessing.PIXEL_RESOLUTION_DEFAULT;

	// Token: 0x040030DA RID: 12506
	[Space]
	public bool OverridePixelBaselineAmount;

	// Token: 0x040030DB RID: 12507
	[Range(0f, 1f)]
	public float PixelBaselineAmount;

	// Token: 0x040030DC RID: 12508
	[Space]
	public bool OverridePixelRedChannel;

	// Token: 0x040030DD RID: 12509
	[Range(-1f, 1f)]
	public float PixelRedChannel;

	// Token: 0x040030DE RID: 12510
	[Space]
	public bool OverridePixelGreenChannel;

	// Token: 0x040030DF RID: 12511
	[Range(-1f, 1f)]
	public float PixelGreenChannel;

	// Token: 0x040030E0 RID: 12512
	[Space]
	public bool OverridePixelBlueChannel;

	// Token: 0x040030E1 RID: 12513
	[Range(-1f, 1f)]
	public float PixelBlueChannel;

	// Token: 0x040030E2 RID: 12514
	[Space]
	public bool OverridePixelAlphaChannel;

	// Token: 0x040030E3 RID: 12515
	[Range(-1f, 1f)]
	public float PixelAlphaChannel;

	// Token: 0x040030E4 RID: 12516
	[Space]
	[Header("Dither")]
	public bool EnableDitherEffect;

	// Token: 0x040030E5 RID: 12517
	[Space]
	public bool OverrideDitherDark;

	// Token: 0x040030E6 RID: 12518
	public Color DitherDark = MobilePostProcessing.DITHER_DARK_DEFAULT;

	// Token: 0x040030E7 RID: 12519
	[Space]
	public bool OverrideDitherLight;

	// Token: 0x040030E8 RID: 12520
	public Color DitherLight = MobilePostProcessing.DITHER_LIGHT_DEFAULT;

	// Token: 0x040030E9 RID: 12521
	[Space]
	[Header("Screen Flip")]
	public bool OverrideScreenFlipXEffect;

	// Token: 0x040030EA RID: 12522
	public bool ScreenFlipX;

	// Token: 0x040030EB RID: 12523
	[Space]
	public bool OverrideScreenFlipYEffect;

	// Token: 0x040030EC RID: 12524
	public bool ScreenFlipY;

	// Token: 0x040030ED RID: 12525
	[Space]
	[Header("Animated Overlay")]
	public bool EnableAnimatedOverlayEffect;

	// Token: 0x040030EE RID: 12526
	[Space]
	public bool OverrideAnimOverlayTexture;

	// Token: 0x040030EF RID: 12527
	public Texture2D AnimOverlayTexture = MobilePostProcessing.ANIMOVERLAY_TEXTURE_DEFAULT;

	// Token: 0x040030F0 RID: 12528
	[Space]
	public bool OverrideAnimOverlaySpritesheetSize;

	// Token: 0x040030F1 RID: 12529
	public Vector2 AnimOverlaySpritesheetSize = MobilePostProcessing.ANIMOVERLAY_SPRITESHEET_SIZE_DEFAULT;

	// Token: 0x040030F2 RID: 12530
	[Space]
	public bool OverrideAnimOverlayFramerate;

	// Token: 0x040030F3 RID: 12531
	[Range(0f, 60f)]
	public int AnimOverlayFramerate = 30;

	// Token: 0x040030F4 RID: 12532
	[Space]
	public bool OverrideAnimOverlayStrength;

	// Token: 0x040030F5 RID: 12533
	[Range(0f, 1f)]
	public float AnimOverlayStrength = 0.5f;

	// Token: 0x040030F6 RID: 12534
	[Space]
	[Header("Circular Darkness")]
	public bool EnableCircularDarknessEffect;

	// Token: 0x040030F7 RID: 12535
	[Space]
	public bool OverrideCircDarknessColor;

	// Token: 0x040030F8 RID: 12536
	public Color CircDarknessColor = MobilePostProcessing.CIRCDARKNESS_COLOR_DEFAULT;

	// Token: 0x040030F9 RID: 12537
	[Space]
	public bool OverrideCircDarknessAmount;

	// Token: 0x040030FA RID: 12538
	[Range(0f, 1f)]
	public float CircDarknessAmount = 0.5f;

	// Token: 0x040030FB RID: 12539
	[Space]
	public bool OverrideCircDarknessSoftness;

	// Token: 0x040030FC RID: 12540
	[Range(0.001f, 1f)]
	public float CircDarknessSoftness = 0.5f;
}

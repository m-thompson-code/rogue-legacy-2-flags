using System;
using UnityEngine;

// Token: 0x02000B8B RID: 2955
[CreateAssetMenu(fileName = "New Profile", menuName = "Custom/Mobile Post Processing Profile")]
public class MobilePostProcessingProfile : ScriptableObject
{
	// Token: 0x06005925 RID: 22821 RVA: 0x00030812 File Offset: 0x0002EA12
	public MobilePostProcessingProfile Clone()
	{
		return (MobilePostProcessingProfile)base.MemberwiseClone();
	}

	// Token: 0x040042BA RID: 17082
	[Header("Blur")]
	public bool EnableBlurEffect;

	// Token: 0x040042BB RID: 17083
	[Space]
	public bool OverrideBlurOverallAmount;

	// Token: 0x040042BC RID: 17084
	[Range(0f, 1f)]
	public float BlurOverallAmount = 1f;

	// Token: 0x040042BD RID: 17085
	[Space]
	public bool OverrideBlurBaselineAmount;

	// Token: 0x040042BE RID: 17086
	[Range(0f, 1f)]
	public float BlurBaselineAmount;

	// Token: 0x040042BF RID: 17087
	[Space]
	public bool OverrideBlurRedChannel;

	// Token: 0x040042C0 RID: 17088
	[Range(-1f, 1f)]
	public float BlurRedChannel;

	// Token: 0x040042C1 RID: 17089
	[Space]
	public bool OverrideBlurGreenChannel;

	// Token: 0x040042C2 RID: 17090
	[Range(-1f, 1f)]
	public float BlurGreenChannel;

	// Token: 0x040042C3 RID: 17091
	[Space]
	public bool OverrideBlurBlueChannel;

	// Token: 0x040042C4 RID: 17092
	[Range(-1f, 1f)]
	public float BlurBlueChannel;

	// Token: 0x040042C5 RID: 17093
	[Space]
	public bool OverrideBlurAlphaChannel;

	// Token: 0x040042C6 RID: 17094
	[Range(-1f, 1f)]
	public float BlurAlphaChannel;

	// Token: 0x040042C7 RID: 17095
	[Header("Bloom")]
	public bool EnableBloomEffect;

	// Token: 0x040042C8 RID: 17096
	[Space]
	public bool OverrideBloomColor;

	// Token: 0x040042C9 RID: 17097
	public Color BloomColor = MobilePostProcessing.BLOOM_COLOR_DEFAULT;

	// Token: 0x040042CA RID: 17098
	[Space]
	public bool OverrideBloomAmount;

	// Token: 0x040042CB RID: 17099
	[Range(0f, 5f)]
	public float BloomAmount = 1f;

	// Token: 0x040042CC RID: 17100
	[Space]
	public bool OverrideBloomDiffuse;

	// Token: 0x040042CD RID: 17101
	[Range(0f, 1f)]
	public float BloomDiffuse = 0.5f;

	// Token: 0x040042CE RID: 17102
	[Space]
	public bool OverrideBloomThreshold;

	// Token: 0x040042CF RID: 17103
	[Range(0f, 1f)]
	public float BloomThreshold = 0.5f;

	// Token: 0x040042D0 RID: 17104
	[Space]
	public bool OverrideBloomSoftness;

	// Token: 0x040042D1 RID: 17105
	[Range(0f, 1f)]
	public float BloomSoftness = 0.5f;

	// Token: 0x040042D2 RID: 17106
	[Space]
	[Header("LUT")]
	public bool EnableLUTEffect;

	// Token: 0x040042D3 RID: 17107
	[Space]
	public bool OverrideLutAmount;

	// Token: 0x040042D4 RID: 17108
	[Range(0f, 1f)]
	public float LutAmount = 0.5f;

	// Token: 0x040042D5 RID: 17109
	[Space]
	public bool OverrideSourceLut;

	// Token: 0x040042D6 RID: 17110
	public Texture2D SourceLut;

	// Token: 0x040042D7 RID: 17111
	[Space]
	[Header("Image Filtering")]
	public bool EnableImageFilteringEffect;

	// Token: 0x040042D8 RID: 17112
	[Space]
	public bool OverrideColor;

	// Token: 0x040042D9 RID: 17113
	public Color Color = MobilePostProcessing.COLOR_DEFAULT;

	// Token: 0x040042DA RID: 17114
	[Space]
	public bool OverrideContrast;

	// Token: 0x040042DB RID: 17115
	[Range(0f, 1f)]
	public float Contrast;

	// Token: 0x040042DC RID: 17116
	[Space]
	public bool OverrideBrightness;

	// Token: 0x040042DD RID: 17117
	[Range(-1f, 1f)]
	public float Brightness;

	// Token: 0x040042DE RID: 17118
	[Space]
	public bool OverrideSaturation;

	// Token: 0x040042DF RID: 17119
	[Range(-1f, 1f)]
	public float Saturation;

	// Token: 0x040042E0 RID: 17120
	[Space]
	public bool OverrideExposure;

	// Token: 0x040042E1 RID: 17121
	[Range(-1f, 1f)]
	public float Exposure;

	// Token: 0x040042E2 RID: 17122
	[Space]
	public bool OverrideGamma;

	// Token: 0x040042E3 RID: 17123
	[Range(-1f, 1f)]
	public float Gamma;

	// Token: 0x040042E4 RID: 17124
	[Space]
	[Header("Gradient Map")]
	public bool EnableGradientMapEffect;

	// Token: 0x040042E5 RID: 17125
	[Space]
	public bool OverrideGradientBase;

	// Token: 0x040042E6 RID: 17126
	public Color GradientBase = MobilePostProcessing.GRADIENT_BASE_DEFAULT;

	// Token: 0x040042E7 RID: 17127
	[Space]
	public bool OverrideGradientDark;

	// Token: 0x040042E8 RID: 17128
	public Color GradientDark = MobilePostProcessing.GRADIENT_DARK_DEFAULT;

	// Token: 0x040042E9 RID: 17129
	[Space]
	public bool OverrideGradientMid;

	// Token: 0x040042EA RID: 17130
	public Color GradientMid = MobilePostProcessing.GRADIENT_MID_DEFAULT;

	// Token: 0x040042EB RID: 17131
	[Space]
	public bool OverrideGradientLight;

	// Token: 0x040042EC RID: 17132
	public Color GradientLight = MobilePostProcessing.GRADIENT_LIGHT_DEFAULT;

	// Token: 0x040042ED RID: 17133
	[Space]
	public bool OverrideGradientAmount;

	// Token: 0x040042EE RID: 17134
	[Range(0f, 1f)]
	public float GradientAmount = 0.5f;

	// Token: 0x040042EF RID: 17135
	[Space]
	public bool OverrideDarkCutoff;

	// Token: 0x040042F0 RID: 17136
	[Range(0f, 0.3f)]
	public float DarkCutoff;

	// Token: 0x040042F1 RID: 17137
	[Space]
	public bool OverrideLightCutoff;

	// Token: 0x040042F2 RID: 17138
	[Range(0.7f, 1f)]
	public float LightCutoff = 1f;

	// Token: 0x040042F3 RID: 17139
	[Space]
	[Header("Overlay Gradient")]
	public bool EnableOverlayGradientEffect;

	// Token: 0x040042F4 RID: 17140
	[Space]
	public bool OverrideOverlayTop;

	// Token: 0x040042F5 RID: 17141
	public Color OverlayTop = MobilePostProcessing.OVERLAY_TOP_DEFAULT;

	// Token: 0x040042F6 RID: 17142
	[Space]
	public bool OverrideOverlayBottom;

	// Token: 0x040042F7 RID: 17143
	public Color OverlayBottom = MobilePostProcessing.OVERLAY_BOTTOM_DEFAULT;

	// Token: 0x040042F8 RID: 17144
	[Space]
	public bool OverrideOverlayAmount;

	// Token: 0x040042F9 RID: 17145
	[Range(0f, 1f)]
	public float OverlayAmount = 0.5f;

	// Token: 0x040042FA RID: 17146
	[Space]
	[Header("Clamp Black")]
	public bool EnableClampBlackEffect;

	// Token: 0x040042FB RID: 17147
	[Space]
	public bool OverrideClampColor;

	// Token: 0x040042FC RID: 17148
	public Color ClampColor = MobilePostProcessing.CLAMP_COLOR_DEFAULT;

	// Token: 0x040042FD RID: 17149
	[Space]
	public bool OverrideClampFillAmount;

	// Token: 0x040042FE RID: 17150
	[Range(0f, 1f)]
	public float ClampFillAmount = 0.5f;

	// Token: 0x040042FF RID: 17151
	[Space]
	[Header("Tint")]
	public bool EnableTintEffect;

	// Token: 0x04004300 RID: 17152
	[Space]
	public bool OverrideTintColor;

	// Token: 0x04004301 RID: 17153
	public Color TintColor = MobilePostProcessing.TINT_COLOR_DEFAULT;

	// Token: 0x04004302 RID: 17154
	[Space]
	public bool OverrideTintOverallAmount;

	// Token: 0x04004303 RID: 17155
	[Range(0f, 1f)]
	public float TintOverallAmount = 1f;

	// Token: 0x04004304 RID: 17156
	[Space]
	public bool OverrideTintBaselineAmount;

	// Token: 0x04004305 RID: 17157
	[Range(0f, 1f)]
	public float TintBaselineAmount;

	// Token: 0x04004306 RID: 17158
	[Space]
	public bool OverrideTintRedChannel;

	// Token: 0x04004307 RID: 17159
	[Range(-1f, 1f)]
	public float TintRedChannel;

	// Token: 0x04004308 RID: 17160
	[Space]
	public bool OverrideTintGreenChannel;

	// Token: 0x04004309 RID: 17161
	[Range(-1f, 1f)]
	public float TintGreenChannel;

	// Token: 0x0400430A RID: 17162
	[Space]
	public bool OverrideTintBlueChannel;

	// Token: 0x0400430B RID: 17163
	[Range(-1f, 1f)]
	public float TintBlueChannel;

	// Token: 0x0400430C RID: 17164
	[Space]
	public bool OverrideTintAlphaChannel;

	// Token: 0x0400430D RID: 17165
	[Range(-1f, 1f)]
	public float TintAlphaChannel;

	// Token: 0x0400430E RID: 17166
	[Space]
	[Header("Chromatic Abberation")]
	public bool EnableChromaticAbberationEffect;

	// Token: 0x0400430F RID: 17167
	[Space]
	public bool OverrideOffset;

	// Token: 0x04004310 RID: 17168
	public float Offset = 1f;

	// Token: 0x04004311 RID: 17169
	[Space]
	public bool OverrideFishEyeDistortion;

	// Token: 0x04004312 RID: 17170
	[Range(-1f, 1f)]
	public float FishEyeDistortion;

	// Token: 0x04004313 RID: 17171
	[Space]
	[Header("Distortion")]
	public bool EnableDistortionEffect;

	// Token: 0x04004314 RID: 17172
	[Space]
	public bool OverrideLensDistortion;

	// Token: 0x04004315 RID: 17173
	[Range(0f, 1f)]
	public float LensDistortion = 0.5f;

	// Token: 0x04004316 RID: 17174
	[Space]
	[Header("Vignette")]
	public bool EnableVignetteEffect;

	// Token: 0x04004317 RID: 17175
	[Space]
	public bool OverrideVignetteCenter;

	// Token: 0x04004318 RID: 17176
	public Vector2 VignetteCenter = MobilePostProcessing.VIGNETTE_CENTER_DEFAULT;

	// Token: 0x04004319 RID: 17177
	[Space]
	public bool OverrideVignetteColor;

	// Token: 0x0400431A RID: 17178
	public Color VignetteColor = MobilePostProcessing.VIGNETTE_COLOR_DEFAULT;

	// Token: 0x0400431B RID: 17179
	[Space]
	public bool OverrideVignetteAmount;

	// Token: 0x0400431C RID: 17180
	[Range(0f, 1f)]
	public float VignetteAmount = 0.5f;

	// Token: 0x0400431D RID: 17181
	[Space]
	public bool OverrideVignetteSoftness;

	// Token: 0x0400431E RID: 17182
	[Range(0.001f, 1f)]
	public float VignetteSoftness = 0.5f;

	// Token: 0x0400431F RID: 17183
	[Space]
	public bool OverrideVignetteRoundness;

	// Token: 0x04004320 RID: 17184
	[Range(0f, 1f)]
	public float VignetteRoundness = 0.5f;

	// Token: 0x04004321 RID: 17185
	[Space]
	[Header("Mist")]
	public bool EnableMistEffect;

	// Token: 0x04004322 RID: 17186
	[Space]
	public bool OverrideMistColor;

	// Token: 0x04004323 RID: 17187
	public Color MistColor = MobilePostProcessing.MIST_COLOR_DEFAULT;

	// Token: 0x04004324 RID: 17188
	[Space]
	public bool OverrideMistAmount;

	// Token: 0x04004325 RID: 17189
	[Range(0f, 1f)]
	public float MistAmount = 0.1f;

	// Token: 0x04004326 RID: 17190
	[Space]
	[Header("Pixelation")]
	public bool EnablePixelationEffect;

	// Token: 0x04004327 RID: 17191
	[Space]
	public bool OverridePixelResolution;

	// Token: 0x04004328 RID: 17192
	public Vector2 PixelResolution = MobilePostProcessing.PIXEL_RESOLUTION_DEFAULT;

	// Token: 0x04004329 RID: 17193
	[Space]
	public bool OverridePixelBaselineAmount;

	// Token: 0x0400432A RID: 17194
	[Range(0f, 1f)]
	public float PixelBaselineAmount;

	// Token: 0x0400432B RID: 17195
	[Space]
	public bool OverridePixelRedChannel;

	// Token: 0x0400432C RID: 17196
	[Range(-1f, 1f)]
	public float PixelRedChannel;

	// Token: 0x0400432D RID: 17197
	[Space]
	public bool OverridePixelGreenChannel;

	// Token: 0x0400432E RID: 17198
	[Range(-1f, 1f)]
	public float PixelGreenChannel;

	// Token: 0x0400432F RID: 17199
	[Space]
	public bool OverridePixelBlueChannel;

	// Token: 0x04004330 RID: 17200
	[Range(-1f, 1f)]
	public float PixelBlueChannel;

	// Token: 0x04004331 RID: 17201
	[Space]
	public bool OverridePixelAlphaChannel;

	// Token: 0x04004332 RID: 17202
	[Range(-1f, 1f)]
	public float PixelAlphaChannel;

	// Token: 0x04004333 RID: 17203
	[Space]
	[Header("Dither")]
	public bool EnableDitherEffect;

	// Token: 0x04004334 RID: 17204
	[Space]
	public bool OverrideDitherDark;

	// Token: 0x04004335 RID: 17205
	public Color DitherDark = MobilePostProcessing.DITHER_DARK_DEFAULT;

	// Token: 0x04004336 RID: 17206
	[Space]
	public bool OverrideDitherLight;

	// Token: 0x04004337 RID: 17207
	public Color DitherLight = MobilePostProcessing.DITHER_LIGHT_DEFAULT;

	// Token: 0x04004338 RID: 17208
	[Space]
	[Header("Screen Flip")]
	public bool OverrideScreenFlipXEffect;

	// Token: 0x04004339 RID: 17209
	public bool ScreenFlipX;

	// Token: 0x0400433A RID: 17210
	[Space]
	public bool OverrideScreenFlipYEffect;

	// Token: 0x0400433B RID: 17211
	public bool ScreenFlipY;

	// Token: 0x0400433C RID: 17212
	[Space]
	[Header("Animated Overlay")]
	public bool EnableAnimatedOverlayEffect;

	// Token: 0x0400433D RID: 17213
	[Space]
	public bool OverrideAnimOverlayTexture;

	// Token: 0x0400433E RID: 17214
	public Texture2D AnimOverlayTexture = MobilePostProcessing.ANIMOVERLAY_TEXTURE_DEFAULT;

	// Token: 0x0400433F RID: 17215
	[Space]
	public bool OverrideAnimOverlaySpritesheetSize;

	// Token: 0x04004340 RID: 17216
	public Vector2 AnimOverlaySpritesheetSize = MobilePostProcessing.ANIMOVERLAY_SPRITESHEET_SIZE_DEFAULT;

	// Token: 0x04004341 RID: 17217
	[Space]
	public bool OverrideAnimOverlayFramerate;

	// Token: 0x04004342 RID: 17218
	[Range(0f, 60f)]
	public int AnimOverlayFramerate = 30;

	// Token: 0x04004343 RID: 17219
	[Space]
	public bool OverrideAnimOverlayStrength;

	// Token: 0x04004344 RID: 17220
	[Range(0f, 1f)]
	public float AnimOverlayStrength = 0.5f;

	// Token: 0x04004345 RID: 17221
	[Space]
	[Header("Circular Darkness")]
	public bool EnableCircularDarknessEffect;

	// Token: 0x04004346 RID: 17222
	[Space]
	public bool OverrideCircDarknessColor;

	// Token: 0x04004347 RID: 17223
	public Color CircDarknessColor = MobilePostProcessing.CIRCDARKNESS_COLOR_DEFAULT;

	// Token: 0x04004348 RID: 17224
	[Space]
	public bool OverrideCircDarknessAmount;

	// Token: 0x04004349 RID: 17225
	[Range(0f, 1f)]
	public float CircDarknessAmount = 0.5f;

	// Token: 0x0400434A RID: 17226
	[Space]
	public bool OverrideCircDarknessSoftness;

	// Token: 0x0400434B RID: 17227
	[Range(0.001f, 1f)]
	public float CircDarknessSoftness = 0.5f;
}

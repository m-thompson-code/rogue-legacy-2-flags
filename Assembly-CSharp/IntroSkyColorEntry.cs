using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003C4 RID: 964
[Serializable]
public class IntroSkyColorEntry
{
	// Token: 0x04001C5C RID: 7260
	public Color SkyColor;

	// Token: 0x04001C5D RID: 7261
	public Color HorizonColor;

	// Token: 0x04001C5E RID: 7262
	public Color CloudMainColor;

	// Token: 0x04001C5F RID: 7263
	public Color CloudHighlightColor;

	// Token: 0x04001C60 RID: 7264
	public Color StarColor;

	// Token: 0x04001C61 RID: 7265
	public Color RightColor;

	// Token: 0x04001C62 RID: 7266
	public Color BottomColor;

	// Token: 0x04001C63 RID: 7267
	[FormerlySerializedAs("FogColor")]
	public Color MistColor;

	// Token: 0x04001C64 RID: 7268
	public Color FogRenderColor;

	// Token: 0x04001C65 RID: 7269
	public Color Beam1Color;

	// Token: 0x04001C66 RID: 7270
	public Color Beam2Color;

	// Token: 0x04001C67 RID: 7271
	public Color RubbleColor1;

	// Token: 0x04001C68 RID: 7272
	public Color RubbleColor2;

	// Token: 0x04001C69 RID: 7273
	public Color GlowColor;

	// Token: 0x04001C6A RID: 7274
	public MobilePostProcessingProfile PostProcessProfile;

	// Token: 0x04001C6B RID: 7275
	public Sprite BannerSprite;
}

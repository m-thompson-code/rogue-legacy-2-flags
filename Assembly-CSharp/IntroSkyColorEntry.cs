using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000211 RID: 529
[Serializable]
public class IntroSkyColorEntry
{
	// Token: 0x04001559 RID: 5465
	public Color SkyColor;

	// Token: 0x0400155A RID: 5466
	public Color HorizonColor;

	// Token: 0x0400155B RID: 5467
	public Color CloudMainColor;

	// Token: 0x0400155C RID: 5468
	public Color CloudHighlightColor;

	// Token: 0x0400155D RID: 5469
	public Color StarColor;

	// Token: 0x0400155E RID: 5470
	public Color RightColor;

	// Token: 0x0400155F RID: 5471
	public Color BottomColor;

	// Token: 0x04001560 RID: 5472
	[FormerlySerializedAs("FogColor")]
	public Color MistColor;

	// Token: 0x04001561 RID: 5473
	public Color FogRenderColor;

	// Token: 0x04001562 RID: 5474
	public Color Beam1Color;

	// Token: 0x04001563 RID: 5475
	public Color Beam2Color;

	// Token: 0x04001564 RID: 5476
	public Color RubbleColor1;

	// Token: 0x04001565 RID: 5477
	public Color RubbleColor2;

	// Token: 0x04001566 RID: 5478
	public Color GlowColor;

	// Token: 0x04001567 RID: 5479
	public MobilePostProcessingProfile PostProcessProfile;

	// Token: 0x04001568 RID: 5480
	public Sprite BannerSprite;
}

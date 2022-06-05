using System;
using UnityEngine;

// Token: 0x02000A7C RID: 2684
[Serializable]
public class TunnelSpawnControllerVisuals
{
	// Token: 0x0600512B RID: 20779 RVA: 0x00133954 File Offset: 0x00131B54
	public void SetSprite(TunnelCategory category, TunnelDirection tunnelType)
	{
		Sprite sprite = this.DefaultSprite;
		Sprite sprite2 = this.EntranceSprite;
		Sprite sprite3 = this.ExitSprite;
		if (category <= TunnelCategory.Boss)
		{
			if (category != TunnelCategory.Default)
			{
				if (category == TunnelCategory.Boss)
				{
					sprite2 = this.BossEntranceSprite;
					sprite3 = this.BossExitSprite;
				}
			}
		}
		else if (category != TunnelCategory.Bonus)
		{
			if (category == TunnelCategory.Final)
			{
				sprite2 = this.FinalEntranceSprite;
				sprite3 = this.FinalExitSprite;
			}
		}
		if (tunnelType != TunnelDirection.Entrance)
		{
			if (tunnelType == TunnelDirection.Exit)
			{
				sprite = sprite3;
			}
		}
		else
		{
			sprite = sprite2;
		}
		this.SpriteRenderer.sprite = sprite;
	}

	// Token: 0x04003D4A RID: 15690
	public SpriteRenderer SpriteRenderer;

	// Token: 0x04003D4B RID: 15691
	public Sprite DefaultSprite;

	// Token: 0x04003D4C RID: 15692
	public Sprite EntranceSprite;

	// Token: 0x04003D4D RID: 15693
	public Sprite ExitSprite;

	// Token: 0x04003D4E RID: 15694
	public Sprite BossEntranceSprite;

	// Token: 0x04003D4F RID: 15695
	public Sprite BossExitSprite;

	// Token: 0x04003D50 RID: 15696
	public Sprite FinalEntranceSprite;

	// Token: 0x04003D51 RID: 15697
	public Sprite FinalExitSprite;
}

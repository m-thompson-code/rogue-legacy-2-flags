using System;
using UnityEngine;

// Token: 0x0200064D RID: 1613
[Serializable]
public class TunnelSpawnControllerVisuals
{
	// Token: 0x06003A46 RID: 14918 RVA: 0x000C5C1C File Offset: 0x000C3E1C
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

	// Token: 0x04002CB4 RID: 11444
	public SpriteRenderer SpriteRenderer;

	// Token: 0x04002CB5 RID: 11445
	public Sprite DefaultSprite;

	// Token: 0x04002CB6 RID: 11446
	public Sprite EntranceSprite;

	// Token: 0x04002CB7 RID: 11447
	public Sprite ExitSprite;

	// Token: 0x04002CB8 RID: 11448
	public Sprite BossEntranceSprite;

	// Token: 0x04002CB9 RID: 11449
	public Sprite BossExitSprite;

	// Token: 0x04002CBA RID: 11450
	public Sprite FinalEntranceSprite;

	// Token: 0x04002CBB RID: 11451
	public Sprite FinalExitSprite;
}

using System;

// Token: 0x020004C9 RID: 1225
[Serializable]
public class HighestNGBossBeatenEntry
{
	// Token: 0x06002771 RID: 10097 RVA: 0x000B99F4 File Offset: 0x000B7BF4
	public int GetHighestNGBossBeaten(BossID bossID)
	{
		if (bossID <= BossID.Study_Boss)
		{
			if (bossID <= BossID.Bridge_Boss)
			{
				if (bossID == BossID.Castle_Boss)
				{
					return this.HighestCastleBossBeaten;
				}
				if (bossID == BossID.Bridge_Boss)
				{
					return this.HighestBridgeBossBeaten;
				}
			}
			else
			{
				if (bossID == BossID.Forest_Boss)
				{
					return this.HighestForestBossBeaten;
				}
				if (bossID == BossID.Study_Boss)
				{
					return this.HighestStudyBossBeaten;
				}
			}
		}
		else if (bossID <= BossID.Cave_Boss)
		{
			if (bossID == BossID.Tower_Boss)
			{
				return this.HighestTowerBossBeaten;
			}
			if (bossID == BossID.Cave_Boss)
			{
				return this.HighestCaveBossBeaten;
			}
		}
		else
		{
			if (bossID == BossID.Garden_Boss)
			{
				return this.HighestGardenBossBeaten;
			}
			if (bossID == BossID.Final_Boss)
			{
				return this.HighestFinalBossBeaten;
			}
		}
		return -1;
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x000B9A7C File Offset: 0x000B7C7C
	public void SetHighestNGBossBeaten(BossID bossID, int value, bool forceOverride)
	{
		int highestNGBossBeaten = this.GetHighestNGBossBeaten(bossID);
		if (value <= highestNGBossBeaten && !forceOverride)
		{
			return;
		}
		if (bossID <= BossID.Study_Boss)
		{
			if (bossID <= BossID.Bridge_Boss)
			{
				if (bossID == BossID.Castle_Boss)
				{
					this.HighestCastleBossBeaten = value;
					return;
				}
				if (bossID != BossID.Bridge_Boss)
				{
					return;
				}
				this.HighestBridgeBossBeaten = value;
				return;
			}
			else
			{
				if (bossID == BossID.Forest_Boss)
				{
					this.HighestForestBossBeaten = value;
					return;
				}
				if (bossID != BossID.Study_Boss)
				{
					return;
				}
				this.HighestStudyBossBeaten = value;
				return;
			}
		}
		else if (bossID <= BossID.Cave_Boss)
		{
			if (bossID == BossID.Tower_Boss)
			{
				this.HighestTowerBossBeaten = value;
				return;
			}
			if (bossID != BossID.Cave_Boss)
			{
				return;
			}
			this.HighestCaveBossBeaten = value;
			return;
		}
		else
		{
			if (bossID == BossID.Garden_Boss)
			{
				this.HighestGardenBossBeaten = value;
				return;
			}
			if (bossID != BossID.Final_Boss)
			{
				return;
			}
			this.HighestFinalBossBeaten = value;
			return;
		}
	}

	// Token: 0x0400220C RID: 8716
	public int HighestCastleBossBeaten = -1;

	// Token: 0x0400220D RID: 8717
	public int HighestBridgeBossBeaten = -1;

	// Token: 0x0400220E RID: 8718
	public int HighestForestBossBeaten = -1;

	// Token: 0x0400220F RID: 8719
	public int HighestStudyBossBeaten = -1;

	// Token: 0x04002210 RID: 8720
	public int HighestTowerBossBeaten = -1;

	// Token: 0x04002211 RID: 8721
	public int HighestCaveBossBeaten = -1;

	// Token: 0x04002212 RID: 8722
	public int HighestGardenBossBeaten = -1;

	// Token: 0x04002213 RID: 8723
	public int HighestFinalBossBeaten = -1;
}

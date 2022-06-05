using System;

// Token: 0x020002D4 RID: 724
[Serializable]
public class HighestNGBossBeatenEntry
{
	// Token: 0x06001CA7 RID: 7335 RVA: 0x0005D66C File Offset: 0x0005B86C
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

	// Token: 0x06001CA8 RID: 7336 RVA: 0x0005D6F4 File Offset: 0x0005B8F4
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

	// Token: 0x04001A05 RID: 6661
	public int HighestCastleBossBeaten = -1;

	// Token: 0x04001A06 RID: 6662
	public int HighestBridgeBossBeaten = -1;

	// Token: 0x04001A07 RID: 6663
	public int HighestForestBossBeaten = -1;

	// Token: 0x04001A08 RID: 6664
	public int HighestStudyBossBeaten = -1;

	// Token: 0x04001A09 RID: 6665
	public int HighestTowerBossBeaten = -1;

	// Token: 0x04001A0A RID: 6666
	public int HighestCaveBossBeaten = -1;

	// Token: 0x04001A0B RID: 6667
	public int HighestGardenBossBeaten = -1;

	// Token: 0x04001A0C RID: 6668
	public int HighestFinalBossBeaten = -1;
}

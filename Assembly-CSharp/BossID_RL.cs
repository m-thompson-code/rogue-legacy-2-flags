using System;

// Token: 0x02000BBD RID: 3005
public class BossID_RL
{
	// Token: 0x06005A1E RID: 23070 RVA: 0x00154DA8 File Offset: 0x00152FA8
	public static bool IsBossBeaten(BossID bossID)
	{
		if (bossID <= BossID.Study_Boss)
		{
			if (bossID <= BossID.Bridge_Boss)
			{
				if (bossID == BossID.Castle_Boss)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CastleBoss_Defeated);
				}
				if (bossID == BossID.Bridge_Boss)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BridgeBoss_Defeated);
				}
			}
			else
			{
				if (bossID == BossID.Forest_Boss)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ForestBoss_Defeated);
				}
				if (bossID == BossID.Study_Boss)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyBoss_Defeated);
				}
			}
		}
		else if (bossID <= BossID.Cave_Boss)
		{
			if (bossID == BossID.Tower_Boss)
			{
				return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TowerBoss_Defeated);
			}
			if (bossID == BossID.Cave_Boss)
			{
				return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated);
			}
		}
		else
		{
			if (bossID == BossID.Garden_Boss)
			{
				return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Defeated);
			}
			if (bossID == BossID.Final_Boss)
			{
				return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.FinalBoss_Defeated);
			}
		}
		return false;
	}

	// Token: 0x06005A1F RID: 23071 RVA: 0x000313DE File Offset: 0x0002F5DE
	public static BiomeType GetBossIDToBiome(BossID bossID)
	{
		if (bossID <= BossID.Forest_Boss)
		{
			if (bossID == BossID.Castle_Boss)
			{
				return BiomeType.Castle;
			}
			if (bossID == BossID.Bridge_Boss)
			{
				return BiomeType.Stone;
			}
			if (bossID == BossID.Forest_Boss)
			{
				return BiomeType.Forest;
			}
		}
		else
		{
			if (bossID == BossID.Study_Boss)
			{
				return BiomeType.Study;
			}
			if (bossID == BossID.Tower_Boss)
			{
				return BiomeType.Tower;
			}
			if (bossID == BossID.Cave_Boss)
			{
				return BiomeType.Cave;
			}
		}
		return BiomeType.None;
	}

	// Token: 0x06005A20 RID: 23072 RVA: 0x0003141D File Offset: 0x0002F61D
	public static BossID GetBiomeToBossID(BiomeType biomeType)
	{
		if (biomeType <= BiomeType.Forest)
		{
			if (biomeType == BiomeType.Castle)
			{
				return BossID.Castle_Boss;
			}
			if (biomeType == BiomeType.Cave)
			{
				return BossID.Cave_Boss;
			}
			if (biomeType == BiomeType.Forest)
			{
				return BossID.Forest_Boss;
			}
		}
		else
		{
			if (biomeType == BiomeType.Stone)
			{
				return BossID.Bridge_Boss;
			}
			if (biomeType == BiomeType.Study)
			{
				return BossID.Study_Boss;
			}
			if (biomeType == BiomeType.Tower)
			{
				return BossID.Tower_Boss;
			}
		}
		return BossID.None;
	}

	// Token: 0x17001E1F RID: 7711
	// (get) Token: 0x06005A21 RID: 23073 RVA: 0x0003145C File Offset: 0x0002F65C
	public static BossID[] TypeArray
	{
		get
		{
			if (BossID_RL.m_typeArray == null)
			{
				BossID_RL.m_typeArray = (Enum.GetValues(typeof(BossID)) as BossID[]);
			}
			return BossID_RL.m_typeArray;
		}
	}

	// Token: 0x0400454E RID: 17742
	private static BossID[] m_typeArray;
}

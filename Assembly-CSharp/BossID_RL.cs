using System;

// Token: 0x0200070A RID: 1802
public class BossID_RL
{
	// Token: 0x060040D5 RID: 16597 RVA: 0x000E5864 File Offset: 0x000E3A64
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

	// Token: 0x060040D6 RID: 16598 RVA: 0x000E593D File Offset: 0x000E3B3D
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

	// Token: 0x060040D7 RID: 16599 RVA: 0x000E597C File Offset: 0x000E3B7C
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

	// Token: 0x17001623 RID: 5667
	// (get) Token: 0x060040D8 RID: 16600 RVA: 0x000E59BB File Offset: 0x000E3BBB
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

	// Token: 0x040032D3 RID: 13011
	private static BossID[] m_typeArray;
}

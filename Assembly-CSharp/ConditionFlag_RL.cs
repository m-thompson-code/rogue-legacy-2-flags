using System;

// Token: 0x0200071E RID: 1822
public class ConditionFlag_RL
{
	// Token: 0x1700162C RID: 5676
	// (get) Token: 0x060040EC RID: 16620 RVA: 0x000E5D57 File Offset: 0x000E3F57
	public static ConditionFlag[] TypeArray
	{
		get
		{
			if (ConditionFlag_RL.m_typeArray == null)
			{
				ConditionFlag_RL.m_typeArray = (Enum.GetValues(typeof(ConditionFlag)) as ConditionFlag[]);
			}
			return ConditionFlag_RL.m_typeArray;
		}
	}

	// Token: 0x060040ED RID: 16621 RVA: 0x000E5D80 File Offset: 0x000E3F80
	public static bool IsConditionFulfilled(ConditionFlag id)
	{
		if (id <= ConditionFlag.Insight_CastleBoss_DoorOpened_Discovered)
		{
			if (id <= ConditionFlag.Heirloom_VoidDash)
			{
				switch (id)
				{
				case ConditionFlag.Heirloom_Dash:
					return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) > 0;
				case ConditionFlag.Heirloom_Memory:
					return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockMemory) > 0;
				case ConditionFlag.Heirloom_Dash | ConditionFlag.Heirloom_Memory:
					break;
				case ConditionFlag.Heirloom_DoubleJump:
					return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockDoubleJump) > 0;
				default:
					if (id == ConditionFlag.Heirloom_Downstrike)
					{
						return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockBouncableDownstrike) > 0;
					}
					if (id == ConditionFlag.Heirloom_VoidDash)
					{
						return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockVoidDash) > 0;
					}
					break;
				}
			}
			else if (id <= ConditionFlag.Insight_HeirloomDoubleJump_Hidden_Discovered)
			{
				if (id == ConditionFlag.Insight_HeirloomSpinKick_Hidden_Discovered)
				{
					return SaveManager.PlayerSaveData.GetInsightState(InsightType.HeirloomSpinKick_Hidden) > InsightState.Undiscovered;
				}
				if (id == ConditionFlag.Insight_HeirloomDoubleJump_Hidden_Discovered)
				{
					return SaveManager.PlayerSaveData.GetInsightState(InsightType.HeirloomDoubleJump_Hidden) > InsightState.Undiscovered;
				}
			}
			else
			{
				if (id == ConditionFlag.Insight_CastleBoss_DoorOpened_Resolved)
				{
					return SaveManager.PlayerSaveData.GetInsightState(InsightType.CastleBoss_DoorOpened) > InsightState.DiscoveredAndViewed;
				}
				if (id == ConditionFlag.Insight_CastleBoss_DoorOpened_Discovered)
				{
					return SaveManager.PlayerSaveData.GetInsightState(InsightType.CastleBoss_DoorOpened) > InsightState.Undiscovered;
				}
			}
		}
		else if (id <= ConditionFlag.BossDefeated_Study)
		{
			if (id <= ConditionFlag.BossDefeated_Bridge)
			{
				if (id == ConditionFlag.BossDefeated_Castle)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CastleBoss_Defeated);
				}
				if (id == ConditionFlag.BossDefeated_Bridge)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BridgeBoss_Defeated);
				}
			}
			else
			{
				if (id == ConditionFlag.BossDefeated_Forest)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ForestBoss_Defeated);
				}
				if (id == ConditionFlag.BossDefeated_Study)
				{
					return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyBoss_Defeated);
				}
			}
		}
		else if (id <= ConditionFlag.BossDefeated_Cave)
		{
			if (id == ConditionFlag.BossDefeated_Tower)
			{
				return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TowerBoss_Defeated);
			}
			if (id == ConditionFlag.BossDefeated_Cave)
			{
				return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated);
			}
		}
		else
		{
			if (id == ConditionFlag.BossDefeated_Final)
			{
				return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.FinalBoss_Defeated);
			}
			if (id == ConditionFlag.BossDefeated_Garden)
			{
				return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Defeated);
			}
		}
		return true;
	}

	// Token: 0x04003378 RID: 13176
	private static ConditionFlag[] m_typeArray;
}

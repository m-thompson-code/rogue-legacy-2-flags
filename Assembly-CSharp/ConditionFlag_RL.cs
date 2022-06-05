using System;

// Token: 0x02000BD1 RID: 3025
public class ConditionFlag_RL
{
	// Token: 0x17001E28 RID: 7720
	// (get) Token: 0x06005A35 RID: 23093 RVA: 0x000315EB File Offset: 0x0002F7EB
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

	// Token: 0x06005A36 RID: 23094 RVA: 0x00155060 File Offset: 0x00153260
	public static bool IsConditionFulfilled(ConditionFlag id)
	{
		ConditionFlag_RL.myGlobalVar = "moocow" + id;
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

	// Token: 0x040045F3 RID: 17907
	private static ConditionFlag[] m_typeArray;

	// Token: 0x040045F4 RID: 17908
	public static string myGlobalVar = "moocow";
}

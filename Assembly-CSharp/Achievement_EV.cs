using System;

// Token: 0x0200004E RID: 78
public class Achievement_EV
{
	// Token: 0x06000149 RID: 329 RVA: 0x00047E8C File Offset: 0x0004608C
	public static void RunAchievementSafetyChecks()
	{
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CastleBoss_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossCastleDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CastleBoss_Prime_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossCastleAdvancedDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BridgeBoss_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossBridgeDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.BridgeBoss_Prime_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossBridgeAdvancedDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ForestBoss_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossForestDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ForestBoss_Prime_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossForestAdvancedDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyBoss_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossStudyDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.StudyBoss_Prime_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossStudyAdvancedDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TowerBoss_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossTowerDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TowerBoss_Prime_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossTowerAdvancedDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossCaveDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Prime_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossCaveAdvancedDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossGardenDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Prime_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossGardenAdvancedDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.FinalBoss_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossFinalDefeated, true, false);
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.FinalBoss_Prime_Defeated_FirstTime))
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.BossFinalAdvancedDefeated, true, false);
		}
		if (ChallengeManager.GetChallengeTrophyRank(ChallengeType.NightmareKhidr, true) >= ChallengeTrophyRank.Bronze)
		{
			SaveManager.ModeSaveData.SetAchievementUnlocked(AchievementType.NightmareKhidr, true, false);
		}
	}
}

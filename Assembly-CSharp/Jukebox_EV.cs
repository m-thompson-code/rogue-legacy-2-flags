using System;
using System.Collections.Generic;

// Token: 0x02000077 RID: 119
public class Jukebox_EV
{
	// Token: 0x040003C0 RID: 960
	public static Dictionary<SongID, JukeboxData> JukeboxDataDict = new Dictionary<SongID, JukeboxData>
	{
		{
			SongID.LineageSceneBGM_02_ASITP_Interstitial_2,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_INTERSTITIAL2_1",
				AchievementUnlockType = AchievementType.None
			}
		},
		{
			SongID.LineageSceneBGM_01_ASITP_Interstitial_1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_INTERSTITIAL1_1",
				AchievementUnlockType = AchievementType.PrequelReveal
			}
		},
		{
			SongID.JUKEBOX_TitleSceneBGM_01_Tettix_Main_Title,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_MAINTITLE_1",
				AchievementUnlockType = AchievementType.EnableHouseRules
			}
		},
		{
			SongID.JUKEBOX_CastleBiomeBGM_01_ASITP_Level_1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_LEVEL1_1",
				AchievementUnlockType = AchievementType.HubtownSpinKick
			}
		},
		{
			SongID.JUKEBOX_CastleBiomeBGM_02_Tettix_Level_5,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_LEVEL5_1",
				AchievementUnlockType = AchievementType.AllSkills
			}
		},
		{
			SongID.JUKEBOX_CastleBossBGM_ASTIP_Boss1_Phase1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS1_1",
				AchievementUnlockType = AchievementType.BossCastleDefeated
			}
		},
		{
			SongID.JUKEBOX_BridgeBiomeBGM_01_ASTIP_Level_3,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_LEVEL3_1",
				AchievementUnlockType = AchievementType.BossBridgeAdvancedDefeated
			}
		},
		{
			SongID.JUKEBOX_BridgeBiomeBGM_02_Tettix_Level_10,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_LEVEL10_1",
				AchievementUnlockType = AchievementType.UnlockHighestUnity
			}
		},
		{
			SongID.JUKEBOX_BridgeBossBGM_Tettix_Miniboss_BGM_137,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_MINIBOSS_137BPM_1",
				AchievementUnlockType = AchievementType.BossBridgeDefeated
			}
		},
		{
			SongID.JUKEBOX_ForestBiomeBGM_01_ASITP_Level_2,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_LEVEL2_1",
				AchievementUnlockType = AchievementType.AllBlessings
			}
		},
		{
			SongID.JUKEBOX_ForestBiomeBGM_02_Tettix_Level_7,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_LEVEL7_1",
				AchievementUnlockType = AchievementType.BossForestAdvancedDefeated
			}
		},
		{
			SongID.JUKEBOX_ForestBossBGM_Tettix_Naamah_180_BPM,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_NAAMAH_180BPM_1",
				AchievementUnlockType = AchievementType.BossForestDefeated
			}
		},
		{
			SongID.JUKEBOX_StudyBiomeBGM_02_Tettix_Level_9,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_LEVEL9_1",
				AchievementUnlockType = AchievementType.AllRunes
			}
		},
		{
			SongID.JUKEBOX_StudyBiomeBGM_01_ASITP_Level_5,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_LEVEL5_1",
				AchievementUnlockType = AchievementType.BossStudyAdvancedDefeated
			}
		},
		{
			SongID.JUKEBOX_StudyBossBGM_Boss5_Phase1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS5_1",
				AchievementUnlockType = AchievementType.BossStudyDefeated
			}
		},
		{
			SongID.JUKEBOX_StudyBossBGM_Boss5_Phase2,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS5_PHASE_2_1",
				AchievementUnlockType = AchievementType.EndTimerFail
			}
		},
		{
			SongID.JUKEBOX_TowerBiomeBGM_02_ASITP_Level_7,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_LEVEL7_1",
				AchievementUnlockType = AchievementType.Icarus
			}
		},
		{
			SongID.JUKEBOX_TowerBiomeBGM_01_Tettix_Level_4,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_LEVEL4_1",
				AchievementUnlockType = AchievementType.BossTowerAdvancedDefeated
			}
		},
		{
			SongID.JUKEBOX_TowerBossBGM_ASITP_Boss_3,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS3_1",
				AchievementUnlockType = AchievementType.BossTowerDefeated
			}
		},
		{
			SongID.JUKEBOX_CaveBiomeBGM_01_Tettix_Level_6,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_LEVEL6_1",
				AchievementUnlockType = AchievementType.PetDragon
			}
		},
		{
			SongID.JUKEBOX_CaveBiomeBGM_02_ASITP_Level_6,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_LEVEL6_1",
				AchievementUnlockType = AchievementType.AllEquipment
			}
		},
		{
			SongID.JUKEBOX_CaveBossBGM_Tettix_Blacksmith_Boss,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_BLACKSMITHBOSS_1",
				AchievementUnlockType = AchievementType.BossCaveDefeated
			}
		},
		{
			SongID.JUKEBOX_TraitorBoss_Tettix,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_TRAITOR_130BPM_1",
				AchievementUnlockType = AchievementType.BossGardenDefeated
			}
		},
		{
			SongID.JUKEBOX_FinalBoss_ASITP_Basic,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS6_1",
				AchievementUnlockType = AchievementType.BossFinalDefeated
			}
		},
		{
			SongID.JUKEBOX_FinalBoss_ASITP_Stemage,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS6_STEMAGE_1",
				AchievementUnlockType = AchievementType.BossFinalAdvancedDefeated
			}
		},
		{
			SongID.JUKEBOX_Credits_ASITP,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_CREDITS_1",
				AchievementUnlockType = AchievementType.StoryTutorial
			}
		},
		{
			SongID.JUKEBOX_Parade_ASITP_Videri_Enemy_Parade,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_PARADE_1",
				AchievementUnlockType = AchievementType.BossFinalAdvancedDefeated
			}
		},
		{
			SongID.JUKEBOX_HeirloomSceneBGM_01_ASITP_Level_4,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_LEVEL4_1",
				AchievementUnlockType = AchievementType.BossCastleAdvancedDefeated
			}
		},
		{
			SongID.JUKEBOX_Misc_Synth_ASITP_SubBoss_1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_SUBBOSS1_1",
				AchievementUnlockType = AchievementType.BoxerBoss
			}
		},
		{
			SongID.JUKEBOX_Misc_GrandFight_Tettix_Boss_1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_BOSS1_1",
				AchievementUnlockType = AchievementType.UnlockHighMastery
			}
		},
		{
			SongID.JUKEBOX_Misc_Horror_ASITP_Boss_4,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS4_1",
				AchievementUnlockType = AchievementType.BossCaveAdvancedDefeated
			}
		},
		{
			SongID.Misc_RogueLegacy_1_ASITP_PistolShrimp_1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_PISTOL_SHRIMP_1",
				AchievementUnlockType = AchievementType.NightmareKhidr
			}
		},
		{
			SongID.JUKEBOX_Misc_TwinMech_ASITP_Boss_2_Phase_1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS2_PHASE_1_1",
				AchievementUnlockType = AchievementType.SoulShopOverload
			}
		},
		{
			SongID.JUKEBOX_Misc_TwinMech_ASITP_Boss_2_Phase_2,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_BOSS2_PHASE_2_1",
				AchievementUnlockType = AchievementType.AllScarsBronze
			}
		},
		{
			SongID.JUKEBOX_Misc_Church_Tettix_Level_2,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_TETTIX_LEVEL2_1",
				AchievementUnlockType = AchievementType.AllFriends
			}
		},
		{
			SongID.JUKEBOX_Misc_PirateSong_1_ASITP_PirateLoop_1,
			new JukeboxData
			{
				SongTitleLocID = "LOC_ID_SONG_ASITP_PIRATE_SONG_1",
				AchievementUnlockType = AchievementType.TitleDrop
			}
		}
	};
}

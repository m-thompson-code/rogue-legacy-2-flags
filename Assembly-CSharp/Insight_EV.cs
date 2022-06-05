using System;
using System.Collections.Generic;

// Token: 0x0200006A RID: 106
public class Insight_EV
{
	// Token: 0x0400038F RID: 911
	public const float INSIGHT_PLAYER_DAMAGE_MOD = 1.15f;

	// Token: 0x04000390 RID: 912
	public const float INSIGHT_BOSS_DAMAGE_MOD = 1f;

	// Token: 0x04000391 RID: 913
	public const float INSIGHT_FINALBOSS_PLAYER_DAMAGE_MOD = 0.05f;

	// Token: 0x04000392 RID: 914
	public const float INSIGHT_FINALBOSS_DAMAGE_MOD = 0f;

	// Token: 0x04000393 RID: 915
	public static Dictionary<InsightType, InsightLocIDEntry> LocIDTable = new Dictionary<InsightType, InsightLocIDEntry>
	{
		{
			InsightType.HeirloomDash,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_DASH_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_DASH_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_DASH_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_DASH_RESOLVED_1"
			}
		},
		{
			InsightType.HeirloomMemory,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_MEMORY_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_MEMORY_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_MEMORY_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_MEMORY_RESOLVED_1"
			}
		},
		{
			InsightType.SpellSwordBossCombatBonus,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_SPELLSWORD_BOSS_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_SPELLSWORD_BOSS_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_SPELLSWORD_BOSS_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_SPELLSWORD_BOSS_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.CastleBoss_DoorOpened,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_SPELLSWORD_BOSS_ENTRANCE_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_SPELLSWORD_BOSS_ENTRANCE_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_SPELLSWORD_BOSS_ENTRANCE_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_SPELLSWORD_BOSS_ENTRANCE_RESOLVED_1"
			}
		},
		{
			InsightType.HeirloomSpinKick_Hidden,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_FIND_HIDDEN_SPINKICK_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_FIND_HIDDEN_SPINKICK_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_FIND_HIDDEN_SPINKICK_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_FIND_HIDDEN_SPINKICK_RESOLVED_1"
			}
		},
		{
			InsightType.HeirloomSpinKick_Projectiles,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_SPINKICK_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_SPINKICK_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_SPINKICK_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_SPINKICK_RESOLVED_1"
			}
		},
		{
			InsightType.SkeletonBossCombatBonus,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_BRIDGE_BOSS_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_BRIDGE_BOSS_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_BRIDGE_BOSS_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_BRIDGE_BOSS_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.BridgeBoss_GateRaised,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_BRIDGE_BOSS_DEFEAT_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_BRIDGE_BOSS_DEFEAT_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_BRIDGE_BOSS_DEFEAT_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_BRIDGE_BOSS_DEFEAT_RESOLVED_1"
			}
		},
		{
			InsightType.HeirloomDoubleJump_Hidden,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_FIND_HIDDEN_DOUBLEJUMP_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_FIND_HIDDEN_DOUBLEJUMP_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_FIND_HIDDEN_DOUBLEJUMP_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_FIND_HIDDEN_DOUBLEJUMP_RESOLVED_1"
			}
		},
		{
			InsightType.HeirloomDoubleJump,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_DOUBLEJUMP_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_DOUBLEJUMP_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_DOUBLEJUMP_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_DOUBLEJUMP_RESOLVED_1"
			}
		},
		{
			InsightType.DancingBossCombatBonus,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_FOREST_BOSS_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_FOREST_BOSS_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_FOREST_BOSS_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_FOREST_BOSS_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.ForestBoss_DoorOpened,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_FOREST_BOSS_ENTRANCE_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_FOREST_BOSS_ENTRANCE_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_FOREST_BOSS_ENTRANCE_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_FOREST_BOSS_ENTRANCE_RESOLVED_1"
			}
		},
		{
			InsightType.HeirloomVoidDash,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_VOIDDASH_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_VOIDDASH_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_VOIDDASH_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_VOIDDASH_RESOLVED_1"
			}
		},
		{
			InsightType.StudyBossCombatBonus,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_STUDY_BOSS_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_STUDY_BOSS_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_STUDY_BOSS_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_STUDY_BOSS_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.StudyBoss_DoorOpened,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_STUDY_BOSS_ENTRANCE_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_STUDY_BOSS_ENTRANCE_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_STUDY_BOSS_ENTRANCE_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_STUDY_BOSS_ENTRANCE_RESOLVED_1"
			}
		},
		{
			InsightType.TowerBossCombatBonus,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_TOWER_BOSS_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_TOWER_BOSS_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_TOWER_BOSS_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_TOWER_BOSS_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.HeirloomLantern,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_LANTERN_HEIRLOOM_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_LANTERN_HEIRLOOM_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_LANTERN_HEIRLOOM_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_LANTERN_HEIRLOOM_RESOLVED_1"
			}
		},
		{
			InsightType.CaveBoss_DoorOpened,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_CAVE_BOSS_ENTRANCE_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_CAVE_BOSS_ENTRANCE_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_CAVE_BOSS_ENTRANCE_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_CAVE_BOSS_ENTRANCE_RESOLVED_1"
			}
		},
		{
			InsightType.CaveBossCombatBonus,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_CAVE_BOSS_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_CAVE_BOSS_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_CAVE_BOSS_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_CAVE_BOSS_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.TraitorBoss_HPReduce_SpellSwordBoss,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_CASTLE_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_CASTLE_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_CASTLE_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_CASTLE_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.FinalBoss_HPReduce_SkeletonBoss,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_BRIDGE_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_BRIDGE_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_BRIDGE_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_BRIDGE_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.TraitorBoss_HPReduce_DancingBoss,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_FOREST_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_FOREST_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_FOREST_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_FOREST_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.TraitorBoss_HPReduce_StudyBoss,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_STUDY_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_STUDY_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_STUDY_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_STUDY_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.FinalBoss_HPReduce_TowerBoss,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_TOWER_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_TOWER_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_TOWER_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_TOWER_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.FinalBoss_HPReduce_CaveBoss,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_CAVE_BONUS_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_CAVE_BONUS_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_CAVE_BONUS_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_PRIME_BOSS_CAVE_BONUS_RESOLVED_1"
			}
		},
		{
			InsightType.FinalDoorEntranceObjective,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_FINAL_DOOR_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_FINAL_DOOR_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_FINAL_DOOR_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_FINAL_DOOR_RESOLVED_1"
			}
		},
		{
			InsightType.TutorialEntranceObjective,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_TUTORIAL_INVADE_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_TUTORIAL_INVADE_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_TUTORIAL_INVADE_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_TUTORIAL_INVADE_RESOLVED_1"
			}
		},
		{
			InsightType.HeirloomEarthShift,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_EARTH_SHIFT_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_EARTH_SHIFT_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_EARTH_SHIFT_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_HEIRLOOM_EARTH_SHIFT_RESOLVED_1"
			}
		},
		{
			InsightType.Ending_RebelsHidout,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_REBEL_ROOM_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_REBEL_ROOM_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_REBEL_ROOM_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_REBEL_ROOM_RESOLVED_1"
			}
		},
		{
			InsightType.ScarChallenges_Complete,
			new InsightLocIDEntry
			{
				TitleLocID = "LOC_ID_INSIGHT_TEXT_CHALLENGE_COMPLETE_TITLE_1",
				SubTitleLocID = "LOC_ID_INSIGHT_TEXT_CHALLENGE_COMPLETE_SUBTITLE_1",
				DiscoveredTextLocID = "LOC_ID_INSIGHT_TEXT_CHALLENGE_COMPLETE_DISCOVERED_1",
				ResolvedTextLocID = "LOC_ID_INSIGHT_TEXT_CHALLENGE_COMPLETE_RESOLVED_1"
			}
		}
	};

	// Token: 0x04000394 RID: 916
	public static Dictionary<string, InsightUnlockEntry> InsightUnlockTable = new Dictionary<string, InsightUnlockEntry>
	{
		{
			"LOC_ID_JOURNAL_DESCRIPTION_HEIRLOOM_DASH_TALK_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.HeirloomDash,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_HEIRLOOM_MEMORY_TALK_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.HeirloomMemory,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_SPELL_SWORD_LIGHT_TORCHES_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.CastleBoss_DoorOpened,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_SPELLSWORD_INSIGHT_CLUE_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.SpellSwordBossCombatBonus,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_SPELLSWORD_INSIGHT_SECRET_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.SpellSwordBossCombatBonus,
				Discovered = false
			}
		},
		{
			"LOC_ID_HEIRLOOM_TEXT_SPIN_KICK_ENTRANCE_CLUE_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.HeirloomSpinKick_Hidden,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_BRIDGE_CLUE_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.SkeletonBossCombatBonus,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_BRIDGE_SOLUTION_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.SkeletonBossCombatBonus,
				Discovered = false
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_BRIDGE_GATE_LOCK_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.BridgeBoss_GateRaised,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_FOREST_HIDDEN_DOUBLEJUMP_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.HeirloomDoubleJump_Hidden,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_OPENING_DOOR_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.ForestBoss_DoorOpened,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_CONSERVATORY_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.DancingBossCombatBonus,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_REGRET_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.DancingBossCombatBonus,
				Discovered = false
			}
		},
		{
			"LOC_ID_MEMORY_DESCRIPTION_SWORDKNIGHT_BOSS_WARNING_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.HeirloomVoidDash,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_TEXT_STUDY_BOSS_ENTRANCE_CLUE_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.StudyBoss_DoorOpened,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_MEMORY_CLUE_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.StudyBossCombatBonus,
				Discovered = true
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_POISON_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.StudyBossCombatBonus,
				Discovered = false
			}
		},
		{
			"LOC_ID_MEMORY_DESCRIPTION_TOWER_BOSS_CLUE_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.TowerBossCombatBonus,
				Discovered = true
			}
		},
		{
			"LOC_ID_MEMORY_DESCRIPTION_TOWER_BOSS_SOLUTION_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.TowerBossCombatBonus,
				Discovered = false
			}
		},
		{
			"LOC_ID_MYSTERIOUS_KNIGHT_INSIGHT_TO_LANTERN_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.HeirloomLantern,
				Discovered = true
			}
		},
		{
			"LOC_ID_DRAGON_FREEING_THE_DRAGON_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.CaveBoss_DoorOpened,
				Discovered = true
			}
		},
		{
			"LOC_ID_MISC_JOURNAL_CAVE_HIDDEN_LOCATION_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.CaveBossCombatBonus,
				Discovered = true
			}
		},
		{
			"LOC_ID_MISC_JOURNAL_ARMING_THE_REBELS_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.CaveBossCombatBonus,
				Discovered = false
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_SPELLSWORD_DEATH_NGPLUS_3",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.TraitorBoss_HPReduce_SpellSwordBoss,
				Discovered = false
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_BRIDGE_ROOTS_5",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.FinalBoss_HPReduce_SkeletonBoss,
				Discovered = false
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_DANCING_BOSS_RAISING_REBELS_6",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.TraitorBoss_HPReduce_DancingBoss,
				Discovered = false
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_STUDY_BOSS_NEW_DIMENSIONS_5",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.TraitorBoss_HPReduce_StudyBoss,
				Discovered = false
			}
		},
		{
			"LOC_ID_MEMORY_TEXT_PUNISHING_OTHERS_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.FinalBoss_HPReduce_TowerBoss,
				Discovered = false
			}
		},
		{
			"LOC_ID_MEMORY_TITLE_CAVE_BOSS_BETRAYING_ALL_5",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.FinalBoss_HPReduce_CaveBoss,
				Discovered = false
			}
		},
		{
			"LOC_ID_JOURNAL_DESCRIPTION_GOLDENDOOR_MEMORY_TALK_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.FinalDoorEntranceObjective,
				Discovered = true
			}
		},
		{
			"LOC_ID_HEIRLOOM_TEXT_EARTH_SHIFT_INTRO_1",
			new InsightUnlockEntry
			{
				InsightToUnlock = InsightType.HeirloomEarthShift,
				Discovered = true
			}
		}
	};
}

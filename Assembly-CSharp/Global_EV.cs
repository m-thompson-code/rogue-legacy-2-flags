using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
public static class Global_EV
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000172 RID: 370 RVA: 0x0000CF88 File Offset: 0x0000B188
	public static float TerminalVelocity
	{
		get
		{
			if (Global_EV.m_isTerminalVelocityOverrideSet)
			{
				return Global_EV.m_terminalVelocityOverride;
			}
			return -38f;
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000CF9C File Offset: 0x0000B19C
	public static void SetTerminalVelocityOverride(bool isOverrideSet, float value = 0f)
	{
		Global_EV.m_isTerminalVelocityOverrideSet = isOverrideSet;
		Global_EV.m_terminalVelocityOverride = value;
	}

	// Token: 0x040002D0 RID: 720
	public const float FORCE_OF_GRAVITY = -50f;

	// Token: 0x040002D1 RID: 721
	private const float TERMINAL_VELOCITY = -38f;

	// Token: 0x040002D2 RID: 722
	public const float GLOBAL_FALL_MULTIPLIER = 1f;

	// Token: 0x040002D3 RID: 723
	public const float GLOBAL_ASCENT_MULTIPLIER = 1f;

	// Token: 0x040002D4 RID: 724
	public const float DEFAULT_RUMBLE_AMOUNT = 0.5f;

	// Token: 0x040002D5 RID: 725
	public const float REPEAT_HIT_DURATION = 0.5f;

	// Token: 0x040002D6 RID: 726
	public const float PROJECTILE_CAN_HIT_OWNER_DELAY = 0.5f;

	// Token: 0x040002D7 RID: 727
	public const int ENEMY_ROTATION_STEPPING_INCREMENT = 40;

	// Token: 0x040002D8 RID: 728
	public const int ROOM_COUNT_BURDEN_STARTING_LEVEL = 0;

	// Token: 0x040002D9 RID: 729
	public const bool INFINITE_PURCHASING_POWER = false;

	// Token: 0x040002DA RID: 730
	public const FoundState FORCED_EQUIPMENT_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002DB RID: 731
	public const FoundState FORCED_RUNE_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002DC RID: 732
	public const FoundState FORCED_CHALLENGE_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002DD RID: 733
	public const FoundState FORCED_EMPATHY_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002DE RID: 734
	public const FoundState FORCED_BURDEN_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002DF RID: 735
	public const bool UNLOCK_ALL_EQUIPMENT_SETS = false;

	// Token: 0x040002E0 RID: 736
	public const bool UNLOCK_SKILL_TREE = false;

	// Token: 0x040002E1 RID: 737
	public const bool UNLOCK_ALL_SHOPS = false;

	// Token: 0x040002E2 RID: 738
	public const bool UNLOCK_ALL_JOURNAL_ENTRIES = false;

	// Token: 0x040002E3 RID: 739
	public const bool UNLOCK_ALL_GENETICIST_ENTRIES = false;

	// Token: 0x040002E4 RID: 740
	public const bool UNLOCK_ALL_TRAITS_SEEN = false;

	// Token: 0x040002E5 RID: 741
	public const bool UNLOCK_ALL_SONGS = false;

	// Token: 0x040002E6 RID: 742
	public const InsightState FORCED_INSIGHT_STATE = InsightState.Undiscovered;

	// Token: 0x040002E7 RID: 743
	public const bool UNLOCK_TRAIT_GOLD_GAIN_SKILL = false;

	// Token: 0x040002E8 RID: 744
	public const bool UNLOCK_ALL_TROPHIES = false;

	// Token: 0x040002E9 RID: 745
	public const bool ALLOW_INFINITE_CHILD_REROLLS = false;

	// Token: 0x040002EA RID: 746
	public const bool DISABLE_SKILL_TREE_ANIM_LOCKING = false;

	// Token: 0x040002EB RID: 747
	public const bool FORCE_SAVE_FLAGS_TRUE = false;

	// Token: 0x040002EC RID: 748
	public const bool DISABLE_STARTING_SHOP_DIALOGUE = false;

	// Token: 0x040002ED RID: 749
	public const bool RUN_RANDOM_PORTRAITS_TEST = false;

	// Token: 0x040002EE RID: 750
	public const bool DISABLE_SAVING = false;

	// Token: 0x040002EF RID: 751
	public const bool DISABLE_SKILLTREE_LOAD_ON_ENTER_HUBTOWN = false;

	// Token: 0x040002F0 RID: 752
	public const bool HIDE_INPUT_ICONS = false;

	// Token: 0x040002F1 RID: 753
	public const bool GENERATE_RANDOM_CHAR_CLASS = false;

	// Token: 0x040002F2 RID: 754
	public const bool UNLOCK_ALL_CLASSES = false;

	// Token: 0x040002F3 RID: 755
	public const bool LOAD_DEMO = false;

	// Token: 0x040002F4 RID: 756
	public const bool CYCLE_ENEMY_ATTACKS_FOR_AUDIO_TESTING = false;

	// Token: 0x040002F5 RID: 757
	public const bool DISABLE_BOSS_INTROS = false;

	// Token: 0x040002F6 RID: 758
	public const bool FORCE_UPDATE_SAVE_FILES = false;

	// Token: 0x040002F7 RID: 759
	public const bool DISABLE_DEATH_PANNING = false;

	// Token: 0x040002F8 RID: 760
	public const bool DISABLE_TUTORIAL = false;

	// Token: 0x040002F9 RID: 761
	public const bool USE_TRAILER_CONFIG = false;

	// Token: 0x040002FA RID: 762
	public const bool DISABLE_CULLING_GROUPS = false;

	// Token: 0x040002FB RID: 763
	public const bool DISABLE_INTRO_CUTSCENE = false;

	// Token: 0x040002FC RID: 764
	public const bool UNLOCK_ALL_HEIRLOOMS = false;

	// Token: 0x040002FD RID: 765
	public const bool DISABLE_HEIRLOOM_REQUIREMENT_LOCKING = false;

	// Token: 0x040002FE RID: 766
	public const bool DISABLE_SAVEFILE_ROOM_AUTOCORRECTION = false;

	// Token: 0x040002FF RID: 767
	public const bool DISABLE_SKILL_TREE_POPUPS = false;

	// Token: 0x04000300 RID: 768
	public const int MASTERY_STARTING_LEVEL = 0;

	// Token: 0x04000301 RID: 769
	public const bool DISABLE_REVISION_FILE_UPDATING = false;

	// Token: 0x04000302 RID: 770
	public const bool DISABLE_PAUSE_ON_LOSE_FOCUS = false;

	// Token: 0x04000303 RID: 771
	public const bool DISABLE_BOSS_BEATEN_FINAL_DOOR_CUTSCENE = false;

	// Token: 0x04000304 RID: 772
	public const bool UNLOCK_INFINITE_RELIC_REROLLS = false;

	// Token: 0x04000305 RID: 773
	public const bool DISABLE_SKILL_TREE_LEVELS = false;

	// Token: 0x04000306 RID: 774
	public const bool ENABLE_NPC_DIALOGUE_TESTING = false;

	// Token: 0x04000307 RID: 775
	public const bool IGNORE_NPC_DIALOGUE_CONDITIONS = false;

	// Token: 0x04000308 RID: 776
	public const DeathSpecialCutsceneState DEATH_SPECIAL_CUTSCENE_STATE = DeathSpecialCutsceneState.Default;

	// Token: 0x04000309 RID: 777
	public const bool UNLOCK_ALL_NGPLUS_LEVELS = false;

	// Token: 0x0400030A RID: 778
	public static Vector2Int RELIC_ROOM_TEST_RELICS = new Vector2Int(0, 0);

	// Token: 0x0400030B RID: 779
	public const int RELIC_ROOM_TEST_NUM_RELICS_GAINED = -1;

	// Token: 0x0400030C RID: 780
	public const int DEBUG_GIVE_SKILLS_AMOUNT = 0;

	// Token: 0x0400030D RID: 781
	public const bool UNLOCK_ALL_SCAR_MEMORIES = false;

	// Token: 0x0400030E RID: 782
	public const bool UNLOCK_LINEAGE_VARIANTS = false;

	// Token: 0x0400030F RID: 783
	public const bool DISABLE_BIOMERULE_ENEMY_MODS = false;

	// Token: 0x04000310 RID: 784
	public const bool UNLOCK_MAX_EQUIPMENT_LEVELS = false;

	// Token: 0x04000311 RID: 785
	public const int STARTING_NUM_JUMPS = 1;

	// Token: 0x04000312 RID: 786
	public const int STARTING_NUM_DASHES = 0;

	// Token: 0x04000313 RID: 787
	public const HolidayType HOLIDAY_TYPE_TESTER = HolidayType.None;

	// Token: 0x04000314 RID: 788
	public const bool UNLOCK_GOLDEN_DOORS = false;

	// Token: 0x04000315 RID: 789
	public const bool KILL_PLAYER_ON_GAME_COMPLETION = true;

	// Token: 0x04000316 RID: 790
	public const bool FORCE_CONTACT_DAMAGE_TESTING = false;

	// Token: 0x04000317 RID: 791
	private static bool m_isTerminalVelocityOverrideSet = false;

	// Token: 0x04000318 RID: 792
	private static float m_terminalVelocityOverride = 0f;
}

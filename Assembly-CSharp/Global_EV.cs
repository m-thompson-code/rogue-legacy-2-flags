using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
public static class Global_EV
{
	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000186 RID: 390 RVA: 0x0000396C File Offset: 0x00001B6C
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

	// Token: 0x06000187 RID: 391 RVA: 0x00003980 File Offset: 0x00001B80
	public static void SetTerminalVelocityOverride(bool isOverrideSet, float value = 0f)
	{
		Global_EV.m_isTerminalVelocityOverrideSet = isOverrideSet;
		Global_EV.m_terminalVelocityOverride = value;
	}

	// Token: 0x040002F1 RID: 753
	public const float FORCE_OF_GRAVITY = -50f;

	// Token: 0x040002F2 RID: 754
	private const float TERMINAL_VELOCITY = -38f;

	// Token: 0x040002F3 RID: 755
	public const float GLOBAL_FALL_MULTIPLIER = 1f;

	// Token: 0x040002F4 RID: 756
	public const float GLOBAL_ASCENT_MULTIPLIER = 1f;

	// Token: 0x040002F5 RID: 757
	public const float DEFAULT_RUMBLE_AMOUNT = 0.5f;

	// Token: 0x040002F6 RID: 758
	public const float REPEAT_HIT_DURATION = 0.5f;

	// Token: 0x040002F7 RID: 759
	public const float PROJECTILE_CAN_HIT_OWNER_DELAY = 0.5f;

	// Token: 0x040002F8 RID: 760
	public const int ENEMY_ROTATION_STEPPING_INCREMENT = 40;

	// Token: 0x040002F9 RID: 761
	public const int ROOM_COUNT_BURDEN_STARTING_LEVEL = 0;

	// Token: 0x040002FA RID: 762
	public const bool INFINITE_PURCHASING_POWER = false;

	// Token: 0x040002FB RID: 763
	public const FoundState FORCED_EQUIPMENT_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002FC RID: 764
	public const FoundState FORCED_RUNE_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002FD RID: 765
	public const FoundState FORCED_CHALLENGE_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002FE RID: 766
	public const FoundState FORCED_EMPATHY_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x040002FF RID: 767
	public const FoundState FORCED_BURDEN_FOUNDSTATE = FoundState.NotFound;

	// Token: 0x04000300 RID: 768
	public const bool UNLOCK_ALL_EQUIPMENT_SETS = false;

	// Token: 0x04000301 RID: 769
	public const bool UNLOCK_SKILL_TREE = false;

	// Token: 0x04000302 RID: 770
	public const bool UNLOCK_ALL_SHOPS = false;

	// Token: 0x04000303 RID: 771
	public const bool UNLOCK_ALL_JOURNAL_ENTRIES = false;

	// Token: 0x04000304 RID: 772
	public const bool UNLOCK_ALL_GENETICIST_ENTRIES = false;

	// Token: 0x04000305 RID: 773
	public const bool UNLOCK_ALL_TRAITS_SEEN = false;

	// Token: 0x04000306 RID: 774
	public const bool UNLOCK_ALL_SONGS = false;

	// Token: 0x04000307 RID: 775
	public const InsightState FORCED_INSIGHT_STATE = InsightState.Undiscovered;

	// Token: 0x04000308 RID: 776
	public const bool UNLOCK_TRAIT_GOLD_GAIN_SKILL = false;

	// Token: 0x04000309 RID: 777
	public const bool UNLOCK_ALL_TROPHIES = false;

	// Token: 0x0400030A RID: 778
	public const bool ALLOW_INFINITE_CHILD_REROLLS = false;

	// Token: 0x0400030B RID: 779
	public const bool DISABLE_SKILL_TREE_ANIM_LOCKING = false;

	// Token: 0x0400030C RID: 780
	public const bool FORCE_SAVE_FLAGS_TRUE = false;

	// Token: 0x0400030D RID: 781
	public const bool DISABLE_STARTING_SHOP_DIALOGUE = false;

	// Token: 0x0400030E RID: 782
	public const bool RUN_RANDOM_PORTRAITS_TEST = false;

	// Token: 0x0400030F RID: 783
	public const bool DISABLE_SAVING = false;

	// Token: 0x04000310 RID: 784
	public const bool DISABLE_SKILLTREE_LOAD_ON_ENTER_HUBTOWN = false;

	// Token: 0x04000311 RID: 785
	public const bool HIDE_INPUT_ICONS = false;

	// Token: 0x04000312 RID: 786
	public const bool GENERATE_RANDOM_CHAR_CLASS = false;

	// Token: 0x04000313 RID: 787
	public const bool UNLOCK_ALL_CLASSES = false;

	// Token: 0x04000314 RID: 788
	public const bool LOAD_DEMO = false;

	// Token: 0x04000315 RID: 789
	public const bool CYCLE_ENEMY_ATTACKS_FOR_AUDIO_TESTING = false;

	// Token: 0x04000316 RID: 790
	public const bool DISABLE_BOSS_INTROS = false;

	// Token: 0x04000317 RID: 791
	public const bool FORCE_UPDATE_SAVE_FILES = false;

	// Token: 0x04000318 RID: 792
	public const bool DISABLE_DEATH_PANNING = false;

	// Token: 0x04000319 RID: 793
	public const bool DISABLE_TUTORIAL = false;

	// Token: 0x0400031A RID: 794
	public const bool USE_TRAILER_CONFIG = false;

	// Token: 0x0400031B RID: 795
	public const bool DISABLE_CULLING_GROUPS = false;

	// Token: 0x0400031C RID: 796
	public const bool DISABLE_INTRO_CUTSCENE = false;

	// Token: 0x0400031D RID: 797
	public const bool UNLOCK_ALL_HEIRLOOMS = false;

	// Token: 0x0400031E RID: 798
	public const bool DISABLE_HEIRLOOM_REQUIREMENT_LOCKING = false;

	// Token: 0x0400031F RID: 799
	public const bool DISABLE_SAVEFILE_ROOM_AUTOCORRECTION = false;

	// Token: 0x04000320 RID: 800
	public const bool DISABLE_SKILL_TREE_POPUPS = false;

	// Token: 0x04000321 RID: 801
	public const int MASTERY_STARTING_LEVEL = 0;

	// Token: 0x04000322 RID: 802
	public const bool DISABLE_REVISION_FILE_UPDATING = false;

	// Token: 0x04000323 RID: 803
	public const bool DISABLE_PAUSE_ON_LOSE_FOCUS = false;

	// Token: 0x04000324 RID: 804
	public const bool DISABLE_BOSS_BEATEN_FINAL_DOOR_CUTSCENE = false;

	// Token: 0x04000325 RID: 805
	public const bool UNLOCK_INFINITE_RELIC_REROLLS = false;

	// Token: 0x04000326 RID: 806
	public const bool DISABLE_SKILL_TREE_LEVELS = false;

	// Token: 0x04000327 RID: 807
	public const bool ENABLE_NPC_DIALOGUE_TESTING = false;

	// Token: 0x04000328 RID: 808
	public const bool IGNORE_NPC_DIALOGUE_CONDITIONS = false;

	// Token: 0x04000329 RID: 809
	public const DeathSpecialCutsceneState DEATH_SPECIAL_CUTSCENE_STATE = DeathSpecialCutsceneState.Default;

	// Token: 0x0400032A RID: 810
	public const bool UNLOCK_ALL_NGPLUS_LEVELS = false;

	// Token: 0x0400032B RID: 811
	public static Vector2Int RELIC_ROOM_TEST_RELICS = new Vector2Int(0, 0);

	// Token: 0x0400032C RID: 812
	public const int RELIC_ROOM_TEST_NUM_RELICS_GAINED = -1;

	// Token: 0x0400032D RID: 813
	public const int DEBUG_GIVE_SKILLS_AMOUNT = 0;

	// Token: 0x0400032E RID: 814
	public const bool UNLOCK_ALL_SCAR_MEMORIES = false;

	// Token: 0x0400032F RID: 815
	public const bool UNLOCK_LINEAGE_VARIANTS = false;

	// Token: 0x04000330 RID: 816
	public const bool DISABLE_BIOMERULE_ENEMY_MODS = false;

	// Token: 0x04000331 RID: 817
	public const bool UNLOCK_MAX_EQUIPMENT_LEVELS = false;

	// Token: 0x04000332 RID: 818
	public const int STARTING_NUM_JUMPS = 1;

	// Token: 0x04000333 RID: 819
	public const int STARTING_NUM_DASHES = 0;

	// Token: 0x04000334 RID: 820
	public const HolidayType HOLIDAY_TYPE_TESTER = HolidayType.None;

	// Token: 0x04000335 RID: 821
	public const bool UNLOCK_GOLDEN_DOORS = false;

	// Token: 0x04000336 RID: 822
	public const bool KILL_PLAYER_ON_GAME_COMPLETION = true;

	// Token: 0x04000337 RID: 823
	public const bool FORCE_CONTACT_DAMAGE_TESTING = false;

	// Token: 0x04000338 RID: 824
	private static bool m_isTerminalVelocityOverrideSet = false;

	// Token: 0x04000339 RID: 825
	private static float m_terminalVelocityOverride = 0f;
}

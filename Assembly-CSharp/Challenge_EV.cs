using System;
using System.Collections.Generic;

// Token: 0x02000053 RID: 83
public class Challenge_EV
{
	// Token: 0x04000262 RID: 610
	public static readonly Dictionary<string, ChallengeType> ScarUnlockTable = new Dictionary<string, ChallengeType>
	{
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_TwinMech_1",
			ChallengeType.TwinMech
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_PlatformRanger_1",
			ChallengeType.PlatformRanger
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_BrotherAndSister_1",
			ChallengeType.BrotherAndSister
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_SmallChest_1",
			ChallengeType.SmallChest
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_PlatformBoat_1",
			ChallengeType.PlatformBoat
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_PlatformAxe_1",
			ChallengeType.PlatformAxe
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_IntroCombat_1",
			ChallengeType.IntroCombat
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_PlatformKatana_1",
			ChallengeType.PlatformKatana
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_TwoLovers_1",
			ChallengeType.TwoLovers
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_ThreeSkeletons_1",
			ChallengeType.ThreeSkeletons
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_SwordKnightMiniboss_1",
			ChallengeType.SwordKnightMiniboss
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_NightmareKhidr_1",
			ChallengeType.NightmareKhidr
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_PlatformClimb_1",
			ChallengeType.PlatformClimb
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_BigBattle_1",
			ChallengeType.BigBattle
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_SubBossBattle_1",
			ChallengeType.SubBossBattle
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_FourHands_1",
			ChallengeType.FourHands
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_TwoRebels_1",
			ChallengeType.TwoRebels
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_BossRush_1",
			ChallengeType.BossRush
		},
		{
			"LOC_ID_CHALLENGE_MEMORY_TEXT_BossRushRemix_1",
			ChallengeType.BossRushRemix
		}
	};

	// Token: 0x04000263 RID: 611
	public static readonly Dictionary<ChallengeType, PlayerSaveFlag> ScarBossRequirementTable = new Dictionary<ChallengeType, PlayerSaveFlag>
	{
		{
			ChallengeType.Tutorial,
			PlayerSaveFlag.ChallengeDialogue_Intro
		},
		{
			ChallengeType.PlatformAxe,
			PlayerSaveFlag.ChallengeDialogue_Intro
		},
		{
			ChallengeType.IntroCombat,
			PlayerSaveFlag.ChallengeDialogue_Intro
		},
		{
			ChallengeType.TwinMech,
			PlayerSaveFlag.CastleBoss_Defeated_FirstTime
		},
		{
			ChallengeType.PlatformRanger,
			PlayerSaveFlag.BridgeBoss_Defeated_FirstTime
		},
		{
			ChallengeType.BrotherAndSister,
			PlayerSaveFlag.ForestBoss_Defeated_FirstTime
		},
		{
			ChallengeType.SmallChest,
			PlayerSaveFlag.StudyBoss_Defeated_FirstTime
		},
		{
			ChallengeType.FourHands,
			PlayerSaveFlag.CaveBoss_Defeated_FirstTime
		},
		{
			ChallengeType.BossRush,
			PlayerSaveFlag.FinalBoss_Defeated_FirstTime
		},
		{
			ChallengeType.SubBossBattle,
			PlayerSaveFlag.CastleBoss_Prime_Defeated_FirstTime
		},
		{
			ChallengeType.PlatformBoat,
			PlayerSaveFlag.BridgeBoss_Prime_Defeated_FirstTime
		},
		{
			ChallengeType.PlatformKatana,
			PlayerSaveFlag.ForestBoss_Prime_Defeated_FirstTime
		},
		{
			ChallengeType.TwoLovers,
			PlayerSaveFlag.StudyBoss_Prime_Defeated_FirstTime
		},
		{
			ChallengeType.NightmareKhidr,
			PlayerSaveFlag.TowerBoss_Prime_Defeated_FirstTime
		},
		{
			ChallengeType.PlatformClimb,
			PlayerSaveFlag.CaveBoss_Prime_Defeated_FirstTime
		},
		{
			ChallengeType.BigBattle,
			PlayerSaveFlag.CaveBoss_Prime_Defeated_FirstTime
		},
		{
			ChallengeType.TwoRebels,
			PlayerSaveFlag.FinalBoss_Prime_Defeated_FirstTime
		},
		{
			ChallengeType.BossRushRemix,
			PlayerSaveFlag.FinalBoss_Prime_Defeated_FirstTime
		}
	};

	// Token: 0x04000264 RID: 612
	public static readonly RelicType[] RELIC_EXCLUSION_ARRAY = new RelicType[]
	{
		RelicType.GoldCombatChallenge,
		RelicType.ResolveCombatChallenge,
		RelicType.FoodChallenge,
		RelicType.GoldDeathCurse,
		RelicType.ChestHealthRestore,
		RelicType.GoldCombatChallenge,
		RelicType.NoGoldXPBonus,
		RelicType.FreeFairyChest,
		RelicType.ExtraLife,
		RelicType.MeatMaxHealth,
		RelicType.FlightBonusCurse,
		RelicType.RangeDamageBonusCurse,
		RelicType.FreeEnemyKill,
		RelicType.FreeHitRegenerate,
		RelicType.NoSpikeDamage,
		RelicType.SporeburstKillAdd,
		RelicType.EnemiesDropMeat,
		RelicType.MagicDamageEnemyCount,
		RelicType.CritKillsHeal,
		RelicType.SpellKillMaxMana,
		RelicType.LowResolveMagicDamage,
		RelicType.LowResolveWeaponDamage
	};

	// Token: 0x04000265 RID: 613
	public static readonly ChallengeType[] CHALLENGE_ORDER = new ChallengeType[]
	{
		ChallengeType.Tutorial,
		ChallengeType.IntroCombat,
		ChallengeType.PlatformAxe,
		ChallengeType.TwinMech,
		ChallengeType.PlatformRanger,
		ChallengeType.BrotherAndSister,
		ChallengeType.SmallChest,
		ChallengeType.FourHands,
		ChallengeType.SubBossBattle,
		ChallengeType.PlatformBoat,
		ChallengeType.PlatformKatana,
		ChallengeType.TwoLovers,
		ChallengeType.NightmareKhidr,
		ChallengeType.PlatformClimb,
		ChallengeType.BigBattle,
		ChallengeType.TwoRebels
	};

	// Token: 0x04000266 RID: 614
	public const float HANDICAP_DROP_RATE_GOLD_CHESTS = 0f;

	// Token: 0x04000267 RID: 615
	public const float HANDICAP_DROP_RATE_FAIRY_CHESTS = 0f;

	// Token: 0x04000268 RID: 616
	public const int MAX_SCORE = 50000;

	// Token: 0x04000269 RID: 617
	public const int FLAT_VICTORY_POINTS = 2500;

	// Token: 0x0400026A RID: 618
	public const int HIT_MAX_SCORE = 7500;

	// Token: 0x0400026B RID: 619
	public const int HIT_TAKEN_SCORE_PENALTY_ADD = 500;

	// Token: 0x0400026C RID: 620
	public const int RESOLVE_MAX_SCORE = 7500;

	// Token: 0x0400026D RID: 621
	public const int RESOLVE_STARTING_AMOUNT = 250;

	// Token: 0x0400026E RID: 622
	public const int RESOLVE_SCORE_PENALTY = 50;

	// Token: 0x0400026F RID: 623
	public const int TIMER_MAX_SCORE = 7500;

	// Token: 0x04000270 RID: 624
	public const int TIMER_SCORE_PENALTY_ADD = 10;

	// Token: 0x04000271 RID: 625
	public const float TIMER_SCORE_PENALTY_INTERVAL = 0.1f;

	// Token: 0x04000272 RID: 626
	public const float HANDICAP_STARTING_MOD_BONUS = 2f;

	// Token: 0x04000273 RID: 627
	public const float HANDICAP_MOD_SCORE_PENALTY = 0.1f;

	// Token: 0x04000274 RID: 628
	public const int PLATFORM_GOLD_OVER_PAR = 0;

	// Token: 0x04000275 RID: 629
	public const int PLATFORM_SILVER_OVER_PAR = 3;

	// Token: 0x04000276 RID: 630
	public const int PLATFORM_BRONZE_OVER_PAR = 10;

	// Token: 0x04000277 RID: 631
	public const float DEATH_SLOW_TIMESCALE = 0.25f;

	// Token: 0x04000278 RID: 632
	public const float DEATH_SECONDS_BEFORE_WARP = 1.5f;

	// Token: 0x04000279 RID: 633
	public const int RELIC_COST_DOWN_SKILL_LEVEL_OVERRIDE = 5;

	// Token: 0x0400027A RID: 634
	public const int RELIC_REROLL_SKILL_LEVEL_OVERRIDE = 3;

	// Token: 0x0400027B RID: 635
	public const int REROLL_RELIC_ROOM_CAP_SKILL_LEVEL_OVERRIDE = 1;

	// Token: 0x0400027C RID: 636
	public const float END_CHALLENGE_DELAY_TIME = 3f;

	// Token: 0x0400027D RID: 637
	public const float TIMER_HIT_ADD_TIME = 0f;
}

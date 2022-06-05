using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public static class Enemy_EV
{
	// Token: 0x0600017E RID: 382 RVA: 0x000038A6 File Offset: 0x00001AA6
	public static float GetSummonValue(EnemyRank enemyRank)
	{
		switch (enemyRank)
		{
		default:
			return 1f;
		case EnemyRank.Expert:
			return 2f;
		case EnemyRank.Miniboss:
			return 10f;
		}
	}

	// Token: 0x040002C4 RID: 708
	public static Vector2 ENEMY_BASE_KNOCKBACK_DISTANCE = new Vector2(6.75f, 12.25f);

	// Token: 0x040002C5 RID: 709
	public static Vector2 ENEMY_DEATH_KNOCKBACK_DISTANCE = new Vector2(30f, 20f);

	// Token: 0x040002C6 RID: 710
	public const float ENEMY_FLIGHT_KNOCKBACK_DECELERATION = 15f;

	// Token: 0x040002C7 RID: 711
	public const float ENEMY_FLIGHT_IDLE_DECELERATION_MOD = 1.5f;

	// Token: 0x040002C8 RID: 712
	public const float ENEMY_KILLED_SLOWDOWN_AMOUNT = 0.01f;

	// Token: 0x040002C9 RID: 713
	public const float ENEMY_KILLED_SLOWDOWN_DURATION = 0f;

	// Token: 0x040002CA RID: 714
	public const float ENEMY_HIT_SLOWDOWN_AMOUNT = 0.5f;

	// Token: 0x040002CB RID: 715
	public const float ENEMY_HIT_SLOWDOWN_DURATION = 0.065f;

	// Token: 0x040002CC RID: 716
	public const float ENEMY_HIT_SLOWDOWN_CD = 0.55f;

	// Token: 0x040002CD RID: 717
	public const float FORCE_ENEMY_IDLE_DURATION_ON_ROOM_TRANSITION = 0.175f;

	// Token: 0x040002CE RID: 718
	public const float FORCE_ENEMY_IDLE_DURATION_ON_ROOM_TRANSITION_LONG = 0.7f;

	// Token: 0x040002CF RID: 719
	public const float HEALTH_GAIN_PER_LEVEL_MOD = 0.0625f;

	// Token: 0x040002D0 RID: 720
	public const float STRENGTH_GAIN_PER_LEVEL_MOD = 0.0575f;

	// Token: 0x040002D1 RID: 721
	public static float MIN_AGGRO_DURATION = 3.25f;

	// Token: 0x040002D2 RID: 722
	public const float ENEMY_ACTIVATION_WIDTH = 9f;

	// Token: 0x040002D3 RID: 723
	public const float ENEMY_ACTIVATION_HEIGHT = 5.5f;

	// Token: 0x040002D4 RID: 724
	public const float ENEMY_DEACTIVATION_WIDTH = 16f;

	// Token: 0x040002D5 RID: 725
	public const float ENEMY_DEACTIVATION_HEIGHT = 12f;

	// Token: 0x040002D6 RID: 726
	public const float ENEMY_POSITION_RESET_TIMEOUT = 5f;

	// Token: 0x040002D7 RID: 727
	public const float ENEMY_FLAMETHROWER_HITBOX_SPAWN_DELAY = 0.25f;

	// Token: 0x040002D8 RID: 728
	public const float ENEMY_FALSE_LEVEL_SCALING = 2.5f;

	// Token: 0x040002D9 RID: 729
	public const float ENEMY_FAR_RANGE_MOD = 0.5f;

	// Token: 0x040002DA RID: 730
	public const bool USE_RECTS_FOR_LOGIC_RANGES = true;

	// Token: 0x040002DB RID: 731
	public const float ENEMY_RANGE_RECT_RATIO = 0.6f;

	// Token: 0x040002DC RID: 732
	public const bool DISABLE_ITEM_DROP_ON_ENEMY_DEATH = false;

	// Token: 0x040002DD RID: 733
	public const bool DISABLE_ITEM_DROP_ON_SUMMONED_ENEMY_DEATH = true;

	// Token: 0x040002DE RID: 734
	public const bool DISABLE_EXPERT_BOUNCY_SPIKES = true;

	// Token: 0x040002DF RID: 735
	public const bool ENABLE_DASH_KILL_INDICATOR = false;

	// Token: 0x040002E0 RID: 736
	public const float COMMANDER_HP_MOD = 1.75f;

	// Token: 0x040002E1 RID: 737
	public const float COMMANDER_DMG_MOD = 1f;

	// Token: 0x040002E2 RID: 738
	public const float ARMOR_SHRED_CRIT_DAMAGE_MOD = 1.15f;

	// Token: 0x040002E3 RID: 739
	public const float ATTACKING_WITH_CONTACT_DAMAGE_TRIGGER_DELAY = 0.1f;

	// Token: 0x040002E4 RID: 740
	public const bool TEDDY_CUSTOM_CONTACT_DAMAGE_CURATION = true;

	// Token: 0x040002E5 RID: 741
	public const float ENEMY_AGGRESSION_MOD_CLAMPED_MIN = 0.25f;
}

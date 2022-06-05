using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public static class Enemy_EV
{
	// Token: 0x0600016A RID: 362 RVA: 0x0000CE22 File Offset: 0x0000B022
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

	// Token: 0x040002A3 RID: 675
	public static Vector2 ENEMY_BASE_KNOCKBACK_DISTANCE = new Vector2(6.75f, 12.25f);

	// Token: 0x040002A4 RID: 676
	public static Vector2 ENEMY_DEATH_KNOCKBACK_DISTANCE = new Vector2(30f, 20f);

	// Token: 0x040002A5 RID: 677
	public const float ENEMY_FLIGHT_KNOCKBACK_DECELERATION = 15f;

	// Token: 0x040002A6 RID: 678
	public const float ENEMY_FLIGHT_IDLE_DECELERATION_MOD = 1.5f;

	// Token: 0x040002A7 RID: 679
	public const float ENEMY_KILLED_SLOWDOWN_AMOUNT = 0.01f;

	// Token: 0x040002A8 RID: 680
	public const float ENEMY_KILLED_SLOWDOWN_DURATION = 0f;

	// Token: 0x040002A9 RID: 681
	public const float ENEMY_HIT_SLOWDOWN_AMOUNT = 0.5f;

	// Token: 0x040002AA RID: 682
	public const float ENEMY_HIT_SLOWDOWN_DURATION = 0.065f;

	// Token: 0x040002AB RID: 683
	public const float ENEMY_HIT_SLOWDOWN_CD = 0.55f;

	// Token: 0x040002AC RID: 684
	public const float FORCE_ENEMY_IDLE_DURATION_ON_ROOM_TRANSITION = 0.175f;

	// Token: 0x040002AD RID: 685
	public const float FORCE_ENEMY_IDLE_DURATION_ON_ROOM_TRANSITION_LONG = 0.7f;

	// Token: 0x040002AE RID: 686
	public const float HEALTH_GAIN_PER_LEVEL_MOD = 0.0625f;

	// Token: 0x040002AF RID: 687
	public const float STRENGTH_GAIN_PER_LEVEL_MOD = 0.0575f;

	// Token: 0x040002B0 RID: 688
	public static float MIN_AGGRO_DURATION = 3.25f;

	// Token: 0x040002B1 RID: 689
	public const float ENEMY_ACTIVATION_WIDTH = 9f;

	// Token: 0x040002B2 RID: 690
	public const float ENEMY_ACTIVATION_HEIGHT = 5.5f;

	// Token: 0x040002B3 RID: 691
	public const float ENEMY_DEACTIVATION_WIDTH = 16f;

	// Token: 0x040002B4 RID: 692
	public const float ENEMY_DEACTIVATION_HEIGHT = 12f;

	// Token: 0x040002B5 RID: 693
	public const float ENEMY_POSITION_RESET_TIMEOUT = 5f;

	// Token: 0x040002B6 RID: 694
	public const float ENEMY_FLAMETHROWER_HITBOX_SPAWN_DELAY = 0.25f;

	// Token: 0x040002B7 RID: 695
	public const float ENEMY_FALSE_LEVEL_SCALING = 2.5f;

	// Token: 0x040002B8 RID: 696
	public const float ENEMY_FAR_RANGE_MOD = 0.5f;

	// Token: 0x040002B9 RID: 697
	public const bool USE_RECTS_FOR_LOGIC_RANGES = true;

	// Token: 0x040002BA RID: 698
	public const float ENEMY_RANGE_RECT_RATIO = 0.6f;

	// Token: 0x040002BB RID: 699
	public const bool DISABLE_ITEM_DROP_ON_ENEMY_DEATH = false;

	// Token: 0x040002BC RID: 700
	public const bool DISABLE_ITEM_DROP_ON_SUMMONED_ENEMY_DEATH = true;

	// Token: 0x040002BD RID: 701
	public const bool DISABLE_EXPERT_BOUNCY_SPIKES = true;

	// Token: 0x040002BE RID: 702
	public const bool ENABLE_DASH_KILL_INDICATOR = false;

	// Token: 0x040002BF RID: 703
	public const float COMMANDER_HP_MOD = 1.75f;

	// Token: 0x040002C0 RID: 704
	public const float COMMANDER_DMG_MOD = 1f;

	// Token: 0x040002C1 RID: 705
	public const float ARMOR_SHRED_CRIT_DAMAGE_MOD = 1.15f;

	// Token: 0x040002C2 RID: 706
	public const float ATTACKING_WITH_CONTACT_DAMAGE_TRIGGER_DELAY = 0.1f;

	// Token: 0x040002C3 RID: 707
	public const bool TEDDY_CUSTOM_CONTACT_DAMAGE_CURATION = true;

	// Token: 0x040002C4 RID: 708
	public const float ENEMY_AGGRESSION_MOD_CLAMPED_MIN = 0.25f;
}

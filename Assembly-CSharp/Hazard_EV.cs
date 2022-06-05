using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000066 RID: 102
public static class Hazard_EV
{
	// Token: 0x06000175 RID: 373 RVA: 0x0000CFC8 File Offset: 0x0000B1C8
	public static float GetDamageAmount(BaseRoom room)
	{
		float num = 0f;
		if (room)
		{
			float level = (float)room.Level;
			float num2 = 0.007f * (float)SaveManager.PlayerSaveData.NewGamePlusLevel;
			num = (level * 0.0575f + num2 + 1f) * 16f;
			num *= 1f + BurdenManager.GetBurdenStatGain(BurdenType.RoomThreat);
			if (room.BiomeType == BiomeType.Tower || room.BiomeType == BiomeType.TowerExterior)
			{
				num *= 0.75f;
			}
		}
		else if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController();
			num = 16f;
			num *= 1f + BurdenManager.GetBurdenStatGain(BurdenType.RoomThreat);
		}
		return num;
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000176 RID: 374 RVA: 0x0000D069 File Offset: 0x0000B269
	public static Vector2 NIGHTMARE_LANCER_INITIAL_DELAY
	{
		get
		{
			return new Vector2(2f, 4f);
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000177 RID: 375 RVA: 0x0000D07A File Offset: 0x0000B27A
	public static Vector2 NIGHTMARE_LANCER_RANDOM_DELAY
	{
		get
		{
			return new Vector2(3.5f, 6f);
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000178 RID: 376 RVA: 0x0000D08B File Offset: 0x0000B28B
	public static Vector2Int NIGHTMARE_LANCER_RANDOM_AMOUNT
	{
		get
		{
			return new Vector2Int(1, 2);
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000179 RID: 377 RVA: 0x0000D094 File Offset: 0x0000B294
	public static Vector2 NIGHTMARE_LANCER_RANDOM_SPAWN_INTERVAL
	{
		get
		{
			return new Vector2(0.4f, 0.75f);
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x0600017A RID: 378 RVA: 0x0000D0A5 File Offset: 0x0000B2A5
	public static Vector2 NIGHTMARE_LANCER_RANDOM_X_OFFSET
	{
		get
		{
			return new Vector2(8f, 20f);
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x0600017B RID: 379 RVA: 0x0000D0B6 File Offset: 0x0000B2B6
	public static Vector2 NIGHTMARE_LANCER_RANDOM_Y_OFFSET
	{
		get
		{
			return new Vector2(0f, 10f);
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x0600017C RID: 380 RVA: 0x0000D0C7 File Offset: 0x0000B2C7
	public static Vector2 FLYING_WEAPON_WAVE_INITIAL_DELAY
	{
		get
		{
			return new Vector2(5.5f, 7.5f);
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x0600017D RID: 381 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
	public static Vector2 FLYING_WEAPON_WAVE_RANDOM_DELAY
	{
		get
		{
			return new Vector2(11f, 14.5f);
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x0600017E RID: 382 RVA: 0x0000D0E9 File Offset: 0x0000B2E9
	public static Vector2 FLYING_WEAPON_WAVE_RANDOM_POS_OFFSET
	{
		get
		{
			return new Vector2(-3f, 3f);
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600017F RID: 383 RVA: 0x0000D0FA File Offset: 0x0000B2FA
	public static Vector2 FLYING_WEAPON_WAVE_RANDOM_ANGLE
	{
		get
		{
			return new Vector2(-10f, 10f);
		}
	}

	// Token: 0x04000319 RID: 793
	public const bool USE_ROOM_LEVEL_FOR_DAMAGE_CALCULATION = true;

	// Token: 0x0400031A RID: 794
	public const float HAZARD_DAMAGE_FLAT = 16f;

	// Token: 0x0400031B RID: 795
	public const float HAZARD_DAMAGE_ENEMY_MULTIPLIER = 0.2f;

	// Token: 0x0400031C RID: 796
	public const float HAZARD_DAMAGE_TOWER_MOD = 0.75f;

	// Token: 0x0400031D RID: 797
	public const float SPIKE_DAMAGE_MULTIPLIER = 0.2f;

	// Token: 0x0400031E RID: 798
	public const float SPIKETRAP_BASE_DAMAGE = 30f;

	// Token: 0x0400031F RID: 799
	public const float SPIKETRAP_EXTENSION_DELAY = 0.425f;

	// Token: 0x04000320 RID: 800
	public const float SPIKETRAP_RETRACTION_DELAY = 0.45f;

	// Token: 0x04000321 RID: 801
	public const float BREAKABLE_SPIKE_ON_ENTER_REGEN_DELAY = 0.2f;

	// Token: 0x04000322 RID: 802
	public const float BREAKABLE_SPIKE_DAMAGE_DELAY = 1f;

	// Token: 0x04000323 RID: 803
	public const float BREAKABLE_SPIKE_TALL_ON_ENTER_REGEN_DELAY = 0.2f;

	// Token: 0x04000324 RID: 804
	public const float BREAKABLE_TALL_SPIKE_DAMAGE_DELAY = 1.3f;

	// Token: 0x04000325 RID: 805
	public const float BREAKABLE_SPIKE_TALL_ON_HIT_REGEN_DELAY = 6.5f;

	// Token: 0x04000326 RID: 806
	public const float REGULAR_TELEPORTING_HAZARD_DAMAGE_MULTIPLIER = 0.15f;

	// Token: 0x04000327 RID: 807
	public const float HEIRLOOM_TELEPORTING_HAZARD_DAMAGE_MULTIPLIER = 0f;

	// Token: 0x04000328 RID: 808
	public const float BUZZSAW_MOVE_SPEED = 10f;

	// Token: 0x04000329 RID: 809
	public const float BUZZSAW_RETRACT_DURATION = 2f;

	// Token: 0x0400032A RID: 810
	public const float PRESSURE_PLATE_DELAY_BEFORE_FIRING = 0.2f;

	// Token: 0x0400032B RID: 811
	public const float PRESSURE_PLATE_FIRE_DURATION = 0.6f;

	// Token: 0x0400032C RID: 812
	public const float SPRING_POWER = 42f;

	// Token: 0x0400032D RID: 813
	public const float SPRING_RESET_DELAY = 0.25f;

	// Token: 0x0400032E RID: 814
	public const bool SPRING_TRIGGERS_ON_JUMP = false;

	// Token: 0x0400032F RID: 815
	public const float CONVEYOR_SPEED = 10f;

	// Token: 0x04000330 RID: 816
	public const float TURRET_ACTIVATION_DISTANCE = 55f;

	// Token: 0x04000331 RID: 817
	public const float TURRET_BASE_DAMAGE = 25f;

	// Token: 0x04000332 RID: 818
	public const float TURRET_TELL_DELAY = 0.75f;

	// Token: 0x04000333 RID: 819
	public const float TURRET_FORCED_INITIAL_DELAY = 0.875f;

	// Token: 0x04000334 RID: 820
	public const float TURRET_NORMAL_LOOP_DELAY = 2.5f;

	// Token: 0x04000335 RID: 821
	public const float TURRET_NORMAL_PROJ_SPEED_MOD = 10f;

	// Token: 0x04000336 RID: 822
	public const float TURRET_SLOW_LOOP_DELAY = 3.5f;

	// Token: 0x04000337 RID: 823
	public const float TURRET_SLOW_PROJ_SPEED_MOD = 10f;

	// Token: 0x04000338 RID: 824
	public const float TURRET_FAST_LOOP_DELAY = 1.5f;

	// Token: 0x04000339 RID: 825
	public const float TURRET_FAST_PROJ_SPEED_MOD = 10f;

	// Token: 0x0400033A RID: 826
	public const float FLAME_TURRET_ON_DURATION_MOD = 0.5f;

	// Token: 0x0400033B RID: 827
	public const float FLAME_TURRET_OFF_DURATION_MOD = 1.5f;

	// Token: 0x0400033C RID: 828
	public const float RAYCAST_TURRET_INITIALIZATION_DELAY = 1f;

	// Token: 0x0400033D RID: 829
	public const float RAYCAST_TURRET_FIRE_DELAY = 0.5f;

	// Token: 0x0400033E RID: 830
	public const float RAYCAST_TURRET_DELAY_BETWEEN_SHOTS = 4f;

	// Token: 0x0400033F RID: 831
	public const float RAYCAST_TURRET_DETECTED_RANGE = 25f;

	// Token: 0x04000340 RID: 832
	public const float RAYCAST_CURSE_TURRET_INITIALIZATION_DELAY = 1f;

	// Token: 0x04000341 RID: 833
	public const float RAYCAST_CURSE_TURRET_FIRE_DELAY = 0.5f;

	// Token: 0x04000342 RID: 834
	public const float RAYCAST_CURSE_TURRET_DELAY_BETWEEN_SHOTS = 8f;

	// Token: 0x04000343 RID: 835
	public const float RAYCAST_CURSE_TURRET_DETECTED_RANGE = 15f;

	// Token: 0x04000344 RID: 836
	public const float ORBITER_DAMAGE_MULTIPLIER = 0.25f;

	// Token: 0x04000345 RID: 837
	public const float ORBITER_BASE_DAMAGE = 30f;

	// Token: 0x04000346 RID: 838
	public const float ORBITER_BASE_ROTATION_SPEED = 100f;

	// Token: 0x04000347 RID: 839
	public const float ORBITER_EXPANSION_DURATION = 2.5f;

	// Token: 0x04000348 RID: 840
	public const float ORBITER_ENABLE_DAMAGE_DELAY = 0.25f;

	// Token: 0x04000349 RID: 841
	public static Dictionary<BiomeType, float> POINT_HAZARD_RADIUS_MULTIPLIER = new Dictionary<BiomeType, float>();

	// Token: 0x0400034A RID: 842
	public const float WINDMILL_BASE_ROTATION_SPEED = 75f;

	// Token: 0x0400034B RID: 843
	public const float PROXIMITY_MINE_ATTACK_COOLDOWN = 7.5f;

	// Token: 0x0400034C RID: 844
	public const float PROXIMITY_MINE_RADIUS_MOD = 2f;

	// Token: 0x0400034D RID: 845
	public const float PROXIMITY_MINE_ATTACK_TELL_DELAY = 2.25f;

	// Token: 0x0400034E RID: 846
	public const float PROXIMITY_MINE_TRIGGER_RADIUS = 5f;

	// Token: 0x0400034F RID: 847
	public const float PROXIMITY_PROJECTILE_ATTACK_COOLDOWN = 8.5f;

	// Token: 0x04000350 RID: 848
	public const float PROXIMITY_PROJECTILE_ATTACK_TELL_DELAY = 1.25f;

	// Token: 0x04000351 RID: 849
	public const float PROXIMITY_PROJECTILE_TRIGGER_RADIUS = 5f;

	// Token: 0x04000352 RID: 850
	public const float VOID_TRAP_COOLDOWN = 10f;

	// Token: 0x04000353 RID: 851
	public const string VOID_TRAP_PROJECTILE_NAME = "VoidTrapHazardProjectile";

	// Token: 0x04000354 RID: 852
	public static Vector2 VOID_TRAP_CAROUSEL_OFFSET = Vector2.zero;

	// Token: 0x04000355 RID: 853
	public const float VOID_TRAP_RADIUS_MOD = 1.17f;

	// Token: 0x04000356 RID: 854
	public const float VOID_TRAP_TELL_DELAY = 0.25f;

	// Token: 0x04000357 RID: 855
	public const float VOID_TRAP_ATTACK_DURATION = 5f;

	// Token: 0x04000358 RID: 856
	public const float VOID_TRAP_TRIGGER_RADIUS = 3f;

	// Token: 0x04000359 RID: 857
	public const int VOID_TRAP_PROJECTILE_COUNT = 8;

	// Token: 0x0400035A RID: 858
	public const float VOID_TRAP_ROTATION_SPEED = 360f;

	// Token: 0x0400035B RID: 859
	public const float SHRINKONDASH_STATE_ONE_SCALE = 6f;

	// Token: 0x0400035C RID: 860
	public const float SHRINKONDASH_STATE_TWO_SCALE = 6f;

	// Token: 0x0400035D RID: 861
	public const float SHRINKONDASH_SHRINK_SCALE = 0.25f;

	// Token: 0x0400035E RID: 862
	public const float SHRINKONDASH_RESIZE_DELAY = 2f;

	// Token: 0x0400035F RID: 863
	public const float SHRINKONDASH_RESIZE_SPEED = 1.5f;

	// Token: 0x04000360 RID: 864
	public const float LOCAL_TELEPORTER_EXIT_IMMUNITY_WINDOW = 0.2f;

	// Token: 0x04000361 RID: 865
	public const float CLOUD_RESPAWN_DURATION = 0.25f;

	// Token: 0x04000362 RID: 866
	public const float SNOWMOUND_MOVEMENT_MOD = 0.35f;

	// Token: 0x04000363 RID: 867
	public const bool SNOWMOUND_ONLY_SLOWS_ON_GROUND = true;

	// Token: 0x04000364 RID: 868
	public const float SENTRY_ATTACK_COOLDOWN = 0.5f;

	// Token: 0x04000365 RID: 869
	public const string SENTRY_PROJECTILE_NAME = "SentryBounceBoltProjectile";

	// Token: 0x04000366 RID: 870
	public static Vector2 SENTRY_PROJECTILE_OFFSET = new Vector2(0f, 0.5f);

	// Token: 0x04000367 RID: 871
	public const float SENTRY_RADIUS_RANGE_CHECK_MOD = 1.75f;

	// Token: 0x04000368 RID: 872
	public const float SENTRY_ATTACK_TELL_DELAY = 0.25f;

	// Token: 0x04000369 RID: 873
	public const float SENTRY_REST_DURATION = 9f;

	// Token: 0x0400036A RID: 874
	public const float SENTRY_FORCED_INITIAL_DELAY = 0.875f;

	// Token: 0x0400036B RID: 875
	public const float ICE_CRYSTAL_GROWTH_SPEED = 0.3f;

	// Token: 0x0400036C RID: 876
	public const float ICE_CRYSTAL_INITIAL_GROWTH_DELAY = 0.75f;

	// Token: 0x0400036D RID: 877
	public const float ICE_CRYSTAL_SHATTER_GROWTH_DELAY = 2.75f;

	// Token: 0x0400036E RID: 878
	public const float ICE_CRYSTAL_SHATTER_DAMAGE_DELAY = 0.65f;

	// Token: 0x0400036F RID: 879
	public const bool ICE_CRYSTAL_SYNC_DISABLE_GROWTH_WHILE_ASLEEP = true;

	// Token: 0x04000370 RID: 880
	public const bool ICE_CRYSTAL_SYNC_GROW_ON_AWAKEN = true;

	// Token: 0x04000371 RID: 881
	public const float CORPSE_SPAWN_ODDS = 0.375f;

	// Token: 0x04000372 RID: 882
	public const float EXHAUST_POINT_RADIUS_RANGE_CHECK_MOD = 1.75f;

	// Token: 0x04000373 RID: 883
	public const int EXHAUST_POINT_EXHAUST_AMOUNT = 20;

	// Token: 0x04000374 RID: 884
	public const float BRIDGE_BIOME_UP_CANNONBALL_INITIAL_DELAY = 5f;

	// Token: 0x04000375 RID: 885
	public const float BRIDGE_BIOME_UP_CANNONBALL_MIN_DELAY = 8f;

	// Token: 0x04000376 RID: 886
	public const float BRIDGE_BIOME_UP_CANNONBALL_MAX_DELAY = 13f;

	// Token: 0x04000377 RID: 887
	public static Vector2 VOID_WAVE_INITIAL_DELAY = new Vector2(2f, 4f);

	// Token: 0x04000378 RID: 888
	public static Vector2 VOID_WAVE_RANDOM_DELAY = new Vector2(8f, 12.5f);

	// Token: 0x04000379 RID: 889
	public const float VOID_WAVE_WARNING_DURATION = 2.5f;

	// Token: 0x0400037A RID: 890
	public const float NIGHTMARE_LANCER_WARNING_DURATION = 1.75f;

	// Token: 0x0400037B RID: 891
	public const bool TOWER_LANCER_SPAWNS_RELATIVE_TO_SCREEN = false;

	// Token: 0x0400037C RID: 892
	public const float FLYING_WEAPON_WAVE_WARNING_DURATION = 1.5f;

	// Token: 0x0400037D RID: 893
	public const bool FLYING_WEAPON_ENABLE_LOCK_IN_GRACE_DELAY = true;

	// Token: 0x0400037E RID: 894
	public const float FLYING_WEAPON_WAVE_WARNING_LOCKED_DURATION = 0.35f;

	// Token: 0x0400037F RID: 895
	public const float WATER_RISING_DELAY = 3f;

	// Token: 0x04000380 RID: 896
	public const float RISING_WATER_SPEED = 5f;
}

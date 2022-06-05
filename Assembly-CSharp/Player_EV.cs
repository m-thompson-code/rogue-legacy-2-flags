using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class Player_EV
{
	// Token: 0x060001B0 RID: 432 RVA: 0x000108A3 File Offset: 0x0000EAA3
	public static float CalculateCritChance(float dexterity)
	{
		return dexterity / (dexterity + 200f) / 1f;
	}

	// Token: 0x040003DF RID: 991
	public const int PLAYER_NUM_DEATH_ANIMS = 12;

	// Token: 0x040003E0 RID: 992
	public static int PLAYER_DEATH_ANIM_OVERRIDE = -1;

	// Token: 0x040003E1 RID: 993
	public const int PLAYER_HEIRLOOM_DEATH_INDEX = 8;

	// Token: 0x040003E2 RID: 994
	public const int PLAYER_FART_DEATH_INDEX = 9;

	// Token: 0x040003E3 RID: 995
	public static ClassType[] STARTING_UNLOCKED_CLASSES = new ClassType[]
	{
		ClassType.SwordClass
	};

	// Token: 0x040003E4 RID: 996
	public const ClassType PLAYER_STARTING_CLASS = ClassType.SwordClass;

	// Token: 0x040003E5 RID: 997
	public const AbilityType PLAYER_STARTING_SPELL = AbilityType.FireballSpell;

	// Token: 0x040003E6 RID: 998
	public const AbilityType PLAYER_STARTING_TALENT = AbilityType.ShieldBlockTalent;

	// Token: 0x040003E7 RID: 999
	public static float CHARACTER_HIT_SINGLE_BLINK_DURATION = 0.1f;

	// Token: 0x040003E8 RID: 1000
	public const float PLAYER_QUICK_DROP_ANGLE_AMOUNT = 15f;

	// Token: 0x040003E9 RID: 1001
	public const float PLAYER_ORIGINAL_SCALE = 1.4f;

	// Token: 0x040003EA RID: 1002
	public const float PLAYER_MAX_SCALE = 2.1f;

	// Token: 0x040003EB RID: 1003
	public const float PLAYER_MIN_SCALE = 0.77f;

	// Token: 0x040003EC RID: 1004
	public const float PLAYER_ATTACK_FLIP_CHECK_DURATION = 0.285f;

	// Token: 0x040003ED RID: 1005
	public const float PLAYER_ATTACK_FLIP_BOX_SIZE_X_ADD = 1.25f;

	// Token: 0x040003EE RID: 1006
	public const float PLAYER_ATTACK_FlIP_BOX_SIZE_Y_ADD = 1.6f;

	// Token: 0x040003EF RID: 1007
	public const bool PLAYER_ATTACK_FLIP_FORCE_DIRECTION = false;

	// Token: 0x040003F0 RID: 1008
	public const float PLAYER_KNOCKBACK_MOVEMENT_DISABLE_DURATION = 0.25f;

	// Token: 0x040003F1 RID: 1009
	public const int BASE_EQUIPMENT_WEIGHT = 50;

	// Token: 0x040003F2 RID: 1010
	public const int BASE_RUNE_WEIGHT = 50;

	// Token: 0x040003F3 RID: 1011
	public const int PLAYER_BASE_STAT = 15;

	// Token: 0x040003F4 RID: 1012
	public const float PLAYER_BASE_STRENGTH = 15f;

	// Token: 0x040003F5 RID: 1013
	public const float PLAYER_BASE_MAGIC = 15f;

	// Token: 0x040003F6 RID: 1014
	public const int PLAYER_BASE_VITALITY = 15;

	// Token: 0x040003F7 RID: 1015
	public const float PLAYER_BASE_RESOLVE = 1f;

	// Token: 0x040003F8 RID: 1016
	public const int PLAYER_GRACE_BONUS_HP = 100;

	// Token: 0x040003F9 RID: 1017
	public const bool USE_NEW_ARMOR_ALG = true;

	// Token: 0x040003FA RID: 1018
	public const int PLAYER_BASE_ARMOR = 0;

	// Token: 0x040003FB RID: 1019
	public const float PLAYER_ARMOR_REDUCTION_CONST = 0.7f;

	// Token: 0x040003FC RID: 1020
	public const float NEW_ARMOR_MIN_BLOCK_AMOUNT = 0.35f;

	// Token: 0x040003FD RID: 1021
	public const int NEW_ARMOR_ONHIT_DEGRADATION = 0;

	// Token: 0x040003FE RID: 1022
	public const float PLAYER_BASE_DEXTERITY = 5f;

	// Token: 0x040003FF RID: 1023
	public const float PLAYER_BASE_MAGIC_DEXTERITY = 5f;

	// Token: 0x04000400 RID: 1024
	public const float PLAYER_BASE_CRIT_CHANCE = 0f;

	// Token: 0x04000401 RID: 1025
	public const float PLAYER_BASE_CRIT_DAMAGE = 1.1f;

	// Token: 0x04000402 RID: 1026
	public const float PLAYER_SUPERCRIT_CHANCE_MOD = 0.5f;

	// Token: 0x04000403 RID: 1027
	public const float PLAYER_SUPERCRIT_DAMAGE_MOD = 0.35f;

	// Token: 0x04000404 RID: 1028
	public const float PLAYER_GUARANTEED_CRIT_CHANCE_ADD = 100f;

	// Token: 0x04000405 RID: 1029
	public static Color PLAYER_GUARANTEED_CRIT_COLOR = new Color(1f, 0.9f, 0f, 0.5f);

	// Token: 0x04000406 RID: 1030
	public const float PLAYER_BASE_MAGIC_CRIT_CHANCE = 0f;

	// Token: 0x04000407 RID: 1031
	public const float PLAYER_BASE_MAGIC_CRIT_DAMAGE = 1.1f;

	// Token: 0x04000408 RID: 1032
	public const float PLAYER_BASE_DEXTERITY_DIVIDER = 200f;

	// Token: 0x04000409 RID: 1033
	public const float PLAYER_FINAL_DEXTERITY_DIVIDER = 1f;

	// Token: 0x0400040A RID: 1034
	public const float PLAYER_HEALTH_PER_VITALITY = 10f;

	// Token: 0x0400040B RID: 1035
	public const float PLAYER_MAGIC_CRIT_CHANCE_BONUS_DAMAGE = 0.005f;

	// Token: 0x0400040C RID: 1036
	public const float HP_LOST_PER_RESOLVE = 0.01f;

	// Token: 0x0400040D RID: 1037
	public const float RESOLVE_YELLOW_PERCENT = 1f;

	// Token: 0x0400040E RID: 1038
	public const float RESOLVE_RED_PERCENT = 0.5f;

	// Token: 0x0400040F RID: 1039
	public const float STARTING_MANA = 1f;

	// Token: 0x04000410 RID: 1040
	public const int BASE_MANA = 100;

	// Token: 0x04000411 RID: 1041
	public const float BASE_MANA_GAIN_PER_SECOND = 0.75f;

	// Token: 0x04000412 RID: 1042
	public const float REGEN_MANA_WHEN_BELOW_PERCENT = 1f;

	// Token: 0x04000413 RID: 1043
	public const float MANA_REGEN_INITIAL_DELAY = 2f;

	// Token: 0x04000414 RID: 1044
	public const float BASE_MANA_GAIN_PER_ENEMY_HIT = 0.01f;

	// Token: 0x04000415 RID: 1045
	public const float BASE_MANA_GAIN_PER_ENEMY_KILL = 0.05f;

	// Token: 0x04000416 RID: 1046
	public const float BASE_MANA_GAIN_PER_GOLD = 0.1f;

	// Token: 0x04000417 RID: 1047
	public const float BASE_MANA_GAIN_PER_BLOCK = 0.1f;

	// Token: 0x04000418 RID: 1048
	public const float BASE_MANA_GAIN_PER_PLAYER_HIT = 0.1f;

	// Token: 0x04000419 RID: 1049
	public const float PLAYER_HIT_INVINCIBILITY_DURATION = 1f;

	// Token: 0x0400041A RID: 1050
	public const float PLAYER_HIT_STUN_DURATION = 99f;

	// Token: 0x0400041B RID: 1051
	public const float PLAYER_HIT_SLOWDOWN_AMOUNT = 0.1f;

	// Token: 0x0400041C RID: 1052
	public const float PLAYER_HIT_SLOWDOWN_DURATION = 0.5f;

	// Token: 0x0400041D RID: 1053
	public static Vector2 PLAYER_BASE_KNOCKBACK_DISTANCE = new Vector2(10.5f, 13.75f);

	// Token: 0x0400041E RID: 1054
	public const float PLAYER_RICOCHET_LOCKOUT_DURATION = 0.45f;

	// Token: 0x0400041F RID: 1055
	public static Vector2 BASE_RICOCHET_DISTANCE = new Vector2(-10.5f, 13.75f);

	// Token: 0x04000420 RID: 1056
	public const float PLAYER_HIT_AIR_RECOVERY_DELAY = 0.15f;

	// Token: 0x04000421 RID: 1057
	public const float PLAYER_AIR_RECOVERY_SLOWDOWN_AMOUNT = 0.1f;

	// Token: 0x04000422 RID: 1058
	public const float PLAYER_AIR_RECOVERY_SLOWDOWN_DURATION = 0.1f;

	// Token: 0x04000423 RID: 1059
	public const float ACCELERATION_ON_GROUND = 120f;

	// Token: 0x04000424 RID: 1060
	public const float ACCELERATION_IN_AIR = 120f;

	// Token: 0x04000425 RID: 1061
	public const float WALK_SPEED = 12f;

	// Token: 0x04000426 RID: 1062
	public const float STARTING_FLIGHT_DURATION = 5f;

	// Token: 0x04000427 RID: 1063
	public const bool TOUCHING_GROUND_STOPS_FLIGHT = false;

	// Token: 0x04000428 RID: 1064
	public const bool ENABLE_CUSTOM_MAGE_TALENT_POOL = true;

	// Token: 0x04000429 RID: 1065
	public const int STARTING_NUM_JUMPS = 1;

	// Token: 0x0400042A RID: 1066
	public const float JUMP_HEIGHT = 8.75f;

	// Token: 0x0400042B RID: 1067
	public const float DOUBLE_JUMP_HEIGHT = 4.75f;

	// Token: 0x0400042C RID: 1068
	public const float JUMP_TIME_WINDOW = 0.1375f;

	// Token: 0x0400042D RID: 1069
	public const float JUMP_RELEASE_FORCE = 4.5f;

	// Token: 0x0400042E RID: 1070
	public const float JUMP_LEEWAY_AMOUNT = 0.2f;

	// Token: 0x0400042F RID: 1071
	public const float DOUBLE_JUMP_LOCKOUT_DURATION = 0.1f;

	// Token: 0x04000430 RID: 1072
	public const float JUMP_QUEUE_DURATION = 0.2f;

	// Token: 0x04000431 RID: 1073
	public const bool CAN_JUMP_WHILE_DASHING = true;

	// Token: 0x04000432 RID: 1074
	public const bool CAN_JUMP_WHILE_VOID_DASHING = true;

	// Token: 0x04000433 RID: 1075
	public const bool ENABLE_OMNIDASH = false;

	// Token: 0x04000434 RID: 1076
	public const float DASH_SPEED = 26f;

	// Token: 0x04000435 RID: 1077
	public const float DASH_DISTANCE = 8f;

	// Token: 0x04000436 RID: 1078
	public const float DASH_COOLDOWN = 0f;

	// Token: 0x04000437 RID: 1079
	public const float DASH_VOID_DISTANCE_MOD = 2f;

	// Token: 0x04000438 RID: 1080
	public const int STARTING_NUM_DASHES = 0;

	// Token: 0x04000439 RID: 1081
	public const float DASH_DAMAGE_MOD = 1.25f;

	// Token: 0x0400043A RID: 1082
	public const float DASH_KNOCKBACK_MOD = 0.5f;

	// Token: 0x0400043B RID: 1083
	public const bool CAN_DASH_RECOVER = true;

	// Token: 0x0400043C RID: 1084
	public const bool CAN_ALWAYS_DASH_GROUNDED = true;

	// Token: 0x0400043D RID: 1085
	public static Vector2Int DOWN_STRIKE_DOWNKICK_MINMAX_ANGLE = new Vector2Int(-115, -65);

	// Token: 0x0400043E RID: 1086
	public static Vector2Int DOWN_STRIKE_FORWARDKICK_MINMAX_ANGLE = new Vector2Int(-140, -35);

	// Token: 0x0400043F RID: 1087
	public const float DOWN_STRIKE_FORWARDKICK_ANGLE = -55f;

	// Token: 0x04000440 RID: 1088
	public const float DOWN_STRIKE_SPEED = 32f;

	// Token: 0x04000441 RID: 1089
	public const float DOWN_STRIKE_BOUNCE_HEIGHT = 22f;

	// Token: 0x04000442 RID: 1090
	public const bool DOWN_STRIKE_RESET_DOUBLEJUMP = false;

	// Token: 0x04000443 RID: 1091
	public const bool DOWN_STRIKE_RESET_DASH = false;

	// Token: 0x04000444 RID: 1092
	public const float DOWN_STRIKE_DEADZONE = 1f;

	// Token: 0x04000445 RID: 1093
	public const bool DOWN_STRIKE_CAN_DASH = true;

	// Token: 0x04000446 RID: 1094
	public const bool DOWN_STRIKE_DASH_RETAIN_INERTIA = true;

	// Token: 0x04000447 RID: 1095
	public const float SPINKICK_IMMUNITY_GRACE = 0.015f;

	// Token: 0x04000448 RID: 1096
	public const float LOOK_MAX_RANGE = 5.25f;

	// Token: 0x04000449 RID: 1097
	public const float LOOK_SPEED = 0.25f;

	// Token: 0x0400044A RID: 1098
	public const float ABILITY_QUEUE_DURATION = 0.175f;

	// Token: 0x0400044B RID: 1099
	public const float SPINKICK_ATTACK_LOCK_DURATION = 0.175f;

	// Token: 0x0400044C RID: 1100
	public const float DOWN_STRIKE_BOUNCE_INPUTLOCK_DURATION = 0.275f;

	// Token: 0x0400044D RID: 1101
	public const float DUAL_BLADE_CLASS_SUPER_CRIT_ADD = 0.1f;

	// Token: 0x0400044E RID: 1102
	public const float SABER_CLASS_FREE_CRIT_DURATION = 1f;

	// Token: 0x0400044F RID: 1103
	public const float LADLE_CLASS_BURN_DURATION = 3.05f;

	// Token: 0x04000450 RID: 1104
	public const int ASTRO_CLASS_BONUS_MANA_ADD = 1;

	// Token: 0x04000451 RID: 1105
	public const bool MAGE_CLASS_PASSIVE_TRIGGERS_WITH_SPIN_KICK = true;
}

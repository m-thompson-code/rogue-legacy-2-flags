using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class Ability_EV
{
	// Token: 0x040001A8 RID: 424
	public const float ABILITY_CD_REDUC_DELAY = 0.5f;

	// Token: 0x040001A9 RID: 425
	public const float CLOAK_BASE_DURATION = 3f;

	// Token: 0x040001AA RID: 426
	public const float CLOAK_SPEED_MOD = 0.5f;

	// Token: 0x040001AB RID: 427
	public const float CLOAK_DISABLE_INVINCIBILITY_DURATION = 0.1f;

	// Token: 0x040001AC RID: 428
	public static Vector2 CLOAKSTRIKE_INITIAL_VELOCITY = new Vector2(0f, 16.5f);

	// Token: 0x040001AD RID: 429
	public static Vector2 CLOAKSTRIKE_ATTACK_VELOCITY = new Vector2(21f, -31f);

	// Token: 0x040001AE RID: 430
	public static Vector2 CLOAKSTRIKE_RICOCHET_MOD = new Vector2(1f, 1.25f);

	// Token: 0x040001AF RID: 431
	public const float CLOAK_VULNERABLE_DURATON = 2f;

	// Token: 0x040001B0 RID: 432
	public const float ROLL_SPEED = 22f;

	// Token: 0x040001B1 RID: 433
	public const float ROLL_DURATION = 0.3f;

	// Token: 0x040001B2 RID: 434
	public const float ROLL_EXIT_INVINCIBILITY_DURATION = 0.1f;

	// Token: 0x040001B3 RID: 435
	public const float ROLL_DAMAGE_INCREASE_DURATION = 0f;

	// Token: 0x040001B4 RID: 436
	public const float ROLL_DAMAGE_INCREASE_AMOUNT = 0f;

	// Token: 0x040001B5 RID: 437
	public const float ROLL_FREE_CRIT_DURATION = 0.5f;

	// Token: 0x040001B6 RID: 438
	public const PlayerStat LIFESTEAL_RUNE_SET_STAT = PlayerStat.Strength;

	// Token: 0x040001B7 RID: 439
	public const PlayerStat SOULSTEAL_RUNE_SET_STAT = PlayerStat.Magic;

	// Token: 0x040001B8 RID: 440
	public const PlayerStat RETURNDAMAGE_RUNE_SET_STAT = PlayerStat.Vitality;

	// Token: 0x040001B9 RID: 441
	public const float LANCE_ATTACK_FALL_MOD = 0.025f;

	// Token: 0x040001BA RID: 442
	public static Vector2 LANCE_SWING_ATTACK_KNOCKBACK = new Vector2(0f, 8f);

	// Token: 0x040001BB RID: 443
	public const bool LANCE_SWING_ATTACK_APPLY_KNOCKBACK_ON_GROUND = false;

	// Token: 0x040001BC RID: 444
	public const float LANCE_DASH_ATTACK_ADD_CD = 0f;

	// Token: 0x040001BD RID: 445
	public const bool LANCE_CHANGE_DURING_CHARGE = true;

	// Token: 0x040001BE RID: 446
	public const float LANCE_DASH_1_MIN_CHARGE = 0f;

	// Token: 0x040001BF RID: 447
	public const float LANCE_DASH_1_DISTANCE = 0f;

	// Token: 0x040001C0 RID: 448
	public const float LANCE_DASH_1_SPEED = 0f;

	// Token: 0x040001C1 RID: 449
	public const int LANCE_DASH_1_STR_ADD = 0;

	// Token: 0x040001C2 RID: 450
	public const float LANCE_DASH_2_MIN_CHARGE = 0.5f;

	// Token: 0x040001C3 RID: 451
	public const float LANCE_DASH_2_DISTANCE = 30f;

	// Token: 0x040001C4 RID: 452
	public const float LANCE_DASH_2_SPEED = 34f;

	// Token: 0x040001C5 RID: 453
	public const float LANCE_DASH_VERTICAL_MOVEMENT = 18f;

	// Token: 0x040001C6 RID: 454
	public const float SABER_DASH_DISTANCE = 0f;

	// Token: 0x040001C7 RID: 455
	public const float SABER_DASH_SPEED = 0f;

	// Token: 0x040001C8 RID: 456
	public const float SABER_DASH_GRAVITY_DELAY = float.NaN;

	// Token: 0x040001C9 RID: 457
	public const bool BOW_GROUNDED_DISABLE_KNOCKBACK = true;

	// Token: 0x040001CA RID: 458
	public const bool SPEAR_SPIN_RESETS_CD = true;

	// Token: 0x040001CB RID: 459
	public const int SPEAR_SPIN_MANA_REGEN_AMOUNT = 15;

	// Token: 0x040001CC RID: 460
	public const float SPEAR_SPIN_SLOW_DURATION = 0.175f;

	// Token: 0x040001CD RID: 461
	public const float SPEAR_SPIN_SLOW_AMOUNT = 0.05f;

	// Token: 0x040001CE RID: 462
	public const float SPEAR_SPIN_GRACE = 0.05f;

	// Token: 0x040001CF RID: 463
	public const float SHIELD_BLOCK_INVINCIBILITY_DURATION = 1.25f;

	// Token: 0x040001D0 RID: 464
	public const float SHIELD_BLOCK_SLOWDOWN_DURATION = 0.25f;

	// Token: 0x040001D1 RID: 465
	public const float SHIELD_BLOCK_SLOWNDOWN_AMOUNT = 0.1f;

	// Token: 0x040001D2 RID: 466
	public const float SHIELD_BLOCK_PROJECTILE_DELAY = 0.15f;

	// Token: 0x040001D3 RID: 467
	public static Vector2 SHIELD_BLOCK_KNOCKBACK_ADD = new Vector2(11f, 11.15f);

	// Token: 0x040001D4 RID: 468
	public const bool SHIELD_BLOCK_USE_PERCENTAGE_OVER_MAGIC_SCALE = true;

	// Token: 0x040001D5 RID: 469
	public const float SHIELD_BLOCK_PERFECT_TIMING = 0.135f;

	// Token: 0x040001D6 RID: 470
	public const float SHIELD_BLOCK_REGULAR_SCALING = 1.5f;

	// Token: 0x040001D7 RID: 471
	public const float SHIELD_BLOCK_PERFECT_SCALING = 2.5f;

	// Token: 0x040001D8 RID: 472
	public const float SHIELD_BLOCK_REGULAR_DAMAGE_REDUCTION = 0.5f;

	// Token: 0x040001D9 RID: 473
	public const float SHIELD_BLOCK_PERFECT_DAMAGE_REDUCTION = 0f;

	// Token: 0x040001DA RID: 474
	public const float CREATE_PLATFORM_VERTICAL_PUSH = 10f;

	// Token: 0x040001DB RID: 475
	public const int POOL_BALL_NUM_BOUNCES = 3;

	// Token: 0x040001DC RID: 476
	public const float POOL_BALL_DAMAGE_MOD_PER_BOUNCE = 7f;

	// Token: 0x040001DD RID: 477
	public const float POOL_BALL_MAX_BOUNCES__APPLIED_TO_MULTIPLIER = 1f;

	// Token: 0x040001DE RID: 478
	public const float SUPERFART_PUSH_AMOUNT = 25f;

	// Token: 0x040001DF RID: 479
	public const float FLAME_BARRIER_TIC_RATE = 0.5f;

	// Token: 0x040001E0 RID: 480
	public const float TIME_SLOW_AMOUNT = 0.2f;

	// Token: 0x040001E1 RID: 481
	public const float TIME_SLOW_MOVEMENT_INCREASE = 5f;

	// Token: 0x040001E2 RID: 482
	public const float TIME_SLOW_TIC_RATE = 0.5f;

	// Token: 0x040001E3 RID: 483
	public const float COOKING_MAGIC_SCALE_HEAL_AMOUNT = 2f;

	// Token: 0x040001E4 RID: 484
	public const float COOKING_MAGIC_SCALE_MANA_REGEN_AMOUNT = 0f;

	// Token: 0x040001E5 RID: 485
	public const float COOKING_MAGIC_FLAT_MANA_REGEN_AMOUNT = 100f;

	// Token: 0x040001E6 RID: 486
	public const float RELOAD_SPECIAL_MESSAGE_ODDS = 0.025f;

	// Token: 0x040001E7 RID: 487
	public const float DEFAULT_RELOAD_TIME = 0.6f;

	// Token: 0x040001E8 RID: 488
	public const float RELOAD_MIN_RELOAD_TIME = 0.1f;

	// Token: 0x040001E9 RID: 489
	public const float KNOCKOUT_SLOW_AMOUNT = 0.05f;

	// Token: 0x040001EA RID: 490
	public static Vector2 KNOCKOUT_MIN_MAX_ANGLE = new Vector2(10f, 170f);

	// Token: 0x040001EB RID: 491
	public const bool ENABLE_CIRCLE_SHIELD = false;

	// Token: 0x040001EC RID: 492
	public const int KATANA_MAX_ANGLE = 35;

	// Token: 0x040001ED RID: 493
	public const float TELESLICE_TELEPORT_DISTANCE = 12f;

	// Token: 0x040001EE RID: 494
	public const float TELESLICE_EXIT_INVINCIBILITY_DURATION = 0.1f;

	// Token: 0x040001EF RID: 495
	public const bool TELESLICE_RESET_ON_KILL = true;

	// Token: 0x040001F0 RID: 496
	public const int PISTOL_GUARANTEED_CRIT_NUMBER = 10;

	// Token: 0x040001F1 RID: 497
	public const int DRAGON_PISTOL_FIREBALL_NUMBER = 5;

	// Token: 0x040001F2 RID: 498
	public const float LADLE_ABILITY_FREE_CRIT_DURATION = 3f;

	// Token: 0x040001F3 RID: 499
	public const int MAGIC_WAND_MANA_COST_FOR_FREE_CRIT = 50;

	// Token: 0x040001F4 RID: 500
	public const float MAGIC_WAND_FREE_CRIT_DURATION = 3f;

	// Token: 0x040001F5 RID: 501
	public const int LUTE_MAX_PROJECTILES = 3;

	// Token: 0x040001F6 RID: 502
	public const int MANA_BOMB_Y_PUSH_AMOUNT = 16;

	// Token: 0x040001F7 RID: 503
	public const int MANA_BOMB_MANA_REGEN_AMOUNT = 0;

	// Token: 0x040001F8 RID: 504
	public const bool SCYTHE_DASH_ON_ATTACK = true;

	// Token: 0x040001F9 RID: 505
	public const float SCYTHE_DASH_SPEED = 22f;

	// Token: 0x040001FA RID: 506
	public const float KINETIC_BOW_RELOAD_CHARGE = 1f;

	// Token: 0x040001FB RID: 507
	public const bool CANNON_CAN_ONLY_SHOOT_GROUNDED = false;

	// Token: 0x040001FC RID: 508
	public const float CROWS_NEST_MAX_MOVE = 50f;

	// Token: 0x040001FD RID: 509
	public const float CROWS_NEST_MOVE_SPEED = 14f;

	// Token: 0x040001FE RID: 510
	public static Vector2 CROWS_NEST_RANDOM_BULLET_ANGLES = new Vector2(-1f, 4f);

	// Token: 0x040001FF RID: 511
	public const float CROWS_NEST_DESTROY_PROJ_INITIAL_ANGLE = 0f;

	// Token: 0x04000200 RID: 512
	public const float CROWS_NEST_FIRE_REPEAT_DURATION = 0.15f;

	// Token: 0x04000201 RID: 513
	public const float CROWS_NEST_STOP_FIRE_ELAPSED_PERCENT = 0.8f;

	// Token: 0x04000202 RID: 514
	public const bool CROWS_NEST_DESTROY_ON_EXIT_BOAT = false;

	// Token: 0x04000203 RID: 515
	public const bool CROWS_NEST_ALWAYS_FIRE = true;

	// Token: 0x04000204 RID: 516
	public const bool CROWS_NEST_FULL_MOVEMENT = true;

	// Token: 0x04000205 RID: 517
	public const float COMET_BASE_DURATION = 1.5f;

	// Token: 0x04000206 RID: 518
	public const float COMET_SPEED_MOD = 1f;

	// Token: 0x04000207 RID: 519
	public const float COMET_DISABLE_INVINCIBILITY_DURATION = 0.1f;

	// Token: 0x04000208 RID: 520
	public const bool COMET_MAKES_PLAYER_INVINCIBLE = true;

	// Token: 0x04000209 RID: 521
	public const int COMET_MANA_REQUIRED_PER_CHARGE = 1000;

	// Token: 0x0400020A RID: 522
	public const int COMET_NUM_STARTING_CHARGES = 1;
}

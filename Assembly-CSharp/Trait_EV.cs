﻿using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
public static class Trait_EV
{
	// Token: 0x040005AB RID: 1451
	public const float TRAIT_RARITY_ONE_ODDS = 1f;

	// Token: 0x040005AC RID: 1452
	public const float TRAIT_RARITY_TWO_ODDS = 0f;

	// Token: 0x040005AD RID: 1453
	public const float TRAIT_RARITY_THREE_ODDS = 0f;

	// Token: 0x040005AE RID: 1454
	public const float TRAIT_ODDS_OF_GETTING_FIRST_TRAIT = 0.675f;

	// Token: 0x040005AF RID: 1455
	public const float TRAIT_ODDS_OF_GETTING_SECOND_TRAIT = 0.35f;

	// Token: 0x040005B0 RID: 1456
	public static readonly EnemyType[] TRAIT_EFFECT_EXCEPTION_ARRAY = new EnemyType[]
	{
		EnemyType.PaintingEnemy,
		EnemyType.MimicChest,
		EnemyType.MimicChestBoss,
		EnemyType.Dummy,
		EnemyType.Target
	};

	// Token: 0x040005B1 RID: 1457
	public const float ANTIQUE_CHANCE = 0.22f;

	// Token: 0x040005B2 RID: 1458
	public const float MIN_GOLD_GAIN_FOR_SPARKLES = 1f;

	// Token: 0x040005B3 RID: 1459
	public const float PLAYER_GIGANTISM_SCALE = 2.1f;

	// Token: 0x040005B4 RID: 1460
	public const float PLAYER_DWARFISM_SCALE = 0.77f;

	// Token: 0x040005B5 RID: 1461
	public static Vector2 PLAYER_KNOCKED_FAR_KNOCKBACK_MOD = new Vector2(1.4f, 1.3f);

	// Token: 0x040005B6 RID: 1462
	public static Vector2 PLAYER_KNOCKED_LOW_KNOCKBACK_MOD = new Vector2(0.35f, 0.2f);

	// Token: 0x040005B7 RID: 1463
	public static Vector2 ENEMY_KNOCKED_FAR_KNOCKBACK_MOD = new Vector2(1.4f, 1.4f);

	// Token: 0x040005B8 RID: 1464
	public static Vector2 ENEMY_KNOCKED_LOW_KNOCKBACK_MOD = new Vector2(0.5f, 0.5f);

	// Token: 0x040005B9 RID: 1465
	public const float HIGH_JUMP_MAX_JUMP_HEIGHT_MOD = 1.5f;

	// Token: 0x040005BA RID: 1466
	public const float LOWER_STORE_PRICE_DISCOUNT_MOD = 0.85f;

	// Token: 0x040005BB RID: 1467
	public const float COLOR_TRAILS_TAIL_DURATION = 18f;

	// Token: 0x040005BC RID: 1468
	public const float COLOR_TRAILS_SIZE_MOD = 1.5f;

	// Token: 0x040005BD RID: 1469
	public const float SMALL_HITBOX_HEALTH_MOD = 0.75f;

	// Token: 0x040005BE RID: 1470
	public const float SMALL_HITBOX_SIZE_MOD = 0.1f;

	// Token: 0x040005BF RID: 1471
	public const float CANNOT_ATTACK_HEALTH_MOD = 0.4f;

	// Token: 0x040005C0 RID: 1472
	public const float CAN_NOW_ATTACK_HEALTH_MOD = 0.4f;

	// Token: 0x040005C1 RID: 1473
	public const float DAMAGE_BOOST_WEAPON_DAMAGE_MOD = 1.5f;

	// Token: 0x040005C2 RID: 1474
	public const float DAMAGE_BOOST_HEALTH_MOD = 0.75f;

	// Token: 0x040005C3 RID: 1475
	public const float DAMAGE_BOOST_MANA_MOD = 1f;

	// Token: 0x040005C4 RID: 1476
	public const float MAGIC_BOOST_MAGIC_DAMAGE_MOD = 1.5f;

	// Token: 0x040005C5 RID: 1477
	public const float MAGIC_BOOST_HEALTH_MOD = 0.75f;

	// Token: 0x040005C6 RID: 1478
	public const float MAGIC_BOOST_MANA_MOD = 1.5f;

	// Token: 0x040005C7 RID: 1479
	public const float MANA_REGEN_MAGIC_MOD = 1f;

	// Token: 0x040005C8 RID: 1480
	public const float MANA_REGEN_WEAPON_DAMAGE_MOD = 0.5f;

	// Token: 0x040005C9 RID: 1481
	public const float MANA_REGEN_HEALTH_MOD = 0.5f;

	// Token: 0x040005CA RID: 1482
	public const float MANA_REGEN_MANA_MOD = 1f;

	// Token: 0x040005CB RID: 1483
	public const float BONUS_STRENGTH_STRENGTH_MOD = 1.1f;

	// Token: 0x040005CC RID: 1484
	public const float BONUS_HEALTH_HEALTH_MOD = 0.1f;

	// Token: 0x040005CD RID: 1485
	public const float BLUR_ON_HIT_DURATION = 2.5f;

	// Token: 0x040005CE RID: 1486
	public const float BLUR_ON_HIT_RADIUS = 4f;

	// Token: 0x040005CF RID: 1487
	public const float BLURRY_CLOSE_RADIUS = 10f;

	// Token: 0x040005D0 RID: 1488
	public const float BLURRY_FAR_RADIUS = 8f;

	// Token: 0x040005D1 RID: 1489
	public const float HORIZONTAL_DARKNESS_VISION_SCALE = 6f;

	// Token: 0x040005D2 RID: 1490
	public const float DARK_SCREEN_RADIUS = 15.5f;

	// Token: 0x040005D3 RID: 1491
	public const float CHEER_ON_KILLS_SPOTLIGHT_SCALE = 5.5f;

	// Token: 0x040005D4 RID: 1492
	public const bool CHEER_ON_KILLS_SPOTLIGHT_ENEMIES = true;

	// Token: 0x040005D5 RID: 1493
	public static Vector3 FART_POSITION = new Vector3(-1.25f, 1.2f, 0f);

	// Token: 0x040005D6 RID: 1494
	public const float FART_CHANCE = 0.16f;

	// Token: 0x040005D7 RID: 1495
	public const float SUPER_FART_CHANCE = 0.09f;

	// Token: 0x040005D8 RID: 1496
	public static Vector2 SUPER_FART_ACCELERATION = new Vector2(0f, 2f);

	// Token: 0x040005D9 RID: 1497
	public const float SUPER_FART_DURATION = 0.15f;

	// Token: 0x040005DA RID: 1498
	public const float BREAK_PROPS_FOR_MANA_REGEN_AMOUNT = 0.01f;

	// Token: 0x040005DB RID: 1499
	public const float BREAK_PROPS_FOR_MANA_CD_REDUC_AMOUNT = 0f;

	// Token: 0x040005DC RID: 1500
	public static Vector2 FAKE_SELF_DAMAGE_DAMAGE_TAKEN_MOD = new Vector2(1.75f, 6.5f);

	// Token: 0x040005DD RID: 1501
	public static Vector2 FAKE_SELF_DAMAGE_DAMAGE_DEALT_MOD = new Vector2(4f, 15f);

	// Token: 0x040005DE RID: 1502
	public const float FAKE_SELF_DAMAGE_SCREENSHAKE_MOD = 2.5f;

	// Token: 0x040005DF RID: 1503
	public const float MANACOSTANDDAMAGEUP_CD_MOD = 1f;

	// Token: 0x040005E0 RID: 1504
	public const float MANACOSTANDDAMAGEUP_SPELLCOST_MOD = 2f;

	// Token: 0x040005E1 RID: 1505
	public const float MANACOSTANDDAMAGEUP_DMG_MOD = 2f;

	// Token: 0x040005E2 RID: 1506
	public const float LOWER_GRAVITY_GRAVITY_ASCENT_MOD = 1f;

	// Token: 0x040005E3 RID: 1507
	public const float LOWER_GRAVITY_GRAVITY_DESCENT_MOD = 0.35f;

	// Token: 0x040005E4 RID: 1508
	public const float LONGER_CD_MOD = 1.25f;

	// Token: 0x040005E5 RID: 1509
	public const float LONGER_CD_ADD = 5f;

	// Token: 0x040005E6 RID: 1510
	public const float NO_MEAT_DAMAGE_MOD = 1f;

	// Token: 0x040005E7 RID: 1511
	public const byte DISPOSITION_NUM_SIDS = 4;

	// Token: 0x040005E8 RID: 1512
	public const byte DISPOSITION_NUM_GIDS = 3;

	// Token: 0x040005E9 RID: 1513
	public const float VAMPIRISM_DAMAGE_INCREASE_MOD = 2.25f;

	// Token: 0x040005EA RID: 1514
	public const float VAMPIRISM_LIFE_STEAL_MOD = 0.2f;

	// Token: 0x040005EB RID: 1515
	public const float NO_IMMUNITY_WINDOW_ON_HIT_IMMUNITY_DURATION = 0.1f;

	// Token: 0x040005EC RID: 1516
	public const float MEGA_HEALTH_HEALTH_MOD = 2f;

	// Token: 0x040005ED RID: 1517
	public const float MANA_FROM_HURT_MANA_GAIN = 0.5f;

	// Token: 0x040005EE RID: 1518
	public static Vector2 BONUS_CHEST_GOLD_DIE_ROLL = new Vector2(0f, 7f);

	// Token: 0x040005EF RID: 1519
	public static float BONUS_CHEST_GOLD_DIE_MOD = 0.5f;

	// Token: 0x040005F0 RID: 1520
	public static Vector2 BONUS_CHEST_GOLD_GOLD_MOD = new Vector2(0f, 0f);

	// Token: 0x040005F1 RID: 1521
	public const float REVEAL_ALL_CHESTS_HEALTH_MOD = 0.9f;

	// Token: 0x040005F2 RID: 1522
	public const float SUPER_HEALER_HEALTH_MOD = 1f;

	// Token: 0x040005F3 RID: 1523
	public const float SUPER_HEALER_HEALTH_REGEN_DELAY = 2.5f;

	// Token: 0x040005F4 RID: 1524
	public const float SUPER_HEALER_REGEN_PERCENT = 0.05f;

	// Token: 0x040005F5 RID: 1525
	public const float SUPER_HEALER_MAX_HEALTH_PERCENT_MOD = 0.0625f;

	// Token: 0x040005F6 RID: 1526
	public const float SUPER_HEALER_REGEN_TICK = 0.25f;

	// Token: 0x040005F7 RID: 1527
	public const float OMNI_DASH_DISTANCE_MOD = 1f;

	// Token: 0x040005F8 RID: 1528
	public const float OMNI_DASH_HEALTH_MOD = 0.8f;

	// Token: 0x040005F9 RID: 1529
	public const float BOUNCE_TERRAIN_HEALTH_MOD = 0.7f;

	// Token: 0x040005FA RID: 1530
	public const float DISARM_ON_HURT_DURATION = 2f;

	// Token: 0x040005FB RID: 1531
	public const float NOT_MOVING_SLOW_GAME_IDLE_MOD = 0.01f;

	// Token: 0x040005FC RID: 1532
	public const float NO_MANA_CAP_DAMAGE_MOD = 0.02f;

	// Token: 0x040005FD RID: 1533
	public const float NO_MANA_CAP_TICK_DURATION = 1f;

	// Token: 0x040005FE RID: 1534
	public const string EXPLOSIVE_ENEMIES_PROJECTILE_NAME = "ExplosiveEnemiesPotionProjectile";

	// Token: 0x040005FF RID: 1535
	public const string EXPLOSIVE_ENEMIES_EXPLOSION_PROJECTILE_NAME = "ExplosiveEnemiesPotionExplosionProjectile";

	// Token: 0x04000600 RID: 1536
	public static Vector2 EXPLOSIVE_ENEMIES_PROJECTILE_ANGLE = new Vector2(85f, 95f);

	// Token: 0x04000601 RID: 1537
	public const string EXPLOSIVE_CHESTS_PROJECTILE_NAME = "ExplosiveChestsPotionProjectile";

	// Token: 0x04000602 RID: 1538
	public const string EXPLOSIVE_CHESTS_EXPLOSION_PROJECTILE_NAME = "ExplosiveChestsPotionExplosionProjectile";

	// Token: 0x04000603 RID: 1539
	public static Vector2 EXPLOSIVE_CHEST_PROJECTILE_ANGLE = new Vector2(60f, 120f);

	// Token: 0x04000604 RID: 1540
	public const float ANGRY_ON_HIT_DURATION = 7.5f;

	// Token: 0x04000605 RID: 1541
	public const float ANGRY_ON_HIT_DAMAGE_BONUS = 1.25f;

	// Token: 0x04000606 RID: 1542
	public const float ANGRY_ON_HIT_MOVESPEED_BONUS = 1.5f;

	// Token: 0x04000607 RID: 1543
	public const float ANGRY_ON_HIT_RED_TINT_OPACITY = 0.75f;

	// Token: 0x04000608 RID: 1544
	public const float INVULN_DASH_HEALTH_MOD = 0.5f;

	// Token: 0x04000609 RID: 1545
	public static Vector2 GAME_SHAKE_TIME_RANDOMIZER = new Vector2(8f, 18f);

	// Token: 0x0400060A RID: 1546
	public static Vector2 GAME_SHAKE_DURATION = new Vector2(1f, 3f);

	// Token: 0x0400060B RID: 1547
	public static Vector2 GAME_SHAKE_INTENSITY = new Vector2(0.5f, 2f);
}

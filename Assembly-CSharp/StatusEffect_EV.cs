using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class StatusEffect_EV
{
	// Token: 0x040004FA RID: 1274
	public const int MAX_PROJECTILE_EFFECTS = 5;

	// Token: 0x040004FB RID: 1275
	public const float ENEMY_BURN_DAMAGE_TIC_RATE = 0.5f;

	// Token: 0x040004FC RID: 1276
	public const float ENEMY_BURN_DAMAGE_SCALE_BASE = 0.55f;

	// Token: 0x040004FD RID: 1277
	public const float ENEMY_BURN_DURATION_OVERRIDE = 3.05f;

	// Token: 0x040004FE RID: 1278
	public const float ENEMY_BURN_CRIT_DURATION_MOD = 0.35f;

	// Token: 0x040004FF RID: 1279
	public const float BURN_IMMUNITY_DURATION_OVERRIDE = 0f;

	// Token: 0x04000500 RID: 1280
	public const float ENEMY_BLEED_DAMAGE_TIC_RATE = 0.5f;

	// Token: 0x04000501 RID: 1281
	public const float ENEMY_BLEED_DAMAGE_SCALE_BASE = 0.25f;

	// Token: 0x04000502 RID: 1282
	public const float ENEMY_BLEED_DURATION_OVERRIDE = 1.55f;

	// Token: 0x04000503 RID: 1283
	public const float ENEMY_BANE_DAMAGE_TIC_RATE = 1.5f;

	// Token: 0x04000504 RID: 1284
	public const float ENEMY_BANE_DAMAGE_SCALE_BASE = 1f;

	// Token: 0x04000505 RID: 1285
	public const float ENEMY_BANE_DURATION_OVERRIDE = 1.55f;

	// Token: 0x04000506 RID: 1286
	public const float ENEMY_POISON_DAMAGE_TIC_RATE = 0.5f;

	// Token: 0x04000507 RID: 1287
	public const float ENEMY_POISON_DAMAGE_SCALE_BASE = 0.05f;

	// Token: 0x04000508 RID: 1288
	public const float ENEMY_POISON_DURATION_OVERRIDE = 4f;

	// Token: 0x04000509 RID: 1289
	public const float ENEMY_POISON_SKILL_CRIT_TRIGGER_DURATION = 6f;

	// Token: 0x0400050A RID: 1290
	public const int ENEMY_POISON_MAX_STACKS = 10;

	// Token: 0x0400050B RID: 1291
	public const float FREEZE_DURATION_OVERRIDE = 3f;

	// Token: 0x0400050C RID: 1292
	public const bool BREAK_FREEZE_ON_HIT = true;

	// Token: 0x0400050D RID: 1293
	public const float KNOCKBACK_MOD_ON_FREEZE = 0f;

	// Token: 0x0400050E RID: 1294
	public const float FREEZE_UNBREAKABLE_DURATION = 0.25f;

	// Token: 0x0400050F RID: 1295
	public const float FREEZE_BOSS_DURATION_MOD = 0.15f;

	// Token: 0x04000510 RID: 1296
	public const float FREEZE_IMMUNITY_AUTO_APPLY_DURATION = 0f;

	// Token: 0x04000511 RID: 1297
	public const float FREEZE_IMMUNITY_DURATION_OVERRIDE = 0f;

	// Token: 0x04000512 RID: 1298
	public const float GOD_MODE_DMG_DEALT_MOD = 5f;

	// Token: 0x04000513 RID: 1299
	public const float GOD_MODE_SPEED_MOD = 2f;

	// Token: 0x04000514 RID: 1300
	public const float GOD_MODE_DMG_TAKEN_MOD = 0f;

	// Token: 0x04000515 RID: 1301
	public const float GOD_MOD_DURATION_OVERRIDE = 1f;

	// Token: 0x04000516 RID: 1302
	public const float MANA_BURN_DEFAULT_DURATION = 2.55f;

	// Token: 0x04000517 RID: 1303
	public const float MANA_BURN_DEFAULT_TIC_RATE = 0.1f;

	// Token: 0x04000518 RID: 1304
	public const float MANA_BURN_DEFAULT_MANA_GAIN = 1f;

	// Token: 0x04000519 RID: 1305
	public const float MANA_BURN_DEATH_MANA_GAIN = 20f;

	// Token: 0x0400051A RID: 1306
	public const float SPORE_BURST_DEFAULT_DURATION = 1.25f;

	// Token: 0x0400051B RID: 1307
	public const float ARMOR_BREAK_DEFAULT_DURATION = 2.5f;

	// Token: 0x0400051C RID: 1308
	public const float ARMOR_BREAK_DAMAGE_INCREASE = 1.2f;

	// Token: 0x0400051D RID: 1309
	public const float MAGIC_BREAK_DEFAULT_DURATION = 2.5f;

	// Token: 0x0400051E RID: 1310
	public const float MAGIC_BREAK_DAMAGE_INCREASE = 1.2f;

	// Token: 0x0400051F RID: 1311
	public const float SUAVE_DEFAULT_DURATION = 9999999f;

	// Token: 0x04000520 RID: 1312
	public const float SUAVE_INTELLIGENCE_DAMAGE_MOD = 0.15f;

	// Token: 0x04000521 RID: 1313
	public const float PHASED_DEFAULT_DURATION = 3f;

	// Token: 0x04000522 RID: 1314
	public const float DIZZY_DEFAULT_DURATION = 3f;

	// Token: 0x04000523 RID: 1315
	public const float DEATH_DELAY_DEFAULT_DURATION = 3f;

	// Token: 0x04000524 RID: 1316
	public const float COMBO_DELAY_DEFAULT_DURATION = 2f;

	// Token: 0x04000525 RID: 1317
	public const int COMBO_MAX_STACKS = 30;

	// Token: 0x04000526 RID: 1318
	public const float COMBO_STACK_DAMAGE_MOD = 0.02f;

	// Token: 0x04000527 RID: 1319
	public const int COMBO_STACK_GUARANTEED_CRIT_COUNT = 15;

	// Token: 0x04000528 RID: 1320
	public const bool APPLY_PLAYER_COMBO_INSTEAD_OF_ENEMY_COMBO = true;

	// Token: 0x04000529 RID: 1321
	public const int DANCE_MAX_STACKS = 5;

	// Token: 0x0400052A RID: 1322
	public const float DANCE_STACK_DAMAGE_MOD = 0.15f;

	// Token: 0x0400052B RID: 1323
	public const float KNOCKOUT_DEFAULT_DURATION = 0.6f;

	// Token: 0x0400052C RID: 1324
	public const float KNOCKOUT_PUSH_MAGNITUDE = 33f;

	// Token: 0x0400052D RID: 1325
	public const float KNOCKOUT_LEVEL_MAGIC_MOD = 0.3f;

	// Token: 0x0400052E RID: 1326
	public const float KNOCKOUT_DISABLE_ONEWAY_COLLISION_DURATION = 0.25f;

	// Token: 0x0400052F RID: 1327
	public const bool KNOCKOUT_TIMEOUT_EXPLOSION = true;

	// Token: 0x04000530 RID: 1328
	public const string KNOCKOUT_PROJECTILE_NAME = "KnockoutStatusEffectProjectile";

	// Token: 0x04000531 RID: 1329
	public const float VULNERABLE_DEFAULT_DURATION = 2f;

	// Token: 0x04000532 RID: 1330
	public const bool VULNERABLE_CONSUME_ON_HIT = false;

	// Token: 0x04000533 RID: 1331
	public const float FREE_CRIT_DEFAULT_DURATION = 2f;

	// Token: 0x04000534 RID: 1332
	public const float NO_CONTACT_DAMAGE_DEFAULT_DURATION = 9999999f;

	// Token: 0x04000535 RID: 1333
	public const float ENEMY_APPLIESINVULN_RADIUS = 25f;

	// Token: 0x04000536 RID: 1334
	public const float ENEMY_APPLIESINVULN_DEFAULT_DURATION = 3.4028235E+38f;

	// Token: 0x04000537 RID: 1335
	public const string ENEMY_APPLIESINVULN_RADIUS_PROJECTILE_NAME = "StatusEffectTeamInvincibilityProjectile";

	// Token: 0x04000538 RID: 1336
	public const float ENEMY_INVULN_DEFAULT_DURATION = 0.5f;

	// Token: 0x04000539 RID: 1337
	public const float ENEMY_CURSE_PROJECTILE_DEFAULT_DURATION = 3.4028235E+38f;

	// Token: 0x0400053A RID: 1338
	public const int ENEMY_CURSE_PROJECTILE_NUM_PROJECTILES = 2;

	// Token: 0x0400053B RID: 1339
	public const float ENEMY_CURSE_PROJECTILE_COOLDOWN = 2f;

	// Token: 0x0400053C RID: 1340
	public const float ENEMY_CURSE_PROJECTILE_INTERVAL = 3.5f;

	// Token: 0x0400053D RID: 1341
	public const float ENEMY_CURSE_PROJECTILE_DETECTION_DISTANCE = 35f;

	// Token: 0x0400053E RID: 1342
	public const string ENEMY_CURSE_PROJECTILE_NAME = "StatusEffectCurseProjectile";

	// Token: 0x0400053F RID: 1343
	public const string ENEMY_EXPLODE_PROJECTILE_NAME = "StatusEffectExplosionProjectile";

	// Token: 0x04000540 RID: 1344
	public const string ENEMY_EXPLODE_WARNING_PROJECTILE_NAME = "StatusEffectExplosionWarningProjectile";

	// Token: 0x04000541 RID: 1345
	public const float ENEMY_EXPLODE_RADIUS = 11f;

	// Token: 0x04000542 RID: 1346
	public const float ENEMY_EXPLODE_DEFAULT_DURATION = 3.4028235E+38f;

	// Token: 0x04000543 RID: 1347
	public const float ENEMY_EXPLODE_TIME_INTERVAL = 1.25f;

	// Token: 0x04000544 RID: 1348
	public const float ENEMY_EXPLODE_WARNING_DURATION = 1.25f;

	// Token: 0x04000545 RID: 1349
	public const float ENEMY_EXPLODE_COOLDOWN = 4.25f;

	// Token: 0x04000546 RID: 1350
	public const int ENEMY_FREEHIT_COUNT = 4;

	// Token: 0x04000547 RID: 1351
	public const float ENEMY_FREEHIT_RECHARGE_TIMER = 3.5f;

	// Token: 0x04000548 RID: 1352
	public const float ENEMY_SIZE_MOD = 1.25f;

	// Token: 0x04000549 RID: 1353
	public const float ENEMY_SIZE_HP_MOD = 2f;

	// Token: 0x0400054A RID: 1354
	public const float ENEMY_SIZE_DAMAGE_MOD = 1.5f;

	// Token: 0x0400054B RID: 1355
	public const float ENEMY_SIZE_DEFAULT_DURATION = 3.4028235E+38f;

	// Token: 0x0400054C RID: 1356
	public const float ENEMY_ARMORSHRED_MAX_ARMOR_MOD = 0.1f;

	// Token: 0x0400054D RID: 1357
	public const float ENEMY_ARMORSHRED_DEFAULT_DURATION = 3.4028235E+38f;

	// Token: 0x0400054E RID: 1358
	public const float PLAYER_EXHAUST_MAX_HP_PER_STACK = 0.01f;

	// Token: 0x0400054F RID: 1359
	public const float PLAYER_EXHAUST_DEFAULT_DURATION = 2.5f;

	// Token: 0x04000550 RID: 1360
	public const float ENEMY_INVULNWINDOW_INVULN_DURATION = 5f;

	// Token: 0x04000551 RID: 1361
	public const float ENEMY_INVULNWINDOW_HEALTH_TRIGGER_AMOUNT = 0.01f;

	// Token: 0x04000552 RID: 1362
	public const float ENEMY_INVULNWINDOW_ATTACKS_PER_SECOND = 1.25f;

	// Token: 0x04000553 RID: 1363
	public const float ENEMY_INVULNWINDOW_ATTACK_INITIAL_DELAY = 1.15f;

	// Token: 0x04000554 RID: 1364
	public static Vector2 ENEMY_INVULNWINDOW_PROJECTILE_OFFSET = new Vector2(-35f, 35f);

	// Token: 0x04000555 RID: 1365
	public const string ENEMY_INVULNWINDOW_PROJECTILE_NAME = "StatusEffectInvulnWindowProjectile";

	// Token: 0x04000556 RID: 1366
	public const int ENEMY_AGGRO_MAX_STACKS = 3;

	// Token: 0x04000557 RID: 1367
	public const float ENEMY_AGGRO_DAMAGE_DEALT_MOD = 0.08f;

	// Token: 0x04000558 RID: 1368
	public const float ENEMY_AGGRO_DAMAGE_TAKEN_MOD = 0.15f;

	// Token: 0x04000559 RID: 1369
	public const float ENEMY_AGGRO_MOVE_SPEED_MOD = 0.12f;

	// Token: 0x0400055A RID: 1370
	public const float ENEMY_AGGRO_STACK_DROP_DELAY = 2.75f;

	// Token: 0x0400055B RID: 1371
	public const float ENEMY_FLAMER_DELAY_BETWEEN_WAVES = 2.25f;

	// Token: 0x0400055C RID: 1372
	public const int ENEMY_FLAMER_PROJECTILES_PER_WAVE = 2;

	// Token: 0x0400055D RID: 1373
	public const float ENEMY_FLAMER_DELAY_BETWEEN_PROJECTILES = 0.25f;

	// Token: 0x0400055E RID: 1374
	public static Vector2 ENEMY_FLAME_PROJECTILE_OFFSET = new Vector2(-5f, 5f);

	// Token: 0x0400055F RID: 1375
	public const string ENEMY_FLAMER_PROJECTILE_NAME = "StatusEffectFireballProjectile";

	// Token: 0x04000560 RID: 1376
	public const float ENEMY_INVULNTIMER_DEFAULT_DURATION = 5f;

	// Token: 0x04000561 RID: 1377
	public const float ENEMY_SPEED_MOD = 1.25f;

	// Token: 0x04000562 RID: 1378
	public const float ENEMY_SPEED_DEFAULT_DURATION = 3.4028235E+38f;

	// Token: 0x04000563 RID: 1379
	public const float ENEMY_DISARM_DEFAULT_DURATION = 3.4028235E+38f;

	// Token: 0x04000564 RID: 1380
	public const float PLAYER_DISARMED_DEFAULT_DURATION = 3f;

	// Token: 0x04000565 RID: 1381
	public static StatusEffectType[] COMMANDER_STATUS_EFFECT_ARRAY = new StatusEffectType[]
	{
		StatusEffectType.Enemy_Size,
		StatusEffectType.Enemy_AppliesInvuln,
		StatusEffectType.Enemy_FreeHit,
		StatusEffectType.Enemy_Curse_Projectile,
		StatusEffectType.Enemy_Explode,
		StatusEffectType.Enemy_ArmorShred,
		StatusEffectType.Enemy_InvulnWindow,
		StatusEffectType.Enemy_Aggro,
		StatusEffectType.Enemy_Flamer
	};

	// Token: 0x04000566 RID: 1382
	public const string COMMANDER_LOC_ID = "LOC_ID_INDEX_COMMANDER_BUFFS_COMMANDER_TITLE_1";

	// Token: 0x04000567 RID: 1383
	public static Dictionary<StatusEffectType, string> STATUS_EFFECT_TITLE_LOC_IDS = new Dictionary<StatusEffectType, string>
	{
		{
			StatusEffectType.Player_Suave,
			"LOC_ID_INDEX_STATUS_EFFECTS_SUAVE_TITLE_1"
		},
		{
			StatusEffectType.Player_NoContactDamage,
			"LOC_ID_INDEX_STATUS_EFFECTS_NO_CONTACT_DAMAGE_TITLE_1"
		},
		{
			StatusEffectType.Player_Cloak,
			"LOC_ID_INDEX_STATUS_EFFECTS_CLOAK_TITLE_1"
		},
		{
			StatusEffectType.Player_Combo,
			"LOC_ID_INDEX_STATUS_EFFECTS_COMBO_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Knockout,
			"LOC_ID_INDEX_STATUS_EFFECTS_KNOCK_OUT_TITLE_1"
		},
		{
			StatusEffectType.Player_Dance,
			"LOC_ID_INDEX_STATUS_EFFECTS_DANCE_TITLE_1"
		},
		{
			StatusEffectType.Player_Exhaust,
			"LOC_ID_INDEX_STATUS_EFFECTS_EXHAUST_TITLE_1"
		},
		{
			StatusEffectType.Player_FreeCrit,
			"LOC_ID_INDEX_STATUS_EFFECTS_FREE_CRIT_TITLE_1"
		},
		{
			StatusEffectType.Player_Disarmed,
			"LOC_ID_INDEX_STATUS_EFFECTS_DISARM_TITLE_1"
		},
		{
			StatusEffectType.Enemy_ArmorBreak,
			"LOC_ID_INDEX_STATUS_EFFECTS_ARMOR_BREAK_TITLE_1"
		},
		{
			StatusEffectType.Enemy_MagicBreak,
			"LOC_ID_INDEX_STATUS_EFFECTS_MAGIC_BREAK_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Freeze,
			"LOC_ID_INDEX_STATUS_EFFECTS_FROZEN_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Burn,
			"LOC_ID_INDEX_STATUS_EFFECTS_BURN_TITLE_1"
		},
		{
			StatusEffectType.Enemy_SporeBurst,
			"LOC_ID_INDEX_STATUS_EFFECTS_SPORE_BURST_TITLE_1"
		},
		{
			StatusEffectType.Enemy_ManaBurn,
			"LOC_ID_INDEX_STATUS_EFFECTS_MANA_LEECH_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Poison,
			"LOC_ID_INDEX_STATUS_EFFECTS_POISON_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Vulnerable,
			"LOC_ID_INDEX_STATUS_EFFECTS_VULNERABLE_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Size,
			"LOC_ID_INDEX_COMMANDER_BUFFS_SIZE_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Speed,
			"LOC_ID_INDEX_COMMANDER_BUFFS_SPEED_TITLE_1"
		},
		{
			StatusEffectType.Enemy_AppliesInvuln,
			"LOC_ID_INDEX_COMMANDER_BUFFS_APPLIES_INVULN_TITLE_1"
		},
		{
			StatusEffectType.Enemy_FreeHit,
			"LOC_ID_INDEX_COMMANDER_BUFFS_FREE_HIT_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Disarm,
			"LOC_ID_INDEX_COMMANDER_BUFFS_DISARM_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Curse_Projectile,
			"LOC_ID_INDEX_COMMANDER_BUFFS_CURSED_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Explode,
			"LOC_ID_INDEX_COMMANDER_BUFFS_EXPLOSIVE_TITLE_1"
		},
		{
			StatusEffectType.Enemy_ArmorShred,
			"LOC_ID_INDEX_COMMANDER_BUFFS_ARMOR_SHRED_TITLE_1"
		},
		{
			StatusEffectType.Enemy_InvulnWindow,
			"LOC_ID_INDEX_COMMANDER_BUFFS_INVULNWINDOW_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Aggro,
			"LOC_ID_INDEX_COMMANDER_BUFFS_AGGRO_TITLE_1"
		},
		{
			StatusEffectType.Enemy_Flamer,
			"LOC_ID_INDEX_COMMANDER_BUFFS_FLAMER_TITLE_1"
		}
	};

	// Token: 0x04000568 RID: 1384
	public static Dictionary<StatusEffectType, string> STATUS_EFFECT_DESC_LOC_IDS = new Dictionary<StatusEffectType, string>
	{
		{
			StatusEffectType.Player_Suave,
			"LOC_ID_INDEX_STATUS_EFFECTS_SUAVE_DESCRIPTION_1"
		},
		{
			StatusEffectType.Player_NoContactDamage,
			"LOC_ID_INDEX_STATUS_EFFECTS_NO_CONTACT_DAMAGE_DESCRIPTION_1"
		},
		{
			StatusEffectType.Player_Cloak,
			"LOC_ID_INDEX_STATUS_EFFECTS_CLOAK_DESCRIPTION_1"
		},
		{
			StatusEffectType.Player_Combo,
			"LOC_ID_INDEX_STATUS_EFFECTS_COMBO_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Knockout,
			"LOC_ID_INDEX_STATUS_EFFECTS_KNOCK_OUT_DESCRIPTION_1"
		},
		{
			StatusEffectType.Player_Dance,
			"LOC_ID_INDEX_STATUS_EFFECTS_DANCE_DESCRIPTION_1"
		},
		{
			StatusEffectType.Player_Exhaust,
			"LOC_ID_INDEX_STATUS_EFFECTS_EXHAUST_DESCRIPTION_1"
		},
		{
			StatusEffectType.Player_FreeCrit,
			"LOC_ID_INDEX_STATUS_EFFECTS_FREE_CRIT_DESCRIPTION_1"
		},
		{
			StatusEffectType.Player_Disarmed,
			"LOC_ID_INDEX_STATUS_EFFECTS_DISARM_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_ArmorBreak,
			"LOC_ID_INDEX_STATUS_EFFECTS_ARMOR_BREAK_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_MagicBreak,
			"LOC_ID_INDEX_STATUS_EFFECTS_MAGIC_BREAK_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Freeze,
			"LOC_ID_INDEX_STATUS_EFFECTS_FROZEN_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Burn,
			"LOC_ID_INDEX_STATUS_EFFECTS_BURN_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_SporeBurst,
			"LOC_ID_INDEX_STATUS_EFFECTS_SPORE_BURST_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_ManaBurn,
			"LOC_ID_INDEX_STATUS_EFFECTS_MANA_LEECH_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Poison,
			"LOC_ID_INDEX_STATUS_EFFECTS_POISON_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Vulnerable,
			"LOC_ID_INDEX_STATUS_EFFECTS_VULNERABLE_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Size,
			"LOC_ID_INDEX_COMMANDER_BUFFS_SIZE_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Speed,
			"LOC_ID_INDEX_COMMANDER_BUFFS_SPEED_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_AppliesInvuln,
			"LOC_ID_INDEX_COMMANDER_BUFFS_APPLIES_INVULN_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_FreeHit,
			"LOC_ID_INDEX_COMMANDER_BUFFS_FREE_HIT_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Disarm,
			"LOC_ID_INDEX_COMMANDER_BUFFS_DISARM_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Curse_Projectile,
			"LOC_ID_INDEX_COMMANDER_BUFFS_CURSED_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Explode,
			"LOC_ID_INDEX_COMMANDER_BUFFS_EXPLOSIVE_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_ArmorShred,
			"LOC_ID_INDEX_COMMANDER_BUFFS_ARMOR_SHRED_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_InvulnWindow,
			"LOC_ID_INDEX_COMMANDER_BUFFS_INVULNWINDOW_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Aggro,
			"LOC_ID_INDEX_COMMANDER_BUFFS_AGGRO_DESCRIPTION_1"
		},
		{
			StatusEffectType.Enemy_Flamer,
			"LOC_ID_INDEX_COMMANDER_BUFFS_FLAMER_DESCRIPTION_1"
		}
	};
}

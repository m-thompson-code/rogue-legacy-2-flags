using System;

// Token: 0x02000C48 RID: 3144
public enum TraitType
{
	// Token: 0x04004B33 RID: 19251
	None,
	// Token: 0x04004B34 RID: 19252
	NoColor = 10,
	// Token: 0x04004B35 RID: 19253
	Bald = 20,
	// Token: 0x04004B36 RID: 19254
	NoHealthBar = 30,
	// Token: 0x04004B37 RID: 19255
	EasyBreakables = 40,
	// Token: 0x04004B38 RID: 19256
	Swearing = 50,
	// Token: 0x04004B39 RID: 19257
	FakeEnemies = 60,
	// Token: 0x04004B3A RID: 19258
	YouAreSmall = 70,
	// Token: 0x04004B3B RID: 19259
	ShowEnemiesOnMap = 80,
	// Token: 0x04004B3C RID: 19260
	PlayerKnockedLow = 90,
	// Token: 0x04004B3D RID: 19261
	PlayerKnockedFar = 95,
	// Token: 0x04004B3E RID: 19262
	Disposition = 100,
	// Token: 0x04004B3F RID: 19263
	YouAreLarge = 110,
	// Token: 0x04004B40 RID: 19264
	DarkScreen = 120,
	// Token: 0x04004B41 RID: 19265
	Fart = 130,
	// Token: 0x04004B42 RID: 19266
	EnemyKnockedLow = 140,
	// Token: 0x04004B43 RID: 19267
	BreakPropsForMana = 150,
	// Token: 0x04004B44 RID: 19268
	UpsideDown = 160,
	// Token: 0x04004B45 RID: 19269
	Oversaturate = 170,
	// Token: 0x04004B46 RID: 19270
	CuteGame = 180,
	// Token: 0x04004B47 RID: 19271
	OneHitDeath = 190,
	// Token: 0x04004B48 RID: 19272
	AngryOnHit = 200,
	// Token: 0x04004B49 RID: 19273
	CantAttack = 210,
	// Token: 0x04004B4A RID: 19274
	CanNowAttack,
	// Token: 0x04004B4B RID: 19275
	OmniDash = 220,
	// Token: 0x04004B4C RID: 19276
	HighJump = 230,
	// Token: 0x04004B4D RID: 19277
	InvulnDash = 240,
	// Token: 0x04004B4E RID: 19278
	BoosWhenHit = 250,
	// Token: 0x04004B4F RID: 19279
	GainDownStrike = 260,
	// Token: 0x04004B50 RID: 19280
	DamageBoost = 270,
	// Token: 0x04004B51 RID: 19281
	MagicBoost = 280,
	// Token: 0x04004B52 RID: 19282
	EnemyKnockedFar = 290,
	// Token: 0x04004B53 RID: 19283
	Dog = 300,
	// Token: 0x04004B54 RID: 19284
	LongerCD = 310,
	// Token: 0x04004B55 RID: 19285
	BonusHealth = 320,
	// Token: 0x04004B56 RID: 19286
	BonusStrength = 330,
	// Token: 0x04004B57 RID: 19287
	BonusMagicStrength = 340,
	// Token: 0x04004B58 RID: 19288
	FMFFan,
	// Token: 0x04004B59 RID: 19289
	NoEnemyHealthBar,
	// Token: 0x04004B5A RID: 19290
	ManaCostAndDamageUp,
	// Token: 0x04004B5B RID: 19291
	FreeLife,
	// Token: 0x04004B5C RID: 19292
	RevealAllChests,
	// Token: 0x04004B5D RID: 19293
	BounceTerrain = 350,
	// Token: 0x04004B5E RID: 19294
	LowerStorePrice = 360,
	// Token: 0x04004B5F RID: 19295
	GameRunsFaster = 370,
	// Token: 0x04004B60 RID: 19296
	ChickensAreEnemies = 380,
	// Token: 0x04004B61 RID: 19297
	NoMap = 390,
	// Token: 0x04004B62 RID: 19298
	FakeSelfDamage = 410,
	// Token: 0x04004B63 RID: 19299
	BlurryFar = 420,
	// Token: 0x04004B64 RID: 19300
	OldYellowTint = 430,
	// Token: 0x04004B65 RID: 19301
	RandomizeSpells = 440,
	// Token: 0x04004B66 RID: 19302
	NoProjectileIndicators = 450,
	// Token: 0x04004B67 RID: 19303
	HorizontalDarkness = 452,
	// Token: 0x04004B68 RID: 19304
	EnemiesCensored = 460,
	// Token: 0x04004B69 RID: 19305
	EnemiesBlackFill = 462,
	// Token: 0x04004B6A RID: 19306
	LowerGravity = 470,
	// Token: 0x04004B6B RID: 19307
	MagnetRangeBoost = 480,
	// Token: 0x04004B6C RID: 19308
	CarryFood = 490,
	// Token: 0x04004B6D RID: 19309
	RandomSounds = 500,
	// Token: 0x04004B6E RID: 19310
	BlurOnHit = 510,
	// Token: 0x04004B6F RID: 19311
	ColorTrails = 520,
	// Token: 0x04004B70 RID: 19312
	WeaponSpellSwitch = 530,
	// Token: 0x04004B71 RID: 19313
	FindBoss = 540,
	// Token: 0x04004B72 RID: 19314
	NoMeat = 550,
	// Token: 0x04004B73 RID: 19315
	SmallHitbox = 570,
	// Token: 0x04004B74 RID: 19316
	CheerOnKills = 580,
	// Token: 0x04004B75 RID: 19317
	Retro = 590,
	// Token: 0x04004B76 RID: 19318
	CameraZoomOut = 600,
	// Token: 0x04004B77 RID: 19319
	CameraZoomIn = 610,
	// Token: 0x04004B78 RID: 19320
	FreeRelic,
	// Token: 0x04004B79 RID: 19321
	DisableSpikeTraps = 620,
	// Token: 0x04004B7A RID: 19322
	BackwardSpell = 630,
	// Token: 0x04004B7B RID: 19323
	GameShake = 640,
	// Token: 0x04004B7C RID: 19324
	WordScramble = 650,
	// Token: 0x04004B7D RID: 19325
	GreenPlatformOpen = 660,
	// Token: 0x04004B7E RID: 19326
	BlurryClose = 670,
	// Token: 0x04004B7F RID: 19327
	DisableAttackLock = 680,
	// Token: 0x04004B80 RID: 19328
	CantSeeChildren = 690,
	// Token: 0x04004B81 RID: 19329
	Unused_1 = 720,
	// Token: 0x04004B82 RID: 19330
	LifeTimer = 730,
	// Token: 0x04004B83 RID: 19331
	SuperFart = 740,
	// Token: 0x04004B84 RID: 19332
	HomingBullets = 750,
	// Token: 0x04004B85 RID: 19333
	TeleportDamage = 760,
	// Token: 0x04004B86 RID: 19334
	FriendsLeave = 770,
	// Token: 0x04004B87 RID: 19335
	Vampire = 780,
	// Token: 0x04004B88 RID: 19336
	RandomizeWeapons = 790,
	// Token: 0x04004B89 RID: 19337
	ChanceHazardMissing = 800,
	// Token: 0x04004B8A RID: 19338
	NotMovingSlowGame = 810,
	// Token: 0x04004B8B RID: 19339
	MapReveal = 820,
	// Token: 0x04004B8C RID: 19340
	NoImmunityWindow = 830,
	// Token: 0x04004B8D RID: 19341
	NoManaCap = 840,
	// Token: 0x04004B8E RID: 19342
	MegaHealth = 850,
	// Token: 0x04004B8F RID: 19343
	ItemsGoFlying = 860,
	// Token: 0x04004B90 RID: 19344
	ManaFromHurt = 870,
	// Token: 0x04004B91 RID: 19345
	BonusChestGold = 880,
	// Token: 0x04004B92 RID: 19346
	SuperHealer = 890,
	// Token: 0x04004B93 RID: 19347
	TwinRelics = 900,
	// Token: 0x04004B94 RID: 19348
	MushroomGrow = 910,
	// Token: 0x04004B95 RID: 19349
	YouAreBlue = 920,
	// Token: 0x04004B96 RID: 19350
	SkillCritsOnly = 930,
	// Token: 0x04004B97 RID: 19351
	DisarmOnHurt = 940,
	// Token: 0x04004B98 RID: 19352
	ExplosiveChests = 950,
	// Token: 0x04004B99 RID: 19353
	ExplosiveEnemies = 960,
	// Token: 0x04004B9A RID: 19354
	HalloweenHoliday = 970,
	// Token: 0x04004B9B RID: 19355
	ChristmasHoliday,
	// Token: 0x04004B9C RID: 19356
	ToneDeaf = 980,
	// Token: 0x04004B9D RID: 19357
	RandomizeKit = 990,
	// Token: 0x04004B9E RID: 19358
	Antique_Old = 861,
	// Token: 0x04004B9F RID: 19359
	Antique = 10000
}

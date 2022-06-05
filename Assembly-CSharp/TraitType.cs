using System;

// Token: 0x02000787 RID: 1927
public enum TraitType
{
	// Token: 0x04003883 RID: 14467
	None,
	// Token: 0x04003884 RID: 14468
	NoColor = 10,
	// Token: 0x04003885 RID: 14469
	Bald = 20,
	// Token: 0x04003886 RID: 14470
	NoHealthBar = 30,
	// Token: 0x04003887 RID: 14471
	EasyBreakables = 40,
	// Token: 0x04003888 RID: 14472
	Swearing = 50,
	// Token: 0x04003889 RID: 14473
	FakeEnemies = 60,
	// Token: 0x0400388A RID: 14474
	YouAreSmall = 70,
	// Token: 0x0400388B RID: 14475
	ShowEnemiesOnMap = 80,
	// Token: 0x0400388C RID: 14476
	PlayerKnockedLow = 90,
	// Token: 0x0400388D RID: 14477
	PlayerKnockedFar = 95,
	// Token: 0x0400388E RID: 14478
	Disposition = 100,
	// Token: 0x0400388F RID: 14479
	YouAreLarge = 110,
	// Token: 0x04003890 RID: 14480
	DarkScreen = 120,
	// Token: 0x04003891 RID: 14481
	Fart = 130,
	// Token: 0x04003892 RID: 14482
	EnemyKnockedLow = 140,
	// Token: 0x04003893 RID: 14483
	BreakPropsForMana = 150,
	// Token: 0x04003894 RID: 14484
	UpsideDown = 160,
	// Token: 0x04003895 RID: 14485
	Oversaturate = 170,
	// Token: 0x04003896 RID: 14486
	CuteGame = 180,
	// Token: 0x04003897 RID: 14487
	OneHitDeath = 190,
	// Token: 0x04003898 RID: 14488
	AngryOnHit = 200,
	// Token: 0x04003899 RID: 14489
	CantAttack = 210,
	// Token: 0x0400389A RID: 14490
	CanNowAttack,
	// Token: 0x0400389B RID: 14491
	OmniDash = 220,
	// Token: 0x0400389C RID: 14492
	HighJump = 230,
	// Token: 0x0400389D RID: 14493
	InvulnDash = 240,
	// Token: 0x0400389E RID: 14494
	BoosWhenHit = 250,
	// Token: 0x0400389F RID: 14495
	GainDownStrike = 260,
	// Token: 0x040038A0 RID: 14496
	DamageBoost = 270,
	// Token: 0x040038A1 RID: 14497
	MagicBoost = 280,
	// Token: 0x040038A2 RID: 14498
	EnemyKnockedFar = 290,
	// Token: 0x040038A3 RID: 14499
	Dog = 300,
	// Token: 0x040038A4 RID: 14500
	LongerCD = 310,
	// Token: 0x040038A5 RID: 14501
	BonusHealth = 320,
	// Token: 0x040038A6 RID: 14502
	BonusStrength = 330,
	// Token: 0x040038A7 RID: 14503
	BonusMagicStrength = 340,
	// Token: 0x040038A8 RID: 14504
	FMFFan,
	// Token: 0x040038A9 RID: 14505
	NoEnemyHealthBar,
	// Token: 0x040038AA RID: 14506
	ManaCostAndDamageUp,
	// Token: 0x040038AB RID: 14507
	FreeLife,
	// Token: 0x040038AC RID: 14508
	RevealAllChests,
	// Token: 0x040038AD RID: 14509
	BounceTerrain = 350,
	// Token: 0x040038AE RID: 14510
	LowerStorePrice = 360,
	// Token: 0x040038AF RID: 14511
	GameRunsFaster = 370,
	// Token: 0x040038B0 RID: 14512
	ChickensAreEnemies = 380,
	// Token: 0x040038B1 RID: 14513
	NoMap = 390,
	// Token: 0x040038B2 RID: 14514
	FakeSelfDamage = 410,
	// Token: 0x040038B3 RID: 14515
	BlurryFar = 420,
	// Token: 0x040038B4 RID: 14516
	OldYellowTint = 430,
	// Token: 0x040038B5 RID: 14517
	RandomizeSpells = 440,
	// Token: 0x040038B6 RID: 14518
	NoProjectileIndicators = 450,
	// Token: 0x040038B7 RID: 14519
	HorizontalDarkness = 452,
	// Token: 0x040038B8 RID: 14520
	EnemiesCensored = 460,
	// Token: 0x040038B9 RID: 14521
	EnemiesBlackFill = 462,
	// Token: 0x040038BA RID: 14522
	LowerGravity = 470,
	// Token: 0x040038BB RID: 14523
	MagnetRangeBoost = 480,
	// Token: 0x040038BC RID: 14524
	CarryFood = 490,
	// Token: 0x040038BD RID: 14525
	RandomSounds = 500,
	// Token: 0x040038BE RID: 14526
	BlurOnHit = 510,
	// Token: 0x040038BF RID: 14527
	ColorTrails = 520,
	// Token: 0x040038C0 RID: 14528
	WeaponSpellSwitch = 530,
	// Token: 0x040038C1 RID: 14529
	FindBoss = 540,
	// Token: 0x040038C2 RID: 14530
	NoMeat = 550,
	// Token: 0x040038C3 RID: 14531
	SmallHitbox = 570,
	// Token: 0x040038C4 RID: 14532
	CheerOnKills = 580,
	// Token: 0x040038C5 RID: 14533
	Retro = 590,
	// Token: 0x040038C6 RID: 14534
	CameraZoomOut = 600,
	// Token: 0x040038C7 RID: 14535
	CameraZoomIn = 610,
	// Token: 0x040038C8 RID: 14536
	FreeRelic,
	// Token: 0x040038C9 RID: 14537
	DisableSpikeTraps = 620,
	// Token: 0x040038CA RID: 14538
	BackwardSpell = 630,
	// Token: 0x040038CB RID: 14539
	GameShake = 640,
	// Token: 0x040038CC RID: 14540
	WordScramble = 650,
	// Token: 0x040038CD RID: 14541
	GreenPlatformOpen = 660,
	// Token: 0x040038CE RID: 14542
	BlurryClose = 670,
	// Token: 0x040038CF RID: 14543
	DisableAttackLock = 680,
	// Token: 0x040038D0 RID: 14544
	CantSeeChildren = 690,
	// Token: 0x040038D1 RID: 14545
	Unused_1 = 720,
	// Token: 0x040038D2 RID: 14546
	LifeTimer = 730,
	// Token: 0x040038D3 RID: 14547
	SuperFart = 740,
	// Token: 0x040038D4 RID: 14548
	HomingBullets = 750,
	// Token: 0x040038D5 RID: 14549
	TeleportDamage = 760,
	// Token: 0x040038D6 RID: 14550
	FriendsLeave = 770,
	// Token: 0x040038D7 RID: 14551
	Vampire = 780,
	// Token: 0x040038D8 RID: 14552
	RandomizeWeapons = 790,
	// Token: 0x040038D9 RID: 14553
	ChanceHazardMissing = 800,
	// Token: 0x040038DA RID: 14554
	NotMovingSlowGame = 810,
	// Token: 0x040038DB RID: 14555
	MapReveal = 820,
	// Token: 0x040038DC RID: 14556
	NoImmunityWindow = 830,
	// Token: 0x040038DD RID: 14557
	NoManaCap = 840,
	// Token: 0x040038DE RID: 14558
	MegaHealth = 850,
	// Token: 0x040038DF RID: 14559
	ItemsGoFlying = 860,
	// Token: 0x040038E0 RID: 14560
	ManaFromHurt = 870,
	// Token: 0x040038E1 RID: 14561
	BonusChestGold = 880,
	// Token: 0x040038E2 RID: 14562
	SuperHealer = 890,
	// Token: 0x040038E3 RID: 14563
	TwinRelics = 900,
	// Token: 0x040038E4 RID: 14564
	MushroomGrow = 910,
	// Token: 0x040038E5 RID: 14565
	YouAreBlue = 920,
	// Token: 0x040038E6 RID: 14566
	SkillCritsOnly = 930,
	// Token: 0x040038E7 RID: 14567
	DisarmOnHurt = 940,
	// Token: 0x040038E8 RID: 14568
	ExplosiveChests = 950,
	// Token: 0x040038E9 RID: 14569
	ExplosiveEnemies = 960,
	// Token: 0x040038EA RID: 14570
	HalloweenHoliday = 970,
	// Token: 0x040038EB RID: 14571
	ChristmasHoliday,
	// Token: 0x040038EC RID: 14572
	ToneDeaf = 980,
	// Token: 0x040038ED RID: 14573
	RandomizeKit = 990,
	// Token: 0x040038EE RID: 14574
	Antique_Old = 861,
	// Token: 0x040038EF RID: 14575
	Antique = 10000
}

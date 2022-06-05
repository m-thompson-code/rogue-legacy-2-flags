using System;

// Token: 0x02000C21 RID: 3105
public enum RelicType
{
	// Token: 0x04004826 RID: 18470
	None,
	// Token: 0x04004827 RID: 18471
	ExtraLife = 10,
	// Token: 0x04004828 RID: 18472
	ExtraLifeUsed = 15,
	// Token: 0x04004829 RID: 18473
	ExtraLife_Unity = 17,
	// Token: 0x0400482A RID: 18474
	ExtraLife_UnityUsed,
	// Token: 0x0400482B RID: 18475
	LowMultiJump = 20,
	// Token: 0x0400482C RID: 18476
	BonusDamageCurse = 30,
	// Token: 0x0400482D RID: 18477
	GoldDeathCurse = 40,
	// Token: 0x0400482E RID: 18478
	GodMode = 50,
	// Token: 0x0400482F RID: 18479
	FreeEnemyKill = 60,
	// Token: 0x04004830 RID: 18480
	TakeNoDamage = 70,
	// Token: 0x04004831 RID: 18481
	BonusDamageOnNextHit = 80,
	// Token: 0x04004832 RID: 18482
	PlatformOnAerial = 90,
	// Token: 0x04004833 RID: 18483
	ChestHealthRestore = 100,
	// Token: 0x04004834 RID: 18484
	MeatMaxHealth = 110,
	// Token: 0x04004835 RID: 18485
	MeatChanceUp = 120,
	// Token: 0x04004836 RID: 18486
	ExtendInvuln = 130,
	// Token: 0x04004837 RID: 18487
	InvulnDamageBuff = 140,
	// Token: 0x04004838 RID: 18488
	DamageBuffStatusEffect = 150,
	// Token: 0x04004839 RID: 18489
	DamageReductionStatusEffect = 160,
	// Token: 0x0400483A RID: 18490
	AttackCooldown = 170,
	// Token: 0x0400483B RID: 18491
	RelicAmountDamageUp = 180,
	// Token: 0x0400483C RID: 18492
	WeaponsBurnAdd = 190,
	// Token: 0x0400483D RID: 18493
	SpellsBurnAdd = 200,
	// Token: 0x0400483E RID: 18494
	SpellsDamageCloud = 210,
	// Token: 0x0400483F RID: 18495
	EnemiesDropMeat = 220,
	// Token: 0x04004840 RID: 18496
	NoSpikeDamage = 230,
	// Token: 0x04004841 RID: 18497
	BonusMana = 240,
	// Token: 0x04004842 RID: 18498
	SpinKickArmorBreak = 250,
	// Token: 0x04004843 RID: 18499
	NoGoldXPBonus = 260,
	// Token: 0x04004844 RID: 18500
	ManaRestoreOnHurt = 270,
	// Token: 0x04004845 RID: 18501
	MagicDamageEnemyCount = 280,
	// Token: 0x04004846 RID: 18502
	FreeFairyChest = 290,
	// Token: 0x04004847 RID: 18503
	FreeFairyChestUsed = 295,
	// Token: 0x04004848 RID: 18504
	DashStrikeDamageUp = 300,
	// Token: 0x04004849 RID: 18505
	ResolveCombatChallenge = 310,
	// Token: 0x0400484A RID: 18506
	ResolveCombatChallengeUsed = 315,
	// Token: 0x0400484B RID: 18507
	CritKillsHeal = 320,
	// Token: 0x0400484C RID: 18508
	AllCritDamageUp = 330,
	// Token: 0x0400484D RID: 18509
	AllCritChanceUp = 340,
	// Token: 0x0400484E RID: 18510
	MaxManaDamage = 350,
	// Token: 0x0400484F RID: 18511
	SporeburstKillAdd = 360,
	// Token: 0x04004850 RID: 18512
	FatalBlowDodge = 370,
	// Token: 0x04004851 RID: 18513
	GoldIntoResolve = 380,
	// Token: 0x04004852 RID: 18514
	AimSlowTime = 390,
	// Token: 0x04004853 RID: 18515
	WeaponsComboAdd = 400,
	// Token: 0x04004854 RID: 18516
	MaxHealthStatBonus = 410,
	// Token: 0x04004855 RID: 18517
	LowHealthStatBonus = 420,
	// Token: 0x04004856 RID: 18518
	FreeHitRegenerate = 430,
	// Token: 0x04004857 RID: 18519
	ManaDamageReduction = 440,
	// Token: 0x04004858 RID: 18520
	GoldCombatChallenge = 450,
	// Token: 0x04004859 RID: 18521
	FoodHealsMore = 470,
	// Token: 0x0400485A RID: 18522
	SuperCritChanceUp = 480,
	// Token: 0x0400485B RID: 18523
	WeaponSwap = 500,
	// Token: 0x0400485C RID: 18524
	SpellSwap,
	// Token: 0x0400485D RID: 18525
	TalentSwap,
	// Token: 0x0400485E RID: 18526
	SpellKillMaxMana = 510,
	// Token: 0x0400485F RID: 18527
	AttackExhaust = 520,
	// Token: 0x04004860 RID: 18528
	WeaponsPoisonAdd = 530,
	// Token: 0x04004861 RID: 18529
	LowResolveWeaponDamage = 540,
	// Token: 0x04004862 RID: 18530
	LowResolveMagicDamage = 550,
	// Token: 0x04004863 RID: 18531
	GoldCombatChallengeUsed = 560,
	// Token: 0x04004864 RID: 18532
	OnHitAreaDamage = 570,
	// Token: 0x04004865 RID: 18533
	GroundDamageBonus = 580,
	// Token: 0x04004866 RID: 18534
	ProjectileDashStart = 590,
	// Token: 0x04004867 RID: 18535
	ReplacementRelic = 600,
	// Token: 0x04004868 RID: 18536
	SkillCritBonus = 610,
	// Token: 0x04004869 RID: 18537
	RangeDamageBonusCurse = 620,
	// Token: 0x0400486A RID: 18538
	SpinKickLeavesCaltrops = 630,
	// Token: 0x0400486B RID: 18539
	FlightBonusCurse = 640,
	// Token: 0x0400486C RID: 18540
	DamageAuraOnHit = 650,
	// Token: 0x0400486D RID: 18541
	LandShockwave = 660,
	// Token: 0x0400486E RID: 18542
	FoodChallenge = 670,
	// Token: 0x0400486F RID: 18543
	FoodChallengeUsed,
	// Token: 0x04004870 RID: 18544
	FreeCastSpell = 680,
	// Token: 0x04004871 RID: 18545
	NoAttackDamageBonus = 690,
	// Token: 0x04004872 RID: 18546
	DamageNoHitChallenge = 700,
	// Token: 0x04004873 RID: 18547
	DamageNoHitChallengeUsed,
	// Token: 0x04004874 RID: 18548
	SpinKickDamageBonus = 710,
	// Token: 0x04004875 RID: 18549
	Lily1 = 1000,
	// Token: 0x04004876 RID: 18550
	Lily2 = 1010,
	// Token: 0x04004877 RID: 18551
	Lily3 = 1020,
	// Token: 0x04004878 RID: 18552
	DragonKeyBlack = 2000,
	// Token: 0x04004879 RID: 18553
	DragonKeyWhite = 2010
}

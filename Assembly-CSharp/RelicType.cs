using System;

// Token: 0x02000763 RID: 1891
public enum RelicType
{
	// Token: 0x040035AA RID: 13738
	None,
	// Token: 0x040035AB RID: 13739
	ExtraLife = 10,
	// Token: 0x040035AC RID: 13740
	ExtraLifeUsed = 15,
	// Token: 0x040035AD RID: 13741
	ExtraLife_Unity = 17,
	// Token: 0x040035AE RID: 13742
	ExtraLife_UnityUsed,
	// Token: 0x040035AF RID: 13743
	LowMultiJump = 20,
	// Token: 0x040035B0 RID: 13744
	BonusDamageCurse = 30,
	// Token: 0x040035B1 RID: 13745
	GoldDeathCurse = 40,
	// Token: 0x040035B2 RID: 13746
	GodMode = 50,
	// Token: 0x040035B3 RID: 13747
	FreeEnemyKill = 60,
	// Token: 0x040035B4 RID: 13748
	TakeNoDamage = 70,
	// Token: 0x040035B5 RID: 13749
	BonusDamageOnNextHit = 80,
	// Token: 0x040035B6 RID: 13750
	PlatformOnAerial = 90,
	// Token: 0x040035B7 RID: 13751
	ChestHealthRestore = 100,
	// Token: 0x040035B8 RID: 13752
	MeatMaxHealth = 110,
	// Token: 0x040035B9 RID: 13753
	MeatChanceUp = 120,
	// Token: 0x040035BA RID: 13754
	ExtendInvuln = 130,
	// Token: 0x040035BB RID: 13755
	InvulnDamageBuff = 140,
	// Token: 0x040035BC RID: 13756
	DamageBuffStatusEffect = 150,
	// Token: 0x040035BD RID: 13757
	DamageReductionStatusEffect = 160,
	// Token: 0x040035BE RID: 13758
	AttackCooldown = 170,
	// Token: 0x040035BF RID: 13759
	RelicAmountDamageUp = 180,
	// Token: 0x040035C0 RID: 13760
	WeaponsBurnAdd = 190,
	// Token: 0x040035C1 RID: 13761
	SpellsBurnAdd = 200,
	// Token: 0x040035C2 RID: 13762
	SpellsDamageCloud = 210,
	// Token: 0x040035C3 RID: 13763
	EnemiesDropMeat = 220,
	// Token: 0x040035C4 RID: 13764
	NoSpikeDamage = 230,
	// Token: 0x040035C5 RID: 13765
	BonusMana = 240,
	// Token: 0x040035C6 RID: 13766
	SpinKickArmorBreak = 250,
	// Token: 0x040035C7 RID: 13767
	NoGoldXPBonus = 260,
	// Token: 0x040035C8 RID: 13768
	ManaRestoreOnHurt = 270,
	// Token: 0x040035C9 RID: 13769
	MagicDamageEnemyCount = 280,
	// Token: 0x040035CA RID: 13770
	FreeFairyChest = 290,
	// Token: 0x040035CB RID: 13771
	FreeFairyChestUsed = 295,
	// Token: 0x040035CC RID: 13772
	DashStrikeDamageUp = 300,
	// Token: 0x040035CD RID: 13773
	ResolveCombatChallenge = 310,
	// Token: 0x040035CE RID: 13774
	ResolveCombatChallengeUsed = 315,
	// Token: 0x040035CF RID: 13775
	CritKillsHeal = 320,
	// Token: 0x040035D0 RID: 13776
	AllCritDamageUp = 330,
	// Token: 0x040035D1 RID: 13777
	AllCritChanceUp = 340,
	// Token: 0x040035D2 RID: 13778
	MaxManaDamage = 350,
	// Token: 0x040035D3 RID: 13779
	SporeburstKillAdd = 360,
	// Token: 0x040035D4 RID: 13780
	FatalBlowDodge = 370,
	// Token: 0x040035D5 RID: 13781
	GoldIntoResolve = 380,
	// Token: 0x040035D6 RID: 13782
	AimSlowTime = 390,
	// Token: 0x040035D7 RID: 13783
	WeaponsComboAdd = 400,
	// Token: 0x040035D8 RID: 13784
	MaxHealthStatBonus = 410,
	// Token: 0x040035D9 RID: 13785
	LowHealthStatBonus = 420,
	// Token: 0x040035DA RID: 13786
	FreeHitRegenerate = 430,
	// Token: 0x040035DB RID: 13787
	ManaDamageReduction = 440,
	// Token: 0x040035DC RID: 13788
	GoldCombatChallenge = 450,
	// Token: 0x040035DD RID: 13789
	FoodHealsMore = 470,
	// Token: 0x040035DE RID: 13790
	SuperCritChanceUp = 480,
	// Token: 0x040035DF RID: 13791
	WeaponSwap = 500,
	// Token: 0x040035E0 RID: 13792
	SpellSwap,
	// Token: 0x040035E1 RID: 13793
	TalentSwap,
	// Token: 0x040035E2 RID: 13794
	SpellKillMaxMana = 510,
	// Token: 0x040035E3 RID: 13795
	AttackExhaust = 520,
	// Token: 0x040035E4 RID: 13796
	WeaponsPoisonAdd = 530,
	// Token: 0x040035E5 RID: 13797
	LowResolveWeaponDamage = 540,
	// Token: 0x040035E6 RID: 13798
	LowResolveMagicDamage = 550,
	// Token: 0x040035E7 RID: 13799
	GoldCombatChallengeUsed = 560,
	// Token: 0x040035E8 RID: 13800
	OnHitAreaDamage = 570,
	// Token: 0x040035E9 RID: 13801
	GroundDamageBonus = 580,
	// Token: 0x040035EA RID: 13802
	ProjectileDashStart = 590,
	// Token: 0x040035EB RID: 13803
	ReplacementRelic = 600,
	// Token: 0x040035EC RID: 13804
	SkillCritBonus = 610,
	// Token: 0x040035ED RID: 13805
	RangeDamageBonusCurse = 620,
	// Token: 0x040035EE RID: 13806
	SpinKickLeavesCaltrops = 630,
	// Token: 0x040035EF RID: 13807
	FlightBonusCurse = 640,
	// Token: 0x040035F0 RID: 13808
	DamageAuraOnHit = 650,
	// Token: 0x040035F1 RID: 13809
	LandShockwave = 660,
	// Token: 0x040035F2 RID: 13810
	FoodChallenge = 670,
	// Token: 0x040035F3 RID: 13811
	FoodChallengeUsed,
	// Token: 0x040035F4 RID: 13812
	FreeCastSpell = 680,
	// Token: 0x040035F5 RID: 13813
	NoAttackDamageBonus = 690,
	// Token: 0x040035F6 RID: 13814
	DamageNoHitChallenge = 700,
	// Token: 0x040035F7 RID: 13815
	DamageNoHitChallengeUsed,
	// Token: 0x040035F8 RID: 13816
	SpinKickDamageBonus = 710,
	// Token: 0x040035F9 RID: 13817
	Lily1 = 1000,
	// Token: 0x040035FA RID: 13818
	Lily2 = 1010,
	// Token: 0x040035FB RID: 13819
	Lily3 = 1020,
	// Token: 0x040035FC RID: 13820
	DragonKeyBlack = 2000,
	// Token: 0x040035FD RID: 13821
	DragonKeyWhite = 2010
}

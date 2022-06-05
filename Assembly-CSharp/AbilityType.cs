using System;

// Token: 0x02000BAD RID: 2989
public enum AbilityType
{
	// Token: 0x0400445C RID: 17500
	None,
	// Token: 0x0400445D RID: 17501
	Random = 5,
	// Token: 0x0400445E RID: 17502
	____WEAPON_ABILITIES____ = 10,
	// Token: 0x0400445F RID: 17503
	SwordWeapon = 20,
	// Token: 0x04004460 RID: 17504
	PacifistWeapon,
	// Token: 0x04004461 RID: 17505
	SwordBeamWeapon,
	// Token: 0x04004462 RID: 17506
	SpearWeapon = 30,
	// Token: 0x04004463 RID: 17507
	AxeWeapon = 40,
	// Token: 0x04004464 RID: 17508
	AxeSpinnerWeapon,
	// Token: 0x04004465 RID: 17509
	BowWeapon = 50,
	// Token: 0x04004466 RID: 17510
	KunaiWeapon,
	// Token: 0x04004467 RID: 17511
	BolaWeapon,
	// Token: 0x04004468 RID: 17512
	KineticBowWeapon,
	// Token: 0x04004469 RID: 17513
	PistolWeapon,
	// Token: 0x0400446A RID: 17514
	GroundBowWeapon,
	// Token: 0x0400446B RID: 17515
	DragonPistolWeapon,
	// Token: 0x0400446C RID: 17516
	FryingPanWeapon = 60,
	// Token: 0x0400446D RID: 17517
	SpoonsWeapon,
	// Token: 0x0400446E RID: 17518
	ChakramWeapon,
	// Token: 0x0400446F RID: 17519
	TonfaWeapon = 64,
	// Token: 0x04004470 RID: 17520
	DualBladesWeapon = 66,
	// Token: 0x04004471 RID: 17521
	BoxingGloveWeapon = 68,
	// Token: 0x04004472 RID: 17522
	MagicWandWeapon = 70,
	// Token: 0x04004473 RID: 17523
	CannonWeapon = 80,
	// Token: 0x04004474 RID: 17524
	SaberWeapon = 90,
	// Token: 0x04004475 RID: 17525
	FirstWeapon = 100,
	// Token: 0x04004476 RID: 17526
	DaggerWeapon = 110,
	// Token: 0x04004477 RID: 17527
	LanceWeapon = 120,
	// Token: 0x04004478 RID: 17528
	ScytheWeapon = 122,
	// Token: 0x04004479 RID: 17529
	KatanaWeapon = 124,
	// Token: 0x0400447A RID: 17530
	ExplosiveHandsWeapon = 126,
	// Token: 0x0400447B RID: 17531
	LuteWeapon,
	// Token: 0x0400447C RID: 17532
	AstroWandWeapon,
	// Token: 0x0400447D RID: 17533
	____SPELL_ABILITIES____ = 130,
	// Token: 0x0400447E RID: 17534
	FireballSpell = 140,
	// Token: 0x0400447F RID: 17535
	PoisonBombSpell,
	// Token: 0x04004480 RID: 17536
	AxeSpell = 145,
	// Token: 0x04004481 RID: 17537
	TrackerDotSpell,
	// Token: 0x04004482 RID: 17538
	BoomerangSpell = 150,
	// Token: 0x04004483 RID: 17539
	FlameBarrierSpell = 155,
	// Token: 0x04004484 RID: 17540
	DamageZoneSpell,
	// Token: 0x04004485 RID: 17541
	DamageWallSpell = 160,
	// Token: 0x04004486 RID: 17542
	FlameThrowerSpell = 162,
	// Token: 0x04004487 RID: 17543
	RicochetSpikesSpell = 165,
	// Token: 0x04004488 RID: 17544
	StraightBoltSpell = 178,
	// Token: 0x04004489 RID: 17545
	EnergyBounceSpell = 180,
	// Token: 0x0400448A RID: 17546
	PoolBallSpell,
	// Token: 0x0400448B RID: 17547
	SporeStrikeSpell,
	// Token: 0x0400448C RID: 17548
	GravityWellSpell,
	// Token: 0x0400448D RID: 17549
	AilmentCurseSpell,
	// Token: 0x0400448E RID: 17550
	LightningSpell,
	// Token: 0x0400448F RID: 17551
	TimeBombSpell,
	// Token: 0x04004490 RID: 17552
	SporeSpreadSpell,
	// Token: 0x04004491 RID: 17553
	ScreenSliceSpell,
	// Token: 0x04004492 RID: 17554
	FreezeStrikeSpell,
	// Token: 0x04004493 RID: 17555
	____TALENT_ABILITIES____,
	// Token: 0x04004494 RID: 17556
	ShieldBlockTalent = 200,
	// Token: 0x04004495 RID: 17557
	ShoutTalent = 210,
	// Token: 0x04004496 RID: 17558
	Counter = 220,
	// Token: 0x04004497 RID: 17559
	CookingTalent = 225,
	// Token: 0x04004498 RID: 17560
	TimeSlow = 230,
	// Token: 0x04004499 RID: 17561
	CreatePlatformTalent = 240,
	// Token: 0x0400449A RID: 17562
	HpMpSwapTalent = 250,
	// Token: 0x0400449B RID: 17563
	CloakTalent = 260,
	// Token: 0x0400449C RID: 17564
	CloakStrikeTalent,
	// Token: 0x0400449D RID: 17565
	BombardTalent = 270,
	// Token: 0x0400449E RID: 17566
	Dodge = 280,
	// Token: 0x0400449F RID: 17567
	RollTalent = 290,
	// Token: 0x040044A0 RID: 17568
	ManaBombTalent = 294,
	// Token: 0x040044A1 RID: 17569
	ReloadTalent,
	// Token: 0x040044A2 RID: 17570
	KineticReloadTalent,
	// Token: 0x040044A3 RID: 17571
	KiStrikeTalent = 300,
	// Token: 0x040044A4 RID: 17572
	CrescendoTalent = 305,
	// Token: 0x040044A5 RID: 17573
	Log = 310,
	// Token: 0x040044A6 RID: 17574
	EmpowerSpellTalent = 315,
	// Token: 0x040044A7 RID: 17575
	Siphon = 320,
	// Token: 0x040044A8 RID: 17576
	SpearSpinTalent = 325,
	// Token: 0x040044A9 RID: 17577
	Steal = 330,
	// Token: 0x040044AA RID: 17578
	SuperFart = 335,
	// Token: 0x040044AB RID: 17579
	KnockoutTalent = 340,
	// Token: 0x040044AC RID: 17580
	StaticWallTalent = 342,
	// Token: 0x040044AD RID: 17581
	TeleSliceTalent = 344,
	// Token: 0x040044AE RID: 17582
	CrowsNestTalent = 346,
	// Token: 0x040044AF RID: 17583
	CometTalent,
	// Token: 0x040044B0 RID: 17584
	____SPECIAL_ABILITIES____ = 349,
	// Token: 0x040044B1 RID: 17585
	WeaponAbilitySwap,
	// Token: 0x040044B2 RID: 17586
	SpellAbilitySwap = 360,
	// Token: 0x040044B3 RID: 17587
	TalentAbilitySwap = 370
}

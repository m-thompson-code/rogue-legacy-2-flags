using System;

// Token: 0x020006FA RID: 1786
public enum AbilityType
{
	// Token: 0x040031E1 RID: 12769
	None,
	// Token: 0x040031E2 RID: 12770
	Random = 5,
	// Token: 0x040031E3 RID: 12771
	____WEAPON_ABILITIES____ = 10,
	// Token: 0x040031E4 RID: 12772
	SwordWeapon = 20,
	// Token: 0x040031E5 RID: 12773
	PacifistWeapon,
	// Token: 0x040031E6 RID: 12774
	SwordBeamWeapon,
	// Token: 0x040031E7 RID: 12775
	SpearWeapon = 30,
	// Token: 0x040031E8 RID: 12776
	AxeWeapon = 40,
	// Token: 0x040031E9 RID: 12777
	AxeSpinnerWeapon,
	// Token: 0x040031EA RID: 12778
	BowWeapon = 50,
	// Token: 0x040031EB RID: 12779
	KunaiWeapon,
	// Token: 0x040031EC RID: 12780
	BolaWeapon,
	// Token: 0x040031ED RID: 12781
	KineticBowWeapon,
	// Token: 0x040031EE RID: 12782
	PistolWeapon,
	// Token: 0x040031EF RID: 12783
	GroundBowWeapon,
	// Token: 0x040031F0 RID: 12784
	DragonPistolWeapon,
	// Token: 0x040031F1 RID: 12785
	FryingPanWeapon = 60,
	// Token: 0x040031F2 RID: 12786
	SpoonsWeapon,
	// Token: 0x040031F3 RID: 12787
	ChakramWeapon,
	// Token: 0x040031F4 RID: 12788
	TonfaWeapon = 64,
	// Token: 0x040031F5 RID: 12789
	DualBladesWeapon = 66,
	// Token: 0x040031F6 RID: 12790
	BoxingGloveWeapon = 68,
	// Token: 0x040031F7 RID: 12791
	MagicWandWeapon = 70,
	// Token: 0x040031F8 RID: 12792
	CannonWeapon = 80,
	// Token: 0x040031F9 RID: 12793
	SaberWeapon = 90,
	// Token: 0x040031FA RID: 12794
	FirstWeapon = 100,
	// Token: 0x040031FB RID: 12795
	DaggerWeapon = 110,
	// Token: 0x040031FC RID: 12796
	LanceWeapon = 120,
	// Token: 0x040031FD RID: 12797
	ScytheWeapon = 122,
	// Token: 0x040031FE RID: 12798
	KatanaWeapon = 124,
	// Token: 0x040031FF RID: 12799
	ExplosiveHandsWeapon = 126,
	// Token: 0x04003200 RID: 12800
	LuteWeapon,
	// Token: 0x04003201 RID: 12801
	AstroWandWeapon,
	// Token: 0x04003202 RID: 12802
	____SPELL_ABILITIES____ = 130,
	// Token: 0x04003203 RID: 12803
	FireballSpell = 140,
	// Token: 0x04003204 RID: 12804
	PoisonBombSpell,
	// Token: 0x04003205 RID: 12805
	AxeSpell = 145,
	// Token: 0x04003206 RID: 12806
	TrackerDotSpell,
	// Token: 0x04003207 RID: 12807
	BoomerangSpell = 150,
	// Token: 0x04003208 RID: 12808
	FlameBarrierSpell = 155,
	// Token: 0x04003209 RID: 12809
	DamageZoneSpell,
	// Token: 0x0400320A RID: 12810
	DamageWallSpell = 160,
	// Token: 0x0400320B RID: 12811
	FlameThrowerSpell = 162,
	// Token: 0x0400320C RID: 12812
	RicochetSpikesSpell = 165,
	// Token: 0x0400320D RID: 12813
	StraightBoltSpell = 178,
	// Token: 0x0400320E RID: 12814
	EnergyBounceSpell = 180,
	// Token: 0x0400320F RID: 12815
	PoolBallSpell,
	// Token: 0x04003210 RID: 12816
	SporeStrikeSpell,
	// Token: 0x04003211 RID: 12817
	GravityWellSpell,
	// Token: 0x04003212 RID: 12818
	AilmentCurseSpell,
	// Token: 0x04003213 RID: 12819
	LightningSpell,
	// Token: 0x04003214 RID: 12820
	TimeBombSpell,
	// Token: 0x04003215 RID: 12821
	SporeSpreadSpell,
	// Token: 0x04003216 RID: 12822
	ScreenSliceSpell,
	// Token: 0x04003217 RID: 12823
	FreezeStrikeSpell,
	// Token: 0x04003218 RID: 12824
	____TALENT_ABILITIES____,
	// Token: 0x04003219 RID: 12825
	ShieldBlockTalent = 200,
	// Token: 0x0400321A RID: 12826
	ShoutTalent = 210,
	// Token: 0x0400321B RID: 12827
	Counter = 220,
	// Token: 0x0400321C RID: 12828
	CookingTalent = 225,
	// Token: 0x0400321D RID: 12829
	TimeSlow = 230,
	// Token: 0x0400321E RID: 12830
	CreatePlatformTalent = 240,
	// Token: 0x0400321F RID: 12831
	HpMpSwapTalent = 250,
	// Token: 0x04003220 RID: 12832
	CloakTalent = 260,
	// Token: 0x04003221 RID: 12833
	CloakStrikeTalent,
	// Token: 0x04003222 RID: 12834
	BombardTalent = 270,
	// Token: 0x04003223 RID: 12835
	Dodge = 280,
	// Token: 0x04003224 RID: 12836
	RollTalent = 290,
	// Token: 0x04003225 RID: 12837
	ManaBombTalent = 294,
	// Token: 0x04003226 RID: 12838
	ReloadTalent,
	// Token: 0x04003227 RID: 12839
	KineticReloadTalent,
	// Token: 0x04003228 RID: 12840
	KiStrikeTalent = 300,
	// Token: 0x04003229 RID: 12841
	CrescendoTalent = 305,
	// Token: 0x0400322A RID: 12842
	Log = 310,
	// Token: 0x0400322B RID: 12843
	EmpowerSpellTalent = 315,
	// Token: 0x0400322C RID: 12844
	Siphon = 320,
	// Token: 0x0400322D RID: 12845
	SpearSpinTalent = 325,
	// Token: 0x0400322E RID: 12846
	Steal = 330,
	// Token: 0x0400322F RID: 12847
	SuperFart = 335,
	// Token: 0x04003230 RID: 12848
	KnockoutTalent = 340,
	// Token: 0x04003231 RID: 12849
	StaticWallTalent = 342,
	// Token: 0x04003232 RID: 12850
	TeleSliceTalent = 344,
	// Token: 0x04003233 RID: 12851
	CrowsNestTalent = 346,
	// Token: 0x04003234 RID: 12852
	CometTalent,
	// Token: 0x04003235 RID: 12853
	____SPECIAL_ABILITIES____ = 349,
	// Token: 0x04003236 RID: 12854
	WeaponAbilitySwap,
	// Token: 0x04003237 RID: 12855
	SpellAbilitySwap = 360,
	// Token: 0x04003238 RID: 12856
	TalentAbilitySwap = 370
}

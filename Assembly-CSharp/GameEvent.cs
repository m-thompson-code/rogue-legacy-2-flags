using System;

// Token: 0x0200060D RID: 1549
public enum GameEvent
{
	// Token: 0x04002AF2 RID: 10994
	WorldCreationComplete,
	// Token: 0x04002AF3 RID: 10995
	BiomeCreationComplete,
	// Token: 0x04002AF4 RID: 10996
	PlayerEnterRoom,
	// Token: 0x04002AF5 RID: 10997
	PlayerHealthChange,
	// Token: 0x04002AF6 RID: 10998
	PlayerMaxHealthChange,
	// Token: 0x04002AF7 RID: 10999
	PlayerExhaustChange,
	// Token: 0x04002AF8 RID: 11000
	EnemyHealthChange,
	// Token: 0x04002AF9 RID: 11001
	EnemyHit,
	// Token: 0x04002AFA RID: 11002
	PlayerHit,
	// Token: 0x04002AFB RID: 11003
	PlayerManaChange,
	// Token: 0x04002AFC RID: 11004
	PlayerAmmoChange,
	// Token: 0x04002AFD RID: 11005
	PlayerForceManaRegen,
	// Token: 0x04002AFE RID: 11006
	PlayerExitRoom,
	// Token: 0x04002AFF RID: 11007
	GamePauseStateChange,
	// Token: 0x04002B00 RID: 11008
	PlayerEnterParade,
	// Token: 0x04002B01 RID: 11009
	BiomeEnter,
	// Token: 0x04002B02 RID: 11010
	BiomeExit,
	// Token: 0x04002B03 RID: 11011
	LevelManagerStateChange,
	// Token: 0x04002B04 RID: 11012
	PlayerJump,
	// Token: 0x04002B05 RID: 11013
	PlayerDoubleJump,
	// Token: 0x04002B06 RID: 11014
	PlayerWeaponAbilityCast,
	// Token: 0x04002B07 RID: 11015
	PlayerDash,
	// Token: 0x04002B08 RID: 11016
	AbilityCooldownOver,
	// Token: 0x04002B09 RID: 11017
	PlayerSpellAbilityCast,
	// Token: 0x04002B0A RID: 11018
	PlayerTalentAbilityCast,
	// Token: 0x04002B0B RID: 11019
	PlayerDownstrikeCast,
	// Token: 0x04002B0C RID: 11020
	PlayerDownstrikeBounce,
	// Token: 0x04002B0D RID: 11021
	PlayerBlocked,
	// Token: 0x04002B0E RID: 11022
	ChangeAbility,
	// Token: 0x04002B0F RID: 11023
	UpdateAbilityHUD,
	// Token: 0x04002B10 RID: 11024
	UpdateAbilityDisarmState,
	// Token: 0x04002B11 RID: 11025
	PlayerScaleChanged,
	// Token: 0x04002B12 RID: 11026
	PlayerCastingAstroWand,
	// Token: 0x04002B13 RID: 11027
	PlayerEnterFairyRoom,
	// Token: 0x04002B14 RID: 11028
	PlayerExitFairyRoom,
	// Token: 0x04002B15 RID: 11029
	PlayerFairyRoomTriggered,
	// Token: 0x04002B16 RID: 11030
	FairyRoomRuleStateChange,
	// Token: 0x04002B17 RID: 11031
	FairyRoomStateChange,
	// Token: 0x04002B18 RID: 11032
	PlayerEnterClownRoom,
	// Token: 0x04002B19 RID: 11033
	PlayerExitClownRoom,
	// Token: 0x04002B1A RID: 11034
	GoldDropped,
	// Token: 0x04002B1B RID: 11035
	SpecialItemsDropped,
	// Token: 0x04002B1C RID: 11036
	EnemyDeath,
	// Token: 0x04002B1D RID: 11037
	EnemyModeShift,
	// Token: 0x04002B1E RID: 11038
	PlayerDeath,
	// Token: 0x04002B1F RID: 11039
	PlayerFakedDeath,
	// Token: 0x04002B20 RID: 11040
	TraitsChanged,
	// Token: 0x04002B21 RID: 11041
	SkyLightColorChanged,
	// Token: 0x04002B22 RID: 11042
	LevelEditorWorldCreationComplete,
	// Token: 0x04002B23 RID: 11043
	ItemCollected,
	// Token: 0x04002B24 RID: 11044
	GoldChanged,
	// Token: 0x04002B25 RID: 11045
	GoldSavedChanged,
	// Token: 0x04002B26 RID: 11046
	RuneOreChanged,
	// Token: 0x04002B27 RID: 11047
	EquipmentOreChanged,
	// Token: 0x04002B28 RID: 11048
	SoulChanged,
	// Token: 0x04002B29 RID: 11049
	ChestOpened,
	// Token: 0x04002B2A RID: 11050
	RelicLevelChanged,
	// Token: 0x04002B2B RID: 11051
	RelicStatsChanged,
	// Token: 0x04002B2C RID: 11052
	RelicsReset,
	// Token: 0x04002B2D RID: 11053
	RelicPurified,
	// Token: 0x04002B2E RID: 11054
	HeirloomLevelChanged,
	// Token: 0x04002B2F RID: 11055
	BreakableHit,
	// Token: 0x04002B30 RID: 11056
	BreakableDestroyed,
	// Token: 0x04002B31 RID: 11057
	CameraZoomChange,
	// Token: 0x04002B32 RID: 11058
	SpecialRoomCompleted,
	// Token: 0x04002B33 RID: 11059
	PlayerEnterHubTown,
	// Token: 0x04002B34 RID: 11060
	PlayerExitHubTown,
	// Token: 0x04002B35 RID: 11061
	SummonRuleBroadcast,
	// Token: 0x04002B36 RID: 11062
	EnemySummoned,
	// Token: 0x04002B37 RID: 11063
	WorldCreationFailed,
	// Token: 0x04002B38 RID: 11064
	PlayerResolveChanged,
	// Token: 0x04002B39 RID: 11065
	NGPlusChanged,
	// Token: 0x04002B3A RID: 11066
	HouseRulesChanged,
	// Token: 0x04002B3B RID: 11067
	UpdatePools
}

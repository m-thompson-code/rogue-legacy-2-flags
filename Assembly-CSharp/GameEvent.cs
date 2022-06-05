using System;

// Token: 0x02000A2F RID: 2607
public enum GameEvent
{
	// Token: 0x04003B59 RID: 15193
	WorldCreationComplete,
	// Token: 0x04003B5A RID: 15194
	BiomeCreationComplete,
	// Token: 0x04003B5B RID: 15195
	PlayerEnterRoom,
	// Token: 0x04003B5C RID: 15196
	PlayerHealthChange,
	// Token: 0x04003B5D RID: 15197
	PlayerMaxHealthChange,
	// Token: 0x04003B5E RID: 15198
	PlayerExhaustChange,
	// Token: 0x04003B5F RID: 15199
	EnemyHealthChange,
	// Token: 0x04003B60 RID: 15200
	EnemyHit,
	// Token: 0x04003B61 RID: 15201
	PlayerHit,
	// Token: 0x04003B62 RID: 15202
	PlayerManaChange,
	// Token: 0x04003B63 RID: 15203
	PlayerAmmoChange,
	// Token: 0x04003B64 RID: 15204
	PlayerForceManaRegen,
	// Token: 0x04003B65 RID: 15205
	PlayerExitRoom,
	// Token: 0x04003B66 RID: 15206
	GamePauseStateChange,
	// Token: 0x04003B67 RID: 15207
	PlayerEnterParade,
	// Token: 0x04003B68 RID: 15208
	BiomeEnter,
	// Token: 0x04003B69 RID: 15209
	BiomeExit,
	// Token: 0x04003B6A RID: 15210
	LevelManagerStateChange,
	// Token: 0x04003B6B RID: 15211
	PlayerJump,
	// Token: 0x04003B6C RID: 15212
	PlayerDoubleJump,
	// Token: 0x04003B6D RID: 15213
	PlayerWeaponAbilityCast,
	// Token: 0x04003B6E RID: 15214
	PlayerDash,
	// Token: 0x04003B6F RID: 15215
	AbilityCooldownOver,
	// Token: 0x04003B70 RID: 15216
	PlayerSpellAbilityCast,
	// Token: 0x04003B71 RID: 15217
	PlayerTalentAbilityCast,
	// Token: 0x04003B72 RID: 15218
	PlayerDownstrikeCast,
	// Token: 0x04003B73 RID: 15219
	PlayerDownstrikeBounce,
	// Token: 0x04003B74 RID: 15220
	PlayerBlocked,
	// Token: 0x04003B75 RID: 15221
	ChangeAbility,
	// Token: 0x04003B76 RID: 15222
	UpdateAbilityHUD,
	// Token: 0x04003B77 RID: 15223
	UpdateAbilityDisarmState,
	// Token: 0x04003B78 RID: 15224
	PlayerScaleChanged,
	// Token: 0x04003B79 RID: 15225
	PlayerCastingAstroWand,
	// Token: 0x04003B7A RID: 15226
	PlayerEnterFairyRoom,
	// Token: 0x04003B7B RID: 15227
	PlayerExitFairyRoom,
	// Token: 0x04003B7C RID: 15228
	PlayerFairyRoomTriggered,
	// Token: 0x04003B7D RID: 15229
	FairyRoomRuleStateChange,
	// Token: 0x04003B7E RID: 15230
	FairyRoomStateChange,
	// Token: 0x04003B7F RID: 15231
	PlayerEnterClownRoom,
	// Token: 0x04003B80 RID: 15232
	PlayerExitClownRoom,
	// Token: 0x04003B81 RID: 15233
	GoldDropped,
	// Token: 0x04003B82 RID: 15234
	SpecialItemsDropped,
	// Token: 0x04003B83 RID: 15235
	EnemyDeath,
	// Token: 0x04003B84 RID: 15236
	EnemyModeShift,
	// Token: 0x04003B85 RID: 15237
	PlayerDeath,
	// Token: 0x04003B86 RID: 15238
	PlayerFakedDeath,
	// Token: 0x04003B87 RID: 15239
	TraitsChanged,
	// Token: 0x04003B88 RID: 15240
	SkyLightColorChanged,
	// Token: 0x04003B89 RID: 15241
	LevelEditorWorldCreationComplete,
	// Token: 0x04003B8A RID: 15242
	ItemCollected,
	// Token: 0x04003B8B RID: 15243
	GoldChanged,
	// Token: 0x04003B8C RID: 15244
	GoldSavedChanged,
	// Token: 0x04003B8D RID: 15245
	RuneOreChanged,
	// Token: 0x04003B8E RID: 15246
	EquipmentOreChanged,
	// Token: 0x04003B8F RID: 15247
	SoulChanged,
	// Token: 0x04003B90 RID: 15248
	ChestOpened,
	// Token: 0x04003B91 RID: 15249
	RelicLevelChanged,
	// Token: 0x04003B92 RID: 15250
	RelicStatsChanged,
	// Token: 0x04003B93 RID: 15251
	RelicsReset,
	// Token: 0x04003B94 RID: 15252
	RelicPurified,
	// Token: 0x04003B95 RID: 15253
	HeirloomLevelChanged,
	// Token: 0x04003B96 RID: 15254
	BreakableHit,
	// Token: 0x04003B97 RID: 15255
	BreakableDestroyed,
	// Token: 0x04003B98 RID: 15256
	CameraZoomChange,
	// Token: 0x04003B99 RID: 15257
	SpecialRoomCompleted,
	// Token: 0x04003B9A RID: 15258
	PlayerEnterHubTown,
	// Token: 0x04003B9B RID: 15259
	PlayerExitHubTown,
	// Token: 0x04003B9C RID: 15260
	SummonRuleBroadcast,
	// Token: 0x04003B9D RID: 15261
	EnemySummoned,
	// Token: 0x04003B9E RID: 15262
	WorldCreationFailed,
	// Token: 0x04003B9F RID: 15263
	PlayerResolveChanged,
	// Token: 0x04003BA0 RID: 15264
	NGPlusChanged,
	// Token: 0x04003BA1 RID: 15265
	HouseRulesChanged,
	// Token: 0x04003BA2 RID: 15266
	UpdatePools
}

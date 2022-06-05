using System;

// Token: 0x02000C0B RID: 3083
public enum LayerType
{
	// Token: 0x04004772 RID: 18290
	None = -1,
	// Token: 0x04004773 RID: 18291
	Default,
	// Token: 0x04004774 RID: 18292
	TransparentFX,
	// Token: 0x04004775 RID: 18293
	Ignore_Raycast,
	// Token: 0x04004776 RID: 18294
	Water = 4,
	// Token: 0x04004777 RID: 18295
	UI,
	// Token: 0x04004778 RID: 18296
	Terrain_Hitbox_ItemDrop = 7,
	// Token: 0x04004779 RID: 18297
	Platform_CollidesWithAll,
	// Token: 0x0400477A RID: 18298
	Platform_CollidesWithPlayer,
	// Token: 0x0400477B RID: 18299
	Platform_CollidesWithEnemy,
	// Token: 0x0400477C RID: 18300
	Platform_OneWay,
	// Token: 0x0400477D RID: 18301
	Prop_Hitbox,
	// Token: 0x0400477E RID: 18302
	Terrain_Hitbox,
	// Token: 0x0400477F RID: 18303
	LevelBounds,
	// Token: 0x04004780 RID: 18304
	HELPER_NOTOUCH,
	// Token: 0x04004781 RID: 18305
	Weapon_Hitbox,
	// Token: 0x04004782 RID: 18306
	Weapon_Hitbox_HitsPlayerOnly,
	// Token: 0x04004783 RID: 18307
	Terrain_Hitbox_HitsPlatform,
	// Token: 0x04004784 RID: 18308
	Foreground_Persp,
	// Token: 0x04004785 RID: 18309
	Body_Hitbox_ForPlayerOnly,
	// Token: 0x04004786 RID: 18310
	Body_Hitbox,
	// Token: 0x04004787 RID: 18311
	Background_Persp_Near,
	// Token: 0x04004788 RID: 18312
	Foreground_Ortho,
	// Token: 0x04004789 RID: 18313
	Background_Ortho,
	// Token: 0x0400478A RID: 18314
	Foreground_Lights,
	// Token: 0x0400478B RID: 18315
	Background_Persp_Far,
	// Token: 0x0400478C RID: 18316
	TraitMask,
	// Token: 0x0400478D RID: 18317
	Solo,
	// Token: 0x0400478E RID: 18318
	Character,
	// Token: 0x0400478F RID: 18319
	TerrainHazard_Hitbox
}

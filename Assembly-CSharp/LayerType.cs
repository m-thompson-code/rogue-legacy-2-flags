using System;

// Token: 0x0200074D RID: 1869
public enum LayerType
{
	// Token: 0x040034F6 RID: 13558
	None = -1,
	// Token: 0x040034F7 RID: 13559
	Default,
	// Token: 0x040034F8 RID: 13560
	TransparentFX,
	// Token: 0x040034F9 RID: 13561
	Ignore_Raycast,
	// Token: 0x040034FA RID: 13562
	Water = 4,
	// Token: 0x040034FB RID: 13563
	UI,
	// Token: 0x040034FC RID: 13564
	Terrain_Hitbox_ItemDrop = 7,
	// Token: 0x040034FD RID: 13565
	Platform_CollidesWithAll,
	// Token: 0x040034FE RID: 13566
	Platform_CollidesWithPlayer,
	// Token: 0x040034FF RID: 13567
	Platform_CollidesWithEnemy,
	// Token: 0x04003500 RID: 13568
	Platform_OneWay,
	// Token: 0x04003501 RID: 13569
	Prop_Hitbox,
	// Token: 0x04003502 RID: 13570
	Terrain_Hitbox,
	// Token: 0x04003503 RID: 13571
	LevelBounds,
	// Token: 0x04003504 RID: 13572
	HELPER_NOTOUCH,
	// Token: 0x04003505 RID: 13573
	Weapon_Hitbox,
	// Token: 0x04003506 RID: 13574
	Weapon_Hitbox_HitsPlayerOnly,
	// Token: 0x04003507 RID: 13575
	Terrain_Hitbox_HitsPlatform,
	// Token: 0x04003508 RID: 13576
	Foreground_Persp,
	// Token: 0x04003509 RID: 13577
	Body_Hitbox_ForPlayerOnly,
	// Token: 0x0400350A RID: 13578
	Body_Hitbox,
	// Token: 0x0400350B RID: 13579
	Background_Persp_Near,
	// Token: 0x0400350C RID: 13580
	Foreground_Ortho,
	// Token: 0x0400350D RID: 13581
	Background_Ortho,
	// Token: 0x0400350E RID: 13582
	Foreground_Lights,
	// Token: 0x0400350F RID: 13583
	Background_Persp_Far,
	// Token: 0x04003510 RID: 13584
	TraitMask,
	// Token: 0x04003511 RID: 13585
	Solo,
	// Token: 0x04003512 RID: 13586
	Character,
	// Token: 0x04003513 RID: 13587
	TerrainHazard_Hitbox
}

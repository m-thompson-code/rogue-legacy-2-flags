using System;

// Token: 0x0200074E RID: 1870
public static class LayerType_RL
{
	// Token: 0x06004116 RID: 16662 RVA: 0x000E66EC File Offset: 0x000E48EC
	public static LayerType ToEnum(string value)
	{
		if (value != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(value);
			if (num <= 2655785868U)
			{
				if (num <= 1331544411U)
				{
					if (num <= 288168205U)
					{
						if (num != 72723383U)
						{
							if (num != 170935408U)
							{
								if (num == 288168205U)
								{
									if (value == "Foreground_Ortho")
									{
										return LayerType.Foreground_Ortho;
									}
								}
							}
							else if (value == "Weapon_Hitbox_HitsPlayerOnly")
							{
								return LayerType.Weapon_Hitbox_HitsPlayerOnly;
							}
						}
						else if (value == "Foreground_Persp")
						{
							return LayerType.Foreground_Persp;
						}
					}
					else if (num <= 856118464U)
					{
						if (num != 433860734U)
						{
							if (num == 856118464U)
							{
								if (value == "Background_Persp_Far")
								{
									return LayerType.Background_Persp_Far;
								}
							}
						}
						else if (value == "Default")
						{
							return LayerType.Default;
						}
					}
					else if (num != 993156533U)
					{
						if (num == 1331544411U)
						{
							if (value == "Terrain_Hitbox_HitsPlayerOnly")
							{
								return LayerType.Terrain_Hitbox_ItemDrop;
							}
						}
					}
					else if (value == "Platform_CollidesWithPlayer")
					{
						return LayerType.Platform_CollidesWithPlayer;
					}
				}
				else if (num <= 1971233420U)
				{
					if (num != 1717296836U)
					{
						if (num != 1771018090U)
						{
							if (num == 1971233420U)
							{
								if (value == "Weapon_Hitbox")
								{
									return LayerType.Weapon_Hitbox;
								}
							}
						}
						else if (value == "Solo")
						{
							return LayerType.Solo;
						}
					}
					else if (value == "Ignore Raycast")
					{
						return LayerType.Ignore_Raycast;
					}
				}
				else if (num <= 2141809075U)
				{
					if (num != 2079568635U)
					{
						if (num == 2141809075U)
						{
							if (value == "Platform_CollidesWithAll")
							{
								return LayerType.Platform_CollidesWithAll;
							}
						}
					}
					else if (value == "UI")
					{
						return LayerType.UI;
					}
				}
				else if (num != 2650676383U)
				{
					if (num == 2655785868U)
					{
						if (value == "Foreground_Lights")
						{
							return LayerType.Foreground_Lights;
						}
					}
				}
				else if (value == "Body_Hitbox_ForPlayerOnly")
				{
					return LayerType.Body_Hitbox_ForPlayerOnly;
				}
			}
			else if (num <= 3570090381U)
			{
				if (num <= 3110552472U)
				{
					if (num != 2880939860U)
					{
						if (num != 2953743791U)
						{
							if (num == 3110552472U)
							{
								if (value == "HELPER_NOTOUCH")
								{
									return LayerType.HELPER_NOTOUCH;
								}
							}
						}
						else if (value == "Terrain_Hitbox_HitsPlatform")
						{
							return LayerType.Terrain_Hitbox_HitsPlatform;
						}
					}
					else if (value == "Platform_OneWay")
					{
						return LayerType.Platform_OneWay;
					}
				}
				else if (num <= 3129590336U)
				{
					if (num != 3115705312U)
					{
						if (num == 3129590336U)
						{
							if (value == "Body_Hitbox")
							{
								return LayerType.Body_Hitbox;
							}
						}
					}
					else if (value == "Background_Ortho")
					{
						return LayerType.Background_Ortho;
					}
				}
				else if (num != 3271495773U)
				{
					if (num == 3570090381U)
					{
						if (value == "Background_Persp_Near")
						{
							return LayerType.Background_Persp_Near;
						}
					}
				}
				else if (value == "TerrainHazard_Hitbox")
				{
					return LayerType.TerrainHazard_Hitbox;
				}
			}
			else if (num <= 3966976176U)
			{
				if (num <= 3599606039U)
				{
					if (num != 3593819824U)
					{
						if (num == 3599606039U)
						{
							if (value == "TransparentFX")
							{
								return LayerType.TransparentFX;
							}
						}
					}
					else if (value == "Water")
					{
						return LayerType.Water;
					}
				}
				else if (num != 3628603293U)
				{
					if (num == 3966976176U)
					{
						if (value == "Character")
						{
							return LayerType.Character;
						}
					}
				}
				else if (value == "Terrain_Hitbox")
				{
					return LayerType.Terrain_Hitbox;
				}
			}
			else if (num <= 4013742780U)
			{
				if (num != 3972003551U)
				{
					if (num == 4013742780U)
					{
						if (value == "Platform_CollidesWithEnemy")
						{
							return LayerType.Platform_CollidesWithEnemy;
						}
					}
				}
				else if (value == "TraitMask")
				{
					return LayerType.TraitMask;
				}
			}
			else if (num != 4194203456U)
			{
				if (num == 4251462839U)
				{
					if (value == "Prop_Hitbox")
					{
						return LayerType.Prop_Hitbox;
					}
				}
			}
			else if (value == "LevelBounds")
			{
				return LayerType.LevelBounds;
			}
		}
		return LayerType.None;
	}

	// Token: 0x06004117 RID: 16663 RVA: 0x000E6B80 File Offset: 0x000E4D80
	public static string ToString(LayerType value)
	{
		switch (value)
		{
		case LayerType.Default:
			return "Default";
		case LayerType.TransparentFX:
			return "TransparentFX";
		case LayerType.Ignore_Raycast:
			return "Ignore Raycast";
		case LayerType.Water:
			return "Water";
		case LayerType.UI:
			return "UI";
		case LayerType.Terrain_Hitbox_ItemDrop:
			return "Terrain_Hitbox_HitsPlayerOnly";
		case LayerType.Platform_CollidesWithAll:
			return "Platform_CollidesWithAll";
		case LayerType.Platform_CollidesWithPlayer:
			return "Platform_CollidesWithPlayer";
		case LayerType.Platform_CollidesWithEnemy:
			return "Platform_CollidesWithEnemy";
		case LayerType.Platform_OneWay:
			return "Platform_OneWay";
		case LayerType.Prop_Hitbox:
			return "Prop_Hitbox";
		case LayerType.Terrain_Hitbox:
			return "Terrain_Hitbox";
		case LayerType.LevelBounds:
			return "LevelBounds";
		case LayerType.HELPER_NOTOUCH:
			return "HELPER_NOTOUCH";
		case LayerType.Weapon_Hitbox:
			return "Weapon_Hitbox";
		case LayerType.Weapon_Hitbox_HitsPlayerOnly:
			return "Weapon_Hitbox_HitsPlayerOnly";
		case LayerType.Terrain_Hitbox_HitsPlatform:
			return "Terrain_Hitbox_HitsPlatform";
		case LayerType.Foreground_Persp:
			return "Foreground_Persp";
		case LayerType.Body_Hitbox_ForPlayerOnly:
			return "Body_Hitbox_ForPlayerOnly";
		case LayerType.Body_Hitbox:
			return "Body_Hitbox";
		case LayerType.Background_Persp_Near:
			return "Background_Persp_Near";
		case LayerType.Foreground_Ortho:
			return "Foreground_Ortho";
		case LayerType.Background_Ortho:
			return "Background_Ortho";
		case LayerType.Foreground_Lights:
			return "Foreground_Lights";
		case LayerType.Background_Persp_Far:
			return "Background_Persp_Far";
		case LayerType.TraitMask:
			return "TraitMask";
		case LayerType.Solo:
			return "Solo";
		case LayerType.Character:
			return "Character";
		case LayerType.TerrainHazard_Hitbox:
			return "TerrainHazard_Hitbox";
		}
		return "";
	}

	// Token: 0x04003514 RID: 13588
	public const string Default = "Default";

	// Token: 0x04003515 RID: 13589
	public const string TransparentFX = "TransparentFX";

	// Token: 0x04003516 RID: 13590
	public const string Ignore_Raycast = "Ignore Raycast";

	// Token: 0x04003517 RID: 13591
	public const string Water = "Water";

	// Token: 0x04003518 RID: 13592
	public const string UI = "UI";

	// Token: 0x04003519 RID: 13593
	public const string Terrain_Hitbox_ItemDrop = "Terrain_Hitbox_HitsPlayerOnly";

	// Token: 0x0400351A RID: 13594
	public const string Platform_CollidesWithAll = "Platform_CollidesWithAll";

	// Token: 0x0400351B RID: 13595
	public const string Platform_CollidesWithPlayer = "Platform_CollidesWithPlayer";

	// Token: 0x0400351C RID: 13596
	public const string Platform_CollidesWithEnemy = "Platform_CollidesWithEnemy";

	// Token: 0x0400351D RID: 13597
	public const string Platform_OneWay = "Platform_OneWay";

	// Token: 0x0400351E RID: 13598
	public const string Prop_Hitbox = "Prop_Hitbox";

	// Token: 0x0400351F RID: 13599
	public const string Terrain_Hitbox = "Terrain_Hitbox";

	// Token: 0x04003520 RID: 13600
	public const string LevelBounds = "LevelBounds";

	// Token: 0x04003521 RID: 13601
	public const string HELPER_NOTOUCH = "HELPER_NOTOUCH";

	// Token: 0x04003522 RID: 13602
	public const string Weapon_Hitbox = "Weapon_Hitbox";

	// Token: 0x04003523 RID: 13603
	public const string Weapon_Hitbox_HitsPlayerOnly = "Weapon_Hitbox_HitsPlayerOnly";

	// Token: 0x04003524 RID: 13604
	public const string Terrain_Hitbox_HitsPlatform = "Terrain_Hitbox_HitsPlatform";

	// Token: 0x04003525 RID: 13605
	public const string Foreground_Persp = "Foreground_Persp";

	// Token: 0x04003526 RID: 13606
	public const string Body_Hitbox_ForPlayerOnly = "Body_Hitbox_ForPlayerOnly";

	// Token: 0x04003527 RID: 13607
	public const string Body_Hitbox = "Body_Hitbox";

	// Token: 0x04003528 RID: 13608
	public const string Background_Persp_Near = "Background_Persp_Near";

	// Token: 0x04003529 RID: 13609
	public const string Foreground_Ortho = "Foreground_Ortho";

	// Token: 0x0400352A RID: 13610
	public const string Background_Ortho = "Background_Ortho";

	// Token: 0x0400352B RID: 13611
	public const string Foreground_Lights = "Foreground_Lights";

	// Token: 0x0400352C RID: 13612
	public const string Background_Persp_Far = "Background_Persp_Far";

	// Token: 0x0400352D RID: 13613
	public const string TraitMask = "TraitMask";

	// Token: 0x0400352E RID: 13614
	public const string Solo = "Solo";

	// Token: 0x0400352F RID: 13615
	public const string Character = "Character";

	// Token: 0x04003530 RID: 13616
	public const string TerrainHazard_Hitbox = "TerrainHazard_Hitbox";
}

using System;

// Token: 0x02000C0C RID: 3084
public static class LayerType_RL
{
	// Token: 0x06005A93 RID: 23187 RVA: 0x00155F30 File Offset: 0x00154130
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

	// Token: 0x06005A94 RID: 23188 RVA: 0x001563C4 File Offset: 0x001545C4
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

	// Token: 0x04004790 RID: 18320
	public const string Default = "Default";

	// Token: 0x04004791 RID: 18321
	public const string TransparentFX = "TransparentFX";

	// Token: 0x04004792 RID: 18322
	public const string Ignore_Raycast = "Ignore Raycast";

	// Token: 0x04004793 RID: 18323
	public const string Water = "Water";

	// Token: 0x04004794 RID: 18324
	public const string UI = "UI";

	// Token: 0x04004795 RID: 18325
	public const string Terrain_Hitbox_ItemDrop = "Terrain_Hitbox_HitsPlayerOnly";

	// Token: 0x04004796 RID: 18326
	public const string Platform_CollidesWithAll = "Platform_CollidesWithAll";

	// Token: 0x04004797 RID: 18327
	public const string Platform_CollidesWithPlayer = "Platform_CollidesWithPlayer";

	// Token: 0x04004798 RID: 18328
	public const string Platform_CollidesWithEnemy = "Platform_CollidesWithEnemy";

	// Token: 0x04004799 RID: 18329
	public const string Platform_OneWay = "Platform_OneWay";

	// Token: 0x0400479A RID: 18330
	public const string Prop_Hitbox = "Prop_Hitbox";

	// Token: 0x0400479B RID: 18331
	public const string Terrain_Hitbox = "Terrain_Hitbox";

	// Token: 0x0400479C RID: 18332
	public const string LevelBounds = "LevelBounds";

	// Token: 0x0400479D RID: 18333
	public const string HELPER_NOTOUCH = "HELPER_NOTOUCH";

	// Token: 0x0400479E RID: 18334
	public const string Weapon_Hitbox = "Weapon_Hitbox";

	// Token: 0x0400479F RID: 18335
	public const string Weapon_Hitbox_HitsPlayerOnly = "Weapon_Hitbox_HitsPlayerOnly";

	// Token: 0x040047A0 RID: 18336
	public const string Terrain_Hitbox_HitsPlatform = "Terrain_Hitbox_HitsPlatform";

	// Token: 0x040047A1 RID: 18337
	public const string Foreground_Persp = "Foreground_Persp";

	// Token: 0x040047A2 RID: 18338
	public const string Body_Hitbox_ForPlayerOnly = "Body_Hitbox_ForPlayerOnly";

	// Token: 0x040047A3 RID: 18339
	public const string Body_Hitbox = "Body_Hitbox";

	// Token: 0x040047A4 RID: 18340
	public const string Background_Persp_Near = "Background_Persp_Near";

	// Token: 0x040047A5 RID: 18341
	public const string Foreground_Ortho = "Foreground_Ortho";

	// Token: 0x040047A6 RID: 18342
	public const string Background_Ortho = "Background_Ortho";

	// Token: 0x040047A7 RID: 18343
	public const string Foreground_Lights = "Foreground_Lights";

	// Token: 0x040047A8 RID: 18344
	public const string Background_Persp_Far = "Background_Persp_Far";

	// Token: 0x040047A9 RID: 18345
	public const string TraitMask = "TraitMask";

	// Token: 0x040047AA RID: 18346
	public const string Solo = "Solo";

	// Token: 0x040047AB RID: 18347
	public const string Character = "Character";

	// Token: 0x040047AC RID: 18348
	public const string TerrainHazard_Hitbox = "TerrainHazard_Hitbox";
}

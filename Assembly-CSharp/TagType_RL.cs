using System;

// Token: 0x02000C45 RID: 3141
public static class TagType_RL
{
	// Token: 0x06005AD7 RID: 23255 RVA: 0x00157F9C File Offset: 0x0015619C
	public static TagType ToEnum(string value)
	{
		if (value != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(value);
			if (num <= 1873453403U)
			{
				if (num <= 1048913679U)
				{
					if (num <= 889681971U)
					{
						if (num <= 319802154U)
						{
							if (num != 225008822U)
							{
								if (num == 319802154U)
								{
									if (value == "EditorOnly")
									{
										return TagType.EditorOnly;
									}
								}
							}
							else if (value == "OneWay")
							{
								return TagType.OneWay;
							}
						}
						else if (num != 477166624U)
						{
							if (num != 581157159U)
							{
								if (num == 889681971U)
								{
									if (value == "Player_Dodging")
									{
										return TagType.Player_Dodging;
									}
								}
							}
							else if (value == "EquipmentButton")
							{
								return TagType.EquipmentButton;
							}
						}
						else if (value == "Player")
						{
							return TagType.Player;
						}
					}
					else if (num <= 935994695U)
					{
						if (num != 913927915U)
						{
							if (num == 935994695U)
							{
								if (value == "PlayerProjectile")
								{
									return TagType.PlayerProjectile;
								}
							}
						}
						else if (value == "TriggerHazard")
						{
							return TagType.TriggerHazard;
						}
					}
					else if (num != 1004962635U)
					{
						if (num != 1040536566U)
						{
							if (num == 1048913679U)
							{
								if (value == "MagicPlatform")
								{
									return TagType.MagicPlatform;
								}
							}
						}
						else if (value == "Chest")
						{
							return TagType.Chest;
						}
					}
					else if (value == "Enemy")
					{
						return TagType.Enemy;
					}
				}
				else if (num <= 1667813495U)
				{
					if (num <= 1436705836U)
					{
						if (num != 1075685656U)
						{
							if (num == 1436705836U)
							{
								if (value == "EnemyStatusEffect")
								{
									return TagType.EnemyStatusEffect;
								}
							}
						}
						else if (value == "Prop")
						{
							return TagType.Prop;
						}
					}
					else if (num != 1481252212U)
					{
						if (num != 1624112240U)
						{
							if (num == 1667813495U)
							{
								if (value == "Tunnel")
								{
									return TagType.Tunnel;
								}
							}
						}
						else if (value == "Barricade")
						{
							return TagType.Barricade;
						}
					}
					else if (value == "Music")
					{
						return TagType.Music;
					}
				}
				else if (num <= 1768171323U)
				{
					if (num != 1722185712U)
					{
						if (num == 1768171323U)
						{
							if (value == "GameController")
							{
								return TagType.GameController;
							}
						}
					}
					else if (value == "NPC")
					{
						return TagType.NPC;
					}
				}
				else if (num != 1799232731U)
				{
					if (num != 1857824372U)
					{
						if (num == 1873453403U)
						{
							if (value == "ItemDrop")
							{
								return TagType.ItemDrop;
							}
						}
					}
					else if (value == "Effect")
					{
						return TagType.Effect;
					}
				}
				else if (value == "TeleporterTube")
				{
					return TagType.TeleporterTube;
				}
			}
			else if (num <= 3137079997U)
			{
				if (num <= 2395470568U)
				{
					if (num <= 2079518418U)
					{
						if (num != 1899975098U)
						{
							if (num == 2079518418U)
							{
								if (value == "Platform")
								{
									return TagType.Platform;
								}
							}
						}
						else if (value == "Projectile")
						{
							return TagType.Projectile;
						}
					}
					else if (num != 2295680585U)
					{
						if (num != 2357445072U)
						{
							if (num == 2395470568U)
							{
								if (value == "EnemyProjectile")
								{
									return TagType.EnemyProjectile;
								}
							}
						}
						else if (value == "Breakable")
						{
							return TagType.Breakable;
						}
					}
					else if (value == "Door")
					{
						return TagType.Door;
					}
				}
				else if (num <= 2673983443U)
				{
					if (num != 2670325426U)
					{
						if (num == 2673983443U)
						{
							if (value == "Generic_Bounceable")
							{
								return TagType.Generic_Bounceable;
							}
						}
					}
					else if (value == "Finish")
					{
						return TagType.Finish;
					}
				}
				else if (num != 3089727130U)
				{
					if (num != 3132604649U)
					{
						if (num == 3137079997U)
						{
							if (value == "Background")
							{
								return TagType.Background;
							}
						}
					}
					else if (value == "NonResonant_Bounceable")
					{
						return TagType.NonResonant_Bounceable;
					}
				}
				else if (value == "Room")
				{
					return TagType.Room;
				}
			}
			else if (num <= 3643300336U)
			{
				if (num <= 3491759071U)
				{
					if (num != 3185177229U)
					{
						if (num == 3491759071U)
						{
							if (value == "Respawn")
							{
								return TagType.Respawn;
							}
						}
					}
					else if (value == "Hazard")
					{
						return TagType.Hazard;
					}
				}
				else if (num != 3537891049U)
				{
					if (num != 3605377383U)
					{
						if (num == 3643300336U)
						{
							if (value == "LevelEditor")
							{
								return TagType.LevelEditor;
							}
						}
					}
					else if (value == "MainCamera")
					{
						return TagType.MainCamera;
					}
				}
				else if (value == "EditorProp")
				{
					return TagType.EditorProp;
				}
			}
			else if (num <= 4057623701U)
			{
				if (num != 4024307236U)
				{
					if (num != 4050331055U)
					{
						if (num == 4057623701U)
						{
							if (value == "Cutout")
							{
								return TagType.Cutout;
							}
						}
					}
					else if (value == "PlayerStatusEffect")
					{
						return TagType.PlayerStatusEffect;
					}
				}
				else if (value == "FlimsyBreakable")
				{
					return TagType.FlimsyBreakable;
				}
			}
			else if (num != 4150194096U)
			{
				if (num != 4158600414U)
				{
					if (num == 4194203456U)
					{
						if (value == "LevelBounds")
						{
							return TagType.LevelBounds;
						}
					}
				}
				else if (value == "Untagged")
				{
					return TagType.Untagged;
				}
			}
			else if (value == "SpawnController")
			{
				return TagType.SpawnController;
			}
		}
		throw new Exception("Tag: " + value + " does not exist in Tag manager.");
	}

	// Token: 0x06005AD8 RID: 23256 RVA: 0x00158614 File Offset: 0x00156814
	public static string ToString(TagType value)
	{
		switch (value)
		{
		case TagType.Untagged:
			return "Untagged";
		case TagType.Respawn:
			return "Respawn";
		case TagType.Finish:
			return "Finish";
		case TagType.EditorOnly:
			return "EditorOnly";
		case TagType.MainCamera:
			return "MainCamera";
		case TagType.Player:
			return "Player";
		case TagType.GameController:
			return "GameController";
		case TagType.LevelBounds:
			return "LevelBounds";
		case TagType.Music:
			return "Music";
		case TagType.Background:
			return "Background";
		case TagType.Platform:
			return "Platform";
		case TagType.Door:
			return "Door";
		case TagType.Room:
			return "Room";
		case TagType.OneWay:
			return "OneWay";
		case TagType.Enemy:
			return "Enemy";
		case TagType.Breakable:
			return "Breakable";
		case TagType.Projectile:
			return "Projectile";
		case TagType.ItemDrop:
			return "ItemDrop";
		case TagType.Hazard:
			return "Hazard";
		case TagType.PlayerProjectile:
			return "PlayerProjectile";
		case TagType.EnemyProjectile:
			return "EnemyProjectile";
		case TagType.Chest:
			return "Chest";
		case TagType.EquipmentButton:
			return "EquipmentButton";
		case TagType.Prop:
			return "Prop";
		case TagType.EditorProp:
			return "EditorProp";
		case TagType.LevelEditor:
			return "LevelEditor";
		case TagType.Cutout:
			return "Cutout";
		case TagType.Tunnel:
			return "Tunnel";
		case TagType.NPC:
			return "NPC";
		case TagType.Barricade:
			return "Barricade";
		case TagType.TriggerHazard:
			return "TriggerHazard";
		case TagType.Effect:
			return "Effect";
		case TagType.Player_Dodging:
			return "Player_Dodging";
		case TagType.FlimsyBreakable:
			return "FlimsyBreakable";
		case TagType.SpawnController:
			return "SpawnController";
		case TagType.TeleporterTube:
			return "TeleporterTube";
		case TagType.Generic_Bounceable:
			return "Generic_Bounceable";
		case TagType.MagicPlatform:
			return "MagicPlatform";
		case TagType.PlayerStatusEffect:
			return "PlayerStatusEffect";
		case TagType.EnemyStatusEffect:
			return "EnemyStatusEffect";
		case TagType.NonResonant_Bounceable:
			return "NonResonant_Bounceable";
		default:
			throw new Exception("Tag: " + value.ToString() + " does not exist in Tag manager.");
		}
	}

	// Token: 0x04004AED RID: 19181
	public const string Untagged = "Untagged";

	// Token: 0x04004AEE RID: 19182
	public const string Respawn = "Respawn";

	// Token: 0x04004AEF RID: 19183
	public const string Finish = "Finish";

	// Token: 0x04004AF0 RID: 19184
	public const string EditorOnly = "EditorOnly";

	// Token: 0x04004AF1 RID: 19185
	public const string MainCamera = "MainCamera";

	// Token: 0x04004AF2 RID: 19186
	public const string Player = "Player";

	// Token: 0x04004AF3 RID: 19187
	public const string GameController = "GameController";

	// Token: 0x04004AF4 RID: 19188
	public const string LevelBounds = "LevelBounds";

	// Token: 0x04004AF5 RID: 19189
	public const string Music = "Music";

	// Token: 0x04004AF6 RID: 19190
	public const string Background = "Background";

	// Token: 0x04004AF7 RID: 19191
	public const string Platform = "Platform";

	// Token: 0x04004AF8 RID: 19192
	public const string Door = "Door";

	// Token: 0x04004AF9 RID: 19193
	public const string Room = "Room";

	// Token: 0x04004AFA RID: 19194
	public const string OneWay = "OneWay";

	// Token: 0x04004AFB RID: 19195
	public const string Enemy = "Enemy";

	// Token: 0x04004AFC RID: 19196
	public const string Breakable = "Breakable";

	// Token: 0x04004AFD RID: 19197
	public const string Projectile = "Projectile";

	// Token: 0x04004AFE RID: 19198
	public const string ItemDrop = "ItemDrop";

	// Token: 0x04004AFF RID: 19199
	public const string Hazard = "Hazard";

	// Token: 0x04004B00 RID: 19200
	public const string PlayerProjectile = "PlayerProjectile";

	// Token: 0x04004B01 RID: 19201
	public const string EnemyProjectile = "EnemyProjectile";

	// Token: 0x04004B02 RID: 19202
	public const string Chest = "Chest";

	// Token: 0x04004B03 RID: 19203
	public const string EquipmentButton = "EquipmentButton";

	// Token: 0x04004B04 RID: 19204
	public const string Prop = "Prop";

	// Token: 0x04004B05 RID: 19205
	public const string EditorProp = "EditorProp";

	// Token: 0x04004B06 RID: 19206
	public const string LevelEditor = "LevelEditor";

	// Token: 0x04004B07 RID: 19207
	public const string Cutout = "Cutout";

	// Token: 0x04004B08 RID: 19208
	public const string Tunnel = "Tunnel";

	// Token: 0x04004B09 RID: 19209
	public const string NPC = "NPC";

	// Token: 0x04004B0A RID: 19210
	public const string Barricade = "Barricade";

	// Token: 0x04004B0B RID: 19211
	public const string TriggerHazard = "TriggerHazard";

	// Token: 0x04004B0C RID: 19212
	public const string Effect = "Effect";

	// Token: 0x04004B0D RID: 19213
	public const string Player_Dodging = "Player_Dodging";

	// Token: 0x04004B0E RID: 19214
	public const string FlimsyBreakable = "FlimsyBreakable";

	// Token: 0x04004B0F RID: 19215
	public const string SpawnController = "SpawnController";

	// Token: 0x04004B10 RID: 19216
	public const string TeleporterTube = "TeleporterTube";

	// Token: 0x04004B11 RID: 19217
	public const string Generic_Bounceable = "Generic_Bounceable";

	// Token: 0x04004B12 RID: 19218
	public const string MagicPlatform = "MagicPlatform";

	// Token: 0x04004B13 RID: 19219
	public const string PlayerStatusEffect = "PlayerStatusEffect";

	// Token: 0x04004B14 RID: 19220
	public const string EnemyStatusEffect = "EnemyStatusEffect";

	// Token: 0x04004B15 RID: 19221
	public const string NonResonant_Bounceable = "NonResonant_Bounceable";
}

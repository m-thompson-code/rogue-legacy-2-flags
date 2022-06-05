using System;

// Token: 0x02000784 RID: 1924
public static class TagType_RL
{
	// Token: 0x0600415A RID: 16730 RVA: 0x000E8B14 File Offset: 0x000E6D14
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

	// Token: 0x0600415B RID: 16731 RVA: 0x000E918C File Offset: 0x000E738C
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

	// Token: 0x0400383D RID: 14397
	public const string Untagged = "Untagged";

	// Token: 0x0400383E RID: 14398
	public const string Respawn = "Respawn";

	// Token: 0x0400383F RID: 14399
	public const string Finish = "Finish";

	// Token: 0x04003840 RID: 14400
	public const string EditorOnly = "EditorOnly";

	// Token: 0x04003841 RID: 14401
	public const string MainCamera = "MainCamera";

	// Token: 0x04003842 RID: 14402
	public const string Player = "Player";

	// Token: 0x04003843 RID: 14403
	public const string GameController = "GameController";

	// Token: 0x04003844 RID: 14404
	public const string LevelBounds = "LevelBounds";

	// Token: 0x04003845 RID: 14405
	public const string Music = "Music";

	// Token: 0x04003846 RID: 14406
	public const string Background = "Background";

	// Token: 0x04003847 RID: 14407
	public const string Platform = "Platform";

	// Token: 0x04003848 RID: 14408
	public const string Door = "Door";

	// Token: 0x04003849 RID: 14409
	public const string Room = "Room";

	// Token: 0x0400384A RID: 14410
	public const string OneWay = "OneWay";

	// Token: 0x0400384B RID: 14411
	public const string Enemy = "Enemy";

	// Token: 0x0400384C RID: 14412
	public const string Breakable = "Breakable";

	// Token: 0x0400384D RID: 14413
	public const string Projectile = "Projectile";

	// Token: 0x0400384E RID: 14414
	public const string ItemDrop = "ItemDrop";

	// Token: 0x0400384F RID: 14415
	public const string Hazard = "Hazard";

	// Token: 0x04003850 RID: 14416
	public const string PlayerProjectile = "PlayerProjectile";

	// Token: 0x04003851 RID: 14417
	public const string EnemyProjectile = "EnemyProjectile";

	// Token: 0x04003852 RID: 14418
	public const string Chest = "Chest";

	// Token: 0x04003853 RID: 14419
	public const string EquipmentButton = "EquipmentButton";

	// Token: 0x04003854 RID: 14420
	public const string Prop = "Prop";

	// Token: 0x04003855 RID: 14421
	public const string EditorProp = "EditorProp";

	// Token: 0x04003856 RID: 14422
	public const string LevelEditor = "LevelEditor";

	// Token: 0x04003857 RID: 14423
	public const string Cutout = "Cutout";

	// Token: 0x04003858 RID: 14424
	public const string Tunnel = "Tunnel";

	// Token: 0x04003859 RID: 14425
	public const string NPC = "NPC";

	// Token: 0x0400385A RID: 14426
	public const string Barricade = "Barricade";

	// Token: 0x0400385B RID: 14427
	public const string TriggerHazard = "TriggerHazard";

	// Token: 0x0400385C RID: 14428
	public const string Effect = "Effect";

	// Token: 0x0400385D RID: 14429
	public const string Player_Dodging = "Player_Dodging";

	// Token: 0x0400385E RID: 14430
	public const string FlimsyBreakable = "FlimsyBreakable";

	// Token: 0x0400385F RID: 14431
	public const string SpawnController = "SpawnController";

	// Token: 0x04003860 RID: 14432
	public const string TeleporterTube = "TeleporterTube";

	// Token: 0x04003861 RID: 14433
	public const string Generic_Bounceable = "Generic_Bounceable";

	// Token: 0x04003862 RID: 14434
	public const string MagicPlatform = "MagicPlatform";

	// Token: 0x04003863 RID: 14435
	public const string PlayerStatusEffect = "PlayerStatusEffect";

	// Token: 0x04003864 RID: 14436
	public const string EnemyStatusEffect = "EnemyStatusEffect";

	// Token: 0x04003865 RID: 14437
	public const string NonResonant_Bounceable = "NonResonant_Bounceable";
}

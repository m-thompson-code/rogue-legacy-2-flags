using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004A RID: 74
public static class BiomeCreation_EV
{
	// Token: 0x06000139 RID: 313 RVA: 0x0000B7DC File Offset: 0x000099DC
	public static int GetDefaultSpecialRoomWeight(SpecialRoomType specialRoomType)
	{
		if (specialRoomType <= SpecialRoomType.Relic)
		{
			if (specialRoomType <= SpecialRoomType.Journal)
			{
				if (specialRoomType == SpecialRoomType.Clown)
				{
					return 0;
				}
				if (specialRoomType == SpecialRoomType.Journal)
				{
					return 0;
				}
			}
			else
			{
				if (specialRoomType == SpecialRoomType.Healing)
				{
					return 25;
				}
				if (specialRoomType == SpecialRoomType.Relic)
				{
					return 30;
				}
			}
		}
		else if (specialRoomType <= SpecialRoomType.Teleporter)
		{
			if (specialRoomType == SpecialRoomType.Portrait)
			{
				return 0;
			}
			if (specialRoomType == SpecialRoomType.Teleporter)
			{
				return 0;
			}
		}
		else
		{
			if (specialRoomType == SpecialRoomType.Arena)
			{
				return 20;
			}
			if (specialRoomType == SpecialRoomType.Curio)
			{
				return 25;
			}
		}
		return 0;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000B840 File Offset: 0x00009A40
	public static bool GetDuplicateRoomsAllowed(BiomeType biome)
	{
		bool result = false;
		if (BiomeCreation_EV.DUPLICATE_ROOMS_ALLOWED.ContainsKey(biome))
		{
			result = BiomeCreation_EV.DUPLICATE_ROOMS_ALLOWED[biome];
		}
		return result;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000B86C File Offset: 0x00009A6C
	public static int GetBiomeLevel(BiomeType biomeType)
	{
		if (biomeType <= BiomeType.Forest)
		{
			if (biomeType == BiomeType.Castle)
			{
				return 1;
			}
			if (biomeType == BiomeType.Cave)
			{
				return 6;
			}
			if (biomeType == BiomeType.Forest)
			{
				return 3;
			}
		}
		else if (biomeType <= BiomeType.Stone)
		{
			if (biomeType == BiomeType.Garden)
			{
				return -1;
			}
			if (biomeType == BiomeType.Stone)
			{
				return 2;
			}
		}
		else
		{
			if (biomeType == BiomeType.Study)
			{
				return 4;
			}
			if (biomeType == BiomeType.Tower)
			{
				return 5;
			}
		}
		return 0;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000B8C0 File Offset: 0x00009AC0
	public static Dictionary<RoomSide, int> GetBiomeBorderOffsets(BiomeType biome)
	{
		if (biome <= BiomeType.Lake)
		{
			if (biome <= BiomeType.Cave)
			{
				if (biome == BiomeType.Castle)
				{
					return BiomeCreation_EV.CASTLE_BOUNDS;
				}
				if (biome == BiomeType.Cave)
				{
					return BiomeCreation_EV.CAVE_BOUNDS;
				}
			}
			else
			{
				if (biome == BiomeType.Dragon)
				{
					return BiomeCreation_EV.DRAGON_BOUNDS;
				}
				if (biome == BiomeType.Forest)
				{
					return BiomeCreation_EV.FOREST_BOUNDS;
				}
				if (biome == BiomeType.Lake)
				{
					return BiomeCreation_EV.LAKE_BOUNDS;
				}
			}
		}
		else if (biome <= BiomeType.Study)
		{
			if (biome == BiomeType.Stone)
			{
				return BiomeCreation_EV.STONE_BOUNDS;
			}
			if (biome == BiomeType.Study)
			{
				return BiomeCreation_EV.STUDY_BOUNDS;
			}
		}
		else
		{
			if (biome == BiomeType.Sunken)
			{
				return BiomeCreation_EV.SUNKEN_BOUNDS;
			}
			if (biome == BiomeType.Tower)
			{
				return BiomeCreation_EV.TOWER_BOUNDS;
			}
			if (biome == BiomeType.Town)
			{
				return BiomeCreation_EV.TOWN_BOUNDS;
			}
		}
		return BiomeCreation_EV.DEFAULT_BOUNDS;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000B960 File Offset: 0x00009B60
	public static List<RoomSide> GetSidesToReplace(BiomeType biome)
	{
		if (BiomeCreation_EV.REPLACE_SIDES_TABLE.ContainsKey(biome))
		{
			return BiomeCreation_EV.REPLACE_SIDES_TABLE[biome];
		}
		return new List<RoomSide>
		{
			RoomSide.Left,
			RoomSide.Right,
			RoomSide.Top,
			RoomSide.Bottom
		};
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000B99C File Offset: 0x00009B9C
	public static int GetNumberOfRoomsToLookBack(BiomeType biome, int currentRoomCount, int totalTargetRoomCount)
	{
		if (BiomeCreation_EV.TRACE_DEPTH_TABLE.ContainsKey(biome))
		{
			return Mathf.Min(Mathf.RoundToInt(BiomeCreation_EV.TRACE_DEPTH_TABLE[biome] * (float)totalTargetRoomCount), currentRoomCount);
		}
		return currentRoomCount;
	}

	// Token: 0x04000213 RID: 531
	public static int DEFAULT_ROOM_RNG_WEIGHT = 100;

	// Token: 0x04000214 RID: 532
	public const int DEFAULT_HEALING_ROOM_SPAWN_WEIGHT = 25;

	// Token: 0x04000215 RID: 533
	public const int DEFAULT_RELIC_ROOM_SPAWN_WEIGHT = 30;

	// Token: 0x04000216 RID: 534
	public const int DEFAULT_ARENA_ROOM_SPAWN_WEIGHT = 20;

	// Token: 0x04000217 RID: 535
	public const int DEFAULT_CURIO_ROOM_SPAWN_WEIGHT = 25;

	// Token: 0x04000218 RID: 536
	public const int DEFAULT_TELEPORTER_ROOM_SPAWN_WEIGHT = 0;

	// Token: 0x04000219 RID: 537
	public const int DEFAULT_PORTRAIT_ROOM_SPAWN_WEIGHT = 0;

	// Token: 0x0400021A RID: 538
	public const int DEFAULT_CLOWN_ROOM_SPAWN_WEIGHT = 0;

	// Token: 0x0400021B RID: 539
	public const int DEFAULT_JOURNAL_ROOM_SPAWN_WEIGHT = 0;

	// Token: 0x0400021C RID: 540
	public const bool USE_RANDOM_STANDARD_ROOM_POOL_CULLING = false;

	// Token: 0x0400021D RID: 541
	public const int WORLD_BUILD_TIMESLICE_THRESHOLD = 30;

	// Token: 0x0400021E RID: 542
	public const bool FORCE_ROOM_WEIGHTS_TO_DEFAULT = false;

	// Token: 0x0400021F RID: 543
	public const bool VALIDATE_BIOME_AFTER_BUILD = false;

	// Token: 0x04000220 RID: 544
	public const bool TEST_MEGA_BIOMES = false;

	// Token: 0x04000221 RID: 545
	public const float MEGA_BIOME_SIZE_MULTIPLIER = 3f;

	// Token: 0x04000222 RID: 546
	public static Dictionary<BiomeType, List<BiomeTag>> BIOME_TAGS = new Dictionary<BiomeType, List<BiomeTag>>
	{
		{
			BiomeType.Castle,
			new List<BiomeTag>
			{
				BiomeTag.Interior,
				BiomeTag.Player_Dash_FALSE,
				BiomeTag.Player_DoubleJump_FALSE
			}
		},
		{
			BiomeType.Stone,
			new List<BiomeTag>
			{
				BiomeTag.Exterior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_FALSE
			}
		},
		{
			BiomeType.Forest,
			new List<BiomeTag>
			{
				BiomeTag.Exterior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_FALSE
			}
		},
		{
			BiomeType.ForestBottom,
			new List<BiomeTag>
			{
				BiomeTag.Exterior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_FALSE
			}
		},
		{
			BiomeType.ForestTop,
			new List<BiomeTag>
			{
				BiomeTag.Exterior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_FALSE
			}
		},
		{
			BiomeType.Study,
			new List<BiomeTag>
			{
				BiomeTag.Interior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_TRUE
			}
		},
		{
			BiomeType.Tower,
			new List<BiomeTag>
			{
				BiomeTag.Interior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_TRUE
			}
		},
		{
			BiomeType.TowerExterior,
			new List<BiomeTag>
			{
				BiomeTag.Exterior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_TRUE
			}
		},
		{
			BiomeType.Cave,
			new List<BiomeTag>
			{
				BiomeTag.Exterior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_TRUE
			}
		},
		{
			BiomeType.CaveMiddle,
			new List<BiomeTag>
			{
				BiomeTag.Exterior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_TRUE
			}
		},
		{
			BiomeType.CaveBottom,
			new List<BiomeTag>
			{
				BiomeTag.Exterior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_TRUE
			}
		},
		{
			BiomeType.Dragon,
			new List<BiomeTag>
			{
				BiomeTag.Interior,
				BiomeTag.Player_Dash_TRUE,
				BiomeTag.Player_DoubleJump_TRUE
			}
		}
	};

	// Token: 0x04000223 RID: 547
	public static Dictionary<BiomeType, BiomeType> TUNNEL_DESTINATION_BIOME_TYPE_TABLE = new Dictionary<BiomeType, BiomeType>
	{
		{
			BiomeType.Stone,
			BiomeType.Sunken
		}
	};

	// Token: 0x04000224 RID: 548
	public static Dictionary<BiomeType, bool> IS_IT_RAINING_IN_BIOME_TABLE = new Dictionary<BiomeType, bool>
	{
		{
			BiomeType.Stone,
			true
		}
	};

	// Token: 0x04000225 RID: 549
	private static Dictionary<BiomeType, bool> DUPLICATE_ROOMS_ALLOWED = new Dictionary<BiomeType, bool>();

	// Token: 0x04000226 RID: 550
	public static Dictionary<BiomeType, int> FORCE_EASY_ROOMS_AT_BIOME_START = new Dictionary<BiomeType, int>
	{
		{
			BiomeType.Castle,
			2
		}
	};

	// Token: 0x04000227 RID: 551
	public static Dictionary<BiomeType, int> REQUEST_EASY_FAIRY_ROOMS_IN_BIOME = new Dictionary<BiomeType, int>
	{
		{
			BiomeType.Castle,
			1
		},
		{
			BiomeType.Forest,
			1
		}
	};

	// Token: 0x04000228 RID: 552
	public const bool DONT_CREATE_MIRRORED_VERSION_OF_ROOMS_WITH_MAX_NUMBER_DOORS = false;

	// Token: 0x04000229 RID: 553
	public const bool DONT_CREATE_MIRRORED_VERSION_OF_1X1_ROOMS_WITH_DOORS_ON_LEFT_AND_RIGHT = false;

	// Token: 0x0400022A RID: 554
	public const bool DONT_CREATE_MIRRORED_VERSION_OF_1XN_ROOMS_WITH_THE_SAME_DOORS_ON_LEFT_AND_RIGHT = false;

	// Token: 0x0400022B RID: 555
	public static bool USE_DEFAULT_MERGE_LOGIC_FOR_TOWER_INTERIOR = true;

	// Token: 0x0400022C RID: 556
	public const bool MERGE_TOWER_INTERIOR = true;

	// Token: 0x0400022D RID: 557
	public const bool MERGE_TOWER_EXTERIOR = true;

	// Token: 0x0400022E RID: 558
	public const bool MERGE_TOWER_TOP = true;

	// Token: 0x0400022F RID: 559
	public const bool MERGE_DRAGON = true;

	// Token: 0x04000230 RID: 560
	public const float NIBIRU_DEEP_WATER_RAISE_PER_BOSS_KILLED = 2.5f;

	// Token: 0x04000231 RID: 561
	public static List<BiomeType> DISABLE_HORIZONTAL_FILL_IN_BIOMES = new List<BiomeType>
	{
		BiomeType.Stone
	};

	// Token: 0x04000232 RID: 562
	public static List<BiomeType> DISABLE_VERTICAL_FILL_IN_BIOMES = new List<BiomeType>
	{
		BiomeType.TowerExterior
	};

	// Token: 0x04000233 RID: 563
	public static List<BiomeType> DO_NOT_ADD_ONE_WAYS_TO_BOTTOM_DOORS = new List<BiomeType>
	{
		BiomeType.TowerExterior,
		BiomeType.Dragon
	};

	// Token: 0x04000234 RID: 564
	private static Dictionary<RoomSide, int> CASTLE_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			2
		},
		{
			RoomSide.Right,
			30
		},
		{
			RoomSide.Top,
			30
		},
		{
			RoomSide.Bottom,
			-30
		}
	};

	// Token: 0x04000235 RID: 565
	private static Dictionary<RoomSide, int> TOWN_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			-10
		},
		{
			RoomSide.Right,
			100
		},
		{
			RoomSide.Top,
			7
		},
		{
			RoomSide.Bottom,
			-7
		}
	};

	// Token: 0x04000236 RID: 566
	private static Dictionary<RoomSide, int> DRAGON_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			-10
		},
		{
			RoomSide.Right,
			100
		},
		{
			RoomSide.Top,
			0
		},
		{
			RoomSide.Bottom,
			-100
		}
	};

	// Token: 0x04000237 RID: 567
	private static Dictionary<RoomSide, int> FOREST_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			0
		},
		{
			RoomSide.Right,
			50
		},
		{
			RoomSide.Top,
			10
		},
		{
			RoomSide.Bottom,
			-10
		}
	};

	// Token: 0x04000238 RID: 568
	private static Dictionary<RoomSide, int> STONE_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			0
		},
		{
			RoomSide.Right,
			200
		},
		{
			RoomSide.Top,
			1
		},
		{
			RoomSide.Bottom,
			0
		}
	};

	// Token: 0x04000239 RID: 569
	private static Dictionary<RoomSide, int> LAKE_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			-100
		},
		{
			RoomSide.Right,
			100
		},
		{
			RoomSide.Top,
			100
		},
		{
			RoomSide.Bottom,
			-100
		}
	};

	// Token: 0x0400023A RID: 570
	private static Dictionary<RoomSide, int> STUDY_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			-7
		},
		{
			RoomSide.Right,
			7
		},
		{
			RoomSide.Top,
			50
		},
		{
			RoomSide.Bottom,
			2
		}
	};

	// Token: 0x0400023B RID: 571
	private static Dictionary<RoomSide, int> TOWER_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			-10
		},
		{
			RoomSide.Right,
			10
		},
		{
			RoomSide.Top,
			250
		},
		{
			RoomSide.Bottom,
			0
		}
	};

	// Token: 0x0400023C RID: 572
	private static Dictionary<RoomSide, int> CAVE_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			-50
		},
		{
			RoomSide.Right,
			100
		},
		{
			RoomSide.Top,
			-8
		},
		{
			RoomSide.Bottom,
			-30
		}
	};

	// Token: 0x0400023D RID: 573
	private static Dictionary<RoomSide, int> SUNKEN_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			2
		},
		{
			RoomSide.Right,
			30
		},
		{
			RoomSide.Top,
			0
		},
		{
			RoomSide.Bottom,
			-35
		}
	};

	// Token: 0x0400023E RID: 574
	private static Dictionary<RoomSide, int> DEFAULT_BOUNDS = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			-25
		},
		{
			RoomSide.Right,
			25
		},
		{
			RoomSide.Top,
			25
		},
		{
			RoomSide.Bottom,
			-25
		}
	};

	// Token: 0x0400023F RID: 575
	private static Dictionary<BiomeType, float> TRACE_DEPTH_TABLE = new Dictionary<BiomeType, float>
	{
		{
			BiomeType.Study,
			0.5f
		},
		{
			BiomeType.Tower,
			0.5f
		}
	};

	// Token: 0x04000240 RID: 576
	private static Dictionary<BiomeType, List<RoomSide>> REPLACE_SIDES_TABLE = new Dictionary<BiomeType, List<RoomSide>>
	{
		{
			BiomeType.Tower,
			new List<RoomSide>
			{
				RoomSide.Top
			}
		}
	};
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000AB1 RID: 2737
public class CreateRoomsBuildRule_Tower : CreateRoomsBuildRule
{
	// Token: 0x17001C32 RID: 7218
	// (get) Token: 0x0600527C RID: 21116 RVA: 0x00017799 File Offset: 0x00015999
	private BiomeType Tier01Biome
	{
		get
		{
			return BiomeType.Tower;
		}
	}

	// Token: 0x17001C33 RID: 7219
	// (get) Token: 0x0600527D RID: 21117 RVA: 0x0002CE1C File Offset: 0x0002B01C
	private BiomeType Tier02Biome
	{
		get
		{
			return BiomeType.TowerExterior;
		}
	}

	// Token: 0x17001C34 RID: 7220
	// (get) Token: 0x0600527E RID: 21118 RVA: 0x0002CE1C File Offset: 0x0002B01C
	private BiomeType Tier03Biome
	{
		get
		{
			return BiomeType.TowerExterior;
		}
	}

	// Token: 0x17001C35 RID: 7221
	// (get) Token: 0x0600527F RID: 21119 RVA: 0x0002CE23 File Offset: 0x0002B023
	private Dictionary<RoomSide, int> Tier01Border
	{
		get
		{
			return new Dictionary<RoomSide, int>
			{
				{
					RoomSide.Left,
					1
				},
				{
					RoomSide.Right,
					-1
				},
				{
					RoomSide.Top,
					500
				},
				{
					RoomSide.Bottom,
					4
				}
			};
		}
	}

	// Token: 0x17001C36 RID: 7222
	// (get) Token: 0x06005280 RID: 21120 RVA: 0x0002CE4E File Offset: 0x0002B04E
	private Dictionary<RoomSide, int> Tier02Border
	{
		get
		{
			return new Dictionary<RoomSide, int>
			{
				{
					RoomSide.Left,
					-1
				},
				{
					RoomSide.Right,
					-3
				},
				{
					RoomSide.Top,
					500
				},
				{
					RoomSide.Bottom,
					4
				}
			};
		}
	}

	// Token: 0x17001C37 RID: 7223
	// (get) Token: 0x06005281 RID: 21121 RVA: 0x0002CE7A File Offset: 0x0002B07A
	private Dictionary<RoomSide, int> Tier03Border
	{
		get
		{
			return new Dictionary<RoomSide, int>
			{
				{
					RoomSide.Left,
					3
				},
				{
					RoomSide.Right,
					1
				},
				{
					RoomSide.Top,
					500
				},
				{
					RoomSide.Bottom,
					4
				}
			};
		}
	}

	// Token: 0x06005282 RID: 21122 RVA: 0x0002CEA5 File Offset: 0x0002B0A5
	public override IEnumerator CreateRooms(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		this.CreateBorders(biomeController);
		yield return base.CreateRooms(biomeCreator, biomeController);
		this.CreateTopOfTower(biomeCreator, biomeController);
		this.CreateJournalRoom(biomeCreator, biomeController);
		yield break;
	}

	// Token: 0x06005283 RID: 21123 RVA: 0x00139704 File Offset: 0x00137904
	protected override void BuildRoomsAtRandomDoorLocations(BiomeCreator biomeCreator, BiomeController biomeController, int finalRoomCountInBiome, GridPointManager originRoom, List<RoomSetEntry> standardRoomPoolOverride)
	{
		foreach (DoorLocation doorLocation in biomeCreator.GetBuildRoomAtDoorLocations(biomeController, originRoom))
		{
			if (CreateRoomsBuildRule.GetNumberOfRoomsPlacedSoFar(biomeController) >= finalRoomCountInBiome)
			{
				break;
			}
			if (biomeCreator.State == BiomeBuildStateID.Failed)
			{
				break;
			}
			if (GridController.GetIsGridSpaceAvailable(originRoom.GetDoorLeadsToGridCoordinates(doorLocation)))
			{
				Vector2Int doorLeadsToGridCoordinates = originRoom.GetDoorLeadsToGridCoordinates(doorLocation);
				TierID tier = this.GetTier(doorLeadsToGridCoordinates);
				if (tier != TierID.None)
				{
					BiomeType biome = this.GetBiome(tier);
					Bounds border = this.GetBorder(tier);
					RoomTypeEntry targetRoomRequirements = biomeCreator.GetTargetRoomRequirements();
					Dictionary<Vector2Int, List<DoorLocation>> roomSizesAndWhatDoorLocationsTheyFitAt = biomeCreator.GetRoomSizesAndWhatDoorLocationsTheyFitAt(border, RoomLibrary.GetSetCollection(biome).MaxRoomSize, originRoom, doorLocation);
					RoomType roomType;
					RoomSetEntry[] potentialRooms = this.GetPotentialRooms(biomeCreator, biomeController, roomSizesAndWhatDoorLocationsTheyFitAt, targetRoomRequirements, standardRoomPoolOverride, out roomType, biome, tier);
					if (potentialRooms.Length != 0)
					{
						CreateRoomsBuildRule.BuildRoom(biomeCreator, biomeController, originRoom, doorLocation, roomSizesAndWhatDoorLocationsTheyFitAt, roomType, potentialRooms, biome);
					}
					else
					{
						biomeCreator.AddRoomTypeToBacklog(targetRoomRequirements);
						base.PrintNoPotentialRoomsErrorMessage(biome, roomSizesAndWhatDoorLocationsTheyFitAt);
					}
				}
			}
		}
	}

	// Token: 0x06005284 RID: 21124 RVA: 0x00139810 File Offset: 0x00137A10
	private void CreateBorders(BiomeController biomeController)
	{
		GridPointManager transitionRoom = biomeController.GridPointManager.GridPointManagers.First<GridPointManager>();
		this.m_defaultBorder = biomeController.GridPointManager.Border;
		this.m_tier1Border = this.GetBounds(this.Tier01Border, transitionRoom);
		this.m_tier2Border = this.GetBounds(this.Tier02Border, transitionRoom);
		this.m_tier3Border = this.GetBounds(this.Tier03Border, transitionRoom);
	}

	// Token: 0x06005285 RID: 21125 RVA: 0x00139878 File Offset: 0x00137A78
	protected Bounds GetBounds(Dictionary<RoomSide, int> border, GridPointManager transitionRoom)
	{
		int num = transitionRoom.GridCoordinates.x + border[RoomSide.Left];
		int num2 = transitionRoom.GridCoordinates.y + border[RoomSide.Bottom];
		int num3 = transitionRoom.GridCoordinates.x + transitionRoom.Size.x + border[RoomSide.Right];
		int num4 = transitionRoom.GridCoordinates.y + transitionRoom.Size.y + border[RoomSide.Top];
		Bounds result = default(Bounds);
		result.SetMinMax(new Vector2((float)num, (float)num2), new Vector2((float)num3, (float)num4));
		return result;
	}

	// Token: 0x06005286 RID: 21126 RVA: 0x00139934 File Offset: 0x00137B34
	protected BiomeType GetBiome(TierID tier)
	{
		BiomeType result = this.Tier01Biome;
		if (tier == TierID.Two)
		{
			result = this.Tier02Biome;
		}
		else if (tier == TierID.Three)
		{
			result = this.Tier03Biome;
		}
		return result;
	}

	// Token: 0x06005287 RID: 21127 RVA: 0x00139964 File Offset: 0x00137B64
	protected Bounds GetBorder(TierID tier)
	{
		Bounds result = this.m_tier1Border;
		if (tier == TierID.Two)
		{
			result = this.m_tier2Border;
		}
		else if (tier == TierID.Three)
		{
			result = this.m_tier3Border;
		}
		return result;
	}

	// Token: 0x06005288 RID: 21128 RVA: 0x00139994 File Offset: 0x00137B94
	private bool GetAreCoordinatesWithinBorder(Vector2Int coordinates, Bounds border)
	{
		return (float)coordinates.x < border.max.x && (float)coordinates.y < border.max.y && border.Contains(coordinates);
	}

	// Token: 0x06005289 RID: 21129 RVA: 0x0002CEC2 File Offset: 0x0002B0C2
	public TierID GetTier(Vector2Int coords)
	{
		if (this.GetAreCoordinatesWithinBorder(coords, this.m_tier1Border))
		{
			return TierID.One;
		}
		if (this.GetAreCoordinatesWithinBorder(coords, this.m_tier2Border))
		{
			return TierID.Two;
		}
		if (this.GetAreCoordinatesWithinBorder(coords, this.m_tier3Border))
		{
			return TierID.Three;
		}
		return TierID.None;
	}

	// Token: 0x0600528A RID: 21130 RVA: 0x001399E4 File Offset: 0x00137BE4
	private void CreateTopOfTower(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		CreateRoomsBuildRule_Tower.ReplaceRoomAtTopIfNecessary(biomeCreator, biomeController);
		int num = (int)this.m_tier1Border.min.x;
		int y = biomeController.GridPointManager.Extents[RoomSide.Top];
		RoomMetaData roomMetaData = RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntranceCentre_Tower);
		base.CreateSpecificRoom(biomeCreator, biomeController, roomMetaData, new Vector2Int(num, y), new DoorLocation(RoomSide.Bottom, 0), BiomeType.TowerExterior, RoomType.BossEntrance, true);
		RoomMetaData roomMetaData2 = RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntranceLeft_Tower);
		base.CreateSpecificRoom(biomeCreator, biomeController, roomMetaData2, new Vector2Int(num - roomMetaData2.Size.x, y), new DoorLocation(RoomSide.Bottom, 0), BiomeType.TowerExterior, RoomType.BossEntrance, true);
		RoomMetaData roomMetaData3 = RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntranceRight_Tower);
		base.CreateSpecificRoom(biomeCreator, biomeController, roomMetaData3, new Vector2Int(num + roomMetaData3.Size.x, y), new DoorLocation(RoomSide.Bottom, 0), BiomeType.TowerExterior, RoomType.BossEntrance, true);
	}

	// Token: 0x0600528B RID: 21131 RVA: 0x00139AC0 File Offset: 0x00137CC0
	private void CreateJournalRoom(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		bool flag = false;
		DoorLocation doorLocation = default(DoorLocation);
		Vector2Int coords = default(Vector2Int);
		List<GridPointManager> list = new List<GridPointManager>(biomeController.GridPointManager.GridPointManagers);
		RoomUtility.ShuffleList<GridPointManager>(list);
		foreach (GridPointManager gridPointManager in list)
		{
			foreach (DoorLocation doorLocation2 in gridPointManager.DoorLocations)
			{
				Vector2Int doorLeadsToGridCoordinates = gridPointManager.GetDoorLeadsToGridCoordinates(doorLocation2);
				if (this.GetTier(doorLeadsToGridCoordinates) == TierID.One && biomeCreator.GetIsDoorLocationValid(biomeController, gridPointManager, doorLocation2))
				{
					flag = true;
					doorLocation = new DoorLocation(RoomUtility.GetOppositeSide(doorLocation2.RoomSide), 0);
					coords = doorLeadsToGridCoordinates;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (flag)
		{
			base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.Journal_Tower), coords, doorLocation, BiomeType.None, RoomType.None, true);
			return;
		}
		string text = "<color=red>| CreateRoomsBuildRule_Tower | The Tower Interior had no available spaces in which to put the Journal room.</color>";
		Debug.Log(text);
		biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text);
	}

	// Token: 0x0600528C RID: 21132 RVA: 0x00139BE8 File Offset: 0x00137DE8
	private RoomSetEntry[] GetPotentialRooms(BiomeCreator biomeCreator, BiomeController biomeController, Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit, RoomTypeEntry roomRequirements, List<RoomSetEntry> standardRoomPool, out RoomType roomType, BiomeType biome, TierID tier)
	{
		roomType = RoomType.None;
		List<RoomSetEntry> standardRoomPoolOverride = standardRoomPool;
		if (biome == BiomeType.TowerExterior)
		{
			standardRoomPoolOverride = null;
		}
		RoomSetEntry[] array = CreateRoomsBuildRule.GetPotentialRooms(biomeCreator, biomeController, roomSizesThatFit, roomRequirements, standardRoomPoolOverride, out roomType, biome);
		if (tier == TierID.Two && array.Length != 0)
		{
			List<RoomSetEntry> second = (from room in array
			where room.IsMirrored
			select room).ToList<RoomSetEntry>();
			array = array.Intersect(second).ToArray<RoomSetEntry>();
		}
		else if (tier == TierID.Three && array.Length != 0)
		{
			List<RoomSetEntry> second2 = (from room in array
			where !room.IsMirrored
			select room).ToList<RoomSetEntry>();
			array = array.Intersect(second2).ToArray<RoomSetEntry>();
		}
		return array;
	}

	// Token: 0x0600528D RID: 21133 RVA: 0x00139CA4 File Offset: 0x00137EA4
	private static void ReplaceRoomAtTopIfNecessary(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		List<GridPointManager> list = (from point in biomeCreator.GetGridPointsAtExtent(biomeController, RoomSide.Top, 0)
		select point.Owner).Distinct<GridPointManager>().ToList<GridPointManager>();
		if (!list.Any((GridPointManager originRoom) => originRoom.DoorLocations.Any((DoorLocation location) => location.RoomSide == RoomSide.Top)))
		{
			int num = -1;
			RoomSetEntry roomSetEntry = default(RoomSetEntry);
			for (int i = 0; i < list.Count; i++)
			{
				RoomSetEntry replacementRoom = ReplaceRoomUtility.GetReplacementRoom(biomeController, list[i], list[i].DoorLocations, CreateRoomsBuildRule_Tower.m_topDoorList, true);
				if (replacementRoom.RoomMetaData != null)
				{
					num = i;
					roomSetEntry = replacementRoom;
					if (!list[i].RoomMetaData.IsSpecialRoom)
					{
						break;
					}
				}
			}
			if (num != -1)
			{
				list[num].SetRoomMetaData(roomSetEntry.RoomMetaData, roomSetEntry.IsMirrored);
				return;
			}
			string text = "<color=red>| CreateRoomsBuildRule_Tower | The topmost rooms of the tower did not have any top doors, and no suitable replacements were found.</color>";
			Debug.Log(text);
			biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text);
		}
	}

	// Token: 0x04003E04 RID: 15876
	protected Bounds m_defaultBorder;

	// Token: 0x04003E05 RID: 15877
	protected Bounds m_tier1Border;

	// Token: 0x04003E06 RID: 15878
	protected Bounds m_tier2Border;

	// Token: 0x04003E07 RID: 15879
	protected Bounds m_tier3Border;

	// Token: 0x04003E08 RID: 15880
	private static readonly List<DoorLocation> m_topDoorList = new List<DoorLocation>
	{
		new DoorLocation(RoomSide.Top, 0),
		new DoorLocation(RoomSide.Top, 1),
		new DoorLocation(RoomSide.Top, 2)
	};
}

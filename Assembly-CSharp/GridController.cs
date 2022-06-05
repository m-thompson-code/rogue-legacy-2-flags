using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000675 RID: 1653
public class GridController
{
	// Token: 0x170014BF RID: 5311
	// (get) Token: 0x06003B73 RID: 15219 RVA: 0x000CC77B File Offset: 0x000CA97B
	// (set) Token: 0x06003B74 RID: 15220 RVA: 0x000CC782 File Offset: 0x000CA982
	public static Dictionary<RoomSide, int> Extents
	{
		get
		{
			return GridController.m_extents;
		}
		private set
		{
			GridController.m_extents = value;
		}
	}

	// Token: 0x170014C0 RID: 5312
	// (get) Token: 0x06003B75 RID: 15221 RVA: 0x000CC78A File Offset: 0x000CA98A
	// (set) Token: 0x06003B76 RID: 15222 RVA: 0x000CC791 File Offset: 0x000CA991
	public static GridPoint[,] GridPoints
	{
		get
		{
			return GridController.m_gridPoints;
		}
		private set
		{
			GridController.m_gridPoints = value;
		}
	}

	// Token: 0x170014C1 RID: 5313
	// (get) Token: 0x06003B77 RID: 15223 RVA: 0x000CC799 File Offset: 0x000CA999
	public static bool IsInstantiated
	{
		get
		{
			return GridController.m_instance != null;
		}
	}

	// Token: 0x06003B78 RID: 15224 RVA: 0x000CC7A3 File Offset: 0x000CA9A3
	public GridController()
	{
		GridController.m_instance = this;
		if (GridController.GridPoints == null)
		{
			GridController.GridPoints = new GridPoint[1000, 1000];
		}
	}

	// Token: 0x06003B79 RID: 15225 RVA: 0x000CC7CC File Offset: 0x000CA9CC
	public static void AddRoomToGrid(GridPoint gridPoint)
	{
		if (GridController.m_gridOffset == -1)
		{
			GridController.m_gridOffset = 500;
		}
		int num = gridPoint.GridCoordinates.x + GridController.m_gridOffset;
		int num2 = gridPoint.GridCoordinates.y + GridController.m_gridOffset;
		try
		{
			GridController.GridPoints[num, num2] = gridPoint;
		}
		catch (Exception ex)
		{
			if (ex is IndexOutOfRangeException)
			{
				Debug.LogFormat("<color=red>| GridController | The GRID_SIZE value ({0}) is too small. Set it to a larger value.</color>", new object[]
				{
					1000
				});
			}
			throw;
		}
		GridController.m_occupiedGridPointCoords.Add(gridPoint.GridCoordinates);
		GridController.UpdateExtentAfterAdd(gridPoint);
	}

	// Token: 0x06003B7A RID: 15226 RVA: 0x000CC874 File Offset: 0x000CAA74
	public void ClearGridPoint(GridPoint gridPoint)
	{
		if (GridController.m_gridOffset == -1)
		{
			GridController.m_gridOffset = 500;
		}
		for (int i = 0; i < gridPoint.RoomMetaData.Size.x; i++)
		{
			for (int j = 0; j < gridPoint.RoomMetaData.Size.y; j++)
			{
				GridController.GridPoints[gridPoint.GridCoordinates.x + i + GridController.m_gridOffset, gridPoint.GridCoordinates.y + j + GridController.m_gridOffset] = null;
			}
		}
		GridController.m_occupiedGridPointCoords.Remove(gridPoint.GridCoordinates);
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x000CC918 File Offset: 0x000CAB18
	public static Vector2Int GetDoorLeadsToGridCoordinates(Vector2Int roomCoordinates, Vector2Int roomSize, DoorLocation location)
	{
		int x = -1;
		int y = -1;
		if (location.RoomSide == RoomSide.Left || location.RoomSide == RoomSide.Right)
		{
			if (location.RoomSide == RoomSide.Left)
			{
				x = roomCoordinates.x - 1;
			}
			else
			{
				x = roomCoordinates.x + roomSize.x;
			}
			y = roomCoordinates.y + roomSize.y - 1 - location.DoorNumber;
		}
		else if (location.RoomSide == RoomSide.Top || location.RoomSide == RoomSide.Bottom)
		{
			x = roomCoordinates.x + location.DoorNumber;
			y = roomCoordinates.y + roomSize.y;
			if (location.RoomSide == RoomSide.Bottom)
			{
				y = roomCoordinates.y - 1;
			}
		}
		return new Vector2Int(x, y);
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x000CC9D0 File Offset: 0x000CABD0
	public List<DoorLocation> GetDoorLocationsThatFitAtCoordinates(Vector2Int coordinates, Bounds border, RoomSide side, Vector2Int roomSize)
	{
		List<DoorLocation> list = new List<DoorLocation>();
		bool flag = false;
		int num = roomSize.x;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			num = roomSize.y;
		}
		for (int i = 0; i < num; i++)
		{
			if (GridController.GetIsSpaceForRoomAtDoor(coordinates, side, roomSize, i, border))
			{
				flag = true;
				list.Add(new DoorLocation(RoomUtility.GetOppositeSide(side), i));
			}
			else if (flag)
			{
				break;
			}
		}
		return list;
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x000CCA30 File Offset: 0x000CAC30
	public static Vector2 GetRoomCoordinatesFromGridCoordinates(Vector2Int gridCoordinates)
	{
		return new Vector2((float)(gridCoordinates.x * 32), (float)(gridCoordinates.y * 18));
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x000CCA50 File Offset: 0x000CAC50
	public static bool GetIsGridSpaceAvailable(Vector2Int coordinates)
	{
		int num = coordinates.x + GridController.m_gridOffset;
		int num2 = coordinates.y + GridController.m_gridOffset;
		return GridController.GridPoints[num, num2] == null;
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x000CCA88 File Offset: 0x000CAC88
	public static bool GetIsSpaceForRoomAtDoor(Vector2Int coordinates, Vector2Int roomSize, RoomSide side, int atDoorNumber)
	{
		int num = 1;
		if (side == RoomSide.Left || side == RoomSide.Bottom)
		{
			num = -1;
		}
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			for (int i = roomSize.x - 1; i >= 0; i--)
			{
				int num2 = coordinates.x + GridController.m_gridOffset + num * i;
				for (int j = atDoorNumber; j > -roomSize.y + atDoorNumber; j--)
				{
					int num3 = coordinates.y + GridController.m_gridOffset + j;
					if (GridController.GridPoints[num2, num3] != null)
					{
						return false;
					}
				}
			}
		}
		else
		{
			for (int k = roomSize.y - 1; k >= 0; k--)
			{
				int num4 = coordinates.y + GridController.m_gridOffset + num * k;
				for (int l = atDoorNumber; l > -roomSize.x + atDoorNumber; l--)
				{
					int num5 = coordinates.x + GridController.m_gridOffset - l;
					if (GridController.GridPoints[num5, num4] != null)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x000CCB74 File Offset: 0x000CAD74
	public static bool GetIsSpaceForRoomAtDoor(Vector2Int coordinates, RoomSide side, Vector2Int roomSize, int atDoorNumber, Bounds border)
	{
		int num = 1;
		if (side == RoomSide.Left || side == RoomSide.Bottom)
		{
			num = -1;
		}
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			for (int i = roomSize.x - 1; i >= 0; i--)
			{
				int num2 = coordinates.x + num * i;
				int num3 = num2 + GridController.m_gridOffset;
				for (int j = atDoorNumber; j > -roomSize.y + atDoorNumber; j--)
				{
					int num4 = coordinates.y + j;
					int num5 = num4 + GridController.m_gridOffset;
					bool flag = GridController.GridPoints[num3, num5] != null;
					bool flag2 = border.max.x == (float)num2 || border.max.y == (float)num4 || !border.Contains(new Vector2((float)num2, (float)num4));
					if (flag || flag2)
					{
						return false;
					}
				}
			}
		}
		else
		{
			for (int k = roomSize.y - 1; k >= 0; k--)
			{
				int num6 = coordinates.y + num * k;
				int num7 = num6 + GridController.m_gridOffset;
				for (int l = atDoorNumber; l > -roomSize.x + atDoorNumber; l--)
				{
					int num8 = coordinates.x - l;
					int num9 = num8 + GridController.m_gridOffset;
					bool flag3 = GridController.GridPoints[num9, num7] != null;
					bool flag4 = border.max.x == (float)num8 || border.max.y == (float)num6 || !border.Contains(new Vector2((float)num8, (float)num6));
					if (flag3 || flag4)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x000CCD14 File Offset: 0x000CAF14
	public static int GetDoorNumber(GridPoint gridPoint, RoomSide side)
	{
		int result = gridPoint.LocalCoords.x;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			result = gridPoint.Owner.Size.y - 1 - gridPoint.LocalCoords.y;
		}
		return result;
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x000CCD5E File Offset: 0x000CAF5E
	public static Vector2Int GetGridCoordsOnGridPointSide(GridPoint gridPoint, RoomSide side)
	{
		return GridController.GetDoorLeadsToGridCoordinates(gridPoint.GridCoordinates, Vector2Int.one, new DoorLocation(side, 0));
	}

	// Token: 0x06003B83 RID: 15235 RVA: 0x000CCD78 File Offset: 0x000CAF78
	public static Bounds GetGridPointBounds(Vector2Int gridCoordinates)
	{
		Bounds result = default(Bounds);
		Vector2 worldPositionFromRoomCoordinates = GridController.GetWorldPositionFromRoomCoordinates(new Vector2((float)(gridCoordinates.x * 32), (float)(gridCoordinates.y * 18)), new Vector2Int(1, 1));
		result.center = worldPositionFromRoomCoordinates;
		float x = worldPositionFromRoomCoordinates.x - 16f;
		float y = worldPositionFromRoomCoordinates.y - 9f;
		float x2 = worldPositionFromRoomCoordinates.x + 16f;
		float y2 = worldPositionFromRoomCoordinates.y + 9f;
		result.SetMinMax(new Vector2(x, y), new Vector2(x2, y2));
		return result;
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x000CCE1C File Offset: 0x000CB01C
	public static Vector2 GetWorldPositionFromRoomCoordinates(Vector2 roomCoordinates, Vector2Int roomSize)
	{
		float x = 0.5f * (float)roomSize.x * 32f;
		float y = 0.5f * (float)roomSize.y * 18f;
		Vector2 b = new Vector2(x, y);
		return roomCoordinates + b;
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x000CCE64 File Offset: 0x000CB064
	public static Vector2Int GetRoomGridCoordinates(Vector2Int buildCoordinates, Vector2Int roomSize, DoorLocation location)
	{
		int x = buildCoordinates.x;
		int y = buildCoordinates.y;
		if (location != DoorLocation.Empty)
		{
			if (location.RoomSide == RoomSide.Left || location.RoomSide == RoomSide.Right)
			{
				if (location.RoomSide == RoomSide.Left)
				{
					x = buildCoordinates.x;
				}
				else if (location.RoomSide == RoomSide.Right)
				{
					x = buildCoordinates.x - roomSize.x + 1;
				}
				y = buildCoordinates.y - (roomSize.y - 1 - location.DoorNumber);
			}
			else if (location.RoomSide == RoomSide.Top || location.RoomSide == RoomSide.Bottom)
			{
				x = buildCoordinates.x - location.DoorNumber;
				if (location.RoomSide == RoomSide.Top)
				{
					y = buildCoordinates.y - roomSize.y + 1;
				}
			}
		}
		return new Vector2Int(x, y);
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x000CCF38 File Offset: 0x000CB138
	public void OnBiomeControllerDestroyed(BiomeController biomeController)
	{
		foreach (GridPoint gridPoint in biomeController.GridPointManager.GridPoints)
		{
			this.ClearGridPoint(gridPoint);
		}
		GridController.Extents[RoomSide.Left] = GridController.m_occupiedGridPointCoords.Min((Vector2Int coords) => coords.x);
		GridController.Extents[RoomSide.Right] = GridController.m_occupiedGridPointCoords.Max((Vector2Int coords) => coords.x);
		GridController.Extents[RoomSide.Top] = GridController.m_occupiedGridPointCoords.Max((Vector2Int coords) => coords.y);
		GridController.Extents[RoomSide.Bottom] = GridController.m_occupiedGridPointCoords.Min((Vector2Int coords) => coords.y);
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x000CD060 File Offset: 0x000CB260
	private static void UpdateExtentAfterAdd(GridPoint gridPoint)
	{
		if (gridPoint.GridCoordinates.x >= GridController.Extents[RoomSide.Right])
		{
			GridController.Extents[RoomSide.Right] = gridPoint.GridCoordinates.x + 1;
		}
		else if (gridPoint.GridCoordinates.x < GridController.Extents[RoomSide.Left])
		{
			GridController.Extents[RoomSide.Left] = gridPoint.GridCoordinates.x;
		}
		if (gridPoint.GridCoordinates.y >= GridController.Extents[RoomSide.Top])
		{
			GridController.Extents[RoomSide.Top] = gridPoint.GridCoordinates.y + 1;
			return;
		}
		if (gridPoint.GridCoordinates.y < GridController.Extents[RoomSide.Bottom])
		{
			GridController.Extents[RoomSide.Bottom] = gridPoint.GridCoordinates.y;
		}
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x000CD144 File Offset: 0x000CB344
	public static void Reset()
	{
		if (GridController.GridPoints != null)
		{
			for (int i = 0; i < GridController.GridPoints.GetLength(0); i++)
			{
				for (int j = 0; j < GridController.GridPoints.GetLength(1); j++)
				{
					GridController.GridPoints[i, j] = null;
				}
			}
		}
		if (GridController.Extents != null)
		{
			GridController.Extents[RoomSide.Left] = 0;
			GridController.Extents[RoomSide.Right] = 0;
			GridController.Extents[RoomSide.Top] = 0;
			GridController.Extents[RoomSide.Bottom] = 0;
		}
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x000CD1C8 File Offset: 0x000CB3C8
	public static bool GetDoesDoorLeadToDifferentBiome(Door door)
	{
		BiomeType appearanceBiomeType = door.Room.AppearanceBiomeType;
		BiomeType doorLeadsToBiome = GridController.GetDoorLeadsToBiome(door);
		return (appearanceBiomeType != BiomeType.Tower || doorLeadsToBiome != BiomeType.TowerExterior) && (appearanceBiomeType != BiomeType.TowerExterior || doorLeadsToBiome != BiomeType.Tower) && (appearanceBiomeType != BiomeType.Stone || doorLeadsToBiome != BiomeType.Town) && (appearanceBiomeType != BiomeType.Town || doorLeadsToBiome != BiomeType.Stone) && (appearanceBiomeType != BiomeType.TowerExterior || doorLeadsToBiome != BiomeType.Study) && (appearanceBiomeType != BiomeType.Study || doorLeadsToBiome != BiomeType.TowerExterior) && appearanceBiomeType != doorLeadsToBiome;
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x000CD248 File Offset: 0x000CB448
	public static GridPoint GetGridPoint(Vector2Int coords)
	{
		GridPoint result = null;
		if (GridController.GridPoints != null)
		{
			int num = coords.x + GridController.m_gridOffset;
			int num2 = coords.y + GridController.m_gridOffset;
			try
			{
				result = GridController.GridPoints[num, num2];
			}
			catch (Exception)
			{
			}
		}
		return result;
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x000CD2A0 File Offset: 0x000CB4A0
	public static GridPoint GetGridPoint(Door door)
	{
		return (door.Room as Room).GridPointManager.GetGridPoint(new DoorLocation(door.Side, door.Number));
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x000CD2C8 File Offset: 0x000CB4C8
	public static BiomeType GetDoorLeadsToBiome(Door door)
	{
		BiomeType result = door.Room.AppearanceBiomeType;
		GridPoint gridPoint = GridController.GetGridPoint(door.GridPointCoordinates);
		if (gridPoint != null)
		{
			GridPoint connectedGridPoint = gridPoint.GetConnectedGridPoint(door.Side);
			if (connectedGridPoint != null)
			{
				result = connectedGridPoint.Biome;
			}
		}
		return result;
	}

	// Token: 0x04002D11 RID: 11537
	public const int GRID_SIZE = 1000;

	// Token: 0x04002D12 RID: 11538
	private static Dictionary<RoomSide, int> m_extents = new Dictionary<RoomSide, int>
	{
		{
			RoomSide.Left,
			0
		},
		{
			RoomSide.Right,
			0
		},
		{
			RoomSide.Top,
			0
		},
		{
			RoomSide.Bottom,
			0
		}
	};

	// Token: 0x04002D13 RID: 11539
	private static GridPoint[,] m_gridPoints = null;

	// Token: 0x04002D14 RID: 11540
	private static int m_gridOffset = -1;

	// Token: 0x04002D15 RID: 11541
	private static GridController m_instance = null;

	// Token: 0x04002D16 RID: 11542
	private static HashSet<Vector2Int> m_occupiedGridPointCoords = new HashSet<Vector2Int>();
}

using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using ClipperLibFerr;
using Ferr;
using UnityEngine;

// Token: 0x02000A98 RID: 2712
public static class BiomeCreatorTools
{
	// Token: 0x17001C18 RID: 7192
	// (get) Token: 0x060051D8 RID: 20952 RVA: 0x001361F8 File Offset: 0x001343F8
	public static BiomeCreator BiomeCreator
	{
		get
		{
			if (Application.isPlaying)
			{
				if (BiomeCreatorTools.m_biomeCreator == null)
				{
					GameObject gameObject = GameObject.Find("LevelManager");
					if (gameObject != null)
					{
						BiomeCreatorTools.m_biomeCreator = gameObject.GetComponent<BiomeCreator>();
						if (BiomeCreatorTools.m_biomeCreator == null)
						{
							throw new MissingComponentException("BiomeCreator");
						}
					}
				}
				return BiomeCreatorTools.m_biomeCreator;
			}
			return null;
		}
	}

	// Token: 0x060051D9 RID: 20953 RVA: 0x00136258 File Offset: 0x00134458
	public static void PositionRoom(Room destinationRoom, RoomSide atSide, int atDoorIndex, Room room, int doorIndex)
	{
		if (destinationRoom == null)
		{
			throw new ArgumentNullException("destinationRoom");
		}
		if (room == null)
		{
			throw new ArgumentNullException("room");
		}
		if (atDoorIndex < 0)
		{
			throw new ArgumentException("atDoorIndex", "Value must be greater than or equal to 0");
		}
		if (atDoorIndex < 0)
		{
			throw new ArgumentException("doorIndex", "Value must be greater than or equal to 0");
		}
		if (atSide == RoomSide.Any || atSide == RoomSide.None)
		{
			throw new ArgumentException("atSide", "Value must be Left, Right, Top or Bottom");
		}
		if (atSide == RoomSide.Left || atSide == RoomSide.Right)
		{
			float x = 0f;
			if (atSide == RoomSide.Left)
			{
				x = (float)room.UnitDimensions.x;
			}
			Vector2 coords = destinationRoom.GetDoor(atSide, atDoorIndex).Coordinates - new Vector2(x, (float)((room.Size.y - 1 - doorIndex) * 18));
			room.MoveToCoordinates(coords);
			return;
		}
		if (atSide == RoomSide.Top || atSide == RoomSide.Bottom)
		{
			float y = destinationRoom.Coordinates.y + (float)destinationRoom.UnitDimensions.y;
			if (atSide == RoomSide.Bottom)
			{
				y = destinationRoom.Coordinates.y - (float)room.UnitDimensions.y;
			}
			Vector2 coords2 = new Vector2(destinationRoom.Coordinates.x + (float)(atDoorIndex * 32) - (float)(doorIndex * 32), y);
			room.MoveToCoordinates(coords2);
		}
	}

	// Token: 0x060051DA RID: 20954 RVA: 0x00136394 File Offset: 0x00134594
	public static void MergeTerrain(BaseRoom room)
	{
		if (room == null)
		{
			throw new ArgumentNullException("room");
		}
		List<Ferr2DT_PathTerrain> list = room.TerrainManager.Platforms.ToList<Ferr2DT_PathTerrain>();
		if (list != null && list.Count > 0 && Application.isPlaying)
		{
			Ferr2DUtilities.FastEasyBoolean(ClipType.ctUnion, list, Ferr2DUtilities.Ferr2DUndoType.Destroy, true);
		}
	}

	// Token: 0x060051DB RID: 20955 RVA: 0x001363E4 File Offset: 0x001345E4
	public static void CopyToRoom<T>(List<T> data, Room toRoom, string intoChildNamed = "")
	{
		if (toRoom == null)
		{
			throw new ArgumentNullException("toRoom");
		}
		if (data == null || data.Count == 0)
		{
			Debug.LogFormat("<color=yellow>Data being copied into Room is empty</color>", Array.Empty<object>());
			return;
		}
		Transform transform = toRoom.transform;
		if (!string.IsNullOrEmpty(intoChildNamed))
		{
			transform = toRoom.transform.Find(intoChildNamed);
			if (transform == null)
			{
				transform = new GameObject(intoChildNamed).transform;
			}
			transform.SetParent(toRoom.transform, false);
			transform.localPosition = Vector3.zero;
		}
		foreach (T t in data)
		{
			MonoBehaviour monoBehaviour = t as MonoBehaviour;
			if (monoBehaviour != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(monoBehaviour.gameObject, transform);
				gameObject.transform.position = monoBehaviour.transform.position;
				gameObject.name = monoBehaviour.name;
			}
		}
	}

	// Token: 0x060051DC RID: 20956 RVA: 0x001364E4 File Offset: 0x001346E4
	public static OneWay PlaceOneWayAtDoor(Door door)
	{
		if (door == null)
		{
			throw new ArgumentNullException("door");
		}
		if (door.Side != RoomSide.Bottom)
		{
			throw new ArgumentException("door.Side", "Side must be Bottom");
		}
		Transform transform = (door.Room as Room).OneWayLocation;
		if (transform == null)
		{
			transform = new GameObject("One Ways").transform;
			transform.SetParent(door.Room.gameObject.transform, false);
		}
		Ferr2DT_PathTerrain ferr2DT_PathTerrain = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(RoomPrefabLibrary.BottomDoorOneWayPrefab, transform, false);
		door.Room.TerrainManager.AddOneWay(ferr2DT_PathTerrain);
		float x = door.Coordinates.x + 16f;
		float y = door.Coordinates.y + 1f;
		ferr2DT_PathTerrain.transform.position = new Vector3(x, y, 0f);
		ferr2DT_PathTerrain.ClearPoints();
		Vector2 aPt = new Vector2(3f, 0f);
		Vector2 aPt2 = new Vector2(-3f, 0f);
		ferr2DT_PathTerrain.AddPoint(aPt, -1, PointType.Sharp);
		ferr2DT_PathTerrain.AddPoint(aPt2, -1, PointType.Sharp);
		OneWay component = ferr2DT_PathTerrain.GetComponent<OneWay>();
		BiomeType biomeType = ArtUtility.GetTerrainBiomeType(door.Room);
		if (biomeType == BiomeType.None)
		{
			biomeType = BiomeType.Editor;
		}
		component.SetVisuals(biomeType, door.Room);
		door.SetOneWay(component.gameObject);
		return component;
	}

	// Token: 0x060051DD RID: 20957 RVA: 0x00136634 File Offset: 0x00134834
	public static Sky CreateSkyInstance(BiomeArtData biomeArtData)
	{
		if (biomeArtData.SkyData.SkyPrefab)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(biomeArtData.SkyData.SkyPrefab);
			gameObject.GetComponent<Sky>();
			return gameObject.GetComponent<Sky>();
		}
		Debug.LogFormat("<color=yellow>{0}: Sky Prefab is null</color>", new object[]
		{
			Time.frameCount
		});
		return null;
	}

	// Token: 0x060051DE RID: 20958 RVA: 0x00136690 File Offset: 0x00134890
	public static Weather[] CreateWeatherInstances(BiomeArtData biomeArtData)
	{
		BiomeCreatorTools.m_weatherList_STATIC.Clear();
		if (biomeArtData.WeatherData.WeatherPrefabArray != null)
		{
			Weather[] weatherPrefabArray = biomeArtData.WeatherData.WeatherPrefabArray;
			for (int i = 0; i < weatherPrefabArray.Length; i++)
			{
				Weather item = UnityEngine.Object.Instantiate<Weather>(weatherPrefabArray[i]);
				BiomeCreatorTools.m_weatherList_STATIC.Add(item);
			}
			return BiomeCreatorTools.m_weatherList_STATIC.ToArray();
		}
		Debug.LogFormat("<color=yellow>{0}: No weather prefabs found in biome</color>", new object[]
		{
			Time.frameCount
		});
		return null;
	}

	// Token: 0x060051DF RID: 20959 RVA: 0x00136710 File Offset: 0x00134910
	public static void CreateBackground(BaseRoom targetRoom)
	{
		if (targetRoom is Room)
		{
			BiomeCreatorTools.m_backgroundPoolEntryHelper.Clear();
			Room room = targetRoom as Room;
			BiomeType appearanceBiomeType = room.AppearanceBiomeType;
			if (appearanceBiomeType != BiomeType.None)
			{
				float num = room.Coordinates.x;
				float num2 = room.Coordinates.y;
				float num3 = room.Coordinates.x + (float)room.UnitDimensions.x;
				float num4 = room.Coordinates.y + (float)room.UnitDimensions.y;
				foreach (GridPointManager gridPointManager in room.GridPointManager.MergeWithGridPointManagers)
				{
					num = Mathf.Min(num, (float)(gridPointManager.GridCoordinates.x * 32));
					num2 = Mathf.Min(num2, (float)(gridPointManager.GridCoordinates.y * 18));
					num3 = Mathf.Max(num3, (float)((gridPointManager.GridCoordinates.x + gridPointManager.Size.x) * 32));
					num4 = Mathf.Max(num4, (float)((gridPointManager.GridCoordinates.y + gridPointManager.Size.y) * 18));
				}
				Vector2Int vector2Int = new Vector2Int((int)Mathf.Abs(num3 - num), (int)Mathf.Abs(num4 - num2));
				Background background = null;
				RoomBackgroundOverride component = targetRoom.gameObject.GetComponent<RoomBackgroundOverride>();
				bool flag = true;
				bool flag2 = false;
				if (component)
				{
					background = component.BackgroundOverride;
					flag = component.IsTiled;
				}
				else
				{
					BiomeArtData biomeArtData = targetRoom.BiomeArtDataOverride;
					if (!biomeArtData)
					{
						biomeArtData = BiomeArtDataLibrary.GetArtData(appearanceBiomeType);
					}
					if (biomeArtData)
					{
						background = BiomeCreatorTools.GetBackgroundPrefab(biomeArtData.BackgroundData);
						flag2 = biomeArtData.BackgroundData.TileNormally;
					}
					else
					{
						Debug.LogFormat("<color=red>{0}: [BiomeCreatorTools] Failed to find Biome Art Data for Biome ({1})</color>", new object[]
						{
							Time.frameCount,
							appearanceBiomeType
						});
					}
				}
				if (background)
				{
					if (flag)
					{
						Vector2Int vector2Int2 = new Vector2Int(vector2Int.x / 32, vector2Int.y / 18);
						Vector2 a = new Vector2(num - 16f, num2 - 9f);
						for (int i = 0; i < vector2Int2.x + 2; i++)
						{
							if (flag2)
							{
								for (int j = 0; j < vector2Int2.y + 2; j++)
								{
									BackgroundPoolEntry backgroundPoolEntry = new BackgroundPoolEntry();
									backgroundPoolEntry.BackgroundPrefab = background;
									backgroundPoolEntry.Position = a + new Vector2((float)(i * 32), (float)(j * 18));
									BiomeCreatorTools.m_backgroundPoolEntryHelper.Add(backgroundPoolEntry);
								}
							}
							else
							{
								BackgroundPoolEntry backgroundPoolEntry2 = new BackgroundPoolEntry();
								backgroundPoolEntry2.BackgroundPrefab = background;
								backgroundPoolEntry2.Position = a + new Vector2((float)(i * 32), 0f);
								BiomeCreatorTools.m_backgroundPoolEntryHelper.Add(backgroundPoolEntry2);
							}
						}
					}
					else if (background)
					{
						BackgroundPoolEntry backgroundPoolEntry3 = new BackgroundPoolEntry();
						backgroundPoolEntry3.BackgroundPrefab = background;
						backgroundPoolEntry3.Position = room.gameObject.transform.position;
						BiomeCreatorTools.m_backgroundPoolEntryHelper.Add(backgroundPoolEntry3);
					}
				}
			}
			room.SetBackgroundPoolEntries(BiomeCreatorTools.m_backgroundPoolEntryHelper.ToArray());
			return;
		}
		if (targetRoom is MergeRoom)
		{
			Debug.LogFormat("<color=red>{0}: BiomeCreatorTools.CreateBackground() currently only works with Room's</color>", new object[]
			{
				Time.frameCount
			});
		}
	}

	// Token: 0x060051E0 RID: 20960 RVA: 0x00136A9C File Offset: 0x00134C9C
	private static Background GetBackgroundPrefab(BackgroundBiomeArtData backgroundData)
	{
		if (backgroundData != null && backgroundData.Backgrounds != null && backgroundData.Backgrounds.Count > 0)
		{
			List<Background> list = (from entry in backgroundData.Backgrounds
			where entry.Size == new Vector2Int(1, 1)
			select entry).ToList<Background>();
			int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("(BiomeCreatorTools) GetBackgroundPrefab", Array.Empty<object>()), 0, list.Count);
			return list[randomNumber];
		}
		return null;
	}

	// Token: 0x060051E1 RID: 20961 RVA: 0x0002CA83 File Offset: 0x0002AC83
	public static CinemachineVirtualCamera CreateCinemachineVirtualCamera(BaseRoom room)
	{
		if (room == null)
		{
			throw new ArgumentNullException("room");
		}
		return UnityEngine.Object.Instantiate<CinemachineVirtualCamera>(RoomPrefabLibrary.CinemachineVirtualCameraPrefab, room.gameObject.transform, false);
	}

	// Token: 0x060051E2 RID: 20962 RVA: 0x00136B1C File Offset: 0x00134D1C
	public static void SetOuterEdges(IEnumerable<Ferr2DT_PathTerrain> terrainToSet, Bounds bounds)
	{
		foreach (Ferr2DT_PathTerrain terrain in terrainToSet)
		{
			BiomeCreatorTools.SetOuterEdges(terrain, bounds);
		}
	}

	// Token: 0x060051E3 RID: 20963 RVA: 0x00136B64 File Offset: 0x00134D64
	public static void SetOuterEdges(Ferr2DT_PathTerrain terrain, Bounds bounds)
	{
		int count = terrain.PathData.Count;
		for (int i = 0; i < count; i++)
		{
			Vector2 vector = terrain.PathData.Get(i);
			Vector2 vector2;
			if (i < count - 1)
			{
				vector2 = terrain.PathData.Get(i + 1);
			}
			else
			{
				vector2 = terrain.PathData.Get(0);
			}
			vector = terrain.transform.TransformPoint(vector);
			vector2 = terrain.transform.TransformPoint(vector2);
			if (BiomeCreatorTools.AreBothPointsOnSameEdge(bounds, vector, vector2))
			{
				Ferr2D_PointData data = terrain.PathData.GetData(i);
				data.directionOverride = 4;
				terrain.PathData.SetData(i, data);
			}
		}
	}

	// Token: 0x060051E4 RID: 20964 RVA: 0x00136C1C File Offset: 0x00134E1C
	public static bool AreBothPointsOnSameEdge(Bounds bounds, Vector2 pointA, Vector2 pointB)
	{
		bool result = false;
		if ((pointA.x == bounds.min.x && pointB.x == bounds.min.x) || (pointA.x == bounds.max.x && pointB.x == bounds.max.x) || (pointA.y == bounds.min.y && pointB.y == bounds.min.y) || (pointA.y == bounds.max.y && pointB.y == bounds.max.y))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060051E5 RID: 20965 RVA: 0x00136CD0 File Offset: 0x00134ED0
	private static bool GetIsPointInRoom(BaseRoom room, Vector2 point)
	{
		bool result = false;
		if (room is Room)
		{
			result = room.Collider2D.OverlapPoint(point);
		}
		else if (room is MergeRoom)
		{
			foreach (Bounds bounds in (room as MergeRoom).StandaloneRoomBounds)
			{
				if (bounds.Contains(point))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060051E6 RID: 20966 RVA: 0x00136D34 File Offset: 0x00134F34
	public static List<Door> GetDoorsInRoomThatFit(Door door, Room room)
	{
		List<Door> doorsOnSide = room.GetDoorsOnSide(RoomUtility.GetOppositeSide(door.Side));
		Vector2 coordinates = door.Coordinates;
		List<Door> list = new List<Door>();
		foreach (Door door2 in doorsOnSide)
		{
			Vector2 coordinates2 = Vector2.zero;
			if (door.Side == RoomSide.Left || door.Side == RoomSide.Right)
			{
				float x = 0f;
				if (door.Side == RoomSide.Left)
				{
					x = (float)(room.Size.x * 32);
				}
				coordinates2 = coordinates - new Vector2(x, (float)((room.Size.y - 1 - door2.Number) * 18));
			}
			else
			{
				float y = 0f;
				if (door.Side == RoomSide.Bottom)
				{
					y = (float)(-1 * room.Size.y * 18);
				}
				coordinates2 = coordinates + new Vector2((float)(-1 * door2.Number * 32), y);
			}
			if (!BiomeCreatorTools.CanRoomFit(coordinates2, room))
			{
				list.Add(door2);
			}
		}
		foreach (Door item in list)
		{
			doorsOnSide.Remove(item);
		}
		return doorsOnSide;
	}

	// Token: 0x060051E7 RID: 20967 RVA: 0x00136EA4 File Offset: 0x001350A4
	public static Vector2 GetCoordinatesOfRoomConnectedToDoor(Door door, Vector2Int roomSize, int doorNumber)
	{
		float x = 0f;
		float y = 0f;
		RoomSide side = door.Side;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			if (side == RoomSide.Left)
			{
				x = (float)(roomSize.x * 32);
			}
			y = (float)((roomSize.y - 1 - doorNumber) * 18);
		}
		else if (side == RoomSide.Top || side == RoomSide.Bottom)
		{
			x = (float)(doorNumber * 32);
			if (side == RoomSide.Bottom)
			{
				y = (float)(roomSize.y * 18);
			}
		}
		return door.Coordinates - new Vector2(x, y);
	}

	// Token: 0x060051E8 RID: 20968 RVA: 0x00136F20 File Offset: 0x00135120
	public static bool CanRoomFit(Vector2 coordinates, Vector2 roomSize)
	{
		Vector2 a = new Vector2(roomSize.x * 32f, roomSize.y * 18f);
		return !Physics2D.OverlapBox(coordinates + 0.5f * a, BiomeCreatorTools.ROOM_COLLIDER_SCALE * a, 0f, BiomeCreatorTools.ROOM_COLLISION_CHECK_LAYER_MASK);
	}

	// Token: 0x060051E9 RID: 20969 RVA: 0x0002CAAF File Offset: 0x0002ACAF
	public static bool CanRoomFit(Vector2 coordinates, Room room)
	{
		return BiomeCreatorTools.CanRoomFit(coordinates, room.Size);
	}

	// Token: 0x060051EA RID: 20970 RVA: 0x00136F84 File Offset: 0x00135184
	public static void GrowTerrainAlongEdge(BaseRoom room)
	{
		float num = 2f;
		if (room is Room)
		{
			Room room2 = room as Room;
			float x = room2.Coordinates.x;
			float num2 = room2.Coordinates.x + (float)room2.UnitDimensions.x;
			float y = room2.Coordinates.y;
			float num3 = room2.Coordinates.y + (float)room2.UnitDimensions.y;
			Vector2 vector = new Vector2(x, num3);
			Vector2 key = new Vector2(num2, num3);
			Vector2 key2 = new Vector2(num2, y);
			Vector2 key3 = new Vector2(x, y);
			Vector2 vector2 = num * (vector - new Vector2(room2.transform.position.x, room2.transform.position.y)).normalized;
			Vector2 value = new Vector2(-1f * vector2.x, vector2.y);
			Vector2 value2 = new Vector2(vector2.x, -1f * vector2.y);
			Vector2 value3 = new Vector2(-1f * vector2.x, -1f * vector2.y);
			Dictionary<Vector2, Vector2> dictionary = new Dictionary<Vector2, Vector2>
			{
				{
					vector,
					vector2
				},
				{
					key,
					value
				},
				{
					key2,
					value3
				},
				{
					key3,
					value2
				}
			};
			foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in room.TerrainManager.Platforms)
			{
				foreach (Vector2 v in ferr2DT_PathTerrain.PathData.GetPoints(0))
				{
					Vector2 vector3 = room.gameObject.transform.TransformPoint(v);
					Vector2 lhs = Vector2.zero;
					if (dictionary.ContainsKey(vector3))
					{
						lhs = dictionary[vector3];
					}
					else if (vector3.x == x)
					{
						lhs = new Vector2(-num, 0f);
					}
					else if (vector3.x == num2)
					{
						lhs = new Vector2(num, 0f);
					}
					else if (vector3.y == num3)
					{
						lhs = new Vector2(0f, num);
					}
					else if (vector3.y == y)
					{
						lhs = new Vector2(0f, -num);
					}
					lhs != Vector2.zero;
				}
			}
		}
	}

	// Token: 0x060051EB RID: 20971 RVA: 0x00137244 File Offset: 0x00135444
	public static void SimplifyAllFerr2DColliders(BaseRoom room, bool rebuild, bool applyBoxConversion)
	{
		if (room.TerrainManager == null)
		{
			room.SetTerrainManager(new RoomTerrainManager(room));
		}
		else
		{
			room.TerrainManager.Initialize();
		}
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in room.TerrainManager.Platforms)
		{
			if (!ferr2DT_PathTerrain.SimpleCollider)
			{
				ferr2DT_PathTerrain.SimpleCollider = true;
				if (ferr2DT_PathTerrain.GetComponent<Collider2D>() && (rebuild || applyBoxConversion))
				{
					ferr2DT_PathTerrain.PathData.SetDirty();
					ferr2DT_PathTerrain.RecreateCollider();
				}
			}
			if (applyBoxConversion && ferr2DT_PathTerrain.FillMode == Ferr2D_SectionMode.Normal)
			{
				PolygonCollider2D component = ferr2DT_PathTerrain.GetComponent<PolygonCollider2D>();
				if (component && CDGHelper.ConvertPolyToBoxCollider(component, true))
				{
					ferr2DT_PathTerrain.RecreateCollider();
				}
			}
		}
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain2 in room.TerrainManager.OneWays)
		{
			if (!ferr2DT_PathTerrain2.SimpleCollider)
			{
				ferr2DT_PathTerrain2.SimpleCollider = true;
				if (ferr2DT_PathTerrain2.GetComponent<Collider2D>() != null && (rebuild || applyBoxConversion))
				{
					ferr2DT_PathTerrain2.PathData.SetDirty();
					ferr2DT_PathTerrain2.RecreateCollider();
				}
			}
			if (applyBoxConversion && ferr2DT_PathTerrain2.FillMode == Ferr2D_SectionMode.Normal)
			{
				PolygonCollider2D component2 = ferr2DT_PathTerrain2.GetComponent<PolygonCollider2D>();
				if (component2 != null && CDGHelper.ConvertPolyToBoxCollider(component2, true) != null)
				{
					ferr2DT_PathTerrain2.RecreateCollider();
				}
			}
		}
	}

	// Token: 0x060051EC RID: 20972 RVA: 0x001373C0 File Offset: 0x001355C0
	public static void CloseUnconnectedDoorsInRoom(Room room)
	{
		if (room == null)
		{
			throw new ArgumentNullException("room");
		}
		List<Door> doors = room.Doors;
		for (int i = doors.Count - 1; i >= 0; i--)
		{
			Door door = doors[i];
			try
			{
				if (!GameUtility.IsInLevelEditor && GridController.GetDoesDoorLeadToDifferentBiome(door))
				{
					door.SetIsBiomeTransitionPoint(GridController.GetDoorLeadsToBiome(door));
				}
				else if (door.ConnectedDoor == null || door.ConnectedDoor.DisabledFromLevelEditor || door.DisabledFromLevelEditor)
				{
					bool replaceWithWall = true;
					if (BiomeCreation_EV.DISABLE_HORIZONTAL_FILL_IN_BIOMES.Contains(room.AppearanceBiomeType) && (door.Side == RoomSide.Left || door.Side == RoomSide.Right))
					{
						replaceWithWall = false;
					}
					else if (BiomeCreation_EV.DISABLE_VERTICAL_FILL_IN_BIOMES.Contains(room.AppearanceBiomeType) && (door.Side == RoomSide.Top || door.Side == RoomSide.Bottom))
					{
						replaceWithWall = false;
					}
					door.Close(replaceWithWall);
				}
			}
			catch (Exception)
			{
				if (door.Room == null)
				{
					Debug.LogFormat("Room is null on Door", Array.Empty<object>());
				}
				throw;
			}
		}
	}

	// Token: 0x04003DB4 RID: 15796
	private static float ROOM_COLLIDER_SCALE = 0.9f;

	// Token: 0x04003DB5 RID: 15797
	private static LayerMask ROOM_COLLISION_CHECK_LAYER_MASK = LayerMask.NameToLayer("LevelBounds");

	// Token: 0x04003DB6 RID: 15798
	private const float OUTER_EDGE_MULTIPLIER = 0.5f;

	// Token: 0x04003DB7 RID: 15799
	private const string LEVEL_MANAGER_NAME = "LevelManager";

	// Token: 0x04003DB8 RID: 15800
	private const string LEVEL_EDITOR_NAME = "LevelEditor";

	// Token: 0x04003DB9 RID: 15801
	private const string DOORS_CHILD_NAME = "Doors";

	// Token: 0x04003DBA RID: 15802
	private const string WALLS_CHILD_NAME = "Walls";

	// Token: 0x04003DBB RID: 15803
	private const string ONE_WAYS_CHILD_NAME = "One Ways";

	// Token: 0x04003DBC RID: 15804
	private const string ENEMIES_CHILD_NAME = "Enemies";

	// Token: 0x04003DBD RID: 15805
	private const string ITEMS_CHILD_NAME = "Items";

	// Token: 0x04003DBE RID: 15806
	private const string CHESTS_CHILD_NAME = "Chests";

	// Token: 0x04003DBF RID: 15807
	private const string CINEMACHINE_VIRTUAL_CAMERA_PREFAB_PATH = "Prefabs/Camera/Cinemachine Virtual Camera";

	// Token: 0x04003DC0 RID: 15808
	private static BiomeCreator m_biomeCreator = null;

	// Token: 0x04003DC1 RID: 15809
	private static List<Weather> m_weatherList_STATIC = new List<Weather>();

	// Token: 0x04003DC2 RID: 15810
	private static List<BackgroundPoolEntry> m_backgroundPoolEntryHelper = new List<BackgroundPoolEntry>();
}

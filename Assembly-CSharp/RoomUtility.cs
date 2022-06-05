using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public static class RoomUtility
{
	// Token: 0x06001C13 RID: 7187 RVA: 0x0005A9F8 File Offset: 0x00058BF8
	public static Vector2 GetDoorCenterPoint(Vector2 roomCoordinates, Vector2Int roomDimensions, DoorLocation doorLocation)
	{
		Vector2 vector = roomDimensions * new Vector2(32f, 18f);
		Vector2 zero = Vector2.zero;
		if (doorLocation.RoomSide == RoomSide.Top || doorLocation.RoomSide == RoomSide.Bottom)
		{
			float x = roomCoordinates.x + ((float)doorLocation.DoorNumber + 0.5f) * 32f;
			float y = roomCoordinates.y;
			if (doorLocation.RoomSide == RoomSide.Top)
			{
				y = roomCoordinates.y + vector.y;
			}
			zero = new Vector2(x, y);
		}
		else
		{
			if (doorLocation.RoomSide != RoomSide.Left && doorLocation.RoomSide != RoomSide.Right)
			{
				throw new ArgumentException("Side", string.Format("Side must be Left, Right, Top or Bottom", Array.Empty<object>()));
			}
			float x2 = roomCoordinates.x;
			if (doorLocation.RoomSide == RoomSide.Right)
			{
				x2 = roomCoordinates.x + vector.x;
			}
			float y2 = roomCoordinates.y + vector.y - ((float)doorLocation.DoorNumber + 0.5f) * 18f;
			zero = new Vector2(x2, y2);
		}
		return zero;
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x0005AB04 File Offset: 0x00058D04
	public static Room CreateMirrorVersionOfRoom(Room room)
	{
		Room room2 = UnityEngine.Object.Instantiate<Room>(room);
		room2.SetIsMirrored(true);
		RoomUtility.m_mirrorRoomHelper.Clear();
		room2.transform.FindAllTransforms(RoomUtility.m_mirrorRoomHelper);
		foreach (Transform transform in RoomUtility.m_mirrorRoomHelper)
		{
			GameObject gameObject = transform.gameObject;
			if (!gameObject.CompareTag("Door"))
			{
				Ferr2DT_PathTerrain component = gameObject.GetComponent<Ferr2DT_PathTerrain>();
				IMirror component2 = gameObject.GetComponent<IMirror>();
				if (component)
				{
					RoomUtility.MirrorFerr2D(component);
				}
				else
				{
					gameObject.transform.localPosition = RoomUtility.GetMirrorPosition(gameObject.transform);
				}
				if (component2 != null)
				{
					component2.Mirror();
				}
			}
		}
		room2.SetSpawnControllerManager(new RoomSpawnControllerManager(room2));
		SpawnLogicController[] spawnLogicControllers = room2.SpawnControllerManager.SpawnLogicControllers;
		for (int i = 0; i < spawnLogicControllers.Length; i++)
		{
			SpawnConditionsEntry[] spawnConditions = spawnLogicControllers[i].SpawnConditions;
			for (int j = 0; j < spawnConditions.Length; j++)
			{
				SpawnScenarioEntry[] scenarios = spawnConditions[j].Scenarios;
				for (int k = 0; k < scenarios.Length; k++)
				{
					Door_SpawnScenario door_SpawnScenario = scenarios[k].Scenario as Door_SpawnScenario;
					if (door_SpawnScenario != null)
					{
						if (door_SpawnScenario.Side == RoomSide.Left || door_SpawnScenario.Side == RoomSide.Right)
						{
							door_SpawnScenario.Side = RoomUtility.GetOppositeSide(door_SpawnScenario.Side);
						}
						else if ((door_SpawnScenario.Side == RoomSide.Top || door_SpawnScenario.Side == RoomSide.Bottom) && door_SpawnScenario.Number != -1)
						{
							door_SpawnScenario.Number = RoomUtility.GetMirrorDoorNumber(new DoorLocation(door_SpawnScenario.Side, door_SpawnScenario.Number), room2.Size);
						}
					}
				}
			}
		}
		foreach (Door door in room2.Doors)
		{
			RoomUtility.MirrorFerr2D(door.Ferr2D);
			DoorLocation mirrorDoorLocation = RoomUtility.GetMirrorDoorLocation(room2, new DoorLocation(door.Side, door.Number));
			door.Side = mirrorDoorLocation.RoomSide;
			door.Number = mirrorDoorLocation.DoorNumber;
			if (door.Side == RoomSide.Left || door.Side == RoomSide.Right)
			{
				door.GameObject.name = door.Side.ToString() + " Door";
			}
		}
		return room2;
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x0005AD8C File Offset: 0x00058F8C
	public static Vector2 GetDoorCoordinates(Room room, Door door)
	{
		if (door.Side == RoomSide.Left || door.Side == RoomSide.Right)
		{
			float x = room.Coordinates.x;
			if (door.Side == RoomSide.Right)
			{
				x = room.Coordinates.x + (float)room.UnitDimensions.x;
			}
			return new Vector2(x, room.Coordinates.y + (float)((room.Size.y - 1 - door.Number) * 18));
		}
		if (door.Side == RoomSide.Top || door.Side == RoomSide.Bottom)
		{
			float y = room.Coordinates.y;
			if (door.Side == RoomSide.Top)
			{
				y = room.Coordinates.y + (float)room.UnitDimensions.y;
			}
			return new Vector2(room.Coordinates.x + (float)(door.Number * 32), y);
		}
		throw new ArgumentException("Side", string.Format("Side must be Left, Right, Top or Bottom", Array.Empty<object>()));
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x0005AE81 File Offset: 0x00059081
	public static IEnumerable<DoorLocation> GetDoorLocations(GridPoint gridPoint)
	{
		DoorLocation[] doors = gridPoint.RoomMetaData.DoorLocations;
		int num;
		for (int i = 0; i < doors.Length; i = num + 1)
		{
			DoorLocation doorLocation = doors[i];
			if (gridPoint.IsMirrored)
			{
				doorLocation = RoomUtility.GetMirrorDoorLocation(gridPoint.RoomMetaData.Size, doorLocation);
			}
			yield return doorLocation;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x0005AE91 File Offset: 0x00059091
	public static Vector3 GetMirrorPosition(Transform transform)
	{
		return new Vector3(-1f * transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x0005AEBF File Offset: 0x000590BF
	public static DoorLocation GetMirrorDoorLocation(Room room, DoorLocation doorLocation)
	{
		return RoomUtility.GetMirrorDoorLocation(room.Size, doorLocation);
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x0005AED0 File Offset: 0x000590D0
	public static DoorLocation GetMirrorDoorLocation(Vector2Int roomSize, DoorLocation doorLocation)
	{
		RoomSide roomSide = doorLocation.RoomSide;
		int mirrorDoorNumber = RoomUtility.GetMirrorDoorNumber(doorLocation, roomSize);
		if (doorLocation.RoomSide == RoomSide.Left || doorLocation.RoomSide == RoomSide.Right)
		{
			roomSide = RoomUtility.GetOppositeSide(doorLocation.RoomSide);
		}
		return new DoorLocation(roomSide, mirrorDoorNumber);
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x0005AF18 File Offset: 0x00059118
	public static List<DoorLocation> GetMirroredDoorLocations(RoomMetaData roomMetaData)
	{
		List<DoorLocation> list = new List<DoorLocation>();
		for (int i = 0; i < roomMetaData.DoorLocations.Length; i++)
		{
			list.Add(RoomUtility.GetMirrorDoorLocation(roomMetaData.Size, roomMetaData.DoorLocations[i]));
		}
		return list;
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x0005AF5C File Offset: 0x0005915C
	public static RoomSide GetMirrorSide(RoomSide side)
	{
		if (side == RoomSide.Top || side == RoomSide.Bottom)
		{
			return side;
		}
		return RoomUtility.GetOppositeSide(side);
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x0005AF6D File Offset: 0x0005916D
	public static int GetMirrorDoorNumber(DoorLocation doorLocation, Vector2Int roomSize)
	{
		if (doorLocation.RoomSide == RoomSide.Top || doorLocation.RoomSide == RoomSide.Bottom)
		{
			return roomSize.x - 1 - doorLocation.DoorNumber;
		}
		return doorLocation.DoorNumber;
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x0005AF9B File Offset: 0x0005919B
	public static int GetMirrorDoorNumber(RoomSide side, int doorNumber, Vector2Int roomSize)
	{
		return RoomUtility.GetMirrorDoorNumber(new DoorLocation(side, doorNumber), roomSize);
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x0005AFAA File Offset: 0x000591AA
	public static RoomSide GetOppositeSide(RoomSide roomSide)
	{
		switch (roomSide)
		{
		case RoomSide.Top:
			return RoomSide.Bottom;
		case RoomSide.Bottom:
			return RoomSide.Top;
		case RoomSide.Left:
			return RoomSide.Right;
		case RoomSide.Right:
			return RoomSide.Left;
		default:
			return RoomSide.None;
		}
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x0005AFD0 File Offset: 0x000591D0
	public static int GetRoomLevel(BiomeType biome, int roomNumber)
	{
		biome = BiomeType_RL.GetGroupedBiomeType(biome);
		BiomeData data = BiomeDataLibrary.GetData(biome);
		if (data != null)
		{
			float num = (data.RoomLevelScale > 0f) ? data.RoomLevelScale : 1f;
			int num2 = data.EnemyStartLevel * Mathf.RoundToInt(1f + BurdenManager.GetBurdenStatGain(BurdenType.RoomCount));
			return Mathf.FloorToInt((float)roomNumber / num) + num2;
		}
		if (biome != BiomeType.Lineage)
		{
			Debug.LogFormat("<color=red>[RoomUtility] Failed to retrieve Biome Data for Biome ({0})</color>", new object[]
			{
				biome
			});
		}
		return 1;
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x0005B055 File Offset: 0x00059255
	public static RoomType GetRoomType(Room room)
	{
		if (!(room != null))
		{
			return RoomType.None;
		}
		if (room.gameObject.GetComponent<FairyRoomController>())
		{
			return RoomType.Fairy;
		}
		if (room.gameObject.GetComponent<BossRoomController>())
		{
			return RoomType.Boss;
		}
		return RoomType.Standard;
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x0005B094 File Offset: 0x00059294
	public static void InjectRoomReferenceIntoChildrenOfGameObject(BaseRoom room, GameObject rootGameObject)
	{
		IRoomConsumer[] componentsInChildren = rootGameObject.GetComponentsInChildren<IRoomConsumer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetRoom(room);
		}
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x0005B0C0 File Offset: 0x000592C0
	public static void MirrorFerr2D(Ferr2DT_PathTerrain ferr2D)
	{
		Ferr2DUtilities.Flip(ferr2D, true, false, false);
		ferr2D.transform.localPosition = new Vector2(-1f * ferr2D.transform.localPosition.x, ferr2D.transform.localPosition.y);
		IHitboxController componentInChildren = ferr2D.GetComponentInChildren<IHitboxController>();
		if (componentInChildren != null)
		{
			componentInChildren.RealignHitboxColliders();
		}
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x0005B124 File Offset: 0x00059324
	public static void BuildAllFerr2DTerrains(BaseRoom room)
	{
		MergeRoomTools.MergeTerrain(room);
		List<Ferr2DT_PathTerrain> oneWays = room.TerrainManager.OneWays;
		for (int i = 0; i < oneWays.Count; i++)
		{
			Ferr2DT_PathTerrain ferr2DT_PathTerrain = oneWays[i];
			oneWays[i].Build(true);
		}
		Ferr2DT_PathTerrain[] hazards = room.TerrainManager.Hazards;
		for (int j = 0; j < hazards.Length; j++)
		{
			hazards[j].BuildMeshOnly(false);
		}
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x0005B190 File Offset: 0x00059390
	public static void ShuffleList<T>(List<T> list)
	{
		if (list.Count <= 1)
		{
			return;
		}
		string text = "";
		for (int i = 0; i < list.Count; i++)
		{
			string str = text;
			T t = list[i];
			text = str + t.ToString();
			if (i < list.Count - 1)
			{
				text += ", ";
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			int index = j + RNGManager.GetRandomNumber(RngID.BiomeCreation, string.Format("Shuffle List. {0}", text), 0, list.Count - j);
			T value = list[index];
			list[index] = list[j];
			list[j] = value;
		}
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x0005B244 File Offset: 0x00059444
	public static bool GetIsRoomLarge(Vector2Int roomSize)
	{
		bool result = false;
		if (roomSize.x > RoomUtility.MINIMUM_LARGE_ROOM_SIZE.x || roomSize.y > RoomUtility.MINIMUM_LARGE_ROOM_SIZE.y)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x0005B27C File Offset: 0x0005947C
	public static bool GetIsRoomLarge(BaseRoom room)
	{
		if (room == null)
		{
			throw new ArgumentNullException("room");
		}
		return !(room is Room) || RoomUtility.GetIsRoomLarge((room as Room).Size);
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x0005B2AC File Offset: 0x000594AC
	public static int GetHashedRoomID(RoomID id, bool isMirrored)
	{
		return ((13 * 397 ^ id.SceneName.GetHashCode()) * 397 ^ id.Number) * 397 ^ ((!isMirrored) ? 0 : 1);
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x0005B2E0 File Offset: 0x000594E0
	public static int GetDoorLocationMask(RoomSide side, List<DoorLocation> locations)
	{
		int num = 0;
		for (int i = 0; i < locations.Count; i++)
		{
			num |= 1 << (int)(side * RoomSide.Right + locations[i].DoorNumber);
		}
		return num;
	}

	// Token: 0x0400198C RID: 6540
	private static List<Transform> m_mirrorRoomHelper = new List<Transform>();

	// Token: 0x0400198D RID: 6541
	private static Vector2Int MINIMUM_LARGE_ROOM_SIZE = 2 * Vector2Int.one;
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000ACC RID: 2764
public class CreateTunnelRooms_BuildStage : IBiomeBuildStage
{
	// Token: 0x06005312 RID: 21266 RVA: 0x0002D2B3 File Offset: 0x0002B4B3
	public IEnumerator Run(BiomeController biomeController)
	{
		TunnelSpawnController[] tunnelSpawnControllers = biomeController.TunnelSpawnControllers;
		if (tunnelSpawnControllers != null && tunnelSpawnControllers.Length != 0)
		{
			RNGManager.SetSeed(RngID.Tunnel_RoomSeed, (tunnelSpawnControllers[0].Room as Room).GridPointManager.TunnelRoomSeed);
			int num;
			for (int i = 0; i < tunnelSpawnControllers.Length; i = num + 1)
			{
				if (biomeController.Biome != BiomeType.Castle || !tunnelSpawnControllers[i].ShouldSpawn || !(tunnelSpawnControllers[i].Room != null) || tunnelSpawnControllers[i].Room.RoomType != RoomType.Transition || !(tunnelSpawnControllers[i].Tunnel != null))
				{
					if (tunnelSpawnControllers[i].SpawnLogicController != null)
					{
						tunnelSpawnControllers[i].SpawnLogicController.RunIsSpawnedCheck(SpawnScenarioCheckStage.PreMerge);
					}
					if (tunnelSpawnControllers[i].ShouldSpawn && tunnelSpawnControllers[i].Direction == TunnelDirection.Entrance)
					{
						tunnelSpawnControllers[i].Spawn();
						CreateTunnelRooms_BuildStage.m_destinationRooms.Clear();
						CreateTunnelRooms_BuildStage.m_rootToRoomsTable.Clear();
						yield return CreateTunnelRooms_BuildStage.CreateDestination(biomeController, tunnelSpawnControllers[i], null);
					}
				}
				num = i;
			}
		}
		yield break;
	}

	// Token: 0x06005313 RID: 21267 RVA: 0x0002D2C2 File Offset: 0x0002B4C2
	public static IEnumerator RunFromLevelEditor(BiomeController biomeController, Room room)
	{
		if (!GameUtility.IsInLevelEditor)
		{
			yield break;
		}
		CreateTunnelRooms_BuildStage.m_destinationRooms.Clear();
		CreateTunnelRooms_BuildStage.m_rootToRoomsTable.Clear();
		TunnelSpawnController[] tunnelSpawnControllers = room.GetComponentsInChildren<TunnelSpawnController>(true);
		if (tunnelSpawnControllers != null && tunnelSpawnControllers.Length != 0)
		{
			int num;
			for (int i = 0; i < tunnelSpawnControllers.Length; i = num + 1)
			{
				if (tunnelSpawnControllers[i].ShouldSpawn && tunnelSpawnControllers[i].Direction == TunnelDirection.Entrance)
				{
					yield return CreateTunnelRooms_BuildStage.CreateDestination(biomeController, tunnelSpawnControllers[i], null);
				}
				num = i;
			}
		}
		yield break;
	}

	// Token: 0x06005314 RID: 21268 RVA: 0x0002D2D8 File Offset: 0x0002B4D8
	private static Tunnel GetTunnelPrefab(TunnelCategory category)
	{
		return TunnelLibrary.GetPrefab(category);
	}

	// Token: 0x06005315 RID: 21269 RVA: 0x0002D2E0 File Offset: 0x0002B4E0
	private static IEnumerator CreateDestination(BiomeController biomeController, TunnelSpawnController tunnelSpawnController, Tunnel entrance)
	{
		Tunnel tunnel = CreateTunnelRooms_BuildStage.CreateTunnelInstance(tunnelSpawnController, entrance);
		if (tunnelSpawnController.Direction == TunnelDirection.Entrance)
		{
			if (!tunnelSpawnController.IsDestinationRoot)
			{
				RoomType roomType;
				if (!CreateTunnelRooms_BuildStage.GetIsTunnelToSpecialRoom(tunnelSpawnController, out roomType))
				{
					Room roomInstance;
					if (!CreateTunnelRooms_BuildStage.GetHasRoomInChainBeenInstantiated(tunnelSpawnController, out roomInstance))
					{
						yield return CreateTunnelRooms_BuildStage.DestinationDoesNotExist(biomeController, tunnel, tunnelSpawnController);
					}
					else
					{
						CreateTunnelRooms_BuildStage.DestinationAlreadyExists(tunnel, roomInstance);
					}
				}
				else
				{
					int roomSeed = CreateTunnelRooms_BuildStage.GetRoomSeed(tunnelSpawnController.Room);
					GridPointManager gridPointManager = CreateTunnelRooms_BuildStage.CreateLobbyGridPointManager(biomeController, roomType);
					gridPointManager.SetRoomSeed(roomSeed);
					GridPointManager gridPointManager2 = CreateTunnelRooms_BuildStage.CreateSpecialRoomGridPointManager(biomeController, gridPointManager, roomType);
					gridPointManager2.SetRoomSeed(roomSeed);
					yield return CreateTunnelRooms_BuildStage.CreateSpecialRoomDestination(biomeController, tunnel, gridPointManager, gridPointManager2, roomSeed);
				}
			}
		}
		else if (entrance.Index == tunnel.Index)
		{
			CreateTunnelRooms_BuildStage.ConnectTunnels(entrance, tunnel);
		}
		if (entrance)
		{
			tunnelSpawnController.Room.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06005316 RID: 21270 RVA: 0x0002D2FD File Offset: 0x0002B4FD
	private static IEnumerator DestinationDoesNotExist(BiomeController biomeController, Tunnel tunnelInstance, TunnelSpawnController tunnelSpawnController)
	{
		Room room = CreateTunnelRooms_BuildStage.CreateRoomInstance(biomeController, tunnelSpawnController.DestinationRoomPrefab, tunnelSpawnController.LeadsToRoomType);
		if (!CreateTunnelRooms_BuildStage.m_destinationRooms.Contains(room.RoomID))
		{
			CreateTunnelRooms_BuildStage.m_destinationRooms.Add(room.RoomID);
			CreateTunnelRooms_BuildStage.m_rootToRoomsTable[tunnelInstance.Root].Add(room);
			CreateTunnelRooms_BuildStage.InitializeDestinationRoom(biomeController, tunnelSpawnController, room, CreateTunnelRooms_BuildStage.DESTINATION_GRID_COORDINATES);
			TunnelSpawnController[] destinationTunnelSpawnControllers = room.GetComponentsInChildren<TunnelSpawnController>(true);
			int num;
			for (int i = 0; i < destinationTunnelSpawnControllers.Length; i = num + 1)
			{
				yield return CreateTunnelRooms_BuildStage.CreateDestination(biomeController, destinationTunnelSpawnControllers[i], tunnelInstance);
				num = i;
			}
			destinationTunnelSpawnControllers = null;
		}
		else
		{
			bool isEditor = Application.isEditor;
		}
		yield break;
	}

	// Token: 0x06005317 RID: 21271 RVA: 0x0013AB54 File Offset: 0x00138D54
	private static void DestinationAlreadyExists(Tunnel tunnelInstance, Room roomInstance)
	{
		ISpawnController[] spawnControllers = roomInstance.SpawnControllerManager.SpawnControllers;
		int i = 0;
		while (i < spawnControllers.Length)
		{
			TunnelSpawnController tunnelSpawnController = spawnControllers[i] as TunnelSpawnController;
			if (tunnelSpawnController != null && tunnelSpawnController.Index == tunnelInstance.Index)
			{
				if (tunnelSpawnController.Direction != TunnelDirection.Exit || !tunnelSpawnController.ShouldSpawn)
				{
					throw new InvalidOperationException("An error occurred while attempting to connect two Tunnels");
				}
				Tunnel tunnel = tunnelSpawnController.Tunnel;
				if (tunnel == null)
				{
					tunnel = CreateTunnelRooms_BuildStage.CreateTunnelInstance(tunnelSpawnController);
					tunnel.SetRoot(tunnelInstance.Root);
					tunnelSpawnController.SetTunnelInstance(tunnel);
				}
				CreateTunnelRooms_BuildStage.ConnectTunnels(tunnelInstance, tunnelSpawnController.Tunnel);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06005318 RID: 21272 RVA: 0x0013ABF0 File Offset: 0x00138DF0
	private static Tunnel CreateTunnelInstance(TunnelSpawnController tunnelSpawnController, Tunnel entrance)
	{
		if (!tunnelSpawnController.IsDestinationRoot || !(entrance == null))
		{
			Tunnel tunnel = CreateTunnelRooms_BuildStage.CreateTunnelInstance(tunnelSpawnController);
			tunnel.SetIsDestinationRoot(tunnelSpawnController.IsDestinationRoot);
			if (entrance == null)
			{
				CreateTunnelRooms_BuildStage.m_rootToRoomsTable.Add(tunnel, new List<Room>());
				tunnel.SetRoot(tunnel);
			}
			else
			{
				tunnel.SetRoot(entrance.Root);
			}
			tunnelSpawnController.SetTunnelInstance(tunnel);
			return tunnel;
		}
		if (!GameUtility.IsInLevelEditor)
		{
			throw new ArgumentException(string.Format("| CreateTunnelRooms_BuildStage | Given Spawn Controller's destination is Root, but entrance argument is null.", Array.Empty<object>()));
		}
		Debug.Log(string.Format("<color=red>| CreateTunnelRooms_BuildStage | Given Spawn Controller's destination is Root, but entrance argument is null.</color>", Array.Empty<object>()));
		return null;
	}

	// Token: 0x06005319 RID: 21273 RVA: 0x0013AC8C File Offset: 0x00138E8C
	private static bool GetHasRoomInChainBeenInstantiated(TunnelSpawnController spawnController, out Room room)
	{
		room = null;
		bool result = false;
		Tunnel root = spawnController.Tunnel.Root;
		List<Room> list = CreateTunnelRooms_BuildStage.m_rootToRoomsTable[root];
		for (int i = 0; i < list.Count; i++)
		{
			Room room2 = CreateTunnelRooms_BuildStage.m_rootToRoomsTable[root][i];
			if (spawnController.DestinationRoomPrefab.RoomID == room2.RoomID)
			{
				result = true;
				room = room2;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600531A RID: 21274 RVA: 0x0013ACFC File Offset: 0x00138EFC
	private static GridPointManager CreateSpecialRoomGridPointManager(BiomeController biomeController, GridPointManager lobby, RoomType roomType)
	{
		if (RoomLibrary.IsInstantiated && !RoomLibrary.IsLoaded)
		{
			RoomLibrary.LoadCompiledRoomsFromLevelEditor();
		}
		List<RoomSetEntry> list = RoomLibrary.GetSetCollection(BiomeType.Castle).RoomTypeSets[roomType].ToList<RoomSetEntry>();
		if (roomType == RoomType.Bonus)
		{
			SpecialRoomType randomBonusRoomType = CreateRoomsBuildRule.GetRandomBonusRoomType(biomeController, RngID.Tunnel_RoomSeed, null);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].RoomMetaData.SpecialRoomType != randomBonusRoomType)
				{
					list.RemoveAt(i);
				}
			}
		}
		int index = 0;
		if (list.Count > 1)
		{
			index = RNGManager.GetRandomNumber(RngID.Tunnel_RoomSeed, "Get Random Special Room for Tunnel Destination", 0, list.Count);
		}
		RoomSetEntry roomSetEntry = list[index];
		int num = 0;
		if (roomSetEntry.RoomMetaData.DoorLocations.Length > 1)
		{
			num = RNGManager.GetRandomNumber(RngID.Tunnel_RoomSeed, "Choose which Door will connect to Lobby", 0, roomSetEntry.RoomMetaData.DoorLocations.Length);
		}
		DoorLocation doorLocation = roomSetEntry.RoomMetaData.DoorLocations[num];
		if (roomSetEntry.IsMirrored)
		{
			doorLocation = RoomUtility.GetMirrorDoorLocation(roomSetEntry.RoomMetaData.Size, doorLocation);
		}
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(doorLocation.RoomSide);
		DoorLocation location = new DoorLocation(oppositeSide, 0);
		Vector2Int roomGridCoordinates = GridController.GetRoomGridCoordinates(lobby.GetDoorLeadsToGridCoordinates(location), roomSetEntry.RoomMetaData.Size, doorLocation);
		BiomeType tunnelDestinationBiome = CreateTunnelRooms_BuildStage.GetTunnelDestinationBiome(biomeController);
		return CreateTunnelRooms_BuildStage.CreateGridPointManager(biomeController, roomSetEntry.RoomMetaData, roomType, roomGridCoordinates, roomSetEntry.IsMirrored, tunnelDestinationBiome);
	}

	// Token: 0x0600531B RID: 21275 RVA: 0x0013AE64 File Offset: 0x00139064
	private static BiomeType GetTunnelDestinationBiome(BiomeController biomeController)
	{
		BiomeType result = biomeController.Biome;
		if (BiomeCreation_EV.TUNNEL_DESTINATION_BIOME_TYPE_TABLE.ContainsKey(biomeController.Biome))
		{
			result = BiomeCreation_EV.TUNNEL_DESTINATION_BIOME_TYPE_TABLE[biomeController.Biome];
		}
		return result;
	}

	// Token: 0x0600531C RID: 21276 RVA: 0x0013AE9C File Offset: 0x0013909C
	private static GridPointManager CreateLobbyGridPointManager(BiomeController biomeController, RoomType roomType)
	{
		if (CreateTunnelRooms_BuildStage.m_lobbyMetaData == null)
		{
			CreateTunnelRooms_BuildStage.m_lobbyMetaData = CDGResources.Load<RoomMetaData>("Scriptable Objects/Room Meta Data/Levels Linker/Levels Linker_28_RoomMetaData", "", true);
		}
		BiomeType tunnelDestinationBiome = CreateTunnelRooms_BuildStage.GetTunnelDestinationBiome(biomeController);
		return CreateTunnelRooms_BuildStage.CreateGridPointManager(biomeController, CreateTunnelRooms_BuildStage.m_lobbyMetaData, RoomType.Standard, CreateTunnelRooms_BuildStage.DESTINATION_GRID_COORDINATES, false, tunnelDestinationBiome);
	}

	// Token: 0x0600531D RID: 21277 RVA: 0x0013AEE8 File Offset: 0x001390E8
	private static bool GetIsTunnelToSpecialRoom(TunnelSpawnController tunnelSpawnController, out RoomType specialRoomType)
	{
		bool flag = tunnelSpawnController.LeadsToRoomType == RoomType.Fairy || tunnelSpawnController.LeadsToRoomType == RoomType.Bonus;
		bool flag2 = tunnelSpawnController.DestinationRoomPrefabsOverride != null && tunnelSpawnController.DestinationRoomPrefabsOverride.Length == 0;
		if (flag && flag2)
		{
			specialRoomType = tunnelSpawnController.LeadsToRoomType;
			return true;
		}
		specialRoomType = RoomType.None;
		return false;
	}

	// Token: 0x0600531E RID: 21278 RVA: 0x0002D31A File Offset: 0x0002B51A
	private static void ConnectTunnels(Tunnel entrance, Tunnel exit)
	{
		entrance.SetDestination(exit);
		exit.SetDestination(entrance);
		exit.SetRoot(entrance.Root);
	}

	// Token: 0x0600531F RID: 21279 RVA: 0x0013AF38 File Offset: 0x00139138
	private static Tunnel CreateTunnelInstance(TunnelSpawnController tunnelSpawnController)
	{
		Tunnel tunnel = null;
		if (tunnelSpawnController.TunnelPrefabOverride != null)
		{
			tunnel = tunnelSpawnController.TunnelPrefabOverride.GetComponent<Tunnel>();
		}
		if (tunnel == null)
		{
			tunnel = CreateTunnelRooms_BuildStage.GetTunnelPrefab(tunnelSpawnController.Category);
		}
		Tunnel tunnel2 = UnityEngine.Object.Instantiate<Tunnel>(tunnel, tunnelSpawnController.Room.gameObject.transform);
		tunnel2.SetIsLocked(tunnelSpawnController.IsLocked);
		tunnel2.SetRoom(tunnelSpawnController.Room);
		tunnel2.SetIndex(tunnelSpawnController.Index);
		tunnel2.SetIsVisible(tunnelSpawnController.ShowTunnel);
		tunnel2.SetTransitionType(tunnelSpawnController.TransitionType);
		tunnel2.transform.position = tunnelSpawnController.GameObject.transform.position;
		return tunnel2;
	}

	// Token: 0x06005320 RID: 21280 RVA: 0x0013AFE4 File Offset: 0x001391E4
	private static Room CreateRoomInstance(BiomeController biomeController, Room roomPrefab, RoomType roomType)
	{
		Transform parent = null;
		if (biomeController)
		{
			parent = biomeController.StandardRoomStorageLocation;
			if (roomType == RoomType.Boss)
			{
				parent = biomeController.RoomsConnectedByTunnelStorageLocation;
			}
		}
		Room room = UnityEngine.Object.Instantiate<Room>(roomPrefab, parent);
		room.BiomeType = biomeController.Biome;
		CreateTunnelRooms_BuildStage.InvokeRoomCreatedEvent(room);
		return room;
	}

	// Token: 0x06005321 RID: 21281 RVA: 0x0002D336 File Offset: 0x0002B536
	private static void InvokeRoomCreatedEvent(Room room)
	{
		if (CreateTunnelRooms_BuildStage.m_roomCreatedEventArgs == null)
		{
			CreateTunnelRooms_BuildStage.m_roomCreatedEventArgs = new RoomEventArgs(room);
		}
		else
		{
			CreateTunnelRooms_BuildStage.m_roomCreatedEventArgs.Initialize(room);
		}
		if (CreateTunnelRooms_BuildStage.RoomCreatedEvent != null)
		{
			CreateTunnelRooms_BuildStage.RoomCreatedEvent(null, CreateTunnelRooms_BuildStage.m_roomCreatedEventArgs);
		}
	}

	// Token: 0x06005322 RID: 21282 RVA: 0x0002D36E File Offset: 0x0002B56E
	private static int GetNextRoomNumber(BiomeController biomeController)
	{
		return biomeController.GridPointManager.TunnelDestinationGridPointManagers.Count;
	}

	// Token: 0x06005323 RID: 21283 RVA: 0x0002D380 File Offset: 0x0002B580
	private static int GetNextBiomeControllerIndex(BiomeController biomeController)
	{
		return biomeController.GridPointManager.StandaloneRoomCount + biomeController.GridPointManager.MergeRoomCount + biomeController.GridPointManager.TunnelDestinationCount - 1;
	}

	// Token: 0x06005324 RID: 21284 RVA: 0x0013B028 File Offset: 0x00139228
	private static GridPointManager CreateGridPointManager(BiomeController biomeController, RoomMetaData roomMetaData, RoomType roomType, Vector2Int gridCoords, bool isRoomMirrored = false, BiomeType biomeOverride = BiomeType.None)
	{
		GridPointManager gridPointManager = new GridPointManager(CreateTunnelRooms_BuildStage.GetNextRoomNumber(biomeController), gridCoords, biomeController.Biome, roomType, roomMetaData, isRoomMirrored, true);
		if (biomeOverride != BiomeType.None && gridPointManager.AppearanceOverride == BiomeType.None)
		{
			gridPointManager.SetAppearanceOverride(biomeOverride);
		}
		biomeController.GridPointManager.AddTunnelDestinationGridPointManager(gridPointManager);
		int nextBiomeControllerIndex = CreateTunnelRooms_BuildStage.GetNextBiomeControllerIndex(biomeController);
		gridPointManager.SetBiomeControllerIndex(nextBiomeControllerIndex);
		return gridPointManager;
	}

	// Token: 0x06005325 RID: 21285 RVA: 0x0002D3A7 File Offset: 0x0002B5A7
	private static IEnumerator CreateSpecialRoomDestination(BiomeController biomeController, Tunnel tunnelToLobby, GridPointManager lobby, GridPointManager room, int roomSeed)
	{
		Room lobbyInstance = InstantiateRooms_BuildStage.CreateRoomInstance(biomeController, lobby);
		lobbyInstance.gameObject.transform.parent = biomeController.RoomsConnectedByTunnelStorageLocation;
		biomeController.RoomConnectedByTunnelCreatedByWorldBuilder(lobbyInstance);
		CreateTunnelRooms_BuildStage.InvokeRoomCreatedEvent(lobbyInstance);
		Tunnel exit = CreateTunnelRooms_BuildStage.CreateTunnelInstance(lobbyInstance.gameObject.GetComponentInChildren<TunnelSpawnController>());
		CreateTunnelRooms_BuildStage.ConnectTunnels(tunnelToLobby, exit);
		Room roomInstance = InstantiateRooms_BuildStage.CreateRoomInstance(biomeController, room);
		roomInstance.gameObject.transform.parent = biomeController.RoomsConnectedByTunnelStorageLocation;
		yield return new WaitUntil(() => lobbyInstance.Doors.Any((Door door) => door.ConnectedDoor != null));
		List<Door> doorsOnSide = lobbyInstance.GetDoorsOnSide(RoomSide.Bottom);
		for (int j = 0; j < doorsOnSide.Count; j++)
		{
			BiomeCreatorTools.PlaceOneWayAtDoor(doorsOnSide[j]);
		}
		List<Door> doorsOnSide2 = roomInstance.GetDoorsOnSide(RoomSide.Bottom);
		for (int k = 0; k < doorsOnSide2.Count; k++)
		{
			BiomeCreatorTools.PlaceOneWayAtDoor(doorsOnSide2[k]);
		}
		biomeController.RoomConnectedByTunnelCreatedByWorldBuilder(roomInstance);
		CreateTunnelRooms_BuildStage.InvokeRoomCreatedEvent(roomInstance);
		int biomeControllerIndex = lobby.BiomeControllerIndex;
		lobbyInstance.SetBiomeControllerIndex(biomeControllerIndex);
		int biomeControllerIndex2 = room.BiomeControllerIndex;
		roomInstance.SetBiomeControllerIndex(biomeControllerIndex2);
		CreateTunnelRooms_BuildStage.RunSpawnLogicOnRoom(lobbyInstance);
		CreateTunnelRooms_BuildStage.RunSpawnLogicOnRoom(roomInstance);
		if (!GameUtility.IsInLevelEditor)
		{
			BiomeCreatorTools.CreateBackground(lobbyInstance);
			BiomeCreatorTools.CreateBackground(roomInstance);
		}
		lobbyInstance.gameObject.SetActive(false);
		roomInstance.gameObject.SetActive(false);
		TunnelSpawnController[] tunnelSpawnControllers = roomInstance.gameObject.GetComponentsInChildren<TunnelSpawnController>(true);
		if (tunnelSpawnControllers != null && tunnelSpawnControllers.Length != 0)
		{
			int num;
			for (int i = 0; i < tunnelSpawnControllers.Length; i = num + 1)
			{
				if (tunnelSpawnControllers[i].SpawnLogicController != null)
				{
					tunnelSpawnControllers[i].SpawnLogicController.RunIsSpawnedCheck(SpawnScenarioCheckStage.PreMerge);
				}
				if (tunnelSpawnControllers[i].ShouldSpawn && tunnelSpawnControllers[i].Direction == TunnelDirection.Entrance)
				{
					tunnelSpawnControllers[i].Spawn();
					CreateTunnelRooms_BuildStage.m_destinationRooms.Clear();
					CreateTunnelRooms_BuildStage.m_rootToRoomsTable.Clear();
					yield return CreateTunnelRooms_BuildStage.CreateDestination(biomeController, tunnelSpawnControllers[i], null);
				}
				num = i;
			}
		}
		yield break;
	}

	// Token: 0x06005326 RID: 21286 RVA: 0x0013B07C File Offset: 0x0013927C
	private static void InitializeDestinationRoom(BiomeController biomeController, TunnelSpawnController tunnelSpawnController, Room destination, Vector2Int gridCoords)
	{
		GridPointManager gridPointManager = CreateTunnelRooms_BuildStage.CreateGridPointManager(biomeController, tunnelSpawnController.RoomMetaData, tunnelSpawnController.LeadsToRoomType, gridCoords, false, BiomeType.None);
		if (gridPointManager.AppearanceOverride == BiomeType.None)
		{
			BiomeType tunnelDestinationBiome = CreateTunnelRooms_BuildStage.GetTunnelDestinationBiome(biomeController);
			gridPointManager.SetAppearanceOverride(tunnelDestinationBiome);
		}
		destination.Initialise(gridPointManager);
		destination.SetBiomeControllerIndex(gridPointManager.BiomeControllerIndex);
		int roomSeed = CreateTunnelRooms_BuildStage.GetRoomSeed(tunnelSpawnController.Room);
		gridPointManager.SetRoomSeed(roomSeed);
		CreateTunnelRooms_BuildStage.RunSpawnLogicOnRoom(destination);
		destination.transform.parent = biomeController.RoomsConnectedByTunnelStorageLocation;
		Vector2 roomCoordinatesFromGridCoordinates = GridController.GetRoomCoordinatesFromGridCoordinates(CreateTunnelRooms_BuildStage.DESTINATION_GRID_COORDINATES);
		destination.transform.position = GridController.GetWorldPositionFromRoomCoordinates(roomCoordinatesFromGridCoordinates, tunnelSpawnController.RoomMetaData.Size);
		if (biomeController)
		{
			biomeController.RoomConnectedByTunnelCreatedByWorldBuilder(destination);
		}
		if (!GameUtility.IsInLevelEditor)
		{
			BiomeCreatorTools.CreateBackground(destination);
		}
	}

	// Token: 0x06005327 RID: 21287 RVA: 0x0013B13C File Offset: 0x0013933C
	private static int GetRoomSeed(BaseRoom room)
	{
		int roomSeed;
		if (room is Room)
		{
			roomSeed = (room as Room).GridPointManager.RoomSeed;
		}
		else
		{
			roomSeed = (room as MergeRoom).StandaloneGridPointManagers[0].RoomSeed;
		}
		return roomSeed;
	}

	// Token: 0x06005328 RID: 21288 RVA: 0x0013B17C File Offset: 0x0013937C
	private static void RunSpawnLogicOnRoom(Room room)
	{
		for (int i = room.Doors.Count - 1; i >= 0; i--)
		{
			Door door = room.Doors[i];
			if (door.ConnectedDoor == null)
			{
				door.Close(true);
			}
		}
		SpawnLogicController[] spawnLogicControllers = room.SpawnControllerManager.SpawnLogicControllers;
		for (int j = 0; j < spawnLogicControllers.Length; j++)
		{
			spawnLogicControllers[j].RunIsSpawnedCheck(SpawnScenarioCheckStage.PreMerge);
		}
		ISimpleSpawnController[] simpleSpawnControllers_NoProps = room.SpawnControllerManager.SimpleSpawnControllers_NoProps;
		for (int j = 0; j < simpleSpawnControllers_NoProps.Length; j++)
		{
			simpleSpawnControllers_NoProps[j].Spawn();
		}
	}

	// Token: 0x04003E4A RID: 15946
	private static Vector2Int DESTINATION_GRID_COORDINATES = new Vector2Int(-50, 50);

	// Token: 0x04003E4B RID: 15947
	private const string SPECIAL_ROOM_LOBBY_PATH = "Scriptable Objects/Room Meta Data/Levels Linker/Levels Linker_28_RoomMetaData";

	// Token: 0x04003E4C RID: 15948
	public static EventHandler<RoomEventArgs> RoomCreatedEvent;

	// Token: 0x04003E4D RID: 15949
	private static RoomEventArgs m_roomCreatedEventArgs;

	// Token: 0x04003E4E RID: 15950
	private static List<RoomID> m_destinationRooms = new List<RoomID>();

	// Token: 0x04003E4F RID: 15951
	private static Dictionary<Tunnel, List<Room>> m_rootToRoomsTable = new Dictionary<Tunnel, List<Room>>();

	// Token: 0x04003E50 RID: 15952
	private static RoomMetaData m_lobbyMetaData = null;
}

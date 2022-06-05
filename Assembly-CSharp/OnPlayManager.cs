using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using FMODUnity;
using GameEventTracking;
using RLWorldCreation;
using Rooms;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public class OnPlayManager : MonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06003888 RID: 14472 RVA: 0x000C1140 File Offset: 0x000BF340
	// (remove) Token: 0x06003889 RID: 14473 RVA: 0x000C1174 File Offset: 0x000BF374
	public static event System.EventHandler WorldSetupEvent;

	// Token: 0x170013F2 RID: 5106
	// (get) Token: 0x0600388A RID: 14474 RVA: 0x000C11A8 File Offset: 0x000BF3A8
	public List<BaseRoom> ActiveRooms
	{
		get
		{
			List<BaseRoom> list = new List<BaseRoom>();
			if (this.m_currentRoom != null)
			{
				list.Add(this.m_currentRoom);
			}
			if (this.m_additionalRooms != null && this.m_additionalRooms.Count > 0)
			{
				list.AddRange(from room in this.m_additionalRooms
				select room);
			}
			return list;
		}
	}

	// Token: 0x170013F3 RID: 5107
	// (get) Token: 0x0600388B RID: 14475 RVA: 0x000C121C File Offset: 0x000BF41C
	// (set) Token: 0x0600388C RID: 14476 RVA: 0x000C1234 File Offset: 0x000BF434
	private static OnPlayManager Instance
	{
		get
		{
			if (!Application.isPlaying)
			{
				OnPlayManager.m_instance = UnityEngine.Object.FindObjectOfType<OnPlayManager>();
			}
			return OnPlayManager.m_instance;
		}
		set
		{
			OnPlayManager.m_instance = value;
		}
	}

	// Token: 0x170013F4 RID: 5108
	// (get) Token: 0x0600388D RID: 14477 RVA: 0x000C123C File Offset: 0x000BF43C
	// (set) Token: 0x0600388E RID: 14478 RVA: 0x000C1248 File Offset: 0x000BF448
	public static bool IsCameraConstrainerEnabled
	{
		get
		{
			return OnPlayManager.Instance.m_isCameraConstrainerEnabled;
		}
		private set
		{
			OnPlayManager.Instance.m_isCameraConstrainerEnabled = value;
		}
	}

	// Token: 0x170013F5 RID: 5109
	// (get) Token: 0x0600388F RID: 14479 RVA: 0x000C1255 File Offset: 0x000BF455
	public static bool IsInstantiated
	{
		get
		{
			return OnPlayManager.Instance != null;
		}
	}

	// Token: 0x170013F6 RID: 5110
	// (get) Token: 0x06003890 RID: 14480 RVA: 0x000C1262 File Offset: 0x000BF462
	public static Room CurrentRoom
	{
		get
		{
			if (OnPlayManager.Instance != null)
			{
				return OnPlayManager.Instance.m_currentRoom as Room;
			}
			return null;
		}
	}

	// Token: 0x170013F7 RID: 5111
	// (get) Token: 0x06003891 RID: 14481 RVA: 0x000C1282 File Offset: 0x000BF482
	public static List<BaseRoom> RoomList
	{
		get
		{
			return OnPlayManager.m_instance.ActiveRooms;
		}
	}

	// Token: 0x170013F8 RID: 5112
	// (get) Token: 0x06003892 RID: 14482 RVA: 0x000C128E File Offset: 0x000BF48E
	public static BiomeController BiomeController
	{
		get
		{
			return OnPlayManager.Instance.m_biomeController;
		}
	}

	// Token: 0x170013F9 RID: 5113
	// (get) Token: 0x06003893 RID: 14483 RVA: 0x000C129A File Offset: 0x000BF49A
	// (set) Token: 0x06003894 RID: 14484 RVA: 0x000C12A6 File Offset: 0x000BF4A6
	public static BiomeType StartingBiome
	{
		get
		{
			return OnPlayManager.Instance.m_startingBiome;
		}
		private set
		{
			OnPlayManager.Instance.m_startingBiome = value;
		}
	}

	// Token: 0x170013FA RID: 5114
	// (get) Token: 0x06003895 RID: 14485 RVA: 0x000C12B3 File Offset: 0x000BF4B3
	public static KeyCode CycleBiome
	{
		get
		{
			return OnPlayManager.Instance.m_cycleBiome;
		}
	}

	// Token: 0x170013FB RID: 5115
	// (get) Token: 0x06003896 RID: 14486 RVA: 0x000C12BF File Offset: 0x000BF4BF
	public static KeyCode FlipRoom
	{
		get
		{
			return OnPlayManager.Instance.m_flipRoom;
		}
	}

	// Token: 0x170013FC RID: 5116
	// (get) Token: 0x06003897 RID: 14487 RVA: 0x000C12CB File Offset: 0x000BF4CB
	public static KeyCode ToggleCloseAllDoorsButOneKey
	{
		get
		{
			return OnPlayManager.Instance.m_toggleCloseAllDoorsButOneKey;
		}
	}

	// Token: 0x170013FD RID: 5117
	// (get) Token: 0x06003898 RID: 14488 RVA: 0x000C12D7 File Offset: 0x000BF4D7
	public static KeyCode CycleClosedDoorKey
	{
		get
		{
			return OnPlayManager.Instance.m_cycleClosedDoorKey;
		}
	}

	// Token: 0x170013FE RID: 5118
	// (get) Token: 0x06003899 RID: 14489 RVA: 0x000C12E3 File Offset: 0x000BF4E3
	public static KeyCode CycleDifficulty
	{
		get
		{
			return OnPlayManager.Instance.m_cycleDifficulty;
		}
	}

	// Token: 0x170013FF RID: 5119
	// (get) Token: 0x0600389A RID: 14490 RVA: 0x000C12EF File Offset: 0x000BF4EF
	public static KeyCode RandomizeHazards
	{
		get
		{
			return OnPlayManager.Instance.m_randomizeHazards;
		}
	}

	// Token: 0x17001400 RID: 5120
	// (get) Token: 0x0600389B RID: 14491 RVA: 0x000C12FB File Offset: 0x000BF4FB
	// (set) Token: 0x0600389C RID: 14492 RVA: 0x000C1303 File Offset: 0x000BF503
	public Room BaseRoom
	{
		get
		{
			return this.m_baseRoom;
		}
		private set
		{
			this.m_baseRoom = value;
		}
	}

	// Token: 0x17001401 RID: 5121
	// (get) Token: 0x0600389D RID: 14493 RVA: 0x000C130C File Offset: 0x000BF50C
	// (set) Token: 0x0600389E RID: 14494 RVA: 0x000C1321 File Offset: 0x000BF521
	public static BiomeType CurrentBiome
	{
		get
		{
			if (Application.isPlaying)
			{
				return OnPlayManager.Instance.m_currentBiome;
			}
			return BiomeType.None;
		}
		private set
		{
			if (Application.isPlaying)
			{
				OnPlayManager.Instance.m_currentBiome = value;
			}
		}
	}

	// Token: 0x17001402 RID: 5122
	// (get) Token: 0x0600389F RID: 14495 RVA: 0x000C1335 File Offset: 0x000BF535
	// (set) Token: 0x060038A0 RID: 14496 RVA: 0x000C1338 File Offset: 0x000BF538
	public static BiomeType TransitionPointBiome
	{
		get
		{
			return BiomeType.None;
		}
		private set
		{
		}
	}

	// Token: 0x17001403 RID: 5123
	// (get) Token: 0x060038A1 RID: 14497 RVA: 0x000C133A File Offset: 0x000BF53A
	// (set) Token: 0x060038A2 RID: 14498 RVA: 0x000C1342 File Offset: 0x000BF542
	public KeyCode CycleTransitionPointBiome
	{
		get
		{
			return this.m_cycleTransitionPointBiome;
		}
		private set
		{
			this.m_cycleTransitionPointBiome = value;
		}
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x000C134C File Offset: 0x000BF54C
	private void Awake()
	{
		if (OnPlayManager.Instance == null)
		{
			OnPlayManager.Instance = this;
			RuntimeManager.LoadBank("Master.strings", false);
			RuntimeManager.LoadBank("Master", false);
			RuntimeManager.LoadBank("Music", false);
			RuntimeManager.WaitForAllLoads();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x000C139E File Offset: 0x000BF59E
	private IEnumerator Start()
	{
		if (this.m_roomOverride == null)
		{
			Debug.LogFormat("<color=red>| {0} |  Room Override is null</color>", new object[]
			{
				this
			});
			yield break;
		}
		this.DeactivateOtherRooms();
		this.StoreReferenceToBaseRoom();
		OnPlayManager.CurrentBiome = OnPlayManager.StartingBiome;
		CreateTunnelRooms_BuildStage.RoomCreatedEvent = (EventHandler<RoomEventArgs>)Delegate.Combine(CreateTunnelRooms_BuildStage.RoomCreatedEvent, new EventHandler<RoomEventArgs>(this.OnTunnelDestinationCreated));
		yield return new WaitUntil(() => CameraController.IsInstantiated);
		CameraController.SetCameraSettingsForBiome(OnPlayManager.CurrentBiome, this.BaseRoom);
		this.m_currentBiomeIndexInArtDataLibrary = BiomeArtDataLibrary.ArtDataTable.FindIndex((BiomeArtDataEntry entry) => entry.BiomeType == OnPlayManager.CurrentBiome);
		this.m_buildRoomCoroutine = base.StartCoroutine(this.RecreateRoom());
		yield return new WaitUntil(() => this.m_buildRoomCoroutine == null);
		yield break;
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x000C13AD File Offset: 0x000BF5AD
	private void DeactivateOtherRooms()
	{
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x000C13B0 File Offset: 0x000BF5B0
	private void Update()
	{
		if (this.m_currentRoom == null)
		{
			return;
		}
		if ((Input.GetKeyDown(OnPlayManager.CycleClosedDoorKey) || Input.GetKeyDown(OnPlayManager.ToggleCloseAllDoorsButOneKey) || Input.GetKeyDown(OnPlayManager.CycleBiome) || Input.GetKeyDown(OnPlayManager.FlipRoom) || Input.GetKeyDown(OnPlayManager.CycleDifficulty) || Input.GetKeyDown(OnPlayManager.RandomizeHazards) || Input.GetKeyDown(this.CycleTransitionPointBiome)) && this.m_buildRoomCoroutine == null)
		{
			if (Input.GetKeyDown(OnPlayManager.CycleClosedDoorKey))
			{
				this.CycleThroughDoors(false);
			}
			else if (Input.GetKeyDown(OnPlayManager.ToggleCloseAllDoorsButOneKey))
			{
				this.CycleThroughDoors(true);
			}
			else if (Input.GetKeyDown(OnPlayManager.CycleBiome))
			{
				this.CycleThroughBiomes();
			}
			else if (Input.GetKeyDown(OnPlayManager.FlipRoom))
			{
				this.m_flipCurrentRoomHorizontally = !this.m_flipCurrentRoomHorizontally;
			}
			else if (Input.GetKeyDown(OnPlayManager.CycleDifficulty))
			{
				this.CycleThroughDifficultyLevels();
			}
			else if (Input.GetKeyDown(OnPlayManager.RandomizeHazards))
			{
				this.RandomizeHazardsInRoom();
			}
			else if (Input.GetKeyDown(this.CycleTransitionPointBiome))
			{
				this.CycleThroughTransitionPointBiome();
			}
			this.SetupWorld();
		}
	}

	// Token: 0x060038A7 RID: 14503 RVA: 0x000C14D0 File Offset: 0x000BF6D0
	private void CycleThroughTransitionPointBiome()
	{
		BiomeType[] array = Enum.GetValues(typeof(BiomeType)) as BiomeType[];
		List<BiomeType> list = new List<BiomeType>
		{
			BiomeType.None
		};
		for (int i = 0; i < array.Length; i++)
		{
			if (BiomeType_RL.IsValidBiome(array[i]))
			{
				list.Add((BiomeType)array.GetValue(i));
			}
		}
		int num = 0;
		for (int j = 0; j < list.Count; j++)
		{
			if (OnPlayManager.TransitionPointBiome == list[j])
			{
				num = j;
				break;
			}
		}
		if (num == list.Count - 1)
		{
			num = 0;
		}
		else
		{
			num++;
		}
		OnPlayManager.SetTransitionPointBiome(list[num], false);
	}

	// Token: 0x060038A8 RID: 14504 RVA: 0x000C1574 File Offset: 0x000BF774
	private IEnumerator CloseDoors(bool closeAllButOne)
	{
		if (this.m_roomOverride == null)
		{
			Debug.LogFormat("<color=red>| {0} | Room Override is null</color>", new object[]
			{
				this
			});
			yield break;
		}
		if (this.BaseRoom == null)
		{
			Debug.LogFormat("<color=red>| {0} | Base Room is null</color>", new object[]
			{
				this
			});
			yield break;
		}
		if (this.m_currentRoom == null)
		{
			Debug.LogFormat("<color=red>| {0} | Current Room is null</color>", new object[]
			{
				this
			});
			yield break;
		}
		List<Door> doors = this.m_currentRoom.Doors;
		for (int i = 0; i < doors.Count; i++)
		{
			if (doors[i].DisabledFromLevelEditor)
			{
				doors[i].Close(true);
				i--;
			}
		}
		if (this.m_currentDoorIndex >= 0 && this.m_currentDoorIndex < doors.Count)
		{
			if (closeAllButOne)
			{
				for (int j = doors.Count - 1; j >= 0; j--)
				{
					if (j != this.m_currentDoorIndex)
					{
						doors[j].Close(true);
					}
				}
			}
			else
			{
				doors[this.m_currentDoorIndex].Close(true);
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x060038A9 RID: 14505 RVA: 0x000C158A File Offset: 0x000BF78A
	private IEnumerator CompleteWorldCreation()
	{
		yield return null;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.LevelEditorWorldCreationComplete, this, new LevelEditorWorldCreationCompleteEventArgs(this.m_currentRoom));
		yield return null;
		yield return null;
		yield break;
	}

	// Token: 0x060038AA RID: 14506 RVA: 0x000C159C File Offset: 0x000BF79C
	private void CreateRoomBackgrounds()
	{
		if (OnPlayManager.CurrentBiome != BiomeType.Forest)
		{
			bool flag = OnPlayManager.CurrentBiome != BiomeType.Tutorial;
		}
		BiomeCreatorTools.CreateBackground(this.m_currentRoom);
		if (this.m_additionalRooms != null)
		{
			foreach (Room targetRoom in this.m_additionalRooms)
			{
				BiomeCreatorTools.CreateBackground(targetRoom);
			}
		}
	}

	// Token: 0x060038AB RID: 14507 RVA: 0x000C161C File Offset: 0x000BF81C
	private void BuildTerrain()
	{
		if (this.m_additionalRooms != null)
		{
			foreach (Room room in this.m_additionalRooms)
			{
				RoomUtility.BuildAllFerr2DTerrains(room);
			}
		}
		RoomUtility.BuildAllFerr2DTerrains(this.m_currentRoom);
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x000C1680 File Offset: 0x000BF880
	private IEnumerator CreateInstanceOfBaseRoom()
	{
		if (this.m_roomOverride == null)
		{
			Debug.LogFormat("<color=red>| {0} | Room Override is null</color>", new object[]
			{
				this
			});
			yield break;
		}
		BaseRoom currentRoom = this.m_currentRoom;
		if (currentRoom != null)
		{
			currentRoom.PlayerExit(null);
			UnityEngine.Object.Destroy(currentRoom.gameObject);
		}
		Vector3 position = GridController.GetWorldPositionFromRoomCoordinates(Vector2.zero, this.BaseRoom.Size);
		if (this.m_flipCurrentRoomHorizontally)
		{
			this.m_currentRoom = RoomUtility.CreateMirrorVersionOfRoom(this.BaseRoom);
			this.m_currentRoom.gameObject.transform.position = position;
		}
		else
		{
			this.m_currentRoom = UnityEngine.Object.Instantiate<Room>(this.BaseRoom, position, Quaternion.identity);
		}
		this.m_currentRoom.BiomeType = this.m_currentBiome;
		this.m_currentRoom.gameObject.transform.SetParent(this.m_biomeController.gameObject.transform);
		this.m_currentRoom.gameObject.SetActive(true);
		RoomMetaData roomMetaData = new RoomMetaData(this.m_currentRoom as Room);
		RoomType roomType = this.GetRoomType(this.m_currentRoom as Room);
		GridPointManager gridPointManager = new GridPointManager(0, Vector2Int.zero, this.m_currentBiome, roomType, roomMetaData, this.m_flipCurrentRoomHorizontally, false);
		int randomNumber = RNGManager.GetRandomNumber(RngID.Room_RNGSeed_Generator, "Get Random Room Seed", 0, int.MaxValue);
		gridPointManager.SetRoomSeed(randomNumber);
		gridPointManager.SetBiomeControllerIndex(0);
		this.m_currentRoom.SetBiomeControllerIndex(0);
		(this.m_currentRoom as Room).Initialise(gridPointManager);
		this.m_biomeController.StandardRoomCreatedByWorldBuilder(this.m_currentRoom as Room);
		yield return null;
		if (this.m_attachRoomsToDoors)
		{
			yield return this.CreateRoomsConnectedToCurrentRoom();
		}
		if ((this.m_currentRoom as Room).LevelOverride == -1)
		{
			(this.m_currentRoom as Room).SetLevel(this.m_roomLevel);
		}
		yield break;
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x000C1690 File Offset: 0x000BF890
	private RoomType GetRoomType(Room room)
	{
		RoomType result = RoomType.Standard;
		if (room.GetComponent<BossRoomController>() != null)
		{
			result = RoomType.Boss;
		}
		else if (room.GetComponent<FairyRoomController>())
		{
			result = RoomType.Fairy;
		}
		return result;
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x000C16C3 File Offset: 0x000BF8C3
	private IEnumerator CreateRoomsConnectedToCurrentRoom()
	{
		if (this.m_roomCreator == null)
		{
			this.m_roomCreator = base.GetComponent<RoomCreator>();
		}
		if (this.m_additionalRooms == null)
		{
			this.m_additionalRooms = new List<Room>();
		}
		else
		{
			for (int i = this.m_additionalRooms.Count - 1; i >= 0; i--)
			{
				Room room = this.m_additionalRooms[i];
				if (room != null && room.gameObject != null)
				{
					UnityEngine.Object.Destroy(room.gameObject);
				}
			}
			this.m_additionalRooms.Clear();
		}
		yield return null;
		int biomeControllerIndex = 1;
		foreach (Door door in this.m_currentRoom.Doors)
		{
			int doorIndex = 0;
			GameObject original = this.m_roomCreator.SimpleRoomPrefab;
			if (OnPlayManager.CurrentBiome == BiomeType.Stone && (door.Side == RoomSide.Left || door.Side == RoomSide.Right))
			{
				original = this.m_roomCreator.Bridge1x3RoomPrefab;
				doorIndex = door.Number;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, new Vector3(-1000f, 0f, 0f), Quaternion.identity);
			gameObject.name = "DummyRoom:" + door.Side.ToString() + "-" + door.Number.ToString();
			Room component = gameObject.GetComponent<Room>();
			component.BiomeType = this.m_currentBiome;
			gameObject.transform.SetParent(this.m_biomeController.gameObject.transform);
			ISpawnController[] componentsInChildren = gameObject.GetComponentsInChildren<ISpawnController>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].SpawnLogicController.RunIsSpawnedCheck(SpawnScenarioCheckStage.PreMerge);
			}
			this.m_additionalRooms.Add(component);
			BiomeType biomeType = OnPlayManager.CurrentBiome;
			if (biomeType == BiomeType.TowerExterior)
			{
				biomeType = BiomeType.Tower;
			}
			BiomeCreatorTools.PositionRoom(this.m_currentRoom as Room, door.Side, door.Number, component, doorIndex);
			RoomMetaData roomMetaData = new RoomMetaData(component);
			RoomContentMetaDataFactory.CreateContentMetaData(roomMetaData);
			int roomNumber = biomeControllerIndex;
			DoorLocation location = new DoorLocation(door.Side, door.Number);
			DoorLocation location2 = new DoorLocation(RoomUtility.GetOppositeSide(door.Side), 0);
			Vector2Int roomGridCoordinates = GridController.GetRoomGridCoordinates(GridController.GetDoorLeadsToGridCoordinates((this.m_currentRoom as Room).GridPointManager.GridCoordinates, (this.m_currentRoom as Room).GridPointManager.Size, location), roomMetaData.Size, location2);
			GridPointManager gridPointManager = new GridPointManager(roomNumber, roomGridCoordinates, biomeType, RoomType.Standard, roomMetaData, false, false);
			gridPointManager.SetBiomeControllerIndex(biomeControllerIndex);
			component.Initialise(gridPointManager);
			component.SetBiomeControllerIndex(biomeControllerIndex);
			this.CreateSky(component);
			this.CreateWeather(component);
			int num = biomeControllerIndex;
			biomeControllerIndex = num + 1;
			this.m_biomeController.StandardRoomCreatedByWorldBuilder(component);
			yield return null;
		}
		List<Door>.Enumerator enumerator = default(List<Door>.Enumerator);
		this.m_biomeController.GridPointManager.SetStandaloneRoomCount(this.m_biomeController.StandaloneRoomCount);
		yield return new WaitForSeconds(0.1f);
		using (List<Room>.Enumerator enumerator2 = this.m_additionalRooms.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				Room room2 = enumerator2.Current;
				BiomeCreatorTools.CloseUnconnectedDoorsInRoom(room2);
				BiomeCreatorTools.CreateCinemachineVirtualCamera(room2);
				room2.gameObject.SetActive(false);
			}
			yield break;
		}
		yield break;
		yield break;
	}

	// Token: 0x060038AF RID: 14511 RVA: 0x000C16D4 File Offset: 0x000BF8D4
	private void CreateSky(BaseRoom room)
	{
		if (BiomeDataLibrary.GetData(OnPlayManager.CurrentBiome))
		{
			BiomeType biome = OnPlayManager.CurrentBiome;
			Room room2 = room as Room;
			if (!room2.IsNativeNull() && room2.AppearanceOverride != BiomeType.None)
			{
				biome = room2.AppearanceOverride;
			}
			BiomeArtData biomeArtData = room.BiomeArtDataOverride;
			if (!biomeArtData)
			{
				biomeArtData = BiomeArtDataLibrary.GetArtData(biome);
			}
			Sky sky;
			if (!this.m_skyTable.TryGetValue(biomeArtData.SkyData, out sky))
			{
				sky = BiomeCreatorTools.CreateSkyInstance(biomeArtData);
				this.m_skyTable.Add(biomeArtData.SkyData, sky);
			}
			sky.RoomList.Add(room);
		}
	}

	// Token: 0x060038B0 RID: 14512 RVA: 0x000C1768 File Offset: 0x000BF968
	private void CreateWeather(BaseRoom room)
	{
		BiomeType biome = OnPlayManager.CurrentBiome;
		Room room2 = room as Room;
		if (!room2.IsNativeNull() && room2.AppearanceOverride != BiomeType.None)
		{
			biome = room2.AppearanceOverride;
		}
		BiomeArtData biomeArtData = room.BiomeArtDataOverride;
		if (!biomeArtData)
		{
			biomeArtData = BiomeArtDataLibrary.GetArtData(biome);
		}
		if (biomeArtData)
		{
			Weather[] array;
			if (!this.m_weatherTable.TryGetValue(biomeArtData.WeatherData, out array))
			{
				array = BiomeCreatorTools.CreateWeatherInstances(biomeArtData);
				this.m_weatherTable.Add(biomeArtData.WeatherData, array);
			}
			if (array != null)
			{
				Weather[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].RoomList.Add(room);
				}
			}
		}
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x000C1810 File Offset: 0x000BFA10
	private void CycleThroughBiomes()
	{
		this.m_currentBiomeIndexInArtDataLibrary++;
		if (this.m_currentBiomeIndexInArtDataLibrary == BiomeArtDataLibrary.ArtDataTable.Count)
		{
			this.m_currentBiomeIndexInArtDataLibrary = 0;
		}
		OnPlayManager.SetCurrentBiome(BiomeArtDataLibrary.ArtDataTable[this.m_currentBiomeIndexInArtDataLibrary].BiomeType);
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x000C185E File Offset: 0x000BFA5E
	private void CycleThroughDifficultyLevels()
	{
		GameUtility.Difficulty++;
		Debug.LogFormat("<color=purple>Difficulty Level = {0}</purple>", new object[]
		{
			GameUtility.Difficulty
		});
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x000C188C File Offset: 0x000BFA8C
	private void CycleThroughDoors(bool closeAllButOneDoor)
	{
		this.m_closeAllDoorsButOneToggle = closeAllButOneDoor;
		if (this.m_doorCount == -1)
		{
			this.m_doorCount = this.BaseRoom.Doors.Count;
		}
		this.m_currentDoorIndex++;
		if (this.m_currentDoorIndex > this.m_doorCount - 1)
		{
			this.m_currentDoorIndex = -1;
		}
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x000C18E4 File Offset: 0x000BFAE4
	private void OnTunnelDestinationCreated(object sender, RoomEventArgs eventArgs)
	{
		Room room = eventArgs.Room as Room;
		this.m_additionalRooms.Add(room);
		this.CreateSky(room);
		this.CreateWeather(room);
		BiomeCreatorTools.CreateCinemachineVirtualCamera(eventArgs.Room);
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x000C1923 File Offset: 0x000BFB23
	private IEnumerator PlaceOneWaysAlongBottomDoors()
	{
		if (OnPlayManager.CurrentBiome != BiomeType.TowerExterior)
		{
			List<Door> doorsOnSide = (this.m_currentRoom as Room).GetDoorsOnSide(RoomSide.Bottom);
			if (doorsOnSide != null && doorsOnSide.Count > 0)
			{
				using (List<Door>.Enumerator enumerator = doorsOnSide.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						OnPlayManager.<>c__DisplayClass96_0 CS$<>8__locals1 = new OnPlayManager.<>c__DisplayClass96_0();
						CS$<>8__locals1.door = enumerator.Current;
						yield return new WaitUntil(() => CS$<>8__locals1.door.Room != null);
						BiomeCreatorTools.PlaceOneWayAtDoor(CS$<>8__locals1.door);
						CS$<>8__locals1 = null;
					}
				}
				List<Door>.Enumerator enumerator = default(List<Door>.Enumerator);
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x000C1932 File Offset: 0x000BFB32
	private void RandomizeHazardsInRoom()
	{
		HazardRandomizer.RandomizeHazards(from entry in this.BaseRoom.SpawnControllerManager.SpawnControllers
		select entry as IHazardSpawnController);
	}

	// Token: 0x060038B7 RID: 14519 RVA: 0x000C196D File Offset: 0x000BFB6D
	private IEnumerator RecreateRoom()
	{
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdatePools, null, null);
		if (this.m_biomeController != null)
		{
			UnityEngine.Object.Destroy(this.m_biomeController.gameObject);
		}
		this.m_biomeController = WorldBuilder.CreateBiomeController(OnPlayManager.CurrentBiome, null);
		yield return this.CreateInstanceOfBaseRoom();
		yield return this.PlaceOneWaysAlongBottomDoors();
		if (this.m_previousBiome != OnPlayManager.CurrentBiome)
		{
			this.m_previousBiome = OnPlayManager.CurrentBiome;
			this.CreateSky(this.m_currentRoom);
			this.CreateWeather(this.m_currentRoom);
		}
		this.m_currentRoom.CinemachineCamera.GetComponent<CinemachineConfiner_RL>().enabled = OnPlayManager.IsCameraConstrainerEnabled;
		yield return this.CloseDoors(this.m_closeAllDoorsButOneToggle);
		BiomeType transitionPointBiome = OnPlayManager.TransitionPointBiome;
		if (transitionPointBiome != BiomeType.None)
		{
			foreach (Door door in this.m_currentRoom.Doors)
			{
				yield return CreateTransitionPoints_BuildStage.RunFromLevelEditor(transitionPointBiome, door);
			}
			List<Door>.Enumerator enumerator = default(List<Door>.Enumerator);
		}
		this.UpdateSpawnLogicControllers(SpawnScenarioCheckStage.PreMerge);
		this.SpawnSimpleSpawnControllers();
		yield return CreateTunnelRooms_BuildStage.RunFromLevelEditor(this.m_biomeController, this.m_currentRoom as Room);
		yield return this.CompleteWorldCreation();
		this.BuildTerrain();
		this.CreateRoomBackgrounds();
		BiomeTransitionController.UpdateObjectPools(BiomeType.None, this.m_biomeController.Biome);
		MapController.Reset();
		MapController.Initialize();
		if (ItemDropManager.IsInitialized)
		{
			ItemDropManager.DisableAllItemDrops();
		}
		if (EnemyManager.IsInitialized)
		{
			EnemyManager.DisableAllEnemies();
			EnemyManager.DisableAllSummonedEnemies();
		}
		this.SpawnPlayer();
		if (OnPlayManager.WorldSetupEvent != null)
		{
			OnPlayManager.WorldSetupEvent(this, EventArgs.Empty);
		}
		this.m_buildRoomCoroutine = null;
		yield break;
		yield break;
	}

	// Token: 0x060038B8 RID: 14520 RVA: 0x000C197C File Offset: 0x000BFB7C
	public static void Restart()
	{
		GameEventTrackerManager.Reset();
		MapController.Reset();
		OnPlayManager.Instance.SetupWorld();
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x000C1994 File Offset: 0x000BFB94
	public static void SetBiome(BiomeType biome)
	{
		if (biome == BiomeType.Any || biome == BiomeType.None || biome == BiomeType.Editor || biome == BiomeType.Spawn || biome == BiomeType.Special)
		{
			return;
		}
		if (OnPlayManager.Instance.m_buildRoomCoroutine != null)
		{
			return;
		}
		for (int i = 0; i < BiomeArtDataLibrary.ArtDataTable.Count; i++)
		{
			if (biome == BiomeArtDataLibrary.ArtDataTable[i].BiomeType)
			{
				OnPlayManager.Instance.m_currentBiomeIndexInArtDataLibrary = i;
				break;
			}
		}
		OnPlayManager.SetCurrentBiome(biome);
		OnPlayManager.Instance.m_buildRoomCoroutine = OnPlayManager.Instance.StartCoroutine(OnPlayManager.Instance.RecreateRoom());
	}

	// Token: 0x060038BA RID: 14522 RVA: 0x000C1A22 File Offset: 0x000BFC22
	public static void SetTransitionPointBiome(BiomeType transitionPointBiome, bool rebuild = false)
	{
		if (transitionPointBiome != BiomeType.None && !BiomeType_RL.IsValidBiome(transitionPointBiome))
		{
			return;
		}
		OnPlayManager.TransitionPointBiome = transitionPointBiome;
		if (rebuild)
		{
			OnPlayManager.Instance.m_buildRoomCoroutine = OnPlayManager.Instance.StartCoroutine(OnPlayManager.Instance.RecreateRoom());
		}
	}

	// Token: 0x060038BB RID: 14523 RVA: 0x000C1A57 File Offset: 0x000BFC57
	public static void SetDifficulty(int difficulty)
	{
		GameUtility.Difficulty = difficulty;
		OnPlayManager.Instance.m_buildRoomCoroutine = OnPlayManager.Instance.StartCoroutine(OnPlayManager.Instance.RecreateRoom());
	}

	// Token: 0x060038BC RID: 14524 RVA: 0x000C1A7D File Offset: 0x000BFC7D
	public static void SetCameraConstrainerIsEnabled(bool isEnabled)
	{
		OnPlayManager.IsCameraConstrainerEnabled = isEnabled;
		bool isPlaying = Application.isPlaying;
		if (Application.isPlaying)
		{
			OnPlayManager.Instance.m_currentRoom.CinemachineCamera.GetComponent<CinemachineConfiner_RL>().enabled = OnPlayManager.IsCameraConstrainerEnabled;
		}
	}

	// Token: 0x060038BD RID: 14525 RVA: 0x000C1AB0 File Offset: 0x000BFCB0
	private static void SetCurrentBiome(BiomeType biome)
	{
		OnPlayManager.Instance.m_previousBiome = OnPlayManager.CurrentBiome;
		OnPlayManager.CurrentBiome = biome;
		CameraController.SetCameraSettingsForBiome(OnPlayManager.CurrentBiome, null);
	}

	// Token: 0x060038BE RID: 14526 RVA: 0x000C1AD2 File Offset: 0x000BFCD2
	public static void SetStartingBiome(BiomeType biome)
	{
		if (!BiomeType_RL.IsValidBiome(biome))
		{
			return;
		}
		OnPlayManager.StartingBiome = biome;
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x000C1AE3 File Offset: 0x000BFCE3
	public void SetupWorld()
	{
		PlayerManager.GetPlayerController().ResetCharacter();
		this.m_buildRoomCoroutine = base.StartCoroutine(this.RecreateRoom());
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x000C1B04 File Offset: 0x000BFD04
	private void SpawnSimpleSpawnControllers()
	{
		foreach (BaseRoom baseRoom in this.ActiveRooms)
		{
			foreach (ISpawnController spawnController in baseRoom.SpawnControllerManager.SpawnControllers)
			{
				if (spawnController is ISimpleSpawnController)
				{
					(spawnController as ISimpleSpawnController).Spawn();
				}
			}
		}
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x000C1B80 File Offset: 0x000BFD80
	private void SpawnPlayer()
	{
		if (this.m_currentRoom != null)
		{
			if (this.m_player == null)
			{
				this.m_player = PlayerManager.GetPlayer();
			}
			else
			{
				this.m_player.gameObject.SetActive(true);
			}
			PlayerManager.GetPlayerController().CurrentlyInRoom = null;
			PlayerManager.GetPlayerController().ResetStates();
			if (PlayerManager.GetPlayerController().IsInvincible)
			{
				PlayerManager.GetPlayerController().CharacterHitResponse.SetInvincibleTime(PlayerManager.GetPlayerController().InvincibilityTimer, false, true);
			}
			this.m_currentRoom.PlacePlayerInRoom(null);
			return;
		}
		Debug.LogFormat("<color=red>{0}: Can't spawn Player because m_currentRoom is null</color>", new object[]
		{
			Time.frameCount
		});
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x000C1C2D File Offset: 0x000BFE2D
	public void ClearActiveRooms()
	{
		this.m_additionalRooms.Clear();
		this.m_currentRoom = null;
	}

	// Token: 0x060038C3 RID: 14531 RVA: 0x000C1C41 File Offset: 0x000BFE41
	private void StoreReferenceToBaseRoom()
	{
		this.m_baseRoom = this.m_roomOverride;
		BiomeCreatorTools.CreateCinemachineVirtualCamera(this.BaseRoom);
		this.BaseRoom.gameObject.SetActive(false);
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x000C1C6C File Offset: 0x000BFE6C
	private void UpdateSpawnLogicControllers(SpawnScenarioCheckStage checkStage = SpawnScenarioCheckStage.PreMerge)
	{
		foreach (BaseRoom baseRoom in this.ActiveRooms)
		{
			foreach (ISpawnController spawnController in baseRoom.SpawnControllerManager.SpawnControllers)
			{
				if (spawnController != null && spawnController.SpawnLogicController != null)
				{
					spawnController.SpawnLogicController.RunIsSpawnedCheck(checkStage);
				}
			}
		}
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x000C1CF4 File Offset: 0x000BFEF4
	private void AddCurrentRoomToGrid(Room room)
	{
		new RoomMetaData(room);
	}

	// Token: 0x04002BBA RID: 11194
	private List<Room> m_additionalRooms;

	// Token: 0x04002BBB RID: 11195
	private Room m_baseRoom;

	// Token: 0x04002BBC RID: 11196
	private Coroutine m_buildRoomCoroutine;

	// Token: 0x04002BBD RID: 11197
	private bool m_closeAllDoorsButOneToggle;

	// Token: 0x04002BBE RID: 11198
	private int m_currentBiomeIndexInArtDataLibrary;

	// Token: 0x04002BBF RID: 11199
	private int m_currentDoorIndex = -1;

	// Token: 0x04002BC0 RID: 11200
	private BaseRoom m_currentRoom;

	// Token: 0x04002BC1 RID: 11201
	private int m_cutoutCount = -1;

	// Token: 0x04002BC2 RID: 11202
	private int m_doorCount = -1;

	// Token: 0x04002BC3 RID: 11203
	private static OnPlayManager m_instance = null;

	// Token: 0x04002BC4 RID: 11204
	private GameObject m_player;

	// Token: 0x04002BC5 RID: 11205
	private RoomCreator m_roomCreator;

	// Token: 0x04002BC6 RID: 11206
	private Dictionary<SkyBiomeArtData, Sky> m_skyTable = new Dictionary<SkyBiomeArtData, Sky>();

	// Token: 0x04002BC7 RID: 11207
	private Dictionary<WeatherBiomeArtData, Weather[]> m_weatherTable = new Dictionary<WeatherBiomeArtData, Weather[]>();

	// Token: 0x04002BC8 RID: 11208
	private static Vector2 ROOM_FOR_TUNNEL_TESTING_POSITION = new Vector2(1000f, 1000f);

	// Token: 0x04002BC9 RID: 11209
	private const string CREATE_TRANSITION_POINTS_EDITOR_PREFS_KEY = "CreateTransitionPoints";

	// Token: 0x04002BCA RID: 11210
	[SerializeField]
	private Room m_roomOverride;

	// Token: 0x04002BCB RID: 11211
	[SerializeField]
	private KeyCode m_cycleClosedDoorKey = KeyCode.KeypadPlus;

	// Token: 0x04002BCC RID: 11212
	[SerializeField]
	private KeyCode m_toggleCloseAllDoorsButOneKey = KeyCode.KeypadMinus;

	// Token: 0x04002BCD RID: 11213
	[SerializeField]
	private KeyCode m_cycleBiome = KeyCode.End;

	// Token: 0x04002BCE RID: 11214
	[SerializeField]
	private KeyCode m_cycleTransitionPointBiome = KeyCode.PageDown;

	// Token: 0x04002BCF RID: 11215
	[SerializeField]
	private KeyCode m_flipRoom = KeyCode.Insert;

	// Token: 0x04002BD0 RID: 11216
	[SerializeField]
	private KeyCode m_cycleDifficulty = KeyCode.Period;

	// Token: 0x04002BD1 RID: 11217
	[SerializeField]
	private KeyCode m_randomizeHazards = KeyCode.Slash;

	// Token: 0x04002BD2 RID: 11218
	[SerializeField]
	[ReadOnly]
	private BiomeType m_currentBiome;

	// Token: 0x04002BD3 RID: 11219
	[SerializeField]
	private bool m_attachRoomsToDoors = true;

	// Token: 0x04002BD4 RID: 11220
	[SerializeField]
	private bool m_flipCurrentRoomHorizontally;

	// Token: 0x04002BD5 RID: 11221
	[SerializeField]
	private BiomeType m_startingBiome = BiomeType.Castle;

	// Token: 0x04002BD6 RID: 11222
	[SerializeField]
	private bool m_isCameraConstrainerEnabled = true;

	// Token: 0x04002BD7 RID: 11223
	[Header("Overrides")]
	[SerializeField]
	private int m_roomLevel = 1;

	// Token: 0x04002BD9 RID: 11225
	private BiomeType m_previousBiome;

	// Token: 0x04002BDA RID: 11226
	private BiomeController m_biomeController;
}

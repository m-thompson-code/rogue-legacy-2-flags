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

// Token: 0x02000A44 RID: 2628
public class OnPlayManager : MonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06004F2C RID: 20268 RVA: 0x0012F300 File Offset: 0x0012D500
	// (remove) Token: 0x06004F2D RID: 20269 RVA: 0x0012F334 File Offset: 0x0012D534
	public static event System.EventHandler WorldSetupEvent;

	// Token: 0x17001B49 RID: 6985
	// (get) Token: 0x06004F2E RID: 20270 RVA: 0x0012F368 File Offset: 0x0012D568
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

	// Token: 0x17001B4A RID: 6986
	// (get) Token: 0x06004F2F RID: 20271 RVA: 0x0002B2B1 File Offset: 0x000294B1
	// (set) Token: 0x06004F30 RID: 20272 RVA: 0x0002B2C9 File Offset: 0x000294C9
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

	// Token: 0x17001B4B RID: 6987
	// (get) Token: 0x06004F31 RID: 20273 RVA: 0x0002B2D1 File Offset: 0x000294D1
	// (set) Token: 0x06004F32 RID: 20274 RVA: 0x0002B2DD File Offset: 0x000294DD
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

	// Token: 0x17001B4C RID: 6988
	// (get) Token: 0x06004F33 RID: 20275 RVA: 0x0002B2EA File Offset: 0x000294EA
	public static bool IsInstantiated
	{
		get
		{
			return OnPlayManager.Instance != null;
		}
	}

	// Token: 0x17001B4D RID: 6989
	// (get) Token: 0x06004F34 RID: 20276 RVA: 0x0002B2F7 File Offset: 0x000294F7
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

	// Token: 0x17001B4E RID: 6990
	// (get) Token: 0x06004F35 RID: 20277 RVA: 0x0002B317 File Offset: 0x00029517
	public static List<BaseRoom> RoomList
	{
		get
		{
			return OnPlayManager.m_instance.ActiveRooms;
		}
	}

	// Token: 0x17001B4F RID: 6991
	// (get) Token: 0x06004F36 RID: 20278 RVA: 0x0002B323 File Offset: 0x00029523
	public static BiomeController BiomeController
	{
		get
		{
			return OnPlayManager.Instance.m_biomeController;
		}
	}

	// Token: 0x17001B50 RID: 6992
	// (get) Token: 0x06004F37 RID: 20279 RVA: 0x0002B32F File Offset: 0x0002952F
	// (set) Token: 0x06004F38 RID: 20280 RVA: 0x0002B33B File Offset: 0x0002953B
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

	// Token: 0x17001B51 RID: 6993
	// (get) Token: 0x06004F39 RID: 20281 RVA: 0x0002B348 File Offset: 0x00029548
	public static KeyCode CycleBiome
	{
		get
		{
			return OnPlayManager.Instance.m_cycleBiome;
		}
	}

	// Token: 0x17001B52 RID: 6994
	// (get) Token: 0x06004F3A RID: 20282 RVA: 0x0002B354 File Offset: 0x00029554
	public static KeyCode FlipRoom
	{
		get
		{
			return OnPlayManager.Instance.m_flipRoom;
		}
	}

	// Token: 0x17001B53 RID: 6995
	// (get) Token: 0x06004F3B RID: 20283 RVA: 0x0002B360 File Offset: 0x00029560
	public static KeyCode ToggleCloseAllDoorsButOneKey
	{
		get
		{
			return OnPlayManager.Instance.m_toggleCloseAllDoorsButOneKey;
		}
	}

	// Token: 0x17001B54 RID: 6996
	// (get) Token: 0x06004F3C RID: 20284 RVA: 0x0002B36C File Offset: 0x0002956C
	public static KeyCode CycleClosedDoorKey
	{
		get
		{
			return OnPlayManager.Instance.m_cycleClosedDoorKey;
		}
	}

	// Token: 0x17001B55 RID: 6997
	// (get) Token: 0x06004F3D RID: 20285 RVA: 0x0002B378 File Offset: 0x00029578
	public static KeyCode CycleDifficulty
	{
		get
		{
			return OnPlayManager.Instance.m_cycleDifficulty;
		}
	}

	// Token: 0x17001B56 RID: 6998
	// (get) Token: 0x06004F3E RID: 20286 RVA: 0x0002B384 File Offset: 0x00029584
	public static KeyCode RandomizeHazards
	{
		get
		{
			return OnPlayManager.Instance.m_randomizeHazards;
		}
	}

	// Token: 0x17001B57 RID: 6999
	// (get) Token: 0x06004F3F RID: 20287 RVA: 0x0002B390 File Offset: 0x00029590
	// (set) Token: 0x06004F40 RID: 20288 RVA: 0x0002B398 File Offset: 0x00029598
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

	// Token: 0x17001B58 RID: 7000
	// (get) Token: 0x06004F41 RID: 20289 RVA: 0x0002B3A1 File Offset: 0x000295A1
	// (set) Token: 0x06004F42 RID: 20290 RVA: 0x0002B3B6 File Offset: 0x000295B6
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

	// Token: 0x17001B59 RID: 7001
	// (get) Token: 0x06004F43 RID: 20291 RVA: 0x00003CD2 File Offset: 0x00001ED2
	// (set) Token: 0x06004F44 RID: 20292 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x17001B5A RID: 7002
	// (get) Token: 0x06004F45 RID: 20293 RVA: 0x0002B3CA File Offset: 0x000295CA
	// (set) Token: 0x06004F46 RID: 20294 RVA: 0x0002B3D2 File Offset: 0x000295D2
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

	// Token: 0x06004F47 RID: 20295 RVA: 0x0012F3DC File Offset: 0x0012D5DC
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

	// Token: 0x06004F48 RID: 20296 RVA: 0x0002B3DB File Offset: 0x000295DB
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

	// Token: 0x06004F49 RID: 20297 RVA: 0x00002FCA File Offset: 0x000011CA
	private void DeactivateOtherRooms()
	{
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x0012F430 File Offset: 0x0012D630
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

	// Token: 0x06004F4B RID: 20299 RVA: 0x0012F550 File Offset: 0x0012D750
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

	// Token: 0x06004F4C RID: 20300 RVA: 0x0002B3EA File Offset: 0x000295EA
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

	// Token: 0x06004F4D RID: 20301 RVA: 0x0002B400 File Offset: 0x00029600
	private IEnumerator CompleteWorldCreation()
	{
		yield return null;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.LevelEditorWorldCreationComplete, this, new LevelEditorWorldCreationCompleteEventArgs(this.m_currentRoom));
		yield return null;
		yield return null;
		yield break;
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x0012F5F4 File Offset: 0x0012D7F4
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

	// Token: 0x06004F4F RID: 20303 RVA: 0x0012F674 File Offset: 0x0012D874
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

	// Token: 0x06004F50 RID: 20304 RVA: 0x0002B40F File Offset: 0x0002960F
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

	// Token: 0x06004F51 RID: 20305 RVA: 0x0012F6D8 File Offset: 0x0012D8D8
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

	// Token: 0x06004F52 RID: 20306 RVA: 0x0002B41E File Offset: 0x0002961E
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

	// Token: 0x06004F53 RID: 20307 RVA: 0x0012F70C File Offset: 0x0012D90C
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

	// Token: 0x06004F54 RID: 20308 RVA: 0x0012F7A0 File Offset: 0x0012D9A0
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

	// Token: 0x06004F55 RID: 20309 RVA: 0x0012F848 File Offset: 0x0012DA48
	private void CycleThroughBiomes()
	{
		this.m_currentBiomeIndexInArtDataLibrary++;
		if (this.m_currentBiomeIndexInArtDataLibrary == BiomeArtDataLibrary.ArtDataTable.Count)
		{
			this.m_currentBiomeIndexInArtDataLibrary = 0;
		}
		OnPlayManager.SetCurrentBiome(BiomeArtDataLibrary.ArtDataTable[this.m_currentBiomeIndexInArtDataLibrary].BiomeType);
	}

	// Token: 0x06004F56 RID: 20310 RVA: 0x0002B42D File Offset: 0x0002962D
	private void CycleThroughDifficultyLevels()
	{
		GameUtility.Difficulty++;
		Debug.LogFormat("<color=purple>Difficulty Level = {0}</purple>", new object[]
		{
			GameUtility.Difficulty
		});
	}

	// Token: 0x06004F57 RID: 20311 RVA: 0x0012F898 File Offset: 0x0012DA98
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

	// Token: 0x06004F58 RID: 20312 RVA: 0x0012F8F0 File Offset: 0x0012DAF0
	private void OnTunnelDestinationCreated(object sender, RoomEventArgs eventArgs)
	{
		Room room = eventArgs.Room as Room;
		this.m_additionalRooms.Add(room);
		this.CreateSky(room);
		this.CreateWeather(room);
		BiomeCreatorTools.CreateCinemachineVirtualCamera(eventArgs.Room);
	}

	// Token: 0x06004F59 RID: 20313 RVA: 0x0002B458 File Offset: 0x00029658
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

	// Token: 0x06004F5A RID: 20314 RVA: 0x0002B467 File Offset: 0x00029667
	private void RandomizeHazardsInRoom()
	{
		HazardRandomizer.RandomizeHazards(from entry in this.BaseRoom.SpawnControllerManager.SpawnControllers
		select entry as IHazardSpawnController);
	}

	// Token: 0x06004F5B RID: 20315 RVA: 0x0002B4A2 File Offset: 0x000296A2
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

	// Token: 0x06004F5C RID: 20316 RVA: 0x0002B4B1 File Offset: 0x000296B1
	public static void Restart()
	{
		GameEventTrackerManager.Reset();
		MapController.Reset();
		OnPlayManager.Instance.SetupWorld();
	}

	// Token: 0x06004F5D RID: 20317 RVA: 0x0012F930 File Offset: 0x0012DB30
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

	// Token: 0x06004F5E RID: 20318 RVA: 0x0002B4C7 File Offset: 0x000296C7
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

	// Token: 0x06004F5F RID: 20319 RVA: 0x0002B4FC File Offset: 0x000296FC
	public static void SetDifficulty(int difficulty)
	{
		GameUtility.Difficulty = difficulty;
		OnPlayManager.Instance.m_buildRoomCoroutine = OnPlayManager.Instance.StartCoroutine(OnPlayManager.Instance.RecreateRoom());
	}

	// Token: 0x06004F60 RID: 20320 RVA: 0x0002B522 File Offset: 0x00029722
	public static void SetCameraConstrainerIsEnabled(bool isEnabled)
	{
		OnPlayManager.IsCameraConstrainerEnabled = isEnabled;
		bool isPlaying = Application.isPlaying;
		if (Application.isPlaying)
		{
			OnPlayManager.Instance.m_currentRoom.CinemachineCamera.GetComponent<CinemachineConfiner_RL>().enabled = OnPlayManager.IsCameraConstrainerEnabled;
		}
	}

	// Token: 0x06004F61 RID: 20321 RVA: 0x0002B555 File Offset: 0x00029755
	private static void SetCurrentBiome(BiomeType biome)
	{
		OnPlayManager.Instance.m_previousBiome = OnPlayManager.CurrentBiome;
		OnPlayManager.CurrentBiome = biome;
		CameraController.SetCameraSettingsForBiome(OnPlayManager.CurrentBiome, null);
	}

	// Token: 0x06004F62 RID: 20322 RVA: 0x0002B577 File Offset: 0x00029777
	public static void SetStartingBiome(BiomeType biome)
	{
		if (!BiomeType_RL.IsValidBiome(biome))
		{
			return;
		}
		OnPlayManager.StartingBiome = biome;
	}

	// Token: 0x06004F63 RID: 20323 RVA: 0x0002B588 File Offset: 0x00029788
	public void SetupWorld()
	{
		PlayerManager.GetPlayerController().ResetCharacter();
		this.m_buildRoomCoroutine = base.StartCoroutine(this.RecreateRoom());
	}

	// Token: 0x06004F64 RID: 20324 RVA: 0x0012F9C0 File Offset: 0x0012DBC0
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

	// Token: 0x06004F65 RID: 20325 RVA: 0x0012FA3C File Offset: 0x0012DC3C
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

	// Token: 0x06004F66 RID: 20326 RVA: 0x0002B5A6 File Offset: 0x000297A6
	public void ClearActiveRooms()
	{
		this.m_additionalRooms.Clear();
		this.m_currentRoom = null;
	}

	// Token: 0x06004F67 RID: 20327 RVA: 0x0002B5BA File Offset: 0x000297BA
	private void StoreReferenceToBaseRoom()
	{
		this.m_baseRoom = this.m_roomOverride;
		BiomeCreatorTools.CreateCinemachineVirtualCamera(this.BaseRoom);
		this.BaseRoom.gameObject.SetActive(false);
	}

	// Token: 0x06004F68 RID: 20328 RVA: 0x0012FAEC File Offset: 0x0012DCEC
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

	// Token: 0x06004F69 RID: 20329 RVA: 0x0002B5E5 File Offset: 0x000297E5
	private void AddCurrentRoomToGrid(Room room)
	{
		new RoomMetaData(room);
	}

	// Token: 0x04003C27 RID: 15399
	private List<Room> m_additionalRooms;

	// Token: 0x04003C28 RID: 15400
	private Room m_baseRoom;

	// Token: 0x04003C29 RID: 15401
	private Coroutine m_buildRoomCoroutine;

	// Token: 0x04003C2A RID: 15402
	private bool m_closeAllDoorsButOneToggle;

	// Token: 0x04003C2B RID: 15403
	private int m_currentBiomeIndexInArtDataLibrary;

	// Token: 0x04003C2C RID: 15404
	private int m_currentDoorIndex = -1;

	// Token: 0x04003C2D RID: 15405
	private BaseRoom m_currentRoom;

	// Token: 0x04003C2E RID: 15406
	private int m_cutoutCount = -1;

	// Token: 0x04003C2F RID: 15407
	private int m_doorCount = -1;

	// Token: 0x04003C30 RID: 15408
	private static OnPlayManager m_instance = null;

	// Token: 0x04003C31 RID: 15409
	private GameObject m_player;

	// Token: 0x04003C32 RID: 15410
	private RoomCreator m_roomCreator;

	// Token: 0x04003C33 RID: 15411
	private Dictionary<SkyBiomeArtData, Sky> m_skyTable = new Dictionary<SkyBiomeArtData, Sky>();

	// Token: 0x04003C34 RID: 15412
	private Dictionary<WeatherBiomeArtData, Weather[]> m_weatherTable = new Dictionary<WeatherBiomeArtData, Weather[]>();

	// Token: 0x04003C35 RID: 15413
	private static Vector2 ROOM_FOR_TUNNEL_TESTING_POSITION = new Vector2(1000f, 1000f);

	// Token: 0x04003C36 RID: 15414
	private const string CREATE_TRANSITION_POINTS_EDITOR_PREFS_KEY = "CreateTransitionPoints";

	// Token: 0x04003C37 RID: 15415
	[SerializeField]
	private Room m_roomOverride;

	// Token: 0x04003C38 RID: 15416
	[SerializeField]
	private KeyCode m_cycleClosedDoorKey = KeyCode.KeypadPlus;

	// Token: 0x04003C39 RID: 15417
	[SerializeField]
	private KeyCode m_toggleCloseAllDoorsButOneKey = KeyCode.KeypadMinus;

	// Token: 0x04003C3A RID: 15418
	[SerializeField]
	private KeyCode m_cycleBiome = KeyCode.End;

	// Token: 0x04003C3B RID: 15419
	[SerializeField]
	private KeyCode m_cycleTransitionPointBiome = KeyCode.PageDown;

	// Token: 0x04003C3C RID: 15420
	[SerializeField]
	private KeyCode m_flipRoom = KeyCode.Insert;

	// Token: 0x04003C3D RID: 15421
	[SerializeField]
	private KeyCode m_cycleDifficulty = KeyCode.Period;

	// Token: 0x04003C3E RID: 15422
	[SerializeField]
	private KeyCode m_randomizeHazards = KeyCode.Slash;

	// Token: 0x04003C3F RID: 15423
	[SerializeField]
	[ReadOnly]
	private BiomeType m_currentBiome;

	// Token: 0x04003C40 RID: 15424
	[SerializeField]
	private bool m_attachRoomsToDoors = true;

	// Token: 0x04003C41 RID: 15425
	[SerializeField]
	private bool m_flipCurrentRoomHorizontally;

	// Token: 0x04003C42 RID: 15426
	[SerializeField]
	private BiomeType m_startingBiome = BiomeType.Castle;

	// Token: 0x04003C43 RID: 15427
	[SerializeField]
	private bool m_isCameraConstrainerEnabled = true;

	// Token: 0x04003C44 RID: 15428
	[Header("Overrides")]
	[SerializeField]
	private int m_roomLevel = 1;

	// Token: 0x04003C46 RID: 15430
	private BiomeType m_previousBiome;

	// Token: 0x04003C47 RID: 15431
	private BiomeController m_biomeController;
}

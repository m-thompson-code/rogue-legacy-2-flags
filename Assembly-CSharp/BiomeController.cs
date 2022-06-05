using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

// Token: 0x02000A17 RID: 2583
public class BiomeController : MonoBehaviour
{
	// Token: 0x17001AD9 RID: 6873
	// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x0002A2EC File Offset: 0x000284EC
	public int ChestCount
	{
		get
		{
			if (this.m_chestCount == -1)
			{
				this.m_chestCount = base.gameObject.GetComponentsInChildren<ChestObj>().Length;
			}
			return this.m_chestCount;
		}
	}

	// Token: 0x17001ADA RID: 6874
	// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x0002A310 File Offset: 0x00028510
	public int StandaloneRoomCount
	{
		get
		{
			if (this.StandaloneRooms != null)
			{
				return this.StandaloneRooms.Count;
			}
			return 0;
		}
	}

	// Token: 0x17001ADB RID: 6875
	// (get) Token: 0x06004DB9 RID: 19897 RVA: 0x0002A327 File Offset: 0x00028527
	public int MergeRoomCount
	{
		get
		{
			if (this.MergeRooms != null)
			{
				return this.MergeRooms.Count;
			}
			return 0;
		}
	}

	// Token: 0x17001ADC RID: 6876
	// (get) Token: 0x06004DBA RID: 19898 RVA: 0x0002A33E File Offset: 0x0002853E
	public int CurrentRoomCount
	{
		get
		{
			return this.StandaloneRoomCount;
		}
	}

	// Token: 0x17001ADD RID: 6877
	// (get) Token: 0x06004DBB RID: 19899 RVA: 0x0012C304 File Offset: 0x0012A504
	public int TargetRoomCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<RoomType, int> keyValuePair in this.TargetRoomCountsByRoomType)
			{
				num += keyValuePair.Value;
			}
			return num;
		}
	}

	// Token: 0x17001ADE RID: 6878
	// (get) Token: 0x06004DBC RID: 19900 RVA: 0x0002A346 File Offset: 0x00028546
	// (set) Token: 0x06004DBD RID: 19901 RVA: 0x0002A34E File Offset: 0x0002854E
	public Bounds Bounds { get; private set; }

	// Token: 0x17001ADF RID: 6879
	// (get) Token: 0x06004DBE RID: 19902 RVA: 0x0012C360 File Offset: 0x0012A560
	public Room TransitionRoom
	{
		get
		{
			if (this.Biome == BiomeType.Garden && SaveManager.PlayerSaveData.EndingSpawnRoom != EndingSpawnRoomType.None)
			{
				foreach (BaseRoom baseRoom in this.Rooms)
				{
					Room room = (Room)baseRoom;
					EndingSpawnRoomTypeController component = room.GetComponent<EndingSpawnRoomTypeController>();
					if (component && component.EndingSpawnRoomType == SaveManager.PlayerSaveData.EndingSpawnRoom)
					{
						return room;
					}
				}
			}
			return this.m_transitionRoom;
		}
	}

	// Token: 0x17001AE0 RID: 6880
	// (get) Token: 0x06004DBF RID: 19903 RVA: 0x0002A357 File Offset: 0x00028557
	// (set) Token: 0x06004DC0 RID: 19904 RVA: 0x0002A35F File Offset: 0x0002855F
	public List<Room> StandaloneRooms { get; private set; }

	// Token: 0x17001AE1 RID: 6881
	// (get) Token: 0x06004DC1 RID: 19905 RVA: 0x0002A368 File Offset: 0x00028568
	// (set) Token: 0x06004DC2 RID: 19906 RVA: 0x0002A370 File Offset: 0x00028570
	public List<MergeRoom> MergeRooms { get; private set; }

	// Token: 0x17001AE2 RID: 6882
	// (get) Token: 0x06004DC3 RID: 19907 RVA: 0x0002A379 File Offset: 0x00028579
	// (set) Token: 0x06004DC4 RID: 19908 RVA: 0x0002A381 File Offset: 0x00028581
	public List<Room> RoomsConnectedByTunnel { get; private set; }

	// Token: 0x17001AE3 RID: 6883
	// (get) Token: 0x06004DC5 RID: 19909 RVA: 0x0002A38A File Offset: 0x0002858A
	// (set) Token: 0x06004DC6 RID: 19910 RVA: 0x0002A392 File Offset: 0x00028592
	public List<BaseRoom> Rooms { get; private set; }

	// Token: 0x17001AE4 RID: 6884
	// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x0002A39B File Offset: 0x0002859B
	public int TotalRoomCount
	{
		get
		{
			if (this.Rooms == null)
			{
				return 0;
			}
			return this.Rooms.Count;
		}
	}

	// Token: 0x17001AE5 RID: 6885
	// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x0002A3B2 File Offset: 0x000285B2
	// (set) Token: 0x06004DC9 RID: 19913 RVA: 0x0002A3BA File Offset: 0x000285BA
	public bool IsInitialised { get; private set; }

	// Token: 0x17001AE6 RID: 6886
	// (get) Token: 0x06004DCA RID: 19914 RVA: 0x0012C3F4 File Offset: 0x0012A5F4
	public bool IsInstantiated
	{
		get
		{
			if (this.Biome == BiomeType.Castle)
			{
				return this.Rooms != null && this.Rooms.Count > 1 && this.Rooms[0].RoomType == RoomType.Transition;
			}
			return this.Rooms != null && this.Rooms.Count > 0;
		}
	}

	// Token: 0x17001AE7 RID: 6887
	// (get) Token: 0x06004DCB RID: 19915 RVA: 0x0002A3C3 File Offset: 0x000285C3
	// (set) Token: 0x06004DCC RID: 19916 RVA: 0x0002A3CB File Offset: 0x000285CB
	public BiomeType Biome
	{
		get
		{
			return this.m_biome;
		}
		private set
		{
			this.m_biome = value;
		}
	}

	// Token: 0x17001AE8 RID: 6888
	// (get) Token: 0x06004DCD RID: 19917 RVA: 0x0002A3D4 File Offset: 0x000285D4
	// (set) Token: 0x06004DCE RID: 19918 RVA: 0x0002A3DC File Offset: 0x000285DC
	public Dictionary<RoomSide, Room> ConnectionRoomTable { get; set; }

	// Token: 0x17001AE9 RID: 6889
	// (get) Token: 0x06004DCF RID: 19919 RVA: 0x0002A3E5 File Offset: 0x000285E5
	// (set) Token: 0x06004DD0 RID: 19920 RVA: 0x0002A3ED File Offset: 0x000285ED
	public List<ISpawnController> SpawnControllers { get; private set; }

	// Token: 0x17001AEA RID: 6890
	// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x0002A3F6 File Offset: 0x000285F6
	public Bounds Border
	{
		get
		{
			return this.GridPointManager.Border;
		}
	}

	// Token: 0x17001AEB RID: 6891
	// (get) Token: 0x06004DD2 RID: 19922 RVA: 0x0002A403 File Offset: 0x00028603
	// (set) Token: 0x06004DD3 RID: 19923 RVA: 0x0002A40B File Offset: 0x0002860B
	public Dictionary<Vector2Int, int> RoomSizeCount { get; set; }

	// Token: 0x17001AEC RID: 6892
	// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x0002A414 File Offset: 0x00028614
	// (set) Token: 0x06004DD5 RID: 19925 RVA: 0x0002A41C File Offset: 0x0002861C
	public Dictionary<RoomType, int> TargetRoomCountsByRoomType { get; set; }

	// Token: 0x17001AED RID: 6893
	// (get) Token: 0x06004DD6 RID: 19926 RVA: 0x0002A425 File Offset: 0x00028625
	public List<Tunnel> Tunnels
	{
		get
		{
			return base.gameObject.GetComponentsInChildren<Tunnel>().ToList<Tunnel>();
		}
	}

	// Token: 0x17001AEE RID: 6894
	// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x0002A437 File Offset: 0x00028637
	public TunnelSpawnController[] TunnelSpawnControllers
	{
		get
		{
			return base.gameObject.GetComponentsInChildren<TunnelSpawnController>(true);
		}
	}

	// Token: 0x17001AEF RID: 6895
	// (get) Token: 0x06004DD8 RID: 19928 RVA: 0x0012C454 File Offset: 0x0012A654
	public Transform StandardRoomStorageLocation
	{
		get
		{
			if (this.m_standardRoomStorageLocation == null)
			{
				GameObject gameObject = new GameObject(BiomeController.STANDARD_ROOM_STORAGE_LOCATION);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				this.m_standardRoomStorageLocation = gameObject.transform;
			}
			return this.m_standardRoomStorageLocation;
		}
	}

	// Token: 0x17001AF0 RID: 6896
	// (get) Token: 0x06004DD9 RID: 19929 RVA: 0x0012C4B0 File Offset: 0x0012A6B0
	public Transform RoomsConnectedByTunnelStorageLocation
	{
		get
		{
			if (this.m_roomsConnectedByTunnelStorageLocation == null)
			{
				GameObject gameObject = new GameObject(BiomeController.ROOMS_CONNECTED_BY_TUNNEL_STORAGE_LOCATION);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				this.m_roomsConnectedByTunnelStorageLocation = gameObject.transform;
			}
			return this.m_roomsConnectedByTunnelStorageLocation;
		}
	}

	// Token: 0x17001AF1 RID: 6897
	// (get) Token: 0x06004DDA RID: 19930 RVA: 0x0002A445 File Offset: 0x00028645
	// (set) Token: 0x06004DDB RID: 19931 RVA: 0x0002A44D File Offset: 0x0002864D
	public List<CinemachineVirtualCamera> CinemachineVirtualCameras { get; private set; }

	// Token: 0x17001AF2 RID: 6898
	// (get) Token: 0x06004DDC RID: 19932 RVA: 0x0002A456 File Offset: 0x00028656
	public BiomeData BiomeData
	{
		get
		{
			if (this.m_biomeData == null && this.Biome != BiomeType.None)
			{
				this.m_biomeData = BiomeDataLibrary.GetData(this.Biome);
			}
			return this.m_biomeData;
		}
	}

	// Token: 0x17001AF3 RID: 6899
	// (get) Token: 0x06004DDD RID: 19933 RVA: 0x0002A485 File Offset: 0x00028685
	public BiomeGridPointManager GridPointManager
	{
		get
		{
			if (this.m_gridPointManager == null)
			{
				this.m_gridPointManager = new BiomeGridPointManager(this.Biome);
			}
			return this.m_gridPointManager;
		}
	}

	// Token: 0x17001AF4 RID: 6900
	// (get) Token: 0x06004DDE RID: 19934 RVA: 0x0002A4A6 File Offset: 0x000286A6
	// (set) Token: 0x06004DDF RID: 19935 RVA: 0x0002A4AE File Offset: 0x000286AE
	public List<Sky> Skies { get; private set; }

	// Token: 0x17001AF5 RID: 6901
	// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x0002A4B7 File Offset: 0x000286B7
	// (set) Token: 0x06004DE1 RID: 19937 RVA: 0x0002A4BF File Offset: 0x000286BF
	public List<Weather> Weather { get; private set; }

	// Token: 0x06004DE2 RID: 19938 RVA: 0x0002A4C8 File Offset: 0x000286C8
	public void Initialise(BiomeType biome)
	{
		if (this.IsInitialised)
		{
			throw new Exception("BiomeController has already been initialised. Method should only be called once.");
		}
		if (biome == BiomeType.None || biome == BiomeType.Any)
		{
			throw new ArgumentException("biome");
		}
		this.Biome = biome;
		this.IsInitialised = true;
	}

	// Token: 0x06004DE3 RID: 19939 RVA: 0x0002A501 File Offset: 0x00028701
	public void SetTransitionRoom(Room transitionRoom)
	{
		this.m_transitionRoom = transitionRoom;
	}

	// Token: 0x06004DE4 RID: 19940 RVA: 0x0002A50A File Offset: 0x0002870A
	public void StandardRoomCreatedByWorldBuilder(Room room)
	{
		this.AddRoom(room);
	}

	// Token: 0x06004DE5 RID: 19941 RVA: 0x0012C50C File Offset: 0x0012A70C
	private void AddRoom(BaseRoom room)
	{
		if (this.Rooms == null)
		{
			this.Rooms = new List<BaseRoom>();
		}
		this.Rooms.Add(room);
		if (room is Room)
		{
			if (this.StandaloneRooms == null)
			{
				this.StandaloneRooms = new List<Room>();
			}
			this.StandaloneRooms.Add(room as Room);
			if (this.Bounds == default(Bounds))
			{
				this.Bounds = new Bounds(room.gameObject.transform.position, Vector3.zero);
			}
			this.GrowBounds(room as Room);
			if (this.SpawnControllers == null)
			{
				this.SpawnControllers = new List<ISpawnController>();
			}
			this.SpawnControllers.AddRange(room.gameObject.GetComponentsInChildren<ISpawnController>());
			return;
		}
		if (room is MergeRoom)
		{
			if (this.MergeRooms == null)
			{
				this.MergeRooms = new List<MergeRoom>();
			}
			this.MergeRooms.Add(room as MergeRoom);
		}
	}

	// Token: 0x06004DE6 RID: 19942 RVA: 0x0012C600 File Offset: 0x0012A800
	public BaseRoom GetRoom(int biomeControllerIndex)
	{
		if (this.Rooms == null)
		{
			return null;
		}
		if (biomeControllerIndex < 0 || biomeControllerIndex > this.Rooms.Count)
		{
			int currentSeed = RNGSeedManager.GetCurrentSeed(SceneLoadingUtility.ActiveScene.name);
			Debug.Log(string.Concat(new string[]
			{
				"roomIndexOutOfRange - BiomeControllerIndex: ",
				biomeControllerIndex.ToString(),
				" biomeType: ",
				this.Biome.ToString(),
				" seed: ",
				RNGSeedManager.GetSeedAsHex(currentSeed),
				"-",
				BurdenManager.GetBurdenLevel(BurdenType.RoomCount).ToString()
			}));
			throw new IndexOutOfRangeException(string.Concat(new string[]
			{
				"roomIndex - BiomeControllerIndex: ",
				biomeControllerIndex.ToString(),
				" biomeType: ",
				this.Biome.ToString(),
				" seed: ",
				RNGSeedManager.GetSeedAsHex(currentSeed),
				"-",
				BurdenManager.GetBurdenLevel(BurdenType.RoomCount).ToString()
			}));
		}
		int index = 0;
		for (int i = 0; i < this.Rooms.Count; i++)
		{
			if (this.Rooms[i].BiomeControllerIndex == biomeControllerIndex)
			{
				index = i;
				break;
			}
		}
		return this.Rooms[index];
	}

	// Token: 0x06004DE7 RID: 19943 RVA: 0x0002A50A File Offset: 0x0002870A
	public void MergeRoomCreatedByWorldBuilder(MergeRoom mergeRoom)
	{
		this.AddRoom(mergeRoom);
	}

	// Token: 0x06004DE8 RID: 19944 RVA: 0x0002A513 File Offset: 0x00028713
	public void RoomConnectedByTunnelCreatedByWorldBuilder(Room room)
	{
		if (this.Rooms == null)
		{
			this.Rooms = new List<BaseRoom>();
		}
		if (this.RoomsConnectedByTunnel == null)
		{
			this.RoomsConnectedByTunnel = new List<Room>();
		}
		this.RoomsConnectedByTunnel.Add(room);
		this.Rooms.Add(room);
	}

	// Token: 0x06004DE9 RID: 19945 RVA: 0x0012C75C File Offset: 0x0012A95C
	private void GrowBounds(Room room)
	{
		Bounds bounds = default(Bounds);
		bounds = this.Bounds;
		bounds.Encapsulate(room.Bounds);
		this.Bounds = bounds;
	}

	// Token: 0x06004DEA RID: 19946 RVA: 0x0002A553 File Offset: 0x00028753
	public void InvalidConnectionRoomReplacedByWorldBuiler(Room replacedRoom, Room replacementRoom)
	{
		if (this.StandaloneRooms.Contains(replacedRoom))
		{
			this.StandaloneRooms.Remove(replacedRoom);
		}
		else
		{
			Debug.LogFormat("<color=red>[{0}] StandaloneRooms does not replacedRoom, but should.</color>", new object[]
			{
				this
			});
		}
		this.StandaloneRooms.Add(replacementRoom);
	}

	// Token: 0x06004DEB RID: 19947 RVA: 0x0012C78C File Offset: 0x0012A98C
	public void Reset()
	{
		bool flag = WorldBuilder.Instance.LevelManager.Mode != LevelManagerMode.Debug;
		if (this.StandaloneRooms != null)
		{
			if (this.Biome == BiomeType.Castle && flag)
			{
				for (int i = this.StandaloneRooms.Count - 1; i >= 0; i--)
				{
					if (this.StandaloneRooms[i].RoomType != RoomType.Transition)
					{
						this.StandaloneRooms[i] = null;
						this.StandaloneRooms.RemoveAt(i);
					}
				}
			}
			else
			{
				this.StandaloneRooms.Clear();
			}
		}
		if (this.MergeRooms != null)
		{
			this.MergeRooms.Clear();
		}
		for (int j = this.Rooms.Count - 1; j >= 0; j--)
		{
			if (this.Biome != BiomeType.Castle || !flag || this.Rooms[j].RoomType != RoomType.Transition)
			{
				UnityEngine.Object.Destroy(this.Rooms[j].gameObject);
			}
		}
		if (this.Rooms != null)
		{
			if (this.Biome == BiomeType.Castle && flag)
			{
				for (int k = this.Rooms.Count - 1; k >= 0; k--)
				{
					if (this.Rooms[k].RoomType != RoomType.Transition)
					{
						this.Rooms[k] = null;
						this.Rooms.RemoveAt(k);
					}
				}
			}
			else
			{
				this.Rooms.Clear();
			}
		}
		if (this.RoomsConnectedByTunnel != null)
		{
			this.RoomsConnectedByTunnel.Clear();
		}
		if (this.CinemachineVirtualCameras != null)
		{
			this.CinemachineVirtualCameras.Clear();
		}
		this.GridPointManager.RemoveTunnelDestinations();
		if (this.Skies != null)
		{
			for (int l = this.Skies.Count - 1; l >= 0; l--)
			{
				UnityEngine.Object.Destroy(this.Skies[l].gameObject);
			}
			this.Skies.Clear();
		}
		if (this.Weather != null)
		{
			for (int m = this.Weather.Count - 1; m >= 0; m--)
			{
				UnityEngine.Object.Destroy(this.Weather[m].gameObject);
			}
			this.Weather.Clear();
		}
	}

	// Token: 0x06004DEC RID: 19948 RVA: 0x0002A592 File Offset: 0x00028792
	public void CinemachineVirtualCameraCreatedByWorldBuilder(CinemachineVirtualCamera cinemachineVirtualCamera)
	{
		if (this.CinemachineVirtualCameras == null)
		{
			this.CinemachineVirtualCameras = new List<CinemachineVirtualCamera>();
		}
		this.CinemachineVirtualCameras.Add(cinemachineVirtualCamera);
	}

	// Token: 0x06004DED RID: 19949 RVA: 0x0002A5B3 File Offset: 0x000287B3
	public int GetRemainingRoomCount()
	{
		return this.TargetRoomCount - this.CurrentRoomCount;
	}

	// Token: 0x06004DEE RID: 19950 RVA: 0x0002A5C2 File Offset: 0x000287C2
	public void SetConnectionPoint(RoomSide side, Room connectionPoint)
	{
		if (this.ConnectionRoomTable == null)
		{
			this.ConnectionRoomTable = new Dictionary<RoomSide, Room>();
		}
		this.ConnectionRoomTable.Add(side, connectionPoint);
	}

	// Token: 0x06004DEF RID: 19951 RVA: 0x0002A5E4 File Offset: 0x000287E4
	public void SkyCreatedByWorldBuilder(Sky sky)
	{
		if (this.Skies == null)
		{
			this.Skies = new List<Sky>();
		}
		if (!this.Skies.Contains(sky))
		{
			this.Skies.Add(sky);
		}
	}

	// Token: 0x06004DF0 RID: 19952 RVA: 0x0012C9A4 File Offset: 0x0012ABA4
	public void WeatherCreatedByWorldBuilder(Weather[] weather)
	{
		if (this.Weather == null)
		{
			this.Weather = new List<Weather>();
		}
		for (int i = 0; i < weather.Length; i++)
		{
			this.Weather.Add(weather[i]);
		}
	}

	// Token: 0x04003ACE RID: 15054
	private static string STANDARD_ROOM_STORAGE_LOCATION = "Rooms";

	// Token: 0x04003ACF RID: 15055
	private static string ROOMS_CONNECTED_BY_TUNNEL_STORAGE_LOCATION = "Rooms Connected By Tunnel";

	// Token: 0x04003AD0 RID: 15056
	private BiomeType m_biome;

	// Token: 0x04003AD1 RID: 15057
	private int m_chestCount = -1;

	// Token: 0x04003AD2 RID: 15058
	private Transform m_standardRoomStorageLocation;

	// Token: 0x04003AD3 RID: 15059
	private Transform m_roomsConnectedByTunnelStorageLocation;

	// Token: 0x04003AD4 RID: 15060
	private Bounds m_border;

	// Token: 0x04003AD5 RID: 15061
	private BiomeData m_biomeData;

	// Token: 0x04003AD6 RID: 15062
	private BiomeGridPointManager m_gridPointManager;

	// Token: 0x04003AD8 RID: 15064
	private Room m_transitionRoom;
}

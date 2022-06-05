using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class BiomeController : MonoBehaviour
{
	// Token: 0x17001386 RID: 4998
	// (get) Token: 0x0600372E RID: 14126 RVA: 0x000BD3CE File Offset: 0x000BB5CE
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

	// Token: 0x17001387 RID: 4999
	// (get) Token: 0x0600372F RID: 14127 RVA: 0x000BD3F2 File Offset: 0x000BB5F2
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

	// Token: 0x17001388 RID: 5000
	// (get) Token: 0x06003730 RID: 14128 RVA: 0x000BD409 File Offset: 0x000BB609
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

	// Token: 0x17001389 RID: 5001
	// (get) Token: 0x06003731 RID: 14129 RVA: 0x000BD420 File Offset: 0x000BB620
	public int CurrentRoomCount
	{
		get
		{
			return this.StandaloneRoomCount;
		}
	}

	// Token: 0x1700138A RID: 5002
	// (get) Token: 0x06003732 RID: 14130 RVA: 0x000BD428 File Offset: 0x000BB628
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

	// Token: 0x1700138B RID: 5003
	// (get) Token: 0x06003733 RID: 14131 RVA: 0x000BD484 File Offset: 0x000BB684
	// (set) Token: 0x06003734 RID: 14132 RVA: 0x000BD48C File Offset: 0x000BB68C
	public Bounds Bounds { get; private set; }

	// Token: 0x1700138C RID: 5004
	// (get) Token: 0x06003735 RID: 14133 RVA: 0x000BD498 File Offset: 0x000BB698
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

	// Token: 0x1700138D RID: 5005
	// (get) Token: 0x06003736 RID: 14134 RVA: 0x000BD52C File Offset: 0x000BB72C
	// (set) Token: 0x06003737 RID: 14135 RVA: 0x000BD534 File Offset: 0x000BB734
	public List<Room> StandaloneRooms { get; private set; }

	// Token: 0x1700138E RID: 5006
	// (get) Token: 0x06003738 RID: 14136 RVA: 0x000BD53D File Offset: 0x000BB73D
	// (set) Token: 0x06003739 RID: 14137 RVA: 0x000BD545 File Offset: 0x000BB745
	public List<MergeRoom> MergeRooms { get; private set; }

	// Token: 0x1700138F RID: 5007
	// (get) Token: 0x0600373A RID: 14138 RVA: 0x000BD54E File Offset: 0x000BB74E
	// (set) Token: 0x0600373B RID: 14139 RVA: 0x000BD556 File Offset: 0x000BB756
	public List<Room> RoomsConnectedByTunnel { get; private set; }

	// Token: 0x17001390 RID: 5008
	// (get) Token: 0x0600373C RID: 14140 RVA: 0x000BD55F File Offset: 0x000BB75F
	// (set) Token: 0x0600373D RID: 14141 RVA: 0x000BD567 File Offset: 0x000BB767
	public List<BaseRoom> Rooms { get; private set; }

	// Token: 0x17001391 RID: 5009
	// (get) Token: 0x0600373E RID: 14142 RVA: 0x000BD570 File Offset: 0x000BB770
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

	// Token: 0x17001392 RID: 5010
	// (get) Token: 0x0600373F RID: 14143 RVA: 0x000BD587 File Offset: 0x000BB787
	// (set) Token: 0x06003740 RID: 14144 RVA: 0x000BD58F File Offset: 0x000BB78F
	public bool IsInitialised { get; private set; }

	// Token: 0x17001393 RID: 5011
	// (get) Token: 0x06003741 RID: 14145 RVA: 0x000BD598 File Offset: 0x000BB798
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

	// Token: 0x17001394 RID: 5012
	// (get) Token: 0x06003742 RID: 14146 RVA: 0x000BD5F5 File Offset: 0x000BB7F5
	// (set) Token: 0x06003743 RID: 14147 RVA: 0x000BD5FD File Offset: 0x000BB7FD
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

	// Token: 0x17001395 RID: 5013
	// (get) Token: 0x06003744 RID: 14148 RVA: 0x000BD606 File Offset: 0x000BB806
	// (set) Token: 0x06003745 RID: 14149 RVA: 0x000BD60E File Offset: 0x000BB80E
	public Dictionary<RoomSide, Room> ConnectionRoomTable { get; set; }

	// Token: 0x17001396 RID: 5014
	// (get) Token: 0x06003746 RID: 14150 RVA: 0x000BD617 File Offset: 0x000BB817
	// (set) Token: 0x06003747 RID: 14151 RVA: 0x000BD61F File Offset: 0x000BB81F
	public List<ISpawnController> SpawnControllers { get; private set; }

	// Token: 0x17001397 RID: 5015
	// (get) Token: 0x06003748 RID: 14152 RVA: 0x000BD628 File Offset: 0x000BB828
	public Bounds Border
	{
		get
		{
			return this.GridPointManager.Border;
		}
	}

	// Token: 0x17001398 RID: 5016
	// (get) Token: 0x06003749 RID: 14153 RVA: 0x000BD635 File Offset: 0x000BB835
	// (set) Token: 0x0600374A RID: 14154 RVA: 0x000BD63D File Offset: 0x000BB83D
	public Dictionary<Vector2Int, int> RoomSizeCount { get; set; }

	// Token: 0x17001399 RID: 5017
	// (get) Token: 0x0600374B RID: 14155 RVA: 0x000BD646 File Offset: 0x000BB846
	// (set) Token: 0x0600374C RID: 14156 RVA: 0x000BD64E File Offset: 0x000BB84E
	public Dictionary<RoomType, int> TargetRoomCountsByRoomType { get; set; }

	// Token: 0x1700139A RID: 5018
	// (get) Token: 0x0600374D RID: 14157 RVA: 0x000BD657 File Offset: 0x000BB857
	public List<Tunnel> Tunnels
	{
		get
		{
			return base.gameObject.GetComponentsInChildren<Tunnel>().ToList<Tunnel>();
		}
	}

	// Token: 0x1700139B RID: 5019
	// (get) Token: 0x0600374E RID: 14158 RVA: 0x000BD669 File Offset: 0x000BB869
	public TunnelSpawnController[] TunnelSpawnControllers
	{
		get
		{
			return base.gameObject.GetComponentsInChildren<TunnelSpawnController>(true);
		}
	}

	// Token: 0x1700139C RID: 5020
	// (get) Token: 0x0600374F RID: 14159 RVA: 0x000BD678 File Offset: 0x000BB878
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

	// Token: 0x1700139D RID: 5021
	// (get) Token: 0x06003750 RID: 14160 RVA: 0x000BD6D4 File Offset: 0x000BB8D4
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

	// Token: 0x1700139E RID: 5022
	// (get) Token: 0x06003751 RID: 14161 RVA: 0x000BD72D File Offset: 0x000BB92D
	// (set) Token: 0x06003752 RID: 14162 RVA: 0x000BD735 File Offset: 0x000BB935
	public List<CinemachineVirtualCamera> CinemachineVirtualCameras { get; private set; }

	// Token: 0x1700139F RID: 5023
	// (get) Token: 0x06003753 RID: 14163 RVA: 0x000BD73E File Offset: 0x000BB93E
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

	// Token: 0x170013A0 RID: 5024
	// (get) Token: 0x06003754 RID: 14164 RVA: 0x000BD76D File Offset: 0x000BB96D
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

	// Token: 0x170013A1 RID: 5025
	// (get) Token: 0x06003755 RID: 14165 RVA: 0x000BD78E File Offset: 0x000BB98E
	// (set) Token: 0x06003756 RID: 14166 RVA: 0x000BD796 File Offset: 0x000BB996
	public List<Sky> Skies { get; private set; }

	// Token: 0x170013A2 RID: 5026
	// (get) Token: 0x06003757 RID: 14167 RVA: 0x000BD79F File Offset: 0x000BB99F
	// (set) Token: 0x06003758 RID: 14168 RVA: 0x000BD7A7 File Offset: 0x000BB9A7
	public List<Weather> Weather { get; private set; }

	// Token: 0x06003759 RID: 14169 RVA: 0x000BD7B0 File Offset: 0x000BB9B0
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

	// Token: 0x0600375A RID: 14170 RVA: 0x000BD7E9 File Offset: 0x000BB9E9
	public void SetTransitionRoom(Room transitionRoom)
	{
		this.m_transitionRoom = transitionRoom;
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x000BD7F2 File Offset: 0x000BB9F2
	public void StandardRoomCreatedByWorldBuilder(Room room)
	{
		this.AddRoom(room);
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x000BD7FC File Offset: 0x000BB9FC
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

	// Token: 0x0600375D RID: 14173 RVA: 0x000BD8F0 File Offset: 0x000BBAF0
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

	// Token: 0x0600375E RID: 14174 RVA: 0x000BDA4C File Offset: 0x000BBC4C
	public void MergeRoomCreatedByWorldBuilder(MergeRoom mergeRoom)
	{
		this.AddRoom(mergeRoom);
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x000BDA55 File Offset: 0x000BBC55
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

	// Token: 0x06003760 RID: 14176 RVA: 0x000BDA98 File Offset: 0x000BBC98
	private void GrowBounds(Room room)
	{
		Bounds bounds = default(Bounds);
		bounds = this.Bounds;
		bounds.Encapsulate(room.Bounds);
		this.Bounds = bounds;
	}

	// Token: 0x06003761 RID: 14177 RVA: 0x000BDAC8 File Offset: 0x000BBCC8
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

	// Token: 0x06003762 RID: 14178 RVA: 0x000BDB08 File Offset: 0x000BBD08
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

	// Token: 0x06003763 RID: 14179 RVA: 0x000BDD1E File Offset: 0x000BBF1E
	public void CinemachineVirtualCameraCreatedByWorldBuilder(CinemachineVirtualCamera cinemachineVirtualCamera)
	{
		if (this.CinemachineVirtualCameras == null)
		{
			this.CinemachineVirtualCameras = new List<CinemachineVirtualCamera>();
		}
		this.CinemachineVirtualCameras.Add(cinemachineVirtualCamera);
	}

	// Token: 0x06003764 RID: 14180 RVA: 0x000BDD3F File Offset: 0x000BBF3F
	public int GetRemainingRoomCount()
	{
		return this.TargetRoomCount - this.CurrentRoomCount;
	}

	// Token: 0x06003765 RID: 14181 RVA: 0x000BDD4E File Offset: 0x000BBF4E
	public void SetConnectionPoint(RoomSide side, Room connectionPoint)
	{
		if (this.ConnectionRoomTable == null)
		{
			this.ConnectionRoomTable = new Dictionary<RoomSide, Room>();
		}
		this.ConnectionRoomTable.Add(side, connectionPoint);
	}

	// Token: 0x06003766 RID: 14182 RVA: 0x000BDD70 File Offset: 0x000BBF70
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

	// Token: 0x06003767 RID: 14183 RVA: 0x000BDDA0 File Offset: 0x000BBFA0
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

	// Token: 0x04002A7C RID: 10876
	private static string STANDARD_ROOM_STORAGE_LOCATION = "Rooms";

	// Token: 0x04002A7D RID: 10877
	private static string ROOMS_CONNECTED_BY_TUNNEL_STORAGE_LOCATION = "Rooms Connected By Tunnel";

	// Token: 0x04002A7E RID: 10878
	private BiomeType m_biome;

	// Token: 0x04002A7F RID: 10879
	private int m_chestCount = -1;

	// Token: 0x04002A80 RID: 10880
	private Transform m_standardRoomStorageLocation;

	// Token: 0x04002A81 RID: 10881
	private Transform m_roomsConnectedByTunnelStorageLocation;

	// Token: 0x04002A82 RID: 10882
	private Bounds m_border;

	// Token: 0x04002A83 RID: 10883
	private BiomeData m_biomeData;

	// Token: 0x04002A84 RID: 10884
	private BiomeGridPointManager m_gridPointManager;

	// Token: 0x04002A86 RID: 10886
	private Room m_transitionRoom;
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000783 RID: 1923
public class MergeRoom : BaseRoom, ISummoner, ILevelConsumer
{
	// Token: 0x170015B6 RID: 5558
	// (get) Token: 0x06003AD5 RID: 15061 RVA: 0x000F22BC File Offset: 0x000F04BC
	public override bool SpawnedAsEasyRoom
	{
		get
		{
			int num;
			if (BiomeCreation_EV.FORCE_EASY_ROOMS_AT_BIOME_START.TryGetValue(base.BiomeType, out num) && BurdenManager.GetTotalBurdenLevel() == 0 && this.StandaloneGridPointManagers.Length != 0)
			{
				int roomNumber = this.StandaloneGridPointManagers[0].RoomNumber;
				return this.StandaloneGridPointManagers[0].RoomType == RoomType.Standard && !this.StandaloneGridPointManagers[0].IsTunnelDestination && roomNumber <= num + 1;
			}
			return false;
		}
	}

	// Token: 0x170015B7 RID: 5559
	// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x0000F49B File Offset: 0x0000D69B
	public override BiomeArtData BiomeArtDataOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170015B8 RID: 5560
	// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x0002047C File Offset: 0x0001E67C
	public override Bounds Bounds
	{
		get
		{
			return this.m_bounds;
		}
	}

	// Token: 0x170015B9 RID: 5561
	// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x00020484 File Offset: 0x0001E684
	public override List<Door> Doors
	{
		get
		{
			if (this.m_doors == null)
			{
				this.m_doors = new List<Door>();
			}
			return this.m_doors;
		}
	}

	// Token: 0x170015BA RID: 5562
	// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x0002049F File Offset: 0x0001E69F
	// (set) Token: 0x06003ADA RID: 15066 RVA: 0x000204A7 File Offset: 0x0001E6A7
	public Dictionary<GridPointManager, GlobalTeleporterController> GlobalTeleporters
	{
		get
		{
			return this.m_globalTeleporters;
		}
		private set
		{
			this.m_globalTeleporters = value;
		}
	}

	// Token: 0x170015BB RID: 5563
	// (get) Token: 0x06003ADB RID: 15067 RVA: 0x000204B0 File Offset: 0x0001E6B0
	public override int Level
	{
		get
		{
			if (this.m_levelOverride != -1)
			{
				return this.m_levelOverride;
			}
			return this.StandaloneRoomLevels[0];
		}
	}

	// Token: 0x170015BC RID: 5564
	// (get) Token: 0x06003ADC RID: 15068 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override RoomType RoomType
	{
		get
		{
			return RoomType.Standard;
		}
	}

	// Token: 0x170015BD RID: 5565
	// (get) Token: 0x06003ADD RID: 15069 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override SpecialRoomType SpecialRoomType
	{
		get
		{
			return SpecialRoomType.None;
		}
	}

	// Token: 0x170015BE RID: 5566
	// (get) Token: 0x06003ADE RID: 15070 RVA: 0x000204CA File Offset: 0x0001E6CA
	// (set) Token: 0x06003ADF RID: 15071 RVA: 0x000204D2 File Offset: 0x0001E6D2
	public RoomEnemyManager[] StandaloneRoomEnemyManagers { get; private set; }

	// Token: 0x170015BF RID: 5567
	// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x000204DB File Offset: 0x0001E6DB
	// (set) Token: 0x06003AE1 RID: 15073 RVA: 0x000204E3 File Offset: 0x0001E6E3
	public Bounds[] StandaloneRoomBounds { get; private set; }

	// Token: 0x170015C0 RID: 5568
	// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x000204EC File Offset: 0x0001E6EC
	// (set) Token: 0x06003AE3 RID: 15075 RVA: 0x000204F4 File Offset: 0x0001E6F4
	public GridPointManager[] StandaloneGridPointManagers { get; private set; }

	// Token: 0x170015C1 RID: 5569
	// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x000204FD File Offset: 0x0001E6FD
	// (set) Token: 0x06003AE5 RID: 15077 RVA: 0x00020505 File Offset: 0x0001E705
	public int[] StandaloneRoomLevels { get; private set; }

	// Token: 0x170015C2 RID: 5570
	// (get) Token: 0x06003AE6 RID: 15078 RVA: 0x0002050E File Offset: 0x0001E70E
	// (set) Token: 0x06003AE7 RID: 15079 RVA: 0x00020516 File Offset: 0x0001E716
	public Dictionary<Transform, List<Ferr2DT_PathTerrain>> StandaloneRoomToTerrainTable
	{
		get
		{
			return this.m_standaloneRoomToTerrainTable;
		}
		private set
		{
			this.m_standaloneRoomToTerrainTable = value;
		}
	}

	// Token: 0x170015C3 RID: 5571
	// (get) Token: 0x06003AE8 RID: 15080 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool AllowItemDrops
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003AE9 RID: 15081 RVA: 0x0002051F File Offset: 0x0001E71F
	public void AddRooms(List<Room> rooms)
	{
		base.StartCoroutine(this.AddRoomsCoroutine(rooms));
	}

	// Token: 0x06003AEA RID: 15082 RVA: 0x0002052F File Offset: 0x0001E72F
	private IEnumerator AddRoomsCoroutine(List<Room> rooms)
	{
		if (rooms == null)
		{
			throw new ArgumentNullException("rooms");
		}
		if (rooms.Count < 2)
		{
			throw new ArgumentException("rooms", "Count must be greater than or equal to 2");
		}
		IEnumerable<IGrouping<BiomeType, Room>> enumerable = from entry in rooms
		group entry by entry.AppearanceBiomeType;
		if (enumerable.Count<IGrouping<BiomeType, Room>>() != 1)
		{
			string text = "";
			foreach (IGrouping<BiomeType, Room> grouping in enumerable)
			{
				text = text + grouping.Key.ToString() + " | ";
			}
			foreach (Room room in rooms)
			{
				Debug.Log("RoomID: " + room.GridPointManager.RoomMetaData.name + " BiomeType: " + room.AppearanceBiomeType.ToString());
			}
			throw new ArgumentException("Room.Biome", "Rooms must be from the same Biome. BiomeTypes found: " + text);
		}
		base.AppearanceBiomeType = rooms.First<Room>().AppearanceBiomeType;
		this.m_isInBiome = rooms.First<Room>().BiomeType;
		this.InitializeProperties(rooms);
		base.transform.position = this.Bounds.center;
		if (base.AppearanceBiomeType == BiomeType.Stone)
		{
			this.CreateBridgeBiomeChunkColliders(rooms);
		}
		this.PerformOperationsOnInternalDoors(rooms);
		BiomeController biomeController = WorldBuilder.GetBiomeController(base.BiomeType);
		foreach (Room room2 in rooms)
		{
			this.CopyRoom(room2);
			biomeController.Rooms.Remove(room2);
			if (base.Backgrounds == null && room2.Backgrounds != null && room2.Backgrounds.Length != 0)
			{
				base.SetBackgroundPoolEntries(room2.Backgrounds);
			}
		}
		base.SetSpawnControllerManager(new RoomSpawnControllerManager(this));
		base.SetTerrainManager(new RoomTerrainManager(this));
		base.SaveController = new RoomSaveController(this);
		yield return null;
		base.InjectRoom();
		yield break;
	}

	// Token: 0x06003AEB RID: 15083 RVA: 0x000F232C File Offset: 0x000F052C
	private void CopyRoom(Room room)
	{
		room.OnMerge();
		room.gameObject.transform.SetParent(base.transform, true);
		Component[] components = room.gameObject.GetComponents<Component>();
		for (int i = components.Length - 1; i >= 0; i--)
		{
			Component component = components[i];
			if (!(component is Transform) && !(component is Collider2D) && !(component is RoomEnemyManager))
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x00020545 File Offset: 0x0001E745
	private void CreateBridgeBiomeChunkColliders(List<Room> rooms)
	{
		GameObject gameObject = new GameObject("Merge Room Chunks");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.AddComponent<MergeRoomChunkManager>().Initialize(this, rooms);
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x000F2394 File Offset: 0x000F0594
	protected override int GetDecoSeed()
	{
		int result = -1;
		if (this.StandaloneGridPointManagers != null && this.StandaloneGridPointManagers.Length != 0)
		{
			result = this.StandaloneGridPointManagers[0].DecoSeed;
		}
		return result;
	}

	// Token: 0x06003AEE RID: 15086 RVA: 0x000F23C4 File Offset: 0x000F05C4
	private bool GetIsInternalDoor(List<Room> rooms, Door door)
	{
		bool flag = false;
		if (door.Side == RoomSide.Left)
		{
			flag = (Mathf.Abs(this.Bounds.min.x - door.CenterPoint.x) < 0.1f);
		}
		else if (door.Side == RoomSide.Right)
		{
			flag = (Mathf.Abs(this.Bounds.max.x - door.CenterPoint.x) < 0.1f);
		}
		else if (door.Side == RoomSide.Bottom)
		{
			flag = (Mathf.Abs(this.Bounds.min.y - door.CenterPoint.y) < 0.1f);
		}
		else if (door.Side == RoomSide.Top)
		{
			flag = (Mathf.Abs(this.Bounds.max.y - door.CenterPoint.y) < 0.1f);
		}
		return !flag;
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x000F24B4 File Offset: 0x000F06B4
	protected override int GetSpecialPropSeed()
	{
		int result = -1;
		if (this.StandaloneGridPointManagers != null && this.StandaloneGridPointManagers.Length != 0)
		{
			result = this.StandaloneGridPointManagers[0].SpecialPropSeed;
		}
		return result;
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x000F24E4 File Offset: 0x000F06E4
	private void GrowBounds(Room room)
	{
		Bounds bounds = default(Bounds);
		bounds = this.Bounds;
		bounds.Encapsulate(room.Bounds);
		this.m_bounds = bounds;
		float x = this.m_bounds.center.x - this.m_bounds.extents.x;
		float y = this.m_bounds.center.y - this.m_bounds.extents.y;
		base.BoundsRect = new Rect(new Vector2(x, y), this.m_bounds.size);
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x000F257C File Offset: 0x000F077C
	private void InitializeProperties(List<Room> rooms)
	{
		this.StandaloneGridPointManagers = new GridPointManager[rooms.Count];
		this.StandaloneRoomBounds = new Bounds[rooms.Count];
		this.StandaloneRoomLevels = new int[rooms.Count];
		this.StandaloneRoomEnemyManagers = new RoomEnemyManager[rooms.Count];
		this.m_bounds = new Bounds(rooms.First<Room>().gameObject.transform.position, Vector3.zero);
		float x = this.m_bounds.center.x - this.m_bounds.extents.x;
		float y = this.m_bounds.center.y - this.m_bounds.extents.y;
		base.BoundsRect = new Rect(new Vector2(x, y), this.m_bounds.size);
		for (int i = 0; i < rooms.Count; i++)
		{
			this.StandaloneRoomToTerrainTable.Add(rooms[i].gameObject.transform, rooms[i].TerrainManager.Platforms);
			this.StandaloneRoomBounds[i] = rooms[i].Bounds;
			this.StandaloneGridPointManagers[i] = rooms[i].GridPointManager;
			this.StandaloneRoomLevels[i] = rooms[i].Level;
			this.StandaloneRoomEnemyManagers[i] = rooms[i].RoomEnemyManager;
			if (rooms[i].GlobalTeleporter != null)
			{
				this.GlobalTeleporters.Add(rooms[i].GridPointManager, rooms[i].GlobalTeleporter);
			}
			this.GrowBounds(rooms[i]);
			base.ConnectedRooms.AddRange(rooms[i].ConnectedRooms);
		}
	}

	// Token: 0x06003AF2 RID: 15090 RVA: 0x000F274C File Offset: 0x000F094C
	private void PerformOperationsOnInternalDoors(List<Room> rooms)
	{
		foreach (Room room in rooms)
		{
			for (int i = room.Doors.Count - 1; i >= 0; i--)
			{
				Door door = room.Doors[i];
				if (this.GetIsInternalDoor(rooms, door))
				{
					UnityEngine.Object.Destroy(door.GameObject);
				}
				else
				{
					if (this.m_doors == null)
					{
						this.m_doors = new List<Door>();
					}
					door.PlayerEnterRelay.AddListener(new Action<object, DoorEventArgs>(base.OnPlayerExitViaDoor), false);
					this.m_doors.Add(door);
				}
			}
		}
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x0002057E File Offset: 0x0001E77E
	public override void SetLevel(int value)
	{
		this.m_levelOverride = value;
	}

	// Token: 0x06003AF5 RID: 15093 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISummoner.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002EEB RID: 12011
	private Bounds m_bounds;

	// Token: 0x04002EEC RID: 12012
	private Dictionary<GridPointManager, GlobalTeleporterController> m_globalTeleporters = new Dictionary<GridPointManager, GlobalTeleporterController>();

	// Token: 0x04002EED RID: 12013
	private Dictionary<Transform, List<Ferr2DT_PathTerrain>> m_standaloneRoomToTerrainTable = new Dictionary<Transform, List<Ferr2DT_PathTerrain>>();

	// Token: 0x04002EEE RID: 12014
	private int m_levelOverride = -1;
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public class MergeRoom : BaseRoom, ISummoner, ILevelConsumer
{
	// Token: 0x17001077 RID: 4215
	// (get) Token: 0x06002AAA RID: 10922 RVA: 0x000907DC File Offset: 0x0008E9DC
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

	// Token: 0x17001078 RID: 4216
	// (get) Token: 0x06002AAB RID: 10923 RVA: 0x0009084C File Offset: 0x0008EA4C
	public override BiomeArtData BiomeArtDataOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17001079 RID: 4217
	// (get) Token: 0x06002AAC RID: 10924 RVA: 0x0009084F File Offset: 0x0008EA4F
	public override Bounds Bounds
	{
		get
		{
			return this.m_bounds;
		}
	}

	// Token: 0x1700107A RID: 4218
	// (get) Token: 0x06002AAD RID: 10925 RVA: 0x00090857 File Offset: 0x0008EA57
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

	// Token: 0x1700107B RID: 4219
	// (get) Token: 0x06002AAE RID: 10926 RVA: 0x00090872 File Offset: 0x0008EA72
	// (set) Token: 0x06002AAF RID: 10927 RVA: 0x0009087A File Offset: 0x0008EA7A
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

	// Token: 0x1700107C RID: 4220
	// (get) Token: 0x06002AB0 RID: 10928 RVA: 0x00090883 File Offset: 0x0008EA83
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

	// Token: 0x1700107D RID: 4221
	// (get) Token: 0x06002AB1 RID: 10929 RVA: 0x0009089D File Offset: 0x0008EA9D
	public override RoomType RoomType
	{
		get
		{
			return RoomType.Standard;
		}
	}

	// Token: 0x1700107E RID: 4222
	// (get) Token: 0x06002AB2 RID: 10930 RVA: 0x000908A0 File Offset: 0x0008EAA0
	public override SpecialRoomType SpecialRoomType
	{
		get
		{
			return SpecialRoomType.None;
		}
	}

	// Token: 0x1700107F RID: 4223
	// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000908A3 File Offset: 0x0008EAA3
	// (set) Token: 0x06002AB4 RID: 10932 RVA: 0x000908AB File Offset: 0x0008EAAB
	public RoomEnemyManager[] StandaloneRoomEnemyManagers { get; private set; }

	// Token: 0x17001080 RID: 4224
	// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000908B4 File Offset: 0x0008EAB4
	// (set) Token: 0x06002AB6 RID: 10934 RVA: 0x000908BC File Offset: 0x0008EABC
	public Bounds[] StandaloneRoomBounds { get; private set; }

	// Token: 0x17001081 RID: 4225
	// (get) Token: 0x06002AB7 RID: 10935 RVA: 0x000908C5 File Offset: 0x0008EAC5
	// (set) Token: 0x06002AB8 RID: 10936 RVA: 0x000908CD File Offset: 0x0008EACD
	public GridPointManager[] StandaloneGridPointManagers { get; private set; }

	// Token: 0x17001082 RID: 4226
	// (get) Token: 0x06002AB9 RID: 10937 RVA: 0x000908D6 File Offset: 0x0008EAD6
	// (set) Token: 0x06002ABA RID: 10938 RVA: 0x000908DE File Offset: 0x0008EADE
	public int[] StandaloneRoomLevels { get; private set; }

	// Token: 0x17001083 RID: 4227
	// (get) Token: 0x06002ABB RID: 10939 RVA: 0x000908E7 File Offset: 0x0008EAE7
	// (set) Token: 0x06002ABC RID: 10940 RVA: 0x000908EF File Offset: 0x0008EAEF
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

	// Token: 0x17001084 RID: 4228
	// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000908F8 File Offset: 0x0008EAF8
	public override bool AllowItemDrops
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002ABE RID: 10942 RVA: 0x000908FB File Offset: 0x0008EAFB
	public void AddRooms(List<Room> rooms)
	{
		base.StartCoroutine(this.AddRoomsCoroutine(rooms));
	}

	// Token: 0x06002ABF RID: 10943 RVA: 0x0009090B File Offset: 0x0008EB0B
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

	// Token: 0x06002AC0 RID: 10944 RVA: 0x00090924 File Offset: 0x0008EB24
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

	// Token: 0x06002AC1 RID: 10945 RVA: 0x0009098C File Offset: 0x0008EB8C
	private void CreateBridgeBiomeChunkColliders(List<Room> rooms)
	{
		GameObject gameObject = new GameObject("Merge Room Chunks");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.AddComponent<MergeRoomChunkManager>().Initialize(this, rooms);
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x000909C8 File Offset: 0x0008EBC8
	protected override int GetDecoSeed()
	{
		int result = -1;
		if (this.StandaloneGridPointManagers != null && this.StandaloneGridPointManagers.Length != 0)
		{
			result = this.StandaloneGridPointManagers[0].DecoSeed;
		}
		return result;
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x000909F8 File Offset: 0x0008EBF8
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

	// Token: 0x06002AC4 RID: 10948 RVA: 0x00090AE8 File Offset: 0x0008ECE8
	protected override int GetSpecialPropSeed()
	{
		int result = -1;
		if (this.StandaloneGridPointManagers != null && this.StandaloneGridPointManagers.Length != 0)
		{
			result = this.StandaloneGridPointManagers[0].SpecialPropSeed;
		}
		return result;
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x00090B18 File Offset: 0x0008ED18
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

	// Token: 0x06002AC6 RID: 10950 RVA: 0x00090BB0 File Offset: 0x0008EDB0
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

	// Token: 0x06002AC7 RID: 10951 RVA: 0x00090D80 File Offset: 0x0008EF80
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

	// Token: 0x06002AC8 RID: 10952 RVA: 0x00090E40 File Offset: 0x0008F040
	public override void SetLevel(int value)
	{
		this.m_levelOverride = value;
	}

	// Token: 0x06002ACA RID: 10954 RVA: 0x00090E6E File Offset: 0x0008F06E
	GameObject ISummoner.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040022EB RID: 8939
	private Bounds m_bounds;

	// Token: 0x040022EC RID: 8940
	private Dictionary<GridPointManager, GlobalTeleporterController> m_globalTeleporters = new Dictionary<GridPointManager, GlobalTeleporterController>();

	// Token: 0x040022ED RID: 8941
	private Dictionary<Transform, List<Ferr2DT_PathTerrain>> m_standaloneRoomToTerrainTable = new Dictionary<Transform, List<Ferr2DT_PathTerrain>>();

	// Token: 0x040022EE RID: 8942
	private int m_levelOverride = -1;
}

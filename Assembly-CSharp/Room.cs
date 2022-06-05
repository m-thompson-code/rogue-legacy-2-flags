using System;
using System.Collections.Generic;
using System.Linq;
using Foreground;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004B3 RID: 1203
[ExecuteInEditMode]
public class Room : BaseRoom, ISummoner, ILevelConsumer
{
	// Token: 0x17001102 RID: 4354
	// (get) Token: 0x06002C74 RID: 11380 RVA: 0x00096A01 File Offset: 0x00094C01
	public IRelayLink<object, EventArgs> DoorClosedRelay
	{
		get
		{
			return this.m_doorClosedRelay.link;
		}
	}

	// Token: 0x17001103 RID: 4355
	// (get) Token: 0x06002C75 RID: 11381 RVA: 0x00096A0E File Offset: 0x00094C0E
	public IRelayLink<object, RoomConnectedEventArgs> RoomConnectedRelay
	{
		get
		{
			return this.m_roomConnectedRelay.link;
		}
	}

	// Token: 0x17001104 RID: 4356
	// (get) Token: 0x06002C76 RID: 11382 RVA: 0x00096A1B File Offset: 0x00094C1B
	public IRelayLink<object, EventArgs> RoomMergedRelay
	{
		get
		{
			return this.m_roomMergedRelay.link;
		}
	}

	// Token: 0x17001105 RID: 4357
	// (get) Token: 0x06002C77 RID: 11383 RVA: 0x00096A28 File Offset: 0x00094C28
	public override bool SpawnedAsEasyRoom
	{
		get
		{
			int num;
			return BiomeCreation_EV.FORCE_EASY_ROOMS_AT_BIOME_START.TryGetValue(base.BiomeType, out num) && BurdenManager.GetTotalBurdenLevel() == 0 && (this.RoomType == RoomType.Standard && this.SpecialRoomType == SpecialRoomType.None) && !this.GridPointManager.IsTunnelDestination && this.RoomNumber <= num + 1;
		}
	}

	// Token: 0x17001106 RID: 4358
	// (get) Token: 0x06002C78 RID: 11384 RVA: 0x00096A85 File Offset: 0x00094C85
	public int LevelOverride
	{
		get
		{
			return this.m_levelOverride;
		}
	}

	// Token: 0x17001107 RID: 4359
	// (get) Token: 0x06002C79 RID: 11385 RVA: 0x00096A8D File Offset: 0x00094C8D
	public override BiomeArtData BiomeArtDataOverride
	{
		get
		{
			return this.m_biomeArtDataOverride;
		}
	}

	// Token: 0x17001108 RID: 4360
	// (get) Token: 0x06002C7A RID: 11386 RVA: 0x00096A95 File Offset: 0x00094C95
	public BiomeType AppearanceOverride
	{
		get
		{
			return this.m_appearanceOverride;
		}
	}

	// Token: 0x17001109 RID: 4361
	// (get) Token: 0x06002C7B RID: 11387 RVA: 0x00096AA0 File Offset: 0x00094CA0
	public override Bounds Bounds
	{
		get
		{
			if (Application.isPlaying)
			{
				if (this.IsInitialised && this.m_bounds == default(Bounds))
				{
					this.m_bounds = new Bounds(base.transform.position, new Vector3((float)this.UnitDimensions.x, (float)this.UnitDimensions.y, 0f));
					float x = this.m_bounds.center.x - this.m_bounds.extents.x;
					float y = this.m_bounds.center.y - this.m_bounds.extents.y;
					base.BoundsRect = new Rect(new Vector2(x, y), this.m_bounds.size);
				}
				return this.m_bounds;
			}
			return new Bounds(base.transform.position, new Vector3((float)this.UnitDimensions.x, (float)this.UnitDimensions.y, 0f));
		}
	}

	// Token: 0x1700110A RID: 4362
	// (get) Token: 0x06002C7C RID: 11388 RVA: 0x00096BBE File Offset: 0x00094DBE
	// (set) Token: 0x06002C7D RID: 11389 RVA: 0x00096BC6 File Offset: 0x00094DC6
	public int RoomNumber
	{
		get
		{
			return this.m_roomNumber;
		}
		private set
		{
			this.m_roomNumber = value;
		}
	}

	// Token: 0x1700110B RID: 4363
	// (get) Token: 0x06002C7E RID: 11390 RVA: 0x00096BCF File Offset: 0x00094DCF
	public bool CanFlip
	{
		get
		{
			return this.m_canFlip;
		}
	}

	// Token: 0x1700110C RID: 4364
	// (get) Token: 0x06002C7F RID: 11391 RVA: 0x00096BD7 File Offset: 0x00094DD7
	// (set) Token: 0x06002C80 RID: 11392 RVA: 0x00096BDF File Offset: 0x00094DDF
	public bool CanMerge
	{
		get
		{
			return this.m_canMerge;
		}
		set
		{
			this.m_canMerge = value;
		}
	}

	// Token: 0x1700110D RID: 4365
	// (get) Token: 0x06002C81 RID: 11393 RVA: 0x00096BE8 File Offset: 0x00094DE8
	public Vector2 Coordinates
	{
		get
		{
			Vector2 vector = base.transform.position;
			return new Vector2(vector.x - 0.5f * (float)this.UnitDimensions.x, vector.y - 0.5f * (float)this.UnitDimensions.y);
		}
	}

	// Token: 0x1700110E RID: 4366
	// (get) Token: 0x06002C82 RID: 11394 RVA: 0x00096C43 File Offset: 0x00094E43
	public override bool DisableRoomLetterBoxing
	{
		get
		{
			return this.m_disableRoomLetteringBoxing;
		}
	}

	// Token: 0x1700110F RID: 4367
	// (get) Token: 0x06002C83 RID: 11395 RVA: 0x00096C4B File Offset: 0x00094E4B
	public override bool DisableRoomSaving
	{
		get
		{
			return this.m_disableRoomSaving;
		}
	}

	// Token: 0x17001110 RID: 4368
	// (get) Token: 0x06002C84 RID: 11396 RVA: 0x00096C53 File Offset: 0x00094E53
	public GlobalTeleporterController GlobalTeleporter
	{
		get
		{
			if (!this.m_hasSearchedForGlobalTeleporter && this.m_globalTeleporter == null)
			{
				this.m_hasSearchedForGlobalTeleporter = true;
				this.m_globalTeleporter = base.GetComponentInChildren<GlobalTeleporterController>();
			}
			return this.m_globalTeleporter;
		}
	}

	// Token: 0x17001111 RID: 4369
	// (get) Token: 0x06002C85 RID: 11397 RVA: 0x00096C84 File Offset: 0x00094E84
	// (set) Token: 0x06002C86 RID: 11398 RVA: 0x00096C8C File Offset: 0x00094E8C
	public GridPointManager GridPointManager { get; private set; }

	// Token: 0x17001112 RID: 4370
	// (get) Token: 0x06002C87 RID: 11399 RVA: 0x00096C95 File Offset: 0x00094E95
	// (set) Token: 0x06002C88 RID: 11400 RVA: 0x00096C9D File Offset: 0x00094E9D
	public bool IsDirty
	{
		get
		{
			return this.m_isDirty;
		}
		set
		{
			this.m_isDirty = value;
		}
	}

	// Token: 0x17001113 RID: 4371
	// (get) Token: 0x06002C89 RID: 11401 RVA: 0x00096CA6 File Offset: 0x00094EA6
	// (set) Token: 0x06002C8A RID: 11402 RVA: 0x00096CAE File Offset: 0x00094EAE
	public bool IsInitialised { get; private set; }

	// Token: 0x17001114 RID: 4372
	// (get) Token: 0x06002C8B RID: 11403 RVA: 0x00096CB7 File Offset: 0x00094EB7
	// (set) Token: 0x06002C8C RID: 11404 RVA: 0x00096CBF File Offset: 0x00094EBF
	public bool IsMirrored { get; private set; }

	// Token: 0x17001115 RID: 4373
	// (get) Token: 0x06002C8D RID: 11405 RVA: 0x00096CC8 File Offset: 0x00094EC8
	public bool IsStandardSize
	{
		get
		{
			return this.UnitDimensions.x % 32 == 0 && this.UnitDimensions.y % 18 == 0;
		}
	}

	// Token: 0x17001116 RID: 4374
	// (get) Token: 0x06002C8E RID: 11406 RVA: 0x00096CFE File Offset: 0x00094EFE
	public override int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x17001117 RID: 4375
	// (get) Token: 0x06002C8F RID: 11407 RVA: 0x00096D06 File Offset: 0x00094F06
	public PlayerRoomSpawn PlayerSpawn
	{
		get
		{
			if (this.m_spawn == null)
			{
				this.m_spawn = base.GetComponentInChildren<PlayerRoomSpawn>();
			}
			return this.m_spawn;
		}
	}

	// Token: 0x17001118 RID: 4376
	// (get) Token: 0x06002C90 RID: 11408 RVA: 0x00096D28 File Offset: 0x00094F28
	// (set) Token: 0x06002C91 RID: 11409 RVA: 0x00096D30 File Offset: 0x00094F30
	public RoomEnemyManager RoomEnemyManager { get; private set; }

	// Token: 0x17001119 RID: 4377
	// (get) Token: 0x06002C92 RID: 11410 RVA: 0x00096D39 File Offset: 0x00094F39
	// (set) Token: 0x06002C93 RID: 11411 RVA: 0x00096D41 File Offset: 0x00094F41
	public RoomID RoomID
	{
		get
		{
			return this.m_roomID;
		}
		protected set
		{
			this.m_roomID = value;
		}
	}

	// Token: 0x1700111A RID: 4378
	// (get) Token: 0x06002C94 RID: 11412 RVA: 0x00096D4C File Offset: 0x00094F4C
	public Vector2Int Size
	{
		get
		{
			int num = this.UnitDimensions.x / 32;
			int num2 = this.UnitDimensions.y / 18;
			if (this.UnitDimensions.x % 32 != 0)
			{
				num++;
			}
			if (this.UnitDimensions.y % 18 != 0)
			{
				num2++;
			}
			return new Vector2Int(num, num2);
		}
	}

	// Token: 0x1700111B RID: 4379
	// (get) Token: 0x06002C95 RID: 11413 RVA: 0x00096DB2 File Offset: 0x00094FB2
	public override SpecialRoomType SpecialRoomType
	{
		get
		{
			return this.GridPointManager.RoomMetaData.SpecialRoomType;
		}
	}

	// Token: 0x1700111C RID: 4380
	// (get) Token: 0x06002C96 RID: 11414 RVA: 0x00096DC4 File Offset: 0x00094FC4
	// (set) Token: 0x06002C97 RID: 11415 RVA: 0x00096DCC File Offset: 0x00094FCC
	public Vector2Int UnitDimensions
	{
		get
		{
			return this.m_unitDimensions;
		}
		private set
		{
			this.m_unitDimensions = value;
		}
	}

	// Token: 0x1700111D RID: 4381
	// (get) Token: 0x06002C98 RID: 11416 RVA: 0x00096DD5 File Offset: 0x00094FD5
	// (set) Token: 0x06002C99 RID: 11417 RVA: 0x00096E05 File Offset: 0x00095005
	public GameObject DoorsLocation
	{
		get
		{
			if (!this.m_doorsLocation)
			{
				this.m_doorsLocation = base.transform.Find("Doors").gameObject;
			}
			return this.m_doorsLocation;
		}
		set
		{
			value.transform.SetParent(base.transform);
			this.m_doorsLocation = value;
		}
	}

	// Token: 0x1700111E RID: 4382
	// (get) Token: 0x06002C9A RID: 11418 RVA: 0x00096E20 File Offset: 0x00095020
	// (set) Token: 0x06002C9B RID: 11419 RVA: 0x00096E7B File Offset: 0x0009507B
	public GameObject WallsLocation
	{
		get
		{
			if (!this.m_walls)
			{
				Transform transform = base.transform.FindDeep("Walls");
				if (!transform)
				{
					throw new NullReferenceException(string.Format("No Child called Walls found on Room ({0})", base.name));
				}
				this.m_walls = transform.gameObject;
			}
			return this.m_walls;
		}
		set
		{
			value.transform.SetParent(base.transform);
			this.m_walls = value;
		}
	}

	// Token: 0x1700111F RID: 4383
	// (get) Token: 0x06002C9C RID: 11420 RVA: 0x00096E98 File Offset: 0x00095098
	public Transform EnemyLocation
	{
		get
		{
			if (this.m_enemies == null)
			{
				this.m_enemies = base.transform.Find("Enemies");
				if (this.m_enemies == null && Application.isEditor)
				{
					GameObject gameObject = new GameObject("Enemies");
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					this.m_enemies = gameObject.transform;
				}
			}
			return this.m_enemies;
		}
	}

	// Token: 0x17001120 RID: 4384
	// (get) Token: 0x06002C9D RID: 11421 RVA: 0x00096F1C File Offset: 0x0009511C
	public Transform OneWayLocation
	{
		get
		{
			if (!Application.isPlaying)
			{
				if (this.m_oneWayLocation == null)
				{
					this.m_oneWayLocation = base.transform.Find("One Ways");
					if (this.m_oneWayLocation == null)
					{
						GameObject gameObject = new GameObject("One Ways");
						gameObject.transform.SetParent(base.transform);
						gameObject.transform.localPosition = Vector3.zero;
						this.m_oneWayLocation = gameObject.transform;
					}
				}
				return this.m_oneWayLocation;
			}
			return base.transform;
		}
	}

	// Token: 0x17001121 RID: 4385
	// (get) Token: 0x06002C9E RID: 11422 RVA: 0x00096FA7 File Offset: 0x000951A7
	public override bool AllowItemDrops
	{
		get
		{
			return this.m_allowItemDrops;
		}
	}

	// Token: 0x17001122 RID: 4386
	// (get) Token: 0x06002C9F RID: 11423 RVA: 0x00096FAF File Offset: 0x000951AF
	public bool IsEasy
	{
		get
		{
			return this.m_isEasy;
		}
	}

	// Token: 0x17001123 RID: 4387
	// (get) Token: 0x06002CA0 RID: 11424 RVA: 0x00096FB7 File Offset: 0x000951B7
	public bool DisableOneWaysAtBottomDoors
	{
		get
		{
			return this.m_disableOneWaysAtBottomDoors;
		}
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x00096FC0 File Offset: 0x000951C0
	private void Start()
	{
		if (Application.isPlaying)
		{
			foreach (Door door in this.Doors)
			{
				this.AddDoorEventHandlers(door);
			}
		}
	}

	// Token: 0x06002CA2 RID: 11426 RVA: 0x0009701C File Offset: 0x0009521C
	protected override void OnDestroy()
	{
		if (Application.isPlaying && this.Doors != null)
		{
			for (int i = this.Doors.Count - 1; i >= 0; i--)
			{
				this.RemoveDoor(this.Doors[i]);
			}
		}
		base.OnDestroy();
	}

	// Token: 0x06002CA3 RID: 11427 RVA: 0x00097068 File Offset: 0x00095268
	private void OnDrawGizmos()
	{
	}

	// Token: 0x06002CA4 RID: 11428 RVA: 0x0009706C File Offset: 0x0009526C
	public void Initialise(BiomeType biome, RoomType roomType, int roomNumber = -1)
	{
		if (this.GridPointManager != null && this.GridPointManager.AppearanceOverride != BiomeType.None)
		{
			biome = this.GridPointManager.AppearanceOverride;
		}
		base.AppearanceBiomeType = biome;
		this.RoomType = roomType;
		this.RoomNumber = roomNumber;
		this.CalculateRoomLevel();
		this.UpdateVisualAppearance();
		ForegroundController component = base.GetComponent<ForegroundController>();
		if (component && !component.IsInitialized)
		{
			component.Initialize();
		}
		if (base.SpawnControllerManager == null)
		{
			base.SetSpawnControllerManager(new RoomSpawnControllerManager(this));
		}
		if (base.TerrainManager == null)
		{
			base.SetTerrainManager(new RoomTerrainManager(this));
		}
		if (base.SaveController == null)
		{
			base.SaveController = new RoomSaveController(this);
		}
		this.m_roomInitializedRelay.Dispatch(this, EventArgs.Empty);
		this.RoomEnemyManager = base.GetComponent<RoomEnemyManager>();
		if (this.RoomEnemyManager == null)
		{
			Debug.LogFormat("<color=red>| {0} | Room ({1}) is missing a RoomEnemyManager component. In the meantime one will be added, but if you see this message please add a task to pivotal to add one.</color>", new object[]
			{
				this,
				base.name
			});
			this.RoomEnemyManager = base.gameObject.AddComponent<RoomEnemyManager>();
		}
		base.InjectRoom();
		this.IsInitialised = true;
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x0009717B File Offset: 0x0009537B
	public void Initialise(GridPointManager gridPointManager)
	{
		this.GridPointManager = gridPointManager;
		this.Initialise(gridPointManager.Biome, gridPointManager.RoomType, gridPointManager.RoomNumber);
	}

	// Token: 0x06002CA6 RID: 11430 RVA: 0x0009719C File Offset: 0x0009539C
	private void AddConnectedRoom(BaseRoom room)
	{
		if (room == null)
		{
			throw new ArgumentNullException("room", string.Format("[{0}] Argument can't be null", this));
		}
		if (!base.ConnectedRooms.Contains(room))
		{
			base.ConnectedRooms.Add(room);
			this.DispatchRoomConnectedRelay(room);
		}
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x000971EC File Offset: 0x000953EC
	private void ApplyBiomeArtData(BiomeArtData biomeArtData)
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component)
		{
			component.enabled = false;
		}
		Ferr2DBiomeArtData ferr2DBiomeArtData = biomeArtData.Ferr2DBiomeArtData;
		Ferr2DSettings settings = null;
		if (ferr2DBiomeArtData != null && ferr2DBiomeArtData.TerrainMaster != null)
		{
			settings = ferr2DBiomeArtData.TerrainMaster.Ferr2DSettings;
		}
		if (base.TerrainManager == null)
		{
			base.TerrainManager = new RoomTerrainManager(this);
		}
		this.UpdateFerr2DSetting(base.TerrainManager.Platforms, settings);
		Ferr2DSettings settings2 = null;
		if (ferr2DBiomeArtData != null && ferr2DBiomeArtData.OneWayMaster != null)
		{
			settings2 = ferr2DBiomeArtData.OneWayMaster.Ferr2DSettings;
		}
		this.UpdateFerr2DSetting(base.TerrainManager.OneWays, settings2);
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x00097280 File Offset: 0x00095480
	public void CalculateRoomLevel()
	{
		int roomNumber = this.RoomNumber;
		if (base.SpawnControllerManager != null)
		{
			foreach (TunnelSpawnController tunnelSpawnController in base.SpawnControllerManager.TunnelSpawnControllers)
			{
				if (tunnelSpawnController && tunnelSpawnController.Tunnel != null && tunnelSpawnController.Tunnel.Root != null)
				{
					roomNumber = tunnelSpawnController.Tunnel.Root.Room.BiomeControllerIndex;
					break;
				}
			}
		}
		int num;
		if (this.RoomType != RoomType.Boss)
		{
			num = RoomUtility.GetRoomLevel(base.BiomeType, roomNumber);
		}
		else
		{
			num = BiomeDataLibrary.GetData(base.BiomeType).BossStartLevel * Mathf.RoundToInt(1f + BurdenManager.GetBurdenStatGain(BurdenType.RoomCount));
		}
		int num2 = SaveManager.PlayerSaveData.NewGamePlusLevel * 44;
		num += num2;
		if (this.m_levelOverride > -1)
		{
			num = this.m_levelOverride;
		}
		this.SetLevel(num);
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x0009736C File Offset: 0x0009556C
	public void InjectLevel()
	{
		foreach (ILevelConsumer levelConsumer in base.GetComponentsInChildren<ILevelConsumer>())
		{
			if (levelConsumer != this)
			{
				levelConsumer.SetLevel(this.Level);
			}
		}
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x000973A2 File Offset: 0x000955A2
	public void MoveToCoordinates(Vector2 coords)
	{
		base.transform.position = GridController.GetWorldPositionFromRoomCoordinates(coords, this.Size);
	}

	// Token: 0x06002CAB RID: 11435 RVA: 0x000973C0 File Offset: 0x000955C0
	public void OnMerge()
	{
		this.m_roomMergedRelay.Dispatch(this, EventArgs.Empty);
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x000973D4 File Offset: 0x000955D4
	private void UpdateVisualAppearance()
	{
		BiomeType terrainBiomeType = ArtUtility.GetTerrainBiomeType(this);
		BiomeArtData biomeArtData = this.BiomeArtDataOverride;
		if (!biomeArtData)
		{
			biomeArtData = BiomeArtDataLibrary.GetArtData(terrainBiomeType);
		}
		this.ApplyBiomeArtData(biomeArtData);
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x00097408 File Offset: 0x00095608
	private void UpdateFerr2DSetting(IEnumerable<Ferr2DT_PathTerrain> ferr2DObjects, Ferr2DSettings settings)
	{
		if (!settings.Master)
		{
			return;
		}
		QualitySettings.GetQualityLevel();
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in ferr2DObjects)
		{
			if (!ferr2DT_PathTerrain.OverrideDisabledMergeMaterial)
			{
				ferr2DT_PathTerrain.SetMaterial(settings.Material);
			}
			ferr2DT_PathTerrain.fillSplit = false;
			ferr2DT_PathTerrain.fillSplitDistance = settings.GridSpacing;
			ferr2DT_PathTerrain.vertexColorType = settings.ColorType;
			ferr2DT_PathTerrain.vertexGradientDistance = settings.VertexGradientDistance;
			ferr2DT_PathTerrain.vertexGradient = settings.VertexGradient;
			ferr2DT_PathTerrain.pixelsPerUnit = settings.PixelsPerUnit;
			ferr2DT_PathTerrain.createTangents = settings.CreateTangents;
			ferr2DT_PathTerrain.fillZ = settings.FillZ;
			Renderer component = ferr2DT_PathTerrain.GetComponent<Renderer>();
			component.sortingLayerID = settings.SortingLayer;
			component.sortingOrder = settings.OrderInLayer;
		}
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x000974F0 File Offset: 0x000956F0
	private void AddDoorEventHandlers(Door door)
	{
		door.DoorDestroyedRelay.AddListener(new Action<object, EventArgs>(this.OnDoorDestroyed), false);
		door.PlayerEnterRelay.AddListener(new Action<object, DoorEventArgs>(base.OnPlayerExitViaDoor), false);
		door.DoorConnectRelay.AddListener(new Action<object, DoorConnectEventArgs>(this.OnDoorConnect), false);
		door.CloseRelay.AddListener(new Action<object, DoorEventArgs>(this.OnDoorClose), false);
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x00097564 File Offset: 0x00095764
	private void RemoveDoor(Door door)
	{
		this.Doors.Remove(door);
		door.DoorDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnDoorDestroyed));
		door.PlayerEnterRelay.RemoveListener(new Action<object, DoorEventArgs>(base.OnPlayerExitViaDoor));
		door.DoorConnectRelay.RemoveListener(new Action<object, DoorConnectEventArgs>(this.OnDoorConnect));
		door.CloseRelay.RemoveListener(new Action<object, DoorEventArgs>(this.OnDoorClose));
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x000975DE File Offset: 0x000957DE
	protected virtual void DispatchRoomConnectedRelay(BaseRoom room)
	{
		if (this.m_roomConnectedEventArgs == null)
		{
			this.m_roomConnectedEventArgs = new RoomConnectedEventArgs(this, room);
		}
		else
		{
			this.m_roomConnectedEventArgs.Initialize(this, room);
		}
		this.m_roomConnectedRelay.Dispatch(this, this.m_roomConnectedEventArgs);
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x00097618 File Offset: 0x00095818
	private void OnDoorClose(object sender, DoorEventArgs eventArgs)
	{
		Door door = eventArgs.Door;
		this.RemoveDoor(door);
		this.m_doorClosedRelay.Dispatch(this, EventArgs.Empty);
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x00097644 File Offset: 0x00095844
	private void OnDoorConnect(object sender, DoorConnectEventArgs eventArgs)
	{
		if (!(eventArgs.DoorB != null))
		{
			Debug.LogFormat("[{0}]: DoorB argument is null", new object[]
			{
				this
			});
			return;
		}
		if (eventArgs.DoorB.Room != null)
		{
			this.AddConnectedRoom(eventArgs.DoorB.Room);
			return;
		}
		Debug.LogFormat("<color=red>[{0}]: Door ({1}) is connected to a Door whose Room Property is null. Did you forget to Initialise that Door's Room?</color>", new object[]
		{
			this,
			eventArgs.DoorA.Side.ToString() + "-" + eventArgs.DoorA.Number.ToString()
		});
	}

	// Token: 0x06002CB3 RID: 11443 RVA: 0x000976E8 File Offset: 0x000958E8
	private void OnDoorDestroyed(object sender, EventArgs eventArgs)
	{
		Door door = sender as Door;
		this.RemoveDoor(door);
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x00097704 File Offset: 0x00095904
	protected override int GetDecoSeed()
	{
		int result = -1;
		if (this.GridPointManager != null)
		{
			result = this.GridPointManager.DecoSeed;
		}
		return result;
	}

	// Token: 0x06002CB5 RID: 11445 RVA: 0x00097728 File Offset: 0x00095928
	protected override int GetSpecialPropSeed()
	{
		int result = -1;
		if (this.GridPointManager != null)
		{
			result = this.GridPointManager.SpecialPropSeed;
		}
		return result;
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x0009774C File Offset: 0x0009594C
	public Door GetDoor(RoomSide side, int doorNumber)
	{
		Door result = null;
		List<Door> doorsOnSide = this.GetDoorsOnSide(side);
		if (doorsOnSide.Count > 0)
		{
			for (int i = 0; i < doorsOnSide.Count; i++)
			{
				if (doorsOnSide[i].Number == doorNumber)
				{
					result = doorsOnSide[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x00097798 File Offset: 0x00095998
	public List<Door> GetDoorsOnSide(RoomSide side)
	{
		if (side != RoomSide.Any)
		{
			Room.m_doorListHelper_STATIC.Clear();
			List<Door> doors = this.Doors;
			int count = doors.Count;
			for (int i = 0; i < count; i++)
			{
				Door door = doors[i];
				if (door.Side == side)
				{
					Room.m_doorListHelper_STATIC.Add(door);
				}
			}
			return Room.m_doorListHelper_STATIC;
		}
		return this.Doors;
	}

	// Token: 0x06002CB8 RID: 11448 RVA: 0x000977F5 File Offset: 0x000959F5
	public List<Door> GetDoorsOnSideCopy(RoomSide side)
	{
		return this.GetDoorsOnSide(side).ToList<Door>();
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x00097803 File Offset: 0x00095A03
	public void SetCanMerge(bool canMerge)
	{
		if (this.CanMerge != canMerge)
		{
			this.CanMerge = canMerge;
		}
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x00097815 File Offset: 0x00095A15
	public void SetIsMirrored(bool isMirrored)
	{
		this.IsMirrored = isMirrored;
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x0009781E File Offset: 0x00095A1E
	public override void SetLevel(int value)
	{
		this.m_level = value;
		this.InjectLevel();
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x0009782D File Offset: 0x00095A2D
	public void SetUnitDimensions(int unitWidth, int unitHeight)
	{
		this.UnitDimensions = new Vector2Int(unitWidth, unitHeight);
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x0009783C File Offset: 0x00095A3C
	public void ForceBiomeArtDataOverride(BiomeArtData biomeArtDataOverride)
	{
		this.m_biomeArtDataOverride = biomeArtDataOverride;
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x00097845 File Offset: 0x00095A45
	public void ResetBounds()
	{
		this.m_bounds = default(Bounds);
	}

	// Token: 0x06002CC1 RID: 11457 RVA: 0x000978D4 File Offset: 0x00095AD4
	GameObject ISummoner.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040023F2 RID: 9202
	[SerializeField]
	private bool m_canMerge = true;

	// Token: 0x040023F3 RID: 9203
	[SerializeField]
	private bool m_canFlip = true;

	// Token: 0x040023F4 RID: 9204
	[SerializeField]
	private bool m_disableRoomSaving;

	// Token: 0x040023F5 RID: 9205
	[SerializeField]
	private bool m_disableRoomLetteringBoxing;

	// Token: 0x040023F6 RID: 9206
	[SerializeField]
	private bool m_disableOneWaysAtBottomDoors;

	// Token: 0x040023F7 RID: 9207
	[SerializeField]
	private BiomeType m_appearanceOverride;

	// Token: 0x040023F8 RID: 9208
	[SerializeField]
	private BiomeArtData m_biomeArtDataOverride;

	// Token: 0x040023F9 RID: 9209
	[SerializeField]
	private int m_levelOverride = -1;

	// Token: 0x040023FA RID: 9210
	[SerializeField]
	private bool m_allowItemDrops = true;

	// Token: 0x040023FB RID: 9211
	[SerializeField]
	private bool m_isEasy;

	// Token: 0x040023FC RID: 9212
	[SerializeField]
	[HideInInspector]
	private Vector2Int m_unitDimensions = new Vector2Int(32, 18);

	// Token: 0x040023FD RID: 9213
	[SerializeField]
	[HideInInspector]
	private RoomID m_roomID;

	// Token: 0x040023FE RID: 9214
	[SerializeField]
	[HideInInspector]
	private bool m_isDirty = true;

	// Token: 0x040023FF RID: 9215
	private Bounds m_bounds;

	// Token: 0x04002400 RID: 9216
	private bool m_hasSearchedForGlobalTeleporter;

	// Token: 0x04002401 RID: 9217
	private GlobalTeleporterController m_globalTeleporter;

	// Token: 0x04002402 RID: 9218
	private PlayerRoomSpawn m_spawn;

	// Token: 0x04002403 RID: 9219
	private int m_roomNumber = -1;

	// Token: 0x04002404 RID: 9220
	protected GameObject m_doorsLocation;

	// Token: 0x04002405 RID: 9221
	protected GameObject m_walls;

	// Token: 0x04002406 RID: 9222
	protected Transform m_enemies;

	// Token: 0x04002407 RID: 9223
	private int m_level = -1;

	// Token: 0x04002408 RID: 9224
	protected Transform m_oneWayLocation;

	// Token: 0x04002409 RID: 9225
	private RoomConnectedEventArgs m_roomConnectedEventArgs;

	// Token: 0x0400240A RID: 9226
	protected Relay<object, EventArgs> m_doorClosedRelay = new Relay<object, EventArgs>();

	// Token: 0x0400240B RID: 9227
	protected Relay<object, RoomConnectedEventArgs> m_roomConnectedRelay = new Relay<object, RoomConnectedEventArgs>();

	// Token: 0x0400240C RID: 9228
	protected Relay<object, EventArgs> m_roomMergedRelay = new Relay<object, EventArgs>();

	// Token: 0x04002411 RID: 9233
	private static List<Door> m_doorListHelper_STATIC = new List<Door>();
}

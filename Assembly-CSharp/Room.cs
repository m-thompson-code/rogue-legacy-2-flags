using System;
using System.Collections.Generic;
using System.Linq;
using Foreground;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020007CA RID: 1994
[ExecuteInEditMode]
public class Room : BaseRoom, ISummoner, ILevelConsumer
{
	// Token: 0x17001669 RID: 5737
	// (get) Token: 0x06003D1D RID: 15645 RVA: 0x00021CBF File Offset: 0x0001FEBF
	public IRelayLink<object, EventArgs> DoorClosedRelay
	{
		get
		{
			return this.m_doorClosedRelay.link;
		}
	}

	// Token: 0x1700166A RID: 5738
	// (get) Token: 0x06003D1E RID: 15646 RVA: 0x00021CCC File Offset: 0x0001FECC
	public IRelayLink<object, RoomConnectedEventArgs> RoomConnectedRelay
	{
		get
		{
			return this.m_roomConnectedRelay.link;
		}
	}

	// Token: 0x1700166B RID: 5739
	// (get) Token: 0x06003D1F RID: 15647 RVA: 0x00021CD9 File Offset: 0x0001FED9
	public IRelayLink<object, EventArgs> RoomMergedRelay
	{
		get
		{
			return this.m_roomMergedRelay.link;
		}
	}

	// Token: 0x1700166C RID: 5740
	// (get) Token: 0x06003D20 RID: 15648 RVA: 0x000F7E8C File Offset: 0x000F608C
	public override bool SpawnedAsEasyRoom
	{
		get
		{
			int num;
			return BiomeCreation_EV.FORCE_EASY_ROOMS_AT_BIOME_START.TryGetValue(base.BiomeType, out num) && BurdenManager.GetTotalBurdenLevel() == 0 && (this.RoomType == RoomType.Standard && this.SpecialRoomType == SpecialRoomType.None) && !this.GridPointManager.IsTunnelDestination && this.RoomNumber <= num + 1;
		}
	}

	// Token: 0x1700166D RID: 5741
	// (get) Token: 0x06003D21 RID: 15649 RVA: 0x00021CE6 File Offset: 0x0001FEE6
	public int LevelOverride
	{
		get
		{
			return this.m_levelOverride;
		}
	}

	// Token: 0x1700166E RID: 5742
	// (get) Token: 0x06003D22 RID: 15650 RVA: 0x00021CEE File Offset: 0x0001FEEE
	public override BiomeArtData BiomeArtDataOverride
	{
		get
		{
			return this.m_biomeArtDataOverride;
		}
	}

	// Token: 0x1700166F RID: 5743
	// (get) Token: 0x06003D23 RID: 15651 RVA: 0x00021CF6 File Offset: 0x0001FEF6
	public BiomeType AppearanceOverride
	{
		get
		{
			return this.m_appearanceOverride;
		}
	}

	// Token: 0x17001670 RID: 5744
	// (get) Token: 0x06003D24 RID: 15652 RVA: 0x000F7EEC File Offset: 0x000F60EC
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

	// Token: 0x17001671 RID: 5745
	// (get) Token: 0x06003D25 RID: 15653 RVA: 0x00021CFE File Offset: 0x0001FEFE
	// (set) Token: 0x06003D26 RID: 15654 RVA: 0x00021D06 File Offset: 0x0001FF06
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

	// Token: 0x17001672 RID: 5746
	// (get) Token: 0x06003D27 RID: 15655 RVA: 0x00021D0F File Offset: 0x0001FF0F
	public bool CanFlip
	{
		get
		{
			return this.m_canFlip;
		}
	}

	// Token: 0x17001673 RID: 5747
	// (get) Token: 0x06003D28 RID: 15656 RVA: 0x00021D17 File Offset: 0x0001FF17
	// (set) Token: 0x06003D29 RID: 15657 RVA: 0x00021D1F File Offset: 0x0001FF1F
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

	// Token: 0x17001674 RID: 5748
	// (get) Token: 0x06003D2A RID: 15658 RVA: 0x000F800C File Offset: 0x000F620C
	public Vector2 Coordinates
	{
		get
		{
			Vector2 vector = base.transform.position;
			return new Vector2(vector.x - 0.5f * (float)this.UnitDimensions.x, vector.y - 0.5f * (float)this.UnitDimensions.y);
		}
	}

	// Token: 0x17001675 RID: 5749
	// (get) Token: 0x06003D2B RID: 15659 RVA: 0x00021D28 File Offset: 0x0001FF28
	public override bool DisableRoomLetterBoxing
	{
		get
		{
			return this.m_disableRoomLetteringBoxing;
		}
	}

	// Token: 0x17001676 RID: 5750
	// (get) Token: 0x06003D2C RID: 15660 RVA: 0x00021D30 File Offset: 0x0001FF30
	public override bool DisableRoomSaving
	{
		get
		{
			return this.m_disableRoomSaving;
		}
	}

	// Token: 0x17001677 RID: 5751
	// (get) Token: 0x06003D2D RID: 15661 RVA: 0x00021D38 File Offset: 0x0001FF38
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

	// Token: 0x17001678 RID: 5752
	// (get) Token: 0x06003D2E RID: 15662 RVA: 0x00021D69 File Offset: 0x0001FF69
	// (set) Token: 0x06003D2F RID: 15663 RVA: 0x00021D71 File Offset: 0x0001FF71
	public GridPointManager GridPointManager { get; private set; }

	// Token: 0x17001679 RID: 5753
	// (get) Token: 0x06003D30 RID: 15664 RVA: 0x00021D7A File Offset: 0x0001FF7A
	// (set) Token: 0x06003D31 RID: 15665 RVA: 0x00021D82 File Offset: 0x0001FF82
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

	// Token: 0x1700167A RID: 5754
	// (get) Token: 0x06003D32 RID: 15666 RVA: 0x00021D8B File Offset: 0x0001FF8B
	// (set) Token: 0x06003D33 RID: 15667 RVA: 0x00021D93 File Offset: 0x0001FF93
	public bool IsInitialised { get; private set; }

	// Token: 0x1700167B RID: 5755
	// (get) Token: 0x06003D34 RID: 15668 RVA: 0x00021D9C File Offset: 0x0001FF9C
	// (set) Token: 0x06003D35 RID: 15669 RVA: 0x00021DA4 File Offset: 0x0001FFA4
	public bool IsMirrored { get; private set; }

	// Token: 0x1700167C RID: 5756
	// (get) Token: 0x06003D36 RID: 15670 RVA: 0x000F8068 File Offset: 0x000F6268
	public bool IsStandardSize
	{
		get
		{
			return this.UnitDimensions.x % 32 == 0 && this.UnitDimensions.y % 18 == 0;
		}
	}

	// Token: 0x1700167D RID: 5757
	// (get) Token: 0x06003D37 RID: 15671 RVA: 0x00021DAD File Offset: 0x0001FFAD
	public override int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x1700167E RID: 5758
	// (get) Token: 0x06003D38 RID: 15672 RVA: 0x00021DB5 File Offset: 0x0001FFB5
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

	// Token: 0x1700167F RID: 5759
	// (get) Token: 0x06003D39 RID: 15673 RVA: 0x00021DD7 File Offset: 0x0001FFD7
	// (set) Token: 0x06003D3A RID: 15674 RVA: 0x00021DDF File Offset: 0x0001FFDF
	public RoomEnemyManager RoomEnemyManager { get; private set; }

	// Token: 0x17001680 RID: 5760
	// (get) Token: 0x06003D3B RID: 15675 RVA: 0x00021DE8 File Offset: 0x0001FFE8
	// (set) Token: 0x06003D3C RID: 15676 RVA: 0x00021DF0 File Offset: 0x0001FFF0
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

	// Token: 0x17001681 RID: 5761
	// (get) Token: 0x06003D3D RID: 15677 RVA: 0x000F80A0 File Offset: 0x000F62A0
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

	// Token: 0x17001682 RID: 5762
	// (get) Token: 0x06003D3E RID: 15678 RVA: 0x00021DF9 File Offset: 0x0001FFF9
	public override SpecialRoomType SpecialRoomType
	{
		get
		{
			return this.GridPointManager.RoomMetaData.SpecialRoomType;
		}
	}

	// Token: 0x17001683 RID: 5763
	// (get) Token: 0x06003D3F RID: 15679 RVA: 0x00021E0B File Offset: 0x0002000B
	// (set) Token: 0x06003D40 RID: 15680 RVA: 0x00021E13 File Offset: 0x00020013
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

	// Token: 0x17001684 RID: 5764
	// (get) Token: 0x06003D41 RID: 15681 RVA: 0x00021E1C File Offset: 0x0002001C
	// (set) Token: 0x06003D42 RID: 15682 RVA: 0x00021E4C File Offset: 0x0002004C
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

	// Token: 0x17001685 RID: 5765
	// (get) Token: 0x06003D43 RID: 15683 RVA: 0x000F8108 File Offset: 0x000F6308
	// (set) Token: 0x06003D44 RID: 15684 RVA: 0x00021E66 File Offset: 0x00020066
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

	// Token: 0x17001686 RID: 5766
	// (get) Token: 0x06003D45 RID: 15685 RVA: 0x000F8164 File Offset: 0x000F6364
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

	// Token: 0x17001687 RID: 5767
	// (get) Token: 0x06003D46 RID: 15686 RVA: 0x000F81E8 File Offset: 0x000F63E8
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

	// Token: 0x17001688 RID: 5768
	// (get) Token: 0x06003D47 RID: 15687 RVA: 0x00021E80 File Offset: 0x00020080
	public override bool AllowItemDrops
	{
		get
		{
			return this.m_allowItemDrops;
		}
	}

	// Token: 0x17001689 RID: 5769
	// (get) Token: 0x06003D48 RID: 15688 RVA: 0x00021E88 File Offset: 0x00020088
	public bool IsEasy
	{
		get
		{
			return this.m_isEasy;
		}
	}

	// Token: 0x1700168A RID: 5770
	// (get) Token: 0x06003D49 RID: 15689 RVA: 0x00021E90 File Offset: 0x00020090
	public bool DisableOneWaysAtBottomDoors
	{
		get
		{
			return this.m_disableOneWaysAtBottomDoors;
		}
	}

	// Token: 0x06003D4A RID: 15690 RVA: 0x000F8274 File Offset: 0x000F6474
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

	// Token: 0x06003D4B RID: 15691 RVA: 0x000F82D0 File Offset: 0x000F64D0
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

	// Token: 0x06003D4C RID: 15692 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnDrawGizmos()
	{
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x000F831C File Offset: 0x000F651C
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

	// Token: 0x06003D4E RID: 15694 RVA: 0x00021E98 File Offset: 0x00020098
	public void Initialise(GridPointManager gridPointManager)
	{
		this.GridPointManager = gridPointManager;
		this.Initialise(gridPointManager.Biome, gridPointManager.RoomType, gridPointManager.RoomNumber);
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x000F842C File Offset: 0x000F662C
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

	// Token: 0x06003D50 RID: 15696 RVA: 0x000F847C File Offset: 0x000F667C
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

	// Token: 0x06003D51 RID: 15697 RVA: 0x000F8510 File Offset: 0x000F6710
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

	// Token: 0x06003D52 RID: 15698 RVA: 0x000F85FC File Offset: 0x000F67FC
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

	// Token: 0x06003D53 RID: 15699 RVA: 0x00021EB9 File Offset: 0x000200B9
	public void MoveToCoordinates(Vector2 coords)
	{
		base.transform.position = GridController.GetWorldPositionFromRoomCoordinates(coords, this.Size);
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x00021ED7 File Offset: 0x000200D7
	public void OnMerge()
	{
		this.m_roomMergedRelay.Dispatch(this, EventArgs.Empty);
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x000F8634 File Offset: 0x000F6834
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

	// Token: 0x06003D56 RID: 15702 RVA: 0x000F8668 File Offset: 0x000F6868
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

	// Token: 0x06003D57 RID: 15703 RVA: 0x000F8750 File Offset: 0x000F6950
	private void AddDoorEventHandlers(Door door)
	{
		door.DoorDestroyedRelay.AddListener(new Action<object, EventArgs>(this.OnDoorDestroyed), false);
		door.PlayerEnterRelay.AddListener(new Action<object, DoorEventArgs>(base.OnPlayerExitViaDoor), false);
		door.DoorConnectRelay.AddListener(new Action<object, DoorConnectEventArgs>(this.OnDoorConnect), false);
		door.CloseRelay.AddListener(new Action<object, DoorEventArgs>(this.OnDoorClose), false);
	}

	// Token: 0x06003D58 RID: 15704 RVA: 0x000F87C4 File Offset: 0x000F69C4
	private void RemoveDoor(Door door)
	{
		this.Doors.Remove(door);
		door.DoorDestroyedRelay.RemoveListener(new Action<object, EventArgs>(this.OnDoorDestroyed));
		door.PlayerEnterRelay.RemoveListener(new Action<object, DoorEventArgs>(base.OnPlayerExitViaDoor));
		door.DoorConnectRelay.RemoveListener(new Action<object, DoorConnectEventArgs>(this.OnDoorConnect));
		door.CloseRelay.RemoveListener(new Action<object, DoorEventArgs>(this.OnDoorClose));
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x00021EEA File Offset: 0x000200EA
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

	// Token: 0x06003D5A RID: 15706 RVA: 0x000F8840 File Offset: 0x000F6A40
	private void OnDoorClose(object sender, DoorEventArgs eventArgs)
	{
		Door door = eventArgs.Door;
		this.RemoveDoor(door);
		this.m_doorClosedRelay.Dispatch(this, EventArgs.Empty);
	}

	// Token: 0x06003D5B RID: 15707 RVA: 0x000F886C File Offset: 0x000F6A6C
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

	// Token: 0x06003D5C RID: 15708 RVA: 0x000F8910 File Offset: 0x000F6B10
	private void OnDoorDestroyed(object sender, EventArgs eventArgs)
	{
		Door door = sender as Door;
		this.RemoveDoor(door);
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x000F892C File Offset: 0x000F6B2C
	protected override int GetDecoSeed()
	{
		int result = -1;
		if (this.GridPointManager != null)
		{
			result = this.GridPointManager.DecoSeed;
		}
		return result;
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x000F8950 File Offset: 0x000F6B50
	protected override int GetSpecialPropSeed()
	{
		int result = -1;
		if (this.GridPointManager != null)
		{
			result = this.GridPointManager.SpecialPropSeed;
		}
		return result;
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x000F8974 File Offset: 0x000F6B74
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

	// Token: 0x06003D60 RID: 15712 RVA: 0x000F89C0 File Offset: 0x000F6BC0
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

	// Token: 0x06003D61 RID: 15713 RVA: 0x00021F22 File Offset: 0x00020122
	public List<Door> GetDoorsOnSideCopy(RoomSide side)
	{
		return this.GetDoorsOnSide(side).ToList<Door>();
	}

	// Token: 0x06003D62 RID: 15714 RVA: 0x00021F30 File Offset: 0x00020130
	public void SetCanMerge(bool canMerge)
	{
		if (this.CanMerge != canMerge)
		{
			this.CanMerge = canMerge;
		}
	}

	// Token: 0x06003D63 RID: 15715 RVA: 0x00021F42 File Offset: 0x00020142
	public void SetIsMirrored(bool isMirrored)
	{
		this.IsMirrored = isMirrored;
	}

	// Token: 0x06003D64 RID: 15716 RVA: 0x00021F4B File Offset: 0x0002014B
	public override void SetLevel(int value)
	{
		this.m_level = value;
		this.InjectLevel();
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x00021F5A File Offset: 0x0002015A
	public void SetUnitDimensions(int unitWidth, int unitHeight)
	{
		this.UnitDimensions = new Vector2Int(unitWidth, unitHeight);
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x00021F69 File Offset: 0x00020169
	public void ForceBiomeArtDataOverride(BiomeArtData biomeArtDataOverride)
	{
		this.m_biomeArtDataOverride = biomeArtDataOverride;
	}

	// Token: 0x06003D67 RID: 15719 RVA: 0x00021F72 File Offset: 0x00020172
	public void ResetBounds()
	{
		this.m_bounds = default(Bounds);
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISummoner.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003053 RID: 12371
	[SerializeField]
	private bool m_canMerge = true;

	// Token: 0x04003054 RID: 12372
	[SerializeField]
	private bool m_canFlip = true;

	// Token: 0x04003055 RID: 12373
	[SerializeField]
	private bool m_disableRoomSaving;

	// Token: 0x04003056 RID: 12374
	[SerializeField]
	private bool m_disableRoomLetteringBoxing;

	// Token: 0x04003057 RID: 12375
	[SerializeField]
	private bool m_disableOneWaysAtBottomDoors;

	// Token: 0x04003058 RID: 12376
	[SerializeField]
	private BiomeType m_appearanceOverride;

	// Token: 0x04003059 RID: 12377
	[SerializeField]
	private BiomeArtData m_biomeArtDataOverride;

	// Token: 0x0400305A RID: 12378
	[SerializeField]
	private int m_levelOverride = -1;

	// Token: 0x0400305B RID: 12379
	[SerializeField]
	private bool m_allowItemDrops = true;

	// Token: 0x0400305C RID: 12380
	[SerializeField]
	private bool m_isEasy;

	// Token: 0x0400305D RID: 12381
	[SerializeField]
	[HideInInspector]
	private Vector2Int m_unitDimensions = new Vector2Int(32, 18);

	// Token: 0x0400305E RID: 12382
	[SerializeField]
	[HideInInspector]
	private RoomID m_roomID;

	// Token: 0x0400305F RID: 12383
	[SerializeField]
	[HideInInspector]
	private bool m_isDirty = true;

	// Token: 0x04003060 RID: 12384
	private Bounds m_bounds;

	// Token: 0x04003061 RID: 12385
	private bool m_hasSearchedForGlobalTeleporter;

	// Token: 0x04003062 RID: 12386
	private GlobalTeleporterController m_globalTeleporter;

	// Token: 0x04003063 RID: 12387
	private PlayerRoomSpawn m_spawn;

	// Token: 0x04003064 RID: 12388
	private int m_roomNumber = -1;

	// Token: 0x04003065 RID: 12389
	protected GameObject m_doorsLocation;

	// Token: 0x04003066 RID: 12390
	protected GameObject m_walls;

	// Token: 0x04003067 RID: 12391
	protected Transform m_enemies;

	// Token: 0x04003068 RID: 12392
	private int m_level = -1;

	// Token: 0x04003069 RID: 12393
	protected Transform m_oneWayLocation;

	// Token: 0x0400306A RID: 12394
	private RoomConnectedEventArgs m_roomConnectedEventArgs;

	// Token: 0x0400306B RID: 12395
	protected Relay<object, EventArgs> m_doorClosedRelay = new Relay<object, EventArgs>();

	// Token: 0x0400306C RID: 12396
	protected Relay<object, RoomConnectedEventArgs> m_roomConnectedRelay = new Relay<object, RoomConnectedEventArgs>();

	// Token: 0x0400306D RID: 12397
	protected Relay<object, EventArgs> m_roomMergedRelay = new Relay<object, EventArgs>();

	// Token: 0x04003072 RID: 12402
	private static List<Door> m_doorListHelper_STATIC = new List<Door>();
}

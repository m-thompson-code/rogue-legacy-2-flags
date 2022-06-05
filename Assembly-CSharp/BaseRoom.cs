using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RLWorldCreation;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
public abstract class BaseRoom : MonoBehaviour, ISummoner, ILevelConsumer
{
	// Token: 0x17000EF1 RID: 3825
	// (get) Token: 0x06002521 RID: 9505 RVA: 0x0007B2E9 File Offset: 0x000794E9
	public IRelayLink<object, RoomViaDoorEventArgs> PlayerEnterRelay
	{
		get
		{
			return this.m_playerEnterRelay.link;
		}
	}

	// Token: 0x17000EF2 RID: 3826
	// (get) Token: 0x06002522 RID: 9506 RVA: 0x0007B2F6 File Offset: 0x000794F6
	public IRelayLink<object, RoomViaDoorEventArgs> PlayerExitRelay
	{
		get
		{
			return this.m_playerExitRelay.link;
		}
	}

	// Token: 0x17000EF3 RID: 3827
	// (get) Token: 0x06002523 RID: 9507 RVA: 0x0007B303 File Offset: 0x00079503
	public IRelayLink<object, EventArgs> RoomInitializedRelay
	{
		get
		{
			return this.m_roomInitializedRelay.link;
		}
	}

	// Token: 0x17000EF4 RID: 3828
	// (get) Token: 0x06002524 RID: 9508 RVA: 0x0007B310 File Offset: 0x00079510
	public IRelayLink<object, EventArgs> RoomDestroyedRelay
	{
		get
		{
			return this.m_roomDestroyedRelay.link;
		}
	}

	// Token: 0x17000EF5 RID: 3829
	// (get) Token: 0x06002525 RID: 9509
	public abstract bool SpawnedAsEasyRoom { get; }

	// Token: 0x17000EF6 RID: 3830
	// (get) Token: 0x06002526 RID: 9510
	public abstract BiomeArtData BiomeArtDataOverride { get; }

	// Token: 0x17000EF7 RID: 3831
	// (get) Token: 0x06002527 RID: 9511 RVA: 0x0007B31D File Offset: 0x0007951D
	// (set) Token: 0x06002528 RID: 9512 RVA: 0x0007B325 File Offset: 0x00079525
	public BackgroundPoolEntry[] Backgrounds { get; protected set; }

	// Token: 0x17000EF8 RID: 3832
	// (get) Token: 0x06002529 RID: 9513 RVA: 0x0007B32E File Offset: 0x0007952E
	// (set) Token: 0x0600252A RID: 9514 RVA: 0x0007B336 File Offset: 0x00079536
	public BiomeType AppearanceBiomeType { get; protected set; }

	// Token: 0x17000EF9 RID: 3833
	// (get) Token: 0x0600252B RID: 9515
	public abstract Bounds Bounds { get; }

	// Token: 0x17000EFA RID: 3834
	// (get) Token: 0x0600252C RID: 9516 RVA: 0x0007B33F File Offset: 0x0007953F
	// (set) Token: 0x0600252D RID: 9517 RVA: 0x0007B347 File Offset: 0x00079547
	public Rect BoundsRect { get; protected set; }

	// Token: 0x17000EFB RID: 3835
	// (get) Token: 0x0600252E RID: 9518 RVA: 0x0007B350 File Offset: 0x00079550
	// (set) Token: 0x0600252F RID: 9519 RVA: 0x0007B358 File Offset: 0x00079558
	public int BiomeControllerIndex { get; protected set; }

	// Token: 0x17000EFC RID: 3836
	// (get) Token: 0x06002530 RID: 9520 RVA: 0x0007B361 File Offset: 0x00079561
	public CinemachineVirtualCameraManager CinemachineCamera
	{
		get
		{
			if (!this.m_cinemachineCamera)
			{
				this.m_cinemachineCamera = base.GetComponentInChildren<CinemachineVirtualCameraManager>();
			}
			return this.m_cinemachineCamera;
		}
	}

	// Token: 0x17000EFD RID: 3837
	// (get) Token: 0x06002531 RID: 9521 RVA: 0x0007B382 File Offset: 0x00079582
	public Collider2D Collider2D
	{
		get
		{
			if (!this.m_collider)
			{
				this.m_collider = base.GetComponent<Collider2D>();
			}
			return this.m_collider;
		}
	}

	// Token: 0x17000EFE RID: 3838
	// (get) Token: 0x06002532 RID: 9522 RVA: 0x0007B3A3 File Offset: 0x000795A3
	// (set) Token: 0x06002533 RID: 9523 RVA: 0x0007B3AB File Offset: 0x000795AB
	public List<BaseRoom> ConnectedRooms
	{
		get
		{
			return this.m_connectedRooms;
		}
		protected set
		{
			this.m_connectedRooms = value;
		}
	}

	// Token: 0x17000EFF RID: 3839
	// (get) Token: 0x06002534 RID: 9524 RVA: 0x0007B3B4 File Offset: 0x000795B4
	public virtual bool DisableRoomLetterBoxing
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F00 RID: 3840
	// (get) Token: 0x06002535 RID: 9525 RVA: 0x0007B3B7 File Offset: 0x000795B7
	public virtual bool DisableRoomSaving
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F01 RID: 3841
	// (get) Token: 0x06002536 RID: 9526 RVA: 0x0007B3BA File Offset: 0x000795BA
	public virtual List<Door> Doors
	{
		get
		{
			if (this.m_doors == null)
			{
				this.m_doors = base.GetComponentsInChildren<Door>().ToList<Door>();
			}
			return this.m_doors;
		}
	}

	// Token: 0x17000F02 RID: 3842
	// (get) Token: 0x06002537 RID: 9527 RVA: 0x0007B3DB File Offset: 0x000795DB
	// (set) Token: 0x06002538 RID: 9528 RVA: 0x0007B3E3 File Offset: 0x000795E3
	public BiomeType BiomeType
	{
		get
		{
			return this.m_isInBiome;
		}
		set
		{
			this.m_isInBiome = value;
		}
	}

	// Token: 0x17000F03 RID: 3843
	// (get) Token: 0x06002539 RID: 9529 RVA: 0x0007B3EC File Offset: 0x000795EC
	// (set) Token: 0x0600253A RID: 9530 RVA: 0x0007B3F4 File Offset: 0x000795F4
	public bool IsPlayerInRoom { get; protected set; }

	// Token: 0x17000F04 RID: 3844
	// (get) Token: 0x0600253B RID: 9531
	public abstract int Level { get; }

	// Token: 0x17000F05 RID: 3845
	// (get) Token: 0x0600253C RID: 9532 RVA: 0x0007B3FD File Offset: 0x000795FD
	// (set) Token: 0x0600253D RID: 9533 RVA: 0x0007B405 File Offset: 0x00079605
	public virtual RoomType RoomType { get; protected set; }

	// Token: 0x17000F06 RID: 3846
	// (get) Token: 0x0600253E RID: 9534 RVA: 0x0007B40E File Offset: 0x0007960E
	// (set) Token: 0x0600253F RID: 9535 RVA: 0x0007B416 File Offset: 0x00079616
	public RoomSaveController SaveController { get; protected set; }

	// Token: 0x17000F07 RID: 3847
	// (get) Token: 0x06002540 RID: 9536 RVA: 0x0007B41F File Offset: 0x0007961F
	// (set) Token: 0x06002541 RID: 9537 RVA: 0x0007B427 File Offset: 0x00079627
	public RoomSpawnControllerManager SpawnControllerManager { get; protected set; }

	// Token: 0x17000F08 RID: 3848
	// (get) Token: 0x06002542 RID: 9538 RVA: 0x0007B430 File Offset: 0x00079630
	public virtual SpecialRoomType SpecialRoomType
	{
		get
		{
			return this.m_specialRoomType;
		}
	}

	// Token: 0x17000F09 RID: 3849
	// (get) Token: 0x06002543 RID: 9539 RVA: 0x0007B438 File Offset: 0x00079638
	// (set) Token: 0x06002544 RID: 9540 RVA: 0x0007B440 File Offset: 0x00079640
	public RoomTerrainManager TerrainManager { get; protected set; }

	// Token: 0x17000F0A RID: 3850
	// (get) Token: 0x06002545 RID: 9541 RVA: 0x0007B449 File Offset: 0x00079649
	public virtual bool AllowItemDrops { get; }

	// Token: 0x06002546 RID: 9542 RVA: 0x0007B451 File Offset: 0x00079651
	protected virtual void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.ConnectedRooms = null;
			this.m_roomDestroyedRelay.Dispatch(this, EventArgs.Empty);
		}
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x0007B472 File Offset: 0x00079672
	public void SetSpawnControllerManager(RoomSpawnControllerManager spawnControllerManager)
	{
		this.SpawnControllerManager = spawnControllerManager;
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x0007B47B File Offset: 0x0007967B
	public void SetTerrainManager(RoomTerrainManager terrainManager)
	{
		this.TerrainManager = terrainManager;
	}

	// Token: 0x06002549 RID: 9545 RVA: 0x0007B484 File Offset: 0x00079684
	public void SetBiomeControllerIndex(int index)
	{
		this.BiomeControllerIndex = index;
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x0007B490 File Offset: 0x00079690
	public void InjectRoom()
	{
		IRoomConsumer[] componentsInChildren = base.gameObject.GetComponentsInChildren<IRoomConsumer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetRoom(this);
		}
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x0007B4C0 File Offset: 0x000796C0
	public void SetBackgroundPoolEntries(BackgroundPoolEntry[] backgrounds)
	{
		this.Backgrounds = backgrounds;
	}

	// Token: 0x0600254C RID: 9548
	public abstract void SetLevel(int level);

	// Token: 0x0600254D RID: 9549 RVA: 0x0007B4C9 File Offset: 0x000796C9
	public void PlacePlayerInRoom(Door door)
	{
		this.PlacePlayerInRoom_Start();
		if (door != null)
		{
			PlayerManager.GetPlayerController().EnterRoom(door);
		}
		else
		{
			PlayerManager.GetPlayerController().EnterRoom(this);
		}
		this.PlacePlayerInRoom_End(door);
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x0007B4F9 File Offset: 0x000796F9
	public void PlacePlayerInRoom(Vector3 localPosition)
	{
		this.PlacePlayerInRoom_Start();
		PlayerManager.GetPlayerController().EnterRoom(this, null, localPosition);
		this.PlacePlayerInRoom_End(null);
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x0007B515 File Offset: 0x00079715
	private void PlacePlayerInRoom_Start()
	{
		this.IsPlayerInRoom = true;
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x0007B520 File Offset: 0x00079720
	private void PlacePlayerInRoom_End(Door door)
	{
		RNGManager.SetSeed(RngID.SpecialProps_RoomSeed, this.GetSpecialPropSeed());
		this.InitializeSpawnControllerInstances();
		this.InitializeBackgrounds();
		this.SaveController.OnPlayerEnter_LoadStageData();
		if (this.m_roomViaDoorEventArgs == null)
		{
			this.m_roomViaDoorEventArgs = new RoomViaDoorEventArgs(this, door);
		}
		else
		{
			this.m_roomViaDoorEventArgs.Initialise(this, door);
		}
		CameraController.UpdateRoomCameraSettings(this.m_roomViaDoorEventArgs);
		this.BroadcastPlayerEnterRoomEvents(door);
	}

	// Token: 0x06002551 RID: 9553
	protected abstract int GetDecoSeed();

	// Token: 0x06002552 RID: 9554
	protected abstract int GetSpecialPropSeed();

	// Token: 0x06002553 RID: 9555 RVA: 0x0007B58C File Offset: 0x0007978C
	private void InitializeSpawnControllerInstances()
	{
		PropSpawnController[] propSpawnControllers = this.SpawnControllerManager.PropSpawnControllers;
		for (int i = 0; i < propSpawnControllers.Length; i++)
		{
			propSpawnControllers[i].InitializePropInstance();
		}
		EnemySpawnController[] enemySpawnControllers = this.SpawnControllerManager.EnemySpawnControllers;
		for (int i = 0; i < enemySpawnControllers.Length; i++)
		{
			enemySpawnControllers[i].InitializeEnemyInstance();
		}
		ChestSpawnController[] chestSpawnControllers = this.SpawnControllerManager.ChestSpawnControllers;
		for (int i = 0; i < chestSpawnControllers.Length; i++)
		{
			chestSpawnControllers[i].InitializeChestInstance();
		}
	}

	// Token: 0x06002554 RID: 9556 RVA: 0x0007B600 File Offset: 0x00079800
	private void InitializeBackgrounds()
	{
		if (this.Backgrounds == null)
		{
			return;
		}
		foreach (BackgroundPoolEntry backgroundPoolEntry in this.Backgrounds)
		{
			if (backgroundPoolEntry.BackgroundPrefab)
			{
				Background background = BackgroundsPoolManager.GetBackground(backgroundPoolEntry.BackgroundPrefab);
				if (!background)
				{
					background = BackgroundsPoolManager.GetBackground(backgroundPoolEntry.BackgroundPrefab);
					throw new Exception("Failed to find pooled background: " + backgroundPoolEntry.BackgroundPrefab.name + ". BackgroundsPoolManager did not create pools correctly.");
				}
				background.transform.SetParent(base.transform);
				background.transform.position = backgroundPoolEntry.Position;
				background.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x0007B6B7 File Offset: 0x000798B7
	private void BroadcastPlayerEnterRoomEvents(Door door)
	{
		this.m_playerEnterRelay.Dispatch(this, this.m_roomViaDoorEventArgs);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterRoom, this, this.m_roomViaDoorEventArgs);
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x0007B6D8 File Offset: 0x000798D8
	protected void OnPlayerExitViaDoor(object sender, DoorEventArgs eventArgs)
	{
		this.PlayerExit(eventArgs.Door);
	}

	// Token: 0x06002557 RID: 9559 RVA: 0x0007B6E8 File Offset: 0x000798E8
	public void PlayerExit(Door door)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController.IsDead)
		{
			return;
		}
		if (playerController.CurrentHealth <= 0f && !ChallengeManager.IsInChallenge)
		{
			base.StartCoroutine(this.DelayEnterRoomCoroutine(door));
			return;
		}
		bool flag = true;
		if (door != null)
		{
			if (door.IsBiomeTransitionPoint)
			{
				BiomeTransitionController.Run(door);
				flag = false;
			}
			else if (door.ConnectedDoor != null && door.ConnectedDoor.Room != null)
			{
				Door connectedDoor = door.ConnectedDoor;
				BaseRoom room = connectedDoor.Room;
				bool flag2 = room.Bounds.size.x > 160f || room.Bounds.size.y > 90f || this.Bounds.size.x > 160f || this.Bounds.size.y > 90f;
				bool flag3 = this.BiomeType == BiomeType.Tower && ((this.AppearanceBiomeType == BiomeType.Tower && room.AppearanceBiomeType == BiomeType.TowerExterior) || (this.AppearanceBiomeType == BiomeType.TowerExterior && room.AppearanceBiomeType == BiomeType.Tower));
				if (flag2 || flag3)
				{
					flag = false;
					SceneLoader_RL.RunTransitionWithLogic(this.LargeRoomTransitionCoroutine(door, connectedDoor, room), TransitionID.QuickSwipe, false);
				}
				else
				{
					this.BroadcastPlayerExitRoomEvents(door);
					if (flag)
					{
						flag = false;
						base.gameObject.SetActive(false);
					}
					room.PlacePlayerInRoom(connectedDoor);
				}
			}
			else if (door.ConnectedDoor == null)
			{
				Debug.LogFormat("<color=red>You entered a Door whose Connected Door is null</color>", Array.Empty<object>());
			}
			else if (door.ConnectedDoor.Room == null)
			{
				Debug.LogFormat("<color=red>You entered a Door whose Connected Door's Room is null</color>", Array.Empty<object>());
			}
		}
		else
		{
			this.BroadcastPlayerExitRoomEvents(null);
		}
		if (flag)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x0007B8CE File Offset: 0x00079ACE
	private IEnumerator DelayEnterRoomCoroutine(Door door)
	{
		yield return null;
		while (GameManager.IsGamePaused)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (!playerController.IsDead && playerController.CurrentHealth > 0f)
		{
			this.PlayerExit(door);
		}
		yield break;
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x0007B8E4 File Offset: 0x00079AE4
	private IEnumerator LargeRoomTransitionCoroutine(Door doorBeingExited, Door doorBeingEntered, BaseRoom roomBeingEntered)
	{
		this.BroadcastPlayerExitRoomEvents(doorBeingExited);
		base.gameObject.SetActive(false);
		roomBeingEntered.PlacePlayerInRoom(doorBeingEntered);
		yield break;
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x0007B908 File Offset: 0x00079B08
	public void BroadcastPlayerExitRoomEvents(Door door)
	{
		this.IsPlayerInRoom = false;
		if (this.m_roomViaDoorEventArgs == null)
		{
			this.m_roomViaDoorEventArgs = new RoomViaDoorEventArgs(this, door);
		}
		else
		{
			this.m_roomViaDoorEventArgs.Initialise(this, door);
		}
		this.SaveController.OnPlayerExit_SaveStageData();
		this.m_playerExitRelay.Dispatch(this, this.m_roomViaDoorEventArgs);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerExitRoom, this, this.m_roomViaDoorEventArgs);
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x0007B9AA File Offset: 0x00079BAA
	GameObject ISummoner.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001F5E RID: 8030
	protected CinemachineVirtualCameraManager m_cinemachineCamera;

	// Token: 0x04001F5F RID: 8031
	protected Collider2D m_collider;

	// Token: 0x04001F60 RID: 8032
	protected List<BaseRoom> m_connectedRooms = new List<BaseRoom>();

	// Token: 0x04001F61 RID: 8033
	protected List<Door> m_doors;

	// Token: 0x04001F62 RID: 8034
	protected BiomeType m_isInBiome;

	// Token: 0x04001F63 RID: 8035
	protected RoomViaDoorEventArgs m_roomViaDoorEventArgs;

	// Token: 0x04001F64 RID: 8036
	protected SpecialRoomType m_specialRoomType;

	// Token: 0x04001F65 RID: 8037
	protected Relay<object, RoomViaDoorEventArgs> m_playerEnterRelay = new Relay<object, RoomViaDoorEventArgs>();

	// Token: 0x04001F66 RID: 8038
	protected Relay<object, RoomViaDoorEventArgs> m_playerExitRelay = new Relay<object, RoomViaDoorEventArgs>();

	// Token: 0x04001F67 RID: 8039
	protected Relay<object, EventArgs> m_roomInitializedRelay = new Relay<object, EventArgs>();

	// Token: 0x04001F68 RID: 8040
	protected Relay<object, EventArgs> m_roomDestroyedRelay = new Relay<object, EventArgs>();
}

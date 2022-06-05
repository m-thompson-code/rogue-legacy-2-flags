using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RLWorldCreation;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000695 RID: 1685
public abstract class BaseRoom : MonoBehaviour, ISummoner, ILevelConsumer
{
	// Token: 0x1700139A RID: 5018
	// (get) Token: 0x0600337B RID: 13179 RVA: 0x0001C346 File Offset: 0x0001A546
	public IRelayLink<object, RoomViaDoorEventArgs> PlayerEnterRelay
	{
		get
		{
			return this.m_playerEnterRelay.link;
		}
	}

	// Token: 0x1700139B RID: 5019
	// (get) Token: 0x0600337C RID: 13180 RVA: 0x0001C353 File Offset: 0x0001A553
	public IRelayLink<object, RoomViaDoorEventArgs> PlayerExitRelay
	{
		get
		{
			return this.m_playerExitRelay.link;
		}
	}

	// Token: 0x1700139C RID: 5020
	// (get) Token: 0x0600337D RID: 13181 RVA: 0x0001C360 File Offset: 0x0001A560
	public IRelayLink<object, EventArgs> RoomInitializedRelay
	{
		get
		{
			return this.m_roomInitializedRelay.link;
		}
	}

	// Token: 0x1700139D RID: 5021
	// (get) Token: 0x0600337E RID: 13182 RVA: 0x0001C36D File Offset: 0x0001A56D
	public IRelayLink<object, EventArgs> RoomDestroyedRelay
	{
		get
		{
			return this.m_roomDestroyedRelay.link;
		}
	}

	// Token: 0x1700139E RID: 5022
	// (get) Token: 0x0600337F RID: 13183
	public abstract bool SpawnedAsEasyRoom { get; }

	// Token: 0x1700139F RID: 5023
	// (get) Token: 0x06003380 RID: 13184
	public abstract BiomeArtData BiomeArtDataOverride { get; }

	// Token: 0x170013A0 RID: 5024
	// (get) Token: 0x06003381 RID: 13185 RVA: 0x0001C37A File Offset: 0x0001A57A
	// (set) Token: 0x06003382 RID: 13186 RVA: 0x0001C382 File Offset: 0x0001A582
	public BackgroundPoolEntry[] Backgrounds { get; protected set; }

	// Token: 0x170013A1 RID: 5025
	// (get) Token: 0x06003383 RID: 13187 RVA: 0x0001C38B File Offset: 0x0001A58B
	// (set) Token: 0x06003384 RID: 13188 RVA: 0x0001C393 File Offset: 0x0001A593
	public BiomeType AppearanceBiomeType { get; protected set; }

	// Token: 0x170013A2 RID: 5026
	// (get) Token: 0x06003385 RID: 13189
	public abstract Bounds Bounds { get; }

	// Token: 0x170013A3 RID: 5027
	// (get) Token: 0x06003386 RID: 13190 RVA: 0x0001C39C File Offset: 0x0001A59C
	// (set) Token: 0x06003387 RID: 13191 RVA: 0x0001C3A4 File Offset: 0x0001A5A4
	public Rect BoundsRect { get; protected set; }

	// Token: 0x170013A4 RID: 5028
	// (get) Token: 0x06003388 RID: 13192 RVA: 0x0001C3AD File Offset: 0x0001A5AD
	// (set) Token: 0x06003389 RID: 13193 RVA: 0x0001C3B5 File Offset: 0x0001A5B5
	public int BiomeControllerIndex { get; protected set; }

	// Token: 0x170013A5 RID: 5029
	// (get) Token: 0x0600338A RID: 13194 RVA: 0x0001C3BE File Offset: 0x0001A5BE
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

	// Token: 0x170013A6 RID: 5030
	// (get) Token: 0x0600338B RID: 13195 RVA: 0x0001C3DF File Offset: 0x0001A5DF
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

	// Token: 0x170013A7 RID: 5031
	// (get) Token: 0x0600338C RID: 13196 RVA: 0x0001C400 File Offset: 0x0001A600
	// (set) Token: 0x0600338D RID: 13197 RVA: 0x0001C408 File Offset: 0x0001A608
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

	// Token: 0x170013A8 RID: 5032
	// (get) Token: 0x0600338E RID: 13198 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public virtual bool DisableRoomLetterBoxing
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170013A9 RID: 5033
	// (get) Token: 0x0600338F RID: 13199 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public virtual bool DisableRoomSaving
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170013AA RID: 5034
	// (get) Token: 0x06003390 RID: 13200 RVA: 0x0001C411 File Offset: 0x0001A611
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

	// Token: 0x170013AB RID: 5035
	// (get) Token: 0x06003391 RID: 13201 RVA: 0x0001C432 File Offset: 0x0001A632
	// (set) Token: 0x06003392 RID: 13202 RVA: 0x0001C43A File Offset: 0x0001A63A
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

	// Token: 0x170013AC RID: 5036
	// (get) Token: 0x06003393 RID: 13203 RVA: 0x0001C443 File Offset: 0x0001A643
	// (set) Token: 0x06003394 RID: 13204 RVA: 0x0001C44B File Offset: 0x0001A64B
	public bool IsPlayerInRoom { get; protected set; }

	// Token: 0x170013AD RID: 5037
	// (get) Token: 0x06003395 RID: 13205
	public abstract int Level { get; }

	// Token: 0x170013AE RID: 5038
	// (get) Token: 0x06003396 RID: 13206 RVA: 0x0001C454 File Offset: 0x0001A654
	// (set) Token: 0x06003397 RID: 13207 RVA: 0x0001C45C File Offset: 0x0001A65C
	public virtual RoomType RoomType { get; protected set; }

	// Token: 0x170013AF RID: 5039
	// (get) Token: 0x06003398 RID: 13208 RVA: 0x0001C465 File Offset: 0x0001A665
	// (set) Token: 0x06003399 RID: 13209 RVA: 0x0001C46D File Offset: 0x0001A66D
	public RoomSaveController SaveController { get; protected set; }

	// Token: 0x170013B0 RID: 5040
	// (get) Token: 0x0600339A RID: 13210 RVA: 0x0001C476 File Offset: 0x0001A676
	// (set) Token: 0x0600339B RID: 13211 RVA: 0x0001C47E File Offset: 0x0001A67E
	public RoomSpawnControllerManager SpawnControllerManager { get; protected set; }

	// Token: 0x170013B1 RID: 5041
	// (get) Token: 0x0600339C RID: 13212 RVA: 0x0001C487 File Offset: 0x0001A687
	public virtual SpecialRoomType SpecialRoomType
	{
		get
		{
			return this.m_specialRoomType;
		}
	}

	// Token: 0x170013B2 RID: 5042
	// (get) Token: 0x0600339D RID: 13213 RVA: 0x0001C48F File Offset: 0x0001A68F
	// (set) Token: 0x0600339E RID: 13214 RVA: 0x0001C497 File Offset: 0x0001A697
	public RoomTerrainManager TerrainManager { get; protected set; }

	// Token: 0x170013B3 RID: 5043
	// (get) Token: 0x0600339F RID: 13215 RVA: 0x0001C4A0 File Offset: 0x0001A6A0
	public virtual bool AllowItemDrops { get; }

	// Token: 0x060033A0 RID: 13216 RVA: 0x0001C4A8 File Offset: 0x0001A6A8
	protected virtual void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.ConnectedRooms = null;
			this.m_roomDestroyedRelay.Dispatch(this, EventArgs.Empty);
		}
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x0001C4C9 File Offset: 0x0001A6C9
	public void SetSpawnControllerManager(RoomSpawnControllerManager spawnControllerManager)
	{
		this.SpawnControllerManager = spawnControllerManager;
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x0001C4D2 File Offset: 0x0001A6D2
	public void SetTerrainManager(RoomTerrainManager terrainManager)
	{
		this.TerrainManager = terrainManager;
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x0001C4DB File Offset: 0x0001A6DB
	public void SetBiomeControllerIndex(int index)
	{
		this.BiomeControllerIndex = index;
	}

	// Token: 0x060033A4 RID: 13220 RVA: 0x000DB3C8 File Offset: 0x000D95C8
	public void InjectRoom()
	{
		IRoomConsumer[] componentsInChildren = base.gameObject.GetComponentsInChildren<IRoomConsumer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetRoom(this);
		}
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x0001C4E4 File Offset: 0x0001A6E4
	public void SetBackgroundPoolEntries(BackgroundPoolEntry[] backgrounds)
	{
		this.Backgrounds = backgrounds;
	}

	// Token: 0x060033A6 RID: 13222
	public abstract void SetLevel(int level);

	// Token: 0x060033A7 RID: 13223 RVA: 0x0001C4ED File Offset: 0x0001A6ED
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

	// Token: 0x060033A8 RID: 13224 RVA: 0x0001C51D File Offset: 0x0001A71D
	public void PlacePlayerInRoom(Vector3 localPosition)
	{
		this.PlacePlayerInRoom_Start();
		PlayerManager.GetPlayerController().EnterRoom(this, null, localPosition);
		this.PlacePlayerInRoom_End(null);
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x0001C539 File Offset: 0x0001A739
	private void PlacePlayerInRoom_Start()
	{
		this.IsPlayerInRoom = true;
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x000DB3F8 File Offset: 0x000D95F8
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

	// Token: 0x060033AB RID: 13227
	protected abstract int GetDecoSeed();

	// Token: 0x060033AC RID: 13228
	protected abstract int GetSpecialPropSeed();

	// Token: 0x060033AD RID: 13229 RVA: 0x000DB464 File Offset: 0x000D9664
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

	// Token: 0x060033AE RID: 13230 RVA: 0x000DB4D8 File Offset: 0x000D96D8
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

	// Token: 0x060033AF RID: 13231 RVA: 0x0001C542 File Offset: 0x0001A742
	private void BroadcastPlayerEnterRoomEvents(Door door)
	{
		this.m_playerEnterRelay.Dispatch(this, this.m_roomViaDoorEventArgs);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterRoom, this, this.m_roomViaDoorEventArgs);
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x0001C563 File Offset: 0x0001A763
	protected void OnPlayerExitViaDoor(object sender, DoorEventArgs eventArgs)
	{
		this.PlayerExit(eventArgs.Door);
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x000DB590 File Offset: 0x000D9790
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

	// Token: 0x060033B2 RID: 13234 RVA: 0x0001C571 File Offset: 0x0001A771
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

	// Token: 0x060033B3 RID: 13235 RVA: 0x0001C587 File Offset: 0x0001A787
	private IEnumerator LargeRoomTransitionCoroutine(Door doorBeingExited, Door doorBeingEntered, BaseRoom roomBeingEntered)
	{
		this.BroadcastPlayerExitRoomEvents(doorBeingExited);
		base.gameObject.SetActive(false);
		roomBeingEntered.PlacePlayerInRoom(doorBeingEntered);
		yield break;
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x000DB778 File Offset: 0x000D9978
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

	// Token: 0x060033B6 RID: 13238 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISummoner.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040029F0 RID: 10736
	protected CinemachineVirtualCameraManager m_cinemachineCamera;

	// Token: 0x040029F1 RID: 10737
	protected Collider2D m_collider;

	// Token: 0x040029F2 RID: 10738
	protected List<BaseRoom> m_connectedRooms = new List<BaseRoom>();

	// Token: 0x040029F3 RID: 10739
	protected List<Door> m_doors;

	// Token: 0x040029F4 RID: 10740
	protected BiomeType m_isInBiome;

	// Token: 0x040029F5 RID: 10741
	protected RoomViaDoorEventArgs m_roomViaDoorEventArgs;

	// Token: 0x040029F6 RID: 10742
	protected SpecialRoomType m_specialRoomType;

	// Token: 0x040029F7 RID: 10743
	protected Relay<object, RoomViaDoorEventArgs> m_playerEnterRelay = new Relay<object, RoomViaDoorEventArgs>();

	// Token: 0x040029F8 RID: 10744
	protected Relay<object, RoomViaDoorEventArgs> m_playerExitRelay = new Relay<object, RoomViaDoorEventArgs>();

	// Token: 0x040029F9 RID: 10745
	protected Relay<object, EventArgs> m_roomInitializedRelay = new Relay<object, EventArgs>();

	// Token: 0x040029FA RID: 10746
	protected Relay<object, EventArgs> m_roomDestroyedRelay = new Relay<object, EventArgs>();
}

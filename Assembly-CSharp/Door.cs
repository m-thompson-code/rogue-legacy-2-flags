using System;
using System.Collections;
using System.Collections.Generic;
using Ferr;
using MoreMountains.CorgiEngine;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020006AF RID: 1711
public class Door : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170013FD RID: 5117
	// (get) Token: 0x0600349D RID: 13469 RVA: 0x0001CE01 File Offset: 0x0001B001
	public IRelayLink<object, DoorEventArgs> PlayerEnterRelay
	{
		get
		{
			return this.m_playerEnterRelay.link;
		}
	}

	// Token: 0x170013FE RID: 5118
	// (get) Token: 0x0600349E RID: 13470 RVA: 0x0001CE0E File Offset: 0x0001B00E
	public IRelayLink<object, DoorEventArgs> CloseRelay
	{
		get
		{
			return this.m_closeRelay.link;
		}
	}

	// Token: 0x170013FF RID: 5119
	// (get) Token: 0x0600349F RID: 13471 RVA: 0x0001CE1B File Offset: 0x0001B01B
	public IRelayLink<object, DoorConnectEventArgs> DoorConnectRelay
	{
		get
		{
			return this.m_doorConnectRelay.link;
		}
	}

	// Token: 0x17001400 RID: 5120
	// (get) Token: 0x060034A0 RID: 13472 RVA: 0x0001CE28 File Offset: 0x0001B028
	public IRelayLink<object, DoorConnectEventArgs> DoorDisconnectRelay
	{
		get
		{
			return this.m_doorDisconnectRelay.link;
		}
	}

	// Token: 0x17001401 RID: 5121
	// (get) Token: 0x060034A1 RID: 13473 RVA: 0x0001CE35 File Offset: 0x0001B035
	public IRelayLink<object, EventArgs> DoorDestroyedRelay
	{
		get
		{
			return this.m_doorDestroyedRelay.link;
		}
	}

	// Token: 0x17001402 RID: 5122
	// (get) Token: 0x060034A2 RID: 13474 RVA: 0x0001CE42 File Offset: 0x0001B042
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x000DDC0C File Offset: 0x000DBE0C
	public void SetRoom(BaseRoom value)
	{
		if (this.m_room)
		{
			this.RemoveRoomEventHandlers();
		}
		this.m_room = value;
		if (this.m_room)
		{
			this.AddRoomEventHandlers();
		}
		if (value is Room)
		{
			Room room = value as Room;
			this.CenterPoint = RoomUtility.GetDoorCenterPoint(room.Coordinates, room.Size, new DoorLocation(this.Side, this.Number));
			this.Coordinates = RoomUtility.GetDoorCoordinates(value as Room, this);
			this.GridPointManager = (value as Room).GridPointManager;
			if (this.GridPointManager != null)
			{
				GridPoint gridPoint = this.GridPointManager.GetGridPoint(new DoorLocation(this.Side, this.Number));
				if (gridPoint != null)
				{
					this.GridPointCoordinates = gridPoint.GridCoordinates;
				}
			}
		}
	}

	// Token: 0x17001403 RID: 5123
	// (get) Token: 0x060034A4 RID: 13476 RVA: 0x000DDCD8 File Offset: 0x000DBED8
	public Ferr2DT_PathTerrain Ferr2D
	{
		get
		{
			if (this.m_terrain == null)
			{
				this.m_terrain = base.GetComponent<Ferr2DT_PathTerrain>();
				if (this.m_terrain == null)
				{
					Debug.LogFormat("Unable to find Ferr2DT_PathTerrain Component on ({0})", new object[]
					{
						base.name
					});
				}
			}
			return this.m_terrain;
		}
	}

	// Token: 0x17001404 RID: 5124
	// (get) Token: 0x060034A5 RID: 13477 RVA: 0x0001CE4A File Offset: 0x0001B04A
	// (set) Token: 0x060034A6 RID: 13478 RVA: 0x0001CE52 File Offset: 0x0001B052
	public Door ConnectedDoor
	{
		get
		{
			return this.m_connectedDoor;
		}
		private set
		{
			this.m_connectedDoor = value;
		}
	}

	// Token: 0x17001405 RID: 5125
	// (get) Token: 0x060034A7 RID: 13479 RVA: 0x0001CE5B File Offset: 0x0001B05B
	public BaseRoom ConnectedRoom
	{
		get
		{
			if (this.ConnectedDoor != null)
			{
				return this.ConnectedDoor.Room;
			}
			return null;
		}
	}

	// Token: 0x17001406 RID: 5126
	// (get) Token: 0x060034A8 RID: 13480 RVA: 0x0001CE78 File Offset: 0x0001B078
	// (set) Token: 0x060034A9 RID: 13481 RVA: 0x0001CE80 File Offset: 0x0001B080
	public GridPointManager GridPointManager { get; private set; }

	// Token: 0x17001407 RID: 5127
	// (get) Token: 0x060034AA RID: 13482 RVA: 0x0001CE89 File Offset: 0x0001B089
	// (set) Token: 0x060034AB RID: 13483 RVA: 0x0001CE91 File Offset: 0x0001B091
	public Vector2Int GridPointCoordinates { get; private set; }

	// Token: 0x17001408 RID: 5128
	// (get) Token: 0x060034AC RID: 13484 RVA: 0x0001CE9A File Offset: 0x0001B09A
	// (set) Token: 0x060034AD RID: 13485 RVA: 0x0001CEA2 File Offset: 0x0001B0A2
	public Vector2 Coordinates { get; private set; }

	// Token: 0x17001409 RID: 5129
	// (get) Token: 0x060034AE RID: 13486 RVA: 0x0001CEAB File Offset: 0x0001B0AB
	public bool DisabledFromLevelEditor
	{
		get
		{
			return this.m_disableDoor;
		}
	}

	// Token: 0x1700140A RID: 5130
	// (get) Token: 0x060034AF RID: 13487 RVA: 0x0001CEB3 File Offset: 0x0001B0B3
	// (set) Token: 0x060034B0 RID: 13488 RVA: 0x0001CEBB File Offset: 0x0001B0BB
	public Vector2 CenterPoint
	{
		get
		{
			return this.m_centerPoint;
		}
		private set
		{
			this.m_centerPoint = value;
		}
	}

	// Token: 0x1700140B RID: 5131
	// (get) Token: 0x060034B1 RID: 13489 RVA: 0x0001CEC4 File Offset: 0x0001B0C4
	// (set) Token: 0x060034B2 RID: 13490 RVA: 0x0001CECC File Offset: 0x0001B0CC
	public int Number
	{
		get
		{
			return this.m_index;
		}
		set
		{
			this.m_index = value;
		}
	}

	// Token: 0x1700140C RID: 5132
	// (get) Token: 0x060034B3 RID: 13491 RVA: 0x0001CED5 File Offset: 0x0001B0D5
	// (set) Token: 0x060034B4 RID: 13492 RVA: 0x0001CEDD File Offset: 0x0001B0DD
	public RoomSide Side
	{
		get
		{
			return this.m_side;
		}
		set
		{
			this.m_side = value;
		}
	}

	// Token: 0x1700140D RID: 5133
	// (get) Token: 0x060034B5 RID: 13493 RVA: 0x0001CEE6 File Offset: 0x0001B0E6
	// (set) Token: 0x060034B6 RID: 13494 RVA: 0x0001CEEE File Offset: 0x0001B0EE
	public GameObject OneWay { get; private set; }

	// Token: 0x060034B7 RID: 13495 RVA: 0x0001CEF7 File Offset: 0x0001B0F7
	public void SetOneWay(GameObject oneWay)
	{
		this.OneWay = oneWay;
	}

	// Token: 0x1700140E RID: 5134
	// (get) Token: 0x060034B8 RID: 13496 RVA: 0x0001CF00 File Offset: 0x0001B100
	public GameObject GameObject
	{
		get
		{
			if (this != null)
			{
				return base.gameObject;
			}
			return null;
		}
	}

	// Token: 0x1700140F RID: 5135
	// (get) Token: 0x060034B9 RID: 13497 RVA: 0x0001CF13 File Offset: 0x0001B113
	public bool IsBiomeTransitionPoint
	{
		get
		{
			return this.TransitionsToBiome > BiomeType.None;
		}
	}

	// Token: 0x17001410 RID: 5136
	// (get) Token: 0x060034BA RID: 13498 RVA: 0x0001CF1E File Offset: 0x0001B11E
	// (set) Token: 0x060034BB RID: 13499 RVA: 0x0001CF26 File Offset: 0x0001B126
	public BiomeType TransitionsToBiome
	{
		get
		{
			return this.m_transitionsToBiome;
		}
		private set
		{
			this.m_transitionsToBiome = value;
		}
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x0001CF2F File Offset: 0x0001B12F
	public void SetIsBiomeTransitionPoint(BiomeType biome)
	{
		this.TransitionsToBiome = biome;
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x000DDD2C File Offset: 0x000DBF2C
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerExitRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom);
		this.m_onRoomDestroyed = new Action<object, EventArgs>(this.OnRoomDestroyed);
		this.m_onConnectedDoorDestroyed = new Action<object, EventArgs>(this.OnConnectedDoorDestroyed);
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x000DDD84 File Offset: 0x000DBF84
	private void Start()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component)
		{
			component.enabled = false;
		}
		this.m_collider = base.GetComponent<PolygonCollider2D>();
		if (!this.m_collider)
		{
			throw new MissingComponentException("PolygonCollider2D");
		}
		if (!this.m_collider.isTrigger)
		{
			this.m_collider.isTrigger = true;
			return;
		}
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x000DDDE8 File Offset: 0x000DBFE8
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (!this.m_hasExitTriggerBeenCreated)
		{
			this.m_hasExitTriggerBeenCreated = true;
			this.CreateExitTrigger();
			this.CreateLedge();
		}
		if (this.m_exitTrigger != null)
		{
			this.m_exitTrigger.SetTriggerActive(true);
			return;
		}
		Debug.LogFormat("<color=red>{0}: Door ({1}, {2}) in Room ({3}) m_exitTrigger is null.</color>", new object[]
		{
			Time.frameCount,
			this.Side,
			this.Number,
			this.Room.gameObject.name
		});
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x000DDE78 File Offset: 0x000DC078
	private void OnPlayerExitRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (this.m_exitTrigger != null && this.m_exitTrigger.IsTriggerActive)
		{
			this.m_exitTrigger.SetTriggerActive(false);
			return;
		}
		if (this.m_exitTrigger == null)
		{
			Debug.LogFormat("<color=red>{0}: Door ({1}, {2}) in Room ({3}) m_exitTrigger is null.</color>", new object[]
			{
				Time.frameCount,
				this.Side,
				this.Number,
				this.Room.gameObject.name
			});
		}
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x0001CF38 File Offset: 0x0001B138
	private void OnRoomDestroyed(object sender, EventArgs eventArgs)
	{
		this.SetRoom(null);
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x000DDF08 File Offset: 0x000DC108
	private void CreateExitTrigger()
	{
		Vector2 exitColliderSize = this.GetExitColliderSize();
		Vector2 exitColliderOffset = this.GetExitColliderOffset(exitColliderSize);
		if (this.Side == RoomSide.Left || this.Side == RoomSide.Right)
		{
			int num = 0;
			using (List<Door>.Enumerator enumerator = this.Room.Doors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Side == this.Side)
					{
						num++;
					}
				}
			}
			if (num == 1)
			{
				exitColliderSize.y *= 2f;
			}
		}
		this.m_exitTrigger = PlayerTrigger.CreateInstance(exitColliderSize, this.CenterPoint + exitColliderOffset, base.transform);
		this.m_exitTrigger.PlayerEnter += this.OnPlayerEnterExitTrigger;
		this.m_exitTrigger.SetTriggerActive(false);
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x0001CF41 File Offset: 0x0001B141
	private Vector2 GetExitColliderSize()
	{
		return new Vector2(32f, 18f);
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x000DDFE4 File Offset: 0x000DC1E4
	private Vector2 GetExitColliderOffset(Vector2 size)
	{
		Vector2 vector = Vector2.zero;
		switch (this.Side)
		{
		case RoomSide.Top:
			vector = new Vector2(0f, 0.5f * size.y);
			break;
		case RoomSide.Bottom:
			vector = new Vector2(0f, -0.5f * size.y);
			break;
		case RoomSide.Left:
			vector = new Vector2(-0.5f * size.x, 0f);
			break;
		case RoomSide.Right:
			vector = new Vector2(0.5f * size.x, 0f);
			break;
		}
		switch (this.Side)
		{
		case RoomSide.Top:
			if (Room_EV.TRANSITION_TOP_DISTANCE < 0f)
			{
				vector += new Vector2(0f, Room_EV.TRANSITION_TOP_DISTANCE);
			}
			break;
		case RoomSide.Bottom:
			if (Room_EV.TRANSITION_BOTTOM_DISTANCE > 0f)
			{
				vector += new Vector2(0f, Room_EV.TRANSITION_BOTTOM_DISTANCE);
			}
			break;
		case RoomSide.Left:
			if (Room_EV.TRANSITION_LEFT_RIGHT_DISTANCE < 0f)
			{
				vector += new Vector2(-Room_EV.TRANSITION_LEFT_RIGHT_DISTANCE, 0f);
			}
			break;
		case RoomSide.Right:
			if (Room_EV.TRANSITION_LEFT_RIGHT_DISTANCE < 0f)
			{
				vector += new Vector2(Room_EV.TRANSITION_LEFT_RIGHT_DISTANCE, 0f);
			}
			break;
		}
		return vector;
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x000DE134 File Offset: 0x000DC334
	private void CreateLedge()
	{
		if (GameUtility.IsInGame && !WorldBuilder.DeactivateRoomGameObjects)
		{
			return;
		}
		if (this.m_disableLedge)
		{
			return;
		}
		if (this.Side == RoomSide.Left || this.Side == RoomSide.Right)
		{
			int num = 1;
			if (this.Side == RoomSide.Right)
			{
				num = -1;
			}
			Vector2 position = this.CenterPoint - new Vector2((float)num * 0.5f * 4f, 2.5f);
			this.CreateLedge(new Vector2(4f, 1f), position);
			bool flag = false;
			if (this.Room.AppearanceBiomeType == BiomeType.Stone)
			{
				if (this.Room.RoomType == RoomType.Transition && this.Side == RoomSide.Left)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				Vector2 position2 = this.CenterPoint - new Vector2((float)num * 0.5f * 4f, -3.5f);
				this.CreateLedge(new Vector2(4f, 1f), position2);
				return;
			}
		}
		else
		{
			if (this.Room.BiomeType == BiomeType.Tower && this.Room.AppearanceBiomeType == BiomeType.TowerExterior)
			{
				return;
			}
			int num2 = 1;
			if (this.Side == RoomSide.Bottom)
			{
				num2 = -1;
			}
			Vector2 position3 = this.CenterPoint + new Vector2(-3.5f, (float)num2 * 0.5f * 4f);
			this.CreateLedge(new Vector2(1f, 4f), position3);
			Vector2 position4 = this.CenterPoint + new Vector2(3.5f, (float)num2 * 0.5f * 4f);
			this.CreateLedge(new Vector2(1f, 4f), position4);
		}
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x000DE2D4 File Offset: 0x000DC4D4
	private void CreateLedge(Vector2 size, Vector2 position)
	{
		GameObject gameObject = new GameObject("Ledge", new Type[]
		{
			typeof(BoxCollider2D)
		});
		gameObject.layer = LayerMask.NameToLayer("Platform_CollidesWithAll");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.position = position;
		gameObject.GetComponent<BoxCollider2D>().size = size;
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x000DE33C File Offset: 0x000DC53C
	private void OnPlayerEnterExitTrigger(object sender, EventArgs eventArgs)
	{
		if (this.m_exitTriggerCoroutine != null)
		{
			Debug.LogFormat("<color=red>{0}: Coroutine was still running, but should not have been.</color>", new object[]
			{
				Time.frameCount
			});
			base.StopCoroutine(this.m_exitTriggerCoroutine);
			this.m_exitTriggerCoroutine = null;
		}
		this.m_exitTriggerCoroutine = base.StartCoroutine(this.OnPlayerStayExitTrigger());
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x0001CF52 File Offset: 0x0001B152
	private IEnumerator OnPlayerStayExitTrigger()
	{
		while (this.m_exitTrigger.IsPlayerInTrigger)
		{
			if (this.GetOnTriggerStayExitRoom())
			{
				RoomSide side = this.Side;
				Vector3 position = PlayerManager.GetPlayer().transform.position;
				Vector2 centerPoint = this.CenterPoint;
				if (side == RoomSide.Left || side == RoomSide.Right)
				{
					Vector2 centerPoint2 = this.CenterPoint;
				}
				if (this.m_doorArgs == null)
				{
					this.m_doorArgs = new DoorEventArgs(this);
				}
				this.m_playerEnterRelay.Dispatch(this, this.m_doorArgs);
				break;
			}
			yield return null;
		}
		this.m_exitTriggerCoroutine = null;
		yield break;
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x000DE394 File Offset: 0x000DC594
	private bool GetOnTriggerStayExitRoom()
	{
		GameObject player = PlayerManager.GetPlayer();
		PlayerController playerController = PlayerManager.GetPlayerController();
		bool result = false;
		if (this.Side != RoomSide.Bottom && (playerController.ConditionState == CharacterStates.CharacterConditions.Stunned || ((playerController.ConditionState == CharacterStates.CharacterConditions.DisableHorizontalMovement || playerController.CharacterMove.MovementDisabled) && !playerController.DisableDoorBlock)))
		{
			Vector3 localPosition = player.transform.localPosition;
			switch (this.Side)
			{
			case RoomSide.Top:
				if (player.transform.localPosition.y > this.CenterPoint.y && playerController.Velocity.y > 0f)
				{
					playerController.SetVelocityY(0f, false);
					localPosition.y = this.CenterPoint.y;
				}
				break;
			case RoomSide.Bottom:
				if (player.transform.localPosition.y < this.CenterPoint.y && playerController.Velocity.y < 0f)
				{
					playerController.SetVelocityY(0f, false);
					localPosition.y = this.CenterPoint.y;
				}
				break;
			case RoomSide.Left:
				if (player.transform.localPosition.x < this.CenterPoint.x && playerController.Velocity.x < 0f)
				{
					playerController.SetVelocityX(0f, false);
					localPosition.x = this.CenterPoint.x;
				}
				break;
			case RoomSide.Right:
				if (player.transform.localPosition.x > this.CenterPoint.x && playerController.Velocity.x > 0f)
				{
					playerController.SetVelocityX(0f, false);
					localPosition.x = this.CenterPoint.x;
				}
				break;
			}
			player.transform.localPosition = localPosition;
		}
		else
		{
			switch (this.Side)
			{
			case RoomSide.Top:
				if (player.transform.localPosition.y > this.CenterPoint.y + Room_EV.TRANSITION_TOP_DISTANCE && playerController.Velocity.y > 0f)
				{
					result = true;
				}
				break;
			case RoomSide.Bottom:
				if (player.transform.localPosition.y < this.CenterPoint.y + Room_EV.TRANSITION_BOTTOM_DISTANCE && playerController.Velocity.y < 0f)
				{
					result = true;
				}
				break;
			case RoomSide.Left:
				if (player.transform.localPosition.x < this.CenterPoint.x - Room_EV.TRANSITION_LEFT_RIGHT_DISTANCE && playerController.Velocity.x < 0f)
				{
					result = true;
				}
				break;
			case RoomSide.Right:
				if (player.transform.localPosition.x > this.CenterPoint.x + Room_EV.TRANSITION_LEFT_RIGHT_DISTANCE && playerController.Velocity.x > 0f)
				{
					result = true;
				}
				break;
			}
		}
		return result;
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x0001CF61 File Offset: 0x0001B161
	public void InitialiseInEditor(RoomSide side, int number)
	{
		this.Side = side;
		this.Number = number;
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x0001CF71 File Offset: 0x0001B171
	private void OnDestroy()
	{
		this.m_doorDestroyedRelay.Dispatch(this, EventArgs.Empty);
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x000DE688 File Offset: 0x000DC888
	public void Close(bool replaceWithWall = true)
	{
		if (replaceWithWall)
		{
			Ferr2DT_PathTerrain ferr2DT_PathTerrain = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(RoomPrefabLibrary.DoorSealPrefab);
			this.Room.TerrainManager.AddPlatform(ferr2DT_PathTerrain);
			ferr2DT_PathTerrain.transform.position = base.transform.position;
			ferr2DT_PathTerrain.transform.SetParent(base.transform.parent);
			ferr2DT_PathTerrain.name = "***Door Seal***";
			ferr2DT_PathTerrain.ClearPoints();
			foreach (Vector2 aPt in this.Ferr2D.PathData.GetPoints(0))
			{
				ferr2DT_PathTerrain.AddPoint(aPt, -1, PointType.Sharp);
			}
			ferr2DT_PathTerrain.Build(true);
		}
		this.RemoveRoomEventHandlers();
		if (this.OneWay != null)
		{
			this.Room.TerrainManager.RemoveOneWay(this.OneWay);
			UnityEngine.Object.Destroy(this.OneWay.gameObject);
		}
		this.m_closeRelay.Dispatch(this, new DoorEventArgs(this));
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x000DE7A8 File Offset: 0x000DC9A8
	private void AddRoomEventHandlers()
	{
		this.m_room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
		this.m_room.PlayerExitRelay.AddListener(this.m_onPlayerExitRoom, false);
		this.m_room.RoomDestroyedRelay.AddListener(this.m_onRoomDestroyed, false);
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x000DE800 File Offset: 0x000DCA00
	private void RemoveRoomEventHandlers()
	{
		this.m_room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
		this.m_room.PlayerExitRelay.RemoveListener(this.m_onPlayerExitRoom);
		this.m_room.RoomDestroyedRelay.RemoveListener(this.m_onRoomDestroyed);
	}

	// Token: 0x060034CF RID: 13519 RVA: 0x0001CF84 File Offset: 0x0001B184
	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying)
		{
			Gizmos.DrawSphere(this.CenterPoint, 0.5f);
		}
	}

	// Token: 0x060034D0 RID: 13520 RVA: 0x000DE854 File Offset: 0x000DCA54
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Door"))
		{
			Door component = collision.GetComponent<Door>();
			if (component != null && component.Room != this.Room && component.Side == RoomUtility.GetOppositeSide(this.Side) && !component.DisabledFromLevelEditor)
			{
				this.ConnectedDoor = component;
				if (this.m_doorConnectEventArgs == null)
				{
					this.m_doorConnectEventArgs = new DoorConnectEventArgs(this, component);
				}
				else
				{
					this.m_doorConnectEventArgs.Initialize(this, component);
				}
				this.m_doorConnectRelay.Dispatch(this, this.m_doorConnectEventArgs);
				this.ConnectedDoor.DoorDestroyedRelay.AddListener(this.m_onConnectedDoorDestroyed, false);
				return;
			}
			if (component == null)
			{
				Debug.LogFormat("{0}: Unable to find Door Component on ({1})", new object[]
				{
					this,
					collision.name
				});
			}
		}
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x000DE930 File Offset: 0x000DCB30
	private void OnConnectedDoorDestroyed(object sender, EventArgs eventArgs)
	{
		if (this.ConnectedDoor)
		{
			if (this.m_doorConnectEventArgs == null)
			{
				this.m_doorConnectEventArgs = new DoorConnectEventArgs(this, this.ConnectedDoor);
			}
			else
			{
				this.m_doorConnectEventArgs.Initialize(this, this.ConnectedDoor);
			}
			this.m_doorDisconnectRelay.Dispatch(this, this.m_doorConnectEventArgs);
			this.ConnectedDoor.DoorDestroyedRelay.RemoveListener(this.m_onConnectedDoorDestroyed);
			this.ConnectedDoor = null;
		}
	}

	// Token: 0x04002A86 RID: 10886
	private const int LEDGE_LENGTH = 4;

	// Token: 0x04002A87 RID: 10887
	private const int LEDGE_THICKNESS = 1;

	// Token: 0x04002A88 RID: 10888
	private Ferr2DT_PathTerrain m_terrain;

	// Token: 0x04002A89 RID: 10889
	private BaseRoom m_room;

	// Token: 0x04002A8A RID: 10890
	private Door m_connectedDoor;

	// Token: 0x04002A8B RID: 10891
	private PolygonCollider2D m_collider;

	// Token: 0x04002A8C RID: 10892
	private Vector2 m_centerPoint;

	// Token: 0x04002A8D RID: 10893
	private PlayerTrigger m_exitTrigger;

	// Token: 0x04002A8E RID: 10894
	private Coroutine m_exitTriggerCoroutine;

	// Token: 0x04002A8F RID: 10895
	private Vector2 m_roomCoordinates = new Vector2(-10000f, -10000f);

	// Token: 0x04002A90 RID: 10896
	private bool m_hasExitTriggerBeenCreated;

	// Token: 0x04002A91 RID: 10897
	private BiomeType m_transitionsToBiome;

	// Token: 0x04002A92 RID: 10898
	private Relay<object, DoorEventArgs> m_playerEnterRelay = new Relay<object, DoorEventArgs>();

	// Token: 0x04002A93 RID: 10899
	private Relay<object, DoorEventArgs> m_closeRelay = new Relay<object, DoorEventArgs>();

	// Token: 0x04002A94 RID: 10900
	private Relay<object, DoorConnectEventArgs> m_doorConnectRelay = new Relay<object, DoorConnectEventArgs>();

	// Token: 0x04002A95 RID: 10901
	private Relay<object, DoorConnectEventArgs> m_doorDisconnectRelay = new Relay<object, DoorConnectEventArgs>();

	// Token: 0x04002A96 RID: 10902
	private Relay<object, EventArgs> m_doorDestroyedRelay = new Relay<object, EventArgs>();

	// Token: 0x04002A97 RID: 10903
	private DoorConnectEventArgs m_doorConnectEventArgs;

	// Token: 0x04002A98 RID: 10904
	private DoorEventArgs m_doorArgs;

	// Token: 0x04002A99 RID: 10905
	private Action<object, EventArgs> m_onConnectedDoorDestroyed;

	// Token: 0x04002A9A RID: 10906
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002A9B RID: 10907
	private Action<object, RoomViaDoorEventArgs> m_onPlayerExitRoom;

	// Token: 0x04002A9C RID: 10908
	private Action<object, EventArgs> m_onRoomDestroyed;

	// Token: 0x04002A9D RID: 10909
	[SerializeField]
	private RoomSide m_side = RoomSide.None;

	// Token: 0x04002A9E RID: 10910
	[SerializeField]
	private int m_index = -1;

	// Token: 0x04002A9F RID: 10911
	[SerializeField]
	private bool m_disableDoor;

	// Token: 0x04002AA0 RID: 10912
	[SerializeField]
	private bool m_disableLedge;
}

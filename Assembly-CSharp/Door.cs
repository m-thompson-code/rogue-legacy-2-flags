using System;
using System.Collections;
using System.Collections.Generic;
using Ferr;
using MoreMountains.CorgiEngine;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000401 RID: 1025
public class Door : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17000F48 RID: 3912
	// (get) Token: 0x06002618 RID: 9752 RVA: 0x0007DC04 File Offset: 0x0007BE04
	public IRelayLink<object, DoorEventArgs> PlayerEnterRelay
	{
		get
		{
			return this.m_playerEnterRelay.link;
		}
	}

	// Token: 0x17000F49 RID: 3913
	// (get) Token: 0x06002619 RID: 9753 RVA: 0x0007DC11 File Offset: 0x0007BE11
	public IRelayLink<object, DoorEventArgs> CloseRelay
	{
		get
		{
			return this.m_closeRelay.link;
		}
	}

	// Token: 0x17000F4A RID: 3914
	// (get) Token: 0x0600261A RID: 9754 RVA: 0x0007DC1E File Offset: 0x0007BE1E
	public IRelayLink<object, DoorConnectEventArgs> DoorConnectRelay
	{
		get
		{
			return this.m_doorConnectRelay.link;
		}
	}

	// Token: 0x17000F4B RID: 3915
	// (get) Token: 0x0600261B RID: 9755 RVA: 0x0007DC2B File Offset: 0x0007BE2B
	public IRelayLink<object, DoorConnectEventArgs> DoorDisconnectRelay
	{
		get
		{
			return this.m_doorDisconnectRelay.link;
		}
	}

	// Token: 0x17000F4C RID: 3916
	// (get) Token: 0x0600261C RID: 9756 RVA: 0x0007DC38 File Offset: 0x0007BE38
	public IRelayLink<object, EventArgs> DoorDestroyedRelay
	{
		get
		{
			return this.m_doorDestroyedRelay.link;
		}
	}

	// Token: 0x17000F4D RID: 3917
	// (get) Token: 0x0600261D RID: 9757 RVA: 0x0007DC45 File Offset: 0x0007BE45
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x0600261E RID: 9758 RVA: 0x0007DC50 File Offset: 0x0007BE50
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

	// Token: 0x17000F4E RID: 3918
	// (get) Token: 0x0600261F RID: 9759 RVA: 0x0007DD1C File Offset: 0x0007BF1C
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

	// Token: 0x17000F4F RID: 3919
	// (get) Token: 0x06002620 RID: 9760 RVA: 0x0007DD70 File Offset: 0x0007BF70
	// (set) Token: 0x06002621 RID: 9761 RVA: 0x0007DD78 File Offset: 0x0007BF78
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

	// Token: 0x17000F50 RID: 3920
	// (get) Token: 0x06002622 RID: 9762 RVA: 0x0007DD81 File Offset: 0x0007BF81
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

	// Token: 0x17000F51 RID: 3921
	// (get) Token: 0x06002623 RID: 9763 RVA: 0x0007DD9E File Offset: 0x0007BF9E
	// (set) Token: 0x06002624 RID: 9764 RVA: 0x0007DDA6 File Offset: 0x0007BFA6
	public GridPointManager GridPointManager { get; private set; }

	// Token: 0x17000F52 RID: 3922
	// (get) Token: 0x06002625 RID: 9765 RVA: 0x0007DDAF File Offset: 0x0007BFAF
	// (set) Token: 0x06002626 RID: 9766 RVA: 0x0007DDB7 File Offset: 0x0007BFB7
	public Vector2Int GridPointCoordinates { get; private set; }

	// Token: 0x17000F53 RID: 3923
	// (get) Token: 0x06002627 RID: 9767 RVA: 0x0007DDC0 File Offset: 0x0007BFC0
	// (set) Token: 0x06002628 RID: 9768 RVA: 0x0007DDC8 File Offset: 0x0007BFC8
	public Vector2 Coordinates { get; private set; }

	// Token: 0x17000F54 RID: 3924
	// (get) Token: 0x06002629 RID: 9769 RVA: 0x0007DDD1 File Offset: 0x0007BFD1
	public bool DisabledFromLevelEditor
	{
		get
		{
			return this.m_disableDoor;
		}
	}

	// Token: 0x17000F55 RID: 3925
	// (get) Token: 0x0600262A RID: 9770 RVA: 0x0007DDD9 File Offset: 0x0007BFD9
	// (set) Token: 0x0600262B RID: 9771 RVA: 0x0007DDE1 File Offset: 0x0007BFE1
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

	// Token: 0x17000F56 RID: 3926
	// (get) Token: 0x0600262C RID: 9772 RVA: 0x0007DDEA File Offset: 0x0007BFEA
	// (set) Token: 0x0600262D RID: 9773 RVA: 0x0007DDF2 File Offset: 0x0007BFF2
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

	// Token: 0x17000F57 RID: 3927
	// (get) Token: 0x0600262E RID: 9774 RVA: 0x0007DDFB File Offset: 0x0007BFFB
	// (set) Token: 0x0600262F RID: 9775 RVA: 0x0007DE03 File Offset: 0x0007C003
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

	// Token: 0x17000F58 RID: 3928
	// (get) Token: 0x06002630 RID: 9776 RVA: 0x0007DE0C File Offset: 0x0007C00C
	// (set) Token: 0x06002631 RID: 9777 RVA: 0x0007DE14 File Offset: 0x0007C014
	public GameObject OneWay { get; private set; }

	// Token: 0x06002632 RID: 9778 RVA: 0x0007DE1D File Offset: 0x0007C01D
	public void SetOneWay(GameObject oneWay)
	{
		this.OneWay = oneWay;
	}

	// Token: 0x17000F59 RID: 3929
	// (get) Token: 0x06002633 RID: 9779 RVA: 0x0007DE26 File Offset: 0x0007C026
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

	// Token: 0x17000F5A RID: 3930
	// (get) Token: 0x06002634 RID: 9780 RVA: 0x0007DE39 File Offset: 0x0007C039
	public bool IsBiomeTransitionPoint
	{
		get
		{
			return this.TransitionsToBiome > BiomeType.None;
		}
	}

	// Token: 0x17000F5B RID: 3931
	// (get) Token: 0x06002635 RID: 9781 RVA: 0x0007DE44 File Offset: 0x0007C044
	// (set) Token: 0x06002636 RID: 9782 RVA: 0x0007DE4C File Offset: 0x0007C04C
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

	// Token: 0x06002637 RID: 9783 RVA: 0x0007DE55 File Offset: 0x0007C055
	public void SetIsBiomeTransitionPoint(BiomeType biome)
	{
		this.TransitionsToBiome = biome;
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x0007DE60 File Offset: 0x0007C060
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerExitRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExitRoom);
		this.m_onRoomDestroyed = new Action<object, EventArgs>(this.OnRoomDestroyed);
		this.m_onConnectedDoorDestroyed = new Action<object, EventArgs>(this.OnConnectedDoorDestroyed);
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x0007DEB8 File Offset: 0x0007C0B8
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

	// Token: 0x0600263A RID: 9786 RVA: 0x0007DF1C File Offset: 0x0007C11C
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

	// Token: 0x0600263B RID: 9787 RVA: 0x0007DFAC File Offset: 0x0007C1AC
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

	// Token: 0x0600263C RID: 9788 RVA: 0x0007E03B File Offset: 0x0007C23B
	private void OnRoomDestroyed(object sender, EventArgs eventArgs)
	{
		this.SetRoom(null);
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x0007E044 File Offset: 0x0007C244
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

	// Token: 0x0600263E RID: 9790 RVA: 0x0007E120 File Offset: 0x0007C320
	private Vector2 GetExitColliderSize()
	{
		return new Vector2(32f, 18f);
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x0007E134 File Offset: 0x0007C334
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

	// Token: 0x06002640 RID: 9792 RVA: 0x0007E284 File Offset: 0x0007C484
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

	// Token: 0x06002641 RID: 9793 RVA: 0x0007E424 File Offset: 0x0007C624
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

	// Token: 0x06002642 RID: 9794 RVA: 0x0007E48C File Offset: 0x0007C68C
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

	// Token: 0x06002643 RID: 9795 RVA: 0x0007E4E3 File Offset: 0x0007C6E3
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

	// Token: 0x06002644 RID: 9796 RVA: 0x0007E4F4 File Offset: 0x0007C6F4
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

	// Token: 0x06002645 RID: 9797 RVA: 0x0007E7E8 File Offset: 0x0007C9E8
	public void InitialiseInEditor(RoomSide side, int number)
	{
		this.Side = side;
		this.Number = number;
	}

	// Token: 0x06002646 RID: 9798 RVA: 0x0007E7F8 File Offset: 0x0007C9F8
	private void OnDestroy()
	{
		this.m_doorDestroyedRelay.Dispatch(this, EventArgs.Empty);
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x0007E80C File Offset: 0x0007CA0C
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

	// Token: 0x06002648 RID: 9800 RVA: 0x0007E92C File Offset: 0x0007CB2C
	private void AddRoomEventHandlers()
	{
		this.m_room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
		this.m_room.PlayerExitRelay.AddListener(this.m_onPlayerExitRoom, false);
		this.m_room.RoomDestroyedRelay.AddListener(this.m_onRoomDestroyed, false);
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x0007E984 File Offset: 0x0007CB84
	private void RemoveRoomEventHandlers()
	{
		this.m_room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
		this.m_room.PlayerExitRelay.RemoveListener(this.m_onPlayerExitRoom);
		this.m_room.RoomDestroyedRelay.RemoveListener(this.m_onRoomDestroyed);
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x0007E9D6 File Offset: 0x0007CBD6
	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying)
		{
			Gizmos.DrawSphere(this.CenterPoint, 0.5f);
		}
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x0007E9F4 File Offset: 0x0007CBF4
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

	// Token: 0x0600264C RID: 9804 RVA: 0x0007EAD0 File Offset: 0x0007CCD0
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

	// Token: 0x04001FCF RID: 8143
	private const int LEDGE_LENGTH = 4;

	// Token: 0x04001FD0 RID: 8144
	private const int LEDGE_THICKNESS = 1;

	// Token: 0x04001FD1 RID: 8145
	private Ferr2DT_PathTerrain m_terrain;

	// Token: 0x04001FD2 RID: 8146
	private BaseRoom m_room;

	// Token: 0x04001FD3 RID: 8147
	private Door m_connectedDoor;

	// Token: 0x04001FD4 RID: 8148
	private PolygonCollider2D m_collider;

	// Token: 0x04001FD5 RID: 8149
	private Vector2 m_centerPoint;

	// Token: 0x04001FD6 RID: 8150
	private PlayerTrigger m_exitTrigger;

	// Token: 0x04001FD7 RID: 8151
	private Coroutine m_exitTriggerCoroutine;

	// Token: 0x04001FD8 RID: 8152
	private Vector2 m_roomCoordinates = new Vector2(-10000f, -10000f);

	// Token: 0x04001FD9 RID: 8153
	private bool m_hasExitTriggerBeenCreated;

	// Token: 0x04001FDA RID: 8154
	private BiomeType m_transitionsToBiome;

	// Token: 0x04001FDB RID: 8155
	private Relay<object, DoorEventArgs> m_playerEnterRelay = new Relay<object, DoorEventArgs>();

	// Token: 0x04001FDC RID: 8156
	private Relay<object, DoorEventArgs> m_closeRelay = new Relay<object, DoorEventArgs>();

	// Token: 0x04001FDD RID: 8157
	private Relay<object, DoorConnectEventArgs> m_doorConnectRelay = new Relay<object, DoorConnectEventArgs>();

	// Token: 0x04001FDE RID: 8158
	private Relay<object, DoorConnectEventArgs> m_doorDisconnectRelay = new Relay<object, DoorConnectEventArgs>();

	// Token: 0x04001FDF RID: 8159
	private Relay<object, EventArgs> m_doorDestroyedRelay = new Relay<object, EventArgs>();

	// Token: 0x04001FE0 RID: 8160
	private DoorConnectEventArgs m_doorConnectEventArgs;

	// Token: 0x04001FE1 RID: 8161
	private DoorEventArgs m_doorArgs;

	// Token: 0x04001FE2 RID: 8162
	private Action<object, EventArgs> m_onConnectedDoorDestroyed;

	// Token: 0x04001FE3 RID: 8163
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001FE4 RID: 8164
	private Action<object, RoomViaDoorEventArgs> m_onPlayerExitRoom;

	// Token: 0x04001FE5 RID: 8165
	private Action<object, EventArgs> m_onRoomDestroyed;

	// Token: 0x04001FE6 RID: 8166
	[SerializeField]
	private RoomSide m_side = RoomSide.None;

	// Token: 0x04001FE7 RID: 8167
	[SerializeField]
	private int m_index = -1;

	// Token: 0x04001FE8 RID: 8168
	[SerializeField]
	private bool m_disableDoor;

	// Token: 0x04001FE9 RID: 8169
	[SerializeField]
	private bool m_disableLedge;
}

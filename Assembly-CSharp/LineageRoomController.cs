using System;
using System.Collections;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class LineageRoomController : MonoBehaviour
{
	// Token: 0x17001053 RID: 4179
	// (get) Token: 0x060029FE RID: 10750 RVA: 0x0008AD1A File Offset: 0x00088F1A
	public static int NumRooms
	{
		get
		{
			return LineageRoomController.NumConnectorRooms + 2;
		}
	}

	// Token: 0x17001054 RID: 4180
	// (get) Token: 0x060029FF RID: 10751 RVA: 0x0008AD24 File Offset: 0x00088F24
	public bool InstantiatingFromLineageWindow
	{
		get
		{
			return SceneLoadingUtility.ActiveScene.name == SceneLoadingUtility.GetSceneName(SceneID.Lineage);
		}
	}

	// Token: 0x17001055 RID: 4181
	// (get) Token: 0x06002A00 RID: 10752 RVA: 0x0008AD4A File Offset: 0x00088F4A
	public Room EndingRoom
	{
		get
		{
			return this.m_endingRoom;
		}
	}

	// Token: 0x17001056 RID: 4182
	// (get) Token: 0x06002A01 RID: 10753 RVA: 0x0008AD52 File Offset: 0x00088F52
	public GameObject LineageModelPositionObject
	{
		get
		{
			return this.m_lineageModelPositionObject;
		}
	}

	// Token: 0x17001057 RID: 4183
	// (get) Token: 0x06002A02 RID: 10754 RVA: 0x0008AD5A File Offset: 0x00088F5A
	public LineagePortrait LeftPortrait
	{
		get
		{
			return this.m_leftPortrait;
		}
	}

	// Token: 0x17001058 RID: 4184
	// (get) Token: 0x06002A03 RID: 10755 RVA: 0x0008AD62 File Offset: 0x00088F62
	public LineagePortrait RightPortrait
	{
		get
		{
			return this.m_rightPortrait;
		}
	}

	// Token: 0x17001059 RID: 4185
	// (get) Token: 0x06002A04 RID: 10756 RVA: 0x0008AD6A File Offset: 0x00088F6A
	public LineagePortrait CentrePortrait
	{
		get
		{
			return this.m_centrePortrait;
		}
	}

	// Token: 0x1700105A RID: 4186
	// (get) Token: 0x06002A05 RID: 10757 RVA: 0x0008AD72 File Offset: 0x00088F72
	// (set) Token: 0x06002A06 RID: 10758 RVA: 0x0008AD7A File Offset: 0x00088F7A
	public bool IsInitialized { get; private set; }

	// Token: 0x06002A07 RID: 10759 RVA: 0x0008AD83 File Offset: 0x00088F83
	public void EnableSpotlight(bool turnOn)
	{
		if (this.m_roomType == LineageRoomController.LineageRoomType.Ending)
		{
			this.m_animator.SetBool("SpotlightOn", turnOn);
			return;
		}
		if (this.m_endingRoom)
		{
			this.m_endingRoom.GetComponent<LineageRoomController>().EnableSpotlight(turnOn);
		}
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x0008ADC0 File Offset: 0x00088FC0
	private void Awake()
	{
		this.m_room = base.GetComponent<Room>();
		this.m_animator = base.GetComponent<Animator>();
		if (this.m_roomTrigger && this.InstantiatingFromLineageWindow)
		{
			this.m_roomTrigger.PlayerEnter += this.OnEnterRoom;
		}
		if (!LineageRoomController.m_sky && this.InstantiatingFromLineageWindow)
		{
			Sky sky = BiomeCreatorTools.CreateSkyInstance(BiomeArtDataLibrary.GetArtData(BiomeType.Lineage));
			sky.RoomList.Add(this.m_room);
			LineageRoomController.m_sky = sky.gameObject;
		}
		if (this.m_roomType == LineageRoomController.LineageRoomType.Starting)
		{
			this.m_onBiomeCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnBiomeCreationComplete);
			this.Initialize();
		}
		if (this.m_roomType == LineageRoomController.LineageRoomType.Ending)
		{
			if (this.InstantiatingFromLineageWindow)
			{
				this.m_finalBossTunnelObject.SetActive(false);
			}
			this.m_onHeirSelectionChanged = new Action<MonoBehaviour, EventArgs>(this.OnHeirSelectionChanged);
			Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.Lineage_SelectedNewHeir, this.m_onHeirSelectionChanged);
		}
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x0008AEAB File Offset: 0x000890AB
	private void OnEnable()
	{
		if ((this.m_roomType == LineageRoomController.LineageRoomType.Starting || this.m_roomType == LineageRoomController.LineageRoomType.Ending) && !this.InstantiatingFromLineageWindow && WorldBuilder.State == BiomeBuildStateID.Complete)
		{
			base.StartCoroutine(this.OnEnterManorCoroutine());
		}
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x0008AEDE File Offset: 0x000890DE
	private IEnumerator OnEnterManorCoroutine()
	{
		if (this.m_roomType == LineageRoomController.LineageRoomType.Starting)
		{
			this.SetupRoom(0);
			int max = Mathf.Max(1, LineageRoomController.NumConnectorRooms);
			int num = Mathf.Clamp(LineageRoomController.NumConnectorRooms, 0, max);
			for (int i = 0; i < num; i++)
			{
				LineageRoomController.m_connectingRoomArray[i].GetComponent<LineageRoomController>().SetupRoom(i + 1);
			}
			this.m_endingRoom.GetComponent<LineageRoomController>().SetupRoom(LineageRoomController.NumConnectorRooms - 1);
		}
		while (SceneLoader_RL.IsRunningTransitionWithLogic)
		{
			yield return null;
		}
		if (this.m_roomType == LineageRoomController.LineageRoomType.Ending)
		{
			AmbientSoundController.Instance.PlayManorAmbientAudio();
			ForceNowEnteringEventArgs eventArgs = new ForceNowEnteringEventArgs("LOC_ID_LOCATION_TITLE_HOME_1", 7);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.ForceNowEntering, this, eventArgs);
		}
		yield break;
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x0008AEED File Offset: 0x000890ED
	private void Start()
	{
		if (!LineageRoomController.m_hasBiomeEnterEventBroadcast)
		{
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.BiomeEnter, this, new BiomeEventArgs(BiomeType.Lineage));
			LineageRoomController.m_hasBiomeEnterEventBroadcast = true;
		}
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x0008AF0C File Offset: 0x0008910C
	private void OnDestroy()
	{
		if (this.m_roomTrigger && this.InstantiatingFromLineageWindow)
		{
			this.m_roomTrigger.PlayerEnter -= this.OnEnterRoom;
		}
		if (this.m_roomType == LineageRoomController.LineageRoomType.Starting)
		{
			if (LineageRoomController.m_connectingRoomArray != null)
			{
				Array.Clear(LineageRoomController.m_connectingRoomArray, 0, LineageRoomController.m_connectingRoomArray.Length);
			}
			LineageRoomController.m_connectingRoomArray = null;
			if (LineageRoomController.m_connectingRoomControllerArray != null)
			{
				Array.Clear(LineageRoomController.m_connectingRoomControllerArray, 0, LineageRoomController.m_connectingRoomControllerArray.Length);
			}
			LineageRoomController.m_connectingRoomControllerArray = null;
			LineageRoomController.m_currentRoom = null;
			LineageRoomController.m_roomIndex = 0;
			LineageRoomController.m_startingRoomPosition = Vector2.zero;
			LineageRoomController.NumConnectorRooms = 0;
			LineageRoomController.NumPortraits = 0;
		}
		if (this.m_roomType == LineageRoomController.LineageRoomType.Ending)
		{
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.Lineage_SelectedNewHeir, this.m_onHeirSelectionChanged);
		}
		LineageRoomController.m_hasBiomeEnterEventBroadcast = false;
		if (LineageRoomController.m_sky)
		{
			UnityEngine.Object.Destroy(LineageRoomController.m_sky);
			LineageRoomController.m_sky = null;
		}
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x0008AFF0 File Offset: 0x000891F0
	private void Initialize()
	{
		if (SaveManager.LineageSaveData.LineageHeirList.Count <= 0)
		{
			throw new Exception("No portraits found in save data. Lineage Room Controller currently cannot handle this scenario.");
		}
		LineageRoomController.NumPortraits = SaveManager.LineageSaveData.LineageHeirList.Count;
		if (!this.InstantiatingFromLineageWindow)
		{
			LineageRoomController.NumPortraits = Mathf.Min(LineageRoomController.NumPortraits, 20);
		}
		int num = LineageRoomController.NumPortraits - 4;
		if (num > 0)
		{
			num = Mathf.CeilToInt((float)num / 3f);
			LineageRoomController.NumConnectorRooms = num;
		}
		this.m_leftPortrait.gameObject.SetActive(false);
		this.m_centrePortrait.gameObject.SetActive(false);
		this.m_rightPortrait.gameObject.SetActive(false);
		this.SetupRoom(0);
		int num2 = this.InstantiatingFromLineageWindow ? 3 : Mathf.Max(1, num);
		LineageRoomController.m_connectingRoomArray = new Room[num2];
		LineageRoomController.m_connectingRoomControllerArray = new LineageRoomController[num2];
		LineageRoomController.m_startingRoomPosition = this.m_room.transform.position;
		int num3 = Mathf.Clamp(LineageRoomController.NumConnectorRooms, 0, num2);
		for (int i = 0; i < num3; i++)
		{
			Room room = UnityEngine.Object.Instantiate<Room>(this.m_connectorRoomPrefab, this.InstantiatingFromLineageWindow ? this.m_room.transform.parent : this.m_room.transform);
			if (this.InstantiatingFromLineageWindow)
			{
				this.PositionRoomAtIndex(room.gameObject, LineageRoomController.NumConnectorRooms - i);
			}
			else
			{
				this.PositionRoomAtIndex(room.gameObject, i + 1);
			}
			room.Initialise(BiomeType.Lineage, RoomType.Standard, 0);
			RoomUtility.BuildAllFerr2DTerrains(room);
			LineageRoomController.m_connectingRoomArray[i] = room;
			LineageRoomController component = room.GetComponent<LineageRoomController>();
			if (this.InstantiatingFromLineageWindow)
			{
				component.SetupRoom(LineageRoomController.NumConnectorRooms - i);
			}
			LineageRoomController.m_connectingRoomControllerArray[i] = component;
		}
		this.m_endingRoom = UnityEngine.Object.Instantiate<Room>(this.m_endingRoomPrefab, this.InstantiatingFromLineageWindow ? this.m_room.transform.parent : this.m_room.transform);
		this.m_endingRoom.Initialise(BiomeType.Lineage, RoomType.Standard, 0);
		RoomUtility.BuildAllFerr2DTerrains(this.m_endingRoom);
		this.PositionRoomAtIndex(this.m_endingRoom.gameObject, LineageRoomController.NumConnectorRooms + 1);
		if (this.InstantiatingFromLineageWindow)
		{
			this.m_endingRoom.GetComponent<LineageRoomController>().SetupRoom(LineageRoomController.NumConnectorRooms - 1);
		}
		if (this.InstantiatingFromLineageWindow)
		{
			LineageRoomController.m_sky.transform.SetPositionX(this.m_endingRoom.gameObject.transform.position.x);
		}
		if (!this.m_room.IsInitialised)
		{
			this.m_room.Initialise(BiomeType.Lineage, RoomType.Standard, -1);
			RoomUtility.BuildAllFerr2DTerrains(this.m_room);
		}
		Vector2[] array = new Vector2[]
		{
			new Vector2(this.m_room.Bounds.min.x, this.m_room.Bounds.max.y),
			new Vector2(this.m_room.Bounds.min.x, this.m_room.Bounds.min.y),
			new Vector2(this.m_room.Bounds.max.x + (float)(this.m_room.UnitDimensions.x * (LineageRoomController.NumConnectorRooms + 1)), this.m_room.Bounds.min.y),
			new Vector2(this.m_room.Bounds.max.x + (float)(this.m_room.UnitDimensions.x * (LineageRoomController.NumConnectorRooms + 1)), this.m_room.Bounds.max.y)
		};
		for (int j = 0; j < array.Length; j++)
		{
			Vector2[] array2 = array;
			int num4 = j;
			array2[num4].x = array2[num4].x - this.m_room.transform.position.x;
			Vector2[] array3 = array;
			int num5 = j;
			array3[num5].y = array3[num5].y - this.m_room.transform.position.y;
		}
		this.m_room.GetComponent<PolygonCollider2D>().SetPath(0, array);
		if (!this.InstantiatingFromLineageWindow)
		{
			this.m_room.ResetBounds();
			if (GameUtility.IsInLevelEditor)
			{
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onBiomeCreationComplete);
			}
			else
			{
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeCreationComplete, this.m_onBiomeCreationComplete);
			}
		}
		LineageRoomController.ResetRoomIndex(this, this.InstantiatingFromLineageWindow);
		this.IsInitialized = true;
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x0008B47C File Offset: 0x0008967C
	public static void ResetRoomIndex(LineageRoomController startingRoom, bool instantiatingFromLineageWindow)
	{
		if (instantiatingFromLineageWindow)
		{
			LineageRoomController.m_currentRoom = startingRoom.EndingRoom;
			LineageRoomController.m_roomIndex = LineageRoomController.NumRooms - 1;
			return;
		}
		LineageRoomController.m_currentRoom = startingRoom.m_room;
		LineageRoomController.m_roomIndex = 0;
	}

	// Token: 0x06002A0F RID: 10767 RVA: 0x0008B4AC File Offset: 0x000896AC
	private void OnHeirSelectionChanged(MonoBehaviour sender, EventArgs args)
	{
		if (this.m_roomType == LineageRoomController.LineageRoomType.Ending)
		{
			LineageWindowController lineageWindowController = sender as LineageWindowController;
			if (lineageWindowController.NumberOfSuccessors <= 1)
			{
				this.m_animator.SetFloat("CharacterSelection", 0.5f);
				return;
			}
			float desiredSpotlightPos = (float)lineageWindowController.CurrentSelectedCharacterIndex / (float)(lineageWindowController.NumberOfSuccessors - 1);
			if (this.m_spotlightTweenCoroutine != null)
			{
				base.StopCoroutine(this.m_spotlightTweenCoroutine);
			}
			this.m_spotlightTweenCoroutine = base.StartCoroutine(this.SpotlightTweenCoroutine(desiredSpotlightPos));
		}
	}

	// Token: 0x06002A10 RID: 10768 RVA: 0x0008B522 File Offset: 0x00089722
	private IEnumerator SpotlightTweenCoroutine(float desiredSpotlightPos)
	{
		float tweenSpeed = 10f * Time.unscaledDeltaTime;
		bool moveLeft = desiredSpotlightPos < this.m_currentSpotlightPos;
		while ((this.m_currentSpotlightPos < desiredSpotlightPos && !moveLeft) || (this.m_currentSpotlightPos > desiredSpotlightPos && moveLeft))
		{
			if (!moveLeft)
			{
				this.m_currentSpotlightPos += tweenSpeed;
			}
			else
			{
				this.m_currentSpotlightPos -= tweenSpeed;
			}
			this.m_animator.SetFloat("CharacterSelection", this.m_currentSpotlightPos);
			if ((this.m_currentSpotlightPos < desiredSpotlightPos && !moveLeft) || (this.m_currentSpotlightPos > desiredSpotlightPos && moveLeft))
			{
				yield return null;
			}
		}
		this.m_animator.SetFloat("CharacterSelection", desiredSpotlightPos);
		yield break;
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x0008B538 File Offset: 0x00089738
	private void PositionRoomAtIndex(GameObject room, int index)
	{
		Vector3 startingRoomPosition = LineageRoomController.m_startingRoomPosition;
		startingRoomPosition.x = LineageRoomController.m_startingRoomPosition.x + (float)(index * this.m_room.UnitDimensions.x);
		room.transform.position = startingRoomPosition;
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x0008B580 File Offset: 0x00089780
	private void OnEnterRoom(object sender, EventArgs args)
	{
		if (LineageRoomController.m_currentRoom != this.m_room)
		{
			bool incremented = false;
			if (this.m_room.transform.position.x > LineageRoomController.m_currentRoom.transform.position.x)
			{
				LineageRoomController.m_roomIndex++;
				incremented = true;
			}
			else
			{
				LineageRoomController.m_roomIndex--;
			}
			LineageRoomController.m_currentRoom = this.m_room;
			if (LineageRoomController.m_roomIndex > 1 && LineageRoomController.m_roomIndex < LineageRoomController.NumConnectorRooms)
			{
				this.ShiftRooms(incremented);
			}
		}
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x0008B610 File Offset: 0x00089810
	private void ShiftRooms(bool incremented)
	{
		int i = 0;
		while (i < LineageRoomController.m_connectingRoomArray.Length)
		{
			Room room = LineageRoomController.m_connectingRoomArray[i];
			if (room != LineageRoomController.m_currentRoom && Mathf.Abs(LineageRoomController.m_currentRoom.transform.position.x - room.transform.position.x) > this.m_room.Bounds.size.x)
			{
				LineageRoomController lineageRoomController = LineageRoomController.m_connectingRoomControllerArray[i];
				if (incremented)
				{
					this.PositionRoomAtIndex(room.gameObject, LineageRoomController.m_roomIndex + 1);
					lineageRoomController.SetupRoom(LineageRoomController.m_roomIndex + 1);
					return;
				}
				this.PositionRoomAtIndex(room.gameObject, LineageRoomController.m_roomIndex - 1);
				lineageRoomController.SetupRoom(LineageRoomController.m_roomIndex - 1);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x0008B6E0 File Offset: 0x000898E0
	public void SetupRoom(int roomIndex)
	{
		if (this.m_roomType == LineageRoomController.LineageRoomType.Starting)
		{
			if (LineageRoomController.NumPortraits > 1)
			{
				int num = LineageRoomController.NumPortraits - 1;
				int num2 = (int)((float)num / 3f) * 3;
				int num3 = num - num2;
				if (num3 == 0)
				{
					num3 = 3;
				}
				if (num3 == 1)
				{
					this.m_rightPortrait.gameObject.SetActive(true);
					this.SetPortrait(this.m_rightPortrait, 0);
					return;
				}
				if (num3 == 2)
				{
					this.m_rightPortrait.gameObject.SetActive(true);
					this.m_centrePortrait.gameObject.SetActive(true);
					this.SetPortrait(this.m_rightPortrait, 1);
					this.SetPortrait(this.m_centrePortrait, 0);
					return;
				}
				if (num3 == 3)
				{
					this.m_rightPortrait.gameObject.SetActive(true);
					this.m_centrePortrait.gameObject.SetActive(true);
					this.m_leftPortrait.gameObject.SetActive(true);
					this.SetPortrait(this.m_rightPortrait, 2);
					this.SetPortrait(this.m_centrePortrait, 1);
					this.SetPortrait(this.m_leftPortrait, 0);
					return;
				}
			}
		}
		else
		{
			if (this.m_roomType == LineageRoomController.LineageRoomType.Connecting)
			{
				int num4 = LineageRoomController.NumPortraits - 1;
				int num5 = (int)((float)num4 / 3f) * 3;
				int num6 = num4 - num5;
				if (num6 == 0)
				{
					num6 = 3;
				}
				int num7 = roomIndex * 3 - (3 - num6);
				this.SetPortrait(this.m_leftPortrait, num7);
				this.SetPortrait(this.m_centrePortrait, num7 + 1);
				this.SetPortrait(this.m_rightPortrait, num7 + 2);
				return;
			}
			this.SetPortrait(this.m_leftPortrait, LineageRoomController.NumPortraits - 1);
		}
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x0008B858 File Offset: 0x00089A58
	private void SetPortrait(LineagePortrait portrait, int portraitIndex)
	{
		CharacterData portraitLook;
		if (this.InstantiatingFromLineageWindow)
		{
			portraitLook = SaveManager.LineageSaveData.LineageHeirList[portraitIndex];
		}
		else
		{
			int count = SaveManager.LineageSaveData.LineageHeirList.Count;
			int num = count - LineageRoomController.NumPortraits + portraitIndex;
			num = Mathf.Clamp(num, 0, count - 1);
			portraitLook = SaveManager.LineageSaveData.LineageHeirList[num];
		}
		portrait.Index = portraitIndex;
		portrait.SetPortraitLook(portraitLook);
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x0008B8C4 File Offset: 0x00089AC4
	public LineagePortrait GetPortraitAtIndex(int portraitIndex)
	{
		if (this.RightPortrait.Index == portraitIndex)
		{
			return this.RightPortrait;
		}
		if (this.CentrePortrait.Index == portraitIndex)
		{
			return this.CentrePortrait;
		}
		if (this.LeftPortrait.Index == portraitIndex)
		{
			return this.LeftPortrait;
		}
		LineageRoomController component = this.m_endingRoom.GetComponent<LineageRoomController>();
		if (component.LeftPortrait.Index == portraitIndex)
		{
			return component.LeftPortrait;
		}
		foreach (LineageRoomController lineageRoomController in LineageRoomController.m_connectingRoomControllerArray)
		{
			if (lineageRoomController.LeftPortrait.Index == portraitIndex)
			{
				return lineageRoomController.LeftPortrait;
			}
			if (lineageRoomController.RightPortrait.Index == portraitIndex)
			{
				return lineageRoomController.RightPortrait;
			}
			if (lineageRoomController.CentrePortrait.Index == portraitIndex)
			{
				return lineageRoomController.CentrePortrait;
			}
		}
		return null;
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x0008B98C File Offset: 0x00089B8C
	private void OnBiomeCreationComplete(MonoBehaviour sender, EventArgs args)
	{
		if (GameUtility.IsInLevelEditor)
		{
			foreach (Sky sky in UnityEngine.Object.FindObjectsOfType<Sky>(true))
			{
				if (sky.name.StartsWith("Lineage"))
				{
					sky.transform.position = new Vector3(this.m_endingRoom.gameObject.transform.position.x, this.m_endingRoom.gameObject.transform.position.y, sky.transform.position.z);
					break;
				}
			}
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onBiomeCreationComplete);
			return;
		}
		foreach (Sky sky2 in WorldBuilder.GetBiomeController(BiomeType.Garden).Skies)
		{
			if (sky2.name.StartsWith("Lineage"))
			{
				sky2.transform.position = new Vector3(this.m_endingRoom.gameObject.transform.position.x, this.m_endingRoom.gameObject.transform.position.y, sky2.transform.position.z);
				break;
			}
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeCreationComplete, this.m_onBiomeCreationComplete);
	}

	// Token: 0x04002259 RID: 8793
	private static Room[] m_connectingRoomArray;

	// Token: 0x0400225A RID: 8794
	private static LineageRoomController[] m_connectingRoomControllerArray;

	// Token: 0x0400225B RID: 8795
	private static Room m_currentRoom;

	// Token: 0x0400225C RID: 8796
	private static int m_roomIndex;

	// Token: 0x0400225D RID: 8797
	private static Vector3 m_startingRoomPosition;

	// Token: 0x0400225E RID: 8798
	public static int NumConnectorRooms;

	// Token: 0x0400225F RID: 8799
	public static int NumPortraits;

	// Token: 0x04002260 RID: 8800
	[SerializeField]
	private LineageRoomController.LineageRoomType m_roomType;

	// Token: 0x04002261 RID: 8801
	[Header("Starting Room Fields")]
	[SerializeField]
	private Room m_connectorRoomPrefab;

	// Token: 0x04002262 RID: 8802
	[SerializeField]
	private Room m_endingRoomPrefab;

	// Token: 0x04002263 RID: 8803
	[Header("Connecting Room Fields")]
	[SerializeField]
	private LineagePortrait m_leftPortrait;

	// Token: 0x04002264 RID: 8804
	[SerializeField]
	private LineagePortrait m_centrePortrait;

	// Token: 0x04002265 RID: 8805
	[SerializeField]
	private LineagePortrait m_rightPortrait;

	// Token: 0x04002266 RID: 8806
	[Header("End Room Fields")]
	[SerializeField]
	private GameObject m_lineageModelPositionObject;

	// Token: 0x04002267 RID: 8807
	[SerializeField]
	private GameObject m_finalBossTunnelObject;

	// Token: 0x04002268 RID: 8808
	[Header("Fields For All Rooms")]
	[SerializeField]
	private PlayerTrigger m_roomTrigger;

	// Token: 0x04002269 RID: 8809
	private Room m_room;

	// Token: 0x0400226A RID: 8810
	private Room m_endingRoom;

	// Token: 0x0400226B RID: 8811
	private static bool m_hasBiomeEnterEventBroadcast;

	// Token: 0x0400226C RID: 8812
	private static GameObject m_sky;

	// Token: 0x0400226D RID: 8813
	private Animator m_animator;

	// Token: 0x0400226E RID: 8814
	private Action<MonoBehaviour, EventArgs> m_onHeirSelectionChanged;

	// Token: 0x0400226F RID: 8815
	private Action<MonoBehaviour, EventArgs> m_onBiomeCreationComplete;

	// Token: 0x04002271 RID: 8817
	private Coroutine m_spotlightTweenCoroutine;

	// Token: 0x04002272 RID: 8818
	private float m_currentSpotlightPos;

	// Token: 0x02000C6F RID: 3183
	private enum LineageRoomType
	{
		// Token: 0x04005071 RID: 20593
		None,
		// Token: 0x04005072 RID: 20594
		Starting = 10,
		// Token: 0x04005073 RID: 20595
		Connecting = 20,
		// Token: 0x04005074 RID: 20596
		Ending = 30
	}
}

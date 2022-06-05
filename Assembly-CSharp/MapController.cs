using System;
using System.Collections;
using System.Collections.Generic;
using RLWorldCreation;
using RL_Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000482 RID: 1154
public class MapController : MonoBehaviour
{
	// Token: 0x06002A27 RID: 10791 RVA: 0x0008BCAC File Offset: 0x00089EAC
	public static GameObject GetSpecialIconPrefab(GridPointManager roomToCheck, bool getUsed, bool isMergeRoom)
	{
		if (!roomToCheck.RoomMetaData.ShowIconOnMap)
		{
			return null;
		}
		RoomType roomType = roomToCheck.RoomType;
		if (roomType <= RoomType.BossEntrance)
		{
			if (roomType != RoomType.Fairy)
			{
				if (roomType == RoomType.BossEntrance)
				{
					if (!getUsed)
					{
						return MapController.m_instance.m_bossIconPrefab;
					}
					return MapController.m_instance.m_bossBeatenIconPrefab;
				}
			}
			else
			{
				if (!getUsed)
				{
					return MapController.m_instance.m_mapFairyChestClosedIconPrefab;
				}
				return MapController.m_instance.m_mapFairyChestOpenedIconPrefab;
			}
		}
		else if (roomType == RoomType.Bonus || roomType == RoomType.Mandatory)
		{
			SpecialRoomType specialRoomType = roomToCheck.RoomMetaData.SpecialRoomType;
			if (specialRoomType <= SpecialRoomType.SubbossEntrance)
			{
				if (specialRoomType == SpecialRoomType.Subboss || specialRoomType == SpecialRoomType.SubbossEntrance)
				{
					if (!getUsed)
					{
						return MapController.m_instance.m_subbossIconPrefab;
					}
					return MapController.m_instance.m_subbossBeatenIconPrefab;
				}
			}
			else if (specialRoomType == SpecialRoomType.Heirloom || specialRoomType == SpecialRoomType.HeirloomEntrance)
			{
				if (!getUsed)
				{
					return MapController.m_instance.m_heirloomRoomIconPrefab;
				}
				return MapController.m_instance.m_heirloomRoomCompleteIconPrefab;
			}
			if (roomToCheck.RoomType == RoomType.Mandatory && (specialRoomType == SpecialRoomType.None || specialRoomType == SpecialRoomType.WhitePipUnique) && !isMergeRoom)
			{
				if (!getUsed)
				{
					return MapController.m_instance.m_npcRoomIconPrefab;
				}
				return MapController.m_instance.m_npcRoomUsedIconPrefab;
			}
			else
			{
				if (!getUsed)
				{
					return MapController.m_instance.m_specialRoomIconPrefab;
				}
				return MapController.m_instance.m_specialRoomUsedIconPrefab;
			}
		}
		return null;
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x0008BDCC File Offset: 0x00089FCC
	public static Rect GetBiomeRect(BiomeType biomeType)
	{
		Rect result;
		if (MapController.m_instance.m_biomeMapBoundsTable.TryGetValue(biomeType, out result))
		{
			return result;
		}
		return Rect.zero;
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x0008BDF4 File Offset: 0x00089FF4
	public static MapRoomEntry GetMapRoomEntry(BiomeType biomeType, int biomeControllerIndex)
	{
		biomeType = BiomeType_RL.GetGroupedBiomeType(biomeType);
		MapRoomEntry result = null;
		if (MapController.m_instance.m_mapRoomEntryDict.ContainsKey(biomeType) && MapController.m_instance.m_mapRoomEntryDict[biomeType].ContainsKey(biomeControllerIndex))
		{
			result = MapController.m_instance.m_mapRoomEntryDict[biomeType][biomeControllerIndex];
		}
		return result;
	}

	// Token: 0x1700105E RID: 4190
	// (get) Token: 0x06002A2A RID: 10794 RVA: 0x0008BE4D File Offset: 0x0008A04D
	public static List<GlobalTeleporterController> TeleporterList
	{
		get
		{
			return MapController.m_instance.m_teleporterList;
		}
	}

	// Token: 0x1700105F RID: 4191
	// (get) Token: 0x06002A2B RID: 10795 RVA: 0x0008BE59 File Offset: 0x0008A059
	// (set) Token: 0x06002A2C RID: 10796 RVA: 0x0008BE60 File Offset: 0x0008A060
	public static bool IsCameraMoving { get; private set; }

	// Token: 0x17001060 RID: 4192
	// (get) Token: 0x06002A2D RID: 10797 RVA: 0x0008BE68 File Offset: 0x0008A068
	// (set) Token: 0x06002A2E RID: 10798 RVA: 0x0008BE6F File Offset: 0x0008A06F
	public static bool IsInitialized
	{
		get
		{
			return MapController.m_isInitialized;
		}
		private set
		{
			MapController.m_isInitialized = value;
		}
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x0008BE78 File Offset: 0x0008A078
	public static bool WasRoomVisited(BiomeType biomeType, int biomeControllerIndex)
	{
		bool result = false;
		MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(biomeType, biomeControllerIndex);
		if (mapRoomEntry != null)
		{
			result = mapRoomEntry.WasVisited;
		}
		return result;
	}

	// Token: 0x17001061 RID: 4193
	// (get) Token: 0x06002A30 RID: 10800 RVA: 0x0008BEA0 File Offset: 0x0008A0A0
	public static Camera Camera
	{
		get
		{
			return MapController.m_instance.m_miniMapCamera;
		}
	}

	// Token: 0x17001062 RID: 4194
	// (get) Token: 0x06002A31 RID: 10801 RVA: 0x0008BEAC File Offset: 0x0008A0AC
	public static Camera MapWindowCamera
	{
		get
		{
			return MapController.m_instance.m_mapWindowCamera;
		}
	}

	// Token: 0x17001063 RID: 4195
	// (get) Token: 0x06002A32 RID: 10802 RVA: 0x0008BEB8 File Offset: 0x0008A0B8
	public static Camera DeathMapCamera
	{
		get
		{
			return MapController.m_instance.m_deathMapCamera;
		}
	}

	// Token: 0x17001064 RID: 4196
	// (get) Token: 0x06002A33 RID: 10803 RVA: 0x0008BEC4 File Offset: 0x0008A0C4
	public static Vector3 PlayerIconPosition
	{
		get
		{
			if (MapController.m_playerIcon)
			{
				return MapController.m_playerIcon.transform.position;
			}
			return MapController.MAP_STARTING_POSITION;
		}
	}

	// Token: 0x17001065 RID: 4197
	// (get) Token: 0x06002A34 RID: 10804 RVA: 0x0008BEEC File Offset: 0x0008A0EC
	// (set) Token: 0x06002A35 RID: 10805 RVA: 0x0008BEF8 File Offset: 0x0008A0F8
	public static CameraFollowMode FollowMode
	{
		get
		{
			return MapController.m_instance.m_followMode;
		}
		set
		{
			MapController.m_instance.m_followMode = value;
		}
	}

	// Token: 0x17001066 RID: 4198
	// (get) Token: 0x06002A36 RID: 10806 RVA: 0x0008BF05 File Offset: 0x0008A105
	// (set) Token: 0x06002A37 RID: 10807 RVA: 0x0008BF11 File Offset: 0x0008A111
	public static List<GridPointManager> GridPointManagersContainingTeleporters
	{
		get
		{
			return MapController.m_instance.m_gridPointManagersContainingTeleporters;
		}
		private set
		{
			MapController.m_instance.m_gridPointManagersContainingTeleporters = value;
		}
	}

	// Token: 0x17001067 RID: 4199
	// (get) Token: 0x06002A38 RID: 10808 RVA: 0x0008BF1E File Offset: 0x0008A11E
	// (set) Token: 0x06002A39 RID: 10809 RVA: 0x0008BF25 File Offset: 0x0008A125
	public static float MapIconAnimationCurveValue { get; private set; }

	// Token: 0x06002A3A RID: 10810 RVA: 0x0008BF30 File Offset: 0x0008A130
	private void Awake()
	{
		if (!MapController.m_instance)
		{
			MapController.m_instance = this;
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
			this.m_onBiomeEnter = new Action<MonoBehaviour, EventArgs>(this.OnBiomeEnter);
			this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
			this.m_onUpdateCurrentRoomIcons = new Action<MonoBehaviour, EventArgs>(this.OnUpdateCurrentRoomIcons);
			this.m_linkTunnelBonusRooms = new Action<MonoBehaviour, EventArgs>(this.LinkTunnelBonusRooms);
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChestOpened, this.m_onUpdateCurrentRoomIcons);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onUpdateCurrentRoomIcons);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SpecialRoomCompleted, this.m_onUpdateCurrentRoomIcons);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeCreationComplete, this.m_linkTunnelBonusRooms);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x0008C024 File Offset: 0x0008A224
	private void OnDestroy()
	{
		this.m_miniMapCamera.targetTexture = null;
		MapController.m_isCameraFollowOn = false;
		MapController.m_previousPlayerIconPosition = Vector3.zero;
		if (MapController.m_playerIcon)
		{
			UnityEngine.Object.Destroy(MapController.m_playerIcon);
		}
		MapController.m_playerIcon = null;
		if (MapController.m_teleporterLine)
		{
			UnityEngine.Object.Destroy(MapController.m_teleporterLine);
		}
		MapController.m_teleporterLine = null;
		if (MapController.m_mapObj)
		{
			UnityEngine.Object.Destroy(MapController.m_mapObj);
		}
		MapController.m_mapObj = null;
		if (MapController.m_enemyKilledLocation)
		{
			UnityEngine.Object.Destroy(MapController.m_enemyKilledLocation);
		}
		MapController.m_enemyKilledLocation = null;
		MapController.m_isInitialized = false;
		MapController.m_instance = null;
		SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChestOpened, this.m_onUpdateCurrentRoomIcons);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onUpdateCurrentRoomIcons);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.SpecialRoomCompleted, this.m_onUpdateCurrentRoomIcons);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeCreationComplete, this.m_linkTunnelBonusRooms);
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x0008C133 File Offset: 0x0008A333
	private void OnEnable()
	{
		base.StartCoroutine(this.MapIconAnimationCoroutine());
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x0008C142 File Offset: 0x0008A342
	private IEnumerator MapIconAnimationCoroutine()
	{
		float startTime = Time.unscaledTime;
		bool isGamePaused = GameManager.IsGamePaused && !WindowManager.GetIsWindowOpen(WindowID.PlayerDeath);
		float duration = this.m_animCurve.keys[this.m_animCurve.keys.Length - 1].time;
		float elapsedPercent = 0f;
		while (elapsedPercent < 1f)
		{
			elapsedPercent = (Time.unscaledTime - startTime) / duration;
			if (isGamePaused)
			{
				MapController.MapIconAnimationCurveValue = this.m_animCurve.Evaluate(elapsedPercent);
			}
			else
			{
				MapController.MapIconAnimationCurveValue = 0f;
			}
			yield return null;
		}
		float delay = Time.unscaledTime + this.m_animLoopdelay;
		while (Time.unscaledTime < delay)
		{
			yield return null;
		}
		base.StartCoroutine(this.MapIconAnimationCoroutine());
		yield break;
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x0008C154 File Offset: 0x0008A354
	public static Vector3 GetWorldPositionFromMap(Vector3 position)
	{
		Vector3 vector = position;
		vector /= 0.06666667f;
		vector.z = 1f;
		vector.x -= MapController.MAP_STARTING_POSITION.x;
		vector.y -= MapController.MAP_STARTING_POSITION.y;
		return vector;
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x0008C1A8 File Offset: 0x0008A3A8
	public static void Initialize()
	{
		MapController.m_mapObj = new GameObject("Map Objects");
		MapController.m_mapObj.transform.SetParent(MapController.m_instance.transform);
		MapController.m_playerIcon = UnityEngine.Object.Instantiate<GameObject>(MapController.m_instance.m_mapPlayerIconPrefab, MapController.m_mapObj.transform);
		MapController.SetMapCameraPosition(MapController.MAP_STARTING_POSITION);
		MapController.UpdatePlayerIconPosition();
		MapController.m_teleporterLine = UnityEngine.Object.Instantiate<SpriteRenderer>(MapController.m_instance.m_mapTeleporterLine, MapController.m_mapObj.transform);
		MapController.m_teleporterLine.gameObject.SetActive(false);
		MapController.CreateWorldMap();
		MapController.SetCameraFollowIsOn(true);
		MapController.FollowMode = CameraFollowMode.Constant;
		foreach (KeyValuePair<BiomeType, Dictionary<int, MapRoomEntry>> keyValuePair in MapController.m_instance.m_mapRoomEntryDict)
		{
			foreach (KeyValuePair<int, MapRoomEntry> keyValuePair2 in keyValuePair.Value)
			{
				if (keyValuePair2.Value.RoomType == RoomType.Transition && SaveManager.PlayerSaveData.GetTeleporterIsUnlocked(keyValuePair.Key))
				{
					MapRoomEntry value = keyValuePair2.Value;
					value.gameObject.SetActive(true);
					value.WasVisited = true;
					value.ToggleIconVisibility(MapIconType.Teleporter, -1, true);
				}
				else
				{
					MapController.SetRoomVisible(keyValuePair.Key, keyValuePair2.Key, false, false);
				}
			}
		}
		MapController.IsInitialized = true;
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x0008C338 File Offset: 0x0008A538
	public static void LoadStageSaveData()
	{
		if (!GameUtility.IsInLevelEditor)
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<BiomeType, BiomeController> keyValuePair in WorldBuilder.BiomeControllers)
			{
				BiomeType key = keyValuePair.Key;
				BiomeController value = keyValuePair.Value;
				if (value != null && value.GridPointManager.GridPointManagers != null)
				{
					list.Clear();
					foreach (GridPointManager gridPointManager in value.GridPointManager.GridPointManagers)
					{
						if (!list.Contains(gridPointManager.BiomeControllerIndex))
						{
							list.Add(gridPointManager.BiomeControllerIndex);
							MapController.SetRoomVisibleUsingSaveData(key, gridPointManager.BiomeControllerIndex, gridPointManager);
						}
					}
				}
			}
		}
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x0008C438 File Offset: 0x0008A638
	public static void CentreCameraAroundPlayerIcon()
	{
		MapController.SetMapCameraPosition(MapController.PlayerIconPosition);
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x0008C450 File Offset: 0x0008A650
	public static void CreateWorldMap()
	{
		MapController.m_instance.m_biomeMapBoundsTable.Clear();
		MapController.m_instance.m_mapEntriesThatNeedLinking.Clear();
		foreach (KeyValuePair<BiomeType, BiomeController> keyValuePair in WorldBuilder.BiomeControllers)
		{
			BiomeType key = keyValuePair.Key;
			BiomeController value = keyValuePair.Value;
			MapController.m_instance.CreateGridPointRoomsOnMap(key, value.GridPointManager.GridPointManagers);
			if (TraitManager.IsTraitActive(TraitType.RevealAllChests))
			{
				MapController.ShowAllChestsInBiome(key);
			}
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveTuningForkTriggered))
		{
			MapController.SetCaveWhitePipVisibility(true);
		}
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x0008C508 File Offset: 0x0008A708
	public static Vector3 GetScaledPoint(Vector2 point)
	{
		return MapController.GetScaledPoint(Vector2.zero, point);
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x0008C515 File Offset: 0x0008A715
	public static Vector3 GetScaledPoint(Vector2 center, float xPosition, float yPosition)
	{
		return MapController.GetScaledPoint(center, new Vector2(xPosition, yPosition));
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x0008C524 File Offset: 0x0008A724
	public static Vector3 GetScaledPoint(Vector2 center, Vector2 point)
	{
		float x = 0.06666667f * (point.x - center.x) + center.x;
		float y = 0.06666667f * (point.y - center.y) + center.y;
		return new Vector3(x, y, 1f);
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x0008C574 File Offset: 0x0008A774
	private void CreateRoomsOnMap(BiomeType biomeType, List<BaseRoom> rooms)
	{
		if (rooms == null || rooms.Count == 0)
		{
			Debug.LogFormat("<color=red>[{0}] rooms argument is null or empty</color>", new object[]
			{
				MapController.m_instance
			});
			return;
		}
		biomeType = BiomeType_RL.GetGroupedBiomeType(biomeType);
		Dictionary<int, MapRoomEntry> dictionary = new Dictionary<int, MapRoomEntry>();
		foreach (BaseRoom baseRoom in rooms)
		{
			MapRoomEntry value = this.CreateMapRoomEntryForRoom_LevelEditorOnly(baseRoom);
			if (!dictionary.ContainsKey(baseRoom.BiomeControllerIndex))
			{
				dictionary.Add(baseRoom.BiomeControllerIndex, value);
			}
			else
			{
				Debug.Log(string.Concat(new string[]
				{
					"<color=red>WARNING: BiomeControllerIndex - ",
					baseRoom.BiomeControllerIndex.ToString(),
					" for Biome - ",
					biomeType.ToString(),
					" already found in MapController.MapRoomEntryList</color>"
				}));
			}
		}
		if (this.m_mapRoomEntryDict.ContainsKey(biomeType))
		{
			this.m_mapRoomEntryDict[biomeType] = dictionary;
			return;
		}
		this.m_mapRoomEntryDict.Add(biomeType, dictionary);
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x0008C688 File Offset: 0x0008A888
	private MapRoomEntry CreateMapRoomEntryForRoom_LevelEditorOnly(BaseRoom room)
	{
		MapRoomEntry mapRoomEntry = UnityEngine.Object.Instantiate<MapRoomEntry>(MapController.m_instance.m_mapEntryPrefab, MapController.m_mapObj.transform, true);
		mapRoomEntry.gameObject.name = room.gameObject.name + " - Map Room Entry";
		mapRoomEntry.gameObject.transform.position = MapController.GetMapPositionFromWorld(room.gameObject.transform.position, false);
		mapRoomEntry.CreateRoomTerrainForRoom(room);
		mapRoomEntry.CreateDoorIconsForRoom(MapController.m_instance.m_mapHorizontalDoor, MapController.m_instance.m_mapVerticalDoor, room);
		mapRoomEntry.CreateEnemyIconsForRoom(MapController.m_instance.m_mapEnemyIcon, MapController.m_instance.m_mapEnemyKilledIcon, room);
		mapRoomEntry.ToggleIconVisibility(MapIconType.EnemyKilled, -1, false);
		mapRoomEntry.CreateChestIconsForRoom(MapController.m_instance.m_mapChestOpenIconPrefab, MapController.m_instance.m_mapChestClosedIconPrefab, room);
		mapRoomEntry.ToggleIconVisibility(MapIconType.ChestOpened, -1, false);
		mapRoomEntry.RoomType = room.RoomType;
		RoomType roomType = room.RoomType;
		if (roomType != RoomType.Fairy)
		{
			if (roomType != RoomType.BossEntrance)
			{
				if (roomType == RoomType.Bonus)
				{
					SpecialRoomType specialRoomType = SpecialRoomType.None;
					BaseSpecialRoomController component = room.gameObject.GetComponent<BaseSpecialRoomController>();
					if (component)
					{
						specialRoomType = component.SpecialRoomType;
					}
					mapRoomEntry.SpecialRoomType = specialRoomType;
					if (specialRoomType <= SpecialRoomType.SubbossEntrance)
					{
						if (specialRoomType == SpecialRoomType.Subboss || specialRoomType == SpecialRoomType.SubbossEntrance)
						{
							mapRoomEntry.CreateSpecialRoomIconsForRoom(MapController.m_instance.m_subbossIconPrefab, MapController.m_instance.m_subbossBeatenIconPrefab, room);
							mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
							goto IL_23B;
						}
					}
					else if (specialRoomType == SpecialRoomType.Heirloom || specialRoomType == SpecialRoomType.HeirloomEntrance)
					{
						mapRoomEntry.CreateSpecialRoomIconsForRoom(MapController.m_instance.m_heirloomRoomIconPrefab, MapController.m_instance.m_heirloomRoomCompleteIconPrefab, room);
						mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
						goto IL_23B;
					}
					if (room.RoomType == RoomType.Mandatory && (specialRoomType == SpecialRoomType.None || specialRoomType == SpecialRoomType.WhitePipUnique))
					{
						mapRoomEntry.CreateSpecialRoomIconsForRoom(MapController.m_instance.m_npcRoomIconPrefab, MapController.m_instance.m_npcRoomUsedIconPrefab, room);
						mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
					}
					else
					{
						mapRoomEntry.CreateSpecialRoomIconsForRoom(MapController.m_instance.m_specialRoomIconPrefab, MapController.m_instance.m_specialRoomUsedIconPrefab, room);
						mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
					}
				}
			}
			else
			{
				mapRoomEntry.CreateSpecialRoomIconsForRoom(MapController.m_instance.m_bossIconPrefab, MapController.m_instance.m_bossBeatenIconPrefab, room);
				mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
			}
		}
		else
		{
			mapRoomEntry.CreateSpecialRoomIconsForRoom(MapController.m_instance.m_mapFairyChestClosedIconPrefab, MapController.m_instance.m_mapFairyChestOpenedIconPrefab, room);
			mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
		}
		IL_23B:
		if (room.BiomeType == BiomeType.Cave && (room.RoomType == RoomType.BossEntrance || mapRoomEntry.SpecialRoomType == SpecialRoomType.SubbossEntrance || mapRoomEntry.SpecialRoomType == SpecialRoomType.WhitePip || mapRoomEntry.SpecialRoomType == SpecialRoomType.WhitePipUnique))
		{
			mapRoomEntry.CreateSpecialIndicatorIconForRoom(MapController.m_instance.m_specialIndicatorIconPrefab, room);
			mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
		}
		MapController.CreateTeleporterIconsForGridPoint((room as Room).GridPointManager, mapRoomEntry);
		mapRoomEntry.gameObject.SetActive(false);
		return mapRoomEntry;
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x0008C944 File Offset: 0x0008AB44
	private void CreateGridPointRoomsOnMap(BiomeType biomeType, List<GridPointManager> rooms)
	{
		if (rooms == null || rooms.Count == 0)
		{
			Debug.LogFormat("<color=red>[{0}] rooms argument is null or empty</color>", new object[]
			{
				MapController.m_instance
			});
			return;
		}
		biomeType = BiomeType_RL.GetGroupedBiomeType(biomeType);
		Dictionary<int, MapRoomEntry> dictionary = new Dictionary<int, MapRoomEntry>();
		List<GridPointManager> list = new List<GridPointManager>();
		List<GridPointManager> list2 = new List<GridPointManager>();
		for (int i = 0; i < rooms.Count; i++)
		{
			if (rooms[i].MergeWithGridPointManagers.Count == 0)
			{
				list.Add(rooms[i]);
			}
			else if (!list2.Contains(rooms[i]))
			{
				bool flag = true;
				for (int j = 0; j < rooms[i].MergeWithGridPointManagers.Count; j++)
				{
					if (list2.Contains(rooms[i].MergeWithGridPointManagers[j]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list2.Add(rooms[i]);
				}
			}
		}
		list.AddRange(list2);
		foreach (GridPointManager gridPointManager in list)
		{
			MapRoomEntry value = MapController.CreateMapRoomEntryForGridPoint(gridPointManager);
			dictionary.Add(gridPointManager.BiomeControllerIndex, value);
		}
		if (this.m_mapRoomEntryDict.ContainsKey(biomeType))
		{
			this.m_mapRoomEntryDict[biomeType] = dictionary;
		}
		else
		{
			this.m_mapRoomEntryDict.Add(biomeType, dictionary);
		}
		if (!this.m_biomeMapBoundsTable.ContainsKey(biomeType))
		{
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MinValue;
			float num4 = float.MaxValue;
			foreach (KeyValuePair<int, MapRoomEntry> keyValuePair in this.m_mapRoomEntryDict[biomeType])
			{
				MapRoomEntry value2 = keyValuePair.Value;
				if (!value2.IsTunnelExit)
				{
					if (value2.AbsBounds.xMin < num)
					{
						num = value2.AbsBounds.xMin;
					}
					if (value2.AbsBounds.xMax > num2)
					{
						num2 = value2.AbsBounds.xMax;
					}
					if (value2.AbsBounds.yMin < num4)
					{
						num4 = value2.AbsBounds.yMin;
					}
					if (value2.AbsBounds.yMax > num3)
					{
						num3 = value2.AbsBounds.yMax;
					}
				}
			}
			Rect value3 = new Rect(num, num4, num2 - num, num3 - num4);
			this.m_biomeMapBoundsTable.Add(biomeType, value3);
		}
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x0008CBF8 File Offset: 0x0008ADF8
	public static MapRoomEntry CreateMapRoomEntryForGridPoint(GridPointManager roomGridPoint)
	{
		MapRoomEntry mapRoomEntry = UnityEngine.Object.Instantiate<MapRoomEntry>(MapController.m_instance.m_mapEntryPrefab, MapController.m_mapObj.transform, true);
		mapRoomEntry.gameObject.name = roomGridPoint.RoomMetaData.ID.ToString() + " - Map Room Entry";
		Vector2 worldPositionFromRoomCoordinates = GridController.GetWorldPositionFromRoomCoordinates(GridController.GetRoomCoordinatesFromGridCoordinates(roomGridPoint.GridCoordinates), roomGridPoint.Size);
		mapRoomEntry.gameObject.transform.position = MapController.GetMapPositionFromWorld(worldPositionFromRoomCoordinates, false);
		mapRoomEntry.CreateRoomTerrainForGridPoint(roomGridPoint);
		mapRoomEntry.CreateDoorIconsForGridPoint(MapController.m_instance.m_mapHorizontalDoor, MapController.m_instance.m_mapVerticalDoor, roomGridPoint);
		mapRoomEntry.CreateEnemyIconsForGridPoint(MapController.m_instance.m_mapEnemyIcon, MapController.m_instance.m_mapEnemyKilledIcon, roomGridPoint);
		mapRoomEntry.CreateChestIconsForGridPoint(MapController.m_instance.m_mapChestOpenIconPrefab, MapController.m_instance.m_mapChestClosedIconPrefab, roomGridPoint);
		MapController.CreateTeleporterIconsForGridPoint(roomGridPoint, mapRoomEntry);
		mapRoomEntry.CreateSpecialRoomIconsForGridPoint(roomGridPoint, MapController.m_instance.m_specialIndicatorIconPrefab);
		mapRoomEntry.IsTunnelExit = roomGridPoint.IsTunnelDestination;
		mapRoomEntry.IsMergeRoomEntry = (roomGridPoint.MergeWithGridPointManagers.Count > 0);
		mapRoomEntry.BiomeControllerIndex = roomGridPoint.BiomeControllerIndex;
		mapRoomEntry.gameObject.SetActive(false);
		return mapRoomEntry;
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x0008CD2C File Offset: 0x0008AF2C
	private void LinkTunnelBonusRooms(object sender, EventArgs args)
	{
		BiomeEventArgs biomeEventArgs = args as BiomeEventArgs;
		List<MapRoomEntry> list;
		if (biomeEventArgs != null && this.m_mapEntriesThatNeedLinking.TryGetValue(biomeEventArgs.Biome, out list))
		{
			BiomeController biomeController = null;
			if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
			{
				biomeController = OnPlayManager.BiomeController;
			}
			else
			{
				biomeController = WorldBuilder.GetBiomeController(biomeEventArgs.Biome);
			}
			foreach (MapRoomEntry mapRoomEntry in list)
			{
				if (mapRoomEntry.BiomeControllerIndex < 0 || mapRoomEntry.BiomeControllerIndex > biomeController.Rooms.Count)
				{
					Debug.Log("<color=red>Something went wrong with populating the biomeControllerIndices for the MapRoomEntry.</color>");
				}
				else
				{
					BaseRoom room = biomeController.GetRoom(mapRoomEntry.BiomeControllerIndex);
					this.m_mapEntryTunnelSpawners.Clear();
					room.GetComponentsInChildren<TunnelSpawnController>(true, this.m_mapEntryTunnelSpawners);
					for (int i = 0; i < this.m_mapEntryTunnelSpawners.Count; i++)
					{
						TunnelSpawnController tunnelSpawnController = this.m_mapEntryTunnelSpawners[i];
						bool flag = tunnelSpawnController.RoomMetaData == null;
						bool flag2 = !flag && tunnelSpawnController.RoomMetaData.SpecialRoomType != SpecialRoomType.None && tunnelSpawnController.RoomMetaData.SpecialRoomType != SpecialRoomType.BossEntrance && tunnelSpawnController.LeadsToRoomType != RoomType.Boss;
						if (!tunnelSpawnController.ShouldSpawn || (!flag && !flag2))
						{
							this.m_mapEntryTunnelSpawners.RemoveAt(i);
							i--;
						}
					}
					int count = this.m_mapEntryTunnelSpawners.Count;
					int num = mapRoomEntry.RoomCompleteBiomeControllerIndexOverrides.Length;
					if (count == num)
					{
						for (int j = 0; j < count; j++)
						{
							Tunnel tunnel = this.m_mapEntryTunnelSpawners[j].Tunnel;
							if (tunnel)
							{
								BaseRoom room2 = tunnel.Destination.Room;
								if (room2.GetComponent<BaseSpecialRoomController>())
								{
									mapRoomEntry.RoomCompleteBiomeControllerIndexOverrides[j] = room2.BiomeControllerIndex;
								}
								else
								{
									foreach (BaseRoom baseRoom in room2.ConnectedRooms)
									{
										if (baseRoom.GetComponent<BaseSpecialRoomController>())
										{
											mapRoomEntry.RoomCompleteBiomeControllerIndexOverrides[j] = baseRoom.BiomeControllerIndex;
										}
									}
								}
							}
						}
					}
					else
					{
						Debug.Log(string.Concat(new string[]
						{
							"<color=red>WARNING: Cannot link tunnel room map entries in Biome: ",
							biomeEventArgs.Biome.ToString(),
							". TunnelSpawner count (",
							count.ToString(),
							") and MapRoomEntry SpecialIcon count (",
							num.ToString(),
							") mismatch. RoomName: ",
							room.name,
							"</color>"
						}));
					}
				}
			}
			list.Clear();
			this.m_mapEntriesThatNeedLinking.Remove(biomeEventArgs.Biome);
		}
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x0008D038 File Offset: 0x0008B238
	public static void AddMapEntryNeedsLinking(BiomeType biome, MapRoomEntry entry)
	{
		List<MapRoomEntry> list;
		if (MapController.m_instance.m_mapEntriesThatNeedLinking.TryGetValue(biome, out list))
		{
			if (!list.Contains(entry))
			{
				list.Add(entry);
				return;
			}
		}
		else
		{
			MapController.m_instance.m_mapEntriesThatNeedLinking.Add(biome, new List<MapRoomEntry>());
			MapController.m_instance.m_mapEntriesThatNeedLinking[biome].Add(entry);
		}
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x0008D095 File Offset: 0x0008B295
	private static void CreateTeleporterIconsForGridPoint(GridPointManager room, MapRoomEntry mapRoomEntry)
	{
		if (mapRoomEntry.CreateTeleporterIconForGridPoint(MapController.m_instance.m_mapTeleporterIcon, room))
		{
			MapController.GridPointManagersContainingTeleporters.Add(room);
		}
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x0008D0B8 File Offset: 0x0008B2B8
	private static MapRoomEntry CreateMapRoomEntryPostRoomInstantiation(BiomeType biome, int biomeControllerIndex)
	{
		MapRoomEntry mapRoomEntry = null;
		BiomeController biomeController = WorldBuilder.GetBiomeController(biome);
		if (!biomeController)
		{
			biomeController = OnPlayManager.BiomeController;
		}
		if (biomeController)
		{
			mapRoomEntry = MapController.CreateMapRoomEntryForGridPoint((biomeController.GetRoom(biomeControllerIndex) as Room).GridPointManager);
			Dictionary<int, MapRoomEntry> dictionary = MapController.m_instance.m_mapRoomEntryDict[biome];
			if (dictionary != null)
			{
				dictionary.Add(biomeControllerIndex, mapRoomEntry);
			}
			else
			{
				Debug.LogFormat("<color=red>| MapController | Failed to find entry for <b>{0}</b> Biome in Map Room Entry Dictionary, but should have.</color>", new object[]
				{
					biome
				});
			}
		}
		return mapRoomEntry;
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x0008D133 File Offset: 0x0008B333
	public static void DisplayAllRooms()
	{
		MapController.SetAllBiomeVisibility(true, true, false);
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x0008D140 File Offset: 0x0008B340
	public static Vector3 GetMapPositionFromWorld(Vector3 position, bool applyIconYOffset = false)
	{
		if (applyIconYOffset)
		{
			position.y += 2f;
		}
		Vector3 scaledPoint = MapController.GetScaledPoint(position);
		scaledPoint.x += MapController.MAP_STARTING_POSITION.x;
		scaledPoint.y += MapController.MAP_STARTING_POSITION.y;
		return scaledPoint;
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x0008D198 File Offset: 0x0008B398
	private void FixedUpdate()
	{
		if (MapController.m_isCameraFollowOn && PlayerManager.IsInstantiated)
		{
			Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(PlayerManager.GetPlayerController().Midpoint, true);
			mapPositionFromWorld.z = 0.9f;
			MapController.m_playerIcon.gameObject.transform.position = mapPositionFromWorld;
			MapController.CentreCameraAroundPlayerIcon();
		}
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x0008D1EC File Offset: 0x0008B3EC
	private void LateUpdate()
	{
		if (MapController.m_teleporterLine && MapController.m_teleporterLine.gameObject.activeInHierarchy)
		{
			float x = CDGHelper.DistanceBetweenPts(MapController.PlayerIconPosition, this.m_miniMapCamera.transform.position);
			Vector2 size = MapController.m_teleporterLine.size;
			size.x = x;
			MapController.m_teleporterLine.size = size;
			Vector3 localEulerAngles = MapController.m_teleporterLine.gameObject.transform.localEulerAngles;
			float z = CDGHelper.AngleBetweenPts(MapController.PlayerIconPosition, this.m_miniMapCamera.transform.position);
			localEulerAngles.z = z;
			MapController.m_teleporterLine.gameObject.transform.localEulerAngles = localEulerAngles;
			MapController.m_teleporterLine.gameObject.transform.position = MapController.PlayerIconPosition;
		}
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x0008D2D0 File Offset: 0x0008B4D0
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		if (!MapController.IsInitialized)
		{
			MapController.Initialize();
		}
		BaseRoom room = (args as RoomViaDoorEventArgs).Room;
		BiomeType biomeType = room.BiomeType;
		MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(biomeType, room.BiomeControllerIndex);
		if (mapRoomEntry == null)
		{
			mapRoomEntry = MapController.CreateMapRoomEntryPostRoomInstantiation(biomeType, room.BiomeControllerIndex);
		}
		if (!CutsceneManager.IsCutsceneActive)
		{
			MapController.SetRoomVisible(biomeType, room.BiomeControllerIndex, true, false);
			if (mapRoomEntry.IsTunnelExit)
			{
				for (int i = 0; i < room.ConnectedRooms.Count; i++)
				{
					int biomeControllerIndex = room.ConnectedRooms[i].BiomeControllerIndex;
					MapRoomEntry mapRoomEntry2 = MapController.GetMapRoomEntry(biomeType, biomeControllerIndex);
					if (mapRoomEntry2 && mapRoomEntry2.WasVisited)
					{
						MapController.SetRoomVisible(biomeType, biomeControllerIndex, true, false);
					}
				}
			}
		}
		if (TraitManager.IsTraitActive(TraitType.MapReveal))
		{
			if (MapController.m_playerIcon.activeSelf)
			{
				MapController.m_playerIcon.SetActive(false);
				return;
			}
		}
		else if (!MapController.m_playerIcon.activeSelf)
		{
			MapController.m_playerIcon.SetActive(true);
		}
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x0008D3C8 File Offset: 0x0008B5C8
	private void OnBiomeEnter(object sender, EventArgs args)
	{
		if (CutsceneManager.ExitRoomTunnel)
		{
			return;
		}
		BiomeType groupedBiomeType = BiomeType_RL.GetGroupedBiomeType((args as BiomeEventArgs).Biome);
		if (!BiomeType_RL.IsValidBiome(groupedBiomeType))
		{
			return;
		}
		if (TraitManager.IsTraitActive(TraitType.MapReveal))
		{
			MapController.ShowAllRoomsInBiome(groupedBiomeType);
		}
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x0008D410 File Offset: 0x0008B610
	public static void ShowAllRoomsInBiome(BiomeType biome)
	{
		MapController.SetAllBiomeVisibility(false, false, true);
		if (MapController.m_instance.m_mapRoomEntryDict.ContainsKey(biome))
		{
			foreach (KeyValuePair<int, MapRoomEntry> keyValuePair in MapController.m_instance.m_mapRoomEntryDict[biome])
			{
				MapRoomEntry value = keyValuePair.Value;
				value.gameObject.SetActive(true);
				RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(biome, keyValuePair.Key);
				if (roomSaveData.IsNativeNull() || !roomSaveData.RoomVisited)
				{
					value.ToggleTerrainVisibility(true);
					value.ToggleAllIconVisibility(false);
					value.ToggleIconVisibility(MapIconType.Door, -1, true);
					value.ToggleIconVisibility(MapIconType.SpecialRoom, -1, true);
					value.ToggleIconVisibility(MapIconType.ChestClosed, -1, true);
					value.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
					value.ToggleIconVisibility(MapIconType.Teleporter, -1, true);
					if (value.RoomType == RoomType.Fairy)
					{
						value.ToggleIconVisibility(MapIconType.ChestClosed, -1, false);
						value.ToggleIconVisibility(MapIconType.ChestOpened, -1, false);
					}
				}
			}
		}
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x0008D520 File Offset: 0x0008B720
	public static void ShowAllChestsInBiome(BiomeType biome)
	{
		if (MapController.m_instance.m_mapRoomEntryDict.ContainsKey(biome))
		{
			foreach (KeyValuePair<int, MapRoomEntry> keyValuePair in MapController.m_instance.m_mapRoomEntryDict[biome])
			{
				MapRoomEntry value = keyValuePair.Value;
				if (!value.WasVisited && value.RoomType != RoomType.Fairy)
				{
					value.gameObject.SetActive(true);
					value.ToggleTerrainVisibility(false);
					value.ToggleAllIconVisibility(false);
					value.ToggleIconVisibility(MapIconType.ChestClosed, -1, true);
					value.ToggleIconVisibility(MapIconType.ChestOpened, -1, false);
				}
			}
		}
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x0008D5D4 File Offset: 0x0008B7D4
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		if (MapController.IsInitialized)
		{
			RoomViaDoorEventArgs roomViaDoorEventArgs = args as RoomViaDoorEventArgs;
			BaseRoom room = roomViaDoorEventArgs.Room;
			BiomeType biomeType = room.BiomeType;
			MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(biomeType, room.BiomeControllerIndex);
			if (mapRoomEntry != null && mapRoomEntry.IsTunnelExit && roomViaDoorEventArgs.ViaDoor == null)
			{
				foreach (KeyValuePair<int, MapRoomEntry> keyValuePair in this.m_mapRoomEntryDict[biomeType])
				{
					int key = keyValuePair.Key;
					mapRoomEntry = keyValuePair.Value;
					if (mapRoomEntry.IsTunnelExit)
					{
						MapController.SetRoomVisible(biomeType, key, false, false);
					}
				}
			}
		}
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x0008D698 File Offset: 0x0008B898
	private void OnUpdateCurrentRoomIcons(object sender, EventArgs args)
	{
		if (PlayerManager.IsInstantiated)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			MapController.SetRoomVisible(currentPlayerRoom.BiomeType, currentPlayerRoom.BiomeControllerIndex, true, false);
		}
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x0008D6C5 File Offset: 0x0008B8C5
	private void OnActiveSceneChanged(Scene current, Scene next)
	{
		MapController.GridPointManagersContainingTeleporters.Clear();
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x0008D6D4 File Offset: 0x0008B8D4
	public static void Reset()
	{
		MapController.Camera.orthographicSize = 2f;
		MapController.MapWindowCamera.gameObject.SetActive(false);
		MapController.DeathMapCamera.gameObject.SetActive(false);
		MapController.m_isCameraFollowOn = false;
		MapController.m_previousPlayerIconPosition = Vector3.zero;
		UnityEngine.Object.Destroy(MapController.m_mapObj);
		foreach (KeyValuePair<BiomeType, Dictionary<int, MapRoomEntry>> keyValuePair in MapController.m_instance.m_mapRoomEntryDict)
		{
			foreach (KeyValuePair<int, MapRoomEntry> keyValuePair2 in keyValuePair.Value)
			{
				UnityEngine.Object.Destroy(keyValuePair2.Value);
			}
			keyValuePair.Value.Clear();
		}
		MapController.m_instance.m_mapRoomEntryDict.Clear();
		MapController.m_instance.m_biomeMapBoundsTable.Clear();
		MapController.m_isInitialized = false;
		MapController.m_enemyKilledLocation = null;
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x0008D7EC File Offset: 0x0008B9EC
	private void ResetCameraPosition()
	{
		MapController.SetMapCameraPosition(MapController.MAP_STARTING_POSITION);
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x0008D800 File Offset: 0x0008BA00
	public static void SetCameraFollowIsOn(bool isOn)
	{
		MapController.m_isCameraFollowOn = isOn;
		if (isOn)
		{
			MapController.m_instance.m_cameraDestination = new Vector3(MapController.PlayerIconPosition.x, MapController.PlayerIconPosition.y, MapController.Camera.transform.position.z);
		}
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x0008D850 File Offset: 0x0008BA50
	public static void SetAllBiomeVisibility(bool isAllVisible, bool updateWasVisitedState, bool retainVisitedRoomData)
	{
		if (MapController.m_instance.m_mapRoomEntryDict == null)
		{
			return;
		}
		if (!isAllVisible && MapController.m_playerIcon)
		{
			MapController.m_playerIcon.SetActive(false);
		}
		foreach (KeyValuePair<BiomeType, Dictionary<int, MapRoomEntry>> keyValuePair in MapController.m_instance.m_mapRoomEntryDict)
		{
			foreach (KeyValuePair<int, MapRoomEntry> keyValuePair2 in keyValuePair.Value)
			{
				MapRoomEntry value = keyValuePair2.Value;
				if (!value.IsTunnelExit)
				{
					RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(keyValuePair.Key, keyValuePair2.Key);
					if (retainVisitedRoomData && !roomSaveData.IsNativeNull() && roomSaveData.RoomVisited)
					{
						value.gameObject.SetActive(isAllVisible);
					}
					else
					{
						value.gameObject.SetActive(isAllVisible);
						value.ToggleTerrainVisibility(isAllVisible);
						value.ToggleAllIconVisibility(false);
						if (isAllVisible)
						{
							value.ToggleIconVisibility(MapIconType.Door, -1, true);
							value.ToggleIconVisibility(MapIconType.SpecialRoom, -1, true);
							bool flag = roomSaveData.BiomeType == BiomeType.Tower && roomSaveData.RoomName.Contains("Exterior");
							if (value.RoomType != RoomType.Fairy || flag)
							{
								value.ToggleIconVisibility(MapIconType.ChestClosed, -1, true);
							}
							value.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, true);
							value.ToggleIconVisibility(MapIconType.Teleporter, -1, true);
						}
						if (updateWasVisitedState)
						{
							value.WasVisited = isAllVisible;
						}
					}
				}
			}
		}
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x0008DA14 File Offset: 0x0008BC14
	public static void SetVisitedRoomsVisibility(bool isVisible)
	{
		if (MapController.m_instance.m_mapRoomEntryDict == null)
		{
			return;
		}
		MapController.m_playerIcon.SetActive(true);
		foreach (KeyValuePair<BiomeType, Dictionary<int, MapRoomEntry>> keyValuePair in MapController.m_instance.m_mapRoomEntryDict)
		{
			foreach (KeyValuePair<int, MapRoomEntry> keyValuePair2 in keyValuePair.Value)
			{
				if (!keyValuePair2.Value.IsTunnelExit && keyValuePair2.Value.WasVisited)
				{
					keyValuePair2.Value.ToggleTerrainVisibility(true);
					keyValuePair2.Value.ToggleIconVisibility(MapIconType.Door, -1, true);
					keyValuePair2.Value.gameObject.SetActive(isVisible);
				}
			}
		}
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x0008DB0C File Offset: 0x0008BD0C
	public static void SetCaveWhitePipVisibility(bool visible)
	{
		if (TraitManager.IsTraitActive(TraitType.MapReveal))
		{
			return;
		}
		Dictionary<int, MapRoomEntry> dictionary;
		if (MapController.m_instance.m_mapRoomEntryDict.TryGetValue(BiomeType.Cave, out dictionary))
		{
			foreach (KeyValuePair<int, MapRoomEntry> keyValuePair in dictionary)
			{
				bool flag = false;
				if (keyValuePair.Value.SpecialRoomType == SpecialRoomType.WhitePip || keyValuePair.Value.SpecialRoomType == SpecialRoomType.WhitePipUnique || keyValuePair.Value.SpecialRoomType == SpecialRoomType.BossEntrance || keyValuePair.Value.SpecialRoomType == SpecialRoomType.SubbossEntrance)
				{
					flag = true;
				}
				if (flag)
				{
					if (visible)
					{
						bool flag2 = true;
						if (!keyValuePair.Value.WasVisited)
						{
							keyValuePair.Value.ToggleTerrainVisibility(false);
							keyValuePair.Value.ToggleIconVisibility(MapIconType.Door, -1, false);
							keyValuePair.Value.ToggleIconVisibility(MapIconType.Teleporter, -1, false);
							keyValuePair.Value.ToggleIconVisibility(MapIconType.SpecialRoom, -1, false);
						}
						else
						{
							flag2 = false;
						}
						if (flag2)
						{
							keyValuePair.Value.gameObject.SetActive(true);
							keyValuePair.Value.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, true);
						}
					}
					else
					{
						if (!keyValuePair.Value.WasVisited)
						{
							keyValuePair.Value.gameObject.SetActive(false);
						}
						keyValuePair.Value.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
					}
				}
			}
		}
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x0008DC88 File Offset: 0x0008BE88
	public static void SetIconVisibility(MapIconType iconType, BiomeType biome, int biomeControllerIndex, bool visible, int index = 0)
	{
		biome = BiomeType_RL.GetGroupedBiomeType(biome);
		if (MapController.m_instance.m_mapRoomEntryDict != null && MapController.m_instance.m_mapRoomEntryDict.ContainsKey(biome))
		{
			MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(biome, biomeControllerIndex);
			if (mapRoomEntry != null)
			{
				mapRoomEntry.ToggleIconVisibility(iconType, index, visible);
				return;
			}
		}
		else
		{
			Debug.LogFormat("<color=red>| {0} | Map Room Dictionary is null or Key ({1}) wasn't present in Dictionary</color>", new object[]
			{
				MapController.m_instance,
				biome
			});
		}
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x0008DCF8 File Offset: 0x0008BEF8
	public static void SetRoomVisibleUsingSaveData(BiomeType biome, int biomeControllerIndex, GridPointManager room)
	{
		RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(biome, biomeControllerIndex);
		if (roomSaveData == null || roomSaveData.IsEmpty || !roomSaveData.RoomVisited)
		{
			return;
		}
		biome = BiomeType_RL.GetGroupedBiomeType(biome);
		if (MapController.m_instance.m_mapRoomEntryDict != null && MapController.m_instance.m_mapRoomEntryDict.ContainsKey(biome) && MapController.m_instance.m_mapRoomEntryDict[biome].Count > 0 && biomeControllerIndex >= 0 && biomeControllerIndex < MapController.m_instance.m_mapRoomEntryDict[biome].Count)
		{
			MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(biome, biomeControllerIndex);
			if (mapRoomEntry)
			{
				mapRoomEntry.gameObject.SetActive(roomSaveData.RoomVisited);
				mapRoomEntry.ToggleTerrainVisibility(roomSaveData.RoomVisited);
				mapRoomEntry.ToggleIconVisibility(MapIconType.Door, -1, roomSaveData.RoomVisited);
				mapRoomEntry.WasVisited = roomSaveData.RoomVisited;
				if (mapRoomEntry.WasVisited && mapRoomEntry.RoomType != RoomType.Fairy && mapRoomEntry.RoomType != RoomType.Bonus)
				{
					for (int i = 0; i < roomSaveData.ChestStates.Length; i++)
					{
						if (!roomSaveData.ChestStates[i].IsSpawned)
						{
							mapRoomEntry.ToggleIconVisibility(MapIconType.ChestClosed, i, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.ChestOpened, i, false);
						}
						else
						{
							bool isStateActive = roomSaveData.ChestStates[i].IsStateActive;
							mapRoomEntry.ToggleIconVisibility(MapIconType.ChestClosed, i, isStateActive);
							mapRoomEntry.ToggleIconVisibility(MapIconType.ChestOpened, i, !isStateActive);
						}
					}
				}
				else
				{
					mapRoomEntry.ToggleIconVisibility(MapIconType.ChestClosed, -1, false);
					mapRoomEntry.ToggleIconVisibility(MapIconType.ChestOpened, -1, false);
				}
				mapRoomEntry.ToggleIconVisibility(MapIconType.Teleporter, -1, !mapRoomEntry.HasSpecialRoomIcon);
				if (mapRoomEntry.HasSpecialRoomIcon)
				{
					if (roomSaveData.RoomCompleteBiomeControllerIndexOverrides == null || roomSaveData.RoomCompleteBiomeControllerIndexOverrides.Length == 0)
					{
						bool isRoomComplete = roomSaveData.IsRoomComplete;
						mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoom, 0, !isRoomComplete);
						mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, 0, isRoomComplete);
					}
					else
					{
						for (int j = 0; j < roomSaveData.RoomCompleteBiomeControllerIndexOverrides.Length; j++)
						{
							RoomSaveData roomSaveData2 = SaveManager.StageSaveData.GetRoomSaveData(biome, roomSaveData.RoomCompleteBiomeControllerIndexOverrides[j]);
							if (!roomSaveData2.IsNativeNull())
							{
								bool isRoomComplete2 = roomSaveData2.IsRoomComplete;
								mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoom, j, !isRoomComplete2);
								mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, j, isRoomComplete2);
							}
						}
					}
				}
				if (mapRoomEntry.HasSpecialRoomIcon || mapRoomEntry.HasTeleporterIcon)
				{
					mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
				}
			}
		}
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x0008DF30 File Offset: 0x0008C130
	public static void SetRoomVisible(BiomeType biome, int roomIndex, bool isVisible, bool showReenactment = false)
	{
		biome = BiomeType_RL.GetGroupedBiomeType(biome);
		if (MapController.m_instance.m_mapRoomEntryDict != null && MapController.m_instance.m_mapRoomEntryDict.ContainsKey(biome))
		{
			MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(biome, roomIndex);
			if (!mapRoomEntry)
			{
				BaseRoom room = WorldBuilder.GetBiomeController(biome).GetRoom(roomIndex);
				GridPointManager gridPointManager;
				if (room is Room)
				{
					gridPointManager = (room as Room).GridPointManager;
				}
				else
				{
					gridPointManager = (room as MergeRoom).StandaloneGridPointManagers[0];
				}
				mapRoomEntry = MapController.CreateMapRoomEntryForGridPoint(gridPointManager);
				MapController.m_instance.m_mapRoomEntryDict[biome].Add(gridPointManager.BiomeControllerIndex, mapRoomEntry);
				Debug.Log(string.Concat(new string[]
				{
					"<color=red>WARNING: Map room entry: ",
					roomIndex.ToString(),
					" in biome: ",
					biome.ToString(),
					" could not be found in map controller. Creating a new one while setting room visible, but this really shouldn't happen.</color>"
				}));
			}
			if (mapRoomEntry)
			{
				if (!showReenactment)
				{
					BaseRoom room2;
					if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
					{
						room2 = OnPlayManager.BiomeController.GetRoom(roomIndex);
					}
					else
					{
						room2 = WorldBuilder.GetBiomeController(biome).GetRoom(roomIndex);
					}
					if (mapRoomEntry && room2)
					{
						mapRoomEntry.gameObject.SetActive(isVisible);
						mapRoomEntry.ToggleTerrainVisibility(isVisible);
						mapRoomEntry.ToggleIconVisibility(MapIconType.Door, -1, isVisible);
						mapRoomEntry.WasVisited = isVisible;
						if (!isVisible)
						{
							mapRoomEntry.ToggleIconVisibility(MapIconType.Enemy, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.EnemyKilled, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.ChestClosed, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.ChestOpened, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoom, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.Teleporter, -1, false);
							return;
						}
						if (TraitManager.IsTraitActive(TraitType.ShowEnemiesOnMap))
						{
							EnemySpawnController[] enemySpawnControllers = room2.SpawnControllerManager.EnemySpawnControllers;
							for (int i = 0; i < enemySpawnControllers.Length; i++)
							{
								EnemySpawnController enemySpawnController = enemySpawnControllers[i];
								bool flag = enemySpawnController.EnemyInstance == null || (enemySpawnController.EnemyInstance != null && enemySpawnController.EnemyInstance.IsDead);
								if (enemySpawnController.ShouldSpawn && !flag)
								{
									mapRoomEntry.ToggleIconVisibility(MapIconType.Enemy, i, true);
								}
								else
								{
									mapRoomEntry.ToggleIconVisibility(MapIconType.Enemy, i, false);
								}
							}
						}
						else
						{
							mapRoomEntry.ToggleIconVisibility(MapIconType.Enemy, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.EnemyKilled, -1, false);
						}
						if (room2.RoomType != RoomType.Fairy && room2.RoomType != RoomType.Bonus)
						{
							ChestSpawnController[] chestSpawnControllers = room2.SpawnControllerManager.ChestSpawnControllers;
							for (int j = 0; j < chestSpawnControllers.Length; j++)
							{
								ChestSpawnController chestSpawnController = chestSpawnControllers[j];
								bool visible = false;
								bool visible2 = false;
								if (chestSpawnController.ShouldSpawn)
								{
									if (chestSpawnController.ChestInstance.IsOpen)
									{
										visible = true;
									}
									else
									{
										visible2 = true;
									}
								}
								mapRoomEntry.ToggleIconVisibility(MapIconType.ChestOpened, j, visible);
								mapRoomEntry.ToggleIconVisibility(MapIconType.ChestClosed, j, visible2);
							}
						}
						else
						{
							mapRoomEntry.ToggleIconVisibility(MapIconType.ChestOpened, -1, false);
							mapRoomEntry.ToggleIconVisibility(MapIconType.ChestClosed, -1, false);
						}
						RoomType roomType = mapRoomEntry.RoomType;
						if (roomType <= RoomType.BossEntrance)
						{
							if (roomType != RoomType.Standard)
							{
								if (roomType != RoomType.Fairy && roomType != RoomType.BossEntrance)
								{
									goto IL_432;
								}
							}
							else if (!mapRoomEntry.IsMergeRoomEntry)
							{
								goto IL_432;
							}
						}
						else if (roomType != RoomType.Boss && roomType != RoomType.Bonus && roomType != RoomType.Mandatory)
						{
							goto IL_432;
						}
						if (mapRoomEntry.RoomCompleteBiomeControllerIndexOverrides == null || mapRoomEntry.RoomCompleteBiomeControllerIndexOverrides.Length == 0)
						{
							BaseSpecialRoomController componentInChildren = room2.gameObject.GetComponentInChildren<BaseSpecialRoomController>();
							if (!componentInChildren || !componentInChildren.IsRoomComplete)
							{
								mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoom, -1, true);
								mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
							}
							else
							{
								mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoom, -1, false);
								mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, true);
							}
						}
						else
						{
							RoomSaveData roomSaveData = SaveManager.StageSaveData.GetRoomSaveData(biome, roomIndex);
							if (!roomSaveData.IsNativeNull() && roomSaveData.RoomCompleteBiomeControllerIndexOverrides == null)
							{
								roomSaveData.RoomCompleteBiomeControllerIndexOverrides = mapRoomEntry.RoomCompleteBiomeControllerIndexOverrides;
							}
							for (int k = 0; k < mapRoomEntry.RoomCompleteBiomeControllerIndexOverrides.Length; k++)
							{
								int biomeControllerIndex = mapRoomEntry.RoomCompleteBiomeControllerIndexOverrides[k];
								RoomSaveData roomSaveData2 = SaveManager.StageSaveData.GetRoomSaveData(biome, biomeControllerIndex);
								bool flag2;
								if (!roomSaveData2.IsNativeNull())
								{
									flag2 = roomSaveData2.IsRoomComplete;
								}
								else
								{
									BaseRoom room3;
									if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
									{
										room3 = OnPlayManager.BiomeController.GetRoom(biomeControllerIndex);
									}
									else
									{
										room3 = WorldBuilder.GetBiomeController(biome).GetRoom(biomeControllerIndex);
									}
									BaseSpecialRoomController componentInChildren2 = room3.gameObject.GetComponentInChildren<BaseSpecialRoomController>();
									flag2 = (componentInChildren2 && componentInChildren2.IsRoomComplete);
								}
								if (!flag2)
								{
									mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoom, k, true);
									mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, k, false);
								}
								else
								{
									mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoom, k, false);
									mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialRoomUsed, k, true);
								}
							}
						}
						IL_432:
						mapRoomEntry.ToggleIconVisibility(MapIconType.Teleporter, -1, !mapRoomEntry.HasSpecialRoomIcon);
						if (mapRoomEntry.HasSpecialRoomIcon || mapRoomEntry.HasTeleporterIcon)
						{
							mapRoomEntry.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
							return;
						}
					}
				}
				else if (mapRoomEntry)
				{
					mapRoomEntry.gameObject.SetActive(isVisible);
					mapRoomEntry.ToggleTerrainVisibility(isVisible);
					mapRoomEntry.ToggleIconVisibility(MapIconType.Door, -1, isVisible);
					return;
				}
			}
			else if (!GameUtility.IsInLevelEditor)
			{
				Debug.LogFormat("<color=red>| {0} | {1} Biome's Map Room Dictionary doesn't contain an entry for Biome Controller Index ({2})</color>", new object[]
				{
					MapController.m_instance,
					biome,
					roomIndex
				});
				return;
			}
		}
		else if (!GameUtility.IsInLevelEditor)
		{
			Debug.LogFormat("<color=red>| {0} | Map Room Dictionary is null or Key ({1}) wasn't present in Dictionary</color>", new object[]
			{
				MapController.m_instance,
				biome
			});
		}
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x0008E470 File Offset: 0x0008C670
	public static void SetPlayerIconInRoom(BiomeType biome, int biomeControllerIndex, Vector2 worldPosition = default(Vector2))
	{
		if (!MapController.m_playerIcon.activeInHierarchy)
		{
			MapController.m_playerIcon.SetActive(true);
		}
		MapRoomEntry mapRoomEntry = MapController.GetMapRoomEntry(biome, biomeControllerIndex);
		if (mapRoomEntry != null)
		{
			Vector3 position = (worldPosition == default(Vector2)) ? mapRoomEntry.AbsBounds.center : new Vector2(MapController.GetMapPositionFromWorld(worldPosition, true).x, mapRoomEntry.AbsBounds.center.y);
			position.z = 0.9f;
			MapController.m_playerIcon.transform.position = position;
		}
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x0008E511 File Offset: 0x0008C711
	public static void SetMapCameraPosition(Vector3 position)
	{
		MapController.m_instance.m_miniMapCamera.transform.position = new Vector3(position.x, position.y, MapController.m_instance.m_miniMapCamera.transform.position.z);
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x0008E551 File Offset: 0x0008C751
	public static void TweenCameraToPlayer(float duration)
	{
		MapController.TweenCameraToPosition(MapController.m_instance.m_mapPlayerIconPrefab.transform.position, duration, true);
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x0008E578 File Offset: 0x0008C778
	public static void TweenCameraToPosition(Vector3 position, float duration, bool lockInput)
	{
		if (MapController.m_instance.m_tweenCameraCoroutine != null)
		{
			MapController.m_instance.StopCoroutine(MapController.m_instance.m_tweenCameraCoroutine);
		}
		if (duration > 0f)
		{
			MapController.m_instance.m_tweenCameraCoroutine = MapController.m_instance.StartCoroutine(MapController.m_instance.TweenCameraCoroutine(position, duration, lockInput));
			return;
		}
		MapController.SetMapCameraPosition(position);
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x0008E5DF File Offset: 0x0008C7DF
	private IEnumerator TweenCameraCoroutine(Vector3 position, float duration, bool lockInput)
	{
		if (lockInput)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
		}
		yield return TweenManager.TweenTo_UnscaledTime(MapController.m_instance.m_miniMapCamera.transform, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"position.x",
			position.x,
			"position.y",
			position.y,
			"position.z",
			0
		}).TweenCoroutine;
		if (lockInput)
		{
			RewiredMapController.SetCurrentMapEnabled(true);
		}
		yield break;
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x0008E5FC File Offset: 0x0008C7FC
	public static void UpdatePlayerIconPosition()
	{
		if (PlayerManager.IsInstantiated)
		{
			Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(PlayerManager.GetPlayerController().Midpoint, true);
			mapPositionFromWorld.z = 0.9f;
			MapController.m_playerIcon.transform.position = mapPositionFromWorld;
		}
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x0008E63D File Offset: 0x0008C83D
	public static IEnumerator TeleportPlayerToRoom_Coroutine(BiomeType biomeType, GridPointManager gridPointManager, bool runExitAnim = true)
	{
		PlayerManager.GetCurrentPlayerRoom().PlayerExit(null);
		BiomeType originBiome = PlayerManager.GetCurrentPlayerRoom().BiomeType;
		if (originBiome != biomeType)
		{
			yield return BiomeTransitionController.BiomeTransitionCoroutine(originBiome, biomeType);
			BiomeTransitionController.DestroyOldCastleTransitionRoom(biomeType);
			BiomeTransitionController.DestroyBiomeRoomInstances(originBiome);
		}
		BiomeController biomeController = null;
		if (WorldBuilder.IsInstantiated)
		{
			biomeController = WorldBuilder.GetBiomeController(biomeType);
		}
		else if (GameUtility.IsInLevelEditor)
		{
			biomeController = OnPlayManager.BiomeController;
		}
		if (biomeController != null)
		{
			BaseRoom room = biomeController.GetRoom(gridPointManager.BiomeControllerIndex);
			GlobalTeleporterController globalTeleporterController;
			if (room is Room)
			{
				globalTeleporterController = (room as Room).GlobalTeleporter;
			}
			else
			{
				globalTeleporterController = (room as MergeRoom).GlobalTeleporters[gridPointManager];
			}
			if (globalTeleporterController)
			{
				Vector3 position = globalTeleporterController.transform.position;
				Vector3 localPosition = room.gameObject.transform.InverseTransformPoint(position);
				room.PlacePlayerInRoom(localPosition);
				if (runExitAnim)
				{
					globalTeleporterController.OnExitTeleportPlayer();
				}
			}
		}
		yield break;
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x0008E65A File Offset: 0x0008C85A
	public static void SetTeleporterLineVisible(bool visible)
	{
		if (MapController.m_teleporterLine.gameObject.activeSelf != visible)
		{
			MapController.m_teleporterLine.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06002A6A RID: 10858 RVA: 0x0008E67E File Offset: 0x0008C87E
	public static void SetPlayerIconVisible(bool visible)
	{
		MapController.m_playerIcon.gameObject.SetActive(visible);
	}

	// Token: 0x04002285 RID: 8837
	private const float ICON_Y_POSITION_OFFSET = 2f;

	// Token: 0x04002286 RID: 8838
	[SerializeField]
	private CameraFollowMode m_followMode = CameraFollowMode.Snap;

	// Token: 0x04002287 RID: 8839
	[SerializeField]
	private Camera m_miniMapCamera;

	// Token: 0x04002288 RID: 8840
	[SerializeField]
	private Camera m_mapWindowCamera;

	// Token: 0x04002289 RID: 8841
	[SerializeField]
	private Camera m_deathMapCamera;

	// Token: 0x0400228A RID: 8842
	[Header("Map Entry Prefab")]
	[SerializeField]
	private MapRoomEntry m_mapEntryPrefab;

	// Token: 0x0400228B RID: 8843
	[Header("Map Icon Animations")]
	[SerializeField]
	private AnimationCurve m_animCurve;

	// Token: 0x0400228C RID: 8844
	[SerializeField]
	private float m_animLoopdelay = 1f;

	// Token: 0x0400228D RID: 8845
	[Header("Player Icon")]
	[SerializeField]
	private GameObject m_mapPlayerIconPrefab;

	// Token: 0x0400228E RID: 8846
	[Header("Door Icons")]
	[SerializeField]
	private GameObject m_mapHorizontalDoor;

	// Token: 0x0400228F RID: 8847
	[SerializeField]
	private GameObject m_mapVerticalDoor;

	// Token: 0x04002290 RID: 8848
	[Header("Enemy Icons")]
	[SerializeField]
	private GameObject m_mapEnemyIcon;

	// Token: 0x04002291 RID: 8849
	[SerializeField]
	private GameObject m_mapEnemyKilledIcon;

	// Token: 0x04002292 RID: 8850
	[Header("Teleporter Icons")]
	[SerializeField]
	private GameObject m_mapTeleporterIcon;

	// Token: 0x04002293 RID: 8851
	[SerializeField]
	private SpriteRenderer m_mapTeleporterLine;

	// Token: 0x04002294 RID: 8852
	[Header("Chest Icons")]
	[SerializeField]
	private GameObject m_mapChestClosedIconPrefab;

	// Token: 0x04002295 RID: 8853
	[SerializeField]
	private GameObject m_mapChestOpenIconPrefab;

	// Token: 0x04002296 RID: 8854
	[SerializeField]
	private GameObject m_mapFairyChestClosedIconPrefab;

	// Token: 0x04002297 RID: 8855
	[SerializeField]
	private GameObject m_mapFairyChestOpenedIconPrefab;

	// Token: 0x04002298 RID: 8856
	[Header("Special Room Icons")]
	[SerializeField]
	private GameObject m_specialRoomIconPrefab;

	// Token: 0x04002299 RID: 8857
	[SerializeField]
	private GameObject m_specialRoomUsedIconPrefab;

	// Token: 0x0400229A RID: 8858
	[SerializeField]
	private GameObject m_heirloomRoomIconPrefab;

	// Token: 0x0400229B RID: 8859
	[SerializeField]
	private GameObject m_heirloomRoomCompleteIconPrefab;

	// Token: 0x0400229C RID: 8860
	[Header("Boss Icons")]
	[SerializeField]
	private GameObject m_bossIconPrefab;

	// Token: 0x0400229D RID: 8861
	[SerializeField]
	private GameObject m_bossBeatenIconPrefab;

	// Token: 0x0400229E RID: 8862
	[SerializeField]
	private GameObject m_subbossIconPrefab;

	// Token: 0x0400229F RID: 8863
	[SerializeField]
	private GameObject m_subbossBeatenIconPrefab;

	// Token: 0x040022A0 RID: 8864
	[Header("NPC Icons")]
	[SerializeField]
	private GameObject m_npcRoomIconPrefab;

	// Token: 0x040022A1 RID: 8865
	[SerializeField]
	private GameObject m_npcRoomUsedIconPrefab;

	// Token: 0x040022A2 RID: 8866
	[Header("Special Indicator Icon")]
	[SerializeField]
	private GameObject m_specialIndicatorIconPrefab;

	// Token: 0x040022A3 RID: 8867
	public const float ROOM_ABS_SHRINK_AMOUNT = 0.06f;

	// Token: 0x040022A4 RID: 8868
	private const string ENEMY_LOCATION_NAME = "Enemy Objects";

	// Token: 0x040022A5 RID: 8869
	private const string ENEMIES_KILLED_LOCATION_NAME = "Enemies Killed";

	// Token: 0x040022A6 RID: 8870
	private const float FOLLOW_IF_X_OR_Y_ARE_LESS_THAN = 0.25f;

	// Token: 0x040022A7 RID: 8871
	private const float FOLLOW_IF_X_OR_Y_ARE_GREATER_THAN = 0.75f;

	// Token: 0x040022A8 RID: 8872
	public static Vector2 MAP_STARTING_POSITION = new Vector2(-100000f, -100000f);

	// Token: 0x040022A9 RID: 8873
	public const float FULL_MAP_CAMERA_SIZE = 9f;

	// Token: 0x040022AA RID: 8874
	public const float HUD_MAP_CAMERA_SIZE = 2f;

	// Token: 0x040022AB RID: 8875
	private Vector3 m_cameraDestination = new Vector3(-1f, -1f, -1f);

	// Token: 0x040022AC RID: 8876
	private Coroutine m_tweenCameraCoroutine;

	// Token: 0x040022AD RID: 8877
	private static bool m_isCameraFollowOn = false;

	// Token: 0x040022AE RID: 8878
	private static Vector3 m_previousPlayerIconPosition;

	// Token: 0x040022AF RID: 8879
	private static MapController m_instance = null;

	// Token: 0x040022B0 RID: 8880
	private static GameObject m_playerIcon;

	// Token: 0x040022B1 RID: 8881
	private static SpriteRenderer m_teleporterLine;

	// Token: 0x040022B2 RID: 8882
	private static GameObject m_mapObj;

	// Token: 0x040022B3 RID: 8883
	private Dictionary<BiomeType, Dictionary<int, MapRoomEntry>> m_mapRoomEntryDict = new Dictionary<BiomeType, Dictionary<int, MapRoomEntry>>();

	// Token: 0x040022B4 RID: 8884
	private static bool m_isInitialized = false;

	// Token: 0x040022B5 RID: 8885
	private List<GlobalTeleporterController> m_teleporterList = new List<GlobalTeleporterController>();

	// Token: 0x040022B6 RID: 8886
	private List<GridPointManager> m_gridPointManagersContainingTeleporters = new List<GridPointManager>();

	// Token: 0x040022B7 RID: 8887
	private static Transform m_enemyKilledLocation = null;

	// Token: 0x040022B8 RID: 8888
	private Rect m_globalMapBounds;

	// Token: 0x040022B9 RID: 8889
	private Dictionary<BiomeType, Rect> m_biomeMapBoundsTable = new Dictionary<BiomeType, Rect>();

	// Token: 0x040022BA RID: 8890
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x040022BB RID: 8891
	private Action<MonoBehaviour, EventArgs> m_onBiomeEnter;

	// Token: 0x040022BC RID: 8892
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x040022BD RID: 8893
	private Action<MonoBehaviour, EventArgs> m_onUpdateCurrentRoomIcons;

	// Token: 0x040022BE RID: 8894
	private Action<MonoBehaviour, EventArgs> m_linkTunnelBonusRooms;

	// Token: 0x040022C1 RID: 8897
	private Dictionary<BiomeType, List<MapRoomEntry>> m_mapEntriesThatNeedLinking = new Dictionary<BiomeType, List<MapRoomEntry>>();

	// Token: 0x040022C2 RID: 8898
	private List<TunnelSpawnController> m_mapEntryTunnelSpawners = new List<TunnelSpawnController>();

	// Token: 0x040022C3 RID: 8899
	private const string TOWER_EXTERIOR_NAME_CHECK = "Exterior";
}

using System;
using System.Collections;
using System.Collections.Generic;
using RLWorldCreation;
using RL_Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200077B RID: 1915
public class MapController : MonoBehaviour
{
	// Token: 0x06003A3D RID: 14909 RVA: 0x000ED838 File Offset: 0x000EBA38
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

	// Token: 0x06003A3E RID: 14910 RVA: 0x000ED958 File Offset: 0x000EBB58
	public static Rect GetBiomeRect(BiomeType biomeType)
	{
		Rect result;
		if (MapController.m_instance.m_biomeMapBoundsTable.TryGetValue(biomeType, out result))
		{
			return result;
		}
		return Rect.zero;
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x000ED980 File Offset: 0x000EBB80
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

	// Token: 0x17001597 RID: 5527
	// (get) Token: 0x06003A40 RID: 14912 RVA: 0x00020025 File Offset: 0x0001E225
	public static List<GlobalTeleporterController> TeleporterList
	{
		get
		{
			return MapController.m_instance.m_teleporterList;
		}
	}

	// Token: 0x17001598 RID: 5528
	// (get) Token: 0x06003A41 RID: 14913 RVA: 0x00020031 File Offset: 0x0001E231
	// (set) Token: 0x06003A42 RID: 14914 RVA: 0x00020038 File Offset: 0x0001E238
	public static bool IsCameraMoving { get; private set; }

	// Token: 0x17001599 RID: 5529
	// (get) Token: 0x06003A43 RID: 14915 RVA: 0x00020040 File Offset: 0x0001E240
	// (set) Token: 0x06003A44 RID: 14916 RVA: 0x00020047 File Offset: 0x0001E247
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

	// Token: 0x06003A45 RID: 14917 RVA: 0x000ED9DC File Offset: 0x000EBBDC
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

	// Token: 0x1700159A RID: 5530
	// (get) Token: 0x06003A46 RID: 14918 RVA: 0x0002004F File Offset: 0x0001E24F
	public static Camera Camera
	{
		get
		{
			return MapController.m_instance.m_miniMapCamera;
		}
	}

	// Token: 0x1700159B RID: 5531
	// (get) Token: 0x06003A47 RID: 14919 RVA: 0x0002005B File Offset: 0x0001E25B
	public static Camera MapWindowCamera
	{
		get
		{
			return MapController.m_instance.m_mapWindowCamera;
		}
	}

	// Token: 0x1700159C RID: 5532
	// (get) Token: 0x06003A48 RID: 14920 RVA: 0x00020067 File Offset: 0x0001E267
	public static Camera DeathMapCamera
	{
		get
		{
			return MapController.m_instance.m_deathMapCamera;
		}
	}

	// Token: 0x1700159D RID: 5533
	// (get) Token: 0x06003A49 RID: 14921 RVA: 0x00020073 File Offset: 0x0001E273
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

	// Token: 0x1700159E RID: 5534
	// (get) Token: 0x06003A4A RID: 14922 RVA: 0x0002009B File Offset: 0x0001E29B
	// (set) Token: 0x06003A4B RID: 14923 RVA: 0x000200A7 File Offset: 0x0001E2A7
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

	// Token: 0x1700159F RID: 5535
	// (get) Token: 0x06003A4C RID: 14924 RVA: 0x000200B4 File Offset: 0x0001E2B4
	// (set) Token: 0x06003A4D RID: 14925 RVA: 0x000200C0 File Offset: 0x0001E2C0
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

	// Token: 0x170015A0 RID: 5536
	// (get) Token: 0x06003A4E RID: 14926 RVA: 0x000200CD File Offset: 0x0001E2CD
	// (set) Token: 0x06003A4F RID: 14927 RVA: 0x000200D4 File Offset: 0x0001E2D4
	public static float MapIconAnimationCurveValue { get; private set; }

	// Token: 0x06003A50 RID: 14928 RVA: 0x000EDA04 File Offset: 0x000EBC04
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

	// Token: 0x06003A51 RID: 14929 RVA: 0x000EDAF8 File Offset: 0x000EBCF8
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

	// Token: 0x06003A52 RID: 14930 RVA: 0x000200DC File Offset: 0x0001E2DC
	private void OnEnable()
	{
		base.StartCoroutine(this.MapIconAnimationCoroutine());
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x000200EB File Offset: 0x0001E2EB
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

	// Token: 0x06003A54 RID: 14932 RVA: 0x000EDC08 File Offset: 0x000EBE08
	public static Vector3 GetWorldPositionFromMap(Vector3 position)
	{
		Vector3 vector = position;
		vector /= 0.06666667f;
		vector.z = 1f;
		vector.x -= MapController.MAP_STARTING_POSITION.x;
		vector.y -= MapController.MAP_STARTING_POSITION.y;
		return vector;
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x000EDC5C File Offset: 0x000EBE5C
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

	// Token: 0x06003A56 RID: 14934 RVA: 0x000EDDEC File Offset: 0x000EBFEC
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

	// Token: 0x06003A57 RID: 14935 RVA: 0x000200FA File Offset: 0x0001E2FA
	public static void CentreCameraAroundPlayerIcon()
	{
		MapController.SetMapCameraPosition(MapController.PlayerIconPosition);
	}

	// Token: 0x06003A58 RID: 14936 RVA: 0x000EDEEC File Offset: 0x000EC0EC
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

	// Token: 0x06003A59 RID: 14937 RVA: 0x00020110 File Offset: 0x0001E310
	public static Vector3 GetScaledPoint(Vector2 point)
	{
		return MapController.GetScaledPoint(Vector2.zero, point);
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x0002011D File Offset: 0x0001E31D
	public static Vector3 GetScaledPoint(Vector2 center, float xPosition, float yPosition)
	{
		return MapController.GetScaledPoint(center, new Vector2(xPosition, yPosition));
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x000EDFA4 File Offset: 0x000EC1A4
	public static Vector3 GetScaledPoint(Vector2 center, Vector2 point)
	{
		float x = 0.06666667f * (point.x - center.x) + center.x;
		float y = 0.06666667f * (point.y - center.y) + center.y;
		return new Vector3(x, y, 1f);
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x000EDFF4 File Offset: 0x000EC1F4
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

	// Token: 0x06003A5D RID: 14941 RVA: 0x000EE108 File Offset: 0x000EC308
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

	// Token: 0x06003A5E RID: 14942 RVA: 0x000EE3C4 File Offset: 0x000EC5C4
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

	// Token: 0x06003A5F RID: 14943 RVA: 0x000EE678 File Offset: 0x000EC878
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

	// Token: 0x06003A60 RID: 14944 RVA: 0x000EE7AC File Offset: 0x000EC9AC
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

	// Token: 0x06003A61 RID: 14945 RVA: 0x000EEAB8 File Offset: 0x000ECCB8
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

	// Token: 0x06003A62 RID: 14946 RVA: 0x0002012C File Offset: 0x0001E32C
	private static void CreateTeleporterIconsForGridPoint(GridPointManager room, MapRoomEntry mapRoomEntry)
	{
		if (mapRoomEntry.CreateTeleporterIconForGridPoint(MapController.m_instance.m_mapTeleporterIcon, room))
		{
			MapController.GridPointManagersContainingTeleporters.Add(room);
		}
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x000EEB18 File Offset: 0x000ECD18
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

	// Token: 0x06003A64 RID: 14948 RVA: 0x0002014C File Offset: 0x0001E34C
	public static void DisplayAllRooms()
	{
		MapController.SetAllBiomeVisibility(true, true, false);
	}

	// Token: 0x06003A65 RID: 14949 RVA: 0x000EEB94 File Offset: 0x000ECD94
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

	// Token: 0x06003A66 RID: 14950 RVA: 0x000EEBEC File Offset: 0x000ECDEC
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

	// Token: 0x06003A67 RID: 14951 RVA: 0x000EEC40 File Offset: 0x000ECE40
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

	// Token: 0x06003A68 RID: 14952 RVA: 0x000EED24 File Offset: 0x000ECF24
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

	// Token: 0x06003A69 RID: 14953 RVA: 0x000EEE1C File Offset: 0x000ED01C
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

	// Token: 0x06003A6A RID: 14954 RVA: 0x000EEE64 File Offset: 0x000ED064
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

	// Token: 0x06003A6B RID: 14955 RVA: 0x000EEF74 File Offset: 0x000ED174
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

	// Token: 0x06003A6C RID: 14956 RVA: 0x000EF028 File Offset: 0x000ED228
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

	// Token: 0x06003A6D RID: 14957 RVA: 0x000EF0EC File Offset: 0x000ED2EC
	private void OnUpdateCurrentRoomIcons(object sender, EventArgs args)
	{
		if (PlayerManager.IsInstantiated)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			MapController.SetRoomVisible(currentPlayerRoom.BiomeType, currentPlayerRoom.BiomeControllerIndex, true, false);
		}
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x00020156 File Offset: 0x0001E356
	private void OnActiveSceneChanged(Scene current, Scene next)
	{
		MapController.GridPointManagersContainingTeleporters.Clear();
	}

	// Token: 0x06003A6F RID: 14959 RVA: 0x000EF11C File Offset: 0x000ED31C
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

	// Token: 0x06003A70 RID: 14960 RVA: 0x00020162 File Offset: 0x0001E362
	private void ResetCameraPosition()
	{
		MapController.SetMapCameraPosition(MapController.MAP_STARTING_POSITION);
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x000EF234 File Offset: 0x000ED434
	public static void SetCameraFollowIsOn(bool isOn)
	{
		MapController.m_isCameraFollowOn = isOn;
		if (isOn)
		{
			MapController.m_instance.m_cameraDestination = new Vector3(MapController.PlayerIconPosition.x, MapController.PlayerIconPosition.y, MapController.Camera.transform.position.z);
		}
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x000EF284 File Offset: 0x000ED484
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

	// Token: 0x06003A73 RID: 14963 RVA: 0x000EF448 File Offset: 0x000ED648
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

	// Token: 0x06003A74 RID: 14964 RVA: 0x000EF540 File Offset: 0x000ED740
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

	// Token: 0x06003A75 RID: 14965 RVA: 0x000EF6BC File Offset: 0x000ED8BC
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

	// Token: 0x06003A76 RID: 14966 RVA: 0x000EF72C File Offset: 0x000ED92C
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

	// Token: 0x06003A77 RID: 14967 RVA: 0x000EF964 File Offset: 0x000EDB64
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

	// Token: 0x06003A78 RID: 14968 RVA: 0x000EFEA4 File Offset: 0x000EE0A4
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

	// Token: 0x06003A79 RID: 14969 RVA: 0x00020173 File Offset: 0x0001E373
	public static void SetMapCameraPosition(Vector3 position)
	{
		MapController.m_instance.m_miniMapCamera.transform.position = new Vector3(position.x, position.y, MapController.m_instance.m_miniMapCamera.transform.position.z);
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x000201B3 File Offset: 0x0001E3B3
	public static void TweenCameraToPlayer(float duration)
	{
		MapController.TweenCameraToPosition(MapController.m_instance.m_mapPlayerIconPrefab.transform.position, duration, true);
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x000EFF48 File Offset: 0x000EE148
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

	// Token: 0x06003A7C RID: 14972 RVA: 0x000201DA File Offset: 0x0001E3DA
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

	// Token: 0x06003A7D RID: 14973 RVA: 0x000EFFB0 File Offset: 0x000EE1B0
	public static void UpdatePlayerIconPosition()
	{
		if (PlayerManager.IsInstantiated)
		{
			Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(PlayerManager.GetPlayerController().Midpoint, true);
			mapPositionFromWorld.z = 0.9f;
			MapController.m_playerIcon.transform.position = mapPositionFromWorld;
		}
	}

	// Token: 0x06003A7E RID: 14974 RVA: 0x000201F7 File Offset: 0x0001E3F7
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

	// Token: 0x06003A7F RID: 14975 RVA: 0x00020214 File Offset: 0x0001E414
	public static void SetTeleporterLineVisible(bool visible)
	{
		if (MapController.m_teleporterLine.gameObject.activeSelf != visible)
		{
			MapController.m_teleporterLine.gameObject.SetActive(visible);
		}
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x00020238 File Offset: 0x0001E438
	public static void SetPlayerIconVisible(bool visible)
	{
		MapController.m_playerIcon.gameObject.SetActive(visible);
	}

	// Token: 0x04002E6F RID: 11887
	private const float ICON_Y_POSITION_OFFSET = 2f;

	// Token: 0x04002E70 RID: 11888
	[SerializeField]
	private CameraFollowMode m_followMode = CameraFollowMode.Snap;

	// Token: 0x04002E71 RID: 11889
	[SerializeField]
	private Camera m_miniMapCamera;

	// Token: 0x04002E72 RID: 11890
	[SerializeField]
	private Camera m_mapWindowCamera;

	// Token: 0x04002E73 RID: 11891
	[SerializeField]
	private Camera m_deathMapCamera;

	// Token: 0x04002E74 RID: 11892
	[Header("Map Entry Prefab")]
	[SerializeField]
	private MapRoomEntry m_mapEntryPrefab;

	// Token: 0x04002E75 RID: 11893
	[Header("Map Icon Animations")]
	[SerializeField]
	private AnimationCurve m_animCurve;

	// Token: 0x04002E76 RID: 11894
	[SerializeField]
	private float m_animLoopdelay = 1f;

	// Token: 0x04002E77 RID: 11895
	[Header("Player Icon")]
	[SerializeField]
	private GameObject m_mapPlayerIconPrefab;

	// Token: 0x04002E78 RID: 11896
	[Header("Door Icons")]
	[SerializeField]
	private GameObject m_mapHorizontalDoor;

	// Token: 0x04002E79 RID: 11897
	[SerializeField]
	private GameObject m_mapVerticalDoor;

	// Token: 0x04002E7A RID: 11898
	[Header("Enemy Icons")]
	[SerializeField]
	private GameObject m_mapEnemyIcon;

	// Token: 0x04002E7B RID: 11899
	[SerializeField]
	private GameObject m_mapEnemyKilledIcon;

	// Token: 0x04002E7C RID: 11900
	[Header("Teleporter Icons")]
	[SerializeField]
	private GameObject m_mapTeleporterIcon;

	// Token: 0x04002E7D RID: 11901
	[SerializeField]
	private SpriteRenderer m_mapTeleporterLine;

	// Token: 0x04002E7E RID: 11902
	[Header("Chest Icons")]
	[SerializeField]
	private GameObject m_mapChestClosedIconPrefab;

	// Token: 0x04002E7F RID: 11903
	[SerializeField]
	private GameObject m_mapChestOpenIconPrefab;

	// Token: 0x04002E80 RID: 11904
	[SerializeField]
	private GameObject m_mapFairyChestClosedIconPrefab;

	// Token: 0x04002E81 RID: 11905
	[SerializeField]
	private GameObject m_mapFairyChestOpenedIconPrefab;

	// Token: 0x04002E82 RID: 11906
	[Header("Special Room Icons")]
	[SerializeField]
	private GameObject m_specialRoomIconPrefab;

	// Token: 0x04002E83 RID: 11907
	[SerializeField]
	private GameObject m_specialRoomUsedIconPrefab;

	// Token: 0x04002E84 RID: 11908
	[SerializeField]
	private GameObject m_heirloomRoomIconPrefab;

	// Token: 0x04002E85 RID: 11909
	[SerializeField]
	private GameObject m_heirloomRoomCompleteIconPrefab;

	// Token: 0x04002E86 RID: 11910
	[Header("Boss Icons")]
	[SerializeField]
	private GameObject m_bossIconPrefab;

	// Token: 0x04002E87 RID: 11911
	[SerializeField]
	private GameObject m_bossBeatenIconPrefab;

	// Token: 0x04002E88 RID: 11912
	[SerializeField]
	private GameObject m_subbossIconPrefab;

	// Token: 0x04002E89 RID: 11913
	[SerializeField]
	private GameObject m_subbossBeatenIconPrefab;

	// Token: 0x04002E8A RID: 11914
	[Header("NPC Icons")]
	[SerializeField]
	private GameObject m_npcRoomIconPrefab;

	// Token: 0x04002E8B RID: 11915
	[SerializeField]
	private GameObject m_npcRoomUsedIconPrefab;

	// Token: 0x04002E8C RID: 11916
	[Header("Special Indicator Icon")]
	[SerializeField]
	private GameObject m_specialIndicatorIconPrefab;

	// Token: 0x04002E8D RID: 11917
	public const float ROOM_ABS_SHRINK_AMOUNT = 0.06f;

	// Token: 0x04002E8E RID: 11918
	private const string ENEMY_LOCATION_NAME = "Enemy Objects";

	// Token: 0x04002E8F RID: 11919
	private const string ENEMIES_KILLED_LOCATION_NAME = "Enemies Killed";

	// Token: 0x04002E90 RID: 11920
	private const float FOLLOW_IF_X_OR_Y_ARE_LESS_THAN = 0.25f;

	// Token: 0x04002E91 RID: 11921
	private const float FOLLOW_IF_X_OR_Y_ARE_GREATER_THAN = 0.75f;

	// Token: 0x04002E92 RID: 11922
	public static Vector2 MAP_STARTING_POSITION = new Vector2(-100000f, -100000f);

	// Token: 0x04002E93 RID: 11923
	public const float FULL_MAP_CAMERA_SIZE = 9f;

	// Token: 0x04002E94 RID: 11924
	public const float HUD_MAP_CAMERA_SIZE = 2f;

	// Token: 0x04002E95 RID: 11925
	private Vector3 m_cameraDestination = new Vector3(-1f, -1f, -1f);

	// Token: 0x04002E96 RID: 11926
	private Coroutine m_tweenCameraCoroutine;

	// Token: 0x04002E97 RID: 11927
	private static bool m_isCameraFollowOn = false;

	// Token: 0x04002E98 RID: 11928
	private static Vector3 m_previousPlayerIconPosition;

	// Token: 0x04002E99 RID: 11929
	private static MapController m_instance = null;

	// Token: 0x04002E9A RID: 11930
	private static GameObject m_playerIcon;

	// Token: 0x04002E9B RID: 11931
	private static SpriteRenderer m_teleporterLine;

	// Token: 0x04002E9C RID: 11932
	private static GameObject m_mapObj;

	// Token: 0x04002E9D RID: 11933
	private Dictionary<BiomeType, Dictionary<int, MapRoomEntry>> m_mapRoomEntryDict = new Dictionary<BiomeType, Dictionary<int, MapRoomEntry>>();

	// Token: 0x04002E9E RID: 11934
	private static bool m_isInitialized = false;

	// Token: 0x04002E9F RID: 11935
	private List<GlobalTeleporterController> m_teleporterList = new List<GlobalTeleporterController>();

	// Token: 0x04002EA0 RID: 11936
	private List<GridPointManager> m_gridPointManagersContainingTeleporters = new List<GridPointManager>();

	// Token: 0x04002EA1 RID: 11937
	private static Transform m_enemyKilledLocation = null;

	// Token: 0x04002EA2 RID: 11938
	private Rect m_globalMapBounds;

	// Token: 0x04002EA3 RID: 11939
	private Dictionary<BiomeType, Rect> m_biomeMapBoundsTable = new Dictionary<BiomeType, Rect>();

	// Token: 0x04002EA4 RID: 11940
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002EA5 RID: 11941
	private Action<MonoBehaviour, EventArgs> m_onBiomeEnter;

	// Token: 0x04002EA6 RID: 11942
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04002EA7 RID: 11943
	private Action<MonoBehaviour, EventArgs> m_onUpdateCurrentRoomIcons;

	// Token: 0x04002EA8 RID: 11944
	private Action<MonoBehaviour, EventArgs> m_linkTunnelBonusRooms;

	// Token: 0x04002EAB RID: 11947
	private Dictionary<BiomeType, List<MapRoomEntry>> m_mapEntriesThatNeedLinking = new Dictionary<BiomeType, List<MapRoomEntry>>();

	// Token: 0x04002EAC RID: 11948
	private List<TunnelSpawnController> m_mapEntryTunnelSpawners = new List<TunnelSpawnController>();

	// Token: 0x04002EAD RID: 11949
	private const string TOWER_EXTERIOR_NAME_CHECK = "Exterior";
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using GameEventTracking;
using Rewired;
using RLAudio;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000B4B RID: 2891
public class SaveManager : MonoBehaviour
{
	// Token: 0x17001D63 RID: 7523
	// (get) Token: 0x060057A5 RID: 22437 RVA: 0x0002FB2B File Offset: 0x0002DD2B
	private static SaveManager Instance
	{
		get
		{
			return SaveManager.m_instance;
		}
	}

	// Token: 0x17001D64 RID: 7524
	// (get) Token: 0x060057A6 RID: 22438 RVA: 0x0002FB32 File Offset: 0x0002DD32
	// (set) Token: 0x060057A7 RID: 22439 RVA: 0x0002FB39 File Offset: 0x0002DD39
	public static bool IsRunning
	{
		get
		{
			return SaveManager.m_isRunning;
		}
		private set
		{
			SaveManager.m_isRunning = value;
		}
	}

	// Token: 0x17001D65 RID: 7525
	// (get) Token: 0x060057A8 RID: 22440 RVA: 0x0002FB41 File Offset: 0x0002DD41
	// (set) Token: 0x060057A9 RID: 22441 RVA: 0x0002FB48 File Offset: 0x0002DD48
	public static bool LoadingFailed { get; set; }

	// Token: 0x17001D66 RID: 7526
	// (get) Token: 0x060057AA RID: 22442 RVA: 0x0002FB50 File Offset: 0x0002DD50
	// (set) Token: 0x060057AB RID: 22443 RVA: 0x0002FB57 File Offset: 0x0002DD57
	public static float TimeSinceLastSave { get; private set; }

	// Token: 0x17001D67 RID: 7527
	// (get) Token: 0x060057AC RID: 22444 RVA: 0x0002FB5F File Offset: 0x0002DD5F
	// (set) Token: 0x060057AD RID: 22445 RVA: 0x0002FB66 File Offset: 0x0002DD66
	private static int FrameCountSinceLastSave { get; set; }

	// Token: 0x17001D68 RID: 7528
	// (get) Token: 0x060057AE RID: 22446 RVA: 0x0002FB6E File Offset: 0x0002DD6E
	public static int CurrentProfile
	{
		get
		{
			return (int)SaveManager.ConfigData.CurrentProfile;
		}
	}

	// Token: 0x17001D69 RID: 7529
	// (get) Token: 0x060057AF RID: 22447 RVA: 0x0002FB7A File Offset: 0x0002DD7A
	// (set) Token: 0x060057B0 RID: 22448 RVA: 0x0002FB81 File Offset: 0x0002DD81
	public static bool DisableSaving { get; set; }

	// Token: 0x17001D6A RID: 7530
	// (get) Token: 0x060057B1 RID: 22449 RVA: 0x0002FB89 File Offset: 0x0002DD89
	// (set) Token: 0x060057B2 RID: 22450 RVA: 0x0002FB90 File Offset: 0x0002DD90
	public static bool IsCopyingBackupFiles { get; set; }

	// Token: 0x17001D6B RID: 7531
	// (get) Token: 0x060057B3 RID: 22451 RVA: 0x0002FB98 File Offset: 0x0002DD98
	// (set) Token: 0x060057B4 RID: 22452 RVA: 0x0002FBA0 File Offset: 0x0002DDA0
	private BinaryFormatter BinaryFormatter { get; set; }

	// Token: 0x17001D6C RID: 7532
	// (get) Token: 0x060057B5 RID: 22453 RVA: 0x0002FBA9 File Offset: 0x0002DDA9
	// (set) Token: 0x060057B6 RID: 22454 RVA: 0x0002FBB0 File Offset: 0x0002DDB0
	private static CultureInfo ForcedCulture { get; set; }

	// Token: 0x17001D6D RID: 7533
	// (get) Token: 0x060057B7 RID: 22455 RVA: 0x0002FBB8 File Offset: 0x0002DDB8
	// (set) Token: 0x060057B8 RID: 22456 RVA: 0x0002FBC0 File Offset: 0x0002DDC0
	private bool Internal_IsInitialized { get; set; }

	// Token: 0x060057B9 RID: 22457 RVA: 0x0014D4A4 File Offset: 0x0014B6A4
	private static string GetSaveDirectoryPath(int profileSlot, bool getBackupDirectory)
	{
		string text = SaveFileSystem.PersistentDataPath;
		text = Path.Combine(text, "Saves");
		string platformDirectoryName = StoreAPIManager.GetPlatformDirectoryName();
		text = Path.Combine(text, platformDirectoryName);
		string userIDString = StoreAPIManager.GetUserIDString();
		if (!string.IsNullOrEmpty(userIDString))
		{
			text = Path.Combine(text, userIDString);
		}
		if (profileSlot != -1)
		{
			text = Path.Combine(text, "Profile" + profileSlot.ToString());
			if (getBackupDirectory)
			{
				text = Path.Combine(text, SaveFileSystem.BackupFolderName);
			}
		}
		return text;
	}

	// Token: 0x060057BA RID: 22458 RVA: 0x0014D514 File Offset: 0x0014B714
	private static void CreateBaseSaveDirectories()
	{
		string text = SaveFileSystem.PersistentDataPath;
		text = Path.Combine(text, "Saves");
		SaveFileSystem.CreateDirectory(text);
		string platformDirectoryName = StoreAPIManager.GetPlatformDirectoryName();
		if (!string.IsNullOrEmpty(platformDirectoryName))
		{
			text = Path.Combine(text, platformDirectoryName);
			SaveFileSystem.CreateDirectory(text);
		}
		string userIDString = StoreAPIManager.GetUserIDString();
		if (!string.IsNullOrEmpty(userIDString))
		{
			text = Path.Combine(text, userIDString);
			SaveFileSystem.CreateDirectory(text);
		}
		string path = text;
		for (int i = 0; i < 5; i++)
		{
			text = Path.Combine(path, "Profile" + i.ToString());
			SaveFileSystem.CreateDirectory(text);
		}
		Debug.Log("Created base save directories");
	}

	// Token: 0x060057BB RID: 22459 RVA: 0x0014D5AC File Offset: 0x0014B7AC
	private static string GetFullFilePath(int profileSlot, SaveDataType saveType, bool useBackupFileName)
	{
		string saveDirectoryPath = SaveManager.GetSaveDirectoryPath(profileSlot, useBackupFileName);
		string path;
		if (!useBackupFileName)
		{
			path = saveType.ToString() + ".rc2dat";
		}
		else
		{
			string str = DateTime.Now.ToString("dd_MM_yyyy__HH_mm_ss", SaveManager.ForcedCulture);
			path = saveType.ToString() + "_" + str + ".rc2dat";
		}
		return Path.Combine(saveDirectoryPath, path);
	}

	// Token: 0x060057BC RID: 22460 RVA: 0x0014D61C File Offset: 0x0014B81C
	private List<string> GetBackupFullFilePathes(int profileSlot, SaveDataType saveType)
	{
		List<string> list = this.m_backFilePathsArray[profileSlot];
		this.m_backupFilesListHelper.Clear();
		if (list != null)
		{
			string saveTypeString = saveType.ToString();
			foreach (string text in list)
			{
				if (this.IsValidBackupPath(text, saveTypeString))
				{
					this.m_backupFilesListHelper.Add(text);
				}
			}
			this.m_backupFilesListHelper.Sort(new Comparison<string>(this.DateTimeComparator));
			this.m_backupFilesListHelper.Reverse();
		}
		return this.m_backupFilesListHelper;
	}

	// Token: 0x060057BD RID: 22461 RVA: 0x0014D6C8 File Offset: 0x0014B8C8
	private bool IsValidBackupPath(string path, string saveTypeString)
	{
		if (!path.Contains(saveTypeString))
		{
			return false;
		}
		if (!path.Contains(".rc2dat"))
		{
			return false;
		}
		int length = "dd_MM_yyyy__HH_mm_ss".Length;
		int num = path.IndexOf(".rc2dat");
		DateTime dateTime;
		return DateTime.TryParseExact(path.Substring(num - length, length), "dd_MM_yyyy__HH_mm_ss", SaveManager.ForcedCulture, DateTimeStyles.None, out dateTime);
	}

	// Token: 0x060057BE RID: 22462 RVA: 0x0002FBC9 File Offset: 0x0002DDC9
	private void Awake()
	{
		if (SaveManager.m_instance == null)
		{
			SaveManager.m_instance = this;
			return;
		}
		throw new Exception("SaveManager has been instantiated twice.  This should never happen");
	}

	// Token: 0x060057BF RID: 22463 RVA: 0x0002FBE9 File Offset: 0x0002DDE9
	private IEnumerator Start()
	{
		while (StoreAPIManager.InitState != StoreAPIManager.StoreInitState.Succeeded)
		{
			yield return null;
		}
		SaveManager.m_instance.Initialize();
		SaveManager.m_instance.OnAwakeLoadConfig();
		yield break;
	}

	// Token: 0x060057C0 RID: 22464 RVA: 0x0002FBF1 File Offset: 0x0002DDF1
	public static void SetCultureInfo(CultureInfo cultureInfo)
	{
		SaveManager.ForcedCulture = cultureInfo;
	}

	// Token: 0x060057C1 RID: 22465 RVA: 0x0014D724 File Offset: 0x0014B924
	private void Initialize()
	{
		this.BinaryFormatter = new BinaryFormatter();
		if (SaveManager.ForcedCulture == null)
		{
			SaveManager.ForcedCulture = new CultureInfo("en-US", false);
			SaveManager.ForcedCulture.NumberFormat.CurrencyDecimalSeparator = ".";
			CultureInfo.CurrentCulture = SaveManager.ForcedCulture;
		}
		for (int i = 0; i < 5; i++)
		{
			string saveDirectoryPath = SaveManager.GetSaveDirectoryPath(i, true);
			if (SaveFileSystem.DirectoryExists(saveDirectoryPath))
			{
				SaveManager.Instance.m_backFilePathsArray[i] = SaveFileSystem.GetBackupFilesInDirectory(saveDirectoryPath);
			}
		}
		this.m_onWorldCreationComplete_UpdateSaveData = new Action<MonoBehaviour, EventArgs>(this.OnWorldCreationComplete_UpdateSaveData);
		this.m_onLevelEditorWorldCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnLevelEditorWorldCreationComplete);
		this.m_onBiomeCreationComplete_UpdateSaveData = new Action<MonoBehaviour, EventArgs>(this.OnBiomeCreationComplete_UpdateSaveData);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete_UpdateSaveData);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onLevelEditorWorldCreationComplete);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeCreationComplete, this.m_onBiomeCreationComplete_UpdateSaveData);
		this.Internal_IsInitialized = true;
	}

	// Token: 0x060057C2 RID: 22466 RVA: 0x0014D804 File Offset: 0x0014BA04
	private static void CreateSaveDirectoriesIfNeeded()
	{
		if (!SaveManager.Instance.m_checkedForSaveDirectories)
		{
			if (!SaveFileSystem.DirectoryExists(SaveManager.GetSaveDirectoryPath(-1, false)))
			{
				SaveManager.CreateBaseSaveDirectories();
			}
			for (int i = 0; i < 5; i++)
			{
				string saveDirectoryPath = SaveManager.GetSaveDirectoryPath(i, true);
				if (!SaveFileSystem.DirectoryExists(saveDirectoryPath))
				{
					SaveFileSystem.CreateDirectory(saveDirectoryPath);
				}
				if (SaveManager.Instance.m_backFilePathsArray[i] == null)
				{
					SaveManager.Instance.m_backFilePathsArray[i] = new List<string>();
				}
			}
			SaveManager.Instance.m_checkedForSaveDirectories = true;
		}
	}

	// Token: 0x060057C3 RID: 22467 RVA: 0x0014D87C File Offset: 0x0014BA7C
	private void OnAwakeLoadConfig()
	{
		if (!SaveManager.IsConfigFileLoaded)
		{
			SaveManager.LoadConfigFile();
		}
		if (SaveManager.ConfigData.ScreenWidth == -1 || SaveManager.ConfigData.ScreenHeight == -1)
		{
			Resolution currentResolution = Screen.currentResolution;
			SaveManager.ConfigData.ScreenWidth = currentResolution.width;
			SaveManager.ConfigData.ScreenHeight = currentResolution.height;
			SaveManager.SaveConfigFile();
		}
		AudioManager.SetMasterVolume(SaveManager.ConfigData.MasterVolume);
		AudioManager.SetMusicVolume(SaveManager.ConfigData.MusicVolume);
		AudioManager.SetSFXVolume(SaveManager.ConfigData.SFXVolume);
		QualitySettings.SetQualityLevel(SaveManager.ConfigData.QualitySetting, true);
		GameResolutionManager.SetVsyncEnable(SaveManager.ConfigData.EnableVsync);
		SaveManager.LoadProfileConfigFile();
	}

	// Token: 0x060057C4 RID: 22468 RVA: 0x0014D930 File Offset: 0x0014BB30
	public static void LoadCurrentProfileData()
	{
		SaveManager.IsRunning = true;
		if (SaveManager.DoesSaveExist(SaveManager.CurrentProfile, SaveDataType.Player, false))
		{
			LOAD_RESULT load_RESULT = SaveManager.LoadAllGameData(SaveManager.CurrentProfile);
			if (load_RESULT != LOAD_RESULT.OK)
			{
				if (load_RESULT != LOAD_RESULT.FAILED_STAGE)
				{
					SaveManager.CreateNewSaveData();
					SaveManager.LoadingFailed = true;
					Debug.Log("Failed to load Current Profile Data. RESULT: " + load_RESULT.ToString());
					return;
				}
				if (SaveManager.PlayerSaveData.InCastle)
				{
					SaveManager.StageSaveData = new StageSaveData();
					SaveManager.StageSaveData.ForceResetWorld = true;
					return;
				}
			}
		}
		else
		{
			SaveManager.CreateNewSaveData();
		}
	}

	// Token: 0x060057C5 RID: 22469 RVA: 0x0002FBF9 File Offset: 0x0002DDF9
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete_UpdateSaveData);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onLevelEditorWorldCreationComplete);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeCreationComplete, this.m_onBiomeCreationComplete_UpdateSaveData);
	}

	// Token: 0x060057C6 RID: 22470 RVA: 0x0014D9B4 File Offset: 0x0014BBB4
	private void OnBiomeCreationComplete_UpdateSaveData(MonoBehaviour sender, EventArgs eventArgs)
	{
		BiomeEventArgs biomeEventArgs = eventArgs as BiomeEventArgs;
		if (biomeEventArgs != null)
		{
			SaveManager.StageSaveData.VerifyBiomeSaveData(biomeEventArgs.Biome);
		}
	}

	// Token: 0x060057C7 RID: 22471 RVA: 0x0014D9DC File Offset: 0x0014BBDC
	private void OnWorldCreationComplete_UpdateSaveData(MonoBehaviour sender, EventArgs args)
	{
		SaveManager.PlayerSaveData.HasStartedGame = true;
		bool flag = false;
		if (SceneLoader_RL.CurrentScene != SceneLoadingUtility.GetSceneName(SceneID.World) && (WorldBuilder.BiomeControllers.ContainsKey(BiomeType.HubTown) || WorldBuilder.BiomeControllers.ContainsKey(BiomeType.Tutorial)))
		{
			bool forceResetWorld = SaveManager.StageSaveData.ForceResetWorld;
			SaveManager.StageSaveData = new StageSaveData();
			SaveManager.StageSaveData.ForceResetWorld = forceResetWorld;
			SaveManager.StageSaveData.CreateEmptyRoomSaveDataList();
			if (PlayerManager.IsInstantiated)
			{
				PlayerManager.GetPlayerController().ResetHealth();
				PlayerManager.GetPlayerController().ResetMana();
			}
			SaveManager.PlayerSaveData.UpdateCachedData();
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DriftHouseUnlocked) && Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, false) > 0)
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.DriftHouseUnlocked, true);
			}
		}
		else if ((!SaveManager.PlayerSaveData.InCastle && SaveManager.PlayerSaveData.CastleLockState == CastleLockState.NotLocked) || !SaveManager.DoesSaveExist(SaveManager.CurrentProfile, SaveDataType.Stage, false) || (!SaveManager.PlayerSaveData.InHubTown && !SaveManager.IsRunning))
		{
			Debug.Log("Creating new StageSaveData.");
			SaveManager.StageSaveData = new StageSaveData();
			SaveManager.StageSaveData.CreateEmptyRoomSaveDataList();
			SaveManager.StageSaveData.BiomeCreationSeed = RNGManager.GetSeed(RngID.BiomeCreation);
			SaveManager.StageSaveData.MergeRoomSeed = RNGManager.GetSeed(RngID.MergeRooms);
			SaveManager.StageSaveData.PropSeed = RNGManager.GetSeed(RngID.Prop);
			SaveManager.StageSaveData.EnemySeed = RNGManager.GetSeed(RngID.Enemy);
			flag = true;
		}
		else if (SaveManager.PlayerSaveData.InCastle)
		{
			Debug.Log("Loading old StageSaveData - Resuming Player Save progress");
			SaveManager.StageSaveData.TimesTrackerWasLoaded++;
			GameEventTrackerManager.RoomEventTracker.RoomsEntered.AddRange(SaveManager.StageSaveData.RoomTrackerDataList);
			GameEventTrackerManager.EnemyEventTracker.EnemiesKilled.AddRange(SaveManager.StageSaveData.EnemyTrackerDataList);
			GameEventTrackerManager.ItemEventTracker.ChestsOpened.AddRange(SaveManager.StageSaveData.ChestTrackerDataList);
			GameEventTrackerManager.ItemEventTracker.ItemsCollected.AddRange(SaveManager.StageSaveData.ItemTrackerDataList);
			global::PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.CurrentArmor = SaveManager.PlayerSaveData.CachedData.CurrentArmor;
			playerController.SpellOrbs = SaveManager.PlayerSaveData.CachedData.SpellOrbs;
			playerController.SetHealth((float)SaveManager.PlayerSaveData.CachedData.CurrentHealth, false, true);
			playerController.SetMana((float)SaveManager.PlayerSaveData.CachedData.CurrentMana, false, true, true);
			playerController.CastAbility.SetAbilityAmmo(CastAbilityType.Weapon, SaveManager.PlayerSaveData.CachedData.CurrentWeaponAmmo, false);
			playerController.CastAbility.SetAbilityAmmo(CastAbilityType.Spell, SaveManager.PlayerSaveData.CachedData.CurrentSpellAmmo, false);
			playerController.CastAbility.SetAbilityAmmo(CastAbilityType.Talent, SaveManager.PlayerSaveData.CachedData.CurrentTalentAmmo, false);
		}
		else
		{
			Debug.Log("Loading old StageSaveData - Resetting Player Save progress");
			GameEventTrackerManager.RoomEventTracker.Reset();
			GameEventTrackerManager.EnemyEventTracker.Reset();
			GameEventTrackerManager.ItemEventTracker.Reset();
			SaveManager.StageSaveData.TimesTrackerWasLoaded = 0;
			foreach (KeyValuePair<BiomeType, List<RoomSaveData>> keyValuePair in SaveManager.StageSaveData.RoomSaveDataDict)
			{
				Dictionary<int, RoomSaveData> roomDataLookupTable = SaveManager.StageSaveData.GetRoomDataLookupTable(keyValuePair.Key);
				if (roomDataLookupTable != null)
				{
					foreach (KeyValuePair<int, RoomSaveData> keyValuePair2 in roomDataLookupTable)
					{
						RoomSaveData value = keyValuePair2.Value;
						if (!value.IsEmpty)
						{
							if (keyValuePair.Key == BiomeType.Forest && SaveManager.PlayerSaveData.GetInsightState(InsightType.ForestBoss_DoorOpened) < InsightState.ResolvedButNotViewed)
							{
								string a = value.RoomID.ToString();
								if (a == "Levels Forest Mandatory 1" || a == "Levels Forest Mandatory 2" || a == "Levels Forest Mandatory 3")
								{
									value.IsRoomComplete = false;
								}
							}
							if (keyValuePair.Key == BiomeType.Cave)
							{
								if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_WhiteDoor_Opened) && value.RoomID.ToString() == "Levels Cave Mandatory 22")
								{
									value.IsRoomComplete = false;
								}
								if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_BlackDoor_Opened) && value.RoomID.ToString() == "Levels Cave Mandatory 19")
								{
									value.IsRoomComplete = false;
								}
							}
							if (value.RoomType == RoomType.Fairy && value.IsRoomComplete)
							{
								bool flag2 = false;
								for (int i = 0; i < value.ChestStates.Length; i++)
								{
									if (value.ChestStates[i].IsStateActive)
									{
										flag2 = true;
									}
								}
								if (flag2)
								{
									value.IsRoomComplete = false;
								}
							}
							for (int j = 0; j < value.BreakableStates.Length; j++)
							{
								value.BreakableStates[j].IsStateActive = true;
							}
							for (int k = 0; k < value.DecoBreakableStates.Length; k++)
							{
								value.DecoBreakableStates[k].IsStateActive = true;
							}
							for (int l = 0; l < value.EnemyStates.Length; l++)
							{
								value.EnemyStates[l].IsStateActive = true;
							}
						}
					}
				}
			}
			flag = true;
		}
		if (flag)
		{
			SaveManager.PlayerSaveData.InHubTown = false;
			SaveManager.PlayerSaveData.InCastle = true;
			SaveManager.PlayerSaveData.RunAccumulatedXP = 0;
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CastleBoss_FreeHeal_Used, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.BridgeBoss_FreeHeal_Used, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.ForestBoss_FreeHeal_Used, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.StudyBoss_FreeHeal_Used, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TowerBoss_FreeHeal_Used, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveBoss_FreeHeal_Used, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.GardenBoss_FreeHeal_Used, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.FinalBoss_FreeHeal_Used, false);
			NPCDialogueManager.PopulateNPCDialogues(false);
			NPCDialogueManager.InitializeGlobalDialogueCD();
			if ((int)SaveManager.PlayerSaveData.TreeCutsceneDisplayCount < Ending_EV.TREE_CUTSCENE_DIALOGUE_1_LOCID.Length || (int)SaveManager.PlayerSaveData.HestiaCutsceneDisplayCount < Ending_EV.HESTIA_CUTSCENE_DIALOGUE_1_LOCID.Length)
			{
				float num = 0f;
				int num2 = SaveManager.PlayerSaveData.TimesDiedSinceHestia - 6 + 1;
				if (num2 > 0)
				{
					num = Mathf.Clamp((float)num2 * 0.25f, 0f, 1f);
				}
				float num3 = UnityEngine.Random.Range(0f, 1f);
				if (num3 > 0f && num3 <= num)
				{
					if ((int)SaveManager.PlayerSaveData.HestiaCutsceneDisplayCount < Ending_EV.HESTIA_CUTSCENE_DIALOGUE_1_LOCID.Length && (SaveManager.PlayerSaveData.TreeCutsceneDisplayCount > SaveManager.PlayerSaveData.HestiaCutsceneDisplayCount || (int)SaveManager.PlayerSaveData.TreeCutsceneDisplayCount >= Ending_EV.TREE_CUTSCENE_DIALOGUE_1_LOCID.Length))
					{
						SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Play_Hestia_DeathCutscene, true);
					}
					else
					{
						SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Play_Tree_DeathCutscene, true);
					}
				}
			}
			SaveManager.SaveAllCurrentProfileGameData(SavingType.FileAndBackup, true, true);
		}
		SaveManager.StageSaveData.LinkTrackerData();
		MapController.LoadStageSaveData();
	}

	// Token: 0x060057C8 RID: 22472 RVA: 0x0014E0F4 File Offset: 0x0014C2F4
	private void OnLevelEditorWorldCreationComplete(MonoBehaviour sender, EventArgs args)
	{
		LevelEditorWorldCreationCompleteEventArgs levelEditorWorldCreationCompleteEventArgs = args as LevelEditorWorldCreationCompleteEventArgs;
		SaveManager.StageSaveData.CreateLevelEditorRoomSaveDataList(levelEditorWorldCreationCompleteEventArgs.BuiltRoom);
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DriftHouseUnlocked) && Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, false) > 0)
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.DriftHouseUnlocked, true);
		}
	}

	// Token: 0x060057C9 RID: 22473 RVA: 0x0014E14C File Offset: 0x0014C34C
	private static void CreateSaveFile(SaveFileSystem.SaveBatch saveBatch, string fileNameAndPath, object saveObject, BinaryFormatter binaryFormatter)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, saveObject);
			memoryStream.Close();
			SaveFileSystem.WriteAllBytes(saveBatch, fileNameAndPath, memoryStream.ToArray());
		}
		catch (Exception ex)
		{
			Debug.Log("<color=red>FILE SAVE FAILED.</color>  Save Path: " + fileNameAndPath + "  Error: " + ex.Message);
		}
	}

	// Token: 0x060057CA RID: 22474 RVA: 0x0014E1AC File Offset: 0x0014C3AC
	private void SaveFileAsync(SaveFileSystem.SaveBatch saveBatch, int profileSlot, SaveDataType saveType, bool saveBackup, object saveObject = null)
	{
		if (saveObject == null)
		{
			switch (saveType)
			{
			case SaveDataType.Player:
				SaveManager.PlayerSaveData.UpdateCachedData();
				saveObject = SaveManager.PlayerSaveData;
				break;
			case SaveDataType.Stage:
				saveObject = SaveManager.StageSaveData;
				break;
			case SaveDataType.Equipment:
				saveObject = SaveManager.EquipmentSaveData;
				break;
			case SaveDataType.Lineage:
				saveObject = SaveManager.LineageSaveData;
				break;
			case SaveDataType.GameMode:
				saveObject = SaveManager.ModeSaveData;
				break;
			}
		}
		string fullFilePath = SaveManager.GetFullFilePath(profileSlot, saveType, saveBackup);
		Task lastAsyncTask = SaveManager.m_lastAsyncTask;
		SaveManager.m_lastAsyncTask = this.SaveAsync(lastAsyncTask, saveBatch, fullFilePath, profileSlot, saveType, saveBackup, saveObject);
	}

	// Token: 0x060057CB RID: 22475 RVA: 0x0014E234 File Offset: 0x0014C434
	private Task SaveAsync(Task lastAsyncTask, SaveFileSystem.SaveBatch saveBatch, string filePath, int profileSlot, SaveDataType saveType, bool saveBackup, object saveObject)
	{
		SaveManager.<SaveAsync>d__81 <SaveAsync>d__;
		<SaveAsync>d__.<>4__this = this;
		<SaveAsync>d__.lastAsyncTask = lastAsyncTask;
		<SaveAsync>d__.saveBatch = saveBatch;
		<SaveAsync>d__.filePath = filePath;
		<SaveAsync>d__.profileSlot = profileSlot;
		<SaveAsync>d__.saveType = saveType;
		<SaveAsync>d__.saveBackup = saveBackup;
		<SaveAsync>d__.saveObject = saveObject;
		<SaveAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SaveAsync>d__.<>1__state = -1;
		AsyncTaskMethodBuilder <>t__builder = <SaveAsync>d__.<>t__builder;
		<>t__builder.Start<SaveManager.<SaveAsync>d__81>(ref <SaveAsync>d__);
		return <SaveAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060057CC RID: 22476 RVA: 0x0014E2B8 File Offset: 0x0014C4B8
	private void DeleteOldBackups(SaveFileSystem.SaveBatch saveBatch, int profileSlot, SaveDataType saveType)
	{
		List<string> backupFullFilePathes = this.GetBackupFullFilePathes(profileSlot, saveType);
		if (backupFullFilePathes.Count <= 5)
		{
			return;
		}
		backupFullFilePathes.Sort(new Comparison<string>(this.DateTimeComparator));
		backupFullFilePathes.Reverse();
		backupFullFilePathes.RemoveRange(0, 5);
		foreach (string text in backupFullFilePathes)
		{
			SaveFileSystem.DeleteFile(saveBatch, text);
			this.m_backFilePathsArray[profileSlot].Remove(text);
		}
	}

	// Token: 0x060057CD RID: 22477 RVA: 0x0014E348 File Offset: 0x0014C548
	private int DateTimeComparator(string fileA, string fileB)
	{
		DateTime t = SaveManager.PathToDateTime(fileA);
		DateTime t2 = SaveManager.PathToDateTime(fileB);
		return DateTime.Compare(t, t2);
	}

	// Token: 0x060057CE RID: 22478 RVA: 0x0014E368 File Offset: 0x0014C568
	public static DateTime PathToDateTime(string filePath)
	{
		int length = "dd_MM_yyyy__HH_mm_ss".Length;
		int num = filePath.IndexOf(".rc2dat");
		return DateTime.ParseExact(filePath.Substring(num - length, length), "dd_MM_yyyy__HH_mm_ss", SaveManager.ForcedCulture);
	}

	// Token: 0x060057CF RID: 22479 RVA: 0x0014E3A8 File Offset: 0x0014C5A8
	private static LOAD_RESULT LoadFile(string filePath, out object output, BinaryFormatter binaryFormatter)
	{
		output = null;
		if (SaveFileSystem.FileExists(filePath))
		{
			LOAD_RESULT result;
			try
			{
				byte[] array = SaveFileSystem.ReadAllBytes(filePath);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					memoryStream.Write(array, 0, array.Length);
					memoryStream.Position = 0L;
					output = binaryFormatter.Deserialize(memoryStream);
				}
				result = LOAD_RESULT.OK;
			}
			catch (Exception ex)
			{
				Debug.Log(string.Concat(new string[]
				{
					"<color=red>FILE LOAD FAILED.  File Path: ",
					filePath,
					"  Error: ",
					ex.Message,
					"</color>"
				}));
				if (filePath.Contains(SaveDataType.Player.ToString()))
				{
					result = LOAD_RESULT.FAILED_PLAYER;
				}
				else if (filePath.Contains(SaveDataType.Equipment.ToString()))
				{
					result = LOAD_RESULT.FAILED_EQUIPMENT;
				}
				else if (filePath.Contains(SaveDataType.Lineage.ToString()))
				{
					result = LOAD_RESULT.FAILED_LINEAGE;
				}
				else if (filePath.Contains(SaveDataType.Stage.ToString()))
				{
					result = LOAD_RESULT.FAILED_STAGE;
				}
				else if (filePath.Contains(SaveDataType.GameMode.ToString()))
				{
					result = LOAD_RESULT.FAILED_GAMEMODE;
				}
				else
				{
					result = LOAD_RESULT.FAILED_UNKNOWN;
				}
			}
			return result;
		}
		Debug.Log("<color=red>Could not load file: " + filePath + ". File does not exist.</color>");
		if (filePath.Contains(SaveDataType.Player.ToString()))
		{
			return LOAD_RESULT.NO_FILE_FOUND_PLAYER;
		}
		if (filePath.Contains(SaveDataType.Equipment.ToString()))
		{
			return LOAD_RESULT.NO_FILE_FOUND_EQUIPMENT;
		}
		if (filePath.Contains(SaveDataType.Lineage.ToString()))
		{
			return LOAD_RESULT.NO_FILE_FOUND_LINEAGE;
		}
		if (filePath.Contains(SaveDataType.Stage.ToString()))
		{
			return LOAD_RESULT.NO_FILE_FOUND_STAGE;
		}
		if (filePath.Contains(SaveDataType.GameMode.ToString()))
		{
			return LOAD_RESULT.NO_FILE_FOUND_GAMEMODE;
		}
		return LOAD_RESULT.NO_FILE_FOUND_UNKNOWN;
	}

	// Token: 0x060057D0 RID: 22480 RVA: 0x0002FC20 File Offset: 0x0002DE20
	public static void ForceInitialize()
	{
		bool isInitialized = SaveManager.IsInitialized;
	}

	// Token: 0x060057D1 RID: 22481 RVA: 0x0002FC28 File Offset: 0x0002DE28
	public static bool DoesSaveExist(int profileSlot, SaveDataType saveType, bool checkForBackup)
	{
		if (!checkForBackup)
		{
			return SaveFileSystem.FileExists(SaveManager.GetFullFilePath(profileSlot, saveType, checkForBackup));
		}
		return SaveManager.Instance.GetBackupFullFilePathes(profileSlot, saveType).Count > 0;
	}

	// Token: 0x17001D6E RID: 7534
	// (get) Token: 0x060057D2 RID: 22482 RVA: 0x0002FC4F File Offset: 0x0002DE4F
	public static CultureInfo CultureInfo
	{
		get
		{
			return SaveManager.ForcedCulture;
		}
	}

	// Token: 0x17001D6F RID: 7535
	// (get) Token: 0x060057D3 RID: 22483 RVA: 0x0002FC56 File Offset: 0x0002DE56
	public static bool IsInitialized
	{
		get
		{
			return SaveManager.Instance.Internal_IsInitialized;
		}
	}

	// Token: 0x060057D4 RID: 22484 RVA: 0x0002FC62 File Offset: 0x0002DE62
	public static List<string> GetBackupPathes(int profileSlot, SaveDataType saveType)
	{
		return SaveManager.Instance.GetBackupFullFilePathes(profileSlot, saveType);
	}

	// Token: 0x060057D5 RID: 22485 RVA: 0x0002FC70 File Offset: 0x0002DE70
	public static void SaveCurrentProfileGameData(SaveFileSystem.SaveBatch saveBatch, SaveDataType saveType, SavingType backupType, bool ignoreTimeRestriction, object saveObject = null)
	{
		SaveManager.SaveGameData(saveBatch, SaveManager.CurrentProfile, saveType, backupType, ignoreTimeRestriction, saveObject);
	}

	// Token: 0x060057D6 RID: 22486 RVA: 0x0002FC82 File Offset: 0x0002DE82
	public static void SaveCurrentProfileGameData(SaveDataType saveType, SavingType backupType, bool ignoreTimeRestriction, object saveObject = null)
	{
		SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(SaveManager.CurrentProfile);
		SaveManager.SaveCurrentProfileGameData(saveBatch, saveType, backupType, ignoreTimeRestriction, saveObject);
		saveBatch.End();
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x0014E578 File Offset: 0x0014C778
	public static void SaveAllCurrentProfileGameData(SavingType savingType, bool saveStageData, bool ignoreTimeRestriction)
	{
		SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(SaveManager.CurrentProfile);
		SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.Player, savingType, ignoreTimeRestriction, null);
		SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.Equipment, savingType, ignoreTimeRestriction, null);
		SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.Lineage, savingType, ignoreTimeRestriction, null);
		SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.GameMode, savingType, ignoreTimeRestriction, null);
		if (saveStageData)
		{
			SaveManager.SaveCurrentProfileGameData(saveBatch, SaveDataType.Stage, savingType, ignoreTimeRestriction, null);
		}
		saveBatch.End();
	}

	// Token: 0x060057D8 RID: 22488 RVA: 0x0014E5CC File Offset: 0x0014C7CC
	private static void SaveGameData(SaveFileSystem.SaveBatch saveBatch, int profileSlot, SaveDataType saveType, SavingType backupType, bool ignoreTimeRestriction, object saveObject = null)
	{
		if (SaveManager.DisableSaving)
		{
			return;
		}
		if (Application.isPlaying)
		{
			SaveManager.CreateSaveDirectoriesIfNeeded();
			if (!SaveManager.IsRunning)
			{
				if (!SaveManager.m_hasMainMenuSaveFailedMessageBeenLogged)
				{
					SaveManager.m_hasMainMenuSaveFailedMessageBeenLogged = true;
					Debug.Log("Save failed. Must enter Main Menu at least once for auto-saving to work");
				}
				return;
			}
			if (SaveManager.LoadingFailed)
			{
				Debug.Log("Save failed. Corrupted save file detected. Please load backup save file.");
				return;
			}
			if (!ignoreTimeRestriction && SaveManager.FrameCountSinceLastSave != Time.frameCount && Time.unscaledTime < SaveManager.TimeSinceLastSave + 10f)
			{
				return;
			}
			if (saveType == SaveDataType.Stage && !SaveManager.IsCopyingBackupFiles && SceneLoader_RL.CurrentScene != SceneLoadingUtility.GetSceneName(SceneID.World) && !SaveManager.StageSaveData.ForceResetWorld)
			{
				Debug.Log("Cannot save StageData. You are not in the world scene.");
				return;
			}
		}
		if (!ignoreTimeRestriction)
		{
			SaveManager.FrameCountSinceLastSave = Time.frameCount;
			SaveManager.TimeSinceLastSave = Time.unscaledTime;
		}
		switch (backupType)
		{
		case SavingType.FileAndBackup:
			SaveManager.Instance.SaveFileAsync(saveBatch, profileSlot, saveType, false, saveObject);
			SaveManager.Instance.SaveFileAsync(saveBatch, profileSlot, saveType, true, saveObject);
			return;
		case SavingType.FileOnly:
			SaveManager.Instance.SaveFileAsync(saveBatch, profileSlot, saveType, false, saveObject);
			return;
		case SavingType.BackupOnly:
			SaveManager.Instance.SaveFileAsync(saveBatch, profileSlot, saveType, true, saveObject);
			return;
		default:
			return;
		}
	}

	// Token: 0x060057D9 RID: 22489 RVA: 0x0002FC9D File Offset: 0x0002DE9D
	public static void CreateNewSaveData()
	{
		SaveManager.PlayerSaveData = new global::PlayerSaveData();
		SaveManager.EquipmentSaveData = new EquipmentSaveData();
		SaveManager.LineageSaveData = new LineageSaveData();
		SaveManager.ModeSaveData = new ModeSaveData();
	}

	// Token: 0x060057DA RID: 22490 RVA: 0x0014E6E8 File Offset: 0x0014C8E8
	public static LOAD_RESULT LoadGameDataAndUpdate(int profileSlot, SaveDataType saveType)
	{
		object obj = null;
		LOAD_RESULT load_RESULT = SaveManager.LoadGameData(profileSlot, saveType, out obj);
		if (load_RESULT == LOAD_RESULT.OK)
		{
			if (profileSlot == SaveManager.CurrentProfile)
			{
				(obj as IVersionUpdateable).UpdateVersion();
			}
			switch (saveType)
			{
			case SaveDataType.Player:
				SaveManager.PlayerSaveData = (obj as global::PlayerSaveData);
				if (SaveManager.PlayerSaveData == null)
				{
					load_RESULT = LOAD_RESULT.FAILED_PLAYER;
				}
				break;
			case SaveDataType.Stage:
				SaveManager.StageSaveData = (obj as StageSaveData);
				if (SaveManager.StageSaveData == null)
				{
					load_RESULT = LOAD_RESULT.FAILED_STAGE;
				}
				break;
			case SaveDataType.Equipment:
				SaveManager.EquipmentSaveData = (obj as EquipmentSaveData);
				if (SaveManager.EquipmentSaveData == null)
				{
					load_RESULT = LOAD_RESULT.FAILED_EQUIPMENT;
				}
				break;
			case SaveDataType.Lineage:
				SaveManager.LineageSaveData = (obj as LineageSaveData);
				if (SaveManager.LineageSaveData == null)
				{
					load_RESULT = LOAD_RESULT.FAILED_LINEAGE;
				}
				break;
			case SaveDataType.GameMode:
				SaveManager.ModeSaveData = (obj as ModeSaveData);
				if (SaveManager.ModeSaveData == null)
				{
					load_RESULT = LOAD_RESULT.FAILED_GAMEMODE;
				}
				break;
			}
		}
		return load_RESULT;
	}

	// Token: 0x060057DB RID: 22491 RVA: 0x0002FCC7 File Offset: 0x0002DEC7
	public static LOAD_RESULT LoadGameDataByFilePath(string filePath, out object loadDataOutput)
	{
		return SaveManager.LoadFile(filePath, out loadDataOutput, SaveManager.Instance.BinaryFormatter);
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x0002FCDA File Offset: 0x0002DEDA
	public static LOAD_RESULT LoadGameData(int profileSlot, SaveDataType saveType, out object loadDataOutput)
	{
		return SaveManager.LoadFile(SaveManager.GetFullFilePath(profileSlot, saveType, false), out loadDataOutput, SaveManager.Instance.BinaryFormatter);
	}

	// Token: 0x060057DD RID: 22493 RVA: 0x0014E7A4 File Offset: 0x0014C9A4
	public static LOAD_RESULT LoadAllGameData(int profileSlot)
	{
		LOAD_RESULT load_RESULT = SaveManager.LoadGameDataAndUpdate(profileSlot, SaveDataType.GameMode);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("<color=red>Failed to load SaveType: " + SaveDataType.GameMode.ToString() + ".</color>");
			return load_RESULT;
		}
		load_RESULT = SaveManager.LoadGameDataAndUpdate(profileSlot, SaveDataType.Player);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("<color=red>Failed to load SaveType: " + SaveDataType.Player.ToString() + ".</color>");
			return load_RESULT;
		}
		load_RESULT = SaveManager.LoadGameDataAndUpdate(profileSlot, SaveDataType.Equipment);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("<color=red>Failed to load SaveType: " + SaveDataType.Equipment.ToString() + ".</color>");
			return load_RESULT;
		}
		load_RESULT = SaveManager.LoadGameDataAndUpdate(profileSlot, SaveDataType.Lineage);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("<color=red>Failed to load SaveType: " + SaveDataType.Lineage.ToString() + ".</color>");
			return load_RESULT;
		}
		if (SaveManager.DoesSaveExist(profileSlot, SaveDataType.Stage, false))
		{
			load_RESULT = SaveManager.LoadGameDataAndUpdate(profileSlot, SaveDataType.Stage);
			if (load_RESULT != LOAD_RESULT.OK)
			{
				Debug.Log("<color=red>Failed to load SaveType: " + SaveDataType.Stage.ToString() + ".</color>");
				return load_RESULT;
			}
			if (load_RESULT == LOAD_RESULT.OK && SaveManager.StageSaveData.RoomSaveDataDict.ContainsKey(BiomeType.HubTown) && SaveManager.StageSaveData.RoomSaveDataDict.Count == 1)
			{
				load_RESULT = LOAD_RESULT.FAILED_STAGE;
				Debug.Log("<color=red>Failed to load SaveType: " + SaveDataType.Stage.ToString() + " because stage data was hubtown.</color>");
				return load_RESULT;
			}
		}
		return LOAD_RESULT.OK;
	}

	// Token: 0x060057DE RID: 22494 RVA: 0x0014E8FC File Offset: 0x0014CAFC
	public static LOAD_RESULT LoadGameDataViaEditor(int profileSlot, SaveDataType saveType, out object loadDataOutput)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		return SaveManager.LoadFile(SaveManager.GetFullFilePath(profileSlot, saveType, false), out loadDataOutput, binaryFormatter);
	}

	// Token: 0x060057DF RID: 22495 RVA: 0x0014E920 File Offset: 0x0014CB20
	public static void SaveGameDataViaEditor(int profileSlot, SaveDataType saveType, object saveObject)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		string fullFilePath = SaveManager.GetFullFilePath(profileSlot, saveType, false);
		SaveManager.CreateSaveFile(new SaveFileSystem.SaveBatch(), fullFilePath, saveObject, binaryFormatter);
	}

	// Token: 0x060057E0 RID: 22496 RVA: 0x0014E94C File Offset: 0x0014CB4C
	public static void DeleteSaveFile(SaveFileSystem.SaveBatch saveBatch, int profileSlot, SaveDataType saveType)
	{
		string fullFilePath = SaveManager.GetFullFilePath(profileSlot, saveType, false);
		if (SaveFileSystem.FileExists(fullFilePath))
		{
			SaveFileSystem.DeleteFile(saveBatch, fullFilePath);
			Debug.Log("Deleted save file: " + saveType.ToString() + " for profile slot " + profileSlot.ToString());
		}
	}

	// Token: 0x17001D70 RID: 7536
	// (get) Token: 0x060057E1 RID: 22497 RVA: 0x0002FCF4 File Offset: 0x0002DEF4
	// (set) Token: 0x060057E2 RID: 22498 RVA: 0x0002FCFB File Offset: 0x0002DEFB
	public static bool IsConfigFileLoaded { get; private set; }

	// Token: 0x060057E3 RID: 22499 RVA: 0x0014E99C File Offset: 0x0014CB9C
	public static void SaveConfigFile()
	{
		SaveManager.CreateSaveDirectoriesIfNeeded();
		string configPath = SaveManager.GetConfigPath();
		using (StringWriter stringWriter = new StringWriter())
		{
			try
			{
				stringWriter.WriteLine("[VERSION]");
				stringWriter.WriteLine("RevisionNumber=" + SaveManager.ConfigData.REVISION_NUMBER.ToString());
				stringWriter.WriteLine();
				stringWriter.WriteLine("[Selected Profile]");
				stringWriter.WriteLine("Profile=" + SaveManager.ConfigData.CurrentProfile.ToString());
				stringWriter.WriteLine();
				stringWriter.WriteLine("[Screen Resolution]");
				stringWriter.WriteLine("ScreenWidth=" + SaveManager.ConfigData.ScreenWidth.ToString());
				stringWriter.WriteLine("ScreenHeight=" + SaveManager.ConfigData.ScreenHeight.ToString());
				stringWriter.WriteLine();
				stringWriter.WriteLine("[Screen Mode]");
				stringWriter.WriteLine("PrimaryDisplay=" + SaveManager.ConfigData.PrimaryDisplay.ToString());
				stringWriter.WriteLine("ScreenMode=" + SaveManager.ConfigData.ScreenMode.ToString());
				stringWriter.WriteLine("Disable_16_9=" + SaveManager.ConfigData.Disable_16_9.ToString());
				stringWriter.WriteLine("DisableCursorConfine=" + SaveManager.ConfigData.DisableCursorConfine.ToString());
				stringWriter.WriteLine();
				stringWriter.WriteLine("[Graphics Quality]");
				stringWriter.WriteLine("QualitySetting=" + SaveManager.ConfigData.QualitySetting.ToString());
				stringWriter.WriteLine("EnableVsync=" + SaveManager.ConfigData.EnableVsync.ToString());
				stringWriter.WriteLine("FPSLimit=" + SaveManager.ConfigData.FPSLimit.ToString());
				stringWriter.WriteLine();
				stringWriter.WriteLine("[Game Volume]");
				stringWriter.WriteLine("MasterVol=" + string.Format("{0:F2}", SaveManager.ConfigData.MasterVolume));
				stringWriter.WriteLine("MusicVol=" + string.Format("{0:F2}", SaveManager.ConfigData.MusicVolume));
				stringWriter.WriteLine("SFXVol=" + string.Format("{0:F2}", SaveManager.ConfigData.SFXVolume));
				stringWriter.WriteLine();
				stringWriter.WriteLine("[Controls]");
				stringWriter.WriteLine("DeadZone=" + string.Format("{0:F2}", SaveManager.ConfigData.DeadZone));
				stringWriter.WriteLine("AimFidelity=" + SaveManager.ConfigData.AimFidelity.ToString());
				stringWriter.WriteLine("InputIconSetting=" + SaveManager.ConfigData.InputIconSetting.ToString());
				stringWriter.WriteLine("DisableRumble=" + SaveManager.ConfigData.DisableRumble.ToString());
				stringWriter.WriteLine();
				stringWriter.WriteLine("[Game Settings]");
				stringWriter.WriteLine("UseNonScientificNames=" + SaveManager.ConfigData.UseNonScientificNames.ToString());
				stringWriter.WriteLine("EnableDualButtonDash=" + SaveManager.ConfigData.EnableDualButtonDash.ToString());
				stringWriter.WriteLine("EnableQuickDrop=" + SaveManager.ConfigData.EnableQuickDrop.ToString());
				stringWriter.WriteLine("DisableMouseAiming=" + SaveManager.ConfigData.ToggleMouseAiming.ToString());
				stringWriter.WriteLine("EnableMouseAttackFlip=" + SaveManager.ConfigData.ToggleMouseAttackFlip.ToString());
				stringWriter.WriteLine("DisableHitSlowdown=" + SaveManager.ConfigData.DisableSlowdownOnHit.ToString());
				stringWriter.WriteLine("DisablePressDownSpinKick=" + SaveManager.ConfigData.DisablePressDownSpinKick.ToString());
				stringWriter.WriteLine("DisableReloadInteractButton=" + SaveManager.ConfigData.DisableReloadInteractButton.ToString());
				stringWriter.WriteLine("DisableHUDFadeOut=" + SaveManager.ConfigData.DisableHUDFadeOut.ToString());
				stringWriter.WriteLine("EnableMusicOnPause=" + SaveManager.ConfigData.EnableMusicOnPause.ToString());
				stringWriter.WriteLine("Language=" + SaveManager.ConfigData.Language.ToString());
				stringWriter.WriteLine();
				stringWriter.WriteLine("[User Reports]");
				stringWriter.WriteLine("UserReportName=" + string.Format("{0:F2}", SaveManager.ConfigData.UserReportName));
				stringWriter.WriteLine("UserReportEmail=" + string.Format("{0:F2}", SaveManager.ConfigData.UserReportEmail));
				stringWriter.WriteLine();
				SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(-1);
				SaveFileSystem.WriteAllBytes(saveBatch, configPath, Encoding.ASCII.GetBytes(stringWriter.ToString()));
				saveBatch.End();
			}
			catch (Exception ex)
			{
				throw new Exception("Could not save config file. Error: " + ex.Message);
			}
			Debug.Log("<color=green>Config file saved successfully.</color>");
		}
	}

	// Token: 0x060057E4 RID: 22500 RVA: 0x0014EED8 File Offset: 0x0014D0D8
	public static string GetConfigPath()
	{
		string path = Path.Combine(SaveFileSystem.PersistentDataPath, "Saves");
		string platformDirectoryName = StoreAPIManager.GetPlatformDirectoryName();
		return Path.Combine(Path.Combine(path, platformDirectoryName), "GameConfig.ini");
	}

	// Token: 0x060057E5 RID: 22501 RVA: 0x0014EF0C File Offset: 0x0014D10C
	public static void LoadConfigFile()
	{
		string configPath = SaveManager.GetConfigPath();
		if (!SaveFileSystem.FileExists(configPath))
		{
			Debug.Log("<color=yellow>Could not find config file.  Creating default config file.</color>");
			SaveManager.SaveConfigFile();
		}
		try
		{
			byte[] bytes = SaveFileSystem.ReadAllBytes(configPath);
			using (StringReader stringReader = new StringReader(Encoding.ASCII.GetString(bytes)))
			{
				string text;
				while ((text = stringReader.ReadLine()) != null)
				{
					int num = text.IndexOf("=");
					if (num != -1)
					{
						string text2 = text.Substring(0, num);
						string text3 = text.Substring(num + 1);
						if (!string.IsNullOrEmpty(text3) && text2 != null)
						{
							uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
							if (num2 <= 2232984626U)
							{
								if (num2 <= 970332931U)
								{
									if (num2 <= 393071022U)
									{
										if (num2 != 111036251U)
										{
											if (num2 != 241042190U)
											{
												if (num2 == 393071022U)
												{
													if (text2 == "ScreenMode")
													{
														SaveManager.ConfigData.ScreenMode = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
													}
												}
											}
											else if (text2 == "EnableMouseAttackFlip")
											{
												SaveManager.ConfigData.ToggleMouseAttackFlip = bool.Parse(text3);
											}
										}
										else if (text2 == "FPSLimit")
										{
											SaveManager.ConfigData.FPSLimit = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
										}
									}
									else if (num2 <= 723308068U)
									{
										if (num2 != 406609144U)
										{
											if (num2 == 723308068U)
											{
												if (text2 == "DisablePressDownSpinKick")
												{
													SaveManager.ConfigData.DisablePressDownSpinKick = bool.Parse(text3);
												}
											}
										}
										else if (text2 == "DisableReloadInteractButton")
										{
											SaveManager.ConfigData.DisableReloadInteractButton = bool.Parse(text3);
										}
									}
									else if (num2 != 764098295U)
									{
										if (num2 == 970332931U)
										{
											if (text2 == "EnableVsync")
											{
												SaveManager.ConfigData.EnableVsync = bool.Parse(text3);
											}
										}
									}
									else if (text2 == "DisableCursorConfine")
									{
										SaveManager.ConfigData.DisableCursorConfine = bool.Parse(text3);
									}
								}
								else if (num2 <= 1662862245U)
								{
									if (num2 <= 1515344127U)
									{
										if (num2 != 1022377904U)
										{
											if (num2 == 1515344127U)
											{
												if (text2 == "Disable_16_9")
												{
													SaveManager.ConfigData.Disable_16_9 = bool.Parse(text3);
												}
											}
										}
										else if (text2 == "UserReportEmail")
										{
											SaveManager.ConfigData.UserReportEmail = text3;
										}
									}
									else if (num2 != 1586689466U)
									{
										if (num2 == 1662862245U)
										{
											if (text2 == "DeadZone")
											{
												SaveManager.ConfigData.DeadZone = Mathf.Clamp(float.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo), 0f, 1f);
											}
										}
									}
									else if (text2 == "UseNonScientificNames")
									{
										SaveManager.ConfigData.UseNonScientificNames = bool.Parse(text3);
									}
								}
								else if (num2 <= 2067154761U)
								{
									if (num2 != 1761316584U)
									{
										if (num2 == 2067154761U)
										{
											if (text2 == "RevisionNumber")
											{
												SaveManager.ConfigData.REVISION_NUMBER = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
											}
										}
									}
									else if (text2 == "AimFidelity")
									{
										SaveManager.ConfigData.AimFidelity = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
									}
								}
								else if (num2 != 2080855286U)
								{
									if (num2 == 2232984626U)
									{
										if (text2 == "DisableHUDFadeOut")
										{
											SaveManager.ConfigData.DisableHUDFadeOut = bool.Parse(text3);
										}
									}
								}
								else if (text2 == "ScreenHeight")
								{
									SaveManager.ConfigData.ScreenHeight = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
								}
							}
							else if (num2 <= 3440849550U)
							{
								if (num2 <= 2811653349U)
								{
									if (num2 <= 2464506289U)
									{
										if (num2 != 2286991138U)
										{
											if (num2 == 2464506289U)
											{
												if (text2 == "DisableHitSlowdown")
												{
													SaveManager.ConfigData.DisableSlowdownOnHit = bool.Parse(text3);
												}
											}
										}
										else if (text2 == "DisableRumble")
										{
											SaveManager.ConfigData.DisableRumble = bool.Parse(text3);
										}
									}
									else if (num2 != 2591284123U)
									{
										if (num2 == 2811653349U)
										{
											if (text2 == "MusicVol")
											{
												SaveManager.ConfigData.MusicVolume = Mathf.Clamp(float.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo), 0f, 1f);
											}
										}
									}
									else if (text2 == "Language")
									{
										SaveManager.ConfigData.Language = (LanguageType)Enum.Parse(typeof(LanguageType), text3);
									}
								}
								else if (num2 <= 3265821073U)
								{
									if (num2 != 2950313672U)
									{
										if (num2 == 3265821073U)
										{
											if (text2 == "PrimaryDisplay")
											{
												SaveManager.ConfigData.PrimaryDisplay = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
											}
										}
									}
									else if (text2 == "EnableQuickDrop")
									{
										SaveManager.ConfigData.EnableQuickDrop = bool.Parse(text3);
									}
								}
								else if (num2 != 3282277021U)
								{
									if (num2 == 3440849550U)
									{
										if (text2 == "Profile")
										{
											SaveManager.ConfigData.CurrentProfile = byte.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
										}
									}
								}
								else if (text2 == "DisableMouseAiming")
								{
									SaveManager.ConfigData.ToggleMouseAiming = bool.Parse(text3);
								}
							}
							else if (num2 <= 3742089894U)
							{
								if (num2 <= 3569705650U)
								{
									if (num2 != 3506588379U)
									{
										if (num2 == 3569705650U)
										{
											if (text2 == "QualitySetting")
											{
												SaveManager.ConfigData.QualitySetting = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
											}
										}
									}
									else if (text2 == "ScreenWidth")
									{
										SaveManager.ConfigData.ScreenWidth = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
									}
								}
								else if (num2 != 3670229300U)
								{
									if (num2 == 3742089894U)
									{
										if (text2 == "InputIconSetting")
										{
											SaveManager.ConfigData.InputIconSetting = (InputIconSetting)Enum.Parse(typeof(InputIconSetting), text3);
										}
									}
								}
								else if (text2 == "MasterVol")
								{
									SaveManager.ConfigData.MasterVolume = Mathf.Clamp(float.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo), 0f, 1f);
								}
							}
							else if (num2 <= 3901760619U)
							{
								if (num2 != 3889261010U)
								{
									if (num2 == 3901760619U)
									{
										if (text2 == "UserReportName")
										{
											SaveManager.ConfigData.UserReportName = text3;
										}
									}
								}
								else if (text2 == "EnableMusicOnPause")
								{
									SaveManager.ConfigData.EnableMusicOnPause = bool.Parse(text3);
								}
							}
							else if (num2 != 4050388797U)
							{
								if (num2 == 4155496668U)
								{
									if (text2 == "EnableDualButtonDash")
									{
										SaveManager.ConfigData.EnableDualButtonDash = bool.Parse(text3);
									}
								}
							}
							else if (text2 == "SFXVol")
							{
								SaveManager.ConfigData.SFXVolume = Mathf.Clamp(float.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo), 0f, 1f);
							}
						}
					}
				}
				Debug.Log("<color=green>Config file loaded successfully.</color>");
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Could not load config file. Error: " + ex.Message);
		}
		SaveManager.ConfigData.UpdateVersion();
		RewiredOnStartupController.UpdateJoystickCalibrationMap();
		SaveManager.IsConfigFileLoaded = true;
	}

	// Token: 0x060057E6 RID: 22502 RVA: 0x0002FD03 File Offset: 0x0002DF03
	public static void SaveAllControllerMaps()
	{
		SaveManager.SaveControllerMap(false);
		SaveManager.SaveControllerMap(true);
	}

	// Token: 0x060057E7 RID: 22503 RVA: 0x0014F880 File Offset: 0x0014DA80
	public static void SaveControllerMap(bool saveGamepad)
	{
		int mapCategoryID = Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.ActionRemappable);
		int mapCategoryID2 = Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.WindowRemappable);
		Player player = ReInput.players.GetPlayer(0);
		if (!saveGamepad)
		{
			Debug.Log("Attempting to save Keyboard/Mouse controller map...");
			ControllerMap map = player.controllers.maps.GetMap(ControllerType.Keyboard, 0, mapCategoryID, 0);
			ControllerMap map2 = player.controllers.maps.GetMap(ControllerType.Keyboard, 0, mapCategoryID2, 0);
			ControllerMap map3 = player.controllers.maps.GetMap(ControllerType.Mouse, 0, mapCategoryID, 0);
			ControllerMap map4 = player.controllers.maps.GetMap(ControllerType.Mouse, 0, mapCategoryID2, 0);
			SaveManager.SaveMapToXML(map, "KBAction");
			SaveManager.SaveMapToXML(map2, "KBWindow");
			SaveManager.SaveMapToXML(map3, "MouseAction");
			SaveManager.SaveMapToXML(map4, "MouseWindow");
			return;
		}
		Debug.Log("Attempting to save Gamepad controller map...");
		int firstAvailableJoystickControllerID = RewiredOnStartupController.GetFirstAvailableJoystickControllerID();
		if (firstAvailableJoystickControllerID != -1)
		{
			ControllerMap map5 = player.controllers.maps.GetMap(ControllerType.Joystick, firstAvailableJoystickControllerID, mapCategoryID, 0);
			ControllerMap map6 = player.controllers.maps.GetMap(ControllerType.Joystick, firstAvailableJoystickControllerID, mapCategoryID2, 0);
			SaveManager.SaveMapToXML(map5, "GamepadAction");
			SaveManager.SaveMapToXML(map6, "GamepadWindow");
			return;
		}
		Debug.Log("<color=yellow>Failed to save Gamepad controller map. No viable Gamepad connected.</color>");
	}

	// Token: 0x060057E8 RID: 22504 RVA: 0x0014F9A0 File Offset: 0x0014DBA0
	private static void SaveMapToXML(ControllerMap controllerMap, string mapPrependID)
	{
		SaveManager.CreateSaveDirectoriesIfNeeded();
		string controllerPath = SaveManager.GetControllerPath(mapPrependID);
		string str = mapPrependID + "ControllerMapV3RL1.xml";
		if (controllerMap == null)
		{
			Debug.Log("<color=yellow>Could not save Controller Map: " + str + ". Controller map is null (may not be connected).</color>");
			return;
		}
		string s;
		if (controllerMap.controllerType == ControllerType.Joystick)
		{
			s = controllerMap.ToControllerTemplateMap<IGamepadTemplate>().ToXmlString();
		}
		else
		{
			s = controllerMap.ToXmlString();
		}
		try
		{
			SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(-1);
			SaveFileSystem.WriteAllBytes(saveBatch, controllerPath, Encoding.ASCII.GetBytes(s));
			saveBatch.End();
		}
		catch (Exception ex)
		{
			throw new Exception("Could not save Controller Map: " + str + ". Error: " + ex.Message);
		}
		Debug.Log("<color=green>Controller Map " + str + " saved successfully.</color>");
	}

	// Token: 0x060057E9 RID: 22505 RVA: 0x0014FA60 File Offset: 0x0014DC60
	private static string GetControllerPath(string mapPrependID)
	{
		string path = SaveFileSystem.PersistentDataPath;
		path = Path.Combine(path, "Saves");
		string platformDirectoryName = StoreAPIManager.GetPlatformDirectoryName();
		path = Path.Combine(path, platformDirectoryName);
		string userIDString = StoreAPIManager.GetUserIDString();
		if (!string.IsNullOrEmpty(userIDString))
		{
			path = Path.Combine(path, userIDString);
		}
		string path2 = mapPrependID + "ControllerMapV3RL1.xml";
		return Path.Combine(path, path2);
	}

	// Token: 0x060057EA RID: 22506 RVA: 0x0014FAB8 File Offset: 0x0014DCB8
	public static void LoadAllControllerMaps()
	{
		SaveManager.LoadControllerMap(false, 0);
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (!Rewired_RL.IsStandardJoystick(joystick))
			{
				ReInput.controllers.RemoveControllerFromAllPlayers(joystick, true);
			}
			else
			{
				SaveManager.LoadControllerMap(true, joystick.identifier.controllerId);
			}
		}
	}

	// Token: 0x060057EB RID: 22507 RVA: 0x0014FB34 File Offset: 0x0014DD34
	public static void LoadControllerMap(bool loadGamepad, int controllerID = 0)
	{
		int mapCategoryID = Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.ActionRemappable);
		int mapCategoryID2 = Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.WindowRemappable);
		Player player = ReInput.players.GetPlayer(0);
		if (!loadGamepad)
		{
			ControllerMap map = player.controllers.maps.GetMap(ControllerType.Keyboard, 0, mapCategoryID, 0);
			ControllerMap map2 = player.controllers.maps.GetMap(ControllerType.Keyboard, 0, mapCategoryID2, 0);
			ControllerMap map3 = player.controllers.maps.GetMap(ControllerType.Mouse, 0, mapCategoryID, 0);
			ControllerMap map4 = player.controllers.maps.GetMap(ControllerType.Mouse, 0, mapCategoryID2, 0);
			SaveManager.LoadMapFromXML(map, "KBAction");
			SaveManager.LoadMapFromXML(map2, "KBWindow");
			SaveManager.LoadMapFromXML(map3, "MouseAction");
			SaveManager.LoadMapFromXML(map4, "MouseWindow");
			return;
		}
		ControllerMap map5 = player.controllers.maps.GetMap(ControllerType.Joystick, controllerID, mapCategoryID, 0);
		ControllerMap map6 = player.controllers.maps.GetMap(ControllerType.Joystick, controllerID, mapCategoryID2, 0);
		SaveManager.LoadMapFromXML(map5, "GamepadAction");
		SaveManager.LoadMapFromXML(map6, "GamepadWindow");
	}

	// Token: 0x060057EC RID: 22508 RVA: 0x0014FC24 File Offset: 0x0014DE24
	private static void LoadMapFromXML(ControllerMap controllerMap, string mapPrependID)
	{
		string controllerPath = SaveManager.GetControllerPath(mapPrependID);
		string str = mapPrependID + "ControllerMapV3RL1.xml";
		if (controllerMap == null)
		{
			Debug.Log("<color=yellow>Could not load Controller Map: " + str + ". Controller map is null (controller may not be connected).</color>");
			return;
		}
		if (!SaveFileSystem.FileExists(controllerPath))
		{
			return;
		}
		string xmlString = null;
		try
		{
			xmlString = Encoding.ASCII.GetString(SaveFileSystem.ReadAllBytes(controllerPath));
		}
		catch (Exception ex)
		{
			throw new Exception("Could not load Controller Map: " + str + ". Error: " + ex.Message);
		}
		Player player = ReInput.players.GetPlayer(0);
		if (controllerMap.controllerType == ControllerType.Joystick)
		{
			ControllerMap map = ControllerTemplateMap.FromXml(xmlString).ToControllerMap(controllerMap.controller);
			player.controllers.maps.AddMap(controllerMap.controller, map);
		}
		else
		{
			player.controllers.maps.AddMapFromXml(controllerMap.controllerType, controllerMap.controllerId, xmlString);
		}
		Debug.Log("<color=green>Controller Map " + str + " loaded successfully.</color>");
	}

	// Token: 0x17001D71 RID: 7537
	// (get) Token: 0x060057ED RID: 22509 RVA: 0x0002FD11 File Offset: 0x0002DF11
	// (set) Token: 0x060057EE RID: 22510 RVA: 0x0002FD18 File Offset: 0x0002DF18
	public static bool IsProfileConfigFileLoaded { get; private set; }

	// Token: 0x060057EF RID: 22511 RVA: 0x0014FD20 File Offset: 0x0014DF20
	public static void LoadProfileConfigFile()
	{
		string text = SaveManager.GetSaveDirectoryPath(SaveManager.CurrentProfile, false);
		text = Path.Combine(text, "ProfileConfig.ini");
		if (!SaveFileSystem.FileExists(text))
		{
			return;
		}
		try
		{
			byte[] bytes = SaveFileSystem.ReadAllBytes(text);
			using (StringReader stringReader = new StringReader(Encoding.ASCII.GetString(bytes)))
			{
				string text2;
				while ((text2 = stringReader.ReadLine()) != null)
				{
					int num = text2.IndexOf("=");
					if (num != -1)
					{
						string text3 = text2.Substring(0, num);
						string text4 = text2.Substring(num + 1);
						if (!string.IsNullOrEmpty(text4) && text3 != null)
						{
							uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text3);
							if (num2 <= 2067154761U)
							{
								if (num2 != 723308068U)
								{
									if (num2 != 1586689466U)
									{
										if (num2 == 2067154761U)
										{
											if (text3 == "RevisionNumber")
											{
												SaveManager.ProfileConfigData.REVISION_NUMBER = int.Parse(text4, NumberStyles.Any, SaveManager.CultureInfo);
											}
										}
									}
									else if (text3 == "UseNonScientificNames")
									{
										SaveManager.ProfileConfigData.UseNonScientificNames = bool.Parse(text4);
									}
								}
								else if (text3 == "DisablePressDownSpinKick")
								{
									SaveManager.ProfileConfigData.DisablePressDownSpinKick = bool.Parse(text4);
								}
							}
							else if (num2 <= 2950313672U)
							{
								if (num2 != 2464506289U)
								{
									if (num2 == 2950313672U)
									{
										if (text3 == "EnableQuickDrop")
										{
											SaveManager.ProfileConfigData.EnableQuickDrop = bool.Parse(text4);
										}
									}
								}
								else if (text3 == "DisableHitSlowdown")
								{
									SaveManager.ProfileConfigData.DisableSlowdownOnHit = bool.Parse(text4);
								}
							}
							else if (num2 != 3282277021U)
							{
								if (num2 == 4155496668U)
								{
									if (text3 == "EnableDualButtonDash")
									{
										SaveManager.ProfileConfigData.EnableDualButtonDash = bool.Parse(text4);
									}
								}
							}
							else if (text3 == "DisableMouseAiming")
							{
								SaveManager.ProfileConfigData.ToggleMouseAiming = bool.Parse(text4);
							}
						}
					}
				}
				Debug.Log("<color=green>Profile Config file loaded successfully.</color>");
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Could not load profile config file. Error: " + ex.Message);
		}
		SaveManager.ProfileConfigData.UpdateVersion();
		SaveManager.IsProfileConfigFileLoaded = true;
	}

	// Token: 0x060057F0 RID: 22512 RVA: 0x0014FFC8 File Offset: 0x0014E1C8
	public static void SaveProfileConfigFile()
	{
		string text = SaveManager.GetSaveDirectoryPath(SaveManager.CurrentProfile, false);
		text = Path.Combine(text, "ProfileConfig.ini");
		try
		{
			using (StringWriter stringWriter = new StringWriter())
			{
				stringWriter.WriteLine("[VERSION]");
				stringWriter.WriteLine("RevisionNumber=" + SaveManager.ProfileConfigData.REVISION_NUMBER.ToString());
				stringWriter.WriteLine();
				stringWriter.WriteLine("[Game Settings]");
				stringWriter.WriteLine("UseNonScientificNames=" + SaveManager.ProfileConfigData.UseNonScientificNames.ToString());
				stringWriter.WriteLine("EnableDualButtonDash=" + SaveManager.ProfileConfigData.EnableDualButtonDash.ToString());
				stringWriter.WriteLine("EnableQuickDrop=" + SaveManager.ProfileConfigData.EnableQuickDrop.ToString());
				stringWriter.WriteLine("DisableMouseAiming=" + SaveManager.ProfileConfigData.ToggleMouseAiming.ToString());
				stringWriter.WriteLine("DisableHitSlowdown=" + SaveManager.ProfileConfigData.DisableSlowdownOnHit.ToString());
				stringWriter.WriteLine("DisablePressDownSpinKick=" + SaveManager.ProfileConfigData.DisablePressDownSpinKick.ToString());
				stringWriter.WriteLine();
				SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(-1);
				SaveFileSystem.WriteAllBytes(saveBatch, text, Encoding.ASCII.GetBytes(stringWriter.ToString()));
				saveBatch.End();
			}
			Debug.Log("<color=green>Profile Config file saved successfully.</color>");
		}
		catch (Exception ex)
		{
			throw new Exception("Could not save profile config file. Error: " + ex.Message);
		}
	}

	// Token: 0x17001D72 RID: 7538
	// (get) Token: 0x060057F1 RID: 22513 RVA: 0x0002FD20 File Offset: 0x0002DF20
	// (set) Token: 0x060057F2 RID: 22514 RVA: 0x0002FD27 File Offset: 0x0002DF27
	public static bool IsTrailerConfigFileLoaded { get; private set; }

	// Token: 0x060057F3 RID: 22515 RVA: 0x00150174 File Offset: 0x0014E374
	public static void LoadTrailerConfigFile()
	{
		string trailerPath = SaveManager.GetTrailerPath();
		if (!SaveFileSystem.FileExists(trailerPath))
		{
			Debug.Log("<color=yellow>Could not find trailer config file. Did not load successfully.</color>");
			return;
		}
		try
		{
			byte[] bytes = SaveFileSystem.ReadAllBytes(trailerPath);
			using (StringReader stringReader = new StringReader(Encoding.ASCII.GetString(bytes)))
			{
				string text;
				while ((text = stringReader.ReadLine()) != null)
				{
					int num = text.IndexOf("=");
					if (num != -1)
					{
						string text2 = text.Substring(0, num);
						string text3 = text.Substring(num + 1);
						if (!string.IsNullOrEmpty(text3) && text2 != null)
						{
							if (!(text2 == "Trait1"))
							{
								if (!(text2 == "Trait2"))
								{
									if (text2 == "DeathAnim")
									{
										SaveManager.TrailerConfigData.DeathAnim = int.Parse(text3, NumberStyles.Any, SaveManager.CultureInfo);
									}
								}
								else
								{
									SaveManager.TrailerConfigData.Trait2 = text3;
								}
							}
							else
							{
								SaveManager.TrailerConfigData.Trait1 = text3;
							}
						}
					}
				}
			}
			Debug.Log("<color=green>Trailer Config file loaded successfully.</color>");
		}
		catch (Exception ex)
		{
			throw new Exception("Could not load trailer config file. Error: " + ex.Message);
		}
		SaveManager.IsTrailerConfigFileLoaded = true;
	}

	// Token: 0x060057F4 RID: 22516 RVA: 0x001502B8 File Offset: 0x0014E4B8
	private static string GetTrailerPath()
	{
		string path = Path.Combine(SaveFileSystem.PersistentDataPath, "Saves");
		string platformDirectoryName = StoreAPIManager.GetPlatformDirectoryName();
		return Path.Combine(Path.Combine(path, platformDirectoryName), "TrailerConfig.ini");
	}

	// Token: 0x040040CC RID: 16588
	private const string SAVE_PATH_SAVE_FOLDER = "Saves";

	// Token: 0x040040CD RID: 16589
	private const string SAVE_PATH_PROFILE_FOLDER = "Profile";

	// Token: 0x040040CE RID: 16590
	public const string DATE_FORMAT = "dd_MM_yyyy__HH_mm_ss";

	// Token: 0x040040CF RID: 16591
	private const string FOREST_LILY_ROOM1_ID = "Levels Forest Mandatory 1";

	// Token: 0x040040D0 RID: 16592
	private const string FOREST_LILY_ROOM2_ID = "Levels Forest Mandatory 2";

	// Token: 0x040040D1 RID: 16593
	private const string FOREST_LILY_ROOM3_ID = "Levels Forest Mandatory 3";

	// Token: 0x040040D2 RID: 16594
	private const string CAVE_WHITE_KEY_ROOM_ID = "Levels Cave Mandatory 22";

	// Token: 0x040040D3 RID: 16595
	private const string CAVE_BLACK_KEY_ROOM_ID = "Levels Cave Mandatory 19";

	// Token: 0x040040D4 RID: 16596
	public static global::PlayerSaveData PlayerSaveData = new global::PlayerSaveData();

	// Token: 0x040040D5 RID: 16597
	public static StageSaveData StageSaveData = new StageSaveData();

	// Token: 0x040040D6 RID: 16598
	public static EquipmentSaveData EquipmentSaveData = new EquipmentSaveData();

	// Token: 0x040040D7 RID: 16599
	public static LineageSaveData LineageSaveData = new LineageSaveData();

	// Token: 0x040040D8 RID: 16600
	public static ModeSaveData ModeSaveData = new ModeSaveData();

	// Token: 0x040040D9 RID: 16601
	private static Task m_lastAsyncTask;

	// Token: 0x040040DA RID: 16602
	private static bool m_isRunning;

	// Token: 0x040040DB RID: 16603
	private static bool m_hasLevelEditorSaveFailedMessageBeenLogged = false;

	// Token: 0x040040DC RID: 16604
	private static bool m_hasMainMenuSaveFailedMessageBeenLogged = false;

	// Token: 0x040040DD RID: 16605
	private static SaveManager m_instance;

	// Token: 0x040040E3 RID: 16611
	private List<string>[] m_backFilePathsArray = new List<string>[5];

	// Token: 0x040040E4 RID: 16612
	private List<string> m_backupFilesListHelper = new List<string>();

	// Token: 0x040040E5 RID: 16613
	private bool m_checkedForSaveDirectories;

	// Token: 0x040040E6 RID: 16614
	private Action<MonoBehaviour, EventArgs> m_onWorldCreationComplete_UpdateSaveData;

	// Token: 0x040040E7 RID: 16615
	private Action<MonoBehaviour, EventArgs> m_onLevelEditorWorldCreationComplete;

	// Token: 0x040040E8 RID: 16616
	private Action<MonoBehaviour, EventArgs> m_onBiomeCreationComplete_UpdateSaveData;

	// Token: 0x040040EC RID: 16620
	private const string CONFIG_FILE_NAME = "GameConfig.ini";

	// Token: 0x040040ED RID: 16621
	public static ConfigSaveData ConfigData = new ConfigSaveData();

	// Token: 0x040040EF RID: 16623
	private const string CONTROLLERMAP_FILE_NAME = "ControllerMapV3RL1.xml";

	// Token: 0x040040F0 RID: 16624
	private const string PROFILE_CONFIG_FILE_NAME = "ProfileConfig.ini";

	// Token: 0x040040F1 RID: 16625
	public static ProfileConfigSaveData ProfileConfigData = new ProfileConfigSaveData();

	// Token: 0x040040F3 RID: 16627
	private const string TRAILER_CONFIG_FILE_NAME = "TrailerConfig.ini";

	// Token: 0x040040F4 RID: 16628
	public static TrailerConfigData TrailerConfigData = new TrailerConfigData();
}

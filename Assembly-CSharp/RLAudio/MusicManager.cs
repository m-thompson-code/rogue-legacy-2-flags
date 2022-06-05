using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using RLWorldCreation;
using SceneManagement_RL;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008FB RID: 2299
	public class MusicManager : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x06004B6E RID: 19310 RVA: 0x0010F2F9 File Offset: 0x0010D4F9
		// (set) Token: 0x06004B6F RID: 19311 RVA: 0x0010F300 File Offset: 0x0010D500
		public static SongID CurrentSong { get; private set; }

		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x06004B70 RID: 19312 RVA: 0x0010F308 File Offset: 0x0010D508
		// (set) Token: 0x06004B71 RID: 19313 RVA: 0x0010F30F File Offset: 0x0010D50F
		public static SongID LastSongPlayed { get; private set; }

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x06004B72 RID: 19314 RVA: 0x0010F317 File Offset: 0x0010D517
		public static EventInstance CurrentMusicInstance
		{
			get
			{
				return MusicManager.m_currentMusicInstance;
			}
		}

		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x06004B73 RID: 19315 RVA: 0x0010F31E File Offset: 0x0010D51E
		public static bool IsPlayingOverride
		{
			get
			{
				return MusicManager.m_isPlayingOverride;
			}
		}

		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x06004B74 RID: 19316 RVA: 0x0010F325 File Offset: 0x0010D525
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x06004B75 RID: 19317 RVA: 0x0010F32D File Offset: 0x0010D52D
		// (set) Token: 0x06004B76 RID: 19318 RVA: 0x0010F334 File Offset: 0x0010D534
		private static MusicManager Instance { get; set; }

		// Token: 0x06004B77 RID: 19319 RVA: 0x0010F33C File Offset: 0x0010D53C
		private void Awake()
		{
			if (!MusicManager.Instance)
			{
				MusicManager.Instance = this;
				this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
				this.m_onEnterBiome = new Action<MonoBehaviour, EventArgs>(this.OnEnterBiome);
				this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
				this.m_onBossTookDamage = new Action<float>(this.OnBossTookDamage);
				this.m_onBossDefeated = new Action(this.OnBossDefeated);
				this.m_onBossOutroStart = new Action(this.OnBossOutroStart);
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x0010F3D4 File Offset: 0x0010D5D4
		private void Start()
		{
			if (MusicManager.Instance == this)
			{
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onEnterBiome);
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
				SceneLoader_RL.SceneLoadingStartRelay.AddListener(new Action<string>(this.OnSceneStartedLoading), false);
				BiomeTransitionController.TransitionStartRelay.AddListener(new Action<BiomeType, BiomeType>(this.OnBiomeTransitionStart), false);
				MusicLibrary.Initialize();
				this.InitializeInstanceLookupTable();
				this.m_waitBeforePlaying = new WaitRL_Yield(2f, true);
			}
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x0010F460 File Offset: 0x0010D660
		private void OnDestroy()
		{
			if (MusicManager.Instance == this)
			{
				MusicManager.Instance = null;
				Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
				Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onEnterBiome);
				Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
				SceneLoader_RL.SceneLoadingStartRelay.RemoveListener(new Action<string>(this.OnSceneStartedLoading));
				BiomeTransitionController.TransitionStartRelay.RemoveListener(new Action<BiomeType, BiomeType>(this.OnBiomeTransitionStart));
			}
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x0010F4D4 File Offset: 0x0010D6D4
		private void InitializeInstanceLookupTable()
		{
			foreach (SongID songID in SongType_RL.TypeArray)
			{
				string fmodeventPath = MusicLibrary.GetFMODEventPath(songID);
				if (fmodeventPath != string.Empty)
				{
					try
					{
						MusicManager.m_musicInstanceLookupTable.Add(songID, RuntimeManager.CreateInstance(fmodeventPath));
					}
					catch (Exception ex)
					{
						if (ex is EventNotFoundException)
						{
							Debug.LogFormat("<color=red>| MusicManager | No FMOD Event found with path ({0})</color>", new object[]
							{
								fmodeventPath
							});
						}
					}
				}
			}
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x0010F550 File Offset: 0x0010D750
		private void OnEnterBiome(MonoBehaviour sender, EventArgs args)
		{
			if (MusicManager.m_currentMusicInstance.isValid())
			{
				MusicManager.StopMusic();
			}
			BiomeEventArgs biomeEventArgs = args as BiomeEventArgs;
			if (biomeEventArgs != null && biomeEventArgs.Biome == BiomeType.Lineage && SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.Lineage))
			{
				this.PlayBiomeMusic(BiomeType.Lineage);
			}
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x0010F59D File Offset: 0x0010D79D
		private void OnSceneStartedLoading(string sceneName)
		{
			MusicManager.StopMusic();
			MusicManager.m_songToPlayInCastle = SongID.None;
			MusicManager.m_songToPlayInBridge = SongID.None;
			MusicManager.m_songToPlayInForest = SongID.None;
			MusicManager.m_songToPlayInStudy = SongID.None;
			MusicManager.m_songToPlayInTower = SongID.None;
			MusicManager.m_songToPlayInCave = SongID.None;
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x0010F5C8 File Offset: 0x0010D7C8
		private void OnBiomeTransitionStart(BiomeType origin, BiomeType destination)
		{
			if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "leaveLevel"))
			{
				MusicManager.m_currentMusicInstance.setParameterByName("leaveLevel", 0.51f, false);
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "leaveIntro"))
				{
					MusicManager.m_currentMusicInstance.setParameterByName("leaveIntro", 0.51f, false);
					return;
				}
			}
			else
			{
				MusicManager.StopMusic();
			}
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x0010F62C File Offset: 0x0010D82C
		private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
		{
			if (CutsceneManager.IsCutsceneActive)
			{
				return;
			}
			RoomViaDoorEventArgs roomViaDoorEventArgs = args as RoomViaDoorEventArgs;
			if (roomViaDoorEventArgs != null)
			{
				this.UpdateMusicOnRoomChange(roomViaDoorEventArgs);
			}
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x0010F652 File Offset: 0x0010D852
		private void OnPlayerDeath(MonoBehaviour sender, EventArgs args)
		{
			if (this.m_currentBossRoomController)
			{
				this.PlayerDiedInBossRoom();
			}
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x0010F668 File Offset: 0x0010D868
		private void UpdateMusicOnRoomChange(RoomViaDoorEventArgs eventArgs)
		{
			if (eventArgs.Room.RoomType == RoomType.Boss && eventArgs.Room.SpecialRoomType != SpecialRoomType.Subboss)
			{
				this.PlayerEnteredBossRoom(eventArgs);
				return;
			}
			bool flag;
			SongID musicOverride = MusicManager.GetMusicOverride(eventArgs, out flag);
			if (flag)
			{
				if (musicOverride != MusicManager.CurrentSong)
				{
					MusicManager.StopMusic();
					if (musicOverride != SongID.None)
					{
						MusicManager.PlayMusic(musicOverride, true, false);
						return;
					}
				}
			}
			else
			{
				if (MusicManager.CurrentSong == SongID.None || MusicManager.m_isPlayingOverride)
				{
					this.PlayBiomeMusic(eventArgs.Room.BiomeType);
					return;
				}
				if (!MusicManager.m_isPlayingOverride && AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "leaveIntro"))
				{
					MusicManager.m_currentMusicInstance.setParameterByName("leaveIntro", 0.51f, false);
				}
			}
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x0010F710 File Offset: 0x0010D910
		private static SongID GetMusicOverride(RoomViaDoorEventArgs eventArgs, out bool hasOverride)
		{
			SongID result = SongID.None;
			hasOverride = false;
			if (eventArgs.Room is Room)
			{
				RoomMusicOverrideController component = eventArgs.Room.gameObject.GetComponent<RoomMusicOverrideController>();
				if (component)
				{
					result = component.Music;
					hasOverride = true;
				}
			}
			return result;
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x0010F754 File Offset: 0x0010D954
		private SongID GetBiomeMusicForThisRun(BiomeType biome)
		{
			if (biome <= BiomeType.Forest)
			{
				if (biome == BiomeType.Castle)
				{
					return MusicManager.m_songToPlayInCastle;
				}
				if (biome == BiomeType.Cave)
				{
					return MusicManager.m_songToPlayInCave;
				}
				if (biome == BiomeType.Forest)
				{
					return MusicManager.m_songToPlayInForest;
				}
			}
			else if (biome <= BiomeType.Study)
			{
				if (biome == BiomeType.Stone)
				{
					return MusicManager.m_songToPlayInBridge;
				}
				if (biome == BiomeType.Study)
				{
					return MusicManager.m_songToPlayInStudy;
				}
			}
			else if (biome == BiomeType.Tower || biome == BiomeType.TowerExterior)
			{
				return MusicManager.m_songToPlayInTower;
			}
			return SongID.None;
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x0010F7C0 File Offset: 0x0010D9C0
		private void SetBiomeMusicForThisRun(BiomeType biome, SongID song)
		{
			if (biome <= BiomeType.Forest)
			{
				if (biome == BiomeType.Castle)
				{
					MusicManager.m_songToPlayInCastle = song;
					return;
				}
				if (biome == BiomeType.Cave)
				{
					MusicManager.m_songToPlayInCave = song;
					return;
				}
				if (biome != BiomeType.Forest)
				{
					return;
				}
				MusicManager.m_songToPlayInForest = song;
				return;
			}
			else if (biome <= BiomeType.Study)
			{
				if (biome == BiomeType.Stone)
				{
					MusicManager.m_songToPlayInBridge = song;
					return;
				}
				if (biome != BiomeType.Study)
				{
					return;
				}
				MusicManager.m_songToPlayInStudy = song;
				return;
			}
			else
			{
				if (biome != BiomeType.Tower && biome != BiomeType.TowerExterior)
				{
					return;
				}
				MusicManager.m_songToPlayInTower = song;
				return;
			}
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x0010F82C File Offset: 0x0010DA2C
		private void PlayBiomeMusic(BiomeType biome)
		{
			SongID songID = this.GetBiomeMusicForThisRun(biome);
			bool skipIntro = false;
			if (songID == SongID.None)
			{
				SongID[] biomeMusic = MusicLibrary.GetBiomeMusic(biome);
				if (biomeMusic.Length != 0)
				{
					int num = UnityEngine.Random.Range(0, biomeMusic.Length);
					songID = biomeMusic[num];
				}
			}
			else
			{
				skipIntro = true;
			}
			if (songID != SongID.None)
			{
				this.SetBiomeMusicForThisRun(biome, songID);
				MusicManager.PlayMusic(songID, false, skipIntro);
			}
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x0010F878 File Offset: 0x0010DA78
		public static void PlayMusic(SongID music, bool isOverride, bool skipIntro = false)
		{
			if (!MusicLibrary.IsInitialized)
			{
				MusicLibrary.Initialize();
			}
			if (MusicManager.Instance.changeTrackCoroutine != null)
			{
				MusicManager.Instance.StopCoroutine(MusicManager.Instance.changeTrackCoroutine);
			}
			if (MusicManager.CurrentSong != music)
			{
				MusicManager.Instance.changeTrackCoroutine = MusicManager.Instance.StartCoroutine(MusicManager.Instance.MusicTrackChangeCoroutine(music, isOverride, skipIntro));
			}
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x0010F8DA File Offset: 0x0010DADA
		private IEnumerator MusicTrackChangeCoroutine(SongID music, bool isOverride, bool skipIntro)
		{
			if (MusicManager.m_currentMusicInstance.isValid() && AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "leaveLevel"))
			{
				MusicManager.m_currentMusicInstance.setParameterByName("leaveLevel", 0.51f, false);
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "leaveIntro"))
				{
					MusicManager.m_currentMusicInstance.setParameterByName("leaveIntro", 0.51f, false);
				}
				if (MusicManager.CurrentSong != SongID.None)
				{
					yield return this.m_waitBeforePlaying;
				}
			}
			AudioManager.Stop(MusicManager.m_currentMusicInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			if (!MusicManager.m_musicInstanceLookupTable.TryGetValue(music, out MusicManager.m_currentMusicInstance))
			{
				Debug.Log("Music track SongID: " + music.ToString() + " not found in music lookup table.");
			}
			if (MusicManager.m_currentMusicInstance.isValid())
			{
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossHealth"))
				{
					MusicManager.m_currentMusicInstance.setParameterByName("bossHealth", 1f, false);
				}
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossEncounterProgress"))
				{
					RuntimeManager.StudioSystem.setParameterByName("bossEncounterProgress", 0f, false);
				}
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossEncounterProgress_cain"))
				{
					RuntimeManager.StudioSystem.setParameterByName("bossEncounterProgress_cain", 0f, false);
				}
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "skipIntro"))
				{
					if (skipIntro)
					{
						MusicManager.m_currentMusicInstance.setParameterByName("skipIntro", 0.5f, false);
					}
					else
					{
						MusicManager.m_currentMusicInstance.setParameterByName("skipIntro", 0f, false);
					}
				}
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "leaveLevel"))
				{
					MusicManager.m_currentMusicInstance.setParameterByName("leaveLevel", 0f, false);
				}
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "leaveIntro"))
				{
					MusicManager.m_currentMusicInstance.setParameterByName("leaveIntro", 0f, false);
				}
				AudioManager.Play(MusicManager.Instance, MusicManager.m_currentMusicInstance);
			}
			MusicManager.LastSongPlayed = MusicManager.CurrentSong;
			MusicManager.CurrentSong = music;
			MusicManager.m_isPlayingOverride = isOverride;
			this.changeTrackCoroutine = null;
			yield break;
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x0010F900 File Offset: 0x0010DB00
		public static void StopMusic()
		{
			if (MusicManager.m_currentMusicInstance.isValid())
			{
				AudioManager.Stop(MusicManager.m_currentMusicInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (MusicManager.Instance && MusicManager.Instance.changeTrackCoroutine != null)
			{
				MusicManager.Instance.StopCoroutine(MusicManager.Instance.changeTrackCoroutine);
			}
			MusicManager.CurrentSong = SongID.None;
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x0010F956 File Offset: 0x0010DB56
		public static void StopMusicOverride()
		{
			if (MusicManager.m_isPlayingOverride)
			{
				MusicManager.StopMusic();
				MusicManager.PlayMusic(MusicManager.LastSongPlayed, false, true);
			}
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x0010F970 File Offset: 0x0010DB70
		private void PlayerEnteredBossRoom(RoomViaDoorEventArgs eventArgs)
		{
			this.m_currentBossRoomController = eventArgs.Room.gameObject.GetComponentInChildren<BossRoomController>();
			if (this.m_currentBossRoomController)
			{
				this.m_currentBossRoomController.BossTookDamageRelay.AddListener(this.m_onBossTookDamage, false);
				this.m_currentBossRoomController.BossDefeatedRelay.AddListener(this.m_onBossDefeated, false);
				this.m_currentBossRoomController.OutroStartRelay.AddListener(this.m_onBossOutroStart, false);
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Failed to get BossRoomController in boss room. If you see this message please add a bug report to Pivotal along with the Stack Trace.</color>", new object[]
			{
				this
			});
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x0010F9FD File Offset: 0x0010DBFD
		private void PlayerDiedInBossRoom()
		{
			this.UnsubscribeFromBossRoomEvents();
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x0010FA08 File Offset: 0x0010DC08
		private void UnsubscribeFromBossRoomEvents()
		{
			this.m_currentBossRoomController.BossTookDamageRelay.RemoveListener(this.m_onBossTookDamage);
			this.m_currentBossRoomController.BossDefeatedRelay.RemoveListener(this.m_onBossDefeated);
			this.m_currentBossRoomController.OutroStartRelay.RemoveListener(this.m_onBossOutroStart);
			this.m_currentBossRoomController = null;
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x0010FA61 File Offset: 0x0010DC61
		private void OnBossTookDamage(float bossHealthAsPercentage)
		{
			if (MusicManager.m_currentMusicInstance.isValid() && AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossHealth"))
			{
				MusicManager.m_currentMusicInstance.setParameterByName("bossHealth", bossHealthAsPercentage, false);
			}
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x0010FA94 File Offset: 0x0010DC94
		private void OnBossOutroStart()
		{
			if (MusicManager.m_currentMusicInstance.isValid())
			{
				if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossEncounterProgress"))
				{
					RuntimeManager.StudioSystem.setParameterByName("bossEncounterProgress", 1f, false);
					return;
				}
				if (!AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossHealth"))
				{
					MusicManager.StopMusic();
				}
			}
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x0010FAEE File Offset: 0x0010DCEE
		private void OnBossDefeated()
		{
			this.UnsubscribeFromBossRoomEvents();
			MusicManager.CurrentSong = SongID.None;
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x0010FAFC File Offset: 0x0010DCFC
		public static void SetBossEncounterParam(float value)
		{
			if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossEncounterProgress"))
			{
				RuntimeManager.StudioSystem.setParameterByName("bossEncounterProgress", value, false);
			}
		}

		// Token: 0x04003F71 RID: 16241
		private const float START_PLAYING_MUSIC_AFTER_TIME = 2f;

		// Token: 0x04003F72 RID: 16242
		private const string LEAVE_LEVEL_PARAMETER = "leaveLevel";

		// Token: 0x04003F73 RID: 16243
		private const float LEAVE_LEVEL_VALUE = 0.51f;

		// Token: 0x04003F74 RID: 16244
		private const string LEAVE_INTRO_PARAMETER = "leaveIntro";

		// Token: 0x04003F75 RID: 16245
		private const float LEAVE_INTRO_VALUE = 0.51f;

		// Token: 0x04003F76 RID: 16246
		private const string BOSS_HEALTH_PARAMETER = "bossHealth";

		// Token: 0x04003F77 RID: 16247
		private const string BOSS_ENCOUNTER_PROGRESS_PARAMETER = "bossEncounterProgress";

		// Token: 0x04003F78 RID: 16248
		private const string BOSS_ENCOUNTER_PROGRESS_CAIN_PARAMETER = "bossEncounterProgress_cain";

		// Token: 0x04003F79 RID: 16249
		private const string SKIP_INTRO_PARAMETER = "skipIntro";

		// Token: 0x04003F7A RID: 16250
		private const float SKIP_INTRO_VALUE = 0.5f;

		// Token: 0x04003F7B RID: 16251
		private static EventInstance m_currentMusicInstance;

		// Token: 0x04003F7C RID: 16252
		private static Dictionary<SongID, EventInstance> m_musicInstanceLookupTable = new Dictionary<SongID, EventInstance>();

		// Token: 0x04003F7D RID: 16253
		private WaitRL_Yield m_waitBeforePlaying;

		// Token: 0x04003F7E RID: 16254
		private Coroutine changeTrackCoroutine;

		// Token: 0x04003F7F RID: 16255
		private BossRoomController m_currentBossRoomController;

		// Token: 0x04003F80 RID: 16256
		private static bool m_isPlayingOverride;

		// Token: 0x04003F81 RID: 16257
		private static SongID m_songToPlayInCastle;

		// Token: 0x04003F82 RID: 16258
		private static SongID m_songToPlayInBridge;

		// Token: 0x04003F83 RID: 16259
		private static SongID m_songToPlayInForest;

		// Token: 0x04003F84 RID: 16260
		private static SongID m_songToPlayInStudy;

		// Token: 0x04003F85 RID: 16261
		private static SongID m_songToPlayInTower;

		// Token: 0x04003F86 RID: 16262
		private static SongID m_songToPlayInCave;

		// Token: 0x04003F87 RID: 16263
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x04003F88 RID: 16264
		private Action<MonoBehaviour, EventArgs> m_onEnterBiome;

		// Token: 0x04003F89 RID: 16265
		private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

		// Token: 0x04003F8A RID: 16266
		private Action<float> m_onBossTookDamage;

		// Token: 0x04003F8B RID: 16267
		private Action m_onBossDefeated;

		// Token: 0x04003F8C RID: 16268
		private Action m_onBossOutroStart;
	}
}

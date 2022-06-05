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
	// Token: 0x02000E74 RID: 3700
	public class MusicManager : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002153 RID: 8531
		// (get) Token: 0x06006851 RID: 26705 RVA: 0x00039BDE File Offset: 0x00037DDE
		// (set) Token: 0x06006852 RID: 26706 RVA: 0x00039BE5 File Offset: 0x00037DE5
		public static SongID CurrentSong { get; private set; }

		// Token: 0x17002154 RID: 8532
		// (get) Token: 0x06006853 RID: 26707 RVA: 0x00039BED File Offset: 0x00037DED
		// (set) Token: 0x06006854 RID: 26708 RVA: 0x00039BF4 File Offset: 0x00037DF4
		public static SongID LastSongPlayed { get; private set; }

		// Token: 0x17002155 RID: 8533
		// (get) Token: 0x06006855 RID: 26709 RVA: 0x00039BFC File Offset: 0x00037DFC
		public static EventInstance CurrentMusicInstance
		{
			get
			{
				return MusicManager.m_currentMusicInstance;
			}
		}

		// Token: 0x17002156 RID: 8534
		// (get) Token: 0x06006856 RID: 26710 RVA: 0x00039C03 File Offset: 0x00037E03
		public static bool IsPlayingOverride
		{
			get
			{
				return MusicManager.m_isPlayingOverride;
			}
		}

		// Token: 0x17002157 RID: 8535
		// (get) Token: 0x06006857 RID: 26711 RVA: 0x00009A7B File Offset: 0x00007C7B
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x17002158 RID: 8536
		// (get) Token: 0x06006858 RID: 26712 RVA: 0x00039C0A File Offset: 0x00037E0A
		// (set) Token: 0x06006859 RID: 26713 RVA: 0x00039C11 File Offset: 0x00037E11
		private static MusicManager Instance { get; set; }

		// Token: 0x0600685A RID: 26714 RVA: 0x0017F538 File Offset: 0x0017D738
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

		// Token: 0x0600685B RID: 26715 RVA: 0x0017F5D0 File Offset: 0x0017D7D0
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

		// Token: 0x0600685C RID: 26716 RVA: 0x0017F65C File Offset: 0x0017D85C
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

		// Token: 0x0600685D RID: 26717 RVA: 0x0017F6D0 File Offset: 0x0017D8D0
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

		// Token: 0x0600685E RID: 26718 RVA: 0x0017F74C File Offset: 0x0017D94C
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

		// Token: 0x0600685F RID: 26719 RVA: 0x00039C19 File Offset: 0x00037E19
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

		// Token: 0x06006860 RID: 26720 RVA: 0x0017F79C File Offset: 0x0017D99C
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

		// Token: 0x06006861 RID: 26721 RVA: 0x0017F800 File Offset: 0x0017DA00
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

		// Token: 0x06006862 RID: 26722 RVA: 0x00039C44 File Offset: 0x00037E44
		private void OnPlayerDeath(MonoBehaviour sender, EventArgs args)
		{
			if (this.m_currentBossRoomController)
			{
				this.PlayerDiedInBossRoom();
			}
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x0017F828 File Offset: 0x0017DA28
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

		// Token: 0x06006864 RID: 26724 RVA: 0x0017F8D0 File Offset: 0x0017DAD0
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

		// Token: 0x06006865 RID: 26725 RVA: 0x0017F914 File Offset: 0x0017DB14
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

		// Token: 0x06006866 RID: 26726 RVA: 0x0017F980 File Offset: 0x0017DB80
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

		// Token: 0x06006867 RID: 26727 RVA: 0x0017F9EC File Offset: 0x0017DBEC
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

		// Token: 0x06006868 RID: 26728 RVA: 0x0017FA38 File Offset: 0x0017DC38
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

		// Token: 0x06006869 RID: 26729 RVA: 0x00039C59 File Offset: 0x00037E59
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

		// Token: 0x0600686A RID: 26730 RVA: 0x0017FA9C File Offset: 0x0017DC9C
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

		// Token: 0x0600686B RID: 26731 RVA: 0x00039C7D File Offset: 0x00037E7D
		public static void StopMusicOverride()
		{
			if (MusicManager.m_isPlayingOverride)
			{
				MusicManager.StopMusic();
				MusicManager.PlayMusic(MusicManager.LastSongPlayed, false, true);
			}
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x0017FAF4 File Offset: 0x0017DCF4
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

		// Token: 0x0600686D RID: 26733 RVA: 0x00039C97 File Offset: 0x00037E97
		private void PlayerDiedInBossRoom()
		{
			this.UnsubscribeFromBossRoomEvents();
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x0017FB84 File Offset: 0x0017DD84
		private void UnsubscribeFromBossRoomEvents()
		{
			this.m_currentBossRoomController.BossTookDamageRelay.RemoveListener(this.m_onBossTookDamage);
			this.m_currentBossRoomController.BossDefeatedRelay.RemoveListener(this.m_onBossDefeated);
			this.m_currentBossRoomController.OutroStartRelay.RemoveListener(this.m_onBossOutroStart);
			this.m_currentBossRoomController = null;
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x00039C9F File Offset: 0x00037E9F
		private void OnBossTookDamage(float bossHealthAsPercentage)
		{
			if (MusicManager.m_currentMusicInstance.isValid() && AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossHealth"))
			{
				MusicManager.m_currentMusicInstance.setParameterByName("bossHealth", bossHealthAsPercentage, false);
			}
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x0017FBE0 File Offset: 0x0017DDE0
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

		// Token: 0x06006871 RID: 26737 RVA: 0x00039CD0 File Offset: 0x00037ED0
		private void OnBossDefeated()
		{
			this.UnsubscribeFromBossRoomEvents();
			MusicManager.CurrentSong = SongID.None;
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x0017FC3C File Offset: 0x0017DE3C
		public static void SetBossEncounterParam(float value)
		{
			if (AudioUtility.GetHasParameter(MusicManager.m_currentMusicInstance, "bossEncounterProgress"))
			{
				RuntimeManager.StudioSystem.setParameterByName("bossEncounterProgress", value, false);
			}
		}

		// Token: 0x040054BD RID: 21693
		private const float START_PLAYING_MUSIC_AFTER_TIME = 2f;

		// Token: 0x040054BE RID: 21694
		private const string LEAVE_LEVEL_PARAMETER = "leaveLevel";

		// Token: 0x040054BF RID: 21695
		private const float LEAVE_LEVEL_VALUE = 0.51f;

		// Token: 0x040054C0 RID: 21696
		private const string LEAVE_INTRO_PARAMETER = "leaveIntro";

		// Token: 0x040054C1 RID: 21697
		private const float LEAVE_INTRO_VALUE = 0.51f;

		// Token: 0x040054C2 RID: 21698
		private const string BOSS_HEALTH_PARAMETER = "bossHealth";

		// Token: 0x040054C3 RID: 21699
		private const string BOSS_ENCOUNTER_PROGRESS_PARAMETER = "bossEncounterProgress";

		// Token: 0x040054C4 RID: 21700
		private const string BOSS_ENCOUNTER_PROGRESS_CAIN_PARAMETER = "bossEncounterProgress_cain";

		// Token: 0x040054C5 RID: 21701
		private const string SKIP_INTRO_PARAMETER = "skipIntro";

		// Token: 0x040054C6 RID: 21702
		private const float SKIP_INTRO_VALUE = 0.5f;

		// Token: 0x040054C7 RID: 21703
		private static EventInstance m_currentMusicInstance;

		// Token: 0x040054C8 RID: 21704
		private static Dictionary<SongID, EventInstance> m_musicInstanceLookupTable = new Dictionary<SongID, EventInstance>();

		// Token: 0x040054C9 RID: 21705
		private WaitRL_Yield m_waitBeforePlaying;

		// Token: 0x040054CA RID: 21706
		private Coroutine changeTrackCoroutine;

		// Token: 0x040054CB RID: 21707
		private BossRoomController m_currentBossRoomController;

		// Token: 0x040054CC RID: 21708
		private static bool m_isPlayingOverride;

		// Token: 0x040054CD RID: 21709
		private static SongID m_songToPlayInCastle;

		// Token: 0x040054CE RID: 21710
		private static SongID m_songToPlayInBridge;

		// Token: 0x040054CF RID: 21711
		private static SongID m_songToPlayInForest;

		// Token: 0x040054D0 RID: 21712
		private static SongID m_songToPlayInStudy;

		// Token: 0x040054D1 RID: 21713
		private static SongID m_songToPlayInTower;

		// Token: 0x040054D2 RID: 21714
		private static SongID m_songToPlayInCave;

		// Token: 0x040054D3 RID: 21715
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x040054D4 RID: 21716
		private Action<MonoBehaviour, EventArgs> m_onEnterBiome;

		// Token: 0x040054D5 RID: 21717
		private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

		// Token: 0x040054D6 RID: 21718
		private Action<float> m_onBossTookDamage;

		// Token: 0x040054D7 RID: 21719
		private Action m_onBossDefeated;

		// Token: 0x040054D8 RID: 21720
		private Action m_onBossOutroStart;
	}
}

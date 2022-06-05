using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RLAudio
{
	// Token: 0x02000E3F RID: 3647
	public class AmbientSoundController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x170020F7 RID: 8439
		// (get) Token: 0x060066C7 RID: 26311 RVA: 0x00009A7B File Offset: 0x00007C7B
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x170020F8 RID: 8440
		// (get) Token: 0x060066C8 RID: 26312 RVA: 0x0003892B File Offset: 0x00036B2B
		// (set) Token: 0x060066C9 RID: 26313 RVA: 0x00038933 File Offset: 0x00036B33
		public EventInstance CurrentEvent
		{
			get
			{
				return this.m_currentEvent;
			}
			private set
			{
				this.m_currentEvent = value;
			}
		}

		// Token: 0x170020F9 RID: 8441
		// (get) Token: 0x060066CA RID: 26314 RVA: 0x0003893C File Offset: 0x00036B3C
		// (set) Token: 0x060066CB RID: 26315 RVA: 0x00038944 File Offset: 0x00036B44
		public EventInstance CurrentSnapshot
		{
			get
			{
				return this.m_currentSnapshot;
			}
			private set
			{
				this.m_currentSnapshot = value;
			}
		}

		// Token: 0x170020FA RID: 8442
		// (get) Token: 0x060066CC RID: 26316 RVA: 0x0003894D File Offset: 0x00036B4D
		// (set) Token: 0x060066CD RID: 26317 RVA: 0x00038954 File Offset: 0x00036B54
		public static AmbientSoundController Instance
		{
			get
			{
				return AmbientSoundController.m_instance;
			}
			private set
			{
				AmbientSoundController.m_instance = value;
			}
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x0017BAE4 File Offset: 0x00179CE4
		private void Awake()
		{
			if (!AmbientSoundController.Instance)
			{
				AmbientSoundController.Instance = this;
				this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
				this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
				this.m_onBiomeEnter = new Action<MonoBehaviour, EventArgs>(this.OnBiomeEnter);
				this.m_onEnterMainMenu = new Action<MonoBehaviour, EventArgs>(this.OnEnterMainMenu);
				this.m_onSkillTreeWindowOpenedUnityEvent = new UnityAction(this.OnSkillTreeWindowOpened);
				this.m_onSkillTreeWindowClosedUnityEvent = new UnityAction(this.OnSkillTreeWindowClosed);
				this.m_onTransitionStart = new Action(this.OnTransitionStart);
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060066CF RID: 26319 RVA: 0x0017BB90 File Offset: 0x00179D90
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
			Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.EnterMainMenu, this.m_onEnterMainMenu);
			SceneLoader_RL.TransitionStartRelay.AddListener(this.m_onTransitionStart, false);
		}

		// Token: 0x060066D0 RID: 26320 RVA: 0x0017BBE4 File Offset: 0x00179DE4
		private void OnDestroy()
		{
			if (AmbientSoundController.Instance == this)
			{
				AmbientSoundController.Instance = null;
				Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
				Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
				Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
				Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.EnterMainMenu, this.m_onEnterMainMenu);
				SceneLoader_RL.TransitionStartRelay.RemoveListener(this.m_onTransitionStart);
				if (AmbientSoundController.m_antiquarianSnapshot.isValid())
				{
					AmbientSoundController.m_antiquarianSnapshot.release();
				}
				if (AmbientSoundController.m_toneDeafSnapshot.isValid())
				{
					AmbientSoundController.m_toneDeafSnapshot.release();
				}
			}
		}

		// Token: 0x060066D1 RID: 26321 RVA: 0x0017BC78 File Offset: 0x00179E78
		private void OnBiomeEnter(MonoBehaviour sender, EventArgs args)
		{
			if (args is BiomeEventArgs)
			{
				BiomeEventArgs biomeEventArgs = args as BiomeEventArgs;
				if (biomeEventArgs.Biome == BiomeType.Lineage && SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.Lineage))
				{
					AmbientSoundController.AmbientSoundControllerTableKey tableKey = this.GetTableKey(BiomeType.Lineage, RoomType.Transition, false);
					if (!this.m_audioInstanceTable.ContainsKey(tableKey))
					{
						this.AddTableEntry(tableKey);
					}
					this.PlayAudio(tableKey, null);
					return;
				}
				if (biomeEventArgs.Biome == BiomeType.HubTown)
				{
					this.OnEnterHubTown();
				}
			}
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x0003895C File Offset: 0x00036B5C
		private void OnTransitionStart()
		{
			if (this.StopOnTransition)
			{
				this.StopCurrentAmbientSoundAndSnapshot();
			}
		}

		// Token: 0x060066D3 RID: 26323 RVA: 0x0017BCEC File Offset: 0x00179EEC
		private void OnEnterHubTown()
		{
			SceneManager.sceneLoaded -= this.OnHubTownSceneClosed;
			SceneManager.sceneLoaded += this.OnHubTownSceneClosed;
			WindowController windowController = WindowManager.GetWindowController(WindowID.SkillTree);
			if (windowController)
			{
				windowController.WindowOpenedEvent.RemoveListener(this.m_onSkillTreeWindowOpenedUnityEvent);
				windowController.WindowClosedEvent.RemoveListener(this.m_onSkillTreeWindowClosedUnityEvent);
				windowController.WindowOpenedEvent.AddListener(this.m_onSkillTreeWindowOpenedUnityEvent);
				windowController.WindowClosedEvent.AddListener(this.m_onSkillTreeWindowClosedUnityEvent);
			}
			this.GetTableKey(BiomeType.HubTown, RoomType.Standard, false);
			this.GetTableKey(BiomeType.HubTown, RoomType.Transition, true);
		}

		// Token: 0x060066D4 RID: 26324 RVA: 0x0017BD88 File Offset: 0x00179F88
		private void OnHubTownSceneClosed(Scene scene, LoadSceneMode mode)
		{
			SceneManager.sceneLoaded -= this.OnHubTownSceneClosed;
			if (WindowManager.GetIsWindowLoaded(WindowID.SkillTree))
			{
				WindowController windowController = WindowManager.GetWindowController(WindowID.SkillTree);
				if (windowController != null)
				{
					windowController.WindowOpenedEvent.RemoveListener(new UnityAction(this.OnSkillTreeWindowOpened));
					windowController.WindowClosedEvent.RemoveListener(new UnityAction(this.OnSkillTreeWindowClosed));
				}
			}
		}

		// Token: 0x060066D5 RID: 26325 RVA: 0x0017BDEC File Offset: 0x00179FEC
		private void OnSkillTreeWindowOpened()
		{
			AmbientSoundController.AmbientSoundControllerTableKey tableKey = this.GetTableKey(BiomeType.HubTown, RoomType.Standard, false);
			if (!this.m_audioInstanceTable.ContainsKey(tableKey))
			{
				this.AddTableEntry(tableKey);
			}
			this.PlayAudio(tableKey, null);
			this.m_inSkillTree = true;
		}

		// Token: 0x060066D6 RID: 26326 RVA: 0x0017BE28 File Offset: 0x0017A028
		private void OnSkillTreeWindowClosed()
		{
			AmbientSoundController.AmbientSoundControllerTableKey tableKey = this.GetTableKey(BiomeType.HubTown, RoomType.Transition, true);
			if (!this.m_audioInstanceTable.ContainsKey(tableKey))
			{
				this.AddTableEntry(tableKey);
			}
			this.PlayAudio(tableKey, null);
			this.m_inSkillTree = false;
		}

		// Token: 0x060066D7 RID: 26327 RVA: 0x0003896C File Offset: 0x00036B6C
		private void OnEnterMainMenu(MonoBehaviour sender, EventArgs args)
		{
			this.StopCurrentAmbientSoundAndSnapshot();
		}

		// Token: 0x060066D8 RID: 26328 RVA: 0x00038974 File Offset: 0x00036B74
		public static EventInstance GetCurrentEventInstance()
		{
			return AmbientSoundController.Instance.CurrentEvent;
		}

		// Token: 0x060066D9 RID: 26329 RVA: 0x0003896C File Offset: 0x00036B6C
		private void OnPlayerDeath(MonoBehaviour sender, EventArgs args)
		{
			this.StopCurrentAmbientSoundAndSnapshot();
		}

		// Token: 0x060066DA RID: 26330 RVA: 0x0017BE68 File Offset: 0x0017A068
		private void StopCurrentAmbientSoundAndSnapshot()
		{
			if (this.CurrentEvent.isValid())
			{
				AudioManager.Stop(this.CurrentEvent, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.CurrentEvent = default(EventInstance);
			}
			if (this.CurrentSnapshot.isValid())
			{
				AudioManager.Stop(this.CurrentSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.CurrentSnapshot = default(EventInstance);
			}
		}

		// Token: 0x060066DB RID: 26331 RVA: 0x0017BECC File Offset: 0x0017A0CC
		private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
		{
			if (this.m_inSkillTree)
			{
				return;
			}
			RoomViaDoorEventArgs roomViaDoorEventArgs = args as RoomViaDoorEventArgs;
			if (roomViaDoorEventArgs != null)
			{
				AmbientSoundController.AmbientSoundControllerTableKey tableKey = this.GetTableKey(roomViaDoorEventArgs.Room);
				if (!this.m_audioInstanceTable.ContainsKey(tableKey))
				{
					this.AddTableEntry(tableKey);
				}
				AmbientSoundOverride component = roomViaDoorEventArgs.Room.gameObject.GetComponent<AmbientSoundOverride>();
				this.PlayAudio(tableKey, component);
				if (component && AudioUtility.GetHasParameter(this.CurrentEvent, "finalBindingStone_activating"))
				{
					this.CurrentEvent.setParameterByName("finalBindingStone_activating", 0f, false);
					return;
				}
			}
			else
			{
				Debug.LogFormat("<color=red>| {0} | Failed to cast event args as RoomViaDoorEventArgs</color>", new object[]
				{
					this
				});
			}
		}

		// Token: 0x060066DC RID: 26332 RVA: 0x0017BF70 File Offset: 0x0017A170
		private void AddTableEntry(AmbientSoundController.AmbientSoundControllerTableKey key)
		{
			string ambientAudioSnapshotPath = AmbientSoundLibrary.GetAmbientAudioSnapshotPath(key.Biome, key.RoomType, key.IsLarge);
			EventInstance value = default(EventInstance);
			if (!string.IsNullOrEmpty(ambientAudioSnapshotPath))
			{
				value = RuntimeManager.CreateInstance(ambientAudioSnapshotPath);
			}
			this.m_snapshotInstanceTable.Add(key, value);
			string[] ambientAudioEventPaths = AmbientSoundLibrary.GetAmbientAudioEventPaths(key.Biome, key.RoomType, key.IsLarge);
			this.m_audioInstanceTable.Add(key, new EventInstance[ambientAudioEventPaths.Length]);
			for (int i = 0; i < ambientAudioEventPaths.Length; i++)
			{
				this.m_audioInstanceTable[key][i] = RuntimeManager.CreateInstance(ambientAudioEventPaths[i]);
			}
		}

		// Token: 0x060066DD RID: 26333 RVA: 0x0017C014 File Offset: 0x0017A214
		private EventInstance[] GetPotentialAudio(AmbientSoundController.AmbientSoundControllerTableKey key, AmbientSoundOverride soundOverride)
		{
			EventInstance[] result;
			if (soundOverride != null && soundOverride.HasAudioOverride)
			{
				result = soundOverride.Audio;
			}
			else
			{
				result = this.m_audioInstanceTable[key];
			}
			return result;
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x0017C04C File Offset: 0x0017A24C
		private EventInstance GetSnapshot(AmbientSoundController.AmbientSoundControllerTableKey key, AmbientSoundOverride soundOverride)
		{
			bool flag = TraitManager.IsInitialized && TraitManager.IsTraitActive(TraitType.OldYellowTint);
			bool flag2 = TraitManager.IsInitialized && TraitManager.IsTraitActive(TraitType.ToneDeaf);
			EventInstance result;
			if (!flag && !flag2)
			{
				if (soundOverride != null && soundOverride.HasSnapshotOverride)
				{
					result = soundOverride.Snapshot;
				}
				else
				{
					result = this.m_snapshotInstanceTable[key];
				}
			}
			else if (flag)
			{
				if (!AmbientSoundController.m_antiquarianSnapshot.isValid())
				{
					AmbientSoundController.m_antiquarianSnapshot = AudioUtility.GetEventInstance("snapshot:/Trait_Antique", base.transform);
				}
				result = AmbientSoundController.m_antiquarianSnapshot;
			}
			else
			{
				if (!AmbientSoundController.m_toneDeafSnapshot.isValid())
				{
					AmbientSoundController.m_toneDeafSnapshot = AudioUtility.GetEventInstance("snapshot:/Trait_Tonedeaf", base.transform);
				}
				result = AmbientSoundController.m_toneDeafSnapshot;
			}
			return result;
		}

		// Token: 0x060066DF RID: 26335 RVA: 0x0017C108 File Offset: 0x0017A308
		private AmbientSoundController.AmbientSoundControllerTableKey GetTableKey(BaseRoom room)
		{
			BiomeType appearanceBiomeType = room.AppearanceBiomeType;
			RoomType roomType = this.GetRoomType(room);
			bool isRoomLarge = RoomUtility.GetIsRoomLarge(room);
			return this.GetTableKey(appearanceBiomeType, roomType, isRoomLarge);
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x00038980 File Offset: 0x00036B80
		private AmbientSoundController.AmbientSoundControllerTableKey GetTableKey(BiomeType biome, RoomType roomType, bool isRoomLarge)
		{
			return new AmbientSoundController.AmbientSoundControllerTableKey(biome, roomType, isRoomLarge);
		}

		// Token: 0x060066E1 RID: 26337 RVA: 0x0017C134 File Offset: 0x0017A334
		private RoomType GetRoomType(BaseRoom room)
		{
			RoomType result = RoomType.Standard;
			if (room is Room)
			{
				Room room2 = room as Room;
				result = room2.RoomType;
				if (room2.RoomType == RoomType.Fairy)
				{
					FairyRoomController component = room2.gameObject.GetComponent<FairyRoomController>();
					if (component != null && component.FairyRoomRuleEntries != null)
					{
						if (component.State == FairyRoomState.Failed || component.State == FairyRoomState.Passed)
						{
							result = RoomType.Standard;
						}
						else
						{
							for (int i = 0; i < component.FairyRoomRuleEntries.Count; i++)
							{
								if (component.FairyRoomRuleEntries[i].FairyRuleID == FairyRuleID.HiddenChest)
								{
									result = RoomType.Standard;
									break;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x0017C1C8 File Offset: 0x0017A3C8
		private void PlayAudio(AmbientSoundController.AmbientSoundControllerTableKey key, AmbientSoundOverride ambientSoundOverride)
		{
			if (this.m_muteAmbientSFX)
			{
				return;
			}
			EventInstance[] potentialAudio = this.GetPotentialAudio(key, ambientSoundOverride);
			bool isRaining = false;
			if (BiomeCreation_EV.IS_IT_RAINING_IN_BIOME_TABLE.ContainsKey(key.Biome))
			{
				isRaining = BiomeCreation_EV.IS_IT_RAINING_IN_BIOME_TABLE[key.Biome];
			}
			this.PlayRandomAmbientAudio(potentialAudio, isRaining);
			EventInstance snapshot = this.GetSnapshot(key, ambientSoundOverride);
			this.PlaySnapshot(snapshot);
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x0017C228 File Offset: 0x0017A428
		public void PlayManorAmbientAudio()
		{
			if (this.m_muteAmbientSFX)
			{
				return;
			}
			AmbientSoundController.AmbientSoundControllerTableKey tableKey = this.GetTableKey(BiomeType.Lineage, RoomType.Standard, true);
			if (!this.m_audioInstanceTable.ContainsKey(tableKey))
			{
				this.AddTableEntry(tableKey);
			}
			this.PlayAudio(tableKey, null);
			this.PlaySnapshot(this.GetSnapshot(tableKey, null));
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x0017C274 File Offset: 0x0017A474
		private void PlayRandomAmbientAudio(EventInstance[] eventInstances, bool isRaining)
		{
			if (eventInstances.Length != 0)
			{
				int num = 0;
				if (eventInstances.Length > 1)
				{
					num = UnityEngine.Random.Range(0, eventInstances.Length);
				}
				if (!this.CurrentEvent.Equals(eventInstances[num]))
				{
					AudioManager.Stop(this.CurrentEvent, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					this.CurrentEvent = eventInstances[num];
					AudioManager.Play(this, this.CurrentEvent);
					float value = 0f;
					if (isRaining)
					{
						value = 1f;
					}
					this.CurrentEvent.setParameterByName("Weather", value, false);
				}
			}
		}

		// Token: 0x060066E5 RID: 26341 RVA: 0x0017C304 File Offset: 0x0017A504
		private void PlaySnapshot(EventInstance eventInstance)
		{
			if (eventInstance.isValid() && !this.CurrentSnapshot.Equals(eventInstance))
			{
				AudioManager.Stop(this.CurrentSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.CurrentSnapshot = eventInstance;
				AudioManager.Play(this, this.CurrentSnapshot);
			}
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x0003898A File Offset: 0x00036B8A
		public static void SetSnapshotOverride(EventInstance eventInstance)
		{
			AmbientSoundController.Instance.m_previousSnapshot = AmbientSoundController.Instance.CurrentSnapshot;
			AmbientSoundController.Instance.PlaySnapshot(eventInstance);
		}

		// Token: 0x060066E7 RID: 26343 RVA: 0x000389AB File Offset: 0x00036BAB
		public static void ClearSnapshotOverride()
		{
			AmbientSoundController.Instance.PlaySnapshot(AmbientSoundController.Instance.m_previousSnapshot);
		}

		// Token: 0x04005366 RID: 21350
		[SerializeField]
		private bool m_muteAmbientSFX;

		// Token: 0x04005367 RID: 21351
		private const string ANTIQUE_TRAIT_SNAPSHOT_PATH = "snapshot:/Trait_Antique";

		// Token: 0x04005368 RID: 21352
		private const string TONE_DEAF_TRAIT_SNAPSHOT_PATH = "snapshot:/Trait_Tonedeaf";

		// Token: 0x04005369 RID: 21353
		private Dictionary<AmbientSoundController.AmbientSoundControllerTableKey, EventInstance[]> m_audioInstanceTable = new Dictionary<AmbientSoundController.AmbientSoundControllerTableKey, EventInstance[]>();

		// Token: 0x0400536A RID: 21354
		private Dictionary<AmbientSoundController.AmbientSoundControllerTableKey, EventInstance> m_snapshotInstanceTable = new Dictionary<AmbientSoundController.AmbientSoundControllerTableKey, EventInstance>();

		// Token: 0x0400536B RID: 21355
		private EventInstance m_currentEvent;

		// Token: 0x0400536C RID: 21356
		private EventInstance m_currentSnapshot;

		// Token: 0x0400536D RID: 21357
		private EventInstance m_previousSnapshot;

		// Token: 0x0400536E RID: 21358
		private bool m_inSkillTree;

		// Token: 0x0400536F RID: 21359
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x04005370 RID: 21360
		private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

		// Token: 0x04005371 RID: 21361
		private Action<MonoBehaviour, EventArgs> m_onBiomeEnter;

		// Token: 0x04005372 RID: 21362
		private Action<MonoBehaviour, EventArgs> m_onEnterMainMenu;

		// Token: 0x04005373 RID: 21363
		private Action m_onTransitionStart;

		// Token: 0x04005374 RID: 21364
		private UnityAction m_onSkillTreeWindowOpenedUnityEvent;

		// Token: 0x04005375 RID: 21365
		private UnityAction m_onSkillTreeWindowClosedUnityEvent;

		// Token: 0x04005376 RID: 21366
		private static AmbientSoundController m_instance;

		// Token: 0x04005377 RID: 21367
		private static EventInstance m_antiquarianSnapshot;

		// Token: 0x04005378 RID: 21368
		private static EventInstance m_toneDeafSnapshot;

		// Token: 0x04005379 RID: 21369
		public bool StopOnTransition = true;

		// Token: 0x02000E40 RID: 3648
		private struct AmbientSoundControllerTableKey : IEquatable<AmbientSoundController.AmbientSoundControllerTableKey>
		{
			// Token: 0x060066EA RID: 26346 RVA: 0x000389E6 File Offset: 0x00036BE6
			public AmbientSoundControllerTableKey(BiomeType biome, RoomType roomType, bool isLarge)
			{
				this.Biome = biome;
				this.RoomType = roomType;
				this.IsLarge = isLarge;
			}

			// Token: 0x170020FB RID: 8443
			// (get) Token: 0x060066EB RID: 26347 RVA: 0x000389FD File Offset: 0x00036BFD
			public readonly BiomeType Biome { get; }

			// Token: 0x170020FC RID: 8444
			// (get) Token: 0x060066EC RID: 26348 RVA: 0x00038A05 File Offset: 0x00036C05
			public readonly RoomType RoomType { get; }

			// Token: 0x170020FD RID: 8445
			// (get) Token: 0x060066ED RID: 26349 RVA: 0x00038A0D File Offset: 0x00036C0D
			public readonly bool IsLarge { get; }

			// Token: 0x060066EE RID: 26350 RVA: 0x00038A15 File Offset: 0x00036C15
			public static bool operator ==(AmbientSoundController.AmbientSoundControllerTableKey keyA, AmbientSoundController.AmbientSoundControllerTableKey keyB)
			{
				return keyA.Equals(keyB);
			}

			// Token: 0x060066EF RID: 26351 RVA: 0x00038A1F File Offset: 0x00036C1F
			public static bool operator !=(AmbientSoundController.AmbientSoundControllerTableKey keyA, AmbientSoundController.AmbientSoundControllerTableKey keyB)
			{
				return !keyA.Equals(keyB);
			}

			// Token: 0x060066F0 RID: 26352 RVA: 0x00038A2C File Offset: 0x00036C2C
			public bool Equals(AmbientSoundController.AmbientSoundControllerTableKey other)
			{
				return this.Biome == other.Biome && this.RoomType == other.RoomType && this.IsLarge == other.IsLarge;
			}

			// Token: 0x060066F1 RID: 26353 RVA: 0x00038A5D File Offset: 0x00036C5D
			public override bool Equals(object obj)
			{
				return obj is AmbientSoundController.AmbientSoundControllerTableKey && this.Equals((AmbientSoundController.AmbientSoundControllerTableKey)obj);
			}

			// Token: 0x060066F2 RID: 26354 RVA: 0x0017C358 File Offset: 0x0017A558
			public override int GetHashCode()
			{
				return ((-1554893279 * -1521134295 + this.Biome.GetHashCode()) * -1521134295 + this.RoomType.GetHashCode()) * -1521134295 + this.IsLarge.GetHashCode();
			}
		}
	}
}

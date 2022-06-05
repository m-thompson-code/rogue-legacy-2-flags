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
	// Token: 0x020008D2 RID: 2258
	public class AmbientSoundController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x06004A20 RID: 18976 RVA: 0x0010AFDA File Offset: 0x001091DA
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x06004A21 RID: 18977 RVA: 0x0010AFE2 File Offset: 0x001091E2
		// (set) Token: 0x06004A22 RID: 18978 RVA: 0x0010AFEA File Offset: 0x001091EA
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

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x06004A23 RID: 18979 RVA: 0x0010AFF3 File Offset: 0x001091F3
		// (set) Token: 0x06004A24 RID: 18980 RVA: 0x0010AFFB File Offset: 0x001091FB
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

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x06004A25 RID: 18981 RVA: 0x0010B004 File Offset: 0x00109204
		// (set) Token: 0x06004A26 RID: 18982 RVA: 0x0010B00B File Offset: 0x0010920B
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

		// Token: 0x06004A27 RID: 18983 RVA: 0x0010B014 File Offset: 0x00109214
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

		// Token: 0x06004A28 RID: 18984 RVA: 0x0010B0C0 File Offset: 0x001092C0
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
			Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.EnterMainMenu, this.m_onEnterMainMenu);
			SceneLoader_RL.TransitionStartRelay.AddListener(this.m_onTransitionStart, false);
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x0010B114 File Offset: 0x00109314
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

		// Token: 0x06004A2A RID: 18986 RVA: 0x0010B1A8 File Offset: 0x001093A8
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

		// Token: 0x06004A2B RID: 18987 RVA: 0x0010B21B File Offset: 0x0010941B
		private void OnTransitionStart()
		{
			if (this.StopOnTransition)
			{
				this.StopCurrentAmbientSoundAndSnapshot();
			}
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x0010B22C File Offset: 0x0010942C
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

		// Token: 0x06004A2D RID: 18989 RVA: 0x0010B2C8 File Offset: 0x001094C8
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

		// Token: 0x06004A2E RID: 18990 RVA: 0x0010B32C File Offset: 0x0010952C
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

		// Token: 0x06004A2F RID: 18991 RVA: 0x0010B368 File Offset: 0x00109568
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

		// Token: 0x06004A30 RID: 18992 RVA: 0x0010B3A5 File Offset: 0x001095A5
		private void OnEnterMainMenu(MonoBehaviour sender, EventArgs args)
		{
			this.StopCurrentAmbientSoundAndSnapshot();
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x0010B3AD File Offset: 0x001095AD
		public static EventInstance GetCurrentEventInstance()
		{
			return AmbientSoundController.Instance.CurrentEvent;
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x0010B3B9 File Offset: 0x001095B9
		private void OnPlayerDeath(MonoBehaviour sender, EventArgs args)
		{
			this.StopCurrentAmbientSoundAndSnapshot();
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x0010B3C4 File Offset: 0x001095C4
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

		// Token: 0x06004A34 RID: 18996 RVA: 0x0010B428 File Offset: 0x00109628
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

		// Token: 0x06004A35 RID: 18997 RVA: 0x0010B4CC File Offset: 0x001096CC
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

		// Token: 0x06004A36 RID: 18998 RVA: 0x0010B570 File Offset: 0x00109770
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

		// Token: 0x06004A37 RID: 18999 RVA: 0x0010B5A8 File Offset: 0x001097A8
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

		// Token: 0x06004A38 RID: 19000 RVA: 0x0010B664 File Offset: 0x00109864
		private AmbientSoundController.AmbientSoundControllerTableKey GetTableKey(BaseRoom room)
		{
			BiomeType appearanceBiomeType = room.AppearanceBiomeType;
			RoomType roomType = this.GetRoomType(room);
			bool isRoomLarge = RoomUtility.GetIsRoomLarge(room);
			return this.GetTableKey(appearanceBiomeType, roomType, isRoomLarge);
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x0010B690 File Offset: 0x00109890
		private AmbientSoundController.AmbientSoundControllerTableKey GetTableKey(BiomeType biome, RoomType roomType, bool isRoomLarge)
		{
			return new AmbientSoundController.AmbientSoundControllerTableKey(biome, roomType, isRoomLarge);
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x0010B69C File Offset: 0x0010989C
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

		// Token: 0x06004A3B RID: 19003 RVA: 0x0010B730 File Offset: 0x00109930
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

		// Token: 0x06004A3C RID: 19004 RVA: 0x0010B790 File Offset: 0x00109990
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

		// Token: 0x06004A3D RID: 19005 RVA: 0x0010B7DC File Offset: 0x001099DC
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

		// Token: 0x06004A3E RID: 19006 RVA: 0x0010B86C File Offset: 0x00109A6C
		private void PlaySnapshot(EventInstance eventInstance)
		{
			if (eventInstance.isValid() && !this.CurrentSnapshot.Equals(eventInstance))
			{
				AudioManager.Stop(this.CurrentSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.CurrentSnapshot = eventInstance;
				AudioManager.Play(this, this.CurrentSnapshot);
			}
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x0010B8BD File Offset: 0x00109ABD
		public static void SetSnapshotOverride(EventInstance eventInstance)
		{
			AmbientSoundController.Instance.m_previousSnapshot = AmbientSoundController.Instance.CurrentSnapshot;
			AmbientSoundController.Instance.PlaySnapshot(eventInstance);
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x0010B8DE File Offset: 0x00109ADE
		public static void ClearSnapshotOverride()
		{
			AmbientSoundController.Instance.PlaySnapshot(AmbientSoundController.Instance.m_previousSnapshot);
		}

		// Token: 0x04003E54 RID: 15956
		[SerializeField]
		private bool m_muteAmbientSFX;

		// Token: 0x04003E55 RID: 15957
		private const string ANTIQUE_TRAIT_SNAPSHOT_PATH = "snapshot:/Trait_Antique";

		// Token: 0x04003E56 RID: 15958
		private const string TONE_DEAF_TRAIT_SNAPSHOT_PATH = "snapshot:/Trait_Tonedeaf";

		// Token: 0x04003E57 RID: 15959
		private Dictionary<AmbientSoundController.AmbientSoundControllerTableKey, EventInstance[]> m_audioInstanceTable = new Dictionary<AmbientSoundController.AmbientSoundControllerTableKey, EventInstance[]>();

		// Token: 0x04003E58 RID: 15960
		private Dictionary<AmbientSoundController.AmbientSoundControllerTableKey, EventInstance> m_snapshotInstanceTable = new Dictionary<AmbientSoundController.AmbientSoundControllerTableKey, EventInstance>();

		// Token: 0x04003E59 RID: 15961
		private EventInstance m_currentEvent;

		// Token: 0x04003E5A RID: 15962
		private EventInstance m_currentSnapshot;

		// Token: 0x04003E5B RID: 15963
		private EventInstance m_previousSnapshot;

		// Token: 0x04003E5C RID: 15964
		private bool m_inSkillTree;

		// Token: 0x04003E5D RID: 15965
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

		// Token: 0x04003E5E RID: 15966
		private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

		// Token: 0x04003E5F RID: 15967
		private Action<MonoBehaviour, EventArgs> m_onBiomeEnter;

		// Token: 0x04003E60 RID: 15968
		private Action<MonoBehaviour, EventArgs> m_onEnterMainMenu;

		// Token: 0x04003E61 RID: 15969
		private Action m_onTransitionStart;

		// Token: 0x04003E62 RID: 15970
		private UnityAction m_onSkillTreeWindowOpenedUnityEvent;

		// Token: 0x04003E63 RID: 15971
		private UnityAction m_onSkillTreeWindowClosedUnityEvent;

		// Token: 0x04003E64 RID: 15972
		private static AmbientSoundController m_instance;

		// Token: 0x04003E65 RID: 15973
		private static EventInstance m_antiquarianSnapshot;

		// Token: 0x04003E66 RID: 15974
		private static EventInstance m_toneDeafSnapshot;

		// Token: 0x04003E67 RID: 15975
		public bool StopOnTransition = true;

		// Token: 0x02000EDE RID: 3806
		private struct AmbientSoundControllerTableKey : IEquatable<AmbientSoundController.AmbientSoundControllerTableKey>
		{
			// Token: 0x06006EE8 RID: 28392 RVA: 0x0019C733 File Offset: 0x0019A933
			public AmbientSoundControllerTableKey(BiomeType biome, RoomType roomType, bool isLarge)
			{
				this.Biome = biome;
				this.RoomType = roomType;
				this.IsLarge = isLarge;
			}

			// Token: 0x17002407 RID: 9223
			// (get) Token: 0x06006EE9 RID: 28393 RVA: 0x0019C74A File Offset: 0x0019A94A
			public readonly BiomeType Biome { get; }

			// Token: 0x17002408 RID: 9224
			// (get) Token: 0x06006EEA RID: 28394 RVA: 0x0019C752 File Offset: 0x0019A952
			public readonly RoomType RoomType { get; }

			// Token: 0x17002409 RID: 9225
			// (get) Token: 0x06006EEB RID: 28395 RVA: 0x0019C75A File Offset: 0x0019A95A
			public readonly bool IsLarge { get; }

			// Token: 0x06006EEC RID: 28396 RVA: 0x0019C762 File Offset: 0x0019A962
			public static bool operator ==(AmbientSoundController.AmbientSoundControllerTableKey keyA, AmbientSoundController.AmbientSoundControllerTableKey keyB)
			{
				return keyA.Equals(keyB);
			}

			// Token: 0x06006EED RID: 28397 RVA: 0x0019C76C File Offset: 0x0019A96C
			public static bool operator !=(AmbientSoundController.AmbientSoundControllerTableKey keyA, AmbientSoundController.AmbientSoundControllerTableKey keyB)
			{
				return !keyA.Equals(keyB);
			}

			// Token: 0x06006EEE RID: 28398 RVA: 0x0019C779 File Offset: 0x0019A979
			public bool Equals(AmbientSoundController.AmbientSoundControllerTableKey other)
			{
				return this.Biome == other.Biome && this.RoomType == other.RoomType && this.IsLarge == other.IsLarge;
			}

			// Token: 0x06006EEF RID: 28399 RVA: 0x0019C7AA File Offset: 0x0019A9AA
			public override bool Equals(object obj)
			{
				return obj is AmbientSoundController.AmbientSoundControllerTableKey && this.Equals((AmbientSoundController.AmbientSoundControllerTableKey)obj);
			}

			// Token: 0x06006EF0 RID: 28400 RVA: 0x0019C7C4 File Offset: 0x0019A9C4
			public override int GetHashCode()
			{
				return ((-1554893279 * -1521134295 + this.Biome.GetHashCode()) * -1521134295 + this.RoomType.GetHashCode()) * -1521134295 + this.IsLarge.GetHashCode();
			}
		}
	}
}

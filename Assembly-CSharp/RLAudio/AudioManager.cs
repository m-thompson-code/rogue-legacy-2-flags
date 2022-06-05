using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008DD RID: 2269
	public class AudioManager : MonoBehaviour
	{
		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x06004A77 RID: 19063 RVA: 0x0010C0EF File Offset: 0x0010A2EF
		// (set) Token: 0x06004A78 RID: 19064 RVA: 0x0010C0F6 File Offset: 0x0010A2F6
		private static AudioManager Instance
		{
			get
			{
				return AudioManager.m_instance;
			}
			set
			{
				AudioManager.m_instance = value;
			}
		}

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x06004A79 RID: 19065 RVA: 0x0010C100 File Offset: 0x0010A300
		public static bool IsMusicPaused
		{
			get
			{
				if (AudioManager.IsMasterPaused)
				{
					return true;
				}
				bool result = false;
				if (AudioManager.m_musicBus.isValid())
				{
					AudioManager.m_musicBus.getPaused(out result);
				}
				return result;
			}
		}

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x06004A7A RID: 19066 RVA: 0x0010C134 File Offset: 0x0010A334
		public static bool IsSFXPaused
		{
			get
			{
				if (AudioManager.IsMasterPaused)
				{
					return true;
				}
				bool result = false;
				if (AudioManager.m_sfxGameplayBus.isValid())
				{
					AudioManager.m_sfxGameplayBus.getPaused(out result);
				}
				return result;
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x06004A7B RID: 19067 RVA: 0x0010C168 File Offset: 0x0010A368
		public static bool IsMasterPaused
		{
			get
			{
				bool result = false;
				if (AudioManager.m_masterBus.isValid())
				{
					AudioManager.m_masterBus.getPaused(out result);
				}
				return result;
			}
		}

		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x06004A7C RID: 19068 RVA: 0x0010C191 File Offset: 0x0010A391
		public static bool IsInitialized
		{
			get
			{
				return RuntimeManager.IsInitialized && RuntimeManager.HasBanksLoaded && AudioManager.IsMasterBusLoaded && AudioManager.IsMusicBusLoaded && AudioManager.IsSFXBusLoaded;
			}
		}

		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x06004A7D RID: 19069 RVA: 0x0010C1B6 File Offset: 0x0010A3B6
		public static bool IsMasterBusLoaded
		{
			get
			{
				return AudioManager.m_masterBus.isValid();
			}
		}

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x06004A7E RID: 19070 RVA: 0x0010C1C2 File Offset: 0x0010A3C2
		public static bool IsMusicBusLoaded
		{
			get
			{
				return AudioManager.m_musicBus.isValid();
			}
		}

		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x06004A7F RID: 19071 RVA: 0x0010C1CE File Offset: 0x0010A3CE
		public static bool IsSFXBusLoaded
		{
			get
			{
				return AudioManager.m_sfxBus.isValid() && AudioManager.m_sfxGameplayBus.isValid();
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x06004A80 RID: 19072 RVA: 0x0010C1E8 File Offset: 0x0010A3E8
		public static bool IsEnemySFXBusLoaded
		{
			get
			{
				return AudioManager.m_sfxBus.isValid() && AudioManager.m_sfxEnemyBus.isValid();
			}
		}

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x06004A81 RID: 19073 RVA: 0x0010C202 File Offset: 0x0010A402
		public static bool IsPlayerSFXBusLoaded
		{
			get
			{
				return AudioManager.m_sfxBus.isValid() && AudioManager.m_sfxPlayerBus.isValid();
			}
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x0010C21C File Offset: 0x0010A41C
		private void Awake()
		{
			if (AudioManager.Instance == null)
			{
				AudioManager.Instance = this;
				AudioManager.Initialize();
			}
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x0010C238 File Offset: 0x0010A438
		private static void CheckAudioBuses()
		{
			if (!AudioManager.m_masterBus.isValid())
			{
				AudioManager.m_masterBus = RuntimeManager.GetBus("Bus:/");
			}
			if (!AudioManager.m_musicBus.isValid())
			{
				AudioManager.m_musicBus = RuntimeManager.GetBus("bus:/Master/User_Music");
			}
			if (!AudioManager.m_sfxBus.isValid())
			{
				AudioManager.m_sfxBus = RuntimeManager.GetBus("bus:/Master/User_Sfx");
			}
			if (!AudioManager.m_sfxGameplayBus.isValid())
			{
				AudioManager.m_sfxGameplayBus = RuntimeManager.GetBus("bus:/Master/User_Sfx/Sfx_Master/Gameplay");
			}
			if (!AudioManager.m_sfxEnemyBus.isValid())
			{
				AudioManager.m_sfxEnemyBus = RuntimeManager.GetBus("bus:/Master/User_Sfx/Sfx_Master/Gameplay/Enemies");
			}
			if (!AudioManager.m_sfxPlayerBus.isValid())
			{
				AudioManager.m_sfxPlayerBus = RuntimeManager.GetBus("bus:/Master/User_Sfx/Sfx_Master/Gameplay/PlayerCharacter");
			}
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x0010C2E8 File Offset: 0x0010A4E8
		public static Guid GetEventID(string audioEvent)
		{
			if (!AudioManager.m_audioEventIDTable.ContainsKey(audioEvent))
			{
				Guid value;
				RuntimeManager.GetEventDescription(audioEvent).getID(out value);
				AudioManager.m_audioEventIDTable[audioEvent] = value;
			}
			return AudioManager.m_audioEventIDTable[audioEvent];
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x0010C32C File Offset: 0x0010A52C
		public static PARAMETER_ID GetParameterID(Guid audioEvent, string parameter)
		{
			AudioManager.EventParameterPair key = new AudioManager.EventParameterPair(audioEvent, parameter);
			if (!AudioManager.m_parameterIDTable.ContainsKey(key))
			{
				PARAMETER_DESCRIPTION parameter_DESCRIPTION;
				RuntimeManager.GetEventDescription(audioEvent).getParameterDescriptionByName(parameter, out parameter_DESCRIPTION);
				AudioManager.m_parameterIDTable[key] = parameter_DESCRIPTION.id;
			}
			return AudioManager.m_parameterIDTable[key];
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x0010C37D File Offset: 0x0010A57D
		private static void Initialize()
		{
			AudioManager.CheckAudioBuses();
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x0010C384 File Offset: 0x0010A584
		public static float GetMasterVolume()
		{
			return AudioManager.GetVolume(AudioManager.m_masterBus);
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x0010C390 File Offset: 0x0010A590
		public static float GetMusicVolume()
		{
			return AudioManager.GetVolume(AudioManager.m_musicBus);
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x0010C39C File Offset: 0x0010A59C
		public static float GetSFXVolume()
		{
			return AudioManager.GetVolume(AudioManager.m_sfxBus);
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0010C3A8 File Offset: 0x0010A5A8
		private static float GetVolume(Bus bus)
		{
			AudioManager.CheckAudioBuses();
			float result = 1f;
			if (bus.isValid())
			{
				bus.getVolume(out result);
			}
			else
			{
				UnityEngine.Debug.LogFormat("<color=red>| AudioManager | Failed to find Bus ({0}). If you see this message please add a bug to Pivotal along with Stack Trace</color>", new object[]
				{
					bus.ToString()
				});
			}
			return result;
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x0010C3F8 File Offset: 0x0010A5F8
		public static void Play(IAudioEventEmitter caller, EventInstance eventInstance)
		{
			if (AudioManager.Instance.m_logCallerDescriptionsToConsole)
			{
				EventDescription eventDescription;
				eventInstance.getDescription(out eventDescription);
				string eventPath;
				eventDescription.getPath(out eventPath);
				AudioManager.LogAudioEventToConsole(caller, eventPath);
			}
			eventInstance.start();
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x0010C434 File Offset: 0x0010A634
		public static void Play(IAudioEventEmitter caller, StudioEventEmitter studioEventEmitter)
		{
			if (AudioManager.Instance.m_logCallerDescriptionsToConsole)
			{
				AudioManager.LogAudioEventToConsole(caller, studioEventEmitter.Event);
			}
			studioEventEmitter.Play();
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x0010C454 File Offset: 0x0010A654
		private static void LogAudioEventToConsole(IAudioEventEmitter caller, string eventPath)
		{
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x0010C458 File Offset: 0x0010A658
		public static void PlayAttached(IAudioEventEmitter caller, EventInstance eventInstance, GameObject gameObject)
		{
			Rigidbody2D rigidBody2D = null;
			RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject.transform, rigidBody2D);
			AudioManager.Play(caller, eventInstance);
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x0010C47B File Offset: 0x0010A67B
		public static void PlayOneShotAttached(IAudioEventEmitter caller, string path, GameObject gameObject)
		{
			if (!string.IsNullOrEmpty(path))
			{
				if (AudioManager.Instance.m_logCallerDescriptionsToConsole)
				{
					AudioManager.LogAudioEventToConsole(caller, path);
				}
				RuntimeManager.PlayOneShotAttached(path, gameObject);
			}
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x0010C49F File Offset: 0x0010A69F
		public static void PlayOneShot(IAudioEventEmitter caller, string path, Vector3 position = default(Vector3))
		{
			if (!string.IsNullOrEmpty(path))
			{
				if (AudioManager.Instance.m_logCallerDescriptionsToConsole)
				{
					AudioManager.LogAudioEventToConsole(caller, path);
				}
				RuntimeManager.PlayOneShot(path, position);
			}
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x0010C4C3 File Offset: 0x0010A6C3
		public static void PlayDelayedOneShot(IAudioEventEmitter caller, string path, Vector2 position, float delayTime = -1f)
		{
			AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayDelayedOneShotCoroutine(caller, path, position, delayTime));
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x0010C4DE File Offset: 0x0010A6DE
		private IEnumerator PlayDelayedOneShotCoroutine(IAudioEventEmitter caller, string path, Vector2 position, float time)
		{
			float waitTime = (time >= 0f) ? time : UnityEngine.Random.Range(0f, this.m_maxRandomDelay);
			if (this.m_waitDelay == null)
			{
				this.m_waitDelay = new WaitRL_Yield(waitTime, false);
			}
			else
			{
				this.m_waitDelay.CreateNew(waitTime, false);
			}
			yield return this.m_waitDelay;
			AudioManager.PlayOneShot(caller, path, position);
			yield break;
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x0010C50A File Offset: 0x0010A70A
		public static void Play(IAudioEventEmitter caller, EventInstance eventInstance, Vector3 worldPosition)
		{
			eventInstance.set3DAttributes(worldPosition.To3DAttributes());
			AudioManager.Play(caller, eventInstance);
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x0010C521 File Offset: 0x0010A721
		public static void Play(IAudioEventEmitter caller, string eventPath, Vector3 worldPosition = default(Vector3))
		{
			RuntimeManager.PlayOneShot(eventPath, worldPosition);
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x0010C52A File Offset: 0x0010A72A
		public static void SetMasterVolume(float volume)
		{
			AudioManager.SetVolume(AudioManager.m_masterBus, volume);
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x0010C537 File Offset: 0x0010A737
		public static void SetMusicVolume(float volume)
		{
			AudioManager.SetVolume(AudioManager.m_musicBus, volume);
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0010C544 File Offset: 0x0010A744
		public static void SetSFXVolume(float volume)
		{
			AudioManager.SetVolume(AudioManager.m_sfxBus, volume);
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x0010C554 File Offset: 0x0010A754
		private static void SetVolume(Bus bus, float volume)
		{
			AudioManager.CheckAudioBuses();
			if (bus.isValid())
			{
				bus.setVolume(volume);
				return;
			}
			string text = "";
			bus.getPath(out text);
			UnityEngine.Debug.LogFormat("<color=red>| AudioManager | Failed to find Bus ({0}). If you see this message please add a bug to Pivotal along with Stack Trace</color>", new object[]
			{
				text
			});
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x0010C5A0 File Offset: 0x0010A7A0
		public static void SetMasterPaused(bool pause)
		{
			if (AudioManager.IsMasterBusLoaded)
			{
				RESULT result = AudioManager.m_masterBus.setPaused(pause);
				if (result != RESULT.OK)
				{
					UnityEngine.Debug.Log("Could not pause Master. FMOD.RESULT: " + result.ToString());
				}
			}
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x0010C5E0 File Offset: 0x0010A7E0
		public static void SetMusicPaused(bool pause)
		{
			if (AudioManager.IsMusicBusLoaded)
			{
				RESULT result = AudioManager.m_musicBus.setPaused(pause);
				if (result != RESULT.OK)
				{
					UnityEngine.Debug.Log("Could not pause Music. FMOD.RESULT: " + result.ToString());
				}
			}
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x0010C620 File Offset: 0x0010A820
		public static void SetSFXPaused(bool pause)
		{
			if (AudioManager.IsSFXBusLoaded)
			{
				RESULT result = AudioManager.m_sfxGameplayBus.setPaused(pause);
				if (result != RESULT.OK)
				{
					UnityEngine.Debug.Log("Could not pause SFX. FMOD.RESULT: " + result.ToString());
				}
			}
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0010C660 File Offset: 0x0010A860
		public static void SetEnemySFXPaused(bool pause)
		{
			if (AudioManager.IsEnemySFXBusLoaded)
			{
				RESULT result = AudioManager.m_sfxEnemyBus.setPaused(pause);
				if (result != RESULT.OK)
				{
					UnityEngine.Debug.Log("Could not pause SFX. FMOD.RESULT: " + result.ToString());
				}
			}
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x0010C6A0 File Offset: 0x0010A8A0
		public static void SetPlayerSFXPaused(bool pause)
		{
			if (AudioManager.IsPlayerSFXBusLoaded)
			{
				RESULT result = AudioManager.m_sfxPlayerBus.setPaused(pause);
				if (result != RESULT.OK)
				{
					UnityEngine.Debug.Log("Could not pause SFX. FMOD.RESULT: " + result.ToString());
				}
			}
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x0010C6DF File Offset: 0x0010A8DF
		public static void Stop(EventInstance eventInstance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
		{
			eventInstance.stop(stopMode);
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x0010C6EA File Offset: 0x0010A8EA
		public static void StopAllAudio(FMOD.Studio.STOP_MODE stopMode)
		{
			if (AudioManager.m_masterBus.isValid())
			{
				AudioManager.m_masterBus.stopAllEvents(stopMode);
			}
		}

		// Token: 0x04003E97 RID: 16023
		[SerializeField]
		private float m_maxRandomDelay = 0.25f;

		// Token: 0x04003E98 RID: 16024
		[SerializeField]
		private bool m_logCallerDescriptionsToConsole;

		// Token: 0x04003E99 RID: 16025
		[SerializeField]
		[Tooltip("Only display Audio Events whose paths contain the following string.")]
		private string m_eventPathFilter = string.Empty;

		// Token: 0x04003E9A RID: 16026
		private const string SFX_BUS_PATH = "bus:/Master/User_Sfx";

		// Token: 0x04003E9B RID: 16027
		private const string SFX_GAMEPLAY_BUS_PATH = "bus:/Master/User_Sfx/Sfx_Master/Gameplay";

		// Token: 0x04003E9C RID: 16028
		private const string SFX_ENEMY_BUS_PATH = "bus:/Master/User_Sfx/Sfx_Master/Gameplay/Enemies";

		// Token: 0x04003E9D RID: 16029
		private const string SFX_PLAYER_BUS_PATH = "bus:/Master/User_Sfx/Sfx_Master/Gameplay/PlayerCharacter";

		// Token: 0x04003E9E RID: 16030
		private const string MUSIC_BUS_PATH = "bus:/Master/User_Music";

		// Token: 0x04003E9F RID: 16031
		private const string MASTER_BUS_PATH = "Bus:/";

		// Token: 0x04003EA0 RID: 16032
		private static Dictionary<string, Guid> m_audioEventIDTable = new Dictionary<string, Guid>();

		// Token: 0x04003EA1 RID: 16033
		private static Dictionary<AudioManager.EventParameterPair, PARAMETER_ID> m_parameterIDTable = new Dictionary<AudioManager.EventParameterPair, PARAMETER_ID>();

		// Token: 0x04003EA2 RID: 16034
		private static Bus m_masterBus;

		// Token: 0x04003EA3 RID: 16035
		private static Bus m_musicBus;

		// Token: 0x04003EA4 RID: 16036
		private static Bus m_sfxBus;

		// Token: 0x04003EA5 RID: 16037
		private static Bus m_sfxGameplayBus;

		// Token: 0x04003EA6 RID: 16038
		private static Bus m_sfxEnemyBus;

		// Token: 0x04003EA7 RID: 16039
		private static Bus m_sfxPlayerBus;

		// Token: 0x04003EA8 RID: 16040
		private static AudioManager m_instance;

		// Token: 0x04003EA9 RID: 16041
		private WaitRL_Yield m_waitDelay;

		// Token: 0x02000EDF RID: 3807
		private struct EventParameterPair
		{
			// Token: 0x06006EF1 RID: 28401 RVA: 0x0019C821 File Offset: 0x0019AA21
			public EventParameterPair(Guid audioEvent, string parameter)
			{
				this.AudioEvent = audioEvent;
				this.Parameter = parameter;
			}

			// Token: 0x1700240A RID: 9226
			// (get) Token: 0x06006EF2 RID: 28402 RVA: 0x0019C831 File Offset: 0x0019AA31
			public readonly Guid AudioEvent { get; }

			// Token: 0x1700240B RID: 9227
			// (get) Token: 0x06006EF3 RID: 28403 RVA: 0x0019C839 File Offset: 0x0019AA39
			public readonly string Parameter { get; }
		}
	}
}

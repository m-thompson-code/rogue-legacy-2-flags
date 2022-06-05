using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E4B RID: 3659
	public class AudioManager : MonoBehaviour
	{
		// Token: 0x17002117 RID: 8471
		// (get) Token: 0x06006727 RID: 26407 RVA: 0x00038CE0 File Offset: 0x00036EE0
		// (set) Token: 0x06006728 RID: 26408 RVA: 0x00038CE7 File Offset: 0x00036EE7
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

		// Token: 0x17002118 RID: 8472
		// (get) Token: 0x06006729 RID: 26409 RVA: 0x0017C8FC File Offset: 0x0017AAFC
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

		// Token: 0x17002119 RID: 8473
		// (get) Token: 0x0600672A RID: 26410 RVA: 0x0017C930 File Offset: 0x0017AB30
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

		// Token: 0x1700211A RID: 8474
		// (get) Token: 0x0600672B RID: 26411 RVA: 0x0017C964 File Offset: 0x0017AB64
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

		// Token: 0x1700211B RID: 8475
		// (get) Token: 0x0600672C RID: 26412 RVA: 0x00038CEF File Offset: 0x00036EEF
		public static bool IsInitialized
		{
			get
			{
				return RuntimeManager.IsInitialized && RuntimeManager.HasBanksLoaded && AudioManager.IsMasterBusLoaded && AudioManager.IsMusicBusLoaded && AudioManager.IsSFXBusLoaded;
			}
		}

		// Token: 0x1700211C RID: 8476
		// (get) Token: 0x0600672D RID: 26413 RVA: 0x00038D14 File Offset: 0x00036F14
		public static bool IsMasterBusLoaded
		{
			get
			{
				return AudioManager.m_masterBus.isValid();
			}
		}

		// Token: 0x1700211D RID: 8477
		// (get) Token: 0x0600672E RID: 26414 RVA: 0x00038D20 File Offset: 0x00036F20
		public static bool IsMusicBusLoaded
		{
			get
			{
				return AudioManager.m_musicBus.isValid();
			}
		}

		// Token: 0x1700211E RID: 8478
		// (get) Token: 0x0600672F RID: 26415 RVA: 0x00038D2C File Offset: 0x00036F2C
		public static bool IsSFXBusLoaded
		{
			get
			{
				return AudioManager.m_sfxBus.isValid() && AudioManager.m_sfxGameplayBus.isValid();
			}
		}

		// Token: 0x1700211F RID: 8479
		// (get) Token: 0x06006730 RID: 26416 RVA: 0x00038D46 File Offset: 0x00036F46
		public static bool IsEnemySFXBusLoaded
		{
			get
			{
				return AudioManager.m_sfxBus.isValid() && AudioManager.m_sfxEnemyBus.isValid();
			}
		}

		// Token: 0x17002120 RID: 8480
		// (get) Token: 0x06006731 RID: 26417 RVA: 0x00038D60 File Offset: 0x00036F60
		public static bool IsPlayerSFXBusLoaded
		{
			get
			{
				return AudioManager.m_sfxBus.isValid() && AudioManager.m_sfxPlayerBus.isValid();
			}
		}

		// Token: 0x06006732 RID: 26418 RVA: 0x00038D7A File Offset: 0x00036F7A
		private void Awake()
		{
			if (AudioManager.Instance == null)
			{
				AudioManager.Instance = this;
				AudioManager.Initialize();
			}
		}

		// Token: 0x06006733 RID: 26419 RVA: 0x0017C990 File Offset: 0x0017AB90
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

		// Token: 0x06006734 RID: 26420 RVA: 0x0017CA40 File Offset: 0x0017AC40
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

		// Token: 0x06006735 RID: 26421 RVA: 0x0017CA84 File Offset: 0x0017AC84
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

		// Token: 0x06006736 RID: 26422 RVA: 0x00038D94 File Offset: 0x00036F94
		private static void Initialize()
		{
			AudioManager.CheckAudioBuses();
		}

		// Token: 0x06006737 RID: 26423 RVA: 0x00038D9B File Offset: 0x00036F9B
		public static float GetMasterVolume()
		{
			return AudioManager.GetVolume(AudioManager.m_masterBus);
		}

		// Token: 0x06006738 RID: 26424 RVA: 0x00038DA7 File Offset: 0x00036FA7
		public static float GetMusicVolume()
		{
			return AudioManager.GetVolume(AudioManager.m_musicBus);
		}

		// Token: 0x06006739 RID: 26425 RVA: 0x00038DB3 File Offset: 0x00036FB3
		public static float GetSFXVolume()
		{
			return AudioManager.GetVolume(AudioManager.m_sfxBus);
		}

		// Token: 0x0600673A RID: 26426 RVA: 0x0017CAD8 File Offset: 0x0017ACD8
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

		// Token: 0x0600673B RID: 26427 RVA: 0x0017CB28 File Offset: 0x0017AD28
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

		// Token: 0x0600673C RID: 26428 RVA: 0x00038DBF File Offset: 0x00036FBF
		public static void Play(IAudioEventEmitter caller, StudioEventEmitter studioEventEmitter)
		{
			if (AudioManager.Instance.m_logCallerDescriptionsToConsole)
			{
				AudioManager.LogAudioEventToConsole(caller, studioEventEmitter.Event);
			}
			studioEventEmitter.Play();
		}

		// Token: 0x0600673D RID: 26429 RVA: 0x00002FCA File Offset: 0x000011CA
		private static void LogAudioEventToConsole(IAudioEventEmitter caller, string eventPath)
		{
		}

		// Token: 0x0600673E RID: 26430 RVA: 0x0017CB64 File Offset: 0x0017AD64
		public static void PlayAttached(IAudioEventEmitter caller, EventInstance eventInstance, GameObject gameObject)
		{
			Rigidbody2D rigidBody2D = null;
			RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject.transform, rigidBody2D);
			AudioManager.Play(caller, eventInstance);
		}

		// Token: 0x0600673F RID: 26431 RVA: 0x00038DDF File Offset: 0x00036FDF
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

		// Token: 0x06006740 RID: 26432 RVA: 0x00038E03 File Offset: 0x00037003
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

		// Token: 0x06006741 RID: 26433 RVA: 0x00038E27 File Offset: 0x00037027
		public static void PlayDelayedOneShot(IAudioEventEmitter caller, string path, Vector2 position, float delayTime = -1f)
		{
			AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayDelayedOneShotCoroutine(caller, path, position, delayTime));
		}

		// Token: 0x06006742 RID: 26434 RVA: 0x00038E42 File Offset: 0x00037042
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

		// Token: 0x06006743 RID: 26435 RVA: 0x00038E6E File Offset: 0x0003706E
		public static void Play(IAudioEventEmitter caller, EventInstance eventInstance, Vector3 worldPosition)
		{
			eventInstance.set3DAttributes(worldPosition.To3DAttributes());
			AudioManager.Play(caller, eventInstance);
		}

		// Token: 0x06006744 RID: 26436 RVA: 0x00038E85 File Offset: 0x00037085
		public static void Play(IAudioEventEmitter caller, string eventPath, Vector3 worldPosition = default(Vector3))
		{
			RuntimeManager.PlayOneShot(eventPath, worldPosition);
		}

		// Token: 0x06006745 RID: 26437 RVA: 0x00038E8E File Offset: 0x0003708E
		public static void SetMasterVolume(float volume)
		{
			AudioManager.SetVolume(AudioManager.m_masterBus, volume);
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x00038E9B File Offset: 0x0003709B
		public static void SetMusicVolume(float volume)
		{
			AudioManager.SetVolume(AudioManager.m_musicBus, volume);
		}

		// Token: 0x06006747 RID: 26439 RVA: 0x00038EA8 File Offset: 0x000370A8
		public static void SetSFXVolume(float volume)
		{
			AudioManager.SetVolume(AudioManager.m_sfxBus, volume);
		}

		// Token: 0x06006748 RID: 26440 RVA: 0x0017CB88 File Offset: 0x0017AD88
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

		// Token: 0x06006749 RID: 26441 RVA: 0x0017CBD4 File Offset: 0x0017ADD4
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

		// Token: 0x0600674A RID: 26442 RVA: 0x0017CC14 File Offset: 0x0017AE14
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

		// Token: 0x0600674B RID: 26443 RVA: 0x0017CC54 File Offset: 0x0017AE54
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

		// Token: 0x0600674C RID: 26444 RVA: 0x0017CC94 File Offset: 0x0017AE94
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

		// Token: 0x0600674D RID: 26445 RVA: 0x0017CCD4 File Offset: 0x0017AED4
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

		// Token: 0x0600674E RID: 26446 RVA: 0x00038EB5 File Offset: 0x000370B5
		public static void Stop(EventInstance eventInstance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
		{
			eventInstance.stop(stopMode);
		}

		// Token: 0x0600674F RID: 26447 RVA: 0x00038EC0 File Offset: 0x000370C0
		public static void StopAllAudio(FMOD.Studio.STOP_MODE stopMode)
		{
			if (AudioManager.m_masterBus.isValid())
			{
				AudioManager.m_masterBus.stopAllEvents(stopMode);
			}
		}

		// Token: 0x040053AC RID: 21420
		[SerializeField]
		private float m_maxRandomDelay = 0.25f;

		// Token: 0x040053AD RID: 21421
		[SerializeField]
		private bool m_logCallerDescriptionsToConsole;

		// Token: 0x040053AE RID: 21422
		[SerializeField]
		[Tooltip("Only display Audio Events whose paths contain the following string.")]
		private string m_eventPathFilter = string.Empty;

		// Token: 0x040053AF RID: 21423
		private const string SFX_BUS_PATH = "bus:/Master/User_Sfx";

		// Token: 0x040053B0 RID: 21424
		private const string SFX_GAMEPLAY_BUS_PATH = "bus:/Master/User_Sfx/Sfx_Master/Gameplay";

		// Token: 0x040053B1 RID: 21425
		private const string SFX_ENEMY_BUS_PATH = "bus:/Master/User_Sfx/Sfx_Master/Gameplay/Enemies";

		// Token: 0x040053B2 RID: 21426
		private const string SFX_PLAYER_BUS_PATH = "bus:/Master/User_Sfx/Sfx_Master/Gameplay/PlayerCharacter";

		// Token: 0x040053B3 RID: 21427
		private const string MUSIC_BUS_PATH = "bus:/Master/User_Music";

		// Token: 0x040053B4 RID: 21428
		private const string MASTER_BUS_PATH = "Bus:/";

		// Token: 0x040053B5 RID: 21429
		private static Dictionary<string, Guid> m_audioEventIDTable = new Dictionary<string, Guid>();

		// Token: 0x040053B6 RID: 21430
		private static Dictionary<AudioManager.EventParameterPair, PARAMETER_ID> m_parameterIDTable = new Dictionary<AudioManager.EventParameterPair, PARAMETER_ID>();

		// Token: 0x040053B7 RID: 21431
		private static Bus m_masterBus;

		// Token: 0x040053B8 RID: 21432
		private static Bus m_musicBus;

		// Token: 0x040053B9 RID: 21433
		private static Bus m_sfxBus;

		// Token: 0x040053BA RID: 21434
		private static Bus m_sfxGameplayBus;

		// Token: 0x040053BB RID: 21435
		private static Bus m_sfxEnemyBus;

		// Token: 0x040053BC RID: 21436
		private static Bus m_sfxPlayerBus;

		// Token: 0x040053BD RID: 21437
		private static AudioManager m_instance;

		// Token: 0x040053BE RID: 21438
		private WaitRL_Yield m_waitDelay;

		// Token: 0x02000E4C RID: 3660
		private struct EventParameterPair
		{
			// Token: 0x06006752 RID: 26450 RVA: 0x00038F0E File Offset: 0x0003710E
			public EventParameterPair(Guid audioEvent, string parameter)
			{
				this.AudioEvent = audioEvent;
				this.Parameter = parameter;
			}

			// Token: 0x17002121 RID: 8481
			// (get) Token: 0x06006753 RID: 26451 RVA: 0x00038F1E File Offset: 0x0003711E
			public readonly Guid AudioEvent { get; }

			// Token: 0x17002122 RID: 8482
			// (get) Token: 0x06006754 RID: 26452 RVA: 0x00038F26 File Offset: 0x00037126
			public readonly string Parameter { get; }
		}
	}
}

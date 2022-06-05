using System;
using System.Collections.Generic;
using FMOD.Studio;
using SceneManagement_RL;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E9F RID: 3743
	public class WaterAudioSourceManager : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700218E RID: 8590
		// (get) Token: 0x06006979 RID: 27001 RVA: 0x0003A83E File Offset: 0x00038A3E
		public string Description
		{
			get
			{
				if (this.m_description == string.Empty)
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x1700218F RID: 8591
		// (get) Token: 0x0600697A RID: 27002 RVA: 0x0003A864 File Offset: 0x00038A64
		// (set) Token: 0x0600697B RID: 27003 RVA: 0x0003A86B File Offset: 0x00038A6B
		public static bool IsPlaying
		{
			get
			{
				return WaterAudioSourceManager.m_isPlaying;
			}
			private set
			{
				WaterAudioSourceManager.m_isPlaying = value;
			}
		}

		// Token: 0x17002190 RID: 8592
		// (get) Token: 0x0600697C RID: 27004 RVA: 0x0003A873 File Offset: 0x00038A73
		// (set) Token: 0x0600697D RID: 27005 RVA: 0x0003A87A File Offset: 0x00038A7A
		public static List<WaterAudioSource> WaterSources
		{
			get
			{
				return WaterAudioSourceManager.m_waterSources;
			}
			private set
			{
				WaterAudioSourceManager.m_waterSources = value;
			}
		}

		// Token: 0x17002191 RID: 8593
		// (get) Token: 0x0600697E RID: 27006 RVA: 0x0003A882 File Offset: 0x00038A82
		// (set) Token: 0x0600697F RID: 27007 RVA: 0x0003A889 File Offset: 0x00038A89
		public static EventInstance CurrentWaterEventInstance
		{
			get
			{
				return WaterAudioSourceManager.m_currentWaterEventInstance;
			}
			private set
			{
				WaterAudioSourceManager.m_currentWaterEventInstance = value;
			}
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x0018261C File Offset: 0x0018081C
		private void Awake()
		{
			if (WaterAudioSourceManager.m_audioEventEmitterGameObject == null)
			{
				WaterAudioSourceManager.m_audioEventEmitterGameObject = new GameObject("Water Audio Source");
				WaterAudioSourceManager.m_audioEventEmitterGameObject.transform.SetParent(base.transform);
				WaterAudioSourceManager.m_defaultWaterAudioEventInstance = AudioUtility.GetEventInstance("event:/Environment/Spots/amb_spot_waves_light", WaterAudioSourceManager.m_audioEventEmitterGameObject.transform);
				WaterAudioSourceManager.m_choppyWaterAudioEventInstance = AudioUtility.GetEventInstance("event:/Environment/Spots/amb_spot_waves_heavy", WaterAudioSourceManager.m_audioEventEmitterGameObject.transform);
			}
			SceneLoader_RL.TransitionStartRelay.AddListener(new Action(this.OnTransitionStart), false);
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x0003A891 File Offset: 0x00038A91
		private void OnDestroy()
		{
			if (WaterAudioSourceManager.m_defaultWaterAudioEventInstance.isValid())
			{
				WaterAudioSourceManager.m_defaultWaterAudioEventInstance.release();
			}
			if (WaterAudioSourceManager.m_choppyWaterAudioEventInstance.isValid())
			{
				WaterAudioSourceManager.m_choppyWaterAudioEventInstance.release();
			}
		}

		// Token: 0x06006982 RID: 27010 RVA: 0x0003A8C1 File Offset: 0x00038AC1
		private void OnTransitionStart()
		{
			if (WaterAudioSourceManager.IsPlaying)
			{
				WaterAudioSourceManager.WaterSources.Clear();
				this.Stop();
			}
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x0003A8DA File Offset: 0x00038ADA
		public static void AddWaterSource(WaterAudioSource source)
		{
			if ((WaterAudioSourceManager.WaterSources.Count <= 0 || source.WaterLevel == WaterAudioSourceManager.WaterSources[0].WaterLevel) && !WaterAudioSourceManager.WaterSources.Contains(source))
			{
				WaterAudioSourceManager.WaterSources.Add(source);
			}
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x0003A919 File Offset: 0x00038B19
		public static void RemoveWaterSource(WaterAudioSource source)
		{
			if (WaterAudioSourceManager.WaterSources.Contains(source))
			{
				WaterAudioSourceManager.WaterSources.Remove(source);
			}
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x001826A4 File Offset: 0x001808A4
		private void Update()
		{
			if (WaterAudioSourceManager.m_audioEventEmitterGameObject == null || !WaterAudioSourceManager.m_audioEventEmitterGameObject.activeInHierarchy || !CameraController.IsInstantiated)
			{
				return;
			}
			if (WaterAudioSourceManager.IsPlaying)
			{
				this.Stop();
			}
			else if (!WaterAudioSourceManager.IsPlaying)
			{
				this.Play();
			}
			if (WaterAudioSourceManager.IsPlaying)
			{
				WaterAudioSourceManager.UpdateAudioSourcePosition();
			}
		}

		// Token: 0x06006986 RID: 27014 RVA: 0x001826FC File Offset: 0x001808FC
		private static void UpdateAudioSourcePosition()
		{
			Vector2 vector = CameraController.GameCamera.transform.position;
			float d = WaterAudioSourceManager.WaterSources[0].transform.position.y - vector.y;
			WaterAudioSourceManager.m_audioEventEmitterGameObject.transform.position = vector + d * Vector2.up;
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x00182768 File Offset: 0x00180968
		private void Play()
		{
			if (WaterAudioSourceManager.WaterSources.Count > 0)
			{
				WaterAudioSourceManager.CurrentWaterEventInstance = this.GetWaterInstance();
				AudioManager.PlayAttached(this, WaterAudioSourceManager.CurrentWaterEventInstance, WaterAudioSourceManager.m_audioEventEmitterGameObject);
				float weatherParameterValue = WaterAudioSourceManager.GetWeatherParameterValue();
				WaterAudioSourceManager.CurrentWaterEventInstance.setParameterByName("Weather", weatherParameterValue, false);
				WaterAudioSourceManager.IsPlaying = true;
			}
		}

		// Token: 0x06006988 RID: 27016 RVA: 0x0003A934 File Offset: 0x00038B34
		private EventInstance GetWaterInstance()
		{
			if (WaterAudioSourceManager.WaterSources[0].IsChoppy)
			{
				return WaterAudioSourceManager.m_choppyWaterAudioEventInstance;
			}
			return WaterAudioSourceManager.m_defaultWaterAudioEventInstance;
		}

		// Token: 0x06006989 RID: 27017 RVA: 0x001827C0 File Offset: 0x001809C0
		private static float GetWeatherParameterValue()
		{
			bool flag = false;
			BiomeType appearanceBiomeType = PlayerManager.GetCurrentPlayerRoom().AppearanceBiomeType;
			if (BiomeCreation_EV.IS_IT_RAINING_IN_BIOME_TABLE.ContainsKey(appearanceBiomeType))
			{
				flag = BiomeCreation_EV.IS_IT_RAINING_IN_BIOME_TABLE[appearanceBiomeType];
			}
			float result = 0f;
			if (flag)
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x0600698A RID: 27018 RVA: 0x00182804 File Offset: 0x00180A04
		private void Stop()
		{
			if (WaterAudioSourceManager.WaterSources.Count == 0)
			{
				if (WaterAudioSourceManager.CurrentWaterEventInstance.isValid())
				{
					AudioManager.Stop(WaterAudioSourceManager.CurrentWaterEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					WaterAudioSourceManager.CurrentWaterEventInstance = default(EventInstance);
				}
				WaterAudioSourceManager.IsPlaying = false;
			}
		}

		// Token: 0x040055D5 RID: 21973
		private static GameObject m_audioEventEmitterGameObject = null;

		// Token: 0x040055D6 RID: 21974
		private static EventInstance m_defaultWaterAudioEventInstance = default(EventInstance);

		// Token: 0x040055D7 RID: 21975
		private static EventInstance m_choppyWaterAudioEventInstance = default(EventInstance);

		// Token: 0x040055D8 RID: 21976
		private static EventInstance m_currentWaterEventInstance;

		// Token: 0x040055D9 RID: 21977
		private string m_description = string.Empty;

		// Token: 0x040055DA RID: 21978
		private static bool m_isPlaying = false;

		// Token: 0x040055DB RID: 21979
		private static List<WaterAudioSource> m_waterSources = new List<WaterAudioSource>();

		// Token: 0x040055DC RID: 21980
		private static bool m_hasWarningBeenDisplayed = false;

		// Token: 0x040055DD RID: 21981
		private const string DEFAULT_WATER_PATH = "event:/Environment/Spots/amb_spot_waves_light";

		// Token: 0x040055DE RID: 21982
		private const string CHOPPY_WATER_PATH = "event:/Environment/Spots/amb_spot_waves_heavy";

		// Token: 0x040055DF RID: 21983
		private const float IS_RAINING_PARAMETER_VALUE = 1f;

		// Token: 0x040055E0 RID: 21984
		private const float IS_NOT_RAINING_PARAMETER_VALUE = 0f;
	}
}

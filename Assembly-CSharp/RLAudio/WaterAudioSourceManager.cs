using System;
using System.Collections.Generic;
using FMOD.Studio;
using SceneManagement_RL;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000920 RID: 2336
	public class WaterAudioSourceManager : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x06004C72 RID: 19570 RVA: 0x0011292A File Offset: 0x00110B2A
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

		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x06004C73 RID: 19571 RVA: 0x00112950 File Offset: 0x00110B50
		// (set) Token: 0x06004C74 RID: 19572 RVA: 0x00112957 File Offset: 0x00110B57
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

		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x06004C75 RID: 19573 RVA: 0x0011295F File Offset: 0x00110B5F
		// (set) Token: 0x06004C76 RID: 19574 RVA: 0x00112966 File Offset: 0x00110B66
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

		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x06004C77 RID: 19575 RVA: 0x0011296E File Offset: 0x00110B6E
		// (set) Token: 0x06004C78 RID: 19576 RVA: 0x00112975 File Offset: 0x00110B75
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

		// Token: 0x06004C79 RID: 19577 RVA: 0x00112980 File Offset: 0x00110B80
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

		// Token: 0x06004C7A RID: 19578 RVA: 0x00112A08 File Offset: 0x00110C08
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

		// Token: 0x06004C7B RID: 19579 RVA: 0x00112A38 File Offset: 0x00110C38
		private void OnTransitionStart()
		{
			if (WaterAudioSourceManager.IsPlaying)
			{
				WaterAudioSourceManager.WaterSources.Clear();
				this.Stop();
			}
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x00112A51 File Offset: 0x00110C51
		public static void AddWaterSource(WaterAudioSource source)
		{
			if ((WaterAudioSourceManager.WaterSources.Count <= 0 || source.WaterLevel == WaterAudioSourceManager.WaterSources[0].WaterLevel) && !WaterAudioSourceManager.WaterSources.Contains(source))
			{
				WaterAudioSourceManager.WaterSources.Add(source);
			}
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x00112A90 File Offset: 0x00110C90
		public static void RemoveWaterSource(WaterAudioSource source)
		{
			if (WaterAudioSourceManager.WaterSources.Contains(source))
			{
				WaterAudioSourceManager.WaterSources.Remove(source);
			}
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x00112AAC File Offset: 0x00110CAC
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

		// Token: 0x06004C7F RID: 19583 RVA: 0x00112B04 File Offset: 0x00110D04
		private static void UpdateAudioSourcePosition()
		{
			Vector2 vector = CameraController.GameCamera.transform.position;
			float d = WaterAudioSourceManager.WaterSources[0].transform.position.y - vector.y;
			WaterAudioSourceManager.m_audioEventEmitterGameObject.transform.position = vector + d * Vector2.up;
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x00112B70 File Offset: 0x00110D70
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

		// Token: 0x06004C81 RID: 19585 RVA: 0x00112BC6 File Offset: 0x00110DC6
		private EventInstance GetWaterInstance()
		{
			if (WaterAudioSourceManager.WaterSources[0].IsChoppy)
			{
				return WaterAudioSourceManager.m_choppyWaterAudioEventInstance;
			}
			return WaterAudioSourceManager.m_defaultWaterAudioEventInstance;
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x00112BE8 File Offset: 0x00110DE8
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

		// Token: 0x06004C83 RID: 19587 RVA: 0x00112C2C File Offset: 0x00110E2C
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

		// Token: 0x04004069 RID: 16489
		private static GameObject m_audioEventEmitterGameObject = null;

		// Token: 0x0400406A RID: 16490
		private static EventInstance m_defaultWaterAudioEventInstance = default(EventInstance);

		// Token: 0x0400406B RID: 16491
		private static EventInstance m_choppyWaterAudioEventInstance = default(EventInstance);

		// Token: 0x0400406C RID: 16492
		private static EventInstance m_currentWaterEventInstance;

		// Token: 0x0400406D RID: 16493
		private string m_description = string.Empty;

		// Token: 0x0400406E RID: 16494
		private static bool m_isPlaying = false;

		// Token: 0x0400406F RID: 16495
		private static List<WaterAudioSource> m_waterSources = new List<WaterAudioSource>();

		// Token: 0x04004070 RID: 16496
		private static bool m_hasWarningBeenDisplayed = false;

		// Token: 0x04004071 RID: 16497
		private const string DEFAULT_WATER_PATH = "event:/Environment/Spots/amb_spot_waves_light";

		// Token: 0x04004072 RID: 16498
		private const string CHOPPY_WATER_PATH = "event:/Environment/Spots/amb_spot_waves_heavy";

		// Token: 0x04004073 RID: 16499
		private const float IS_RAINING_PARAMETER_VALUE = 1f;

		// Token: 0x04004074 RID: 16500
		private const float IS_NOT_RAINING_PARAMETER_VALUE = 0f;
	}
}

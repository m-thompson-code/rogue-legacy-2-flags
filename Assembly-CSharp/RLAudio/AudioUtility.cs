using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008DE RID: 2270
	public static class AudioUtility
	{
		// Token: 0x06004AA2 RID: 19106 RVA: 0x0010C738 File Offset: 0x0010A938
		public static EventInstance GetEventInstance(string eventPath, Transform transform)
		{
			if (!string.IsNullOrEmpty(eventPath))
			{
				EventDescription eventDescription;
				try
				{
					eventDescription = RuntimeManager.GetEventDescription(eventPath);
				}
				catch (Exception)
				{
					throw;
				}
				EventInstance result = RuntimeManager.CreateInstance(eventPath);
				bool flag;
				eventDescription.is3D(out flag);
				if (flag)
				{
					if (transform != null)
					{
						result.set3DAttributes(transform.To3DAttributes());
					}
					else
					{
						UnityEngine.Debug.LogFormat("<color=red>| AudioUtility | The FMOD Event at <b>({0})</b> is 3D but you didn't pass in a reference to the GameObject's Transform. If you see this message, please add a bug report to Pivotal.</color>", new object[]
						{
							eventPath
						});
					}
				}
				return result;
			}
			return default(EventInstance);
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x0010C7B8 File Offset: 0x0010A9B8
		public static float GetPlayerSizeAudioParameterValue()
		{
			float result = 0.5f;
			if (PlayerManager.IsInstantiated)
			{
				if (PlayerManager.GetPlayerController().transform.localScale.x < 1.4f)
				{
					result = 1f;
				}
				else if (PlayerManager.GetPlayerController().transform.localScale.x > 1.4f)
				{
					result = 0f;
				}
			}
			return result;
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x0010C818 File Offset: 0x0010AA18
		public static float GetPlayerGenderAudioParameterValue()
		{
			float result = 1f;
			if (SaveManager.IsInitialized)
			{
				bool flag = SaveManager.PlayerSaveData.CurrentCharacter.IsFemale;
				if (SaveManager.PlayerSaveData.CurrentCharacter.Disposition_ID == 1)
				{
					flag = !flag;
				}
				if (flag)
				{
					result = 0f;
				}
			}
			return result;
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x0010C864 File Offset: 0x0010AA64
		public static bool GetHasParameter(EventInstance eventInstance, string parameterName)
		{
			float num;
			return eventInstance.isValid() && parameterName != string.Empty && eventInstance.getParameterByName(parameterName, out num) == RESULT.OK;
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x0010C898 File Offset: 0x0010AA98
		public static PLAYBACK_STATE GetCurrentPlaybackState(EventInstance eventInstance)
		{
			PLAYBACK_STATE result;
			eventInstance.getPlaybackState(out result);
			return result;
		}

		// Token: 0x04003EAA RID: 16042
		public const string PLAYER_SIZE_PARAMETER = "Player_Size";

		// Token: 0x04003EAB RID: 16043
		public const string GENDER_PARAMETER = "gender";
	}
}

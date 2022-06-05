using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E4E RID: 3662
	public static class AudioUtility
	{
		// Token: 0x0600675B RID: 26459 RVA: 0x0017CDC4 File Offset: 0x0017AFC4
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

		// Token: 0x0600675C RID: 26460 RVA: 0x0017CE44 File Offset: 0x0017B044
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

		// Token: 0x0600675D RID: 26461 RVA: 0x0017CEA4 File Offset: 0x0017B0A4
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

		// Token: 0x0600675E RID: 26462 RVA: 0x0017CEF0 File Offset: 0x0017B0F0
		public static bool GetHasParameter(EventInstance eventInstance, string parameterName)
		{
			float num;
			return eventInstance.isValid() && parameterName != string.Empty && eventInstance.getParameterByName(parameterName, out num) == RESULT.OK;
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x0017CF24 File Offset: 0x0017B124
		public static PLAYBACK_STATE GetCurrentPlaybackState(EventInstance eventInstance)
		{
			PLAYBACK_STATE result;
			eventInstance.getPlaybackState(out result);
			return result;
		}

		// Token: 0x040053C8 RID: 21448
		public const string PLAYER_SIZE_PARAMETER = "Player_Size";

		// Token: 0x040053C9 RID: 21449
		public const string GENDER_PARAMETER = "gender";
	}
}

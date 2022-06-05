using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

namespace RLAudio
{
	// Token: 0x02000913 RID: 2323
	public class ShoutAbilityAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001884 RID: 6276
		// (get) Token: 0x06004C30 RID: 19504 RVA: 0x00111A8F File Offset: 0x0010FC8F
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

		// Token: 0x06004C31 RID: 19505 RVA: 0x00111AB5 File Offset: 0x0010FCB5
		private void Awake()
		{
			base.GetComponent<Shout_Ability>().OnShoutEvent.AddListener(new UnityAction<GameObject>(this.OnShout));
			this.m_shoutEventInstance = AudioUtility.GetEventInstance(this.m_shoutAudioPath, base.transform);
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x00111AEA File Offset: 0x0010FCEA
		private void OnDestroy()
		{
			if (this.m_shoutEventInstance.isValid())
			{
				this.m_shoutEventInstance.release();
			}
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x00111B08 File Offset: 0x0010FD08
		private void OnShout(GameObject playerGameObject)
		{
			float playerGenderAudioParameterValue = AudioUtility.GetPlayerGenderAudioParameterValue();
			float playerSizeAudioParameterValue = AudioUtility.GetPlayerSizeAudioParameterValue();
			this.m_shoutEventInstance.setParameterByName("gender", playerGenderAudioParameterValue, false);
			this.m_shoutEventInstance.setParameterByName("Player_Size", playerSizeAudioParameterValue, false);
			AudioManager.PlayAttached(this, this.m_shoutEventInstance, base.gameObject);
		}

		// Token: 0x04004024 RID: 16420
		[SerializeField]
		[EventRef]
		private string m_shoutAudioPath;

		// Token: 0x04004025 RID: 16421
		private string m_description = string.Empty;

		// Token: 0x04004026 RID: 16422
		private EventInstance m_shoutEventInstance;
	}
}

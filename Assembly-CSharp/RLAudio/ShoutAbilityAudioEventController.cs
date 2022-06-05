using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

namespace RLAudio
{
	// Token: 0x02000E90 RID: 3728
	public class ShoutAbilityAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700217D RID: 8573
		// (get) Token: 0x0600692B RID: 26923 RVA: 0x0003A54E File Offset: 0x0003874E
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

		// Token: 0x0600692C RID: 26924 RVA: 0x0003A574 File Offset: 0x00038774
		private void Awake()
		{
			base.GetComponent<Shout_Ability>().OnShoutEvent.AddListener(new UnityAction<GameObject>(this.OnShout));
			this.m_shoutEventInstance = AudioUtility.GetEventInstance(this.m_shoutAudioPath, base.transform);
		}

		// Token: 0x0600692D RID: 26925 RVA: 0x0003A5A9 File Offset: 0x000387A9
		private void OnDestroy()
		{
			if (this.m_shoutEventInstance.isValid())
			{
				this.m_shoutEventInstance.release();
			}
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x001818A0 File Offset: 0x0017FAA0
		private void OnShout(GameObject playerGameObject)
		{
			float playerGenderAudioParameterValue = AudioUtility.GetPlayerGenderAudioParameterValue();
			float playerSizeAudioParameterValue = AudioUtility.GetPlayerSizeAudioParameterValue();
			this.m_shoutEventInstance.setParameterByName("gender", playerGenderAudioParameterValue, false);
			this.m_shoutEventInstance.setParameterByName("Player_Size", playerSizeAudioParameterValue, false);
			AudioManager.PlayAttached(this, this.m_shoutEventInstance, base.gameObject);
		}

		// Token: 0x04005584 RID: 21892
		[SerializeField]
		[EventRef]
		private string m_shoutAudioPath;

		// Token: 0x04005585 RID: 21893
		private string m_description = string.Empty;

		// Token: 0x04005586 RID: 21894
		private EventInstance m_shoutEventInstance;
	}
}

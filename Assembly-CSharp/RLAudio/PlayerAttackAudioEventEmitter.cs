using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000904 RID: 2308
	public class PlayerAttackAudioEventEmitter : OnEnterEventEmitter
	{
		// Token: 0x06004BBF RID: 19391 RVA: 0x001104E8 File Offset: 0x0010E6E8
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			float playerGenderAudioParameterValue = AudioUtility.GetPlayerGenderAudioParameterValue();
			float playerSizeAudioParameterValue = AudioUtility.GetPlayerSizeAudioParameterValue();
			if (this.m_eventInstance.isValid())
			{
				this.m_eventInstance.setParameterByName("gender", playerGenderAudioParameterValue, false);
				this.m_eventInstance.setParameterByName("Player_Size", playerSizeAudioParameterValue, false);
			}
		}
	}
}

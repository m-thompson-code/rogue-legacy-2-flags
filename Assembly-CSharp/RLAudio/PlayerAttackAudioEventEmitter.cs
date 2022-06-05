using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E80 RID: 3712
	public class PlayerAttackAudioEventEmitter : OnEnterEventEmitter
	{
		// Token: 0x060068B4 RID: 26804 RVA: 0x0018084C File Offset: 0x0017EA4C
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

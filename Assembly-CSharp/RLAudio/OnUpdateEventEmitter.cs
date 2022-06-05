using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E7B RID: 3707
	public class OnUpdateEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x0600688B RID: 26763 RVA: 0x0017FF78 File Offset: 0x0017E178
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (!base.GetShouldPlay(animator))
			{
				return;
			}
			if ((this.m_playTimes == null || this.m_playTimes.Length == 0) && !this.m_hasWarningBeenLogged)
			{
				this.m_hasWarningBeenLogged = true;
				Debug.LogFormat("<color=red>| {0} | No play times have been set. Either remove this State Behaviour or set some play times.</color>", new object[]
				{
					base.Description
				});
				return;
			}
			this.Reset();
		}

		// Token: 0x0600688C RID: 26764 RVA: 0x0017FFD8 File Offset: 0x0017E1D8
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!base.GetShouldPlay(animator))
			{
				return;
			}
			if (this.m_playTimes == null || this.m_playTimes.Length == 0)
			{
				return;
			}
			float num = stateInfo.normalizedTime % 1f;
			if (num < this.m_previousNormalizedTime)
			{
				this.Reset();
			}
			if (this.m_nextPlayTimeIndex != -1 && num >= this.m_playTimes[this.m_nextPlayTimeIndex])
			{
				if (!this.m_skipFirstPlayTimeOnEnter || (this.m_skipFirstPlayTimeOnEnter && this.m_skippedFirstPlayTime))
				{
					base.Play(animator);
				}
				else
				{
					this.m_skippedFirstPlayTime = true;
				}
				if (this.m_nextPlayTimeIndex < this.m_playTimes.Length - 1)
				{
					this.m_nextPlayTimeIndex++;
				}
				else
				{
					this.m_nextPlayTimeIndex = -1;
				}
			}
			this.m_previousNormalizedTime = num;
		}

		// Token: 0x0600688D RID: 26765 RVA: 0x00039E16 File Offset: 0x00038016
		private void Reset()
		{
			this.m_nextPlayTimeIndex = 0;
			this.m_previousNormalizedTime = -1f;
		}

		// Token: 0x040054EC RID: 21740
		[SerializeField]
		[Range(0f, 1f)]
		private float[] m_playTimes;

		// Token: 0x040054ED RID: 21741
		[SerializeField]
		private bool m_skipFirstPlayTimeOnEnter;

		// Token: 0x040054EE RID: 21742
		private bool m_skippedFirstPlayTime;

		// Token: 0x040054EF RID: 21743
		private int m_nextPlayTimeIndex = -1;

		// Token: 0x040054F0 RID: 21744
		private float m_previousNormalizedTime = -1f;

		// Token: 0x040054F1 RID: 21745
		private bool m_hasWarningBeenLogged;
	}
}

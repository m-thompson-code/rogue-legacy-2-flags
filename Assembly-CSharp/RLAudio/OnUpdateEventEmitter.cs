using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000901 RID: 2305
	public class OnUpdateEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06004BA2 RID: 19362 RVA: 0x0010FD30 File Offset: 0x0010DF30
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

		// Token: 0x06004BA3 RID: 19363 RVA: 0x0010FD90 File Offset: 0x0010DF90
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

		// Token: 0x06004BA4 RID: 19364 RVA: 0x0010FE47 File Offset: 0x0010E047
		private void Reset()
		{
			this.m_nextPlayTimeIndex = 0;
			this.m_previousNormalizedTime = -1f;
		}

		// Token: 0x04003F9A RID: 16282
		[SerializeField]
		[Range(0f, 1f)]
		private float[] m_playTimes;

		// Token: 0x04003F9B RID: 16283
		[SerializeField]
		private bool m_skipFirstPlayTimeOnEnter;

		// Token: 0x04003F9C RID: 16284
		private bool m_skippedFirstPlayTime;

		// Token: 0x04003F9D RID: 16285
		private int m_nextPlayTimeIndex = -1;

		// Token: 0x04003F9E RID: 16286
		private float m_previousNormalizedTime = -1f;

		// Token: 0x04003F9F RID: 16287
		private bool m_hasWarningBeenLogged;
	}
}

using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E77 RID: 3703
	public class OnEnterEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x0600687F RID: 26751 RVA: 0x00039D36 File Offset: 0x00037F36
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (!base.GetShouldPlay(animator))
			{
				return;
			}
			this.m_hasPlayed = false;
			if (this.m_timeAsPercent == 0f)
			{
				base.Play(animator);
				this.m_hasPlayed = true;
			}
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x0017FEB8 File Offset: 0x0017E0B8
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if (this.m_timeAsPercent == 0f)
			{
				return;
			}
			if (!base.GetShouldPlay(animator))
			{
				return;
			}
			float normalizedTime = animatorStateInfo.normalizedTime;
			if (!this.m_hasPlayed && normalizedTime >= this.m_timeAsPercent)
			{
				base.Play(animator);
				this.m_hasPlayed = true;
			}
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x00039D6D File Offset: 0x00037F6D
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!base.GetShouldPlay(animator))
			{
				return;
			}
			if (this.m_stopOnExit)
			{
				base.Stop();
			}
		}

		// Token: 0x040054E4 RID: 21732
		[SerializeField]
		private bool m_stopOnExit;

		// Token: 0x040054E5 RID: 21733
		[SerializeField]
		[Range(0f, 3f)]
		private float m_timeAsPercent;

		// Token: 0x040054E6 RID: 21734
		private bool m_hasPlayed;
	}
}

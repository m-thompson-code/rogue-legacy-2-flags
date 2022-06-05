using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008FD RID: 2301
	public class OnEnterEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06004B96 RID: 19350 RVA: 0x0010FB78 File Offset: 0x0010DD78
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

		// Token: 0x06004B97 RID: 19351 RVA: 0x0010FBB0 File Offset: 0x0010DDB0
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

		// Token: 0x06004B98 RID: 19352 RVA: 0x0010FBFC File Offset: 0x0010DDFC
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

		// Token: 0x04003F92 RID: 16274
		[SerializeField]
		private bool m_stopOnExit;

		// Token: 0x04003F93 RID: 16275
		[SerializeField]
		[Range(0f, 3f)]
		private float m_timeAsPercent;

		// Token: 0x04003F94 RID: 16276
		private bool m_hasPlayed;
	}
}

using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000900 RID: 2304
	public class OnTriggerEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06004B9F RID: 19359 RVA: 0x0010FCE5 File Offset: 0x0010DEE5
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (this.m_triggerID == -1)
			{
				this.m_triggerID = Animator.StringToHash(this.m_triggerName);
			}
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x0010FD0A File Offset: 0x0010DF0A
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (animator.GetBool(this.m_triggerID))
			{
				base.Play(animator);
			}
		}

		// Token: 0x04003F98 RID: 16280
		[SerializeField]
		private string m_triggerName;

		// Token: 0x04003F99 RID: 16281
		private int m_triggerID = -1;
	}
}

using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E7A RID: 3706
	public class OnTriggerEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06006888 RID: 26760 RVA: 0x00039DCB File Offset: 0x00037FCB
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (this.m_triggerID == -1)
			{
				this.m_triggerID = Animator.StringToHash(this.m_triggerName);
			}
		}

		// Token: 0x06006889 RID: 26761 RVA: 0x00039DF0 File Offset: 0x00037FF0
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (animator.GetBool(this.m_triggerID))
			{
				base.Play(animator);
			}
		}

		// Token: 0x040054EA RID: 21738
		[SerializeField]
		private string m_triggerName;

		// Token: 0x040054EB RID: 21739
		private int m_triggerID = -1;
	}
}

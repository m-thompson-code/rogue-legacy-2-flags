using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E79 RID: 3705
	public class OnFacingChangeEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06006885 RID: 26757 RVA: 0x0017FF04 File Offset: 0x0017E104
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (!this.m_hasSearchedForCorgiController)
			{
				this.m_hasSearchedForCorgiController = true;
				this.m_characterController = animator.GetComponent<BaseCharacterController>();
				if (this.m_characterController == null)
				{
					Debug.LogFormat("<color=red>| {0} | This behaviour requires that the Animator's GameObject has a BaseCharacterController component attached. If you see this message, please add a bug to Pivotal and attach the stack trace.</color>", new object[]
					{
						this
					});
				}
			}
			if (this.m_characterController != null)
			{
				this.m_previousIsFacingRightValue = this.m_characterController.IsFacingRight;
			}
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x00039D90 File Offset: 0x00037F90
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.m_characterController != null && this.m_characterController.IsFacingRight != this.m_previousIsFacingRightValue)
			{
				this.m_previousIsFacingRightValue = this.m_characterController.IsFacingRight;
				base.Play(animator);
			}
		}

		// Token: 0x040054E7 RID: 21735
		private bool m_hasSearchedForCorgiController;

		// Token: 0x040054E8 RID: 21736
		private BaseCharacterController m_characterController;

		// Token: 0x040054E9 RID: 21737
		private bool m_previousIsFacingRightValue;
	}
}

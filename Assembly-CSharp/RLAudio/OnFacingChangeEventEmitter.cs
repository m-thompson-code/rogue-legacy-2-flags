using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008FF RID: 2303
	public class OnFacingChangeEventEmitter : AnimBehaviourEventEmitter
	{
		// Token: 0x06004B9C RID: 19356 RVA: 0x0010FC30 File Offset: 0x0010DE30
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

		// Token: 0x06004B9D RID: 19357 RVA: 0x0010FCA2 File Offset: 0x0010DEA2
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.m_characterController != null && this.m_characterController.IsFacingRight != this.m_previousIsFacingRightValue)
			{
				this.m_previousIsFacingRightValue = this.m_characterController.IsFacingRight;
				base.Play(animator);
			}
		}

		// Token: 0x04003F95 RID: 16277
		private bool m_hasSearchedForCorgiController;

		// Token: 0x04003F96 RID: 16278
		private BaseCharacterController m_characterController;

		// Token: 0x04003F97 RID: 16279
		private bool m_previousIsFacingRightValue;
	}
}

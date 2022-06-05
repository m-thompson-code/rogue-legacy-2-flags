using System;
using UnityEngine;

// Token: 0x0200031D RID: 797
[SharedBetweenAnimators]
public class AbilityAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x17000C46 RID: 3142
	// (get) Token: 0x06001961 RID: 6497 RVA: 0x0000CC1C File Offset: 0x0000AE1C
	public AbilityAnimState AnimatorStateType
	{
		get
		{
			return this.m_animatorStateType;
		}
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x0000CC24 File Offset: 0x0000AE24
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (animator.GetBool(this.m_isTraitorBossID))
		{
			return;
		}
		AbilityAnimBehaviourManager.CurrentAnimatorState = this;
		AbilityAnimBehaviourManager.InvokeAnimOnEnter(this.AnimatorStateType);
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x0000CC46 File Offset: 0x0000AE46
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (animator.GetBool(this.m_isTraitorBossID))
		{
			return;
		}
		if (AbilityAnimBehaviourManager.CurrentAnimatorState == this)
		{
			if (this.m_animatorStateType == AbilityAnimState.Exit)
			{
				AbilityAnimBehaviourManager.AnimationComplete = true;
			}
			AbilityAnimBehaviourManager.InvokeAnimOnExit(this.AnimatorStateType);
		}
	}

	// Token: 0x0400180A RID: 6154
	[Space(10f)]
	[Tooltip("The type of state the animator is in.")]
	[SerializeField]
	private AbilityAnimState m_animatorStateType;

	// Token: 0x0400180B RID: 6155
	private int m_isTraitorBossID = Animator.StringToHash("IsTraitorBoss");
}

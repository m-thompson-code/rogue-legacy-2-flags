using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
[SharedBetweenAnimators]
public class AbilityAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06001118 RID: 4376 RVA: 0x000315DB File Offset: 0x0002F7DB
	public AbilityAnimState AnimatorStateType
	{
		get
		{
			return this.m_animatorStateType;
		}
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x000315E3 File Offset: 0x0002F7E3
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (animator.GetBool(this.m_isTraitorBossID))
		{
			return;
		}
		AbilityAnimBehaviourManager.CurrentAnimatorState = this;
		AbilityAnimBehaviourManager.InvokeAnimOnEnter(this.AnimatorStateType);
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x00031605 File Offset: 0x0002F805
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

	// Token: 0x04001201 RID: 4609
	[Space(10f)]
	[Tooltip("The type of state the animator is in.")]
	[SerializeField]
	private AbilityAnimState m_animatorStateType;

	// Token: 0x04001202 RID: 4610
	private int m_isTraitorBossID = Animator.StringToHash("IsTraitorBoss");
}

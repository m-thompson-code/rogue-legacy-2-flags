using System;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class RandomizeParamAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x0600113B RID: 4411 RVA: 0x00031ED8 File Offset: 0x000300D8
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.m_currentLoop = 0;
		if (this.m_randomizeEveryOnEnter)
		{
			this.m_randomNum = UnityEngine.Random.Range(this.m_randomMin, this.m_randomMax);
		}
		else if (!this.m_randomNumCached)
		{
			this.m_randomNumCached = true;
			this.m_randomNum = UnityEngine.Random.Range(this.m_randomMin, this.m_randomMax);
		}
		animator.SetFloat(this.m_paramName, this.m_randomNum);
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00031F45 File Offset: 0x00030145
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.m_randomizeEveryAnimLoop && this.m_currentLoop < (int)stateInfo.normalizedTime)
		{
			this.m_currentLoop = (int)stateInfo.normalizedTime;
			this.m_randomNum = UnityEngine.Random.Range(this.m_randomMin, this.m_randomMax);
		}
	}

	// Token: 0x04001224 RID: 4644
	[SerializeField]
	private string m_paramName;

	// Token: 0x04001225 RID: 4645
	[SerializeField]
	private float m_randomMin;

	// Token: 0x04001226 RID: 4646
	[SerializeField]
	private float m_randomMax = 1f;

	// Token: 0x04001227 RID: 4647
	[SerializeField]
	private bool m_randomizeEveryOnEnter = true;

	// Token: 0x04001228 RID: 4648
	[SerializeField]
	private bool m_randomizeEveryAnimLoop;

	// Token: 0x04001229 RID: 4649
	private bool m_randomNumCached;

	// Token: 0x0400122A RID: 4650
	private float m_randomNum;

	// Token: 0x0400122B RID: 4651
	private int m_currentLoop;
}

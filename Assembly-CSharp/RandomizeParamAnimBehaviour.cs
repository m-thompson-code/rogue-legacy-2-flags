using System;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class RandomizeParamAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x06001984 RID: 6532 RVA: 0x00090294 File Offset: 0x0008E494
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

	// Token: 0x06001985 RID: 6533 RVA: 0x0000CDE1 File Offset: 0x0000AFE1
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.m_randomizeEveryAnimLoop && this.m_currentLoop < (int)stateInfo.normalizedTime)
		{
			this.m_currentLoop = (int)stateInfo.normalizedTime;
			this.m_randomNum = UnityEngine.Random.Range(this.m_randomMin, this.m_randomMax);
		}
	}

	// Token: 0x0400182D RID: 6189
	[SerializeField]
	private string m_paramName;

	// Token: 0x0400182E RID: 6190
	[SerializeField]
	private float m_randomMin;

	// Token: 0x0400182F RID: 6191
	[SerializeField]
	private float m_randomMax = 1f;

	// Token: 0x04001830 RID: 6192
	[SerializeField]
	private bool m_randomizeEveryOnEnter = true;

	// Token: 0x04001831 RID: 6193
	[SerializeField]
	private bool m_randomizeEveryAnimLoop;

	// Token: 0x04001832 RID: 6194
	private bool m_randomNumCached;

	// Token: 0x04001833 RID: 6195
	private float m_randomNum;

	// Token: 0x04001834 RID: 6196
	private int m_currentLoop;
}

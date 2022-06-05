using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000CF7 RID: 3319
internal class WaitUntilAnimComplete_Yield : IEnumerator
{
	// Token: 0x06005EB3 RID: 24243 RVA: 0x0003436E File Offset: 0x0003256E
	public WaitUntilAnimComplete_Yield(Animator animator, int animLayer)
	{
		this.CreateNew(animator, animLayer);
	}

	// Token: 0x17001F21 RID: 7969
	// (get) Token: 0x06005EB4 RID: 24244 RVA: 0x0000F49B File Offset: 0x0000D69B
	public object Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06005EB5 RID: 24245 RVA: 0x00162B78 File Offset: 0x00160D78
	public bool MoveNext()
	{
		if (this.m_needsReset)
		{
			this.CreateNew(this.m_animator, this.m_animLayer);
		}
		if (!this.m_clipIsEmpty)
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer);
			bool flag = currentAnimatorStateInfo.normalizedTime < 1f && currentAnimatorStateInfo.shortNameHash == this.m_storedStateNameHash;
			if (!flag)
			{
				this.m_needsReset = true;
			}
			return flag;
		}
		this.m_needsReset = true;
		return false;
	}

	// Token: 0x06005EB6 RID: 24246 RVA: 0x00162BEC File Offset: 0x00160DEC
	public void CreateNew(Animator animator, int animLayer)
	{
		this.m_needsReset = false;
		this.m_animator = animator;
		this.m_animLayer = animLayer;
		this.m_storedStateNameHash = this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer).shortNameHash;
		this.m_clipIsEmpty = (this.m_animator.GetCurrentAnimatorClipInfoCount(this.m_animLayer) <= 0);
	}

	// Token: 0x06005EB7 RID: 24247 RVA: 0x0003437E File Offset: 0x0003257E
	public void Reset()
	{
		this.CreateNew(this.m_animator, this.m_animLayer);
	}

	// Token: 0x04004DC5 RID: 19909
	private Animator m_animator;

	// Token: 0x04004DC6 RID: 19910
	private int m_storedStateNameHash;

	// Token: 0x04004DC7 RID: 19911
	private bool m_needsReset;

	// Token: 0x04004DC8 RID: 19912
	private int m_animLayer;

	// Token: 0x04004DC9 RID: 19913
	private bool m_clipIsEmpty;
}

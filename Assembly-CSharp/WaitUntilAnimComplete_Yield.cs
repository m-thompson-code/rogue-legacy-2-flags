using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000820 RID: 2080
internal class WaitUntilAnimComplete_Yield : IEnumerator
{
	// Token: 0x060044E7 RID: 17639 RVA: 0x000F4FD0 File Offset: 0x000F31D0
	public WaitUntilAnimComplete_Yield(Animator animator, int animLayer)
	{
		this.CreateNew(animator, animLayer);
	}

	// Token: 0x17001713 RID: 5907
	// (get) Token: 0x060044E8 RID: 17640 RVA: 0x000F4FE0 File Offset: 0x000F31E0
	public object Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060044E9 RID: 17641 RVA: 0x000F4FE4 File Offset: 0x000F31E4
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

	// Token: 0x060044EA RID: 17642 RVA: 0x000F5058 File Offset: 0x000F3258
	public void CreateNew(Animator animator, int animLayer)
	{
		this.m_needsReset = false;
		this.m_animator = animator;
		this.m_animLayer = animLayer;
		this.m_storedStateNameHash = this.m_animator.GetCurrentAnimatorStateInfo(this.m_animLayer).shortNameHash;
		this.m_clipIsEmpty = (this.m_animator.GetCurrentAnimatorClipInfoCount(this.m_animLayer) <= 0);
	}

	// Token: 0x060044EB RID: 17643 RVA: 0x000F50B6 File Offset: 0x000F32B6
	public void Reset()
	{
		this.CreateNew(this.m_animator, this.m_animLayer);
	}

	// Token: 0x04003AC1 RID: 15041
	private Animator m_animator;

	// Token: 0x04003AC2 RID: 15042
	private int m_storedStateNameHash;

	// Token: 0x04003AC3 RID: 15043
	private bool m_needsReset;

	// Token: 0x04003AC4 RID: 15044
	private int m_animLayer;

	// Token: 0x04003AC5 RID: 15045
	private bool m_clipIsEmpty;
}

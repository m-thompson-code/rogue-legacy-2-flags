using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200081F RID: 2079
public class WaitUntilAnimChanged_Yield : IEnumerator
{
	// Token: 0x060044E2 RID: 17634 RVA: 0x000F4F2B File Offset: 0x000F312B
	public WaitUntilAnimChanged_Yield(Animator animator, string animatorBoolName, AnimationLayer layer = AnimationLayer.Base)
	{
		this.CreateNew(animator, animatorBoolName, layer);
	}

	// Token: 0x17001712 RID: 5906
	// (get) Token: 0x060044E3 RID: 17635 RVA: 0x000F4F3C File Offset: 0x000F313C
	public object Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060044E4 RID: 17636 RVA: 0x000F4F40 File Offset: 0x000F3140
	public bool MoveNext()
	{
		if (this.m_needsReset)
		{
			this.CreateNew(this.m_animator, this.m_animatorBoolName, (AnimationLayer)this.m_animationLayer);
		}
		if (this.m_animator.GetCurrentAnimatorStateInfo(this.m_animationLayer).IsName(this.m_animatorBoolName))
		{
			this.m_needsReset = true;
			return false;
		}
		return true;
	}

	// Token: 0x060044E5 RID: 17637 RVA: 0x000F4F98 File Offset: 0x000F3198
	public void CreateNew(Animator animator, string animatorBoolName, AnimationLayer layer)
	{
		this.m_needsReset = false;
		this.m_animator = animator;
		this.m_animatorBoolName = animatorBoolName;
		this.m_animationLayer = (int)layer;
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x000F4FB6 File Offset: 0x000F31B6
	public void Reset()
	{
		this.CreateNew(this.m_animator, this.m_animatorBoolName, (AnimationLayer)this.m_animationLayer);
	}

	// Token: 0x04003ABD RID: 15037
	private Animator m_animator;

	// Token: 0x04003ABE RID: 15038
	private string m_animatorBoolName;

	// Token: 0x04003ABF RID: 15039
	private int m_animationLayer;

	// Token: 0x04003AC0 RID: 15040
	private bool m_needsReset;
}

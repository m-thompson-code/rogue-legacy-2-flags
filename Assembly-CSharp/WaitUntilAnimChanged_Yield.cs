using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000CF6 RID: 3318
public class WaitUntilAnimChanged_Yield : IEnumerator
{
	// Token: 0x06005EAE RID: 24238 RVA: 0x00034325 File Offset: 0x00032525
	public WaitUntilAnimChanged_Yield(Animator animator, string animatorBoolName, AnimationLayer layer = AnimationLayer.Base)
	{
		this.CreateNew(animator, animatorBoolName, layer);
	}

	// Token: 0x17001F20 RID: 7968
	// (get) Token: 0x06005EAF RID: 24239 RVA: 0x0000F49B File Offset: 0x0000D69B
	public object Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06005EB0 RID: 24240 RVA: 0x00162B20 File Offset: 0x00160D20
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

	// Token: 0x06005EB1 RID: 24241 RVA: 0x00034336 File Offset: 0x00032536
	public void CreateNew(Animator animator, string animatorBoolName, AnimationLayer layer)
	{
		this.m_needsReset = false;
		this.m_animator = animator;
		this.m_animatorBoolName = animatorBoolName;
		this.m_animationLayer = (int)layer;
	}

	// Token: 0x06005EB2 RID: 24242 RVA: 0x00034354 File Offset: 0x00032554
	public void Reset()
	{
		this.CreateNew(this.m_animator, this.m_animatorBoolName, (AnimationLayer)this.m_animationLayer);
	}

	// Token: 0x04004DC1 RID: 19905
	private Animator m_animator;

	// Token: 0x04004DC2 RID: 19906
	private string m_animatorBoolName;

	// Token: 0x04004DC3 RID: 19907
	private int m_animationLayer;

	// Token: 0x04004DC4 RID: 19908
	private bool m_needsReset;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B9 RID: 1977
public class SetAnimParamAtLifeSpanProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003C14 RID: 15380 RVA: 0x000212AA File Offset: 0x0001F4AA
	private void OnEnable()
	{
		if (base.SourceProjectile)
		{
			base.StartCoroutine(this.FlashCoroutine());
		}
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x000212C6 File Offset: 0x0001F4C6
	private IEnumerator FlashCoroutine()
	{
		float num = base.SourceProjectile.Lifespan * this.m_triggerAtNormalizedLifeSpan;
		float endTime = Time.time + num;
		while (Time.time < endTime)
		{
			yield return null;
		}
		if (base.SourceProjectile)
		{
			AnimatorControllerParameterType animParamType = this.m_animParamType;
			switch (animParamType)
			{
			case AnimatorControllerParameterType.Float:
				base.SourceProjectile.Animator.SetFloat(this.m_animParamName, this.m_numValue);
				break;
			case (AnimatorControllerParameterType)2:
				break;
			case AnimatorControllerParameterType.Int:
				base.SourceProjectile.Animator.SetInteger(this.m_animParamName, (int)this.m_numValue);
				break;
			case AnimatorControllerParameterType.Bool:
				base.SourceProjectile.Animator.SetBool(this.m_animParamName, this.m_boolValue);
				break;
			default:
				if (animParamType == AnimatorControllerParameterType.Trigger)
				{
					if (this.m_boolValue)
					{
						base.SourceProjectile.Animator.SetTrigger(this.m_animParamName);
					}
				}
				break;
			}
		}
		yield break;
	}

	// Token: 0x04002FAF RID: 12207
	[SerializeField]
	private AnimatorControllerParameterType m_animParamType;

	// Token: 0x04002FB0 RID: 12208
	[SerializeField]
	private float m_triggerAtNormalizedLifeSpan = 0.8f;

	// Token: 0x04002FB1 RID: 12209
	[SerializeField]
	private string m_animParamName;

	// Token: 0x04002FB2 RID: 12210
	[SerializeField]
	private float m_numValue;

	// Token: 0x04002FB3 RID: 12211
	[SerializeField]
	private bool m_boolValue;
}

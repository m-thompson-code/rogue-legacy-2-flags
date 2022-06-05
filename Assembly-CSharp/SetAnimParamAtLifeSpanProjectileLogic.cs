using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AB RID: 1195
public class SetAnimParamAtLifeSpanProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B9B RID: 11163 RVA: 0x000942FE File Offset: 0x000924FE
	private void OnEnable()
	{
		if (base.SourceProjectile)
		{
			base.StartCoroutine(this.FlashCoroutine());
		}
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x0009431A File Offset: 0x0009251A
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

	// Token: 0x04002372 RID: 9074
	[SerializeField]
	private AnimatorControllerParameterType m_animParamType;

	// Token: 0x04002373 RID: 9075
	[SerializeField]
	private float m_triggerAtNormalizedLifeSpan = 0.8f;

	// Token: 0x04002374 RID: 9076
	[SerializeField]
	private string m_animParamName;

	// Token: 0x04002375 RID: 9077
	[SerializeField]
	private float m_numValue;

	// Token: 0x04002376 RID: 9078
	[SerializeField]
	private bool m_boolValue;
}

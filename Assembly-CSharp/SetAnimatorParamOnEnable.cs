using System;
using UnityEngine;

// Token: 0x020004F1 RID: 1265
public class SetAnimatorParamOnEnable : MonoBehaviour
{
	// Token: 0x060028C6 RID: 10438 RVA: 0x000BEE20 File Offset: 0x000BD020
	private void OnEnable()
	{
		if (this.m_animator)
		{
			AnimatorControllerParameterType paramType = this.m_paramType;
			switch (paramType)
			{
			case AnimatorControllerParameterType.Float:
				this.m_animator.SetFloat(this.m_paramName, this.m_numberValue);
				break;
			case (AnimatorControllerParameterType)2:
				break;
			case AnimatorControllerParameterType.Int:
				this.m_animator.SetInteger(this.m_paramName, (int)this.m_numberValue);
				return;
			case AnimatorControllerParameterType.Bool:
				this.m_animator.SetBool(this.m_paramName, true);
				return;
			default:
				if (paramType != AnimatorControllerParameterType.Trigger)
				{
					return;
				}
				this.m_animator.SetTrigger(this.m_paramName);
				return;
			}
		}
	}

	// Token: 0x040023BA RID: 9146
	[SerializeField]
	private Animator m_animator;

	// Token: 0x040023BB RID: 9147
	[SerializeField]
	private AnimatorControllerParameterType m_paramType;

	// Token: 0x040023BC RID: 9148
	[SerializeField]
	private string m_paramName;

	// Token: 0x040023BD RID: 9149
	[SerializeField]
	private float m_numberValue;
}

using System;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class SetAnimatorParamOnEnable : MonoBehaviour
{
	// Token: 0x06001D81 RID: 7553 RVA: 0x000611D0 File Offset: 0x0005F3D0
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

	// Token: 0x04001B6F RID: 7023
	[SerializeField]
	private Animator m_animator;

	// Token: 0x04001B70 RID: 7024
	[SerializeField]
	private AnimatorControllerParameterType m_paramType;

	// Token: 0x04001B71 RID: 7025
	[SerializeField]
	private string m_paramName;

	// Token: 0x04001B72 RID: 7026
	[SerializeField]
	private float m_numberValue;
}

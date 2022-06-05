using System;
using UnityEngine;

// Token: 0x02000321 RID: 801
[Serializable]
public struct AnimBehaviourCondition
{
	// Token: 0x06001971 RID: 6513 RVA: 0x0008FB6C File Offset: 0x0008DD6C
	public bool IsTrue(Animator animator)
	{
		if (!this.m_isInitialized)
		{
			this.m_isInitialized = true;
			this.m_hasParameter = global::AnimatorUtility.HasParameter(animator, this.ParamName);
			if (this.m_hasParameter)
			{
				this.m_id = Animator.StringToHash(this.ParamName);
			}
		}
		if (this.m_hasParameter)
		{
			AnimatorControllerParameterType paramType = this.ParamType;
			switch (paramType)
			{
			case AnimatorControllerParameterType.Float:
				switch (this.EqualityType)
				{
				case ParamEqualityType.Greater:
					return animator.GetFloat(this.m_id) > this.FloatValue;
				case ParamEqualityType.Less:
					return animator.GetFloat(this.m_id) < this.FloatValue;
				case ParamEqualityType.Equals:
					return animator.GetFloat(this.m_id) == this.FloatValue;
				case ParamEqualityType.NotEqual:
					return animator.GetFloat(this.m_id) != this.FloatValue;
				case ParamEqualityType.GreaterOrEqual:
					return animator.GetFloat(this.m_id) >= this.FloatValue;
				case ParamEqualityType.LessOrEqual:
					return animator.GetFloat(this.m_id) <= this.FloatValue;
				}
				break;
			case (AnimatorControllerParameterType)2:
				break;
			case AnimatorControllerParameterType.Int:
				switch (this.EqualityType)
				{
				case ParamEqualityType.Greater:
					return animator.GetInteger(this.m_id) > this.IntValue;
				case ParamEqualityType.Less:
					return animator.GetInteger(this.m_id) < this.IntValue;
				case ParamEqualityType.Equals:
					return animator.GetInteger(this.m_id) == this.IntValue;
				case ParamEqualityType.NotEqual:
					return animator.GetInteger(this.m_id) != this.IntValue;
				case ParamEqualityType.GreaterOrEqual:
					return animator.GetInteger(this.m_id) >= this.IntValue;
				case ParamEqualityType.LessOrEqual:
					return animator.GetInteger(this.m_id) <= this.IntValue;
				}
				break;
			case AnimatorControllerParameterType.Bool:
				if (this.BoolType == ParamBoolType.True)
				{
					return animator.GetBool(this.m_id);
				}
				return !animator.GetBool(this.m_id);
			default:
				if (paramType == AnimatorControllerParameterType.Trigger)
				{
					return animator.GetBool(this.m_id);
				}
				break;
			}
		}
		return false;
	}

	// Token: 0x0400181B RID: 6171
	public string ParamName;

	// Token: 0x0400181C RID: 6172
	public AnimatorControllerParameterType ParamType;

	// Token: 0x0400181D RID: 6173
	public ParamBoolType BoolType;

	// Token: 0x0400181E RID: 6174
	public int IntValue;

	// Token: 0x0400181F RID: 6175
	public float FloatValue;

	// Token: 0x04001820 RID: 6176
	public ParamEqualityType EqualityType;

	// Token: 0x04001821 RID: 6177
	private bool m_isInitialized;

	// Token: 0x04001822 RID: 6178
	private bool m_hasParameter;

	// Token: 0x04001823 RID: 6179
	private int m_id;
}

using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
[Serializable]
public struct AnimBehaviourCondition
{
	// Token: 0x06001128 RID: 4392 RVA: 0x00031720 File Offset: 0x0002F920
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

	// Token: 0x04001212 RID: 4626
	public string ParamName;

	// Token: 0x04001213 RID: 4627
	public AnimatorControllerParameterType ParamType;

	// Token: 0x04001214 RID: 4628
	public ParamBoolType BoolType;

	// Token: 0x04001215 RID: 4629
	public int IntValue;

	// Token: 0x04001216 RID: 4630
	public float FloatValue;

	// Token: 0x04001217 RID: 4631
	public ParamEqualityType EqualityType;

	// Token: 0x04001218 RID: 4632
	private bool m_isInitialized;

	// Token: 0x04001219 RID: 4633
	private bool m_hasParameter;

	// Token: 0x0400121A RID: 4634
	private int m_id;
}

using System;
using UnityEngine;

// Token: 0x020006FE RID: 1790
[Serializable]
public class AnimationControllerEntry
{
	// Token: 0x1700161B RID: 5659
	// (get) Token: 0x060040BF RID: 16575 RVA: 0x000E56EE File Offset: 0x000E38EE
	// (set) Token: 0x060040C0 RID: 16576 RVA: 0x000E56F6 File Offset: 0x000E38F6
	public int Id
	{
		get
		{
			return this.m_id;
		}
		set
		{
			this.m_id = value;
		}
	}

	// Token: 0x1700161C RID: 5660
	// (get) Token: 0x060040C1 RID: 16577 RVA: 0x000E56FF File Offset: 0x000E38FF
	// (set) Token: 0x060040C2 RID: 16578 RVA: 0x000E5707 File Offset: 0x000E3907
	public string AnimationParameter
	{
		get
		{
			return this.m_animationParameter;
		}
		set
		{
			this.m_animationParameter = value;
		}
	}

	// Token: 0x1700161D RID: 5661
	// (get) Token: 0x060040C3 RID: 16579 RVA: 0x000E5710 File Offset: 0x000E3910
	// (set) Token: 0x060040C4 RID: 16580 RVA: 0x000E5718 File Offset: 0x000E3918
	public bool BoolValue
	{
		get
		{
			return this.m_boolValue;
		}
		set
		{
			this.m_boolValue = value;
		}
	}

	// Token: 0x1700161E RID: 5662
	// (get) Token: 0x060040C5 RID: 16581 RVA: 0x000E5721 File Offset: 0x000E3921
	// (set) Token: 0x060040C6 RID: 16582 RVA: 0x000E5729 File Offset: 0x000E3929
	public int IntValue
	{
		get
		{
			return this.m_intValue;
		}
		set
		{
			this.m_intValue = value;
		}
	}

	// Token: 0x1700161F RID: 5663
	// (get) Token: 0x060040C7 RID: 16583 RVA: 0x000E5732 File Offset: 0x000E3932
	// (set) Token: 0x060040C8 RID: 16584 RVA: 0x000E573A File Offset: 0x000E393A
	public AnimatorControllerParameterType ParameterType
	{
		get
		{
			return this.m_parameterType;
		}
		set
		{
			this.m_parameterType = value;
		}
	}

	// Token: 0x17001620 RID: 5664
	// (get) Token: 0x060040C9 RID: 16585 RVA: 0x000E5743 File Offset: 0x000E3943
	// (set) Token: 0x060040CA RID: 16586 RVA: 0x000E574B File Offset: 0x000E394B
	public float FloatValue
	{
		get
		{
			return this.m_floatValue;
		}
		set
		{
			this.m_floatValue = value;
		}
	}

	// Token: 0x0400327C RID: 12924
	[SerializeField]
	[ReadOnly]
	private int m_id;

	// Token: 0x0400327D RID: 12925
	[SerializeField]
	private string m_animationParameter;

	// Token: 0x0400327E RID: 12926
	[SerializeField]
	private AnimatorControllerParameterType m_parameterType = AnimatorControllerParameterType.Bool;

	// Token: 0x0400327F RID: 12927
	[SerializeField]
	private bool m_boolValue;

	// Token: 0x04003280 RID: 12928
	[SerializeField]
	private int m_intValue;

	// Token: 0x04003281 RID: 12929
	[SerializeField]
	private float m_floatValue;
}

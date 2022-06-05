using System;
using UnityEngine;

// Token: 0x02000BB1 RID: 2993
[Serializable]
public class AnimationControllerEntry
{
	// Token: 0x17001E17 RID: 7703
	// (get) Token: 0x06005A08 RID: 23048 RVA: 0x00031281 File Offset: 0x0002F481
	// (set) Token: 0x06005A09 RID: 23049 RVA: 0x00031289 File Offset: 0x0002F489
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

	// Token: 0x17001E18 RID: 7704
	// (get) Token: 0x06005A0A RID: 23050 RVA: 0x00031292 File Offset: 0x0002F492
	// (set) Token: 0x06005A0B RID: 23051 RVA: 0x0003129A File Offset: 0x0002F49A
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

	// Token: 0x17001E19 RID: 7705
	// (get) Token: 0x06005A0C RID: 23052 RVA: 0x000312A3 File Offset: 0x0002F4A3
	// (set) Token: 0x06005A0D RID: 23053 RVA: 0x000312AB File Offset: 0x0002F4AB
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

	// Token: 0x17001E1A RID: 7706
	// (get) Token: 0x06005A0E RID: 23054 RVA: 0x000312B4 File Offset: 0x0002F4B4
	// (set) Token: 0x06005A0F RID: 23055 RVA: 0x000312BC File Offset: 0x0002F4BC
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

	// Token: 0x17001E1B RID: 7707
	// (get) Token: 0x06005A10 RID: 23056 RVA: 0x000312C5 File Offset: 0x0002F4C5
	// (set) Token: 0x06005A11 RID: 23057 RVA: 0x000312CD File Offset: 0x0002F4CD
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

	// Token: 0x17001E1C RID: 7708
	// (get) Token: 0x06005A12 RID: 23058 RVA: 0x000312D6 File Offset: 0x0002F4D6
	// (set) Token: 0x06005A13 RID: 23059 RVA: 0x000312DE File Offset: 0x0002F4DE
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

	// Token: 0x040044F7 RID: 17655
	[SerializeField]
	[ReadOnly]
	private int m_id;

	// Token: 0x040044F8 RID: 17656
	[SerializeField]
	private string m_animationParameter;

	// Token: 0x040044F9 RID: 17657
	[SerializeField]
	private AnimatorControllerParameterType m_parameterType = AnimatorControllerParameterType.Bool;

	// Token: 0x040044FA RID: 17658
	[SerializeField]
	private bool m_boolValue;

	// Token: 0x040044FB RID: 17659
	[SerializeField]
	private int m_intValue;

	// Token: 0x040044FC RID: 17660
	[SerializeField]
	private float m_floatValue;
}

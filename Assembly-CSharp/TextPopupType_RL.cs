using System;

// Token: 0x02000C47 RID: 3143
public class TextPopupType_RL
{
	// Token: 0x17001E47 RID: 7751
	// (get) Token: 0x06005AD9 RID: 23257 RVA: 0x00031D88 File Offset: 0x0002FF88
	public static TextPopupType[] TypeArray
	{
		get
		{
			if (TextPopupType_RL.m_typeArray == null)
			{
				TextPopupType_RL.m_typeArray = (Enum.GetValues(typeof(TextPopupType)) as TextPopupType[]);
			}
			return TextPopupType_RL.m_typeArray;
		}
	}

	// Token: 0x04004B31 RID: 19249
	private static TextPopupType[] m_typeArray;
}

using System;

// Token: 0x02000786 RID: 1926
public class TextPopupType_RL
{
	// Token: 0x1700164B RID: 5707
	// (get) Token: 0x0600415C RID: 16732 RVA: 0x000E935A File Offset: 0x000E755A
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

	// Token: 0x04003881 RID: 14465
	private static TextPopupType[] m_typeArray;
}

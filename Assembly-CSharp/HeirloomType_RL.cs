using System;

// Token: 0x02000BFA RID: 3066
public class HeirloomType_RL
{
	// Token: 0x17001E2E RID: 7726
	// (get) Token: 0x06005A81 RID: 23169 RVA: 0x000318EB File Offset: 0x0002FAEB
	public static HeirloomType[] TypeArray
	{
		get
		{
			if (HeirloomType_RL.m_typeArray == null)
			{
				HeirloomType_RL.m_typeArray = (Enum.GetValues(typeof(HeirloomType)) as HeirloomType[]);
			}
			return HeirloomType_RL.m_typeArray;
		}
	}

	// Token: 0x040046F1 RID: 18161
	private static HeirloomType[] m_typeArray;
}

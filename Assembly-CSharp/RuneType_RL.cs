using System;

// Token: 0x02000C2B RID: 3115
public class RuneType_RL
{
	// Token: 0x17001E42 RID: 7746
	// (get) Token: 0x06005AC4 RID: 23236 RVA: 0x00031CC5 File Offset: 0x0002FEC5
	public static RuneType[] TypeArray
	{
		get
		{
			if (RuneType_RL.m_typeArray == null)
			{
				RuneType_RL.m_typeArray = (Enum.GetValues(typeof(RuneType)) as RuneType[]);
			}
			return RuneType_RL.m_typeArray;
		}
	}

	// Token: 0x04004921 RID: 18721
	private static RuneType[] m_typeArray;
}

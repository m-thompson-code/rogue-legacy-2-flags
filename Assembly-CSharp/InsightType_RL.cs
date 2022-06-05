using System;

// Token: 0x02000744 RID: 1860
public class InsightType_RL
{
	// Token: 0x17001634 RID: 5684
	// (get) Token: 0x06004109 RID: 16649 RVA: 0x000E64E7 File Offset: 0x000E46E7
	public static InsightType[] TypeArray
	{
		get
		{
			if (InsightType_RL.m_insightTypeArray == null)
			{
				InsightType_RL.m_insightTypeArray = (Enum.GetValues(typeof(InsightType)) as InsightType[]);
			}
			return InsightType_RL.m_insightTypeArray;
		}
	}

	// Token: 0x0600410A RID: 16650 RVA: 0x000E6510 File Offset: 0x000E4710
	public static InsightType GetInsightFromHeirloomType(HeirloomType heirloomType)
	{
		if (heirloomType <= HeirloomType.UnlockBouncableDownstrike)
		{
			if (heirloomType == HeirloomType.UnlockDoubleJump)
			{
				return InsightType.HeirloomDoubleJump;
			}
			if (heirloomType == HeirloomType.UnlockAirDash)
			{
				return InsightType.HeirloomDash;
			}
			if (heirloomType == HeirloomType.UnlockBouncableDownstrike)
			{
				return InsightType.HeirloomSpinKick_Projectiles;
			}
		}
		else
		{
			if (heirloomType == HeirloomType.UnlockMemory)
			{
				return InsightType.HeirloomMemory;
			}
			if (heirloomType == HeirloomType.UnlockVoidDash)
			{
				return InsightType.HeirloomVoidDash;
			}
			if (heirloomType == HeirloomType.UnlockEarthShift)
			{
				return InsightType.HeirloomEarthShift;
			}
		}
		return InsightType.None;
	}

	// Token: 0x040034B2 RID: 13490
	private static InsightType[] m_insightTypeArray;
}

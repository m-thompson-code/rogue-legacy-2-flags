using System;

// Token: 0x02000C02 RID: 3074
public class InsightType_RL
{
	// Token: 0x17001E30 RID: 7728
	// (get) Token: 0x06005A86 RID: 23174 RVA: 0x00031939 File Offset: 0x0002FB39
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

	// Token: 0x06005A87 RID: 23175 RVA: 0x00155E6C File Offset: 0x0015406C
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

	// Token: 0x0400472E RID: 18222
	private static InsightType[] m_insightTypeArray;
}

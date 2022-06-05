using System;
using UnityEngine;

// Token: 0x020003BC RID: 956
public class HolidayLookController : MonoBehaviour
{
	// Token: 0x06001F84 RID: 8068 RVA: 0x000108DD File Offset: 0x0000EADD
	private void Awake()
	{
		this.m_storedMat = this.m_rendererToChange.sharedMaterial;
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x000108F0 File Offset: 0x0000EAF0
	private void OnEnable()
	{
		if (HolidayLookController.IsHoliday(this.m_holidayType))
		{
			this.m_rendererToChange.sharedMaterial = this.m_materialToApply;
			this.m_holidayLookApplied = true;
		}
	}

	// Token: 0x06001F86 RID: 8070 RVA: 0x00010917 File Offset: 0x0000EB17
	private void OnDisable()
	{
		if (this.m_holidayLookApplied)
		{
			this.m_rendererToChange.sharedMaterial = this.m_storedMat;
			this.m_holidayLookApplied = false;
		}
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x00010939 File Offset: 0x0000EB39
	public static bool IsHoliday(HolidayType holidayType)
	{
		if (holidayType != HolidayType.Halloween)
		{
			if (holidayType == HolidayType.Christmas)
			{
				if (TraitManager.IsInitialized && TraitManager.IsTraitActive(TraitType.ChristmasHoliday))
				{
					return true;
				}
			}
		}
		else if (TraitManager.IsInitialized && TraitManager.IsTraitActive(TraitType.HalloweenHoliday))
		{
			return true;
		}
		return holidayType == HolidayLookController.GetHolidayType();
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x000A2FE4 File Offset: 0x000A11E4
	private static HolidayType GetHolidayType()
	{
		if (!HolidayLookController.m_holidayTypeIsCached)
		{
			DateTime now = DateTime.Now;
			int dayOfYear = now.DayOfYear;
			bool flag = DateTime.IsLeapYear(now.Year);
			int num = flag ? 366 : 365;
			int num2 = 999;
			int num3 = 0;
			foreach (HolidayType holidayType in Enum.GetValues(typeof(HolidayType)) as HolidayType[])
			{
				if (holidayType != HolidayType.None)
				{
					if (holidayType != HolidayType.Halloween)
					{
						if (holidayType == HolidayType.Christmas)
						{
							num2 = 354;
							num3 = 18;
						}
					}
					else
					{
						num2 = 301;
						num3 = 7;
					}
					if (flag && dayOfYear > 58)
					{
						num2++;
					}
					int num4 = num2;
					int num5 = num2 + num3;
					bool flag2;
					if (num5 > num)
					{
						flag2 = (dayOfYear >= num4 || dayOfYear <= num5 - num);
					}
					else
					{
						flag2 = (dayOfYear >= num4 && dayOfYear <= num5);
					}
					if (flag2)
					{
						HolidayLookController.m_cachedHolidayType = holidayType;
						break;
					}
				}
			}
			HolidayLookController.m_holidayTypeIsCached = true;
		}
		return HolidayLookController.m_cachedHolidayType;
	}

	// Token: 0x04001C21 RID: 7201
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x04001C22 RID: 7202
	[SerializeField]
	private Material m_materialToApply;

	// Token: 0x04001C23 RID: 7203
	[SerializeField]
	private Renderer m_rendererToChange;

	// Token: 0x04001C24 RID: 7204
	private static bool m_holidayTypeIsCached;

	// Token: 0x04001C25 RID: 7205
	private static HolidayType m_cachedHolidayType;

	// Token: 0x04001C26 RID: 7206
	private Material m_storedMat;

	// Token: 0x04001C27 RID: 7207
	private bool m_holidayLookApplied;

	// Token: 0x04001C28 RID: 7208
	private const int LEAP_YEAR_DAY = 58;
}

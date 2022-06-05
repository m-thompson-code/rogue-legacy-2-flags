using System;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class HolidayLookController : MonoBehaviour
{
	// Token: 0x060015F4 RID: 5620 RVA: 0x000446C1 File Offset: 0x000428C1
	private void Awake()
	{
		this.m_storedMat = this.m_rendererToChange.sharedMaterial;
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x000446D4 File Offset: 0x000428D4
	private void OnEnable()
	{
		if (HolidayLookController.IsHoliday(this.m_holidayType))
		{
			this.m_rendererToChange.sharedMaterial = this.m_materialToApply;
			this.m_holidayLookApplied = true;
		}
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x000446FB File Offset: 0x000428FB
	private void OnDisable()
	{
		if (this.m_holidayLookApplied)
		{
			this.m_rendererToChange.sharedMaterial = this.m_storedMat;
			this.m_holidayLookApplied = false;
		}
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x0004471D File Offset: 0x0004291D
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

	// Token: 0x060015F8 RID: 5624 RVA: 0x0004475C File Offset: 0x0004295C
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

	// Token: 0x04001524 RID: 5412
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x04001525 RID: 5413
	[SerializeField]
	private Material m_materialToApply;

	// Token: 0x04001526 RID: 5414
	[SerializeField]
	private Renderer m_rendererToChange;

	// Token: 0x04001527 RID: 5415
	private static bool m_holidayTypeIsCached;

	// Token: 0x04001528 RID: 5416
	private static HolidayType m_cachedHolidayType;

	// Token: 0x04001529 RID: 5417
	private Material m_storedMat;

	// Token: 0x0400152A RID: 5418
	private bool m_holidayLookApplied;

	// Token: 0x0400152B RID: 5419
	private const int LEAP_YEAR_DAY = 58;
}

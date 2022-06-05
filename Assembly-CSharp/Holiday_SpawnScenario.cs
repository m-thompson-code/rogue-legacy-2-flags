using System;
using UnityEngine;

// Token: 0x0200063F RID: 1599
public class Holiday_SpawnScenario : SpawnScenario
{
	// Token: 0x17001467 RID: 5223
	// (get) Token: 0x060039CA RID: 14794 RVA: 0x000C4D8C File Offset: 0x000C2F8C
	public override string GizmoDescription
	{
		get
		{
			if (this.m_previousHolidayType != this.m_holidayType || this.m_previousIsNot != this.m_isNot)
			{
				this.m_previousHolidayType = this.m_holidayType;
				this.m_previousIsNot = this.m_isNot;
				this.m_description = string.Format("HoTy: {0}{1}", this.m_isNot ? "!" : "", this.m_holidayType.ToString());
			}
			return this.m_description;
		}
	}

	// Token: 0x17001468 RID: 5224
	// (get) Token: 0x060039CB RID: 14795 RVA: 0x000C4E08 File Offset: 0x000C3008
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.Holiday;
		}
	}

	// Token: 0x17001469 RID: 5225
	// (get) Token: 0x060039CC RID: 14796 RVA: 0x000C4E0C File Offset: 0x000C300C
	// (set) Token: 0x060039CD RID: 14797 RVA: 0x000C4E14 File Offset: 0x000C3014
	public HolidayType HolidayType
	{
		get
		{
			return this.m_holidayType;
		}
		set
		{
			this.m_holidayType = value;
		}
	}

	// Token: 0x1700146A RID: 5226
	// (get) Token: 0x060039CE RID: 14798 RVA: 0x000C4E1D File Offset: 0x000C301D
	// (set) Token: 0x060039CF RID: 14799 RVA: 0x000C4E25 File Offset: 0x000C3025
	public bool IsNot
	{
		get
		{
			return this.m_isNot;
		}
		set
		{
			this.m_isNot = value;
		}
	}

	// Token: 0x060039D0 RID: 14800 RVA: 0x000C4E2E File Offset: 0x000C302E
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.IsTrue = (HolidayLookController.IsHoliday(this.HolidayType) == !this.m_isNot);
	}

	// Token: 0x060039D1 RID: 14801 RVA: 0x000C4E4C File Offset: 0x000C304C
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.IsTrue = (HolidayLookController.IsHoliday(this.HolidayType) == !this.m_isNot);
	}

	// Token: 0x04002C71 RID: 11377
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x04002C72 RID: 11378
	[SerializeField]
	private bool m_isNot;

	// Token: 0x04002C73 RID: 11379
	private string m_description = string.Empty;

	// Token: 0x04002C74 RID: 11380
	private HolidayType m_previousHolidayType;

	// Token: 0x04002C75 RID: 11381
	private bool m_previousIsNot;
}

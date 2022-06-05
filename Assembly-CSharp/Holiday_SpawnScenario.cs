using System;
using UnityEngine;

// Token: 0x02000A6C RID: 2668
public class Holiday_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BCE RID: 7118
	// (get) Token: 0x060050A9 RID: 20649 RVA: 0x00133040 File Offset: 0x00131240
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

	// Token: 0x17001BCF RID: 7119
	// (get) Token: 0x060050AA RID: 20650 RVA: 0x00006CB3 File Offset: 0x00004EB3
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.Holiday;
		}
	}

	// Token: 0x17001BD0 RID: 7120
	// (get) Token: 0x060050AB RID: 20651 RVA: 0x0002C06B File Offset: 0x0002A26B
	// (set) Token: 0x060050AC RID: 20652 RVA: 0x0002C073 File Offset: 0x0002A273
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

	// Token: 0x17001BD1 RID: 7121
	// (get) Token: 0x060050AD RID: 20653 RVA: 0x0002C07C File Offset: 0x0002A27C
	// (set) Token: 0x060050AE RID: 20654 RVA: 0x0002C084 File Offset: 0x0002A284
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

	// Token: 0x060050AF RID: 20655 RVA: 0x0002C08D File Offset: 0x0002A28D
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.IsTrue = (HolidayLookController.IsHoliday(this.HolidayType) == !this.m_isNot);
	}

	// Token: 0x060050B0 RID: 20656 RVA: 0x0002C08D File Offset: 0x0002A28D
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.IsTrue = (HolidayLookController.IsHoliday(this.HolidayType) == !this.m_isNot);
	}

	// Token: 0x04003D03 RID: 15619
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x04003D04 RID: 15620
	[SerializeField]
	private bool m_isNot;

	// Token: 0x04003D05 RID: 15621
	private string m_description = string.Empty;

	// Token: 0x04003D06 RID: 15622
	private HolidayType m_previousHolidayType;

	// Token: 0x04003D07 RID: 15623
	private bool m_previousIsNot;
}

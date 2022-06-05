using System;

// Token: 0x020001EB RID: 491
public class Slug_Expert_AIScript : Slug_Basic_AIScript
{
	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06000D7A RID: 3450 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_verticalShot_RepeatAttackPattern
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override int m_verticalShot_TotalShotSpread
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x06000D7C RID: 3452 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float m_verticalShot_SpeedMod
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06000D7D RID: 3453 RVA: 0x00007B8D File Offset: 0x00005D8D
	protected override int m_verticalShot_SideBulletAngleOffset
	{
		get
		{
			return 25;
		}
	}
}

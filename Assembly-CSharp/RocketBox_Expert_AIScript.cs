using System;

// Token: 0x020001B1 RID: 433
public class RocketBox_Expert_AIScript : RocketBox_Basic_AIScript
{
	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00004792 File Offset: 0x00002992
	protected override int m_shoot_NumberOfBullets
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool m_shoot_VerticalBullets
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_HomingBullets
	{
		get
		{
			return true;
		}
	}
}

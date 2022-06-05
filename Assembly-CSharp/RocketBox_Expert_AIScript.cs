using System;

// Token: 0x0200010B RID: 267
public class RocketBox_Expert_AIScript : RocketBox_Basic_AIScript
{
	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x0600082F RID: 2095 RVA: 0x0001C16B File Offset: 0x0001A36B
	protected override int m_shoot_NumberOfBullets
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06000830 RID: 2096 RVA: 0x0001C16E File Offset: 0x0001A36E
	protected override bool m_shoot_VerticalBullets
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06000831 RID: 2097 RVA: 0x0001C171 File Offset: 0x0001A371
	protected override bool m_shoot_HomingBullets
	{
		get
		{
			return true;
		}
	}
}

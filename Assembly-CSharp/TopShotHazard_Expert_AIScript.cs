using System;

// Token: 0x02000234 RID: 564
public class TopShotHazard_Expert_AIScript : TopShotHazard_Basic_AIScript
{
	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_fireBullet_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x00004536 File Offset: 0x00002736
	protected override float m_fireBullet_AdditionalSpreadBullets
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_spreadShot_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0000886A File Offset: 0x00006A6A
	protected override float m_spreadShot_InitialAngle
	{
		get
		{
			return 90f;
		}
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x00003DA4 File Offset: 0x00001FA4
	protected override float m_spreadShot_SpreadAngle
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_spreadShot_AdditionalSpreadBullets
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float m_spreadShot_ShotLoop
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x000050CB File Offset: 0x000032CB
	protected override float m_spreadShot_ShotLoopDelay
	{
		get
		{
			return 0.35f;
		}
	}
}

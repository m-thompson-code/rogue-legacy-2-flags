using System;

// Token: 0x02000230 RID: 560
public class TopShotHazard_Advanced_AIScript : TopShotHazard_Basic_AIScript
{
	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_fireBullet_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float m_fireBullet_AdditionalSpreadBullets
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000757 RID: 1879
	// (get) Token: 0x06000FAA RID: 4010 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float m_fireBullet_ShotLoop
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x06000FAB RID: 4011 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float m_fireBullet_ShotLoopDelay
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00003C54 File Offset: 0x00001E54
	protected override float m_spreadShot_ShotLoop
	{
		get
		{
			return 3f;
		}
	}
}

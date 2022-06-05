using System;

// Token: 0x02000235 RID: 565
public class TopShotHazard_Miniboss_AIScript : TopShotHazard_Basic_AIScript
{
	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x00003CEB File Offset: 0x00001EEB
	protected override float SHOT_TRIGGER_WIDTH
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_snapToFloor
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x06000FEB RID: 4075 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_fireBullet_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x06000FEC RID: 4076 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_fireBullet_AdditionalSpreadBullets
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x06000FED RID: 4077 RVA: 0x0000569A File Offset: 0x0000389A
	protected override float m_fireBullet_ShotLoop
	{
		get
		{
			return 7f;
		}
	}

	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x06000FEE RID: 4078 RVA: 0x00006A26 File Offset: 0x00004C26
	protected override float m_fireBullet_ShotLoopDelay
	{
		get
		{
			return 0.225f;
		}
	}
}

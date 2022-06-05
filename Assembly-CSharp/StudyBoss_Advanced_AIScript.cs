using System;

// Token: 0x02000215 RID: 533
public class StudyBoss_Advanced_AIScript : StudyBoss_Basic_AIScript
{
	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_advancedAttack
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06000E9F RID: 3743 RVA: 0x000047A4 File Offset: 0x000029A4
	protected override int m_verticalBeamAttackCount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00003E63 File Offset: 0x00002063
	protected override float m_verticalBeamLifetime
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x000047A7 File Offset: 0x000029A7
	protected override int m_dashAttackFireballCountAtEnd
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x000046FA File Offset: 0x000028FA
	protected override int m_shieldAttackProjectileCount
	{
		get
		{
			return 10;
		}
	}
}

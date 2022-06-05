using System;

// Token: 0x020001FA RID: 506
public class SpearKnight_Expert_AIScript : SpearKnight_Basic_AIScript
{
	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06000DEE RID: 3566 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float DashUppercut_JumpSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_throw_Attack_ProjectileAmount
	{
		get
		{
			return 2;
		}
	}
}

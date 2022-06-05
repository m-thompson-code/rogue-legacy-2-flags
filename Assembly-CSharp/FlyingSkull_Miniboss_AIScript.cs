using System;

// Token: 0x020000E8 RID: 232
public class FlyingSkull_Miniboss_AIScript : FlyingSkull_Basic_AIScript
{
	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001A149 File Offset: 0x00018349
	protected override bool m_shoot_ShootNear
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06000719 RID: 1817 RVA: 0x0001A14C File Offset: 0x0001834C
	protected override bool m_shoot_ShootFar
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001A14F File Offset: 0x0001834F
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x0600071B RID: 1819 RVA: 0x0001A152 File Offset: 0x00018352
	protected override bool m_shoot_ShootMirror
	{
		get
		{
			return true;
		}
	}
}

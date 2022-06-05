using System;

// Token: 0x02000170 RID: 368
public class FlyingShield_Miniboss_AIScript : FlyingShield_Basic_AIScript
{
	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x060009F9 RID: 2553 RVA: 0x000068DA File Offset: 0x00004ADA
	protected override float m_spinMove_Attack_MoveSpeed
	{
		get
		{
			return 27f;
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x060009FA RID: 2554 RVA: 0x0000611B File Offset: 0x0000431B
	protected override float m_spinMove_Attack_Duration
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_hasTailSpin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_hasTailRam
	{
		get
		{
			return true;
		}
	}
}

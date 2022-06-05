using System;

// Token: 0x0200016F RID: 367
public class FlyingShield_Expert_AIScript : FlyingShield_Basic_AIScript
{
	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x060009F4 RID: 2548 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_hasTailSpin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_hasTailRam
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x060009F6 RID: 2550 RVA: 0x000068D3 File Offset: 0x00004AD3
	protected override float m_spinMove_Attack_MoveSpeed
	{
		get
		{
			return 24f;
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x060009F7 RID: 2551 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_spinMove_Attack_FlameDrops
	{
		get
		{
			return 0f;
		}
	}
}

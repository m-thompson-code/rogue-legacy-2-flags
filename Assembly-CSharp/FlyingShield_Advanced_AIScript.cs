using System;

// Token: 0x0200016B RID: 363
public class FlyingShield_Advanced_AIScript : FlyingShield_Basic_AIScript
{
	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_hasTailSpin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_hasTailRam
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float m_spinMove_Attack_FlameDrops
	{
		get
		{
			return 0f;
		}
	}
}

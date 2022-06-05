using System;

// Token: 0x020000E3 RID: 227
public class FlyingShield_Expert_AIScript : FlyingShield_Basic_AIScript
{
	// Token: 0x1700038C RID: 908
	// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00019EB7 File Offset: 0x000180B7
	protected override bool m_hasTailSpin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00019EBA File Offset: 0x000180BA
	protected override bool m_hasTailRam
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00019EBD File Offset: 0x000180BD
	protected override float m_spinMove_Attack_MoveSpeed
	{
		get
		{
			return 24f;
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x060006DA RID: 1754 RVA: 0x00019EC4 File Offset: 0x000180C4
	protected override float m_spinMove_Attack_FlameDrops
	{
		get
		{
			return 0f;
		}
	}
}

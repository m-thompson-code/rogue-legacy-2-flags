using System;

// Token: 0x020000D2 RID: 210
public class FlyingAxe_Expert_AIScript : FlyingAxe_Basic_AIScript
{
	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06000613 RID: 1555 RVA: 0x00018E04 File Offset: 0x00017004
	protected override float m_sideSpin_Attack_TurnSpeed
	{
		get
		{
			return 105f;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06000614 RID: 1556 RVA: 0x00018E0B File Offset: 0x0001700B
	protected override float m_sideSpin_Attack_MovementSpeed
	{
		get
		{
			return 12f;
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06000615 RID: 1557 RVA: 0x00018E12 File Offset: 0x00017012
	protected override float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 2.5f;
		}
	}
}

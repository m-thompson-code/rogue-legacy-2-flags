using System;

// Token: 0x02000139 RID: 313
public class SwordKnight_Advanced_AIScript : SwordKnight_Basic_AIScript
{
	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x060009DF RID: 2527 RVA: 0x0001FC64 File Offset: 0x0001DE64
	protected override float m_slash_Attack_Speed
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0001FC6B File Offset: 0x0001DE6B
	protected override float m_slash_Attack_Duration
	{
		get
		{
			return 0.175f;
		}
	}
}

using System;

// Token: 0x02000225 RID: 549
public class SwordKnight_Advanced_AIScript : SwordKnight_Basic_AIScript
{
	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06000F3C RID: 3900 RVA: 0x000052B0 File Offset: 0x000034B0
	protected override float m_slash_Attack_Speed
	{
		get
		{
			return 18f;
		}
	}

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06000F3D RID: 3901 RVA: 0x00003FB0 File Offset: 0x000021B0
	protected override float m_slash_Attack_Duration
	{
		get
		{
			return 0.175f;
		}
	}
}

using System;

// Token: 0x0200022A RID: 554
public class SwordKnight_Expert_AIScript : SwordKnight_Basic_AIScript
{
	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06000F65 RID: 3941 RVA: 0x000068D3 File Offset: 0x00004AD3
	protected override float m_slash_Attack_Speed
	{
		get
		{
			return 24f;
		}
	}

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06000F66 RID: 3942 RVA: 0x00003FB0 File Offset: 0x000021B0
	protected override float m_slash_Attack_Duration
	{
		get
		{
			return 0.175f;
		}
	}

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x06000F67 RID: 3943 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_cricket_Attack_ProjectileAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06000F68 RID: 3944 RVA: 0x000086AA File Offset: 0x000068AA
	protected override float m_cricket_Attack_ProjectileDelay
	{
		get
		{
			return 0.275f;
		}
	}
}

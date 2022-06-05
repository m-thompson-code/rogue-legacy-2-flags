using System;

// Token: 0x02000153 RID: 339
public class Zombie_Advanced_AIScript : Zombie_Basic_AIScript
{
	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x06000B71 RID: 2929 RVA: 0x00022C07 File Offset: 0x00020E07
	protected override float m_tunnel_moveSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00022C0E File Offset: 0x00020E0E
	protected override float Lunge_Count
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06000B73 RID: 2931 RVA: 0x00022C15 File Offset: 0x00020E15
	protected override float Delay_Between_Lunges
	{
		get
		{
			return 0.15f;
		}
	}
}

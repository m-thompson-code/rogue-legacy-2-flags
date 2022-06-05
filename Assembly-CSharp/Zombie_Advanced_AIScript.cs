using System;

// Token: 0x0200026D RID: 621
public class Zombie_Advanced_AIScript : Zombie_Basic_AIScript
{
	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x060011DC RID: 4572 RVA: 0x0000611B File Offset: 0x0000431B
	protected override float m_tunnel_moveSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x060011DD RID: 4573 RVA: 0x00004536 File Offset: 0x00002736
	protected override float Lunge_Count
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x060011DE RID: 4574 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float Delay_Between_Lunges
	{
		get
		{
			return 0.15f;
		}
	}
}

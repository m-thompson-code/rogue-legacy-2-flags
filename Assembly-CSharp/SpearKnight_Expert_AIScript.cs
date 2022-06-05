using System;

// Token: 0x02000127 RID: 295
public class SpearKnight_Expert_AIScript : SpearKnight_Basic_AIScript
{
	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06000927 RID: 2343 RVA: 0x0001DF69 File Offset: 0x0001C169
	protected override float DashUppercut_JumpSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06000928 RID: 2344 RVA: 0x0001DF70 File Offset: 0x0001C170
	protected override int m_throw_Attack_ProjectileAmount
	{
		get
		{
			return 2;
		}
	}
}

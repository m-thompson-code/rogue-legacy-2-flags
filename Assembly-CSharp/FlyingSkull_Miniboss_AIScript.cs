using System;

// Token: 0x02000177 RID: 375
public class FlyingSkull_Miniboss_AIScript : FlyingSkull_Basic_AIScript
{
	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ShootNear
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ShootFar
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ShootMirror
	{
		get
		{
			return true;
		}
	}
}

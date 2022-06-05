using System;

// Token: 0x02000176 RID: 374
public class FlyingSkull_Expert_AIScript : FlyingSkull_Basic_AIScript
{
	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06000A42 RID: 2626 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ShootNear
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06000A43 RID: 2627 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ShootFar
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06000A45 RID: 2629 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ShootMirror
	{
		get
		{
			return true;
		}
	}
}

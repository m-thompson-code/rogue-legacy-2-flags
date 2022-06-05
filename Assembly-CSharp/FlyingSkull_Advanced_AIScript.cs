using System;

// Token: 0x02000171 RID: 369
public class FlyingSkull_Advanced_AIScript : FlyingSkull_Basic_AIScript
{
	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ShootNear
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ShootFar
	{
		get
		{
			return true;
		}
	}
}

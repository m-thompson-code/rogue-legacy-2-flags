using System;

// Token: 0x020000E7 RID: 231
public class FlyingSkull_Expert_AIScript : FlyingSkull_Basic_AIScript
{
	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001A135 File Offset: 0x00018335
	protected override bool m_shoot_ShootNear
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06000714 RID: 1812 RVA: 0x0001A138 File Offset: 0x00018338
	protected override bool m_shoot_ShootFar
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06000715 RID: 1813 RVA: 0x0001A13B File Offset: 0x0001833B
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06000716 RID: 1814 RVA: 0x0001A13E File Offset: 0x0001833E
	protected override bool m_shoot_ShootMirror
	{
		get
		{
			return true;
		}
	}
}

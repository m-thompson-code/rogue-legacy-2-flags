using System;

// Token: 0x020000B1 RID: 177
public class ElementalDash_Expert_AIScript : ElementalDash_Basic_AIScript
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600042A RID: 1066 RVA: 0x00015944 File Offset: 0x00013B44
	protected override int m_shoot_TotalShots
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x0600042B RID: 1067 RVA: 0x00015947 File Offset: 0x00013B47
	protected override float m_shoot_TotalShotDuration
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x0600042C RID: 1068 RVA: 0x0001594E File Offset: 0x00013B4E
	protected override bool m_voidWall_CreateSideWalls
	{
		get
		{
			return true;
		}
	}
}

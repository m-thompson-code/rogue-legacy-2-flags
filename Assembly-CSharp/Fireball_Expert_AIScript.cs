using System;

// Token: 0x020000CA RID: 202
public class Fireball_Expert_AIScript : Fireball_Basic_AIScript
{
	// Token: 0x170002CB RID: 715
	// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001861F File Offset: 0x0001681F
	protected override bool m_dropsFireballsWhileWalking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00018622 File Offset: 0x00016822
	protected override bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00018625 File Offset: 0x00016825
	protected override float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 1.5f;
		}
	}
}

using System;

// Token: 0x020000CB RID: 203
public class Fireball_Miniboss_AIScript : Fireball_Basic_AIScript
{
	// Token: 0x170002CE RID: 718
	// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00018634 File Offset: 0x00016834
	protected override bool m_dropsFireballsWhileWalking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00018637 File Offset: 0x00016837
	protected override bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001863A File Offset: 0x0001683A
	protected override float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x060005CB RID: 1483 RVA: 0x00018641 File Offset: 0x00016841
	protected override float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0.5f;
		}
	}
}

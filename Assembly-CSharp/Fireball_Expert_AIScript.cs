using System;

// Token: 0x02000143 RID: 323
public class Fireball_Expert_AIScript : Fireball_Basic_AIScript
{
	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06000875 RID: 2165 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_dropsFireballsWhileWalking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06000876 RID: 2166 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06000877 RID: 2167 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 1.5f;
		}
	}
}

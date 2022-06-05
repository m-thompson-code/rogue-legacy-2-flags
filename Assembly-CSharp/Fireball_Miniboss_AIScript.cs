using System;

// Token: 0x02000144 RID: 324
public class Fireball_Miniboss_AIScript : Fireball_Basic_AIScript
{
	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_dropsFireballsWhileWalking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x0600087B RID: 2171 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x0600087C RID: 2172 RVA: 0x0000457A File Offset: 0x0000277A
	protected override float m_dash_Exit_ForceIdle
	{
		get
		{
			return 0.5f;
		}
	}
}

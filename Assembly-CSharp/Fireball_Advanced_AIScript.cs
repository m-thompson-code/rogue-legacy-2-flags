using System;

// Token: 0x0200013E RID: 318
public class Fireball_Advanced_AIScript : Fireball_Basic_AIScript
{
	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06000838 RID: 2104 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_dropsFireballsWhileWalking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06000839 RID: 2105 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float m_timeBetweenWalkTowardFireballDrops
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x0600083A RID: 2106 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool m_dropsFireballsDuringDashAttack
	{
		get
		{
			return false;
		}
	}
}

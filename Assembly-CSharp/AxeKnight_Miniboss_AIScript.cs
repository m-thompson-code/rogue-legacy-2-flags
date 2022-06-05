using System;

// Token: 0x02000092 RID: 146
public class AxeKnight_Miniboss_AIScript : AxeKnight_Basic_AIScript
{
	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600021C RID: 540 RVA: 0x00012527 File Offset: 0x00010727
	protected override bool m_throwSecondAxe
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600021D RID: 541 RVA: 0x0001252A File Offset: 0x0001072A
	protected override bool m_jump_spawnAxeOnLand
	{
		get
		{
			return true;
		}
	}
}

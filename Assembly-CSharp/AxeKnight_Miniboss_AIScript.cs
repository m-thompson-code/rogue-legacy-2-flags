using System;

// Token: 0x020000A3 RID: 163
public class AxeKnight_Miniboss_AIScript : AxeKnight_Basic_AIScript
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000266 RID: 614 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_throwSecondAxe
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000267 RID: 615 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_jump_spawnAxeOnLand
	{
		get
		{
			return true;
		}
	}
}

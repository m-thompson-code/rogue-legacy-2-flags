using System;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class MimicChestBoss_Miniboss_AIScript : MimicChestBoss_Basic_AIScript
{
	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00006E96 File Offset: 0x00005096
	protected override float m_dashAttackSpeed
	{
		get
		{
			return 25f;
		}
	}

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00006E9D File Offset: 0x0000509D
	protected override Vector2 m_dashAttackDuration
	{
		get
		{
			return new Vector2(2f, 2f);
		}
	}

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00006EAE File Offset: 0x000050AE
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(17f, 20f);
		}
	}
}

using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class MimicChestBoss_Miniboss_AIScript : MimicChestBoss_Basic_AIScript
{
	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x0600079B RID: 1947 RVA: 0x0001AA5F File Offset: 0x00018C5F
	protected override float m_dashAttackSpeed
	{
		get
		{
			return 25f;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x0600079C RID: 1948 RVA: 0x0001AA66 File Offset: 0x00018C66
	protected override Vector2 m_dashAttackDuration
	{
		get
		{
			return new Vector2(2f, 2f);
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x0600079D RID: 1949 RVA: 0x0001AA77 File Offset: 0x00018C77
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(17f, 20f);
		}
	}
}

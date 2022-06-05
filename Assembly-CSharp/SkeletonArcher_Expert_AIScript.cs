using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class SkeletonArcher_Expert_AIScript : SkeletonArcher_Basic_AIScript
{
	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_shoot_ExtraArrowCount
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00007629 File Offset: 0x00005829
	protected override Vector2 m_jump_Power
	{
		get
		{
			return new Vector2(10f, 32f);
		}
	}
}

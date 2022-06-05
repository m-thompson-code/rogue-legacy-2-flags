using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class SkeletonArcher_Expert_AIScript : SkeletonArcher_Basic_AIScript
{
	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x0001C5C6 File Offset: 0x0001A7C6
	protected override bool m_shoot_ExtraArrowCount
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x0001C5C9 File Offset: 0x0001A7C9
	protected override Vector2 m_jump_Power
	{
		get
		{
			return new Vector2(10f, 32f);
		}
	}
}

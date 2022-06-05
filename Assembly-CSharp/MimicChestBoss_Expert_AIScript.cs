using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class MimicChestBoss_Expert_AIScript : MimicChestBoss_Basic_AIScript
{
	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00006E85 File Offset: 0x00005085
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(17.5f, 17f);
		}
	}
}

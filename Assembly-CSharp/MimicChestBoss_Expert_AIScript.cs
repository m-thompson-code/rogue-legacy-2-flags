using System;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class MimicChestBoss_Expert_AIScript : MimicChestBoss_Basic_AIScript
{
	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06000799 RID: 1945 RVA: 0x0001AA46 File Offset: 0x00018C46
	protected override Vector2 JumpHeight
	{
		get
		{
			return new Vector2(17.5f, 17f);
		}
	}
}

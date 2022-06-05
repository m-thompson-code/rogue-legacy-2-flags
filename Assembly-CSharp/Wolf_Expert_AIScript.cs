using System;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class Wolf_Expert_AIScript : Wolf_Basic_AIScript
{
	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00022AD9 File Offset: 0x00020CD9
	protected override Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(18f, 26f);
		}
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00022AEA File Offset: 0x00020CEA
	protected override Vector2 m_dash_AttackHold_ForwardPower
	{
		get
		{
			return new Vector2(34f, 12f);
		}
	}
}

using System;
using UnityEngine;

// Token: 0x02000262 RID: 610
public class Wolf_Advanced_AIScript : Wolf_Basic_AIScript
{
	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x06001184 RID: 4484 RVA: 0x00009180 File Offset: 0x00007380
	protected override Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(18f, 26f);
		}
	}

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x06001185 RID: 4485 RVA: 0x00009191 File Offset: 0x00007391
	protected override Vector2 m_dash_AttackHold_ForwardPower
	{
		get
		{
			return new Vector2(32f, 12f);
		}
	}
}

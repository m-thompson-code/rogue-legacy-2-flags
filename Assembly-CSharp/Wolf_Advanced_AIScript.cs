using System;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class Wolf_Advanced_AIScript : Wolf_Basic_AIScript
{
	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0002283D File Offset: 0x00020A3D
	protected override Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(18f, 26f);
		}
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0002284E File Offset: 0x00020A4E
	protected override Vector2 m_dash_AttackHold_ForwardPower
	{
		get
		{
			return new Vector2(32f, 12f);
		}
	}
}

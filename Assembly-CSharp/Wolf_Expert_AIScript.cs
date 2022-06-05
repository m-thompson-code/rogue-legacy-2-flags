using System;
using UnityEngine;

// Token: 0x0200026A RID: 618
public class Wolf_Expert_AIScript : Wolf_Basic_AIScript
{
	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x060011C9 RID: 4553 RVA: 0x00009180 File Offset: 0x00007380
	protected override Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(18f, 26f);
		}
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x060011CA RID: 4554 RVA: 0x0000931E File Offset: 0x0000751E
	protected override Vector2 m_dash_AttackHold_ForwardPower
	{
		get
		{
			return new Vector2(34f, 12f);
		}
	}
}

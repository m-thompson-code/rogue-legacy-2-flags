using System;
using UnityEngine;

// Token: 0x02000603 RID: 1539
[Serializable]
public class DecoEntry
{
	// Token: 0x170013CE RID: 5070
	// (get) Token: 0x060037E6 RID: 14310 RVA: 0x000BF479 File Offset: 0x000BD679
	// (set) Token: 0x060037E7 RID: 14311 RVA: 0x000BF481 File Offset: 0x000BD681
	public bool CanFlip
	{
		get
		{
			return this.m_canFlip;
		}
		set
		{
			this.m_canFlip = value;
		}
	}

	// Token: 0x170013CF RID: 5071
	// (get) Token: 0x060037E8 RID: 14312 RVA: 0x000BF48A File Offset: 0x000BD68A
	// (set) Token: 0x060037E9 RID: 14313 RVA: 0x000BF492 File Offset: 0x000BD692
	public Deco Deco
	{
		get
		{
			return this.m_deco;
		}
		set
		{
			this.m_deco = value;
		}
	}

	// Token: 0x04002ACC RID: 10956
	[SerializeField]
	private Deco m_deco;

	// Token: 0x04002ACD RID: 10957
	[SerializeField]
	private bool m_canFlip = true;
}

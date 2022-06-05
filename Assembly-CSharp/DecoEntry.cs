using System;
using UnityEngine;

// Token: 0x02000A24 RID: 2596
[Serializable]
public class DecoEntry
{
	// Token: 0x17001B25 RID: 6949
	// (get) Token: 0x06004E83 RID: 20099 RVA: 0x0002ABB8 File Offset: 0x00028DB8
	// (set) Token: 0x06004E84 RID: 20100 RVA: 0x0002ABC0 File Offset: 0x00028DC0
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

	// Token: 0x17001B26 RID: 6950
	// (get) Token: 0x06004E85 RID: 20101 RVA: 0x0002ABC9 File Offset: 0x00028DC9
	// (set) Token: 0x06004E86 RID: 20102 RVA: 0x0002ABD1 File Offset: 0x00028DD1
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

	// Token: 0x04003B2F RID: 15151
	[SerializeField]
	private Deco m_deco;

	// Token: 0x04003B30 RID: 15152
	[SerializeField]
	private bool m_canFlip = true;
}

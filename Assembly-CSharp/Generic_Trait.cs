using System;
using UnityEngine;

// Token: 0x020005A6 RID: 1446
public class Generic_Trait : BaseTrait
{
	// Token: 0x17001215 RID: 4629
	// (get) Token: 0x06002D5E RID: 11614 RVA: 0x00019080 File Offset: 0x00017280
	public override TraitType TraitType
	{
		get
		{
			return this.m_traitType;
		}
	}

	// Token: 0x040025BD RID: 9661
	[SerializeField]
	private TraitType m_traitType;
}

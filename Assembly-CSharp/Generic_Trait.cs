using System;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class Generic_Trait : BaseTrait
{
	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x0600202B RID: 8235 RVA: 0x000664A5 File Offset: 0x000646A5
	public override TraitType TraitType
	{
		get
		{
			return this.m_traitType;
		}
	}

	// Token: 0x04001C52 RID: 7250
	[SerializeField]
	private TraitType m_traitType;
}

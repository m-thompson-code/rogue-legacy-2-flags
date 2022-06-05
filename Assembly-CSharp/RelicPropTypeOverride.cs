using System;
using UnityEngine;

// Token: 0x020004DE RID: 1246
public class RelicPropTypeOverride : MonoBehaviour
{
	// Token: 0x17001186 RID: 4486
	// (get) Token: 0x06002E9D RID: 11933 RVA: 0x0009E65E File Offset: 0x0009C85E
	public RelicType Relic1Override
	{
		get
		{
			return this.m_relic1Override;
		}
	}

	// Token: 0x17001187 RID: 4487
	// (get) Token: 0x06002E9E RID: 11934 RVA: 0x0009E666 File Offset: 0x0009C866
	public RelicType Relic2Override
	{
		get
		{
			return this.m_relic2Override;
		}
	}

	// Token: 0x0400252F RID: 9519
	[SerializeField]
	private RelicType m_relic1Override;

	// Token: 0x04002530 RID: 9520
	[SerializeField]
	private RelicType m_relic2Override;
}

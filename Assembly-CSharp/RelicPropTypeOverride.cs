using System;
using UnityEngine;

// Token: 0x02000823 RID: 2083
public class RelicPropTypeOverride : MonoBehaviour
{
	// Token: 0x17001741 RID: 5953
	// (get) Token: 0x06004045 RID: 16453 RVA: 0x000237AE File Offset: 0x000219AE
	public RelicType Relic1Override
	{
		get
		{
			return this.m_relic1Override;
		}
	}

	// Token: 0x17001742 RID: 5954
	// (get) Token: 0x06004046 RID: 16454 RVA: 0x000237B6 File Offset: 0x000219B6
	public RelicType Relic2Override
	{
		get
		{
			return this.m_relic2Override;
		}
	}

	// Token: 0x0400324A RID: 12874
	[SerializeField]
	private RelicType m_relic1Override;

	// Token: 0x0400324B RID: 12875
	[SerializeField]
	private RelicType m_relic2Override;
}

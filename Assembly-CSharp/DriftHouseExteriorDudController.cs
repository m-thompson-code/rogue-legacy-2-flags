using System;
using UnityEngine;

// Token: 0x02000904 RID: 2308
public class DriftHouseExteriorDudController : BaseSpecialPropController
{
	// Token: 0x0600461D RID: 17949 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void InitializePooledPropOnEnter()
	{
	}

	// Token: 0x04003622 RID: 13858
	[SerializeField]
	private GameObject m_closedShack;

	// Token: 0x04003623 RID: 13859
	[SerializeField]
	private GameObject m_openShack;
}

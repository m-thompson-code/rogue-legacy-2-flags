using System;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class DriftHouseExteriorDudController : BaseSpecialPropController
{
	// Token: 0x060031EB RID: 12779 RVA: 0x000A8D73 File Offset: 0x000A6F73
	protected override void InitializePooledPropOnEnter()
	{
	}

	// Token: 0x0400274D RID: 10061
	[SerializeField]
	private GameObject m_closedShack;

	// Token: 0x0400274E RID: 10062
	[SerializeField]
	private GameObject m_openShack;
}

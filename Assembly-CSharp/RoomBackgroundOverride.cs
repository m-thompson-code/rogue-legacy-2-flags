using System;
using UnityEngine;

// Token: 0x02000A5C RID: 2652
public class RoomBackgroundOverride : MonoBehaviour
{
	// Token: 0x17001BA1 RID: 7073
	// (get) Token: 0x0600502C RID: 20524 RVA: 0x0002BCBD File Offset: 0x00029EBD
	public Background BackgroundOverride
	{
		get
		{
			return this.m_backgroundOverride;
		}
	}

	// Token: 0x17001BA2 RID: 7074
	// (get) Token: 0x0600502D RID: 20525 RVA: 0x0002BCC5 File Offset: 0x00029EC5
	public bool IsTiled
	{
		get
		{
			return this.m_isTiled;
		}
	}

	// Token: 0x04003CB1 RID: 15537
	[SerializeField]
	private Background m_backgroundOverride;

	// Token: 0x04003CB2 RID: 15538
	[SerializeField]
	private bool m_isTiled;
}

using System;
using UnityEngine;

// Token: 0x02000875 RID: 2165
public class MapRoomEntryIconOverrideController : MonoBehaviour
{
	// Token: 0x170017D4 RID: 6100
	// (get) Token: 0x060042A2 RID: 17058 RVA: 0x00024DB2 File Offset: 0x00022FB2
	public bool ShowRoomIconOnMap
	{
		get
		{
			return this.m_showRoomIconOnMap;
		}
	}

	// Token: 0x04003412 RID: 13330
	[SerializeField]
	private bool m_showRoomIconOnMap;
}

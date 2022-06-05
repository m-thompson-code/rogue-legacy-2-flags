using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
public class MapRoomEntryIconOverrideController : MonoBehaviour
{
	// Token: 0x170011CB RID: 4555
	// (get) Token: 0x0600300A RID: 12298 RVA: 0x000A47CC File Offset: 0x000A29CC
	public bool ShowRoomIconOnMap
	{
		get
		{
			return this.m_showRoomIconOnMap;
		}
	}

	// Token: 0x04002642 RID: 9794
	[SerializeField]
	private bool m_showRoomIconOnMap;
}

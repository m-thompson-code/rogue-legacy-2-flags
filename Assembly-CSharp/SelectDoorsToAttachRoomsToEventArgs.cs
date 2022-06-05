using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000797 RID: 1943
public class SelectDoorsToAttachRoomsToEventArgs : EventArgs
{
	// Token: 0x0600419E RID: 16798 RVA: 0x000E9B33 File Offset: 0x000E7D33
	public SelectDoorsToAttachRoomsToEventArgs(BaseRoom room, Dictionary<RoomSide, List<int>> doorTable)
	{
		this.Room = room;
		this.DoorTable = doorTable;
		this.Time = UnityEngine.Time.time;
	}

	// Token: 0x1700165C RID: 5724
	// (get) Token: 0x0600419F RID: 16799 RVA: 0x000E9B54 File Offset: 0x000E7D54
	// (set) Token: 0x060041A0 RID: 16800 RVA: 0x000E9B5C File Offset: 0x000E7D5C
	public BaseRoom Room { get; protected set; }

	// Token: 0x1700165D RID: 5725
	// (get) Token: 0x060041A1 RID: 16801 RVA: 0x000E9B65 File Offset: 0x000E7D65
	// (set) Token: 0x060041A2 RID: 16802 RVA: 0x000E9B6D File Offset: 0x000E7D6D
	public Dictionary<RoomSide, List<int>> DoorTable { get; private set; }

	// Token: 0x1700165E RID: 5726
	// (get) Token: 0x060041A3 RID: 16803 RVA: 0x000E9B76 File Offset: 0x000E7D76
	// (set) Token: 0x060041A4 RID: 16804 RVA: 0x000E9B7E File Offset: 0x000E7D7E
	public float Time { get; protected set; }
}

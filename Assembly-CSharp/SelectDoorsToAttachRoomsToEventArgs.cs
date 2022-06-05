using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C5A RID: 3162
public class SelectDoorsToAttachRoomsToEventArgs : EventArgs
{
	// Token: 0x06005B1B RID: 23323 RVA: 0x00032009 File Offset: 0x00030209
	public SelectDoorsToAttachRoomsToEventArgs(BaseRoom room, Dictionary<RoomSide, List<int>> doorTable)
	{
		this.Room = room;
		this.DoorTable = doorTable;
		this.Time = UnityEngine.Time.time;
	}

	// Token: 0x17001E58 RID: 7768
	// (get) Token: 0x06005B1C RID: 23324 RVA: 0x0003202A File Offset: 0x0003022A
	// (set) Token: 0x06005B1D RID: 23325 RVA: 0x00032032 File Offset: 0x00030232
	public BaseRoom Room { get; protected set; }

	// Token: 0x17001E59 RID: 7769
	// (get) Token: 0x06005B1E RID: 23326 RVA: 0x0003203B File Offset: 0x0003023B
	// (set) Token: 0x06005B1F RID: 23327 RVA: 0x00032043 File Offset: 0x00030243
	public Dictionary<RoomSide, List<int>> DoorTable { get; private set; }

	// Token: 0x17001E5A RID: 7770
	// (get) Token: 0x06005B20 RID: 23328 RVA: 0x0003204C File Offset: 0x0003024C
	// (set) Token: 0x06005B21 RID: 23329 RVA: 0x00032054 File Offset: 0x00030254
	public float Time { get; protected set; }
}

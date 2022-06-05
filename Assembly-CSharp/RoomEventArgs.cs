using System;
using UnityEngine;

// Token: 0x02000C59 RID: 3161
public class RoomEventArgs : EventArgs
{
	// Token: 0x17001E56 RID: 7766
	// (get) Token: 0x06005B15 RID: 23317 RVA: 0x00031FC4 File Offset: 0x000301C4
	// (set) Token: 0x06005B16 RID: 23318 RVA: 0x00031FCC File Offset: 0x000301CC
	public BaseRoom Room { get; protected set; }

	// Token: 0x17001E57 RID: 7767
	// (get) Token: 0x06005B17 RID: 23319 RVA: 0x00031FD5 File Offset: 0x000301D5
	// (set) Token: 0x06005B18 RID: 23320 RVA: 0x00031FDD File Offset: 0x000301DD
	public float Time { get; protected set; }

	// Token: 0x06005B19 RID: 23321 RVA: 0x00031FE6 File Offset: 0x000301E6
	public RoomEventArgs(BaseRoom room)
	{
		this.Initialize(room);
	}

	// Token: 0x06005B1A RID: 23322 RVA: 0x00031FF5 File Offset: 0x000301F5
	public void Initialize(BaseRoom room)
	{
		this.Room = room;
		this.Time = UnityEngine.Time.time;
	}
}

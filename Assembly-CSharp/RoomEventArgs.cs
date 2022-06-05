using System;
using UnityEngine;

// Token: 0x02000796 RID: 1942
public class RoomEventArgs : EventArgs
{
	// Token: 0x1700165A RID: 5722
	// (get) Token: 0x06004198 RID: 16792 RVA: 0x000E9AEE File Offset: 0x000E7CEE
	// (set) Token: 0x06004199 RID: 16793 RVA: 0x000E9AF6 File Offset: 0x000E7CF6
	public BaseRoom Room { get; protected set; }

	// Token: 0x1700165B RID: 5723
	// (get) Token: 0x0600419A RID: 16794 RVA: 0x000E9AFF File Offset: 0x000E7CFF
	// (set) Token: 0x0600419B RID: 16795 RVA: 0x000E9B07 File Offset: 0x000E7D07
	public float Time { get; protected set; }

	// Token: 0x0600419C RID: 16796 RVA: 0x000E9B10 File Offset: 0x000E7D10
	public RoomEventArgs(BaseRoom room)
	{
		this.Initialize(room);
	}

	// Token: 0x0600419D RID: 16797 RVA: 0x000E9B1F File Offset: 0x000E7D1F
	public void Initialize(BaseRoom room)
	{
		this.Room = room;
		this.Time = UnityEngine.Time.time;
	}
}

using System;

// Token: 0x02000790 RID: 1936
public class RoomMergeEventArgs : EventArgs
{
	// Token: 0x0600417B RID: 16763 RVA: 0x000E99A8 File Offset: 0x000E7BA8
	public RoomMergeEventArgs(Room newRoom, Room roomA, Room roomB, RoomSide atSideOfRoomA)
	{
		this.NewRoom = newRoom;
		this.RoomA = roomA;
		this.RoomB = roomB;
		this.AtSideOfA = atSideOfRoomA;
	}

	// Token: 0x1700164F RID: 5711
	// (get) Token: 0x0600417C RID: 16764 RVA: 0x000E99CD File Offset: 0x000E7BCD
	// (set) Token: 0x0600417D RID: 16765 RVA: 0x000E99D5 File Offset: 0x000E7BD5
	public Room NewRoom { get; private set; }

	// Token: 0x17001650 RID: 5712
	// (get) Token: 0x0600417E RID: 16766 RVA: 0x000E99DE File Offset: 0x000E7BDE
	// (set) Token: 0x0600417F RID: 16767 RVA: 0x000E99E6 File Offset: 0x000E7BE6
	public Room RoomA { get; private set; }

	// Token: 0x17001651 RID: 5713
	// (get) Token: 0x06004180 RID: 16768 RVA: 0x000E99EF File Offset: 0x000E7BEF
	// (set) Token: 0x06004181 RID: 16769 RVA: 0x000E99F7 File Offset: 0x000E7BF7
	public Room RoomB { get; private set; }

	// Token: 0x17001652 RID: 5714
	// (get) Token: 0x06004182 RID: 16770 RVA: 0x000E9A00 File Offset: 0x000E7C00
	// (set) Token: 0x06004183 RID: 16771 RVA: 0x000E9A08 File Offset: 0x000E7C08
	public RoomSide AtSideOfA { get; private set; }
}

using System;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
[Serializable]
public struct RoomWeightEntry
{
	// Token: 0x17001CB0 RID: 7344
	// (get) Token: 0x0600547D RID: 21629 RVA: 0x0002DC8C File Offset: 0x0002BE8C
	public RoomID RoomID
	{
		get
		{
			return this.m_roomId;
		}
	}

	// Token: 0x17001CB1 RID: 7345
	// (get) Token: 0x0600547E RID: 21630 RVA: 0x0002DC94 File Offset: 0x0002BE94
	public int RNGWeight
	{
		get
		{
			return this.m_weight;
		}
	}

	// Token: 0x17001CB2 RID: 7346
	// (get) Token: 0x0600547F RID: 21631 RVA: 0x0002DC9C File Offset: 0x0002BE9C
	public bool IsMirrored
	{
		get
		{
			return this.m_isMirrored;
		}
	}

	// Token: 0x06005480 RID: 21632 RVA: 0x0002DCA4 File Offset: 0x0002BEA4
	public RoomWeightEntry(RoomID roomID, bool isMirrored)
	{
		this = new RoomWeightEntry(roomID, isMirrored, 100);
	}

	// Token: 0x06005481 RID: 21633 RVA: 0x0002DCB0 File Offset: 0x0002BEB0
	public RoomWeightEntry(RoomID roomID, bool isMirrored, int weight)
	{
		this.m_roomId = roomID;
		this.m_weight = weight;
		this.m_isMirrored = isMirrored;
	}

	// Token: 0x06005482 RID: 21634 RVA: 0x0002DCC7 File Offset: 0x0002BEC7
	public void SetWeight(int weight)
	{
		this.m_weight = weight;
	}

	// Token: 0x04003EFD RID: 16125
	[SerializeField]
	private RoomID m_roomId;

	// Token: 0x04003EFE RID: 16126
	[SerializeField]
	private int m_weight;

	// Token: 0x04003EFF RID: 16127
	[SerializeField]
	private bool m_isMirrored;

	// Token: 0x04003F00 RID: 16128
	public const int DEFAULT_ROOM_WEIGHT = 100;
}

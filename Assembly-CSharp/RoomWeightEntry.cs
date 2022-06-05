using System;
using UnityEngine;

// Token: 0x02000682 RID: 1666
[Serializable]
public struct RoomWeightEntry
{
	// Token: 0x170014F2 RID: 5362
	// (get) Token: 0x06003C1E RID: 15390 RVA: 0x000D02A0 File Offset: 0x000CE4A0
	public RoomID RoomID
	{
		get
		{
			return this.m_roomId;
		}
	}

	// Token: 0x170014F3 RID: 5363
	// (get) Token: 0x06003C1F RID: 15391 RVA: 0x000D02A8 File Offset: 0x000CE4A8
	public int RNGWeight
	{
		get
		{
			return this.m_weight;
		}
	}

	// Token: 0x170014F4 RID: 5364
	// (get) Token: 0x06003C20 RID: 15392 RVA: 0x000D02B0 File Offset: 0x000CE4B0
	public bool IsMirrored
	{
		get
		{
			return this.m_isMirrored;
		}
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x000D02B8 File Offset: 0x000CE4B8
	public RoomWeightEntry(RoomID roomID, bool isMirrored)
	{
		this = new RoomWeightEntry(roomID, isMirrored, 100);
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x000D02C4 File Offset: 0x000CE4C4
	public RoomWeightEntry(RoomID roomID, bool isMirrored, int weight)
	{
		this.m_roomId = roomID;
		this.m_weight = weight;
		this.m_isMirrored = isMirrored;
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x000D02DB File Offset: 0x000CE4DB
	public void SetWeight(int weight)
	{
		this.m_weight = weight;
	}

	// Token: 0x04002D54 RID: 11604
	[SerializeField]
	private RoomID m_roomId;

	// Token: 0x04002D55 RID: 11605
	[SerializeField]
	private int m_weight;

	// Token: 0x04002D56 RID: 11606
	[SerializeField]
	private bool m_isMirrored;

	// Token: 0x04002D57 RID: 11607
	public const int DEFAULT_ROOM_WEIGHT = 100;
}

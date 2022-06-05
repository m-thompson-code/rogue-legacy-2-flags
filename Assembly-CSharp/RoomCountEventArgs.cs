using System;

// Token: 0x02000C57 RID: 3159
public class RoomCountEventArgs : EventArgs
{
	// Token: 0x06005B0F RID: 23311 RVA: 0x00031F80 File Offset: 0x00030180
	public RoomCountEventArgs(int count)
	{
		this.Count = count;
	}

	// Token: 0x17001E54 RID: 7764
	// (get) Token: 0x06005B10 RID: 23312 RVA: 0x00031F8F File Offset: 0x0003018F
	// (set) Token: 0x06005B11 RID: 23313 RVA: 0x00031F97 File Offset: 0x00030197
	public int Count { get; private set; }
}

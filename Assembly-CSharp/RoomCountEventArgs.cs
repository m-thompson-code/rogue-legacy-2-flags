using System;

// Token: 0x02000794 RID: 1940
public class RoomCountEventArgs : EventArgs
{
	// Token: 0x06004192 RID: 16786 RVA: 0x000E9AAA File Offset: 0x000E7CAA
	public RoomCountEventArgs(int count)
	{
		this.Count = count;
	}

	// Token: 0x17001658 RID: 5720
	// (get) Token: 0x06004193 RID: 16787 RVA: 0x000E9AB9 File Offset: 0x000E7CB9
	// (set) Token: 0x06004194 RID: 16788 RVA: 0x000E9AC1 File Offset: 0x000E7CC1
	public int Count { get; private set; }
}

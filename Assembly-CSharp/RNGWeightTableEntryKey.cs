using System;

// Token: 0x02000683 RID: 1667
public struct RNGWeightTableEntryKey
{
	// Token: 0x170014F5 RID: 5365
	// (get) Token: 0x06003C24 RID: 15396 RVA: 0x000D02E4 File Offset: 0x000CE4E4
	public readonly bool IsMirrored { get; }

	// Token: 0x170014F6 RID: 5366
	// (get) Token: 0x06003C25 RID: 15397 RVA: 0x000D02EC File Offset: 0x000CE4EC
	public readonly int ID { get; }

	// Token: 0x06003C26 RID: 15398 RVA: 0x000D02F4 File Offset: 0x000CE4F4
	public RNGWeightTableEntryKey(int roomHashID, bool isMirrored)
	{
		this.ID = roomHashID;
		this.IsMirrored = isMirrored;
	}
}

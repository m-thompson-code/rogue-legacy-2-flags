using System;

// Token: 0x02000B03 RID: 2819
public struct RNGWeightTableEntryKey
{
	// Token: 0x17001CB3 RID: 7347
	// (get) Token: 0x06005483 RID: 21635 RVA: 0x0002DCD0 File Offset: 0x0002BED0
	public readonly bool IsMirrored { get; }

	// Token: 0x17001CB4 RID: 7348
	// (get) Token: 0x06005484 RID: 21636 RVA: 0x0002DCD8 File Offset: 0x0002BED8
	public readonly int ID { get; }

	// Token: 0x06005485 RID: 21637 RVA: 0x0002DCE0 File Offset: 0x0002BEE0
	public RNGWeightTableEntryKey(int roomHashID, bool isMirrored)
	{
		this.ID = roomHashID;
		this.IsMirrored = isMirrored;
	}
}

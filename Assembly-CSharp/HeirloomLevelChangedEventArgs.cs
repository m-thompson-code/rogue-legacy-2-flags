using System;

// Token: 0x02000CB6 RID: 3254
public class HeirloomLevelChangedEventArgs : EventArgs
{
	// Token: 0x06005D2D RID: 23853 RVA: 0x00033428 File Offset: 0x00031628
	public HeirloomLevelChangedEventArgs(HeirloomType heirloomType, int prevLevel, int newLevel)
	{
		this.Initialize(heirloomType, prevLevel, newLevel);
	}

	// Token: 0x06005D2E RID: 23854 RVA: 0x00033439 File Offset: 0x00031639
	public void Initialize(HeirloomType heirloomType, int oldLevel, int newLevel)
	{
		this.HeirloomType = heirloomType;
		this.PrevLevel = oldLevel;
		this.NewLevel = newLevel;
	}

	// Token: 0x17001EE2 RID: 7906
	// (get) Token: 0x06005D2F RID: 23855 RVA: 0x00033450 File Offset: 0x00031650
	// (set) Token: 0x06005D30 RID: 23856 RVA: 0x00033458 File Offset: 0x00031658
	public HeirloomType HeirloomType { get; private set; }

	// Token: 0x17001EE3 RID: 7907
	// (get) Token: 0x06005D31 RID: 23857 RVA: 0x00033461 File Offset: 0x00031661
	// (set) Token: 0x06005D32 RID: 23858 RVA: 0x00033469 File Offset: 0x00031669
	public int PrevLevel { get; private set; }

	// Token: 0x17001EE4 RID: 7908
	// (get) Token: 0x06005D33 RID: 23859 RVA: 0x00033472 File Offset: 0x00031672
	// (set) Token: 0x06005D34 RID: 23860 RVA: 0x0003347A File Offset: 0x0003167A
	public int NewLevel { get; private set; }
}

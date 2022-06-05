using System;

// Token: 0x020007F0 RID: 2032
public class HeirloomLevelChangedEventArgs : EventArgs
{
	// Token: 0x060043A4 RID: 17316 RVA: 0x000ECA12 File Offset: 0x000EAC12
	public HeirloomLevelChangedEventArgs(HeirloomType heirloomType, int prevLevel, int newLevel)
	{
		this.Initialize(heirloomType, prevLevel, newLevel);
	}

	// Token: 0x060043A5 RID: 17317 RVA: 0x000ECA23 File Offset: 0x000EAC23
	public void Initialize(HeirloomType heirloomType, int oldLevel, int newLevel)
	{
		this.HeirloomType = heirloomType;
		this.PrevLevel = oldLevel;
		this.NewLevel = newLevel;
	}

	// Token: 0x170016E4 RID: 5860
	// (get) Token: 0x060043A6 RID: 17318 RVA: 0x000ECA3A File Offset: 0x000EAC3A
	// (set) Token: 0x060043A7 RID: 17319 RVA: 0x000ECA42 File Offset: 0x000EAC42
	public HeirloomType HeirloomType { get; private set; }

	// Token: 0x170016E5 RID: 5861
	// (get) Token: 0x060043A8 RID: 17320 RVA: 0x000ECA4B File Offset: 0x000EAC4B
	// (set) Token: 0x060043A9 RID: 17321 RVA: 0x000ECA53 File Offset: 0x000EAC53
	public int PrevLevel { get; private set; }

	// Token: 0x170016E6 RID: 5862
	// (get) Token: 0x060043AA RID: 17322 RVA: 0x000ECA5C File Offset: 0x000EAC5C
	// (set) Token: 0x060043AB RID: 17323 RVA: 0x000ECA64 File Offset: 0x000EAC64
	public int NewLevel { get; private set; }
}

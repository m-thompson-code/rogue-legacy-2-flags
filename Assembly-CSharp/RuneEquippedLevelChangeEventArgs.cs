using System;

// Token: 0x02000C93 RID: 3219
public class RuneEquippedLevelChangeEventArgs : EventArgs
{
	// Token: 0x06005C69 RID: 23657 RVA: 0x00032BC8 File Offset: 0x00030DC8
	public RuneEquippedLevelChangeEventArgs(RuneType runeType, int newLevel)
	{
		this.Initialize(runeType, newLevel);
	}

	// Token: 0x06005C6A RID: 23658 RVA: 0x00032BD8 File Offset: 0x00030DD8
	public void Initialize(RuneType runeType, int newLevel)
	{
		this.RuneType = runeType;
		this.NewLevel = newLevel;
	}

	// Token: 0x17001EA3 RID: 7843
	// (get) Token: 0x06005C6B RID: 23659 RVA: 0x00032BE8 File Offset: 0x00030DE8
	// (set) Token: 0x06005C6C RID: 23660 RVA: 0x00032BF0 File Offset: 0x00030DF0
	public int NewLevel { get; private set; }

	// Token: 0x17001EA4 RID: 7844
	// (get) Token: 0x06005C6D RID: 23661 RVA: 0x00032BF9 File Offset: 0x00030DF9
	// (set) Token: 0x06005C6E RID: 23662 RVA: 0x00032C01 File Offset: 0x00030E01
	public RuneType RuneType { get; private set; }
}

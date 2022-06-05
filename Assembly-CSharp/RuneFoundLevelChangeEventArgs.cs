using System;

// Token: 0x02000C95 RID: 3221
public class RuneFoundLevelChangeEventArgs : EventArgs
{
	// Token: 0x06005C75 RID: 23669 RVA: 0x00032C4C File Offset: 0x00030E4C
	public RuneFoundLevelChangeEventArgs(RuneType runeType, int newLevel)
	{
		this.Initialize(runeType, newLevel);
	}

	// Token: 0x06005C76 RID: 23670 RVA: 0x00032C5C File Offset: 0x00030E5C
	public void Initialize(RuneType runeType, int newLevel)
	{
		this.RuneType = runeType;
		this.NewLevel = newLevel;
	}

	// Token: 0x17001EA7 RID: 7847
	// (get) Token: 0x06005C77 RID: 23671 RVA: 0x00032C6C File Offset: 0x00030E6C
	// (set) Token: 0x06005C78 RID: 23672 RVA: 0x00032C74 File Offset: 0x00030E74
	public RuneType RuneType { get; private set; }

	// Token: 0x17001EA8 RID: 7848
	// (get) Token: 0x06005C79 RID: 23673 RVA: 0x00032C7D File Offset: 0x00030E7D
	// (set) Token: 0x06005C7A RID: 23674 RVA: 0x00032C85 File Offset: 0x00030E85
	public int NewLevel { get; private set; }
}

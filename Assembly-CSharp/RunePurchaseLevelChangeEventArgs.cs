using System;

// Token: 0x02000C94 RID: 3220
public class RunePurchaseLevelChangeEventArgs : EventArgs
{
	// Token: 0x06005C6F RID: 23663 RVA: 0x00032C0A File Offset: 0x00030E0A
	public RunePurchaseLevelChangeEventArgs(RuneType runeType, int newLevel)
	{
		this.Initialize(runeType, newLevel);
	}

	// Token: 0x06005C70 RID: 23664 RVA: 0x00032C1A File Offset: 0x00030E1A
	public void Initialize(RuneType runeType, int newLevel)
	{
		this.RuneType = runeType;
		this.NewLevel = newLevel;
	}

	// Token: 0x17001EA5 RID: 7845
	// (get) Token: 0x06005C71 RID: 23665 RVA: 0x00032C2A File Offset: 0x00030E2A
	// (set) Token: 0x06005C72 RID: 23666 RVA: 0x00032C32 File Offset: 0x00030E32
	public RuneType RuneType { get; private set; }

	// Token: 0x17001EA6 RID: 7846
	// (get) Token: 0x06005C73 RID: 23667 RVA: 0x00032C3B File Offset: 0x00030E3B
	// (set) Token: 0x06005C74 RID: 23668 RVA: 0x00032C43 File Offset: 0x00030E43
	public int NewLevel { get; private set; }
}

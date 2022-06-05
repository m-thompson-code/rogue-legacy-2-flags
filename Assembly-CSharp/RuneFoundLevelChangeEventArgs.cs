using System;

// Token: 0x020007CF RID: 1999
public class RuneFoundLevelChangeEventArgs : EventArgs
{
	// Token: 0x060042EC RID: 17132 RVA: 0x000EC236 File Offset: 0x000EA436
	public RuneFoundLevelChangeEventArgs(RuneType runeType, int newLevel)
	{
		this.Initialize(runeType, newLevel);
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x000EC246 File Offset: 0x000EA446
	public void Initialize(RuneType runeType, int newLevel)
	{
		this.RuneType = runeType;
		this.NewLevel = newLevel;
	}

	// Token: 0x170016A9 RID: 5801
	// (get) Token: 0x060042EE RID: 17134 RVA: 0x000EC256 File Offset: 0x000EA456
	// (set) Token: 0x060042EF RID: 17135 RVA: 0x000EC25E File Offset: 0x000EA45E
	public RuneType RuneType { get; private set; }

	// Token: 0x170016AA RID: 5802
	// (get) Token: 0x060042F0 RID: 17136 RVA: 0x000EC267 File Offset: 0x000EA467
	// (set) Token: 0x060042F1 RID: 17137 RVA: 0x000EC26F File Offset: 0x000EA46F
	public int NewLevel { get; private set; }
}

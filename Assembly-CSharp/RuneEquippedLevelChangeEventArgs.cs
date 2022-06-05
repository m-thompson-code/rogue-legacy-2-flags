using System;

// Token: 0x020007CD RID: 1997
public class RuneEquippedLevelChangeEventArgs : EventArgs
{
	// Token: 0x060042E0 RID: 17120 RVA: 0x000EC1B2 File Offset: 0x000EA3B2
	public RuneEquippedLevelChangeEventArgs(RuneType runeType, int newLevel)
	{
		this.Initialize(runeType, newLevel);
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x000EC1C2 File Offset: 0x000EA3C2
	public void Initialize(RuneType runeType, int newLevel)
	{
		this.RuneType = runeType;
		this.NewLevel = newLevel;
	}

	// Token: 0x170016A5 RID: 5797
	// (get) Token: 0x060042E2 RID: 17122 RVA: 0x000EC1D2 File Offset: 0x000EA3D2
	// (set) Token: 0x060042E3 RID: 17123 RVA: 0x000EC1DA File Offset: 0x000EA3DA
	public int NewLevel { get; private set; }

	// Token: 0x170016A6 RID: 5798
	// (get) Token: 0x060042E4 RID: 17124 RVA: 0x000EC1E3 File Offset: 0x000EA3E3
	// (set) Token: 0x060042E5 RID: 17125 RVA: 0x000EC1EB File Offset: 0x000EA3EB
	public RuneType RuneType { get; private set; }
}

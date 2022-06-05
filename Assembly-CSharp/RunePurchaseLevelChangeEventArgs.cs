using System;

// Token: 0x020007CE RID: 1998
public class RunePurchaseLevelChangeEventArgs : EventArgs
{
	// Token: 0x060042E6 RID: 17126 RVA: 0x000EC1F4 File Offset: 0x000EA3F4
	public RunePurchaseLevelChangeEventArgs(RuneType runeType, int newLevel)
	{
		this.Initialize(runeType, newLevel);
	}

	// Token: 0x060042E7 RID: 17127 RVA: 0x000EC204 File Offset: 0x000EA404
	public void Initialize(RuneType runeType, int newLevel)
	{
		this.RuneType = runeType;
		this.NewLevel = newLevel;
	}

	// Token: 0x170016A7 RID: 5799
	// (get) Token: 0x060042E8 RID: 17128 RVA: 0x000EC214 File Offset: 0x000EA414
	// (set) Token: 0x060042E9 RID: 17129 RVA: 0x000EC21C File Offset: 0x000EA41C
	public RuneType RuneType { get; private set; }

	// Token: 0x170016A8 RID: 5800
	// (get) Token: 0x060042EA RID: 17130 RVA: 0x000EC225 File Offset: 0x000EA425
	// (set) Token: 0x060042EB RID: 17131 RVA: 0x000EC22D File Offset: 0x000EA42D
	public int NewLevel { get; private set; }
}

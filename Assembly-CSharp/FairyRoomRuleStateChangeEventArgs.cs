using System;

// Token: 0x020007E7 RID: 2023
public class FairyRoomRuleStateChangeEventArgs : EventArgs
{
	// Token: 0x06004378 RID: 17272 RVA: 0x000EC836 File Offset: 0x000EAA36
	public FairyRoomRuleStateChangeEventArgs(FairyRule rule)
	{
		this.Initialise(rule);
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x000EC845 File Offset: 0x000EAA45
	public void Initialise(FairyRule rule)
	{
		this.Rule = rule;
	}

	// Token: 0x170016D7 RID: 5847
	// (get) Token: 0x0600437A RID: 17274 RVA: 0x000EC84E File Offset: 0x000EAA4E
	// (set) Token: 0x0600437B RID: 17275 RVA: 0x000EC856 File Offset: 0x000EAA56
	public FairyRule Rule { get; private set; }
}

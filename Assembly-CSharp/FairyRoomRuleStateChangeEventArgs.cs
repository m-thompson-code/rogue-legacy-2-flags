using System;

// Token: 0x02000CAD RID: 3245
public class FairyRoomRuleStateChangeEventArgs : EventArgs
{
	// Token: 0x06005D01 RID: 23809 RVA: 0x0003324C File Offset: 0x0003144C
	public FairyRoomRuleStateChangeEventArgs(FairyRule rule)
	{
		this.Initialise(rule);
	}

	// Token: 0x06005D02 RID: 23810 RVA: 0x0003325B File Offset: 0x0003145B
	public void Initialise(FairyRule rule)
	{
		this.Rule = rule;
	}

	// Token: 0x17001ED5 RID: 7893
	// (get) Token: 0x06005D03 RID: 23811 RVA: 0x00033264 File Offset: 0x00031464
	// (set) Token: 0x06005D04 RID: 23812 RVA: 0x0003326C File Offset: 0x0003146C
	public FairyRule Rule { get; private set; }
}

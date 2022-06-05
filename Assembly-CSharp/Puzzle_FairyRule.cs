using System;

// Token: 0x02000440 RID: 1088
public class Puzzle_FairyRule : FairyRule
{
	// Token: 0x17000FBC RID: 4028
	// (get) Token: 0x060027ED RID: 10221 RVA: 0x00084708 File Offset: 0x00082908
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_PUZZLE_1";
		}
	}

	// Token: 0x17000FBD RID: 4029
	// (get) Token: 0x060027EE RID: 10222 RVA: 0x0008470F File Offset: 0x0008290F
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.Puzzle;
		}
	}

	// Token: 0x17000FBE RID: 4030
	// (get) Token: 0x060027EF RID: 10223 RVA: 0x00084713 File Offset: 0x00082913
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x00084716 File Offset: 0x00082916
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		base.State = FairyRoomState.Running;
	}
}

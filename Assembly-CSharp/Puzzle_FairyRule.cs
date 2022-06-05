using System;

// Token: 0x02000709 RID: 1801
public class Puzzle_FairyRule : FairyRule
{
	// Token: 0x1700149F RID: 5279
	// (get) Token: 0x060036FF RID: 14079 RVA: 0x0001E437 File Offset: 0x0001C637
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_PUZZLE_1";
		}
	}

	// Token: 0x170014A0 RID: 5280
	// (get) Token: 0x06003700 RID: 14080 RVA: 0x00004527 File Offset: 0x00002727
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.Puzzle;
		}
	}

	// Token: 0x170014A1 RID: 5281
	// (get) Token: 0x06003701 RID: 14081 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x0001E43E File Offset: 0x0001C63E
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		base.State = FairyRoomState.Running;
	}
}

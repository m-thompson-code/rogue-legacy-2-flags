using System;
using System.Collections;

// Token: 0x02000441 RID: 1089
public class ReachMe_FairyRule : FairyRule
{
	// Token: 0x17000FBF RID: 4031
	// (get) Token: 0x060027F2 RID: 10226 RVA: 0x00084728 File Offset: 0x00082928
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.ReachMe;
		}
	}

	// Token: 0x17000FC0 RID: 4032
	// (get) Token: 0x060027F3 RID: 10227 RVA: 0x0008472C File Offset: 0x0008292C
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_REACH_1";
		}
	}

	// Token: 0x17000FC1 RID: 4033
	// (get) Token: 0x060027F4 RID: 10228 RVA: 0x00084733 File Offset: 0x00082933
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x00084736 File Offset: 0x00082936
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		base.State = FairyRoomState.Running;
		this.m_chestInteractable = fairyRoomController.Chest.Interactable;
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x00084751 File Offset: 0x00082951
	private IEnumerator RunTimer()
	{
		for (;;)
		{
			if (this.m_chestInteractable.IsPlayerInTrigger)
			{
				base.SetIsPassed();
				base.StopAllCoroutines();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060027F7 RID: 10231 RVA: 0x00084760 File Offset: 0x00082960
	public override void StopRule()
	{
		base.StopRule();
		base.StopAllCoroutines();
	}

	// Token: 0x0400213D RID: 8509
	private Interactable m_chestInteractable;
}

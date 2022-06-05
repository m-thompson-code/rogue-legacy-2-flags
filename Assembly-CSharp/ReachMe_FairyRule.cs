using System;
using System.Collections;

// Token: 0x0200070A RID: 1802
public class ReachMe_FairyRule : FairyRule
{
	// Token: 0x170014A2 RID: 5282
	// (get) Token: 0x06003704 RID: 14084 RVA: 0x00006581 File Offset: 0x00004781
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.ReachMe;
		}
	}

	// Token: 0x170014A3 RID: 5283
	// (get) Token: 0x06003705 RID: 14085 RVA: 0x0001E448 File Offset: 0x0001C648
	public override string Description
	{
		get
		{
			return "LOC_ID_FAIRY_RULE_REACH_1";
		}
	}

	// Token: 0x170014A4 RID: 5284
	// (get) Token: 0x06003706 RID: 14086 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x0001E44F File Offset: 0x0001C64F
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		base.State = FairyRoomState.Running;
		this.m_chestInteractable = fairyRoomController.Chest.Interactable;
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x0001E46A File Offset: 0x0001C66A
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

	// Token: 0x06003709 RID: 14089 RVA: 0x0001E479 File Offset: 0x0001C679
	public override void StopRule()
	{
		base.StopRule();
		base.StopAllCoroutines();
	}

	// Token: 0x04002C6E RID: 11374
	private Interactable m_chestInteractable;
}

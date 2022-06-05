using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x02000522 RID: 1314
public class RunDialogue_SummonRule : BaseSummonRule
{
	// Token: 0x170011EA RID: 4586
	// (get) Token: 0x0600308D RID: 12429 RVA: 0x000A5D6D File Offset: 0x000A3F6D
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.RunDialogue;
		}
	}

	// Token: 0x170011EB RID: 4587
	// (get) Token: 0x0600308E RID: 12430 RVA: 0x000A5D74 File Offset: 0x000A3F74
	public override string RuleLabel
	{
		get
		{
			return "Run Dialogue";
		}
	}

	// Token: 0x0600308F RID: 12431 RVA: 0x000A5D7B File Offset: 0x000A3F7B
	public override IEnumerator RunSummonRule()
	{
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue(this.m_titleTextLocID, this.m_bodyTextLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400268A RID: 9866
	[SerializeField]
	private string m_titleTextLocID;

	// Token: 0x0400268B RID: 9867
	[SerializeField]
	private string m_bodyTextLocID;
}

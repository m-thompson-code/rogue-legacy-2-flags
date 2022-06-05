using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x0200089D RID: 2205
public class RunDialogue_SummonRule : BaseSummonRule
{
	// Token: 0x1700180D RID: 6157
	// (get) Token: 0x06004373 RID: 17267 RVA: 0x0002543C File Offset: 0x0002363C
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.RunDialogue;
		}
	}

	// Token: 0x1700180E RID: 6158
	// (get) Token: 0x06004374 RID: 17268 RVA: 0x00025443 File Offset: 0x00023643
	public override string RuleLabel
	{
		get
		{
			return "Run Dialogue";
		}
	}

	// Token: 0x06004375 RID: 17269 RVA: 0x0002544A File Offset: 0x0002364A
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

	// Token: 0x04003488 RID: 13448
	[SerializeField]
	private string m_titleTextLocID;

	// Token: 0x04003489 RID: 13449
	[SerializeField]
	private string m_bodyTextLocID;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000525 RID: 1317
public class SetGlobalTimer_SummonRule : BaseSummonRule
{
	// Token: 0x170011F0 RID: 4592
	// (get) Token: 0x06003099 RID: 12441 RVA: 0x000A5DF1 File Offset: 0x000A3FF1
	public override Color BoxColor
	{
		get
		{
			return Color.yellow;
		}
	}

	// Token: 0x170011F1 RID: 4593
	// (get) Token: 0x0600309A RID: 12442 RVA: 0x000A5DF8 File Offset: 0x000A3FF8
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetGlobalTimer;
		}
	}

	// Token: 0x170011F2 RID: 4594
	// (get) Token: 0x0600309B RID: 12443 RVA: 0x000A5DFF File Offset: 0x000A3FFF
	public override string RuleLabel
	{
		get
		{
			return "Set Global Timer";
		}
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x000A5E06 File Offset: 0x000A4006
	public override IEnumerator RunSummonRule()
	{
		if (this.m_resetTimer)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.ResetGlobalTimer, null, null);
		}
		if (this.m_startTimer)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StartGlobalTimer, null, null);
		}
		else if (this.m_stopTimer)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StopGlobalTimer, null, null);
		}
		if (this.m_displayTimer)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayGlobalTimer, null, null);
		}
		else if (this.m_hideTimer)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.HideGlobalTimer, null, null);
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x04002692 RID: 9874
	[SerializeField]
	private bool m_startTimer;

	// Token: 0x04002693 RID: 9875
	[SerializeField]
	private bool m_stopTimer;

	// Token: 0x04002694 RID: 9876
	[SerializeField]
	private bool m_displayTimer;

	// Token: 0x04002695 RID: 9877
	[SerializeField]
	private bool m_hideTimer;

	// Token: 0x04002696 RID: 9878
	[SerializeField]
	private bool m_resetTimer;

	// Token: 0x04002697 RID: 9879
	[SerializeField]
	private float m_slowDuration;
}

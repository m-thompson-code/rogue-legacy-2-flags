using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A3 RID: 2211
public class SetGlobalTimer_SummonRule : BaseSummonRule
{
	// Token: 0x17001819 RID: 6169
	// (get) Token: 0x06004391 RID: 17297 RVA: 0x000252B5 File Offset: 0x000234B5
	public override Color BoxColor
	{
		get
		{
			return Color.yellow;
		}
	}

	// Token: 0x1700181A RID: 6170
	// (get) Token: 0x06004392 RID: 17298 RVA: 0x000254F6 File Offset: 0x000236F6
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetGlobalTimer;
		}
	}

	// Token: 0x1700181B RID: 6171
	// (get) Token: 0x06004393 RID: 17299 RVA: 0x000254FD File Offset: 0x000236FD
	public override string RuleLabel
	{
		get
		{
			return "Set Global Timer";
		}
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x00025504 File Offset: 0x00023704
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

	// Token: 0x04003499 RID: 13465
	[SerializeField]
	private bool m_startTimer;

	// Token: 0x0400349A RID: 13466
	[SerializeField]
	private bool m_stopTimer;

	// Token: 0x0400349B RID: 13467
	[SerializeField]
	private bool m_displayTimer;

	// Token: 0x0400349C RID: 13468
	[SerializeField]
	private bool m_hideTimer;

	// Token: 0x0400349D RID: 13469
	[SerializeField]
	private bool m_resetTimer;

	// Token: 0x0400349E RID: 13470
	[SerializeField]
	private float m_slowDuration;
}

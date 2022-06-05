using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000891 RID: 2193
[Serializable]
public class DebugTrace_SummonRule : BaseSummonRule
{
	// Token: 0x170017F2 RID: 6130
	// (get) Token: 0x06004334 RID: 17204 RVA: 0x000252B5 File Offset: 0x000234B5
	public override Color BoxColor
	{
		get
		{
			return Color.yellow;
		}
	}

	// Token: 0x170017F3 RID: 6131
	// (get) Token: 0x06004335 RID: 17205 RVA: 0x0000B5D8 File Offset: 0x000097D8
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.DebugTrace;
		}
	}

	// Token: 0x170017F4 RID: 6132
	// (get) Token: 0x06004336 RID: 17206 RVA: 0x000252BC File Offset: 0x000234BC
	public override string RuleLabel
	{
		get
		{
			return "Debug Trace";
		}
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x000252C3 File Offset: 0x000234C3
	public override IEnumerator RunSummonRule()
	{
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400346A RID: 13418
	[SerializeField]
	private string m_debugString;
}

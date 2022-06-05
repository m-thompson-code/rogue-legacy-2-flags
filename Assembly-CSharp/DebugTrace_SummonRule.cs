using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051C RID: 1308
[Serializable]
public class DebugTrace_SummonRule : BaseSummonRule
{
	// Token: 0x170011DB RID: 4571
	// (get) Token: 0x06003072 RID: 12402 RVA: 0x000A5C42 File Offset: 0x000A3E42
	public override Color BoxColor
	{
		get
		{
			return Color.yellow;
		}
	}

	// Token: 0x170011DC RID: 4572
	// (get) Token: 0x06003073 RID: 12403 RVA: 0x000A5C49 File Offset: 0x000A3E49
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.DebugTrace;
		}
	}

	// Token: 0x170011DD RID: 4573
	// (get) Token: 0x06003074 RID: 12404 RVA: 0x000A5C50 File Offset: 0x000A3E50
	public override string RuleLabel
	{
		get
		{
			return "Debug Trace";
		}
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x000A5C57 File Offset: 0x000A3E57
	public override IEnumerator RunSummonRule()
	{
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400267F RID: 9855
	[SerializeField]
	private string m_debugString;
}

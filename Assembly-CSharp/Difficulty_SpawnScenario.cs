using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A67 RID: 2663
public class Difficulty_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BC1 RID: 7105
	// (get) Token: 0x06005086 RID: 20614 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
	// (set) Token: 0x06005087 RID: 20615 RVA: 0x0002BFA8 File Offset: 0x0002A1A8
	public int Difficulty
	{
		get
		{
			return this.m_level;
		}
		set
		{
			this.m_level = value;
		}
	}

	// Token: 0x17001BC2 RID: 7106
	// (get) Token: 0x06005088 RID: 20616 RVA: 0x0002BFB1 File Offset: 0x0002A1B1
	// (set) Token: 0x06005089 RID: 20617 RVA: 0x0002BFB9 File Offset: 0x0002A1B9
	public int ComparisonOperatorIndex
	{
		get
		{
			return this.m_comparisonOperator;
		}
		set
		{
			this.m_comparisonOperator = value;
		}
	}

	// Token: 0x17001BC3 RID: 7107
	// (get) Token: 0x0600508A RID: 20618 RVA: 0x001329A4 File Offset: 0x00130BA4
	public override string GizmoDescription
	{
		get
		{
			return Difficulty_SpawnScenario.INDEX_TO_STRING_TABLE[this.ComparisonOperatorIndex] + this.Difficulty.ToString();
		}
	}

	// Token: 0x17001BC4 RID: 7108
	// (get) Token: 0x0600508B RID: 20619 RVA: 0x00005315 File Offset: 0x00003515
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.Difficulty;
		}
	}

	// Token: 0x0600508C RID: 20620 RVA: 0x0002BFC2 File Offset: 0x0002A1C2
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.GetIsTrue();
	}

	// Token: 0x0600508D RID: 20621 RVA: 0x0002BFC2 File Offset: 0x0002A1C2
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.GetIsTrue();
	}

	// Token: 0x0600508E RID: 20622 RVA: 0x001329D4 File Offset: 0x00130BD4
	private void GetIsTrue()
	{
		switch (Difficulty_SpawnScenario.INDEX_TO_COMPARISON_OPERATOR_TABLE[this.ComparisonOperatorIndex])
		{
		case ComparisonOperators_RL.EqualTo:
			this.IsTrue = (GameUtility.Difficulty == this.Difficulty);
			return;
		case ComparisonOperators_RL.NotEqualTo:
			this.IsTrue = (GameUtility.Difficulty != this.Difficulty);
			return;
		case ComparisonOperators_RL.GreaterThan:
			this.IsTrue = (GameUtility.Difficulty > this.Difficulty);
			return;
		case ComparisonOperators_RL.LessThan:
			this.IsTrue = (GameUtility.Difficulty < this.Difficulty);
			return;
		case ComparisonOperators_RL.EqualsOrGreaterThan:
			this.IsTrue = (GameUtility.Difficulty >= this.Difficulty);
			return;
		case ComparisonOperators_RL.EqualsOrLessThan:
			this.IsTrue = (GameUtility.Difficulty <= this.Difficulty);
			return;
		default:
			return;
		}
	}

	// Token: 0x04003CF1 RID: 15601
	[SerializeField]
	private int m_level;

	// Token: 0x04003CF2 RID: 15602
	[SerializeField]
	private int m_comparisonOperator;

	// Token: 0x04003CF3 RID: 15603
	private static Dictionary<int, ComparisonOperators_RL> INDEX_TO_COMPARISON_OPERATOR_TABLE = new Dictionary<int, ComparisonOperators_RL>
	{
		{
			0,
			ComparisonOperators_RL.EqualTo
		},
		{
			1,
			ComparisonOperators_RL.LessThan
		},
		{
			2,
			ComparisonOperators_RL.GreaterThan
		},
		{
			3,
			ComparisonOperators_RL.EqualsOrLessThan
		},
		{
			4,
			ComparisonOperators_RL.EqualsOrGreaterThan
		}
	};

	// Token: 0x04003CF4 RID: 15604
	private static Dictionary<int, string> INDEX_TO_STRING_TABLE = new Dictionary<int, string>
	{
		{
			0,
			"=="
		},
		{
			1,
			"<"
		},
		{
			2,
			">"
		},
		{
			3,
			"<="
		},
		{
			4,
			">="
		}
	};
}

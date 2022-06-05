using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class Difficulty_SpawnScenario : SpawnScenario
{
	// Token: 0x1700145A RID: 5210
	// (get) Token: 0x060039A7 RID: 14759 RVA: 0x000C45F5 File Offset: 0x000C27F5
	// (set) Token: 0x060039A8 RID: 14760 RVA: 0x000C45FD File Offset: 0x000C27FD
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

	// Token: 0x1700145B RID: 5211
	// (get) Token: 0x060039A9 RID: 14761 RVA: 0x000C4606 File Offset: 0x000C2806
	// (set) Token: 0x060039AA RID: 14762 RVA: 0x000C460E File Offset: 0x000C280E
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

	// Token: 0x1700145C RID: 5212
	// (get) Token: 0x060039AB RID: 14763 RVA: 0x000C4618 File Offset: 0x000C2818
	public override string GizmoDescription
	{
		get
		{
			return Difficulty_SpawnScenario.INDEX_TO_STRING_TABLE[this.ComparisonOperatorIndex] + this.Difficulty.ToString();
		}
	}

	// Token: 0x1700145D RID: 5213
	// (get) Token: 0x060039AC RID: 14764 RVA: 0x000C4648 File Offset: 0x000C2848
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.Difficulty;
		}
	}

	// Token: 0x060039AD RID: 14765 RVA: 0x000C464C File Offset: 0x000C284C
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.GetIsTrue();
	}

	// Token: 0x060039AE RID: 14766 RVA: 0x000C4654 File Offset: 0x000C2854
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.GetIsTrue();
	}

	// Token: 0x060039AF RID: 14767 RVA: 0x000C465C File Offset: 0x000C285C
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

	// Token: 0x04002C5F RID: 11359
	[SerializeField]
	private int m_level;

	// Token: 0x04002C60 RID: 11360
	[SerializeField]
	private int m_comparisonOperator;

	// Token: 0x04002C61 RID: 11361
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

	// Token: 0x04002C62 RID: 11362
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

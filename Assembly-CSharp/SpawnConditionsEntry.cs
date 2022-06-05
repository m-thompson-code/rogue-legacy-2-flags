using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000642 RID: 1602
[Serializable]
public class SpawnConditionsEntry
{
	// Token: 0x060039DD RID: 14813 RVA: 0x000C4F14 File Offset: 0x000C3114
	public SpawnConditionsEntry()
	{
		this.AddScenario();
	}

	// Token: 0x1700146F RID: 5231
	// (get) Token: 0x060039DE RID: 14814 RVA: 0x000C4F22 File Offset: 0x000C3122
	// (set) Token: 0x060039DF RID: 14815 RVA: 0x000C4F2A File Offset: 0x000C312A
	public bool IsTrue { get; set; }

	// Token: 0x17001470 RID: 5232
	// (get) Token: 0x060039E0 RID: 14816 RVA: 0x000C4F33 File Offset: 0x000C3133
	// (set) Token: 0x060039E1 RID: 14817 RVA: 0x000C4F3B File Offset: 0x000C313B
	public SpawnScenarioEntry[] Scenarios
	{
		get
		{
			return this.m_scenarios;
		}
		set
		{
			this.m_scenarios = value;
		}
	}

	// Token: 0x17001471 RID: 5233
	// (get) Token: 0x060039E2 RID: 14818 RVA: 0x000C4F44 File Offset: 0x000C3144
	// (set) Token: 0x060039E3 RID: 14819 RVA: 0x000C4F4C File Offset: 0x000C314C
	public ScenarioData[] ScenarioData
	{
		get
		{
			return this.m_scenarioData;
		}
		set
		{
			this.m_scenarioData = value;
		}
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x000C4F58 File Offset: 0x000C3158
	public void AddScenario()
	{
		List<SpawnScenarioEntry> list;
		if (this.Scenarios == null)
		{
			list = new List<SpawnScenarioEntry>();
		}
		else
		{
			list = this.Scenarios.ToList<SpawnScenarioEntry>();
		}
		list.Add(new SpawnScenarioEntry());
		this.Scenarios = list.ToArray();
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x000C4F98 File Offset: 0x000C3198
	public void DeleteScenario(SpawnScenarioEntry scenarioEntry)
	{
		List<SpawnScenarioEntry> list = this.Scenarios.ToList<SpawnScenarioEntry>();
		if (list.Remove(scenarioEntry))
		{
			this.Scenarios = list.ToArray();
		}
	}

	// Token: 0x060039E6 RID: 14822 RVA: 0x000C4FC6 File Offset: 0x000C31C6
	public bool GetIsTrue()
	{
		if (this.Scenarios != null && this.Scenarios.Length != 0)
		{
			return this.Scenarios.All((SpawnScenarioEntry entry) => entry.Scenario.IsTrue);
		}
		return false;
	}

	// Token: 0x04002C7B RID: 11387
	[SerializeField]
	private ScenarioData[] m_scenarioData;

	// Token: 0x04002C7C RID: 11388
	private SpawnScenarioEntry[] m_scenarios;
}

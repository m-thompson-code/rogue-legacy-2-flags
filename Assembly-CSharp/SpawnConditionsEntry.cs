using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000A6F RID: 2671
[Serializable]
public class SpawnConditionsEntry
{
	// Token: 0x060050BC RID: 20668 RVA: 0x0002C114 File Offset: 0x0002A314
	public SpawnConditionsEntry()
	{
		this.AddScenario();
	}

	// Token: 0x17001BD6 RID: 7126
	// (get) Token: 0x060050BD RID: 20669 RVA: 0x0002C122 File Offset: 0x0002A322
	// (set) Token: 0x060050BE RID: 20670 RVA: 0x0002C12A File Offset: 0x0002A32A
	public bool IsTrue { get; set; }

	// Token: 0x17001BD7 RID: 7127
	// (get) Token: 0x060050BF RID: 20671 RVA: 0x0002C133 File Offset: 0x0002A333
	// (set) Token: 0x060050C0 RID: 20672 RVA: 0x0002C13B File Offset: 0x0002A33B
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

	// Token: 0x17001BD8 RID: 7128
	// (get) Token: 0x060050C1 RID: 20673 RVA: 0x0002C144 File Offset: 0x0002A344
	// (set) Token: 0x060050C2 RID: 20674 RVA: 0x0002C14C File Offset: 0x0002A34C
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

	// Token: 0x060050C3 RID: 20675 RVA: 0x001330F4 File Offset: 0x001312F4
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

	// Token: 0x060050C4 RID: 20676 RVA: 0x00133134 File Offset: 0x00131334
	public void DeleteScenario(SpawnScenarioEntry scenarioEntry)
	{
		List<SpawnScenarioEntry> list = this.Scenarios.ToList<SpawnScenarioEntry>();
		if (list.Remove(scenarioEntry))
		{
			this.Scenarios = list.ToArray();
		}
	}

	// Token: 0x060050C5 RID: 20677 RVA: 0x0002C155 File Offset: 0x0002A355
	public bool GetIsTrue()
	{
		if (this.Scenarios != null && this.Scenarios.Length != 0)
		{
			return this.Scenarios.All((SpawnScenarioEntry entry) => entry.Scenario.IsTrue);
		}
		return false;
	}

	// Token: 0x04003D0D RID: 15629
	[SerializeField]
	private ScenarioData[] m_scenarioData;

	// Token: 0x04003D0E RID: 15630
	private SpawnScenarioEntry[] m_scenarios;
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000A74 RID: 2676
public class SpawnLogicController : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x17001BDD RID: 7133
	// (get) Token: 0x060050D5 RID: 20693 RVA: 0x0002C1EE File Offset: 0x0002A3EE
	// (set) Token: 0x060050D6 RID: 20694 RVA: 0x0002C1F6 File Offset: 0x0002A3F6
	public bool ShouldSpawn { get; private set; }

	// Token: 0x17001BDE RID: 7134
	// (get) Token: 0x060050D7 RID: 20695 RVA: 0x0002C1FF File Offset: 0x0002A3FF
	// (set) Token: 0x060050D8 RID: 20696 RVA: 0x0002C207 File Offset: 0x0002A407
	public SpawnConditionsEntry[] SpawnConditions
	{
		get
		{
			return this.m_spawnConditions;
		}
		private set
		{
			this.m_spawnConditions = value;
		}
	}

	// Token: 0x17001BDF RID: 7135
	// (get) Token: 0x060050D9 RID: 20697 RVA: 0x0002C210 File Offset: 0x0002A410
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x060050DA RID: 20698 RVA: 0x0002C218 File Offset: 0x0002A418
	public void SetRoom(BaseRoom value)
	{
		this.m_room = value;
	}

	// Token: 0x060050DB RID: 20699 RVA: 0x00133164 File Offset: 0x00131364
	private void Start()
	{
		if (this.SpawnConditions != null)
		{
			foreach (SpawnConditionsEntry spawnConditionsEntry in this.SpawnConditions)
			{
				if (spawnConditionsEntry.Scenarios != null)
				{
					foreach (SpawnScenarioEntry spawnScenarioEntry in spawnConditionsEntry.Scenarios)
					{
						if (spawnScenarioEntry.Scenario != null)
						{
							spawnScenarioEntry.Scenario.Start();
						}
					}
				}
			}
		}
	}

	// Token: 0x060050DC RID: 20700 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnDrawGizmos()
	{
	}

	// Token: 0x060050DD RID: 20701 RVA: 0x001331D0 File Offset: 0x001313D0
	public void RunIsSpawnedCheck(SpawnScenarioCheckStage checkStage = SpawnScenarioCheckStage.PreMerge)
	{
		this.ShouldSpawn = true;
		if (this.SpawnConditions != null && this.SpawnConditions.Length != 0)
		{
			foreach (SpawnConditionsEntry spawnConditionsEntry in this.SpawnConditions)
			{
				if (spawnConditionsEntry.Scenarios != null)
				{
					foreach (SpawnScenarioEntry spawnScenarioEntry in spawnConditionsEntry.Scenarios)
					{
						if (spawnScenarioEntry.Scenario != null && spawnScenarioEntry.Scenario.CheckStage == checkStage)
						{
							spawnScenarioEntry.Scenario.RunIsTrueCheck(this.Room);
						}
					}
				}
			}
			this.ShouldSpawn = this.SpawnConditions.Any((SpawnConditionsEntry condition) => condition.GetIsTrue());
		}
	}

	// Token: 0x060050DE RID: 20702 RVA: 0x0002C221 File Offset: 0x0002A421
	public bool GetIsSpawnedBasedOnConditions()
	{
		return this.ShouldSpawn;
	}

	// Token: 0x060050DF RID: 20703 RVA: 0x0002C229 File Offset: 0x0002A429
	public SpawnConditionsEntry AddCondition()
	{
		return this.AddCondition(new SpawnConditionsEntry());
	}

	// Token: 0x060050E0 RID: 20704 RVA: 0x00133298 File Offset: 0x00131498
	public SpawnConditionsEntry AddCondition(SpawnConditionsEntry condition)
	{
		if (Application.isPlaying)
		{
			return null;
		}
		List<SpawnConditionsEntry> list = this.SpawnConditions.ToList<SpawnConditionsEntry>();
		list.Add(condition);
		this.SpawnConditions = list.ToArray();
		return condition;
	}

	// Token: 0x060050E1 RID: 20705 RVA: 0x001332D0 File Offset: 0x001314D0
	public void DeleteCondition(SpawnConditionsEntry condition)
	{
		List<SpawnConditionsEntry> list = this.SpawnConditions.ToList<SpawnConditionsEntry>();
		if (list.Remove(condition))
		{
			this.SpawnConditions = list.ToArray();
		}
	}

	// Token: 0x060050E2 RID: 20706 RVA: 0x00133300 File Offset: 0x00131500
	public void OnBeforeSerialize()
	{
		foreach (SpawnConditionsEntry spawnConditionsEntry in this.SpawnConditions)
		{
			if (spawnConditionsEntry.Scenarios != null)
			{
				if (spawnConditionsEntry.ScenarioData == null || spawnConditionsEntry.ScenarioData.Length != spawnConditionsEntry.Scenarios.Length)
				{
					spawnConditionsEntry.ScenarioData = new ScenarioData[spawnConditionsEntry.Scenarios.Length];
				}
				for (int j = 0; j < spawnConditionsEntry.Scenarios.Length; j++)
				{
					string dataType = "";
					string data = "";
					if (spawnConditionsEntry.Scenarios[j].Scenario != null)
					{
						dataType = spawnConditionsEntry.Scenarios[j].Scenario.GetTypeAsString();
						data = spawnConditionsEntry.Scenarios[j].Scenario.GetDataAsString();
					}
					spawnConditionsEntry.ScenarioData[j].DataType = dataType;
					spawnConditionsEntry.ScenarioData[j].Data = data;
				}
			}
		}
	}

	// Token: 0x060050E3 RID: 20707 RVA: 0x001333E4 File Offset: 0x001315E4
	public void OnAfterDeserialize()
	{
		foreach (SpawnConditionsEntry spawnConditionsEntry in this.SpawnConditions)
		{
			if (spawnConditionsEntry.ScenarioData != null)
			{
				spawnConditionsEntry.Scenarios = new SpawnScenarioEntry[spawnConditionsEntry.ScenarioData.Length];
				for (int j = 0; j < spawnConditionsEntry.ScenarioData.Length; j++)
				{
					Type type = Type.GetType(spawnConditionsEntry.ScenarioData[j].DataType);
					if (type != null)
					{
						if (spawnConditionsEntry.Scenarios[j] == null)
						{
							spawnConditionsEntry.Scenarios[j] = new SpawnScenarioEntry();
						}
						SpawnScenario scenario = (SpawnScenario)JsonUtility.FromJson(spawnConditionsEntry.ScenarioData[j].Data, type);
						spawnConditionsEntry.Scenarios[j].Scenario = scenario;
					}
				}
			}
		}
	}

	// Token: 0x04003D18 RID: 15640
	[SerializeField]
	private SpawnConditionsEntry[] m_spawnConditions = new SpawnConditionsEntry[0];

	// Token: 0x04003D19 RID: 15641
	private const int TEXT_SIZE = 10;

	// Token: 0x04003D1A RID: 15642
	private BaseRoom m_room;
}

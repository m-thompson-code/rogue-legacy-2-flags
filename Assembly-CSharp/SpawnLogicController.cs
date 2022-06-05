using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000646 RID: 1606
public class SpawnLogicController : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x17001476 RID: 5238
	// (get) Token: 0x060039F3 RID: 14835 RVA: 0x000C5054 File Offset: 0x000C3254
	// (set) Token: 0x060039F4 RID: 14836 RVA: 0x000C505C File Offset: 0x000C325C
	public bool ShouldSpawn { get; private set; }

	// Token: 0x17001477 RID: 5239
	// (get) Token: 0x060039F5 RID: 14837 RVA: 0x000C5065 File Offset: 0x000C3265
	// (set) Token: 0x060039F6 RID: 14838 RVA: 0x000C506D File Offset: 0x000C326D
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

	// Token: 0x17001478 RID: 5240
	// (get) Token: 0x060039F7 RID: 14839 RVA: 0x000C5076 File Offset: 0x000C3276
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x060039F8 RID: 14840 RVA: 0x000C507E File Offset: 0x000C327E
	public void SetRoom(BaseRoom value)
	{
		this.m_room = value;
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x000C5088 File Offset: 0x000C3288
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

	// Token: 0x060039FA RID: 14842 RVA: 0x000C50F2 File Offset: 0x000C32F2
	private void OnDrawGizmos()
	{
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x000C50F4 File Offset: 0x000C32F4
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

	// Token: 0x060039FC RID: 14844 RVA: 0x000C51B9 File Offset: 0x000C33B9
	public bool GetIsSpawnedBasedOnConditions()
	{
		return this.ShouldSpawn;
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x000C51C1 File Offset: 0x000C33C1
	public SpawnConditionsEntry AddCondition()
	{
		return this.AddCondition(new SpawnConditionsEntry());
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x000C51D0 File Offset: 0x000C33D0
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

	// Token: 0x060039FF RID: 14847 RVA: 0x000C5208 File Offset: 0x000C3408
	public void DeleteCondition(SpawnConditionsEntry condition)
	{
		List<SpawnConditionsEntry> list = this.SpawnConditions.ToList<SpawnConditionsEntry>();
		if (list.Remove(condition))
		{
			this.SpawnConditions = list.ToArray();
		}
	}

	// Token: 0x06003A00 RID: 14848 RVA: 0x000C5238 File Offset: 0x000C3438
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

	// Token: 0x06003A01 RID: 14849 RVA: 0x000C531C File Offset: 0x000C351C
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

	// Token: 0x04002C84 RID: 11396
	[SerializeField]
	private SpawnConditionsEntry[] m_spawnConditions = new SpawnConditionsEntry[0];

	// Token: 0x04002C85 RID: 11397
	private const int TEXT_SIZE = 10;

	// Token: 0x04002C86 RID: 11398
	private BaseRoom m_room;
}

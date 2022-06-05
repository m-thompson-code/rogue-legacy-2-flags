using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000AFE RID: 2814
[Serializable]
public class RoomContentEntry
{
	// Token: 0x17001CAB RID: 7339
	// (get) Token: 0x0600546C RID: 21612 RVA: 0x0002DBB8 File Offset: 0x0002BDB8
	public RoomContentType ContentType
	{
		get
		{
			return this.m_contentType;
		}
	}

	// Token: 0x17001CAC RID: 7340
	// (get) Token: 0x0600546D RID: 21613 RVA: 0x0002DBC0 File Offset: 0x0002BDC0
	public Vector2 LocalPosition
	{
		get
		{
			return this.m_localPosition;
		}
	}

	// Token: 0x17001CAD RID: 7341
	// (get) Token: 0x0600546E RID: 21614 RVA: 0x0002DBC8 File Offset: 0x0002BDC8
	public SpawnConditionsEntry[] SpawnConditions
	{
		get
		{
			return this.m_spawnConditions;
		}
	}

	// Token: 0x0600546F RID: 21615 RVA: 0x0002DBD0 File Offset: 0x0002BDD0
	public RoomContentEntry(RoomContentType contentType, Vector2 localPosition, SpawnConditionsEntry[] spawnConditions)
	{
		this.m_contentType = contentType;
		this.m_localPosition = localPosition;
		this.m_spawnConditions = spawnConditions;
	}

	// Token: 0x06005470 RID: 21616 RVA: 0x0013FE24 File Offset: 0x0013E024
	public bool GetIsSpawned(GridPointManager gridPointManager)
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
		bool result = true;
		if (this.SpawnConditions != null && this.SpawnConditions.Length != 0)
		{
			foreach (SpawnConditionsEntry spawnConditionsEntry2 in this.SpawnConditions)
			{
				if (spawnConditionsEntry2.Scenarios != null)
				{
					foreach (SpawnScenarioEntry spawnScenarioEntry in spawnConditionsEntry2.Scenarios)
					{
						if (spawnScenarioEntry.Scenario != null)
						{
							spawnScenarioEntry.Scenario.RunIsTrueCheck(gridPointManager);
						}
					}
				}
			}
			result = this.SpawnConditions.Any((SpawnConditionsEntry condition) => condition.GetIsTrue());
		}
		return result;
	}

	// Token: 0x04003EEE RID: 16110
	[SerializeField]
	private RoomContentType m_contentType;

	// Token: 0x04003EEF RID: 16111
	[SerializeField]
	private Vector2 m_localPosition;

	// Token: 0x04003EF0 RID: 16112
	[SerializeField]
	private SpawnConditionsEntry[] m_spawnConditions;
}

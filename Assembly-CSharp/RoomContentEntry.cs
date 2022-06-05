using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200067F RID: 1663
[Serializable]
public class RoomContentEntry
{
	// Token: 0x170014ED RID: 5357
	// (get) Token: 0x06003C10 RID: 15376 RVA: 0x000CFF23 File Offset: 0x000CE123
	public RoomContentType ContentType
	{
		get
		{
			return this.m_contentType;
		}
	}

	// Token: 0x170014EE RID: 5358
	// (get) Token: 0x06003C11 RID: 15377 RVA: 0x000CFF2B File Offset: 0x000CE12B
	public Vector2 LocalPosition
	{
		get
		{
			return this.m_localPosition;
		}
	}

	// Token: 0x170014EF RID: 5359
	// (get) Token: 0x06003C12 RID: 15378 RVA: 0x000CFF33 File Offset: 0x000CE133
	public SpawnConditionsEntry[] SpawnConditions
	{
		get
		{
			return this.m_spawnConditions;
		}
	}

	// Token: 0x06003C13 RID: 15379 RVA: 0x000CFF3B File Offset: 0x000CE13B
	public RoomContentEntry(RoomContentType contentType, Vector2 localPosition, SpawnConditionsEntry[] spawnConditions)
	{
		this.m_contentType = contentType;
		this.m_localPosition = localPosition;
		this.m_spawnConditions = spawnConditions;
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x000CFF58 File Offset: 0x000CE158
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

	// Token: 0x04002D47 RID: 11591
	[SerializeField]
	private RoomContentType m_contentType;

	// Token: 0x04002D48 RID: 11592
	[SerializeField]
	private Vector2 m_localPosition;

	// Token: 0x04002D49 RID: 11593
	[SerializeField]
	private SpawnConditionsEntry[] m_spawnConditions;
}

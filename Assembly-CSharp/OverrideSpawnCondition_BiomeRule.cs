using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005F6 RID: 1526
[CreateAssetMenu(menuName = "Custom/Biome Rules/Override Spawn Condition")]
public class OverrideSpawnCondition_BiomeRule : BiomeRule
{
	// Token: 0x1700137A RID: 4986
	// (get) Token: 0x06003706 RID: 14086 RVA: 0x000BCE9F File Offset: 0x000BB09F
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.WorldCreationComplete;
		}
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x000BCEA4 File Offset: 0x000BB0A4
	private SpawnConditionOverride GetSpawnConditionOverride(BiomeType biome)
	{
		SpawnConditionOverride spawnConditionOverride = BiomeRuleManager.GetSpawnConditionOverride(biome);
		if (spawnConditionOverride == null)
		{
			Debug.LogFormat("<color=red>| {0} | No SpawnConditionOverride found for {1} Biome. If you see this message, please add a bug report to pivotal along with the Stack Trace</color>", new object[]
			{
				this,
				biome
			});
		}
		return spawnConditionOverride;
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x000BCED9 File Offset: 0x000BB0D9
	public override IEnumerator RunRule(BiomeType biome)
	{
		SpawnConditionOverride spawnConditionOverride = this.GetSpawnConditionOverride(biome);
		if (spawnConditionOverride != null)
		{
			this.SetOverrideIsOn(spawnConditionOverride, true);
			spawnConditionOverride.SetOverrideValue(this.m_condition, this.m_value);
		}
		yield break;
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x000BCEEF File Offset: 0x000BB0EF
	private void SetOverrideIsOn(SpawnConditionOverride spawnConditionOverride, bool isOn)
	{
		spawnConditionOverride.SetIsOverrideOn(this.m_condition, isOn);
	}

	// Token: 0x0600370A RID: 14090 RVA: 0x000BCF00 File Offset: 0x000BB100
	public override void UndoRule(BiomeType biome)
	{
		SpawnConditionOverride spawnConditionOverride = this.GetSpawnConditionOverride(biome);
		if (spawnConditionOverride != null)
		{
			this.SetOverrideIsOn(spawnConditionOverride, false);
		}
	}

	// Token: 0x04002A60 RID: 10848
	[SerializeField]
	private SpawnConditionOverrideID m_condition = SpawnConditionOverrideID.Dash;

	// Token: 0x04002A61 RID: 10849
	[SerializeField]
	private bool m_value = true;
}

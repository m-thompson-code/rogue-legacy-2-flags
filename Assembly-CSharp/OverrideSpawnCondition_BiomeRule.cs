using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A0B RID: 2571
[CreateAssetMenu(menuName = "Custom/Biome Rules/Override Spawn Condition")]
public class OverrideSpawnCondition_BiomeRule : BiomeRule
{
	// Token: 0x17001ABD RID: 6845
	// (get) Token: 0x06004D5F RID: 19807 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.WorldCreationComplete;
		}
	}

	// Token: 0x06004D60 RID: 19808 RVA: 0x0012B81C File Offset: 0x00129A1C
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

	// Token: 0x06004D61 RID: 19809 RVA: 0x0002A03B File Offset: 0x0002823B
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

	// Token: 0x06004D62 RID: 19810 RVA: 0x0002A051 File Offset: 0x00028251
	private void SetOverrideIsOn(SpawnConditionOverride spawnConditionOverride, bool isOn)
	{
		spawnConditionOverride.SetIsOverrideOn(this.m_condition, isOn);
	}

	// Token: 0x06004D63 RID: 19811 RVA: 0x0012B854 File Offset: 0x00129A54
	public override void UndoRule(BiomeType biome)
	{
		SpawnConditionOverride spawnConditionOverride = this.GetSpawnConditionOverride(biome);
		if (spawnConditionOverride != null)
		{
			this.SetOverrideIsOn(spawnConditionOverride, false);
		}
	}

	// Token: 0x04003A8F RID: 14991
	[SerializeField]
	private SpawnConditionOverrideID m_condition = SpawnConditionOverrideID.Dash;

	// Token: 0x04003A90 RID: 14992
	[SerializeField]
	private bool m_value = true;
}

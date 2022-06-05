using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005F2 RID: 1522
[CreateAssetMenu(menuName = "Custom/Biome Rules/Gameplay Mods/Change Game Speed")]
public class ChangeGameSpeed_GameplayMod_BiomeRule : BiomeRule
{
	// Token: 0x17001375 RID: 4981
	// (get) Token: 0x060036F4 RID: 14068 RVA: 0x000BCDBC File Offset: 0x000BAFBC
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x060036F5 RID: 14069 RVA: 0x000BCDC0 File Offset: 0x000BAFC0
	public override IEnumerator RunRule(BiomeType biome)
	{
		if (this.m_speedMultiplier <= 0f)
		{
			Debug.LogFormat("<color=red>|{0}| Speed Multiplier is set to ({1}). Its value must be greater than 0. Setting it to 1.</color>", new object[]
			{
				this,
				this.m_speedMultiplier
			});
			this.m_speedMultiplier = 1f;
		}
		RLTimeScale.SetTimeScale(TimeScaleType.Game, this.m_speedMultiplier);
		yield break;
	}

	// Token: 0x060036F6 RID: 14070 RVA: 0x000BCDCF File Offset: 0x000BAFCF
	public override void UndoRule(BiomeType biome)
	{
		RLTimeScale.SetTimeScale(TimeScaleType.Game, 1f);
	}

	// Token: 0x04002A56 RID: 10838
	[SerializeField]
	private float m_speedMultiplier = 1f;
}

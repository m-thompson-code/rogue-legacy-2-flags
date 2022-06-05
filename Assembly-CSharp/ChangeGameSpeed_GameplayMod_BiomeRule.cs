using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A04 RID: 2564
[CreateAssetMenu(menuName = "Custom/Biome Rules/Gameplay Mods/Change Game Speed")]
public class ChangeGameSpeed_GameplayMod_BiomeRule : BiomeRule
{
	// Token: 0x17001AB2 RID: 6834
	// (get) Token: 0x06004D3B RID: 19771 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004D3C RID: 19772 RVA: 0x00029F2C File Offset: 0x0002812C
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

	// Token: 0x06004D3D RID: 19773 RVA: 0x000192EE File Offset: 0x000174EE
	public override void UndoRule(BiomeType biome)
	{
		RLTimeScale.SetTimeScale(TimeScaleType.Game, 1f);
	}

	// Token: 0x04003A7C RID: 14972
	[SerializeField]
	private float m_speedMultiplier = 1f;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005F7 RID: 1527
[CreateAssetMenu(menuName = "Custom/Biome Rules/Override Terminal Velocity")]
public class OverrideTerminalVelocity_BiomeRule : BiomeRule
{
	// Token: 0x1700137B RID: 4987
	// (get) Token: 0x0600370C RID: 14092 RVA: 0x000BCF37 File Offset: 0x000BB137
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x0600370D RID: 14093 RVA: 0x000BCF3B File Offset: 0x000BB13B
	public override IEnumerator RunRule(BiomeType biome)
	{
		Global_EV.SetTerminalVelocityOverride(true, this.m_terminalVelocity);
		yield break;
	}

	// Token: 0x0600370E RID: 14094 RVA: 0x000BCF4A File Offset: 0x000BB14A
	public override void UndoRule(BiomeType biome)
	{
		Global_EV.SetTerminalVelocityOverride(false, 0f);
	}

	// Token: 0x04002A62 RID: 10850
	[SerializeField]
	private float m_terminalVelocity = -10f;
}

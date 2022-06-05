using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A0D RID: 2573
[CreateAssetMenu(menuName = "Custom/Biome Rules/Override Terminal Velocity")]
public class OverrideTerminalVelocity_BiomeRule : BiomeRule
{
	// Token: 0x17001AC0 RID: 6848
	// (get) Token: 0x06004D6B RID: 19819 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004D6C RID: 19820 RVA: 0x0002A08E File Offset: 0x0002828E
	public override IEnumerator RunRule(BiomeType biome)
	{
		Global_EV.SetTerminalVelocityOverride(true, this.m_terminalVelocity);
		yield break;
	}

	// Token: 0x06004D6D RID: 19821 RVA: 0x0002A09D File Offset: 0x0002829D
	public override void UndoRule(BiomeType biome)
	{
		Global_EV.SetTerminalVelocityOverride(false, 0f);
	}

	// Token: 0x04003A95 RID: 14997
	[SerializeField]
	private float m_terminalVelocity = -10f;
}

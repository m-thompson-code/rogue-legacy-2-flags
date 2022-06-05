using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000A08 RID: 2568
[CreateAssetMenu(menuName = "Custom/Biome Rules/Gameplay Mods/Enemy HP Mod")]
public class EnemyHPMod_BiomeRule : BiomeRule
{
	// Token: 0x17001AB9 RID: 6841
	// (get) Token: 0x06004D52 RID: 19794 RVA: 0x00029FD0 File Offset: 0x000281D0
	// (set) Token: 0x06004D53 RID: 19795 RVA: 0x00029FD7 File Offset: 0x000281D7
	public static float MaxHealthMod { get; private set; } = 1f;

	// Token: 0x17001ABA RID: 6842
	// (get) Token: 0x06004D54 RID: 19796 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004D55 RID: 19797 RVA: 0x00029FDF File Offset: 0x000281DF
	public override IEnumerator RunRule(BiomeType biome)
	{
		EnemyHPMod_BiomeRule.MaxHealthMod = 1f;
		if (SaveManager.PlayerSaveData.NewGamePlusLevel == 0)
		{
			EnemyHPMod_BiomeRule.MaxHealthMod = this.m_ng0MaxHealthMod;
		}
		else
		{
			EnemyHPMod_BiomeRule.MaxHealthMod = this.m_ngPlusMaxHealthMod;
		}
		yield break;
	}

	// Token: 0x06004D56 RID: 19798 RVA: 0x00029FEE File Offset: 0x000281EE
	public override void UndoRule(BiomeType biome)
	{
		EnemyHPMod_BiomeRule.MaxHealthMod = 1f;
	}

	// Token: 0x04003A86 RID: 14982
	[SerializeField]
	[FormerlySerializedAs("m_maxHealthMod")]
	private float m_ng0MaxHealthMod = 1f;

	// Token: 0x04003A87 RID: 14983
	[Tooltip("Applies to NG+1 and above")]
	[SerializeField]
	private float m_ngPlusMaxHealthMod = 1f;
}

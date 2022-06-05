using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000A06 RID: 2566
[CreateAssetMenu(menuName = "Custom/Biome Rules/Gameplay Mods/Enemy Damage Mod")]
public class EnemyDamageMod_BiomeRule : BiomeRule
{
	// Token: 0x17001AB5 RID: 6837
	// (get) Token: 0x06004D45 RID: 19781 RVA: 0x00029F65 File Offset: 0x00028165
	// (set) Token: 0x06004D46 RID: 19782 RVA: 0x00029F6C File Offset: 0x0002816C
	public static float StrengthMod { get; private set; } = 1f;

	// Token: 0x17001AB6 RID: 6838
	// (get) Token: 0x06004D47 RID: 19783 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004D48 RID: 19784 RVA: 0x00029F74 File Offset: 0x00028174
	public override IEnumerator RunRule(BiomeType biome)
	{
		EnemyDamageMod_BiomeRule.StrengthMod = 1f;
		if (SaveManager.PlayerSaveData.NewGamePlusLevel == 0)
		{
			EnemyDamageMod_BiomeRule.StrengthMod = this.m_ng0DamageMod;
		}
		else
		{
			EnemyDamageMod_BiomeRule.StrengthMod = this.m_ngPlusDamageMod;
		}
		yield break;
	}

	// Token: 0x06004D49 RID: 19785 RVA: 0x00029F83 File Offset: 0x00028183
	public override void UndoRule(BiomeType biome)
	{
		EnemyDamageMod_BiomeRule.StrengthMod = 1f;
	}

	// Token: 0x04003A80 RID: 14976
	[SerializeField]
	[FormerlySerializedAs("m_damageMod")]
	private float m_ng0DamageMod = 1f;

	// Token: 0x04003A81 RID: 14977
	[Tooltip("Applies to NG+1 and above")]
	[SerializeField]
	private float m_ngPlusDamageMod = 1f;
}

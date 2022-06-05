using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005F3 RID: 1523
[CreateAssetMenu(menuName = "Custom/Biome Rules/Gameplay Mods/Enemy Damage Mod")]
public class EnemyDamageMod_BiomeRule : BiomeRule
{
	// Token: 0x17001376 RID: 4982
	// (get) Token: 0x060036F8 RID: 14072 RVA: 0x000BCDEF File Offset: 0x000BAFEF
	// (set) Token: 0x060036F9 RID: 14073 RVA: 0x000BCDF6 File Offset: 0x000BAFF6
	public static float StrengthMod { get; private set; } = 1f;

	// Token: 0x17001377 RID: 4983
	// (get) Token: 0x060036FA RID: 14074 RVA: 0x000BCDFE File Offset: 0x000BAFFE
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x000BCE02 File Offset: 0x000BB002
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

	// Token: 0x060036FC RID: 14076 RVA: 0x000BCE11 File Offset: 0x000BB011
	public override void UndoRule(BiomeType biome)
	{
		EnemyDamageMod_BiomeRule.StrengthMod = 1f;
	}

	// Token: 0x04002A57 RID: 10839
	[SerializeField]
	[FormerlySerializedAs("m_damageMod")]
	private float m_ng0DamageMod = 1f;

	// Token: 0x04002A58 RID: 10840
	[Tooltip("Applies to NG+1 and above")]
	[SerializeField]
	private float m_ngPlusDamageMod = 1f;
}

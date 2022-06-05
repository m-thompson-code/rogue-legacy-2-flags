using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005F4 RID: 1524
[CreateAssetMenu(menuName = "Custom/Biome Rules/Gameplay Mods/Enemy HP Mod")]
public class EnemyHPMod_BiomeRule : BiomeRule
{
	// Token: 0x17001378 RID: 4984
	// (get) Token: 0x060036FF RID: 14079 RVA: 0x000BCE47 File Offset: 0x000BB047
	// (set) Token: 0x06003700 RID: 14080 RVA: 0x000BCE4E File Offset: 0x000BB04E
	public static float MaxHealthMod { get; private set; } = 1f;

	// Token: 0x17001379 RID: 4985
	// (get) Token: 0x06003701 RID: 14081 RVA: 0x000BCE56 File Offset: 0x000BB056
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x000BCE5A File Offset: 0x000BB05A
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

	// Token: 0x06003703 RID: 14083 RVA: 0x000BCE69 File Offset: 0x000BB069
	public override void UndoRule(BiomeType biome)
	{
		EnemyHPMod_BiomeRule.MaxHealthMod = 1f;
	}

	// Token: 0x04002A5A RID: 10842
	[SerializeField]
	[FormerlySerializedAs("m_maxHealthMod")]
	private float m_ng0MaxHealthMod = 1f;

	// Token: 0x04002A5B RID: 10843
	[Tooltip("Applies to NG+1 and above")]
	[SerializeField]
	private float m_ngPlusMaxHealthMod = 1f;
}

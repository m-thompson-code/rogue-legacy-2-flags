using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009F8 RID: 2552
[CreateAssetMenu(menuName = "Custom/Biome Rules/Gameplay Mods/Bridge Gold Gain")]
public class BridgeGoldGain_BiomeRule : BiomeRule
{
	// Token: 0x17001A9D RID: 6813
	// (get) Token: 0x06004CF3 RID: 19699 RVA: 0x00029D16 File Offset: 0x00027F16
	// (set) Token: 0x06004CF4 RID: 19700 RVA: 0x00029D1D File Offset: 0x00027F1D
	public static float GoldGainMod { get; private set; }

	// Token: 0x17001A9E RID: 6814
	// (get) Token: 0x06004CF5 RID: 19701 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004CF6 RID: 19702 RVA: 0x00029D25 File Offset: 0x00027F25
	public override IEnumerator RunRule(BiomeType biome)
	{
		BridgeGoldGain_BiomeRule.GoldGainMod = 0.2f;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
		if (biome != BiomeType.Stone && biome != BiomeType.Tower && biome != BiomeType.TowerExterior)
		{
			Debug.Log("<color=yellow>WARNING: The Bridge biome gold gain mod is being applied to a biome other than the Bridge or Tower.</color>");
		}
		yield break;
	}

	// Token: 0x06004CF7 RID: 19703 RVA: 0x00029D34 File Offset: 0x00027F34
	public override void UndoRule(BiomeType biome)
	{
		BridgeGoldGain_BiomeRule.GoldGainMod = 0f;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
	}
}

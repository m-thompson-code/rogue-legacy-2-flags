using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005EE RID: 1518
[CreateAssetMenu(menuName = "Custom/Biome Rules/Gameplay Mods/Bridge Gold Gain")]
public class BridgeGoldGain_BiomeRule : BiomeRule
{
	// Token: 0x1700136C RID: 4972
	// (get) Token: 0x060036D5 RID: 14037 RVA: 0x000BCA07 File Offset: 0x000BAC07
	// (set) Token: 0x060036D6 RID: 14038 RVA: 0x000BCA0E File Offset: 0x000BAC0E
	public static float GoldGainMod { get; private set; }

	// Token: 0x1700136D RID: 4973
	// (get) Token: 0x060036D7 RID: 14039 RVA: 0x000BCA16 File Offset: 0x000BAC16
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x000BCA1A File Offset: 0x000BAC1A
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

	// Token: 0x060036D9 RID: 14041 RVA: 0x000BCA29 File Offset: 0x000BAC29
	public override void UndoRule(BiomeType biome)
	{
		BridgeGoldGain_BiomeRule.GoldGainMod = 0f;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
	}
}

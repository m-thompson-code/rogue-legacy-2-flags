using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007D RID: 125
public static class NPC_EV
{
	// Token: 0x060001BF RID: 447 RVA: 0x0004CCB8 File Offset: 0x0004AEB8
	public static float GetArchitectGoldMod(int timesCastleLocked = -1)
	{
		if (timesCastleLocked == -1)
		{
			timesCastleLocked = (int)SaveManager.PlayerSaveData.TimesCastleLocked;
		}
		float architectCostMods = SkillTreeLogicHelper.GetArchitectCostMods();
		if (timesCastleLocked >= 1)
		{
			return Mathf.Clamp(1f - (0f + (float)timesCastleLocked * (0.2f - architectCostMods)), 0.3f, 1f);
		}
		return 1f;
	}

	// Token: 0x040003EF RID: 1007
	public const float TELEPORTER_NG_COST_MULTIPLIER = 2.5f;

	// Token: 0x040003F0 RID: 1008
	public const float TELEPORTER_NG_COST_EXPONENTIAL = 250f;

	// Token: 0x040003F1 RID: 1009
	public const float ARCHITECT_BASE_PRICE_MOD = 0f;

	// Token: 0x040003F2 RID: 1010
	public const float ARCHITECT_GOLD_REDUCTION_MOD = 0.2f;

	// Token: 0x040003F3 RID: 1011
	public const float ARCHITECT_MIN_GOLD_MOD = 0.3f;

	// Token: 0x040003F4 RID: 1012
	public static Dictionary<BiomeType, int> PIZZA_GIRL_TELEPORTER_COST_TABLE = new Dictionary<BiomeType, int>
	{
		{
			BiomeType.Stone,
			1750
		},
		{
			BiomeType.Study,
			1750
		},
		{
			BiomeType.Cave,
			1750
		},
		{
			BiomeType.Tower,
			1750
		},
		{
			BiomeType.Forest,
			1750
		}
	};
}

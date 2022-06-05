using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000075 RID: 117
public static class NPC_EV
{
	// Token: 0x060001AB RID: 427 RVA: 0x000107A8 File Offset: 0x0000E9A8
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

	// Token: 0x040003CE RID: 974
	public const float TELEPORTER_NG_COST_MULTIPLIER = 2.5f;

	// Token: 0x040003CF RID: 975
	public const float TELEPORTER_NG_COST_EXPONENTIAL = 250f;

	// Token: 0x040003D0 RID: 976
	public const float ARCHITECT_BASE_PRICE_MOD = 0f;

	// Token: 0x040003D1 RID: 977
	public const float ARCHITECT_GOLD_REDUCTION_MOD = 0.2f;

	// Token: 0x040003D2 RID: 978
	public const float ARCHITECT_MIN_GOLD_MOD = 0.3f;

	// Token: 0x040003D3 RID: 979
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

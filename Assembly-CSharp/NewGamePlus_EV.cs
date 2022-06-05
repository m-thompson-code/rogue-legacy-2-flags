using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class NewGamePlus_EV
{
	// Token: 0x060001C1 RID: 449 RVA: 0x0004CD68 File Offset: 0x0004AF68
	public static int GetBurdensRequiredForNG(int ngLevel)
	{
		int result = 0;
		if (ngLevel > 0)
		{
			result = 2 * Mathf.Min(10, ngLevel) + Mathf.Max(0, ngLevel - 10);
		}
		return result;
	}

	// Token: 0x040003F5 RID: 1013
	public const int BURDEN_COST_PER_NG = 2;

	// Token: 0x040003F6 RID: 1014
	public const int BURDEN_COST_PER_NG_SLOW_SCALING = 1;

	// Token: 0x040003F7 RID: 1015
	public const int BURDEN_SCALING_NG_TRIGGER = 10;

	// Token: 0x040003F8 RID: 1016
	public const int MAX_NG_PLUS = 100;

	// Token: 0x040003F9 RID: 1017
	public const int NEWGAMEPLUS_BURDEN_CAP = 30;

	// Token: 0x040003FA RID: 1018
	public const int ROOM_LEVEL_GAIN_PER_NG = 44;

	// Token: 0x040003FB RID: 1019
	public const bool NEWGAMEPLUS_TELEPORT_TO_HUBTOWN = false;

	// Token: 0x040003FC RID: 1020
	public const float ENEMY_PROJECTILE_SPEED_TURN_MOD = 1.07f;

	// Token: 0x040003FD RID: 1021
	public const float ENEMY_HEALTH_MOD_PER_NG = 0.01f;

	// Token: 0x040003FE RID: 1022
	public const float ENEMY_DAMAGE_MOD_PER_NG = 0.007f;

	// Token: 0x040003FF RID: 1023
	public static readonly BurdenType[] BURDEN_ORDER = new BurdenType[]
	{
		BurdenType.EnemyDamage,
		BurdenType.EnemyHealth,
		BurdenType.EnemyEvolve,
		BurdenType.EnemyLifesteal,
		BurdenType.EnemyArmorShred,
		BurdenType.EnemyAdapt,
		BurdenType.EnemyAggression,
		BurdenType.EnemySpeed,
		BurdenType.EnemyProjectiles,
		BurdenType.RoomThreat,
		BurdenType.RoomCount,
		BurdenType.CommanderTraits,
		BurdenType.BossPower,
		BurdenType.CastleBossUp,
		BurdenType.CastleBiomeUp,
		BurdenType.BridgeBossUp,
		BurdenType.BridgeBiomeUp,
		BurdenType.ForestBossUp,
		BurdenType.ForestBiomeUp,
		BurdenType.StudyBossUp,
		BurdenType.StudyBiomeUp,
		BurdenType.TowerBossUp,
		BurdenType.TowerBiomeUp,
		BurdenType.CaveBossUp,
		BurdenType.CaveBiomeUp,
		BurdenType.GardenBossUp,
		BurdenType.FinalBossUp
	};
}

using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class NewGamePlus_EV
{
	// Token: 0x060001AD RID: 429 RVA: 0x00010858 File Offset: 0x0000EA58
	public static int GetBurdensRequiredForNG(int ngLevel)
	{
		int result = 0;
		if (ngLevel > 0)
		{
			result = 2 * Mathf.Min(10, ngLevel) + Mathf.Max(0, ngLevel - 10);
		}
		return result;
	}

	// Token: 0x040003D4 RID: 980
	public const int BURDEN_COST_PER_NG = 2;

	// Token: 0x040003D5 RID: 981
	public const int BURDEN_COST_PER_NG_SLOW_SCALING = 1;

	// Token: 0x040003D6 RID: 982
	public const int BURDEN_SCALING_NG_TRIGGER = 10;

	// Token: 0x040003D7 RID: 983
	public const int MAX_NG_PLUS = 100;

	// Token: 0x040003D8 RID: 984
	public const int NEWGAMEPLUS_BURDEN_CAP = 30;

	// Token: 0x040003D9 RID: 985
	public const int ROOM_LEVEL_GAIN_PER_NG = 44;

	// Token: 0x040003DA RID: 986
	public const bool NEWGAMEPLUS_TELEPORT_TO_HUBTOWN = false;

	// Token: 0x040003DB RID: 987
	public const float ENEMY_PROJECTILE_SPEED_TURN_MOD = 1.07f;

	// Token: 0x040003DC RID: 988
	public const float ENEMY_HEALTH_MOD_PER_NG = 0.01f;

	// Token: 0x040003DD RID: 989
	public const float ENEMY_DAMAGE_MOD_PER_NG = 0.007f;

	// Token: 0x040003DE RID: 990
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

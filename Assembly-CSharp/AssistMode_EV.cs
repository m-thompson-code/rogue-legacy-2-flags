using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public static class AssistMode_EV
{
	// Token: 0x06000138 RID: 312 RVA: 0x0000B714 File Offset: 0x00009914
	public static int GetDifficulty()
	{
		int num = 5000;
		int num2 = 100 - Mathf.RoundToInt(SaveManager.PlayerSaveData.Assist_EnemyDamageMod * 100f);
		num -= num2 * 10;
		int num3 = 100 - Mathf.RoundToInt(SaveManager.PlayerSaveData.Assist_EnemyHealthMod * 100f);
		num -= num3 * 10;
		int num4 = 100 - Mathf.RoundToInt(SaveManager.PlayerSaveData.Assist_AimTimeSlow * 100f);
		num -= num4 * 50;
		int num5 = 100 - Mathf.RoundToInt(SaveManager.PlayerSaveData.Assist_BurdenRequirementsMod * 100f);
		num -= num5 * 5;
		if (SaveManager.PlayerSaveData.Assist_EnableFlightToggle)
		{
			num -= 400;
		}
		if (SaveManager.PlayerSaveData.Assist_DisableEnemyContactDamage)
		{
			num -= 1000;
		}
		return num + BurdenManager.GetTotalBurdenLevel() * 100;
	}

	// Token: 0x040001F6 RID: 502
	public const int ENEMY_DAMAGE_SCALE_AMOUNT = 5;

	// Token: 0x040001F7 RID: 503
	public const int ENEMY_DAMAGE_MIN_AMOUNT = 50;

	// Token: 0x040001F8 RID: 504
	public const int ENEMY_DAMAGE_MAX_AMOUNT = 200;

	// Token: 0x040001F9 RID: 505
	public const int ENEMY_HEALTH_SCALE_AMOUNT = 5;

	// Token: 0x040001FA RID: 506
	public const int ENEMY_HEALTH_MIN_AMOUNT = 50;

	// Token: 0x040001FB RID: 507
	public const int ENEMY_HEALTH_MAX_AMOUNT = 200;

	// Token: 0x040001FC RID: 508
	public const int AIM_TIME_SLOW_SCALE_AMOUNT = 5;

	// Token: 0x040001FD RID: 509
	public const int AIM_TIME_SLOW_MIN_AMOUNT = 25;

	// Token: 0x040001FE RID: 510
	public const int AIM_TIME_SLOW_MAX_AMOUNT = 100;

	// Token: 0x040001FF RID: 511
	public const int BURDEN_REQUIREMENT_SCALE_AMOUNT = 50;

	// Token: 0x04000200 RID: 512
	public const int BURDEN_REQUIREMENT_MIN_AMOUNT = 50;

	// Token: 0x04000201 RID: 513
	public const int BURDEN_REQUIREMENT_MAX_AMOUNT = 200;

	// Token: 0x04000202 RID: 514
	public const bool BURDEN_REQUIREMENT_MODFIES_MAX_BURDENS = true;

	// Token: 0x04000203 RID: 515
	public const int DIFFICULTY_INITIAL_VALUE = 5000;

	// Token: 0x04000204 RID: 516
	public const int DIFFICULTY_ENEMY_DAMAGE_MOD = 10;

	// Token: 0x04000205 RID: 517
	public const int DIFFICULTY_ENEMY_HEALTH_MOD = 10;

	// Token: 0x04000206 RID: 518
	public const int DIFFICULTY_BURDEN_REQUIREMENT_MOD = 5;

	// Token: 0x04000207 RID: 519
	public const int DIFFICULTY_AIM_TIME_SLOW_MOD = 50;

	// Token: 0x04000208 RID: 520
	public const int DIFFICULTY_TOGGLE_FLIGHT_ADD = 400;

	// Token: 0x04000209 RID: 521
	public const int DIFFICULTY_TOGGLE_NO_COLLISION_ADD = 1000;

	// Token: 0x0400020A RID: 522
	public const int DIFFICULTY_PER_BURDEN_ADD = 100;
}

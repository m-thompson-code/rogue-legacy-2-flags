using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public static class AssistMode_EV
{
	// Token: 0x0600014C RID: 332 RVA: 0x000480BC File Offset: 0x000462BC
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

	// Token: 0x04000217 RID: 535
	public const int ENEMY_DAMAGE_SCALE_AMOUNT = 5;

	// Token: 0x04000218 RID: 536
	public const int ENEMY_DAMAGE_MIN_AMOUNT = 50;

	// Token: 0x04000219 RID: 537
	public const int ENEMY_DAMAGE_MAX_AMOUNT = 200;

	// Token: 0x0400021A RID: 538
	public const int ENEMY_HEALTH_SCALE_AMOUNT = 5;

	// Token: 0x0400021B RID: 539
	public const int ENEMY_HEALTH_MIN_AMOUNT = 50;

	// Token: 0x0400021C RID: 540
	public const int ENEMY_HEALTH_MAX_AMOUNT = 200;

	// Token: 0x0400021D RID: 541
	public const int AIM_TIME_SLOW_SCALE_AMOUNT = 5;

	// Token: 0x0400021E RID: 542
	public const int AIM_TIME_SLOW_MIN_AMOUNT = 25;

	// Token: 0x0400021F RID: 543
	public const int AIM_TIME_SLOW_MAX_AMOUNT = 100;

	// Token: 0x04000220 RID: 544
	public const int BURDEN_REQUIREMENT_SCALE_AMOUNT = 50;

	// Token: 0x04000221 RID: 545
	public const int BURDEN_REQUIREMENT_MIN_AMOUNT = 50;

	// Token: 0x04000222 RID: 546
	public const int BURDEN_REQUIREMENT_MAX_AMOUNT = 200;

	// Token: 0x04000223 RID: 547
	public const bool BURDEN_REQUIREMENT_MODFIES_MAX_BURDENS = true;

	// Token: 0x04000224 RID: 548
	public const int DIFFICULTY_INITIAL_VALUE = 5000;

	// Token: 0x04000225 RID: 549
	public const int DIFFICULTY_ENEMY_DAMAGE_MOD = 10;

	// Token: 0x04000226 RID: 550
	public const int DIFFICULTY_ENEMY_HEALTH_MOD = 10;

	// Token: 0x04000227 RID: 551
	public const int DIFFICULTY_BURDEN_REQUIREMENT_MOD = 5;

	// Token: 0x04000228 RID: 552
	public const int DIFFICULTY_AIM_TIME_SLOW_MOD = 50;

	// Token: 0x04000229 RID: 553
	public const int DIFFICULTY_TOGGLE_FLIGHT_ADD = 400;

	// Token: 0x0400022A RID: 554
	public const int DIFFICULTY_TOGGLE_NO_COLLISION_ADD = 1000;

	// Token: 0x0400022B RID: 555
	public const int DIFFICULTY_PER_BURDEN_ADD = 100;
}

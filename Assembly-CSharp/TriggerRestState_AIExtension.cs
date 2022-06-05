using System;
using UnityEngine;

// Token: 0x02000293 RID: 659
public static class TriggerRestState_AIExtension
{
	// Token: 0x060012C4 RID: 4804 RVA: 0x00082CB8 File Offset: 0x00080EB8
	public static void TriggerRestState(this BaseAIScript aiScript, Vector2 minMax, bool ignoreMods)
	{
		float duration = UnityEngine.Random.Range(aiScript.EnemyController.ActualRestCooldown.x, aiScript.EnemyController.ActualRestCooldown.y);
		aiScript.LogicController.EnterRestState(duration, ignoreMods);
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x0000986C File Offset: 0x00007A6C
	public static void TriggerRestState(this BaseAIScript aiScript, bool ignoreMods)
	{
		aiScript.TriggerRestState(aiScript.EnemyController.ActualRestCooldown, ignoreMods);
	}
}

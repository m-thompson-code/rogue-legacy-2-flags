using System;
using UnityEngine;

// Token: 0x02000164 RID: 356
public static class TriggerRestState_AIExtension
{
	// Token: 0x06000BDB RID: 3035 RVA: 0x00023CC0 File Offset: 0x00021EC0
	public static void TriggerRestState(this BaseAIScript aiScript, Vector2 minMax, bool ignoreMods)
	{
		float duration = UnityEngine.Random.Range(aiScript.EnemyController.ActualRestCooldown.x, aiScript.EnemyController.ActualRestCooldown.y);
		aiScript.LogicController.EnterRestState(duration, ignoreMods);
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x00023D00 File Offset: 0x00021F00
	public static void TriggerRestState(this BaseAIScript aiScript, bool ignoreMods)
	{
		aiScript.TriggerRestState(aiScript.EnemyController.ActualRestCooldown, ignoreMods);
	}
}

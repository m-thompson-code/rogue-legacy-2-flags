using System;

// Token: 0x02000292 RID: 658
public static class TriggerLBCooldown_AIExtension
{
	// Token: 0x060012C3 RID: 4803 RVA: 0x00009852 File Offset: 0x00007A52
	public static void TriggerAttackCooldown(this BaseAIScript aiScript, float cooldown, bool ignoreMods)
	{
		aiScript.LogicController.SetLBCooldown(aiScript.LogicController.CurrentLogicBlockName, cooldown, ignoreMods);
	}
}

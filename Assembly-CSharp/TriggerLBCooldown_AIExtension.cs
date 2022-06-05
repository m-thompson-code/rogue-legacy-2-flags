using System;

// Token: 0x02000163 RID: 355
public static class TriggerLBCooldown_AIExtension
{
	// Token: 0x06000BDA RID: 3034 RVA: 0x00023CA3 File Offset: 0x00021EA3
	public static void TriggerAttackCooldown(this BaseAIScript aiScript, float cooldown, bool ignoreMods)
	{
		aiScript.LogicController.SetLBCooldown(aiScript.LogicController.CurrentLogicBlockName, cooldown, ignoreMods);
	}
}

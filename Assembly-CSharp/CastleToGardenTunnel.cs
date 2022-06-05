using System;

// Token: 0x02000564 RID: 1380
public class CastleToGardenTunnel : BossTunnel
{
	// Token: 0x060032AD RID: 12973 RVA: 0x000AB725 File Offset: 0x000A9925
	protected override PlayerSaveFlag GetBossFreeHealUsedFlag()
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Defeated))
		{
			return PlayerSaveFlag.GardenBoss_FreeHeal_Used;
		}
		return PlayerSaveFlag.FinalBoss_FreeHeal_Used;
	}
}

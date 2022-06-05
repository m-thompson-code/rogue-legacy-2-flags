using System;

// Token: 0x02000934 RID: 2356
public class CastleToGardenTunnel : BossTunnel
{
	// Token: 0x06004782 RID: 18306 RVA: 0x00027346 File Offset: 0x00025546
	protected override PlayerSaveFlag GetBossFreeHealUsedFlag()
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Defeated))
		{
			return PlayerSaveFlag.GardenBoss_FreeHeal_Used;
		}
		return PlayerSaveFlag.FinalBoss_FreeHeal_Used;
	}
}

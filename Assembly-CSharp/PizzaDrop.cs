using System;

// Token: 0x0200076D RID: 1901
public class PizzaDrop : HealthDrop
{
	// Token: 0x1700157B RID: 5499
	// (get) Token: 0x060039E8 RID: 14824 RVA: 0x0001FD3D File Offset: 0x0001DF3D
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.PizzaDrop;
		}
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x000EC464 File Offset: 0x000EA664
	protected override void GainHealth(float hpGain = 0f)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController)
		{
			float num = (float)SaveManager.PlayerSaveData.GetRelic(RelicType.FoodHealsMore).Level * 0.08f;
			hpGain = (float)playerController.ActualMaxHealth * (0f + num + SkillTreeLogicHelper.GetPotionMods() + 0.2f);
			float num2 = playerController.ActualMagic * 2f;
			hpGain += num2;
			base.GainHealth(hpGain);
		}
	}
}

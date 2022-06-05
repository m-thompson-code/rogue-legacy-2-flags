using System;

// Token: 0x02000477 RID: 1143
public class PizzaDrop : HealthDrop
{
	// Token: 0x17001046 RID: 4166
	// (get) Token: 0x060029DE RID: 10718 RVA: 0x0008A7E5 File Offset: 0x000889E5
	public override ItemDropType ItemDropType
	{
		get
		{
			return ItemDropType.PizzaDrop;
		}
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x0008A7EC File Offset: 0x000889EC
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

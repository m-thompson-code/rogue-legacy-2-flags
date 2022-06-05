using System;

// Token: 0x02000361 RID: 865
public class SuperFart_Trait : BaseTrait
{
	// Token: 0x17000DF2 RID: 3570
	// (get) Token: 0x06002099 RID: 8345 RVA: 0x00066BE9 File Offset: 0x00064DE9
	public override TraitType TraitType
	{
		get
		{
			return TraitType.SuperFart;
		}
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x00066BF0 File Offset: 0x00064DF0
	private void Start()
	{
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.TalentSwap).Level <= 0)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			SaveManager.PlayerSaveData.CurrentCharacter.Talent = AbilityType.SuperFart;
			playerController.CharacterClass.SetAbility(CastAbilityType.Talent, AbilityType.SuperFart, true);
		}
	}

	// Token: 0x04001C61 RID: 7265
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001C62 RID: 7266
	private bool m_isFarting;
}

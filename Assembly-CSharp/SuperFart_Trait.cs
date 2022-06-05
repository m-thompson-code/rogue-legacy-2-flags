using System;

// Token: 0x020005D7 RID: 1495
public class SuperFart_Trait : BaseTrait
{
	// Token: 0x1700125D RID: 4701
	// (get) Token: 0x06002E32 RID: 11826 RVA: 0x00019493 File Offset: 0x00017693
	public override TraitType TraitType
	{
		get
		{
			return TraitType.SuperFart;
		}
	}

	// Token: 0x06002E33 RID: 11827 RVA: 0x000C7A30 File Offset: 0x000C5C30
	private void Start()
	{
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.TalentSwap).Level <= 0)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			SaveManager.PlayerSaveData.CurrentCharacter.Talent = AbilityType.SuperFart;
			playerController.CharacterClass.SetAbility(CastAbilityType.Talent, AbilityType.SuperFart, true);
		}
	}

	// Token: 0x040025FB RID: 9723
	private WaitRL_Yield m_waitYield;

	// Token: 0x040025FC RID: 9724
	private bool m_isFarting;
}

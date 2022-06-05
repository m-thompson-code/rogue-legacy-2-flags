using System;

// Token: 0x0200057C RID: 1404
public class CantAttack_Trait : BaseTrait
{
	// Token: 0x170011E6 RID: 4582
	// (get) Token: 0x06002C9C RID: 11420 RVA: 0x00017A13 File Offset: 0x00015C13
	public override TraitType TraitType
	{
		get
		{
			return TraitType.CantAttack;
		}
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x00018B05 File Offset: 0x00016D05
	private void Start()
	{
		if (SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponSwap).Level <= 0)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			SaveManager.PlayerSaveData.CurrentCharacter.Weapon = AbilityType.PacifistWeapon;
			playerController.CharacterClass.SetAbility(CastAbilityType.Weapon, AbilityType.PacifistWeapon, true);
		}
	}
}

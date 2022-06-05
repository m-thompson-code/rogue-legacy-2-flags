using System;

// Token: 0x0200032B RID: 811
public class CantAttack_Trait : BaseTrait
{
	// Token: 0x17000DAF RID: 3503
	// (get) Token: 0x06001FBF RID: 8127 RVA: 0x000653EA File Offset: 0x000635EA
	public override TraitType TraitType
	{
		get
		{
			return TraitType.CantAttack;
		}
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000653F1 File Offset: 0x000635F1
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

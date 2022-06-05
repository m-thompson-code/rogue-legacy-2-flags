using System;

// Token: 0x020007E6 RID: 2022
public class ChangeAbilityEventArgs : EventArgs
{
	// Token: 0x06004372 RID: 17266 RVA: 0x000EC7F4 File Offset: 0x000EA9F4
	public ChangeAbilityEventArgs(CastAbilityType castAbilityType, IAbility ability)
	{
		this.Initialise(castAbilityType, ability);
	}

	// Token: 0x06004373 RID: 17267 RVA: 0x000EC804 File Offset: 0x000EAA04
	public void Initialise(CastAbilityType castAbilityType, IAbility ability)
	{
		this.CastAbilityType = castAbilityType;
		this.Ability = ability;
	}

	// Token: 0x170016D5 RID: 5845
	// (get) Token: 0x06004374 RID: 17268 RVA: 0x000EC814 File Offset: 0x000EAA14
	// (set) Token: 0x06004375 RID: 17269 RVA: 0x000EC81C File Offset: 0x000EAA1C
	public CastAbilityType CastAbilityType { get; private set; }

	// Token: 0x170016D6 RID: 5846
	// (get) Token: 0x06004376 RID: 17270 RVA: 0x000EC825 File Offset: 0x000EAA25
	// (set) Token: 0x06004377 RID: 17271 RVA: 0x000EC82D File Offset: 0x000EAA2D
	public IAbility Ability { get; private set; }
}

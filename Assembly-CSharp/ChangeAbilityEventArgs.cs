using System;

// Token: 0x02000CAC RID: 3244
public class ChangeAbilityEventArgs : EventArgs
{
	// Token: 0x06005CFB RID: 23803 RVA: 0x0003320A File Offset: 0x0003140A
	public ChangeAbilityEventArgs(CastAbilityType castAbilityType, IAbility ability)
	{
		this.Initialise(castAbilityType, ability);
	}

	// Token: 0x06005CFC RID: 23804 RVA: 0x0003321A File Offset: 0x0003141A
	public void Initialise(CastAbilityType castAbilityType, IAbility ability)
	{
		this.CastAbilityType = castAbilityType;
		this.Ability = ability;
	}

	// Token: 0x17001ED3 RID: 7891
	// (get) Token: 0x06005CFD RID: 23805 RVA: 0x0003322A File Offset: 0x0003142A
	// (set) Token: 0x06005CFE RID: 23806 RVA: 0x00033232 File Offset: 0x00031432
	public CastAbilityType CastAbilityType { get; private set; }

	// Token: 0x17001ED4 RID: 7892
	// (get) Token: 0x06005CFF RID: 23807 RVA: 0x0003323B File Offset: 0x0003143B
	// (set) Token: 0x06005D00 RID: 23808 RVA: 0x00033243 File Offset: 0x00031443
	public IAbility Ability { get; private set; }
}

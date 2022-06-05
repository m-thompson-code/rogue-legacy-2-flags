using System;

// Token: 0x020007E1 RID: 2017
public class AbilityUsedEventArgs : EventArgs
{
	// Token: 0x06004358 RID: 17240 RVA: 0x000EC6DC File Offset: 0x000EA8DC
	public AbilityUsedEventArgs(IAbility ability)
	{
		this.Initialise(ability);
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x000EC6EB File Offset: 0x000EA8EB
	public void Initialise(IAbility ability)
	{
		this.Ability = ability;
	}

	// Token: 0x170016CD RID: 5837
	// (get) Token: 0x0600435A RID: 17242 RVA: 0x000EC6F4 File Offset: 0x000EA8F4
	// (set) Token: 0x0600435B RID: 17243 RVA: 0x000EC6FC File Offset: 0x000EA8FC
	public IAbility Ability { get; private set; }
}

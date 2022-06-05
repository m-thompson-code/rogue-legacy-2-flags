using System;

// Token: 0x020007E2 RID: 2018
public class AbilityCooldownOverEventArgs : EventArgs
{
	// Token: 0x0600435C RID: 17244 RVA: 0x000EC705 File Offset: 0x000EA905
	public AbilityCooldownOverEventArgs(IAbility ability)
	{
		this.Initialise(ability);
	}

	// Token: 0x0600435D RID: 17245 RVA: 0x000EC714 File Offset: 0x000EA914
	public void Initialise(IAbility ability)
	{
		this.Ability = ability;
	}

	// Token: 0x170016CE RID: 5838
	// (get) Token: 0x0600435E RID: 17246 RVA: 0x000EC71D File Offset: 0x000EA91D
	// (set) Token: 0x0600435F RID: 17247 RVA: 0x000EC725 File Offset: 0x000EA925
	public IAbility Ability { get; private set; }
}

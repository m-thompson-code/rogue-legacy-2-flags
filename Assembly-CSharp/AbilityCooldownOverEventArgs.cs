using System;

// Token: 0x02000CA8 RID: 3240
public class AbilityCooldownOverEventArgs : EventArgs
{
	// Token: 0x06005CE5 RID: 23781 RVA: 0x0003311B File Offset: 0x0003131B
	public AbilityCooldownOverEventArgs(IAbility ability)
	{
		this.Initialise(ability);
	}

	// Token: 0x06005CE6 RID: 23782 RVA: 0x0003312A File Offset: 0x0003132A
	public void Initialise(IAbility ability)
	{
		this.Ability = ability;
	}

	// Token: 0x17001ECC RID: 7884
	// (get) Token: 0x06005CE7 RID: 23783 RVA: 0x00033133 File Offset: 0x00031333
	// (set) Token: 0x06005CE8 RID: 23784 RVA: 0x0003313B File Offset: 0x0003133B
	public IAbility Ability { get; private set; }
}

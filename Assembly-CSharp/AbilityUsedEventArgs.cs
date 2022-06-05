using System;

// Token: 0x02000CA7 RID: 3239
public class AbilityUsedEventArgs : EventArgs
{
	// Token: 0x06005CE1 RID: 23777 RVA: 0x000330F2 File Offset: 0x000312F2
	public AbilityUsedEventArgs(IAbility ability)
	{
		this.Initialise(ability);
	}

	// Token: 0x06005CE2 RID: 23778 RVA: 0x00033101 File Offset: 0x00031301
	public void Initialise(IAbility ability)
	{
		this.Ability = ability;
	}

	// Token: 0x17001ECB RID: 7883
	// (get) Token: 0x06005CE3 RID: 23779 RVA: 0x0003310A File Offset: 0x0003130A
	// (set) Token: 0x06005CE4 RID: 23780 RVA: 0x00033112 File Offset: 0x00031312
	public IAbility Ability { get; private set; }
}

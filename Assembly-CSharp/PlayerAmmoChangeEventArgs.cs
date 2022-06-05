using System;

// Token: 0x020007B7 RID: 1975
public class PlayerAmmoChangeEventArgs : EventArgs
{
	// Token: 0x0600426B RID: 17003 RVA: 0x000EBC9C File Offset: 0x000E9E9C
	public PlayerAmmoChangeEventArgs(IAbility ability, float newAmmo, float prevAmmo)
	{
		this.Initialize(ability, newAmmo, prevAmmo);
	}

	// Token: 0x0600426C RID: 17004 RVA: 0x000EBCAD File Offset: 0x000E9EAD
	public void Initialize(IAbility ability, float newAmmo, float prevAmmo)
	{
		this.AbilityObj = ability;
		this.NewAmmoValue = newAmmo;
		this.PrevAmmoValue = prevAmmo;
	}

	// Token: 0x1700167D RID: 5757
	// (get) Token: 0x0600426D RID: 17005 RVA: 0x000EBCC4 File Offset: 0x000E9EC4
	// (set) Token: 0x0600426E RID: 17006 RVA: 0x000EBCCC File Offset: 0x000E9ECC
	public IAbility AbilityObj { get; private set; }

	// Token: 0x1700167E RID: 5758
	// (get) Token: 0x0600426F RID: 17007 RVA: 0x000EBCD5 File Offset: 0x000E9ED5
	// (set) Token: 0x06004270 RID: 17008 RVA: 0x000EBCDD File Offset: 0x000E9EDD
	public float NewAmmoValue { get; private set; }

	// Token: 0x1700167F RID: 5759
	// (get) Token: 0x06004271 RID: 17009 RVA: 0x000EBCE6 File Offset: 0x000E9EE6
	// (set) Token: 0x06004272 RID: 17010 RVA: 0x000EBCEE File Offset: 0x000E9EEE
	public float PrevAmmoValue { get; private set; }
}

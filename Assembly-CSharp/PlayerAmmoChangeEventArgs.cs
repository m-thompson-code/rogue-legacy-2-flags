using System;

// Token: 0x02000C7D RID: 3197
public class PlayerAmmoChangeEventArgs : EventArgs
{
	// Token: 0x06005BF4 RID: 23540 RVA: 0x000326B2 File Offset: 0x000308B2
	public PlayerAmmoChangeEventArgs(IAbility ability, float newAmmo, float prevAmmo)
	{
		this.Initialize(ability, newAmmo, prevAmmo);
	}

	// Token: 0x06005BF5 RID: 23541 RVA: 0x000326C3 File Offset: 0x000308C3
	public void Initialize(IAbility ability, float newAmmo, float prevAmmo)
	{
		this.AbilityObj = ability;
		this.NewAmmoValue = newAmmo;
		this.PrevAmmoValue = prevAmmo;
	}

	// Token: 0x17001E7B RID: 7803
	// (get) Token: 0x06005BF6 RID: 23542 RVA: 0x000326DA File Offset: 0x000308DA
	// (set) Token: 0x06005BF7 RID: 23543 RVA: 0x000326E2 File Offset: 0x000308E2
	public IAbility AbilityObj { get; private set; }

	// Token: 0x17001E7C RID: 7804
	// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x000326EB File Offset: 0x000308EB
	// (set) Token: 0x06005BF9 RID: 23545 RVA: 0x000326F3 File Offset: 0x000308F3
	public float NewAmmoValue { get; private set; }

	// Token: 0x17001E7D RID: 7805
	// (get) Token: 0x06005BFA RID: 23546 RVA: 0x000326FC File Offset: 0x000308FC
	// (set) Token: 0x06005BFB RID: 23547 RVA: 0x00032704 File Offset: 0x00030904
	public float PrevAmmoValue { get; private set; }
}

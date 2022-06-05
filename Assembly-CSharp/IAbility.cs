using System;

// Token: 0x0200059D RID: 1437
public interface IAbility
{
	// Token: 0x170012F7 RID: 4855
	// (get) Token: 0x06003604 RID: 13828
	AbilityData AbilityData { get; }

	// Token: 0x170012F8 RID: 4856
	// (get) Token: 0x06003605 RID: 13829
	AbilityType AbilityType { get; }

	// Token: 0x170012F9 RID: 4857
	// (get) Token: 0x06003606 RID: 13830
	CastAbilityType CastAbilityType { get; }

	// Token: 0x170012FA RID: 4858
	// (get) Token: 0x06003607 RID: 13831
	int CurrentAmmo { get; }

	// Token: 0x170012FB RID: 4859
	// (get) Token: 0x06003608 RID: 13832
	int MaxAmmo { get; }

	// Token: 0x170012FC RID: 4860
	// (get) Token: 0x06003609 RID: 13833
	int BaseCost { get; }

	// Token: 0x170012FD RID: 4861
	// (get) Token: 0x0600360A RID: 13834
	bool DealsNoDamage { get; }

	// Token: 0x170012FE RID: 4862
	// (get) Token: 0x0600360B RID: 13835
	// (set) Token: 0x0600360C RID: 13836
	bool ForceTriggerCrit { get; set; }
}

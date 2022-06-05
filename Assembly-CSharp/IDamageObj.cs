using System;
using UnityEngine;

// Token: 0x020005AC RID: 1452
public interface IDamageObj
{
	// Token: 0x1700130A RID: 4874
	// (get) Token: 0x0600361C RID: 13852
	GameObject gameObject { get; }

	// Token: 0x1700130B RID: 4875
	// (get) Token: 0x0600361D RID: 13853
	float BaseDamage { get; }

	// Token: 0x1700130C RID: 4876
	// (get) Token: 0x0600361E RID: 13854
	float ActualDamage { get; }

	// Token: 0x1700130D RID: 4877
	// (get) Token: 0x0600361F RID: 13855
	float ActualCritChance { get; }

	// Token: 0x1700130E RID: 4878
	// (get) Token: 0x06003620 RID: 13856
	float ActualCritDamage { get; }

	// Token: 0x1700130F RID: 4879
	// (get) Token: 0x06003621 RID: 13857
	Vector2 ExternalKnockbackMod { get; }

	// Token: 0x17001310 RID: 4880
	// (get) Token: 0x06003622 RID: 13858
	// (set) Token: 0x06003623 RID: 13859
	float BaseKnockbackStrength { get; set; }

	// Token: 0x17001311 RID: 4881
	// (get) Token: 0x06003624 RID: 13860
	float ActualKnockbackStrength { get; }

	// Token: 0x17001312 RID: 4882
	// (get) Token: 0x06003625 RID: 13861
	// (set) Token: 0x06003626 RID: 13862
	float BaseStunStrength { get; set; }

	// Token: 0x17001313 RID: 4883
	// (get) Token: 0x06003627 RID: 13863
	float ActualStunStrength { get; }

	// Token: 0x17001314 RID: 4884
	// (get) Token: 0x06003628 RID: 13864
	string RelicDamageTypeString { get; }

	// Token: 0x17001315 RID: 4885
	// (get) Token: 0x06003629 RID: 13865
	StrikeType StrikeType { get; }

	// Token: 0x17001316 RID: 4886
	// (get) Token: 0x0600362A RID: 13866
	bool IsDotDamage { get; }

	// Token: 0x17001317 RID: 4887
	// (get) Token: 0x0600362B RID: 13867
	StatusEffectType[] StatusEffectTypes { get; }

	// Token: 0x17001318 RID: 4888
	// (get) Token: 0x0600362C RID: 13868
	float[] StatusEffectDurations { get; }
}

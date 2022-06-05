using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000890 RID: 2192
public abstract class BaseSummonRule
{
	// Token: 0x170017EC RID: 6124
	// (get) Token: 0x06004324 RID: 17188 RVA: 0x0002524C File Offset: 0x0002344C
	// (set) Token: 0x06004325 RID: 17189 RVA: 0x00025254 File Offset: 0x00023454
	public SummonRuleController SummonController { get; private set; }

	// Token: 0x170017ED RID: 6125
	// (get) Token: 0x06004326 RID: 17190
	public abstract string RuleLabel { get; }

	// Token: 0x170017EE RID: 6126
	// (get) Token: 0x06004327 RID: 17191
	public abstract SummonRuleType RuleType { get; }

	// Token: 0x170017EF RID: 6127
	// (get) Token: 0x06004328 RID: 17192 RVA: 0x0002525D File Offset: 0x0002345D
	public virtual Color BoxColor { get; } = Color.white;

	// Token: 0x170017F0 RID: 6128
	// (get) Token: 0x06004329 RID: 17193 RVA: 0x00025265 File Offset: 0x00023465
	// (set) Token: 0x0600432A RID: 17194 RVA: 0x0002526D File Offset: 0x0002346D
	public bool IsRuleComplete { get; protected set; }

	// Token: 0x0600432B RID: 17195
	public abstract IEnumerator RunSummonRule();

	// Token: 0x0600432C RID: 17196 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void OnEnable()
	{
	}

	// Token: 0x0600432D RID: 17197 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void OnDisable()
	{
	}

	// Token: 0x170017F1 RID: 6129
	// (get) Token: 0x0600432E RID: 17198 RVA: 0x00025276 File Offset: 0x00023476
	// (set) Token: 0x0600432F RID: 17199 RVA: 0x0002527E File Offset: 0x0002347E
	public UnityEngine.Object SerializedObject { get; protected set; }

	// Token: 0x06004330 RID: 17200 RVA: 0x00025287 File Offset: 0x00023487
	public void SetSerializedObject(UnityEngine.Object serializedObj)
	{
		this.SerializedObject = serializedObj;
	}

	// Token: 0x06004331 RID: 17201 RVA: 0x00025290 File Offset: 0x00023490
	public virtual void Initialize(SummonRuleController summonController)
	{
		this.SummonController = summonController;
	}

	// Token: 0x06004332 RID: 17202 RVA: 0x00025299 File Offset: 0x00023499
	public void ResetRule()
	{
		this.IsRuleComplete = false;
	}
}

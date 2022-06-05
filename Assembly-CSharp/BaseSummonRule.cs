using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051B RID: 1307
public abstract class BaseSummonRule
{
	// Token: 0x170011D5 RID: 4565
	// (get) Token: 0x06003062 RID: 12386 RVA: 0x000A5BD5 File Offset: 0x000A3DD5
	// (set) Token: 0x06003063 RID: 12387 RVA: 0x000A5BDD File Offset: 0x000A3DDD
	public SummonRuleController SummonController { get; private set; }

	// Token: 0x170011D6 RID: 4566
	// (get) Token: 0x06003064 RID: 12388
	public abstract string RuleLabel { get; }

	// Token: 0x170011D7 RID: 4567
	// (get) Token: 0x06003065 RID: 12389
	public abstract SummonRuleType RuleType { get; }

	// Token: 0x170011D8 RID: 4568
	// (get) Token: 0x06003066 RID: 12390 RVA: 0x000A5BE6 File Offset: 0x000A3DE6
	public virtual Color BoxColor { get; } = Color.white;

	// Token: 0x170011D9 RID: 4569
	// (get) Token: 0x06003067 RID: 12391 RVA: 0x000A5BEE File Offset: 0x000A3DEE
	// (set) Token: 0x06003068 RID: 12392 RVA: 0x000A5BF6 File Offset: 0x000A3DF6
	public bool IsRuleComplete { get; protected set; }

	// Token: 0x06003069 RID: 12393
	public abstract IEnumerator RunSummonRule();

	// Token: 0x0600306A RID: 12394 RVA: 0x000A5BFF File Offset: 0x000A3DFF
	public virtual void OnEnable()
	{
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x000A5C01 File Offset: 0x000A3E01
	public virtual void OnDisable()
	{
	}

	// Token: 0x170011DA RID: 4570
	// (get) Token: 0x0600306C RID: 12396 RVA: 0x000A5C03 File Offset: 0x000A3E03
	// (set) Token: 0x0600306D RID: 12397 RVA: 0x000A5C0B File Offset: 0x000A3E0B
	public UnityEngine.Object SerializedObject { get; protected set; }

	// Token: 0x0600306E RID: 12398 RVA: 0x000A5C14 File Offset: 0x000A3E14
	public void SetSerializedObject(UnityEngine.Object serializedObj)
	{
		this.SerializedObject = serializedObj;
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x000A5C1D File Offset: 0x000A3E1D
	public virtual void Initialize(SummonRuleController summonController)
	{
		this.SummonController = summonController;
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000A5C26 File Offset: 0x000A3E26
	public void ResetRule()
	{
		this.IsRuleComplete = false;
	}
}

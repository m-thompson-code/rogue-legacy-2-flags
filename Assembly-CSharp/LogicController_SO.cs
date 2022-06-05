using System;
using UnityEngine;

// Token: 0x020006D9 RID: 1753
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Enemy Logic Controller")]
public class LogicController_SO : ScriptableObject
{
	// Token: 0x04003049 RID: 12361
	[Space(10f)]
	public StringInt_Dictionary BasicCloseLogic;

	// Token: 0x0400304A RID: 12362
	[Space(10f)]
	public StringInt_Dictionary BasicMediumLogic;

	// Token: 0x0400304B RID: 12363
	[Space(10f)]
	public StringInt_Dictionary BasicFarLogic;

	// Token: 0x0400304C RID: 12364
	[Space(10f)]
	public StringInt_Dictionary BasicRestLogic;

	// Token: 0x0400304D RID: 12365
	[Space(10f)]
	public StringInt_Dictionary BasicWanderLogic;

	// Token: 0x0400304E RID: 12366
	[Space(10f)]
	public StringInt_Dictionary AdvancedCloseLogic;

	// Token: 0x0400304F RID: 12367
	[Space(10f)]
	public StringInt_Dictionary AdvancedMediumLogic;

	// Token: 0x04003050 RID: 12368
	[Space(10f)]
	public StringInt_Dictionary AdvancedFarLogic;

	// Token: 0x04003051 RID: 12369
	[Space(10f)]
	public StringInt_Dictionary AdvancedRestLogic;

	// Token: 0x04003052 RID: 12370
	[Space(10f)]
	public StringInt_Dictionary AdvancerWanderLogic;

	// Token: 0x04003053 RID: 12371
	[Space(10f)]
	public StringInt_Dictionary ExpertCloseLogic;

	// Token: 0x04003054 RID: 12372
	[Space(10f)]
	public StringInt_Dictionary ExpertMediumLogic;

	// Token: 0x04003055 RID: 12373
	[Space(10f)]
	public StringInt_Dictionary ExpertFarLogic;

	// Token: 0x04003056 RID: 12374
	[Space(10f)]
	public StringInt_Dictionary ExpertRestLogic;

	// Token: 0x04003057 RID: 12375
	[Space(10f)]
	public StringInt_Dictionary ExpertWanderLogic;

	// Token: 0x04003058 RID: 12376
	[Space(10f)]
	public StringInt_Dictionary MinibossCloseLogic;

	// Token: 0x04003059 RID: 12377
	[Space(10f)]
	public StringInt_Dictionary MinibossMediumLogic;

	// Token: 0x0400305A RID: 12378
	[Space(10f)]
	public StringInt_Dictionary MinibossFarLogic;

	// Token: 0x0400305B RID: 12379
	[Space(10f)]
	public StringInt_Dictionary MinibossRestLogic;

	// Token: 0x0400305C RID: 12380
	[Space(10f)]
	public StringInt_Dictionary MinibossWanderLogic;
}

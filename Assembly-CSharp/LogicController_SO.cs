using System;
using UnityEngine;

// Token: 0x02000B84 RID: 2948
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Enemy Logic Controller")]
public class LogicController_SO : ScriptableObject
{
	// Token: 0x04004298 RID: 17048
	[Space(10f)]
	public StringInt_Dictionary BasicCloseLogic;

	// Token: 0x04004299 RID: 17049
	[Space(10f)]
	public StringInt_Dictionary BasicMediumLogic;

	// Token: 0x0400429A RID: 17050
	[Space(10f)]
	public StringInt_Dictionary BasicFarLogic;

	// Token: 0x0400429B RID: 17051
	[Space(10f)]
	public StringInt_Dictionary BasicRestLogic;

	// Token: 0x0400429C RID: 17052
	[Space(10f)]
	public StringInt_Dictionary BasicWanderLogic;

	// Token: 0x0400429D RID: 17053
	[Space(10f)]
	public StringInt_Dictionary AdvancedCloseLogic;

	// Token: 0x0400429E RID: 17054
	[Space(10f)]
	public StringInt_Dictionary AdvancedMediumLogic;

	// Token: 0x0400429F RID: 17055
	[Space(10f)]
	public StringInt_Dictionary AdvancedFarLogic;

	// Token: 0x040042A0 RID: 17056
	[Space(10f)]
	public StringInt_Dictionary AdvancedRestLogic;

	// Token: 0x040042A1 RID: 17057
	[Space(10f)]
	public StringInt_Dictionary AdvancerWanderLogic;

	// Token: 0x040042A2 RID: 17058
	[Space(10f)]
	public StringInt_Dictionary ExpertCloseLogic;

	// Token: 0x040042A3 RID: 17059
	[Space(10f)]
	public StringInt_Dictionary ExpertMediumLogic;

	// Token: 0x040042A4 RID: 17060
	[Space(10f)]
	public StringInt_Dictionary ExpertFarLogic;

	// Token: 0x040042A5 RID: 17061
	[Space(10f)]
	public StringInt_Dictionary ExpertRestLogic;

	// Token: 0x040042A6 RID: 17062
	[Space(10f)]
	public StringInt_Dictionary ExpertWanderLogic;

	// Token: 0x040042A7 RID: 17063
	[Space(10f)]
	public StringInt_Dictionary MinibossCloseLogic;

	// Token: 0x040042A8 RID: 17064
	[Space(10f)]
	public StringInt_Dictionary MinibossMediumLogic;

	// Token: 0x040042A9 RID: 17065
	[Space(10f)]
	public StringInt_Dictionary MinibossFarLogic;

	// Token: 0x040042AA RID: 17066
	[Space(10f)]
	public StringInt_Dictionary MinibossRestLogic;

	// Token: 0x040042AB RID: 17067
	[Space(10f)]
	public StringInt_Dictionary MinibossWanderLogic;
}

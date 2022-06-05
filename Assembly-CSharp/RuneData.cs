using System;
using UnityEngine;

// Token: 0x02000B98 RID: 2968
public class RuneData : ScriptableObject
{
	// Token: 0x0400438F RID: 17295
	public RuneType RuneType;

	// Token: 0x04004390 RID: 17296
	public int Location;

	// Token: 0x04004391 RID: 17297
	public int BaseItemLevel;

	// Token: 0x04004392 RID: 17298
	public int GoldCost;

	// Token: 0x04004393 RID: 17299
	public int BlackStoneCost;

	// Token: 0x04004394 RID: 17300
	public bool Disabled;

	// Token: 0x04004395 RID: 17301
	public int BaseWeight;

	// Token: 0x04004396 RID: 17302
	public float StatMod01;

	// Token: 0x04004397 RID: 17303
	public float StatMod02;

	// Token: 0x04004398 RID: 17304
	public float StatMod03;

	// Token: 0x04004399 RID: 17305
	public int MaximumLevel;

	// Token: 0x0400439A RID: 17306
	public int ScalingItemLevel;

	// Token: 0x0400439B RID: 17307
	public int ScalingGoldCost;

	// Token: 0x0400439C RID: 17308
	public int ScalingBlackStoneCost;

	// Token: 0x0400439D RID: 17309
	public int ScalingWeight;

	// Token: 0x0400439E RID: 17310
	public float ScalingStatMod01;

	// Token: 0x0400439F RID: 17311
	public float ScalingStatMod02;

	// Token: 0x040043A0 RID: 17312
	public float ScalingStatMod03;

	// Token: 0x040043A1 RID: 17313
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x040043A2 RID: 17314
	public string Description;

	// Token: 0x040043A3 RID: 17315
	public string Controls;
}

using System;

// Token: 0x020009DC RID: 2524
public interface IPivotConsumer
{
	// Token: 0x17001A82 RID: 6786
	// (get) Token: 0x06004CA6 RID: 19622
	PivotPoint PivotPoint { get; }

	// Token: 0x06004CA7 RID: 19623
	void SetPivot(PivotPoint pivotPoint);
}

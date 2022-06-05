using System;

// Token: 0x020005D4 RID: 1492
public interface IPivotConsumer
{
	// Token: 0x17001355 RID: 4949
	// (get) Token: 0x06003694 RID: 13972
	PivotPoint PivotPoint { get; }

	// Token: 0x06003695 RID: 13973
	void SetPivot(PivotPoint pivotPoint);
}

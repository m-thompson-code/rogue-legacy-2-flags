using System;

// Token: 0x020009D9 RID: 2521
public interface IHazardSpawnController : IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer, IStateConsumer, ISetSpawnType, IHasProjectileNameArray
{
	// Token: 0x17001A7E RID: 6782
	// (get) Token: 0x06004C9F RID: 19615
	HazardCategory Category { get; }

	// Token: 0x17001A7F RID: 6783
	// (get) Token: 0x06004CA0 RID: 19616
	HazardType Type { get; }

	// Token: 0x17001A80 RID: 6784
	// (get) Token: 0x06004CA1 RID: 19617
	IHazard Hazard { get; }

	// Token: 0x06004CA2 RID: 19618
	void SetType(HazardType hazardType);
}

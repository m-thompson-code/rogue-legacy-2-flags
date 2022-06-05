using System;

// Token: 0x020005D1 RID: 1489
public interface IHazardSpawnController : IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer, IStateConsumer, ISetSpawnType, IHasProjectileNameArray
{
	// Token: 0x17001351 RID: 4945
	// (get) Token: 0x0600368D RID: 13965
	HazardCategory Category { get; }

	// Token: 0x17001352 RID: 4946
	// (get) Token: 0x0600368E RID: 13966
	HazardType Type { get; }

	// Token: 0x17001353 RID: 4947
	// (get) Token: 0x0600368F RID: 13967
	IHazard Hazard { get; }

	// Token: 0x06003690 RID: 13968
	void SetType(HazardType hazardType);
}

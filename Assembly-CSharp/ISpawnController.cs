using System;
using UnityEngine;

// Token: 0x020005CD RID: 1485
public interface ISpawnController
{
	// Token: 0x1700134E RID: 4942
	// (get) Token: 0x06003688 RID: 13960
	GameObject gameObject { get; }

	// Token: 0x1700134F RID: 4943
	// (get) Token: 0x06003689 RID: 13961
	bool ShouldSpawn { get; }

	// Token: 0x17001350 RID: 4944
	// (get) Token: 0x0600368A RID: 13962
	SpawnLogicController SpawnLogicController { get; }
}

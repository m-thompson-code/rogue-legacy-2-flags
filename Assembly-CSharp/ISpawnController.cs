using System;
using UnityEngine;

// Token: 0x020009D5 RID: 2517
public interface ISpawnController
{
	// Token: 0x17001A7B RID: 6779
	// (get) Token: 0x06004C9A RID: 19610
	GameObject gameObject { get; }

	// Token: 0x17001A7C RID: 6780
	// (get) Token: 0x06004C9B RID: 19611
	bool ShouldSpawn { get; }

	// Token: 0x17001A7D RID: 6781
	// (get) Token: 0x06004C9C RID: 19612
	SpawnLogicController SpawnLogicController { get; }
}

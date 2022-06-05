using System;
using System.Collections.Generic;

// Token: 0x020007C5 RID: 1989
public class WorldBuildCompleteEventArgs : EventArgs
{
	// Token: 0x060042AD RID: 17069 RVA: 0x000EBF64 File Offset: 0x000EA164
	public WorldBuildCompleteEventArgs(int masterSeed, int seed, List<BiomeController> biomeControllers, bool isSuccess)
	{
		this.MasterSeed = masterSeed;
		this.BiomeCreationSeed = seed;
		this.BiomeControllers = biomeControllers;
		this.IsSuccess = isSuccess;
	}

	// Token: 0x17001691 RID: 5777
	// (get) Token: 0x060042AE RID: 17070 RVA: 0x000EBF89 File Offset: 0x000EA189
	public int MasterSeed { get; }

	// Token: 0x17001692 RID: 5778
	// (get) Token: 0x060042AF RID: 17071 RVA: 0x000EBF91 File Offset: 0x000EA191
	// (set) Token: 0x060042B0 RID: 17072 RVA: 0x000EBF99 File Offset: 0x000EA199
	public int BiomeCreationSeed { get; private set; }

	// Token: 0x17001693 RID: 5779
	// (get) Token: 0x060042B1 RID: 17073 RVA: 0x000EBFA2 File Offset: 0x000EA1A2
	// (set) Token: 0x060042B2 RID: 17074 RVA: 0x000EBFAA File Offset: 0x000EA1AA
	public List<BiomeController> BiomeControllers { get; private set; }

	// Token: 0x17001694 RID: 5780
	// (get) Token: 0x060042B3 RID: 17075 RVA: 0x000EBFB3 File Offset: 0x000EA1B3
	public bool IsSuccess { get; }
}

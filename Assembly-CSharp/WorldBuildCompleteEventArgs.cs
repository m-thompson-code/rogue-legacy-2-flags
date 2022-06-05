using System;
using System.Collections.Generic;

// Token: 0x02000C8B RID: 3211
public class WorldBuildCompleteEventArgs : EventArgs
{
	// Token: 0x06005C36 RID: 23606 RVA: 0x0003297A File Offset: 0x00030B7A
	public WorldBuildCompleteEventArgs(int masterSeed, int seed, List<BiomeController> biomeControllers, bool isSuccess)
	{
		this.MasterSeed = masterSeed;
		this.BiomeCreationSeed = seed;
		this.BiomeControllers = biomeControllers;
		this.IsSuccess = isSuccess;
	}

	// Token: 0x17001E8F RID: 7823
	// (get) Token: 0x06005C37 RID: 23607 RVA: 0x0003299F File Offset: 0x00030B9F
	public int MasterSeed { get; }

	// Token: 0x17001E90 RID: 7824
	// (get) Token: 0x06005C38 RID: 23608 RVA: 0x000329A7 File Offset: 0x00030BA7
	// (set) Token: 0x06005C39 RID: 23609 RVA: 0x000329AF File Offset: 0x00030BAF
	public int BiomeCreationSeed { get; private set; }

	// Token: 0x17001E91 RID: 7825
	// (get) Token: 0x06005C3A RID: 23610 RVA: 0x000329B8 File Offset: 0x00030BB8
	// (set) Token: 0x06005C3B RID: 23611 RVA: 0x000329C0 File Offset: 0x00030BC0
	public List<BiomeController> BiomeControllers { get; private set; }

	// Token: 0x17001E92 RID: 7826
	// (get) Token: 0x06005C3C RID: 23612 RVA: 0x000329C9 File Offset: 0x00030BC9
	public bool IsSuccess { get; }
}

using System;

// Token: 0x020007AC RID: 1964
public class DoorConnectEventArgs : EventArgs
{
	// Token: 0x06004220 RID: 16928 RVA: 0x000EB94F File Offset: 0x000E9B4F
	public DoorConnectEventArgs(Door doorA, Door doorB)
	{
		this.Initialize(doorA, doorB);
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x000EB95F File Offset: 0x000E9B5F
	public void Initialize(Door doorA, Door doorB)
	{
		this.DoorA = doorA;
		this.DoorB = doorB;
	}

	// Token: 0x17001662 RID: 5730
	// (get) Token: 0x06004222 RID: 16930 RVA: 0x000EB96F File Offset: 0x000E9B6F
	// (set) Token: 0x06004223 RID: 16931 RVA: 0x000EB977 File Offset: 0x000E9B77
	public Door DoorA { get; private set; }

	// Token: 0x17001663 RID: 5731
	// (get) Token: 0x06004224 RID: 16932 RVA: 0x000EB980 File Offset: 0x000E9B80
	// (set) Token: 0x06004225 RID: 16933 RVA: 0x000EB988 File Offset: 0x000E9B88
	public Door DoorB { get; private set; }
}

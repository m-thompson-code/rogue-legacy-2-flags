using System;

// Token: 0x02000C72 RID: 3186
public class DoorConnectEventArgs : EventArgs
{
	// Token: 0x06005BA9 RID: 23465 RVA: 0x00032365 File Offset: 0x00030565
	public DoorConnectEventArgs(Door doorA, Door doorB)
	{
		this.Initialize(doorA, doorB);
	}

	// Token: 0x06005BAA RID: 23466 RVA: 0x00032375 File Offset: 0x00030575
	public void Initialize(Door doorA, Door doorB)
	{
		this.DoorA = doorA;
		this.DoorB = doorB;
	}

	// Token: 0x17001E60 RID: 7776
	// (get) Token: 0x06005BAB RID: 23467 RVA: 0x00032385 File Offset: 0x00030585
	// (set) Token: 0x06005BAC RID: 23468 RVA: 0x0003238D File Offset: 0x0003058D
	public Door DoorA { get; private set; }

	// Token: 0x17001E61 RID: 7777
	// (get) Token: 0x06005BAD RID: 23469 RVA: 0x00032396 File Offset: 0x00030596
	// (set) Token: 0x06005BAE RID: 23470 RVA: 0x0003239E File Offset: 0x0003059E
	public Door DoorB { get; private set; }
}

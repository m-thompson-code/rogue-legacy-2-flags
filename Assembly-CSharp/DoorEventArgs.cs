using System;

// Token: 0x02000C55 RID: 3157
public class DoorEventArgs : EventArgs
{
	// Token: 0x06005B04 RID: 23300 RVA: 0x00031F07 File Offset: 0x00030107
	public DoorEventArgs(Door door)
	{
		this.Initialize(door);
	}

	// Token: 0x06005B05 RID: 23301 RVA: 0x00031F16 File Offset: 0x00030116
	public void Initialize(Door door)
	{
		this.Door = door;
	}

	// Token: 0x17001E50 RID: 7760
	// (get) Token: 0x06005B06 RID: 23302 RVA: 0x00031F1F File Offset: 0x0003011F
	// (set) Token: 0x06005B07 RID: 23303 RVA: 0x00031F27 File Offset: 0x00030127
	public Door Door { get; private set; }
}

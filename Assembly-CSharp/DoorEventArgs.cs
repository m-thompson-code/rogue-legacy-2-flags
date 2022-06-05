using System;

// Token: 0x02000792 RID: 1938
public class DoorEventArgs : EventArgs
{
	// Token: 0x06004187 RID: 16775 RVA: 0x000E9A31 File Offset: 0x000E7C31
	public DoorEventArgs(Door door)
	{
		this.Initialize(door);
	}

	// Token: 0x06004188 RID: 16776 RVA: 0x000E9A40 File Offset: 0x000E7C40
	public void Initialize(Door door)
	{
		this.Door = door;
	}

	// Token: 0x17001654 RID: 5716
	// (get) Token: 0x06004189 RID: 16777 RVA: 0x000E9A49 File Offset: 0x000E7C49
	// (set) Token: 0x0600418A RID: 16778 RVA: 0x000E9A51 File Offset: 0x000E7C51
	public Door Door { get; private set; }
}

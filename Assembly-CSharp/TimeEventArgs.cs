using System;
using UnityEngine;

// Token: 0x02000C58 RID: 3160
public class TimeEventArgs : EventArgs
{
	// Token: 0x06005B12 RID: 23314 RVA: 0x00031FA0 File Offset: 0x000301A0
	public TimeEventArgs()
	{
		this.Time = UnityEngine.Time.time;
	}

	// Token: 0x17001E55 RID: 7765
	// (get) Token: 0x06005B13 RID: 23315 RVA: 0x00031FB3 File Offset: 0x000301B3
	// (set) Token: 0x06005B14 RID: 23316 RVA: 0x00031FBB File Offset: 0x000301BB
	public float Time { get; protected set; }
}

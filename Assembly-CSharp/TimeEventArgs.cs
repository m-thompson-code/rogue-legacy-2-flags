using System;
using UnityEngine;

// Token: 0x02000795 RID: 1941
public class TimeEventArgs : EventArgs
{
	// Token: 0x06004195 RID: 16789 RVA: 0x000E9ACA File Offset: 0x000E7CCA
	public TimeEventArgs()
	{
		this.Time = UnityEngine.Time.time;
	}

	// Token: 0x17001659 RID: 5721
	// (get) Token: 0x06004196 RID: 16790 RVA: 0x000E9ADD File Offset: 0x000E7CDD
	// (set) Token: 0x06004197 RID: 16791 RVA: 0x000E9AE5 File Offset: 0x000E7CE5
	public float Time { get; protected set; }
}

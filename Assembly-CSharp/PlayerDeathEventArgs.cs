using System;
using UnityEngine;

// Token: 0x02000C7E RID: 3198
public class PlayerDeathEventArgs : EventArgs
{
	// Token: 0x06005BFC RID: 23548 RVA: 0x0003270D File Offset: 0x0003090D
	public PlayerDeathEventArgs(PlayerController victim, GameObject killer)
	{
		this.Initialize(victim, killer);
	}

	// Token: 0x06005BFD RID: 23549 RVA: 0x0003271D File Offset: 0x0003091D
	public void Initialize(PlayerController victim, GameObject killer)
	{
		this.Victim = victim;
		this.Killer = killer;
	}

	// Token: 0x17001E7E RID: 7806
	// (get) Token: 0x06005BFE RID: 23550 RVA: 0x0003272D File Offset: 0x0003092D
	// (set) Token: 0x06005BFF RID: 23551 RVA: 0x00032735 File Offset: 0x00030935
	public GameObject Killer { get; private set; }

	// Token: 0x17001E7F RID: 7807
	// (get) Token: 0x06005C00 RID: 23552 RVA: 0x0003273E File Offset: 0x0003093E
	// (set) Token: 0x06005C01 RID: 23553 RVA: 0x00032746 File Offset: 0x00030946
	public PlayerController Victim { get; private set; }
}

using System;
using UnityEngine;

// Token: 0x02000C80 RID: 3200
public class EnemyDeathEventArgs : EventArgs
{
	// Token: 0x06005C08 RID: 23560 RVA: 0x00032791 File Offset: 0x00030991
	public EnemyDeathEventArgs(EnemyController victim, GameObject killer)
	{
		this.Initialize(victim, killer);
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x000327A1 File Offset: 0x000309A1
	public void Initialize(EnemyController victim, GameObject killer)
	{
		this.Victim = victim;
		this.Killer = killer;
	}

	// Token: 0x17001E82 RID: 7810
	// (get) Token: 0x06005C0A RID: 23562 RVA: 0x000327B1 File Offset: 0x000309B1
	// (set) Token: 0x06005C0B RID: 23563 RVA: 0x000327B9 File Offset: 0x000309B9
	public GameObject Killer { get; private set; }

	// Token: 0x17001E83 RID: 7811
	// (get) Token: 0x06005C0C RID: 23564 RVA: 0x000327C2 File Offset: 0x000309C2
	// (set) Token: 0x06005C0D RID: 23565 RVA: 0x000327CA File Offset: 0x000309CA
	public EnemyController Victim { get; private set; }
}

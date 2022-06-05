using System;
using UnityEngine;

// Token: 0x020007B8 RID: 1976
public class PlayerDeathEventArgs : EventArgs
{
	// Token: 0x06004273 RID: 17011 RVA: 0x000EBCF7 File Offset: 0x000E9EF7
	public PlayerDeathEventArgs(PlayerController victim, GameObject killer)
	{
		this.Initialize(victim, killer);
	}

	// Token: 0x06004274 RID: 17012 RVA: 0x000EBD07 File Offset: 0x000E9F07
	public void Initialize(PlayerController victim, GameObject killer)
	{
		this.Victim = victim;
		this.Killer = killer;
	}

	// Token: 0x17001680 RID: 5760
	// (get) Token: 0x06004275 RID: 17013 RVA: 0x000EBD17 File Offset: 0x000E9F17
	// (set) Token: 0x06004276 RID: 17014 RVA: 0x000EBD1F File Offset: 0x000E9F1F
	public GameObject Killer { get; private set; }

	// Token: 0x17001681 RID: 5761
	// (get) Token: 0x06004277 RID: 17015 RVA: 0x000EBD28 File Offset: 0x000E9F28
	// (set) Token: 0x06004278 RID: 17016 RVA: 0x000EBD30 File Offset: 0x000E9F30
	public PlayerController Victim { get; private set; }
}

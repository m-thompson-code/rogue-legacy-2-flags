using System;
using UnityEngine;

// Token: 0x020007BA RID: 1978
public class EnemyDeathEventArgs : EventArgs
{
	// Token: 0x0600427F RID: 17023 RVA: 0x000EBD7B File Offset: 0x000E9F7B
	public EnemyDeathEventArgs(EnemyController victim, GameObject killer)
	{
		this.Initialize(victim, killer);
	}

	// Token: 0x06004280 RID: 17024 RVA: 0x000EBD8B File Offset: 0x000E9F8B
	public void Initialize(EnemyController victim, GameObject killer)
	{
		this.Victim = victim;
		this.Killer = killer;
	}

	// Token: 0x17001684 RID: 5764
	// (get) Token: 0x06004281 RID: 17025 RVA: 0x000EBD9B File Offset: 0x000E9F9B
	// (set) Token: 0x06004282 RID: 17026 RVA: 0x000EBDA3 File Offset: 0x000E9FA3
	public GameObject Killer { get; private set; }

	// Token: 0x17001685 RID: 5765
	// (get) Token: 0x06004283 RID: 17027 RVA: 0x000EBDAC File Offset: 0x000E9FAC
	// (set) Token: 0x06004284 RID: 17028 RVA: 0x000EBDB4 File Offset: 0x000E9FB4
	public EnemyController Victim { get; private set; }
}

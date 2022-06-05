using System;
using UnityEngine;

// Token: 0x020007B2 RID: 1970
public class PlayerDownstrikeEventArgs : EventArgs
{
	// Token: 0x0600424B RID: 16971 RVA: 0x000EBB39 File Offset: 0x000E9D39
	public PlayerDownstrikeEventArgs(Projectile_RL projectile, GameObject collidedObj)
	{
		this.Initialise(projectile, collidedObj);
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x000EBB49 File Offset: 0x000E9D49
	public void Initialise(Projectile_RL projectile, GameObject collidedObj)
	{
		this.Projectile = projectile;
		this.CollidedObj = collidedObj;
	}

	// Token: 0x17001672 RID: 5746
	// (get) Token: 0x0600424D RID: 16973 RVA: 0x000EBB59 File Offset: 0x000E9D59
	// (set) Token: 0x0600424E RID: 16974 RVA: 0x000EBB61 File Offset: 0x000E9D61
	public Projectile_RL Projectile { get; private set; }

	// Token: 0x17001673 RID: 5747
	// (get) Token: 0x0600424F RID: 16975 RVA: 0x000EBB6A File Offset: 0x000E9D6A
	// (set) Token: 0x06004250 RID: 16976 RVA: 0x000EBB72 File Offset: 0x000E9D72
	public GameObject CollidedObj { get; private set; }
}

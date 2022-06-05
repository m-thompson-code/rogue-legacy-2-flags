using System;
using UnityEngine;

// Token: 0x02000C78 RID: 3192
public class PlayerDownstrikeEventArgs : EventArgs
{
	// Token: 0x06005BD4 RID: 23508 RVA: 0x0003254F File Offset: 0x0003074F
	public PlayerDownstrikeEventArgs(Projectile_RL projectile, GameObject collidedObj)
	{
		this.Initialise(projectile, collidedObj);
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x0003255F File Offset: 0x0003075F
	public void Initialise(Projectile_RL projectile, GameObject collidedObj)
	{
		this.Projectile = projectile;
		this.CollidedObj = collidedObj;
	}

	// Token: 0x17001E70 RID: 7792
	// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x0003256F File Offset: 0x0003076F
	// (set) Token: 0x06005BD7 RID: 23511 RVA: 0x00032577 File Offset: 0x00030777
	public Projectile_RL Projectile { get; private set; }

	// Token: 0x17001E71 RID: 7793
	// (get) Token: 0x06005BD8 RID: 23512 RVA: 0x00032580 File Offset: 0x00030780
	// (set) Token: 0x06005BD9 RID: 23513 RVA: 0x00032588 File Offset: 0x00030788
	public GameObject CollidedObj { get; private set; }
}

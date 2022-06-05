using System;
using UnityEngine;

// Token: 0x02000496 RID: 1174
public abstract class BaseProjectileLogic : MonoBehaviour
{
	// Token: 0x17001098 RID: 4248
	// (get) Token: 0x06002B2B RID: 11051 RVA: 0x00092431 File Offset: 0x00090631
	// (set) Token: 0x06002B2C RID: 11052 RVA: 0x00092439 File Offset: 0x00090639
	public Projectile_RL SourceProjectile { get; protected set; }

	// Token: 0x06002B2D RID: 11053 RVA: 0x00092442 File Offset: 0x00090642
	protected virtual void Awake()
	{
		this.SourceProjectile = base.GetComponent<Projectile_RL>();
	}
}

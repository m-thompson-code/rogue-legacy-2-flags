using System;
using UnityEngine;

// Token: 0x0200079A RID: 1946
public abstract class BaseProjectileLogic : MonoBehaviour
{
	// Token: 0x170015E1 RID: 5601
	// (get) Token: 0x06003B7A RID: 15226 RVA: 0x00020ACE File Offset: 0x0001ECCE
	// (set) Token: 0x06003B7B RID: 15227 RVA: 0x00020AD6 File Offset: 0x0001ECD6
	public Projectile_RL SourceProjectile { get; protected set; }

	// Token: 0x06003B7C RID: 15228 RVA: 0x00020ADF File Offset: 0x0001ECDF
	protected virtual void Awake()
	{
		this.SourceProjectile = base.GetComponent<Projectile_RL>();
	}
}

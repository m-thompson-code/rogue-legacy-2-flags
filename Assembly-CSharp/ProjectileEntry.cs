using System;
using UnityEngine;

// Token: 0x02000240 RID: 576
[Serializable]
public class ProjectileEntry
{
	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x0600171F RID: 5919 RVA: 0x00048039 File Offset: 0x00046239
	// (set) Token: 0x06001720 RID: 5920 RVA: 0x00048041 File Offset: 0x00046241
	public Projectile_RL ProjectilePrefab
	{
		get
		{
			return this.m_projectilePrefab;
		}
		set
		{
			this.m_projectilePrefab = value;
		}
	}

	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x06001721 RID: 5921 RVA: 0x0004804A File Offset: 0x0004624A
	// (set) Token: 0x06001722 RID: 5922 RVA: 0x00048052 File Offset: 0x00046252
	public int PoolSize
	{
		get
		{
			return this.m_poolSize;
		}
		set
		{
			this.m_poolSize = value;
		}
	}

	// Token: 0x04001698 RID: 5784
	[SerializeField]
	private Projectile_RL m_projectilePrefab;

	// Token: 0x04001699 RID: 5785
	[SerializeField]
	private int m_poolSize = 10;
}

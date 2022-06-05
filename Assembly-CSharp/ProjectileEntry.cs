using System;
using UnityEngine;

// Token: 0x020003FD RID: 1021
[Serializable]
public class ProjectileEntry
{
	// Token: 0x17000E76 RID: 3702
	// (get) Token: 0x060020D2 RID: 8402 RVA: 0x00011675 File Offset: 0x0000F875
	// (set) Token: 0x060020D3 RID: 8403 RVA: 0x0001167D File Offset: 0x0000F87D
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

	// Token: 0x17000E77 RID: 3703
	// (get) Token: 0x060020D4 RID: 8404 RVA: 0x00011686 File Offset: 0x0000F886
	// (set) Token: 0x060020D5 RID: 8405 RVA: 0x0001168E File Offset: 0x0000F88E
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

	// Token: 0x04001DB0 RID: 7600
	[SerializeField]
	private Projectile_RL m_projectilePrefab;

	// Token: 0x04001DB1 RID: 7601
	[SerializeField]
	private int m_poolSize = 10;
}

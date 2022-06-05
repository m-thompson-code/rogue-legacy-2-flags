using System;
using UnityEngine;

// Token: 0x020003A2 RID: 930
public class FireProjectileOnTrigger : MonoBehaviour, IHasProjectileNameArray
{
	// Token: 0x17000DFF RID: 3583
	// (get) Token: 0x06001EDD RID: 7901 RVA: 0x00010323 File Offset: 0x0000E523
	public virtual string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					this.m_projectileToSpawn
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x17000E00 RID: 3584
	// (get) Token: 0x06001EDE RID: 7902 RVA: 0x00010348 File Offset: 0x0000E548
	// (set) Token: 0x06001EDF RID: 7903 RVA: 0x00010350 File Offset: 0x0000E550
	public string ProjectileToSpawn
	{
		get
		{
			return this.m_projectileToSpawn;
		}
		set
		{
			this.m_projectileToSpawn = value;
		}
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x000A14C0 File Offset: 0x0009F6C0
	public void FireProjectile()
	{
		ProjectileManager.FireProjectile(base.gameObject, this.m_projectileToSpawn, Vector2.zero, false, 0f, 1f, false, true, true, true);
	}

	// Token: 0x04001B98 RID: 7064
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x04001B99 RID: 7065
	[NonSerialized]
	protected string[] m_projectileNameArray;
}

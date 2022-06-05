using System;
using UnityEngine;

// Token: 0x020001FD RID: 509
public class FireProjectileOnTrigger : MonoBehaviour, IHasProjectileNameArray
{
	// Token: 0x17000AEB RID: 2795
	// (get) Token: 0x06001583 RID: 5507 RVA: 0x00042EA4 File Offset: 0x000410A4
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

	// Token: 0x17000AEC RID: 2796
	// (get) Token: 0x06001584 RID: 5508 RVA: 0x00042EC9 File Offset: 0x000410C9
	// (set) Token: 0x06001585 RID: 5509 RVA: 0x00042ED1 File Offset: 0x000410D1
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

	// Token: 0x06001586 RID: 5510 RVA: 0x00042EDC File Offset: 0x000410DC
	public void FireProjectile()
	{
		ProjectileManager.FireProjectile(base.gameObject, this.m_projectileToSpawn, Vector2.zero, false, 0f, 1f, false, true, true, true);
	}

	// Token: 0x040014C9 RID: 5321
	[SerializeField]
	private string m_projectileToSpawn;

	// Token: 0x040014CA RID: 5322
	[NonSerialized]
	protected string[] m_projectileNameArray;
}

using System;
using UnityEngine;

// Token: 0x0200072F RID: 1839
public class PressurePlate_Hazard : Hazard, IHasProjectileNameArray
{
	// Token: 0x17001510 RID: 5392
	// (get) Token: 0x06003854 RID: 14420 RVA: 0x0001EE6C File Offset: 0x0001D06C
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					"PressurePlateProjectile"
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x000E7F6C File Offset: 0x000E616C
	public void Shoot()
	{
		this.m_firedProjectile = ProjectileManager.FireProjectile(base.gameObject, this.ProjectileNameArray[0], Vector2.zero, false, 0f, 1f, false, true, true, true);
		Vector3 localEulerAngles = this.m_firedProjectile.transform.localEulerAngles;
		localEulerAngles.z = 90f;
		this.m_firedProjectile.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x0001EE90 File Offset: 0x0001D090
	public void StopShooting()
	{
		if (this.m_firedProjectile != null && this.m_firedProjectile.isActiveAndEnabled)
		{
			base.StopAllCoroutines();
			this.m_firedProjectile.FlagForDestruction(null);
		}
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x0001EEBF File Offset: 0x0001D0BF
	public override void ResetHazard()
	{
		this.StopShooting();
	}

	// Token: 0x04002D2F RID: 11567
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x04002D30 RID: 11568
	private Projectile_RL m_firedProjectile;
}

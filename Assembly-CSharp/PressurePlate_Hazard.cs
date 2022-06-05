using System;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class PressurePlate_Hazard : Hazard, IHasProjectileNameArray
{
	// Token: 0x17001005 RID: 4101
	// (get) Token: 0x060028C8 RID: 10440 RVA: 0x00086DBD File Offset: 0x00084FBD
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

	// Token: 0x060028C9 RID: 10441 RVA: 0x00086DE4 File Offset: 0x00084FE4
	public void Shoot()
	{
		this.m_firedProjectile = ProjectileManager.FireProjectile(base.gameObject, this.ProjectileNameArray[0], Vector2.zero, false, 0f, 1f, false, true, true, true);
		Vector3 localEulerAngles = this.m_firedProjectile.transform.localEulerAngles;
		localEulerAngles.z = 90f;
		this.m_firedProjectile.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x060028CA RID: 10442 RVA: 0x00086E4C File Offset: 0x0008504C
	public void StopShooting()
	{
		if (this.m_firedProjectile != null && this.m_firedProjectile.isActiveAndEnabled)
		{
			base.StopAllCoroutines();
			this.m_firedProjectile.FlagForDestruction(null);
		}
	}

	// Token: 0x060028CB RID: 10443 RVA: 0x00086E7B File Offset: 0x0008507B
	public override void ResetHazard()
	{
		this.StopShooting();
	}

	// Token: 0x040021A8 RID: 8616
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x040021A9 RID: 8617
	private Projectile_RL m_firedProjectile;
}

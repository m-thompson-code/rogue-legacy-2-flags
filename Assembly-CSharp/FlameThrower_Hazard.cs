using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000721 RID: 1825
public class FlameThrower_Hazard : Turret_Hazard
{
	// Token: 0x170014E0 RID: 5344
	// (get) Token: 0x060037C4 RID: 14276 RVA: 0x0001E9C6 File Offset: 0x0001CBC6
	public override string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					this.m_projectileName,
					"WallTurretFlameThrowerWarningProjectile_Template"
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x170014E1 RID: 5345
	// (get) Token: 0x060037C5 RID: 14277 RVA: 0x0001E9F3 File Offset: 0x0001CBF3
	public float FlameDuration
	{
		get
		{
			return base.LoopFireDelay * 0.5f;
		}
	}

	// Token: 0x170014E2 RID: 5346
	// (get) Token: 0x060037C6 RID: 14278 RVA: 0x0001EA01 File Offset: 0x0001CC01
	public override float LoopFireDelay
	{
		get
		{
			return base.LoopFireDelay * 1.5f + this.FlameDuration;
		}
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x000E6D5C File Offset: 0x000E4F5C
	protected override void PlayTell()
	{
		float num = this.m_startingProjectileAngle + base.transform.localEulerAngles.z;
		Vector2 projectileOffset = this.m_projectileOffset;
		if (base.transform.localScale.x < 0f)
		{
			num += 180f;
			projectileOffset.x = -projectileOffset.x;
		}
		if (base.transform.localScale.y < 0f)
		{
			num = -num;
			projectileOffset.y = -projectileOffset.y;
		}
		if (Projectile_RL.OwnsProjectile(base.gameObject, this.m_flameWarningProjectile))
		{
			this.m_flameWarningProjectile.FlagForDestruction(null);
			this.m_flameWarningProjectile = null;
		}
		this.m_flameWarningProjectile = ProjectileManager.FireProjectile(base.gameObject, "WallTurretFlameThrowerWarningProjectile_Template", projectileOffset, true, 0f, 0f, false, true, true, true);
		base.PlayTell();
	}

	// Token: 0x060037C8 RID: 14280 RVA: 0x000E6E30 File Offset: 0x000E5030
	protected override void FireProjectile()
	{
		if (base.InAttackRange)
		{
			if (Projectile_RL.OwnsProjectile(base.gameObject, this.m_flameWarningProjectile))
			{
				this.m_flameWarningProjectile.FlagForDestruction(null);
				this.m_flameWarningProjectile = null;
			}
			float num = this.m_startingProjectileAngle + base.transform.localEulerAngles.z;
			Vector2 projectileOffset = this.m_projectileOffset;
			if (base.transform.localScale.x < 0f)
			{
				num += 180f;
				projectileOffset.x = -projectileOffset.x;
			}
			if (base.transform.localScale.y < 0f)
			{
				num = -num;
				projectileOffset.y = -projectileOffset.y;
			}
			this.m_flameProjectile = ProjectileManager.FireProjectile(base.gameObject, this.m_projectileName, projectileOffset, true, 0f, 0f, false, true, true, true);
			this.m_flameDurationEndTime = Time.time + this.FlameDuration;
			this.m_startFlameCounter = true;
			base.Animator.SetTrigger("Fire");
			base.Animator.ResetTrigger("Tell");
		}
		else
		{
			base.Animator.ResetTrigger("Fire");
			base.Animator.ResetTrigger("Tell");
			base.Animator.SetTrigger("Reset");
		}
		this.m_timesFired++;
		this.PrepFiringLogic();
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x0001EA16 File Offset: 0x0001CC16
	protected override void Update()
	{
		base.Update();
		if (this.m_startFlameCounter && Time.time > this.m_flameDurationEndTime)
		{
			this.m_startFlameCounter = false;
			this.m_flameProjectile.FlagForDestruction(null);
			this.m_flameProjectile = null;
		}
	}

	// Token: 0x060037CA RID: 14282 RVA: 0x0001EA4D File Offset: 0x0001CC4D
	protected override void PrepFiringLogic()
	{
		base.PrepFiringLogic();
		if (Projectile_RL.OwnsProjectile(base.gameObject, this.m_flameWarningProjectile))
		{
			this.m_flameWarningProjectile.FlagForDestruction(null);
			this.m_flameWarningProjectile = null;
		}
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x0001EA7B File Offset: 0x0001CC7B
	private IEnumerator StopProjectileParticleSystem(Projectile_RL projectile)
	{
		ParticleSystem partSys = projectile.GetComponentInChildren<ParticleSystem>();
		if (partSys != null)
		{
			projectile.transform.SetParent(null, true);
			projectile.HitboxController.DisableAllCollisions = true;
			partSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			yield return new WaitUntil(() => !partSys.IsAlive());
			projectile.HitboxController.DisableAllCollisions = false;
			projectile.FlagForDestruction(null);
		}
		else
		{
			projectile.FlagForDestruction(null);
		}
		yield break;
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x000E6F88 File Offset: 0x000E5188
	public override void ResetHazard()
	{
		this.m_startFlameCounter = false;
		if (Projectile_RL.OwnsProjectile(base.gameObject, this.m_flameProjectile))
		{
			this.m_flameProjectile.gameObject.SetActive(false);
		}
		this.m_flameProjectile = null;
		if (Projectile_RL.OwnsProjectile(base.gameObject, this.m_flameWarningProjectile))
		{
			this.m_flameWarningProjectile.gameObject.SetActive(false);
		}
		this.m_flameWarningProjectile = null;
		base.ResetHazard();
	}

	// Token: 0x04002CE2 RID: 11490
	private bool m_startFlameCounter;

	// Token: 0x04002CE3 RID: 11491
	private float m_flameDurationEndTime;

	// Token: 0x04002CE4 RID: 11492
	private Projectile_RL m_flameProjectile;

	// Token: 0x04002CE5 RID: 11493
	private Projectile_RL m_flameWarningProjectile;
}

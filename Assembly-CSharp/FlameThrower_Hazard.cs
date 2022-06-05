using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044B RID: 1099
public class FlameThrower_Hazard : Turret_Hazard
{
	// Token: 0x17000FE3 RID: 4067
	// (get) Token: 0x06002864 RID: 10340 RVA: 0x00085D07 File Offset: 0x00083F07
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

	// Token: 0x17000FE4 RID: 4068
	// (get) Token: 0x06002865 RID: 10341 RVA: 0x00085D34 File Offset: 0x00083F34
	public float FlameDuration
	{
		get
		{
			return base.LoopFireDelay * 0.5f;
		}
	}

	// Token: 0x17000FE5 RID: 4069
	// (get) Token: 0x06002866 RID: 10342 RVA: 0x00085D42 File Offset: 0x00083F42
	public override float LoopFireDelay
	{
		get
		{
			return base.LoopFireDelay * 1.5f + this.FlameDuration;
		}
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x00085D58 File Offset: 0x00083F58
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

	// Token: 0x06002868 RID: 10344 RVA: 0x00085E2C File Offset: 0x0008402C
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

	// Token: 0x06002869 RID: 10345 RVA: 0x00085F84 File Offset: 0x00084184
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

	// Token: 0x0600286A RID: 10346 RVA: 0x00085FBB File Offset: 0x000841BB
	protected override void PrepFiringLogic()
	{
		base.PrepFiringLogic();
		if (Projectile_RL.OwnsProjectile(base.gameObject, this.m_flameWarningProjectile))
		{
			this.m_flameWarningProjectile.FlagForDestruction(null);
			this.m_flameWarningProjectile = null;
		}
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x00085FE9 File Offset: 0x000841E9
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

	// Token: 0x0600286C RID: 10348 RVA: 0x00085FF8 File Offset: 0x000841F8
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

	// Token: 0x04002178 RID: 8568
	private bool m_startFlameCounter;

	// Token: 0x04002179 RID: 8569
	private float m_flameDurationEndTime;

	// Token: 0x0400217A RID: 8570
	private Projectile_RL m_flameProjectile;

	// Token: 0x0400217B RID: 8571
	private Projectile_RL m_flameWarningProjectile;
}

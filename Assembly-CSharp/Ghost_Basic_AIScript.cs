using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class Ghost_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000748 RID: 1864 RVA: 0x0001A40D File Offset: 0x0001860D
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"GhostWarningProjectile",
			"GhostWarningMinibossProjectile",
			"GhostQuakeProjectile",
			"GhostQuakeMinibossProjectile",
			"GhostBoltProjectile",
			"GhostBoltMinibossProjectile"
		};
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001A44B File Offset: 0x0001864B
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x0600074A RID: 1866 RVA: 0x0001A45C File Offset: 0x0001865C
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x0600074B RID: 1867 RVA: 0x0001A46D File Offset: 0x0001866D
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001A47E File Offset: 0x0001867E
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-1f, 1f);
		}
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x0600074D RID: 1869 RVA: 0x0001A48F File Offset: 0x0001868F
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-1f, 1f);
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x0600074E RID: 1870 RVA: 0x0001A4A0 File Offset: 0x000186A0
	protected virtual float m_scare_TellIntroAndHold_Delay
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001A4A7 File Offset: 0x000186A7
	protected virtual float m_scare_Exit_ForceIdle
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x0001A4AE File Offset: 0x000186AE
	protected virtual float m_scare_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06000751 RID: 1873 RVA: 0x0001A4B5 File Offset: 0x000186B5
	protected virtual float m_scare_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06000752 RID: 1874 RVA: 0x0001A4BC File Offset: 0x000186BC
	protected virtual bool m_scare_isMiniboss
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06000753 RID: 1875 RVA: 0x0001A4BF File Offset: 0x000186BF
	protected virtual bool m_scare_fireCardinalProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06000754 RID: 1876 RVA: 0x0001A4C2 File Offset: 0x000186C2
	protected virtual bool m_scare_fireDiagonalProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06000755 RID: 1877 RVA: 0x0001A4C5 File Offset: 0x000186C5
	protected virtual bool m_scare_fireMinibossProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06000756 RID: 1878 RVA: 0x0001A4C8 File Offset: 0x000186C8
	protected virtual float VulnerableDuration
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0001A4CF File Offset: 0x000186CF
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_tempTransparencyArray = enemyController.GetComponentsInChildren<SpriteRenderer>();
		this.SetInvulnerable(true, 0f);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0001A4F0 File Offset: 0x000186F0
	private void SetInvulnerable(bool invulnerable, float duration = 0f)
	{
		this.m_isInvulnerable = invulnerable;
		float a;
		if (invulnerable)
		{
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
			a = 0.25f;
		}
		else
		{
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
			base.EnemyController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
			a = 1f;
		}
		foreach (SpriteRenderer spriteRenderer in this.m_tempTransparencyArray)
		{
			Color color = spriteRenderer.color;
			color.a = a;
			spriteRenderer.color = color;
		}
		this.m_toggleDuration = duration;
		this.m_toggleStartTime = Time.time;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0001A5A2 File Offset: 0x000187A2
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ScareAttack()
	{
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		if (!this.m_scare_isMiniboss)
		{
			this.m_warningProjectile = this.FireProjectile("GhostWarningProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_warningProjectile = this.FireProjectile("GhostWarningMinibossProjectile", 0, true, 0f, 1f, true, true, true);
		}
		this.SetInvulnerable(false, this.VulnerableDuration + this.m_scare_TellIntroAndHold_Delay + 0.2f);
		yield return this.Default_TellIntroAndLoop("Shake_Tell_Intro", this.m_scare_TellIntro_AnimationSpeed, "Shake_Tell_Hold", this.m_scare_TellHold_AnimationSpeed, this.m_scare_TellIntroAndHold_Delay);
		base.StopProjectile(ref this.m_warningProjectile);
		yield return this.Default_Animation("Shake_Attack_Intro", this.m_scare_AttackIntro_AnimationSpeed, this.m_scare_AttackIntro_Delay, true);
		if (!this.m_scare_isMiniboss)
		{
			this.m_attackProjectile = this.FireProjectile("GhostQuakeProjectile", 0, true, 0f, 1f, true, true, true);
		}
		else
		{
			this.m_attackProjectile = this.FireProjectile("GhostQuakeMinibossProjectile", 0, true, 0f, 1f, true, true, true);
		}
		if (this.m_scare_fireCardinalProjectiles)
		{
			this.FireProjectile("GhostBoltProjectile", 0, true, 90f, 1f, true, true, true);
			this.FireProjectile("GhostBoltProjectile", 0, true, 210f, 1f, true, true, true);
			this.FireProjectile("GhostBoltProjectile", 0, true, 330f, 1f, true, true, true);
		}
		if (this.m_scare_fireDiagonalProjectiles)
		{
			this.FireProjectile("GhostBoltProjectile", 0, true, 270f, 1f, true, true, true);
			this.FireProjectile("GhostBoltProjectile", 0, true, 30f, 1f, true, true, true);
			this.FireProjectile("GhostBoltProjectile", 0, true, 150f, 1f, true, true, true);
		}
		if (this.m_scare_fireMinibossProjectiles)
		{
			for (int i = 1; i <= 10; i++)
			{
				this.FireProjectile("GhostBoltMinibossProjectile", 0, true, (float)(i * 36), 1f, true, true, true);
			}
		}
		yield return this.Default_Animation("Shake_Attack_Hold", 1f, 0f, true);
		base.StopProjectile(ref this.m_attackProjectile);
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Animation("Shake_Exit", this.m_scare_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(this.m_scare_Exit_ForceIdle, this.m_scare_Exit_AttackCD);
		yield break;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0001A5B1 File Offset: 0x000187B1
	private void FixedUpdate()
	{
		if (this.m_toggleDuration > 0f && Time.time > this.m_toggleDuration + this.m_toggleStartTime)
		{
			this.SetInvulnerable(!this.m_isInvulnerable, 0f);
		}
	}

	// Token: 0x04000B0E RID: 2830
	protected float m_scare_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000B0F RID: 2831
	protected float m_scare_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000B10 RID: 2832
	protected float m_scare_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000B11 RID: 2833
	protected float m_scare_AttackIntro_Delay;

	// Token: 0x04000B12 RID: 2834
	protected const float m_scare_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000B13 RID: 2835
	protected const float m_scare_AttackHold_Delay = 0.2f;

	// Token: 0x04000B14 RID: 2836
	protected float m_scare_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000B15 RID: 2837
	protected const float m_scare_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000B16 RID: 2838
	protected float m_scare_Exit_AttackCD;

	// Token: 0x04000B17 RID: 2839
	private Projectile_RL m_warningProjectile;

	// Token: 0x04000B18 RID: 2840
	private Projectile_RL m_attackProjectile;

	// Token: 0x04000B19 RID: 2841
	private SpriteRenderer[] m_tempTransparencyArray;

	// Token: 0x04000B1A RID: 2842
	private bool m_isInvulnerable;

	// Token: 0x04000B1B RID: 2843
	private float m_toggleDuration;

	// Token: 0x04000B1C RID: 2844
	private float m_toggleStartTime;
}

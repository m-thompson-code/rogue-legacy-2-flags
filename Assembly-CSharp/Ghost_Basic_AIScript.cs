using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class Ghost_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000A9B RID: 2715 RVA: 0x00006B36 File Offset: 0x00004D36
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

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06000A9C RID: 2716 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06000A9E RID: 2718 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06000A9F RID: 2719 RVA: 0x000047FD File Offset: 0x000029FD
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(-1f, 1f);
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x000047FD File Offset: 0x000029FD
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(-1f, 1f);
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x000052A9 File Offset: 0x000034A9
	protected virtual float m_scare_TellIntroAndHold_Delay
	{
		get
		{
			return 0.9f;
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_scare_Exit_ForceIdle
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_scare_initialProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_scare_exitProjectileDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_scare_isMiniboss
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_scare_fireCardinalProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_scare_fireDiagonalProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_scare_fireMinibossProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x00003C54 File Offset: 0x00001E54
	protected virtual float VulnerableDuration
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00006B74 File Offset: 0x00004D74
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_tempTransparencyArray = enemyController.GetComponentsInChildren<SpriteRenderer>();
		this.SetInvulnerable(true, 0f);
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00068CE0 File Offset: 0x00066EE0
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

	// Token: 0x06000AAC RID: 2732 RVA: 0x00006B95 File Offset: 0x00004D95
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

	// Token: 0x06000AAD RID: 2733 RVA: 0x00006BA4 File Offset: 0x00004DA4
	private void FixedUpdate()
	{
		if (this.m_toggleDuration > 0f && Time.time > this.m_toggleDuration + this.m_toggleStartTime)
		{
			this.SetInvulnerable(!this.m_isInvulnerable, 0f);
		}
	}

	// Token: 0x04000DAF RID: 3503
	protected float m_scare_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000DB0 RID: 3504
	protected float m_scare_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000DB1 RID: 3505
	protected float m_scare_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000DB2 RID: 3506
	protected float m_scare_AttackIntro_Delay;

	// Token: 0x04000DB3 RID: 3507
	protected const float m_scare_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000DB4 RID: 3508
	protected const float m_scare_AttackHold_Delay = 0.2f;

	// Token: 0x04000DB5 RID: 3509
	protected float m_scare_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000DB6 RID: 3510
	protected const float m_scare_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000DB7 RID: 3511
	protected float m_scare_Exit_AttackCD;

	// Token: 0x04000DB8 RID: 3512
	private Projectile_RL m_warningProjectile;

	// Token: 0x04000DB9 RID: 3513
	private Projectile_RL m_attackProjectile;

	// Token: 0x04000DBA RID: 3514
	private SpriteRenderer[] m_tempTransparencyArray;

	// Token: 0x04000DBB RID: 3515
	private bool m_isInvulnerable;

	// Token: 0x04000DBC RID: 3516
	private float m_toggleDuration;

	// Token: 0x04000DBD RID: 3517
	private float m_toggleStartTime;
}

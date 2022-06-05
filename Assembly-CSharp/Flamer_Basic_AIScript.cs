using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class Flamer_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000880 RID: 2176 RVA: 0x0000607D File Offset: 0x0000427D
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlamerLineBoltProjectile",
			"FlamerLineBoltMinibossProjectile",
			"FlamerWarningProjectile",
			"FlamerBombProjectile",
			"FlamerBombExplosionProjectile"
		};
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06000881 RID: 2177 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_jumpAttack_loseGravityDuration
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x000060B3 File Offset: 0x000042B3
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_waitUntilFallingYield = new WaitUntil(() => base.EnemyController.Velocity.y < 0f);
		this.m_loseGravityDurationYield = new WaitRL_Yield(this.m_jumpAttack_loseGravityDuration, false);
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06000883 RID: 2179 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_flameWalk_Exit_ForceIdle
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06000884 RID: 2180 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_flameWalk_AttackCD
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x00004565 File Offset: 0x00002765
	protected virtual float m_flameWalk_AttackDuration
	{
		get
		{
			return 1.75f;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06000886 RID: 2182 RVA: 0x000060E5 File Offset: 0x000042E5
	protected virtual float m_flameWalk_AttackMoveSpeed
	{
		get
		{
			return 6.5f;
		}
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x000060EC File Offset: 0x000042EC
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator CastFlameWalk()
	{
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		this.m_flameTellProjectile = this.FireProjectile("FlamerWarningProjectile", 2, true, 0f, 1f, true, true, true);
		yield return this.Default_TellIntroAndLoop("Flame_Tell_Intro", this.m_flameWalk_TellIntro_AnimationSpeed, "Flame_Tell_Hold", this.m_flameWalk_TellHold_AnimationSpeed, this.m_flameWalk_Tell_Delay);
		base.StopProjectile(ref this.m_flameTellProjectile);
		yield return this.Default_Animation("Flame_Attack_Intro", this.m_flameWalk_AttackIntro_AnimationSpeed, this.m_flameWalk_AttackIntro_Delay, true);
		yield return this.Default_Animation("Flame_Attack_Hold", this.m_flameWalk_AttackHold_AnimationSpeed, this.m_flameWalk_AttackHold_Delay, false);
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.Regular;
		this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltProjectile", 2, true, 0f, 1f, true, true, true);
		if (isFacingRight)
		{
			base.SetVelocityX(this.m_flameWalk_AttackMoveSpeed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_flameWalk_AttackMoveSpeed, false);
		}
		base.EnemyController.GroundHorizontalVelocity = base.EnemyController.Velocity.x;
		if (this.m_flameWalk_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_flameWalk_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.StopProjectile(ref this.m_flameWalkProjectile);
		yield return this.Default_Animation("Flame_Exit", this.m_flameWalk_Exit_AnimationSpeed, this.m_flameWalk_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_flameWalk_Exit_ForceIdle, this.m_flameWalk_AttackCD);
		yield break;
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06000888 RID: 2184 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_flameBolt_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06000889 RID: 2185 RVA: 0x00003E63 File Offset: 0x00002063
	protected virtual float m_flameBolt_AttackCD
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x0600088A RID: 2186 RVA: 0x000060FB File Offset: 0x000042FB
	protected virtual Vector2 m_flameBolt_Jump
	{
		get
		{
			return new Vector2(-12f, 13f);
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x0600088B RID: 2187 RVA: 0x000050CB File Offset: 0x000032CB
	protected virtual float m_flameBolt_SecondShotDelay
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0000610C File Offset: 0x0000430C
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator FlameBolt()
	{
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("BigFlame_Tell_Intro", this.m_flameBolt_TellIntro_AnimationSpeed, "BigFlame_Tell_Hold", this.m_flameBolt_TellHold_AnimationSpeed, this.m_flameBolt_Tell_Delay);
		yield return this.Default_Animation("BigFlame_Attack_Intro", this.m_flameBolt_AttackIntro_AnimationSpeed, this.m_flameBolt_AttackIntro_Delay, true);
		yield return this.Default_Animation("BigFlame_Attack_Hold", this.m_flameBolt_AttackHold_AnimationSpeed, this.m_flameBolt_AttackHold_Delay, false);
		if (base.EnemyController.IsFacingRight)
		{
			this.FireProjectile("FlamerBombProjectile", 3, false, 0f, 1f, true, true, true);
			base.SetVelocityX(this.m_flameBolt_Jump.x, false);
		}
		else
		{
			this.FireProjectile("FlamerBombProjectile", 3, false, 180f, 1f, true, true, true);
			base.SetVelocityX(-this.m_flameBolt_Jump.x, false);
		}
		base.SetVelocityY(this.m_flameBolt_Jump.y, false);
		yield return base.Wait(0.05f, false);
		if (base.EnemyController.IsFacingRight)
		{
			base.EnemyController.JumpHorizontalVelocity = this.m_flameBolt_Jump.x;
		}
		else
		{
			base.EnemyController.JumpHorizontalVelocity = -this.m_flameBolt_Jump.x;
		}
		if (base.EnemyController.EnemyRank == EnemyRank.Expert)
		{
			yield return base.Wait(this.m_flameBolt_SecondShotDelay, false);
			if (base.EnemyController.IsFacingRight)
			{
				this.FireProjectile("FlamerBombProjectile", 3, false, 0f, 1f, true, true, true);
				base.SetVelocityX(this.m_flameBolt_Jump.x, false);
			}
			else
			{
				this.FireProjectile("FlamerBombProjectile", 3, false, 180f, 1f, true, true, true);
				base.SetVelocityX(-this.m_flameBolt_Jump.x, false);
			}
			base.SetVelocityY(this.m_flameBolt_Jump.y, false);
			yield return base.Wait(0.05f, false);
			if (base.EnemyController.IsFacingRight)
			{
				base.EnemyController.JumpHorizontalVelocity = this.m_flameBolt_Jump.x;
			}
			else
			{
				base.EnemyController.JumpHorizontalVelocity = -this.m_flameBolt_Jump.x;
			}
		}
		yield return base.WaitUntilIsGrounded();
		base.SetVelocityX(0f, false);
		yield return this.Default_Animation("BigFlame_Exit", this.m_flameBolt_Exit_AnimationSpeed, this.m_flameBolt_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_flameBolt_Exit_ForceIdle, this.m_flameBolt_AttackCD);
		yield break;
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x0600088D RID: 2189 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_megaFlame_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x0600088E RID: 2190 RVA: 0x00003E63 File Offset: 0x00002063
	protected virtual float m_megaFlame_AttackCD
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x0600088F RID: 2191 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_megaFlame_hasVerticalDashFlame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06000890 RID: 2192 RVA: 0x00003C54 File Offset: 0x00001E54
	protected virtual float m_megaFlame_AttackDuration
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06000891 RID: 2193 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_megaFlame_AttackMidPauseDuration
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06000892 RID: 2194 RVA: 0x00004536 File Offset: 0x00002736
	protected virtual float m_megaFlame_AttackVertDuration
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06000893 RID: 2195 RVA: 0x0000611B File Offset: 0x0000431B
	protected virtual float m_megaFlame_AttackMoveSpeed
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00006122 File Offset: 0x00004322
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public IEnumerator CastMegaFlame()
	{
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("BigFlame_Tell_Intro", this.m_megaFlame_TellIntro_AnimationSpeed, "BigFlame_Tell_Hold", this.m_megaFlame_TellHold_AnimationSpeed, this.m_megaFlame_Tell_Delay);
		yield return this.Default_Animation("BigFlame_Attack_Intro", this.m_megaFlame_AttackIntro_AnimationSpeed, this.m_megaFlame_AttackIntro_Delay, true);
		yield return this.Default_Animation("BigFlame_Attack_Hold", this.m_megaFlame_AttackHold_AnimationSpeed, this.m_megaFlame_AttackHold_Delay, false);
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.Mega1;
		this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 2, false, 0f, 1f, true, true, true);
		this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 0, false, 180f, 1f, true, true, true);
		if (isFacingRight)
		{
			base.SetVelocityX(this.m_megaFlame_AttackMoveSpeed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_megaFlame_AttackMoveSpeed, false);
		}
		if (this.m_megaFlame_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_megaFlame_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		base.StopProjectile(ref this.m_flameWalkProjectile);
		base.StopProjectile(ref this.m_flameWalkProjectile2);
		if (this.m_megaFlame_AttackMidPauseDuration > 0f)
		{
			yield return base.Wait(this.m_megaFlame_AttackMidPauseDuration, false);
		}
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.Mega2;
		this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 70f, 1f, true, true, true);
		this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 110f, 1f, true, true, true);
		this.m_flameWalkProjectile3 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 90f, 1f, true, true, true);
		if (this.m_megaFlame_AttackVertDuration > 0f)
		{
			yield return base.Wait(this.m_megaFlame_AttackVertDuration, false);
		}
		yield return this.Default_Animation("BigFlame_Exit", this.m_megaFlame_Exit_AnimationSpeed, this.m_megaFlame_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		base.StopProjectile(ref this.m_flameWalkProjectile);
		base.StopProjectile(ref this.m_flameWalkProjectile2);
		base.StopProjectile(ref this.m_flameWalkProjectile3);
		yield return this.Default_Attack_Cooldown(this.m_megaFlame_Exit_ForceIdle, this.m_megaFlame_AttackCD);
		yield break;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x000630A4 File Offset: 0x000612A4
	public override void Pause()
	{
		base.Pause();
		if (this.m_flameWalkProjectile && !this.m_flameWalkProjectile.IsFreePoolObj && this.m_flameWalkProjectile.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_flameWalkProjectile);
		}
		if (this.m_flameWalkProjectile2 && !this.m_flameWalkProjectile2.IsFreePoolObj && this.m_flameWalkProjectile2.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_flameWalkProjectile2);
		}
		if (this.m_flameWalkProjectile3 && !this.m_flameWalkProjectile3.IsFreePoolObj && this.m_flameWalkProjectile3.Owner == base.EnemyController.gameObject)
		{
			this.m_wasFrozenWhileFiring = true;
			base.StopProjectile(ref this.m_flameWalkProjectile3);
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00063198 File Offset: 0x00061398
	public override void Unpause()
	{
		base.Unpause();
		if (this.m_wasFrozenWhileFiring)
		{
			switch (this.m_firingState)
			{
			case Flamer_Basic_AIScript.FlameFiringState.Regular:
				this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltProjectile", 2, true, 0f, 1f, true, true, true);
				break;
			case Flamer_Basic_AIScript.FlameFiringState.Mega1:
				this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 2, false, 0f, 1f, true, true, true);
				this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 0, false, 180f, 1f, true, true, true);
				break;
			case Flamer_Basic_AIScript.FlameFiringState.Mega2:
				this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 70f, 1f, true, true, true);
				this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 110f, 1f, true, true, true);
				this.m_flameWalkProjectile3 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, false, 90f, 1f, true, true, true);
				break;
			}
			this.m_wasFrozenWhileFiring = false;
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x000632A4 File Offset: 0x000614A4
	public override void OnLBCompleteOrCancelled()
	{
		base.StopProjectile(ref this.m_flameTellProjectile);
		base.StopProjectile(ref this.m_flameWalkProjectile);
		base.StopProjectile(ref this.m_flameWalkProjectile2);
		base.StopProjectile(ref this.m_flameWalkProjectile3);
		this.m_wasFrozenWhileFiring = false;
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.OnLBCompleteOrCancelled();
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.None;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00006131 File Offset: 0x00004331
	public override void ResetScript()
	{
		base.EnemyController.ControllerCorgi.GravityActive(true);
		base.EnemyController.DisableFriction = false;
		this.m_firingState = Flamer_Basic_AIScript.FlameFiringState.None;
		base.ResetScript();
	}

	// Token: 0x04000C31 RID: 3121
	protected const string FLAME_TELL_PROJECTILE = "FlamerWarningProjectile";

	// Token: 0x04000C32 RID: 3122
	protected Projectile_RL m_flameTellProjectile;

	// Token: 0x04000C33 RID: 3123
	protected Projectile_RL m_flameWalkProjectile;

	// Token: 0x04000C34 RID: 3124
	protected Projectile_RL m_flameWalkProjectile2;

	// Token: 0x04000C35 RID: 3125
	protected Projectile_RL m_flameWalkProjectile3;

	// Token: 0x04000C36 RID: 3126
	protected Flamer_Basic_AIScript.FlameFiringState m_firingState;

	// Token: 0x04000C37 RID: 3127
	protected WaitUntil m_waitUntilFallingYield;

	// Token: 0x04000C38 RID: 3128
	protected WaitRL_Yield m_loseGravityDurationYield;

	// Token: 0x04000C39 RID: 3129
	protected const string FLAMEWALK_TELL_INTRO = "Flame_Tell_Intro";

	// Token: 0x04000C3A RID: 3130
	protected const string FLAMEWALK_TELL_HOLD = "Flame_Tell_Hold";

	// Token: 0x04000C3B RID: 3131
	protected const string FLAMEWALK_ATTACK_INTRO = "Flame_Attack_Intro";

	// Token: 0x04000C3C RID: 3132
	protected const string FLAMEWALK_ATTACK_HOLD = "Flame_Attack_Hold";

	// Token: 0x04000C3D RID: 3133
	protected const string FLAMEWALK_EXIT = "Flame_Exit";

	// Token: 0x04000C3E RID: 3134
	protected const string FLAMEWALK_LINEBOLT_PROJECTILE = "FlamerLineBoltProjectile";

	// Token: 0x04000C3F RID: 3135
	protected float m_flameWalk_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000C40 RID: 3136
	protected float m_flameWalk_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000C41 RID: 3137
	protected float m_flameWalk_Tell_Delay = 1.15f;

	// Token: 0x04000C42 RID: 3138
	protected float m_flameWalk_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C43 RID: 3139
	protected float m_flameWalk_AttackIntro_Delay;

	// Token: 0x04000C44 RID: 3140
	protected float m_flameWalk_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C45 RID: 3141
	protected float m_flameWalk_AttackHold_Delay;

	// Token: 0x04000C46 RID: 3142
	protected float m_flameWalk_Exit_AnimationSpeed = 1f;

	// Token: 0x04000C47 RID: 3143
	protected float m_flameWalk_Exit_Delay;

	// Token: 0x04000C48 RID: 3144
	protected const string FLAME_BOLT_TELL_INTRO = "BigFlame_Tell_Intro";

	// Token: 0x04000C49 RID: 3145
	protected const string FLAME_BOLT_TELL_HOLD = "BigFlame_Tell_Hold";

	// Token: 0x04000C4A RID: 3146
	protected const string FLAME_BOLT_ATTACK_INTRO = "BigFlame_Attack_Intro";

	// Token: 0x04000C4B RID: 3147
	protected const string FLAME_BOLT_ATTACK_HOLD = "BigFlame_Attack_Hold";

	// Token: 0x04000C4C RID: 3148
	protected const string FLAME_BOLT_EXIT = "BigFlame_Exit";

	// Token: 0x04000C4D RID: 3149
	protected const string FLAME_BOLT_PROJECTILE = "FlamerBombProjectile";

	// Token: 0x04000C4E RID: 3150
	protected const string FLAME_BOLT_EXPLOSION_PROJECTILE = "FlamerBombExplosionProjectile";

	// Token: 0x04000C4F RID: 3151
	protected float m_flameBolt_TellIntro_AnimationSpeed = 1.15f;

	// Token: 0x04000C50 RID: 3152
	protected float m_flameBolt_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000C51 RID: 3153
	protected float m_flameBolt_Tell_Delay = 1.3f;

	// Token: 0x04000C52 RID: 3154
	protected float m_flameBolt_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C53 RID: 3155
	protected float m_flameBolt_AttackIntro_Delay;

	// Token: 0x04000C54 RID: 3156
	protected float m_flameBolt_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C55 RID: 3157
	protected float m_flameBolt_AttackHold_Delay;

	// Token: 0x04000C56 RID: 3158
	protected float m_flameBolt_Exit_AnimationSpeed = 1f;

	// Token: 0x04000C57 RID: 3159
	protected float m_flameBolt_Exit_Delay;

	// Token: 0x04000C58 RID: 3160
	protected const string BIGFLAME_TELL_INTRO = "BigFlame_Tell_Intro";

	// Token: 0x04000C59 RID: 3161
	protected const string BIGFLAME_TELL_HOLD = "BigFlame_Tell_Hold";

	// Token: 0x04000C5A RID: 3162
	protected const string BIGFLAME_ATTACK_INTRO = "BigFlame_Attack_Intro";

	// Token: 0x04000C5B RID: 3163
	protected const string BIGFLAME_ATTACK_HOLD = "BigFlame_Attack_Hold";

	// Token: 0x04000C5C RID: 3164
	protected const string BIGFLAME_EXIT = "BigFlame_Exit";

	// Token: 0x04000C5D RID: 3165
	protected const string BIGFLAME_PROJECTILE = "FlamerLineBoltMinibossProjectile";

	// Token: 0x04000C5E RID: 3166
	protected float m_megaFlame_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000C5F RID: 3167
	protected float m_megaFlame_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000C60 RID: 3168
	protected float m_megaFlame_Tell_Delay = 1.75f;

	// Token: 0x04000C61 RID: 3169
	protected float m_megaFlame_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C62 RID: 3170
	protected float m_megaFlame_AttackIntro_Delay;

	// Token: 0x04000C63 RID: 3171
	protected float m_megaFlame_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C64 RID: 3172
	protected float m_megaFlame_AttackHold_Delay;

	// Token: 0x04000C65 RID: 3173
	protected float m_megaFlame_Exit_AnimationSpeed = 1f;

	// Token: 0x04000C66 RID: 3174
	protected float m_megaFlame_Exit_Delay;

	// Token: 0x04000C67 RID: 3175
	private bool m_wasFrozenWhileFiring;

	// Token: 0x02000147 RID: 327
	protected enum FlameFiringState
	{
		// Token: 0x04000C69 RID: 3177
		None,
		// Token: 0x04000C6A RID: 3178
		Regular,
		// Token: 0x04000C6B RID: 3179
		Mega1,
		// Token: 0x04000C6C RID: 3180
		Mega2
	}
}

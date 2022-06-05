using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000263 RID: 611
public class Wolf_Basic_AIScript : BaseAIScript
{
	// Token: 0x06001187 RID: 4487 RVA: 0x0007F978 File Offset: 0x0007DB78
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"WolfShoutProjectile",
			"WolfShoutAlternateProjectile",
			"WolfShoutWarningProjectile",
			"WolfForwardBeamProjectile",
			"WolfWarningForwardBeamProjectile",
			"WolfIceBoltProjectile",
			"WolfIceGravityBoltProjectile",
			"WolfIceBoltExplosionProjectile"
		};
	}

	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x06001188 RID: 4488 RVA: 0x000091AA File Offset: 0x000073AA
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.45f);
		}
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x06001189 RID: 4489 RVA: 0x00004A26 File Offset: 0x00002C26
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x0600118A RID: 4490 RVA: 0x00004A26 File Offset: 0x00002C26
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x000091BB File Offset: 0x000073BB
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.LogicController.OverrideLogicDelay(0.7f);
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x0600118C RID: 4492 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_howl_TellIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x0600118D RID: 4493 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_howl_TellHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x0600118E RID: 4494 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected virtual float m_howl_TellIntroAndHold_Delay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x0600118F RID: 4495 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_howl_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x06001190 RID: 4496 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_howl_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x06001191 RID: 4497 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_howl_AttackHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x06001192 RID: 4498 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected virtual float m_howl_AttackHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06001193 RID: 4499 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_howl_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x06001194 RID: 4500 RVA: 0x00003D9A File Offset: 0x00001F9A
	protected virtual float m_howl_Exit_AttackCD
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x06001195 RID: 4501 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_howl_Randomize_Howl
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x06001196 RID: 4502 RVA: 0x000091D9 File Offset: 0x000073D9
	protected virtual Vector2 m_howl_Randomize_Howl_Timer
	{
		get
		{
			return new Vector2(0.25f, 2.75f);
		}
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x06001197 RID: 4503 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_howl_Spawn_Projectile
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x06001198 RID: 4504 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_howl_At_Start
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x06001199 RID: 4505 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int m_howlMaxSummons
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x000091EA File Offset: 0x000073EA
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Howl_Attack()
	{
		this.StopAndFaceTarget();
		yield return base.Wait(0.1f, false);
		if (this.m_howl_Randomize_Howl)
		{
			float seconds = UnityEngine.Random.Range(this.m_howl_Randomize_Howl_Timer.x, this.m_howl_Randomize_Howl_Timer.y);
			base.Wait(seconds, false);
		}
		if (this.m_howl_Spawn_Projectile)
		{
			this.m_warningProjectile = this.FireProjectile("WolfShoutWarningProjectile", 1, true, 0f, 1f, true, true, true);
		}
		if (this.m_howl_At_Start)
		{
			yield return this.Default_TellIntroAndLoop("Howl_Tell_Intro", this.m_howl_TellIntro_AnimationSpeed, "Howl_Tell_Hold", this.m_howl_TellHold_AnimationSpeed, 0f);
		}
		else
		{
			yield return this.Default_TellIntroAndLoop("Howl_Tell_Intro", this.m_howl_TellIntro_AnimationSpeed, "Howl_Tell_Hold", this.m_howl_TellHold_AnimationSpeed, this.m_howl_TellIntroAndHold_Delay);
		}
		base.SetAttackingWithContactDamage(true, 0.1f);
		if (this.m_howl_Spawn_Projectile)
		{
			base.StopProjectile(ref this.m_warningProjectile);
			this.m_howlProjectile = this.FireProjectile("WolfShoutAlternateProjectile", 1, true, 0f, 1f, true, true, true);
		}
		if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
		{
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 30f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 60f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 90f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 120f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 150f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 180f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 210f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 240f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 270f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 300f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 330f, 1f, true, true, true);
			this.FireProjectile("WolfIceBoltProjectile", 1, false, 0f, 1f, true, true, true);
		}
		yield return this.Default_Animation("Howl_Attack_Intro", this.m_howl_AttackIntro_AnimationSpeed, this.m_howl_AttackIntro_Delay, true);
		yield return this.Default_Animation("Howl_Attack_Hold", this.m_howl_AttackHold_AnimationSpeed, this.m_howl_AttackHold_Delay, true);
		base.SetAttackingWithContactDamage(false, 0.1f);
		if (this.m_howl_Spawn_Projectile)
		{
			base.StopProjectile(ref this.m_howlProjectile);
		}
		yield return this.Default_Animation("Howl_Exit", this.m_jump_Exit_AnimationSpeed, this.m_jump_Exit_Delay, true);
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_howl_Exit_ForceIdle, this.m_howl_Exit_AttackCD);
		yield break;
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x0600119B RID: 4507 RVA: 0x000091F9 File Offset: 0x000073F9
	protected virtual Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(15f, 26f);
		}
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x0000920A File Offset: 0x0000740A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		yield return this.Jump();
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			yield return this.Dash();
		}
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_jump_Exit_ForceIdle, this.m_jump_Exit_AttackCD);
		yield break;
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x00009219 File Offset: 0x00007419
	private IEnumerator Jump()
	{
		this.StopAndFaceTarget();
		yield return base.Wait(0.1f, false);
		yield return this.Default_TellIntroAndLoop("Pounce_Tell_Intro", this.m_jump_TellIntro_AnimationSpeed, "Pounce_Tell_Hold", this.m_jump_TellHold_AnimationSpeed, this.m_jump_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Pounce_Attack_Intro", this.m_jump_AttackIntro_AnimationSpeed, this.m_jump_AttackIntro_Delay, true);
		yield return this.Single_Action_Jump(this.m_jumpPower.x, this.m_jumpPower.y);
		yield return this.Default_Animation("Pounce_Attack_Hold", this.m_jump_AttackHold_AnimationSpeed, this.m_jump_AttackHold_Delay, false);
		yield return base.WaitUntilIsGrounded();
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.FireProjectile("WolfIceGravityBoltProjectile", 3, true, 115f, 1f, true, true, true);
			this.FireProjectile("WolfIceGravityBoltProjectile", 5, true, 90f, 1f, true, true, true);
			this.FireProjectile("WolfIceGravityBoltProjectile", 4, true, 65f, 1f, true, true, true);
		}
		base.SetVelocityX(0f, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Pounce_Exit", this.m_jump_Exit_AnimationSpeed, this.m_jump_Exit_Delay, true);
		yield break;
	}

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x0600119E RID: 4510 RVA: 0x00004A48 File Offset: 0x00002C48
	protected virtual Vector2 m_dash_TellHold_BackPower
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x0600119F RID: 4511 RVA: 0x00009228 File Offset: 0x00007428
	protected virtual Vector2 m_dash_AttackHold_ForwardPower
	{
		get
		{
			return new Vector2(25f, 12f);
		}
	}

	// Token: 0x060011A0 RID: 4512 RVA: 0x00009239 File Offset: 0x00007439
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Dash_Attack()
	{
		yield return this.Dash();
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			yield return this.Jump();
		}
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_Exit_AttackCD);
		yield break;
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x00009248 File Offset: 0x00007448
	private IEnumerator Dash()
	{
		this.StopAndFaceTarget();
		yield return base.Wait(0.1f, false);
		yield return this.Single_Action_Jump(this.m_dash_TellHold_BackPower.x, this.m_dash_TellHold_BackPower.y);
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dash_TellIntro_AnimationSpeed, "Dash_Tell_Hold", this.m_dash_TellHold_AnimationSpeed, this.m_dash_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		if (this.m_dash_TellHold_BackwardsDelay > 0f)
		{
			yield return base.Wait(this.m_dash_TellHold_BackwardsDelay, false);
		}
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimationSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Single_Action_Jump(this.m_dash_AttackHold_ForwardPower.x, this.m_dash_AttackHold_ForwardPower.y);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimationSpeed, this.m_dash_AttackHold_Delay, false);
		yield return base.WaitUntilIsGrounded();
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			this.FireProjectile("WolfIceGravityBoltProjectile", 3, true, 115f, 1f, true, true, true);
			this.FireProjectile("WolfIceGravityBoltProjectile", 5, true, 90f, 1f, true, true, true);
			this.FireProjectile("WolfIceGravityBoltProjectile", 4, true, 65f, 1f, true, true, true);
		}
		base.SetVelocityX(0f, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Dash_Exit", this.m_dash_Exit_AnimationSpeed, this.m_dash_Exit_Delay, true);
		yield break;
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x00009257 File Offset: 0x00007457
	public IEnumerator Single_Action_Jump(float jumpX, float jumpY)
	{
		if (base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(jumpX, false);
		}
		else
		{
			base.SetVelocityX(-jumpX, false);
		}
		base.SetVelocityY(jumpY, false);
		yield return base.Wait(0.05f, false);
		if (base.EnemyController.IsFacingRight)
		{
			base.EnemyController.JumpHorizontalVelocity = jumpX;
		}
		else
		{
			base.EnemyController.JumpHorizontalVelocity = -jumpX;
		}
		yield break;
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x00009274 File Offset: 0x00007474
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_warningProjectile);
		base.StopProjectile(ref this.m_howlProjectile);
	}

	// Token: 0x04001476 RID: 5238
	protected Projectile_RL m_warningProjectile;

	// Token: 0x04001477 RID: 5239
	protected Projectile_RL m_howlProjectile;

	// Token: 0x04001478 RID: 5240
	protected float m_jump_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04001479 RID: 5241
	protected float m_jump_TellHold_AnimationSpeed = 1f;

	// Token: 0x0400147A RID: 5242
	protected float m_jump_TellIntroAndHold_Delay = 0.55f;

	// Token: 0x0400147B RID: 5243
	protected float m_jump_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x0400147C RID: 5244
	protected float m_jump_AttackIntro_Delay;

	// Token: 0x0400147D RID: 5245
	protected float m_jump_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x0400147E RID: 5246
	protected float m_jump_AttackHold_Delay;

	// Token: 0x0400147F RID: 5247
	protected float m_jump_Exit_AnimationSpeed = 1.25f;

	// Token: 0x04001480 RID: 5248
	protected float m_jump_Exit_Delay;

	// Token: 0x04001481 RID: 5249
	protected float m_jump_Exit_ForceIdle;

	// Token: 0x04001482 RID: 5250
	protected float m_jump_Exit_AttackCD = 10f;

	// Token: 0x04001483 RID: 5251
	protected float m_dash_TellIntro_AnimationSpeed = 1.25f;

	// Token: 0x04001484 RID: 5252
	protected float m_dash_TellHold_AnimationSpeed = 1.25f;

	// Token: 0x04001485 RID: 5253
	protected float m_dash_TellIntroAndHold_Delay;

	// Token: 0x04001486 RID: 5254
	protected float m_dash_TellHold_BackwardsDelay = 0.425f;

	// Token: 0x04001487 RID: 5255
	protected float m_dash_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x04001488 RID: 5256
	protected float m_dash_AttackIntro_Delay;

	// Token: 0x04001489 RID: 5257
	protected float m_dash_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x0400148A RID: 5258
	protected float m_dash_AttackHold_Delay;

	// Token: 0x0400148B RID: 5259
	protected float m_dash_Exit_AnimationSpeed = 0.65f;

	// Token: 0x0400148C RID: 5260
	protected float m_dash_Exit_Delay;

	// Token: 0x0400148D RID: 5261
	protected float m_dash_Exit_ForceIdle;

	// Token: 0x0400148E RID: 5262
	protected float m_dash_Exit_AttackCD = 10f;
}

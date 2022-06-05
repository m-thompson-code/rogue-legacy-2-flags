using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class SpearKnight_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000909 RID: 2313 RVA: 0x0001DBF6 File Offset: 0x0001BDF6
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SpearKnightBoltMinibossProjectile",
			"SpearKnightCurseProjectile",
			"SpearKnightCurseProjectile",
			"SpearKnightDaggerBoltRedProjectile",
			"SpearKnightDaggerBoltRedExpertProjectile",
			"SpearKnightDaggerBoltRedMinibossProjectile"
		};
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x0600090A RID: 2314 RVA: 0x0001DC34 File Offset: 0x0001BE34
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.35f, 0.75f);
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x0600090B RID: 2315 RVA: 0x0001DC45 File Offset: 0x0001BE45
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 0.85f);
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x0600090C RID: 2316 RVA: 0x0001DC56 File Offset: 0x0001BE56
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.5f, 0.5f);
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x0600090D RID: 2317 RVA: 0x0001DC67 File Offset: 0x0001BE67
	protected virtual bool m_raiseKnockbackDefenseWhileAttacking
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x0600090E RID: 2318 RVA: 0x0001DC6A File Offset: 0x0001BE6A
	protected virtual int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x0600090F RID: 2319 RVA: 0x0001DC6D File Offset: 0x0001BE6D
	protected virtual float Dash_AttackSpeed
	{
		get
		{
			return 28f;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06000910 RID: 2320 RVA: 0x0001DC74 File Offset: 0x0001BE74
	protected virtual float Dash_AttackDuration
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06000911 RID: 2321 RVA: 0x0001DC7B File Offset: 0x0001BE7B
	protected virtual bool m_dashAttack_DashOffLedges
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06000912 RID: 2322 RVA: 0x0001DC7E File Offset: 0x0001BE7E
	protected virtual float DashUppercut_AttackSpeed
	{
		get
		{
			return 19f;
		}
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06000913 RID: 2323 RVA: 0x0001DC85 File Offset: 0x0001BE85
	protected virtual float DashUppercut_AttackDuration
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06000914 RID: 2324 RVA: 0x0001DC8C File Offset: 0x0001BE8C
	protected virtual float DashUppercut_JumpSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x0001DC93 File Offset: 0x0001BE93
	protected virtual float Uppercut_JumpPower
	{
		get
		{
			return 27f;
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06000916 RID: 2326 RVA: 0x0001DC9A File Offset: 0x0001BE9A
	protected virtual float m_thrust_AttackSpeed
	{
		get
		{
			return 28f;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06000917 RID: 2327 RVA: 0x0001DCA1 File Offset: 0x0001BEA1
	protected virtual float m_thrust_AttackDuration
	{
		get
		{
			return 0.325f;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06000918 RID: 2328 RVA: 0x0001DCA8 File Offset: 0x0001BEA8
	protected virtual float m_thrust_AttackAmount
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x0001DCAF File Offset: 0x0001BEAF
	protected virtual float m_thrust_AttackLoopDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x0600091A RID: 2330 RVA: 0x0001DCB6 File Offset: 0x0001BEB6
	protected virtual float m_throw_Exit_ForceIdle
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x0001DCBD File Offset: 0x0001BEBD
	protected virtual float m_throw_AttackCD
	{
		get
		{
			return 3.5f;
		}
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x0600091C RID: 2332 RVA: 0x0001DCC4 File Offset: 0x0001BEC4
	protected virtual bool m_throw_Attack_TargetPlayer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x0001DCC7 File Offset: 0x0001BEC7
	protected virtual int m_throw_Attack_ProjectileAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x0600091E RID: 2334 RVA: 0x0001DCCA File Offset: 0x0001BECA
	protected virtual float m_throw_Attack_ProjectileDelay
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0001DCD1 File Offset: 0x0001BED1
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAttack()
	{
		this.StopAndFaceTarget();
		float dashSpeed = this.Dash_AttackSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
		}
		base.EnemyController.LockFlip = true;
		if (this.m_dashAttack_DashOffLedges)
		{
			base.EnemyController.FallLedge = true;
		}
		yield return this.Default_TellIntroAndLoop("Dash_Tell_Intro", this.m_dash_TellIntro_AnimationSpeed, "Dash_Tell_Hold", this.m_dash_TellHold_AnimationSpeed, this.m_dash_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Dash_Attack_Intro", this.m_dash_AttackIntro_AnimationSpeed, this.m_dash_AttackIntro_Delay, true);
		yield return this.Default_Animation("Dash_Attack_Hold", this.m_dash_AttackHold_AnimationSpeed, this.m_dash_AttackHold_Delay, false);
		if (this.m_raiseKnockbackDefenseWhileAttacking && base.EnemyController.BaseKnockbackDefense < (float)this.m_knockbackDefenseBoostOverride)
		{
			base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
		}
		base.SetVelocityX(dashSpeed, false);
		base.EnemyController.GroundHorizontalVelocity = dashSpeed;
		base.EnemyController.DisableFriction = true;
		if (this.Dash_AttackDuration > 0f)
		{
			yield return base.Wait(this.Dash_AttackDuration, false);
		}
		yield return base.WaitUntilIsGrounded();
		base.SetVelocityX(0f, false);
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.EnemyController.DisableFriction = false;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Dash_Exit", this.m_dash_Exit_AnimationSpeed, this.m_dash_Exit_Delay, true);
		base.EnemyController.FallLedge = false;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_dash_Exit_ForceIdle, this.m_dash_AttackCD);
		yield break;
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0001DCE0 File Offset: 0x0001BEE0
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator DashAndUppercutAttack()
	{
		this.StopAndFaceTarget();
		float dashSpeed = this.DashUppercut_AttackSpeed;
		float jumpSpeed = this.DashUppercut_JumpSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			dashSpeed = -dashSpeed;
			jumpSpeed = -jumpSpeed;
		}
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("Combo_Tell_Intro", this.m_dashUppercut_TellIntro_AnimationSpeed, "Combo_Tell_Hold", this.m_dashUppercut_TellHold_AnimationSpeed, this.m_dashUppercut_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Combo_Dash_Attack_Intro", this.m_dashUppercut_DashOnly_AttackIntro_AnimationSpeed, this.m_dashUppercut_DashOnly_AttackIntro_Delay, true);
		yield return this.Default_Animation("Combo_Dash_Attack_Hold", this.m_dashUppercut_DashOnly_AttackHold_AnimationSpeed, this.m_dashUppercut_DashOnly_AttackHold_Delay, false);
		base.SetVelocityX(dashSpeed, false);
		base.EnemyController.GroundHorizontalVelocity = dashSpeed;
		if (this.DashUppercut_AttackDuration > 0f)
		{
			yield return base.Wait(this.DashUppercut_AttackDuration, false);
		}
		yield return this.Default_Animation("Combo_Uppercut_Tell", this.m_dashUppercut_UppercutOnly_AttackIntro_AnimationSpeed, this.m_dashUppercut_UppercutOnly_AttackIntro_Delay, true);
		yield return this.Default_Animation("Combo_Uppercut_Hold", this.m_dashUppercut_UppercutOnly_AttackHold_AnimationSpeed, this.m_dashUppercut_UppercutOnly_AttackHold_Delay, false);
		base.SetVelocity(jumpSpeed, this.Uppercut_JumpPower, false);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
		{
			yield return base.Wait(0.45f, false);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 55f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 125f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 75f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 105f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 95f, 1f, true, true, true);
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, 85f, 1f, true, true, true);
		}
		yield return base.WaitUntilIsGrounded();
		yield return this.Default_Animation("Combo_Exit", this.m_dashUppercut_Exit_AnimationSpeed, this.m_dashUppercut_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Attack_Cooldown(this.m_dashUppercut_Exit_ForceIdle, this.m_dashUppercut_AttackCD);
		yield break;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0001DCEF File Offset: 0x0001BEEF
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ThrustAttack()
	{
		int i = 0;
		while ((float)i < this.m_thrust_AttackAmount)
		{
			float thrustSpeed = this.m_thrust_AttackSpeed;
			if (i == 0)
			{
				yield return this.Default_TellIntroAndLoop("Thrust_Tell_Intro", this.m_thrust_TellIntro_AnimationSpeed, "Thrust_Tell_Hold", this.m_thrust_TellHold_AnimationSpeed, this.m_thrust_TellIntroAndHold_Delay);
			}
			else
			{
				yield return this.Default_TellIntroAndLoop("Thrust_Tell_Intro", this.m_thrust_TellIntro_AnimationSpeed, "Thrust_Tell_Hold", this.m_thrust_TellHold_AnimationSpeed, this.m_thrust_TellIntroAndHoldRepeat_Delay);
			}
			this.StopAndFaceTarget();
			if (!base.EnemyController.IsTargetToMyRight)
			{
				thrustSpeed = -thrustSpeed;
			}
			base.EnemyController.LockFlip = true;
			yield return this.Default_Animation("Thrust_Attack_Intro", this.m_thrust_AttackIntro_AnimationSpeed, this.m_thrust_AttackIntro_Delay, true);
			yield return this.Default_Animation("Thrust_Attack_Hold", this.m_thrust_AttackHold_AnimationSpeed, this.m_thrust_AttackHold_Delay, false);
			if (this.m_raiseKnockbackDefenseWhileAttacking && base.EnemyController.BaseKnockbackDefense < (float)this.m_knockbackDefenseBoostOverride)
			{
				base.EnemyController.BaseKnockbackDefense = (float)this.m_knockbackDefenseBoostOverride;
			}
			base.SetVelocityX(thrustSpeed, false);
			base.EnemyController.GroundHorizontalVelocity = thrustSpeed;
			base.EnemyController.DisableFriction = true;
			if (this.m_thrust_AttackDuration > 0f)
			{
				yield return base.Wait(this.m_thrust_AttackDuration, false);
			}
			base.SetVelocityX(0f, false);
			base.EnemyController.GroundHorizontalVelocity = 0f;
			base.EnemyController.DisableFriction = false;
			if (this.m_thrust_AttackLoopDelay > 0f)
			{
				yield return base.Wait(this.m_thrust_AttackLoopDelay, false);
			}
			yield return this.Default_Animation("Thrust_Exit", this.m_thrust_Exit_AnimationSpeed, this.m_thrust_Exit_Delay, true);
			base.EnemyController.LockFlip = false;
			int num = i;
			i = num + 1;
		}
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_thrust_Exit_ForceIdle, this.m_thrust_AttackCD);
		yield break;
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0001DCFE File Offset: 0x0001BEFE
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[RestLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ThrowAttack()
	{
		this.StopAndFaceTarget();
		if (!this.m_throw_Attack_TargetPlayer)
		{
			base.EnemyController.LockFlip = true;
		}
		yield return this.Default_TellIntroAndLoop("SpearKnight_Throw_Tell_Intro", this.m_throw_TellIntro_AnimationSpeed, "SpearKnight_Throw_Tell_Hold", this.m_throw_TellHold_AnimationSpeed, this.m_throw_TellIntroAndHold_Delay);
		int num2;
		for (int i = 0; i < this.m_throw_Attack_ProjectileAmount; i = num2 + 1)
		{
			yield return this.Default_Animation("SpearKnight_Throw_Attack_Intro", this.m_throw_AttackIntro_AnimationSpeed, this.m_throw_AttackIntro_Delay, true);
			yield return this.Default_Animation("SpearKnight_Throw_Attack_Hold", this.m_throw_AttackHold_AnimationSpeed, 0f, false);
			string projectileName = "SpearKnightDaggerBoltRedProjectile";
			if (base.EnemyController.EnemyRank == EnemyRank.Expert)
			{
				projectileName = "SpearKnightDaggerBoltRedExpertProjectile";
			}
			else if (base.EnemyController.EnemyRank == EnemyRank.Miniboss)
			{
				projectileName = "SpearKnightDaggerBoltRedMinibossProjectile";
			}
			if (this.m_throw_Attack_TargetPlayer)
			{
				float num = Mathf.Atan2(base.EnemyController.TargetController.Midpoint.y - base.EnemyController.Midpoint.y, base.EnemyController.TargetController.Midpoint.x - base.EnemyController.Midpoint.x);
				num = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(num), false);
				this.FireProjectile(projectileName, 1, false, num, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile(projectileName, 1, true, 0f, 1f, true, true, true);
			}
			if (this.m_throw_Attack_ProjectileAmount > 1 && this.m_throw_Attack_ProjectileDelay > 0f)
			{
				yield return base.Wait(this.m_throw_Attack_ProjectileDelay, false);
			}
			num2 = i;
		}
		yield return base.Wait(this.m_throw_AttackHold_Delay, false);
		yield return this.Default_Animation("SpearKnight_Throw_Exit", this.m_throw_Exit_AnimationSpeed, this.m_throw_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_throw_Exit_ForceIdle, this.m_throw_AttackCD);
		yield break;
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x0001DD0D File Offset: 0x0001BF0D
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public virtual IEnumerator UpperCutBullets()
	{
		yield break;
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0001DD15 File Offset: 0x0001BF15
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator HeadBobbleAttack()
	{
		this.StopAndFaceTarget();
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("SpearKnight_HeadShake_Tell_Intro", this.m_headBobble_TellIntro_AnimSpeed, "SpearKnight_HeadShake_Tell_Hold", this.m_headBobble_TellHold_AnimSpeed, this.m_headBobble_TellIntroAndHold_Delay);
		yield return this.Default_Animation("SpearKnight_HeadShake_Attack_Intro", this.m_headBobble_AttackIntro_AnimSpeed, this.m_headBobble_AttackIntro_Delay, false);
		yield return this.ChangeAnimationState("SpearKnight_HeadShake_Attack_Hold");
		float projFireInterval = this.m_headBobble_AttackHold_Delay / (float)this.m_numHeadBobbleProjectiles;
		int num;
		for (int i = 0; i < this.m_numHeadBobbleProjectiles; i = num + 1)
		{
			this.FireProjectile("SpearKnightCurseProjectile", 2, false, 90f, 1f, true, true, true);
			yield return base.Wait(projFireInterval, false);
			num = i;
		}
		yield return base.Wait(this.m_headBobble_AttackHold_Exit_Delay, false);
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("SpearKnight_HeadShake_Exit", this.m_headBobble_Exit_AnimSpeed, this.m_headBobble_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_headBobble_Exit_IdleDuration, this.m_headBobble_AttackCD);
		yield break;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0001DD24 File Offset: 0x0001BF24
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.EnemyController.FallLedge = false;
		base.EnemyController.DisableFriction = false;
		base.EnemyController.GroundHorizontalVelocity = 0f;
		base.EnemyController.LockFlip = false;
	}

	// Token: 0x04000C7A RID: 3194
	private const string MINIBOSS_BOLT_PROJECTILE = "SpearKnightBoltMinibossProjectile";

	// Token: 0x04000C7B RID: 3195
	private const string HEAD_BOBBLE_PROJECTILE = "SpearKnightCurseProjectile";

	// Token: 0x04000C7C RID: 3196
	private const string CURSE_PROJECTILE = "SpearKnightCurseProjectile";

	// Token: 0x04000C7D RID: 3197
	private const string DAGGER_BOLT_PROJECTILE = "SpearKnightDaggerBoltRedProjectile";

	// Token: 0x04000C7E RID: 3198
	private const string EXPERT_DAGGER_BOLT_PROJECTILE = "SpearKnightDaggerBoltRedExpertProjectile";

	// Token: 0x04000C7F RID: 3199
	private const string MINIBOSS_DAGGER_BOLT_PROJECTILE = "SpearKnightDaggerBoltRedMinibossProjectile";

	// Token: 0x04000C80 RID: 3200
	protected float m_dash_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000C81 RID: 3201
	protected float m_dash_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000C82 RID: 3202
	protected float m_dash_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000C83 RID: 3203
	protected float m_dash_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C84 RID: 3204
	protected float m_dash_AttackIntro_Delay;

	// Token: 0x04000C85 RID: 3205
	protected float m_dash_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C86 RID: 3206
	protected float m_dash_AttackHold_Delay;

	// Token: 0x04000C87 RID: 3207
	protected float m_dash_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000C88 RID: 3208
	protected float m_dash_Exit_Delay;

	// Token: 0x04000C89 RID: 3209
	protected float m_dash_Exit_ForceIdle = 0.15f;

	// Token: 0x04000C8A RID: 3210
	protected float m_dash_AttackCD = 2.5f;

	// Token: 0x04000C8B RID: 3211
	protected float m_dashUppercut_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000C8C RID: 3212
	protected float m_dashUppercut_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000C8D RID: 3213
	protected float m_dashUppercut_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000C8E RID: 3214
	protected float m_dashUppercut_DashOnly_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C8F RID: 3215
	protected float m_dashUppercut_DashOnly_AttackIntro_Delay;

	// Token: 0x04000C90 RID: 3216
	protected float m_dashUppercut_DashOnly_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C91 RID: 3217
	protected float m_dashUppercut_DashOnly_AttackHold_Delay;

	// Token: 0x04000C92 RID: 3218
	protected float m_dashUppercut_UppercutOnly_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C93 RID: 3219
	protected float m_dashUppercut_UppercutOnly_AttackIntro_Delay;

	// Token: 0x04000C94 RID: 3220
	protected float m_dashUppercut_UppercutOnly_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000C95 RID: 3221
	protected float m_dashUppercut_UppercutOnly_AttackHold_Delay;

	// Token: 0x04000C96 RID: 3222
	protected float m_dashUppercut_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000C97 RID: 3223
	protected float m_dashUppercut_Exit_Delay;

	// Token: 0x04000C98 RID: 3224
	protected float m_dashUppercut_Exit_ForceIdle = 0.15f;

	// Token: 0x04000C99 RID: 3225
	protected float m_dashUppercut_AttackCD;

	// Token: 0x04000C9A RID: 3226
	protected float m_thrust_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000C9B RID: 3227
	protected float m_thrust_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000C9C RID: 3228
	protected float m_thrust_TellIntroAndHold_Delay = 0.75f;

	// Token: 0x04000C9D RID: 3229
	protected float m_thrust_TellIntroAndHoldRepeat_Delay = 0.15f;

	// Token: 0x04000C9E RID: 3230
	protected float m_thrust_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C9F RID: 3231
	protected float m_thrust_AttackIntro_Delay;

	// Token: 0x04000CA0 RID: 3232
	protected float m_thrust_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000CA1 RID: 3233
	protected float m_thrust_AttackHold_Delay;

	// Token: 0x04000CA2 RID: 3234
	protected float m_thrust_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000CA3 RID: 3235
	protected float m_thrust_Exit_Delay;

	// Token: 0x04000CA4 RID: 3236
	protected float m_thrust_Exit_ForceIdle = 0.15f;

	// Token: 0x04000CA5 RID: 3237
	protected float m_thrust_AttackCD = 2.5f;

	// Token: 0x04000CA6 RID: 3238
	protected float m_throw_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000CA7 RID: 3239
	protected float m_throw_TellHold_AnimationSpeed = 0.65f;

	// Token: 0x04000CA8 RID: 3240
	protected float m_throw_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000CA9 RID: 3241
	protected float m_throw_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000CAA RID: 3242
	protected float m_throw_AttackIntro_Delay;

	// Token: 0x04000CAB RID: 3243
	protected float m_throw_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000CAC RID: 3244
	protected float m_throw_AttackHold_Delay = 0.25f;

	// Token: 0x04000CAD RID: 3245
	protected float m_throw_Exit_AnimationSpeed = 0.45f;

	// Token: 0x04000CAE RID: 3246
	protected float m_throw_Exit_Delay = 0.15f;

	// Token: 0x04000CAF RID: 3247
	protected const string HEAD_BOBBLE_TELL_INTRO = "SpearKnight_HeadShake_Tell_Intro";

	// Token: 0x04000CB0 RID: 3248
	protected const string HEAD_BOBBLE_TELL_HOLD = "SpearKnight_HeadShake_Tell_Hold";

	// Token: 0x04000CB1 RID: 3249
	protected const string HEAD_BOBBLE_ATTACK_INTRO = "SpearKnight_HeadShake_Attack_Intro";

	// Token: 0x04000CB2 RID: 3250
	protected const string HEAD_BOBBLE_ATTACK_HOLD = "SpearKnight_HeadShake_Attack_Hold";

	// Token: 0x04000CB3 RID: 3251
	protected const string HEAD_BOBBLE_EXIT = "SpearKnight_HeadShake_Exit";

	// Token: 0x04000CB4 RID: 3252
	protected float m_headBobble_TellIntro_AnimSpeed = 1f;

	// Token: 0x04000CB5 RID: 3253
	protected float m_headBobble_TellHold_AnimSpeed = 1f;

	// Token: 0x04000CB6 RID: 3254
	protected float m_headBobble_TellIntroAndHold_Delay = 0.9f;

	// Token: 0x04000CB7 RID: 3255
	protected float m_headBobble_AttackIntro_AnimSpeed = 1f;

	// Token: 0x04000CB8 RID: 3256
	protected float m_headBobble_AttackIntro_Delay;

	// Token: 0x04000CB9 RID: 3257
	protected float m_headBobble_AttackHold_AnimSpeed = 1f;

	// Token: 0x04000CBA RID: 3258
	protected float m_headBobble_AttackHold_Delay = 1f;

	// Token: 0x04000CBB RID: 3259
	protected float m_headBobble_AttackHold_Exit_Delay = 0.5f;

	// Token: 0x04000CBC RID: 3260
	protected float m_headBobble_Exit_AnimSpeed = 1f;

	// Token: 0x04000CBD RID: 3261
	protected float m_headBobble_Exit_Delay = 0.45f;

	// Token: 0x04000CBE RID: 3262
	protected float m_headBobble_Exit_IdleDuration = 0.15f;

	// Token: 0x04000CBF RID: 3263
	protected float m_headBobble_AttackCD = 15f;

	// Token: 0x04000CC0 RID: 3264
	protected int m_numHeadBobbleProjectiles = 3;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class Flamer_Miniboss_AIScript : Flamer_Basic_AIScript
{
	// Token: 0x060008AF RID: 2223 RVA: 0x000061BB File Offset: 0x000043BB
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlamerLineBoltProjectile",
			"FlamerLineBoltMinibossProjectile",
			"FlamerLineBoltMinibossProjectile",
			"FlamerWarningProjectile"
		};
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00004A00 File Offset: 0x00002C00
	protected override float m_flameWalk_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00003E63 File Offset: 0x00002063
	protected override float m_flameWalk_AttackCD
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float m_flameWalk_AttackMoveSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00004A00 File Offset: 0x00002C00
	protected virtual float m_jumpAttack_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00003E63 File Offset: 0x00002063
	protected virtual float m_jumpAttack_AttackCD
	{
		get
		{
			return 11f;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x060008B5 RID: 2229 RVA: 0x000061E9 File Offset: 0x000043E9
	protected virtual Vector2 m_jumpAttack_Power
	{
		get
		{
			return new Vector2(-14f, 34f);
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_jumpAttack_hasMultipleFlames
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected virtual float m_jumpAttack_AttackDuration
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_jumpAttack_AttackMoveSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x000061FA File Offset: 0x000043FA
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator JumpAttack()
	{
		this.StopAndFaceTarget();
		yield return base.Wait(this.m_jumpAttack_Tell_Delay, false);
		if (!base.EnemyController.IsFacingRight)
		{
			base.SetVelocityX(this.m_jumpAttack_Power.x, true);
		}
		else
		{
			base.SetVelocityX(-this.m_jumpAttack_Power.x, true);
		}
		base.SetVelocityY(this.m_jumpAttack_Power.y, false);
		yield return base.Wait(0.05f, false);
		yield return this.m_waitUntilFallingYield;
		base.SetVelocity(0f, 0f, false);
		float storedKnockbackDefense = base.EnemyController.BaseKnockbackDefense;
		base.EnemyController.BaseKnockbackDefense = 99f;
		base.EnemyController.ControllerCorgi.GravityActive(false);
		this.StopAndFaceTarget();
		bool isFacingRight = base.EnemyController.IsFacingRight;
		base.EnemyController.LockFlip = true;
		yield return this.Default_TellIntroAndLoop("BigFlame_Tell_Intro", this.m_jumpAttack_TellIntro_AnimationSpeed, "BigFlame_Tell_Intro", this.m_jumpAttack_TellHold_AnimationSpeed, this.m_jumpAttack_Tell_Delay);
		yield return this.Default_Animation("BigFlame_Attack_Intro", this.m_jumpAttack_AttackIntro_AnimationSpeed, this.m_jumpAttack_AttackIntro_Delay, true);
		yield return this.Default_Animation("BigFlame_Attack_Hold", this.m_jumpAttack_AttackHold_AnimationSpeed, this.m_jumpAttack_AttackHold_Delay, false);
		this.m_flameWalkProjectile = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, true, -90f, 1f, true, true, true);
		if (this.m_jumpAttack_hasMultipleFlames)
		{
			this.m_flameWalkProjectile2 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, true, -50f, 1f, true, true, true);
			this.m_flameWalkProjectile3 = this.FireProjectile("FlamerLineBoltMinibossProjectile", 1, true, -130f, 1f, true, true, true);
		}
		if (isFacingRight)
		{
			base.SetVelocityX(this.m_jumpAttack_AttackMoveSpeed, false);
		}
		else
		{
			base.SetVelocityX(-this.m_jumpAttack_AttackMoveSpeed, false);
		}
		if (this.m_jumpAttack_AttackDuration > 0f)
		{
			yield return base.Wait(this.m_jumpAttack_AttackDuration, false);
		}
		base.SetVelocityX(0f, false);
		if (this.m_flameWalkProjectile && !this.m_flameWalkProjectile.IsFreePoolObj && this.m_flameWalkProjectile.Owner == base.EnemyController.gameObject)
		{
			this.m_flameWalkProjectile.FlagForDestruction(null);
		}
		if (this.m_flameWalkProjectile2 && !this.m_flameWalkProjectile2.IsFreePoolObj && this.m_flameWalkProjectile2.Owner == base.EnemyController.gameObject)
		{
			this.m_flameWalkProjectile2.FlagForDestruction(null);
		}
		if (this.m_flameWalkProjectile3 && !this.m_flameWalkProjectile3.IsFreePoolObj && this.m_flameWalkProjectile3.Owner == base.EnemyController.gameObject)
		{
			this.m_flameWalkProjectile3.FlagForDestruction(null);
		}
		yield return this.m_loseGravityDurationYield;
		base.EnemyController.BaseKnockbackDefense = storedKnockbackDefense;
		base.EnemyController.ControllerCorgi.GravityActive(true);
		yield return base.WaitUntilIsGrounded();
		yield return base.Wait(this.m_jumpAttack_Exit_Delay, false);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.LockFlip = false;
		yield return this.Default_Attack_Cooldown(this.m_jumpAttack_Exit_ForceIdle, this.m_jumpAttack_AttackCD);
		yield break;
	}

	// Token: 0x04000C78 RID: 3192
	protected const string FLAMEWALK_LINEBOLT_MINIBOSS_PROJECTILE = "FlamerLineBoltMinibossProjectile";

	// Token: 0x04000C79 RID: 3193
	protected float m_jump_Tell_Delay = 0.75f;

	// Token: 0x04000C7A RID: 3194
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000C7B RID: 3195
	protected float m_jumpAttack_TellIntro_AnimationSpeed = 0.85f;

	// Token: 0x04000C7C RID: 3196
	protected float m_jumpAttack_TellHold_AnimationSpeed = 1.3f;

	// Token: 0x04000C7D RID: 3197
	protected float m_jumpAttack_Tell_Delay = 0.5f;

	// Token: 0x04000C7E RID: 3198
	protected float m_jumpAttack_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000C7F RID: 3199
	protected float m_jumpAttack_AttackIntro_Delay;

	// Token: 0x04000C80 RID: 3200
	protected float m_jumpAttack_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000C81 RID: 3201
	protected float m_jumpAttack_AttackHold_Delay;

	// Token: 0x04000C82 RID: 3202
	protected float m_jumpAttack_Exit_AnimationSpeed = 1f;

	// Token: 0x04000C83 RID: 3203
	protected float m_jumpAttack_Exit_Delay;
}

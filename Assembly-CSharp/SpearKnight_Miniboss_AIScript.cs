using System;
using System.Collections;

// Token: 0x02000128 RID: 296
public class SpearKnight_Miniboss_AIScript : SpearKnight_Basic_AIScript
{
	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x0001DF7B File Offset: 0x0001C17B
	protected override float Dash_AttackSpeed
	{
		get
		{
			return 32f;
		}
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x0001DF82 File Offset: 0x0001C182
	protected override float Dash_AttackDuration
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x0600092C RID: 2348 RVA: 0x0001DF89 File Offset: 0x0001C189
	protected override int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x0600092D RID: 2349 RVA: 0x0001DF8C File Offset: 0x0001C18C
	protected override float DashUppercut_JumpSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x0600092E RID: 2350 RVA: 0x0001DF93 File Offset: 0x0001C193
	protected override bool m_throw_Attack_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x0001DF96 File Offset: 0x0001C196
	protected override int m_throw_Attack_ProjectileAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x0001DF99 File Offset: 0x0001C199
	protected override float m_throw_Attack_ProjectileDelay
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0001DFA0 File Offset: 0x0001C1A0
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[MinibossEnemy]
	public override IEnumerator UpperCutBullets()
	{
		this.StopAndFaceTarget();
		float jumpSpeed = this.UppercutBullet_JumpSpeed;
		if (!base.EnemyController.IsTargetToMyRight)
		{
			jumpSpeed = -this.UppercutBullet_JumpSpeed;
		}
		base.EnemyController.LockFlip = true;
		yield return this.Default_Animation("Combo_Uppercut_Tell", this.m_uppercutBullet_AttackIntro_AnimationSpeed, this.m_uppercutBullet_AttackIntro_Delay, true);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("Combo_Uppercut_Hold", this.m_uppercutBullet_AttackHold_AnimationSpeed, this.m_uppercutBullet_AttackHold_Delay, false);
		base.SetVelocity(jumpSpeed, this.UppercutBullet_JumpPower, false);
		int i = 0;
		while ((float)i < this.UppercutBullet_ProjectileAmount)
		{
			this.FireProjectile("SpearKnightBoltMinibossProjectile", 0, true, this.UppercutBullet_ProjectileAngle, 1f, true, true, true);
			yield return base.Wait(this.UppercutBullet_JumpToMaxHeightDuration / this.UppercutBullet_ProjectileAmount, false);
			int num = i;
			i = num + 1;
		}
		yield return base.WaitUntilIsGrounded();
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Combo_Exit", this.m_uppercutBullet_Exit_AnimationSpeed, this.m_uppercutBullet_Exit_Delay, true);
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.Default_Attack_Cooldown(this.m_uppercutBullet_Exit_ForceIdle, this.m_uppercutBullet_AttackCD);
		yield break;
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0001DFAF File Offset: 0x0001C1AF
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
	}

	// Token: 0x04000CC1 RID: 3265
	protected float m_uppercutBullet_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000CC2 RID: 3266
	protected float m_uppercutBullet_AttackIntro_Delay = 0.5f;

	// Token: 0x04000CC3 RID: 3267
	protected float m_uppercutBullet_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000CC4 RID: 3268
	protected float m_uppercutBullet_AttackHold_Delay;

	// Token: 0x04000CC5 RID: 3269
	protected float m_uppercutBullet_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000CC6 RID: 3270
	protected float m_uppercutBullet_Exit_Delay;

	// Token: 0x04000CC7 RID: 3271
	protected float m_uppercutBullet_Exit_ForceIdle = 0.15f;

	// Token: 0x04000CC8 RID: 3272
	protected float m_uppercutBullet_AttackCD;

	// Token: 0x04000CC9 RID: 3273
	protected float UppercutBullet_JumpPower = 31f;

	// Token: 0x04000CCA RID: 3274
	protected float UppercutBullet_JumpSpeed = 17f;

	// Token: 0x04000CCB RID: 3275
	protected float UppercutBullet_ProjectileAmount = 4f;

	// Token: 0x04000CCC RID: 3276
	protected float UppercutBullet_ProjectileAngle = 90f;

	// Token: 0x04000CCD RID: 3277
	protected float UppercutBullet_JumpToMaxHeightDuration = 1f;
}

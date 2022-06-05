using System;
using System.Collections;

// Token: 0x020001FB RID: 507
public class SpearKnight_Miniboss_AIScript : SpearKnight_Basic_AIScript
{
	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00007DD8 File Offset: 0x00005FD8
	protected override float Dash_AttackSpeed
	{
		get
		{
			return 32f;
		}
	}

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x00006780 File Offset: 0x00004980
	protected override float Dash_AttackDuration
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00003E42 File Offset: 0x00002042
	protected override int m_knockbackDefenseBoostOverride
	{
		get
		{
			return 7;
		}
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float DashUppercut_JumpSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_throw_Attack_TargetPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected override int m_throw_Attack_ProjectileAmount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x00003C5B File Offset: 0x00001E5B
	protected override float m_throw_Attack_ProjectileDelay
	{
		get
		{
			return 0.125f;
		}
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x00007DDF File Offset: 0x00005FDF
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

	// Token: 0x06000DF9 RID: 3577 RVA: 0x00006578 File Offset: 0x00004778
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
	}

	// Token: 0x0400103D RID: 4157
	protected float m_uppercutBullet_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x0400103E RID: 4158
	protected float m_uppercutBullet_AttackIntro_Delay = 0.5f;

	// Token: 0x0400103F RID: 4159
	protected float m_uppercutBullet_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04001040 RID: 4160
	protected float m_uppercutBullet_AttackHold_Delay;

	// Token: 0x04001041 RID: 4161
	protected float m_uppercutBullet_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04001042 RID: 4162
	protected float m_uppercutBullet_Exit_Delay;

	// Token: 0x04001043 RID: 4163
	protected float m_uppercutBullet_Exit_ForceIdle = 0.15f;

	// Token: 0x04001044 RID: 4164
	protected float m_uppercutBullet_AttackCD;

	// Token: 0x04001045 RID: 4165
	protected float UppercutBullet_JumpPower = 31f;

	// Token: 0x04001046 RID: 4166
	protected float UppercutBullet_JumpSpeed = 17f;

	// Token: 0x04001047 RID: 4167
	protected float UppercutBullet_ProjectileAmount = 4f;

	// Token: 0x04001048 RID: 4168
	protected float UppercutBullet_ProjectileAngle = 90f;

	// Token: 0x04001049 RID: 4169
	protected float UppercutBullet_JumpToMaxHeightDuration = 1f;
}

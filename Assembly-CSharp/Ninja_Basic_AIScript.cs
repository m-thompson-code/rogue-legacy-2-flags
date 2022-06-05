using System;
using System.Collections;

// Token: 0x0200019D RID: 413
public class Ninja_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000B77 RID: 2935 RVA: 0x0000712C File Offset: 0x0000532C
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"EyeballBounceBoltProjectile"
		};
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06000B78 RID: 2936 RVA: 0x000047A4 File Offset: 0x000029A4
	protected virtual int NumberOfShurikens
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x00007142 File Offset: 0x00005342
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ThrowShuriken()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		base.EnemyController.OrientToTarget();
		float startingAngle = CDGHelper.WrapAngleDegrees(CDGHelper.ToDegrees(base.EnemyController.Orientation), false);
		float spreadAngle = this.m_shurikenSpreadAngle / (float)this.NumberOfShurikens;
		startingAngle -= (this.m_shurikenSpreadAngle - spreadAngle) / 2f;
		yield return this.Default_TellIntroAndLoop("ThrowRock_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "ThrowRock_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_Tell_Delay);
		yield return this.Default_Animation("ThrowRock_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("ThrowRock_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, 0f, false);
		for (int i = 0; i < this.NumberOfShurikens; i++)
		{
			float angle = startingAngle + spreadAngle * (float)i;
			this.FireProjectile("EyeballBounceBoltProjectile", 0, false, angle, 1f, true, true, true);
		}
		if (this.m_shoot_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_shoot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("ThrowRock_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(1f, 1f);
		yield break;
	}

	// Token: 0x04000E10 RID: 3600
	protected float m_shurikenSpreadAngle = 60f;

	// Token: 0x04000E11 RID: 3601
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000E12 RID: 3602
	protected float m_shoot_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000E13 RID: 3603
	protected float m_shoot_Tell_Delay = 1f;

	// Token: 0x04000E14 RID: 3604
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000E15 RID: 3605
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000E16 RID: 3606
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000E17 RID: 3607
	protected float m_shoot_AttackHold_Delay;

	// Token: 0x04000E18 RID: 3608
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000E19 RID: 3609
	protected float m_shoot_Exit_Delay;
}

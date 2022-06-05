using System;
using System.Collections;

// Token: 0x020000FE RID: 254
public class Ninja_Basic_AIScript : BaseAIScript
{
	// Token: 0x060007E8 RID: 2024 RVA: 0x0001B4E2 File Offset: 0x000196E2
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"EyeballBounceBoltProjectile"
		};
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0001B4F8 File Offset: 0x000196F8
	protected virtual int NumberOfShurikens
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0001B4FB File Offset: 0x000196FB
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

	// Token: 0x04000B4E RID: 2894
	protected float m_shurikenSpreadAngle = 60f;

	// Token: 0x04000B4F RID: 2895
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000B50 RID: 2896
	protected float m_shoot_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000B51 RID: 2897
	protected float m_shoot_Tell_Delay = 1f;

	// Token: 0x04000B52 RID: 2898
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000B53 RID: 2899
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000B54 RID: 2900
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000B55 RID: 2901
	protected float m_shoot_AttackHold_Delay;

	// Token: 0x04000B56 RID: 2902
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000B57 RID: 2903
	protected float m_shoot_Exit_Delay;
}

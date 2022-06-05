using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class Starburst_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000E62 RID: 3682 RVA: 0x00007FF6 File Offset: 0x000061F6
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"StarburstPassBoltProjectile",
			"StarburstBoltProjectile",
			"StarburstVoidProjectile",
			"StarburstSlashBoltMinibossProjectile",
			"StarburstBoltMinibossProjectile",
			"StarburstBoltPassMinibossProjectile"
		};
	}

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00006264 File Offset: 0x00004464
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.3f, 0.7f);
		}
	}

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x06000E64 RID: 3684 RVA: 0x00008034 File Offset: 0x00006234
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.8f);
		}
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00008034 File Offset: 0x00006234
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.8f);
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x06000E66 RID: 3686 RVA: 0x00004F89 File Offset: 0x00003189
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x06000E67 RID: 3687 RVA: 0x00007E2F File Offset: 0x0000602F
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 11f);
		}
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06000E68 RID: 3688 RVA: 0x000047A7 File Offset: 0x000029A7
	protected virtual int NumberOfShots
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06000E69 RID: 3689 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool AlternateShots
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x00008045 File Offset: 0x00006245
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Shoot()
	{
		yield return this.Default_TellIntroAndLoop("SingleShot_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "SingleShot_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_TellHold_Delay);
		yield return this.Default_Animation("SingleShot_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		yield return this.Default_Animation("SingleShot_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, this.m_shoot_AttackHold_Delay, false);
		float num = 360f / (float)this.NumberOfShots;
		float num2 = 0f;
		if (this.AlternateShots && this.AlternateCount == 1)
		{
			num2 = num + num / 2f;
			this.AlternateCount = 0;
		}
		else if (this.AlternateShots && this.AlternateCount == 0)
		{
			this.AlternateCount = 1;
		}
		for (int i = 0; i < this.NumberOfShots; i++)
		{
			if (base.LogicController.EnemyLogicType == EnemyLogicType.Miniboss)
			{
				this.FireProjectile("StarburstBoltPassMinibossProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
			else if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
			{
				this.FireProjectile("StarburstVoidProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
			else
			{
				this.FireProjectile("StarburstBoltProjectile", 0, false, num * (float)i + num2, 1f, true, true, true);
			}
		}
		if (this.m_shoot_AttackHold_Delay > 0f)
		{
			base.Wait(this.m_shoot_AttackHold_Delay, false);
		}
		yield return this.Default_Animation("SingleShot_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x00008054 File Offset: 0x00006254
	private void FixedUpdate()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.EnemyController.Target != null)
		{
			this.UpdateEyeball();
		}
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x00074BA0 File Offset: 0x00072DA0
	private void UpdateEyeball()
	{
		base.EnemyController.Animator.SetFloat("LookDirectionX", base.EnemyController.HeadingX);
		base.EnemyController.Animator.SetFloat("LookDirectionY", base.EnemyController.HeadingY);
	}

	// Token: 0x0400115F RID: 4447
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04001160 RID: 4448
	protected float m_shoot_TellHold_AnimationSpeed = 2f;

	// Token: 0x04001161 RID: 4449
	protected float m_shoot_TellHold_Delay = 0.5f;

	// Token: 0x04001162 RID: 4450
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04001163 RID: 4451
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04001164 RID: 4452
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04001165 RID: 4453
	protected float m_shoot_AttackHold_Delay;

	// Token: 0x04001166 RID: 4454
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04001167 RID: 4455
	protected float m_shoot_Exit_Delay;

	// Token: 0x04001168 RID: 4456
	protected float m_shoot_Exit_ForceIdle = 0.15f;

	// Token: 0x04001169 RID: 4457
	protected float m_shoot_Exit_AttackCD;

	// Token: 0x0400116A RID: 4458
	protected int AlternateCount;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class Starburst_Basic_AIScript : BaseAIScript
{
	// Token: 0x0600095F RID: 2399 RVA: 0x0001EB2F File Offset: 0x0001CD2F
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

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06000960 RID: 2400 RVA: 0x0001EB6D File Offset: 0x0001CD6D
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.3f, 0.7f);
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06000961 RID: 2401 RVA: 0x0001EB7E File Offset: 0x0001CD7E
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.8f);
		}
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06000962 RID: 2402 RVA: 0x0001EB8F File Offset: 0x0001CD8F
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.8f);
		}
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06000963 RID: 2403 RVA: 0x0001EBA0 File Offset: 0x0001CDA0
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return new Vector2(1f, 6f);
		}
	}

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06000964 RID: 2404 RVA: 0x0001EBB1 File Offset: 0x0001CDB1
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return new Vector2(2f, 11f);
		}
	}

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06000965 RID: 2405 RVA: 0x0001EBC2 File Offset: 0x0001CDC2
	protected virtual int NumberOfShots
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06000966 RID: 2406 RVA: 0x0001EBC5 File Offset: 0x0001CDC5
	protected virtual bool AlternateShots
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0001EBC8 File Offset: 0x0001CDC8
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

	// Token: 0x06000968 RID: 2408 RVA: 0x0001EBD7 File Offset: 0x0001CDD7
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

	// Token: 0x06000969 RID: 2409 RVA: 0x0001EBFC File Offset: 0x0001CDFC
	private void UpdateEyeball()
	{
		base.EnemyController.Animator.SetFloat("LookDirectionX", base.EnemyController.HeadingX);
		base.EnemyController.Animator.SetFloat("LookDirectionY", base.EnemyController.HeadingY);
	}

	// Token: 0x04000DB4 RID: 3508
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000DB5 RID: 3509
	protected float m_shoot_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000DB6 RID: 3510
	protected float m_shoot_TellHold_Delay = 0.5f;

	// Token: 0x04000DB7 RID: 3511
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000DB8 RID: 3512
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000DB9 RID: 3513
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000DBA RID: 3514
	protected float m_shoot_AttackHold_Delay;

	// Token: 0x04000DBB RID: 3515
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000DBC RID: 3516
	protected float m_shoot_Exit_Delay;

	// Token: 0x04000DBD RID: 3517
	protected float m_shoot_Exit_ForceIdle = 0.15f;

	// Token: 0x04000DBE RID: 3518
	protected float m_shoot_Exit_AttackCD;

	// Token: 0x04000DBF RID: 3519
	protected int AlternateCount;
}

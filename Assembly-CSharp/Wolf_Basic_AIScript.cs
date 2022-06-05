using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class Wolf_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000B46 RID: 2886 RVA: 0x00022868 File Offset: 0x00020A68
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

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06000B47 RID: 2887 RVA: 0x000228C1 File Offset: 0x00020AC1
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.45f);
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x000228D2 File Offset: 0x00020AD2
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06000B49 RID: 2889 RVA: 0x000228E3 File Offset: 0x00020AE3
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.5f, 1f);
		}
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x000228F4 File Offset: 0x00020AF4
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		base.EnemyController.LogicController.OverrideLogicDelay(0.7f);
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00022912 File Offset: 0x00020B12
	protected virtual float m_howl_TellIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00022919 File Offset: 0x00020B19
	protected virtual float m_howl_TellHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00022920 File Offset: 0x00020B20
	protected virtual float m_howl_TellIntroAndHold_Delay
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00022927 File Offset: 0x00020B27
	protected virtual float m_howl_AttackIntro_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0002292E File Offset: 0x00020B2E
	protected virtual float m_howl_AttackIntro_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00022935 File Offset: 0x00020B35
	protected virtual float m_howl_AttackHold_AnimationSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0002293C File Offset: 0x00020B3C
	protected virtual float m_howl_AttackHold_Delay
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00022943 File Offset: 0x00020B43
	protected virtual float m_howl_Exit_ForceIdle
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0002294A File Offset: 0x00020B4A
	protected virtual float m_howl_Exit_AttackCD
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00022951 File Offset: 0x00020B51
	protected virtual bool m_howl_Randomize_Howl
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00022954 File Offset: 0x00020B54
	protected virtual Vector2 m_howl_Randomize_Howl_Timer
	{
		get
		{
			return new Vector2(0.25f, 2.75f);
		}
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x06000B56 RID: 2902 RVA: 0x00022965 File Offset: 0x00020B65
	protected virtual bool m_howl_Spawn_Projectile
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06000B57 RID: 2903 RVA: 0x00022968 File Offset: 0x00020B68
	protected virtual bool m_howl_At_Start
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0002296B File Offset: 0x00020B6B
	protected virtual int m_howlMaxSummons
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x0002296E File Offset: 0x00020B6E
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

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0002297D File Offset: 0x00020B7D
	protected virtual Vector2 m_jumpPower
	{
		get
		{
			return new Vector2(15f, 26f);
		}
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x0002298E File Offset: 0x00020B8E
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

	// Token: 0x06000B5C RID: 2908 RVA: 0x0002299D File Offset: 0x00020B9D
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

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06000B5D RID: 2909 RVA: 0x000229AC File Offset: 0x00020BAC
	protected virtual Vector2 m_dash_TellHold_BackPower
	{
		get
		{
			return new Vector2(0f, 0f);
		}
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x06000B5E RID: 2910 RVA: 0x000229BD File Offset: 0x00020BBD
	protected virtual Vector2 m_dash_AttackHold_ForwardPower
	{
		get
		{
			return new Vector2(25f, 12f);
		}
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x000229CE File Offset: 0x00020BCE
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

	// Token: 0x06000B60 RID: 2912 RVA: 0x000229DD File Offset: 0x00020BDD
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

	// Token: 0x06000B61 RID: 2913 RVA: 0x000229EC File Offset: 0x00020BEC
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

	// Token: 0x06000B62 RID: 2914 RVA: 0x00022A09 File Offset: 0x00020C09
	public override void OnLBCompleteOrCancelled()
	{
		base.OnLBCompleteOrCancelled();
		base.StopProjectile(ref this.m_warningProjectile);
		base.StopProjectile(ref this.m_howlProjectile);
	}

	// Token: 0x04000FD1 RID: 4049
	protected Projectile_RL m_warningProjectile;

	// Token: 0x04000FD2 RID: 4050
	protected Projectile_RL m_howlProjectile;

	// Token: 0x04000FD3 RID: 4051
	protected float m_jump_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000FD4 RID: 4052
	protected float m_jump_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000FD5 RID: 4053
	protected float m_jump_TellIntroAndHold_Delay = 0.55f;

	// Token: 0x04000FD6 RID: 4054
	protected float m_jump_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000FD7 RID: 4055
	protected float m_jump_AttackIntro_Delay;

	// Token: 0x04000FD8 RID: 4056
	protected float m_jump_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000FD9 RID: 4057
	protected float m_jump_AttackHold_Delay;

	// Token: 0x04000FDA RID: 4058
	protected float m_jump_Exit_AnimationSpeed = 1.25f;

	// Token: 0x04000FDB RID: 4059
	protected float m_jump_Exit_Delay;

	// Token: 0x04000FDC RID: 4060
	protected float m_jump_Exit_ForceIdle;

	// Token: 0x04000FDD RID: 4061
	protected float m_jump_Exit_AttackCD = 10f;

	// Token: 0x04000FDE RID: 4062
	protected float m_dash_TellIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000FDF RID: 4063
	protected float m_dash_TellHold_AnimationSpeed = 1.25f;

	// Token: 0x04000FE0 RID: 4064
	protected float m_dash_TellIntroAndHold_Delay;

	// Token: 0x04000FE1 RID: 4065
	protected float m_dash_TellHold_BackwardsDelay = 0.425f;

	// Token: 0x04000FE2 RID: 4066
	protected float m_dash_AttackIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000FE3 RID: 4067
	protected float m_dash_AttackIntro_Delay;

	// Token: 0x04000FE4 RID: 4068
	protected float m_dash_AttackHold_AnimationSpeed = 1.25f;

	// Token: 0x04000FE5 RID: 4069
	protected float m_dash_AttackHold_Delay;

	// Token: 0x04000FE6 RID: 4070
	protected float m_dash_Exit_AnimationSpeed = 0.65f;

	// Token: 0x04000FE7 RID: 4071
	protected float m_dash_Exit_Delay;

	// Token: 0x04000FE8 RID: 4072
	protected float m_dash_Exit_ForceIdle;

	// Token: 0x04000FE9 RID: 4073
	protected float m_dash_Exit_AttackCD = 10f;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class FlyingSword_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000720 RID: 1824 RVA: 0x0001A16F File Offset: 0x0001836F
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"FlyingSwordProjectile",
			"FlyingSwordMinibossProjectile",
			"FlyingSwordDaggerProjectile"
		};
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001A195 File Offset: 0x00018395
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.45f, 0.75f);
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001A1A6 File Offset: 0x000183A6
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06000723 RID: 1827 RVA: 0x0001A1B7 File Offset: 0x000183B7
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.8f, 1.1f);
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06000724 RID: 1828 RVA: 0x0001A1C8 File Offset: 0x000183C8
	protected virtual float m_thrust_Attack_InitialTargetTurnRate
	{
		get
		{
			return 900f;
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06000725 RID: 1829 RVA: 0x0001A1CF File Offset: 0x000183CF
	protected virtual float m_thrust_Attack_TurnRate
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001A1D6 File Offset: 0x000183D6
	protected virtual float m_thrust_Exit_ForceIdle
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x06000727 RID: 1831 RVA: 0x0001A1DD File Offset: 0x000183DD
	protected virtual float m_thrustFast_DashAttackAmount
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001A1E4 File Offset: 0x000183E4
	protected virtual float m_thrustFast_AttackTell_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001A1EB File Offset: 0x000183EB
	protected virtual float m_thrustFast_AttackHold_Delay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001A1F2 File Offset: 0x000183F2
	protected virtual float m_thrustFastAttackExitEndDuration_Fast
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001A1F9 File Offset: 0x000183F9
	protected virtual bool m_thrust_SpawnProjectilesAtEnd
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001A1FC File Offset: 0x000183FC
	protected virtual bool m_thrust_SpawnMinibossProjectilesAtEnd
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0001A1FF File Offset: 0x000183FF
	public IEnumerator Custom_Tell_Animation(float tellIntroDuration, float tellHoldDuration, float reverseDuration)
	{
		base.EnemyController.FollowOffset = Vector3.zero;
		base.EnemyController.BaseTurnSpeed = this.m_thrust_Attack_InitialTargetTurnRate;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield return this.Default_Animation("Thrust_Tell_Intro", this.m_thrust_TellIntro_AnimationSpeed, 0f, false);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return base.Wait(0.25f, false);
		base.EnemyController.PivotFollowsOrientation = true;
		yield return base.WaitUntilAnimComplete(this.DefaultAnimationLayer);
		if (tellIntroDuration > 0f)
		{
			yield return base.Wait(tellIntroDuration, false);
		}
		yield return this.Default_Animation("Thrust_Tell_Hold", this.m_thrust_TellHold_AnimationSpeed, tellHoldDuration, false);
		yield return this.Default_Animation("Thrust_Attack_Intro", this.m_thrust_AttackIntro_AnimationSpeed, this.m_thrust_AttackIntro_Delay, true);
		yield return this.Default_Animation("Thrust_Attack_Hold_BackUp", this.m_thrust_AttackHold_AnimationSpeed, 0f, false);
		base.EnemyController.BaseSpeed *= -this.m_thrust_Attack_ReverseSpeed;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Away;
		if (reverseDuration > 0f)
		{
			yield return base.Wait(reverseDuration, false);
		}
		base.EnemyController.BaseSpeed = base.EnemyController.EnemyData.Speed;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		yield break;
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0001A223 File Offset: 0x00018423
	public IEnumerator Custom_Thrust_Attack()
	{
		base.EnemyController.BaseTurnSpeed = this.m_thrust_Attack_TurnRate;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = true;
		base.EnemyController.PivotFollowsOrientation = true;
		this.SetAnimationSpeedMultiplier(this.m_thrust_AttackHold_AnimationSpeed);
		yield return this.Default_Animation("Thrust_Attack_Hold", this.m_thrust_AttackHold_AnimationSpeed, 0f, false);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_thrust_Attack_ForwardSpeed;
		if (this.m_thrust_Attack_ForwardDuration > 0f)
		{
			yield return base.Wait(this.m_thrust_Attack_ForwardDuration, false);
		}
		base.EnemyController.BaseSpeed = base.EnemyController.EnemyData.Speed;
		base.EnemyController.BaseTurnSpeed = 0f;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		if (this.m_thrust_SpawnProjectilesAtEnd)
		{
			this.FireProjectile("FlyingSwordProjectile", new Vector2(0f, 0f), true, 45f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordProjectile", new Vector2(0f, 0f), true, 135f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordProjectile", new Vector2(0f, 0f), true, 225f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordProjectile", new Vector2(0f, 0f), true, 315f, 1f, true, true, true);
		}
		else if (this.m_thrust_SpawnMinibossProjectilesAtEnd)
		{
			this.FireProjectile("FlyingSwordMinibossProjectile", new Vector2(0f, 0f), true, 45f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordMinibossProjectile", new Vector2(0f, 0f), true, 135f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordMinibossProjectile", new Vector2(0f, 0f), true, 225f, 1f, true, true, true);
			this.FireProjectile("FlyingSwordMinibossProjectile", new Vector2(0f, 0f), true, 315f, 1f, true, true, true);
		}
		if (this.m_thrust_AttackHold_Delay > 0f)
		{
			yield return base.Wait(this.m_thrust_AttackHold_Delay, false);
		}
		yield break;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0001A232 File Offset: 0x00018432
	public IEnumerator Custom_Fast_Exit_Animation()
	{
		yield return this.Default_Animation("Thrust_Exit_Intro", this.m_thrustFast_ExitIntro_AnimationSpeed, this.m_thrustFastAttackExitEndDuration_Fast, true);
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetPivotRotation();
		base.EnemyController.ResetBaseValues();
		yield break;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x0001A241 File Offset: 0x00018441
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Thrust_Attack()
	{
		base.EnemyController.DisableOffscreenWarnings = false;
		ProjectileManager.AttachOffscreenIcon(base.EnemyController, true);
		yield return this.Custom_Tell_Animation(this.m_thrust_TellIntro_Delay, this.m_thrust_TellHold_Delay, this.m_thrust_TellHold_ReverseDuration);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Custom_Thrust_Attack();
		int i = 0;
		while ((float)i < this.m_thrustFast_DashAttackAmount)
		{
			yield return this.Custom_Fast_Exit_Animation();
			yield return this.Custom_Tell_Animation(this.m_thrustFast_AttackTell_Delay, this.m_thrustFast_AttackHold_Delay, this.m_thrust_TellHold_ReverseDuration);
			yield return this.Custom_Thrust_Attack();
			int num = i;
			i = num + 1;
		}
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("Thrust_Exit_Intro", this.m_thrust_ExitIntro_AnimationSpeed, this.m_thrust_ExitIntro_Delay, true);
		base.EnemyController.PivotFollowsOrientation = false;
		base.EnemyController.ResetPivotRotation();
		yield return this.Default_Animation("Thrust_Exit", this.m_thrust_Exit_AnimationSpeed, this.m_thrust_Exit_Delay, true);
		base.EnemyController.DisableOffscreenWarnings = true;
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.ResetBaseValues();
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield return this.Default_Attack_Cooldown(this.m_thrust_Exit_ForceIdle, this.m_thrust_Exit_AttackCD);
		yield break;
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001A250 File Offset: 0x00018450
	protected virtual float m_vertSpin_Attack_TurnSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001A257 File Offset: 0x00018457
	protected virtual float m_vertSpin_Attack_MovementSpeed
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001A25E File Offset: 0x0001845E
	protected virtual int m_vertSpin_Attack_TotalLoops
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06000734 RID: 1844 RVA: 0x0001A261 File Offset: 0x00018461
	protected virtual float m_vertSpin_Attack_DelayPerLoop
	{
		get
		{
			return 0.225f;
		}
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x0001A268 File Offset: 0x00018468
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Vertical_Spin_Attack()
	{
		base.EnemyController.AlwaysFacing = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.EnemyController.FollowOffset = Vector3.zero;
		yield return this.Default_TellIntroAndLoop("VerticalSpin_Tell_Intro", this.m_vertSpin_TellIntro_AnimationSpeed, "VerticalSpin_Tell_Hold", this.m_vertSpin_TellHold_AnimationSpeed, this.m_vertSpin_TellIntroAndHold_Delay);
		base.SetAttackingWithContactDamage(true, 0.1f);
		yield return this.Default_Animation("VerticalSpin_Attack_Intro", this.m_vertSpin_AttackIntro_AnimationSpeed, this.m_vertSpin_AttackIntro_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Towards;
		base.EnemyController.BaseSpeed = this.m_vertSpin_Attack_MovementSpeed;
		base.EnemyController.BaseTurnSpeed = this.m_vertSpin_Attack_TurnSpeed;
		yield return this.Default_Animation("VerticalSpin_Attack_Hold", 1f, 0.5f, true);
		int num;
		for (int i = 0; i < this.m_vertSpin_Attack_TotalLoops; i = num + 1)
		{
			float angle = CDGHelper.VectorToAngle(base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint);
			this.FireProjectile("FlyingSwordDaggerProjectile", new Vector2(0f, 0f), false, angle, 1f, true, true, true);
			yield return base.Wait(this.m_vertSpin_Attack_DelayPerLoop, false);
			num = i;
		}
		base.EnemyController.ResetBaseValues();
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		base.SetAttackingWithContactDamage(false, 0.1f);
		yield return this.Default_Animation("VerticalSpin_Exit", this.m_vertSpin_ExitIntro_AnimationSpeed, 0.15f, true);
		base.EnemyController.ResetTurnTrigger();
		yield return this.Default_Attack_Cooldown(0.25f, this.m_vertSpin_Exit_AttackCD);
		base.EnemyController.AlwaysFacing = true;
		yield break;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x0001A277 File Offset: 0x00018477
	public override IEnumerator WalkAway()
	{
		base.EnemyController.ResetPivotRotation();
		return base.WalkAway();
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x0001A28A File Offset: 0x0001848A
	public override IEnumerator WalkTowards()
	{
		base.EnemyController.ResetPivotRotation();
		return base.WalkTowards();
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x0001A29D File Offset: 0x0001849D
	public override IEnumerator Idle()
	{
		base.EnemyController.ResetPivotRotation();
		base.EnemyController.FollowTarget = true;
		base.EnemyController.LockFlip = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		this.SetAnimationSpeedMultiplier(this.IdleAnimSpeedMod);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		float num = UnityEngine.Random.Range(this.IdleDuration.x, this.IdleDuration.y);
		if (num > 0f)
		{
			yield return base.Wait(num, false);
		}
		this.SetAnimationSpeedMultiplier(this.IdleAnimSpeedMod);
		yield return null;
		yield break;
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x0001A2AC File Offset: 0x000184AC
	public override void OnLBCompleteOrCancelled()
	{
		base.EnemyController.PivotFollowsOrientation = false;
		base.EnemyController.LockFlip = false;
		base.EnemyController.DisableOffscreenWarnings = true;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x04000AEE RID: 2798
	private const string GENERIC_PROJECTILE_NAME = "FlyingSwordProjectile";

	// Token: 0x04000AEF RID: 2799
	private const string MINIBOSS_PROJECTILE_NAME = "FlyingSwordMinibossProjectile";

	// Token: 0x04000AF0 RID: 2800
	private const string DAGGER_PROJECTILE_NAME = "FlyingSwordDaggerProjectile";

	// Token: 0x04000AF1 RID: 2801
	protected float m_thrust_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000AF2 RID: 2802
	protected float m_thrust_TellIntro_Delay;

	// Token: 0x04000AF3 RID: 2803
	protected float m_thrust_TellHold_AnimationSpeed = 1.5f;

	// Token: 0x04000AF4 RID: 2804
	protected float m_thrust_TellHold_Delay = 0.55f;

	// Token: 0x04000AF5 RID: 2805
	protected float m_thrust_TellHold_ReverseDuration = 0.325f;

	// Token: 0x04000AF6 RID: 2806
	protected float m_thrust_AttackIntro_AnimationSpeed = 2.75f;

	// Token: 0x04000AF7 RID: 2807
	protected float m_thrust_AttackIntro_Delay;

	// Token: 0x04000AF8 RID: 2808
	protected float m_thrust_AttackHold_AnimationSpeed = 2.75f;

	// Token: 0x04000AF9 RID: 2809
	protected float m_thrust_AttackHold_Delay;

	// Token: 0x04000AFA RID: 2810
	protected float m_thrust_ExitIntro_AnimationSpeed = 1.2f;

	// Token: 0x04000AFB RID: 2811
	protected float m_thrust_ExitIntro_Delay = 0.175f;

	// Token: 0x04000AFC RID: 2812
	protected float m_thrust_Exit_AnimationSpeed = 1f;

	// Token: 0x04000AFD RID: 2813
	protected float m_thrust_Exit_Delay;

	// Token: 0x04000AFE RID: 2814
	protected float m_thrust_Attack_ReverseSpeed = -3f;

	// Token: 0x04000AFF RID: 2815
	protected float m_thrust_Attack_ForwardDuration = 0.625f;

	// Token: 0x04000B00 RID: 2816
	protected float m_thrust_Attack_ForwardSpeed = 28f;

	// Token: 0x04000B01 RID: 2817
	protected float m_thrust_Exit_AttackCD = 1f;

	// Token: 0x04000B02 RID: 2818
	protected float m_thrustFast_ExitIntro_AnimationSpeed = 1.25f;

	// Token: 0x04000B03 RID: 2819
	protected float m_vertSpin_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000B04 RID: 2820
	protected float m_vertSpin_TellHold_AnimationSpeed = 1f;

	// Token: 0x04000B05 RID: 2821
	protected float m_vertSpin_TellIntroAndHold_Delay = 0.6f;

	// Token: 0x04000B06 RID: 2822
	protected float m_vertSpin_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000B07 RID: 2823
	protected float m_vertSpin_AttackIntro_Delay;

	// Token: 0x04000B08 RID: 2824
	protected const float m_vertSpin_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000B09 RID: 2825
	protected const float m_vertSpin_AttackHold_Delay = 0.5f;

	// Token: 0x04000B0A RID: 2826
	protected float m_vertSpin_ExitIntro_AnimationSpeed = 1f;

	// Token: 0x04000B0B RID: 2827
	protected const float m_vertSpin_ExitIntro_EndDelay = 0.15f;

	// Token: 0x04000B0C RID: 2828
	protected const float m_vertSpin_Exit_ForceIdle = 0.25f;

	// Token: 0x04000B0D RID: 2829
	protected float m_vertSpin_Exit_AttackCD = 5f;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class Sniper_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000D8A RID: 3466 RVA: 0x00007BAA File Offset: 0x00005DAA
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SniperBoltProjectile",
			"SniperSlashBoltProjectile"
		};
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00004A90 File Offset: 0x00002C90
	protected virtual float m_shoot_AimDelayDuration
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float m_shoot_FireDelayDuration
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int AimShot_TotalShots
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06000D91 RID: 3473 RVA: 0x00003D8C File Offset: 0x00001F8C
	protected virtual float AimShot_MultiShotDelay
	{
		get
		{
			return 0.075f;
		}
	}

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool AimShot_FireSideShot
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06000D93 RID: 3475 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool AimShot_FireSlashBolts
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0000611B File Offset: 0x0000431B
	protected virtual float AimShot_SideShotSpread
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x00070C10 File Offset: 0x0006EE10
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		GameObject @object = enemyController.GetComponent<ObjectReferenceFinder>().GetObject("AimIndicator");
		this.m_aimIndicator = @object.GetComponent<SpriteRenderer>();
		this.m_aimIndicator.transform.SetParent(base.EnemyController.Pivot.transform, true);
		this.m_aimIndicator.gameObject.SetActive(false);
		this.m_aimFollowTarget = true;
		this.FacePlayerImmediately();
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x00007BC8 File Offset: 0x00005DC8
	private void OnEnable()
	{
		if (this.m_aimIndicator)
		{
			this.m_aimIndicator.gameObject.SetActive(false);
		}
		if (base.EnemyController && base.EnemyController.IsInitialized)
		{
			this.FacePlayerImmediately();
		}
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x00070C80 File Offset: 0x0006EE80
	private void FacePlayerImmediately()
	{
		Vector3 localEulerAngles = base.EnemyController.Pivot.transform.localEulerAngles;
		localEulerAngles.z = CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
		base.EnemyController.Pivot.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x00007C08 File Offset: 0x00005E08
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator AimAndFire()
	{
		if (this.m_shoot_AimDelayDuration > 0f)
		{
			yield return base.Wait(this.m_shoot_AimDelayDuration, false);
		}
		this.m_aimFollowTarget = false;
		this.m_aimIndicator.gameObject.SetActive(true);
		base.Animator.SetBool("Armed", true);
		yield return this.Default_TellIntroAndLoop("SingleShot_Tell_Intro", this.m_shoot_TellIntro_AnimationSpeed, "SingleShot_Tell_Hold", this.m_shoot_TellHold_AnimationSpeed, this.m_shoot_TellHold_Delay);
		yield return this.Default_Animation("SingleShot_Attack_Intro", this.m_shoot_AttackIntro_AnimationSpeed, this.m_shoot_AttackIntro_Delay, true);
		if (this.m_shoot_FireDelayDuration > 0f)
		{
			yield return base.Wait(this.m_shoot_FireDelayDuration, false);
		}
		float fireAngle = base.EnemyController.Pivot.transform.localEulerAngles.z;
		int num;
		for (int i = 0; i < this.AimShot_TotalShots; i = num + 1)
		{
			if (this.AimShot_FireSlashBolts)
			{
				this.FireProjectile("SniperSlashBoltProjectile", 0, false, fireAngle, 1f, true, true, true);
				if (this.AimShot_FireSideShot)
				{
					this.FireProjectile("SniperSlashBoltProjectile", 0, false, fireAngle + this.AimShot_SideShotSpread, 1f, true, true, true);
					this.FireProjectile("SniperSlashBoltProjectile", 0, false, fireAngle - this.AimShot_SideShotSpread, 1f, true, true, true);
				}
			}
			else
			{
				this.FireProjectile("SniperBoltProjectile", 0, false, fireAngle, 1f, true, true, true);
				if (this.AimShot_FireSideShot)
				{
					this.FireProjectile("SniperBoltProjectile", 0, false, fireAngle + this.AimShot_SideShotSpread, 1f, true, true, true);
					this.FireProjectile("SniperBoltProjectile", 0, false, fireAngle - this.AimShot_SideShotSpread, 1f, true, true, true);
				}
			}
			if (this.AimShot_MultiShotDelay > 0f)
			{
				yield return base.Wait(this.AimShot_MultiShotDelay, false);
			}
			num = i;
		}
		this.m_aimIndicator.gameObject.SetActive(false);
		base.Animator.SetBool("Armed", false);
		yield return this.Default_Animation("SingleShot_Attack_Hold", this.m_shoot_AttackHold_AnimationSpeed, this.m_shoot_AttackHold_Delay, false);
		yield return this.Default_Animation("SingleShot_Exit", this.m_shoot_Exit_AnimationSpeed, this.m_shoot_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		this.m_aimFollowTarget = true;
		this.m_reaimStartTime = Time.time;
		yield return this.Default_Attack_Cooldown(this.m_shoot_Exit_ForceIdle, this.m_shoot_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x00007C17 File Offset: 0x00005E17
	public override void OnLBCompleteOrCancelled()
	{
		base.Animator.SetBool("Armed", false);
		this.m_aimFollowTarget = true;
		base.OnLBCompleteOrCancelled();
		this.m_aimIndicator.gameObject.SetActive(false);
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x00007C48 File Offset: 0x00005E48
	public override void Unpause()
	{
		base.Unpause();
		this.m_reaimStartTime = Time.time;
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x00070CEC File Offset: 0x0006EEEC
	private void Update()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (base.IsPaused)
		{
			return;
		}
		if (this.m_aimFollowTarget)
		{
			float z = CDGHelper.AngleBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
			Vector3 vector = new Vector3(0f, 0f, z);
			float num = 1f;
			float num2 = this.m_reaimStartTime + num;
			if (Time.time < num2)
			{
				Vector3 localEulerAngles = base.EnemyController.Pivot.transform.localEulerAngles;
				localEulerAngles.z = Mathf.LerpAngle(localEulerAngles.z, vector.z, 1f - (num2 - Time.time / num));
				base.EnemyController.Pivot.transform.localEulerAngles = localEulerAngles;
			}
			else
			{
				base.EnemyController.Pivot.transform.localEulerAngles = vector;
			}
			float num3 = CDGHelper.DistanceBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
			Vector2 direction = (base.EnemyController.TargetController.Midpoint - base.EnemyController.Midpoint).normalized;
			RaycastHit2D hit = Physics2D.Raycast(base.EnemyController.Midpoint, direction, num3, base.EnemyController.ControllerCorgi.PlatformMask);
			if (hit)
			{
				num3 = hit.distance;
			}
			Vector2 size = this.m_aimIndicator.size;
			size.x = num3 * (1f / this.m_aimIndicator.transform.lossyScale.x);
			this.m_aimIndicator.size = size;
		}
	}

	// Token: 0x04000FC4 RID: 4036
	protected float m_shoot_TellIntro_AnimationSpeed = 1f;

	// Token: 0x04000FC5 RID: 4037
	protected float m_shoot_TellHold_AnimationSpeed = 2f;

	// Token: 0x04000FC6 RID: 4038
	protected float m_shoot_TellHold_Delay = 0.5f;

	// Token: 0x04000FC7 RID: 4039
	protected float m_shoot_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000FC8 RID: 4040
	protected float m_shoot_AttackIntro_Delay;

	// Token: 0x04000FC9 RID: 4041
	protected float m_shoot_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000FCA RID: 4042
	protected float m_shoot_AttackHold_Delay;

	// Token: 0x04000FCB RID: 4043
	protected float m_shoot_Exit_AnimationSpeed = 1f;

	// Token: 0x04000FCC RID: 4044
	protected float m_shoot_Exit_Delay;

	// Token: 0x04000FCD RID: 4045
	protected float m_shoot_Exit_ForceIdle = 1.25f;

	// Token: 0x04000FCE RID: 4046
	protected float m_shoot_Exit_AttackCD;

	// Token: 0x04000FCF RID: 4047
	private SpriteRenderer m_aimIndicator;

	// Token: 0x04000FD0 RID: 4048
	private bool m_aimFollowTarget = true;

	// Token: 0x04000FD1 RID: 4049
	private float m_reaimStartTime;

	// Token: 0x04000FD2 RID: 4050
	protected const string SINGLE_SHOT_TELL_INTRO = "SingleShot_Tell_Intro";

	// Token: 0x04000FD3 RID: 4051
	protected const string SINGLE_SHOT_TELL_HOLD = "SingleShot_Tell_Hold";

	// Token: 0x04000FD4 RID: 4052
	protected const string SINGLE_SHOT_ATTACK_INTRO = "SingleShot_Attack_Intro";

	// Token: 0x04000FD5 RID: 4053
	protected const string SINGLE_SHOT_ATTACK_HOLD = "SingleShot_Attack_Hold";

	// Token: 0x04000FD6 RID: 4054
	protected const string SINGLE_SHOT_EXIT = "SingleShot_Exit";

	// Token: 0x04000FD7 RID: 4055
	protected const string SINGLE_SHOT_PROJECTILE = "SniperBoltProjectile";
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class SkeletonArcher_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000C39 RID: 3129 RVA: 0x00004E17 File Offset: 0x00003017
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SkeletonArcherBoltProjectile"
		};
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool m_shoot_ExtraArrowCount
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool m_shootArrow_AimAtPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0000752A File Offset: 0x0000572A
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootArrowLow()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.ShootArrowCoroutine((float)this.m_arrowLowAngle);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_shootArrow_Exit_ForceIdle, this.m_shootArrow_AttackCD);
		yield break;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00007539 File Offset: 0x00005739
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootArrowMedium()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.ShootArrowCoroutine((float)this.m_arrowMediumAngle);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_shootArrow_Exit_ForceIdle, this.m_shootArrow_AttackCD);
		yield break;
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00007548 File Offset: 0x00005748
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator ShootArrowHigh()
	{
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.ShootArrowCoroutine((float)this.m_arrowHighAngle);
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_shootArrow_Exit_ForceIdle, this.m_shootArrow_AttackCD);
		yield break;
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x00007557 File Offset: 0x00005757
	public IEnumerator ShootArrowCoroutine(float angle)
	{
		Vector2 posOffset = this.ARROW_POS_OFFSET;
		if (this.m_shootArrow_AimAtPlayer)
		{
			Vector3 vector = new Vector3(base.EnemyController.Midpoint.x, base.EnemyController.Midpoint.y + posOffset.y, base.EnemyController.Midpoint.z);
			Vector3 midpoint = base.EnemyController.TargetController.Midpoint;
			angle = Mathf.Atan2(midpoint.y - vector.y, midpoint.x - vector.x);
			angle = CDGHelper.ToDegrees(angle);
			angle = CDGHelper.WrapAngleDegrees(angle, true);
			if (!base.EnemyController.IsFacingRight)
			{
				angle = CDGHelper.WrapAngleDegrees(-180f - angle, true);
			}
		}
		posOffset = CDGHelper.RotatedPoint(posOffset, Vector2.zero, angle);
		posOffset += base.EnemyController.Midpoint - base.EnemyController.transform.localPosition;
		float t = (angle - -90f) / 180f;
		float value = Mathf.Lerp(4f, 0f, t);
		base.EnemyController.Animator.SetFloat(SkeletonArcher_Basic_AIScript.m_attackDirectionStringHash, value);
		yield return this.Default_TellIntroAndLoop("Shoot_Tell_Intro", this.m_shootArrow_TellIntro_AnimationSpeed, "Shoot_Tell_Hold", this.m_shootArrow_TellHold_AnimationSpeed, 0.5f);
		yield return this.Default_Animation("Shoot_Attack_Intro", this.m_shootArrow_AttackIntro_AnimationSpeed, this.m_shootArrow_AttackIntro_Delay, true);
		yield return this.Default_Animation("Shoot_Attack_Hold", this.m_shootArrow_AttackHold_AnimationSpeed, 0f, false);
		this.FireProjectile("SkeletonArcherBoltProjectile", posOffset, true, angle, 1f, true, true, true);
		if (this.m_shoot_ExtraArrowCount)
		{
			this.FireProjectile("SkeletonArcherBoltProjectile", posOffset, true, angle + 15f, 1f, true, true, true);
			this.FireProjectile("SkeletonArcherBoltProjectile", posOffset, true, angle - 15f, 1f, true, true, true);
		}
		yield return this.Default_Animation("Shoot_Attack_Exit", this.m_shootArrow_Exit_AnimationSpeed, this.m_shootArrow_Exit_Delay, true);
		yield return this.ChangeAnimationState(BaseAIScript.NEUTRAL_STATE);
		yield break;
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0000756D File Offset: 0x0000576D
	protected virtual Vector2 m_jump_Power
	{
		get
		{
			return new Vector2(3f, 32f);
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x06000C44 RID: 3140 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual int m_jump_Amount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected virtual float m_jump_AimDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0000757E File Offset: 0x0000577E
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump()
	{
		int num;
		for (int i = 0; i < this.m_jump_Amount; i = num + 1)
		{
			this.StopAndFaceTarget();
			base.EnemyController.AlwaysFacing = false;
			yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				base.SetVelocityX(this.m_jump_Power.x, true);
			}
			else
			{
				base.SetVelocityX(-this.m_jump_Power.x, true);
			}
			base.SetVelocityY(this.m_jump_Power.y, false);
			yield return this.ChangeAnimationState("JumpUp");
			yield return base.Wait(0.05f, false);
			yield return base.Wait(this.m_jump_AimDelay, false);
			this.FaceTarget();
			yield return this.ShootArrowCoroutine((float)this.m_arrowHighAngle);
			yield return base.WaitUntilIsGrounded();
			num = i;
		}
		if (this.m_jump_Exit_Delay > 0f)
		{
			yield return base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jump_Exit_ForceIdle, this.m_jump_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0000758D File Offset: 0x0000578D
	private float GetAimAngle(float angle)
	{
		return Mathf.Lerp(2f, 0f, angle / 90f);
	}

	// Token: 0x04000E8B RID: 3723
	private static int m_attackDirectionStringHash = Animator.StringToHash("Attack_Direction");

	// Token: 0x04000E8C RID: 3724
	protected float m_shootArrow_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000E8D RID: 3725
	protected float m_shootArrow_TellHold_AnimationSpeed = 0.75f;

	// Token: 0x04000E8E RID: 3726
	protected const float m_shootArrow_TellIntroAndHold_Delay = 0.5f;

	// Token: 0x04000E8F RID: 3727
	protected float m_shootArrow_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000E90 RID: 3728
	protected float m_shootArrow_AttackIntro_Delay;

	// Token: 0x04000E91 RID: 3729
	protected float m_shootArrow_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000E92 RID: 3730
	protected const float m_shootArrow_AttackHold_Delay = 0f;

	// Token: 0x04000E93 RID: 3731
	protected float m_shootArrow_Exit_AnimationSpeed = 1f;

	// Token: 0x04000E94 RID: 3732
	protected float m_shootArrow_Exit_Delay = 0.15f;

	// Token: 0x04000E95 RID: 3733
	protected int m_arrowHighAngle = 82;

	// Token: 0x04000E96 RID: 3734
	protected int m_arrowMediumAngle = 35;

	// Token: 0x04000E97 RID: 3735
	protected int m_arrowLowAngle = 12;

	// Token: 0x04000E98 RID: 3736
	protected float m_shootArrow_Exit_ForceIdle = 0.15f;

	// Token: 0x04000E99 RID: 3737
	protected float m_shootArrow_AttackCD;

	// Token: 0x04000E9A RID: 3738
	protected const string SHOOT_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000E9B RID: 3739
	protected const string SHOOT_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000E9C RID: 3740
	protected const string SHOOT_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000E9D RID: 3741
	protected const string SHOOT_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000E9E RID: 3742
	protected const string SHOOT_EXIT = "Shoot_Attack_Exit";

	// Token: 0x04000E9F RID: 3743
	protected const string ARROW_PROJECTILE = "SkeletonArcherBoltProjectile";

	// Token: 0x04000EA0 RID: 3744
	protected Vector2 ARROW_POS_OFFSET = new Vector2(1f, 0.25f);

	// Token: 0x04000EA1 RID: 3745
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000EA2 RID: 3746
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000EA3 RID: 3747
	protected float m_jump_Exit_Delay;

	// Token: 0x04000EA4 RID: 3748
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x04000EA5 RID: 3749
	protected float m_jump_Exit_AttackCD = 3f;
}

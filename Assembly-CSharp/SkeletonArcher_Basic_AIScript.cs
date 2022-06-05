using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class SkeletonArcher_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000868 RID: 2152 RVA: 0x0001C426 File Offset: 0x0001A626
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"SkeletonArcherBoltProjectile"
		};
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06000869 RID: 2153 RVA: 0x0001C43C File Offset: 0x0001A63C
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x0600086A RID: 2154 RVA: 0x0001C44D File Offset: 0x0001A64D
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x0600086B RID: 2155 RVA: 0x0001C45E File Offset: 0x0001A65E
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x0600086C RID: 2156 RVA: 0x0001C46F File Offset: 0x0001A66F
	protected virtual bool m_shoot_ExtraArrowCount
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x0600086D RID: 2157 RVA: 0x0001C472 File Offset: 0x0001A672
	protected virtual bool m_shootArrow_AimAtPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0001C475 File Offset: 0x0001A675
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

	// Token: 0x0600086F RID: 2159 RVA: 0x0001C484 File Offset: 0x0001A684
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

	// Token: 0x06000870 RID: 2160 RVA: 0x0001C493 File Offset: 0x0001A693
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

	// Token: 0x06000871 RID: 2161 RVA: 0x0001C4A2 File Offset: 0x0001A6A2
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

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06000872 RID: 2162 RVA: 0x0001C4B8 File Offset: 0x0001A6B8
	protected virtual Vector2 m_jump_Power
	{
		get
		{
			return new Vector2(3f, 32f);
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06000873 RID: 2163 RVA: 0x0001C4C9 File Offset: 0x0001A6C9
	protected virtual int m_jump_Amount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06000874 RID: 2164 RVA: 0x0001C4CC File Offset: 0x0001A6CC
	protected virtual float m_jump_AimDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0001C4D3 File Offset: 0x0001A6D3
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

	// Token: 0x06000876 RID: 2166 RVA: 0x0001C4E2 File Offset: 0x0001A6E2
	private float GetAimAngle(float angle)
	{
		return Mathf.Lerp(2f, 0f, angle / 90f);
	}

	// Token: 0x04000B9E RID: 2974
	private static int m_attackDirectionStringHash = Animator.StringToHash("Attack_Direction");

	// Token: 0x04000B9F RID: 2975
	protected float m_shootArrow_TellIntro_AnimationSpeed = 0.75f;

	// Token: 0x04000BA0 RID: 2976
	protected float m_shootArrow_TellHold_AnimationSpeed = 0.75f;

	// Token: 0x04000BA1 RID: 2977
	protected const float m_shootArrow_TellIntroAndHold_Delay = 0.5f;

	// Token: 0x04000BA2 RID: 2978
	protected float m_shootArrow_AttackIntro_AnimationSpeed = 1f;

	// Token: 0x04000BA3 RID: 2979
	protected float m_shootArrow_AttackIntro_Delay;

	// Token: 0x04000BA4 RID: 2980
	protected float m_shootArrow_AttackHold_AnimationSpeed = 1f;

	// Token: 0x04000BA5 RID: 2981
	protected const float m_shootArrow_AttackHold_Delay = 0f;

	// Token: 0x04000BA6 RID: 2982
	protected float m_shootArrow_Exit_AnimationSpeed = 1f;

	// Token: 0x04000BA7 RID: 2983
	protected float m_shootArrow_Exit_Delay = 0.15f;

	// Token: 0x04000BA8 RID: 2984
	protected int m_arrowHighAngle = 82;

	// Token: 0x04000BA9 RID: 2985
	protected int m_arrowMediumAngle = 35;

	// Token: 0x04000BAA RID: 2986
	protected int m_arrowLowAngle = 12;

	// Token: 0x04000BAB RID: 2987
	protected float m_shootArrow_Exit_ForceIdle = 0.15f;

	// Token: 0x04000BAC RID: 2988
	protected float m_shootArrow_AttackCD;

	// Token: 0x04000BAD RID: 2989
	protected const string SHOOT_TELL_INTRO = "Shoot_Tell_Intro";

	// Token: 0x04000BAE RID: 2990
	protected const string SHOOT_TELL_HOLD = "Shoot_Tell_Hold";

	// Token: 0x04000BAF RID: 2991
	protected const string SHOOT_ATTACK_INTRO = "Shoot_Attack_Intro";

	// Token: 0x04000BB0 RID: 2992
	protected const string SHOOT_ATTACK_HOLD = "Shoot_Attack_Hold";

	// Token: 0x04000BB1 RID: 2993
	protected const string SHOOT_EXIT = "Shoot_Attack_Exit";

	// Token: 0x04000BB2 RID: 2994
	protected const string ARROW_PROJECTILE = "SkeletonArcherBoltProjectile";

	// Token: 0x04000BB3 RID: 2995
	protected Vector2 ARROW_POS_OFFSET = new Vector2(1f, 0.25f);

	// Token: 0x04000BB4 RID: 2996
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000BB5 RID: 2997
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000BB6 RID: 2998
	protected float m_jump_Exit_Delay;

	// Token: 0x04000BB7 RID: 2999
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x04000BB8 RID: 3000
	protected float m_jump_Exit_AttackCD = 3f;
}

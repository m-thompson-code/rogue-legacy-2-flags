using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class JumpWallStick_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000AC2 RID: 2754 RVA: 0x000046A7 File Offset: 0x000028A7
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[0];
	}

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00003E00 File Offset: 0x00002000
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00069098 File Offset: 0x00067298
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			Vector3 localEulerAngles = base.EnemyController.Pivot.transform.localEulerAngles;
			localEulerAngles.z = 0f;
			base.EnemyController.Pivot.transform.localEulerAngles = localEulerAngles;
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x000690E8 File Offset: 0x000672E8
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_wallStickCollision = base.EnemyController.GetComponent<WallStickCollision>();
		this.m_wallStickCollision.enabled = false;
		base.EnemyController.ControllerCorgi.DefaultParameters.Gravity = 0f;
		base.LogicController.ExecuteLogicInAir = true;
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00003CEB File Offset: 0x00001EEB
	protected virtual float Jump_Power
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual bool Jump_SpawnProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00006C3F File Offset: 0x00004E3F
	[CloseLogic]
	[MediumLogic]
	[FarLogic]
	[BasicEnemy]
	[AdvancedEnemy]
	[ExpertEnemy]
	[MinibossEnemy]
	public IEnumerator Jump_Attack()
	{
		Vector2 jumpAngleVector = CDGHelper.VectorBetweenPts(base.EnemyController.Midpoint, base.EnemyController.TargetController.Midpoint);
		jumpAngleVector.Normalize();
		if (!this.AllowJumpAttack(jumpAngleVector))
		{
			yield return this.Idle();
			yield break;
		}
		this.StopAndFaceTarget();
		base.EnemyController.AlwaysFacing = false;
		yield return this.Default_Animation("Jump_Tell", this.m_jump_Tell_AnimationSpeed, this.m_jump_Tell_Delay, true);
		base.EnemyController.FlyingMovementType = FlyingMovementType.Override;
		base.EnemyController.SetVelocity(jumpAngleVector.x * this.Jump_Power, jumpAngleVector.y * this.Jump_Power, false);
		yield return this.ChangeAnimationState("JumpUp");
		yield return base.Wait(0.05f, false);
		this.m_wallStickCollision.enabled = true;
		while (!this.m_wallStickCollision.StickingToWall)
		{
			yield return null;
		}
		base.Animator.SetBool("Grounded", true);
		this.m_wallStickCollision.enabled = false;
		base.EnemyController.FlyingMovementType = FlyingMovementType.Stop;
		if (this.m_jump_Exit_Delay > 0f)
		{
			base.Wait(this.m_jump_Exit_Delay, false);
		}
		base.EnemyController.AlwaysFacing = true;
		yield return this.Default_Attack_Cooldown(this.m_jump_Exit_ForceIdle, this.m_jump_Exit_AttackCD);
		yield break;
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00069140 File Offset: 0x00067340
	private bool AllowJumpAttack(Vector2 jumpVector)
	{
		RaycastHit2D hit = Physics2D.Raycast(base.EnemyController.Midpoint, jumpVector, 20f, base.EnemyController.ControllerCorgi.PlatformMask);
		return !hit || !this.m_wallStickCollision.LastHitRaycast || (hit && this.m_wallStickCollision.LastHitRaycast && hit.collider != this.m_wallStickCollision.LastHitRaycast.collider);
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00006C4E File Offset: 0x00004E4E
	public override void OnLBCompleteOrCancelled()
	{
		this.m_wallStickCollision.enabled = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x04000DC1 RID: 3521
	private WallStickCollision m_wallStickCollision;

	// Token: 0x04000DC2 RID: 3522
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000DC3 RID: 3523
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000DC4 RID: 3524
	protected float m_jump_Exit_Delay;

	// Token: 0x04000DC5 RID: 3525
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x04000DC6 RID: 3526
	protected float m_jump_Exit_AttackCD;
}

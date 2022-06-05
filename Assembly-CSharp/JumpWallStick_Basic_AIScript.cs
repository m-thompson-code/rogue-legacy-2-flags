using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class JumpWallStick_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000769 RID: 1897 RVA: 0x0001A676 File Offset: 0x00018876
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[0];
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x0600076A RID: 1898 RVA: 0x0001A684 File Offset: 0x00018884
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x0600076B RID: 1899 RVA: 0x0001A695 File Offset: 0x00018895
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x0600076C RID: 1900 RVA: 0x0001A6A6 File Offset: 0x000188A6
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(0.45f, 0.85f);
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x0001A6B8 File Offset: 0x000188B8
	private void OnEnable()
	{
		if (base.IsInitialized)
		{
			Vector3 localEulerAngles = base.EnemyController.Pivot.transform.localEulerAngles;
			localEulerAngles.z = 0f;
			base.EnemyController.Pivot.transform.localEulerAngles = localEulerAngles;
		}
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x0001A708 File Offset: 0x00018908
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		this.m_wallStickCollision = base.EnemyController.GetComponent<WallStickCollision>();
		this.m_wallStickCollision.enabled = false;
		base.EnemyController.ControllerCorgi.DefaultParameters.Gravity = 0f;
		base.LogicController.ExecuteLogicInAir = true;
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x0600076F RID: 1903 RVA: 0x0001A75F File Offset: 0x0001895F
	protected virtual float Jump_Power
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06000770 RID: 1904 RVA: 0x0001A766 File Offset: 0x00018966
	protected virtual bool Jump_SpawnProjectiles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0001A769 File Offset: 0x00018969
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

	// Token: 0x06000772 RID: 1906 RVA: 0x0001A778 File Offset: 0x00018978
	private bool AllowJumpAttack(Vector2 jumpVector)
	{
		RaycastHit2D hit = Physics2D.Raycast(base.EnemyController.Midpoint, jumpVector, 20f, base.EnemyController.ControllerCorgi.PlatformMask);
		return !hit || !this.m_wallStickCollision.LastHitRaycast || (hit && this.m_wallStickCollision.LastHitRaycast && hit.collider != this.m_wallStickCollision.LastHitRaycast.collider);
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0001A80E File Offset: 0x00018A0E
	public override void OnLBCompleteOrCancelled()
	{
		this.m_wallStickCollision.enabled = false;
		base.OnLBCompleteOrCancelled();
	}

	// Token: 0x04000B1D RID: 2845
	private WallStickCollision m_wallStickCollision;

	// Token: 0x04000B1E RID: 2846
	protected float m_jump_Tell_Delay = 0.4f;

	// Token: 0x04000B1F RID: 2847
	protected float m_jump_Tell_AnimationSpeed = 0.75f;

	// Token: 0x04000B20 RID: 2848
	protected float m_jump_Exit_Delay;

	// Token: 0x04000B21 RID: 2849
	protected float m_jump_Exit_ForceIdle = 0.15f;

	// Token: 0x04000B22 RID: 2850
	protected float m_jump_Exit_AttackCD;
}

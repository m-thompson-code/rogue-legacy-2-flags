using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

// Token: 0x0200049D RID: 1181
[RequireComponent(typeof(CorgiController))]
public class PreventPlatformDrop : MonoBehaviour
{
	// Token: 0x06002616 RID: 9750 RVA: 0x00015296 File Offset: 0x00013496
	private void Awake()
	{
		this.m_controller = base.GetComponent<CorgiController_RL>();
		this.m_enemyController = base.GetComponent<EnemyController>();
	}

	// Token: 0x06002617 RID: 9751 RVA: 0x000152B0 File Offset: 0x000134B0
	private IEnumerator Start()
	{
		while (!this.m_controller.IsInitialized || !this.m_enemyController.IsInitialized)
		{
			yield return null;
		}
		this.m_raycastLength = this.m_enemyController.ControllerCorgi.BoundsWidth + 0.1f;
		yield break;
	}

	// Token: 0x06002618 RID: 9752 RVA: 0x000B4BB8 File Offset: 0x000B2DB8
	private void FixedUpdate()
	{
		if (this.m_controller.State != null && this.m_controller.State.IsGrounded && this.m_controller.StandingOnCollider && !this.m_enemyController.KnockedIntoAir && (this.m_controller.StandingOnCollider.CompareTag("Platform") || this.m_controller.StandingOnCollider.CompareTag("OneWay")))
		{
			this.CheckGroundRaycast_V2();
		}
	}

	// Token: 0x06002619 RID: 9753 RVA: 0x000B4C38 File Offset: 0x000B2E38
	private void CheckGroundRaycast_V2()
	{
		this.m_enemyController.LeftSidePlatformDropPrevented = false;
		this.m_enemyController.RightSidePlatformDropPrevented = false;
		Vector2 rayOriginPoint = this.m_controller.BoundsBottomLeftCorner + new Vector2(-0.1f, -0.1f);
		Vector2 rayOriginPoint2 = this.m_controller.BoundsBottomRightCorner + new Vector2(0.1f, -0.1f);
		this.m_raycastLeft = MMDebug.RayCast(rayOriginPoint, base.transform.right, this.m_raycastLength, this.m_controller.PlatformMask, Color.blue, this.m_controller.Parameters.DrawRaycastsGizmos);
		this.m_raycastRight = MMDebug.RayCast(rayOriginPoint2, -base.transform.right, this.m_raycastLength, this.m_controller.PlatformMask, Color.blue, this.m_controller.Parameters.DrawRaycastsGizmos);
		bool flag = true;
		if (this.m_raycastLeft && this.m_raycastLeft.distance > 0f && Mathf.Abs(Vector2.Angle(this.m_raycastLeft.normal, base.transform.up)) > this.m_enemyController.ControllerCorgi.Parameters.MaximumSlopeAngle)
		{
			flag = false;
		}
		bool flag2 = true;
		if (this.m_raycastRight && this.m_raycastRight.distance > 0f && Mathf.Abs(Vector2.Angle(this.m_raycastRight.normal, base.transform.up)) > this.m_enemyController.ControllerCorgi.Parameters.MaximumSlopeAngle)
		{
			flag2 = false;
		}
		if (!this.m_raycastLeft || (this.m_raycastLeft && !flag))
		{
			this.m_enemyController.LeftSidePlatformDropPrevented = true;
			if (this.m_controller.Velocity.x < 0f)
			{
				this.m_controller.SetForce(new Vector2(0f, this.m_controller.Velocity.y));
			}
		}
		if (!this.m_raycastRight || (this.m_raycastRight && !flag2))
		{
			this.m_enemyController.RightSidePlatformDropPrevented = true;
			if (this.m_controller.Velocity.x > 0f)
			{
				this.m_controller.SetForce(new Vector2(0f, this.m_controller.Velocity.y));
			}
		}
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x000B4EB8 File Offset: 0x000B30B8
	private void CheckGroundRaycast()
	{
		this.m_enemyController.LeftSidePlatformDropPrevented = false;
		this.m_enemyController.RightSidePlatformDropPrevented = false;
		Vector2 rayOriginPoint = this.m_controller.BoundsBottomLeftCorner;
		rayOriginPoint.y += 0.01f;
		Vector2 rayOriginPoint2 = this.m_controller.BoundsBottomRightCorner;
		rayOriginPoint2.y += 0.01f;
		this.m_raycastLeft = MMDebug.RayCast(rayOriginPoint, -base.transform.up, this.m_raycastLength, this.m_controller.PlatformMask, Color.blue, this.m_controller.Parameters.DrawRaycastsGizmos);
		this.m_raycastRight = MMDebug.RayCast(rayOriginPoint2, -base.transform.up, this.m_raycastLength, this.m_controller.PlatformMask, Color.blue, this.m_controller.Parameters.DrawRaycastsGizmos);
		float num = 0f;
		float num2 = 0f;
		bool flag = true;
		if (this.m_raycastLeft)
		{
			num2 = Mathf.Abs(Vector2.Angle(this.m_raycastLeft.normal, base.transform.up));
			if (num2 > this.m_enemyController.ControllerCorgi.Parameters.MaximumSlopeAngle)
			{
				flag = false;
			}
		}
		bool flag2 = true;
		if (this.m_raycastRight)
		{
			num = Mathf.Abs(Vector2.Angle(this.m_raycastRight.normal, base.transform.up));
			if (num > this.m_enemyController.ControllerCorgi.Parameters.MaximumSlopeAngle)
			{
				flag2 = false;
			}
		}
		if (num2 == 0f && this.m_raycastLeft.distance > 0.5f && num == 0f)
		{
			flag = false;
		}
		else if (num == 0f && this.m_raycastRight.distance > 0.5f && num2 == 0f)
		{
			flag2 = false;
		}
		if (!this.m_raycastLeft || (this.m_raycastLeft && !flag))
		{
			this.m_enemyController.LeftSidePlatformDropPrevented = true;
			if (this.m_controller.Velocity.x < 0f)
			{
				this.m_controller.SetForce(new Vector2(0f, this.m_controller.Velocity.y));
			}
		}
		if (!this.m_raycastRight || (this.m_raycastRight && !flag2))
		{
			this.m_enemyController.RightSidePlatformDropPrevented = true;
			if (this.m_controller.Velocity.x > 0f)
			{
				this.m_controller.SetForce(new Vector2(0f, this.m_controller.Velocity.y));
			}
		}
	}

	// Token: 0x04002104 RID: 8452
	private CorgiController_RL m_controller;

	// Token: 0x04002105 RID: 8453
	private EnemyController m_enemyController;

	// Token: 0x04002106 RID: 8454
	private float m_raycastLength;

	// Token: 0x04002107 RID: 8455
	private RaycastHit2D m_raycastLeft;

	// Token: 0x04002108 RID: 8456
	private RaycastHit2D m_raycastRight;
}

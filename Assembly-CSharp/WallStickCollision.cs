using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003EC RID: 1004
public class WallStickCollision : MonoBehaviour
{
	// Token: 0x17000EE8 RID: 3816
	// (get) Token: 0x06002500 RID: 9472 RVA: 0x0007ADB0 File Offset: 0x00078FB0
	// (set) Token: 0x06002501 RID: 9473 RVA: 0x0007ADB8 File Offset: 0x00078FB8
	public StickToWallCollisionDelegate OnStickEvent { get; set; }

	// Token: 0x17000EE9 RID: 3817
	// (get) Token: 0x06002502 RID: 9474 RVA: 0x0007ADC1 File Offset: 0x00078FC1
	public bool StickingToWall
	{
		get
		{
			return this.m_controller.Velocity == Vector2.zero;
		}
	}

	// Token: 0x17000EEA RID: 3818
	// (get) Token: 0x06002503 RID: 9475 RVA: 0x0007ADD8 File Offset: 0x00078FD8
	// (set) Token: 0x06002504 RID: 9476 RVA: 0x0007ADE0 File Offset: 0x00078FE0
	public RaycastHit2D LastHitRaycast { get; set; }

	// Token: 0x06002505 RID: 9477 RVA: 0x0007ADEC File Offset: 0x00078FEC
	private void Awake()
	{
		GameObject root = this.GetRoot(false);
		this.m_controller = root.GetComponent<BaseCharacterController>();
		base.GetComponent<PreventPlatformDrop>();
		this.m_stickToWall = new Action<CorgiController_RL>(this.StickToWall);
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x0007AE28 File Offset: 0x00079028
	private void OnEnable()
	{
		if (!this.m_stickListenerAdded)
		{
			if (!this.m_controller.IsInitialized)
			{
				base.StartCoroutine(this.EnableCollisionCoroutine());
				return;
			}
			this.m_stickListenerAdded = true;
			this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.AddListener(this.m_stickToWall, false);
		}
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x0007AE7C File Offset: 0x0007907C
	private IEnumerator EnableCollisionCoroutine()
	{
		yield return new WaitUntil(() => this.m_controller.IsInitialized);
		this.m_stickListenerAdded = true;
		this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.AddListener(this.m_stickToWall, false);
		yield break;
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x0007AE8B File Offset: 0x0007908B
	private void OnDisable()
	{
		if (this.m_stickListenerAdded)
		{
			this.m_stickListenerAdded = false;
			this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.RemoveListener(this.m_stickToWall);
		}
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x0007AEB8 File Offset: 0x000790B8
	private void StickToWall(CorgiController_RL corgiController)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (this.StickingToWall)
		{
			return;
		}
		if (this.OnStickEvent != null)
		{
			this.OnStickEvent(base.gameObject);
		}
		this.m_controller.SetVelocity(0f, 0f, false);
		if (corgiController != null && corgiController.ContactList.Count > 0)
		{
			RaycastHit2D lastHitRaycast = corgiController.ContactList[0];
			Vector3 localEulerAngles = this.m_controller.Pivot.transform.localEulerAngles;
			localEulerAngles.z = CDGHelper.VectorToAngle(lastHitRaycast.normal) - 90f;
			this.m_controller.Pivot.transform.localEulerAngles = localEulerAngles;
			this.LastHitRaycast = lastHitRaycast;
		}
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x0007AF76 File Offset: 0x00079176
	private void LateUpdate()
	{
		this.ConstrainEnemyMovementToRoom();
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x0007AF80 File Offset: 0x00079180
	private void ConstrainEnemyMovementToRoom()
	{
		Rect collisionBounds = this.m_controller.CollisionBounds;
		if (collisionBounds.width == 0f || collisionBounds.height == 0f)
		{
			return;
		}
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		if (currentPlayerRoom)
		{
			Rect boundsRect = currentPlayerRoom.BoundsRect;
			if (collisionBounds.xMin <= boundsRect.xMin)
			{
				this.StickToWall(null);
				Vector3 localEulerAngles = this.m_controller.Pivot.transform.localEulerAngles;
				localEulerAngles.z = -90f;
				this.m_controller.Pivot.transform.localEulerAngles = localEulerAngles;
				this.LastHitRaycast = default(RaycastHit2D);
				return;
			}
			if (collisionBounds.xMax >= boundsRect.xMax)
			{
				this.StickToWall(null);
				Vector3 localEulerAngles2 = this.m_controller.Pivot.transform.localEulerAngles;
				localEulerAngles2.z = 90f;
				this.m_controller.Pivot.transform.localEulerAngles = localEulerAngles2;
				this.LastHitRaycast = default(RaycastHit2D);
				return;
			}
			if (collisionBounds.yMin <= boundsRect.yMin)
			{
				this.StickToWall(null);
				Vector3 localEulerAngles3 = this.m_controller.Pivot.transform.localEulerAngles;
				localEulerAngles3.z = 0f;
				this.m_controller.Pivot.transform.localEulerAngles = localEulerAngles3;
				this.LastHitRaycast = default(RaycastHit2D);
				return;
			}
			if (collisionBounds.yMax >= boundsRect.yMax)
			{
				this.StickToWall(null);
				Vector3 localEulerAngles4 = this.m_controller.Pivot.transform.localEulerAngles;
				localEulerAngles4.z = 180f;
				this.m_controller.Pivot.transform.localEulerAngles = localEulerAngles4;
				this.LastHitRaycast = default(RaycastHit2D);
			}
		}
	}

	// Token: 0x04001F51 RID: 8017
	private BaseCharacterController m_controller;

	// Token: 0x04001F52 RID: 8018
	private bool m_stickListenerAdded;

	// Token: 0x04001F53 RID: 8019
	private Action<CorgiController_RL> m_stickToWall;
}

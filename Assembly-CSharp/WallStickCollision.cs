using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200068F RID: 1679
public class WallStickCollision : MonoBehaviour
{
	// Token: 0x1700138D RID: 5005
	// (get) Token: 0x0600334E RID: 13134 RVA: 0x0001C1FC File Offset: 0x0001A3FC
	// (set) Token: 0x0600334F RID: 13135 RVA: 0x0001C204 File Offset: 0x0001A404
	public StickToWallCollisionDelegate OnStickEvent { get; set; }

	// Token: 0x1700138E RID: 5006
	// (get) Token: 0x06003350 RID: 13136 RVA: 0x0001C20D File Offset: 0x0001A40D
	public bool StickingToWall
	{
		get
		{
			return this.m_controller.Velocity == Vector2.zero;
		}
	}

	// Token: 0x1700138F RID: 5007
	// (get) Token: 0x06003351 RID: 13137 RVA: 0x0001C224 File Offset: 0x0001A424
	// (set) Token: 0x06003352 RID: 13138 RVA: 0x0001C22C File Offset: 0x0001A42C
	public RaycastHit2D LastHitRaycast { get; set; }

	// Token: 0x06003353 RID: 13139 RVA: 0x000DAE8C File Offset: 0x000D908C
	private void Awake()
	{
		GameObject root = this.GetRoot(false);
		this.m_controller = root.GetComponent<BaseCharacterController>();
		base.GetComponent<PreventPlatformDrop>();
		this.m_stickToWall = new Action<CorgiController_RL>(this.StickToWall);
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x000DAEC8 File Offset: 0x000D90C8
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

	// Token: 0x06003355 RID: 13141 RVA: 0x0001C235 File Offset: 0x0001A435
	private IEnumerator EnableCollisionCoroutine()
	{
		yield return new WaitUntil(() => this.m_controller.IsInitialized);
		this.m_stickListenerAdded = true;
		this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.AddListener(this.m_stickToWall, false);
		yield break;
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x0001C244 File Offset: 0x0001A444
	private void OnDisable()
	{
		if (this.m_stickListenerAdded)
		{
			this.m_stickListenerAdded = false;
			this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.RemoveListener(this.m_stickToWall);
		}
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x000DAF1C File Offset: 0x000D911C
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

	// Token: 0x06003358 RID: 13144 RVA: 0x0001C271 File Offset: 0x0001A471
	private void LateUpdate()
	{
		this.ConstrainEnemyMovementToRoom();
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x000DAFDC File Offset: 0x000D91DC
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

	// Token: 0x040029DA RID: 10714
	private BaseCharacterController m_controller;

	// Token: 0x040029DB RID: 10715
	private bool m_stickListenerAdded;

	// Token: 0x040029DC RID: 10716
	private Action<CorgiController_RL> m_stickToWall;
}

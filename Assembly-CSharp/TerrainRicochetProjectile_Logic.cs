using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020007BC RID: 1980
public class TerrainRicochetProjectile_Logic : BaseProjectileLogic
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06003C26 RID: 15398 RVA: 0x000F5E90 File Offset: 0x000F4090
	// (remove) Token: 0x06003C27 RID: 15399 RVA: 0x000F5EC8 File Offset: 0x000F40C8
	public event EventHandler OnRicochet;

	// Token: 0x17001602 RID: 5634
	// (get) Token: 0x06003C28 RID: 15400 RVA: 0x00021385 File Offset: 0x0001F585
	public bool JustBounced
	{
		get
		{
			return Time.time < this.m_bounceTimer;
		}
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x000F5F00 File Offset: 0x000F4100
	protected override void Awake()
	{
		base.Awake();
		GameObject root = this.GetRoot(false);
		this.m_controller = root.GetComponent<CorgiController_RL>();
		this.m_hbController = root.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x00021394 File Offset: 0x0001F594
	private IEnumerator Start()
	{
		while (!this.m_controller.IsInitialized)
		{
			yield return null;
		}
		this.m_controller.OnCorgiCollisionRelay.AddListener(new Action<CorgiController_RL>(this.Bounce), false);
		this.m_controller.RetainVelocity = true;
		yield break;
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x000213A3 File Offset: 0x0001F5A3
	private void OnDestroy()
	{
		this.m_controller.OnCorgiCollisionRelay.RemoveListener(new Action<CorgiController_RL>(this.Bounce));
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x000F5F34 File Offset: 0x000F4134
	private void Bounce(CorgiController_RL corgiController)
	{
		if (this.JustBounced)
		{
			return;
		}
		if (this.m_bounceUnityEvent != null)
		{
			this.m_bounceUnityEvent.Invoke();
		}
		this.m_bounceTimer = 0.05f + Time.time;
		Vector2 velocity = base.SourceProjectile.Velocity;
		float magnitude = velocity.magnitude;
		foreach (RaycastHit2D raycastHit2D in corgiController.ContactList)
		{
			Vector2 vector = new Vector2(raycastHit2D.normal.y, raycastHit2D.normal.x * -1f);
			Vector2 heading = 2f * (Vector2.Dot(velocity, vector) / Vector2.Dot(vector, vector)) * vector - velocity;
			base.SourceProjectile.Heading = heading;
			base.SourceProjectile.SetCorgiVelocity(base.SourceProjectile.Heading * magnitude);
		}
		if (this.OnRicochet != null)
		{
			this.OnRicochet(this, null);
		}
	}

	// Token: 0x06003C2D RID: 15405 RVA: 0x000213C2 File Offset: 0x0001F5C2
	private void LateUpdate()
	{
		this.ConstrainEnemyMovementToRoom();
	}

	// Token: 0x06003C2E RID: 15406 RVA: 0x000F6050 File Offset: 0x000F4250
	private void ConstrainEnemyMovementToRoom()
	{
		Bounds bounds = this.m_hbController.GetCollider(HitboxType.Terrain).bounds;
		Room room = PlayerManager.GetCurrentPlayerRoom() as Room;
		if (room)
		{
			Rect boundsRect = room.BoundsRect;
			float num = bounds.center.x - bounds.extents.x;
			float num2 = bounds.center.x + bounds.extents.x;
			float num3 = bounds.center.y - bounds.extents.y;
			float num4 = bounds.center.y + bounds.extents.y;
			bool flag = false;
			if ((num < boundsRect.xMin && this.m_controller.Velocity.x < 0f) || (num2 > boundsRect.xMax && this.m_controller.Velocity.x > 0f))
			{
				this.m_controller.SetHorizontalForce(-this.m_controller.Velocity.x);
				flag = true;
			}
			if ((num3 < boundsRect.yMin && this.m_controller.Velocity.y < 0f) || (num4 > boundsRect.yMax && this.m_controller.Velocity.y > 0f))
			{
				this.m_controller.SetVerticalForce(-this.m_controller.Velocity.y);
				flag = true;
			}
			if (flag)
			{
				base.SourceProjectile.Heading = base.SourceProjectile.Velocity;
				if (this.OnRicochet != null)
				{
					this.OnRicochet(this, null);
				}
				if (this.m_bounceUnityEvent != null)
				{
					this.m_bounceUnityEvent.Invoke();
				}
			}
		}
	}

	// Token: 0x04002FBB RID: 12219
	[SerializeField]
	private UnityEvent m_bounceUnityEvent;

	// Token: 0x04002FBC RID: 12220
	private const float BOUNCE_CHECK_TIMER = 0.05f;

	// Token: 0x04002FBD RID: 12221
	private CorgiController_RL m_controller;

	// Token: 0x04002FBE RID: 12222
	private IHitboxController m_hbController;

	// Token: 0x04002FBF RID: 12223
	private float m_bounceTimer;
}

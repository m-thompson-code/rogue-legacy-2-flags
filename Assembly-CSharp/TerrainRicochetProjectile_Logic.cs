using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004AD RID: 1197
public class TerrainRicochetProjectile_Logic : BaseProjectileLogic
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06002BA7 RID: 11175 RVA: 0x00094750 File Offset: 0x00092950
	// (remove) Token: 0x06002BA8 RID: 11176 RVA: 0x00094788 File Offset: 0x00092988
	public event EventHandler OnRicochet;

	// Token: 0x170010A9 RID: 4265
	// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x000947BD File Offset: 0x000929BD
	public bool JustBounced
	{
		get
		{
			return Time.time < this.m_bounceTimer;
		}
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x000947CC File Offset: 0x000929CC
	protected override void Awake()
	{
		base.Awake();
		GameObject root = this.GetRoot(false);
		this.m_controller = root.GetComponent<CorgiController_RL>();
		this.m_hbController = root.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x000947FF File Offset: 0x000929FF
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

	// Token: 0x06002BAC RID: 11180 RVA: 0x0009480E File Offset: 0x00092A0E
	private void OnDestroy()
	{
		this.m_controller.OnCorgiCollisionRelay.RemoveListener(new Action<CorgiController_RL>(this.Bounce));
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x00094830 File Offset: 0x00092A30
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

	// Token: 0x06002BAE RID: 11182 RVA: 0x0009494C File Offset: 0x00092B4C
	private void LateUpdate()
	{
		this.ConstrainEnemyMovementToRoom();
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x00094954 File Offset: 0x00092B54
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

	// Token: 0x0400237A RID: 9082
	[SerializeField]
	private UnityEvent m_bounceUnityEvent;

	// Token: 0x0400237B RID: 9083
	private const float BOUNCE_CHECK_TIMER = 0.05f;

	// Token: 0x0400237C RID: 9084
	private CorgiController_RL m_controller;

	// Token: 0x0400237D RID: 9085
	private IHitboxController m_hbController;

	// Token: 0x0400237E RID: 9086
	private float m_bounceTimer;
}

using System;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200033C RID: 828
public class BounceCollision : MonoBehaviour
{
	// Token: 0x17000CC3 RID: 3267
	// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x0000DD40 File Offset: 0x0000BF40
	public IRelayLink<GameObject> OnBounceRelay
	{
		get
		{
			return this.m_onBounceRelay.link;
		}
	}

	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x0000DD4D File Offset: 0x0000BF4D
	public bool JustBounced
	{
		get
		{
			return Time.time < this.m_bounceTimer;
		}
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x00093010 File Offset: 0x00091210
	private void Awake()
	{
		GameObject root = this.GetRoot(false);
		this.m_controller = root.GetComponent<BaseCharacterController>();
		base.GetComponent<PreventPlatformDrop>();
		this.m_bounce = new Action<CorgiController_RL>(this.Bounce);
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x0009304C File Offset: 0x0009124C
	private void OnEnable()
	{
		if (!this.m_bounceListenerAdded && this.m_controller.IsInitialized)
		{
			this.m_bounceListenerAdded = true;
			this.m_controller.ControllerCorgi.RetainVelocity = true;
			this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.AddListener(this.m_bounce, false);
		}
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x0000DD5C File Offset: 0x0000BF5C
	private void OnDisable()
	{
		if (this.m_bounceListenerAdded)
		{
			this.m_bounceListenerAdded = false;
			this.m_controller.ControllerCorgi.RetainVelocity = false;
			this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.RemoveListener(this.m_bounce);
		}
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x000930A4 File Offset: 0x000912A4
	private void Bounce(CorgiController_RL corgiController)
	{
		if (this.JustBounced)
		{
			return;
		}
		this.m_onBounceRelay.Dispatch(base.gameObject);
		if (this.BounceUnityEvent != null)
		{
			this.BounceUnityEvent.Invoke();
		}
		this.m_bounceTimer = 0.05f + Time.time;
		if (this.m_bounceTowardsPlayer)
		{
			this.BounceTowardsPlayer();
			return;
		}
		Vector2 velocity = this.m_controller.Velocity;
		float magnitude = velocity.magnitude;
		velocity.Normalize();
		foreach (RaycastHit2D raycastHit2D in corgiController.ContactList)
		{
			Vector2 vector = new Vector2(raycastHit2D.normal.y, raycastHit2D.normal.x * -1f);
			Vector2 vector2 = 2f * (Vector2.Dot(velocity, vector) / Vector2.Dot(vector, vector)) * vector - velocity;
			vector2.Normalize();
			this.m_controller.SetVelocity(vector2.x * magnitude, vector2.y * magnitude, false);
		}
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x000931CC File Offset: 0x000913CC
	private void BounceTowardsPlayer()
	{
		Vector2 vector = CDGHelper.VectorBetweenPts(this.m_controller.Midpoint, PlayerManager.GetPlayerController().Midpoint);
		vector.Normalize();
		this.m_controller.SetVelocity(vector.x * this.m_controller.Velocity.magnitude, vector.y * this.m_controller.Velocity.magnitude, false);
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x00093248 File Offset: 0x00091448
	private void LateUpdate()
	{
		if (!this.m_bounceListenerAdded && this.m_controller.IsInitialized)
		{
			this.m_bounceListenerAdded = true;
			this.m_controller.ControllerCorgi.RetainVelocity = true;
			this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.AddListener(this.m_bounce, false);
		}
		this.ConstrainEnemyMovementToRoom();
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000932A8 File Offset: 0x000914A8
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
			if ((collisionBounds.xMin < boundsRect.xMin && this.m_controller.Velocity.x < 0f) || (collisionBounds.xMax > boundsRect.xMax && this.m_controller.Velocity.x > 0f))
			{
				this.m_onBounceRelay.Dispatch(base.gameObject);
				if (this.BounceUnityEvent != null)
				{
					this.BounceUnityEvent.Invoke();
				}
				if (this.m_bounceTowardsPlayer)
				{
					this.BounceTowardsPlayer();
				}
				else
				{
					this.m_controller.SetVelocityX(-this.m_controller.Velocity.x, false);
				}
			}
			if ((collisionBounds.yMin < boundsRect.yMin && this.m_controller.Velocity.y < 0f) || (collisionBounds.yMax > boundsRect.yMax && this.m_controller.Velocity.y > 0f))
			{
				this.m_onBounceRelay.Dispatch(base.gameObject);
				if (this.BounceUnityEvent != null)
				{
					this.BounceUnityEvent.Invoke();
				}
				if (this.m_bounceTowardsPlayer)
				{
					this.BounceTowardsPlayer();
					return;
				}
				this.m_controller.SetVelocityY(-this.m_controller.Velocity.y, false);
			}
		}
	}

	// Token: 0x040018EC RID: 6380
	[SerializeField]
	private bool m_bounceTowardsPlayer;

	// Token: 0x040018ED RID: 6381
	[SerializeField]
	private UnityEvent BounceUnityEvent;

	// Token: 0x040018EE RID: 6382
	private const float BOUNCE_CHECK_TIMER = 0.05f;

	// Token: 0x040018EF RID: 6383
	private BaseCharacterController m_controller;

	// Token: 0x040018F0 RID: 6384
	private float m_bounceTimer;

	// Token: 0x040018F1 RID: 6385
	private bool m_bounceListenerAdded;

	// Token: 0x040018F2 RID: 6386
	private Action<CorgiController_RL> m_bounce;

	// Token: 0x040018F3 RID: 6387
	private Relay<GameObject> m_onBounceRelay = new Relay<GameObject>();
}

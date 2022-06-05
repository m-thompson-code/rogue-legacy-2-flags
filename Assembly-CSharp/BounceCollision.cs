using System;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001C5 RID: 453
public class BounceCollision : MonoBehaviour
{
	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x0600124E RID: 4686 RVA: 0x000356B0 File Offset: 0x000338B0
	public IRelayLink<GameObject> OnBounceRelay
	{
		get
		{
			return this.m_onBounceRelay.link;
		}
	}

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x0600124F RID: 4687 RVA: 0x000356BD File Offset: 0x000338BD
	public bool JustBounced
	{
		get
		{
			return Time.time < this.m_bounceTimer;
		}
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x000356CC File Offset: 0x000338CC
	private void Awake()
	{
		GameObject root = this.GetRoot(false);
		this.m_controller = root.GetComponent<BaseCharacterController>();
		base.GetComponent<PreventPlatformDrop>();
		this.m_bounce = new Action<CorgiController_RL>(this.Bounce);
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x00035708 File Offset: 0x00033908
	private void OnEnable()
	{
		if (!this.m_bounceListenerAdded && this.m_controller.IsInitialized)
		{
			this.m_bounceListenerAdded = true;
			this.m_controller.ControllerCorgi.RetainVelocity = true;
			this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.AddListener(this.m_bounce, false);
		}
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x0003575F File Offset: 0x0003395F
	private void OnDisable()
	{
		if (this.m_bounceListenerAdded)
		{
			this.m_bounceListenerAdded = false;
			this.m_controller.ControllerCorgi.RetainVelocity = false;
			this.m_controller.ControllerCorgi.OnCorgiCollisionRelay.RemoveListener(this.m_bounce);
		}
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x000357A0 File Offset: 0x000339A0
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

	// Token: 0x06001254 RID: 4692 RVA: 0x000358C8 File Offset: 0x00033AC8
	private void BounceTowardsPlayer()
	{
		Vector2 vector = CDGHelper.VectorBetweenPts(this.m_controller.Midpoint, PlayerManager.GetPlayerController().Midpoint);
		vector.Normalize();
		this.m_controller.SetVelocity(vector.x * this.m_controller.Velocity.magnitude, vector.y * this.m_controller.Velocity.magnitude, false);
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x00035944 File Offset: 0x00033B44
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

	// Token: 0x06001256 RID: 4694 RVA: 0x000359A4 File Offset: 0x00033BA4
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

	// Token: 0x040012C2 RID: 4802
	[SerializeField]
	private bool m_bounceTowardsPlayer;

	// Token: 0x040012C3 RID: 4803
	[SerializeField]
	private UnityEvent BounceUnityEvent;

	// Token: 0x040012C4 RID: 4804
	private const float BOUNCE_CHECK_TIMER = 0.05f;

	// Token: 0x040012C5 RID: 4805
	private BaseCharacterController m_controller;

	// Token: 0x040012C6 RID: 4806
	private float m_bounceTimer;

	// Token: 0x040012C7 RID: 4807
	private bool m_bounceListenerAdded;

	// Token: 0x040012C8 RID: 4808
	private Action<CorgiController_RL> m_bounce;

	// Token: 0x040012C9 RID: 4809
	private Relay<GameObject> m_onBounceRelay = new Relay<GameObject>();
}

using System;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200004B RID: 75
[AddComponentMenu("Corgi Engine/Character/Core/Corgi Controller RL")]
public class CorgiController_RL : CorgiController, IRootObj
{
	// Token: 0x1700001D RID: 29
	// (get) Token: 0x0600010F RID: 271 RVA: 0x000034F3 File Offset: 0x000016F3
	public IRelayLink<object, GameObject> StandingOnChangeRelay
	{
		get
		{
			return this.m_standingOnChangeRelay.link;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000110 RID: 272 RVA: 0x00003500 File Offset: 0x00001700
	public IRelayLink<CorgiController_RL> OnCorgiCollisionRelay
	{
		get
		{
			return this.m_onCorgiCollisionRelay;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000111 RID: 273 RVA: 0x00003508 File Offset: 0x00001708
	public IRelayLink<CorgiController_RL> OnCorgiLandedEnterRelay
	{
		get
		{
			return this.m_onCorgiLandedEnterRelay;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000112 RID: 274 RVA: 0x00003510 File Offset: 0x00001710
	public IRelayLink<CorgiController_RL> OnCorgiLandedExitRelay
	{
		get
		{
			return this.m_onCorgiLandedExitRelay;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000113 RID: 275 RVA: 0x00003518 File Offset: 0x00001718
	public IRelayLink<CorgiController_RL> CorgiStartFallingRelay
	{
		get
		{
			return this.m_corgiStartFallingRelay;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000114 RID: 276 RVA: 0x00003520 File Offset: 0x00001720
	// (set) Token: 0x06000115 RID: 277 RVA: 0x00003528 File Offset: 0x00001728
	public bool PermanentlyDisableUponTouchingPlatform { get; set; }

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000116 RID: 278 RVA: 0x00003531 File Offset: 0x00001731
	// (set) Token: 0x06000117 RID: 279 RVA: 0x00003539 File Offset: 0x00001739
	public bool PermanentlyDisabled { get; private set; }

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000118 RID: 280 RVA: 0x00003542 File Offset: 0x00001742
	// (set) Token: 0x06000119 RID: 281 RVA: 0x0000354A File Offset: 0x0000174A
	public bool DisableCastRaysAbove { get; set; }

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x0600011A RID: 282 RVA: 0x00003553 File Offset: 0x00001753
	// (set) Token: 0x0600011B RID: 283 RVA: 0x0000355B File Offset: 0x0000175B
	public bool DisableYSlopeOffsetCheck { get; set; }

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x0600011C RID: 284 RVA: 0x00003564 File Offset: 0x00001764
	protected bool IsFlying
	{
		get
		{
			return this.m_character && this.m_character.MovementState.CurrentState == CharacterStates.MovementStates.Flying;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600011D RID: 285 RVA: 0x00003589 File Offset: 0x00001789
	public List<Collider2D> StandingOnColliderList
	{
		get
		{
			return this.m_standingOnColliderList;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600011E RID: 286 RVA: 0x00003591 File Offset: 0x00001791
	public List<Collider2D> PreviousStandingOnColliderList
	{
		get
		{
			return this.m_prevStandingOnColliderList;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x0600011F RID: 287 RVA: 0x00003599 File Offset: 0x00001799
	// (set) Token: 0x06000120 RID: 288 RVA: 0x000035A1 File Offset: 0x000017A1
	public Vector3 LastStandingPosition { get; private set; }

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000121 RID: 289 RVA: 0x000035AA File Offset: 0x000017AA
	// (set) Token: 0x06000122 RID: 290 RVA: 0x000035B2 File Offset: 0x000017B2
	public float RayLengthAdd { get; set; }

	// Token: 0x06000123 RID: 291 RVA: 0x000035BB File Offset: 0x000017BB
	public void SetLastStandingPosition(Vector3 pos)
	{
		this.LastStandingPosition = pos;
		this.m_lastStandingTimeCheck = Time.time + 0.5f;
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000124 RID: 292 RVA: 0x000035D5 File Offset: 0x000017D5
	// (set) Token: 0x06000125 RID: 293 RVA: 0x000454DC File Offset: 0x000436DC
	public bool DisableOneWayCollision
	{
		get
		{
			return this.m_disableOnewayCollision;
		}
		set
		{
			this.m_disableOnewayCollision = value;
			if (!value)
			{
				this.PlatformMask |= this.OneWayPlatformMask;
				return;
			}
			this.PlatformMask &= ~this.OneWayPlatformMask;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000126 RID: 294 RVA: 0x000035DD File Offset: 0x000017DD
	// (set) Token: 0x06000127 RID: 295 RVA: 0x000035E5 File Offset: 0x000017E5
	public bool DisablePlatformCollision
	{
		get
		{
			return this.m_disablePlatformCollision;
		}
		set
		{
			this.m_disablePlatformCollision = value;
			if (value)
			{
				this.CollisionsOff();
				return;
			}
			this.CollisionsOn();
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000128 RID: 296 RVA: 0x000035FE File Offset: 0x000017FE
	public List<RaycastHit2D> ContactList
	{
		get
		{
			return this._contactList;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000129 RID: 297 RVA: 0x00003606 File Offset: 0x00001806
	public Rect AbsBounds
	{
		get
		{
			return this.m_absBounds;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600012A RID: 298 RVA: 0x0000360E File Offset: 0x0000180E
	public float BoundsWidth
	{
		get
		{
			return this._boundsWidth;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600012B RID: 299 RVA: 0x00003616 File Offset: 0x00001816
	public float BoundsHeight
	{
		get
		{
			return this._boundsHeight;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x0600012C RID: 300 RVA: 0x0000361E File Offset: 0x0000181E
	public BoxCollider2D BoxCollider
	{
		get
		{
			return this._boxCollider;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x0600012D RID: 301 RVA: 0x00003626 File Offset: 0x00001826
	public bool IsGravityActive
	{
		get
		{
			return this._gravityActive;
		}
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0004553C File Offset: 0x0004373C
	protected override void Awake()
	{
		base.Awake();
		this._transform = base.transform;
		this.m_gameObject = base.gameObject;
		this._contactList = new List<RaycastHit2D>();
		base.State = new CorgiControllerState();
		if (!this.DisableOneWayCollision)
		{
			this.PlatformMask |= this.OneWayPlatformMask;
		}
		this.m_standingOnColliderList = new List<Collider2D>();
		this.m_prevStandingOnColliderList = new List<Collider2D>();
		base.CurrentWallCollider = null;
		base.State.Reset();
		this.m_character = base.GetComponent<Character>();
		this.m_characterExists = this.m_character;
		this.DefaultParameters.Gravity = -50f;
		this.DefaultParameters.AscentMultiplier = 1f;
		this.DefaultParameters.FallMultiplier = 1f;
		this.DefaultParameters.MaximumSlopeAngle = 46f;
		if (this.UseGlobalEVRaycastSettings)
		{
			this.StickWhenWalkingDownSlopes = true;
			this.StickyRaycastLength = 2f;
		}
		this.m_isPlayer = base.CompareTag("Player");
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00045658 File Offset: 0x00043858
	public void InitializeRays()
	{
		if (this.UseGlobalEVRaycastSettings)
		{
			float x = this._boxCollider.bounds.size.x;
			float y = this._boxCollider.bounds.size.y;
			float num = 0.5f;
			if (base.CompareTag("Enemy"))
			{
				num *= 2f;
			}
			int num2 = Mathf.CeilToInt(y / num);
			num2 = Mathf.Clamp(num2, 2, 6);
			int num3 = Mathf.CeilToInt(x / 0.5f);
			num3 = Mathf.Clamp(num3, 2, 6);
			this.NumberOfHorizontalRays = num2;
			this.NumberOfVerticalRays = num3;
		}
		this._sideHitsStorage = new RaycastHit2D[this.NumberOfHorizontalRays];
		this._belowHitsStorage = new RaycastHit2D[this.NumberOfVerticalRays];
		this._aboveHitsStorage = new RaycastHit2D[this.NumberOfVerticalRays];
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00045728 File Offset: 0x00043928
	protected override void Initialization()
	{
		if (this.m_gameObject.GetComponent<DecoController>())
		{
			CorgiController_RL.m_hitboxControllerHelper_STATIC.Clear();
			this.m_gameObject.GetComponentsInChildren<IHitboxController>(CorgiController_RL.m_hitboxControllerHelper_STATIC);
			using (List<IHitboxController>.Enumerator enumerator = CorgiController_RL.m_hitboxControllerHelper_STATIC.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IHitboxController hitboxController = enumerator.Current;
					if (hitboxController.RootGameObject == this.m_gameObject)
					{
						this._boxCollider = (BoxCollider2D)hitboxController.GetCollider(HitboxType.Platform);
						break;
					}
				}
				goto IL_A2;
			}
		}
		IHitboxController componentInChildren = this.m_gameObject.GetComponentInChildren<IHitboxController>();
		if (componentInChildren != null)
		{
			this._boxCollider = (BoxCollider2D)componentInChildren.GetCollider(HitboxType.Platform);
		}
		IL_A2:
		if (!this._boxCollider)
		{
			throw new Exception("Cannot Initialize Object: " + base.name + ". The Corgi Controller MUST have a BoxCollider2D attached to it.");
		}
		this._originalColliderSize = this._boxCollider.size;
		this._originalColliderOffset = this._boxCollider.offset;
		this.SetRaysParameters();
		this.m_boxColliderLayer = this._boxCollider.gameObject.layer;
		this.InitializeRays();
		base.IsInitialized = true;
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0004585C File Offset: 0x00043A5C
	public override void CollisionsOn()
	{
		base.SavePlatformMask();
		this.PlatformMask = this._platformMaskSave;
		if (!this.DisableOneWayCollision)
		{
			this.PlatformMask |= this.OneWayPlatformMask;
		}
		this.PlatformMask |= this.MovingPlatformMask;
		this.PlatformMask |= this.MovingOneWayPlatformMask;
		this.PlatformMask |= this.MidHeightOneWayPlatformMask;
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000362E File Offset: 0x0000182E
	public override void CollisionsOff()
	{
		base.SavePlatformMask();
		this.PlatformMask = 0;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0004590C File Offset: 0x00043B0C
	public override void SetRaysParameters()
	{
		base.SetRaysParameters();
		this.m_absBounds.x = this._boundsBottomLeftCorner.x;
		this.m_absBounds.y = this._boundsBottomLeftCorner.y;
		this.m_absBounds.width = this._boundsWidth;
		this.m_absBounds.height = this._boundsHeight;
		this._boundsCenter = this.m_absBounds.center;
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00045980 File Offset: 0x00043B80
	protected override void FrameInitialization()
	{
		this._boxCollider.gameObject.layer = CorgiController_RL.m_ignoreRaycastLayer_STATIC;
		this.m_lastHeightStandingOn = this.m_heightStandingOn;
		this.m_lastAngleStandingOn = this.m_angleStandingOn;
		base.FrameInitialization();
		this.StandingOn = null;
		base.StandingOnCollider = null;
		this.m_heightStandingOn = float.MaxValue;
		this.m_angleStandingOn = 0f;
		base.IsWithinJumpLeeway = false;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00003642 File Offset: 0x00001842
	protected override void FrameExit()
	{
		this._boxCollider.gameObject.layer = this.m_boxColliderLayer;
		if (this._contactList.Count > 0)
		{
			this.m_onCorgiCollisionRelay.Dispatch(this);
		}
		base.FrameExit();
	}

	// Token: 0x06000136 RID: 310 RVA: 0x000459EC File Offset: 0x00043BEC
	protected override void EveryFrame()
	{
		if (Time.deltaTime == 0f)
		{
			return;
		}
		if (this.PermanentlyDisabled)
		{
			base.enabled = false;
			return;
		}
		bool isCollidingBelow = base.State.IsCollidingBelow;
		Vector2 velocity = this._velocity;
		this.ApplyGravity();
		this.FrameInitialization();
		this.m_slopeYOffset = 0f;
		this.SetRaysParameters();
		if (this.PlatformMask == 0)
		{
			this._transform.Translate(this._newPosition, Space.Self);
			base.State.IsCollidingBelow = false;
			return;
		}
		base.ForcesApplied = this._velocity;
		if (this.NumberOfHorizontalRays > 0)
		{
			this.m_hittingSlopedWall = false;
			if (!this.IsFlying)
			{
				this.RunSlopedWallCheck();
			}
			this.DetermineMovementDirection();
			if (this.CastRaysOnBothSides || !base.State.WasGroundedLastFrame)
			{
				if (this._newPosition.x < 0f)
				{
					this.CastRaysToTheLeft();
					this.CastRaysToTheRight();
				}
				else
				{
					this.CastRaysToTheRight();
					this.CastRaysToTheLeft();
				}
			}
			else if (this._movementDirection == -1f)
			{
				this.CastRaysToTheLeft();
			}
			else
			{
				this.CastRaysToTheRight();
			}
		}
		CorgiController_RL.m_platformRevertDict_STATIC.Clear();
		this.CastRaysBelow();
		foreach (KeyValuePair<GameObject, int> keyValuePair in CorgiController_RL.m_platformRevertDict_STATIC)
		{
			keyValuePair.Key.layer = keyValuePair.Value;
		}
		if (!this.DisableCastRaysAbove)
		{
			this.CastRaysAbove();
		}
		this._transform.Translate(this._newPosition, Space.Self);
		this._newPosition.y = this._newPosition.y - this.m_slopeYOffset;
		this.SetRaysParameters();
		this.ComputeNewSpeed();
		this.SetStates();
		this._externalForce.x = 0f;
		this._externalForce.y = 0f;
		if (this.RetainVelocity)
		{
			this._velocity = velocity;
		}
		this.FrameExit();
		if (isCollidingBelow != base.State.IsCollidingBelow)
		{
			if (base.State.IsCollidingBelow)
			{
				this.m_onCorgiLandedEnterRelay.Dispatch(this);
			}
			else
			{
				this.m_onCorgiLandedExitRelay.Dispatch(this);
			}
		}
		if (this.m_isPlayer)
		{
			this.UpdateLastSafeStandingPosition();
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00045C2C File Offset: 0x00043E2C
	private void UpdateLastSafeStandingPosition()
	{
		if (Time.time > this.m_lastStandingTimeCheck)
		{
			this.m_lastStandingTimeCheck = Time.time + 0.5f;
			if (base.State.IsGrounded && !Cloud.HittingCloud && this.StandingOn && !this.StandingOn.CompareTag("PlayerProjectile") && !this.StandingOn.CompareTag("TriggerHazard") && !this.StandingOn.CompareTag("MagicPlatform") && !this.StandingOn.CompareTag("Enemy") && (this.StandingOn.layer == 8 || this.StandingOn.layer == 9 || this.StandingOn.layer == 11))
			{
				this.LastStandingPosition = this._transform.position;
			}
		}
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000367A File Offset: 0x0000187A
	public void ResetPermanentDisable()
	{
		this.PermanentlyDisabled = false;
		base.enabled = true;
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000368A File Offset: 0x0000188A
	protected override void ApplyGravity()
	{
		base.ApplyGravity();
		if (this._velocity.y < Global_EV.TerminalVelocity)
		{
			this._velocity.y = Global_EV.TerminalVelocity;
		}
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00045D0C File Offset: 0x00043F0C
	protected void RunSlopedWallCheck()
	{
		Vector2 vector = this._transform.up;
		Vector2 vector2 = this._transform.right;
		float distance = Mathf.Abs(this._velocity.x * Time.deltaTime) + this._boundsWidth / 2f + this.HorizontalRayOffset * 2f;
		this._horizontalRayCastFromBottom = (this._boundsBottomRightCorner + this._boundsBottomLeftCorner) / 2f;
		if (this._newPosition.y < 0f && !base.State.IsGrounded)
		{
			Vector2 vector3 = this._horizontalRayCastFromBottom;
			vector3 += vector * this._newPosition.y;
			vector3 += vector2 * this._newPosition.x;
			RaycastHit2D hit = Physics2D.Raycast(vector3, vector2, distance, this.PlatformMask);
			if (hit && hit.distance == 0f)
			{
				Vector2 vector4 = vector3 - this._horizontalRayCastFromBottom;
				float distance2 = Mathf.Abs(vector4.magnitude);
				vector4.Normalize();
				RaycastHit2D item = Physics2D.Raycast(this._horizontalRayCastFromBottom, vector4, distance2, this.PlatformMask);
				float num = Mathf.Abs(Vector2.Angle(item.normal, vector));
				float num2 = num;
				if (Vector3.Cross(vector, item.normal).z < 0f)
				{
					num2 = -num;
				}
				if (num > base.Parameters.MaximumSlopeAngle && num < 90f && item.distance > 0f)
				{
					base.State.LateralSlopeAngle = num2;
					base.CurrentWallCollider = item.collider.gameObject;
					base.State.SlopeAngleOK = false;
					float num3 = this._boundsWidth / 2f + this.HorizontalRayOffset * 2f;
					float num4 = CDGHelper.VectorToAngle(new Vector2(vector4.x, -vector4.y));
					if (num4 > 90f)
					{
						num4 = 180f - num4;
					}
					float num5 = item.point.y - vector3.y;
					float num6 = Mathf.Tan(num * 0.017453292f);
					float num7 = Mathf.Tan(num4 * 0.017453292f);
					float num8 = num5 / num6;
					float num9 = num5 / num7;
					float num10 = num8 + num9;
					num10 += num3;
					if (num2 < 0f)
					{
						this._newPosition.x = num10;
						base.State.IsCollidingLeft = true;
						base.State.DistanceToLeftCollider = 0f;
					}
					else
					{
						this._newPosition.x = -num10;
						base.State.IsCollidingRight = true;
						base.State.DistanceToRightCollider = 0f;
					}
					if (!base.State.IsGrounded && base.Velocity.y > 0f)
					{
						this._newPosition.x = 0f;
					}
					this._contactList.Add(item);
					this._velocity.x = 0f;
					this.m_hittingSlopedWall = true;
				}
			}
		}
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00046038 File Offset: 0x00044238
	protected override void CastRaysToTheSides(float raysDirection)
	{
		Vector2 vector = this._transform.up;
		Vector2 a = this._transform.right;
		this._horizontalRayCastFromBottom = (this._boundsBottomRightCorner + this._boundsBottomLeftCorner) / 2f;
		this._horizontalRayCastToTop = (this._boundsTopLeftCorner + this._boundsTopRightCorner) / 2f;
		this._horizontalRayCastFromBottom += vector * 0.05f;
		this._horizontalRayCastToTop -= vector * 0.05f;
		if (this.m_hittingSlopedWall)
		{
			this._horizontalRayCastFromBottom += vector * this._newPosition.y;
			this._horizontalRayCastToTop += vector * this._newPosition.y;
		}
		float num = Mathf.Abs(this._velocity.x * Time.deltaTime) + this._boundsWidth / 2f + this.HorizontalRayOffset * 2f;
		float num2 = float.MaxValue;
		if (this._sideHitsStorage.Length != this.NumberOfHorizontalRays)
		{
			this._sideHitsStorage = new RaycastHit2D[this.NumberOfHorizontalRays];
		}
		for (int i = 0; i < this.NumberOfHorizontalRays; i++)
		{
			Vector2 vector2 = Vector2.zero;
			if (this.NumberOfHorizontalRays > 1)
			{
				vector2 = Vector2.Lerp(this._horizontalRayCastFromBottom, this._horizontalRayCastToTop, (float)i / (float)(this.NumberOfHorizontalRays - 1));
			}
			else
			{
				vector2 = Vector2.Lerp(this._horizontalRayCastFromBottom, this._horizontalRayCastToTop, 0.5f);
			}
			this._sideHitsStorage[i] = Physics2D.Raycast(vector2, raysDirection * a, num, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask);
			if (this._sideHitsStorage[i] && this._sideHitsStorage[i].distance < num2 && !(this._sideHitsStorage[i].collider == this._ignoredCollider))
			{
				float num3 = Mathf.Abs(Vector2.Angle(this._sideHitsStorage[i].normal, vector));
				if (this._sideHitsStorage[i].distance > 0f || !base.State.IsGrounded)
				{
					if (this._movementDirection == raysDirection || this._velocity.x == 0f)
					{
						base.State.LateralSlopeAngle = num3;
					}
					if (num3 > base.Parameters.MaximumSlopeAngle)
					{
						float num4 = this._boundsWidth / 2f + this.HorizontalRayOffset * 2f;
						if (raysDirection < 0f)
						{
							base.State.IsCollidingLeft = true;
							base.State.DistanceToLeftCollider = this._sideHitsStorage[i].distance;
						}
						else
						{
							base.State.IsCollidingRight = true;
							base.State.DistanceToRightCollider = this._sideHitsStorage[i].distance;
						}
						float num5 = Mathf.Abs(CDGHelper.ToDegrees(Mathf.Atan2(this._velocity.y, Mathf.Abs(this._velocity.x))));
						if (this._sideHitsStorage[i].distance == 0f || this._movementDirection == raysDirection || this._velocity.x == 0f || (this._movementDirection != raysDirection && base.State.IsFalling && num5 >= num3))
						{
							base.CurrentWallCollider = this._sideHitsStorage[i].collider.gameObject;
							base.State.SlopeAngleOK = false;
							num2 = this._sideHitsStorage[i].distance;
							float num6 = MMMaths.DistanceBetweenPointAndLine(this._sideHitsStorage[i].point, this._horizontalRayCastFromBottom, this._horizontalRayCastToTop);
							if (this._sideHitsStorage[i].distance == 0f)
							{
								float num7 = Mathf.Clamp(this._boundsWidth / 2f, 0.5f, float.MaxValue);
								float distance = num + num7;
								Vector2 origin;
								if (raysDirection == -1f)
								{
									origin = vector2;
									origin.x += num7;
								}
								else
								{
									origin = vector2;
									origin.x -= num7;
								}
								this._sideHitsStorage[i] = Physics2D.Raycast(origin, raysDirection * a, distance, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask);
								if (!this._sideHitsStorage[i] || this._sideHitsStorage[i].distance == 0f)
								{
									goto IL_59E;
								}
								num6 = (num7 * 2f - this._sideHitsStorage[i].distance) * -1f;
								num4 = 0f;
							}
							if (raysDirection <= 0f)
							{
								this._newPosition.x = -num6 + num4;
							}
							else
							{
								this._newPosition.x = num6 - num4;
							}
							if (!base.State.IsGrounded && base.Velocity.y > 0f)
							{
								this._newPosition.x = 0f;
							}
							this._contactList.Add(this._sideHitsStorage[i]);
							this._velocity.x = 0f;
						}
					}
				}
			}
			IL_59E:;
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000036B4 File Offset: 0x000018B4
	public void ResetMovementState()
	{
		if (base.State != null)
		{
			base.State.Reset();
		}
		this._velocity = Vector2.zero;
		this.CastRaysBelow();
	}

	// Token: 0x0600013D RID: 317 RVA: 0x000465F8 File Offset: 0x000447F8
	protected override void CastRaysAbove()
	{
		if (this._newPosition.y < 0f)
		{
			return;
		}
		float num = base.State.IsGrounded ? this.HorizontalRayOffset : this._newPosition.y;
		num += this._boundsHeight / 2f;
		bool flag = false;
		Vector2 direction = this._transform.up;
		Vector2 a = this._transform.right;
		this._aboveRayCastStart = (this._boundsBottomLeftCorner + this._boundsTopLeftCorner) / 2f;
		this._aboveRayCastEnd = (this._boundsBottomRightCorner + this._boundsTopRightCorner) / 2f;
		this._aboveRayCastStart += a * this._newPosition.x;
		this._aboveRayCastEnd += a * this._newPosition.x;
		this._aboveRayCastStart.y = this._aboveRayCastStart.y + this.m_slopeYOffset;
		this._aboveRayCastEnd.y = this._aboveRayCastEnd.y + this.m_slopeYOffset;
		if (this._aboveHitsStorage.Length != this.NumberOfVerticalRays)
		{
			this._aboveHitsStorage = new RaycastHit2D[this.NumberOfVerticalRays];
		}
		float num2 = float.MaxValue;
		int num3 = 0;
		for (int i = 0; i < this.NumberOfVerticalRays; i++)
		{
			Vector2 origin = Vector2.zero;
			if (this.NumberOfVerticalRays > 1)
			{
				origin = Vector2.Lerp(this._aboveRayCastStart, this._aboveRayCastEnd, (float)i / (float)(this.NumberOfVerticalRays - 1));
			}
			else
			{
				origin = Vector2.Lerp(this._aboveRayCastStart, this._aboveRayCastEnd, 0.5f);
			}
			this._aboveHitsStorage[i] = Physics2D.Raycast(origin, direction, num, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask);
			if (this._aboveHitsStorage[i] && this._aboveHitsStorage[i].distance > 0f)
			{
				flag = true;
				if (this._aboveHitsStorage[i].collider == this._ignoredCollider)
				{
					break;
				}
				if (this._aboveHitsStorage[i].distance < num2)
				{
					num2 = this._aboveHitsStorage[i].distance;
					num3 = i;
				}
			}
		}
		if (flag)
		{
			if (!base.State.IsGrounded)
			{
				this._newPosition.y = num2 - this._boundsHeight / 2f;
			}
			if (base.State.IsGrounded && this._newPosition.y < 0f)
			{
				this._newPosition.y = 0f;
			}
			base.State.IsCollidingAbove = true;
			if (!base.State.WasTouchingTheCeilingLastFrame)
			{
				this._velocity = new Vector2(this._velocity.x, 0f);
			}
			this._contactList.Add(this._aboveHitsStorage[num3]);
			if (this.m_isPlayer && base.State.IsCollidingBelow && this._velocity.y > 0.15f && base.StandingOnCollider && base.StandingOnCollider.gameObject.layer == 11)
			{
				base.StartCoroutine(this.DisableCollisionsWithOneWayPlatforms(0.1f));
			}
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00046968 File Offset: 0x00044B68
	protected override void CastRaysBelow()
	{
		this._friction = 0f;
		if (this._newPosition.y < -0.0001f)
		{
			base.State.IsFalling = true;
		}
		else
		{
			base.State.IsFalling = false;
		}
		if (this.m_prevIsFalling != base.State.IsFalling)
		{
			if (base.State.IsFalling)
			{
				this.m_corgiStartFallingRelay.Dispatch(this);
			}
			this.m_prevIsFalling = base.State.IsFalling;
		}
		if (base.Parameters.Gravity > 0f && !base.State.IsFalling)
		{
			base.State.IsCollidingBelow = false;
			return;
		}
		float num = this._boundsHeight / 2f + this.VerticalRayOffset + this.m_yRaycastOffset + this.RayLengthAdd;
		if (this._newPosition.y < 0f || base.State.IsCollidingBelow)
		{
			num += Mathf.Abs(this._newPosition.y);
		}
		if (this.m_enablePrecisionPointChecks && (this.m_character.MovementState.CurrentState == CharacterStates.MovementStates.Dashing || !this.IsGravityActive || base.Parameters.FallMultiplier < 0.25f || Time.timeScale < 0.2f))
		{
			num += 0.05f;
		}
		float num2 = this.m_jumpLeewayAmount + this._boundsHeight / 2f;
		float num3 = 0f;
		if (num < num2)
		{
			num3 = num2 - num;
			num += num3;
		}
		Vector2 vector = this._transform.up;
		Vector2 a = this._transform.right;
		this._verticalRayCastFromLeft = (this._boundsBottomLeftCorner + this._boundsTopLeftCorner) / 2f;
		this._verticalRayCastToRight = (this._boundsBottomRightCorner + this._boundsTopRightCorner) / 2f;
		this._verticalRayCastFromLeft.y = this._verticalRayCastFromLeft.y + this.m_yRaycastOffset;
		this._verticalRayCastToRight.y = this._verticalRayCastToRight.y + this.m_yRaycastOffset;
		this._verticalRayCastFromLeft += vector * this.VerticalRayOffset;
		this._verticalRayCastToRight += vector * this.VerticalRayOffset;
		this._verticalRayCastFromLeft += a * this._newPosition.x;
		this._verticalRayCastToRight += a * this._newPosition.x;
		if (this._belowHitsStorage.Length != this.NumberOfVerticalRays)
		{
			this._belowHitsStorage = new RaycastHit2D[this.NumberOfVerticalRays];
		}
		this._raysBelowLayerMaskPlatforms = this.PlatformMask;
		this._raysBelowLayerMaskPlatformsWithoutOneWay = (this.PlatformMask & ~this.MidHeightOneWayPlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask);
		this._raysBelowLayerMaskPlatformsWithoutMidHeight = (this._raysBelowLayerMaskPlatforms & ~this.MidHeightOneWayPlatformMask);
		if (base.StandingOnLastFrame)
		{
			this._savedBelowLayer = base.StandingOnLastFrame.layer;
			if (this.MidHeightOneWayPlatformMask.Contains(base.StandingOnLastFrame.layer))
			{
				base.StandingOnLastFrame.layer = 8;
			}
		}
		if (base.State.WasGroundedLastFrame && base.StandingOnLastFrame && !this.MidHeightOneWayPlatformMask.Contains(base.StandingOnLastFrame.layer))
		{
			this._raysBelowLayerMaskPlatforms = this._raysBelowLayerMaskPlatformsWithoutMidHeight;
		}
		float num4 = float.MaxValue;
		int num5 = 0;
		bool flag = false;
		int num6 = -1;
		for (int i = 0; i < this.NumberOfVerticalRays; i++)
		{
			Vector2 vector2 = Vector2.zero;
			if (this.NumberOfVerticalRays > 1)
			{
				vector2 = Vector2.Lerp(this._verticalRayCastFromLeft, this._verticalRayCastToRight, (float)i / (float)(this.NumberOfVerticalRays - 1));
			}
			else
			{
				vector2 = Vector2.Lerp(this._verticalRayCastFromLeft, this._verticalRayCastToRight, 0.5f);
			}
			if ((this._newPosition.y > 0f && !base.State.WasGroundedLastFrame) || this.IsFlying)
			{
				this._belowHitsStorage[i] = Physics2D.Raycast(vector2, -vector, num, this._raysBelowLayerMaskPlatformsWithoutOneWay);
			}
			else
			{
				this._belowHitsStorage[i] = Physics2D.Raycast(vector2, -vector, num, this._raysBelowLayerMaskPlatforms);
			}
			if (this._belowHitsStorage[i])
			{
				GameObject gameObject = this._belowHitsStorage[i].collider.gameObject;
				if (!this.DisableOneWayCollision && this.OneWayPlatformMask.Contains(gameObject.layer))
				{
					if (MMMaths.DistanceBetweenPointAndLine(this._belowHitsStorage[i].point, this._verticalRayCastFromLeft, this._verticalRayCastToRight) < 0.0001f)
					{
						if (!CorgiController_RL.m_platformRevertDict_STATIC.ContainsKey(gameObject))
						{
							CorgiController_RL.m_platformRevertDict_STATIC.Add(gameObject, gameObject.layer);
							gameObject.layer = CorgiController_RL.m_ignoreRaycastLayer_STATIC;
							i--;
							goto IL_723;
						}
						goto IL_723;
					}
					else if ((!base.State.IsGrounded && base.StandingOnLastFrame == gameObject && this._belowHitsStorage[i].distance < this._boundsHeight / 2f + this.m_yRaycastOffset) || (base.State.IsGrounded && base.StandingOnLastFrame != gameObject && base.StandingOnLastFrame != null && Mathf.Abs(this.m_lastHeightStandingOn - this._belowHitsStorage[i].point.y) > 0.001f && this.m_lastAngleStandingOn == 0f))
					{
						if (!CorgiController_RL.m_platformRevertDict_STATIC.ContainsKey(gameObject))
						{
							CorgiController_RL.m_platformRevertDict_STATIC.Add(gameObject, gameObject.layer);
							gameObject.layer = CorgiController_RL.m_ignoreRaycastLayer_STATIC;
							i--;
							goto IL_723;
						}
						goto IL_723;
					}
				}
				if (Mathf.Abs(Vector2.Angle(this._belowHitsStorage[i].normal, vector)) <= base.Parameters.MaximumSlopeAngle && !(this._belowHitsStorage[i].collider == this._ignoredCollider) && this._belowHitsStorage[i].distance <= 0f && !this.DisableYSlopeOffsetCheck)
				{
					float num7 = Mathf.Clamp(this._boundsHeight / 2f, 0.5f, float.MaxValue);
					Vector2 origin = vector2;
					origin.y += num7;
					float distance = num + num7;
					this._belowHitsStorage[i] = Physics2D.Raycast(origin, -vector, distance, this._raysBelowLayerMaskPlatforms);
					if (Mathf.Abs(Vector2.Angle(this._belowHitsStorage[i].normal, vector)) <= base.Parameters.MaximumSlopeAngle && !(this._belowHitsStorage[i].collider == this._ignoredCollider))
					{
						this.m_slopeYOffset = num7;
					}
				}
			}
			IL_723:;
		}
		for (int j = 0; j < this.NumberOfVerticalRays; j++)
		{
			if (this._belowHitsStorage[j] && !CorgiController_RL.m_platformRevertDict_STATIC.ContainsKey(this._belowHitsStorage[j].collider.gameObject) && this._belowHitsStorage[j].distance != 0f && (Mathf.Abs(Vector2.Angle(this._belowHitsStorage[j].normal, vector)) <= base.Parameters.MaximumSlopeAngle || (base.State.WasGroundedLastFrame && (!base.State.WasGroundedLastFrame || base.State.IsCollidingLeft || base.State.IsCollidingRight))))
			{
				if (this._belowHitsStorage[j].distance > num - num3)
				{
					if (!this._belowHitsStorage[j].collider.CompareTag("MagicPlatform"))
					{
						base.IsWithinJumpLeeway = true;
					}
				}
				else
				{
					if (!this._belowHitsStorage[j].collider.CompareTag("MagicPlatform"))
					{
						base.IsWithinJumpLeeway = true;
					}
					flag = true;
					base.State.BelowSlopeAngle = Vector2.Angle(this._belowHitsStorage[j].normal, vector);
					this._crossBelowSlopeAngle = Vector3.Cross(vector, this._belowHitsStorage[j].normal);
					if (this._crossBelowSlopeAngle.z < 0f)
					{
						base.State.BelowSlopeAngle = -base.State.BelowSlopeAngle;
					}
					if (this._belowHitsStorage[j].distance < num4 || num6 == j)
					{
						num5 = j;
						num4 = this._belowHitsStorage[j].distance;
					}
				}
			}
		}
		if (flag)
		{
			this.StandingOn = this._belowHitsStorage[num5].collider.gameObject;
			if (this.PermanentlyDisableUponTouchingPlatform && this.StandingOn.CompareTag("Platform"))
			{
				this.PermanentlyDisabled = true;
				base.enabled = false;
			}
			if (this.StandingOn != this.m_previouslyStandingOn)
			{
				this.m_previouslyStandingOn = this.StandingOn;
				this.m_standingOnChangeRelay.Dispatch(this, this.StandingOn);
			}
			base.StandingOnCollider = this._belowHitsStorage[num5].collider;
			this.m_heightStandingOn = this._belowHitsStorage[num5].point.y;
			this.m_angleStandingOn = Mathf.Abs(Vector2.Angle(this._belowHitsStorage[num5].normal, vector));
			this._contactList.Add(this._belowHitsStorage[num5]);
			float num8 = this._boundsHeight / 2f + this.m_yRaycastOffset - Mathf.Abs(this._newPosition.y);
			bool flag2 = Mathf.Approximately(num4, num8);
			if (!base.State.WasGroundedLastFrame && !flag2 && num4 < num8 && (this.OneWayPlatformMask.Contains(this.StandingOn.layer) || this.MovingOneWayPlatformMask.Contains(this.StandingOn.layer)))
			{
				base.State.IsCollidingBelow = false;
				base.IsWithinJumpLeeway = false;
				return;
			}
			if (base.Velocity.y < 0.1f)
			{
				base.State.IsFalling = false;
				base.State.IsCollidingBelow = true;
			}
			if (this._externalForce.y > 0f && this._velocity.y > 0f)
			{
				this._newPosition.y = this._velocity.y * Time.deltaTime;
				base.State.IsCollidingBelow = false;
			}
			else
			{
				float num9 = MMMaths.DistanceBetweenPointAndLine(this._belowHitsStorage[num5].point, this._verticalRayCastFromLeft, this._verticalRayCastToRight);
				this._newPosition.y = -num9 + this._boundsHeight / 2f + this.VerticalRayOffset + this.m_yRaycastOffset + this.m_slopeYOffset;
			}
			if (!base.State.WasGroundedLastFrame && this._velocity.y > 0f)
			{
				this._newPosition.y = this._newPosition.y + this._velocity.y * Time.deltaTime;
			}
			if (Mathf.Abs(this._newPosition.y) < 0.0001f)
			{
				this._newPosition.y = 0f;
			}
			if (this.m_characterExists && this.m_character.MovementState.CurrentState == CharacterStates.MovementStates.Dashing && base.State.IsCollidingBelow && this._newPosition.y < 0f)
			{
				this._newPosition.y = 0f;
			}
		}
		else
		{
			base.State.IsCollidingBelow = false;
			if (base.State.OnAMovingPlatform)
			{
				this.DetachFromMovingPlatform();
			}
		}
		if (this.IsFlying)
		{
			base.State.IsCollidingBelow = false;
			base.IsWithinJumpLeeway = false;
		}
		if (this.StickWhenWalkingDownSlopes && !base.State.IsCollidingBelow && this.IsGravityActive && this.m_characterExists && this.m_character.MovementState.CurrentState != CharacterStates.MovementStates.Dashing)
		{
			this.StickToSlope();
		}
		if (!this.DisableStandingOnColliderLogic)
		{
			this.m_prevStandingOnColliderList.Clear();
			this.m_prevStandingOnColliderList.AddRange(this.m_standingOnColliderList);
			this.m_standingOnColliderList.Clear();
			if (base.State.IsCollidingBelow)
			{
				foreach (RaycastHit2D hit in this._belowHitsStorage)
				{
					if (hit && hit.distance == num4 && !this.m_standingOnColliderList.Contains(hit.collider))
					{
						this.m_standingOnColliderList.Add(hit.collider);
					}
				}
				foreach (Collider2D collider2D in this.m_standingOnColliderList)
				{
					if (collider2D && !this.m_prevStandingOnColliderList.Contains(collider2D))
					{
						CorgiController_RL.m_standingOnEnterHelperList.Clear();
						collider2D.GetRoot(false).GetComponentsInChildren<IStandingOnEnter>(CorgiController_RL.m_standingOnEnterHelperList);
						foreach (IStandingOnEnter standingOnEnter in CorgiController_RL.m_standingOnEnterHelperList)
						{
							standingOnEnter.OnStandingEnter(this.m_gameObject);
						}
					}
				}
			}
			foreach (Collider2D collider2D2 in this.m_prevStandingOnColliderList)
			{
				if (collider2D2 && !this.m_standingOnColliderList.Contains(collider2D2))
				{
					CorgiController_RL.m_standingOnExitHelperList.Clear();
					collider2D2.GetRoot(false).GetComponentsInChildren<IStandingOnExit>(CorgiController_RL.m_standingOnExitHelperList);
					foreach (IStandingOnExit standingOnExit in CorgiController_RL.m_standingOnExitHelperList)
					{
						standingOnExit.OnStandingExit(this.m_gameObject);
					}
				}
			}
			if (this.StandingOn && this.StandingOn.CompareTag("MagicPlatform"))
			{
				GameObject root = this.StandingOn.GetRoot(false);
				if (root)
				{
					IForcePlatformCollision component = root.GetComponent<IForcePlatformCollision>();
					if (!component.IsNativeNull())
					{
						component.ForcePlatformCollision();
					}
				}
			}
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00047878 File Offset: 0x00045A78
	protected override void ComputeNewSpeed()
	{
		if (Time.deltaTime > 0f && (!this.m_characterExists || (this.m_characterExists && this.m_character.MovementState.CurrentState != CharacterStates.MovementStates.Jumping)) && base.State.IsGrounded)
		{
			this._velocity = this._newPosition / Time.deltaTime;
		}
		if (base.State.IsGrounded)
		{
			this._velocity.x = this._velocity.x * base.Parameters.SlopeAngleSpeedFactor.Evaluate(Mathf.Abs(base.State.BelowSlopeAngle) * Mathf.Sign(this._velocity.y));
		}
		if (!base.State.OnAMovingPlatform)
		{
			this._velocity.x = Mathf.Clamp(this._velocity.x, -base.Parameters.MaxVelocity.x, base.Parameters.MaxVelocity.x);
			this._velocity.y = Mathf.Clamp(this._velocity.y, -base.Parameters.MaxVelocity.y, base.Parameters.MaxVelocity.y);
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000479AC File Offset: 0x00045BAC
	protected override void StickToSlope()
	{
		if ((this._newPosition.y >= 0f && !base.State.WasGroundedLastFrame) || (this.m_characterExists && (this.m_character.MovementState.CurrentState == CharacterStates.MovementStates.Jumping || this.m_character.MovementState.CurrentState == CharacterStates.MovementStates.Dashing || this.m_character.MovementState.CurrentState == CharacterStates.MovementStates.DownStriking)) || !this.StickWhenWalkingDownSlopes || !base.State.WasGroundedLastFrame || this._externalForce.y > 0f || this._movingPlatform)
		{
			return;
		}
		float num;
		if (this.StickyRaycastLength == 0f)
		{
			num = this._boundsWidth * Mathf.Abs(Mathf.Tan(base.Parameters.MaximumSlopeAngle));
			num += this._boundsHeight / 2f + this.VerticalRayOffset;
		}
		else
		{
			num = this.StickyRaycastLength + this._boundsHeight / 2f;
		}
		if (this.m_character && this._newPosition.x == 0f)
		{
			this._stickRayCastOrigin.x = (this.m_character.IsFacingRight ? this._boundsBottomLeftCorner.x : this._boundsTopRightCorner.x);
		}
		else
		{
			this._stickRayCastOrigin.x = ((this._newPosition.x > 0f) ? this._boundsBottomLeftCorner.x : this._boundsTopRightCorner.x);
		}
		this._stickRayCastOrigin.x = this._stickRayCastOrigin.x + this._newPosition.x;
		this._stickRayCastOrigin.y = this._boundsCenter.y + this.VerticalRayOffset;
		this._stickRaycast = Physics2D.Raycast(this._stickRayCastOrigin, -base.transform.up, num, this.PlatformMask);
		if (this._stickRaycast)
		{
			float num2 = Mathf.Abs(Vector2.Angle(this._stickRaycast.normal, base.transform.up));
			if (num2 > base.Parameters.MaximumSlopeAngle || num2 <= 0f)
			{
				return;
			}
			if (base.StandingOnLastFrame && base.StandingOnLastFrame.layer == 11 && this._stickRaycast.collider.gameObject != base.StandingOnLastFrame.gameObject)
			{
				return;
			}
			if (this._stickRaycast.collider == this._ignoredCollider)
			{
				return;
			}
			this._newPosition.y = -Mathf.Abs(this._stickRaycast.point.y - this._stickRayCastOrigin.y) + this._boundsHeight / 2f + this.VerticalRayOffset;
			this._contactList.Add(this._stickRaycast);
			base.State.IsCollidingBelow = true;
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00047C9C File Offset: 0x00045E9C
	public void ForceStandingOn(Collider2D collider)
	{
		base.StandingOnCollider = collider;
		this.StandingOn = collider.gameObject;
		base.State.IsCollidingBelow = true;
		this._velocity.y = 0f;
		this._newPosition.y = 0f;
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00047CE8 File Offset: 0x00045EE8
	public void ResetState()
	{
		this._boxCollider.gameObject.layer = this.m_boxColliderLayer;
		this._newPosition = Vector2.zero;
		this._velocity = Vector2.zero;
		this._externalForce = Vector2.zero;
		base.State.WasGroundedLastFrame = false;
		base.State.WasTouchingTheCeilingLastFrame = false;
		base.CurrentWallCollider = null;
		base.State.Reset();
		this.StandingOn = null;
		base.StandingOnLastFrame = null;
		base.StandingOnCollider = null;
		this.m_heightStandingOn = float.MaxValue;
		this.m_lastHeightStandingOn = float.MaxValue;
		this.m_angleStandingOn = 0f;
		this.m_lastAngleStandingOn = 0f;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x000036DA File Offset: 0x000018DA
	public void OnDestroy()
	{
		this.m_character = null;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400017E RID: 382
	private static Dictionary<GameObject, int> m_platformRevertDict_STATIC = new Dictionary<GameObject, int>();

	// Token: 0x0400017F RID: 383
	private static int m_ignoreRaycastLayer_STATIC = 2;

	// Token: 0x04000180 RID: 384
	protected Character m_character;

	// Token: 0x04000181 RID: 385
	protected Rect m_absBounds;

	// Token: 0x04000182 RID: 386
	private float m_slopeYOffset;

	// Token: 0x04000183 RID: 387
	private int m_boxColliderLayer;

	// Token: 0x04000184 RID: 388
	private float m_lastHeightStandingOn;

	// Token: 0x04000185 RID: 389
	private float m_heightStandingOn;

	// Token: 0x04000186 RID: 390
	private float m_lastAngleStandingOn;

	// Token: 0x04000187 RID: 391
	private float m_angleStandingOn;

	// Token: 0x04000188 RID: 392
	private bool m_disableOnewayCollision;

	// Token: 0x04000189 RID: 393
	private bool m_disablePlatformCollision;

	// Token: 0x0400018A RID: 394
	private GameObject m_previouslyStandingOn;

	// Token: 0x0400018B RID: 395
	private bool m_prevIsFalling;

	// Token: 0x0400018C RID: 396
	private GameObject m_gameObject;

	// Token: 0x0400018D RID: 397
	private bool m_characterExists;

	// Token: 0x0400018E RID: 398
	private bool m_hittingSlopedWall;

	// Token: 0x0400018F RID: 399
	private bool m_isPlayer;

	// Token: 0x04000190 RID: 400
	private Relay<object, GameObject> m_standingOnChangeRelay = new Relay<object, GameObject>();

	// Token: 0x04000191 RID: 401
	private Relay<CorgiController_RL> m_onCorgiCollisionRelay = new Relay<CorgiController_RL>();

	// Token: 0x04000192 RID: 402
	private Relay<CorgiController_RL> m_onCorgiLandedEnterRelay = new Relay<CorgiController_RL>();

	// Token: 0x04000193 RID: 403
	private Relay<CorgiController_RL> m_onCorgiLandedExitRelay = new Relay<CorgiController_RL>();

	// Token: 0x04000194 RID: 404
	private Relay<CorgiController_RL> m_corgiStartFallingRelay = new Relay<CorgiController_RL>();

	// Token: 0x04000195 RID: 405
	[SerializeField]
	private float m_yRaycastOffset;

	// Token: 0x04000196 RID: 406
	[SerializeField]
	private float m_jumpLeewayAmount;

	// Token: 0x04000197 RID: 407
	[SerializeField]
	private bool m_enablePrecisionPointChecks;

	// Token: 0x04000198 RID: 408
	public bool RetainVelocity;

	// Token: 0x04000199 RID: 409
	public bool UseGlobalEVRaycastSettings = true;

	// Token: 0x0400019A RID: 410
	[Tooltip("Disables IStandingOn check.  Greatly reduces performance hit.")]
	public bool DisableStandingOnColliderLogic = true;

	// Token: 0x0400019B RID: 411
	private List<Collider2D> m_standingOnColliderList;

	// Token: 0x0400019C RID: 412
	private List<Collider2D> m_prevStandingOnColliderList;

	// Token: 0x0400019D RID: 413
	private static List<IStandingOnEnter> m_standingOnEnterHelperList = new List<IStandingOnEnter>();

	// Token: 0x0400019E RID: 414
	private static List<IStandingOnExit> m_standingOnExitHelperList = new List<IStandingOnExit>();

	// Token: 0x040001A5 RID: 421
	private static List<IHitboxController> m_hitboxControllerHelper_STATIC = new List<IHitboxController>();

	// Token: 0x040001A6 RID: 422
	private float m_lastStandingTimeCheck;

	// Token: 0x040001A7 RID: 423
	private const float LAST_STANDING_INTERVAL = 0.5f;
}

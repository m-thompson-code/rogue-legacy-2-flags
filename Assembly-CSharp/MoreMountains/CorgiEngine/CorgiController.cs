using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F1D RID: 3869
	[AddComponentMenu("Corgi Engine/Character/Core/Corgi Controller")]
	public class CorgiController : MonoBehaviour
	{
		// Token: 0x17002441 RID: 9281
		// (get) Token: 0x06006F98 RID: 28568 RVA: 0x0003D8A3 File Offset: 0x0003BAA3
		// (set) Token: 0x06006F99 RID: 28569 RVA: 0x0003D8AB File Offset: 0x0003BAAB
		public CorgiControllerState State { get; protected set; }

		// Token: 0x17002442 RID: 9282
		// (get) Token: 0x06006F9A RID: 28570 RVA: 0x0003D8B4 File Offset: 0x0003BAB4
		public CorgiControllerParameters Parameters
		{
			get
			{
				return this._overrideParameters ?? this.DefaultParameters;
			}
		}

		// Token: 0x17002443 RID: 9283
		// (get) Token: 0x06006F9B RID: 28571 RVA: 0x0003D8C6 File Offset: 0x0003BAC6
		// (set) Token: 0x06006F9C RID: 28572 RVA: 0x0003D8CE File Offset: 0x0003BACE
		public GameObject StandingOnLastFrame { get; protected set; }

		// Token: 0x17002444 RID: 9284
		// (get) Token: 0x06006F9D RID: 28573 RVA: 0x0003D8D7 File Offset: 0x0003BAD7
		// (set) Token: 0x06006F9E RID: 28574 RVA: 0x0003D8DF File Offset: 0x0003BADF
		public Collider2D StandingOnCollider { get; protected set; }

		// Token: 0x17002445 RID: 9285
		// (get) Token: 0x06006F9F RID: 28575 RVA: 0x0003D8E8 File Offset: 0x0003BAE8
		public Vector2 Velocity
		{
			get
			{
				return this._velocity;
			}
		}

		// Token: 0x17002446 RID: 9286
		// (get) Token: 0x06006FA0 RID: 28576 RVA: 0x0003D8F0 File Offset: 0x0003BAF0
		// (set) Token: 0x06006FA1 RID: 28577 RVA: 0x0003D8F8 File Offset: 0x0003BAF8
		public Vector2 ForcesApplied { get; protected set; }

		// Token: 0x17002447 RID: 9287
		// (get) Token: 0x06006FA2 RID: 28578 RVA: 0x0003D901 File Offset: 0x0003BB01
		// (set) Token: 0x06006FA3 RID: 28579 RVA: 0x0003D909 File Offset: 0x0003BB09
		public GameObject CurrentWallCollider { get; protected set; }

		// Token: 0x17002448 RID: 9288
		// (get) Token: 0x06006FA4 RID: 28580 RVA: 0x0003D912 File Offset: 0x0003BB12
		public Vector3 ColliderSize
		{
			get
			{
				return Vector3.Scale(base.transform.localScale, this._boxCollider.size);
			}
		}

		// Token: 0x17002449 RID: 9289
		// (get) Token: 0x06006FA5 RID: 28581 RVA: 0x0018E0DC File Offset: 0x0018C2DC
		public Vector3 ColliderCenterPosition
		{
			get
			{
				return this._boxCollider.bounds.center;
			}
		}

		// Token: 0x1700244A RID: 9290
		// (get) Token: 0x06006FA6 RID: 28582 RVA: 0x0018E0FC File Offset: 0x0018C2FC
		public virtual Vector3 ColliderBottomPosition
		{
			get
			{
				this._colliderBottomCenterPosition.x = this._boxCollider.bounds.center.x;
				this._colliderBottomCenterPosition.y = this._boxCollider.bounds.min.y;
				this._colliderBottomCenterPosition.z = 0f;
				return this._colliderBottomCenterPosition;
			}
		}

		// Token: 0x1700244B RID: 9291
		// (get) Token: 0x06006FA7 RID: 28583 RVA: 0x0018E168 File Offset: 0x0018C368
		public virtual Vector3 ColliderLeftPosition
		{
			get
			{
				this._colliderLeftCenterPosition.x = this._boxCollider.bounds.min.x;
				this._colliderLeftCenterPosition.y = this._boxCollider.bounds.center.y;
				this._colliderLeftCenterPosition.z = 0f;
				return this._colliderLeftCenterPosition;
			}
		}

		// Token: 0x1700244C RID: 9292
		// (get) Token: 0x06006FA8 RID: 28584 RVA: 0x0018E1D4 File Offset: 0x0018C3D4
		public virtual Vector3 ColliderTopPosition
		{
			get
			{
				this._colliderTopCenterPosition.x = this._boxCollider.bounds.center.x;
				this._colliderTopCenterPosition.y = this._boxCollider.bounds.max.y;
				this._colliderTopCenterPosition.z = 0f;
				return this._colliderTopCenterPosition;
			}
		}

		// Token: 0x1700244D RID: 9293
		// (get) Token: 0x06006FA9 RID: 28585 RVA: 0x0018E240 File Offset: 0x0018C440
		public virtual Vector3 ColliderRightPosition
		{
			get
			{
				this._colliderRightCenterPosition.x = this._boxCollider.bounds.max.x;
				this._colliderRightCenterPosition.y = this._boxCollider.bounds.center.y;
				this._colliderRightCenterPosition.z = 0f;
				return this._colliderRightCenterPosition;
			}
		}

		// Token: 0x1700244E RID: 9294
		// (get) Token: 0x06006FAA RID: 28586 RVA: 0x0003D934 File Offset: 0x0003BB34
		public float Friction
		{
			get
			{
				return this._friction;
			}
		}

		// Token: 0x1700244F RID: 9295
		// (get) Token: 0x06006FAB RID: 28587 RVA: 0x0003D93C File Offset: 0x0003BB3C
		public virtual Vector3 BoundsTopLeftCorner
		{
			get
			{
				return this._boundsTopLeftCorner;
			}
		}

		// Token: 0x17002450 RID: 9296
		// (get) Token: 0x06006FAC RID: 28588 RVA: 0x0003D949 File Offset: 0x0003BB49
		public virtual Vector3 BoundsBottomLeftCorner
		{
			get
			{
				return this._boundsBottomLeftCorner;
			}
		}

		// Token: 0x17002451 RID: 9297
		// (get) Token: 0x06006FAD RID: 28589 RVA: 0x0003D956 File Offset: 0x0003BB56
		public virtual Vector3 BoundsTopRightCorner
		{
			get
			{
				return this._boundsTopRightCorner;
			}
		}

		// Token: 0x17002452 RID: 9298
		// (get) Token: 0x06006FAE RID: 28590 RVA: 0x0003D963 File Offset: 0x0003BB63
		public virtual Vector3 BoundsBottomRightCorner
		{
			get
			{
				return this._boundsBottomRightCorner;
			}
		}

		// Token: 0x17002453 RID: 9299
		// (get) Token: 0x06006FAF RID: 28591 RVA: 0x0003D970 File Offset: 0x0003BB70
		public virtual Vector3 BoundsTop
		{
			get
			{
				return (this._boundsTopLeftCorner + this._boundsTopRightCorner) / 2f;
			}
		}

		// Token: 0x17002454 RID: 9300
		// (get) Token: 0x06006FB0 RID: 28592 RVA: 0x0003D992 File Offset: 0x0003BB92
		public virtual Vector3 BoundsBottom
		{
			get
			{
				return (this._boundsBottomLeftCorner + this._boundsBottomRightCorner) / 2f;
			}
		}

		// Token: 0x17002455 RID: 9301
		// (get) Token: 0x06006FB1 RID: 28593 RVA: 0x0003D9B4 File Offset: 0x0003BBB4
		public virtual Vector3 BoundsRight
		{
			get
			{
				return (this._boundsTopRightCorner + this._boundsBottomRightCorner) / 2f;
			}
		}

		// Token: 0x17002456 RID: 9302
		// (get) Token: 0x06006FB2 RID: 28594 RVA: 0x0003D9D6 File Offset: 0x0003BBD6
		public virtual Vector3 BoundsLeft
		{
			get
			{
				return (this._boundsTopLeftCorner + this._boundsBottomLeftCorner) / 2f;
			}
		}

		// Token: 0x17002457 RID: 9303
		// (get) Token: 0x06006FB3 RID: 28595 RVA: 0x0003D9F8 File Offset: 0x0003BBF8
		public virtual Vector3 BoundsCenter
		{
			get
			{
				return this._boundsCenter;
			}
		}

		// Token: 0x17002458 RID: 9304
		// (get) Token: 0x06006FB4 RID: 28596 RVA: 0x0003DA05 File Offset: 0x0003BC05
		public LayerMask SavedPlatformMask
		{
			get
			{
				return this._platformMaskSave;
			}
		}

		// Token: 0x17002459 RID: 9305
		// (get) Token: 0x06006FB5 RID: 28597 RVA: 0x0003DA0D File Offset: 0x0003BC0D
		// (set) Token: 0x06006FB6 RID: 28598 RVA: 0x0003DA15 File Offset: 0x0003BC15
		public bool IsWithinJumpLeeway { get; protected set; }

		// Token: 0x1700245A RID: 9306
		// (get) Token: 0x06006FB7 RID: 28599 RVA: 0x0003DA1E File Offset: 0x0003BC1E
		// (set) Token: 0x06006FB8 RID: 28600 RVA: 0x0003DA26 File Offset: 0x0003BC26
		public bool IsInitialized { get; protected set; }

		// Token: 0x06006FB9 RID: 28601 RVA: 0x0018E2AC File Offset: 0x0018C4AC
		protected virtual void Awake()
		{
			this.SavePlatformMask();
			this.PlatformMask |= this.OneWayPlatformMask;
			this.PlatformMask |= this.MovingPlatformMask;
			this.PlatformMask |= this.MovingOneWayPlatformMask;
			this.PlatformMask |= this.MidHeightOneWayPlatformMask;
		}

		// Token: 0x06006FBA RID: 28602 RVA: 0x0003DA2F File Offset: 0x0003BC2F
		public void SavePlatformMask()
		{
			if (!this.m_savePlatformMaskSet)
			{
				this._platformMaskSave = this.PlatformMask;
				this.m_savePlatformMaskSet = true;
			}
		}

		// Token: 0x06006FBB RID: 28603 RVA: 0x0003DA4C File Offset: 0x0003BC4C
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06006FBC RID: 28604 RVA: 0x0018E348 File Offset: 0x0018C548
		protected virtual void Initialization()
		{
			this._transform = base.transform;
			this._boxCollider = base.GetComponent<BoxCollider2D>();
			this._originalColliderSize = this._boxCollider.size;
			this._originalColliderOffset = this._boxCollider.offset;
			if (this._boxCollider.offset.x != 0f && this.Parameters.DisplayWarnings)
			{
				Debug.LogWarning("The boxcollider for " + base.gameObject.name + " should have an x offset set to zero. Right now this may cause issues when you change direction close to a wall.");
			}
			this._contactList = new List<RaycastHit2D>();
			this.State = new CorgiControllerState();
			this._sideHitsStorage = new RaycastHit2D[this.NumberOfHorizontalRays];
			this._belowHitsStorage = new RaycastHit2D[this.NumberOfVerticalRays];
			this._aboveHitsStorage = new RaycastHit2D[this.NumberOfVerticalRays];
			this.CurrentWallCollider = null;
			this.State.Reset();
			this.SetRaysParameters();
		}

		// Token: 0x06006FBD RID: 28605 RVA: 0x0003DA54 File Offset: 0x0003BC54
		public virtual void AddForce(Vector2 force)
		{
			this._velocity += force;
			this._externalForce += force;
		}

		// Token: 0x06006FBE RID: 28606 RVA: 0x0003DA7A File Offset: 0x0003BC7A
		public virtual void AddHorizontalForce(float x)
		{
			this._velocity.x = this._velocity.x + x;
			this._externalForce.x = this._externalForce.x + x;
		}

		// Token: 0x06006FBF RID: 28607 RVA: 0x0003DA9C File Offset: 0x0003BC9C
		public virtual void AddVerticalForce(float y)
		{
			this._velocity.y = this._velocity.y + y;
			this._externalForce.y = this._externalForce.y + y;
		}

		// Token: 0x06006FC0 RID: 28608 RVA: 0x0003DABE File Offset: 0x0003BCBE
		public virtual void SetForce(Vector2 force)
		{
			this._velocity = force;
			this._externalForce = force;
		}

		// Token: 0x06006FC1 RID: 28609 RVA: 0x0003DACE File Offset: 0x0003BCCE
		public virtual void SetHorizontalForce(float x)
		{
			this._velocity.x = x;
			this._externalForce.x = x;
		}

		// Token: 0x06006FC2 RID: 28610 RVA: 0x0003DAE8 File Offset: 0x0003BCE8
		public virtual void SetVerticalForce(float y)
		{
			this._velocity.y = y;
			this._externalForce.y = y;
		}

		// Token: 0x06006FC3 RID: 28611 RVA: 0x0003DB02 File Offset: 0x0003BD02
		protected virtual void LateUpdate()
		{
			this.EveryFrame();
		}

		// Token: 0x06006FC4 RID: 28612 RVA: 0x0018E434 File Offset: 0x0018C634
		protected virtual void EveryFrame()
		{
			this.ApplyGravity();
			this.FrameInitialization();
			this.SetRaysParameters();
			this.HandleMovingPlatforms();
			this.ForcesApplied = this._velocity;
			this.DetermineMovementDirection();
			if (this.CastRaysOnBothSides)
			{
				this.CastRaysToTheLeft();
				this.CastRaysToTheRight();
			}
			else if (this._movementDirection == -1f)
			{
				this.CastRaysToTheLeft();
			}
			else
			{
				this.CastRaysToTheRight();
			}
			this.CastRaysBelow();
			this.CastRaysAbove();
			this._transform.Translate(this._newPosition, Space.Self);
			this.SetRaysParameters();
			this.ComputeNewSpeed();
			this.SetStates();
			this._externalForce.x = 0f;
			this._externalForce.y = 0f;
			this.FrameExit();
		}

		// Token: 0x06006FC5 RID: 28613 RVA: 0x0018E4F8 File Offset: 0x0018C6F8
		protected virtual void FrameInitialization()
		{
			this._contactList.Clear();
			this._newPosition = this.Velocity * Time.deltaTime;
			this.State.WasGroundedLastFrame = this.State.IsCollidingBelow;
			this.StandingOnLastFrame = this.StandingOn;
			this.State.WasTouchingTheCeilingLastFrame = this.State.IsCollidingAbove;
			this.CurrentWallCollider = null;
			this.State.Reset();
		}

		// Token: 0x06006FC6 RID: 28614 RVA: 0x0003DB0A File Offset: 0x0003BD0A
		protected virtual void FrameExit()
		{
			if (this.StandingOnLastFrame)
			{
				this.StandingOnLastFrame.layer = this._savedBelowLayer;
			}
		}

		// Token: 0x06006FC7 RID: 28615 RVA: 0x0018E570 File Offset: 0x0018C770
		protected virtual void DetermineMovementDirection()
		{
			this._movementDirection = this._storedMovementDirection;
			if (this._velocity.x < -0.0001f || this._externalForce.x < -0.0001f)
			{
				this._movementDirection = -1f;
			}
			else if (this._velocity.x > 0.0001f || this._externalForce.x > 0.0001f)
			{
				this._movementDirection = 1f;
			}
			this._storedMovementDirection = this._movementDirection;
		}

		// Token: 0x06006FC8 RID: 28616 RVA: 0x0018E5F8 File Offset: 0x0018C7F8
		protected virtual void ApplyGravity()
		{
			this._currentGravity = this.Parameters.Gravity;
			if (this._velocity.y > 0f)
			{
				this._currentGravity *= this.Parameters.AscentMultiplier;
			}
			else
			{
				this._currentGravity *= this.Parameters.FallMultiplier;
			}
			if (this._gravityActive)
			{
				this._velocity.y = this._velocity.y + (this._currentGravity + this._movingPlatformCurrentGravity) * Time.deltaTime;
			}
			if (this._fallSlowFactor != 0f)
			{
				this._velocity.y = this._velocity.y * this._fallSlowFactor;
			}
		}

		// Token: 0x06006FC9 RID: 28617 RVA: 0x0018E6A8 File Offset: 0x0018C8A8
		protected virtual void HandleMovingPlatforms()
		{
			if (this._movingPlatform != null)
			{
				if (!float.IsNaN(this._movingPlatform.CurrentSpeed.x) && !float.IsNaN(this._movingPlatform.CurrentSpeed.y) && !float.IsNaN(this._movingPlatform.CurrentSpeed.z))
				{
					this._transform.Translate(this._movingPlatform.CurrentSpeed * Time.deltaTime);
				}
				if (Time.timeScale == 0f || float.IsNaN(this._movingPlatform.CurrentSpeed.x) || float.IsNaN(this._movingPlatform.CurrentSpeed.y) || float.IsNaN(this._movingPlatform.CurrentSpeed.z))
				{
					return;
				}
				if (Time.deltaTime <= 0f)
				{
					return;
				}
				this.State.OnAMovingPlatform = true;
				this.GravityActive(false);
				this._movingPlatformCurrentGravity = -500f;
				this._newPosition.y = this._movingPlatform.CurrentSpeed.y * Time.deltaTime;
				this._velocity = -this._newPosition / Time.deltaTime;
				this._velocity.x = -this._velocity.x;
				this.SetRaysParameters();
			}
		}

		// Token: 0x06006FCA RID: 28618 RVA: 0x0003DB2A File Offset: 0x0003BD2A
		public virtual void DetachFromMovingPlatform()
		{
			if (this._movingPlatform == null)
			{
				return;
			}
			this.GravityActive(true);
			this.State.OnAMovingPlatform = false;
			this._movingPlatform = null;
			this._movingPlatformCurrentGravity = 0f;
		}

		// Token: 0x06006FCB RID: 28619 RVA: 0x0018E804 File Offset: 0x0018CA04
		public virtual bool CastRays(CorgiController.RaycastDirections direction, float rayLength, Color color, ref RaycastHit2D[] storageArray)
		{
			bool result = false;
			switch (direction)
			{
			case CorgiController.RaycastDirections.up:
				this._verticalRayCastFromLeft = (this._boundsBottomLeftCorner + this._boundsTopLeftCorner) / 2f;
				this._verticalRayCastToRight = (this._boundsBottomRightCorner + this._boundsTopRightCorner) / 2f;
				this._verticalRayCastFromLeft += base.transform.up * this.HorizontalRayOffset;
				this._verticalRayCastToRight += base.transform.up * this.HorizontalRayOffset;
				this._verticalRayCastFromLeft += base.transform.right * this._newPosition.x;
				this._verticalRayCastToRight += base.transform.right * this._newPosition.x;
				for (int i = 0; i < this.NumberOfVerticalRays; i++)
				{
					Vector2 rayOriginPoint = Vector2.Lerp(this._verticalRayCastFromLeft, this._verticalRayCastToRight, (float)i / (float)(this.NumberOfVerticalRays - 1));
					if (this._newPosition.y > 0f && !this.State.WasGroundedLastFrame)
					{
						storageArray[i] = MMDebug.RayCast(rayOriginPoint, base.transform.up, rayLength, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask, color, this.Parameters.DrawRaycastsGizmos);
						if (storageArray[i])
						{
							result = true;
						}
					}
				}
				return result;
			case CorgiController.RaycastDirections.down:
				this._verticalRayCastFromLeft = (this._boundsBottomLeftCorner + this._boundsTopLeftCorner) / 2f;
				this._verticalRayCastToRight = (this._boundsBottomRightCorner + this._boundsTopRightCorner) / 2f;
				this._verticalRayCastFromLeft += base.transform.up * this.HorizontalRayOffset;
				this._verticalRayCastToRight += base.transform.up * this.HorizontalRayOffset;
				this._verticalRayCastFromLeft += base.transform.right * this._newPosition.x;
				this._verticalRayCastToRight += base.transform.right * this._newPosition.x;
				for (int j = 0; j < this.NumberOfVerticalRays; j++)
				{
					Vector2 rayOriginPoint2 = Vector2.Lerp(this._verticalRayCastFromLeft, this._verticalRayCastToRight, (float)j / (float)(this.NumberOfVerticalRays - 1));
					if (this._newPosition.y > 0f && !this.State.WasGroundedLastFrame)
					{
						storageArray[j] = MMDebug.RayCast(rayOriginPoint2, -base.transform.up, rayLength, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask, color, this.Parameters.DrawRaycastsGizmos);
						if (storageArray[j])
						{
							result = true;
						}
					}
				}
				return result;
			case CorgiController.RaycastDirections.left:
				this._horizontalRayCastFromBottom = (this._boundsBottomRightCorner + this._boundsBottomLeftCorner) / 2f;
				this._horizontalRayCastToTop = (this._boundsTopLeftCorner + this._boundsTopRightCorner) / 2f;
				this._horizontalRayCastFromBottom += base.transform.up * 0.05f;
				this._horizontalRayCastToTop -= base.transform.up * 0.05f;
				for (int k = 0; k < this.NumberOfHorizontalRays; k++)
				{
					Vector2 rayOriginPoint3 = Vector2.Lerp(this._horizontalRayCastFromBottom, this._horizontalRayCastToTop, (float)k / (float)(this.NumberOfHorizontalRays - 1));
					storageArray[k] = MMDebug.RayCast(rayOriginPoint3, -base.transform.right, rayLength, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask, color, this.Parameters.DrawRaycastsGizmos);
					if (storageArray[k])
					{
						result = true;
					}
				}
				return result;
			case CorgiController.RaycastDirections.right:
				this._horizontalRayCastFromBottom = (this._boundsBottomRightCorner + this._boundsBottomLeftCorner) / 2f;
				this._horizontalRayCastToTop = (this._boundsTopLeftCorner + this._boundsTopRightCorner) / 2f;
				this._horizontalRayCastFromBottom += base.transform.up * 0.05f;
				this._horizontalRayCastToTop -= base.transform.up * 0.05f;
				for (int l = 0; l < this.NumberOfHorizontalRays; l++)
				{
					Vector2 rayOriginPoint4 = Vector2.Lerp(this._horizontalRayCastFromBottom, this._horizontalRayCastToTop, (float)l / (float)(this.NumberOfHorizontalRays - 1));
					storageArray[l] = MMDebug.RayCast(rayOriginPoint4, base.transform.right, rayLength, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask, color, this.Parameters.DrawRaycastsGizmos);
					if (storageArray[l])
					{
						result = true;
					}
				}
				return result;
			default:
				return false;
			}
		}

		// Token: 0x06006FCC RID: 28620 RVA: 0x0003DB60 File Offset: 0x0003BD60
		protected virtual void CastRaysToTheLeft()
		{
			this.CastRaysToTheSides(-1f);
		}

		// Token: 0x06006FCD RID: 28621 RVA: 0x0003DB6D File Offset: 0x0003BD6D
		protected virtual void CastRaysToTheRight()
		{
			this.CastRaysToTheSides(1f);
		}

		// Token: 0x06006FCE RID: 28622 RVA: 0x0018EE30 File Offset: 0x0018D030
		protected virtual void CastRaysToTheSides(float raysDirection)
		{
			this._horizontalRayCastFromBottom = (this._boundsBottomRightCorner + this._boundsBottomLeftCorner) / 2f;
			this._horizontalRayCastToTop = (this._boundsTopLeftCorner + this._boundsTopRightCorner) / 2f;
			this._horizontalRayCastFromBottom += base.transform.up * 0.05f;
			this._horizontalRayCastToTop -= base.transform.up * 0.05f;
			float rayDistance = Mathf.Abs(this._velocity.x * Time.deltaTime) + this._boundsWidth / 2f + this.HorizontalRayOffset * 2f;
			if (this._sideHitsStorage.Length != this.NumberOfHorizontalRays)
			{
				this._sideHitsStorage = new RaycastHit2D[this.NumberOfHorizontalRays];
			}
			for (int i = 0; i < this.NumberOfHorizontalRays; i++)
			{
				Vector2 rayOriginPoint = Vector2.Lerp(this._horizontalRayCastFromBottom, this._horizontalRayCastToTop, (float)i / (float)(this.NumberOfHorizontalRays - 1));
				if (this.State.WasGroundedLastFrame && i == 0)
				{
					this._sideHitsStorage[i] = MMDebug.RayCast(rayOriginPoint, raysDirection * base.transform.right, rayDistance, this.PlatformMask, Colors.Indigo, this.Parameters.DrawRaycastsGizmos);
				}
				else
				{
					this._sideHitsStorage[i] = MMDebug.RayCast(rayOriginPoint, raysDirection * base.transform.right, rayDistance, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask, Colors.Indigo, this.Parameters.DrawRaycastsGizmos);
				}
				if (this._sideHitsStorage[i].distance > 0f)
				{
					if (this._sideHitsStorage[i].collider == this._ignoredCollider)
					{
						break;
					}
					float num = Mathf.Abs(Vector2.Angle(this._sideHitsStorage[i].normal, base.transform.up));
					if (this._movementDirection == raysDirection)
					{
						this.State.LateralSlopeAngle = num;
					}
					if (num > this.Parameters.MaximumSlopeAngle)
					{
						if (raysDirection < 0f)
						{
							this.State.IsCollidingLeft = true;
							this.State.DistanceToLeftCollider = this._sideHitsStorage[i].distance;
						}
						else
						{
							this.State.IsCollidingRight = true;
							this.State.DistanceToRightCollider = this._sideHitsStorage[i].distance;
						}
						if (this._movementDirection == raysDirection)
						{
							this.CurrentWallCollider = this._sideHitsStorage[i].collider.gameObject;
							this.State.SlopeAngleOK = false;
							float num2 = MMMaths.DistanceBetweenPointAndLine(this._sideHitsStorage[i].point, this._horizontalRayCastFromBottom, this._horizontalRayCastToTop);
							if (raysDirection <= 0f)
							{
								this._newPosition.x = -num2 + this._boundsWidth / 2f + this.HorizontalRayOffset * 2f;
							}
							else
							{
								this._newPosition.x = num2 - this._boundsWidth / 2f - this.HorizontalRayOffset * 2f;
							}
							if (!this.State.IsGrounded && this.Velocity.y > 0f)
							{
								this._newPosition.x = 0f;
							}
							this._contactList.Add(this._sideHitsStorage[i]);
							this._velocity.x = 0f;
							return;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06006FCF RID: 28623 RVA: 0x0018F210 File Offset: 0x0018D410
		protected virtual void CastRaysBelow()
		{
			this._friction = 0f;
			if (this._newPosition.y < -0.0001f)
			{
				this.State.IsFalling = true;
			}
			else
			{
				this.State.IsFalling = false;
			}
			if (this.Parameters.Gravity > 0f && !this.State.IsFalling)
			{
				this.State.IsCollidingBelow = false;
				return;
			}
			float num = this._boundsHeight / 2f + this.HorizontalRayOffset;
			if (this.State.OnAMovingPlatform)
			{
				num *= 2f;
			}
			if (this._newPosition.y < 0f)
			{
				num += Mathf.Abs(this._newPosition.y);
			}
			this._verticalRayCastFromLeft = (this._boundsBottomLeftCorner + this._boundsTopLeftCorner) / 2f;
			this._verticalRayCastToRight = (this._boundsBottomRightCorner + this._boundsTopRightCorner) / 2f;
			this._verticalRayCastFromLeft += base.transform.up * this.HorizontalRayOffset;
			this._verticalRayCastToRight += base.transform.up * this.HorizontalRayOffset;
			this._verticalRayCastFromLeft += base.transform.right * this._newPosition.x;
			this._verticalRayCastToRight += base.transform.right * this._newPosition.x;
			if (this._belowHitsStorage.Length != this.NumberOfVerticalRays)
			{
				this._belowHitsStorage = new RaycastHit2D[this.NumberOfVerticalRays];
			}
			this._raysBelowLayerMaskPlatforms = this.PlatformMask;
			this._raysBelowLayerMaskPlatformsWithoutOneWay = (this.PlatformMask & ~this.MidHeightOneWayPlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask);
			this._raysBelowLayerMaskPlatformsWithoutMidHeight = (this._raysBelowLayerMaskPlatforms & ~this.MidHeightOneWayPlatformMask);
			if (this.StandingOnLastFrame != null)
			{
				this._savedBelowLayer = this.StandingOnLastFrame.layer;
				if (this.MidHeightOneWayPlatformMask.Contains(this.StandingOnLastFrame.layer))
				{
					this.StandingOnLastFrame.layer = 8;
				}
			}
			if (this.State.WasGroundedLastFrame && this.StandingOnLastFrame != null && !this.MidHeightOneWayPlatformMask.Contains(this.StandingOnLastFrame.layer))
			{
				this._raysBelowLayerMaskPlatforms = this._raysBelowLayerMaskPlatformsWithoutMidHeight;
			}
			float num2 = float.MaxValue;
			int num3 = 0;
			bool flag = false;
			for (int i = 0; i < this.NumberOfVerticalRays; i++)
			{
				Vector2 rayOriginPoint = Vector2.Lerp(this._verticalRayCastFromLeft, this._verticalRayCastToRight, (float)i / (float)(this.NumberOfVerticalRays - 1));
				if (this._newPosition.y > 0f && !this.State.WasGroundedLastFrame)
				{
					this._belowHitsStorage[i] = MMDebug.RayCast(rayOriginPoint, -base.transform.up, num, this._raysBelowLayerMaskPlatformsWithoutOneWay, Color.blue, this.Parameters.DrawRaycastsGizmos);
				}
				else
				{
					this._belowHitsStorage[i] = MMDebug.RayCast(rayOriginPoint, -base.transform.up, num, this._raysBelowLayerMaskPlatforms, Color.blue, this.Parameters.DrawRaycastsGizmos);
				}
				if (MMMaths.DistanceBetweenPointAndLine(this._belowHitsStorage[num3].point, this._verticalRayCastFromLeft, this._verticalRayCastToRight) < 0.0001f)
				{
					break;
				}
				if (this._belowHitsStorage[i] && !(this._belowHitsStorage[i].collider == this._ignoredCollider))
				{
					flag = true;
					this.State.BelowSlopeAngle = Vector2.Angle(this._belowHitsStorage[i].normal, base.transform.up);
					this._crossBelowSlopeAngle = Vector3.Cross(base.transform.up, this._belowHitsStorage[i].normal);
					if (this._crossBelowSlopeAngle.z < 0f)
					{
						this.State.BelowSlopeAngle = -this.State.BelowSlopeAngle;
					}
					if (this._belowHitsStorage[i].distance < num2)
					{
						num3 = i;
						num2 = this._belowHitsStorage[i].distance;
					}
				}
			}
			if (flag)
			{
				this.StandingOn = this._belowHitsStorage[num3].collider.gameObject;
				this.StandingOnCollider = this._belowHitsStorage[num3].collider;
				if (!this.State.WasGroundedLastFrame && num2 < this._boundsHeight / 2f && (this.OneWayPlatformMask.Contains(this.StandingOn.layer) || this.MovingOneWayPlatformMask.Contains(this.StandingOn.layer)))
				{
					this.State.IsCollidingBelow = false;
					return;
				}
				this.State.IsFalling = false;
				this.State.IsCollidingBelow = true;
				if (this._externalForce.y > 0f && this._velocity.y > 0f)
				{
					this._newPosition.y = this._velocity.y * Time.deltaTime;
					this.State.IsCollidingBelow = false;
				}
				else
				{
					float num4 = MMMaths.DistanceBetweenPointAndLine(this._belowHitsStorage[num3].point, this._verticalRayCastFromLeft, this._verticalRayCastToRight);
					this._newPosition.y = -num4 + this._boundsHeight / 2f + this.HorizontalRayOffset;
				}
				if (!this.State.WasGroundedLastFrame && this._velocity.y > 0f)
				{
					this._newPosition.y = this._newPosition.y + this._velocity.y * Time.deltaTime;
				}
				if (Mathf.Abs(this._newPosition.y) < 0.0001f)
				{
					this._newPosition.y = 0f;
				}
				this._movingPlatformTest = this._belowHitsStorage[num3].collider.gameObject.GetComponentNoAlloc<MMPathMovement>();
				if (this._movingPlatformTest != null && this.State.IsGrounded)
				{
					this._movingPlatform = this._movingPlatformTest.GetComponent<MMPathMovement>();
				}
				else
				{
					this.DetachFromMovingPlatform();
				}
			}
			else
			{
				this.State.IsCollidingBelow = false;
				if (this.State.OnAMovingPlatform)
				{
					this.DetachFromMovingPlatform();
				}
			}
			if (this.StickWhenWalkingDownSlopes)
			{
				this.StickToSlope();
			}
		}

		// Token: 0x06006FD0 RID: 28624 RVA: 0x0018F920 File Offset: 0x0018DB20
		protected virtual void CastRaysAbove()
		{
			if (this._newPosition.y < 0f)
			{
				return;
			}
			float num = this.State.IsGrounded ? this.HorizontalRayOffset : this._newPosition.y;
			num += this._boundsHeight / 2f;
			bool flag = false;
			this._aboveRayCastStart = (this._boundsBottomLeftCorner + this._boundsTopLeftCorner) / 2f;
			this._aboveRayCastEnd = (this._boundsBottomRightCorner + this._boundsTopRightCorner) / 2f;
			this._aboveRayCastStart += base.transform.right * this._newPosition.x;
			this._aboveRayCastEnd += base.transform.right * this._newPosition.x;
			if (this._aboveHitsStorage.Length != this.NumberOfVerticalRays)
			{
				this._aboveHitsStorage = new RaycastHit2D[this.NumberOfVerticalRays];
			}
			float num2 = float.MaxValue;
			for (int i = 0; i < this.NumberOfVerticalRays; i++)
			{
				Vector2 rayOriginPoint = Vector2.Lerp(this._aboveRayCastStart, this._aboveRayCastEnd, (float)i / (float)(this.NumberOfVerticalRays - 1));
				this._aboveHitsStorage[i] = MMDebug.RayCast(rayOriginPoint, base.transform.up, num, this.PlatformMask & ~this.OneWayPlatformMask & ~this.MovingOneWayPlatformMask, Colors.Cyan, this.Parameters.DrawRaycastsGizmos);
				if (this._aboveHitsStorage[i])
				{
					flag = true;
					if (this._aboveHitsStorage[i].collider == this._ignoredCollider)
					{
						break;
					}
					if (this._aboveHitsStorage[i].distance < num2)
					{
						num2 = this._aboveHitsStorage[i].distance;
					}
				}
			}
			if (flag)
			{
				this._newPosition.y = num2 - this._boundsHeight / 2f;
				if (this.State.IsGrounded && this._newPosition.y < 0f)
				{
					this._newPosition.y = 0f;
				}
				this.State.IsCollidingAbove = true;
				if (!this.State.WasTouchingTheCeilingLastFrame)
				{
					this._velocity = new Vector2(this._velocity.x, 0f);
				}
			}
		}

		// Token: 0x06006FD1 RID: 28625 RVA: 0x0018FBAC File Offset: 0x0018DDAC
		protected virtual void StickToSlope()
		{
			if (this._newPosition.y >= 0f || !this.StickWhenWalkingDownSlopes || !this.State.WasGroundedLastFrame || this._externalForce.y > 0f || this._movingPlatform != null)
			{
				return;
			}
			float num;
			if (this.StickyRaycastLength == 0f)
			{
				num = this._boundsWidth * Mathf.Abs(Mathf.Tan(this.Parameters.MaximumSlopeAngle));
				num += this._boundsHeight / 2f + this.HorizontalRayOffset;
			}
			else
			{
				num = this.StickyRaycastLength;
			}
			this._stickRayCastOrigin.x = ((this._newPosition.x > 0f) ? this._boundsBottomLeftCorner.x : this._boundsTopRightCorner.x);
			this._stickRayCastOrigin.x = this._stickRayCastOrigin.x + this._newPosition.x;
			this._stickRayCastOrigin.y = this._boundsCenter.y + this.HorizontalRayOffset;
			this._stickRaycast = MMDebug.RayCast(this._stickRayCastOrigin, -base.transform.up, num, this.PlatformMask, Colors.LightBlue, this.Parameters.DrawRaycastsGizmos);
			if (this._stickRaycast)
			{
				if (this._stickRaycast.collider == this._ignoredCollider)
				{
					return;
				}
				this._newPosition.y = -Mathf.Abs(this._stickRaycast.point.y - this._stickRayCastOrigin.y) + this._boundsHeight / 2f + this.HorizontalRayOffset;
				this.State.IsCollidingBelow = true;
			}
		}

		// Token: 0x06006FD2 RID: 28626 RVA: 0x0018FD6C File Offset: 0x0018DF6C
		protected virtual void ComputeNewSpeed()
		{
			if (Time.deltaTime > 0f)
			{
				this._velocity = this._newPosition / Time.deltaTime;
			}
			if (this.State.IsGrounded)
			{
				this._velocity.x = this._velocity.x * this.Parameters.SlopeAngleSpeedFactor.Evaluate(Mathf.Abs(this.State.BelowSlopeAngle) * Mathf.Sign(this._velocity.y));
			}
			if (!this.State.OnAMovingPlatform)
			{
				this._velocity.x = Mathf.Clamp(this._velocity.x, -this.Parameters.MaxVelocity.x, this.Parameters.MaxVelocity.x);
				this._velocity.y = Mathf.Clamp(this._velocity.y, -this.Parameters.MaxVelocity.y, this.Parameters.MaxVelocity.y);
			}
		}

		// Token: 0x06006FD3 RID: 28627 RVA: 0x0018FE70 File Offset: 0x0018E070
		protected virtual void SetStates()
		{
			if (!this.State.WasGroundedLastFrame && this.State.IsCollidingBelow)
			{
				this.State.JustGotGrounded = true;
			}
			if (this.State.IsCollidingLeft || this.State.IsCollidingRight || this.State.IsCollidingBelow || this.State.IsCollidingAbove)
			{
				this.OnCorgiColliderHit();
			}
		}

		// Token: 0x06006FD4 RID: 28628 RVA: 0x0018FEE0 File Offset: 0x0018E0E0
		public virtual void SetRaysParameters()
		{
			Vector2 offset = this._boxCollider.offset;
			Vector2 size = this._boxCollider.size;
			float y = offset.y + size.y / 2f;
			float y2 = offset.y - size.y / 2f;
			float x = offset.x - size.x / 2f;
			float x2 = offset.x + size.x / 2f;
			this._boundsTopLeftCorner.x = x;
			this._boundsTopLeftCorner.y = y;
			this._boundsTopRightCorner.x = x2;
			this._boundsTopRightCorner.y = y;
			this._boundsBottomLeftCorner.x = x;
			this._boundsBottomLeftCorner.y = y2;
			this._boundsBottomRightCorner.x = x2;
			this._boundsBottomRightCorner.y = y2;
			this._boundsTopLeftCorner = base.transform.TransformPoint(this._boundsTopLeftCorner);
			this._boundsTopRightCorner = base.transform.TransformPoint(this._boundsTopRightCorner);
			this._boundsBottomLeftCorner = base.transform.TransformPoint(this._boundsBottomLeftCorner);
			this._boundsBottomRightCorner = base.transform.TransformPoint(this._boundsBottomRightCorner);
			this._boundsCenter = (this._boundsTopRightCorner + this._boundsBottomLeftCorner) / 2f;
			this._boundsWidth = Vector2.Distance(this._boundsBottomLeftCorner, this._boundsBottomRightCorner);
			this._boundsHeight = Vector2.Distance(this._boundsBottomLeftCorner, this._boundsTopLeftCorner);
		}

		// Token: 0x06006FD5 RID: 28629 RVA: 0x0003DB7A File Offset: 0x0003BD7A
		public virtual void SetIgnoreCollider(Collider2D newIgnoredCollider)
		{
			this._ignoredCollider = newIgnoredCollider;
		}

		// Token: 0x06006FD6 RID: 28630 RVA: 0x0003DB83 File Offset: 0x0003BD83
		public virtual IEnumerator DisableCollisions(float duration)
		{
			if (this.m_waitYield == null)
			{
				this.m_waitYield = new WaitRL_Yield(duration, false);
			}
			else
			{
				this.m_waitYield.CreateNew(duration, false);
			}
			this.CollisionsOff();
			yield return this.m_waitYield;
			this.CollisionsOn();
			yield break;
		}

		// Token: 0x06006FD7 RID: 28631 RVA: 0x0019008C File Offset: 0x0018E28C
		public virtual void CollisionsOn()
		{
			this.PlatformMask = this._platformMaskSave;
			this.PlatformMask |= this.OneWayPlatformMask;
			this.PlatformMask |= this.MovingPlatformMask;
			this.PlatformMask |= this.MovingOneWayPlatformMask;
			this.PlatformMask |= this.MidHeightOneWayPlatformMask;
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x0003DB99 File Offset: 0x0003BD99
		public virtual void CollisionsOff()
		{
			this.PlatformMask = 0;
		}

		// Token: 0x06006FD9 RID: 28633 RVA: 0x0003DBA7 File Offset: 0x0003BDA7
		public virtual IEnumerator DisableCollisionsWithOneWayPlatforms(float duration)
		{
			if (this.DetachmentMethod == CorgiController.DetachmentMethods.Layer)
			{
				this.CollisionsOffWithOneWayPlatformsLayer();
				if (this.m_oneWayWaitYield == null)
				{
					this.m_oneWayWaitYield = new WaitRL_Yield(duration, false);
				}
				else
				{
					this.m_oneWayWaitYield.CreateNew(duration, false);
				}
				yield return this.m_oneWayWaitYield;
				this.CollisionsOn();
			}
			else
			{
				this.SetIgnoreCollider(this.StandingOnCollider);
				if (this.m_oneWayWaitYield == null)
				{
					this.m_oneWayWaitYield = new WaitRL_Yield(duration, false);
				}
				else
				{
					this.m_oneWayWaitYield.CreateNew(duration, false);
				}
				yield return this.m_oneWayWaitYield;
				this.SetIgnoreCollider(null);
			}
			yield break;
		}

		// Token: 0x06006FDA RID: 28634 RVA: 0x0003DBBD File Offset: 0x0003BDBD
		public virtual IEnumerator DisableCollisionsWithMovingPlatforms(float duration)
		{
			if (this.DetachmentMethod == CorgiController.DetachmentMethods.Layer)
			{
				this.CollisionsOffWithMovingPlatformsLayer();
				if (this.m_platformWaitYield == null)
				{
					this.m_platformWaitYield = new WaitRL_Yield(duration, false);
				}
				else
				{
					this.m_platformWaitYield.CreateNew(duration, false);
				}
				yield return this.m_platformWaitYield;
				this.CollisionsOn();
			}
			else
			{
				this.SetIgnoreCollider(this.StandingOnCollider);
				if (this.m_platformWaitYield == null)
				{
					this.m_platformWaitYield = new WaitRL_Yield(duration, false);
				}
				else
				{
					this.m_platformWaitYield.CreateNew(duration, false);
				}
				yield return this.m_platformWaitYield;
				this.SetIgnoreCollider(null);
			}
			yield break;
		}

		// Token: 0x06006FDB RID: 28635 RVA: 0x00190130 File Offset: 0x0018E330
		public virtual void CollisionsOffWithOneWayPlatformsLayer()
		{
			this.PlatformMask -= this.OneWayPlatformMask;
			this.PlatformMask -= this.MovingOneWayPlatformMask;
			this.PlatformMask -= this.MidHeightOneWayPlatformMask;
		}

		// Token: 0x06006FDC RID: 28636 RVA: 0x001901A4 File Offset: 0x0018E3A4
		public virtual void CollisionsOffWithMovingPlatformsLayer()
		{
			this.PlatformMask -= this.MovingPlatformMask;
			this.PlatformMask -= this.MovingOneWayPlatformMask;
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x0003DBD3 File Offset: 0x0003BDD3
		public virtual void ResetParameters()
		{
			this._overrideParameters = this.DefaultParameters;
		}

		// Token: 0x06006FDE RID: 28638 RVA: 0x0003DBE1 File Offset: 0x0003BDE1
		public virtual void SlowFall(float factor)
		{
			this._fallSlowFactor = factor;
		}

		// Token: 0x06006FDF RID: 28639 RVA: 0x0003DBEA File Offset: 0x0003BDEA
		public virtual void GravityActive(bool state)
		{
			if (state)
			{
				this._gravityActive = true;
				return;
			}
			this._gravityActive = false;
		}

		// Token: 0x06006FE0 RID: 28640 RVA: 0x001901F8 File Offset: 0x0018E3F8
		public virtual void ResizeCollider(Vector2 newSize)
		{
			float d = this._originalColliderOffset.y - (this._originalColliderSize.y - newSize.y) / 2f;
			this._boxCollider.size = newSize;
			this._boxCollider.offset = d * Vector3.up;
			this.SetRaysParameters();
			this.State.ColliderResized = true;
		}

		// Token: 0x06006FE1 RID: 28641 RVA: 0x0003DBFE File Offset: 0x0003BDFE
		public virtual void ResetColliderSize()
		{
			this._boxCollider.size = this._originalColliderSize;
			this._boxCollider.offset = this._originalColliderOffset;
			this.SetRaysParameters();
			this.State.ColliderResized = false;
		}

		// Token: 0x06006FE2 RID: 28642 RVA: 0x00190264 File Offset: 0x0018E464
		public virtual bool CanGoBackToOriginalSize()
		{
			if (this._boxCollider.size == this._originalColliderSize)
			{
				return true;
			}
			float rayDistance = this._originalColliderSize.y * base.transform.localScale.y * this.CrouchedRaycastLengthMultiplier;
			this._originalSizeRaycastOrigin = this._boundsTopLeftCorner + base.transform.up * 0.0001f;
			bool flag = MMDebug.RayCast(this._originalSizeRaycastOrigin, base.transform.up, rayDistance, this.PlatformMask - this.OneWayPlatformMask, Colors.LightSlateGray, true);
			this._originalSizeRaycastOrigin = this._boundsTopRightCorner + base.transform.up * 0.0001f;
			bool flag2 = MMDebug.RayCast(this._originalSizeRaycastOrigin, base.transform.up, rayDistance, this.PlatformMask - this.OneWayPlatformMask, Colors.LightSlateGray, true);
			return !flag && !flag2;
		}

		// Token: 0x06006FE3 RID: 28643 RVA: 0x0000360E File Offset: 0x0000180E
		public virtual float Width()
		{
			return this._boundsWidth;
		}

		// Token: 0x06006FE4 RID: 28644 RVA: 0x00003616 File Offset: 0x00001816
		public virtual float Height()
		{
			return this._boundsHeight;
		}

		// Token: 0x06006FE5 RID: 28645 RVA: 0x00190394 File Offset: 0x0018E594
		protected virtual void OnCorgiColliderHit()
		{
			foreach (RaycastHit2D raycastHit2D in this._contactList)
			{
				if (this.Parameters.Physics2DInteraction)
				{
					Rigidbody2D attachedRigidbody = raycastHit2D.collider.attachedRigidbody;
					if (attachedRigidbody == null || attachedRigidbody.isKinematic)
					{
						break;
					}
					Vector3 vector = new Vector3(this._externalForce.x, 0f, 0f);
					attachedRigidbody.velocity = vector.normalized * this.Parameters.Physics2DPushForce;
				}
			}
		}

		// Token: 0x06006FE6 RID: 28646 RVA: 0x0019044C File Offset: 0x0018E64C
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			CorgiControllerPhysicsVolume2D componentNoAlloc = collider.gameObject.GetComponentNoAlloc<CorgiControllerPhysicsVolume2D>();
			if (componentNoAlloc != null)
			{
				this._overrideParameters = componentNoAlloc.ControllerParameters;
				if (componentNoAlloc.ResetForcesOnEntry)
				{
					this.SetForce(Vector2.zero);
				}
				if (componentNoAlloc.MultiplyForcesOnEntry)
				{
					this.SetForce(Vector2.Scale(componentNoAlloc.ForceMultiplierOnEntry, this.Velocity));
				}
			}
		}

		// Token: 0x06006FE7 RID: 28647 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnTriggerStay2D(Collider2D collider)
		{
		}

		// Token: 0x06006FE8 RID: 28648 RVA: 0x0003DC34 File Offset: 0x0003BE34
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			if (collider.gameObject.GetComponentNoAlloc<CorgiControllerPhysicsVolume2D>() != null)
			{
				this._overrideParameters = null;
			}
		}

		// Token: 0x040059D4 RID: 22996
		[Header("Default Parameters")]
		public CorgiControllerParameters DefaultParameters;

		// Token: 0x040059D5 RID: 22997
		[Header("Collision Masks")]
		[Information("You need to define what layer(s) this character will consider a walkable platform/moving platform etc. By default, you want Platforms, MovingPlatforms, OneWayPlatforms, MovingOneWayPlatforms, in this order.", InformationAttribute.InformationType.Info, false)]
		public LayerMask PlatformMask = 0;

		// Token: 0x040059D6 RID: 22998
		public LayerMask MovingPlatformMask = 0;

		// Token: 0x040059D7 RID: 22999
		public LayerMask OneWayPlatformMask = 0;

		// Token: 0x040059D8 RID: 23000
		public LayerMask MovingOneWayPlatformMask = 0;

		// Token: 0x040059D9 RID: 23001
		public LayerMask MidHeightOneWayPlatformMask = 0;

		// Token: 0x040059DA RID: 23002
		public CorgiController.DetachmentMethods DetachmentMethod;

		// Token: 0x040059DB RID: 23003
		[ReadOnly]
		public GameObject StandingOn;

		// Token: 0x040059E0 RID: 23008
		[Header("Raycasting")]
		[Information("Here you can define how many rays are cast horizontally and vertically. You'll want them as far as possible from each other, but close enough that no obstacle or enemy can fit between 2 rays.", InformationAttribute.InformationType.Info, false)]
		public int NumberOfHorizontalRays = 4;

		// Token: 0x040059E1 RID: 23009
		public int NumberOfVerticalRays = 4;

		// Token: 0x040059E2 RID: 23010
		public float HorizontalRayOffset = 0.05f;

		// Token: 0x040059E3 RID: 23011
		[Tooltip("Honestly don't know what this is for. If character is not hooking to slopes when running up them, tweak this number until they do.")]
		public float VerticalRayOffset;

		// Token: 0x040059E4 RID: 23012
		public float CrouchedRaycastLengthMultiplier = 1f;

		// Token: 0x040059E5 RID: 23013
		public bool CastRaysOnBothSides = true;

		// Token: 0x040059E6 RID: 23014
		[Header("Stickiness")]
		[Information("Here you can define whether or not you want your character stick to slopes when walking down them, and how long the raycast handling that should be (0 means automatic length).", InformationAttribute.InformationType.Info, false)]
		public bool StickWhenWalkingDownSlopes = true;

		// Token: 0x040059E7 RID: 23015
		public float StickyRaycastLength = 1.25f;

		// Token: 0x040059EA RID: 23018
		protected CorgiControllerParameters _overrideParameters;

		// Token: 0x040059EB RID: 23019
		protected Vector2 _velocity;

		// Token: 0x040059EC RID: 23020
		protected float _friction;

		// Token: 0x040059ED RID: 23021
		protected float _fallSlowFactor;

		// Token: 0x040059EE RID: 23022
		protected float _currentGravity;

		// Token: 0x040059EF RID: 23023
		protected Vector2 _externalForce;

		// Token: 0x040059F0 RID: 23024
		protected Vector2 _newPosition;

		// Token: 0x040059F1 RID: 23025
		protected Transform _transform;

		// Token: 0x040059F2 RID: 23026
		protected BoxCollider2D _boxCollider;

		// Token: 0x040059F3 RID: 23027
		protected LayerMask _platformMaskSave;

		// Token: 0x040059F4 RID: 23028
		protected LayerMask _raysBelowLayerMaskPlatforms;

		// Token: 0x040059F5 RID: 23029
		protected LayerMask _raysBelowLayerMaskPlatformsWithoutOneWay;

		// Token: 0x040059F6 RID: 23030
		protected LayerMask _raysBelowLayerMaskPlatformsWithoutMidHeight;

		// Token: 0x040059F7 RID: 23031
		protected int _savedBelowLayer;

		// Token: 0x040059F8 RID: 23032
		protected MMPathMovement _movingPlatform;

		// Token: 0x040059F9 RID: 23033
		protected float _movingPlatformCurrentGravity;

		// Token: 0x040059FA RID: 23034
		protected bool _gravityActive = true;

		// Token: 0x040059FB RID: 23035
		protected Collider2D _ignoredCollider;

		// Token: 0x040059FC RID: 23036
		protected const float _smallValue = 0.0001f;

		// Token: 0x040059FD RID: 23037
		protected const float _obstacleHeightTolerance = 0.05f;

		// Token: 0x040059FE RID: 23038
		protected const float _movingPlatformsGravity = -500f;

		// Token: 0x040059FF RID: 23039
		protected Vector2 _originalColliderSize;

		// Token: 0x04005A00 RID: 23040
		protected Vector2 _originalColliderOffset;

		// Token: 0x04005A01 RID: 23041
		protected Vector2 _originalSizeRaycastOrigin;

		// Token: 0x04005A02 RID: 23042
		protected Vector3 _crossBelowSlopeAngle;

		// Token: 0x04005A03 RID: 23043
		protected RaycastHit2D[] _sideHitsStorage;

		// Token: 0x04005A04 RID: 23044
		protected RaycastHit2D[] _belowHitsStorage;

		// Token: 0x04005A05 RID: 23045
		protected RaycastHit2D[] _aboveHitsStorage;

		// Token: 0x04005A06 RID: 23046
		protected RaycastHit2D _stickRaycast;

		// Token: 0x04005A07 RID: 23047
		protected float _movementDirection;

		// Token: 0x04005A08 RID: 23048
		protected float _storedMovementDirection = 1f;

		// Token: 0x04005A09 RID: 23049
		protected const float _movementDirectionThreshold = 0.0001f;

		// Token: 0x04005A0A RID: 23050
		protected Vector2 _horizontalRayCastFromBottom = Vector2.zero;

		// Token: 0x04005A0B RID: 23051
		protected Vector2 _horizontalRayCastToTop = Vector2.zero;

		// Token: 0x04005A0C RID: 23052
		protected Vector2 _verticalRayCastFromLeft = Vector2.zero;

		// Token: 0x04005A0D RID: 23053
		protected Vector2 _verticalRayCastToRight = Vector2.zero;

		// Token: 0x04005A0E RID: 23054
		protected Vector2 _aboveRayCastStart = Vector2.zero;

		// Token: 0x04005A0F RID: 23055
		protected Vector2 _aboveRayCastEnd = Vector2.zero;

		// Token: 0x04005A10 RID: 23056
		protected Vector2 _stickRayCastOrigin = Vector2.zero;

		// Token: 0x04005A11 RID: 23057
		protected Vector3 _colliderBottomCenterPosition;

		// Token: 0x04005A12 RID: 23058
		protected Vector3 _colliderLeftCenterPosition;

		// Token: 0x04005A13 RID: 23059
		protected Vector3 _colliderRightCenterPosition;

		// Token: 0x04005A14 RID: 23060
		protected Vector3 _colliderTopCenterPosition;

		// Token: 0x04005A15 RID: 23061
		protected MMPathMovement _movingPlatformTest;

		// Token: 0x04005A16 RID: 23062
		protected RaycastHit2D[] _raycastNonAlloc = new RaycastHit2D[0];

		// Token: 0x04005A17 RID: 23063
		protected Vector2 _boundsTopLeftCorner;

		// Token: 0x04005A18 RID: 23064
		protected Vector2 _boundsBottomLeftCorner;

		// Token: 0x04005A19 RID: 23065
		protected Vector2 _boundsTopRightCorner;

		// Token: 0x04005A1A RID: 23066
		protected Vector2 _boundsBottomRightCorner;

		// Token: 0x04005A1B RID: 23067
		protected Vector2 _boundsCenter;

		// Token: 0x04005A1C RID: 23068
		protected float _boundsWidth;

		// Token: 0x04005A1D RID: 23069
		protected float _boundsHeight;

		// Token: 0x04005A1E RID: 23070
		protected List<RaycastHit2D> _contactList;

		// Token: 0x04005A1F RID: 23071
		protected bool m_savePlatformMaskSet;

		// Token: 0x04005A20 RID: 23072
		private WaitRL_Yield m_waitYield;

		// Token: 0x04005A21 RID: 23073
		private WaitRL_Yield m_oneWayWaitYield;

		// Token: 0x04005A22 RID: 23074
		private WaitRL_Yield m_platformWaitYield;

		// Token: 0x02000F1E RID: 3870
		public enum RaycastDirections
		{
			// Token: 0x04005A24 RID: 23076
			up,
			// Token: 0x04005A25 RID: 23077
			down,
			// Token: 0x04005A26 RID: 23078
			left,
			// Token: 0x04005A27 RID: 23079
			right
		}

		// Token: 0x02000F1F RID: 3871
		public enum DetachmentMethods
		{
			// Token: 0x04005A29 RID: 23081
			Layer,
			// Token: 0x04005A2A RID: 23082
			Object
		}
	}
}

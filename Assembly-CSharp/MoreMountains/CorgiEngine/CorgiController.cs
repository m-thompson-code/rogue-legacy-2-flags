using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x0200096D RID: 2413
	[AddComponentMenu("Corgi Engine/Character/Core/Corgi Controller")]
	public class CorgiController : MonoBehaviour
	{
		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x00120183 File Offset: 0x0011E383
		// (set) Token: 0x06005198 RID: 20888 RVA: 0x0012018B File Offset: 0x0011E38B
		public CorgiControllerState State { get; protected set; }

		// Token: 0x17001AFB RID: 6907
		// (get) Token: 0x06005199 RID: 20889 RVA: 0x00120194 File Offset: 0x0011E394
		public CorgiControllerParameters Parameters
		{
			get
			{
				return this._overrideParameters ?? this.DefaultParameters;
			}
		}

		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x0600519A RID: 20890 RVA: 0x001201A6 File Offset: 0x0011E3A6
		// (set) Token: 0x0600519B RID: 20891 RVA: 0x001201AE File Offset: 0x0011E3AE
		public GameObject StandingOnLastFrame { get; protected set; }

		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x0600519C RID: 20892 RVA: 0x001201B7 File Offset: 0x0011E3B7
		// (set) Token: 0x0600519D RID: 20893 RVA: 0x001201BF File Offset: 0x0011E3BF
		public Collider2D StandingOnCollider { get; protected set; }

		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x0600519E RID: 20894 RVA: 0x001201C8 File Offset: 0x0011E3C8
		public Vector2 Velocity
		{
			get
			{
				return this._velocity;
			}
		}

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x0600519F RID: 20895 RVA: 0x001201D0 File Offset: 0x0011E3D0
		// (set) Token: 0x060051A0 RID: 20896 RVA: 0x001201D8 File Offset: 0x0011E3D8
		public Vector2 ForcesApplied { get; protected set; }

		// Token: 0x17001B00 RID: 6912
		// (get) Token: 0x060051A1 RID: 20897 RVA: 0x001201E1 File Offset: 0x0011E3E1
		// (set) Token: 0x060051A2 RID: 20898 RVA: 0x001201E9 File Offset: 0x0011E3E9
		public GameObject CurrentWallCollider { get; protected set; }

		// Token: 0x17001B01 RID: 6913
		// (get) Token: 0x060051A3 RID: 20899 RVA: 0x001201F2 File Offset: 0x0011E3F2
		public Vector3 ColliderSize
		{
			get
			{
				return Vector3.Scale(base.transform.localScale, this._boxCollider.size);
			}
		}

		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x060051A4 RID: 20900 RVA: 0x00120214 File Offset: 0x0011E414
		public Vector3 ColliderCenterPosition
		{
			get
			{
				return this._boxCollider.bounds.center;
			}
		}

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x060051A5 RID: 20901 RVA: 0x00120234 File Offset: 0x0011E434
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

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x060051A6 RID: 20902 RVA: 0x001202A0 File Offset: 0x0011E4A0
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

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x060051A7 RID: 20903 RVA: 0x0012030C File Offset: 0x0011E50C
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

		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x060051A8 RID: 20904 RVA: 0x00120378 File Offset: 0x0011E578
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

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x060051A9 RID: 20905 RVA: 0x001203E1 File Offset: 0x0011E5E1
		public float Friction
		{
			get
			{
				return this._friction;
			}
		}

		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x060051AA RID: 20906 RVA: 0x001203E9 File Offset: 0x0011E5E9
		public virtual Vector3 BoundsTopLeftCorner
		{
			get
			{
				return this._boundsTopLeftCorner;
			}
		}

		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x060051AB RID: 20907 RVA: 0x001203F6 File Offset: 0x0011E5F6
		public virtual Vector3 BoundsBottomLeftCorner
		{
			get
			{
				return this._boundsBottomLeftCorner;
			}
		}

		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x060051AC RID: 20908 RVA: 0x00120403 File Offset: 0x0011E603
		public virtual Vector3 BoundsTopRightCorner
		{
			get
			{
				return this._boundsTopRightCorner;
			}
		}

		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x060051AD RID: 20909 RVA: 0x00120410 File Offset: 0x0011E610
		public virtual Vector3 BoundsBottomRightCorner
		{
			get
			{
				return this._boundsBottomRightCorner;
			}
		}

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x060051AE RID: 20910 RVA: 0x0012041D File Offset: 0x0011E61D
		public virtual Vector3 BoundsTop
		{
			get
			{
				return (this._boundsTopLeftCorner + this._boundsTopRightCorner) / 2f;
			}
		}

		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x060051AF RID: 20911 RVA: 0x0012043F File Offset: 0x0011E63F
		public virtual Vector3 BoundsBottom
		{
			get
			{
				return (this._boundsBottomLeftCorner + this._boundsBottomRightCorner) / 2f;
			}
		}

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x060051B0 RID: 20912 RVA: 0x00120461 File Offset: 0x0011E661
		public virtual Vector3 BoundsRight
		{
			get
			{
				return (this._boundsTopRightCorner + this._boundsBottomRightCorner) / 2f;
			}
		}

		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x060051B1 RID: 20913 RVA: 0x00120483 File Offset: 0x0011E683
		public virtual Vector3 BoundsLeft
		{
			get
			{
				return (this._boundsTopLeftCorner + this._boundsBottomLeftCorner) / 2f;
			}
		}

		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x060051B2 RID: 20914 RVA: 0x001204A5 File Offset: 0x0011E6A5
		public virtual Vector3 BoundsCenter
		{
			get
			{
				return this._boundsCenter;
			}
		}

		// Token: 0x17001B11 RID: 6929
		// (get) Token: 0x060051B3 RID: 20915 RVA: 0x001204B2 File Offset: 0x0011E6B2
		public LayerMask SavedPlatformMask
		{
			get
			{
				return this._platformMaskSave;
			}
		}

		// Token: 0x17001B12 RID: 6930
		// (get) Token: 0x060051B4 RID: 20916 RVA: 0x001204BA File Offset: 0x0011E6BA
		// (set) Token: 0x060051B5 RID: 20917 RVA: 0x001204C2 File Offset: 0x0011E6C2
		public bool IsWithinJumpLeeway { get; protected set; }

		// Token: 0x17001B13 RID: 6931
		// (get) Token: 0x060051B6 RID: 20918 RVA: 0x001204CB File Offset: 0x0011E6CB
		// (set) Token: 0x060051B7 RID: 20919 RVA: 0x001204D3 File Offset: 0x0011E6D3
		public bool IsInitialized { get; protected set; }

		// Token: 0x060051B8 RID: 20920 RVA: 0x001204DC File Offset: 0x0011E6DC
		protected virtual void Awake()
		{
			this.SavePlatformMask();
			this.PlatformMask |= this.OneWayPlatformMask;
			this.PlatformMask |= this.MovingPlatformMask;
			this.PlatformMask |= this.MovingOneWayPlatformMask;
			this.PlatformMask |= this.MidHeightOneWayPlatformMask;
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00120577 File Offset: 0x0011E777
		public void SavePlatformMask()
		{
			if (!this.m_savePlatformMaskSet)
			{
				this._platformMaskSave = this.PlatformMask;
				this.m_savePlatformMaskSet = true;
			}
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x00120594 File Offset: 0x0011E794
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x0012059C File Offset: 0x0011E79C
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

		// Token: 0x060051BC RID: 20924 RVA: 0x00120687 File Offset: 0x0011E887
		public virtual void AddForce(Vector2 force)
		{
			this._velocity += force;
			this._externalForce += force;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x001206AD File Offset: 0x0011E8AD
		public virtual void AddHorizontalForce(float x)
		{
			this._velocity.x = this._velocity.x + x;
			this._externalForce.x = this._externalForce.x + x;
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x001206CF File Offset: 0x0011E8CF
		public virtual void AddVerticalForce(float y)
		{
			this._velocity.y = this._velocity.y + y;
			this._externalForce.y = this._externalForce.y + y;
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x001206F1 File Offset: 0x0011E8F1
		public virtual void SetForce(Vector2 force)
		{
			this._velocity = force;
			this._externalForce = force;
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x00120701 File Offset: 0x0011E901
		public virtual void SetHorizontalForce(float x)
		{
			this._velocity.x = x;
			this._externalForce.x = x;
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x0012071B File Offset: 0x0011E91B
		public virtual void SetVerticalForce(float y)
		{
			this._velocity.y = y;
			this._externalForce.y = y;
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x00120735 File Offset: 0x0011E935
		protected virtual void LateUpdate()
		{
			this.EveryFrame();
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x00120740 File Offset: 0x0011E940
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

		// Token: 0x060051C4 RID: 20932 RVA: 0x00120804 File Offset: 0x0011EA04
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

		// Token: 0x060051C5 RID: 20933 RVA: 0x0012087C File Offset: 0x0011EA7C
		protected virtual void FrameExit()
		{
			if (this.StandingOnLastFrame)
			{
				this.StandingOnLastFrame.layer = this._savedBelowLayer;
			}
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x0012089C File Offset: 0x0011EA9C
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

		// Token: 0x060051C7 RID: 20935 RVA: 0x00120924 File Offset: 0x0011EB24
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

		// Token: 0x060051C8 RID: 20936 RVA: 0x001209D4 File Offset: 0x0011EBD4
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

		// Token: 0x060051C9 RID: 20937 RVA: 0x00120B2D File Offset: 0x0011ED2D
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

		// Token: 0x060051CA RID: 20938 RVA: 0x00120B64 File Offset: 0x0011ED64
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

		// Token: 0x060051CB RID: 20939 RVA: 0x0012118F File Offset: 0x0011F38F
		protected virtual void CastRaysToTheLeft()
		{
			this.CastRaysToTheSides(-1f);
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x0012119C File Offset: 0x0011F39C
		protected virtual void CastRaysToTheRight()
		{
			this.CastRaysToTheSides(1f);
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x001211AC File Offset: 0x0011F3AC
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

		// Token: 0x060051CE RID: 20942 RVA: 0x0012158C File Offset: 0x0011F78C
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

		// Token: 0x060051CF RID: 20943 RVA: 0x00121C9C File Offset: 0x0011FE9C
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

		// Token: 0x060051D0 RID: 20944 RVA: 0x00121F28 File Offset: 0x00120128
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

		// Token: 0x060051D1 RID: 20945 RVA: 0x001220E8 File Offset: 0x001202E8
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

		// Token: 0x060051D2 RID: 20946 RVA: 0x001221EC File Offset: 0x001203EC
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

		// Token: 0x060051D3 RID: 20947 RVA: 0x0012225C File Offset: 0x0012045C
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

		// Token: 0x060051D4 RID: 20948 RVA: 0x00122405 File Offset: 0x00120605
		public virtual void SetIgnoreCollider(Collider2D newIgnoredCollider)
		{
			this._ignoredCollider = newIgnoredCollider;
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x0012240E File Offset: 0x0012060E
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

		// Token: 0x060051D6 RID: 20950 RVA: 0x00122424 File Offset: 0x00120624
		public virtual void CollisionsOn()
		{
			this.PlatformMask = this._platformMaskSave;
			this.PlatformMask |= this.OneWayPlatformMask;
			this.PlatformMask |= this.MovingPlatformMask;
			this.PlatformMask |= this.MovingOneWayPlatformMask;
			this.PlatformMask |= this.MidHeightOneWayPlatformMask;
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x001224C5 File Offset: 0x001206C5
		public virtual void CollisionsOff()
		{
			this.PlatformMask = 0;
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x001224D3 File Offset: 0x001206D3
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

		// Token: 0x060051D9 RID: 20953 RVA: 0x001224E9 File Offset: 0x001206E9
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

		// Token: 0x060051DA RID: 20954 RVA: 0x00122500 File Offset: 0x00120700
		public virtual void CollisionsOffWithOneWayPlatformsLayer()
		{
			this.PlatformMask -= this.OneWayPlatformMask;
			this.PlatformMask -= this.MovingOneWayPlatformMask;
			this.PlatformMask -= this.MidHeightOneWayPlatformMask;
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x00122574 File Offset: 0x00120774
		public virtual void CollisionsOffWithMovingPlatformsLayer()
		{
			this.PlatformMask -= this.MovingPlatformMask;
			this.PlatformMask -= this.MovingOneWayPlatformMask;
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x001225C5 File Offset: 0x001207C5
		public virtual void ResetParameters()
		{
			this._overrideParameters = this.DefaultParameters;
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x001225D3 File Offset: 0x001207D3
		public virtual void SlowFall(float factor)
		{
			this._fallSlowFactor = factor;
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x001225DC File Offset: 0x001207DC
		public virtual void GravityActive(bool state)
		{
			if (state)
			{
				this._gravityActive = true;
				return;
			}
			this._gravityActive = false;
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x001225F0 File Offset: 0x001207F0
		public virtual void ResizeCollider(Vector2 newSize)
		{
			float d = this._originalColliderOffset.y - (this._originalColliderSize.y - newSize.y) / 2f;
			this._boxCollider.size = newSize;
			this._boxCollider.offset = d * Vector3.up;
			this.SetRaysParameters();
			this.State.ColliderResized = true;
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x0012265B File Offset: 0x0012085B
		public virtual void ResetColliderSize()
		{
			this._boxCollider.size = this._originalColliderSize;
			this._boxCollider.offset = this._originalColliderOffset;
			this.SetRaysParameters();
			this.State.ColliderResized = false;
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x00122694 File Offset: 0x00120894
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

		// Token: 0x060051E2 RID: 20962 RVA: 0x001227C3 File Offset: 0x001209C3
		public virtual float Width()
		{
			return this._boundsWidth;
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x001227CB File Offset: 0x001209CB
		public virtual float Height()
		{
			return this._boundsHeight;
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x001227D4 File Offset: 0x001209D4
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

		// Token: 0x060051E5 RID: 20965 RVA: 0x0012288C File Offset: 0x00120A8C
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

		// Token: 0x060051E6 RID: 20966 RVA: 0x001228EC File Offset: 0x00120AEC
		protected virtual void OnTriggerStay2D(Collider2D collider)
		{
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x001228EE File Offset: 0x00120AEE
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			if (collider.gameObject.GetComponentNoAlloc<CorgiControllerPhysicsVolume2D>() != null)
			{
				this._overrideParameters = null;
			}
		}

		// Token: 0x040043A2 RID: 17314
		[Header("Default Parameters")]
		public CorgiControllerParameters DefaultParameters;

		// Token: 0x040043A3 RID: 17315
		[Header("Collision Masks")]
		[Information("You need to define what layer(s) this character will consider a walkable platform/moving platform etc. By default, you want Platforms, MovingPlatforms, OneWayPlatforms, MovingOneWayPlatforms, in this order.", InformationAttribute.InformationType.Info, false)]
		public LayerMask PlatformMask = 0;

		// Token: 0x040043A4 RID: 17316
		public LayerMask MovingPlatformMask = 0;

		// Token: 0x040043A5 RID: 17317
		public LayerMask OneWayPlatformMask = 0;

		// Token: 0x040043A6 RID: 17318
		public LayerMask MovingOneWayPlatformMask = 0;

		// Token: 0x040043A7 RID: 17319
		public LayerMask MidHeightOneWayPlatformMask = 0;

		// Token: 0x040043A8 RID: 17320
		public CorgiController.DetachmentMethods DetachmentMethod;

		// Token: 0x040043A9 RID: 17321
		[ReadOnly]
		public GameObject StandingOn;

		// Token: 0x040043AE RID: 17326
		[Header("Raycasting")]
		[Information("Here you can define how many rays are cast horizontally and vertically. You'll want them as far as possible from each other, but close enough that no obstacle or enemy can fit between 2 rays.", InformationAttribute.InformationType.Info, false)]
		public int NumberOfHorizontalRays = 4;

		// Token: 0x040043AF RID: 17327
		public int NumberOfVerticalRays = 4;

		// Token: 0x040043B0 RID: 17328
		public float HorizontalRayOffset = 0.05f;

		// Token: 0x040043B1 RID: 17329
		[Tooltip("Honestly don't know what this is for. If character is not hooking to slopes when running up them, tweak this number until they do.")]
		public float VerticalRayOffset;

		// Token: 0x040043B2 RID: 17330
		public float CrouchedRaycastLengthMultiplier = 1f;

		// Token: 0x040043B3 RID: 17331
		public bool CastRaysOnBothSides = true;

		// Token: 0x040043B4 RID: 17332
		[Header("Stickiness")]
		[Information("Here you can define whether or not you want your character stick to slopes when walking down them, and how long the raycast handling that should be (0 means automatic length).", InformationAttribute.InformationType.Info, false)]
		public bool StickWhenWalkingDownSlopes = true;

		// Token: 0x040043B5 RID: 17333
		public float StickyRaycastLength = 1.25f;

		// Token: 0x040043B8 RID: 17336
		protected CorgiControllerParameters _overrideParameters;

		// Token: 0x040043B9 RID: 17337
		protected Vector2 _velocity;

		// Token: 0x040043BA RID: 17338
		protected float _friction;

		// Token: 0x040043BB RID: 17339
		protected float _fallSlowFactor;

		// Token: 0x040043BC RID: 17340
		protected float _currentGravity;

		// Token: 0x040043BD RID: 17341
		protected Vector2 _externalForce;

		// Token: 0x040043BE RID: 17342
		protected Vector2 _newPosition;

		// Token: 0x040043BF RID: 17343
		protected Transform _transform;

		// Token: 0x040043C0 RID: 17344
		protected BoxCollider2D _boxCollider;

		// Token: 0x040043C1 RID: 17345
		protected LayerMask _platformMaskSave;

		// Token: 0x040043C2 RID: 17346
		protected LayerMask _raysBelowLayerMaskPlatforms;

		// Token: 0x040043C3 RID: 17347
		protected LayerMask _raysBelowLayerMaskPlatformsWithoutOneWay;

		// Token: 0x040043C4 RID: 17348
		protected LayerMask _raysBelowLayerMaskPlatformsWithoutMidHeight;

		// Token: 0x040043C5 RID: 17349
		protected int _savedBelowLayer;

		// Token: 0x040043C6 RID: 17350
		protected MMPathMovement _movingPlatform;

		// Token: 0x040043C7 RID: 17351
		protected float _movingPlatformCurrentGravity;

		// Token: 0x040043C8 RID: 17352
		protected bool _gravityActive = true;

		// Token: 0x040043C9 RID: 17353
		protected Collider2D _ignoredCollider;

		// Token: 0x040043CA RID: 17354
		protected const float _smallValue = 0.0001f;

		// Token: 0x040043CB RID: 17355
		protected const float _obstacleHeightTolerance = 0.05f;

		// Token: 0x040043CC RID: 17356
		protected const float _movingPlatformsGravity = -500f;

		// Token: 0x040043CD RID: 17357
		protected Vector2 _originalColliderSize;

		// Token: 0x040043CE RID: 17358
		protected Vector2 _originalColliderOffset;

		// Token: 0x040043CF RID: 17359
		protected Vector2 _originalSizeRaycastOrigin;

		// Token: 0x040043D0 RID: 17360
		protected Vector3 _crossBelowSlopeAngle;

		// Token: 0x040043D1 RID: 17361
		protected RaycastHit2D[] _sideHitsStorage;

		// Token: 0x040043D2 RID: 17362
		protected RaycastHit2D[] _belowHitsStorage;

		// Token: 0x040043D3 RID: 17363
		protected RaycastHit2D[] _aboveHitsStorage;

		// Token: 0x040043D4 RID: 17364
		protected RaycastHit2D _stickRaycast;

		// Token: 0x040043D5 RID: 17365
		protected float _movementDirection;

		// Token: 0x040043D6 RID: 17366
		protected float _storedMovementDirection = 1f;

		// Token: 0x040043D7 RID: 17367
		protected const float _movementDirectionThreshold = 0.0001f;

		// Token: 0x040043D8 RID: 17368
		protected Vector2 _horizontalRayCastFromBottom = Vector2.zero;

		// Token: 0x040043D9 RID: 17369
		protected Vector2 _horizontalRayCastToTop = Vector2.zero;

		// Token: 0x040043DA RID: 17370
		protected Vector2 _verticalRayCastFromLeft = Vector2.zero;

		// Token: 0x040043DB RID: 17371
		protected Vector2 _verticalRayCastToRight = Vector2.zero;

		// Token: 0x040043DC RID: 17372
		protected Vector2 _aboveRayCastStart = Vector2.zero;

		// Token: 0x040043DD RID: 17373
		protected Vector2 _aboveRayCastEnd = Vector2.zero;

		// Token: 0x040043DE RID: 17374
		protected Vector2 _stickRayCastOrigin = Vector2.zero;

		// Token: 0x040043DF RID: 17375
		protected Vector3 _colliderBottomCenterPosition;

		// Token: 0x040043E0 RID: 17376
		protected Vector3 _colliderLeftCenterPosition;

		// Token: 0x040043E1 RID: 17377
		protected Vector3 _colliderRightCenterPosition;

		// Token: 0x040043E2 RID: 17378
		protected Vector3 _colliderTopCenterPosition;

		// Token: 0x040043E3 RID: 17379
		protected MMPathMovement _movingPlatformTest;

		// Token: 0x040043E4 RID: 17380
		protected RaycastHit2D[] _raycastNonAlloc = new RaycastHit2D[0];

		// Token: 0x040043E5 RID: 17381
		protected Vector2 _boundsTopLeftCorner;

		// Token: 0x040043E6 RID: 17382
		protected Vector2 _boundsBottomLeftCorner;

		// Token: 0x040043E7 RID: 17383
		protected Vector2 _boundsTopRightCorner;

		// Token: 0x040043E8 RID: 17384
		protected Vector2 _boundsBottomRightCorner;

		// Token: 0x040043E9 RID: 17385
		protected Vector2 _boundsCenter;

		// Token: 0x040043EA RID: 17386
		protected float _boundsWidth;

		// Token: 0x040043EB RID: 17387
		protected float _boundsHeight;

		// Token: 0x040043EC RID: 17388
		protected List<RaycastHit2D> _contactList;

		// Token: 0x040043ED RID: 17389
		protected bool m_savePlatformMaskSet;

		// Token: 0x040043EE RID: 17390
		private WaitRL_Yield m_waitYield;

		// Token: 0x040043EF RID: 17391
		private WaitRL_Yield m_oneWayWaitYield;

		// Token: 0x040043F0 RID: 17392
		private WaitRL_Yield m_platformWaitYield;

		// Token: 0x02000F1D RID: 3869
		public enum RaycastDirections
		{
			// Token: 0x04005A94 RID: 23188
			up,
			// Token: 0x04005A95 RID: 23189
			down,
			// Token: 0x04005A96 RID: 23190
			left,
			// Token: 0x04005A97 RID: 23191
			right
		}

		// Token: 0x02000F1E RID: 3870
		public enum DetachmentMethods
		{
			// Token: 0x04005A99 RID: 23193
			Layer,
			// Token: 0x04005A9A RID: 23194
			Object
		}
	}
}

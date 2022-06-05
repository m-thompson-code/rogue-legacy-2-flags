using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x0200096E RID: 2414
	[RequireComponent(typeof(Collider2D))]
	[Serializable]
	public class CorgiControllerParameters
	{
		// Token: 0x040043F1 RID: 17393
		[Header("Gravity")]
		public float Gravity = -30f;

		// Token: 0x040043F2 RID: 17394
		public float FallMultiplier = 1f;

		// Token: 0x040043F3 RID: 17395
		public float AscentMultiplier = 1f;

		// Token: 0x040043F4 RID: 17396
		[Header("Speed")]
		public Vector2 MaxVelocity = new Vector2(100f, 100f);

		// Token: 0x040043F5 RID: 17397
		public float SpeedAccelerationOnGround = 20f;

		// Token: 0x040043F6 RID: 17398
		public float SpeedAccelerationInAir = 5f;

		// Token: 0x040043F7 RID: 17399
		public float SpeedFactor = 1f;

		// Token: 0x040043F8 RID: 17400
		[Header("Slopes")]
		[Range(0f, 90f)]
		public float MaximumSlopeAngle = 30f;

		// Token: 0x040043F9 RID: 17401
		public AnimationCurve SlopeAngleSpeedFactor = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(-90f, 1f),
			new Keyframe(0f, 1f),
			new Keyframe(90f, 1f)
		});

		// Token: 0x040043FA RID: 17402
		[Header("Physics2D Interaction [Experimental]")]
		public bool Physics2DInteraction = true;

		// Token: 0x040043FB RID: 17403
		public float Physics2DPushForce = 2f;

		// Token: 0x040043FC RID: 17404
		[Header("Gizmos")]
		public bool DrawRaycastsGizmos = true;

		// Token: 0x040043FD RID: 17405
		public bool DisplayWarnings = true;
	}
}

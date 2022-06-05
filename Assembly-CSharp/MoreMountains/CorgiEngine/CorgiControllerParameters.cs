using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F23 RID: 3875
	[RequireComponent(typeof(Collider2D))]
	[Serializable]
	public class CorgiControllerParameters
	{
		// Token: 0x04005A37 RID: 23095
		[Header("Gravity")]
		public float Gravity = -30f;

		// Token: 0x04005A38 RID: 23096
		public float FallMultiplier = 1f;

		// Token: 0x04005A39 RID: 23097
		public float AscentMultiplier = 1f;

		// Token: 0x04005A3A RID: 23098
		[Header("Speed")]
		public Vector2 MaxVelocity = new Vector2(100f, 100f);

		// Token: 0x04005A3B RID: 23099
		public float SpeedAccelerationOnGround = 20f;

		// Token: 0x04005A3C RID: 23100
		public float SpeedAccelerationInAir = 5f;

		// Token: 0x04005A3D RID: 23101
		public float SpeedFactor = 1f;

		// Token: 0x04005A3E RID: 23102
		[Header("Slopes")]
		[Range(0f, 90f)]
		public float MaximumSlopeAngle = 30f;

		// Token: 0x04005A3F RID: 23103
		public AnimationCurve SlopeAngleSpeedFactor = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(-90f, 1f),
			new Keyframe(0f, 1f),
			new Keyframe(90f, 1f)
		});

		// Token: 0x04005A40 RID: 23104
		[Header("Physics2D Interaction [Experimental]")]
		public bool Physics2DInteraction = true;

		// Token: 0x04005A41 RID: 23105
		public float Physics2DPushForce = 2f;

		// Token: 0x04005A42 RID: 23106
		[Header("Gizmos")]
		public bool DrawRaycastsGizmos = true;

		// Token: 0x04005A43 RID: 23107
		public bool DisplayWarnings = true;
	}
}

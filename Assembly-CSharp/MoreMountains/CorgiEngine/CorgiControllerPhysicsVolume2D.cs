using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000972 RID: 2418
	[AddComponentMenu("Corgi Engine/Environment/Corgi Controller Override")]
	public class CorgiControllerPhysicsVolume2D : MonoBehaviour
	{
		// Token: 0x0400441A RID: 17434
		public CorgiControllerParameters ControllerParameters;

		// Token: 0x0400441B RID: 17435
		[Header("Force Modification on Entry")]
		public bool ResetForcesOnEntry;

		// Token: 0x0400441C RID: 17436
		public bool MultiplyForcesOnEntry;

		// Token: 0x0400441D RID: 17437
		public Vector2 ForceMultiplierOnEntry = Vector2.zero;
	}
}

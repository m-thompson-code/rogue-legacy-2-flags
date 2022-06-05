using System;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F27 RID: 3879
	[AddComponentMenu("Corgi Engine/Environment/Corgi Controller Override")]
	public class CorgiControllerPhysicsVolume2D : MonoBehaviour
	{
		// Token: 0x04005A60 RID: 23136
		public CorgiControllerParameters ControllerParameters;

		// Token: 0x04005A61 RID: 23137
		[Header("Force Modification on Entry")]
		public bool ResetForcesOnEntry;

		// Token: 0x04005A62 RID: 23138
		public bool MultiplyForcesOnEntry;

		// Token: 0x04005A63 RID: 23139
		public Vector2 ForceMultiplierOnEntry = Vector2.zero;
	}
}

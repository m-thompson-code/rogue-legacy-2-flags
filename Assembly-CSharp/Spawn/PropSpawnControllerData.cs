using System;
using UnityEngine;

namespace Spawn
{
	// Token: 0x02000895 RID: 2197
	[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Spawn Controller Data/Prop")]
	public class PropSpawnControllerData : ScriptableObject
	{
		// Token: 0x04003CD6 RID: 15574
		public CameraLayer CameraLayer = CameraLayer.Game;

		// Token: 0x04003CD7 RID: 15575
		public int SubLayer;

		// Token: 0x04003CD8 RID: 15576
		public int SubLayerMod;

		// Token: 0x04003CD9 RID: 15577
		public BiomePropsEntry[] PropsPerBiome;

		// Token: 0x04003CDA RID: 15578
		public Prop[] DefaultProps;

		// Token: 0x04003CDB RID: 15579
		public PropTags Tags;
	}
}

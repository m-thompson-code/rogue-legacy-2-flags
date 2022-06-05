using System;
using UnityEngine;

namespace Spawn
{
	// Token: 0x02000DC2 RID: 3522
	[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Spawn Controller Data/Prop")]
	public class PropSpawnControllerData : ScriptableObject
	{
		// Token: 0x04005104 RID: 20740
		public CameraLayer CameraLayer = CameraLayer.Game;

		// Token: 0x04005105 RID: 20741
		public int SubLayer;

		// Token: 0x04005106 RID: 20742
		public int SubLayerMod;

		// Token: 0x04005107 RID: 20743
		public BiomePropsEntry[] PropsPerBiome;

		// Token: 0x04005108 RID: 20744
		public Prop[] DefaultProps;

		// Token: 0x04005109 RID: 20745
		public PropTags Tags;
	}
}

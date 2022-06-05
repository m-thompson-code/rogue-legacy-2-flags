using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x0200091C RID: 2332
	public class SurfaceTrigger : MonoBehaviour, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnExitHitResponse
	{
		// Token: 0x06004C61 RID: 19553 RVA: 0x001126AC File Offset: 0x001108AC
		public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
		{
			SurfaceAudioController component = otherHBController.RootGameObject.GetComponent<SurfaceAudioController>();
			if (component != null)
			{
				component.SetSurfaceOverride(this.m_surfaceType);
			}
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x001126DC File Offset: 0x001108DC
		public void TerrainOnExitHitResponse(IHitboxController otherHBController)
		{
			SurfaceAudioController component = otherHBController.RootGameObject.GetComponent<SurfaceAudioController>();
			if (component != null)
			{
				component.SetSurfaceOverride(SurfaceType.None);
			}
		}

		// Token: 0x0400405F RID: 16479
		[SerializeField]
		private SurfaceType m_surfaceType;
	}
}

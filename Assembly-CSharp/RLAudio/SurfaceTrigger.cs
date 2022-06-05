using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E9A RID: 3738
	public class SurfaceTrigger : MonoBehaviour, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnExitHitResponse
	{
		// Token: 0x06006962 RID: 26978 RVA: 0x00182404 File Offset: 0x00180604
		public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
		{
			SurfaceAudioController component = otherHBController.RootGameObject.GetComponent<SurfaceAudioController>();
			if (component != null)
			{
				component.SetSurfaceOverride(this.m_surfaceType);
			}
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x00182434 File Offset: 0x00180634
		public void TerrainOnExitHitResponse(IHitboxController otherHBController)
		{
			SurfaceAudioController component = otherHBController.RootGameObject.GetComponent<SurfaceAudioController>();
			if (component != null)
			{
				component.SetSurfaceOverride(SurfaceType.None);
			}
		}

		// Token: 0x040055C7 RID: 21959
		[SerializeField]
		private SurfaceType m_surfaceType;
	}
}

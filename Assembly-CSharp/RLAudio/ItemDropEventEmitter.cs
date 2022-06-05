using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E6C RID: 3692
	public class ItemDropEventEmitter : MonoBehaviour, IAudioEventEmitter, ITerrainOnEnterHitResponse, IHitResponse
	{
		// Token: 0x1700214B RID: 8523
		// (get) Token: 0x0600682A RID: 26666 RVA: 0x00009A7B File Offset: 0x00007C7B
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x0017EFE8 File Offset: 0x0017D1E8
		private void Start()
		{
			if (!string.IsNullOrEmpty(this.PickupEvent))
			{
				this.m_pickupEventInstance = RuntimeManager.CreateInstance(this.PickupEvent);
			}
			if (!string.IsNullOrEmpty(this.BounceEvent))
			{
				this.m_bounceEventInstance = RuntimeManager.CreateInstance(this.BounceEvent);
			}
			this.m_corgi.OnCorgiLandedEnterRelay.AddListener(new Action<CorgiController_RL>(this.OnBounce), false);
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x00039A09 File Offset: 0x00037C09
		private void OnBounce(CorgiController_RL corgiController)
		{
			AudioManager.PlayAttached(this, this.m_bounceEventInstance, base.gameObject);
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x00039A1D File Offset: 0x00037C1D
		public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
		{
			AudioManager.PlayAttached(this, this.m_pickupEventInstance, base.gameObject);
		}

		// Token: 0x0400549B RID: 21659
		[EventRef]
		public string PickupEvent;

		// Token: 0x0400549C RID: 21660
		public ParamRef[] PickupParams = new ParamRef[0];

		// Token: 0x0400549D RID: 21661
		[EventRef]
		public string BounceEvent;

		// Token: 0x0400549E RID: 21662
		public ParamRef[] BounceParams = new ParamRef[0];

		// Token: 0x0400549F RID: 21663
		[SerializeField]
		private CorgiController_RL m_corgi;

		// Token: 0x040054A0 RID: 21664
		protected EventInstance m_pickupEventInstance;

		// Token: 0x040054A1 RID: 21665
		protected EventInstance m_bounceEventInstance;
	}
}

using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008F5 RID: 2293
	public class ItemDropEventEmitter : MonoBehaviour, IAudioEventEmitter, ITerrainOnEnterHitResponse, IHitResponse
	{
		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x06004B53 RID: 19283 RVA: 0x0010EE66 File Offset: 0x0010D066
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x0010EE70 File Offset: 0x0010D070
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

		// Token: 0x06004B55 RID: 19285 RVA: 0x0010EED7 File Offset: 0x0010D0D7
		private void OnBounce(CorgiController_RL corgiController)
		{
			AudioManager.PlayAttached(this, this.m_bounceEventInstance, base.gameObject);
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x0010EEEB File Offset: 0x0010D0EB
		public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
		{
			AudioManager.PlayAttached(this, this.m_pickupEventInstance, base.gameObject);
		}

		// Token: 0x04003F57 RID: 16215
		[EventRef]
		public string PickupEvent;

		// Token: 0x04003F58 RID: 16216
		public ParamRef[] PickupParams = new ParamRef[0];

		// Token: 0x04003F59 RID: 16217
		[EventRef]
		public string BounceEvent;

		// Token: 0x04003F5A RID: 16218
		public ParamRef[] BounceParams = new ParamRef[0];

		// Token: 0x04003F5B RID: 16219
		[SerializeField]
		private CorgiController_RL m_corgi;

		// Token: 0x04003F5C RID: 16220
		protected EventInstance m_pickupEventInstance;

		// Token: 0x04003F5D RID: 16221
		protected EventInstance m_bounceEventInstance;
	}
}

using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000908 RID: 2312
	public class PoolballProjectileAudioEventEmitter : ProjectileEventEmitter
	{
		// Token: 0x06004BD5 RID: 19413 RVA: 0x00110AD8 File Offset: 0x0010ECD8
		protected override void Awake()
		{
			base.Awake();
			base.gameObject.GetComponent<PoolBallProjectileLogic>().RicochetRelay.AddListener(new Action<int>(this.OnRicochet), false);
			this.m_bounceEventInstance = AudioUtility.GetEventInstance(this.m_bounceEventPath, base.transform);
			this.m_damageAudioData = base.GetComponent<ProjectileDamageAudioData>();
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x00110B31 File Offset: 0x0010ED31
		protected override void OnDisable()
		{
			if (this.m_lifeTimeEventInstance.isValid())
			{
				this.m_lifeTimeEventInstance.setParameterByName("fireToggle", 0f, false);
			}
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x00110B57 File Offset: 0x0010ED57
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.m_bounceEventInstance.isValid())
			{
				this.m_bounceEventInstance.release();
			}
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00110B78 File Offset: 0x0010ED78
		private void OnRicochet(int bounceCount)
		{
			if (!this.m_bounceEventInstance.isValid())
			{
				return;
			}
			if (bounceCount == 1 && this.m_lifeTimeEventInstance.isValid())
			{
				this.m_lifeTimeEventInstance.setParameterByName("fireToggle", 1f, false);
				this.m_damageAudioData.AddHitAudioParameter("poolBallSpeed", 1f);
			}
			int num = 3;
			float value = (float)bounceCount / (float)num;
			this.m_bounceEventInstance.setParameterByName("poolBallSpeed", value, false);
			AudioManager.PlayAttached(this, this.m_bounceEventInstance, base.gameObject);
		}

		// Token: 0x04003FE3 RID: 16355
		[SerializeField]
		[EventRef]
		private string m_bounceEventPath;

		// Token: 0x04003FE4 RID: 16356
		private EventInstance m_bounceEventInstance;

		// Token: 0x04003FE5 RID: 16357
		private ProjectileDamageAudioData m_damageAudioData;

		// Token: 0x04003FE6 RID: 16358
		private const string POOL_BALL_PARAMETER = "poolBallSpeed";
	}
}

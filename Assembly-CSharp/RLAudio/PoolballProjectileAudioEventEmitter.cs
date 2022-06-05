using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E85 RID: 3717
	public class PoolballProjectileAudioEventEmitter : ProjectileEventEmitter
	{
		// Token: 0x060068D0 RID: 26832 RVA: 0x00180D80 File Offset: 0x0017EF80
		protected override void Awake()
		{
			base.Awake();
			base.gameObject.GetComponent<PoolBallProjectileLogic>().RicochetRelay.AddListener(new Action<int>(this.OnRicochet), false);
			this.m_bounceEventInstance = AudioUtility.GetEventInstance(this.m_bounceEventPath, base.transform);
			this.m_damageAudioData = base.GetComponent<ProjectileDamageAudioData>();
		}

		// Token: 0x060068D1 RID: 26833 RVA: 0x0003A0D5 File Offset: 0x000382D5
		protected override void OnDisable()
		{
			if (this.m_lifeTimeEventInstance.isValid())
			{
				this.m_lifeTimeEventInstance.setParameterByName("fireToggle", 0f, false);
			}
		}

		// Token: 0x060068D2 RID: 26834 RVA: 0x0003A0FB File Offset: 0x000382FB
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.m_bounceEventInstance.isValid())
			{
				this.m_bounceEventInstance.release();
			}
		}

		// Token: 0x060068D3 RID: 26835 RVA: 0x00180DDC File Offset: 0x0017EFDC
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

		// Token: 0x04005543 RID: 21827
		[SerializeField]
		[EventRef]
		private string m_bounceEventPath;

		// Token: 0x04005544 RID: 21828
		private EventInstance m_bounceEventInstance;

		// Token: 0x04005545 RID: 21829
		private ProjectileDamageAudioData m_damageAudioData;

		// Token: 0x04005546 RID: 21830
		private const string POOL_BALL_PARAMETER = "poolBallSpeed";
	}
}

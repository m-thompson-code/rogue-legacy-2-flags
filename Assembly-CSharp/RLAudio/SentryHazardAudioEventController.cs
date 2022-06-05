using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000912 RID: 2322
	public class SentryHazardAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001883 RID: 6275
		// (get) Token: 0x06004C27 RID: 19495 RVA: 0x00111977 File Offset: 0x0010FB77
		public string Description
		{
			get
			{
				if (this.m_description == string.Empty)
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x0011199D File Offset: 0x0010FB9D
		private void Awake()
		{
			base.GetComponent<Sentry_Hazard>().PlayerWithinRangeChangeRelay.AddListener(new Action<bool>(this.OnPlayerWithinRangeChange), false);
			this.m_playerWithinRangeEventInstance = AudioUtility.GetEventInstance(this.m_playerWithinRangeAudioPath, base.transform);
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x001119D4 File Offset: 0x0010FBD4
		private void OnDisable()
		{
			if (this.m_playerWithinRangeEventInstance.isValid())
			{
				AudioManager.Stop(this.m_playerWithinRangeEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x001119EF File Offset: 0x0010FBEF
		private void OnDestroy()
		{
			if (this.m_playerWithinRangeEventInstance.isValid())
			{
				this.m_playerWithinRangeEventInstance.release();
			}
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x00111A0A File Offset: 0x0010FC0A
		private void OnPlayerWithinRangeChange(bool isPlayerWithinRange)
		{
			if (isPlayerWithinRange)
			{
				AudioManager.PlayAttached(this, this.m_playerWithinRangeEventInstance, base.gameObject);
				return;
			}
			AudioManager.Stop(this.m_playerWithinRangeEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			AudioManager.PlayOneShotAttached(this, this.m_playerExitRangeAudioPath, base.gameObject);
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x00111A40 File Offset: 0x0010FC40
		public void OnGrow()
		{
			AudioManager.PlayOneShotAttached(this, this.m_growAudioPath, base.gameObject);
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x00111A54 File Offset: 0x0010FC54
		public void OnGrowPrep()
		{
			AudioManager.PlayOneShotAttached(this, this.m_growPrepAudioPath, base.gameObject);
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x00111A68 File Offset: 0x0010FC68
		public void OnShrink()
		{
			AudioManager.PlayOneShotAttached(this, this.m_shrinkAudioPath, base.gameObject);
		}

		// Token: 0x0400401D RID: 16413
		[SerializeField]
		[EventRef]
		private string m_playerWithinRangeAudioPath;

		// Token: 0x0400401E RID: 16414
		[SerializeField]
		[EventRef]
		private string m_playerExitRangeAudioPath;

		// Token: 0x0400401F RID: 16415
		[SerializeField]
		[EventRef]
		private string m_growPrepAudioPath;

		// Token: 0x04004020 RID: 16416
		[SerializeField]
		[EventRef]
		private string m_growAudioPath;

		// Token: 0x04004021 RID: 16417
		[SerializeField]
		[EventRef]
		private string m_shrinkAudioPath;

		// Token: 0x04004022 RID: 16418
		private string m_description = string.Empty;

		// Token: 0x04004023 RID: 16419
		private EventInstance m_playerWithinRangeEventInstance;
	}
}

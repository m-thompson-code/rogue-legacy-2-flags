using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E8F RID: 3727
	public class SentryHazardAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700217C RID: 8572
		// (get) Token: 0x06006922 RID: 26914 RVA: 0x0003A436 File Offset: 0x00038636
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

		// Token: 0x06006923 RID: 26915 RVA: 0x0003A45C File Offset: 0x0003865C
		private void Awake()
		{
			base.GetComponent<Sentry_Hazard>().PlayerWithinRangeChangeRelay.AddListener(new Action<bool>(this.OnPlayerWithinRangeChange), false);
			this.m_playerWithinRangeEventInstance = AudioUtility.GetEventInstance(this.m_playerWithinRangeAudioPath, base.transform);
		}

		// Token: 0x06006924 RID: 26916 RVA: 0x0003A493 File Offset: 0x00038693
		private void OnDisable()
		{
			if (this.m_playerWithinRangeEventInstance.isValid())
			{
				AudioManager.Stop(this.m_playerWithinRangeEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x0003A4AE File Offset: 0x000386AE
		private void OnDestroy()
		{
			if (this.m_playerWithinRangeEventInstance.isValid())
			{
				this.m_playerWithinRangeEventInstance.release();
			}
		}

		// Token: 0x06006926 RID: 26918 RVA: 0x0003A4C9 File Offset: 0x000386C9
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

		// Token: 0x06006927 RID: 26919 RVA: 0x0003A4FF File Offset: 0x000386FF
		public void OnGrow()
		{
			AudioManager.PlayOneShotAttached(this, this.m_growAudioPath, base.gameObject);
		}

		// Token: 0x06006928 RID: 26920 RVA: 0x0003A513 File Offset: 0x00038713
		public void OnGrowPrep()
		{
			AudioManager.PlayOneShotAttached(this, this.m_growPrepAudioPath, base.gameObject);
		}

		// Token: 0x06006929 RID: 26921 RVA: 0x0003A527 File Offset: 0x00038727
		public void OnShrink()
		{
			AudioManager.PlayOneShotAttached(this, this.m_shrinkAudioPath, base.gameObject);
		}

		// Token: 0x0400557D RID: 21885
		[SerializeField]
		[EventRef]
		private string m_playerWithinRangeAudioPath;

		// Token: 0x0400557E RID: 21886
		[SerializeField]
		[EventRef]
		private string m_playerExitRangeAudioPath;

		// Token: 0x0400557F RID: 21887
		[SerializeField]
		[EventRef]
		private string m_growPrepAudioPath;

		// Token: 0x04005580 RID: 21888
		[SerializeField]
		[EventRef]
		private string m_growAudioPath;

		// Token: 0x04005581 RID: 21889
		[SerializeField]
		[EventRef]
		private string m_shrinkAudioPath;

		// Token: 0x04005582 RID: 21890
		private string m_description = string.Empty;

		// Token: 0x04005583 RID: 21891
		private EventInstance m_playerWithinRangeEventInstance;
	}
}

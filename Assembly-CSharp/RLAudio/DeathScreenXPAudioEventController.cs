using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008EC RID: 2284
	public class DeathScreenXPAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x06004B0C RID: 19212 RVA: 0x0010DFAD File Offset: 0x0010C1AD
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

		// Token: 0x06004B0D RID: 19213 RVA: 0x0010DFD4 File Offset: 0x0010C1D4
		private void Awake()
		{
			PlayerDeathRankTextController component = base.GetComponent<PlayerDeathRankTextController>();
			component.XPGainedRelay.AddListener(new Action<int, int>(this.OnXPGained), false);
			component.XPGainStartRelay.AddListener(new Action(this.OnXPGainStart), false);
			component.XPGainCompleteRelay.AddListener(new Action(this.OnXPGainComplete), false);
			component.LevelGainedRelay.AddListener(new Action<int>(this.OnLevelGained), false);
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x0010E04A File Offset: 0x0010C24A
		private void Start()
		{
			this.m_xpGainEventInstance = AudioUtility.GetEventInstance(this.m_xpGainAudioPath, base.transform);
			this.m_xpGainEndEventInstance = AudioUtility.GetEventInstance(this.m_xpGainEndAudioPath, base.transform);
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x0010E07A File Offset: 0x0010C27A
		private void OnDestroy()
		{
			if (this.m_xpGainEventInstance.isValid())
			{
				this.m_xpGainEventInstance.release();
			}
			if (this.m_xpGainEndEventInstance.isValid())
			{
				this.m_xpGainEndEventInstance.release();
			}
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x0010E0AE File Offset: 0x0010C2AE
		private void OnXPGainStart()
		{
			AudioManager.Play(this, this.m_xpGainEventInstance);
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x0010E0BC File Offset: 0x0010C2BC
		private void OnXPGained(int currentXP, int nextLevelXP)
		{
			this.m_percentageComplete = (float)currentXP / (float)nextLevelXP;
			this.m_xpGainEventInstance.setParameterByName("xp_gain", this.m_percentageComplete, false);
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x0010E0E1 File Offset: 0x0010C2E1
		private void OnXPGainComplete()
		{
			AudioManager.Stop(this.m_xpGainEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.m_xpGainEndEventInstance.setParameterByName("xp_gain", this.m_percentageComplete, false);
			AudioManager.Play(this, this.m_xpGainEndEventInstance);
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x0010E114 File Offset: 0x0010C314
		private void OnLevelGained(int level)
		{
			AudioManager.PlayOneShot(this, this.m_levelUpAudioPath, default(Vector3));
		}

		// Token: 0x04003F10 RID: 16144
		[SerializeField]
		[EventRef]
		private string m_xpGainAudioPath;

		// Token: 0x04003F11 RID: 16145
		[SerializeField]
		[EventRef]
		private string m_xpGainEndAudioPath;

		// Token: 0x04003F12 RID: 16146
		[SerializeField]
		[EventRef]
		private string m_levelUpAudioPath;

		// Token: 0x04003F13 RID: 16147
		private string m_description = string.Empty;

		// Token: 0x04003F14 RID: 16148
		private EventInstance m_xpGainEventInstance;

		// Token: 0x04003F15 RID: 16149
		private EventInstance m_xpGainEndEventInstance;

		// Token: 0x04003F16 RID: 16150
		private float m_percentageComplete;

		// Token: 0x04003F17 RID: 16151
		private const string XP_PARAMETER = "xp_gain";
	}
}

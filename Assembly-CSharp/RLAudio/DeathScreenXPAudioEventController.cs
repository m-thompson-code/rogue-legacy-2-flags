using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E61 RID: 3681
	public class DeathScreenXPAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700213B RID: 8507
		// (get) Token: 0x060067D7 RID: 26583 RVA: 0x000395B0 File Offset: 0x000377B0
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

		// Token: 0x060067D8 RID: 26584 RVA: 0x0017E414 File Offset: 0x0017C614
		private void Awake()
		{
			PlayerDeathRankTextController component = base.GetComponent<PlayerDeathRankTextController>();
			component.XPGainedRelay.AddListener(new Action<int, int>(this.OnXPGained), false);
			component.XPGainStartRelay.AddListener(new Action(this.OnXPGainStart), false);
			component.XPGainCompleteRelay.AddListener(new Action(this.OnXPGainComplete), false);
			component.LevelGainedRelay.AddListener(new Action<int>(this.OnLevelGained), false);
		}

		// Token: 0x060067D9 RID: 26585 RVA: 0x000395D6 File Offset: 0x000377D6
		private void Start()
		{
			this.m_xpGainEventInstance = AudioUtility.GetEventInstance(this.m_xpGainAudioPath, base.transform);
			this.m_xpGainEndEventInstance = AudioUtility.GetEventInstance(this.m_xpGainEndAudioPath, base.transform);
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x00039606 File Offset: 0x00037806
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

		// Token: 0x060067DB RID: 26587 RVA: 0x0003963A File Offset: 0x0003783A
		private void OnXPGainStart()
		{
			AudioManager.Play(this, this.m_xpGainEventInstance);
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x00039648 File Offset: 0x00037848
		private void OnXPGained(int currentXP, int nextLevelXP)
		{
			this.m_percentageComplete = (float)currentXP / (float)nextLevelXP;
			this.m_xpGainEventInstance.setParameterByName("xp_gain", this.m_percentageComplete, false);
		}

		// Token: 0x060067DD RID: 26589 RVA: 0x0003966D File Offset: 0x0003786D
		private void OnXPGainComplete()
		{
			AudioManager.Stop(this.m_xpGainEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.m_xpGainEndEventInstance.setParameterByName("xp_gain", this.m_percentageComplete, false);
			AudioManager.Play(this, this.m_xpGainEndEventInstance);
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x0017E48C File Offset: 0x0017C68C
		private void OnLevelGained(int level)
		{
			AudioManager.PlayOneShot(this, this.m_levelUpAudioPath, default(Vector3));
		}

		// Token: 0x0400544A RID: 21578
		[SerializeField]
		[EventRef]
		private string m_xpGainAudioPath;

		// Token: 0x0400544B RID: 21579
		[SerializeField]
		[EventRef]
		private string m_xpGainEndAudioPath;

		// Token: 0x0400544C RID: 21580
		[SerializeField]
		[EventRef]
		private string m_levelUpAudioPath;

		// Token: 0x0400544D RID: 21581
		private string m_description = string.Empty;

		// Token: 0x0400544E RID: 21582
		private EventInstance m_xpGainEventInstance;

		// Token: 0x0400544F RID: 21583
		private EventInstance m_xpGainEndEventInstance;

		// Token: 0x04005450 RID: 21584
		private float m_percentageComplete;

		// Token: 0x04005451 RID: 21585
		private const string XP_PARAMETER = "xp_gain";
	}
}

using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using SceneManagement_RL;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008F0 RID: 2288
	public class FairyRoomAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x06004B2F RID: 19247 RVA: 0x0010E7AC File Offset: 0x0010C9AC
		public string Description
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_description))
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x0010E7D0 File Offset: 0x0010C9D0
		private void Awake()
		{
			this.m_onPlayerTriggerFairyRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerTriggerFairyRoom);
			this.m_onPlayerExitFairyRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitFairyRoom);
			this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
			this.m_onFairyRuleStateChange = new Action<MonoBehaviour, EventArgs>(this.OnFairyRuleStateChange);
			this.m_onSpecialRoomComplete = new Action<MonoBehaviour, EventArgs>(this.OnSpecialRoomComplete);
			this.m_onTransitionStart = new Action(this.OnTransitionStart);
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x0010E84C File Offset: 0x0010CA4C
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerFairyRoomTriggered, this.m_onPlayerTriggerFairyRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitFairyRoom, this.m_onPlayerExitFairyRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.FairyRoomRuleStateChange, this.m_onFairyRuleStateChange);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SpecialRoomCompleted, this.m_onSpecialRoomComplete);
			SceneLoader_RL.TransitionStartRelay.AddListener(this.m_onTransitionStart, false);
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x0010E8AC File Offset: 0x0010CAAC
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFairyRoomTriggered, this.m_onPlayerTriggerFairyRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitFairyRoom, this.m_onPlayerExitFairyRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.FairyRoomRuleStateChange, this.m_onFairyRuleStateChange);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.SpecialRoomCompleted, this.m_onSpecialRoomComplete);
			if (SceneLoader_RL.TransitionStartRelay != null)
			{
				SceneLoader_RL.TransitionStartRelay.RemoveListener(this.m_onTransitionStart);
			}
			if (this.m_countdownEvent.isValid())
			{
				this.m_countdownEvent.release();
			}
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x0010E92C File Offset: 0x0010CB2C
		private void OnSpecialRoomComplete(MonoBehaviour arg1, EventArgs arg2)
		{
			SpecialRoomCompletedEventArgs specialRoomCompletedEventArgs = arg2 as SpecialRoomCompletedEventArgs;
			if (specialRoomCompletedEventArgs != null && specialRoomCompletedEventArgs.SpecialRoomController.SpecialRoomType == SpecialRoomType.Fairy)
			{
				if ((specialRoomCompletedEventArgs.SpecialRoomController as FairyRoomController).State == FairyRoomState.Passed)
				{
					AudioManager.PlayOneShot(this, this.m_victoryEventPath, default(Vector3));
				}
				else
				{
					AudioManager.PlayOneShot(this, this.m_defeatEventPath, default(Vector3));
				}
			}
			this.StopCountdownTimer();
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x0010E99C File Offset: 0x0010CB9C
		private void OnFairyRuleStateChange(MonoBehaviour arg1, EventArgs arg2)
		{
			if (FairyRoomAudioEventController.DisableAudio)
			{
				return;
			}
			FairyRoomRuleStateChangeEventArgs fairyRoomRuleStateChangeEventArgs = arg2 as FairyRoomRuleStateChangeEventArgs;
			if (fairyRoomRuleStateChangeEventArgs != null && fairyRoomRuleStateChangeEventArgs.Rule.State == FairyRoomState.Failed)
			{
				AudioManager.PlayOneShot(this, this.m_ruleFailedEventPath, default(Vector3));
			}
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x0010E9DF File Offset: 0x0010CBDF
		private void OnPlayerDeath(MonoBehaviour arg1, EventArgs arg2)
		{
			this.StopCountdownTimer();
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x0010E9E7 File Offset: 0x0010CBE7
		private void OnTransitionStart()
		{
			this.StopCountdownTimer();
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x0010E9F0 File Offset: 0x0010CBF0
		private void StopCountdownTimer()
		{
			if (this.m_countdownEvent.isValid())
			{
				AudioManager.Stop(this.m_countdownEvent, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				if (this.m_countdownCoroutine != null)
				{
					base.StopCoroutine(this.m_countdownCoroutine);
					this.m_countdownCoroutine = null;
					AudioManager.PlayOneShot(this, this.m_countdownTimerEndEventPath, default(Vector3));
				}
			}
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x0010EA46 File Offset: 0x0010CC46
		private void OnPlayerExitFairyRoom(MonoBehaviour arg1, EventArgs arg2)
		{
			this.StopCountdownTimer();
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x0010EA50 File Offset: 0x0010CC50
		private void OnPlayerTriggerFairyRoom(MonoBehaviour sender, EventArgs eventArgs)
		{
			FairyRoomEnteredEventArgs fairyRoomEnteredEventArgs = eventArgs as FairyRoomEnteredEventArgs;
			if (fairyRoomEnteredEventArgs != null && fairyRoomEnteredEventArgs.FairyRoomController.State != FairyRoomState.Failed && fairyRoomEnteredEventArgs.FairyRoomController.State != FairyRoomState.Passed)
			{
				for (int i = 0; i < fairyRoomEnteredEventArgs.FairyRoomController.FairyRoomRuleEntries.Count; i++)
				{
					if (fairyRoomEnteredEventArgs.FairyRoomController.FairyRoomRuleEntries[i].FairyRuleID == FairyRuleID.TimeLimit)
					{
						if (!this.m_countdownEvent.isValid())
						{
							this.m_countdownEvent = AudioUtility.GetEventInstance(this.m_countdownTimerEventPath, base.transform);
						}
						this.m_countdownCoroutine = base.StartCoroutine(this.CountdownTimerCoroutine(fairyRoomEnteredEventArgs.FairyRoomController.FairyRoomRuleEntries[i].FairyRule as TimeLimit_FairyRule));
						return;
					}
				}
			}
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x0010EB18 File Offset: 0x0010CD18
		private IEnumerator CountdownTimerCoroutine(TimeLimit_FairyRule fairyRule)
		{
			AudioManager.Play(this, this.m_countdownEvent);
			float timeLimit = fairyRule.TimeRemaining;
			while (fairyRule.TimeRemaining > 0f)
			{
				float value = 1f - fairyRule.TimeRemaining / timeLimit;
				this.m_countdownEvent.setParameterByName("fairyTimer", value, false);
				yield return null;
			}
			this.m_countdownCoroutine = null;
			yield break;
		}

		// Token: 0x04003F3A RID: 16186
		public static bool DisableAudio;

		// Token: 0x04003F3B RID: 16187
		[SerializeField]
		[EventRef]
		private string m_countdownTimerEventPath;

		// Token: 0x04003F3C RID: 16188
		[SerializeField]
		[EventRef]
		private string m_countdownTimerEndEventPath;

		// Token: 0x04003F3D RID: 16189
		[SerializeField]
		[EventRef]
		private string m_victoryEventPath;

		// Token: 0x04003F3E RID: 16190
		[SerializeField]
		[EventRef]
		private string m_defeatEventPath;

		// Token: 0x04003F3F RID: 16191
		[SerializeField]
		[EventRef]
		private string m_ruleFailedEventPath;

		// Token: 0x04003F40 RID: 16192
		private string m_description;

		// Token: 0x04003F41 RID: 16193
		private EventInstance m_countdownEvent;

		// Token: 0x04003F42 RID: 16194
		private const string COUNTDOWN_PARAMETER_NAME = "fairyTimer";

		// Token: 0x04003F43 RID: 16195
		private Coroutine m_countdownCoroutine;

		// Token: 0x04003F44 RID: 16196
		private Action<MonoBehaviour, EventArgs> m_onPlayerTriggerFairyRoom;

		// Token: 0x04003F45 RID: 16197
		private Action<MonoBehaviour, EventArgs> m_onPlayerExitFairyRoom;

		// Token: 0x04003F46 RID: 16198
		private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

		// Token: 0x04003F47 RID: 16199
		private Action<MonoBehaviour, EventArgs> m_onFairyRuleStateChange;

		// Token: 0x04003F48 RID: 16200
		private Action<MonoBehaviour, EventArgs> m_onSpecialRoomComplete;

		// Token: 0x04003F49 RID: 16201
		private Action m_onTransitionStart;
	}
}

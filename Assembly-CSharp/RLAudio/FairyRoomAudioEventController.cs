using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using SceneManagement_RL;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E65 RID: 3685
	public class FairyRoomAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700213F RID: 8511
		// (get) Token: 0x060067FA RID: 26618 RVA: 0x00039866 File Offset: 0x00037A66
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

		// Token: 0x060067FB RID: 26619 RVA: 0x0017E960 File Offset: 0x0017CB60
		private void Awake()
		{
			this.m_onPlayerTriggerFairyRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerTriggerFairyRoom);
			this.m_onPlayerExitFairyRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitFairyRoom);
			this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
			this.m_onFairyRuleStateChange = new Action<MonoBehaviour, EventArgs>(this.OnFairyRuleStateChange);
			this.m_onSpecialRoomComplete = new Action<MonoBehaviour, EventArgs>(this.OnSpecialRoomComplete);
			this.m_onTransitionStart = new Action(this.OnTransitionStart);
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x0017E9DC File Offset: 0x0017CBDC
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerFairyRoomTriggered, this.m_onPlayerTriggerFairyRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitFairyRoom, this.m_onPlayerExitFairyRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.FairyRoomRuleStateChange, this.m_onFairyRuleStateChange);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.SpecialRoomCompleted, this.m_onSpecialRoomComplete);
			SceneLoader_RL.TransitionStartRelay.AddListener(this.m_onTransitionStart, false);
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x0017EA3C File Offset: 0x0017CC3C
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

		// Token: 0x060067FE RID: 26622 RVA: 0x0017EABC File Offset: 0x0017CCBC
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

		// Token: 0x060067FF RID: 26623 RVA: 0x0017EB2C File Offset: 0x0017CD2C
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

		// Token: 0x06006800 RID: 26624 RVA: 0x00039887 File Offset: 0x00037A87
		private void OnPlayerDeath(MonoBehaviour arg1, EventArgs arg2)
		{
			this.StopCountdownTimer();
		}

		// Token: 0x06006801 RID: 26625 RVA: 0x00039887 File Offset: 0x00037A87
		private void OnTransitionStart()
		{
			this.StopCountdownTimer();
		}

		// Token: 0x06006802 RID: 26626 RVA: 0x0017EB70 File Offset: 0x0017CD70
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

		// Token: 0x06006803 RID: 26627 RVA: 0x00039887 File Offset: 0x00037A87
		private void OnPlayerExitFairyRoom(MonoBehaviour arg1, EventArgs arg2)
		{
			this.StopCountdownTimer();
		}

		// Token: 0x06006804 RID: 26628 RVA: 0x0017EBC8 File Offset: 0x0017CDC8
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

		// Token: 0x06006805 RID: 26629 RVA: 0x0003988F File Offset: 0x00037A8F
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

		// Token: 0x04005474 RID: 21620
		public static bool DisableAudio;

		// Token: 0x04005475 RID: 21621
		[SerializeField]
		[EventRef]
		private string m_countdownTimerEventPath;

		// Token: 0x04005476 RID: 21622
		[SerializeField]
		[EventRef]
		private string m_countdownTimerEndEventPath;

		// Token: 0x04005477 RID: 21623
		[SerializeField]
		[EventRef]
		private string m_victoryEventPath;

		// Token: 0x04005478 RID: 21624
		[SerializeField]
		[EventRef]
		private string m_defeatEventPath;

		// Token: 0x04005479 RID: 21625
		[SerializeField]
		[EventRef]
		private string m_ruleFailedEventPath;

		// Token: 0x0400547A RID: 21626
		private string m_description;

		// Token: 0x0400547B RID: 21627
		private EventInstance m_countdownEvent;

		// Token: 0x0400547C RID: 21628
		private const string COUNTDOWN_PARAMETER_NAME = "fairyTimer";

		// Token: 0x0400547D RID: 21629
		private Coroutine m_countdownCoroutine;

		// Token: 0x0400547E RID: 21630
		private Action<MonoBehaviour, EventArgs> m_onPlayerTriggerFairyRoom;

		// Token: 0x0400547F RID: 21631
		private Action<MonoBehaviour, EventArgs> m_onPlayerExitFairyRoom;

		// Token: 0x04005480 RID: 21632
		private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

		// Token: 0x04005481 RID: 21633
		private Action<MonoBehaviour, EventArgs> m_onFairyRuleStateChange;

		// Token: 0x04005482 RID: 21634
		private Action<MonoBehaviour, EventArgs> m_onSpecialRoomComplete;

		// Token: 0x04005483 RID: 21635
		private Action m_onTransitionStart;
	}
}

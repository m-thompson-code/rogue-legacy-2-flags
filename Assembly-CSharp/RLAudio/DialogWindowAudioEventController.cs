using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x02000E62 RID: 3682
	public class DialogWindowAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700213C RID: 8508
		// (get) Token: 0x060067E0 RID: 26592 RVA: 0x000396B2 File Offset: 0x000378B2
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

		// Token: 0x060067E1 RID: 26593 RVA: 0x0017E4B0 File Offset: 0x0017C6B0
		private void Awake()
		{
			DialogueWindowController component = base.GetComponent<DialogueWindowController>();
			component.WriteLineOfDialogStartRelay.AddListener(new Action(this.OnWriteCharacter), false);
			component.WriteLineOfDialogEndRelay.AddListener(new Action(this.OnWritePageComplete), false);
			component.WriteCharacterStartRelay.AddListener(new Action(this.OnWritePunctuation), false);
			component.NextLineOfDialogRelay.AddListener(new Action(this.OnNextPage), false);
			component.WindowOpenedEvent.AddListener(new UnityAction(this.OnWindowOpened));
			component.DialogWindowBeginClosingRelay.AddListener(new Action(this.OnWindowClosed), false);
			this.m_writeLineEventInstance = AudioUtility.GetEventInstance(this.m_writeTextEventPath, base.transform);
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x000396D8 File Offset: 0x000378D8
		private void OnDestroy()
		{
			if (this.m_writeLineEventInstance.isValid())
			{
				this.m_writeLineEventInstance.release();
			}
		}

		// Token: 0x060067E3 RID: 26595 RVA: 0x0017E570 File Offset: 0x0017C770
		private void OnWindowOpened()
		{
			AudioManager.PlayOneShot(this, this.m_windowOpenedEventPath, default(Vector3));
			this.m_isFirstPage = true;
		}

		// Token: 0x060067E4 RID: 26596 RVA: 0x0017E59C File Offset: 0x0017C79C
		private void OnWindowClosed()
		{
			AudioManager.PlayOneShot(this, this.m_windowClosedEventPath, default(Vector3));
			this.StopWriteTextAudio();
		}

		// Token: 0x060067E5 RID: 26597 RVA: 0x000396F3 File Offset: 0x000378F3
		private void OnWriteCharacter()
		{
			this.PlayWriteTextAudio();
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x000396FB File Offset: 0x000378FB
		private void PlayWriteTextAudio()
		{
			if (!this.m_isPlayingWriteLineAudio)
			{
				AudioManager.Play(this, this.m_writeLineEventInstance);
				this.m_isPlayingWriteLineAudio = true;
			}
		}

		// Token: 0x060067E7 RID: 26599 RVA: 0x0017E5C4 File Offset: 0x0017C7C4
		private void OnWritePunctuation()
		{
			this.StopWriteTextAudio();
			AudioManager.PlayOneShot(this, this.m_writePunctuationEventPath, default(Vector3));
		}

		// Token: 0x060067E8 RID: 26600 RVA: 0x00039718 File Offset: 0x00037918
		private void StopWriteTextAudio()
		{
			if (this.m_isPlayingWriteLineAudio)
			{
				AudioManager.Stop(this.m_writeLineEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.m_isPlayingWriteLineAudio = false;
			}
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x0017E5EC File Offset: 0x0017C7EC
		private void OnNextPage()
		{
			if (this.m_isFirstPage)
			{
				this.m_isFirstPage = false;
			}
			else
			{
				AudioManager.PlayOneShot(this, this.m_playerInputToNextPageEventPath, default(Vector3));
			}
			this.StopWriteTextAudio();
			this.PlayWriteTextAudio();
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x0017E62C File Offset: 0x0017C82C
		private void OnWritePageComplete()
		{
			AudioManager.PlayOneShot(this, this.m_completeEventPath, default(Vector3));
			this.StopWriteTextAudio();
		}

		// Token: 0x04005452 RID: 21586
		[SerializeField]
		[EventRef]
		private string m_windowOpenedEventPath;

		// Token: 0x04005453 RID: 21587
		[SerializeField]
		[EventRef]
		private string m_windowClosedEventPath;

		// Token: 0x04005454 RID: 21588
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_writeLineEventPath")]
		private string m_writeTextEventPath;

		// Token: 0x04005455 RID: 21589
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_writeCharacterEventPath")]
		private string m_writePunctuationEventPath;

		// Token: 0x04005456 RID: 21590
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_writeCharacterEventPath")]
		private string m_playerInputToNextPageEventPath;

		// Token: 0x04005457 RID: 21591
		[SerializeField]
		[EventRef]
		private string m_completeEventPath;

		// Token: 0x04005458 RID: 21592
		private EventInstance m_writeLineEventInstance;

		// Token: 0x04005459 RID: 21593
		private string m_description = string.Empty;

		// Token: 0x0400545A RID: 21594
		private bool m_isPlayingWriteLineAudio;

		// Token: 0x0400545B RID: 21595
		private bool m_isFirstPage = true;
	}
}

using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RLAudio
{
	// Token: 0x020008ED RID: 2285
	public class DialogWindowAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x06004B15 RID: 19221 RVA: 0x0010E149 File Offset: 0x0010C349
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

		// Token: 0x06004B16 RID: 19222 RVA: 0x0010E170 File Offset: 0x0010C370
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

		// Token: 0x06004B17 RID: 19223 RVA: 0x0010E22D File Offset: 0x0010C42D
		private void OnDestroy()
		{
			if (this.m_writeLineEventInstance.isValid())
			{
				this.m_writeLineEventInstance.release();
			}
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x0010E248 File Offset: 0x0010C448
		private void OnWindowOpened()
		{
			AudioManager.PlayOneShot(this, this.m_windowOpenedEventPath, default(Vector3));
			this.m_isFirstPage = true;
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x0010E274 File Offset: 0x0010C474
		private void OnWindowClosed()
		{
			AudioManager.PlayOneShot(this, this.m_windowClosedEventPath, default(Vector3));
			this.StopWriteTextAudio();
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x0010E29C File Offset: 0x0010C49C
		private void OnWriteCharacter()
		{
			this.PlayWriteTextAudio();
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x0010E2A4 File Offset: 0x0010C4A4
		private void PlayWriteTextAudio()
		{
			if (!this.m_isPlayingWriteLineAudio)
			{
				AudioManager.Play(this, this.m_writeLineEventInstance);
				this.m_isPlayingWriteLineAudio = true;
			}
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x0010E2C4 File Offset: 0x0010C4C4
		private void OnWritePunctuation()
		{
			this.StopWriteTextAudio();
			AudioManager.PlayOneShot(this, this.m_writePunctuationEventPath, default(Vector3));
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x0010E2EC File Offset: 0x0010C4EC
		private void StopWriteTextAudio()
		{
			if (this.m_isPlayingWriteLineAudio)
			{
				AudioManager.Stop(this.m_writeLineEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.m_isPlayingWriteLineAudio = false;
			}
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x0010E30C File Offset: 0x0010C50C
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

		// Token: 0x06004B1F RID: 19231 RVA: 0x0010E34C File Offset: 0x0010C54C
		private void OnWritePageComplete()
		{
			AudioManager.PlayOneShot(this, this.m_completeEventPath, default(Vector3));
			this.StopWriteTextAudio();
		}

		// Token: 0x04003F18 RID: 16152
		[SerializeField]
		[EventRef]
		private string m_windowOpenedEventPath;

		// Token: 0x04003F19 RID: 16153
		[SerializeField]
		[EventRef]
		private string m_windowClosedEventPath;

		// Token: 0x04003F1A RID: 16154
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_writeLineEventPath")]
		private string m_writeTextEventPath;

		// Token: 0x04003F1B RID: 16155
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_writeCharacterEventPath")]
		private string m_writePunctuationEventPath;

		// Token: 0x04003F1C RID: 16156
		[SerializeField]
		[EventRef]
		[FormerlySerializedAs("m_writeCharacterEventPath")]
		private string m_playerInputToNextPageEventPath;

		// Token: 0x04003F1D RID: 16157
		[SerializeField]
		[EventRef]
		private string m_completeEventPath;

		// Token: 0x04003F1E RID: 16158
		private EventInstance m_writeLineEventInstance;

		// Token: 0x04003F1F RID: 16159
		private string m_description = string.Empty;

		// Token: 0x04003F20 RID: 16160
		private bool m_isPlayingWriteLineAudio;

		// Token: 0x04003F21 RID: 16161
		private bool m_isFirstPage = true;
	}
}

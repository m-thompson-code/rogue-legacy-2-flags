using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E7C RID: 3708
	public class OpeningCutsceneAudioEventEmitterController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700215B RID: 8539
		// (get) Token: 0x0600688F RID: 26767 RVA: 0x00039E44 File Offset: 0x00038044
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

		// Token: 0x06006890 RID: 26768 RVA: 0x00180090 File Offset: 0x0017E290
		private void Awake()
		{
			this.m_startEventIntance = AudioUtility.GetEventInstance(this.m_startAudioEventPath, base.transform);
			this.m_blackScreenEventInstance = AudioUtility.GetEventInstance(this.m_blackScreenAudioEventPath, base.transform);
			this.m_starsAppearEventInstance = AudioUtility.GetEventInstance(this.m_starsAppearAudioEventPath, base.transform);
			this.m_castleAppearsEventInstance = AudioUtility.GetEventInstance(this.m_castleAppearsAudioEventPath, base.transform);
			this.m_mainMenuWindowController = base.GetComponent<MainMenuWindowController>();
			this.m_mainMenuWindowController.LoadGameRelay.AddListener(new Action<bool>(this.OnLoadGame), false);
			this.m_mainMenuWindowController.CutsceneStartRelay.AddListener(new Action(this.OnCutsceneStart), false);
			this.m_mainMenuWindowController.CutsceneCompleteRelay.AddListener(new Action(this.OnCutsceneComplete), false);
			this.m_mainMenuWindowController.CutsceneFastForwardRelay.AddListener(new Action<bool>(this.OnCutsceneFastForward), false);
			this.m_mainMenuWindowController.CutsceneBlackRelay.AddListener(new Action(this.OnCutsceneBlack), false);
			this.m_mainMenuWindowController.CloudsAppearRelay.AddListener(new Action(this.OnCloudsAppear), false);
			this.m_mainMenuWindowController.CutsceneCastleAppearsRelay.AddListener(new Action(this.OnCutsceneCastleAppears), false);
			this.m_mainMenuWindowController.CutsceneStarsAppearRelay.AddListener(new Action(this.OnCutsceneStarsAppear), false);
			this.m_mainMenuWindowController.CutsceneDisplayTextRelay.AddListener(new Action<CanvasGroup>(this.OnCutsceneDisplayTextCall), false);
		}

		// Token: 0x06006891 RID: 26769 RVA: 0x00180214 File Offset: 0x0017E414
		private void OnDestroy()
		{
			if (this.m_startEventIntance.isValid())
			{
				this.m_startEventIntance.release();
			}
			if (this.m_blackScreenEventInstance.isValid())
			{
				this.m_blackScreenEventInstance.release();
			}
			if (this.m_starsAppearEventInstance.isValid())
			{
				this.m_starsAppearEventInstance.release();
			}
			if (this.m_castleAppearsEventInstance.isValid())
			{
				this.m_castleAppearsEventInstance.release();
			}
		}

		// Token: 0x06006892 RID: 26770 RVA: 0x00180288 File Offset: 0x0017E488
		private void OnLoadGame(bool isContinueLegacy)
		{
			if (isContinueLegacy)
			{
				AudioManager.PlayOneShot(this, this.m_continueLegacy, default(Vector3));
				return;
			}
			EventInstance eventInstance = AudioUtility.GetEventInstance(this.m_newLegacy, base.transform);
			this.PlayEvent(eventInstance);
		}

		// Token: 0x06006893 RID: 26771 RVA: 0x001802C8 File Offset: 0x0017E4C8
		private void OnCloudsAppear()
		{
			EventInstance eventInstance = AudioUtility.GetEventInstance(this.m_storyPointboatWavesAndSqueaks, base.transform);
			this.PlayEvent(eventInstance);
		}

		// Token: 0x06006894 RID: 26772 RVA: 0x00039E6A File Offset: 0x0003806A
		private void OnCutsceneBlack()
		{
			this.PlayEvent(this.m_blackScreenEventInstance);
		}

		// Token: 0x06006895 RID: 26773 RVA: 0x001802F0 File Offset: 0x0017E4F0
		private void OnCutsceneStarsAppear()
		{
			this.StopEventInstance(this.m_blackScreenEventInstance);
			this.PlayEvent(this.m_starsAppearEventInstance);
			EventInstance eventInstance = AudioUtility.GetEventInstance(this.m_storyPointBoatInWater, base.transform);
			this.PlayEvent(eventInstance);
		}

		// Token: 0x06006896 RID: 26774 RVA: 0x00039E78 File Offset: 0x00038078
		private void StopEventInstance(EventInstance eventInstance)
		{
			this.m_currentEvents.Remove(eventInstance);
			AudioManager.Stop(eventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x06006897 RID: 26775 RVA: 0x00039E8E File Offset: 0x0003808E
		private void OnCutsceneDisplayTextCall(CanvasGroup canvasGroup)
		{
			base.StartCoroutine(this.PlayTextAudio(canvasGroup));
		}

		// Token: 0x06006898 RID: 26776 RVA: 0x00039E9E File Offset: 0x0003809E
		private IEnumerator PlayTextAudio(CanvasGroup canvasGroup)
		{
			while (canvasGroup.alpha == 0f)
			{
				yield return null;
			}
			TextMeshProUGUI componentInChildren = canvasGroup.GetComponentInChildren<TextMeshProUGUI>();
			Vector3 position;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(componentInChildren.rectTransform, componentInChildren.rectTransform.position, CameraController.GameCamera, out position);
			if (!this.m_isFastForwarding)
			{
				AudioManager.PlayOneShot(this, this.m_textAppearsAudioEventPath, position);
			}
			string text = string.Empty;
			switch (this.m_textEntryNumber)
			{
			case 0:
				text = this.m_storyPoint00;
				break;
			case 1:
				text = this.m_storyPoint01;
				break;
			case 2:
				text = this.m_storyPoint02;
				break;
			case 3:
				text = this.m_storyPoint03;
				break;
			case 4:
				text = this.m_storyPoint04;
				break;
			case 5:
				text = this.m_storyPoint05;
				break;
			case 6:
				text = this.m_storyPoint06;
				break;
			}
			if (text != string.Empty)
			{
				EventInstance eventInstance = AudioUtility.GetEventInstance(text, base.transform);
				this.PlayEvent(eventInstance, position);
			}
			this.m_textEntryNumber++;
			yield break;
		}

		// Token: 0x06006899 RID: 26777 RVA: 0x00039EB4 File Offset: 0x000380B4
		private void OnCutsceneCastleAppears()
		{
			this.PlayEvent(this.m_castleAppearsEventInstance);
		}

		// Token: 0x0600689A RID: 26778 RVA: 0x00180330 File Offset: 0x0017E530
		private void OnCutsceneFastForward(bool isFastForwarding)
		{
			this.m_isFastForwarding = isFastForwarding;
			for (int i = 0; i < this.m_currentEvents.Count; i++)
			{
				if (this.m_currentEvents[i].isValid())
				{
					if (this.m_fastForwardCoroutine != null)
					{
						base.StopCoroutine(this.m_fastForwardCoroutine);
					}
					this.m_fastForwardCoroutine = base.StartCoroutine(this.SetFastForwardParameter(isFastForwarding));
				}
			}
		}

		// Token: 0x0600689B RID: 26779 RVA: 0x00039EC2 File Offset: 0x000380C2
		private IEnumerator SetFastForwardParameter(bool isFastForwarding)
		{
			float timeStart = Time.unscaledTime;
			float initialValue = this.m_currentFastForwardValue;
			float targetValue = 0f;
			if (isFastForwarding)
			{
				targetValue = 1f;
			}
			while (Time.unscaledTime - timeStart < 0.25f)
			{
				float t = (Time.unscaledTime - timeStart) / 0.25f;
				this.m_currentFastForwardValue = Mathf.Lerp(initialValue, targetValue, t);
				for (int i = 0; i < this.m_currentEvents.Count; i++)
				{
					if (this.m_currentEvents[i].isValid())
					{
						this.m_currentEvents[i].setParameterByName("FastForward", this.m_currentFastForwardValue, false);
					}
				}
				yield return null;
			}
			this.m_currentFastForwardValue = targetValue;
			this.m_fastForwardCoroutine = null;
			yield break;
		}

		// Token: 0x0600689C RID: 26780 RVA: 0x00180398 File Offset: 0x0017E598
		private void OnCutsceneComplete()
		{
			for (int i = 0; i < this.m_currentEvents.Count; i++)
			{
				if (this.m_currentEvents[i].isValid())
				{
					AudioManager.Stop(this.m_currentEvents[i], FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				}
			}
			AudioManager.PlayOneShot(this, this.m_storyPoint07, default(Vector3));
		}

		// Token: 0x0600689D RID: 26781 RVA: 0x001803F8 File Offset: 0x0017E5F8
		private void OnCutsceneStart()
		{
			if (this.m_mainMenuMusicPlayer != null)
			{
				this.m_mainMenuMusicPlayer.Stop();
			}
			if (this.m_mainMenuAmbientSoundEventEmitter != null)
			{
				this.m_mainMenuAmbientSoundEventEmitter.Stop();
			}
			this.m_textEntryNumber = 0;
			this.PlayEvent(this.m_startEventIntance);
		}

		// Token: 0x0600689E RID: 26782 RVA: 0x00039ED8 File Offset: 0x000380D8
		private void PlayEvent(EventInstance eventInstance)
		{
			this.PlayEvent(eventInstance, Vector3.zero);
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x0018044C File Offset: 0x0017E64C
		private void PlayEvent(EventInstance eventInstance, Vector3 position)
		{
			this.m_currentEvents.Add(eventInstance);
			if (position == Vector3.zero)
			{
				AudioManager.Play(this, eventInstance);
			}
			else
			{
				AudioManager.Play(this, eventInstance, position);
			}
			eventInstance.setParameterByName("FastForward", this.m_currentFastForwardValue, false);
			eventInstance.release();
		}

		// Token: 0x040054F2 RID: 21746
		[SerializeField]
		[EventRef]
		private string m_startAudioEventPath;

		// Token: 0x040054F3 RID: 21747
		[SerializeField]
		[EventRef]
		private string m_blackScreenAudioEventPath;

		// Token: 0x040054F4 RID: 21748
		[SerializeField]
		[EventRef]
		private string m_starsAppearAudioEventPath;

		// Token: 0x040054F5 RID: 21749
		[SerializeField]
		[EventRef]
		private string m_castleAppearsAudioEventPath;

		// Token: 0x040054F6 RID: 21750
		[SerializeField]
		[EventRef]
		private string m_textAppearsAudioEventPath;

		// Token: 0x040054F7 RID: 21751
		[SerializeField]
		[EventRef]
		private string m_continueLegacy;

		// Token: 0x040054F8 RID: 21752
		[SerializeField]
		[EventRef]
		private string m_newLegacy;

		// Token: 0x040054F9 RID: 21753
		[SerializeField]
		[EventRef]
		private string m_storyPoint00;

		// Token: 0x040054FA RID: 21754
		[SerializeField]
		[EventRef]
		private string m_storyPoint01;

		// Token: 0x040054FB RID: 21755
		[SerializeField]
		[EventRef]
		private string m_storyPoint02;

		// Token: 0x040054FC RID: 21756
		[SerializeField]
		[EventRef]
		private string m_storyPoint03;

		// Token: 0x040054FD RID: 21757
		[SerializeField]
		[EventRef]
		private string m_storyPoint04;

		// Token: 0x040054FE RID: 21758
		[SerializeField]
		[EventRef]
		private string m_storyPointBoatInWater;

		// Token: 0x040054FF RID: 21759
		[SerializeField]
		[EventRef]
		private string m_storyPointboatWavesAndSqueaks;

		// Token: 0x04005500 RID: 21760
		[SerializeField]
		[EventRef]
		private string m_storyPoint05;

		// Token: 0x04005501 RID: 21761
		[SerializeField]
		[EventRef]
		private string m_storyPoint06;

		// Token: 0x04005502 RID: 21762
		[SerializeField]
		[EventRef]
		private string m_storyPoint07;

		// Token: 0x04005503 RID: 21763
		[SerializeField]
		private MusicPlayer m_mainMenuMusicPlayer;

		// Token: 0x04005504 RID: 21764
		[SerializeField]
		private StudioEventEmitter m_mainMenuAmbientSoundEventEmitter;

		// Token: 0x04005505 RID: 21765
		private string m_description = string.Empty;

		// Token: 0x04005506 RID: 21766
		private MainMenuWindowController m_mainMenuWindowController;

		// Token: 0x04005507 RID: 21767
		private EventInstance m_startEventIntance;

		// Token: 0x04005508 RID: 21768
		private EventInstance m_blackScreenEventInstance;

		// Token: 0x04005509 RID: 21769
		private EventInstance m_starsAppearEventInstance;

		// Token: 0x0400550A RID: 21770
		private EventInstance m_castleAppearsEventInstance;

		// Token: 0x0400550B RID: 21771
		private bool m_isFastForwarding;

		// Token: 0x0400550C RID: 21772
		private List<EventInstance> m_currentEvents = new List<EventInstance>();

		// Token: 0x0400550D RID: 21773
		private float m_currentFastForwardValue;

		// Token: 0x0400550E RID: 21774
		private Coroutine m_fastForwardCoroutine;

		// Token: 0x0400550F RID: 21775
		private int m_textEntryNumber;
	}
}

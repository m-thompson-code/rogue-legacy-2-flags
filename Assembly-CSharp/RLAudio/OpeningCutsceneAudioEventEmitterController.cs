using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000902 RID: 2306
	public class OpeningCutsceneAudioEventEmitterController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x06004BA6 RID: 19366 RVA: 0x0010FE75 File Offset: 0x0010E075
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

		// Token: 0x06004BA7 RID: 19367 RVA: 0x0010FE9C File Offset: 0x0010E09C
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

		// Token: 0x06004BA8 RID: 19368 RVA: 0x00110020 File Offset: 0x0010E220
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

		// Token: 0x06004BA9 RID: 19369 RVA: 0x00110094 File Offset: 0x0010E294
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

		// Token: 0x06004BAA RID: 19370 RVA: 0x001100D4 File Offset: 0x0010E2D4
		private void OnCloudsAppear()
		{
			EventInstance eventInstance = AudioUtility.GetEventInstance(this.m_storyPointboatWavesAndSqueaks, base.transform);
			this.PlayEvent(eventInstance);
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x001100FA File Offset: 0x0010E2FA
		private void OnCutsceneBlack()
		{
			this.PlayEvent(this.m_blackScreenEventInstance);
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x00110108 File Offset: 0x0010E308
		private void OnCutsceneStarsAppear()
		{
			this.StopEventInstance(this.m_blackScreenEventInstance);
			this.PlayEvent(this.m_starsAppearEventInstance);
			EventInstance eventInstance = AudioUtility.GetEventInstance(this.m_storyPointBoatInWater, base.transform);
			this.PlayEvent(eventInstance);
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x00110146 File Offset: 0x0010E346
		private void StopEventInstance(EventInstance eventInstance)
		{
			this.m_currentEvents.Remove(eventInstance);
			AudioManager.Stop(eventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x0011015C File Offset: 0x0010E35C
		private void OnCutsceneDisplayTextCall(CanvasGroup canvasGroup)
		{
			base.StartCoroutine(this.PlayTextAudio(canvasGroup));
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x0011016C File Offset: 0x0010E36C
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

		// Token: 0x06004BB0 RID: 19376 RVA: 0x00110182 File Offset: 0x0010E382
		private void OnCutsceneCastleAppears()
		{
			this.PlayEvent(this.m_castleAppearsEventInstance);
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x00110190 File Offset: 0x0010E390
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

		// Token: 0x06004BB2 RID: 19378 RVA: 0x001101F7 File Offset: 0x0010E3F7
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

		// Token: 0x06004BB3 RID: 19379 RVA: 0x00110210 File Offset: 0x0010E410
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

		// Token: 0x06004BB4 RID: 19380 RVA: 0x00110270 File Offset: 0x0010E470
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

		// Token: 0x06004BB5 RID: 19381 RVA: 0x001102C2 File Offset: 0x0010E4C2
		private void PlayEvent(EventInstance eventInstance)
		{
			this.PlayEvent(eventInstance, Vector3.zero);
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x001102D0 File Offset: 0x0010E4D0
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

		// Token: 0x04003FA0 RID: 16288
		[SerializeField]
		[EventRef]
		private string m_startAudioEventPath;

		// Token: 0x04003FA1 RID: 16289
		[SerializeField]
		[EventRef]
		private string m_blackScreenAudioEventPath;

		// Token: 0x04003FA2 RID: 16290
		[SerializeField]
		[EventRef]
		private string m_starsAppearAudioEventPath;

		// Token: 0x04003FA3 RID: 16291
		[SerializeField]
		[EventRef]
		private string m_castleAppearsAudioEventPath;

		// Token: 0x04003FA4 RID: 16292
		[SerializeField]
		[EventRef]
		private string m_textAppearsAudioEventPath;

		// Token: 0x04003FA5 RID: 16293
		[SerializeField]
		[EventRef]
		private string m_continueLegacy;

		// Token: 0x04003FA6 RID: 16294
		[SerializeField]
		[EventRef]
		private string m_newLegacy;

		// Token: 0x04003FA7 RID: 16295
		[SerializeField]
		[EventRef]
		private string m_storyPoint00;

		// Token: 0x04003FA8 RID: 16296
		[SerializeField]
		[EventRef]
		private string m_storyPoint01;

		// Token: 0x04003FA9 RID: 16297
		[SerializeField]
		[EventRef]
		private string m_storyPoint02;

		// Token: 0x04003FAA RID: 16298
		[SerializeField]
		[EventRef]
		private string m_storyPoint03;

		// Token: 0x04003FAB RID: 16299
		[SerializeField]
		[EventRef]
		private string m_storyPoint04;

		// Token: 0x04003FAC RID: 16300
		[SerializeField]
		[EventRef]
		private string m_storyPointBoatInWater;

		// Token: 0x04003FAD RID: 16301
		[SerializeField]
		[EventRef]
		private string m_storyPointboatWavesAndSqueaks;

		// Token: 0x04003FAE RID: 16302
		[SerializeField]
		[EventRef]
		private string m_storyPoint05;

		// Token: 0x04003FAF RID: 16303
		[SerializeField]
		[EventRef]
		private string m_storyPoint06;

		// Token: 0x04003FB0 RID: 16304
		[SerializeField]
		[EventRef]
		private string m_storyPoint07;

		// Token: 0x04003FB1 RID: 16305
		[SerializeField]
		private MusicPlayer m_mainMenuMusicPlayer;

		// Token: 0x04003FB2 RID: 16306
		[SerializeField]
		private StudioEventEmitter m_mainMenuAmbientSoundEventEmitter;

		// Token: 0x04003FB3 RID: 16307
		private string m_description = string.Empty;

		// Token: 0x04003FB4 RID: 16308
		private MainMenuWindowController m_mainMenuWindowController;

		// Token: 0x04003FB5 RID: 16309
		private EventInstance m_startEventIntance;

		// Token: 0x04003FB6 RID: 16310
		private EventInstance m_blackScreenEventInstance;

		// Token: 0x04003FB7 RID: 16311
		private EventInstance m_starsAppearEventInstance;

		// Token: 0x04003FB8 RID: 16312
		private EventInstance m_castleAppearsEventInstance;

		// Token: 0x04003FB9 RID: 16313
		private bool m_isFastForwarding;

		// Token: 0x04003FBA RID: 16314
		private List<EventInstance> m_currentEvents = new List<EventInstance>();

		// Token: 0x04003FBB RID: 16315
		private float m_currentFastForwardValue;

		// Token: 0x04003FBC RID: 16316
		private Coroutine m_fastForwardCoroutine;

		// Token: 0x04003FBD RID: 16317
		private int m_textEntryNumber;
	}
}

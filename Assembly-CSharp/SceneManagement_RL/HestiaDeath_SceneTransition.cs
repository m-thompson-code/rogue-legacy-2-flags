using System;
using System.Collections;
using FMOD.Studio;
using Rewired;
using RLAudio;
using RL_Windows;
using TMPro;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x02000E20 RID: 3616
	public class HestiaDeath_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170020B7 RID: 8375
		// (get) Token: 0x060065ED RID: 26093 RVA: 0x00038288 File Offset: 0x00036488
		protected virtual string AmbienceSFXName
		{
			get
			{
				return "event:/Environment/amb_heirloomSpace_withPipes";
			}
		}

		// Token: 0x170020B8 RID: 8376
		// (get) Token: 0x060065EE RID: 26094 RVA: 0x00004A07 File Offset: 0x00002C07
		public override TransitionID ID
		{
			get
			{
				return TransitionID.HestiaDeath;
			}
		}

		// Token: 0x060065EF RID: 26095 RVA: 0x00179D9C File Offset: 0x00177F9C
		protected override void Awake()
		{
			base.Awake();
			this.m_imageCanvasGroup.alpha = 0f;
			this.m_bgCanvasGroup.alpha = 0f;
			this.m_text1.alpha = 0f;
			this.m_text2.alpha = 0f;
			this.m_waitYield = new WaitRL_Yield(0f, false);
			this.m_fastForwardObj.SetActive(false);
			this.m_toggleSpeedUp = new Action<InputActionEventData>(this.ToggleSpeedUp);
		}

		// Token: 0x060065F0 RID: 26096 RVA: 0x0003828F File Offset: 0x0003648F
		private void OnEnable()
		{
			Rewired_RL.Player.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			Rewired_RL.Player.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
		}

		// Token: 0x060065F1 RID: 26097 RVA: 0x000382BF File Offset: 0x000364BF
		private void OnDisable()
		{
			if (!GameManager.IsApplicationClosing)
			{
				Rewired_RL.Player.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
				Rewired_RL.Player.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
			}
		}

		// Token: 0x060065F2 RID: 26098 RVA: 0x000382F6 File Offset: 0x000364F6
		private void OnDestroy()
		{
			if (this.m_ambienceSFXEventInstance.isValid())
			{
				this.m_ambienceSFXEventInstance.release();
			}
		}

		// Token: 0x060065F3 RID: 26099 RVA: 0x00179E20 File Offset: 0x00178020
		protected virtual void InitializeText()
		{
			int num = Mathf.Min((int)SaveManager.PlayerSaveData.HestiaCutsceneDisplayCount, Ending_EV.HESTIA_CUTSCENE_DIALOGUE_1_LOCID.Length - 1);
			this.m_text1.text = LocalizationManager.GetString(Ending_EV.HESTIA_CUTSCENE_DIALOGUE_1_LOCID[num], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			this.m_text2.text = LocalizationManager.GetString(Ending_EV.HESTIA_CUTSCENE_DIALOGUE_2_LOCID[num], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Play_Hestia_DeathCutscene, false);
			global::PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
			playerSaveData.HestiaCutsceneDisplayCount += 1;
		}

		// Token: 0x060065F4 RID: 26100 RVA: 0x00038311 File Offset: 0x00036511
		public IEnumerator TransitionIn()
		{
			if (!this.m_ambienceSFXEventInstance.isValid())
			{
				this.m_ambienceSFXEventInstance = AudioUtility.GetEventInstance(this.AmbienceSFXName, CameraController.UICamera.transform);
			}
			this.InitializeText();
			this.m_imageCanvasGroup.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			this.m_imageCanvasGroup.alpha = 0f;
			this.m_bgCanvasGroup.alpha = 0f;
			this.m_text1.alpha = 0f;
			this.m_text2.alpha = 0f;
			this.m_allowSpeedUp = false;
			this.m_fastForwardObj.SetActive(false);
			this.m_timeScale = 1f;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_bgCanvasGroup, 1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			MapController.DeathMapCamera.gameObject.SetActive(false);
			yield return this.Wait(0.5f);
			(WindowManager.GetWindowController(WindowID.PlayerDeath) as PlayerDeathWindowController).StopDeathSnapshot();
			AudioManager.StopAllAudio(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			if (this.m_ambienceSFXEventInstance.isValid())
			{
				AudioManager.Play(null, this.m_ambienceSFXEventInstance);
			}
			this.m_timeScale = 1f;
			this.m_allowSpeedUp = true;
			RewiredMapController.SetMapEnabled(GameInputMode.Window, true);
			base.StartCoroutine(this.LocalScale(this.m_imageCanvasGroup.transform, 12f, 1f));
			base.StartCoroutine(this.Alpha(this.m_imageCanvasGroup, 1f, 1f));
			yield return this.Wait(2f);
			AudioManager.PlayOneShot(null, "event:/Cut_Scenes/sfx_openingCutscene_textIn", CameraController.UICamera.transform.position);
			base.StartCoroutine(this.Alpha(this.m_text1, 1f, 1f));
			yield return this.Wait(4f);
			AudioManager.PlayOneShot(null, "event:/Cut_Scenes/sfx_openingCutscene_textIn", CameraController.UICamera.transform.position);
			base.StartCoroutine(this.Alpha(this.m_text2, 1f, 1f));
			yield return this.Wait(6f);
			if (this.m_ambienceSFXEventInstance.isValid())
			{
				AudioManager.Stop(this.m_ambienceSFXEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			base.StartCoroutine(this.Alpha(this.m_text1, 1f, 0f));
			base.StartCoroutine(this.Alpha(this.m_text2, 1f, 0f));
			yield return this.Alpha(this.m_imageCanvasGroup, 1f, 0f);
			this.m_allowSpeedUp = false;
			this.m_timeScale = 1f;
			RewiredMapController.SetMapEnabled(GameInputMode.Window, false);
			this.m_fastForwardObj.SetActive(false);
			yield break;
		}

		// Token: 0x060065F5 RID: 26101 RVA: 0x00038320 File Offset: 0x00036520
		private IEnumerator LocalScale(Transform transform, float duration, float amount)
		{
			float elapsedTime = 0f;
			Vector3 localScale = transform.localScale;
			float startingValue = localScale.x;
			float endValue = amount - startingValue;
			while (elapsedTime < duration)
			{
				yield return null;
				elapsedTime += Time.unscaledDeltaTime * this.m_timeScale;
				float value = this.GetValue(new EaseDelegate(Ease.None), elapsedTime, startingValue, endValue, duration);
				localScale.x = (localScale.y = (localScale.z = value));
				transform.localScale = localScale;
			}
			localScale.z = amount;
			localScale.y = amount;
			localScale.x = amount;
			transform.localScale = localScale;
			yield break;
		}

		// Token: 0x060065F6 RID: 26102 RVA: 0x00038344 File Offset: 0x00036544
		private IEnumerator Alpha(CanvasGroup canvasGroup, float duration, float amount)
		{
			float elapsedTime = 0f;
			float startingAlpha = canvasGroup.alpha;
			float endAlpha = amount - startingAlpha;
			while (elapsedTime < duration)
			{
				yield return null;
				elapsedTime += Time.unscaledDeltaTime * this.m_timeScale;
				canvasGroup.alpha = this.GetValue(new EaseDelegate(Ease.None), elapsedTime, startingAlpha, endAlpha, duration);
			}
			canvasGroup.alpha = amount;
			yield break;
		}

		// Token: 0x060065F7 RID: 26103 RVA: 0x00038368 File Offset: 0x00036568
		private IEnumerator Alpha(TMP_Text text, float duration, float amount)
		{
			float elapsedTime = 0f;
			float startingAlpha = text.alpha;
			float endAlpha = amount - startingAlpha;
			while (elapsedTime < duration)
			{
				yield return null;
				elapsedTime += Time.unscaledDeltaTime * this.m_timeScale;
				text.alpha = this.GetValue(new EaseDelegate(Ease.None), elapsedTime, startingAlpha, endAlpha, duration);
			}
			text.alpha = amount;
			yield break;
		}

		// Token: 0x060065F8 RID: 26104 RVA: 0x0003838C File Offset: 0x0003658C
		private float GetValue(EaseDelegate ease, float elapsedTime, float startingValue, float endValue, float totalDuration)
		{
			return ease(elapsedTime, startingValue, endValue, totalDuration);
		}

		// Token: 0x060065F9 RID: 26105 RVA: 0x0003839A File Offset: 0x0003659A
		private IEnumerator Wait(float duration)
		{
			for (float elapsedTime = 0f; elapsedTime < duration; elapsedTime += Time.unscaledDeltaTime * this.m_timeScale)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060065FA RID: 26106 RVA: 0x000383B0 File Offset: 0x000365B0
		public IEnumerator TransitionOut()
		{
			yield return null;
			this.m_waitYield.CreateNew(0.5f, true);
			yield return this.m_waitYield;
			yield return TweenManager.TweenTo_UnscaledTime(this.m_bgCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			}).TweenCoroutine;
			yield break;
		}

		// Token: 0x060065FB RID: 26107 RVA: 0x000383BF File Offset: 0x000365BF
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x060065FC RID: 26108 RVA: 0x00179EB8 File Offset: 0x001780B8
		private void ToggleSpeedUp(InputActionEventData eventData)
		{
			if (!this.m_allowSpeedUp)
			{
				return;
			}
			if (eventData.GetButtonUp())
			{
				this.m_timeScale = 1f;
				this.m_fastForwardObj.SetActive(false);
				return;
			}
			this.m_timeScale = 10f;
			this.m_fastForwardObj.SetActive(true);
		}

		// Token: 0x040052CE RID: 21198
		[SerializeField]
		private CanvasGroup m_bgCanvasGroup;

		// Token: 0x040052CF RID: 21199
		[SerializeField]
		private CanvasGroup m_imageCanvasGroup;

		// Token: 0x040052D0 RID: 21200
		[SerializeField]
		protected TMP_Text m_text1;

		// Token: 0x040052D1 RID: 21201
		[SerializeField]
		protected TMP_Text m_text2;

		// Token: 0x040052D2 RID: 21202
		[SerializeField]
		private GameObject m_fastForwardObj;

		// Token: 0x040052D3 RID: 21203
		private WaitRL_Yield m_waitYield;

		// Token: 0x040052D4 RID: 21204
		private bool m_allowSpeedUp;

		// Token: 0x040052D5 RID: 21205
		private Action<InputActionEventData> m_toggleSpeedUp;

		// Token: 0x040052D6 RID: 21206
		private EventInstance m_ambienceSFXEventInstance;

		// Token: 0x040052D7 RID: 21207
		private float m_timeScale = 1f;
	}
}

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
	// Token: 0x020008C6 RID: 2246
	public class HestiaDeath_SceneTransition : Transition_V2, ISceneLoadingTransition, ITransition
	{
		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x060049B2 RID: 18866 RVA: 0x00109D8F File Offset: 0x00107F8F
		protected virtual string AmbienceSFXName
		{
			get
			{
				return "event:/Environment/amb_heirloomSpace_withPipes";
			}
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x060049B3 RID: 18867 RVA: 0x00109D96 File Offset: 0x00107F96
		public override TransitionID ID
		{
			get
			{
				return TransitionID.HestiaDeath;
			}
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x00109D9C File Offset: 0x00107F9C
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

		// Token: 0x060049B5 RID: 18869 RVA: 0x00109E1E File Offset: 0x0010801E
		private void OnEnable()
		{
			Rewired_RL.Player.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			Rewired_RL.Player.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x00109E4E File Offset: 0x0010804E
		private void OnDisable()
		{
			if (!GameManager.IsApplicationClosing)
			{
				Rewired_RL.Player.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
				Rewired_RL.Player.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
			}
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x00109E85 File Offset: 0x00108085
		private void OnDestroy()
		{
			if (this.m_ambienceSFXEventInstance.isValid())
			{
				this.m_ambienceSFXEventInstance.release();
			}
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x00109EA0 File Offset: 0x001080A0
		protected virtual void InitializeText()
		{
			int num = Mathf.Min((int)SaveManager.PlayerSaveData.HestiaCutsceneDisplayCount, Ending_EV.HESTIA_CUTSCENE_DIALOGUE_1_LOCID.Length - 1);
			this.m_text1.text = LocalizationManager.GetString(Ending_EV.HESTIA_CUTSCENE_DIALOGUE_1_LOCID[num], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			this.m_text2.text = LocalizationManager.GetString(Ending_EV.HESTIA_CUTSCENE_DIALOGUE_2_LOCID[num], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Play_Hestia_DeathCutscene, false);
			global::PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
			playerSaveData.HestiaCutsceneDisplayCount += 1;
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x00109F37 File Offset: 0x00108137
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

		// Token: 0x060049BA RID: 18874 RVA: 0x00109F46 File Offset: 0x00108146
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

		// Token: 0x060049BB RID: 18875 RVA: 0x00109F6A File Offset: 0x0010816A
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

		// Token: 0x060049BC RID: 18876 RVA: 0x00109F8E File Offset: 0x0010818E
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

		// Token: 0x060049BD RID: 18877 RVA: 0x00109FB2 File Offset: 0x001081B2
		private float GetValue(EaseDelegate ease, float elapsedTime, float startingValue, float endValue, float totalDuration)
		{
			return ease(elapsedTime, startingValue, endValue, totalDuration);
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x00109FC0 File Offset: 0x001081C0
		private IEnumerator Wait(float duration)
		{
			for (float elapsedTime = 0f; elapsedTime < duration; elapsedTime += Time.unscaledDeltaTime * this.m_timeScale)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x00109FD6 File Offset: 0x001081D6
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

		// Token: 0x060049C0 RID: 18880 RVA: 0x00109FE5 File Offset: 0x001081E5
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x00109FF0 File Offset: 0x001081F0
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

		// Token: 0x04003E07 RID: 15879
		[SerializeField]
		private CanvasGroup m_bgCanvasGroup;

		// Token: 0x04003E08 RID: 15880
		[SerializeField]
		private CanvasGroup m_imageCanvasGroup;

		// Token: 0x04003E09 RID: 15881
		[SerializeField]
		protected TMP_Text m_text1;

		// Token: 0x04003E0A RID: 15882
		[SerializeField]
		protected TMP_Text m_text2;

		// Token: 0x04003E0B RID: 15883
		[SerializeField]
		private GameObject m_fastForwardObj;

		// Token: 0x04003E0C RID: 15884
		private WaitRL_Yield m_waitYield;

		// Token: 0x04003E0D RID: 15885
		private bool m_allowSpeedUp;

		// Token: 0x04003E0E RID: 15886
		private Action<InputActionEventData> m_toggleSpeedUp;

		// Token: 0x04003E0F RID: 15887
		private EventInstance m_ambienceSFXEventInstance;

		// Token: 0x04003E10 RID: 15888
		private float m_timeScale = 1f;
	}
}

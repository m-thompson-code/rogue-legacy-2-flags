using System;
using System.Collections;
using System.IO;
using FMODUnity;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
public class SplashSceneLoader : MonoBehaviour
{
	// Token: 0x0600280B RID: 10251 RVA: 0x00016818 File Offset: 0x00014A18
	private IEnumerator Start()
	{
		Application.backgroundLoadingPriority = ThreadPriority.High;
		RuntimeManager.LoadBank("Master.strings", false);
		RuntimeManager.LoadBank("Master", true);
		RuntimeManager.LoadBank("Music", false);
		float startTime = Time.time;
		float duration = 0.2f;
		while (Time.time < startTime + duration)
		{
			float t = Time.time - startTime;
			this.m_loadingIndicatorCanvasGroup.alpha = Ease.Quad.EaseOut(t, 0f, 1f, duration);
			yield return null;
		}
		CDGResources.Init(false);
		while (CDGResources.IsLoading)
		{
			yield return null;
		}
		while (RuntimeManager.AnyBankLoading())
		{
			yield return null;
		}
		yield return this.m_sharedGameObjectsLoader.Initialize();
		yield return null;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		yield return TweenManager.TweenTo_UnscaledTime(this.m_loadingIndicatorCanvasGroup, 0.2f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		if (File.Exists(SaveManager.GetConfigPath()))
		{
			while (!SaveManager.IsConfigFileLoaded)
			{
				yield return null;
			}
		}
		float volume = SaveManager.ConfigData.MasterVolume * SaveManager.ConfigData.SFXVolume;
		this.m_audioEmitter.Play();
		this.m_audioEmitter.EventInstance.setVolume(volume);
		yield return TweenManager.TweenTo_UnscaledTime(this.m_canvasGroup, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"alpha",
			1
		});
		yield return new WaitForSecondsRealtime(3f);
		yield return this.WaitUntilStoreInitialized();
		if (StoreAPIManager.InitState == StoreAPIManager.StoreInitState.Succeeded)
		{
			this.LoadScene();
		}
		yield break;
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x00016827 File Offset: 0x00014A27
	private IEnumerator WaitUntilStoreInitialized()
	{
		float num = StoreAPIManager.GetInitTimeout();
		if (num <= 0f)
		{
			num = float.PositiveInfinity;
		}
		float timeoutTime = Time.unscaledTime + num;
		while (StoreAPIManager.InitState == StoreAPIManager.StoreInitState.LoggingIn)
		{
			if (Time.unscaledTime > timeoutTime)
			{
				this.DisplayLoginFailedWindow();
				yield break;
			}
			yield return null;
		}
		if (StoreAPIManager.InitState == StoreAPIManager.StoreInitState.Failed)
		{
			this.DisplayLoginFailedWindow();
			yield break;
		}
		yield break;
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x00016836 File Offset: 0x00014A36
	private void DisplayLoginFailedWindow()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		this.InitializeAPIFailMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x000BC950 File Offset: 0x000BAB50
	private void InitializeAPIFailMenu()
	{
		string text;
		string text2;
		StoreAPIManager.GetInitFailureMessage(out text, out text2);
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText(text, false);
		confirmMenuWindowController.SetDescriptionText(text2, false);
		confirmMenuWindowController.SetNumberOfButtons(1);
		confirmMenuWindowController.SetOnCancelAction(new Action(this.QuitGame));
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(new Action(this.QuitGame));
		Canvas componentInChildren = confirmMenuWindowController.GetComponentInChildren<Canvas>(true);
		componentInChildren.renderMode = RenderMode.ScreenSpaceOverlay;
		componentInChildren.sortingOrder = 10;
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x000BC9D4 File Offset: 0x000BABD4
	private void Update()
	{
		Vector3 localEulerAngles = this.m_rotatingGear.localEulerAngles;
		localEulerAngles.z -= 30f * Time.unscaledDeltaTime;
		this.m_rotatingGear.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x00016856 File Offset: 0x00014A56
	private void QuitGame()
	{
		Application.Quit(10);
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x0001685F File Offset: 0x00014A5F
	private void LoadScene()
	{
		SceneLoader_RL.LoadScene(SceneID.Disclaimer, this.m_transitionID);
	}

	// Token: 0x04002345 RID: 9029
	[SerializeField]
	private TransitionID m_transitionID;

	// Token: 0x04002346 RID: 9030
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04002347 RID: 9031
	[SerializeField]
	private StudioEventEmitter m_audioEmitter;

	// Token: 0x04002348 RID: 9032
	[SerializeField]
	private SharedGameObjects_Loader m_sharedGameObjectsLoader;

	// Token: 0x04002349 RID: 9033
	[SerializeField]
	private CanvasGroup m_loadingIndicatorCanvasGroup;

	// Token: 0x0400234A RID: 9034
	[SerializeField]
	private RectTransform m_rotatingGear;
}

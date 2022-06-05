using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Rewired;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

// Token: 0x02000581 RID: 1409
public class MainMenuWindowController : WindowController
{
	// Token: 0x17001292 RID: 4754
	// (get) Token: 0x0600340F RID: 13327 RVA: 0x000B1BCC File Offset: 0x000AFDCC
	// (set) Token: 0x06003410 RID: 13328 RVA: 0x000B1BD4 File Offset: 0x000AFDD4
	public float TextAlphaMod { get; set; }

	// Token: 0x17001293 RID: 4755
	// (get) Token: 0x06003411 RID: 13329 RVA: 0x000B1BDD File Offset: 0x000AFDDD
	public override WindowID ID
	{
		get
		{
			return WindowID.MainMenu;
		}
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x000B1BE4 File Offset: 0x000AFDE4
	private void Awake()
	{
		this.m_failedLoadQuitGame = new Action(this.FailedLoadQuitGame);
		this.m_confirmMenu_Cancel = new Action(this.ConfirmMenu_Cancel);
		this.m_switchProfile = new Action(this.SwitchProfile);
		this.m_loadBackupWindow = new Action(this.LoadBackupWindow);
		this.m_confirmWorldReset_Confirm = new Action(this.ConfirmWorldReset_Confirm);
		this.m_onConfirmInputHandler = new Action<InputActionEventData>(this.OnConfirmInputHandler);
		this.m_onVerticalInputHandler = new Action<InputActionEventData>(this.OnVerticalInputHandler);
		this.m_toggleSpeedUp = new Action<InputActionEventData>(this.ToggleSpeedUp);
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x000B1C84 File Offset: 0x000AFE84
	public override void Initialize()
	{
		base.Initialize();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_menuDisplayed = false;
		this.m_versionNumberText.text = System_EV.GetVersionString();
		this.m_updateNumberText.text = 32.ToString();
		this.m_menuButtonList = new List<MainMenuButton>();
		base.GetComponentsInChildren<MainMenuButton>(true, this.m_menuButtonList);
		for (int i = 0; i < this.m_menuButtonList.Count; i++)
		{
			MainMenuButton mainMenuButton = this.m_menuButtonList[i];
			mainMenuButton.Initialize(this, i);
			mainMenuButton.MenuButtonSelected = (MainMenuButtonSelectedHandler)Delegate.Combine(mainMenuButton.MenuButtonSelected, new MainMenuButtonSelectedHandler(this.UpdateSelectedOptionItem));
			mainMenuButton.MenuButtonActivated = (MainMenuButtonSelectedHandler)Delegate.Combine(mainMenuButton.MenuButtonActivated, new MainMenuButtonSelectedHandler(this.PlaySelectedSFX));
		}
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x000B1D59 File Offset: 0x000AFF59
	protected override void OnOpen()
	{
		CameraController.GameCamera.transform.position = new Vector3(-60.07f, 13.03f, -25f);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x000B1D8B File Offset: 0x000AFF8B
	private IEnumerator OnOpenCoroutine()
	{
		Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.EnterMainMenu, this, EventArgs.Empty);
		this.m_lockInput = false;
		this.InitializeIntroCutscene();
		this.m_startingButton.OnSelect(null);
		this.m_windowCanvas.gameObject.SetActive(true);
		float saveTimer = Time.unscaledTime + 30f;
		while (SaveFileSystem.IsSaving && Time.unscaledTime < saveTimer)
		{
			yield return null;
		}
		SaveManager.LoadCurrentProfileData();
		if (SaveManager.LoadingFailed)
		{
			base.StartCoroutine(this.DisplayLoadingFailedCoroutine());
		}
		if (this.m_newGamePlusChanger)
		{
			this.m_newGamePlusChanger.UpdateMainMenu();
		}
		if (!this.m_menuDisplayed)
		{
			this.m_isLoading = false;
			this.TextAlphaMod = 1f;
			this.m_menuButtonList[0].OnSelect(null);
			this.m_part1Canvas.gameObject.SetActive(true);
			this.m_part2Canvas.gameObject.SetActive(false);
			this.m_part1Canvas.alpha = 1f;
			this.m_part2Canvas.alpha = 0f;
			this.m_logoAnimator.SetTrigger("Appear");
			this.m_part2Canvas.interactable = false;
		}
		else
		{
			this.m_logoAnimator.SetTrigger("InstantDisappear");
			this.m_part1Canvas.gameObject.SetActive(false);
			this.m_part2Canvas.gameObject.SetActive(true);
			this.m_part1Canvas.alpha = 0f;
			this.m_part2Canvas.alpha = 1f;
			this.m_part2Canvas.interactable = true;
		}
		this.UpdateSelectedProfileText();
		yield break;
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x000B1D9A File Offset: 0x000AFF9A
	private void UpdateSelectedProfileText()
	{
		this.m_profileSlotText.text = string.Format(LocalizationManager.GetString("LOC_ID_MAIN_MENU_PROFILE_SLOT_1", false, false), SaveManager.CurrentProfile + 1);
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x000B1DC4 File Offset: 0x000AFFC4
	private void InitializeIntroCutscene()
	{
		CanvasGroup[] introTextCanvasGroups = this.m_introTextCanvasGroups;
		for (int i = 0; i < introTextCanvasGroups.Length; i++)
		{
			introTextCanvasGroups[i].alpha = 0f;
		}
		this.m_playableDirector.time = 0.0;
		this.m_allowFastForward = false;
		this.m_fastForwardObj.SetActive(false);
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x000B1E1A File Offset: 0x000B001A
	private IEnumerator DisplayLoadingFailedCoroutine()
	{
		yield return null;
		while (SceneLoader_RL.IsLoading)
		{
			yield return null;
		}
		this.InitializeLoadingFailedConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		yield break;
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x000B1E29 File Offset: 0x000B0029
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x000B1E3C File Offset: 0x000B003C
	private void InitializeLoadingFailedConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		if (SaveManager.GetBackupPathes(SaveManager.CurrentProfile, SaveDataType.Player).Count <= 0)
		{
			ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
			confirmMenuWindowController.SetTitleText("LOC_ID_MAIN_MENU_SAVE_FILE_ERROR_TITLE_1", true);
			confirmMenuWindowController.SetDescriptionText("LOC_ID_MAIN_MENU_SAVE_FILE_ERROR_DESCRIPTION_1", true);
			confirmMenuWindowController.SetNumberOfButtons(2);
			confirmMenuWindowController.SetOnCancelAction(this.m_failedLoadQuitGame);
			ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
			buttonAtIndex.SetButtonText("LOC_ID_MAIN_MENU_SWITCH_PROFILE_1", true);
			buttonAtIndex.SetOnClickAction(this.m_switchProfile);
			ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
			buttonAtIndex2.SetButtonText("LOC_ID_MAIN_MENU_QUIT GAME_1", true);
			buttonAtIndex2.SetOnClickAction(this.m_failedLoadQuitGame);
			return;
		}
		ConfirmMenuWindowController confirmMenuWindowController2 = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController2.SetTitleText("LOC_ID_MAIN_MENU_SAVE_FILE_ERROR_TITLE_1", true);
		confirmMenuWindowController2.SetDescriptionText("LOC_ID_MAIN_MENU_SAVE_FILE_ERROR_DESCRIPTION_2", true);
		confirmMenuWindowController2.SetNumberOfButtons(2);
		confirmMenuWindowController2.SetOnCancelAction(this.m_failedLoadQuitGame);
		ConfirmMenu_Button buttonAtIndex3 = confirmMenuWindowController2.GetButtonAtIndex(0);
		buttonAtIndex3.SetButtonText("LOC_ID_MAIN_MENU_LOAD_BACKUP_1", true);
		buttonAtIndex3.SetOnClickAction(this.m_loadBackupWindow);
		ConfirmMenu_Button buttonAtIndex4 = confirmMenuWindowController2.GetButtonAtIndex(1);
		buttonAtIndex4.SetButtonText("LOC_ID_MAIN_MENU_QUIT GAME_1", true);
		buttonAtIndex4.SetOnClickAction(this.m_failedLoadQuitGame);
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x000B1F51 File Offset: 0x000B0151
	private void SwitchProfile()
	{
		SaveManager.LoadingFailed = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.MainMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, false);
		WindowManager.SetWindowIsOpen(WindowID.ProfileSelect, true);
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x000B1F79 File Offset: 0x000B0179
	private void FailedLoadQuitGame()
	{
		Application.Quit(10);
		Application.Quit(30);
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x000B1F89 File Offset: 0x000B0189
	private void LoadBackupWindow()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, true);
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x000B1F9C File Offset: 0x000B019C
	protected override void OnFocus()
	{
		this.AddInputListeners();
		if (!SaveManager.PlayerSaveData.HasStartedGame)
		{
			this.m_startingButton.Text.text = LocalizationManager.GetString("LOC_ID_MAIN_MENU_START_NEW_LEGACY_1", false, false);
			return;
		}
		int newGamePlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
		if (newGamePlusLevel <= 0)
		{
			this.m_startingButton.Text.text = LocalizationManager.GetString("LOC_ID_MAIN_MENU_CONTINUE_LEGACY_1", false, false);
			return;
		}
		this.m_startingButton.Text.text = LocalizationManager.GetString("LOC_ID_MAIN_MENU_CONTINUE_LEGACY_1", false, false) + " +" + newGamePlusLevel.ToString();
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x000B2031 File Offset: 0x000B0231
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x000B203C File Offset: 0x000B023C
	private void DisplayMenu(bool animate)
	{
		if (!animate)
		{
			foreach (MainMenuButton mainMenuButton in this.m_menuButtonList)
			{
				mainMenuButton.Interactable = true;
			}
			this.m_menuDisplayed = true;
			this.m_part1Canvas.alpha = 0f;
			this.m_part2Canvas.alpha = 1f;
			this.m_part1Canvas.gameObject.SetActive(false);
			this.m_part2Canvas.gameObject.SetActive(true);
			this.m_logoAnimator.SetTrigger("InstantDisappear");
			this.m_part2Canvas.interactable = true;
			return;
		}
		this.m_part1Canvas.alpha = 1f;
		this.m_part2Canvas.alpha = 0f;
		this.m_part1Canvas.gameObject.SetActive(true);
		this.m_part2Canvas.gameObject.SetActive(true);
		this.m_part2Canvas.interactable = false;
		base.StartCoroutine(this.DisplayMenuAnimCoroutine());
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x000B2154 File Offset: 0x000B0354
	private IEnumerator DisplayMenuAnimCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_logoAnimator.SetTrigger("Disappear");
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_titleScreen_start", default(Vector3));
		TweenManager.TweenTo_UnscaledTime(this.m_part1Canvas, 0.25f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_part2Canvas, 1f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_part1Canvas.gameObject.SetActive(false);
		foreach (MainMenuButton mainMenuButton in this.m_menuButtonList)
		{
			mainMenuButton.Interactable = true;
		}
		this.m_menuDisplayed = true;
		this.m_part2Canvas.interactable = true;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x000B2164 File Offset: 0x000B0364
	public void LoadGame()
	{
		JukeboxShop.CanSubmitStoreAchievements = true;
		bool hasStartedGame = SaveManager.PlayerSaveData.HasStartedGame;
		this.LoadGameRelay.Dispatch(hasStartedGame);
		if (!SaveManager.PlayerSaveData.HasStartedGame)
		{
			this.m_part2Canvas.interactable = false;
			foreach (MainMenuButton mainMenuButton in this.m_menuButtonList)
			{
				mainMenuButton.Interactable = false;
			}
			base.StopAllCoroutines();
			this.m_isLoading = true;
			base.StartCoroutine(this.TutorialSceneCoroutine());
			return;
		}
		if (SaveManager.PlayerSaveData.IsDead)
		{
			Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.ExitMainMenu, this, EventArgs.Empty);
			SceneLoader_RL.LoadScene(SceneID.Lineage, TransitionID.FadeToBlackWithLoading);
			return;
		}
		if (SaveManager.PlayerSaveData.EndingSpawnRoom >= EndingSpawnRoomType.Hallway)
		{
			WorldBuilder.FirstBiomeOverride = BiomeType.Garden;
			SceneLoader_RL.LoadScene(SceneID.World, TransitionID.FadeToBlackWithLoading);
			return;
		}
		WorldBuilder.FirstBiomeOverride = BiomeType.None;
		if (!SaveManager.PlayerSaveData.InCastle || !SaveManager.DoesSaveExist(SaveManager.CurrentProfile, SaveDataType.Stage, false))
		{
			SceneLoader_RL.LoadScene(SceneID.Town, TransitionID.FadeToBlackWithLoading);
			return;
		}
		if (SaveManager.StageSaveData.ForceResetWorld)
		{
			this.InitializeDeleteWorldConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
			return;
		}
		SceneLoader_RL.LoadScene(SceneID.World, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x000B2294 File Offset: 0x000B0494
	private void InitializeDeleteWorldConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_MAIN_MENU_WORLD_CHANGED_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_MAIN_MENU_WORLD_CHANGED_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_confirmMenu_Cancel);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_MAIN_MENU_WORLD_CHANGED_BUTTON_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmWorldReset_Confirm);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_MAIN_MENU_WORLD_CHANGED_BUTTON_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_confirmMenu_Cancel);
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x000B2323 File Offset: 0x000B0523
	private void ConfirmWorldReset_Confirm()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		PlayerDeathWindowController.GlobalCharacterDeathResetLogic();
		SaveManager.PlayerSaveData.IsDead = true;
		SceneLoader_RL.LoadScene(SceneID.Lineage, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x000B2345 File Offset: 0x000B0545
	private void ConfirmMenu_Cancel()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x000B234F File Offset: 0x000B054F
	private IEnumerator TutorialSceneCoroutine()
	{
		this.m_lockInput = true;
		TweenManager.TweenTo(this.m_part2Canvas, 0.25f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		TweenManager.TweenTo(this.m_logoImage, 1f, new EaseDelegate(Ease.None), new object[]
		{
			"color.a",
			0
		});
		TweenManager.TweenBy(this.m_logoImage.rectTransform, 1f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"anchoredPosition.y",
			100
		});
		TweenManager.TweenTo(this, 1f, new EaseDelegate(Ease.None), new object[]
		{
			"TextAlphaMod",
			0
		});
		TweenManager.TweenTo(this.m_versionNumberText, 1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		TweenManager.TweenTo(this.m_boilerText, 1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		TweenManager.TweenTo(this.m_platformUsernameText, 1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return this.IntroCutsceneCoroutine();
		SaveManager.PlayerSaveData.HasStartedGame = true;
		TutorialRoomController.InitializeTutorialPlayer = true;
		SceneLoader_RL.LoadScene(SceneID.Tutorial, TransitionID.FadeToBlackWithLoading);
		yield break;
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x000B235E File Offset: 0x000B055E
	private IEnumerator IntroCutsceneCoroutine()
	{
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		this.CutsceneStartRelay.Dispatch();
		this.m_playableDirector.Play();
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		this.m_allowFastForward = true;
		while (this.m_playableDirector.time < 4.0)
		{
			yield return null;
		}
		this.CutsceneBlackRelay.Dispatch();
		this.m_postProcessing.SetBaseProfile(this.m_undergroundPPProfile);
		this.m_playableDirector.Pause();
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		int num = 5;
		for (int i = 0; i < 5; i++)
		{
			this.DisplayIntroText(this.m_introTextCanvasGroups[i], 3.5f, (float)(i * 3));
		}
		this.m_waitYield.CreateNew((float)(num * 3 + 3), false);
		yield return this.m_waitYield;
		this.CutsceneStarsAppearRelay.Dispatch();
		this.m_playableDirector.Resume();
		while (this.m_playableDirector.time < 10.0)
		{
			yield return null;
		}
		this.CutsceneCastleAppearsRelay.Dispatch();
		while (this.m_playableDirector.time < 10.5)
		{
			yield return null;
		}
		this.CloudsAppearRelay.Dispatch();
		while (this.m_playableDirector.time < 17.0)
		{
			yield return null;
		}
		yield return this.DisplayIntroTextCoroutine(6, 3f);
		this.m_waitYield.CreateNew(3f, false);
		yield return this.m_waitYield;
		yield return this.DisplayIntroTextCoroutine(7, 3f);
		this.m_waitYield.CreateNew(3f, false);
		yield return this.m_waitYield;
		this.CutsceneCompleteRelay.Dispatch();
		yield break;
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x000B2370 File Offset: 0x000B0570
	private void DisplayIntroText(int index, float duration)
	{
		CanvasGroup canvasGroup = this.m_introTextCanvasGroups[index];
		this.CutsceneDisplayTextRelay.Dispatch(canvasGroup);
		TweenManager.TweenTo(canvasGroup, 0.5f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"alpha",
			1
		});
		TweenManager.TweenTo(canvasGroup, 0.5f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"delay",
			duration,
			"alpha",
			0
		});
	}

	// Token: 0x06003429 RID: 13353 RVA: 0x000B2404 File Offset: 0x000B0604
	private void DisplayIntroText(CanvasGroup textCanvasGroup, float duration, float delay)
	{
		this.CutsceneDisplayTextRelay.Dispatch(textCanvasGroup);
		TweenManager.TweenTo(textCanvasGroup, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"delay",
			delay,
			"alpha",
			1
		});
		TweenManager.TweenTo(textCanvasGroup, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"delay",
			duration + delay,
			"alpha",
			0
		});
	}

	// Token: 0x0600342A RID: 13354 RVA: 0x000B249F File Offset: 0x000B069F
	private IEnumerator DisplayIntroTextCoroutine(int index, float duration)
	{
		CanvasGroup textCanvasGroup = this.m_introTextCanvasGroups[index];
		TweenManager.TweenTo(textCanvasGroup, 0.5f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"alpha",
			1
		});
		this.CutsceneDisplayTextRelay.Dispatch(textCanvasGroup);
		this.m_waitYield.CreateNew(duration, false);
		yield return this.m_waitYield;
		TweenManager.TweenTo(textCanvasGroup, 0.5f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"alpha",
			0
		});
		yield break;
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x000B24BC File Offset: 0x000B06BC
	private void ToggleSpeedUp(InputActionEventData eventData)
	{
		if (this.m_allowFastForward)
		{
			if (!eventData.GetButtonUp())
			{
				this.CutsceneFastForwardRelay.Dispatch(true);
				RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 10f);
				this.m_fastForwardObj.SetActive(true);
				return;
			}
			this.CutsceneFastForwardRelay.Dispatch(false);
			RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
			this.m_fastForwardObj.SetActive(false);
		}
	}

	// Token: 0x0600342C RID: 13356 RVA: 0x000B2521 File Offset: 0x000B0721
	public void SetEnablePatchNotes(bool enable)
	{
		this.m_patchNotesController.SetEnablePatchNotes(enable);
		this.m_lockInput = enable;
		if (!enable)
		{
			this.PlaySelectedSFX(null);
		}
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x000B2540 File Offset: 0x000B0740
	private void OnVerticalInputHandler(InputActionEventData eventData)
	{
		if (this.m_lockInput)
		{
			return;
		}
		int currentSelectedIndex = this.m_currentSelectedIndex;
		float num = eventData.GetAxis();
		if (num == 0f)
		{
			num = -eventData.GetAxisPrev();
		}
		int num2;
		if (num > 0f)
		{
			num2 = ((this.m_currentSelectedIndex - 1 < 0) ? (this.m_menuButtonList.Count - 1) : (this.m_currentSelectedIndex - 1));
		}
		else
		{
			num2 = ((this.m_currentSelectedIndex + 1 >= this.m_menuButtonList.Count) ? 0 : (this.m_currentSelectedIndex + 1));
		}
		if (currentSelectedIndex != num2)
		{
			this.m_menuButtonList[num2].OnSelect(null);
		}
	}

	// Token: 0x0600342E RID: 13358 RVA: 0x000B25DC File Offset: 0x000B07DC
	private void OnConfirmInputHandler(InputActionEventData eventData)
	{
		if (this.m_lockInput)
		{
			return;
		}
		if (eventData.IsCurrentInputSource(ControllerType.Mouse))
		{
			return;
		}
		if (!this.m_menuDisplayed)
		{
			return;
		}
		if (this.m_menuButtonList.Count > 0)
		{
			this.m_menuButtonList[this.m_currentSelectedIndex].ExecuteButton();
		}
	}

	// Token: 0x0600342F RID: 13359 RVA: 0x000B262C File Offset: 0x000B082C
	protected void UpdateSelectedOptionItem(MainMenuButton menuItem)
	{
		if (menuItem.Index == this.m_currentSelectedIndex)
		{
			return;
		}
		if (this.m_menuButtonList[this.m_currentSelectedIndex] != null)
		{
			this.m_menuButtonList[this.m_currentSelectedIndex].OnDeselect(null);
		}
		this.m_currentSelectedIndex = menuItem.Index;
		if (this.m_selectionChangeEvent != null && this.m_menuDisplayed && menuItem.Interactable)
		{
			this.m_selectionChangeEvent.Invoke();
		}
	}

	// Token: 0x06003430 RID: 13360 RVA: 0x000B26A7 File Offset: 0x000B08A7
	protected void PlaySelectedSFX(MainMenuButton menuItem)
	{
		if (this.m_selectEvent != null && this.m_menuDisplayed)
		{
			this.m_selectEvent.Invoke();
		}
	}

	// Token: 0x06003431 RID: 13361
	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	// Token: 0x06003432 RID: 13362
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

	// Token: 0x06003433 RID: 13363 RVA: 0x000B26C4 File Offset: 0x000B08C4
	private void Update()
	{
		if (!MainMenuWindowController.m_hasEverHadFocus)
		{
			int num;
			MainMenuWindowController.GetWindowThreadProcessId(MainMenuWindowController.GetForegroundWindow(), out num);
			int id = Process.GetCurrentProcess().Id;
			if (num == id)
			{
				MainMenuWindowController.m_hasEverHadFocus = true;
			}
			return;
		}
		if (!ReInput.isReady)
		{
			return;
		}
		if (WindowManager.ActiveWindow != this)
		{
			return;
		}
		this.m_startingTextCanvasGroup.alpha = Mathf.Sin(Time.timeSinceLevelLoad * 2f) * this.TextAlphaMod;
		if (!RewiredMapController.IsCurrentMapEnabled)
		{
			return;
		}
		if (this.m_isLoading)
		{
			return;
		}
		Keyboard keyboard = ReInput.controllers.Keyboard;
		if (keyboard.GetModifierKey(ModifierKey.Shift) && keyboard.GetKeyDown(KeyCode.B))
		{
			this.LoadBackupWindow();
			return;
		}
		if (this.m_menuDisplayed)
		{
			return;
		}
		IList<Joystick> joysticks = ReInput.controllers.Joysticks;
		for (int i = 0; i < joysticks.Count; i++)
		{
			Joystick joystick = joysticks[i];
			if (Rewired_RL.IsStandardJoystick(joystick) && joystick.GetAnyButtonUp())
			{
				this.DisplayMenu(true);
				return;
			}
		}
		if (ReInput.controllers.Keyboard.GetAnyButtonUp())
		{
			this.DisplayMenu(true);
			return;
		}
		if (ReInput.controllers.Mouse.GetAnyButtonUp())
		{
			this.DisplayMenu(true);
			return;
		}
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x000B27E8 File Offset: 0x000B09E8
	private void AddInputListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x000B2870 File Offset: 0x000B0A70
	private void RemoveInputListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
	}

	// Token: 0x040028E3 RID: 10467
	[SerializeField]
	private MainMenuButton m_startingButton;

	// Token: 0x040028E4 RID: 10468
	[SerializeField]
	private CanvasGroup m_startingTextCanvasGroup;

	// Token: 0x040028E5 RID: 10469
	[SerializeField]
	private TMP_Text m_boilerText;

	// Token: 0x040028E6 RID: 10470
	[SerializeField]
	private TMP_Text m_versionNumberText;

	// Token: 0x040028E7 RID: 10471
	[SerializeField]
	private TMP_Text m_updateNumberText;

	// Token: 0x040028E8 RID: 10472
	[SerializeField]
	private TMP_Text m_profileSlotText;

	// Token: 0x040028E9 RID: 10473
	[SerializeField]
	private TMP_Text m_platformUsernameText;

	// Token: 0x040028EA RID: 10474
	[SerializeField]
	private Image m_logoImage;

	// Token: 0x040028EB RID: 10475
	[SerializeField]
	private Animator m_logoAnimator;

	// Token: 0x040028EC RID: 10476
	[SerializeField]
	private PlayableDirector m_playableDirector;

	// Token: 0x040028ED RID: 10477
	[SerializeField]
	private CanvasGroup m_part1Canvas;

	// Token: 0x040028EE RID: 10478
	[SerializeField]
	private CanvasGroup m_part2Canvas;

	// Token: 0x040028EF RID: 10479
	[SerializeField]
	private PatchNotesController m_patchNotesController;

	// Token: 0x040028F0 RID: 10480
	[SerializeField]
	private GameObject m_backupInstructionText;

	// Token: 0x040028F1 RID: 10481
	[SerializeField]
	private IntroNewGamePlusChanger m_newGamePlusChanger;

	// Token: 0x040028F2 RID: 10482
	[SerializeField]
	private GameObject m_discordLink;

	// Token: 0x040028F3 RID: 10483
	[SerializeField]
	private GameObject m_discordText;

	// Token: 0x040028F4 RID: 10484
	[SerializeField]
	private UnityEvent m_selectionChangeEvent;

	// Token: 0x040028F5 RID: 10485
	[SerializeField]
	private UnityEvent m_selectEvent;

	// Token: 0x040028F6 RID: 10486
	[Header("Intro Cutscene")]
	[SerializeField]
	private CanvasGroup[] m_introTextCanvasGroups;

	// Token: 0x040028F7 RID: 10487
	[SerializeField]
	private GameObject m_fastForwardObj;

	// Token: 0x040028F8 RID: 10488
	[SerializeField]
	private MobilePostProcessing m_postProcessing;

	// Token: 0x040028F9 RID: 10489
	[SerializeField]
	private MobilePostProcessingProfile m_undergroundPPProfile;

	// Token: 0x040028FA RID: 10490
	private bool m_allowFastForward;

	// Token: 0x040028FB RID: 10491
	private bool m_lockInput;

	// Token: 0x040028FC RID: 10492
	private bool m_isLoading;

	// Token: 0x040028FD RID: 10493
	private bool m_menuDisplayed;

	// Token: 0x040028FE RID: 10494
	private List<MainMenuButton> m_menuButtonList;

	// Token: 0x040028FF RID: 10495
	private int m_currentSelectedIndex;

	// Token: 0x04002900 RID: 10496
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002901 RID: 10497
	public Relay<bool> LoadGameRelay = new Relay<bool>();

	// Token: 0x04002902 RID: 10498
	public Relay CutsceneStartRelay = new Relay();

	// Token: 0x04002903 RID: 10499
	public Relay CutsceneCompleteRelay = new Relay();

	// Token: 0x04002904 RID: 10500
	public Relay<bool> CutsceneFastForwardRelay = new Relay<bool>();

	// Token: 0x04002905 RID: 10501
	public Relay CutsceneBlackRelay = new Relay();

	// Token: 0x04002906 RID: 10502
	public Relay<CanvasGroup> CutsceneDisplayTextRelay = new Relay<CanvasGroup>();

	// Token: 0x04002907 RID: 10503
	public Relay CutsceneCastleAppearsRelay = new Relay();

	// Token: 0x04002908 RID: 10504
	public Relay CloudsAppearRelay = new Relay();

	// Token: 0x04002909 RID: 10505
	public Relay CutsceneStarsAppearRelay = new Relay();

	// Token: 0x0400290A RID: 10506
	private Action m_failedLoadQuitGame;

	// Token: 0x0400290B RID: 10507
	private Action m_confirmMenu_Cancel;

	// Token: 0x0400290C RID: 10508
	private Action m_confirmWorldReset_Confirm;

	// Token: 0x0400290D RID: 10509
	private Action m_switchProfile;

	// Token: 0x0400290E RID: 10510
	private Action m_loadBackupWindow;

	// Token: 0x0400290F RID: 10511
	private Action<InputActionEventData> m_onConfirmInputHandler;

	// Token: 0x04002910 RID: 10512
	private Action<InputActionEventData> m_onVerticalInputHandler;

	// Token: 0x04002911 RID: 10513
	private Action<InputActionEventData> m_toggleSpeedUp;

	// Token: 0x04002913 RID: 10515
	private static bool m_hasEverHadFocus;
}

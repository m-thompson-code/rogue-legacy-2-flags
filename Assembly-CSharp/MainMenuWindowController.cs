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

// Token: 0x0200096D RID: 2413
public class MainMenuWindowController : WindowController
{
	// Token: 0x1700198B RID: 6539
	// (get) Token: 0x06004984 RID: 18820 RVA: 0x00028549 File Offset: 0x00026749
	// (set) Token: 0x06004985 RID: 18821 RVA: 0x00028551 File Offset: 0x00026751
	public float TextAlphaMod { get; set; }

	// Token: 0x1700198C RID: 6540
	// (get) Token: 0x06004986 RID: 18822 RVA: 0x0002855A File Offset: 0x0002675A
	public override WindowID ID
	{
		get
		{
			return WindowID.MainMenu;
		}
	}

	// Token: 0x06004987 RID: 18823 RVA: 0x0011DB30 File Offset: 0x0011BD30
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

	// Token: 0x06004988 RID: 18824 RVA: 0x0011DBD0 File Offset: 0x0011BDD0
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

	// Token: 0x06004989 RID: 18825 RVA: 0x0002855E File Offset: 0x0002675E
	protected override void OnOpen()
	{
		CameraController.GameCamera.transform.position = new Vector3(-60.07f, 13.03f, -25f);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x0600498A RID: 18826 RVA: 0x00028590 File Offset: 0x00026790
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

	// Token: 0x0600498B RID: 18827 RVA: 0x0002859F File Offset: 0x0002679F
	private void UpdateSelectedProfileText()
	{
		this.m_profileSlotText.text = string.Format(LocalizationManager.GetString("LOC_ID_MAIN_MENU_PROFILE_SLOT_1", false, false), SaveManager.CurrentProfile + 1);
	}

	// Token: 0x0600498C RID: 18828 RVA: 0x0011DCA8 File Offset: 0x0011BEA8
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

	// Token: 0x0600498D RID: 18829 RVA: 0x000285C9 File Offset: 0x000267C9
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

	// Token: 0x0600498E RID: 18830 RVA: 0x0000EE94 File Offset: 0x0000D094
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x0600498F RID: 18831 RVA: 0x0011DD00 File Offset: 0x0011BF00
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

	// Token: 0x06004990 RID: 18832 RVA: 0x00019922 File Offset: 0x00017B22
	private void SwitchProfile()
	{
		SaveManager.LoadingFailed = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.MainMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, false);
		WindowManager.SetWindowIsOpen(WindowID.ProfileSelect, true);
	}

	// Token: 0x06004991 RID: 18833 RVA: 0x000285D8 File Offset: 0x000267D8
	private void FailedLoadQuitGame()
	{
		Application.Quit(10);
		Application.Quit(30);
	}

	// Token: 0x06004992 RID: 18834 RVA: 0x000285E8 File Offset: 0x000267E8
	private void LoadBackupWindow()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, true);
	}

	// Token: 0x06004993 RID: 18835 RVA: 0x0011DE18 File Offset: 0x0011C018
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

	// Token: 0x06004994 RID: 18836 RVA: 0x000285FA File Offset: 0x000267FA
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06004995 RID: 18837
	private void DisplayMenu(bool animate)
	{
		if (MainMenuWindowController.splitStep == 0)
		{
			MainMenuWindowController.splitStep = 123123123;
		}
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

	// Token: 0x06004996 RID: 18838 RVA: 0x00028602 File Offset: 0x00026802
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

	// Token: 0x06004997 RID: 18839 RVA: 0x0011DFC8 File Offset: 0x0011C1C8
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

	// Token: 0x06004998 RID: 18840 RVA: 0x0011E0F4 File Offset: 0x0011C2F4
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

	// Token: 0x06004999 RID: 18841 RVA: 0x00028611 File Offset: 0x00026811
	private void ConfirmWorldReset_Confirm()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		PlayerDeathWindowController.GlobalCharacterDeathResetLogic();
		SaveManager.PlayerSaveData.IsDead = true;
		SceneLoader_RL.LoadScene(SceneID.Lineage, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x0600499A RID: 18842 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void ConfirmMenu_Cancel()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x0600499B RID: 18843
	private IEnumerator TutorialSceneCoroutine()
	{
		MainMenuWindowController.splitStep = 234234234;
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

	// Token: 0x0600499C RID: 18844 RVA: 0x00028642 File Offset: 0x00026842
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

	// Token: 0x0600499D RID: 18845 RVA: 0x0011E184 File Offset: 0x0011C384
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

	// Token: 0x0600499E RID: 18846 RVA: 0x0011E218 File Offset: 0x0011C418
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

	// Token: 0x0600499F RID: 18847 RVA: 0x00028651 File Offset: 0x00026851
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

	// Token: 0x060049A0 RID: 18848 RVA: 0x0011E2B4 File Offset: 0x0011C4B4
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

	// Token: 0x060049A1 RID: 18849 RVA: 0x0002866E File Offset: 0x0002686E
	public void SetEnablePatchNotes(bool enable)
	{
		this.m_patchNotesController.SetEnablePatchNotes(enable);
		this.m_lockInput = enable;
		if (!enable)
		{
			this.PlaySelectedSFX(null);
		}
	}

	// Token: 0x060049A2 RID: 18850 RVA: 0x0011E31C File Offset: 0x0011C51C
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

	// Token: 0x060049A3 RID: 18851 RVA: 0x0011E3B4 File Offset: 0x0011C5B4
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

	// Token: 0x060049A4 RID: 18852 RVA: 0x0011E404 File Offset: 0x0011C604
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

	// Token: 0x060049A5 RID: 18853 RVA: 0x0002868D File Offset: 0x0002688D
	protected void PlaySelectedSFX(MainMenuButton menuItem)
	{
		if (this.m_selectEvent != null && this.m_menuDisplayed)
		{
			this.m_selectEvent.Invoke();
		}
	}

	// Token: 0x060049A6 RID: 18854
	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	// Token: 0x060049A7 RID: 18855
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

	// Token: 0x060049A8 RID: 18856 RVA: 0x0011E480 File Offset: 0x0011C680
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

	// Token: 0x060049A9 RID: 18857 RVA: 0x0011E5A4 File Offset: 0x0011C7A4
	private void AddInputListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
	}

	// Token: 0x060049AA RID: 18858 RVA: 0x0011E62C File Offset: 0x0011C82C
	private void RemoveInputListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
	}

	// Token: 0x04003872 RID: 14450
	[SerializeField]
	private MainMenuButton m_startingButton;

	// Token: 0x04003873 RID: 14451
	[SerializeField]
	private CanvasGroup m_startingTextCanvasGroup;

	// Token: 0x04003874 RID: 14452
	[SerializeField]
	private TMP_Text m_boilerText;

	// Token: 0x04003875 RID: 14453
	[SerializeField]
	private TMP_Text m_versionNumberText;

	// Token: 0x04003876 RID: 14454
	[SerializeField]
	private TMP_Text m_updateNumberText;

	// Token: 0x04003877 RID: 14455
	[SerializeField]
	private TMP_Text m_profileSlotText;

	// Token: 0x04003878 RID: 14456
	[SerializeField]
	private TMP_Text m_platformUsernameText;

	// Token: 0x04003879 RID: 14457
	[SerializeField]
	private Image m_logoImage;

	// Token: 0x0400387A RID: 14458
	[SerializeField]
	private Animator m_logoAnimator;

	// Token: 0x0400387B RID: 14459
	[SerializeField]
	private PlayableDirector m_playableDirector;

	// Token: 0x0400387C RID: 14460
	[SerializeField]
	private CanvasGroup m_part1Canvas;

	// Token: 0x0400387D RID: 14461
	[SerializeField]
	private CanvasGroup m_part2Canvas;

	// Token: 0x0400387E RID: 14462
	[SerializeField]
	private PatchNotesController m_patchNotesController;

	// Token: 0x0400387F RID: 14463
	[SerializeField]
	private GameObject m_backupInstructionText;

	// Token: 0x04003880 RID: 14464
	[SerializeField]
	private IntroNewGamePlusChanger m_newGamePlusChanger;

	// Token: 0x04003881 RID: 14465
	[SerializeField]
	private GameObject m_discordLink;

	// Token: 0x04003882 RID: 14466
	[SerializeField]
	private GameObject m_discordText;

	// Token: 0x04003883 RID: 14467
	[SerializeField]
	private UnityEvent m_selectionChangeEvent;

	// Token: 0x04003884 RID: 14468
	[SerializeField]
	private UnityEvent m_selectEvent;

	// Token: 0x04003885 RID: 14469
	[Header("Intro Cutscene")]
	[SerializeField]
	private CanvasGroup[] m_introTextCanvasGroups;

	// Token: 0x04003886 RID: 14470
	[SerializeField]
	private GameObject m_fastForwardObj;

	// Token: 0x04003887 RID: 14471
	[SerializeField]
	private MobilePostProcessing m_postProcessing;

	// Token: 0x04003888 RID: 14472
	[SerializeField]
	private MobilePostProcessingProfile m_undergroundPPProfile;

	// Token: 0x04003889 RID: 14473
	private bool m_allowFastForward;

	// Token: 0x0400388A RID: 14474
	private bool m_lockInput;

	// Token: 0x0400388B RID: 14475
	private bool m_isLoading;

	// Token: 0x0400388C RID: 14476
	private bool m_menuDisplayed;

	// Token: 0x0400388D RID: 14477
	private List<MainMenuButton> m_menuButtonList;

	// Token: 0x0400388E RID: 14478
	private int m_currentSelectedIndex;

	// Token: 0x0400388F RID: 14479
	private WaitRL_Yield m_waitYield;

	// Token: 0x04003890 RID: 14480
	public Relay<bool> LoadGameRelay = new Relay<bool>();

	// Token: 0x04003891 RID: 14481
	public Relay CutsceneStartRelay = new Relay();

	// Token: 0x04003892 RID: 14482
	public Relay CutsceneCompleteRelay = new Relay();

	// Token: 0x04003893 RID: 14483
	public Relay<bool> CutsceneFastForwardRelay = new Relay<bool>();

	// Token: 0x04003894 RID: 14484
	public Relay CutsceneBlackRelay = new Relay();

	// Token: 0x04003895 RID: 14485
	public Relay<CanvasGroup> CutsceneDisplayTextRelay = new Relay<CanvasGroup>();

	// Token: 0x04003896 RID: 14486
	public Relay CutsceneCastleAppearsRelay = new Relay();

	// Token: 0x04003897 RID: 14487
	public Relay CloudsAppearRelay = new Relay();

	// Token: 0x04003898 RID: 14488
	public Relay CutsceneStarsAppearRelay = new Relay();

	// Token: 0x04003899 RID: 14489
	private Action m_failedLoadQuitGame;

	// Token: 0x0400389A RID: 14490
	private Action m_confirmMenu_Cancel;

	// Token: 0x0400389B RID: 14491
	private Action m_confirmWorldReset_Confirm;

	// Token: 0x0400389C RID: 14492
	private Action m_switchProfile;

	// Token: 0x0400389D RID: 14493
	private Action m_loadBackupWindow;

	// Token: 0x0400389E RID: 14494
	private Action<InputActionEventData> m_onConfirmInputHandler;

	// Token: 0x0400389F RID: 14495
	private Action<InputActionEventData> m_onVerticalInputHandler;

	// Token: 0x040038A0 RID: 14496
	private Action<InputActionEventData> m_toggleSpeedUp;

	// Token: 0x040038A2 RID: 14498
	private static bool m_hasEverHadFocus;

	// Token: 0x040038A3 RID: 14499
	public static int splitStep = 123123123;
}

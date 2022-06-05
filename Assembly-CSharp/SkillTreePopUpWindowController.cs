using System;
using System.Collections;
using Rewired;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000592 RID: 1426
public class SkillTreePopUpWindowController : WindowController, IAudioEventEmitter
{
	// Token: 0x170012E2 RID: 4834
	// (get) Token: 0x06003581 RID: 13697 RVA: 0x000B9D75 File Offset: 0x000B7F75
	// (set) Token: 0x06003582 RID: 13698 RVA: 0x000B9D7D File Offset: 0x000B7F7D
	public SkillTreeType PopupType { get; private set; }

	// Token: 0x170012E3 RID: 4835
	// (get) Token: 0x06003583 RID: 13699 RVA: 0x000B9D86 File Offset: 0x000B7F86
	public override WindowID ID
	{
		get
		{
			return WindowID.SkillTreePopUp;
		}
	}

	// Token: 0x170012E4 RID: 4836
	// (get) Token: 0x06003584 RID: 13700 RVA: 0x000B9D89 File Offset: 0x000B7F89
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

	// Token: 0x06003585 RID: 13701 RVA: 0x000B9DAF File Offset: 0x000B7FAF
	private void Awake()
	{
		this.m_cancelClassChange = new Action(this.CancelClassChange);
		this.m_confirmClassChange = new Action(this.ConfirmClassChange);
		this.m_onConfirmButtonPressed = new Action<InputActionEventData>(this.OnConfirmButtonPressed);
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x000B9DE7 File Offset: 0x000B7FE7
	protected override void OnFocus()
	{
		this.AddListeners();
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x000B9DEF File Offset: 0x000B7FEF
	protected override void OnLostFocus()
	{
		this.RemoveListeners();
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x000B9DF8 File Offset: 0x000B7FF8
	public void SetPopupType(SkillTreeType popupType)
	{
		this.PopupType = popupType;
		SkillTreePopupData popupData = SkillTreePopupLibrary.GetPopupData(popupType);
		if (popupData != null)
		{
			this.m_titleTextLocItem.SetString(popupData.TitleLocID);
			this.m_subTitleTextLocItem.SetString(popupData.SubtitleLocID);
			this.m_descriptionTextLocItem.SetString(popupData.DescriptionLocID);
			this.m_popupSprite.sprite = popupData.PopupSprite;
		}
	}

	// Token: 0x06003589 RID: 13705 RVA: 0x000B9E5A File Offset: 0x000B805A
	protected override void OnOpen()
	{
		this.PlayOpenAndCloseAudioSFX(true);
		this.m_displayedClassChange = false;
		base.StartCoroutine(this.OnOpenAnimCoroutine());
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x000B9E78 File Offset: 0x000B8078
	private void PlayOpenAndCloseAudioSFX(bool isOpen)
	{
		string text = string.Empty;
		if (isOpen)
		{
			text = "event:/UI/FrontEnd/ui_fe_generic_popUp";
			if (this.PopupType.ToString().ToLower().Contains("class_unlock"))
			{
				text = "event:/UI/FrontEnd/ui_fe_castle_upgrade_popup_open";
			}
		}
		else if (this.PopupType.ToString().ToLower().Contains("class_unlock"))
		{
			text = "event:/UI/FrontEnd/ui_fe_castle_upgrade_popup_close";
		}
		if (text != string.Empty)
		{
			AudioManager.PlayOneShot(this, text, default(Vector3));
		}
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x000B9F09 File Offset: 0x000B8109
	private IEnumerator OnOpenAnimCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_rayCanvasGroup.alpha = 0f;
		this.m_popupCanvasGroup.alpha = 0.5f;
		Vector3 localEulerAngles = this.m_popupCanvasGroup.transform.localEulerAngles;
		localEulerAngles.z = 40f;
		this.m_popupCanvasGroup.transform.localEulerAngles = localEulerAngles;
		Vector3 vector = this.m_popupCanvasGroup.transform.localScale;
		vector *= 0.25f;
		this.m_popupCanvasGroup.transform.localScale = vector;
		this.m_rayCanvasGroup.transform.localScale = Vector3.zero;
		this.m_windowCanvas.gameObject.SetActive(true);
		float duration = 0.5f;
		TweenManager.TweenTo_UnscaledTime(this.m_rayCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"delay",
			0.1f,
			"alpha",
			1
		});
		TweenManager.TweenTo_UnscaledTime(this.m_rayCanvasGroup.transform, duration, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			1,
			"localScale.y",
			1,
			"localScale.z",
			1
		});
		TweenManager.TweenTo_UnscaledTime(this.m_popupCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		TweenManager.TweenTo_UnscaledTime(this.m_popupCanvasGroup.transform, duration, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localEulerAngles.z",
			0
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_popupCanvasGroup.transform, duration, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			1,
			"localScale.y",
			1,
			"localScale.z",
			1
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x000B9F18 File Offset: 0x000B8118
	protected override void OnClose()
	{
		this.PlayOpenAndCloseAudioSFX(false);
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x000B9F32 File Offset: 0x000B8132
	private void AddListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x0600358E RID: 13710 RVA: 0x000B9F64 File Offset: 0x000B8164
	private void RemoveListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x0600358F RID: 13711 RVA: 0x000B9F98 File Offset: 0x000B8198
	private void OnConfirmButtonPressed(InputActionEventData eventData)
	{
		this.m_classToChangeTo = SkillTreeLogicHelper.GetClassTypeFromSkill(this.PopupType);
		ClassData classData = ClassLibrary.GetClassData(this.m_classToChangeTo);
		if (!this.m_displayedClassChange && this.m_classToChangeTo != ClassType.None && classData != null)
		{
			this.m_displayedClassChange = true;
			this.InitializeConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
			return;
		}
		WindowManager.SetWindowIsOpen(WindowID.SkillTreePopUp, false);
	}

	// Token: 0x06003590 RID: 13712 RVA: 0x000B9FF8 File Offset: 0x000B81F8
	private void InitializeConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		string @string = LocalizationManager.GetString(ClassLibrary.GetClassData(this.m_classToChangeTo).PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_SKILL_TREE_UI_CHANGE_CHARACTER_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText(string.Format(LocalizationManager.GetString("LOC_ID_SKILL_TREE_UI_CHANGE_CHARACTER_TEXT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), @string), false);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelClassChange);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmClassChange);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelClassChange);
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x000BA0D0 File Offset: 0x000B82D0
	private void ConfirmClassChange()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.SkillTreePopUp, false);
		AbilityType spell = SaveManager.PlayerSaveData.CurrentCharacter.Spell;
		AbilityType talent = SaveManager.PlayerSaveData.CurrentCharacter.Talent;
		CharacterCreator.GenerateClass(this.m_classToChangeTo, SaveManager.PlayerSaveData.CurrentCharacter);
		AbilityType abilityType = SaveManager.PlayerSaveData.CurrentCharacter.Talent;
		if (talent == AbilityType.SuperFart)
		{
			abilityType = talent;
		}
		if (TraitManager.IsTraitActive(TraitType.CantAttack))
		{
			SaveManager.PlayerSaveData.CurrentCharacter.Weapon = AbilityType.PacifistWeapon;
		}
		if (abilityType == spell)
		{
			AbilityType[] availableTalents = CharacterCreator.GetAvailableTalents(this.m_classToChangeTo);
			AbilityType abilityType2 = (availableTalents.Length != 0) ? availableTalents[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableTalents.Length)] : AbilityType.None;
			int num = 0;
			while (abilityType2 != AbilityType.None && abilityType2 == spell && num < 50)
			{
				num++;
				abilityType2 = availableTalents[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableTalents.Length)];
			}
			if (num >= 50)
			{
				Debug.LogWarning("<color=yellow>Could not find non-duplicate talent in SkillTreePopupWindowController.</color>");
			}
			abilityType = abilityType2;
		}
		SaveManager.PlayerSaveData.CurrentCharacter.Talent = abilityType;
		SaveManager.PlayerSaveData.CurrentCharacter.Spell = spell;
		SaveManager.PlayerSaveData.SetSpellSeenState(SaveManager.PlayerSaveData.CurrentCharacter.Spell, true);
		SaveManager.PlayerSaveData.SetSpellSeenState(SaveManager.PlayerSaveData.CurrentCharacter.Talent, true);
		LineageWindowController.CharacterLoadedFromLineage = true;
		PlayerManager.GetPlayerController().ResetCharacter();
		LineageWindowController.CharacterLoadedFromLineage = false;
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x000BA22E File Offset: 0x000B842E
	private void CancelClassChange()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.SkillTreePopUp, false);
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x000BA240 File Offset: 0x000B8440
	private void Update()
	{
		Vector3 localEulerAngles = this.m_ray.transform.localEulerAngles;
		localEulerAngles.z -= 10f * Time.unscaledDeltaTime;
		this.m_ray.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x040029C4 RID: 10692
	[SerializeField]
	private Image m_ray;

	// Token: 0x040029C5 RID: 10693
	[SerializeField]
	private Image m_popupSprite;

	// Token: 0x040029C6 RID: 10694
	[SerializeField]
	private LocalizationItem m_titleTextLocItem;

	// Token: 0x040029C7 RID: 10695
	[SerializeField]
	private LocalizationItem m_subTitleTextLocItem;

	// Token: 0x040029C8 RID: 10696
	[SerializeField]
	private LocalizationItem m_descriptionTextLocItem;

	// Token: 0x040029C9 RID: 10697
	[SerializeField]
	private CanvasGroup m_popupCanvasGroup;

	// Token: 0x040029CA RID: 10698
	[SerializeField]
	private CanvasGroup m_rayCanvasGroup;

	// Token: 0x040029CB RID: 10699
	private string m_description = string.Empty;

	// Token: 0x040029CC RID: 10700
	private bool m_displayedClassChange;

	// Token: 0x040029CD RID: 10701
	private ClassType m_classToChangeTo;

	// Token: 0x040029CE RID: 10702
	private Action m_cancelClassChange;

	// Token: 0x040029CF RID: 10703
	private Action m_confirmClassChange;

	// Token: 0x040029D0 RID: 10704
	private Action<InputActionEventData> m_onConfirmButtonPressed;

	// Token: 0x040029D1 RID: 10705
	private const string GENERIC_OPEN_AUDIO_PATH = "event:/UI/FrontEnd/ui_fe_generic_popUp";

	// Token: 0x040029D2 RID: 10706
	private const string CLASS_UNLOCK_OPEN_AUDIO_PATH = "event:/UI/FrontEnd/ui_fe_castle_upgrade_popup_open";

	// Token: 0x040029D3 RID: 10707
	private const string CLASS_UNLOCK_CLOSE_AUDIO_PATH = "event:/UI/FrontEnd/ui_fe_castle_upgrade_popup_close";
}

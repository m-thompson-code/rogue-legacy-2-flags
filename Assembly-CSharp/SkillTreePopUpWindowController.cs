using System;
using System.Collections;
using Rewired;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000990 RID: 2448
public class SkillTreePopUpWindowController : WindowController, IAudioEventEmitter
{
	// Token: 0x170019FF RID: 6655
	// (get) Token: 0x06004B63 RID: 19299 RVA: 0x000294A1 File Offset: 0x000276A1
	// (set) Token: 0x06004B64 RID: 19300 RVA: 0x000294A9 File Offset: 0x000276A9
	public SkillTreeType PopupType { get; private set; }

	// Token: 0x17001A00 RID: 6656
	// (get) Token: 0x06004B65 RID: 19301 RVA: 0x000047A7 File Offset: 0x000029A7
	public override WindowID ID
	{
		get
		{
			return WindowID.SkillTreePopUp;
		}
	}

	// Token: 0x17001A01 RID: 6657
	// (get) Token: 0x06004B66 RID: 19302 RVA: 0x000294B2 File Offset: 0x000276B2
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

	// Token: 0x06004B67 RID: 19303 RVA: 0x000294D8 File Offset: 0x000276D8
	private void Awake()
	{
		this.m_cancelClassChange = new Action(this.CancelClassChange);
		this.m_confirmClassChange = new Action(this.ConfirmClassChange);
		this.m_onConfirmButtonPressed = new Action<InputActionEventData>(this.OnConfirmButtonPressed);
	}

	// Token: 0x06004B68 RID: 19304 RVA: 0x00029510 File Offset: 0x00027710
	protected override void OnFocus()
	{
		this.AddListeners();
	}

	// Token: 0x06004B69 RID: 19305 RVA: 0x00029518 File Offset: 0x00027718
	protected override void OnLostFocus()
	{
		this.RemoveListeners();
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x001277F0 File Offset: 0x001259F0
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

	// Token: 0x06004B6B RID: 19307 RVA: 0x00029520 File Offset: 0x00027720
	protected override void OnOpen()
	{
		this.PlayOpenAndCloseAudioSFX(true);
		this.m_displayedClassChange = false;
		base.StartCoroutine(this.OnOpenAnimCoroutine());
	}

	// Token: 0x06004B6C RID: 19308 RVA: 0x00127854 File Offset: 0x00125A54
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

	// Token: 0x06004B6D RID: 19309 RVA: 0x0002953D File Offset: 0x0002773D
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

	// Token: 0x06004B6E RID: 19310 RVA: 0x0002954C File Offset: 0x0002774C
	protected override void OnClose()
	{
		this.PlayOpenAndCloseAudioSFX(false);
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06004B6F RID: 19311 RVA: 0x00029566 File Offset: 0x00027766
	private void AddListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06004B70 RID: 19312 RVA: 0x00029598 File Offset: 0x00027798
	private void RemoveListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06004B71 RID: 19313 RVA: 0x001278E8 File Offset: 0x00125AE8
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

	// Token: 0x06004B72 RID: 19314 RVA: 0x00127948 File Offset: 0x00125B48
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

	// Token: 0x06004B73 RID: 19315 RVA: 0x00127A20 File Offset: 0x00125C20
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

	// Token: 0x06004B74 RID: 19316 RVA: 0x000295CA File Offset: 0x000277CA
	private void CancelClassChange()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.SkillTreePopUp, false);
	}

	// Token: 0x06004B75 RID: 19317 RVA: 0x00127B80 File Offset: 0x00125D80
	private void Update()
	{
		Vector3 localEulerAngles = this.m_ray.transform.localEulerAngles;
		localEulerAngles.z -= 10f * Time.unscaledDeltaTime;
		this.m_ray.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x0400399D RID: 14749
	[SerializeField]
	private Image m_ray;

	// Token: 0x0400399E RID: 14750
	[SerializeField]
	private Image m_popupSprite;

	// Token: 0x0400399F RID: 14751
	[SerializeField]
	private LocalizationItem m_titleTextLocItem;

	// Token: 0x040039A0 RID: 14752
	[SerializeField]
	private LocalizationItem m_subTitleTextLocItem;

	// Token: 0x040039A1 RID: 14753
	[SerializeField]
	private LocalizationItem m_descriptionTextLocItem;

	// Token: 0x040039A2 RID: 14754
	[SerializeField]
	private CanvasGroup m_popupCanvasGroup;

	// Token: 0x040039A3 RID: 14755
	[SerializeField]
	private CanvasGroup m_rayCanvasGroup;

	// Token: 0x040039A4 RID: 14756
	private string m_description = string.Empty;

	// Token: 0x040039A5 RID: 14757
	private bool m_displayedClassChange;

	// Token: 0x040039A6 RID: 14758
	private ClassType m_classToChangeTo;

	// Token: 0x040039A7 RID: 14759
	private Action m_cancelClassChange;

	// Token: 0x040039A8 RID: 14760
	private Action m_confirmClassChange;

	// Token: 0x040039A9 RID: 14761
	private Action<InputActionEventData> m_onConfirmButtonPressed;

	// Token: 0x040039AA RID: 14762
	private const string GENERIC_OPEN_AUDIO_PATH = "event:/UI/FrontEnd/ui_fe_generic_popUp";

	// Token: 0x040039AB RID: 14763
	private const string CLASS_UNLOCK_OPEN_AUDIO_PATH = "event:/UI/FrontEnd/ui_fe_castle_upgrade_popup_open";

	// Token: 0x040039AC RID: 14764
	private const string CLASS_UNLOCK_CLOSE_AUDIO_PATH = "event:/UI/FrontEnd/ui_fe_castle_upgrade_popup_close";
}

using System;
using System.Collections.Generic;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000584 RID: 1412
public class BlacksmithOmniUIWindowController : BaseOmniUIWindowController<BlacksmithOmniUICategoryEntry, BlacksmithOmniUIEntry>
{
	// Token: 0x170012BF RID: 4799
	// (get) Token: 0x060034A5 RID: 13477 RVA: 0x000B456B File Offset: 0x000B276B
	public override bool CanReset
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170012C0 RID: 4800
	// (get) Token: 0x060034A6 RID: 13478 RVA: 0x000B456E File Offset: 0x000B276E
	public Vector3 UnityDownTextPosition
	{
		get
		{
			if (this.m_unityDownTextPosition)
			{
				return this.m_unityDownTextPosition.transform.position;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x170012C1 RID: 4801
	// (get) Token: 0x060034A7 RID: 13479 RVA: 0x000B4594 File Offset: 0x000B2794
	public override bool CanExit
	{
		get
		{
			int num = 999;
			if (PlayerManager.IsInstantiated)
			{
				num = PlayerManager.GetPlayerController().ActualAllowedEquipmentWeight;
			}
			return (int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Weight) <= num;
		}
	}

	// Token: 0x170012C2 RID: 4802
	// (get) Token: 0x060034A8 RID: 13480 RVA: 0x000B45C6 File Offset: 0x000B27C6
	public EquipmentCategoryType HighlightedCategory
	{
		get
		{
			if (base.IsInitialized && base.HighlightedCategoryIndex != -1)
			{
				return base.ActiveCategoryEntryArray[base.HighlightedCategoryIndex].CategoryType;
			}
			return EquipmentCategoryType.None;
		}
	}

	// Token: 0x170012C3 RID: 4803
	// (get) Token: 0x060034A9 RID: 13481 RVA: 0x000B45ED File Offset: 0x000B27ED
	public EquipmentType SelectedEquipmentType
	{
		get
		{
			if (base.IsInitialized && base.SelectedEntryIndex != -1)
			{
				return base.ActiveEntryArray[base.SelectedEntryIndex].EquipmentType;
			}
			return EquipmentType.None;
		}
	}

	// Token: 0x170012C4 RID: 4804
	// (get) Token: 0x060034AA RID: 13482 RVA: 0x000B4614 File Offset: 0x000B2814
	public override WindowID ID
	{
		get
		{
			return WindowID.Blacksmith;
		}
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x000B4618 File Offset: 0x000B2818
	protected override void Awake()
	{
		base.Awake();
		this.m_confirmUnequipAllGear = new Action(this.ConfirmUnequipAllGear);
		this.m_cancelUnequipAllGear = new Action(this.CancelUnequipAllGear);
		this.m_confirmToggleLoadouts = new Action(this.ConfirmToggleLoadouts);
		this.m_cancelToggleLoadouts = new Action(this.CancelToggleLoadouts);
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x000B4674 File Offset: 0x000B2874
	protected override void CreateCategoryEntries()
	{
		if (base.CategoryEntryArray != null)
		{
			Array.Clear(base.CategoryEntryArray, 0, base.CategoryEntryArray.Length);
			base.CategoryEntryArray = null;
		}
		Array categoryTypeArray = EquipmentType_RL.CategoryTypeArray;
		base.CategoryEntryArray = new BlacksmithOmniUICategoryEntry[categoryTypeArray.Length - 1];
		int num = 0;
		foreach (object obj in categoryTypeArray)
		{
			EquipmentCategoryType equipmentCategoryType = (EquipmentCategoryType)obj;
			if (equipmentCategoryType != EquipmentCategoryType.None)
			{
				base.CategoryEntryArray[num] = UnityEngine.Object.Instantiate<BlacksmithOmniUICategoryEntry>(this.m_categoryEntryPrefab);
				base.CategoryEntryArray[num].transform.SetParent(base.CategoryEntryLayoutGroup.transform);
				base.CategoryEntryArray[num].transform.localScale = Vector3.one;
				base.CategoryEntryArray[num].Initialize(equipmentCategoryType, num, this);
				num++;
			}
		}
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x000B4760 File Offset: 0x000B2960
	protected override void CreateEntries()
	{
		if (base.EntryArray != null)
		{
			Array.Clear(base.EntryArray, 0, base.EntryArray.Length);
			base.EntryArray = null;
		}
		Array typeArray = EquipmentType_RL.TypeArray;
		base.EntryArray = new BlacksmithOmniUIEntry[typeArray.Length - 1];
		int num = 0;
		foreach (object obj in typeArray)
		{
			EquipmentType equipmentType = (EquipmentType)obj;
			if (equipmentType != EquipmentType.None)
			{
				base.EntryArray[num] = UnityEngine.Object.Instantiate<BlacksmithOmniUIEntry>(this.m_entryPrefab);
				base.EntryArray[num].transform.SetParent(base.EntryLayoutGroup.transform);
				base.EntryArray[num].transform.localScale = Vector3.one;
				base.EntryArray[num].Initialize(equipmentType, this);
				base.EntryArray[num].SetEntryIndex(num);
				num++;
			}
		}
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x000B485C File Offset: 0x000B2A5C
	protected override void UpdateScrollArrows(float scrollAmount)
	{
		base.UpdateScrollArrows(scrollAmount);
		if (base.TopScrollArrow.activeSelf || base.BottomScrollArrow.activeSelf)
		{
			float num = (float)base.ActiveEntryArray.Length * this.m_entryHeight;
			float num2 = Mathf.Clamp(1f - base.ScrollBar.value, 0f, 1f);
			float height = base.ContentViewport.rect.height;
			float num3 = height;
			float num4 = num3 - height;
			if (num2 > 0f)
			{
				num3 = height + (num - height) * num2;
				num4 = num3 - height;
			}
			int num5 = Mathf.CeilToInt(num4 / this.m_entryHeight);
			int num6 = Mathf.FloorToInt(num3 / this.m_entryHeight);
			if (base.TopScrollArrow.activeSelf)
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < num5; i++)
				{
					BlacksmithOmniUIEntry blacksmithOmniUIEntry = base.ActiveEntryArray[i];
					if (EquipmentManager.GetFoundState(this.HighlightedCategory, blacksmithOmniUIEntry.EquipmentType) == FoundState.FoundButNotViewed)
					{
						flag = true;
						break;
					}
					if (EquipmentManager.CanPurchaseEquipment(this.HighlightedCategory, blacksmithOmniUIEntry.EquipmentType, true))
					{
						flag2 = true;
					}
				}
				if (flag)
				{
					if (!base.TopScrollNewSymbol.gameObject.activeSelf)
					{
						base.TopScrollNewSymbol.gameObject.SetActive(true);
					}
					if (base.TopScrollUpgradeSymbol.gameObject.activeSelf)
					{
						base.TopScrollUpgradeSymbol.gameObject.SetActive(false);
					}
				}
				else if (flag2)
				{
					if (base.TopScrollNewSymbol.gameObject.activeSelf)
					{
						base.TopScrollNewSymbol.gameObject.SetActive(false);
					}
					if (!base.TopScrollUpgradeSymbol.gameObject.activeSelf)
					{
						base.TopScrollUpgradeSymbol.gameObject.SetActive(true);
					}
				}
				else
				{
					if (base.TopScrollNewSymbol.gameObject.activeSelf)
					{
						base.TopScrollNewSymbol.gameObject.SetActive(false);
					}
					if (base.TopScrollUpgradeSymbol.gameObject.activeSelf)
					{
						base.TopScrollUpgradeSymbol.gameObject.SetActive(false);
					}
				}
			}
			if (base.BottomScrollArrow.activeSelf)
			{
				bool flag3 = false;
				bool flag4 = false;
				for (int j = num6; j < base.ActiveEntryArray.Length; j++)
				{
					BlacksmithOmniUIEntry blacksmithOmniUIEntry2 = base.ActiveEntryArray[j];
					if (EquipmentManager.GetFoundState(this.HighlightedCategory, blacksmithOmniUIEntry2.EquipmentType) == FoundState.FoundButNotViewed)
					{
						flag3 = true;
						break;
					}
					if (EquipmentManager.CanPurchaseEquipment(this.HighlightedCategory, blacksmithOmniUIEntry2.EquipmentType, true))
					{
						flag4 = true;
					}
				}
				if (flag3)
				{
					if (!base.BottomScrollNewSymbol.gameObject.activeSelf)
					{
						base.BottomScrollNewSymbol.gameObject.SetActive(true);
					}
					if (base.BottomScrollUpgradeSymbol.gameObject.activeSelf)
					{
						base.BottomScrollUpgradeSymbol.gameObject.SetActive(false);
						return;
					}
				}
				else if (flag4)
				{
					if (base.BottomScrollNewSymbol.gameObject.activeSelf)
					{
						base.BottomScrollNewSymbol.gameObject.SetActive(false);
					}
					if (!base.BottomScrollUpgradeSymbol.gameObject.activeSelf)
					{
						base.BottomScrollUpgradeSymbol.gameObject.SetActive(true);
						return;
					}
				}
				else
				{
					if (base.BottomScrollNewSymbol.gameObject.activeSelf)
					{
						base.BottomScrollNewSymbol.gameObject.SetActive(false);
					}
					if (base.BottomScrollUpgradeSymbol.gameObject.activeSelf)
					{
						base.BottomScrollUpgradeSymbol.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x000B4BA4 File Offset: 0x000B2DA4
	protected override void OnConfirmButtonJustPressed()
	{
		base.OnConfirmButtonJustPressed();
		if (!this.m_warningMessageVisible && !base.IsInCategories && base.ActiveEntryArray.Length == 0)
		{
			base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_NO_BLUEPRINTS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x000B4BF5 File Offset: 0x000B2DF5
	protected override void OnCancelButtonJustPressed()
	{
		base.OnCancelButtonJustPressed();
		if (!this.m_warningMessageVisible && base.IsInCategories)
		{
			base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_SELECT_CATEGORY_1", false, false);
		}
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x000B4C24 File Offset: 0x000B2E24
	protected override void OnOpen()
	{
		base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_SELECT_CATEGORY_1", false, false);
		if (SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled)
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_GEAR_ON_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		else
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_GEAR_OFF_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		if (this.HasAllEquipment())
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllEquipment, StoreType.All);
		}
		foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
		{
			if (equipmentType != EquipmentType.None)
			{
				EquipmentSetData equipmentSetData = EquipmentSetLibrary.GetEquipmentSetData(equipmentType);
				if (equipmentSetData != null && EquipmentManager.Get_EquipmentSet_CurrentLevel(equipmentType) >= equipmentSetData.SetRequirement03)
				{
					StoreAPIManager.GiveAchievement(AchievementType.UnlockHighestUnity, StoreType.All);
					break;
				}
			}
		}
		base.OnOpen();
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x000B4D04 File Offset: 0x000B2F04
	public bool HasAllEquipment()
	{
		EquipmentManager.GetTotalAvailableBlueprints(BlacksmithOmniUIWindowController.m_achievementHelper_STATIC);
		bool result = true;
		foreach (EquipmentObj equipmentObj in BlacksmithOmniUIWindowController.m_achievementHelper_STATIC)
		{
			if (EquipmentManager.GetFoundState(equipmentObj.CategoryType, equipmentObj.EquipmentType) < FoundState.Purchased)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x000B4D78 File Offset: 0x000B2F78
	protected override void OnClose()
	{
		base.OnClose();
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.ResetBaseValues();
			playerController.RecreateRendererArray();
			playerController.ResetRendererArrayColor();
			playerController.BlinkPulseEffect.ResetAllBlackFills();
			if (SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled)
			{
				EquipmentLoadout equipmentLoadout = SaveManager.EquipmentSaveData.GetEquipmentLoadout(playerController.CharacterClass.ClassType);
				if (equipmentLoadout != null)
				{
					equipmentLoadout.SaveLoadout(SaveManager.PlayerSaveData.CurrentCharacter);
				}
			}
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.Blacksmith_Shop_Closed, this, null);
		SaveManager.PlayerSaveData.UpdateCachedData();
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x000B4DFC File Offset: 0x000B2FFC
	protected override int GetEquippedIndex()
	{
		EquipmentObj equipped = EquipmentManager.GetEquipped(this.HighlightedCategory);
		if (equipped != null)
		{
			for (int i = 0; i < base.ActiveEntryArray.Length; i++)
			{
				if (base.ActiveEntryArray[i].EquipmentType == equipped.EquipmentType)
				{
					return i;
				}
			}
		}
		return base.GetEquippedIndex();
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x000B4E48 File Offset: 0x000B3048
	protected override void OnYButtonJustPressed()
	{
		this.InitializeResetMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x000B4E68 File Offset: 0x000B3068
	private void InitializeResetMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_SHOP_MENU_UNEQUIP_GEAR_MENU_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_SHOP_MENU_UNEQUIP_GEAR_MENU_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelUnequipAllGear);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmUnequipAllGear);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelUnequipAllGear);
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x000B4EF8 File Offset: 0x000B30F8
	private void ConfirmUnequipAllGear()
	{
		BlacksmithOmniUIEquipButton.SetEquipped(EquipmentCategoryType.Head, EquipmentType.None);
		BlacksmithOmniUIEquipButton.SetEquipped(EquipmentCategoryType.Chest, EquipmentType.None);
		BlacksmithOmniUIEquipButton.SetEquipped(EquipmentCategoryType.Weapon, EquipmentType.None);
		BlacksmithOmniUIEquipButton.SetEquipped(EquipmentCategoryType.Cape, EquipmentType.None);
		BlacksmithOmniUIEquipButton.SetEquipped(EquipmentCategoryType.Trinket, EquipmentType.None);
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new BlacksmithOmniUIDescriptionEventArgs(this.HighlightedCategory, this.SelectedEquipmentType, OmniUIButtonType.Equipping);
		}
		else
		{
			this.m_descriptionEventArgs.Initialize(this.HighlightedCategory, this.SelectedEquipmentType, OmniUIButtonType.Equipping);
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.m_descriptionEventArgs);
		int selectedEntryIndex = base.SelectedEntryIndex;
		base.ActiveEntryArray[selectedEntryIndex].OnDeselect(null);
		base.SelectedEntryIndex = -1;
		base.SetSelectedEntryIndex(selectedEntryIndex, false, false);
		this.CancelUnequipAllGear();
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x000B4FA9 File Offset: 0x000B31A9
	private void CancelUnequipAllGear()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x060034B9 RID: 13497 RVA: 0x000B4FBA File Offset: 0x000B31BA
	protected override void OnXButtonJustPressed()
	{
		this.InitializeLoadoutMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x000B4FD8 File Offset: 0x000B31D8
	private void InitializeLoadoutMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenuBig))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenuBig);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenuBig) as ConfirmMenuWindowController;
		if (!SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled)
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_SHOP_MENU_CLASS_GEAR_POPUP_ON_TITLE_1", true);
			confirmMenuWindowController.SetDescriptionText("LOC_ID_SHOP_MENU_CLASS_GEAR_POPUP_ON_TEXT_1", true);
		}
		else
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_SHOP_MENU_CLASS_GEAR_POPUP_OFF_TITLE_1", true);
			confirmMenuWindowController.SetDescriptionText("LOC_ID_SHOP_MENU_CLASS_GEAR_POPUP_OFF_TEXT_1", true);
		}
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelToggleLoadouts);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmToggleLoadouts);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelToggleLoadouts);
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x000B5090 File Offset: 0x000B3290
	private void ConfirmToggleLoadouts()
	{
		if (!SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled)
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_GEAR_ON_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		else
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_GEAR_OFF_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled = !SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled;
		this.CancelToggleLoadouts();
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x000B5112 File Offset: 0x000B3312
	private void CancelToggleLoadouts()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x000B5124 File Offset: 0x000B3324
	protected override void RefreshText(object sender, EventArgs args)
	{
		base.RefreshText(sender, args);
		if (!SaveManager.EquipmentSaveData.EquipmentLoadoutEnabled)
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_GEAR_ON_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			return;
		}
		this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_GEAR_OFF_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
	}

	// Token: 0x04002934 RID: 10548
	[SerializeField]
	private GameObject m_unityDownTextPosition;

	// Token: 0x04002935 RID: 10549
	[SerializeField]
	private TMP_Text m_loadoutText;

	// Token: 0x04002936 RID: 10550
	private Action m_cancelUnequipAllGear;

	// Token: 0x04002937 RID: 10551
	private Action m_confirmUnequipAllGear;

	// Token: 0x04002938 RID: 10552
	private Action m_confirmToggleLoadouts;

	// Token: 0x04002939 RID: 10553
	private Action m_cancelToggleLoadouts;

	// Token: 0x0400293A RID: 10554
	private static List<EquipmentObj> m_achievementHelper_STATIC = new List<EquipmentObj>();

	// Token: 0x0400293B RID: 10555
	private BlacksmithOmniUIDescriptionEventArgs m_descriptionEventArgs;
}

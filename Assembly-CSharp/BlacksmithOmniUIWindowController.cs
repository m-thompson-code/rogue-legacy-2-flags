using System;
using System.Collections.Generic;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x0200097C RID: 2428
public class BlacksmithOmniUIWindowController : BaseOmniUIWindowController<BlacksmithOmniUICategoryEntry, BlacksmithOmniUIEntry>
{
	// Token: 0x170019D0 RID: 6608
	// (get) Token: 0x06004A62 RID: 19042 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool CanReset
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170019D1 RID: 6609
	// (get) Token: 0x06004A63 RID: 19043 RVA: 0x00028B4A File Offset: 0x00026D4A
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

	// Token: 0x170019D2 RID: 6610
	// (get) Token: 0x06004A64 RID: 19044 RVA: 0x0012144C File Offset: 0x0011F64C
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

	// Token: 0x170019D3 RID: 6611
	// (get) Token: 0x06004A65 RID: 19045 RVA: 0x00028B6F File Offset: 0x00026D6F
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

	// Token: 0x170019D4 RID: 6612
	// (get) Token: 0x06004A66 RID: 19046 RVA: 0x00028B96 File Offset: 0x00026D96
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

	// Token: 0x170019D5 RID: 6613
	// (get) Token: 0x06004A67 RID: 19047 RVA: 0x00004762 File Offset: 0x00002962
	public override WindowID ID
	{
		get
		{
			return WindowID.Blacksmith;
		}
	}

	// Token: 0x06004A68 RID: 19048 RVA: 0x00121480 File Offset: 0x0011F680
	protected override void Awake()
	{
		base.Awake();
		this.m_confirmUnequipAllGear = new Action(this.ConfirmUnequipAllGear);
		this.m_cancelUnequipAllGear = new Action(this.CancelUnequipAllGear);
		this.m_confirmToggleLoadouts = new Action(this.ConfirmToggleLoadouts);
		this.m_cancelToggleLoadouts = new Action(this.CancelToggleLoadouts);
	}

	// Token: 0x06004A69 RID: 19049 RVA: 0x001214DC File Offset: 0x0011F6DC
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

	// Token: 0x06004A6A RID: 19050 RVA: 0x001215C8 File Offset: 0x0011F7C8
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

	// Token: 0x06004A6B RID: 19051 RVA: 0x001216C4 File Offset: 0x0011F8C4
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

	// Token: 0x06004A6C RID: 19052 RVA: 0x00121A0C File Offset: 0x0011FC0C
	protected override void OnConfirmButtonJustPressed()
	{
		base.OnConfirmButtonJustPressed();
		if (!this.m_warningMessageVisible && !base.IsInCategories && base.ActiveEntryArray.Length == 0)
		{
			base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_NO_BLUEPRINTS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
	}

	// Token: 0x06004A6D RID: 19053 RVA: 0x00028BBD File Offset: 0x00026DBD
	protected override void OnCancelButtonJustPressed()
	{
		base.OnCancelButtonJustPressed();
		if (!this.m_warningMessageVisible && base.IsInCategories)
		{
			base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_SELECT_CATEGORY_1", false, false);
		}
	}

	// Token: 0x06004A6E RID: 19054 RVA: 0x00121A60 File Offset: 0x0011FC60
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

	// Token: 0x06004A6F RID: 19055 RVA: 0x00121B40 File Offset: 0x0011FD40
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

	// Token: 0x06004A70 RID: 19056 RVA: 0x00121BB4 File Offset: 0x0011FDB4
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

	// Token: 0x06004A71 RID: 19057 RVA: 0x00121C38 File Offset: 0x0011FE38
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

	// Token: 0x06004A72 RID: 19058 RVA: 0x00028BEC File Offset: 0x00026DEC
	protected override void OnYButtonJustPressed()
	{
		this.InitializeResetMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x06004A73 RID: 19059 RVA: 0x00121C84 File Offset: 0x0011FE84
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

	// Token: 0x06004A74 RID: 19060 RVA: 0x00121D14 File Offset: 0x0011FF14
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

	// Token: 0x06004A75 RID: 19061 RVA: 0x00028C09 File Offset: 0x00026E09
	private void CancelUnequipAllGear()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x06004A76 RID: 19062 RVA: 0x00028C1A File Offset: 0x00026E1A
	protected override void OnXButtonJustPressed()
	{
		this.InitializeLoadoutMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x06004A77 RID: 19063 RVA: 0x00121DC8 File Offset: 0x0011FFC8
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

	// Token: 0x06004A78 RID: 19064 RVA: 0x00121E80 File Offset: 0x00120080
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

	// Token: 0x06004A79 RID: 19065 RVA: 0x00028C37 File Offset: 0x00026E37
	private void CancelToggleLoadouts()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x06004A7A RID: 19066 RVA: 0x00121F04 File Offset: 0x00120104
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

	// Token: 0x040038EF RID: 14575
	[SerializeField]
	private GameObject m_unityDownTextPosition;

	// Token: 0x040038F0 RID: 14576
	[SerializeField]
	private TMP_Text m_loadoutText;

	// Token: 0x040038F1 RID: 14577
	private Action m_cancelUnequipAllGear;

	// Token: 0x040038F2 RID: 14578
	private Action m_confirmUnequipAllGear;

	// Token: 0x040038F3 RID: 14579
	private Action m_confirmToggleLoadouts;

	// Token: 0x040038F4 RID: 14580
	private Action m_cancelToggleLoadouts;

	// Token: 0x040038F5 RID: 14581
	private static List<EquipmentObj> m_achievementHelper_STATIC = new List<EquipmentObj>();

	// Token: 0x040038F6 RID: 14582
	private BlacksmithOmniUIDescriptionEventArgs m_descriptionEventArgs;
}

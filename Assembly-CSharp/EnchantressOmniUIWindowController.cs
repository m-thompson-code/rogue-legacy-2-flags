using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x0200097E RID: 2430
public class EnchantressOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, EnchantressOmniUIEntry>
{
	// Token: 0x170019D8 RID: 6616
	// (get) Token: 0x06004A8D RID: 19085 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool CanReset
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170019D9 RID: 6617
	// (get) Token: 0x06004A8E RID: 19086 RVA: 0x00122438 File Offset: 0x00120638
	public override bool CanExit
	{
		get
		{
			int num = 999;
			if (PlayerManager.IsInstantiated)
			{
				num = PlayerManager.GetPlayerController().ActualRuneWeight;
			}
			return RuneManager.GetTotalEquippedWeight() <= num;
		}
	}

	// Token: 0x170019DA RID: 6618
	// (get) Token: 0x06004A8F RID: 19087 RVA: 0x00028CF5 File Offset: 0x00026EF5
	public RuneType SelectedRuneType
	{
		get
		{
			if (base.IsInitialized)
			{
				return base.ActiveEntryArray[base.SelectedEntryIndex].RuneType;
			}
			return RuneType.None;
		}
	}

	// Token: 0x170019DB RID: 6619
	// (get) Token: 0x06004A90 RID: 19088 RVA: 0x000046FA File Offset: 0x000028FA
	public override WindowID ID
	{
		get
		{
			return WindowID.Enchantress;
		}
	}

	// Token: 0x06004A91 RID: 19089 RVA: 0x00122468 File Offset: 0x00120668
	protected override void Awake()
	{
		base.Awake();
		this.m_confirmUnequipAllRunes = new Action(this.ConfirmUnequipAllRunes);
		this.m_cancelUnequipAllRunes = new Action(this.CancelUnequipAllRunes);
		this.m_confirmToggleLoadouts = new Action(this.ConfirmToggleLoadouts);
		this.m_cancelToggleLoadouts = new Action(this.CancelToggleLoadouts);
	}

	// Token: 0x06004A92 RID: 19090 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x06004A93 RID: 19091 RVA: 0x001224C4 File Offset: 0x001206C4
	protected override void CreateEntries()
	{
		if (base.EntryArray != null)
		{
			Array.Clear(base.EntryArray, 0, base.EntryArray.Length);
			base.EntryArray = null;
		}
		RuneType[] typeArray = RuneType_RL.TypeArray;
		base.EntryArray = new EnchantressOmniUIEntry[typeArray.Length - 1];
		int num = 0;
		foreach (RuneType runeType in typeArray)
		{
			if (runeType != RuneType.None)
			{
				base.EntryArray[num] = UnityEngine.Object.Instantiate<EnchantressOmniUIEntry>(this.m_entryPrefab);
				base.EntryArray[num].transform.SetParent(base.EntryLayoutGroup.transform);
				base.EntryArray[num].transform.localScale = Vector3.one;
				base.EntryArray[num].Initialize(runeType, this);
				base.EntryArray[num].SetEntryIndex(num);
				num++;
			}
		}
	}

	// Token: 0x06004A94 RID: 19092 RVA: 0x00122590 File Offset: 0x00120790
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
					EnchantressOmniUIEntry enchantressOmniUIEntry = base.ActiveEntryArray[i];
					if (RuneManager.GetFoundState(enchantressOmniUIEntry.RuneType) == FoundState.FoundButNotViewed)
					{
						flag = true;
						break;
					}
					if (RuneManager.CanPurchaseRune(enchantressOmniUIEntry.RuneType, true))
					{
						flag2 = true;
					}
				}
				if (flag)
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
				else if (flag2)
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, true);
				}
				else
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
			}
			if (base.BottomScrollArrow.activeSelf)
			{
				bool flag3 = false;
				bool flag4 = false;
				for (int j = num6; j < base.ActiveEntryArray.Length; j++)
				{
					EnchantressOmniUIEntry enchantressOmniUIEntry2 = base.ActiveEntryArray[j];
					if (RuneManager.GetFoundState(enchantressOmniUIEntry2.RuneType) == FoundState.FoundButNotViewed)
					{
						flag3 = true;
						break;
					}
					if (RuneManager.CanPurchaseRune(enchantressOmniUIEntry2.RuneType, true))
					{
						flag4 = true;
					}
				}
				if (flag3)
				{
					this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
					return;
				}
				if (flag4)
				{
					this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, true);
					return;
				}
				this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, false);
				this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
			}
		}
	}

	// Token: 0x06004A95 RID: 19093 RVA: 0x00028C7A File Offset: 0x00026E7A
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}

	// Token: 0x06004A96 RID: 19094 RVA: 0x001227EC File Offset: 0x001209EC
	protected override void OnOpen()
	{
		base.OnOpen();
		if (SaveManager.EquipmentSaveData.RuneLoadoutEnabled)
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_RUNES_ON_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		else
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_RUNES_OFF_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		if (this.HasAllRunes())
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllRunes, StoreType.All);
		}
	}

	// Token: 0x06004A97 RID: 19095 RVA: 0x0012286C File Offset: 0x00120A6C
	public bool HasAllRunes()
	{
		bool result = true;
		EnchantressOmniUIEntry[] activeEntryArray = base.ActiveEntryArray;
		for (int i = 0; i < activeEntryArray.Length; i++)
		{
			if (RuneManager.GetFoundState(activeEntryArray[i].RuneType) < FoundState.Purchased)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06004A98 RID: 19096 RVA: 0x001228A8 File Offset: 0x00120AA8
	protected override void OnClose()
	{
		base.OnClose();
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.ResetBaseValues();
			if (SaveManager.EquipmentSaveData.RuneLoadoutEnabled)
			{
				RuneLoadout runeLoadout = SaveManager.EquipmentSaveData.GetRuneLoadout(playerController.CharacterClass.ClassType);
				if (runeLoadout != null)
				{
					runeLoadout.SaveLoadout();
				}
			}
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.Enchantress_Shop_Closed, this, null);
		SaveManager.PlayerSaveData.UpdateCachedData();
	}

	// Token: 0x06004A99 RID: 19097 RVA: 0x00028D13 File Offset: 0x00026F13
	protected override void OnYButtonJustPressed()
	{
		this.InitializeResetMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x06004A9A RID: 19098 RVA: 0x0012290C File Offset: 0x00120B0C
	private void InitializeResetMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_SHOP_MENU_UNEQUIP_RUNES_MENU_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_SHOP_MENU_UNEQUIP_RUNES_MENU_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelUnequipAllRunes);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmUnequipAllRunes);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelUnequipAllRunes);
	}

	// Token: 0x06004A9B RID: 19099 RVA: 0x0012299C File Offset: 0x00120B9C
	private void ConfirmUnequipAllRunes()
	{
		foreach (RuneType runeType in RuneType_RL.TypeArray)
		{
			if (runeType != RuneType.None)
			{
				RuneManager.SetRuneEquippedLevel(runeType, 0, false, true);
			}
		}
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new EnchantressOmniUIDescriptionEventArgs(this.SelectedRuneType, OmniUIButtonType.Equipping);
		}
		else
		{
			this.m_descriptionEventArgs.Initialize(this.SelectedRuneType, OmniUIButtonType.Equipping);
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.m_descriptionEventArgs);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		int selectedEntryIndex = base.SelectedEntryIndex;
		base.ActiveEntryArray[selectedEntryIndex].OnDeselect(null);
		base.SelectedEntryIndex = -1;
		base.SetSelectedEntryIndex(selectedEntryIndex, false, false);
		this.CancelUnequipAllRunes();
	}

	// Token: 0x06004A9C RID: 19100 RVA: 0x00028D30 File Offset: 0x00026F30
	private void CancelUnequipAllRunes()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x06004A9D RID: 19101 RVA: 0x00028D41 File Offset: 0x00026F41
	protected override void OnXButtonJustPressed()
	{
		this.InitializeLoadoutMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x06004A9E RID: 19102 RVA: 0x00122A40 File Offset: 0x00120C40
	private void InitializeLoadoutMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenuBig))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenuBig);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenuBig) as ConfirmMenuWindowController;
		if (!SaveManager.EquipmentSaveData.RuneLoadoutEnabled)
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_SHOP_MENU_CLASS_RUNES_POPUP_ON_TITLE_1", true);
			confirmMenuWindowController.SetDescriptionText("LOC_ID_SHOP_MENU_CLASS_RUNES_POPUP_ON_TEXT_1", true);
		}
		else
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_SHOP_MENU_CLASS_RUNES_POPUP_OFF_TITLE_1", true);
			confirmMenuWindowController.SetDescriptionText("LOC_ID_SHOP_MENU_CLASS_RUNES_POPUP_OFF_TEXT_1", true);
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

	// Token: 0x06004A9F RID: 19103 RVA: 0x00122AF8 File Offset: 0x00120CF8
	private void ConfirmToggleLoadouts()
	{
		if (!SaveManager.EquipmentSaveData.RuneLoadoutEnabled)
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_RUNES_ON_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		else
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_RUNES_OFF_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
		SaveManager.EquipmentSaveData.RuneLoadoutEnabled = !SaveManager.EquipmentSaveData.RuneLoadoutEnabled;
		this.CancelToggleLoadouts();
	}

	// Token: 0x06004AA0 RID: 19104 RVA: 0x00028D5E File Offset: 0x00026F5E
	private void CancelToggleLoadouts()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x06004AA1 RID: 19105 RVA: 0x00122B7C File Offset: 0x00120D7C
	protected override void RefreshText(object sender, EventArgs args)
	{
		base.RefreshText(sender, args);
		if (!SaveManager.EquipmentSaveData.RuneLoadoutEnabled)
		{
			this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_RUNES_ON_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			return;
		}
		this.m_loadoutText.text = LocalizationManager.GetString("LOC_ID_SHOP_MENU_CLASS_RUNES_OFF_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
	}

	// Token: 0x040038FD RID: 14589
	[SerializeField]
	private TMP_Text m_loadoutText;

	// Token: 0x040038FE RID: 14590
	private Action m_confirmUnequipAllRunes;

	// Token: 0x040038FF RID: 14591
	private Action m_cancelUnequipAllRunes;

	// Token: 0x04003900 RID: 14592
	private Action m_confirmToggleLoadouts;

	// Token: 0x04003901 RID: 14593
	private Action m_cancelToggleLoadouts;

	// Token: 0x04003902 RID: 14594
	private EnchantressOmniUIDescriptionEventArgs m_descriptionEventArgs;
}

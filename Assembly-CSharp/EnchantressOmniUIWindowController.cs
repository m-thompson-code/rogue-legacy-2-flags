using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000586 RID: 1414
public class EnchantressOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, EnchantressOmniUIEntry>
{
	// Token: 0x170012C7 RID: 4807
	// (get) Token: 0x060034D0 RID: 13520 RVA: 0x000B570A File Offset: 0x000B390A
	public override bool CanReset
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170012C8 RID: 4808
	// (get) Token: 0x060034D1 RID: 13521 RVA: 0x000B5710 File Offset: 0x000B3910
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

	// Token: 0x170012C9 RID: 4809
	// (get) Token: 0x060034D2 RID: 13522 RVA: 0x000B5740 File Offset: 0x000B3940
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

	// Token: 0x170012CA RID: 4810
	// (get) Token: 0x060034D3 RID: 13523 RVA: 0x000B575E File Offset: 0x000B395E
	public override WindowID ID
	{
		get
		{
			return WindowID.Enchantress;
		}
	}

	// Token: 0x060034D4 RID: 13524 RVA: 0x000B5764 File Offset: 0x000B3964
	protected override void Awake()
	{
		base.Awake();
		this.m_confirmUnequipAllRunes = new Action(this.ConfirmUnequipAllRunes);
		this.m_cancelUnequipAllRunes = new Action(this.CancelUnequipAllRunes);
		this.m_confirmToggleLoadouts = new Action(this.ConfirmToggleLoadouts);
		this.m_cancelToggleLoadouts = new Action(this.CancelToggleLoadouts);
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x000B57BF File Offset: 0x000B39BF
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x060034D6 RID: 13526 RVA: 0x000B57C4 File Offset: 0x000B39C4
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

	// Token: 0x060034D7 RID: 13527 RVA: 0x000B5890 File Offset: 0x000B3A90
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

	// Token: 0x060034D8 RID: 13528 RVA: 0x000B5AEA File Offset: 0x000B3CEA
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x000B5B00 File Offset: 0x000B3D00
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

	// Token: 0x060034DA RID: 13530 RVA: 0x000B5B80 File Offset: 0x000B3D80
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

	// Token: 0x060034DB RID: 13531 RVA: 0x000B5BBC File Offset: 0x000B3DBC
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

	// Token: 0x060034DC RID: 13532 RVA: 0x000B5C20 File Offset: 0x000B3E20
	protected override void OnYButtonJustPressed()
	{
		this.InitializeResetMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x000B5C40 File Offset: 0x000B3E40
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

	// Token: 0x060034DE RID: 13534 RVA: 0x000B5CD0 File Offset: 0x000B3ED0
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

	// Token: 0x060034DF RID: 13535 RVA: 0x000B5D72 File Offset: 0x000B3F72
	private void CancelUnequipAllRunes()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x000B5D83 File Offset: 0x000B3F83
	protected override void OnXButtonJustPressed()
	{
		this.InitializeLoadoutMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x000B5DA0 File Offset: 0x000B3FA0
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

	// Token: 0x060034E2 RID: 13538 RVA: 0x000B5E58 File Offset: 0x000B4058
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

	// Token: 0x060034E3 RID: 13539 RVA: 0x000B5EDA File Offset: 0x000B40DA
	private void CancelToggleLoadouts()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x060034E4 RID: 13540 RVA: 0x000B5EEC File Offset: 0x000B40EC
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

	// Token: 0x04002942 RID: 10562
	[SerializeField]
	private TMP_Text m_loadoutText;

	// Token: 0x04002943 RID: 10563
	private Action m_confirmUnequipAllRunes;

	// Token: 0x04002944 RID: 10564
	private Action m_cancelUnequipAllRunes;

	// Token: 0x04002945 RID: 10565
	private Action m_confirmToggleLoadouts;

	// Token: 0x04002946 RID: 10566
	private Action m_cancelToggleLoadouts;

	// Token: 0x04002947 RID: 10567
	private EnchantressOmniUIDescriptionEventArgs m_descriptionEventArgs;
}

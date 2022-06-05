using System;
using System.Collections.Generic;
using RL_Windows;
using UnityEngine;

// Token: 0x02000982 RID: 2434
public class NewGamePlusOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, NewGamePlusOmniUIEntry>
{
	// Token: 0x170019E3 RID: 6627
	// (get) Token: 0x06004ABC RID: 19132 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool CanReset
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170019E4 RID: 6628
	// (get) Token: 0x06004ABD RID: 19133 RVA: 0x00028DE7 File Offset: 0x00026FE7
	// (set) Token: 0x06004ABE RID: 19134 RVA: 0x00028DEE File Offset: 0x00026FEE
	public static int CurrentNewGamePlusLevel { get; set; }

	// Token: 0x170019E5 RID: 6629
	// (get) Token: 0x06004ABF RID: 19135 RVA: 0x00028DF6 File Offset: 0x00026FF6
	// (set) Token: 0x06004AC0 RID: 19136 RVA: 0x00028DFD File Offset: 0x00026FFD
	public static bool EnteringNGPlus { get; set; }

	// Token: 0x170019E6 RID: 6630
	// (get) Token: 0x06004AC1 RID: 19137 RVA: 0x00028E05 File Offset: 0x00027005
	// (set) Token: 0x06004AC2 RID: 19138 RVA: 0x00028E0D File Offset: 0x0002700D
	public bool InTimelineEntry { get; private set; }

	// Token: 0x170019E7 RID: 6631
	// (get) Token: 0x06004AC3 RID: 19139 RVA: 0x00028E16 File Offset: 0x00027016
	public NewGamePlusOmniUIEntry CurrentlySelectedEntry
	{
		get
		{
			if (this.InTimelineEntry)
			{
				return this.m_timelineEntry;
			}
			if (base.ActiveEntryArray != null && base.SelectedEntryIndex < base.ActiveEntryArray.Length)
			{
				return base.ActiveEntryArray[base.SelectedEntryIndex];
			}
			return null;
		}
	}

	// Token: 0x170019E8 RID: 6632
	// (get) Token: 0x06004AC4 RID: 19140 RVA: 0x00028E4E File Offset: 0x0002704E
	public BurdenType SelectedBurdenType
	{
		get
		{
			if (base.IsInitialized)
			{
				return base.ActiveEntryArray[base.SelectedEntryIndex].BurdenType;
			}
			return BurdenType.None;
		}
	}

	// Token: 0x170019E9 RID: 6633
	// (get) Token: 0x06004AC5 RID: 19141 RVA: 0x00028E6C File Offset: 0x0002706C
	public override WindowID ID
	{
		get
		{
			return WindowID.NewGamePlusNPC;
		}
	}

	// Token: 0x06004AC6 RID: 19142 RVA: 0x00028E70 File Offset: 0x00027070
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelRemoveAllBurdens = new Action(this.CancelRemoveAllBurdens);
		this.m_confirmRemoveAllBurdens = new Action(this.ConfirmRemoveAllBurdens);
		this.m_quitMenu = new Action(this.QuitMenu);
	}

	// Token: 0x06004AC7 RID: 19143 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x06004AC8 RID: 19144 RVA: 0x0012362C File Offset: 0x0012182C
	protected override void CreateEntries()
	{
		if (base.EntryArray != null)
		{
			Array.Clear(base.EntryArray, 0, base.EntryArray.Length);
			base.EntryArray = null;
		}
		Array burden_ORDER = NewGamePlus_EV.BURDEN_ORDER;
		base.EntryArray = new NewGamePlusOmniUIEntry[burden_ORDER.Length];
		int num = 0;
		foreach (object obj in burden_ORDER)
		{
			BurdenType burdenType = (BurdenType)obj;
			if (burdenType != BurdenType.None)
			{
				base.EntryArray[num] = UnityEngine.Object.Instantiate<NewGamePlusOmniUIEntry>(this.m_entryPrefab);
				base.EntryArray[num].transform.SetParent(base.EntryLayoutGroup.transform);
				base.EntryArray[num].transform.localScale = Vector3.one;
				base.EntryArray[num].Initialize(burdenType, this);
				base.EntryArray[num].SetEntryIndex(num);
				num++;
			}
		}
		this.m_timelineEntry.Initialize(BurdenType.None, this);
		this.m_timelineEntry.SetEntryIndex(0);
	}

	// Token: 0x06004AC9 RID: 19145 RVA: 0x0012373C File Offset: 0x0012193C
	protected override void OnOpen()
	{
		this.m_storedBurdenValues.Clear();
		foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
		{
			if (burdenType != BurdenType.None)
			{
				this.m_storedBurdenValues.Add(burdenType, BurdenManager.GetBurdenLevel(burdenType));
			}
		}
		NewGamePlusOmniUIWindowController.EnteringNGPlus = false;
		NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
		base.OnOpen();
		this.SetTimelineEntryActive(true, false, false);
	}

	// Token: 0x06004ACA RID: 19146 RVA: 0x001237A8 File Offset: 0x001219A8
	protected override void OnClose()
	{
		base.OnClose();
		if (!NewGamePlusOmniUIWindowController.EnteringNGPlus)
		{
			Debug.Log("<color=yellow>Player did not enter new timeline. Resetting burden values.</color>");
			foreach (KeyValuePair<BurdenType, int> keyValuePair in this.m_storedBurdenValues)
			{
				BurdenManager.SetBurdenLevel(keyValuePair.Key, keyValuePair.Value, false, false);
			}
		}
	}

	// Token: 0x06004ACB RID: 19147 RVA: 0x00028EAE File Offset: 0x000270AE
	public override void UpdateAllEntryStates()
	{
		base.UpdateAllEntryStates();
		this.m_timelineEntry.UpdateState();
	}

	// Token: 0x06004ACC RID: 19148 RVA: 0x00123820 File Offset: 0x00121A20
	public void SetTimelineEntryActive(bool active, bool playSFX, bool usingMouse)
	{
		this.InTimelineEntry = active;
		if (!this.m_timelineEntry.Interactable)
		{
			this.m_timelineEntry.Interactable = true;
		}
		if (active)
		{
			base.ActiveEntryArray[base.SelectedEntryIndex].OnDeselect(null);
			base.ActiveEntryArray[base.SelectedEntryIndex].DeselectAllButtons();
			this.m_timelineEntry.DeselectAllButtons();
			this.m_timelineEntry.UpdateState();
			this.m_timelineEntry.OnSelect(null);
			this.m_timelineEntry.SelectRightMostButton();
			if (playSFX && this.m_changeSelectedOptionEvent != null)
			{
				this.m_changeSelectedOptionEvent.Invoke();
				return;
			}
		}
		else
		{
			int selectedEntryIndex = base.SelectedEntryIndex;
			base.SelectedEntryIndex = -1;
			base.SetSelectedEntryIndex(selectedEntryIndex, true, usingMouse);
			this.m_timelineEntry.OnDeselect(null);
			this.m_timelineEntry.DeselectAllButtons();
		}
	}

	// Token: 0x06004ACD RID: 19149 RVA: 0x00028EC1 File Offset: 0x000270C1
	protected override void OnUpButtonJustPressed()
	{
		if (this.InTimelineEntry)
		{
			this.SetTimelineEntryActive(false, true, false);
			base.SetSelectedEntryIndex(base.ActiveEntryArray.Length - 1, true, false);
			return;
		}
		if (base.SelectedEntryIndex == 0)
		{
			this.SetTimelineEntryActive(true, true, false);
			return;
		}
		base.OnUpButtonJustPressed();
	}

	// Token: 0x06004ACE RID: 19150 RVA: 0x00028EFF File Offset: 0x000270FF
	protected override void OnDownButtonJustPressed()
	{
		if (this.InTimelineEntry)
		{
			this.SetTimelineEntryActive(false, true, false);
			base.SetSelectedEntryIndex(0, true, false);
			return;
		}
		if (base.SelectedEntryIndex == base.ActiveEntryArray.Length - 1)
		{
			this.SetTimelineEntryActive(true, true, false);
			return;
		}
		base.OnDownButtonJustPressed();
	}

	// Token: 0x06004ACF RID: 19151 RVA: 0x00028F3E File Offset: 0x0002713E
	protected override void OnLeftButtonJustPressed()
	{
		if (this.InTimelineEntry)
		{
			this.m_timelineEntry.SelectButton(false);
			return;
		}
		base.OnLeftButtonJustPressed();
	}

	// Token: 0x06004AD0 RID: 19152 RVA: 0x00028F5B File Offset: 0x0002715B
	protected override void OnRightButtonJustPressed()
	{
		if (this.InTimelineEntry)
		{
			this.m_timelineEntry.SelectButton(true);
			return;
		}
		base.OnRightButtonJustPressed();
	}

	// Token: 0x06004AD1 RID: 19153 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnLBButtonJustPressed()
	{
	}

	// Token: 0x06004AD2 RID: 19154 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnRBButtonJustPressed()
	{
	}

	// Token: 0x06004AD3 RID: 19155 RVA: 0x00028F78 File Offset: 0x00027178
	protected override void OnConfirmButtonJustPressed()
	{
		if (this.InTimelineEntry)
		{
			this.m_timelineEntry.OnConfirmButtonPressed();
			return;
		}
		base.OnConfirmButtonJustPressed();
	}

	// Token: 0x06004AD4 RID: 19156 RVA: 0x001238E8 File Offset: 0x00121AE8
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
				for (int i = 0; i < num5; i++)
				{
					NewGamePlusOmniUIEntry newGamePlusOmniUIEntry = base.ActiveEntryArray[i];
					if (newGamePlusOmniUIEntry.BurdenType != BurdenType.None && BurdenManager.GetFoundState(newGamePlusOmniUIEntry.BurdenType) == FoundState.FoundButNotViewed)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
				else
				{
					this.SetGameObjectActive(base.TopScrollNewSymbol.gameObject, false);
					this.SetGameObjectActive(base.TopScrollUpgradeSymbol.gameObject, false);
				}
			}
			if (base.BottomScrollArrow.activeSelf)
			{
				bool flag2 = false;
				for (int j = num6; j < base.ActiveEntryArray.Length; j++)
				{
					if (BurdenManager.GetFoundState(base.ActiveEntryArray[j].BurdenType) == FoundState.FoundButNotViewed)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, true);
					this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
					return;
				}
				this.SetGameObjectActive(base.BottomScrollNewSymbol.gameObject, false);
				this.SetGameObjectActive(base.BottomScrollUpgradeSymbol.gameObject, false);
			}
		}
	}

	// Token: 0x06004AD5 RID: 19157 RVA: 0x00028C7A File Offset: 0x00026E7A
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}

	// Token: 0x06004AD6 RID: 19158 RVA: 0x00028F94 File Offset: 0x00027194
	protected override void OnYButtonJustPressed()
	{
		this.InitializeResetMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x06004AD7 RID: 19159 RVA: 0x00123ACC File Offset: 0x00121CCC
	private void InitializeResetMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_SHOP_MENU_UNEQUIP_BURDENS_MENU_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_SHOP_MENU_UNEQUIP_BURDENS_MENU_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelRemoveAllBurdens);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmRemoveAllBurdens);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelRemoveAllBurdens);
	}

	// Token: 0x06004AD8 RID: 19160 RVA: 0x00123B5C File Offset: 0x00121D5C
	private void ConfirmRemoveAllBurdens()
	{
		foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
		{
			if (burdenType != BurdenType.None)
			{
				BurdenManager.SetBurdenLevel(burdenType, 0, false, true);
			}
		}
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new NewGamePlusOmniUIDescriptionEventArgs(this.SelectedBurdenType, OmniUIButtonType.Equipping);
		}
		else
		{
			this.m_descriptionEventArgs.Initialize(this.SelectedBurdenType, OmniUIButtonType.Equipping);
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.m_descriptionEventArgs);
		this.SetTimelineEntryActive(true, false, false);
		this.CancelRemoveAllBurdens();
	}

	// Token: 0x06004AD9 RID: 19161 RVA: 0x00028FB1 File Offset: 0x000271B1
	private void CancelRemoveAllBurdens()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x06004ADA RID: 19162 RVA: 0x00123BE4 File Offset: 0x00121DE4
	protected override void OnCancelButtonJustPressed()
	{
		bool flag = false;
		foreach (KeyValuePair<BurdenType, int> keyValuePair in this.m_storedBurdenValues)
		{
			if (BurdenManager.GetBurdenLevel(keyValuePair.Key) != keyValuePair.Value)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.InitializeQuitMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
			base.SetKeyboardEnabled(false);
			return;
		}
		base.OnCancelButtonJustPressed();
	}

	// Token: 0x06004ADB RID: 19163 RVA: 0x00123C6C File Offset: 0x00121E6C
	private void InitializeQuitMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_NEWGAMEPLUS_EXIT_MENU_CONFIRM_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_NEWGAMEPLUS_EXIT_MENU_CONFIRM_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelRemoveAllBurdens);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_quitMenu);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelRemoveAllBurdens);
	}

	// Token: 0x06004ADC RID: 19164 RVA: 0x00028FC2 File Offset: 0x000271C2
	private void QuitMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.SetKeyboardEnabled(true);
		base.OnCancelButtonJustPressed();
	}

	// Token: 0x04003907 RID: 14599
	[SerializeField]
	private NewGamePlusOmniUIEntry m_timelineEntry;

	// Token: 0x04003908 RID: 14600
	private Dictionary<BurdenType, int> m_storedBurdenValues = new Dictionary<BurdenType, int>();

	// Token: 0x04003909 RID: 14601
	private Action m_cancelRemoveAllBurdens;

	// Token: 0x0400390A RID: 14602
	private Action m_confirmRemoveAllBurdens;

	// Token: 0x0400390B RID: 14603
	private Action m_quitMenu;

	// Token: 0x0400390F RID: 14607
	private NewGamePlusOmniUIDescriptionEventArgs m_descriptionEventArgs;
}

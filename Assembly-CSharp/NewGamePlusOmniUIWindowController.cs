using System;
using System.Collections.Generic;
using RL_Windows;
using UnityEngine;

// Token: 0x0200058A RID: 1418
public class NewGamePlusOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, NewGamePlusOmniUIEntry>
{
	// Token: 0x170012D2 RID: 4818
	// (get) Token: 0x060034FF RID: 13567 RVA: 0x000B6A49 File Offset: 0x000B4C49
	public override bool CanReset
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170012D3 RID: 4819
	// (get) Token: 0x06003500 RID: 13568 RVA: 0x000B6A4C File Offset: 0x000B4C4C
	// (set) Token: 0x06003501 RID: 13569 RVA: 0x000B6A53 File Offset: 0x000B4C53
	public static int CurrentNewGamePlusLevel { get; set; }

	// Token: 0x170012D4 RID: 4820
	// (get) Token: 0x06003502 RID: 13570 RVA: 0x000B6A5B File Offset: 0x000B4C5B
	// (set) Token: 0x06003503 RID: 13571 RVA: 0x000B6A62 File Offset: 0x000B4C62
	public static bool EnteringNGPlus { get; set; }

	// Token: 0x170012D5 RID: 4821
	// (get) Token: 0x06003504 RID: 13572 RVA: 0x000B6A6A File Offset: 0x000B4C6A
	// (set) Token: 0x06003505 RID: 13573 RVA: 0x000B6A72 File Offset: 0x000B4C72
	public bool InTimelineEntry { get; private set; }

	// Token: 0x170012D6 RID: 4822
	// (get) Token: 0x06003506 RID: 13574 RVA: 0x000B6A7B File Offset: 0x000B4C7B
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

	// Token: 0x170012D7 RID: 4823
	// (get) Token: 0x06003507 RID: 13575 RVA: 0x000B6AB3 File Offset: 0x000B4CB3
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

	// Token: 0x170012D8 RID: 4824
	// (get) Token: 0x06003508 RID: 13576 RVA: 0x000B6AD1 File Offset: 0x000B4CD1
	public override WindowID ID
	{
		get
		{
			return WindowID.NewGamePlusNPC;
		}
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x000B6AD5 File Offset: 0x000B4CD5
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelRemoveAllBurdens = new Action(this.CancelRemoveAllBurdens);
		this.m_confirmRemoveAllBurdens = new Action(this.ConfirmRemoveAllBurdens);
		this.m_quitMenu = new Action(this.QuitMenu);
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x000B6B13 File Offset: 0x000B4D13
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x000B6B18 File Offset: 0x000B4D18
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

	// Token: 0x0600350C RID: 13580 RVA: 0x000B6C28 File Offset: 0x000B4E28
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

	// Token: 0x0600350D RID: 13581 RVA: 0x000B6C94 File Offset: 0x000B4E94
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

	// Token: 0x0600350E RID: 13582 RVA: 0x000B6D0C File Offset: 0x000B4F0C
	public override void UpdateAllEntryStates()
	{
		base.UpdateAllEntryStates();
		this.m_timelineEntry.UpdateState();
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x000B6D20 File Offset: 0x000B4F20
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

	// Token: 0x06003510 RID: 13584 RVA: 0x000B6DE7 File Offset: 0x000B4FE7
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

	// Token: 0x06003511 RID: 13585 RVA: 0x000B6E25 File Offset: 0x000B5025
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

	// Token: 0x06003512 RID: 13586 RVA: 0x000B6E64 File Offset: 0x000B5064
	protected override void OnLeftButtonJustPressed()
	{
		if (this.InTimelineEntry)
		{
			this.m_timelineEntry.SelectButton(false);
			return;
		}
		base.OnLeftButtonJustPressed();
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x000B6E81 File Offset: 0x000B5081
	protected override void OnRightButtonJustPressed()
	{
		if (this.InTimelineEntry)
		{
			this.m_timelineEntry.SelectButton(true);
			return;
		}
		base.OnRightButtonJustPressed();
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x000B6E9E File Offset: 0x000B509E
	protected override void OnLBButtonJustPressed()
	{
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x000B6EA0 File Offset: 0x000B50A0
	protected override void OnRBButtonJustPressed()
	{
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x000B6EA2 File Offset: 0x000B50A2
	protected override void OnConfirmButtonJustPressed()
	{
		if (this.InTimelineEntry)
		{
			this.m_timelineEntry.OnConfirmButtonPressed();
			return;
		}
		base.OnConfirmButtonJustPressed();
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x000B6EC0 File Offset: 0x000B50C0
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

	// Token: 0x06003518 RID: 13592 RVA: 0x000B70A2 File Offset: 0x000B52A2
	private void SetGameObjectActive(GameObject obj, bool active)
	{
		if (obj.activeSelf == !active)
		{
			obj.SetActive(active);
		}
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x000B70B7 File Offset: 0x000B52B7
	protected override void OnYButtonJustPressed()
	{
		this.InitializeResetMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		base.SetKeyboardEnabled(false);
		base.OnYButtonJustPressed();
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x000B70D4 File Offset: 0x000B52D4
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

	// Token: 0x0600351B RID: 13595 RVA: 0x000B7164 File Offset: 0x000B5364
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

	// Token: 0x0600351C RID: 13596 RVA: 0x000B71E9 File Offset: 0x000B53E9
	private void CancelRemoveAllBurdens()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.SetKeyboardEnabled(true);
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x000B71FC File Offset: 0x000B53FC
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

	// Token: 0x0600351E RID: 13598 RVA: 0x000B7284 File Offset: 0x000B5484
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

	// Token: 0x0600351F RID: 13599 RVA: 0x000B7313 File Offset: 0x000B5513
	private void QuitMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.SetKeyboardEnabled(true);
		base.OnCancelButtonJustPressed();
	}

	// Token: 0x0400294C RID: 10572
	[SerializeField]
	private NewGamePlusOmniUIEntry m_timelineEntry;

	// Token: 0x0400294D RID: 10573
	private Dictionary<BurdenType, int> m_storedBurdenValues = new Dictionary<BurdenType, int>();

	// Token: 0x0400294E RID: 10574
	private Action m_cancelRemoveAllBurdens;

	// Token: 0x0400294F RID: 10575
	private Action m_confirmRemoveAllBurdens;

	// Token: 0x04002950 RID: 10576
	private Action m_quitMenu;

	// Token: 0x04002954 RID: 10580
	private NewGamePlusOmniUIDescriptionEventArgs m_descriptionEventArgs;
}

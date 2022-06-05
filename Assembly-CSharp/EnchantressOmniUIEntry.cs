using System;
using UnityEngine.EventSystems;

// Token: 0x020003A4 RID: 932
public class EnchantressOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17000E53 RID: 3667
	// (get) Token: 0x060022A2 RID: 8866 RVA: 0x00070CC5 File Offset: 0x0006EEC5
	// (set) Token: 0x060022A3 RID: 8867 RVA: 0x00070CCD File Offset: 0x0006EECD
	public RuneType RuneType { get; protected set; }

	// Token: 0x17000E54 RID: 3668
	// (get) Token: 0x060022A4 RID: 8868 RVA: 0x00070CD6 File Offset: 0x0006EED6
	public override EventArgs EntryEventArgs
	{
		get
		{
			if (this.m_eventArgs == null)
			{
				this.m_eventArgs = new EnchantressOmniUIDescriptionEventArgs(this.RuneType, OmniUIButtonType.Purchasing);
			}
			else
			{
				this.m_eventArgs.Initialize(this.RuneType, OmniUIButtonType.Purchasing);
			}
			return this.m_eventArgs;
		}
	}

	// Token: 0x17000E55 RID: 3669
	// (get) Token: 0x060022A5 RID: 8869 RVA: 0x00070D0C File Offset: 0x0006EF0C
	public override bool IsEntryActive
	{
		get
		{
			return RuneManager.GetFoundState(this.RuneType) != FoundState.NotFound;
		}
	}

	// Token: 0x060022A6 RID: 8870 RVA: 0x00070D20 File Offset: 0x0006EF20
	public void Initialize(RuneType runeType, EnchantressOmniUIWindowController windowController)
	{
		this.RuneType = runeType;
		this.Initialize(windowController);
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			((IEnchantressOmniUIButton)buttonArray[i]).RuneType = this.RuneType;
		}
		string @string = LocalizationManager.GetString(RuneLibrary.GetRuneData(this.RuneType).Title, false, false);
		this.m_titleText.text = @string;
	}

	// Token: 0x060022A7 RID: 8871 RVA: 0x00070D88 File Offset: 0x0006EF88
	public override void UpdateActive()
	{
		RuneData runeData = RuneLibrary.GetRuneData(this.RuneType);
		if (runeData == null || runeData.Disabled)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		else if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x00070DE8 File Offset: 0x0006EFE8
	public override void UpdateState()
	{
		if (RuneManager.GetFoundState(this.RuneType) == FoundState.FoundButNotViewed)
		{
			if (!this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(true);
			}
		}
		else if (this.m_newSymbol.gameObject.activeSelf)
		{
			this.m_newSymbol.gameObject.SetActive(false);
		}
		if (this.IsEntryActive)
		{
			this.m_icon.sprite = IconLibrary.GetRuneIcon(this.RuneType);
		}
		else
		{
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
		}
		base.UpdateState();
		if (!this.IsEntryActive)
		{
			this.m_titleText.text = LocalizationManager.GetString("LOC_ID_ENCHANTRESS_RUNE_NOT_FOUND_1", false, false);
			return;
		}
		string @string = LocalizationManager.GetString(RuneLibrary.GetRuneData(this.RuneType).Title, false, false);
		this.m_titleText.text = @string;
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x00070EC6 File Offset: 0x0006F0C6
	public override void OnSelect(BaseEventData eventData)
	{
		if (!base.Interactable)
		{
			return;
		}
		if (RuneManager.GetFoundState(this.RuneType) == FoundState.FoundButNotViewed)
		{
			RuneManager.SetFoundState(this.RuneType, FoundState.FoundAndViewed, false, true);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		}
		base.OnSelect(eventData);
	}

	// Token: 0x04001DD8 RID: 7640
	private EnchantressOmniUIDescriptionEventArgs m_eventArgs;
}

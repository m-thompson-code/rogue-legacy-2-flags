using System;
using UnityEngine.EventSystems;

// Token: 0x02000637 RID: 1591
public class EnchantressOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x170012E6 RID: 4838
	// (get) Token: 0x060030BA RID: 12474 RVA: 0x0001ABAC File Offset: 0x00018DAC
	// (set) Token: 0x060030BB RID: 12475 RVA: 0x0001ABB4 File Offset: 0x00018DB4
	public RuneType RuneType { get; protected set; }

	// Token: 0x170012E7 RID: 4839
	// (get) Token: 0x060030BC RID: 12476 RVA: 0x0001ABBD File Offset: 0x00018DBD
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

	// Token: 0x170012E8 RID: 4840
	// (get) Token: 0x060030BD RID: 12477 RVA: 0x0001ABF3 File Offset: 0x00018DF3
	public override bool IsEntryActive
	{
		get
		{
			return RuneManager.GetFoundState(this.RuneType) != FoundState.NotFound;
		}
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x000D17CC File Offset: 0x000CF9CC
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

	// Token: 0x060030BF RID: 12479 RVA: 0x000D1834 File Offset: 0x000CFA34
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

	// Token: 0x060030C0 RID: 12480 RVA: 0x000D1894 File Offset: 0x000CFA94
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

	// Token: 0x060030C1 RID: 12481 RVA: 0x0001AC07 File Offset: 0x00018E07
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

	// Token: 0x040027F8 RID: 10232
	private EnchantressOmniUIDescriptionEventArgs m_eventArgs;
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000644 RID: 1604
public class JournalOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x170012F5 RID: 4853
	// (get) Token: 0x060030F4 RID: 12532 RVA: 0x0001AE85 File Offset: 0x00019085
	// (set) Token: 0x060030F5 RID: 12533 RVA: 0x0001AE8D File Offset: 0x0001908D
	public int JournalIndex { get; private set; }

	// Token: 0x170012F6 RID: 4854
	// (get) Token: 0x060030F6 RID: 12534 RVA: 0x0001AE96 File Offset: 0x00019096
	// (set) Token: 0x060030F7 RID: 12535 RVA: 0x0001AE9E File Offset: 0x0001909E
	public JournalType JournalType { get; private set; }

	// Token: 0x170012F7 RID: 4855
	// (get) Token: 0x060030F8 RID: 12536 RVA: 0x000D218C File Offset: 0x000D038C
	public override EventArgs EntryEventArgs
	{
		get
		{
			if (this.m_eventArgs == null)
			{
				this.m_eventArgs = new JournalOmniUIDescriptionEventArgs(base.EntryIndex, (this.m_windowController as JournalOmniUIWindowController).HighlightedCategory, this.JournalIndex, this.JournalType);
			}
			else
			{
				this.m_eventArgs.Initialize(base.EntryIndex, (this.m_windowController as JournalOmniUIWindowController).HighlightedCategory, this.JournalIndex, this.JournalType);
			}
			return this.m_eventArgs;
		}
	}

	// Token: 0x170012F8 RID: 4856
	// (get) Token: 0x060030F9 RID: 12537 RVA: 0x000D2204 File Offset: 0x000D0404
	public override bool IsEntryActive
	{
		get
		{
			JournalCategoryType highlightedCategory = (this.m_windowController as JournalOmniUIWindowController).HighlightedCategory;
			if (highlightedCategory == JournalCategoryType.None)
			{
				return false;
			}
			int numJournals = Journal_EV.GetNumJournals(highlightedCategory, JournalType.Journal);
			int numJournals2 = Journal_EV.GetNumJournals(highlightedCategory, JournalType.MemoryFragment);
			return (this.JournalType != JournalType.Journal || this.JournalIndex < numJournals) && (this.JournalType != JournalType.MemoryFragment || this.JournalIndex < numJournals2) && SaveManager.PlayerSaveData.GetJournalsRead(highlightedCategory, this.JournalType) > this.JournalIndex;
		}
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x000D227C File Offset: 0x000D047C
	private void UpdateJournalTypeAndIndex()
	{
		JournalCategoryType highlightedCategory = (this.m_windowController as JournalOmniUIWindowController).HighlightedCategory;
		if (highlightedCategory == JournalCategoryType.None)
		{
			return;
		}
		BiomeType biomeType = JournalType_RL.ConvertJournalCategoryTypeToBiome(highlightedCategory);
		int numJournals = Journal_EV.GetNumJournals(biomeType, JournalType.Journal);
		if (base.EntryIndex >= numJournals)
		{
			this.JournalType = JournalType.MemoryFragment;
			this.JournalIndex = base.EntryIndex - numJournals;
		}
		else
		{
			this.JournalType = JournalType.Journal;
			this.JournalIndex = base.EntryIndex;
		}
		foreach (IJournalOmniUIButton journalOmniUIButton in this.m_buttonArray)
		{
			journalOmniUIButton.EntryIndex = base.EntryIndex;
			journalOmniUIButton.JournalIndex = this.JournalIndex;
			journalOmniUIButton.JournalType = this.JournalType;
		}
		string arg = highlightedCategory.ToString();
		if (biomeType != BiomeType.None)
		{
			arg = LocalizationManager.GetString(BiomeDataLibrary.GetData(biomeType).BiomeNameLocID, false, false);
		}
		if (this.JournalType == JournalType.MemoryFragment)
		{
			this.m_titleText.text = string.Format(LocalizationManager.GetString("LOC_ID_JOURNAL_MEMORY_TITLE_1", false, false), arg);
			return;
		}
		this.m_titleText.text = string.Format(LocalizationManager.GetString("LOC_ID_JOURNAL_JOURNAL_TITLE_1", false, false), arg);
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x000D2390 File Offset: 0x000D0590
	public override void UpdateActive()
	{
		JournalCategoryType highlightedCategory = (this.m_windowController as JournalOmniUIWindowController).HighlightedCategory;
		if (highlightedCategory == JournalCategoryType.None)
		{
			return;
		}
		int numJournals = Journal_EV.GetNumJournals(highlightedCategory, JournalType.Journal);
		int numJournals2 = Journal_EV.GetNumJournals(highlightedCategory, JournalType.MemoryFragment);
		if (base.EntryIndex < numJournals + numJournals2)
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
				return;
			}
		}
		else if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x000D2400 File Offset: 0x000D0600
	public override void Initialize(IOmniUIWindowController windowController)
	{
		base.Initialize(windowController);
		foreach (IJournalOmniUIButton journalOmniUIButton in this.m_buttonArray)
		{
			journalOmniUIButton.JournalIndex = this.JournalIndex;
			journalOmniUIButton.EntryIndex = base.EntryIndex;
			journalOmniUIButton.JournalType = this.JournalType;
			journalOmniUIButton.JournalWindowController = (JournalOmniUIWindowController)windowController;
		}
		this.m_titleText.text = "";
		if (this.m_newSymbol.gameObject.activeSelf)
		{
			this.m_newSymbol.gameObject.SetActive(false);
		}
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x000D2494 File Offset: 0x000D0694
	public override void SetEntryIndex(int index)
	{
		base.SetEntryIndex(index);
		this.m_journalNumber.text = (index + 1).ToString();
		this.UpdateJournalTypeAndIndex();
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x0001AEA7 File Offset: 0x000190A7
	public override void OnSelect(BaseEventData eventData)
	{
		(this.m_windowController as JournalOmniUIWindowController).ResetDescriptionScrollBar();
		base.OnSelect(eventData);
	}

	// Token: 0x04002816 RID: 10262
	[SerializeField]
	private TMP_Text m_journalNumber;

	// Token: 0x04002819 RID: 10265
	private JournalOmniUIDescriptionEventArgs m_eventArgs;
}

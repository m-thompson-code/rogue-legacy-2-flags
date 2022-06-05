using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020003AF RID: 943
public class JournalOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17000E62 RID: 3682
	// (get) Token: 0x060022DC RID: 8924 RVA: 0x00071988 File Offset: 0x0006FB88
	// (set) Token: 0x060022DD RID: 8925 RVA: 0x00071990 File Offset: 0x0006FB90
	public int JournalIndex { get; private set; }

	// Token: 0x17000E63 RID: 3683
	// (get) Token: 0x060022DE RID: 8926 RVA: 0x00071999 File Offset: 0x0006FB99
	// (set) Token: 0x060022DF RID: 8927 RVA: 0x000719A1 File Offset: 0x0006FBA1
	public JournalType JournalType { get; private set; }

	// Token: 0x17000E64 RID: 3684
	// (get) Token: 0x060022E0 RID: 8928 RVA: 0x000719AC File Offset: 0x0006FBAC
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

	// Token: 0x17000E65 RID: 3685
	// (get) Token: 0x060022E1 RID: 8929 RVA: 0x00071A24 File Offset: 0x0006FC24
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

	// Token: 0x060022E2 RID: 8930 RVA: 0x00071A9C File Offset: 0x0006FC9C
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

	// Token: 0x060022E3 RID: 8931 RVA: 0x00071BB0 File Offset: 0x0006FDB0
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

	// Token: 0x060022E4 RID: 8932 RVA: 0x00071C20 File Offset: 0x0006FE20
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

	// Token: 0x060022E5 RID: 8933 RVA: 0x00071CB4 File Offset: 0x0006FEB4
	public override void SetEntryIndex(int index)
	{
		base.SetEntryIndex(index);
		this.m_journalNumber.text = (index + 1).ToString();
		this.UpdateJournalTypeAndIndex();
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x00071CE4 File Offset: 0x0006FEE4
	public override void OnSelect(BaseEventData eventData)
	{
		(this.m_windowController as JournalOmniUIWindowController).ResetDescriptionScrollBar();
		base.OnSelect(eventData);
	}

	// Token: 0x04001DED RID: 7661
	[SerializeField]
	private TMP_Text m_journalNumber;

	// Token: 0x04001DF0 RID: 7664
	private JournalOmniUIDescriptionEventArgs m_eventArgs;
}

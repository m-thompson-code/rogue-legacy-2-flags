using System;
using UnityEngine;

// Token: 0x02000980 RID: 2432
public class JournalOmniUIWindowController : BaseOmniUIWindowController<JournalOmniUICategoryEntry, JournalOmniUIEntry>
{
	// Token: 0x170019DD RID: 6621
	// (get) Token: 0x06004AA9 RID: 19113 RVA: 0x00028D83 File Offset: 0x00026F83
	public GameObject ScrollArrow
	{
		get
		{
			return this.m_scrollArrow;
		}
	}

	// Token: 0x170019DE RID: 6622
	// (get) Token: 0x06004AAA RID: 19114 RVA: 0x00028D8B File Offset: 0x00026F8B
	public ScrollBarInput_RL DescriptionBoxScrollInput
	{
		get
		{
			return this.m_descriptionBoxScrollInput;
		}
	}

	// Token: 0x170019DF RID: 6623
	// (get) Token: 0x06004AAB RID: 19115 RVA: 0x00008269 File Offset: 0x00006469
	public override WindowID ID
	{
		get
		{
			return WindowID.Journal;
		}
	}

	// Token: 0x170019E0 RID: 6624
	// (get) Token: 0x06004AAC RID: 19116 RVA: 0x00122F3C File Offset: 0x0012113C
	public JournalCategoryType HighlightedCategory
	{
		get
		{
			if (base.IsInitialized && base.HighlightedCategoryIndex != -1)
			{
				if (base.ActiveCategoryEntryArray != null)
				{
					if (base.HighlightedCategoryIndex >= 0 && base.HighlightedCategoryIndex < base.ActiveCategoryEntryArray.Length)
					{
						return base.ActiveCategoryEntryArray[base.HighlightedCategoryIndex].CategoryType;
					}
					Debug.LogFormat("<color=red>| {0} | The <b>HighlightedCategoryIndex ({1})</b> is out of bounds.</color>", new object[]
					{
						this,
						base.HighlightedCategoryIndex
					});
				}
				else
				{
					Debug.LogFormat("<color=red>| {0} | The <b>ActiveCategoryEntryArray</b> is null.</color>", new object[]
					{
						this
					});
				}
			}
			return JournalCategoryType.None;
		}
	}

	// Token: 0x06004AAD RID: 19117 RVA: 0x00122FC8 File Offset: 0x001211C8
	protected override void CreateCategoryEntries()
	{
		if (base.CategoryEntryArray != null)
		{
			Array.Clear(base.CategoryEntryArray, 0, base.CategoryEntryArray.Length);
			base.CategoryEntryArray = null;
		}
		Array sortedCategoryTypeArray = JournalType_RL.SortedCategoryTypeArray;
		base.CategoryEntryArray = new JournalOmniUICategoryEntry[sortedCategoryTypeArray.Length - 1];
		int num = 0;
		foreach (object obj in sortedCategoryTypeArray)
		{
			JournalCategoryType journalCategoryType = (JournalCategoryType)obj;
			if (journalCategoryType != JournalCategoryType.None)
			{
				base.CategoryEntryArray[num] = UnityEngine.Object.Instantiate<JournalOmniUICategoryEntry>(this.m_categoryEntryPrefab);
				base.CategoryEntryArray[num].transform.SetParent(base.CategoryEntryLayoutGroup.transform);
				base.CategoryEntryArray[num].transform.localScale = Vector3.one;
				base.CategoryEntryArray[num].Initialize(journalCategoryType, num, this);
				num++;
			}
		}
	}

	// Token: 0x06004AAE RID: 19118 RVA: 0x001230B4 File Offset: 0x001212B4
	protected override void CreateEntries()
	{
		if (base.EntryArray != null)
		{
			Array.Clear(base.EntryArray, 0, base.EntryArray.Length);
			base.EntryArray = null;
		}
		base.EntryArray = new JournalOmniUIEntry[20];
		for (int i = 0; i < base.EntryArray.Length; i++)
		{
			base.EntryArray[i] = UnityEngine.Object.Instantiate<JournalOmniUIEntry>(this.m_entryPrefab);
			base.EntryArray[i].transform.SetParent(base.EntryLayoutGroup.transform);
			base.EntryArray[i].transform.localScale = Vector3.one;
			base.EntryArray[i].Initialize(this);
			base.EntryArray[i].SetEntryIndex(i);
			base.EntryArray[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06004AAF RID: 19119 RVA: 0x00123180 File Offset: 0x00121380
	protected override void OnOpen()
	{
		this.UpdateCategoryVisibility();
		base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_SELECT_CATEGORY_1", false, false);
		base.CommonFields.DescriptionBoxRaycastBlocker.gameObject.SetActive(false);
		base.PurchaseBox.gameObject.SetActive(false);
		this.ScrollArrow.SetActive(false);
		base.OnOpen();
	}

	// Token: 0x06004AB0 RID: 19120 RVA: 0x001231E4 File Offset: 0x001213E4
	protected override void OnConfirmButtonJustPressed()
	{
		base.OnConfirmButtonJustPressed();
		if (!this.m_warningMessageVisible && !base.IsInCategories && base.ActiveEntryArray.Length == 0)
		{
			base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_JOURNAL_NO_JOURNALS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
	}

	// Token: 0x06004AB1 RID: 19121 RVA: 0x00028D93 File Offset: 0x00026F93
	protected override void OnCancelButtonJustPressed()
	{
		base.OnCancelButtonJustPressed();
		if (!this.m_warningMessageVisible && base.IsInCategories)
		{
			base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_SELECT_CATEGORY_1", false, false);
		}
	}

	// Token: 0x06004AB2 RID: 19122 RVA: 0x00123238 File Offset: 0x00121438
	private void UpdateCategoryVisibility()
	{
		foreach (JournalOmniUICategoryEntry journalOmniUICategoryEntry in base.CategoryEntryLayoutGroup.GetComponentsInChildren<JournalOmniUICategoryEntry>(true))
		{
			bool numJournals = Journal_EV.GetNumJournals(journalOmniUICategoryEntry.CategoryType, JournalType.Journal) != 0;
			int numJournals2 = Journal_EV.GetNumJournals(journalOmniUICategoryEntry.CategoryType, JournalType.MemoryFragment);
			int journalsRead = SaveManager.PlayerSaveData.GetJournalsRead(journalOmniUICategoryEntry.CategoryType, JournalType.Journal);
			int journalsRead2 = SaveManager.PlayerSaveData.GetJournalsRead(journalOmniUICategoryEntry.CategoryType, JournalType.MemoryFragment);
			if ((!numJournals || journalsRead == 0) && (numJournals2 == 0 || journalsRead2 == 0))
			{
				if (journalOmniUICategoryEntry.gameObject.activeSelf)
				{
					journalOmniUICategoryEntry.gameObject.SetActive(false);
				}
			}
			else if (!journalOmniUICategoryEntry.gameObject.activeSelf)
			{
				journalOmniUICategoryEntry.gameObject.SetActive(true);
			}
		}
		base.ActiveCategoryEntryArray = base.CategoryEntryLayoutGroup.GetComponentsInChildren<JournalOmniUICategoryEntry>(false);
	}

	// Token: 0x06004AB3 RID: 19123 RVA: 0x00028DC2 File Offset: 0x00026FC2
	public void ResetDescriptionScrollBar()
	{
		this.m_descriptionBoxScrollInput.ResetScrollBar();
	}

	// Token: 0x04003903 RID: 14595
	[SerializeField]
	private ScrollBarInput_RL m_descriptionBoxScrollInput;

	// Token: 0x04003904 RID: 14596
	[SerializeField]
	private RectTransform m_descriptionBoxContentRectTransform;

	// Token: 0x04003905 RID: 14597
	[SerializeField]
	private GameObject m_scrollArrow;
}

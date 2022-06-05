using System;
using UnityEngine;

// Token: 0x02000588 RID: 1416
public class JournalOmniUIWindowController : BaseOmniUIWindowController<JournalOmniUICategoryEntry, JournalOmniUIEntry>
{
	// Token: 0x170012CC RID: 4812
	// (get) Token: 0x060034EC RID: 13548 RVA: 0x000B62D7 File Offset: 0x000B44D7
	public GameObject ScrollArrow
	{
		get
		{
			return this.m_scrollArrow;
		}
	}

	// Token: 0x170012CD RID: 4813
	// (get) Token: 0x060034ED RID: 13549 RVA: 0x000B62DF File Offset: 0x000B44DF
	public ScrollBarInput_RL DescriptionBoxScrollInput
	{
		get
		{
			return this.m_descriptionBoxScrollInput;
		}
	}

	// Token: 0x170012CE RID: 4814
	// (get) Token: 0x060034EE RID: 13550 RVA: 0x000B62E7 File Offset: 0x000B44E7
	public override WindowID ID
	{
		get
		{
			return WindowID.Journal;
		}
	}

	// Token: 0x170012CF RID: 4815
	// (get) Token: 0x060034EF RID: 13551 RVA: 0x000B62EC File Offset: 0x000B44EC
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

	// Token: 0x060034F0 RID: 13552 RVA: 0x000B6378 File Offset: 0x000B4578
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

	// Token: 0x060034F1 RID: 13553 RVA: 0x000B6464 File Offset: 0x000B4664
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

	// Token: 0x060034F2 RID: 13554 RVA: 0x000B6530 File Offset: 0x000B4730
	protected override void OnOpen()
	{
		this.UpdateCategoryVisibility();
		base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_SELECT_CATEGORY_1", false, false);
		base.CommonFields.DescriptionBoxRaycastBlocker.gameObject.SetActive(false);
		base.PurchaseBox.gameObject.SetActive(false);
		this.ScrollArrow.SetActive(false);
		base.OnOpen();
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x000B6594 File Offset: 0x000B4794
	protected override void OnConfirmButtonJustPressed()
	{
		base.OnConfirmButtonJustPressed();
		if (!this.m_warningMessageVisible && !base.IsInCategories && base.ActiveEntryArray.Length == 0)
		{
			base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_JOURNAL_NO_JOURNALS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		}
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x000B65E5 File Offset: 0x000B47E5
	protected override void OnCancelButtonJustPressed()
	{
		base.OnCancelButtonJustPressed();
		if (!this.m_warningMessageVisible && base.IsInCategories)
		{
			base.ChooseCategoryText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_SELECT_CATEGORY_1", false, false);
		}
	}

	// Token: 0x060034F5 RID: 13557 RVA: 0x000B6614 File Offset: 0x000B4814
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

	// Token: 0x060034F6 RID: 13558 RVA: 0x000B66D8 File Offset: 0x000B48D8
	public void ResetDescriptionScrollBar()
	{
		this.m_descriptionBoxScrollInput.ResetScrollBar();
	}

	// Token: 0x04002948 RID: 10568
	[SerializeField]
	private ScrollBarInput_RL m_descriptionBoxScrollInput;

	// Token: 0x04002949 RID: 10569
	[SerializeField]
	private RectTransform m_descriptionBoxContentRectTransform;

	// Token: 0x0400294A RID: 10570
	[SerializeField]
	private GameObject m_scrollArrow;
}

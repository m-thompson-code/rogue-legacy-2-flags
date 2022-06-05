using System;
using System.Text;
using TMPro;
using UnityEngine;

// Token: 0x020003AE RID: 942
public class JournalOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<JournalOmniUIDescriptionEventArgs, JournalOmniUIDescriptionBoxEntry.JournalOmniUIDescriptionBoxType>
{
	// Token: 0x060022D8 RID: 8920 RVA: 0x000717DE File Offset: 0x0006F9DE
	protected override void Awake()
	{
		base.Awake();
		this.m_journalStringBuilder = new StringBuilder();
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000717F1 File Offset: 0x0006F9F1
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		if (this.m_descriptionType == JournalOmniUIDescriptionBoxEntry.JournalOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
		}
		if (this.m_journalNumberText != null)
		{
			this.m_journalNumberText.text = "";
		}
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x00071834 File Offset: 0x0006FA34
	protected override void DisplayDescriptionBox(JournalOmniUIDescriptionEventArgs args)
	{
		JournalCategoryType journalCategoryType = args.JournalCategoryType;
		int journalIndex = args.JournalIndex;
		JournalType journalType = args.JournalType;
		JournalEntry journalEntry = Journal_EV.GetJournalEntry(journalCategoryType, journalType, journalIndex);
		if (this.m_journalNumberText)
		{
			this.m_journalNumberText.text = (args.EntryIndex + 1).ToString();
		}
		if (journalEntry.IsEmpty)
		{
			this.m_titleText.text = "NO JOURNAL ENTRIES EXIST IN JOURNAL CATEGORY: " + journalCategoryType.ToString();
			return;
		}
		JournalOmniUIDescriptionBoxEntry.JournalOmniUIDescriptionBoxType descriptionType = this.m_descriptionType;
		if (descriptionType == JournalOmniUIDescriptionBoxEntry.JournalOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = LocalizationManager.GetString(journalEntry.TitleLocID, false, false);
			return;
		}
		if (descriptionType != JournalOmniUIDescriptionBoxEntry.JournalOmniUIDescriptionBoxType.Text)
		{
			return;
		}
		this.m_journalStringBuilder.Clear();
		this.m_journalStringBuilder.Append(LocalizationManager.GetString(journalEntry.TextLocID, false, false));
		for (int i = 0; i < this.CARRIAGE_RETURN_ARRAY.Length; i++)
		{
			this.m_journalStringBuilder.Replace(this.CARRIAGE_RETURN_ARRAY[i], "\n\n");
		}
		this.m_titleText.SetText(this.m_journalStringBuilder);
	}

	// Token: 0x04001DE9 RID: 7657
	private const string CARRIAGE_RETURN_REPLACEMENT = "\n\n";

	// Token: 0x04001DEA RID: 7658
	private readonly string[] CARRIAGE_RETURN_ARRAY = new string[]
	{
		"</CR>    ",
		"</CR>   ",
		"</CR>  ",
		"</CR> ",
		"</CR>"
	};

	// Token: 0x04001DEB RID: 7659
	[SerializeField]
	private TMP_Text m_journalNumberText;

	// Token: 0x04001DEC RID: 7660
	private StringBuilder m_journalStringBuilder;

	// Token: 0x02000C0D RID: 3085
	public enum JournalOmniUIDescriptionBoxType
	{
		// Token: 0x04004EBA RID: 20154
		None,
		// Token: 0x04004EBB RID: 20155
		Title,
		// Token: 0x04004EBC RID: 20156
		Text
	}
}

using System;
using System.Text;
using TMPro;
using UnityEngine;

// Token: 0x02000642 RID: 1602
public class JournalOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<JournalOmniUIDescriptionEventArgs, JournalOmniUIDescriptionBoxEntry.JournalOmniUIDescriptionBoxType>
{
	// Token: 0x060030F0 RID: 12528 RVA: 0x0001ADF6 File Offset: 0x00018FF6
	protected override void Awake()
	{
		base.Awake();
		this.m_journalStringBuilder = new StringBuilder();
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x0001AE09 File Offset: 0x00019009
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

	// Token: 0x060030F2 RID: 12530 RVA: 0x000D2074 File Offset: 0x000D0274
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

	// Token: 0x0400280E RID: 10254
	private const string CARRIAGE_RETURN_REPLACEMENT = "\n\n";

	// Token: 0x0400280F RID: 10255
	private readonly string[] CARRIAGE_RETURN_ARRAY = new string[]
	{
		"</CR>    ",
		"</CR>   ",
		"</CR>  ",
		"</CR> ",
		"</CR>"
	};

	// Token: 0x04002810 RID: 10256
	[SerializeField]
	private TMP_Text m_journalNumberText;

	// Token: 0x04002811 RID: 10257
	private StringBuilder m_journalStringBuilder;

	// Token: 0x02000643 RID: 1603
	public enum JournalOmniUIDescriptionBoxType
	{
		// Token: 0x04002813 RID: 10259
		None,
		// Token: 0x04002814 RID: 10260
		Title,
		// Token: 0x04002815 RID: 10261
		Text
	}
}

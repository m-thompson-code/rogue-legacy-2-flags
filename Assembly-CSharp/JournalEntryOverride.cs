using System;
using UnityEngine;

// Token: 0x02000818 RID: 2072
public class JournalEntryOverride : MonoBehaviour
{
	// Token: 0x17001724 RID: 5924
	// (get) Token: 0x06003FDC RID: 16348 RVA: 0x00023454 File Offset: 0x00021654
	public string TitleLocIDOverride
	{
		get
		{
			return this.m_titleLocIDOverride;
		}
	}

	// Token: 0x17001725 RID: 5925
	// (get) Token: 0x06003FDD RID: 16349 RVA: 0x0002345C File Offset: 0x0002165C
	public string EntryLocIDOverride
	{
		get
		{
			return this.m_entryLocIDOverride;
		}
	}

	// Token: 0x17001726 RID: 5926
	// (get) Token: 0x06003FDE RID: 16350 RVA: 0x00023464 File Offset: 0x00021664
	public JournalType JournalType
	{
		get
		{
			return this.m_journalType;
		}
	}

	// Token: 0x17001727 RID: 5927
	// (get) Token: 0x06003FDF RID: 16351 RVA: 0x0002346C File Offset: 0x0002166C
	public JournalCategoryType JournalCategoryTypeOverride
	{
		get
		{
			return this.m_journalCategoryTypeOverride;
		}
	}

	// Token: 0x17001728 RID: 5928
	// (get) Token: 0x06003FE0 RID: 16352 RVA: 0x00023474 File Offset: 0x00021674
	public DialogueWindowStyle DialogueWindowStyle
	{
		get
		{
			return this.m_dialogueWindowStyle;
		}
	}

	// Token: 0x17001729 RID: 5929
	// (get) Token: 0x06003FE1 RID: 16353 RVA: 0x0002347C File Offset: 0x0002167C
	public DialoguePortraitType PortraitType
	{
		get
		{
			return this.m_portraitType;
		}
	}

	// Token: 0x040031EC RID: 12780
	[SerializeField]
	private string m_titleLocIDOverride;

	// Token: 0x040031ED RID: 12781
	[SerializeField]
	private string m_entryLocIDOverride;

	// Token: 0x040031EE RID: 12782
	[SerializeField]
	private JournalType m_journalType;

	// Token: 0x040031EF RID: 12783
	[SerializeField]
	private JournalCategoryType m_journalCategoryTypeOverride;

	// Token: 0x040031F0 RID: 12784
	[SerializeField]
	private DialogueWindowStyle m_dialogueWindowStyle = DialogueWindowStyle.VerticalRight;

	// Token: 0x040031F1 RID: 12785
	[SerializeField]
	private DialoguePortraitType m_portraitType;
}

using System;
using UnityEngine;

// Token: 0x020004D9 RID: 1241
public class JournalEntryOverride : MonoBehaviour
{
	// Token: 0x17001171 RID: 4465
	// (get) Token: 0x06002E4C RID: 11852 RVA: 0x0009C96D File Offset: 0x0009AB6D
	public string TitleLocIDOverride
	{
		get
		{
			return this.m_titleLocIDOverride;
		}
	}

	// Token: 0x17001172 RID: 4466
	// (get) Token: 0x06002E4D RID: 11853 RVA: 0x0009C975 File Offset: 0x0009AB75
	public string EntryLocIDOverride
	{
		get
		{
			return this.m_entryLocIDOverride;
		}
	}

	// Token: 0x17001173 RID: 4467
	// (get) Token: 0x06002E4E RID: 11854 RVA: 0x0009C97D File Offset: 0x0009AB7D
	public JournalType JournalType
	{
		get
		{
			return this.m_journalType;
		}
	}

	// Token: 0x17001174 RID: 4468
	// (get) Token: 0x06002E4F RID: 11855 RVA: 0x0009C985 File Offset: 0x0009AB85
	public JournalCategoryType JournalCategoryTypeOverride
	{
		get
		{
			return this.m_journalCategoryTypeOverride;
		}
	}

	// Token: 0x17001175 RID: 4469
	// (get) Token: 0x06002E50 RID: 11856 RVA: 0x0009C98D File Offset: 0x0009AB8D
	public DialogueWindowStyle DialogueWindowStyle
	{
		get
		{
			return this.m_dialogueWindowStyle;
		}
	}

	// Token: 0x17001176 RID: 4470
	// (get) Token: 0x06002E51 RID: 11857 RVA: 0x0009C995 File Offset: 0x0009AB95
	public DialoguePortraitType PortraitType
	{
		get
		{
			return this.m_portraitType;
		}
	}

	// Token: 0x040024EC RID: 9452
	[SerializeField]
	private string m_titleLocIDOverride;

	// Token: 0x040024ED RID: 9453
	[SerializeField]
	private string m_entryLocIDOverride;

	// Token: 0x040024EE RID: 9454
	[SerializeField]
	private JournalType m_journalType;

	// Token: 0x040024EF RID: 9455
	[SerializeField]
	private JournalCategoryType m_journalCategoryTypeOverride;

	// Token: 0x040024F0 RID: 9456
	[SerializeField]
	private DialogueWindowStyle m_dialogueWindowStyle = DialogueWindowStyle.VerticalRight;

	// Token: 0x040024F1 RID: 9457
	[SerializeField]
	private DialoguePortraitType m_portraitType;
}

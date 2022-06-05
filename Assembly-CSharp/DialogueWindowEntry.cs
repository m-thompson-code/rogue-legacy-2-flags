using System;
using TMPro;
using UnityEngine;

// Token: 0x0200095A RID: 2394
public class DialogueWindowEntry : MonoBehaviour
{
	// Token: 0x17001968 RID: 6504
	// (get) Token: 0x060048C8 RID: 18632 RVA: 0x00027F41 File Offset: 0x00026141
	public TMP_Text TitleText
	{
		get
		{
			return this.m_titleText;
		}
	}

	// Token: 0x17001969 RID: 6505
	// (get) Token: 0x060048C9 RID: 18633 RVA: 0x00027F49 File Offset: 0x00026149
	public TMP_Text DialogueText
	{
		get
		{
			return this.m_dialogueText;
		}
	}

	// Token: 0x1700196A RID: 6506
	// (get) Token: 0x060048CA RID: 18634 RVA: 0x00027F51 File Offset: 0x00026151
	public GameObject ArrowObj
	{
		get
		{
			return this.m_arrowObj;
		}
	}

	// Token: 0x1700196B RID: 6507
	// (get) Token: 0x060048CB RID: 18635 RVA: 0x00027F59 File Offset: 0x00026159
	public RectTransform RectTransform
	{
		get
		{
			return this.m_rectTransform;
		}
	}

	// Token: 0x1700196C RID: 6508
	// (get) Token: 0x060048CC RID: 18636 RVA: 0x00027F61 File Offset: 0x00026161
	public CanvasGroup CanvasGroup
	{
		get
		{
			return this.m_canvasGroup;
		}
	}

	// Token: 0x1700196D RID: 6509
	// (get) Token: 0x060048CD RID: 18637 RVA: 0x00027F69 File Offset: 0x00026169
	public Typewrite_RL Typewrite
	{
		get
		{
			return this.m_typeWriter;
		}
	}

	// Token: 0x040037BF RID: 14271
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x040037C0 RID: 14272
	[SerializeField]
	private TMP_Text m_dialogueText;

	// Token: 0x040037C1 RID: 14273
	[SerializeField]
	private GameObject m_arrowObj;

	// Token: 0x040037C2 RID: 14274
	[SerializeField]
	private RectTransform m_rectTransform;

	// Token: 0x040037C3 RID: 14275
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x040037C4 RID: 14276
	[SerializeField]
	private Typewrite_RL m_typeWriter;
}

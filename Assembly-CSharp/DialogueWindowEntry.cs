using System;
using TMPro;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public class DialogueWindowEntry : MonoBehaviour
{
	// Token: 0x17001283 RID: 4739
	// (get) Token: 0x06003393 RID: 13203 RVA: 0x000AE9B4 File Offset: 0x000ACBB4
	public TMP_Text TitleText
	{
		get
		{
			return this.m_titleText;
		}
	}

	// Token: 0x17001284 RID: 4740
	// (get) Token: 0x06003394 RID: 13204 RVA: 0x000AE9BC File Offset: 0x000ACBBC
	public TMP_Text DialogueText
	{
		get
		{
			return this.m_dialogueText;
		}
	}

	// Token: 0x17001285 RID: 4741
	// (get) Token: 0x06003395 RID: 13205 RVA: 0x000AE9C4 File Offset: 0x000ACBC4
	public GameObject ArrowObj
	{
		get
		{
			return this.m_arrowObj;
		}
	}

	// Token: 0x17001286 RID: 4742
	// (get) Token: 0x06003396 RID: 13206 RVA: 0x000AE9CC File Offset: 0x000ACBCC
	public RectTransform RectTransform
	{
		get
		{
			return this.m_rectTransform;
		}
	}

	// Token: 0x17001287 RID: 4743
	// (get) Token: 0x06003397 RID: 13207 RVA: 0x000AE9D4 File Offset: 0x000ACBD4
	public CanvasGroup CanvasGroup
	{
		get
		{
			return this.m_canvasGroup;
		}
	}

	// Token: 0x17001288 RID: 4744
	// (get) Token: 0x06003398 RID: 13208 RVA: 0x000AE9DC File Offset: 0x000ACBDC
	public Typewrite_RL Typewrite
	{
		get
		{
			return this.m_typeWriter;
		}
	}

	// Token: 0x04002859 RID: 10329
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x0400285A RID: 10330
	[SerializeField]
	private TMP_Text m_dialogueText;

	// Token: 0x0400285B RID: 10331
	[SerializeField]
	private GameObject m_arrowObj;

	// Token: 0x0400285C RID: 10332
	[SerializeField]
	private RectTransform m_rectTransform;

	// Token: 0x0400285D RID: 10333
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x0400285E RID: 10334
	[SerializeField]
	private Typewrite_RL m_typeWriter;
}

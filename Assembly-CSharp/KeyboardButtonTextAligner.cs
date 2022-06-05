using System;
using TMPro;
using UnityEngine;

// Token: 0x02000214 RID: 532
public class KeyboardButtonTextAligner : MonoBehaviour
{
	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x06001630 RID: 5680 RVA: 0x0004540A File Offset: 0x0004360A
	// (set) Token: 0x06001631 RID: 5681 RVA: 0x00045412 File Offset: 0x00043612
	public TMP_Text BaseTMPObject { get; set; }

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x06001632 RID: 5682 RVA: 0x0004541B File Offset: 0x0004361B
	// (set) Token: 0x06001633 RID: 5683 RVA: 0x00045423 File Offset: 0x00043623
	public int SpriteIndex
	{
		get
		{
			return this.m_spriteIndex;
		}
		set
		{
			this.m_spriteIndex = value;
		}
	}

	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x06001634 RID: 5684 RVA: 0x0004542C File Offset: 0x0004362C
	// (set) Token: 0x06001635 RID: 5685 RVA: 0x00045434 File Offset: 0x00043634
	public TMP_Text ParentTMPObject
	{
		get
		{
			return this.m_parentTMPObject;
		}
		set
		{
			this.m_parentTMPObject = value;
		}
	}

	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x06001636 RID: 5686 RVA: 0x0004543D File Offset: 0x0004363D
	// (set) Token: 0x06001637 RID: 5687 RVA: 0x00045445 File Offset: 0x00043645
	public Vector2 PositionOffset
	{
		get
		{
			return this.m_positionOffset;
		}
		set
		{
			this.m_positionOffset = value;
		}
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x0004544E File Offset: 0x0004364E
	private void Awake()
	{
		this.m_transformRect = base.GetComponent<RectTransform>();
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x0004545C File Offset: 0x0004365C
	private void LateUpdate()
	{
		if (this.ParentTMPObject != null)
		{
			this.UpdateTypewriting();
			this.UpdateAlpha();
		}
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x00045478 File Offset: 0x00043678
	private void UpdateTypewriting()
	{
		if (this.ParentTMPObject.maxVisibleCharacters <= this.m_cachedCharIndex && this.BaseTMPObject.isActiveAndEnabled)
		{
			this.BaseTMPObject.enabled = false;
			return;
		}
		if (this.ParentTMPObject.maxVisibleCharacters > this.m_cachedCharIndex && !this.BaseTMPObject.isActiveAndEnabled)
		{
			this.BaseTMPObject.enabled = true;
		}
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x000454DE File Offset: 0x000436DE
	private void UpdateAlpha()
	{
		if (this.BaseTMPObject.isActiveAndEnabled && this.BaseTMPObject.alpha != this.ParentTMPObject.alpha)
		{
			this.BaseTMPObject.alpha = this.ParentTMPObject.alpha;
		}
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x0004551C File Offset: 0x0004371C
	public void AlignText()
	{
		this.m_textAlignSuccessful = false;
		if (!this.ParentTMPObject)
		{
			return;
		}
		if (!this.BaseTMPObject || string.IsNullOrEmpty(this.BaseTMPObject.text))
		{
			return;
		}
		this.m_cachedCharIndex = -1;
		int num = 0;
		TMP_CharacterInfo[] characterInfo = this.ParentTMPObject.textInfo.characterInfo;
		for (int i = 0; i < characterInfo.Length; i++)
		{
			if (characterInfo[i].elementType == TMP_TextElementType.Sprite)
			{
				num++;
			}
			if (num == this.m_spriteIndex)
			{
				this.m_cachedCharIndex = i;
				break;
			}
		}
		this.m_cachedString = this.ParentTMPObject.text;
		if (this.m_cachedCharIndex != -1)
		{
			TMP_CharacterInfo tmp_CharacterInfo = this.ParentTMPObject.textInfo.characterInfo[this.m_cachedCharIndex];
			Vector3 bottomLeft = tmp_CharacterInfo.bottomLeft;
			Vector3 bottomRight = tmp_CharacterInfo.bottomRight;
			Vector3 topLeft = tmp_CharacterInfo.topLeft;
			float num2 = bottomRight.x - bottomLeft.x;
			float num3 = topLeft.y - bottomLeft.y;
			float num4 = 0.75f;
			num3 *= num4;
			bottomLeft.y = topLeft.y - num3;
			bottomRight.y = bottomLeft.y;
			Rect rect = new Rect(bottomLeft.x, bottomLeft.y, num2, num3);
			Vector3 localPosition = rect.center + this.m_positionOffset;
			localPosition.z = this.m_transformRect.localPosition.z;
			this.m_transformRect.localPosition = localPosition;
			this.m_transformRect.sizeDelta = new Vector2(num2, num3);
			Vector3 localScale = this.m_transformRect.localScale;
			localScale.y = 0.75f;
			this.m_transformRect.localScale = localScale;
			base.Invoke("ForceUpdateBaseTMPObject", 0f);
			this.m_textAlignSuccessful = true;
		}
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x000456EA File Offset: 0x000438EA
	private void ForceUpdateBaseTMPObject()
	{
		if (this.BaseTMPObject)
		{
			this.BaseTMPObject.gameObject.SetActive(false);
			this.BaseTMPObject.gameObject.SetActive(true);
		}
	}

	// Token: 0x04001577 RID: 5495
	[SerializeField]
	private int m_spriteIndex;

	// Token: 0x04001578 RID: 5496
	[SerializeField]
	private TMP_Text m_parentTMPObject;

	// Token: 0x04001579 RID: 5497
	[SerializeField]
	private Vector2 m_positionOffset;

	// Token: 0x0400157A RID: 5498
	private RectTransform m_transformRect;

	// Token: 0x0400157B RID: 5499
	private int m_cachedCharIndex = -1;

	// Token: 0x0400157C RID: 5500
	private string m_cachedString;

	// Token: 0x0400157D RID: 5501
	private bool m_textAlignSuccessful;
}

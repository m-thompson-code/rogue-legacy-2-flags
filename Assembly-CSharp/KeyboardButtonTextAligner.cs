using System;
using TMPro;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class KeyboardButtonTextAligner : MonoBehaviour
{
	// Token: 0x17000E3A RID: 3642
	// (get) Token: 0x06001FCC RID: 8140 RVA: 0x00010C7C File Offset: 0x0000EE7C
	// (set) Token: 0x06001FCD RID: 8141 RVA: 0x00010C84 File Offset: 0x0000EE84
	public TMP_Text BaseTMPObject { get; set; }

	// Token: 0x17000E3B RID: 3643
	// (get) Token: 0x06001FCE RID: 8142 RVA: 0x00010C8D File Offset: 0x0000EE8D
	// (set) Token: 0x06001FCF RID: 8143 RVA: 0x00010C95 File Offset: 0x0000EE95
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

	// Token: 0x17000E3C RID: 3644
	// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x00010C9E File Offset: 0x0000EE9E
	// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x00010CA6 File Offset: 0x0000EEA6
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

	// Token: 0x17000E3D RID: 3645
	// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x00010CAF File Offset: 0x0000EEAF
	// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x00010CB7 File Offset: 0x0000EEB7
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

	// Token: 0x06001FD4 RID: 8148 RVA: 0x00010CC0 File Offset: 0x0000EEC0
	private void Awake()
	{
		this.m_transformRect = base.GetComponent<RectTransform>();
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x00010CCE File Offset: 0x0000EECE
	private void LateUpdate()
	{
		if (this.ParentTMPObject != null)
		{
			this.UpdateTypewriting();
			this.UpdateAlpha();
		}
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x000A3B74 File Offset: 0x000A1D74
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

	// Token: 0x06001FD7 RID: 8151 RVA: 0x00010CEA File Offset: 0x0000EEEA
	private void UpdateAlpha()
	{
		if (this.BaseTMPObject.isActiveAndEnabled && this.BaseTMPObject.alpha != this.ParentTMPObject.alpha)
		{
			this.BaseTMPObject.alpha = this.ParentTMPObject.alpha;
		}
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x000A3BDC File Offset: 0x000A1DDC
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

	// Token: 0x06001FD9 RID: 8153 RVA: 0x00010D27 File Offset: 0x0000EF27
	private void ForceUpdateBaseTMPObject()
	{
		if (this.BaseTMPObject)
		{
			this.BaseTMPObject.gameObject.SetActive(false);
			this.BaseTMPObject.gameObject.SetActive(true);
		}
	}

	// Token: 0x04001C7A RID: 7290
	[SerializeField]
	private int m_spriteIndex;

	// Token: 0x04001C7B RID: 7291
	[SerializeField]
	private TMP_Text m_parentTMPObject;

	// Token: 0x04001C7C RID: 7292
	[SerializeField]
	private Vector2 m_positionOffset;

	// Token: 0x04001C7D RID: 7293
	private RectTransform m_transformRect;

	// Token: 0x04001C7E RID: 7294
	private int m_cachedCharIndex = -1;

	// Token: 0x04001C7F RID: 7295
	private string m_cachedString;

	// Token: 0x04001C80 RID: 7296
	private bool m_textAlignSuccessful;
}

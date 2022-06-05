using System;
using TMPro;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class FairyRuleHUDEntry : MonoBehaviour
{
	// Token: 0x06001576 RID: 5494 RVA: 0x00042C7D File Offset: 0x00040E7D
	public void Initialise(string text)
	{
		this.m_text.color = this.m_defaultColor;
		this.SetText(text);
	}

	// Token: 0x06001577 RID: 5495 RVA: 0x00042C97 File Offset: 0x00040E97
	public void SetText(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			this.m_text.text = text;
			return;
		}
		this.m_text.text = "Text was not set";
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x00042CC0 File Offset: 0x00040EC0
	public void SetState(FairyRoomState state)
	{
		if (state == FairyRoomState.Passed)
		{
			this.m_text.fontStyle = FontStyles.Normal;
			this.m_text.color = this.m_passedColor;
			return;
		}
		if (state == FairyRoomState.Failed)
		{
			this.m_text.fontStyle = FontStyles.Strikethrough;
			this.m_text.color = this.m_failedColor;
			return;
		}
		this.m_text.fontStyle = FontStyles.Normal;
		this.m_text.color = this.m_defaultColor;
	}

	// Token: 0x040014BB RID: 5307
	[SerializeField]
	private TextMeshProUGUI m_text;

	// Token: 0x040014BC RID: 5308
	[SerializeField]
	private Color m_defaultColor = Color.white;

	// Token: 0x040014BD RID: 5309
	[SerializeField]
	private Color m_passedColor = Color.green;

	// Token: 0x040014BE RID: 5310
	[SerializeField]
	private Color m_failedColor = Color.red;
}

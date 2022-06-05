using System;
using TMPro;
using UnityEngine;

// Token: 0x0200039F RID: 927
public class FairyRuleHUDEntry : MonoBehaviour
{
	// Token: 0x06001ED0 RID: 7888 RVA: 0x000101BE File Offset: 0x0000E3BE
	public void Initialise(string text)
	{
		this.m_text.color = this.m_defaultColor;
		this.SetText(text);
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x000101D8 File Offset: 0x0000E3D8
	public void SetText(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			this.m_text.text = text;
			return;
		}
		this.m_text.text = "Text was not set";
	}

	// Token: 0x06001ED2 RID: 7890 RVA: 0x000A13FC File Offset: 0x0009F5FC
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

	// Token: 0x04001B8A RID: 7050
	[SerializeField]
	private TextMeshProUGUI m_text;

	// Token: 0x04001B8B RID: 7051
	[SerializeField]
	private Color m_defaultColor = Color.white;

	// Token: 0x04001B8C RID: 7052
	[SerializeField]
	private Color m_passedColor = Color.green;

	// Token: 0x04001B8D RID: 7053
	[SerializeField]
	private Color m_failedColor = Color.red;
}

using System;
using UnityEngine;

// Token: 0x02000CB8 RID: 3256
public class DesignNote : MonoBehaviour
{
	// Token: 0x17001EE5 RID: 7909
	// (get) Token: 0x06005D35 RID: 23861 RVA: 0x00033483 File Offset: 0x00031683
	// (set) Token: 0x06005D36 RID: 23862 RVA: 0x0003348B File Offset: 0x0003168B
	public string Text
	{
		get
		{
			return this.m_text;
		}
		set
		{
			this.m_text = value;
		}
	}

	// Token: 0x17001EE6 RID: 7910
	// (get) Token: 0x06005D37 RID: 23863 RVA: 0x00033494 File Offset: 0x00031694
	// (set) Token: 0x06005D38 RID: 23864 RVA: 0x0003349C File Offset: 0x0003169C
	public Color TextColour
	{
		get
		{
			return this.m_textColour;
		}
		set
		{
			this.m_textColour = value;
		}
	}

	// Token: 0x04004CA6 RID: 19622
	[SerializeField]
	private string m_text;

	// Token: 0x04004CA7 RID: 19623
	[SerializeField]
	private Color m_textColour = Color.yellow;
}

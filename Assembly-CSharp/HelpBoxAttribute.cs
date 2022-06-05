using System;
using UnityEngine;

// Token: 0x02000C6B RID: 3179
public class HelpBoxAttribute : PropertyAttribute
{
	// Token: 0x06005BA2 RID: 23458 RVA: 0x0003232F File Offset: 0x0003052F
	public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None)
	{
		this.text = text;
		this.messageType = messageType;
	}

	// Token: 0x04004C1C RID: 19484
	public string text;

	// Token: 0x04004C1D RID: 19485
	public HelpBoxMessageType messageType;
}

using System;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class HelpBoxAttribute : PropertyAttribute
{
	// Token: 0x06004219 RID: 16921 RVA: 0x000EB909 File Offset: 0x000E9B09
	public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None)
	{
		this.text = text;
		this.messageType = messageType;
	}

	// Token: 0x04003957 RID: 14679
	public string text;

	// Token: 0x04003958 RID: 14680
	public HelpBoxMessageType messageType;
}

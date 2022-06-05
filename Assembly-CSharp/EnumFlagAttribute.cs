using System;
using UnityEngine;

// Token: 0x02000C69 RID: 3177
public class EnumFlagAttribute : PropertyAttribute
{
	// Token: 0x06005BA0 RID: 23456 RVA: 0x00032318 File Offset: 0x00030518
	public EnumFlagAttribute()
	{
	}

	// Token: 0x06005BA1 RID: 23457 RVA: 0x00032320 File Offset: 0x00030520
	public EnumFlagAttribute(string name)
	{
		this.enumName = name;
	}

	// Token: 0x04004C16 RID: 19478
	public string enumName;
}

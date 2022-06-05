using System;
using UnityEngine;

// Token: 0x020007A3 RID: 1955
public class EnumFlagAttribute : PropertyAttribute
{
	// Token: 0x06004217 RID: 16919 RVA: 0x000EB8F2 File Offset: 0x000E9AF2
	public EnumFlagAttribute()
	{
	}

	// Token: 0x06004218 RID: 16920 RVA: 0x000EB8FA File Offset: 0x000E9AFA
	public EnumFlagAttribute(string name)
	{
		this.enumName = name;
	}

	// Token: 0x04003951 RID: 14673
	public string enumName;
}

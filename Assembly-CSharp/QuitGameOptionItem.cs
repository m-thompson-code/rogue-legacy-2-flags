using System;
using UnityEngine;

// Token: 0x02000463 RID: 1123
public class QuitGameOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060023CA RID: 9162 RVA: 0x00013B6C File Offset: 0x00011D6C
	public override void ActivateOption()
	{
		base.ActivateOption();
		Application.Quit(10);
	}
}

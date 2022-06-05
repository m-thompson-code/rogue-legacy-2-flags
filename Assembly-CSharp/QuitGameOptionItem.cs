using System;
using UnityEngine;

// Token: 0x02000297 RID: 663
public class QuitGameOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060019DB RID: 6619 RVA: 0x000511F1 File Offset: 0x0004F3F1
	public override void ActivateOption()
	{
		base.ActivateOption();
		Application.Quit(10);
	}
}

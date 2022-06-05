using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006D8 RID: 1752
[Serializable]
public class FileShortcutEntry
{
	// Token: 0x04003045 RID: 12357
	public bool TitleModifiable;

	// Token: 0x04003046 RID: 12358
	public string ShortcutTitle = "New Shortcut";

	// Token: 0x04003047 RID: 12359
	public string Notes;

	// Token: 0x04003048 RID: 12360
	public List<UnityEngine.Object> ObjectList = new List<UnityEngine.Object>();
}

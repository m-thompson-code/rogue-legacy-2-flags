using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B83 RID: 2947
[Serializable]
public class FileShortcutEntry
{
	// Token: 0x04004294 RID: 17044
	public bool TitleModifiable;

	// Token: 0x04004295 RID: 17045
	public string ShortcutTitle = "New Shortcut";

	// Token: 0x04004296 RID: 17046
	public string Notes;

	// Token: 0x04004297 RID: 17047
	public List<UnityEngine.Object> ObjectList = new List<UnityEngine.Object>();
}

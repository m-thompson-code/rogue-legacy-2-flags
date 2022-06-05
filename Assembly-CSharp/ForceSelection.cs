using System;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class ForceSelection : MonoBehaviour
{
	// Token: 0x170013E5 RID: 5093
	// (get) Token: 0x0600383B RID: 14395 RVA: 0x000C01A8 File Offset: 0x000BE3A8
	public ForceSelection.ForceSelectionType SelectionType
	{
		get
		{
			return this.m_selectionType;
		}
	}

	// Token: 0x04002AEE RID: 10990
	[SerializeField]
	private ForceSelection.ForceSelectionType m_selectionType;

	// Token: 0x02000D97 RID: 3479
	public enum ForceSelectionType
	{
		// Token: 0x040054EC RID: 21740
		SelectBase,
		// Token: 0x040054ED RID: 21741
		Lock,
		// Token: 0x040054EE RID: 21742
		None
	}
}

using System;
using UnityEngine;

// Token: 0x02000A2C RID: 2604
public class ForceSelection : MonoBehaviour
{
	// Token: 0x17001B3C RID: 6972
	// (get) Token: 0x06004ED8 RID: 20184 RVA: 0x0002B002 File Offset: 0x00029202
	public ForceSelection.ForceSelectionType SelectionType
	{
		get
		{
			return this.m_selectionType;
		}
	}

	// Token: 0x04003B51 RID: 15185
	[SerializeField]
	private ForceSelection.ForceSelectionType m_selectionType;

	// Token: 0x02000A2D RID: 2605
	public enum ForceSelectionType
	{
		// Token: 0x04003B53 RID: 15187
		SelectBase,
		// Token: 0x04003B54 RID: 15188
		Lock,
		// Token: 0x04003B55 RID: 15189
		None
	}
}

using System;
using UnityEngine;

// Token: 0x020002C7 RID: 711
[Serializable]
public class RLBreakableSaveState : RLSaveState
{
	// Token: 0x17000CA1 RID: 3233
	// (get) Token: 0x06001C5F RID: 7263 RVA: 0x0005BC07 File Offset: 0x00059E07
	// (set) Token: 0x06001C60 RID: 7264 RVA: 0x0005BC0F File Offset: 0x00059E0F
	public bool AttackerIsOnRight
	{
		get
		{
			return this.m_attackedOnRight;
		}
		set
		{
			this.m_attackedOnRight = value;
		}
	}

	// Token: 0x040019A2 RID: 6562
	[SerializeField]
	private bool m_attackedOnRight;
}

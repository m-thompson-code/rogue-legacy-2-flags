using System;
using UnityEngine;

// Token: 0x020004BC RID: 1212
[Serializable]
public class RLBreakableSaveState : RLSaveState
{
	// Token: 0x1700102A RID: 4138
	// (get) Token: 0x06002729 RID: 10025 RVA: 0x000160BB File Offset: 0x000142BB
	// (set) Token: 0x0600272A RID: 10026 RVA: 0x000160C3 File Offset: 0x000142C3
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

	// Token: 0x040021A9 RID: 8617
	[SerializeField]
	private bool m_attackedOnRight;
}

using System;
using UnityEngine;

// Token: 0x020004BD RID: 1213
[Serializable]
public class RLSaveState
{
	// Token: 0x1700102B RID: 4139
	// (get) Token: 0x0600272C RID: 10028 RVA: 0x000160D4 File Offset: 0x000142D4
	// (set) Token: 0x0600272D RID: 10029 RVA: 0x000160DC File Offset: 0x000142DC
	public bool IsSpawned
	{
		get
		{
			return this.m_isSpawned;
		}
		set
		{
			this.m_isSpawned = value;
		}
	}

	// Token: 0x1700102C RID: 4140
	// (get) Token: 0x0600272E RID: 10030 RVA: 0x000160E5 File Offset: 0x000142E5
	// (set) Token: 0x0600272F RID: 10031 RVA: 0x000160ED File Offset: 0x000142ED
	public bool IsStateActive
	{
		get
		{
			return this.m_isStateActive;
		}
		set
		{
			this.m_isStateActive = value;
		}
	}

	// Token: 0x040021AA RID: 8618
	[SerializeField]
	private bool m_isStateActive = true;

	// Token: 0x040021AB RID: 8619
	[SerializeField]
	private bool m_isSpawned;
}

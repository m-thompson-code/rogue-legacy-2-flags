using System;
using UnityEngine;

// Token: 0x020002C8 RID: 712
[Serializable]
public class RLSaveState
{
	// Token: 0x17000CA2 RID: 3234
	// (get) Token: 0x06001C62 RID: 7266 RVA: 0x0005BC20 File Offset: 0x00059E20
	// (set) Token: 0x06001C63 RID: 7267 RVA: 0x0005BC28 File Offset: 0x00059E28
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

	// Token: 0x17000CA3 RID: 3235
	// (get) Token: 0x06001C64 RID: 7268 RVA: 0x0005BC31 File Offset: 0x00059E31
	// (set) Token: 0x06001C65 RID: 7269 RVA: 0x0005BC39 File Offset: 0x00059E39
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

	// Token: 0x040019A3 RID: 6563
	[SerializeField]
	private bool m_isStateActive = true;

	// Token: 0x040019A4 RID: 6564
	[SerializeField]
	private bool m_isSpawned;
}

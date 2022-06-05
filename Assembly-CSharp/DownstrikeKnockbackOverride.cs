using System;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class DownstrikeKnockbackOverride : MonoBehaviour, IRootObj
{
	// Token: 0x17000D83 RID: 3459
	// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0000F482 File Offset: 0x0000D682
	public float Value
	{
		get
		{
			return this.m_knockbackOverride;
		}
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001AD2 RID: 6866
	[SerializeField]
	private float m_knockbackOverride;
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A42 RID: 2626
public class MessengerDebugger : MonoBehaviour
{
	// Token: 0x17001B48 RID: 6984
	// (get) Token: 0x06004F28 RID: 20264 RVA: 0x0002B2A0 File Offset: 0x000294A0
	// (set) Token: 0x06004F29 RID: 20265 RVA: 0x0002B2A8 File Offset: 0x000294A8
	public List<ListenerEntry> Listeners
	{
		get
		{
			return this.m_listeners;
		}
		private set
		{
			this.m_listeners = value;
		}
	}

	// Token: 0x04003C23 RID: 15395
	[SerializeField]
	protected bool m_printDebugStatements;

	// Token: 0x04003C24 RID: 15396
	[SerializeField]
	private List<ListenerEntry> m_listeners;
}

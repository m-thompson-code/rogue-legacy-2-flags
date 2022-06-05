using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200061F RID: 1567
public class MessengerDebugger : MonoBehaviour
{
	// Token: 0x170013F1 RID: 5105
	// (get) Token: 0x06003884 RID: 14468 RVA: 0x000C111F File Offset: 0x000BF31F
	// (set) Token: 0x06003885 RID: 14469 RVA: 0x000C1127 File Offset: 0x000BF327
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

	// Token: 0x04002BB6 RID: 11190
	[SerializeField]
	protected bool m_printDebugStatements;

	// Token: 0x04002BB7 RID: 11191
	[SerializeField]
	private List<ListenerEntry> m_listeners;
}

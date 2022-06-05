using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A43 RID: 2627
public class MessengerDebuggerGenerics<T, U> : MonoBehaviour where T : Messenger<T, U>
{
	// Token: 0x04003C25 RID: 15397
	[SerializeField]
	protected bool m_printDebugStatements;

	// Token: 0x04003C26 RID: 15398
	[SerializeField]
	private List<ListenerEntry> m_listeners;
}

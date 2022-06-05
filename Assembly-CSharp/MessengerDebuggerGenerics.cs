using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000620 RID: 1568
public class MessengerDebuggerGenerics<T, U> : MonoBehaviour where T : Messenger<T, U>
{
	// Token: 0x04002BB8 RID: 11192
	[SerializeField]
	protected bool m_printDebugStatements;

	// Token: 0x04002BB9 RID: 11193
	[SerializeField]
	private List<ListenerEntry> m_listeners;
}

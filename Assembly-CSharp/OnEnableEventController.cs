using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000CCA RID: 3274
public class OnEnableEventController : MonoBehaviour
{
	// Token: 0x06005D7B RID: 23931 RVA: 0x000336D4 File Offset: 0x000318D4
	private void OnEnable()
	{
		if (this.m_onEnableEvent != null)
		{
			this.m_onEnableEvent.Invoke();
		}
	}

	// Token: 0x06005D7C RID: 23932 RVA: 0x000336E9 File Offset: 0x000318E9
	private void OnDisable()
	{
		if (this.m_onDisableEvent != null)
		{
			this.m_onDisableEvent.Invoke();
		}
	}

	// Token: 0x04004CE1 RID: 19681
	[SerializeField]
	private UnityEvent m_onEnableEvent;

	// Token: 0x04004CE2 RID: 19682
	[SerializeField]
	private UnityEvent m_onDisableEvent;
}

using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000802 RID: 2050
public class OnEnableEventController : MonoBehaviour
{
	// Token: 0x060043F2 RID: 17394 RVA: 0x000F0617 File Offset: 0x000EE817
	private void OnEnable()
	{
		if (this.m_onEnableEvent != null)
		{
			this.m_onEnableEvent.Invoke();
		}
	}

	// Token: 0x060043F3 RID: 17395 RVA: 0x000F062C File Offset: 0x000EE82C
	private void OnDisable()
	{
		if (this.m_onDisableEvent != null)
		{
			this.m_onDisableEvent.Invoke();
		}
	}

	// Token: 0x04003A15 RID: 14869
	[SerializeField]
	private UnityEvent m_onEnableEvent;

	// Token: 0x04003A16 RID: 14870
	[SerializeField]
	private UnityEvent m_onDisableEvent;
}

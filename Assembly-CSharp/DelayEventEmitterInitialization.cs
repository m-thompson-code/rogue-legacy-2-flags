using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x020006C2 RID: 1730
public class DelayEventEmitterInitialization : MonoBehaviour
{
	// Token: 0x06003559 RID: 13657 RVA: 0x0001D431 File Offset: 0x0001B631
	private void Awake()
	{
		this.m_eventEmitter.PlayEvent = EmitterGameEvent.None;
		this.m_eventEmitter.StopEvent = EmitterGameEvent.None;
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x0001D44B File Offset: 0x0001B64B
	private void OnEnable()
	{
		if (!this.m_initialized)
		{
			base.StartCoroutine(this.DelayEmitterInitialization());
		}
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x0001D462 File Offset: 0x0001B662
	private IEnumerator DelayEmitterInitialization()
	{
		yield return null;
		this.m_eventEmitter.PlayEvent = this.m_playEvent;
		this.m_eventEmitter.StopEvent = this.m_stopEvent;
		this.m_initialized = true;
		yield break;
	}

	// Token: 0x04002B47 RID: 11079
	[SerializeField]
	private StudioEventEmitter m_eventEmitter;

	// Token: 0x04002B48 RID: 11080
	[SerializeField]
	private EmitterGameEvent m_playEvent;

	// Token: 0x04002B49 RID: 11081
	[SerializeField]
	private EmitterGameEvent m_stopEvent;

	// Token: 0x04002B4A RID: 11082
	private bool m_initialized;
}

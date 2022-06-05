using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

// Token: 0x0200040B RID: 1035
public class DelayEventEmitterInitialization : MonoBehaviour
{
	// Token: 0x060026AA RID: 9898 RVA: 0x00080119 File Offset: 0x0007E319
	private void Awake()
	{
		this.m_eventEmitter.PlayEvent = EmitterGameEvent.None;
		this.m_eventEmitter.StopEvent = EmitterGameEvent.None;
	}

	// Token: 0x060026AB RID: 9899 RVA: 0x00080133 File Offset: 0x0007E333
	private void OnEnable()
	{
		if (!this.m_initialized)
		{
			base.StartCoroutine(this.DelayEmitterInitialization());
		}
	}

	// Token: 0x060026AC RID: 9900 RVA: 0x0008014A File Offset: 0x0007E34A
	private IEnumerator DelayEmitterInitialization()
	{
		yield return null;
		this.m_eventEmitter.PlayEvent = this.m_playEvent;
		this.m_eventEmitter.StopEvent = this.m_stopEvent;
		this.m_initialized = true;
		yield break;
	}

	// Token: 0x04002067 RID: 8295
	[SerializeField]
	private StudioEventEmitter m_eventEmitter;

	// Token: 0x04002068 RID: 8296
	[SerializeField]
	private EmitterGameEvent m_playEvent;

	// Token: 0x04002069 RID: 8297
	[SerializeField]
	private EmitterGameEvent m_stopEvent;

	// Token: 0x0400206A RID: 8298
	private bool m_initialized;
}
